#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using EveCom;
using EveComFramework.Core;
using EveComFramework.KanedaToolkit;
using LavishScriptAPI;
using EveComFramework.Data;

namespace EveComFramework.Security
{
    #region Enums

    public enum FleeTrigger
    {
        Pod,
        NegativeStanding,
        NeutralStanding,
        Paranoid,
        Targeted,
        CapacitorLow,
        ShieldLow,
        ArmorLow,
        CapitalSpawn,
        CynoSystem,
        CynoGrid,
        Forced,
        Panic,
        WhitelistedCharacterOnGrid,
        BubbleOnPOSGrid,
        SuspectLocal,
        SuspectGrid,
        CriminalLocal,
        CriminalGrid,
        None
    }

    public enum FleeType
    {
        NearestStation,
        SecureBookmark,
        SafeBookmarks
    }

    #endregion

    #region Settings

    /// <summary>
    /// Settings for the Security class
    /// </summary>
    public class SecuritySettings : Settings
    {
        public List<FleeTrigger> Triggers = new List<FleeTrigger>
        {
            FleeTrigger.Pod,
            FleeTrigger.NegativeStanding,
            FleeTrigger.NeutralStanding,
            FleeTrigger.Targeted,
            FleeTrigger.CapitalSpawn,
            FleeTrigger.CapacitorLow,
            FleeTrigger.ShieldLow,
            FleeTrigger.ArmorLow,
        };
        public List<FleeType> Types = new List<FleeType>
        {
            FleeType.NearestStation,
            FleeType.SecureBookmark,
            FleeType.SafeBookmarks
        };
        public HashSet<String> WhiteList = new HashSet<string>();
        public bool NegativeAlliance = false;
        public bool NegativeCorp = false;
        public bool NegativeFleet = false;
        public bool NeutralAlliance = false;
        public bool NeutralCorp = false;
        public bool NeutralFleet = false;
        public bool ParanoidAlliance = false;
        public bool ParanoidCorp = false;
        public bool ParanoidFleet = false;
        public bool TargetAlliance = false;
        public bool TargetCorp = false;
        public bool TargetFleet = false;
        public bool IncludeBroadcastTriggers = false;
        public bool BroadcastTrigger = false;
        public bool AlternateStationFlee = false;
        public int CapThreshold = 30;
        public int ShieldThreshold = 30;
        public int ArmorThreshold = 99;
        public string SafeSubstring = "Safe:";
        public string SecureBookmark = "";
        public int FleeWait = 5;
        public bool IntelToolEnabled = false;
        public int IntelToolInterval = 5;
        public string IntelToolURL = "http://inteltool/report/:solarSystem/";
        public string IntelToolPostData = "local=:pilotList";
        public string ISRelayTarget = "all other";
        public int FleeDroneWait = 0;
    }

    #endregion

    /// <summary>
    /// This class manages security operations for bots.  This includes configurable flees based on pilots present in local and properties like shield/armor
    /// </summary>
    public class Security : State
    {
        #region Instantiation

        static Security _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static Security Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Security();
                }
                return _Instance;
            }
        }

        private Security()
        {
            Log.Log("Security module initialized", LogType.DEBUG);
            RegisterCommands();
        }


        #endregion

        #region Variables

        SecurityAudio SecurityAudio = SecurityAudio.Instance;
        List<Bookmark> SafeSpots;
        /// <summary>
        /// Configuration for this class
        /// </summary>
        public SecuritySettings Config = new SecuritySettings();
        /// <summary>
        /// Logger for this class
        /// </summary>
        public Logger Log = new Logger("Security");
        /// <summary>
        /// Dictionary of lists of entity IDs for entities currently scrambling a fleet member keyed by fleet member ID
        /// </summary>
        public HashSet<long> ScramblingEntities = new HashSet<long>();
        /// <summary>
        /// Dictionary of lists of entity IDs for entities currently neuting a fleet member keyed by fleet member ID
        /// </summary>
        public HashSet<long> NeutingEntities = new HashSet<long>();

        Move.Move Move = EveComFramework.Move.Move.Instance;
        Cargo.Cargo Cargo = EveComFramework.Cargo.Cargo.Instance;
        Pilot Hostile = null;
        Comms.Comms Comms = EveComFramework.Comms.Comms.Instance;
        Exceptions Exceptions = Exceptions.Instance;

        public List<string> Triggers = new List<string>();
        public bool StopUntilManualClearance = false;

        #endregion

        #region LSCommands
        private int LSPanic(string[] args)
        {
            Panic();
            return 0;
        }

        private int LSClearPanic(string[] args)
        {
            ClearPanic();
            return 0;
        }
        #endregion

        #region Events

        /// <summary>
        /// Event raised to alert a bot that a flee is in progress
        /// </summary>
        public event Action Alert;
        /// <summary>
        /// Event raised to alert a bot that it is safe after a flee
        /// </summary>
        public event Action ClearAlert;
        /// <summary>
        /// Event raised to alert a bot a flee was unsuccessful (usually due to a scramble)
        /// </summary>
        public event Action AbandonAlert;

        #endregion

        #region Actions

        /// <summary>
        /// Starts/stops this module
        /// </summary>
        /// <param name="val">Enabled=true</param>
        public void Enable(bool val)
        {
            if (val)
            {
                if (Idle)
                {
                    Comms.Panic += Panic;
                    Comms.ClearPanic += ClearPanic;
                    SecurityAudio.Enabled(true);
                    LavishScript.Commands.AddCommand("Panic", LSPanic);
                    LavishScript.Commands.AddCommand("ClearPanic", LSClearPanic);
                    QueueState(CheckSafe);
                }
            }
            else
            {
                Comms.Panic -= Panic;
                Comms.ClearPanic -= ClearPanic;
                SecurityAudio.Enabled(false);
                LavishScript.Commands.RemoveCommand("Panic");
                LavishScript.Commands.RemoveCommand("ClearPanic");
                Clear();
            }
        }

        /// <summary>
        /// Configure this module
        /// </summary>
        public void Configure()
        {
            UI.Security Configuration = new UI.Security();
            Configuration.Show();
        }

        void TriggerAlert()
        {
            _isAlert = true;
            if (Alert != null)
            {
                Alert();
            }
        }

        void RegisterCommands()
        {
            LavishScript.Commands.AddCommand("SecurityAddScrambler", ScramblingEntitiesUpdate);
            LavishScript.Commands.AddCommand("SecurityAddNeuter", NeutingEntitiesUpdate);
            LavishScript.Commands.AddCommand("SecurityBroadcastTrigger", BroadcastTrigger);
            LavishScript.Commands.AddCommand("SecurityClearBroadcastTrigger", ClearBroadcastTrigger);
        }

        private bool _isAlert = false;
        private bool _isPanic = false;
        /// <summary>
        /// Returns true if the bot is currently in panic state
        /// </summary>
        public bool IsPanic
        {
            get { return _isPanic; }
        }

        public bool IsAlert
        {
            get { return _isAlert; }
        }

        /// <summary>
        /// Causes Security to trigger an alert, flee and wait until manually restarted
        /// </summary>
        public void Panic()
        {
            if (!Idle)
            {
                _isPanic = true;
                Clear();
                TriggerAlert();
                QueueState(RecallDrones);
                QueueState(Flee, -1, FleeTrigger.Panic);
                ReportTrigger(FleeTrigger.Panic);
            }
        }

        /// <summary>
        /// Causes security to abandon the panic state
        /// </summary>
        public void ClearPanic()
        {
            _isPanic = false;
            StopUntilManualClearance = false;
        }

        int ScramblingEntitiesUpdate(string[] args)
        {
            try
            {
                ScramblingEntities.Add(long.Parse(args[1]));
            }
            catch { }

            return 0;
        }

        int NeutingEntitiesUpdate(string[] args)
        {
            try
            {
                NeutingEntities.Add(long.Parse(args[1]));
            }
            catch { }

            return 0;
        }

        Dictionary<string, bool> BroadcastSafe = new Dictionary<string, bool>();

        int BroadcastTrigger(string[] args)
        {
            if (Config.IncludeBroadcastTriggers)
            {
                Log.Log("Received broadcasted trigger, processing", LogType.DEBUG);
                Clear();
                TriggerAlert();
                QueueState(RecallDrones);
                QueueState(Flee, -1, FleeTrigger.Forced);
                ReportTrigger(FleeTrigger.Forced);
                BroadcastSafe[args[1]] = false;
            }
            return 0;
        }

        int ClearBroadcastTrigger(string[] args)
        {
            if (Config.IncludeBroadcastTriggers)
            {
                Log.Log("Received clear broadcasted trigger, processing", LogType.DEBUG);
                BroadcastSafe[args[1]] = true;
            }
            return 0;
        }

        FleeTrigger SafeTrigger()
        {
            try
            {
                if (!Standing.Ready) Standing.LoadStandings();

                foreach (FleeTrigger Trigger in Config.Triggers)
                {
                    switch (Trigger)
                    {
                        case FleeTrigger.Pod:
                            if (Session.InSpace && MyShip.ToItem.GroupID == Group.Capsule) return FleeTrigger.Pod;
                            break;
                        case FleeTrigger.CapitalSpawn:
                            if (Entity.All.Any(a => NPCClasses.All.Any(b => b.Key == a.GroupID && b.Value == "Capital"))) return FleeTrigger.CapitalSpawn;
                            break;
                        case FleeTrigger.CynoGrid:
							if (Session.InSpace && Entity.All.Any(a => a.Distance < 8000000 && (a.TypeID == 21094 || a.TypeID == 28650))) return FleeTrigger.CynoGrid;
                            break;
                        case FleeTrigger.CynoSystem:
							if (Session.InSpace && Entity.All.Any(a => a.TypeID == 21094 || a.TypeID == 28650)) return FleeTrigger.CynoSystem;
                            break;
                        case FleeTrigger.WhitelistedCharacterOnGrid:
                            if (Entity.All.Any(ent => ent.IsPC && Config.WhiteList.Contains(ent.Name))) return FleeTrigger.WhitelistedCharacterOnGrid;
                            break;
                        case FleeTrigger.BubbleOnPOSGrid:
                            if (Entity.All.Any(a => a.GroupID == Group.ForceField && a.SurfaceDistance < 100000) && Entity.All.Any(a => a.GroupID == Group.MobileWarpDisruptor)) return FleeTrigger.BubbleOnPOSGrid;
                            break;
                        case FleeTrigger.SuspectLocal:
                            if (Local.Pilots.Any(a => a.IsSuspect)) return FleeTrigger.SuspectLocal;
                            break;
                        case FleeTrigger.SuspectGrid:
                            if (Entity.All.Any(a => Local.Pilots.Any(b => b.IsSuspect && b.ID == a.Pilot.ID))) return FleeTrigger.SuspectGrid;
                            break;
                        case FleeTrigger.CriminalLocal:
                            if (Local.Pilots.Any(a => a.IsCriminal)) return FleeTrigger.CriminalLocal;
                            break;
                        case FleeTrigger.CriminalGrid:
                            if (Entity.All.Any(a => Local.Pilots.Any(b => b.IsCriminal && b.ID == a.Pilot.ID))) return FleeTrigger.CriminalGrid;
                            break;
                        case FleeTrigger.NegativeStanding:
                            List<Pilot> NegativePilots = Local.Pilots.Where(a => a.DerivedStanding() < 0.0 && a.ID != Me.CharID).ToList();
                            if (!Config.NegativeAlliance) { NegativePilots.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                            if (!Config.NegativeCorp) { NegativePilots.RemoveAll(a => a.CorpID == Me.CorpID); }
                            if (!Config.NegativeFleet) { NegativePilots.RemoveAll(a => a.IsFleetMember); }
                            NegativePilots.RemoveAll(a => Config.WhiteList.Contains(a.Name));
                            if (NegativePilots.Any())
                            {
                                Hostile = NegativePilots.FirstOrDefault();
                                return FleeTrigger.NegativeStanding;
                            }
                            break;
                        case FleeTrigger.NeutralStanding:
                            List<Pilot> NeutralPilots = Local.Pilots.Where(a => a.DerivedStanding() <= 0.0 && a.ID != Me.CharID).ToList();
                            if (!Config.NeutralAlliance) { NeutralPilots.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                            if (!Config.NeutralCorp) { NeutralPilots.RemoveAll(a => a.CorpID == Me.CorpID); }
                            if (!Config.NeutralFleet) { NeutralPilots.RemoveAll(a => a.IsFleetMember); }
                            NeutralPilots.RemoveAll(a => Config.WhiteList.Contains(a.Name));
                            if (NeutralPilots.Any())
                            {
                                Hostile = NeutralPilots.FirstOrDefault();
                                return FleeTrigger.NeutralStanding;
                            }
                            break;
                        case FleeTrigger.Paranoid:
                            List<Pilot> Paranoid = Local.Pilots.Where(a => (a.ToAlliance.FromCharDouble <= 0.0 && a.ToCorp.FromCharDouble <= 0.0 && a.ToChar.FromCharDouble <= 0.0 ) && a.ID != Me.CharID).ToList();
                            if (!Config.ParanoidAlliance) { Paranoid.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                            if (!Config.ParanoidCorp) { Paranoid.RemoveAll(a => a.CorpID == Me.CorpID); }
                            if (!Config.ParanoidFleet) { Paranoid.RemoveAll(a => a.IsFleetMember); }
                            Paranoid.RemoveAll(a => Config.WhiteList.Contains(a.Name));
                            if (Paranoid.Any())
                            {
                                Hostile = Paranoid.FirstOrDefault();
                                return FleeTrigger.Paranoid;
                            }
                            break;
                        case FleeTrigger.Targeted:
                            if (Session.InSpace)
                            {
                                List<Pilot> TargetingPilots = Local.Pilots.Where(a => Entity.All.FirstOrDefault(b => b.CharID == a.ID && b.IsTargetingMe) != null).ToList();
                                if (!Config.TargetAlliance) { TargetingPilots.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                                if (!Config.TargetCorp) { TargetingPilots.RemoveAll(a => a.CorpID == Me.CorpID); }
                                if (!Config.TargetFleet) { TargetingPilots.RemoveAll(a => a.IsFleetMember); }
                                TargetingPilots.RemoveAll(a => Config.WhiteList.Contains(a.Name));
                                if (TargetingPilots.Any())
                                {
                                    Hostile = TargetingPilots.FirstOrDefault();
                                    return FleeTrigger.Targeted;
                                }
                            }
                            break;
                        case FleeTrigger.CapacitorLow:
                            if (Session.InSpace && (MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapThreshold) return FleeTrigger.CapacitorLow;
                            break;
                        case FleeTrigger.ShieldLow:
                            if (Session.InSpace && MyShip.ToEntity.ShieldPct < Config.ShieldThreshold) return FleeTrigger.ShieldLow;
                            break;
                        case FleeTrigger.ArmorLow:
                            if (Session.InSpace && MyShip.ToEntity.ArmorPct < Config.ArmorThreshold) return FleeTrigger.ArmorLow;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Exceptions.Post("Security", e);
            }
            return FleeTrigger.None;
        }

        #endregion

        #region States

        bool RecallDrones(object[] Params)
        {
            if (Session.InSpace && Drone.AllInSpace.Any() && MyShip.ToEntity.GroupID != Group.Capsule) Drone.AllInSpace.ReturnToDroneBay();
            return true;
        }

        /// <summary>
        /// Returns an entity that is scrambling or has scrambled a friendly fleet member
        /// </summary>
        public Entity ValidScramble
        {
            get
            {
                if (Session.InFleet)
                {
                    return Entity.All.FirstOrDefault(a => ScramblingEntities.Contains(a.ID) && !a.Exploded && !a.Released);
                }
                return Entity.All.FirstOrDefault(a => a.IsWarpScrambling && !a.Exploded && !a.Released);
            }
        }

        /// <summary>
        /// Returns an entity that is neuting or has neuted a friendly fleet member
        /// </summary>
        public Entity ValidNeuter
        {
            get
            {
                if (Session.InFleet)
                {
                    return Entity.All.FirstOrDefault(a => NeutingEntities.Contains(a.ID) && !a.Exploded && !a.Released && !Triggers.Contains(a.Name));
                }
                return Entity.All.FirstOrDefault(a => (a.IsEnergyNeuting || a.IsEnergyStealing) && !a.Exploded && !a.Released && !Triggers.Contains(a.Name));
            }
        }

        void ReportTrigger(FleeTrigger reported)
        {
            switch (reported)
            {
                case FleeTrigger.Pod:
                    Log.Log("|rIn a pod!");
                    Comms.ChatQueue.Enqueue("<Security> In a pod!");
                    return;
                case FleeTrigger.CapitalSpawn:
                    Log.Log("|rCapital Spawn on grid!");
                    Comms.ChatQueue.Enqueue("<Security> Capital Spawn on grid!");
                    return;
                case FleeTrigger.CynoGrid:
                    Log.Log("|rCyno on grid!");
                    Comms.ChatQueue.Enqueue("<Security> Cyno on grid!");
                    return;
                case FleeTrigger.CynoSystem:
                    Log.Log("|rCyno in system!");
                    Comms.ChatQueue.Enqueue("<Security> Cyno in system!");
                    return;
                case FleeTrigger.WhitelistedCharacterOnGrid:
                    Log.Log("|rWhitelisted character on grid!");
                    Comms.ChatQueue.Enqueue("<Security> Whitelisted character on grid");
                    StopUntilManualClearance = true;
                    return;
                case FleeTrigger.BubbleOnPOSGrid:
                    Log.Log("|rBubble On POS Grid");
                    Comms.ChatQueue.Enqueue("<Security> Bubble On POS Grid");
                    StopUntilManualClearance = true;
                    return;
                case FleeTrigger.SuspectLocal:
                    Log.Log("|rSuspect pilot in system");
                    Comms.ChatQueue.Enqueue("Suspect pilot in system");
                    return;
                case FleeTrigger.SuspectGrid:
                    Log.Log("|rSuspect pilot on grid");
                    Comms.ChatQueue.Enqueue("Suspect pilot on grid");
                    return;
                case FleeTrigger.CriminalLocal:
                    Log.Log("|rCriminal pilot in system");
                    Comms.ChatQueue.Enqueue("Criminal pilot in system");
                    return;
                case FleeTrigger.CriminalGrid:
                    Log.Log("|rCriminal pilot on grid");
                    Comms.ChatQueue.Enqueue("Criminal pilot on grid");
                    return;
                case FleeTrigger.NegativeStanding:
                    Log.Log("|r{0} is negative standing", Hostile.Name);
                    Comms.ChatQueue.Enqueue("<Security> " + Hostile.Name + " is negative standing");
                    return;
                case FleeTrigger.NeutralStanding:
                    Log.Log("|r{0} is neutral standing", Hostile.Name);
                    Comms.ChatQueue.Enqueue("<Security> " + Hostile.Name + " is neutral standing");
                    return;
                case FleeTrigger.Paranoid:
                    Log.Log("|r{0} is neutral to me", Hostile.Name);
                    Comms.ChatQueue.Enqueue("<Security> " + Hostile.Name + " is neutral to me");
                    return;
                case FleeTrigger.Targeted:
                    Log.Log("|r{0} is targeting me", Hostile.Name);
                    Comms.ChatQueue.Enqueue("<Security> " + Hostile.Name + " is targeting me");
                    return;
                case FleeTrigger.CapacitorLow:
                    Log.Log("|rCapacitor is below threshold (|w{0}%|r)", Config.CapThreshold);
                    Comms.ChatQueue.Enqueue(string.Format("<Security> Capacitor is below threshold ({0}%)", Config.CapThreshold));
                    return;
                case FleeTrigger.ShieldLow:
                    Log.Log("|rShield is below threshold (|w{0}%|r)", Config.ShieldThreshold);
                    Comms.ChatQueue.Enqueue(string.Format("<Security> Shield is below threshold ({0}%)", Config.ShieldThreshold));
                    return;
                case FleeTrigger.ArmorLow:
                    Log.Log("|rArmor is below threshold (|w{0}%|r)", Config.ArmorThreshold);
                    Comms.ChatQueue.Enqueue(string.Format("<Security> Armor is below threshold ({0}%)", Config.ArmorThreshold));
                    return;
                case FleeTrigger.Forced:
                    Log.Log("|rFlee trigger forced.");
                    Comms.ChatQueue.Enqueue("<Security> Flee trigger forced.");
                    return;
                case FleeTrigger.Panic:
                    Log.Log("|rPanicking!");
                    Comms.ChatQueue.Enqueue("<Security> Panicking!");
                    return;
            }
        }

        bool CheckSafe(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;

            Entity WarpScrambling = Entity.All.FirstOrDefault(a => a.IsWarpScrambling);
            if (WarpScrambling != null && WarpScrambling.GroupID != Group.EncounterSurveillanceSystem)
            {
                LavishScript.ExecuteCommand("relay \"all\" -noredirect SecurityAddScrambler " + WarpScrambling.ID);
                return false;
            }
            Entity Neuting = Entity.All.FirstOrDefault(a => a.IsEnergyNeuting || a.IsEnergyStealing && !Triggers.Contains(a.Name));
            if (Neuting != null)
            {
                LavishScript.ExecuteCommand("relay \"all\" -noredirect SecurityAddNeuter " + Neuting.ID);
            }

            if (ValidScramble != null) return false;

            FleeTrigger Reported = SafeTrigger();

            switch (Reported)
            {
                case FleeTrigger.NegativeStanding:
                case FleeTrigger.NeutralStanding:
                case FleeTrigger.Paranoid:
                case FleeTrigger.WhitelistedCharacterOnGrid:
                case FleeTrigger.BubbleOnPOSGrid:
                case FleeTrigger.CriminalLocal:
                case FleeTrigger.CriminalGrid:
                case FleeTrigger.SuspectLocal:
                case FleeTrigger.SuspectGrid:
                    if (Config.BroadcastTrigger) LavishScript.ExecuteCommand("relay \"" + Config.ISRelayTarget + "\" -noredirect SecurityBroadcastTrigger " + Me.CharID + " " + Session.SolarSystemID);
                    goto case FleeTrigger.Pod;
                case FleeTrigger.CapitalSpawn:
                case FleeTrigger.CynoGrid:
                case FleeTrigger.CynoSystem:
                case FleeTrigger.Targeted:
                case FleeTrigger.CapacitorLow:
                case FleeTrigger.ShieldLow:
                case FleeTrigger.ArmorLow:
                    if (Session.InSpace && Drone.AllInSpace.Any())
                    {
                        if (Config.FleeDroneWait > 0)
                        {
                            WaitFor(Config.FleeDroneWait, () => !Drone.AllInSpace.Any());
                        }
                        Drone.AllInSpace.ReturnToDroneBay();
                    }
                    goto case FleeTrigger.Pod;
                case FleeTrigger.Pod:
                    TriggerAlert();
                    QueueState(Flee, -1, Reported);
                    ReportTrigger(Reported);
                    return true;
            }

            return false;
        }

        bool Decloak;

        bool CheckClear(object[] Params)
        {
            if (_isPanic || StopUntilManualClearance) return false;
            FleeTrigger Trigger = (FleeTrigger)Params[0];
            int FleeWait = (Trigger == FleeTrigger.ArmorLow || Trigger == FleeTrigger.CapacitorLow || Trigger == FleeTrigger.ShieldLow || Trigger == FleeTrigger.Forced || Trigger == FleeTrigger.Panic) ? 0 : Config.FleeWait;
            AutoModule.AutoModule.Instance.Decloak = false;
            if (Trigger == FleeTrigger.CapacitorLow && Trigger == FleeTrigger.ShieldLow) AutoModule.AutoModule.Instance.Decloak = true;
            if (Trigger == FleeTrigger.ArmorLow && MyShip.Modules.Any(a => a.GroupID == Group.ArmorRepairUnit && a.IsOnline)) AutoModule.AutoModule.Instance.Decloak = true;

            if (SafeTrigger() != FleeTrigger.None) return false;
            if (Config.IncludeBroadcastTriggers && BroadcastSafe.ContainsValue(false)) return false;
            Log.Log("|oArea is now safe");
            Log.Log(" |-gWaiting for |w{0}|-g minutes", FleeWait);
            Comms.ChatQueue.Enqueue(string.Format("<Security> Area is now safe, waiting for {0} minutes", FleeWait));
            QueueState(CheckReset);
            QueueState(Resume);

            AllowResume = DateTime.Now.AddMinutes(FleeWait);
            return true;
        }

        DateTime AllowResume = DateTime.Now;

        bool CheckReset(object[] Params)
        {
            if (AllowResume <= DateTime.Now) return true;
            FleeTrigger Reported = SafeTrigger();
            if (Reported != FleeTrigger.None)
            {
                Log.Log("|oNew flee condition");
                if (Config.BroadcastTrigger && (Reported == FleeTrigger.NegativeStanding || Reported == FleeTrigger.NeutralStanding || Reported == FleeTrigger.Paranoid)) { LavishScript.ExecuteCommand("relay \"" + Config.ISRelayTarget + "\" -noredirect SecurityBroadcastTrigger " + Me.CharID + " " + Session.SolarSystemID); }
                ReportTrigger(Reported);
                Log.Log(" |-gWaiting for safety");
                Comms.ChatQueue.Enqueue("<Security> New flee condition, waiting for safety");
                Clear();
                QueueState(CheckClear, -1, Reported);
            }
            return false;
        }

        bool SignalSuccessful(object[] Params)
        {
            Log.Log("|oReached flee target");
            Log.Log(" |-gWaiting for safety");
            Comms.ChatQueue.Enqueue("<Security> Reached flee target, waiting for safety");
            return true;
        }

        bool Flee(object[] Params)
        {
            FleeTrigger Trigger = (FleeTrigger)Params[0];

            Cargo.Clear();
            Move.Clear();

            Decloak = AutoModule.AutoModule.Instance.Decloak;

            QueueState(WaitFlee);
            QueueState(SignalSuccessful);

            QueueState(CheckClear, -1, Trigger);

            if (Session.InStation)
            {
                return true;
            }
            if (Config.AlternateStationFlee &&
                (Trigger == FleeTrigger.ArmorLow || Trigger == FleeTrigger.ShieldLow || Trigger == FleeTrigger.CapacitorLow) &&
                Entity.All.FirstOrDefault(a => a.GroupID == Group.Station) != null)
            {
                Move.Object(Entity.All.FirstOrDefault(a => a.GroupID == Group.Station));
                return true;
            }
            foreach (FleeType FleeType in Config.Types)
            {
                switch (FleeType)
                {
                    case FleeType.NearestStation:
                        Entity Station = Entity.All.FirstOrDefault(a => a.GroupID == Group.Station);
                        if (Station != null)
                        {
                            Move.Object(Station);
                            return true;
                        }
                        break;
                    case FleeType.SecureBookmark:
                        Bookmark FleeTo = Bookmark.All.PreferredBookmark(a => a.Title == Config.SecureBookmark);
                        if (FleeTo != null)
                        {
                            Move.Bookmark(FleeTo);
                            return true;
                        }
                        Log.Log("Warning: Bookmark not found!");
                        break;
                    case FleeType.SafeBookmarks:
                        if (!SafeSpots.Any())
                        {
                            SafeSpots = Bookmark.All.Where(a => a.Title.Contains(Config.SafeSubstring) && a.LocationID == Session.SolarSystemID).ToList();
                        }
                        if (SafeSpots.Any())
                        {
                            Move.Bookmark(SafeSpots.FirstOrDefault());
                            SafeSpots.Remove(SafeSpots.FirstOrDefault());
                            return true;
                        }
                        break;
                }
            }
            return true;
        }

        bool WaitFlee(object[] Params)
        {
            Entity WarpScrambling = Entity.All.FirstOrDefault(a => a.IsWarpScrambling);
            if ((WarpScrambling != null && WarpScrambling.GroupID != Group.EncounterSurveillanceSystem) || ValidScramble != null)
            {
                if (WarpScrambling != null)
                {
                    LavishScript.ExecuteCommand("relay \"all\" -noredirect SecurityAddScrambler " + WarpScrambling.ID);
                }
                if (AbandonAlert != null)
                {
                    Log.Log("|rAbandoning flee due to a scramble!");
                    Log.Log("|rReturning control to bot!");
                    Comms.ChatQueue.Enqueue("<Security> Flee canceled due to a new scramble!");
                    Clear();
                    QueueState(CheckSafe);
                    Move.Clear();
                    AbandonAlert();
                }
                return false;
            }
            if (!Move.Idle || (Session.InSpace && MyShip.ToEntity.Mode == EntityMode.Warping))
            {
                return false;
            }
            return true;
        }

        bool Resume(object[] Params)
        {
            _isAlert = false;
            AutoModule.AutoModule.Instance.Decloak = Decloak;
            if (ClearAlert != null)
            {
                Log.Log("|oSending ClearAlert command - resume operations");
                Comms.ChatQueue.Enqueue("<Security> Resuming operations");
                ClearAlert();
            }
            if (Config.BroadcastTrigger)
            {
                LavishScript.ExecuteCommand("relay \"" + Config.ISRelayTarget + "\" -noredirect SecurityClearBroadcastTrigger " + Me.CharID + " " + Session.SolarSystemID);
            }
            QueueState(CheckSafe);
            return true;
        }

        #endregion
    }

    #region Settings

    public class SecurityAudioSettings : Settings
    {
        public bool Flee = true;
        public bool Red = false;
        public bool Blue = false;
        public bool Grey = false;
        public bool Local = false;
        public bool ChatInvite = true;
        public bool Grid = false;
        public string Voice = "";
        public int Rate = 0;
        public int Volume = 100;
    }

    #endregion

    public class SecurityAudio : State
    {
        #region Instantiation

        static SecurityAudio _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static SecurityAudio Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SecurityAudio();
                }
                return _Instance;
            }
        }

        private SecurityAudio()
        {
            if (Config.Voice != "") Speech.SelectVoice(Config.Voice);
            NonFleetPlayers.AddNonFleetPlayers();
        }

        #endregion

        #region Variables

        private ChatChannel LocalChat;
        SpeechSynthesizer Speech = new SpeechSynthesizer();
        Queue<string> SpeechQueue = new Queue<string>();
        public SecurityAudioSettings Config = new SecurityAudioSettings();
        int SolarSystem = -1;
        List<Pilot> PilotCache = new List<Pilot>();
        Security Core;
        int LocalCache;
        bool ChatInviteSeen;
        Targets.Targets NonFleetPlayers = new Targets.Targets();
        List<Entity> NonFleetMemberOnGrid = new List<Entity>();
        private DateTime? LastOfficerAlert = null;
        #endregion

        #region Actions

        void Alert()
        {
            if (Config.Flee) SpeechQueue.Enqueue("Flee");
        }

        public void Enabled(bool var)
        {
            if (var)
            {
                QueueState(Init);
                QueueState(Control);
            }
            else
            {
                Clear();
            }
        }

        #endregion

        #region States

        bool Init(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;

            LocalChat = Comms.Comms.Instance.LocalChat;
            LocalCache = LocalChat.Messages.Count;
            return true;
        }

        bool Control(object[] Params)
        {
            if (Core == null)
            {
                Core = Security.Instance;
                Core.Alert += Alert;
            }
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            if (Session.SolarSystemID != SolarSystem)
            {
                PilotCache = Local.Pilots;
                SolarSystem = Session.SolarSystemID;
            }
            List<Pilot> newPilots = Local.Pilots.Where(a => !PilotCache.Contains(a)).ToList();
            foreach (Pilot pilot in newPilots)
            {
                if (Config.Blue && PilotColor(pilot) == PilotColors.Blue) SpeechQueue.Enqueue("Blue");
                if (Config.Grey && PilotColor(pilot) == PilotColors.Grey) SpeechQueue.Enqueue("Grey");
                if (Config.Red && PilotColor(pilot) == PilotColors.Red) SpeechQueue.Enqueue("Red");
            }
            PilotCache = Local.Pilots;

            if (Config.ChatInvite)
            {
                Window ChatInvite = Window.All.FirstOrDefault(a => a.Name.Contains("ChatInvitation"));
                if (!ChatInviteSeen && ChatInvite != null)
                {
                    SpeechQueue.Enqueue("New Chat Invite");
                    ChatInviteSeen = true;
                }
                if (ChatInviteSeen && ChatInvite == null)
                {
                    ChatInviteSeen = false;
                }
            }

            if (Config.Local && LocalCache != LocalChat.Messages.Count)
            {
                if (LocalChat.Messages.Last().SenderID > 1)
                {
                    SpeechQueue.Enqueue("Local chat");
                }
                LocalCache = LocalChat.Messages.Count;
            }

            if (Session.InSpace)
            {
                if (Config.Grid)
                {
                    Entity AddNonFleet = NonFleetPlayers.TargetList.FirstOrDefault(a => !NonFleetMemberOnGrid.Contains(a));
                    if (AddNonFleet != null)
                    {
                        SpeechQueue.Enqueue("Non fleet member on grid");
                        NonFleetMemberOnGrid.Add(AddNonFleet);
                    }
                    NonFleetMemberOnGrid = NonFleetPlayers.TargetList.Where(a => NonFleetMemberOnGrid.Contains(a)).ToList();
                }
                if (Entity.All.Any(a => Data.NPCClasses.OfficerSpawns.Any(b => a.Name.Equals(b))))
                {
                    if (LastOfficerAlert == null || LastOfficerAlert.Value.AddMinutes(1) < DateTime.Now)
                    {
                        SpeechQueue.Enqueue("Officer spawn on grid");
                        LastOfficerAlert = DateTime.Now;
                    }
                }
            }


            if (Config.Voice != "") Speech.SelectVoice(Config.Voice);
            Speech.Rate = Config.Rate;
            Speech.Volume = Config.Volume;
            if (SpeechQueue.Any()) Speech.SpeakAsync(SpeechQueue.Dequeue());

            return false;
        }

        #endregion

        enum PilotColors
        {
            Blue,
            Red,
            Grey
        }

        PilotColors PilotColor(Pilot pilot)
        {
            if (pilot.CorpID == Me.CorpID) return PilotColors.Blue;
            if (pilot.AllianceID == Me.AllianceID) return PilotColors.Blue;

            double relationship = pilot.DerivedStanding();

            if (relationship > 0.0) return PilotColors.Blue;
            if (relationship < 0.0) return PilotColors.Red;
            return PilotColors.Grey;
        }
    }
}

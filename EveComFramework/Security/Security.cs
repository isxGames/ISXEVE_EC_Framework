using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.Security
{
    #region Enums

    #pragma warning disable 1591

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
        Forced,
        None
    }

    public enum FleeType
    {
        NearestStation,
        SecureBookmark,
        SafeBookmarks
    }

    #pragma warning restore 1591

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
            FleeTrigger.CapacitorLow,
            FleeTrigger.ShieldLow,
            FleeTrigger.ArmorLow
        };
        public List<FleeType> Types = new List<FleeType>
        {
            FleeType.NearestStation,
            FleeType.SecureBookmark,
            FleeType.SafeBookmarks
        };
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
        public int CapThreshold = 30;
        public int ShieldThreshold = 30;
        public int ArmorThreshold = 99;
        public string SafeSubstring = "Safe:";
        public string SecureBookmark = "";
        public int FleeWait = 5;
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

        private Security() : base()
        {
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
        public Dictionary<long, List<long>> ScramblingEntities = new Dictionary<long, List<long>>();
        /// <summary>
        /// Dictionary of lists of entity IDs for entities currently neuting a fleet member keyed by fleet member ID
        /// </summary>
        public Dictionary<long, List<long>> NeutingEntities = new Dictionary<long, List<long>>();

        Move.Move Move = EveComFramework.Move.Move.Instance;
        Cargo.Cargo Cargo = EveComFramework.Cargo.Cargo.Instance;
        Pilot Hostile = null;

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
                    SecurityAudio.Enabled(true);
                    QueueState(CheckSafe);
                }
            }
            else
            {
                SecurityAudio.Enabled(false);
                Clear();
            }
        }

        /// <summary>
        /// Start this module
        /// </summary>
        [Obsolete("Depreciated:  Use Security.Enable (6/11/13)")]
        public void Start()
        {
            if (Idle)
            {
                SecurityAudio.Enabled(true);
                QueueState(CheckSafe);
            }

        }

        /// <summary>
        /// Stop this module
        /// </summary>
        [Obsolete("Depreciated:  Use Security.Enable (6/11/13)")]
        public void Stop()
        {
            SecurityAudio.Enabled(false);
            Clear();
        }

        /// <summary>
        /// Trigger a flee manually
        /// </summary>
        [Obsolete("Depreciated:  Not useful anymore.  Speak with Teht if you have need of this method.  6/11/13")]
        public void Flee()
        {
            Clear();
            QueueState(Flee);
        }

        /// <summary>
        /// This was originally intended to reset the security module after a certain duration.
        /// </summary>
        [Obsolete("Depreciated:  Not useful anymore.  Speak with Teht if you have need of this method.  6/11/13")]
        public void Reset(int? Delay = null)
        {
            int iDelay = Delay ?? Config.FleeWait * 60000;
            QueueState(Blank, iDelay);
            QueueState(CheckSafe);
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
            if (Alert == null)
            {
                Log.Log("|rYou do not have an event handler subscribed to Security.Alert!");
                Log.Log("|rThis is bad!  Tell your developer they're not using Security right!");
            }
            else
            {
                Alert();
            }
        }

        void RegisterCommands()
        {
            LavishScriptAPI.LavishScript.Commands.AddCommand("SecurityAddScrambler", ScramblingEntitiesUpdate);
            LavishScriptAPI.LavishScript.Commands.AddCommand("SecurityAddNeuter", NeutingEntitiesUpdate);
        }

        int ScramblingEntitiesUpdate(string[] args)
        {
            try
            {
                if (!ScramblingEntities.ContainsKey(long.Parse(args[1])))
                {
                    List<long> add = new List<long>();
                    add.Add(long.Parse(args[2]));
                    ScramblingEntities.Add(long.Parse(args[1]), add);
                }
                else
                {
                    if (!ScramblingEntities[long.Parse(args[1])].Contains(long.Parse(args[2])))
                    {
                        ScramblingEntities[long.Parse(args[1])].Add(long.Parse(args[2]));
                    }
                }
            }
            catch { }
            
            return 0;
        }

        int NeutingEntitiesUpdate(string[] args)
        {
            try
            {
                if (!NeutingEntities.ContainsKey(long.Parse(args[1])))
                {
                    List<long> add = new List<long>();
                    add.Add(long.Parse(args[2]));
                    NeutingEntities.Add(long.Parse(args[1]), add);
                }
                else
                {
                    if (!NeutingEntities[long.Parse(args[1])].Contains(long.Parse(args[2])))
                    {
                        NeutingEntities[long.Parse(args[1])].Add(long.Parse(args[2]));
                    }
                }
            }
            catch { }

            return 0;
        }

        FleeTrigger SafeTrigger()
        {
            if (!Standing.Ready) Standing.LoadStandings();

            foreach (FleeTrigger Trigger in Config.Triggers)
            {
                switch (Trigger)
                {
                    case FleeTrigger.Pod:
                        if (MyShip.ToItem.GroupID == Group.Capsule) return FleeTrigger.Pod;
                        break;
                    case FleeTrigger.NegativeStanding:
                        List<Pilot> NegativePilots = Local.Pilots.Where(a => (a.ToAlliance.FromAlliance < 0 ||
                                                                                a.ToAlliance.FromCorp < 0 ||
                                                                                a.ToAlliance.FromChar < 0 ||
                                                                                a.ToCorp.FromAlliance < 0 ||
                                                                                a.ToCorp.FromCorp < 0 ||
                                                                                a.ToCorp.FromChar < 0 ||
                                                                                a.ToChar.FromAlliance < 0 ||
                                                                                a.ToChar.FromCorp < 0 ||
                                                                                a.ToChar.FromChar < 0
                                                                             ) &&
                                                                             a.ID != Me.CharID).ToList();
                        if (!Config.NegativeAlliance) { NegativePilots.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                        if (!Config.NegativeCorp) { NegativePilots.RemoveAll(a => a.CorpID == Me.CorpID); }
                        if (!Config.NegativeFleet) { NegativePilots.RemoveAll(a => a.IsFleetMember); }
                        if (NegativePilots.Any())
                        {
                            Hostile = NegativePilots.FirstOrDefault();
                            return FleeTrigger.NegativeStanding;
                        }
                        break;
                    case FleeTrigger.NeutralStanding:
                        List<Pilot> NeutralPilots = Local.Pilots.Where(a => (a.ToAlliance.FromAlliance <= 0 &&
                                                                                a.ToAlliance.FromCorp <= 0 &&
                                                                                a.ToAlliance.FromChar <= 0 &&
                                                                                a.ToCorp.FromAlliance <= 0 &&
                                                                                a.ToCorp.FromCorp <= 0 &&
                                                                                a.ToCorp.FromChar <= 0 &&
                                                                                a.ToChar.FromAlliance <= 0 &&
                                                                                a.ToChar.FromCorp <= 0 &&
                                                                                a.ToChar.FromChar <= 0
                                                                             ) &&
                                                                             a.ID != Me.CharID).ToList();
                        if (!Config.NeutralAlliance) { NeutralPilots.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                        if (!Config.NeutralCorp) { NeutralPilots.RemoveAll(a => a.CorpID == Me.CorpID); }
                        if (!Config.NeutralFleet) { NeutralPilots.RemoveAll(a => a.IsFleetMember); }
                        if (NeutralPilots.Any())
                        {
                            Hostile = NeutralPilots.FirstOrDefault();
                            return FleeTrigger.NeutralStanding;
                        }
                        break;
                    case FleeTrigger.Paranoid:
                        List<Pilot> Paranoid = Local.Pilots.Where(a => (        a.ToAlliance.FromChar <= 0 &&
                                                                                a.ToCorp.FromChar <= 0 &&
                                                                                a.ToChar.FromChar <= 0
                                                                             ) &&
                                                                             a.ID != Me.CharID).ToList();
                        if (!Config.ParanoidAlliance) { Paranoid.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                        if (!Config.ParanoidCorp) { Paranoid.RemoveAll(a => a.CorpID == Me.CorpID); }
                        if (!Config.ParanoidFleet) { Paranoid.RemoveAll(a => a.IsFleetMember); }
                        if (Paranoid.Any())
                        {
                            Hostile = Paranoid.FirstOrDefault();
                            return FleeTrigger.Paranoid;
                        }
                        break;
                    case FleeTrigger.Targeted:
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        List<Pilot> TargetingPilots = Local.Pilots.Where(a => Entity.All.FirstOrDefault(b => b.CharID == a.ID && b.IsTargetingMe) != null).ToList();
                        if (!Config.TargetAlliance) { TargetingPilots.RemoveAll(a => a.AllianceID == Me.AllianceID); }
                        if (!Config.TargetCorp) { TargetingPilots.RemoveAll(a => a.CorpID == Me.CorpID); }
                        if (!Config.TargetFleet) { TargetingPilots.RemoveAll(a => a.IsFleetMember); }
                        if (TargetingPilots.Any())
                        {
                            Hostile = TargetingPilots.FirstOrDefault();
                            return FleeTrigger.NeutralStanding;
                        }
                        break;
                    case FleeTrigger.CapacitorLow:
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapThreshold) return FleeTrigger.CapacitorLow;
                        break;
                    case FleeTrigger.ShieldLow:
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if (MyShip.ToEntity.ShieldPct < Config.ShieldThreshold) return FleeTrigger.ShieldLow;
                        break;
                    case FleeTrigger.ArmorLow:
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if (MyShip.ToEntity.ArmorPct < Config.ArmorThreshold) return FleeTrigger.ArmorLow;
                        break;
                }
            }
            return FleeTrigger.None;
        }

        #endregion

        #region States

        bool Blank(object[] Params)
        {
            Log.Log("Finished");
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
                    List<long> ValidScrambles = ScramblingEntities.Where(kvp => Fleet.Members.Any(mem => mem.ID == kvp.Key)).SelectMany(kvp => kvp.Value).Distinct().ToList();
                    return Entity.All.FirstOrDefault(a => ValidScrambles.Contains(a.ID) && a.Exists && !a.Exploded && !a.Released);
                }
                return Entity.All.FirstOrDefault(a => a.IsWarpScrambling && a.Exists && !a.Exploded && !a.Released);
            }
        }

        public bool IsScrambling(Entity Check)
        {
            if (Session.InFleet)
            {
                return ScramblingEntities.Values.Any(a => a.Contains(Check.ID));
            }
            return Entity.All.Any(a => a.IsWarpScrambling && a.Exists && !a.Exploded && !a.Released);
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
                    List<long> ValidNeuters = NeutingEntities.Where(kvp => Fleet.Members.Any(mem => mem.ID == kvp.Key)).SelectMany(kvp => kvp.Value).Distinct().ToList();
                    return Entity.All.FirstOrDefault(a => ValidNeuters.Contains(a.ID) && a.Exists && !a.Exploded && !a.Released);
                }
                return Entity.All.FirstOrDefault(a => (a.IsEnergyNeuting || a.IsEnergyStealing) && a.Exists && !a.Exploded && !a.Released);
            }
        }

        public bool IsNeuting(Entity Check)
        {
            if (Session.InFleet)
            {
                return NeutingEntities.Values.Any(a => a.Contains(Check.ID));
            }
            return Entity.All.Any(a => (a.IsEnergyNeuting || a.IsEnergyStealing) && a.Exists && !a.Exploded && !a.Released);
        }

        bool CheckSafe(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;

            Entity WarpScrambling = Entity.All.FirstOrDefault(a => a.IsWarpScrambling);
            if (WarpScrambling != null)
            {
                LavishScriptAPI.LavishScript.ExecuteCommand("relay \"all\" -noredirect SecurityAddScrambler " + Me.CharID + " " + WarpScrambling.ID);
                return false;
            }
            Entity Neuting = Entity.All.FirstOrDefault(a => a.IsEnergyNeuting || a.IsEnergyStealing);
            if (Neuting != null)
            {
                LavishScriptAPI.LavishScript.ExecuteCommand("relay \"all\" -noredirect SecurityAddNeuter " + Me.CharID + " " + Neuting.ID);
                return false;
            }

            if (this.ValidScramble != null) return false;

            switch (SafeTrigger())
            {
                case FleeTrigger.Pod:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.Pod);
                    Log.Log("|rIn a pod!");
                    return true;
                case FleeTrigger.NegativeStanding:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.NegativeStanding);
                    DroneControl.DroneControl.Instance.Clear();
                    if (Session.InSpace && Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                    Log.Log("|r{0} is negative standing", Hostile.Name);
                    return true;
                case FleeTrigger.NeutralStanding:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.NeutralStanding);
                    DroneControl.DroneControl.Instance.Clear();
                    if (Session.InSpace && Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                    Log.Log("|r{0} is neutral standing", Hostile.Name);
                    return true;
                case FleeTrigger.Paranoid:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.Paranoid);
                    DroneControl.DroneControl.Instance.Clear();
                    if (Session.InSpace && Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                    Log.Log("|r{0} is neutral to me", Hostile.Name);
                    return true;
                case FleeTrigger.Targeted:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.Targeted);
                    Log.Log("|r{0} is targeting me", Hostile.Name);
                    DroneControl.DroneControl.Instance.Clear();
                    if (Session.InSpace && Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                    return true;
                case FleeTrigger.CapacitorLow:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.CapacitorLow);
                    DroneControl.DroneControl.Instance.Clear();
                    if (Session.InSpace && Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                    Log.Log("|rCapacitor is below threshold (|w{0}%|r)", Config.CapThreshold);
                    return true;
                case FleeTrigger.ShieldLow:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.ShieldLow);
                    DroneControl.DroneControl.Instance.Clear();
                    if (Session.InSpace && Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                    Log.Log("|rShield is below threshold (|w{0}%|r)", Config.ShieldThreshold);
                    return true;
                case FleeTrigger.ArmorLow:
                    TriggerAlert();
                    QueueState(Flee, -1, FleeTrigger.ArmorLow);
                    DroneControl.DroneControl.Instance.Clear();
                    if (Session.InSpace && Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                    Log.Log("|rArmor is below threshold (|w{0}%|r)", Config.ArmorThreshold);
                    return true;
            }

            return false;
        }

        bool Decloak;

        bool CheckClear(object[] Params)
        {
            FleeTrigger Trigger = (FleeTrigger)Params[0];
            int FleeWait = Config.FleeWait;
            if (Trigger == FleeTrigger.ArmorLow || Trigger == FleeTrigger.CapacitorLow || Trigger == FleeTrigger.ShieldLow || Trigger == FleeTrigger.Forced) FleeWait = 0;

            AutoModule.AutoModule.Instance.Decloak = false;
            if (SafeTrigger() != FleeTrigger.None) return false;
            Log.Log("|oArea is now safe");
            Log.Log(" |-gWaiting for |w{0}|-g minutes", FleeWait);
            QueueState(CheckReset);
            QueueState(Resume);

            AllowResume = DateTime.Now.AddMinutes(FleeWait);
            return true;
        }

        DateTime AllowResume = DateTime.Now;

        bool CheckReset(object[] Params)
        {
            if (AllowResume <= DateTime.Now) return true;
            if (SafeTrigger() != FleeTrigger.None)
            {
                Log.Log("|oNew flee condition");
                Log.Log(" |-gWaiting for safety");
                InsertState(CheckClear);
            }
            return false;
        }

        bool Flee(object[] Params)
        {
            FleeTrigger Trigger = (FleeTrigger)Params[0];

            Cargo.Clear();
            Move.Clear();

            Decloak = AutoModule.AutoModule.Instance.Decloak;

            QueueState(Traveling);
            QueueState(LogMessage, 1, string.Format("|oReached flee target"));
            QueueState(LogMessage, 1, string.Format(" |-gWaiting for safety"));
            QueueState(CheckClear, -1, Trigger);

            if (Session.InStation)
            {
                return true;
            }
            foreach (FleeType FleeType in Config.Types)
            {
                switch (FleeType)
                {
                    case FleeType.NearestStation:
                        if (Entity.All.FirstOrDefault(a => a.GroupID == Group.Station) != null)
                        {
                            Move.Object(Entity.All.FirstOrDefault(a => a.GroupID == Group.Station));
                            return true;
                        }
                        break;
                    case FleeType.SecureBookmark:
                        if (Bookmark.All.Count(a => a.Title == Config.SecureBookmark) > 0)
                        {
                            Move.Bookmark(Bookmark.All.FirstOrDefault(a => a.Title == Config.SecureBookmark));
                            return true;
                        }
                        break;
                    case FleeType.SafeBookmarks:
                        if (SafeSpots.Count == 0)
                        {
                            SafeSpots = Bookmark.All.Where(a => a.Title.Contains(Config.SafeSubstring) && a.LocationID == Session.SolarSystemID).ToList();
                        }
                        if (SafeSpots.Count > 0)
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

        bool Traveling(object[] Params)
        {
            if (!Move.Idle || (Session.InSpace && MyShip.ToEntity.Mode == EntityMode.Warping))
            {
                return false;
            }
            return true;
        }

        bool LogMessage(object[] Params)
        {
            Log.Log((string)Params[0]);
            return true;
        }

        bool Resume(object[] Params)
        {
            FleeTrigger Trigger = SafeTrigger();
            if (Trigger != FleeTrigger.None)
            {
                QueueState(LogMessage, 1, string.Format("|oNew flee condition"));
                QueueState(LogMessage, 1, string.Format(" |-gWaiting for safety"));
                QueueState(CheckClear, -1, Trigger);
                return true;
            }

            AutoModule.AutoModule.Instance.Decloak = Decloak;
            if (ClearAlert == null)
            {
                Log.Log("|rYou do not have an event handler subscribed to Security.ClearAlert!");
                Log.Log("|rThis is bad!  Tell your developer they're not using Security right!");
            }
            else
            {
                Log.Log("|oSending ClearAlert command - resume operations");
                ClearAlert();
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

        private SecurityAudio() : base()
        {
            LavishScriptAPI.LavishScript.Events.RegisterEvent("EVE_LocalChat");
            if (Config.Voice != "") Speech.SelectVoice(Config.Voice);
            QueueState(Control);
        }

        #endregion

        #region Variables

        SpeechSynthesizer Speech = new SpeechSynthesizer();
        Queue<string> SpeechQueue = new Queue<string>();
        public SecurityAudioSettings Config = new SecurityAudioSettings();
        int SolarSystem = -1;
        List<Pilot> PilotCache = new List<Pilot>();
        Security Core;

        #endregion

        #region Actions

        void Alert()
        {
            if (Config.Flee) SpeechQueue.Enqueue("Flee");
        }

        void NewLocalChat(object sender, LavishScriptAPI.LSEventArgs args)
        {
            if (Config.Local) SpeechQueue.Enqueue("New local chat message");
        }

        public void Enabled(bool var)
        {
            if (var)
            {
                LavishScriptAPI.LavishScript.ExecuteCommand("LogReader:RegisterLog[\"EVE/logs/Chatlogs/Local\\*.txt\",\"EVE_LocalChat\"]");
                LavishScriptAPI.LavishScript.Events.AttachEventTarget("EVE_LocalChat", NewLocalChat);
            }
            else
            {
                LavishScriptAPI.LavishScript.ExecuteCommand("LogReader:UnregisterLog[\"EVE/logs/Chatlogs/Local\\*.txt\",\"EVE_LocalChat\"]");
                LavishScriptAPI.LavishScript.Events.DetachEventTarget("EVE_LocalChat", NewLocalChat);
            }
        }

        #endregion

        #region States

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
            int val = 0 +
                pilot.ToAlliance.FromAlliance +
                pilot.ToAlliance.FromCorp +
                pilot.ToAlliance.FromChar +
                pilot.ToCorp.FromAlliance +
                pilot.ToCorp.FromCorp +
                pilot.ToCorp.FromChar +
                pilot.ToChar.FromAlliance +
                pilot.ToChar.FromCorp +
                pilot.ToChar.FromChar;
            if (val > 0) return PilotColors.Blue;
            if (val == 0) return PilotColors.Grey;
            if (val < 0) return PilotColors.Red;
            return PilotColors.Grey;
        }
    }
}

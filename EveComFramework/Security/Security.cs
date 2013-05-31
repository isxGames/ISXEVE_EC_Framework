using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;

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
        Forced,
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

    public class SecuritySettings : Settings
    {
        public List<FleeTrigger> Triggers = new List<FleeTrigger>
        {
            FleeTrigger.Pod,
            FleeTrigger.NegativeStanding,
            FleeTrigger.NeutralStanding,
            FleeTrigger.Paranoid,
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

    public class UIData : State
    {
        #region Instantiation

        static UIData _Instance;
        public static UIData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UIData();
                }
                return _Instance;
            }
        }

        private UIData() : base()
        {
            QueueState(Update);
        }

        #endregion

        #region Variables

        public List<string> Bookmarks { get; set; }

        #endregion

        #region States

        bool Update(object[] Params)
        {
            if (!Session.Safe || (!Session.InStation && !Session.InSpace)) return false;
            Bookmarks = Bookmark.All.Select(a => a.Title).ToList();
            return false;
        }

        #endregion
    }

    public class Security : State
    {
        #region Instantiation

        static Security _Instance;
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
        }


        #endregion

        #region Variables

        List<Bookmark> SafeSpots;
        public SecuritySettings Config = new SecuritySettings();
        Move.Move Move = EveComFramework.Move.Move.Instance;
        Cargo.Cargo Cargo = EveComFramework.Cargo.Cargo.Instance;
        public Logger Log = new Logger("Security");
        Pilot Hostile = null;

        #endregion

        #region Events

        public event Action Alert;
        public event Action ClearAlert;

        #endregion

        #region Actions

        public void Start()
        {
            if (Idle)
            {
                QueueState(CheckSafe);
            }

        }

        public void Stop()
        {
            Clear();
        }

        public void Flee()
        {
            Clear();
            QueueState(Flee);
        }

        public void Reset(int? Delay = null)
        {
            int iDelay = Delay ?? Config.FleeWait * 60000;
            QueueState(Blank, iDelay);
            QueueState(CheckSafe);
        }

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

        bool CheckSafe(object[] Params)
        {


            if (Entity.All.FirstOrDefault(a => a.IsWarpScrambling && a.IsTargetingMe) != null)
            {
                return false;
            }

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
                        if (Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                        Log.Log("|r{0} is negative standing", Hostile.Name);
                        return true;
                    case FleeTrigger.NeutralStanding:
                        TriggerAlert();
                        QueueState(Flee, -1, FleeTrigger.NeutralStanding);
                        if (Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                        Log.Log("|r{0} is neutral standing", Hostile.Name);
                        return true;
                    case FleeTrigger.Targeted:
                        TriggerAlert();
                        QueueState(Flee, -1, FleeTrigger.Targeted);
                        Log.Log("|r{0} is targeting me", Hostile.Name);
                        if (Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                        return true;
                    case FleeTrigger.CapacitorLow:
                        TriggerAlert();
                        QueueState(Flee, -1, FleeTrigger.CapacitorLow);
                        if (Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                        Log.Log("|rCapacitor is below threshold (|w{0}%|r)", Config.CapThreshold);
                        return true;
                    case FleeTrigger.ShieldLow:
                        TriggerAlert();
                        QueueState(Flee, -1, FleeTrigger.ShieldLow);
                        if (Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                        Log.Log("|rShield is below threshold (|w{0}%|r)", Config.ShieldThreshold);
                        return true;
                    case FleeTrigger.ArmorLow:
                        TriggerAlert();
                        QueueState(Flee, -1, FleeTrigger.ArmorLow);
                        if (Drone.AllInSpace.Any()) Drone.AllInSpace.ReturnToDroneBay();
                        Log.Log("|rArmor is below threshold (|w{0}%|r)", Config.ArmorThreshold);
                        return true;
                }

            return false;
        }

        bool Decloak;

        bool CheckClear(object[] Params)
        {
            AutoModule.AutoModule.Instance.Decloak = false;
            if (SafeTrigger() != FleeTrigger.None) return false;
            return true;
        }

        bool Flee(object[] Params)
        {
            FleeTrigger Trigger = (FleeTrigger)Params[0];
            int FleeWait = Config.FleeWait * 60000;
            if (Trigger == FleeTrigger.ArmorLow || Trigger == FleeTrigger.CapacitorLow || Trigger == FleeTrigger.ShieldLow || Trigger == FleeTrigger.Forced) FleeWait = -1;

            Cargo.Clear();
            Move.Clear();

            Decloak = AutoModule.AutoModule.Instance.Decloak;

            QueueState(Traveling);
            QueueState(LogMessage, 1, string.Format("|oReached flee target"));
            QueueState(LogMessage, 1, string.Format(" |-gWaiting for safety"));
            QueueState(CheckClear);
            QueueState(LogMessage, 1, string.Format("|oArea is now safe"));
            QueueState(LogMessage, 1, string.Format(" |-gWaiting for |w{0}|-g minutes", FleeWait / 60000));
            QueueState(Resume, FleeWait);
            QueueState(CheckSafe);

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
            if (SafeTrigger() != FleeTrigger.None)
            {
                int FleeWait = Config.FleeWait * 60000;
                InsertState(Resume, FleeWait);
                InsertState(LogMessage, 1, string.Format(" |-gWaiting for |w{0}|-g minutes", FleeWait / 60000));
                InsertState(LogMessage, 1, string.Format("|oArea is now safe"));
                InsertState(CheckClear);
                InsertState(LogMessage, 1, string.Format(" |-gWaiting for safety"));
                InsertState(LogMessage, 1, string.Format("|oNew flee condition"));
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
            return true;
        }

        #endregion
    }
}

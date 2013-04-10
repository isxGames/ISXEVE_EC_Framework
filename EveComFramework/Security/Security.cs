using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework.Security
{
    #region Enums

    public enum FleeTrigger
    {
        Pod,
        NegativeStanding,
        NeutralStanding,
        Targeted,
        CapacitorLow,
        ShieldLow,
        ArmorLow
    }

    internal enum FleeType
    {
        NearestStation,
        SecureBookmark,
        SafeBookmarks
    }

    #endregion

    #region Settings

    internal class SecuritySettings : EveComFramework.Core.Settings
    {
        internal List<FleeTrigger> Triggers = new List<FleeTrigger>
        {
            FleeTrigger.Pod,
            FleeTrigger.NegativeStanding,
            FleeTrigger.NeutralStanding,
            FleeTrigger.Targeted,
            FleeTrigger.CapacitorLow,
            FleeTrigger.ShieldLow,
            FleeTrigger.ArmorLow
        };
        internal List<FleeType> Types = new List<FleeType>
        {
            FleeType.NearestStation,
            FleeType.SecureBookmark,
            FleeType.SafeBookmarks
        };
        internal bool NegativeAlliance = false;
        internal bool NegativeCorp = false;
        internal bool NegativeFleet = false;
        internal bool NeutralAlliance = false;
        internal bool NeutralCorp = false;
        internal bool NeutralFleet = false;
        internal bool TargetAlliance = false;
        internal bool TargetCorp = false;
        internal bool TargetFleet = false;
        internal int CapThreshold = 30;
        internal int ShieldThreshold = 30;
        internal int ArmorThreshold = 99;
        internal string SafeSubstring = "Safe:";
        internal string SecureBookmark = "";
    }

    #endregion


    public class AlertArg : EventArgs
    {
        public FleeTrigger trigger { get; set; }
        public AlertArg(FleeTrigger trigger)
        {
            this.trigger = trigger;
        }
    }

    public class Security : EveComFramework.Core.State
    {
        #region Variables

        List<Bookmark> SafeSpots;
        internal SecuritySettings Config = new SecuritySettings();
        Move.Move Move = EveComFramework.Move.Move.Instance;

        #endregion

        #region Events

        public event EventHandler<AlertArg> Alert;


        protected virtual void TriggerAlert(AlertArg e)
        {
            EventHandler<AlertArg> handler = Alert;

            if (handler != null)
            {
                handler(this, e);
            }
        }


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

        public void Configure()
        {
            UI.Security Configuration = new UI.Security();
            Configuration.Show();
        }

        #endregion

        #region States

        bool CheckSafe(object[] Params)
        {
            if (!Standing.Ready)
            {
                Standing.LoadStandings();
                return false;
            }

            foreach (FleeTrigger Trigger in Config.Triggers)
            {
                switch (Trigger)
                {
                    case FleeTrigger.Pod:
                        if (MyShip.ToItem.GroupID == Group.Capsule)
                        {
                            TriggerAlert(new AlertArg(FleeTrigger.Pod));
                            return true;
                        }
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
                        if (NegativePilots.Count > 0)
                        {
                            TriggerAlert(new AlertArg(FleeTrigger.NegativeStanding));
                            return true;
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
                        if (NeutralPilots.Count > 0)
                        {
                            TriggerAlert(new AlertArg(FleeTrigger.NeutralStanding));
                            return true;
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
                        if (TargetingPilots.Count > 0)
                        {
                            TriggerAlert(new AlertArg(FleeTrigger.Targeted));
                            return true;
                        }
                        break;
                    case FleeTrigger.CapacitorLow:
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapThreshold)
                        {
                            TriggerAlert(new AlertArg(FleeTrigger.CapacitorLow));
                            return true;
                        }
                        break;
                    case FleeTrigger.ShieldLow:
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if (MyShip.ToEntity.ShieldPct < Config.ShieldThreshold)
                        {
                            TriggerAlert(new AlertArg(FleeTrigger.ShieldLow));
                            return true;
                        }
                        break;
                    case FleeTrigger.ArmorLow:
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if (MyShip.ToEntity.ArmorPct < Config.ArmorThreshold)
                        {
                            TriggerAlert(new AlertArg(FleeTrigger.ArmorLow));
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        public void Flee()
        {
            if (Session.InStation)
            {
                return;
            }

            foreach (FleeType FleeType in Config.Types)
            {
                switch (FleeType)
                {
                    case FleeType.NearestStation:
                        if (Entity.All.FirstOrDefault(a => a.GroupID == Group.Station) != null)
                        {
                            Move.Object(Entity.All.FirstOrDefault(a => a.GroupID == Group.Station));
                            return;
                        }
                        break;
                    case FleeType.SecureBookmark:
                        if (Bookmark.All.Count(a => a.Title == Config.SecureBookmark) > 0)
                        {
                            Move.Bookmark(Bookmark.All.FirstOrDefault(a => a.Title == Config.SecureBookmark));
                            return;
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
                            return;
                        }
                        break;
                }
            }
        }

        #endregion
    }
}

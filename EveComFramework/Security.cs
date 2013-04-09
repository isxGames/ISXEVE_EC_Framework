using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework
{
    // Define a class to hold custom event info 
    internal class AlertArg : EventArgs
    {
        public enum EventType
        {
            Pod,
            NegativeStanding,
            NeutralStanding,
            Targeted,
            CapacitorLow,
            ShieldLow,
            ArmorLow
        }

        public EventType trigger { get; set; }
        public AlertArg(EventType trigger)
        {
            this.trigger = trigger;
        }

    }


    class Security : State
    {

        #region Instantiation

        #endregion

        #region Variables

        List<Bookmark> SafeSpots;

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

        public override void Start()
        {
            if (Idle)
            {
                QueueState(CheckSafe);
            }

        }

        public override void Stop()
        {
            Clear();
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

            foreach (string FleeTrigger in Config.FleeTriggers)
            {
                switch (FleeTrigger)
                {
                    case "In Pod":
                        if (MyShip.ToItem.GroupID == Group.Capsule)
                        {
                            TriggerAlert(new AlertArg(AlertArg.EventType.Pod));
                            return true;
                        }
                        break;
                    case "Negative Standing in Local":
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
                            TriggerAlert(new AlertArg(AlertArg.EventType.NegativeStanding));
                            return true;
                        }
                        break;
                    case "Neutral in Local":
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
                            TriggerAlert(new AlertArg(AlertArg.EventType.NeutralStanding));
                            return true;
                        }
                        break;
                    case "Targeted by another player":
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
                            TriggerAlert(new AlertArg(AlertArg.EventType.Targeted));
                            return true;
                        }
                        break;
                    case "Capacitor Low":
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapThreshold)
                        {
                            TriggerAlert(new AlertArg(AlertArg.EventType.CapacitorLow));
                            return true;
                        }
                        break;
                    case "Shield Low":
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if (MyShip.ToEntity.ShieldPct < Config.ShieldThreshold)
                        {
                            TriggerAlert(new AlertArg(AlertArg.EventType.ShieldLow));
                            return true;
                        }
                        break;
                    case "Armor Low":
                        if (!Session.InSpace)
                        {
                            break;
                        }
                        if (MyShip.ToEntity.ArmorPct < Config.ArmorThreshold)
                        {
                            TriggerAlert(new AlertArg(AlertArg.EventType.ArmorLow));
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

            foreach (string FleeType in Config.FleeTypes)
            {
                switch (FleeType)
                {
                    case "Flee to closest station in system":
                        if (Entity.All.FirstOrDefault(a => a.GroupID == Group.Station) != null)
                        {
                            Move.Object(Entity.All.FirstOrDefault(a => a.GroupID == Group.Station));
                            return true;
                        }
                        break;
                    case "Flee to secure bookmark":
                        if (Bookmark.All.Count(a => a.Title == Config.SecureBookmark) > 0)
                        {
                            Move.Bookmark(Bookmark.All.FirstOrDefault(a => a.Title == Config.SecureBookmark));
                            return true;
                        }
                        break;
                    case "Cycle safe bookmarks":
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
                    case "Flee to closest station outside system":
                        break;
                }
            }
        }

        #endregion

    }
}

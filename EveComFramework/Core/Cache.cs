using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework.Core
{
    /// <summary>
    /// This class provides cached information useful for user interfaces
    /// </summary>
    public class Cache : State
    {
        #region Instantiation

        static Cache _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static Cache Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Cache();
                }
                return _Instance;
            }
        }

        private Cache() : base()
        {
            ItemVolume = new Dictionary<string, double>();
            ShipVolume = new Dictionary<string, double>();
            CachedMissions = new Dictionary<string, CachedMission>();
            AvailableAgents = new List<string>();
            ShipNames = new HashSet<string>();
            QueueState(Control);

            LavishScriptAPI.LavishScript.Commands.AddCommand("failedFalconPunch", increment_pulse_because_birds_of_prey_are_douchebags);
        }

        public List<State> myStates = new List<State>();
        Random rnd = new Random();
        int increment_pulse_because_birds_of_prey_are_douchebags(string[] args)
        {
            if (!Security.Security.Instance.Config.falconPunch) return 0;

            foreach (State s in myStates.Where(a => a.ToString() != "Cache"))
            {
                if (DateTime.Now.AddMilliseconds(100) > s.NextPulse)
                {
                    s.NextPulse = DateTime.Now.AddMilliseconds(100 + rnd.Next(-100, 100));
                }
            }
            return 0;
        }

        #endregion

        #region Variables

        /// <summary>
        /// Your pilot's Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Your pilot's CharID
        /// </summary>
        public long CharID { get; set; }
        /// <summary>
        /// Array of bookmark titles
        /// </summary>
        public string[] Bookmarks { get; set; }
        /// <summary>
        /// Array of fleet member names
        /// </summary>
        public string[] FleetMembers { get; set; }
        /// <summary>
        /// Item Volumes, keyed by Types
        /// </summary>
        public Dictionary<string, double> ItemVolume { get; set; }
        public Dictionary<string, double> ShipVolume { get; set; }
        public HashSet<string> ShipNames { get; set; }
        public List<string> Fittings { get; set; }
        public Double ArmorPercent = 1;
        public Double HullPercent = 1;
        public bool DamagedDrones = false;
        public List<string> AvailableAgents { get; set; }

        public class CachedMission
        {
            public int ContentID;
            public string Name;
            public int Level;
            public AgentMission.MissionState State;
            public AgentMission.MissionType Type;
            internal CachedMission (int ContentID, string Name, int Level, AgentMission.MissionState State, AgentMission.MissionType Type)
            {
                this.ContentID = ContentID;
                this.Name = Name;
                this.Level = Level;
                this.State = State;
                this.Type = Type;
            }
        }
        public Dictionary<string, CachedMission> CachedMissions { get; set; }

        #endregion

        #region States

        DateTime BookmarkUpdate = DateTime.Now;
        bool Control(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            Name = Me.Name;
            CharID = Me.CharID;

            if (Bookmarks == null || BookmarkUpdate < DateTime.Now)
            {
                Bookmarks = Bookmark.All.Select(a => a.Title).ToArray();
                BookmarkUpdate = DateTime.Now.AddMinutes(1);
            }
            if (Session.InFleet) FleetMembers = Fleet.Members.Select(a => a.Name).ToArray();
            if (MyShip.CargoBay != null)
            {
                if (MyShip.CargoBay.IsPrimed)
                {
                    MyShip.CargoBay.Items.ForEach(a => { ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                }
                else
                {
                    MyShip.CargoBay.Prime();
                    return false;
                }
            }
            if (MyShip.DroneBay != null)
            {
                if (MyShip.DroneBay.IsPrimed)
                {
                    MyShip.DroneBay.Items.ForEach(a => { ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                }
                else
                {
                    MyShip.DroneBay.Prime();
                    return false;
                }
            }
            AgentMission.All.ForEach(a => { CachedMissions.AddOrUpdate(Agent.Get(a.AgentID).Name, new CachedMission(a.ContentID, a.Name, Agent.Get(a.AgentID).Level, a.State, a.Type)); });
            AvailableAgents = Agent.MyAgents.Select(a => a.Name).ToList();
            if (Session.InStation)
            {
                if (Station.ItemHangar != null)
                {
                    if (Station.ItemHangar.IsPrimed)
                    {
                        Station.ItemHangar.Items.ForEach(a => { ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                    }
                    else
                    {
                        Station.ItemHangar.Prime();
                        return false;
                    }
                }
                if (Station.ShipHangar != null)
                {
                    if (Station.ShipHangar.IsPrimed)
                    {
                        Station.ShipHangar.Items.ForEach(a => { ShipVolume.AddOrUpdate(a.Type, a.Volume); if (a.Name != null) ShipNames.Add(a.Name); });
                    }
                    else
                    {
                        Station.ShipHangar.Prime();
                        return false;
                    }
                }

                if (FittingManager.Ready)
                {
                    Fittings = FittingManager.Fittings.Select(fit => fit.Name).ToList();
                }
                else
                {
                    FittingManager.Prime();
                }
                //for (int i = 0; i <= 6; i++)
                //{
                //    if (Session.InStation && Station.CorpHangar(i) != null)
                //    {
                //        if (Station.CorpHangar(i).IsPrimed)
                //        {
                //            Station.CorpHangar(i).Items.ForEach(a => { ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                //        }
                //        else
                //        {
                //            Station.CorpHangar(i).Prime();
                //            return false;
                //        }
                //    }
                //}
            }

            if (Session.InSpace)
            {
                ArmorPercent = MyShip.Armor / MyShip.MaxArmor;
                HullPercent = MyShip.Hull / MyShip.MaxHull;
                if (Drone.AllInSpace.Any(a => a.ToEntity.ArmorPct < 100 || a.ToEntity.HullPct < 100)) DamagedDrones = true;
            }
            return false;
        }

        #endregion
    }

    #region Utility classes

    static class DictionaryHelper
    {
        public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }

    static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> method)
        {
            foreach (T item in items)
            {
                method(item);
            }
        }
    }

    #endregion
}

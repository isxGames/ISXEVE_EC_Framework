using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.KanedaToolkit;

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

        private Cache()
            : base()
        {
            ItemVolume = new Dictionary<string, double>();
            ShipVolume = new Dictionary<string, double>();
            CachedMissions = new Dictionary<string, CachedMission>();
            AvailableAgents = new List<string>();
            ShipNames = new HashSet<string>();
            QueueState(Control);
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
            internal CachedMission(int ContentID, string Name, int Level, AgentMission.MissionState State, AgentMission.MissionType Type)
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
                    if (MyShip.CargoBay.Items != null && MyShip.CargoBay.Items.Any())
                    {
                        MyShip.CargoBay.Items.ForEach(a => { ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                    }
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
                    if (MyShip.DroneBay.Items != null && MyShip.DroneBay.Items.Any())
                    {
                        MyShip.DroneBay.Items.ForEach(a => { ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                    }
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
                        if (Station.ItemHangar.Items != null && Station.ItemHangar.Items.Any())
                        {
                            Station.ItemHangar.Items.ForEach(a => { ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                        }
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
                        if (Station.ShipHangar.Items != null && Station.ShipHangar.Items.Any())
                        {
                            foreach (Item ship in Station.ShipHangar.Items.Where(ship => ship != null && ship.isUnpacked))
                            {
                                ShipVolume.AddOrUpdate(ship.Type, ship.Volume);
                                if (ship.Name != null) ShipNames.Add(ship.Name);
                            }
                        }
                    }
                    else
                    {
                        Station.ShipHangar.Prime();
                        return false;
                    }
                }

                if (FittingManager.Ready)
                {
                    if (FittingManager.Fittings != null && FittingManager.Fittings.Any())
                    {
                        Fittings = FittingManager.Fittings.Select(fit => fit.Name).ToList();
                    }
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

}

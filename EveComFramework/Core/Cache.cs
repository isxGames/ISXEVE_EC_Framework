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
        public Double ArmorPercent = 1;
        public Double HullPercent = 1;
        public bool DamagedDrones = false;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework.Core
{
    public class GlobalSettings : Settings
    {
        public GlobalSettings() : base("Cache") { }
        /// <summary>
        /// Item Volumes, keyed by Types
        /// </summary>
        public SerializableDictionary<string, double> ItemVolume = new SerializableDictionary<string, double>();
    }

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
            QueueState(Control);
        }

        #endregion

        #region Variables

        internal GlobalSettings GlobalConfig = new GlobalSettings();

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
        /// Array of cargo item type names
        /// </summary>
        [Obsolete("Depreciated:  Use ItemVolume dictionary (6/11/13)")]
        public string[] CargoItems { get; set; }
        /// <summary>
        /// Item Volumes, keyed by Types
        /// </summary>
        public Dictionary<string, double> ItemVolume { get { return GlobalConfig.ItemVolume; } }
        public Double ArmorPercent = 1;
        public Double HullPercent = 1;

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            Name = Me.Name;
            CharID = Me.CharID;
            Bookmarks = Bookmark.All.Select(a => a.Title).ToArray();
            if (Session.InFleet) FleetMembers = Fleet.Members.Select(a => a.Name).ToArray();
            if (MyShip.CargoBay != null && MyShip.CargoBay.IsPrimed) CargoItems = MyShip.CargoBay.Items.Distinct().Select(a => a.Type).ToArray();
            if (MyShip.CargoBay != null && MyShip.CargoBay.IsPrimed)
            {
                MyShip.CargoBay.Items.ForEach(a => { GlobalConfig.ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                GlobalConfig.Save();
            }
            if (Session.InStation && Station.ItemHangar != null && Station.ItemHangar.IsPrimed)
            {
                Station.ItemHangar.Items.ForEach(a => { GlobalConfig.ItemVolume.AddOrUpdate(a.Type, a.Volume); });
                GlobalConfig.Save();
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

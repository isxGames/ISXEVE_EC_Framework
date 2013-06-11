using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework.Core
{
    internal class GlobalSettings : Settings
    {
        internal GlobalSettings() : base("Cache") { }
        /// <summary>
        /// Item Volumes, keyed by Types
        /// </summary>
        internal SerializableDictionary<string, double> ItemVolume = new SerializableDictionary<string, double>();
    }

    public class Cache : State
    {
        #region Instantiation

        static Cache _Instance;
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
        /// Your pilot's name
        /// </summary>
        public string Name { get; set; }
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

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            Name = Me.Name;
            Bookmarks = Bookmark.All.Select(a => a.Title).ToArray();
            if (Session.InFleet) FleetMembers = Fleet.Members.Select(a => a.Name).ToArray();
            if (MyShip.CargoBay != null && MyShip.CargoBay.IsPrimed) CargoItems = MyShip.CargoBay.Items.Distinct().Select(a => a.Type).ToArray();
            if (MyShip.CargoBay != null && MyShip.CargoBay.IsPrimed) MyShip.CargoBay.Items.ForEach(a => { GlobalConfig.ItemVolume.AddOrUpdate(a.Type, a.Volume); });
            if (Session.InStation && Station.ItemHangar != null && Station.ItemHangar.IsPrimed) Station.ItemHangar.Items.ForEach(a => { GlobalConfig.ItemVolume.AddOrUpdate(a.Type, a.Volume); });
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

    public static class ForEachExtension
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

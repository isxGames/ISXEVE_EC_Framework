using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveComFramework.Core
{
    /// <summary>
    /// This class allows modules to signal they are busy
    /// </summary>
    public class Busy
    {
        #region Instantiation

        static Busy _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static Busy Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Busy();
                }
                return _Instance;
            }
        }

        private Busy()
            : base()
        {
        }

        #endregion

        static Dictionary<string, Action> BusySet = new Dictionary<string, Action>();

        /// <summary>
        /// Property indicating if any module is busy
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return BusySet.Count > 0;
            }
        }

        /// <summary>
        /// Property indicating if a specific module is busy
        /// </summary>
        /// <param name="Item">The name of the module</param>
        /// <returns>Busy true/false</returns>
        public bool ItemIsBusy(string Item)
        {
            return BusySet.ContainsKey(Item);
        }

        /// <summary>
        /// Set a module as busy
        /// </summary>
        /// <param name="Item">The name of the module</param>
        /// <param name="Signal">Action to perform when a module signals ready</param>
        public void SetBusy(string Item, Action Signal)
        {
            if (!BusySet.ContainsKey(Item))
            {
                BusySet.Add(Item, Signal);
            }
        }

        /// <summary>
        /// Set a module as not busy
        /// </summary>
        /// <param name="Item">The name of the module</param>
        public void SetDone(string Item)
        {
            if (BusySet.ContainsKey(Item))
            {
                BusySet.Remove(Item);
            }
        }

        /// <summary>
        /// Signal modules that are ready using the actions defined
        /// </summary>
        public void SignalReady()
        {
            BusySet.Values.ToList().ForEach(signal => signal());
        }
    }
}

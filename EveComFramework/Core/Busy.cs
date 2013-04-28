using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveComFramework.Core
{
    public class Busy
    {
        #region Instantiation

        static Busy _Instance;
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

        private Busy() : base()
        {
        }

        #endregion
        
        static Dictionary<string, Action> BusySet = new Dictionary<string, Action>();

        public bool IsBusy
        {
            get
            {
                return BusySet.Count > 0;
            }
        }

        public bool ItemIsBusy(string Item)
        {
            return BusySet.ContainsKey(Item);
        }

        public void SetBusy(string Item, Action Signal)
        {
            if (!BusySet.ContainsKey(Item))
            {
                BusySet.Add(Item, Signal);
            }
        }

        public void SetDone(string Item)
        {
            if (BusySet.ContainsKey(Item))
            {
                BusySet.Remove(Item);
            }
        }
        
        public void SignalReady()
        {
            BusySet.Values.ToList().ForEach(signal => signal());
        }
    }
}

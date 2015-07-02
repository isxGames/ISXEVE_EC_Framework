#pragma warning disable 1591
using System;
using System.Net;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.Stats
{
    public class StatsSettings : Settings
    {
        public StatsSettings() : base("Stats") { }
        public bool optIn;
        public bool optOut;
        public string guid;
    }

    public class Stats : State
    {
        #region Instantiation

        static Stats _Instance;
        public static Stats Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Stats();
                }
                return _Instance;
            }
        }

        private Stats()
        {
            QueueState(Control);
        }

        #endregion

        #region Variables
        public StatsSettings Config = new StatsSettings();
        Logger Log = new Logger("Stats");
        #endregion

        #region States

        bool Control(Object[] Params)
        {
            // no data submission allowed
            if (Config.optOut) return true;

            // Generate GUID if none present
            if (Config.guid == null)
            {
                Config.guid = Guid.NewGuid().ToString();
                Config.Save();
            }

            // Wait for proper session state
            if (!Session.InSpace && !Session.InSpace) return false;

            String data = String.Format(@"GUID={0}&regionID={1}&allianceID={2}&groupID={3}", Config.guid, Session.RegionID, Me.AllianceID, (int) MyShip.ToItem.GroupID);
            if (Config.optIn) // detailed data allowed
            {
                data = data + String.Format(@"&solarSystemID={0}&characterID={1}&typeID={2}", Session.SolarSystemID, Me.CharID, MyShip.ToItem.TypeID);
            }

            try
            {
                WebRequest.Create("http://104.238.149.13/evecom-stats/?" + data).GetResponse();
                Log.Log("|gStatistics submitted. Thank you!");
            }
            catch
            {
                Log.Log("|rNetwork connection failed");
            }
            return true;
        }
        #endregion
    }

}

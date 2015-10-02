#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private String StatsHost = "http://104.238.149.13/evecom-stats/";
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
                QueueState(DatabaseFeeder);
            }

            try
            {
                WebRequest.Create(StatsHost+"?" + data).GetResponse();
            }
            catch
            {
                Log.Log("|rNetwork connection failed");
            }
            return true;
        }

        private List<long> CustomsOffices = new List<long>();
        private bool DatabaseFeeder(object[] Params)
        {
            if (Session.Safe && Session.InSpace)
            {
                List<Entity> ReportCustomsOffices = Entity.All.Where(a => (a.TypeID == 2233 || a.TypeID == 4318) && !CustomsOffices.Contains(a.ID)).ToList();
                if (ReportCustomsOffices.Any())
                {
                    Entity POCO = ReportCustomsOffices.First();
                    String data = String.Format(@"GUID={0}&solarSystemID={1}&ownerID={2}&itemID={3}&typeID={4}&x={5}&y={6}&z={7}", Config.guid, Session.SolarSystemID, POCO.OwnerID, POCO.ID, POCO.TypeID, POCO.Position.X.ToString(CultureInfo.InvariantCulture), POCO.Position.Y.ToString(CultureInfo.InvariantCulture), POCO.Position.Z.ToString(CultureInfo.InvariantCulture));
                    try
                    {
                        Log.Log("Submit POCO data: " + data, LogType.DEBUG);
                        WebRequest.Create(StatsHost + "poco.php?" + data).GetResponse();
                        CustomsOffices.Add(POCO.ID);
                    }
                    catch
                    {
                        Log.Log("|rNetwork connection failed");
                        return true; // Stop reporting stuff if there is no reliable network connection
                    }
                }
            }
            return false;
        }
        #endregion
    }

}

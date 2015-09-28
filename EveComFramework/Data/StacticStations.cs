using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace EveComFramework.Data
{

    /// <summary>
    /// This class provides static information about stations (without pulling data from Eve)
    /// </summary>
    public class StaticStation
    {
        /// <summary>
        /// The station's ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// The station's Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The station's solarsystem ID
        /// </summary>
        public long SolarSystemID { get; set; }
        /// <summary>
        /// The station's constellation ID
        /// </summary>
        public long ConstellationID { get; set; }
        /// <summary>
        /// The station's region ID
        /// </summary>
        public long RegionID { get; set; }
        

        private StaticStation(long ID, string Name, long SolarSystemID, long ConstellationID, long RegionID)
        {
            this.ID = ID;
            this.Name = Name;
            this.SolarSystemID = SolarSystemID;
            this.ConstellationID = ConstellationID;
            this.RegionID = RegionID;
        }

        private static List<StaticStation> _All;
        /// <summary>
        /// List of all stations
        /// </summary>
        public static List<StaticStation> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.StaticStations.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from System in dataDoc.Descendants("Station")
                                select new StaticStation(Convert.ToInt64(System.Attribute("stationID").Value), System.Attribute("stationName").Value, Convert.ToInt64(System.Attribute("solarSystemID").Value), Convert.ToInt64(System.Attribute("constellationID").Value), Convert.ToInt64(System.Attribute("regionID").Value))).ToList();
                    }
                }
                return _All;
            }
        }
    }
}

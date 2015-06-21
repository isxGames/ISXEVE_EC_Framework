using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;

namespace EveComFramework.Data
{

    /// <summary>
    /// This class provides static information about solar systems (without pulling data from Eve)
    /// </summary>
    public class SolarSystem
    {
        /// <summary>
        /// The solar system's ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// The solar system's Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The solar system's security level
        /// </summary>
        public double Security { get; set; }

        /// <summary>
        /// The solar system's FactionID (Only works for High Sec)
        /// </summary>
        public long FactionID { get; set; }

        private SolarSystem(long ID, string Name, double Security, long FactionID)
        {
            this.ID = ID;
            this.Name = Name;
            this.Security = Security;
            this.FactionID = FactionID;
        }

        private static List<SolarSystem> _All;
        /// <summary>
        /// List of all solar systems
        /// </summary>
        public static List<SolarSystem> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.SolarSystems.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from System in dataDoc.Descendants("System")
                                select new SolarSystem(Convert.ToInt64(System.Attribute("ID").Value), System.Attribute("Name").Value, Convert.ToDouble(System.Attribute("Security").Value, CultureInfo.InvariantCulture), Convert.ToInt64(System.Attribute("FactionID").Value))).ToList();
                    }
                }
                return _All;
            }
        }
    }
}

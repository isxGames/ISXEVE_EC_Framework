using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;

namespace EveComFramework.Data
{
    public class SolarSystem
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public double Security { get; set; }

        private SolarSystem(long ID, string Name, double Security)
        {
            this.ID = ID;
            this.Name = Name;
            this.Security = Security;
        }

        private static List<SolarSystem> _All;
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
                                select new SolarSystem(Convert.ToInt64(System.Attribute("ID").Value), System.Attribute("Name").Value, Convert.ToDouble(System.Attribute("Security").Value))).ToList();
                    }
                }
                return _All;
            }
        }
    }
}

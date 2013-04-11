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
    class DroneType
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }

        private DroneType(long ID, string Name, string Group)
        {
            this.ID = ID;
            this.Name = Name;
            this.Group = Group;
        }

        private static List<DroneType> _All;
        public static List<DroneType> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.DroneTypes.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from a in dataDoc.Descendants("Drone")
                                select new DroneType(Convert.ToInt64(a.Attribute("ID").Value), a.Attribute("Name").Value, a.Attribute("Group").Value)).ToList();
                    }
                }
                return _All;
            }
        }
    }
}

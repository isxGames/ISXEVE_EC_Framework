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
    class PriorityTarget
    {
        public string Name { get; set; }


        private static List<string> _All;
        public static List<string> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.PriorityTargets.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from System in dataDoc.Descendants("Target")
                                select System.Attribute("Name").Value).ToList();
                    }
                }
                return _All;
            }
        }
    }
}

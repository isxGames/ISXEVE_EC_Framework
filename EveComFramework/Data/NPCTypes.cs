using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace EveComFramework.Data
{
    class NPCTypes
    {
        public string Name { get; set; }


        private static List<long> _All;
        public static List<long> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.NPCTypes.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from a in dataDoc.Descendants("Type")
                                select Convert.ToInt64(a.Value)).ToList();
                    }
                }
                return _All;
            }
        }
    }
}

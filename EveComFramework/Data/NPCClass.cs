using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using EveCom;

namespace EveComFramework.Data
{
    class NPCClasses
    {
        private static Dictionary<Group, string> _All;
        public static Dictionary<Group, string> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComBot.Data.NPCClasses.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from a in dataDoc.Descendants("Group")
                                select new { ID = Convert.ToInt64(a.Attribute("ID").Value), Class = a.Attribute("Class").Value }).ToDictionary(a => (Group)a.ID, a => a.Class);
                    }
                }
                return _All;
            }
        }
    }
}

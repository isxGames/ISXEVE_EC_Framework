using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace EveComFramework.Data
{

    /// <summary>
    /// This class provides static information about Inventory Groups (without pulling data from Eve)
    /// </summary>
    public class InvGroup
    {
        /// <summary>
        /// The InvGroup's ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The InvGroup's Name
        /// </summary>
        public string Name { get; set; }

        private InvGroup(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        private static List<InvGroup> _All;
        /// <summary>
        /// List of all InvGroups
        /// </summary>
        public static List<InvGroup> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.InvGroups.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from System in dataDoc.Descendants("InvGroup")
                                select new InvGroup(Convert.ToInt32(System.Attribute("ID").Value), System.Attribute("Name").Value)).ToList();
                    }
                }
                return _All;
            }
        }

        /// <summary>
        /// Get InvGroup by ID
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>InvGroup</returns>
        public static InvGroup ByID(int ID)
        {
            return All.FirstOrDefault(a => a.ID == ID);
        }

        /// <summary>
        /// Get InvGroups by Name
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>List of InvGroups</returns>
        public static List<InvGroup> ByName(string Name)
        {
            return All.Where(a => a.Name == Name).ToList();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;

namespace EveComFramework.Data
{

    /// <summary>
    /// This class provides static information about Inventory Types (without pulling data from Eve)
    /// </summary>
    public class InvType
    {
        /// <summary>
        /// The InvType's ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The InvType's Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The InvType's Group
        /// </summary>
        public int GroupID { get; set; }

        private InvType(int ID, string Name, int GroupID)
        {
            this.ID = ID;
            this.Name = Name;
            this.GroupID = GroupID;
        }

        private static List<InvType> _All;
        /// <summary>
        /// List of all InvTypes
        /// </summary>
        public static List<InvType> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.InvTypes.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from System in dataDoc.Descendants("InvType")
                                select new InvType(Convert.ToInt32(System.Attribute("ID").Value), System.Attribute("Name").Value, Convert.ToInt32(System.Attribute("GroupID").Value))).ToList();
                    }
                }
                return _All;
            }
        }

        /// <summary>
        /// Get InvType by ID
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>InvType</returns>
        public static InvType ByID(int ID)
        {
            return All.FirstOrDefault(a => a.ID == ID);
        }

        /// <summary>
        /// Get InvTypes by Name
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>List of InvTypes</returns>
        public static List<InvType> ByName(string Name)
        {
            return All.Where(a => a.Name == Name).ToList();
        }

    }
}

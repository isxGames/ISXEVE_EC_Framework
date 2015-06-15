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
    /// This class provides static information about dungeons (without pulling data from Eve)
    /// </summary>
    public class Dungeon
    {
        /// <summary>
        /// The Dungeon's NameID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The Dungeons's Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Dungeon Type
        /// </summary>
        public string Type { get; set; }

        private Dungeon(int ID, string Name, string Type)
        {
            this.ID = ID;
            this.Name = Name;
            this.Type = Type;
        }

        private static List<Dungeon> _All;
        /// <summary>
        /// List of all dungeons
        /// </summary>
        public static List<Dungeon> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.Dungeons.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from System in dataDoc.Descendants("Dungeon")
                                select new Dungeon(Convert.ToInt32(System.Attribute("ID").Value), System.Attribute("Name").Value, System.Attribute("Type").Value)).ToList();
                    }
                }
                return _All;
            }
        }

        /// <summary>
        /// Get dungeon by ID
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>Dungeon</returns>
        public static Dungeon ByID(int ID)
        {
            return All.FirstOrDefault(a => a.ID == ID);
        }

        /// <summary>
        /// Get dungeons by Name
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>List of Dungeons</returns>
        public static List<Dungeon> ByName(string Name)
        {
            return All.Where(a => a.Name == Name).ToList();
        }

    }
}

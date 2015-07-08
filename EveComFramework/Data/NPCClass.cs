#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using EveCom;

namespace EveComFramework.Data
{
    public class NPCClasses
    {
        private static Dictionary<Group, string> _All;
        public static Dictionary<Group, string> All
        {
            get
            {
                if (_All == null)
                {
                    using (Stream data = Assembly.GetExecutingAssembly().GetManifestResourceStream("EveComFramework.Data.NPCClasses.xml"))
                    {
                        XElement dataDoc = XElement.Load(data);
                        _All = (from a in dataDoc.Descendants("Group")
                                select new { ID = Convert.ToInt64(a.Attribute("ID").Value), Class = a.Attribute("Class").Value }).ToDictionary(a => (Group)a.ID, a => a.Class);
                    }
                }
                return _All;
            }
        }

        public static readonly List<String> FactionSpawns = new List<string>
        {
			"Sentient",
            "Dread Guristas",
            "Shadow Serpentis",
            "True Sansha",
            "Dark Blood",
            "Domination",
            "Psycho"
        };

        public static readonly List<String> OfficerSpawns = new List<string>
        {
            "Ahremen Arkah",
            "Brokara Ryver",
            "Brynn Jerdola",
            "Chelm Soran",
            "Cormack Vaaja",
            "Draclira Merlonne",
            "Estamel Tharchon",
            "Gotan Kreiss",
            "Hakim Stormare",
            "Kaikka Peunato",
            "Mizuro Cybon",
            "Raysere Giant",
            "Selynne Mardakar",
            "Setele Schellan",
            "Tairei Namazoth",
            "Thon Eney",
            "Tobias Kruzhoryy",
            "Tuvan Orth",
            "Unit D-34343",
            "Unit F-435454",
            "Unit P-343554",
            "Unit W-634",
            "Vepas Minimala",
            "Vizan Ankonin"
        };

    }
}

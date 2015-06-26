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
			"Unit D-34343",
			"Unit F-435454",
			"Unit P-343554",
			"Unit W-634",
			"Sentient Alvus Queen",
			"Sentient Alvus Controller",
			"Sentient Alvus Creator",
			"Sentient Alvus Ruler",
			"Sentient Domination Alvus",
			"Sentient Matriarch Alvus",
			"Sentient Patriarch Alvus",
			"Sentient Spearhead Alvus",
			"Sentient Supreme Alvus Parasite",
			"Sentient Swarm Preserver Alvus",
			"Sentient Crippler Alvatis",
			"Sentient Defeater Alvatis",
			"Sentient Enforcer Alvatis",
			"Sentient Exterminator Alvatis",
			"Sentient Siege Alvatis",
			"Sentient Striker Alvatis",
			"Sentient Annihilator Alvum",
			"Sentient Atomizer Alvum",
			"Sentient Bomber Alvum",
			"Sentient Destructor Alvum",
			"Sentient Devastator Alvum",
			"Sentient Disintegrator Alvum",
			"Sentient Nuker Alvum",
			"Sentient Violator Alvum",
			"Sentient Viral Infector Alvum",
			"Sentient Wrecker Alvum",
			"Sentient Dismantler Alvior",
			"Sentient Marauder Alvior",
			"Sentient Predator Alvior",
			"Sentient Ripper Alvior",
			"Sentient Shatter Alvior",
			"Sentient Shredder Alvior",
			"Sentient Barracuda Alvi",
			"Sentient Decimator Alvi",
			"Sentient Devilfish Alvi",
			"Sentient Hunter Alvi",
			"Sentient Infester Alvi",
			"Sentient Raider Alvi",
			"Sentient Render Alvi",
			"Sentient Silverfish Alvi",
			"Sentient Splinter Alvi",
			"Sentient Sunder Alvi",
            "Dread Guristas",
            "Shadow Serpentis",
            "True Sansha",
            "Dark Blood",
            "Domination",
            "Brynn Jerdola",
            "Cormack Vaaja",
            "Setele Schellan",
            "Tuvan Orth",
            "Estamel Tharchon",
            "Kaikka Peunato",
            "Thon Eney",
            "Vepas Minimala",
            "Domination Cherubim",
            "Domination Commander",
            "Domination General",
            "Domination Malakim",
            "Domination Nephilim",
            "Domination Saint",
            "Domination Seraphim",
            "Domination Throne",
            "Domination War General",
            "Domination Warlord",
            "Domination Legatus",
            "Domination Legionnaire",
            "Domination Praefectus",
            "Domination Primus",
            "Domination Tribuni",
            "Domination Tribunus",
            "Domination Breaker",
            "Domination Centurion",
            "Domination Crusher",
            "Domination Defeater",
            "Domination Depredator",
            "Domination Liquidator",
            "Domination Marauder",
            "Domination Phalanx",
            "Domination Predator",
            "Domination Smasher",
            "Domination Defacer",
            "Domination Defiler",
            "Domination Haunter",
            "Domination Seizer",
            "Domination Shatterer",
            "Domination Trasher",
            "Domination Ambusher",
            "Domination Hijacker",
            "Domination Hunter",
            "Domination Impaler",
            "Domination Nomad",
            "Domination Outlaw",
            "Domination Raider",
            "Domination Rogue",
            "Domination Ruffian",
            "Domination Thug",
            "Psycho Ambusher",
            "Psycho Hijacker",
            "Psycho Hunter",
            "Psycho Impaler",
            "Psycho Nomad",
            "Psycho Outlaw",
            "Psycho Raider",
            "Psycho Rogue",
            "Psycho Ruffian",
            "Psycho Thug",
            "Gotan Kreiss",
            "Hakim Stormare",
            "Mizuro Cybon",
            "Tobias Kruzhoryy",
            "Ahremen Arkah",
            "Draclira Merlonne",
            "Raysere Giant",
            "Tairei Namazoth",
            "Brokara Ryver",
            "Chelm Soran",
            "Selynne Mardakar",
            "Vizan Ankonin",
            "Gotan Kreiss",
            "Hakim Stormare",
            "Mizuro Cybon",
            "Tobias Kruzhoryy",
            "Ahremen Arkah",
            "Draclira Merlonne",
            "Raysere Giant",
            "Tairei Namazoth",
            "Brokara Ryver",
            "Chelm Soran",
            "Selynne Mardakar",
            "Vizan Ankonin"
        };

    }
}

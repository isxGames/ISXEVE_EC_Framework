using System;
using System.Collections.Generic;
using System.Linq;
using EveCom;

namespace EveComFramework.KanedaToolkit
{

    public static class PreferredBookmarkExtension
    {
        public static Bookmark PreferredBookmark(this IEnumerable<Bookmark> items, Func<Bookmark, bool> pred)
        {
            Bookmark inSystem = items.Where(pred).FirstOrDefault(a => a.LocationID == Session.SolarSystemID);
            if (inSystem != null)
            {
                return inSystem;
            }
            else
            {
                return items.FirstOrDefault();
            }
        }
    }

}

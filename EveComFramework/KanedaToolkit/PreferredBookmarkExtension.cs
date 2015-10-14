#pragma warning disable 1591
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
            return Bookmark.All.Where(pred)
                .Where(a => Route.GetPathBetween(a.LocationID).Any()) // ignore unreachable bookmarks (w-space)
                .OrderBy(a => Route.GetPathBetween(a.LocationID).Count) // nearest bookmark, prefer in system
                .First();
        }
    }

}

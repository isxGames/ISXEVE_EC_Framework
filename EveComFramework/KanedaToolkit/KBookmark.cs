using EveCom;

namespace EveComFramework.KanedaToolkit
{
    static class KBookmark
    {

        public static bool Dockable (this Bookmark bookmark)
        {
            if (bookmark.GroupID == Group.Station) return true;
            if (bookmark.GroupID == Group.MediumCitadel || bookmark.GroupID == Group.LargeCitadel || bookmark.GroupID == Group.XLargeCitadel || bookmark.GroupID == Group.XXLargeCitadel) return true;
            return false;
        }

    }
}

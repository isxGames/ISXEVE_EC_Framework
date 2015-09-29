#pragma warning disable 1591
using EveCom;

namespace EveComFramework.KanedaToolkit
{
    /// <summary>
    /// extension methods for Entity
    /// </summary>
    public static class KEntity
    {
        /// <summary>
        /// Is this entity warpable? (range, type)
        /// </summary>
        public static bool Warpable(this Entity entity)
        {
            if (entity.SurfaceDistance <= 150000) return false;
            if (entity.CategoryID == Category.Asteroid) return true;
            if (entity.CategoryID == Category.Structure) return true;
            if (entity.CategoryID == Category.Station) return true;
            if (entity.GroupID == Group.CargoContainer) return true;
            if (entity.GroupID == Group.Wreck) return true;
            return false;
        }

    }

}

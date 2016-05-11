#pragma warning disable 1591
using System.Linq;
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
            if (Session.InFleet && entity.IsPC && Fleet.Members.Any(a => a.Name == entity.Name)) return true;
            if (entity.CategoryID == Category.Asteroid || entity.CategoryID == Category.Structure ||
                entity.CategoryID == Category.Station || entity.GroupID == Group.CargoContainer ||
                entity.GroupID == Group.Wreck) return true;
            return false;
        }

        public static bool Collidable(this Entity entity)
        {
            if (entity.Type == "Beacon") return false;
            if (entity.GroupID == Group.LargeCollidableObject || entity.GroupID == Group.LargeCollidableShip ||
                entity.GroupID == Group.LargeCollidableStructure || entity.CategoryID == Category.Asteroid) return true;
            return false;
        }

    }

}

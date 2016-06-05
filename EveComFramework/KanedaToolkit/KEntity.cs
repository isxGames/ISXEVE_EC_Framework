#pragma warning disable 1591
using System.Linq;
using EVE.ISXEVE;
using EVE.ISXEVE.Enums;

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
				entity.GroupID == Group.Wreck || entity.GroupID == Group.MediumCitadel ||
				entity.GroupID == Group.LargeCitadel || entity.GroupID == Group.XLargeCitadel ||
				entity.GroupID == Group.XXLargeCitadel) return true;
			return false;
		}

		/// <summary>
		/// Is this entity a collidable object
		/// </summary>
		public static bool Collidable(this Entity entity)
		{
			if (entity.Type == "Beacon") return false;
			if (entity.GroupID == Group.LargeCollidableObject || entity.GroupID == Group.LargeCollidableShip ||
				entity.GroupID == Group.LargeCollidableStructure || entity.CategoryID == Category.Asteroid) return true;
			return false;
		}

		/// <summary>
		/// Is this entity dockable
		/// </summary>
		public static bool Dockable(this Entity entity)
		{

			if (entity.GroupID == Group.XLargeCitadel || entity.GroupID == Group.XXLargeCitadel) return true;
			if (MyShip.ToEntity.GroupID == Group.Titan || MyShip.ToEntity.GroupID == Group.Supercarrier) return false;
			if (entity.GroupID == Group.Station) return true;
			if (entity.GroupID == Group.LargeCitadel) return true;
			if (MyShip.ToEntity.GroupID == Group.Carrier || MyShip.ToEntity.GroupID == Group.Dreadnought || MyShip.ToEntity.GroupID == Group.ForceAuxiliary) return false;
			if (entity.GroupID == Group.MediumCitadel) return true;
			return false;
		}

	}

}

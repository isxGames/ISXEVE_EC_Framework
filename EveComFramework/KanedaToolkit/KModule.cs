using EveCom;

namespace EveComFramework.KanedaToolkit
{
    /// <summary>
    /// extension methods for Module
    /// </summary>
    public static class KModule
    {
        /// <summary>
        /// Is this module in a state where it can be activated?
        /// </summary>
        public static bool AllowsActivate(this Module module)
        {
            return module.IsOnline && !module.IsActive && !module.IsActivating && !module.IsDeactivating;
        }

        /// <summary>
        /// Is this module in a state where it can be deactivated?
        /// </summary>
        public static bool AllowsDeactivate(this Module module)
        {
            return module.IsOnline && module.IsActive && !module.IsDeactivating;
        }

        /// <summary>
        /// Is this module valid for this target?
        /// </summary>
        /// <param name="module"></param>
        /// <param name="target">Entity to check against</param>
        public static bool ValidTarget(this Module module, Entity target)
        {
            // Gas Cloud Harvester
            if (module.GroupID == Group.GasCloudHarvester && target.GroupID != Group.HarvestableCloud) return false;

            // Ice Harvester
            if (MiningToolkit.IceModules.Contains(module.TypeID) && target.GroupID != Group.Ice) return false;

            // Mining Lasers / Ore
            if (!MiningToolkit.OreGroups.Contains(target.GroupID) &&
                (module.GroupID == Group.MiningLaser ||
                 (module.GroupID == Group.StripMiner && !MiningToolkit.IceModules.Contains(module.TypeID)) ||
                 module.GroupID == Group.FrequencyMiningLaser)) return false;

            // Mercoxit
            if (!MiningToolkit.MercoxitModules.Contains(module.TypeID) && target.GroupID == Group.Mercoxit) return false;

            // Target painters don't work with control towers
            if (module.GroupID == Group.TargetPainter && target.GroupID == Group.ControlTower) return false;
            
            // Salvager
            if (module.GroupID == Group.Salvager && target.GroupID != Group.Wreck) return false;

            // Tractor Beam
            if (module.GroupID == Group.TractorBeam && target.GroupID != Group.Wreck &&
                target.GroupID != Group.CargoContainer) return false;

            // Supercapitals are immune to ewar
            if ((module.GroupID == Group.WarpScrambler || module.GroupID == Group.TargetPainter ||
                 module.GroupID == Group.StasisWeb) &&
                (target.GroupID == Group.Supercarrier || target.GroupID == Group.Titan)) return false;

            // return true if we don't know about module copatiblity
            return true;
        }

    }
}

#pragma warning disable 1591
using System;
using EveCom;
using System.Collections.Generic;
using System.Linq;

namespace EveComFramework.KanedaToolkit
{

    /// <summary>
    /// Mining Toolkit
    /// </summary>
    public class MiningToolkit
    {

        #region Instantiation
        static MiningToolkit _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static MiningToolkit Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MiningToolkit();
                }
                return _Instance;
            }
        }

        private MiningToolkit()
        {

        }

        #endregion

        #region Variables
        public static readonly Dictionary<string, double> CrystalYield = new Dictionary<string, double> 
        {
            {"Arkonor Mining Crystal I", 1.625},
            {"Bistot Mining Crystal I", 1.625},
            {"Crokite Mining Crystal I", 1.625},
            {"Dark Ochre Mining Crystal I", 1.625},
            {"Gneiss Mining Crystal I", 1.625},
            {"Hedbergite Mining Crystal I", 1.625},
            {"Hemorphite Mining Crystal I", 1.625},
            {"Jaspet Mining Crystal I", 1.625},
            {"Kernite Mining Crystal I", 1.625},
            {"Mercoxit Mining Crystal I", 1.625},
            {"Omber Mining Crystal I", 1.625},
            {"Plagioclase Mining Crystal I", 1.625},
            {"Pyroxeres Mining Crystal I", 1.625},
            {"Scordite Mining Crystal I", 1.625},
            {"Spodumain Mining Crystal I", 1.625},
            {"Veldspar Mining Crystal I", 1.625},
            {"Arkonor Mining Crystal II", 1.75},
            {"Bistot Mining Crystal II", 1.75},
            {"Crokite Mining Crystal II", 1.75},
            {"Dark Ochre Mining Crystal II", 1.75},
            {"Gneiss Mining Crystal II", 1.75},
            {"Hedbergite Mining Crystal II", 1.75},
            {"Hemorphite Mining Crystal II", 1.75},
            {"Jaspet Mining Crystal II", 1.75},
            {"Kernite Mining Crystal II", 1.75},
            {"Mercoxit Mining Crystal II", 1.75},
            {"Omber Mining Crystal II", 1.75},
            {"Plagioclase Mining Crystal II", 1.75},
            {"Pyroxeres Mining Crystal II", 1.75},
            {"Scordite Mining Crystal II", 1.75},
            {"Spodumain Mining Crystal II", 1.75},
            {"Veldspar Mining Crystal II", 1.75}
        };

        public static readonly List<int> IceModules = new List<int>() { 16278, 22229, 28752, 37450, 37451, 37452 };
        public static readonly List<int> MercoxitModules = new List<int>() { 12108, 28748, 18068, 24305 };
        public static readonly List<Group> OreGroups = new List<Group>()
        {
            Group.Arkonor,
            Group.Bistot,
            Group.Crokite,
            Group.DarkOchre,
            Group.Gneiss,
            Group.Hedbergite,
            Group.Hemorphite,
            Group.Jaspet,
            Group.Kernite,
            Group.Omber,
            Group.Omber,
            Group.Plagioclase,
            Group.Pyroxeres,
            Group.Scordite,
            Group.Spodumain,
            Group.Veldspar
        };
        #endregion

        #region Helper Methods

        /// <summary>
        /// Load mining crystal for a given asteroid
        /// Returns false once the operation is completed, returns true to notify that another call is required
        /// </summary>
        Dictionary<Module, DateTime> CrystalSwapCooldown = new Dictionary<Module, DateTime>();
        public bool LoadModule(Module mod, Entity roid)
        {
            if (mod.Capacity > 0)
            {
                // Module is offline
                if (!mod.IsOnline)
                {
                    EVEFrame.Log("LoadModule: Module offline");
                    return false;
                }

                // Module is busy - wait
                if (mod.IsReloading || mod.IsActive || mod.IsActivating || mod.IsDeactivating)
                {
                    EVEFrame.Log("LoadModule: Module is busy - wait");
                    return true;
                }

                if (mod.Charge != null)
                {
                    // Do we already have the correct crystal loaded
                    if (MatchingMiningCrystal(roid).Contains(mod.Charge.TypeID)) return false;
                }

                Item matchingCrystal = MyShip.CargoBay.Items.FirstOrDefault(a => MatchingMiningCrystal(roid) != null && MatchingMiningCrystal(roid).Contains(a.TypeID));

                // We're on cooldown from swapping or unloading crystals right now
                if (CrystalSwapCooldown.ContainsKey(mod) && DateTime.Now < CrystalSwapCooldown[mod])
                {
                    EVEFrame.Log("LoadModule: Cooldown");
                    return true;
                }

                if (!MyShip.CargoBay.IsPrimed)
                {
                    MyShip.CargoBay.Prime();
                    return true;
                }

                // Cargo is full, can't unload mining crystal
                if (mod.Charge != null && (MyShip.CargoBay.MaxCapacity - MyShip.CargoBay.UsedCapacity) < mod.Charge.Volume)
                {
                    return false;
                }

                // We have the correct crystal in cargo, load it
                if (matchingCrystal != null)
                {
                    mod.LoadAmmo(matchingCrystal);
                    CrystalSwapCooldown.AddOrUpdate(mod, DateTime.Now.AddSeconds(5));
                    return true;
                }

                // We don't have the correct crystal in cargo and there's a charge loaded, unload it
                if (mod.Charge != null)
                {
                    mod.UnloadAmmo();
                    CrystalSwapCooldown.AddOrUpdate(mod, DateTime.Now.AddSeconds(5));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get compatible mining crystal types for a given asteroid
        /// </summary>
        /// <param name="Asteroid">Asteroid</param>
        /// <returns></returns>
        public List<int> MatchingMiningCrystal(Entity Asteroid)
        {
            if (Asteroid.CategoryID != Category.Asteroid)
                return null;

            switch (Asteroid.GroupID)
            {
                case Group.Arkonor:
                    return new List<int> { 18590, 18036 };

                case Group.Bistot:
                    return new List<int> { 18592, 18038 };

                case Group.Crokite:
                    return new List<int> { 18594, 18040 };

                case Group.DarkOchre:
                    return new List<int> { 18596, 18042 };

                case Group.Gneiss:
                    return new List<int> { 18598, 18044 };

                case Group.Hedbergite:
                    return new List<int> { 18600, 18046 };

                case Group.Hemorphite:
                    return new List<int> { 18602, 18048 };

                case Group.Jaspet:
                    return new List<int> { 18604, 18050 };

                case Group.Kernite:
                    return new List<int> { 18606, 18052 };

                case Group.Mercoxit:
                    return new List<int> { 18608, 18054 };

                case Group.Omber:
                    return new List<int> { 18610, 18056 };

                case Group.Plagioclase:
                    return new List<int> { 18612, 18058 };

                case Group.Pyroxeres:
                    return new List<int> { 18614, 18060 };

                case Group.Scordite:
                    return new List<int> { 18616, 18062 };

                case Group.Spodumain:
                    return new List<int> { 18624, 18064 };

                case Group.Veldspar:
                    return new List<int> { 18618, 18066 };

            }

            return null;
        }

        public static double OreMined(Entity Roid, Dictionary<Module,int> CycleCounts)
        {
            return Roid.ActiveModules.Sum(mod =>
            {
                if (mod.Charge != null && CrystalYield.ContainsKey(mod.Charge.Type))
                {
                    return mod.MiningYield * CrystalYield[mod.Charge.Type] * (mod.Completion + CycleCounts[mod]);
                }
                return mod.MiningYield * (mod.Completion + CycleCounts[mod]);
            }).Value;
        }

        #endregion
    }
    
}

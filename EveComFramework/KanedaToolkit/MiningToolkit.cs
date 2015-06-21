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

        #region Helper Methods

        /// <summary>
        /// Load mining crystal for a given asteroid
        /// </summary>
        Dictionary<Module, DateTime> CrystalSwapCooldown = new Dictionary<Module, DateTime>();
        public bool LoadModule(Module mod, Entity roid)
        {
            if (mod.Capacity > 0)
            {
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

        #endregion
    }
    
}

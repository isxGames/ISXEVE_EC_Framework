#pragma warning disable 1591
using EveCom;

namespace EveComFramework.KanedaToolkit
{
    /// <summary>
    /// extension methods for Module
    /// </summary>
    public static class KModule
    {
        public static bool AllowsActivate(this Module module)
        {
            return module.IsOnline && !module.IsActive && !module.IsActivating && !module.IsDeactivating;
        }

        public static bool AllowsDectivate(this Module module)
        {
            return module.IsOnline && module.IsActive && !module.IsDeactivating;
        }
    }
}

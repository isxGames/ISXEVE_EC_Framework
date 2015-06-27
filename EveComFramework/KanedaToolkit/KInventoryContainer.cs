#pragma warning disable 1591
using EveCom;

namespace EveComFramework.KanedaToolkit
{
    /// <summary>
    /// extension methods for InventoryContainer
    /// </summary>
    public static class KInventoryContainer
    {
        public static double AvailCargo(this InventoryContainer inventoryContainer)
        {
            return inventoryContainer.MaxCapacity - inventoryContainer.UsedCapacity;
        }
    }

}

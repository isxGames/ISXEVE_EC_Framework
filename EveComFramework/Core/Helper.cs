#pragma warning disable 1591
using System.Linq;
using EveCom;

namespace EveComFramework.Core
{
    public class Helper
    {
        Cache Cache = Cache.Instance;

        public bool RepairShip(Logger Console)
        {
            // If ship needs to be repaired, do so
            Window repairShopWindow;
            if ((Cache.ArmorPercent != 1 || Cache.HullPercent != 1 || Cache.DamagedDrones) && Station.HasService(Station.Services.RepairFacilities))
            {
                if (Window.All.Any(a => a.Name != null && a.Name == "Set Quantity"))
                {
                    Console.Log("|oAccepting repair quantity");
                    Window repairQuantityWindow = Window.All.FirstOrDefault(a => a.Name != null && a.Name == "Set Quantity");
                    if (repairQuantityWindow != null)
                    {
                        repairQuantityWindow.ClickButton(Window.Button.OK);
                        Cache.ArmorPercent = 1;
                        Cache.HullPercent = 1;
                        Cache.DamagedDrones = false;
                    }
                    return false;
                }
                if (!Window.All.Any(a => a.Name != null && a.Name == "repairshop"))
                {
                    Console.Log("|oShip needs to be repaired");
                    Console.Log("|oHull: |w{0} |oArmor: |w{1} |oDamaged Drones: |w{2}", Cache.HullPercent, Cache.ArmorPercent, Cache.DamagedDrones);
                    MyShip.ToItem.RepairQuote();
                }
                else
                {
                    Console.Log("|oClicking RepairAll");
                    repairShopWindow = Window.All.FirstOrDefault(a => a.Name != null && a.Name == "repairshop");
                    if (repairShopWindow != null)
                    {
                        repairShopWindow.ClickButton(Window.Button.RepairAll);
                    }
                }
                return false;
            }
            repairShopWindow = Window.All.FirstOrDefault(a => a.Name != null && a.Name == "repairshop");
            if (repairShopWindow != null)
            {
                repairShopWindow.Close();
                return false;
            }

            return true;
        }
    }
}

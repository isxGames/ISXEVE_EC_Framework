using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.Core
{
    public class Helper
    {
        Cache Cache = Cache.Instance;

        public bool RepairShip(Logger Console)
        {
            // If ship needs to be repaired, do so
            if ((Cache.ArmorPercent != 1 || Cache.HullPercent != 1 || Cache.DamagedDrones) && Station.HasService(Station.Services.RepairFacilities))
            {
                if (Window.All.Any(a => a.Name != null && a.Name == "Set Quantity"))
                {
                    Console.Log("|oAccepting repair quantity");
                    Window.All.FirstOrDefault(a => a.Name != null && a.Name == "Set Quantity").ClickButton(Window.Button.OK);
                    Cache.ArmorPercent = 1;
                    Cache.HullPercent = 1;
                    Cache.DamagedDrones = false;
                    return false;
                }
                if (!Window.All.Any(a => a.Name != null && a.Name == "repairshop"))
                {
                    Console.Log("|oShip needs to be repaired");
                    Console.Log("|oRequesting quote");
                    MyShip.ToItem.RepairQuote();
                }
                else
                {
                    Console.Log("|oClicking RepairAll");
                    Window.All.FirstOrDefault(a => a.Name != null && a.Name == "repairshop").ClickButton(Window.Button.RepairAll);
                }
                return false;
            }
            if (Window.All.Any(a => a.Name != null && a.Name == "repairshop"))
            {
                Window.All.FirstOrDefault(a => a.Name != null && a.Name == "repairshop").Close();
                return false;
            }

            return true;
        }
    }
}

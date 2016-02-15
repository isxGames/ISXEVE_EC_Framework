#pragma warning disable 1591
using System.Linq;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.SessionControl
{
    public class DTWindowCleanup : State
    {
        #region Instantiation

        static DTWindowCleanup _Instance;
        public static DTWindowCleanup Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DTWindowCleanup();
                }
                return _Instance;
            }
        }

        private DTWindowCleanup()
        {
            QueueState(Control);
        }

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if (!Session.Safe) return false;

            //close downtime warning windows
            PopupWindow dtWindow = (PopupWindow)Window.All.FirstOrDefault(a => a.Name == "modal" && a.Type == Window.WindowType.PopUp && ((PopupWindow)a).Message.ToLower().Contains("downtime"));
            if (dtWindow != null)
            {
                dtWindow.Close();
                return false;
            }

            PopupWindow droneCallgroupWindow = (PopupWindow) Window.All.FirstOrDefault(a => a.Type == Window.WindowType.PopUp && ((PopupWindow)a).Message.Contains("You are already performing a LaunchDrones call"));
            if (droneCallgroupWindow != null)
            {
                droneCallgroupWindow.Close();
                return false;
            }
            return false;
        }

        #endregion
    }

}

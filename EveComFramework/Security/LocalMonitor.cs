#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.Security
{
    public class LocalMonitor : State
    {
        #region Instantiation

        static LocalMonitor _Instance;
        public static LocalMonitor Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LocalMonitor();
                }
                return _Instance;
            }
        }

        private LocalMonitor()
        {
            QueueState(Control);
        }

        #endregion

        #region Variables
        internal readonly Logger Log = new Logger("Local");
        private long solarSystem;
        private List<Pilot> localPilots;
        #endregion

        #region Events
        public event Action OnLocalChange;
        #endregion

        #region States

        bool Control(object[] Params)
        {
            if (!Session.InSpace && !Session.InStation) return false;

            if (solarSystem == Session.SolarSystemID && localPilots != null)
            {
                if (localPilots.Count != Local.Pilots.Count || Local.Pilots.Any(p => !localPilots.Contains(p)) || localPilots.Any(p => !Local.Pilots.Contains(p)))
                {
                    if (OnLocalChange != null)
                        OnLocalChange();
                }
                else
                {
                    return false;
                }
            }

            solarSystem = Session.SolarSystemID;
            localPilots = Local.Pilots;

            return false;
        }

        #endregion
    }

}

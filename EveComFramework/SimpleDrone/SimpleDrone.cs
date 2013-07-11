using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;
using EveComFramework.Targets;

namespace EveComFramework.SimpleDrone
{
    #region Settings

    public class LocalSettings : EveComFramework.Core.Settings
    {
        public bool ShortRangeClear = true;
        public bool LongRangeClear = false;
        public bool Sentry = false;
    }

    #endregion

    public class SimpleDrone : State
    {
        #region Instantiation

        static SimpleDrone _Instance;
        public static SimpleDrone Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SimpleDrone();
                }
                return _Instance;
            }
        }

        private SimpleDrone() : base()
        {
            Rats.AddPriorityTargets();
            Rats.AddNPCs();
            Rats.AddTargetingMe();

            Rats.Ordering = new RatComparer();            
        }

        #endregion

        #region Variables

        public Core.Logger Log = new Core.Logger("SimpleDrone");
        public LocalSettings Config = new LocalSettings();
        Targets.Targets Rats = new Targets.Targets();
        public Dictionary<long, long> ActiveTargetList = new Dictionary<long, long>();

        #endregion

        #region Actions

        public void Enabled(bool var)
        {
            if (var)
            {
                if (Idle)
                {
                    QueueState(Control);
                }
            }
            else
            {
                Clear();
            }
        }

        public void Configure()
        {
            //UI.DroneControl Configuration = new UI.DroneControl();
            //Configuration.Show();
        }

        #endregion

        #region States

        Entity ActiveTarget;

        bool Control(object[] Params)
        {
            if (!Session.InSpace)
            {
                return false;
            }

            // If we're warping and drones are in space, recall them and stop the module
            if (MyShip.ToEntity.Mode == EntityMode.Warping && Drone.AllInSpace.Any())
            {
                Drone.AllInSpace.ReturnToDroneBay();
                return true;
            }



            return false;
        }

        #endregion
    }
}

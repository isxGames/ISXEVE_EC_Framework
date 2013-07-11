using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.SimpleDrone
{
    #region Settings

    public class DroneControlSettings : EveComFramework.Core.Settings
    {
        public bool ShortRangeClear = true;
        public bool LongRangeClear = false;
        public bool Sentry = false;
    }

    #endregion

    class SimpleDrone : State
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

        }

        #endregion


    }
}

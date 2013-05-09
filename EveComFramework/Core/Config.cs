using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveComFramework.Core
{
    public class Config
    {
        #region Instantiation

        static Config _Instance;
        public static Config Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Config();
                }
                return _Instance;
            }
        }

        private Config() : base()
        {
            InUse = false;
        }

        #endregion

        public string DefaultProfile { get; set; }
        public bool InUse { get; set; }
    }
}

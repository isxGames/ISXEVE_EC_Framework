using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveComFramework.Core
{
    /// <summary>
    /// This class is used to define default information for the Settings module
    /// </summary>
    public class Config
    {
        #region Instantiation

        static Config _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
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
        }

        #endregion

        /// <summary>
        /// The default profile name to use if one is not defined
        /// </summary>
        public string DefaultProfile { get; set; }

        /// <summary>
        /// A custom name to use for specialized global settings filed (bot-dependent)
        /// </summary>
        public string CustomGlobal { get; set; }
    }
}

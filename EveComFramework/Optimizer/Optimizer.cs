using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.Optimizer
{
    #region Settings

    /// <summary>
    /// Settings for the Optimizer class
    /// </summary>
    public class OptimizerSettings : Settings
    {
        public bool Enable3D = true;
        public decimal MaxMemorySize = 200;
    }

    #endregion

    /// <summary>
    /// This class helps reduce the Eve client's resource utilization
    /// </summary>
    internal class Optimizer : State
    {
        [DllImport("kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr hProcess, uint dwMinimumWorkingSetSize, uint dwMaximumWorkingSetSize);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        #region Instantiation

        static Optimizer _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        internal static Optimizer Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Optimizer();
                }
                return _Instance;
            }
        }

        private Optimizer()
        {
            QueueState(Control);
        }

        #endregion

        #region Variables

        /// <summary>
        /// Config for this module
        /// </summary>
        public OptimizerSettings Config = new OptimizerSettings();
        DateTime NextMemoryPulse = DateTime.Now;

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if (EVEFrame.Enable3DRendering != Config.Enable3D && Session.Safe && (Session.InSpace || Session.InStation))
            {
                EVEFrame.Enable3DRendering = Config.Enable3D;
            }
            
            if (DateTime.Now > NextMemoryPulse)
            {
                SetProcessWorkingSetSize(GetCurrentProcess(), Convert.ToUInt32(0), Convert.ToUInt32(Config.MaxMemorySize * 1048576));
                NextMemoryPulse = DateTime.Now.AddMinutes(2);
            }

            return false;
        }

        #endregion
    }
}

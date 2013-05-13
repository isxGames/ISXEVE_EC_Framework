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

    public class OptimizerSettings : Settings
    {
        public bool Enable3D = true;
        public int MaxMemorySize = 200;
    }

    #endregion

    public class Optimizer : State
    {
        [DllImport("kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr hProcess, uint dwMinimumWorkingSetSize, uint dwMaximumWorkingSetSize);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        #region Instantiation

        static Optimizer _Instance;
        public static Optimizer Instance
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

        ~Optimizer()
        {
            if (!EVEFrame.Enable3DRendering) EVEFrame.Enable3DRendering = true;
        }

        private Optimizer() : base()
        {
            QueueState(Control);
        }

        #endregion

        #region Actions

        public void Configure()
        {
            UI.Optimizer Configuration = new UI.Optimizer();
            Configuration.Show();
        }
        #endregion

        #region Variables

        public OptimizerSettings Config = new OptimizerSettings();
        DateTime NextMemoryPulse = DateTime.Now;

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            if (EVEFrame.Enable3DRendering != Config.Enable3D) EVEFrame.Enable3DRendering = Config.Enable3D;

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

#pragma warning disable 1591
using System;
using System.Windows.Forms;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.KanedaToolkit
{
    public class Bot : State
    {

        public Security.Security SecurityModule = null;

        #region Status
        public enum BotStatus
        {
            StatusActive,
            StatusActiveAlert,
            StatusActivePanic,
            StatusInactive
        }

        public BotStatus Status
        {
            get
            {
                if (SecurityModule != null)
                {
                    if (SecurityModule.IsPanic) return BotStatus.StatusActivePanic;
                    if (SecurityModule.IsAlert) return BotStatus.StatusActiveAlert;
                }
                if (Idle) return BotStatus.StatusInactive;
                return BotStatus.StatusActive;
            }
        }
        #endregion

        #region Start/Stop
        public void Start()
        {
            if (Idle)
            {
                QueueState(Initialize);
            }
        }

        public void Stop()
        {
            
        }

        public void Enabled(bool val)
        {
            if (val)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }
        #endregion

        #region States
        protected bool Initialize(object[] Params)
        {
            return (Session.Safe && (Session.InSpace || Session.InStation));
        }

        protected bool Blank(object[] Params)
        {
            return false;
        }
        #endregion
    }

    public class BotUI : Form
    {
        protected Bot Bot = null;
        private Timer wuTimer = new Timer();

        public BotUI()
        {
            wuTimer.Enabled = true;
            wuTimer.Interval = 100;
            wuTimer.Start();
            wuTimer.Tick += wuTimer_Tick;
        }

        private void wuTimer_Tick(object o, EventArgs e)
        {
            if (Bot != null)
            {
                switch (Bot.Status)
                {
                    case Bot.BotStatus.StatusInactive:
                        TaskbarProgress.SetState(Handle, TaskbarProgress.TaskbarStates.NoProgress);
                        break;
                    case Bot.BotStatus.StatusActive:
                        TaskbarProgress.SetState(Handle, TaskbarProgress.TaskbarStates.Normal, 100, 100);
                        break;
                    case Bot.BotStatus.StatusActiveAlert:
                    case Bot.BotStatus.StatusActivePanic:
                        TaskbarProgress.SetState(Handle, TaskbarProgress.TaskbarStates.Error, 100, 100);
                        break;
                }
            }
        }

    }
}

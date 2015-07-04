#pragma warning disable 1591
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
                if(Idle) return BotStatus.StatusInactive;
                if (SecurityModule != null)
                {
                    if (SecurityModule.IsPanic) return BotStatus.StatusActivePanic;
                    if (SecurityModule.IsAlert) return BotStatus.StatusActiveAlert;
                }
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
}

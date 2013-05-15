using EveComFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveComFramework.LoginControl
{
    [Serializable]
    public class Profile
    {
        public string Username;
        public string Password;
        public long CharacterID;
    }

    public class LoginSettings : EveComFramework.Core.Settings
    {
        public List<Profile> Profiles = new List<Profile>();
        public int DownTimeHour = 11;
        public int DownTimeMinute = 30;
    }
    
   

    public class LoginControl : State
    {
        #region Instantiation

        static LoginControl _Instance;
        public static LoginControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LoginControl();
                }
                return _Instance;
            }
        }

        private LoginControl()
            : base()
        {
        }


        #endregion

        #region Variables

        public TimeSpan DownTimeLength = new TimeSpan(0,30,0);
        public LoginSettings Config = new LoginSettings();
        public Logger Log = new Logger("LoginControl");
        public DTControl DTControl = DTControl.Instance;

        private Profile _curProfile;
        private bool _dtCallback;
        private int _minutesBeforeDT;
        private bool _login;
        #endregion

        #region Events

        public event Action LogOut;
        public event Action LoggedIn;

        #endregion

        #region EventHandlers

        private void DTEnded()
        {
            DTControl.DTEnded -= DTEnded;
            if (_curProfile != null)
            {
                QueueState(LoginScreen);
            }
        }

        private void DTStarting(int minutes)
        {
            if (minutes <= _minutesBeforeDT)
            {
                DTControl.DTStarting -= DTStarting;
                if (_dtCallback)
                {
                    if (LogOut != null)
                    {
                        LogOut();
                    }
                }
                else
                {
                    DoLogout();
                }
            }
        }

        #endregion

        #region Actions

        public void DoLogin(string profileUName , bool force = false)
        {
            _curProfile = Config.Profiles.FirstOrDefault(a => a.Username == profileUName);
            if ( _curProfile != null)
            {
                if (DTControl.IsDT)
                {
                    Log.Log("Currently DT , waiting for end before trying to log in");
                    DTControl.DTEnded += DTEnded;
                }
                else
                {
                    Log.Log("Started logging in");
                    QueueState(LoginScreen);
                }                
            }
        }

        public void DoLogout()
        {
            DislodgeCurState(Logout);
        }

        public void WatchForDT(int minutesbefore = 10 , bool callback = true)
        {
            if (Idle)
            {
                _minutesBeforeDT = minutesbefore;
                if (callback)
                {
                    _dtCallback = true;
                    DTControl.DTStarting += DTStarting;
                }
                else
                {
                    _dtCallback = false;
                    DTControl.DTStarting += DTStarting;
                }
            }
            else
            {
                Log.Log("Logincontrol busy , can't watch for DT now");
            }
        }

        public void Configure()
        {
            UI.LoginControl Configuration = new UI.LoginControl();
            Configuration.Show();
        }

        #endregion

        #region States

        #region Utility

        bool Blank(object[] Params)
        {
            Log.Log("Finished");
            return true;
        }

        #endregion

        #region LoggingIn

        bool LoginScreen(object[] Params)
        {
            QueueState(WaitConnect);
            return true;
        }

        bool WaitConnect(object[] Params)
        {
            QueueState(CharScreen);
            return true;
        }

        bool CharScreen(object[] Params)
        {
            QueueState(WaitLoad);
            return true;
        }

        bool WaitLoad(object[] Params)
        {
            return true;
        }
        #endregion

        #region LoggingOut

        bool Logout(object[] Params)
        {
            return true;
        }

        #endregion

        #region Downtime

        bool MonitorDT(object[] Params)
        {
            return true;
        }

        #endregion

        #endregion
    }

    public class DTControl : State
    {
        #region Instantiation

        static DTControl _Instance;
        public static DTControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DTControl();
                }
                return _Instance;
            }
        }

        private DTControl()
            : base()
        {
            QueueState(MonitorDT, 5000);
        }


        #endregion

        #region Variables

        public TimeSpan DownTimeLength = new TimeSpan(0, 30, 0);
        public Logger Log = new Logger("DTControl");
        public bool IsDT = false;

        #endregion

        #region Events

        public event Action DTEnded;
        public event Action<int> DTStarting;

        #endregion

        #region States

        #region Utility

        bool Blank(object[] Params)
        {
            Log.Log("Finished");
            return true;
        }

        #endregion

        bool MonitorDT(object[] Params)
        {
            return false;
        }

        #endregion
    }
}

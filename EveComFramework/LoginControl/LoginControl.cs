using EveComFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveComFramework.LoginControl
{
    [Serializable]
    public class PlaySession
    {
        public DateTime Login;
        public DateTime Logout;
    }

    [Serializable]
    public class Profile
    {
        public List<PlaySession> Sessions;
        public string Username;
        public string Password;
        public long CharacterID;
    }

    public class LoginGlobalSettings : EveComFramework.Core.Settings
    {
        public LoginGlobalSettings() : base("global") { }
        public SerializableDictionary<string, Profile> Profiles = new SerializableDictionary<string,Profile>();
        public int DownTimeHour = 11;
        public int DownTimeMinute = 30;
    }
    
    public class UIData : State
    {
        #region Variables

        public long CharID;
        public string CharName;

        #endregion
        #region Instantiation

        static UIData _Instance;
        public static UIData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UIData();
                }
                return _Instance;
            }
        }

        private UIData()
            : base()
        {
            QueueState(GetUIData);
        }


        #endregion

        public event Action GotData;
        #region States

        bool GetUIData(object[] Params)
        {
            if (EveCom.Me.Name != null && EveCom.Me.CharID != null)
            {
                CharID = EveCom.Me.CharID;
                CharName = EveCom.Me.Name;
                if (GotData != null)
                {
                    GotData();                    
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

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
        public LoginGlobalSettings Config = new LoginGlobalSettings();
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

        public void DoLogin(string characterName , bool force = false)
        {
            _curProfile = Config.Profiles[characterName];
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
                    QueueState(CharScreen);
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
            Configuration.ShowDialog();
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
            if (EveCom.Session.InSpace || EveCom.Session.InStation)
            {
                Log.Log("Already logged in!");
                if (LoggedIn != null)
                {
                    LoggedIn();
                }
                Clear();
                return true;
            }
            if (!EveCom.Login.AtLogin || EveCom.Login.Connecting)
            {
                EveCom.Login.Connect(_curProfile.Username, _curProfile.Password);
                InsertState(LoginScreen);
                WaitFor(5, () => EveCom.Login.Connecting);
                return true;
            }
            return true;
        }

        bool WaitConnect(object[] Params)
        {
            if (EveCom.Login.Connecting || EveCom.Login.Loading)
            {
                return false;
            }
            else if (EveCom.CharSel.AtCharSel && EveCom.CharSel.Ready)
            {
                Log.Log("At char selection screen");
                QueueState(CharScreen);
                return true;
            }
            return false;
        }

        bool CharScreen(object[] Params)
        {
            if (EveCom.Session.InSpace || EveCom.Session.InStation)
            {
                Log.Log("Loaded and in space");
                PlaySession newSession = new PlaySession();
                newSession.Login = EveCom.Session.Now;
                newSession.Logout = EveCom.Session.Now.AddMinutes(1);
                QueueState(WaitLoad);
                return true;
            }
            if (EveCom.CharSel.Loading)
            {
                return false;
            }
            if (EveCom.CharSel.AtCharSel && EveCom.CharSel.Ready && !EveCom.CharSel.Loading)
            {
                foreach (EveCom.CharSel.CharSlot character in EveCom.CharSel.Slots)
                {
                    if (character.CharID == _curProfile.CharacterID)
                    {
                        Log.Log("Activating character {0}", _curProfile.CharacterID.ToString());
                        character.Activate();
                        return false;
                    }
                }
            }
            return false;
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

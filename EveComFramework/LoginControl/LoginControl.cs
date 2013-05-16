using EveComFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InnerSpaceAPI;
using EveCom;
namespace EveComFramework.LoginControl
{
    /// <summary>
    /// Used to document periods a bot is running
    /// </summary>  
    [Serializable]
    public class PlaySession
    {
        /// <summary>
        /// The time the session started
        /// </summary>
        public DateTime Login;
        /// <summary>
        /// The time the session ended, could be up to 1 minute out
        /// </summary>
        public DateTime Logout;
    }

    /// <summary>
    /// Userprofile for an eve account including play sessions, can be serialized
    /// </summary>
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
        /// <summary>
        /// Available userprofiles, keyed by the character name
        /// </summary>
        public SerializableDictionary<string, Profile> Profiles = new SerializableDictionary<string,Profile>();
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

    /// <summary>
    /// Logincontrol provides interface for logging in and out of Eve and awareness of downtime
    /// </summary>
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

        public LoginGlobalSettings Config = new LoginGlobalSettings();
        public Logger Log = new Logger("LoginControl");
        public DTControl DTControl = DTControl.Instance;

        private Profile _curProfile;
        private bool _dtCallback;
        private int _minutesBeforeDT;
        private bool _logged_in = false;
        #endregion

        #region Events

        /// <summary>
        /// Fired when LoginControl thinks it is time to get ready to log out, call DoLogOut afterwards to finish it
        /// </summary>
        public event Action LogOut;
        /// <summary>
        /// Fired when LoginControl has finished logging in to the specified character
        /// </summary>
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

        private void DTStarting(double minutes)
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

        /// <summary>
        /// Attempts to log in, if it's DT it will wait for DT end then try, LoggedIn event is fired when this finishes
        /// </summary>
        /// <param name="characterName"></param>
        /// <param name="force"></param>
        public void DoLogin(string characterName)
        {
            if (!Idle)
            {
                _curProfile = Config.Profiles[characterName];
                if (_curProfile != null)
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
            else
            {
                Log.Log("Busy, can't log in now");
            }
        }
        /// <summary>
        /// Will close the process! Don't do this unless you are ready!
        /// </summary>
        public void DoLogout()
        {
            Clear();
            QueueState(Logout);
        }
        /// <summary>
        /// Fires LogOut event x minutes before DT, can be set to just automatically quit
        /// </summary>
        /// <param name="minutesbefore">Changes how long before DT it will log you out</param>
        /// <param name="callback">If true it will fire the LogOut event instead of just logging out</param>
        public void LogoutOnDT(int minutesbefore = 10 , bool callback = true)
        {
            _minutesBeforeDT = minutesbefore;
            if (callback)
            {
                _dtCallback = true;
                //small trick to make sure we only subscribe once
                DTControl.DTStarting -= DTStarting;
                DTControl.DTStarting += DTStarting;
            }
            else
            {
                _dtCallback = false;
                //small trick to make sure we only subscribe once
                DTControl.DTStarting -= DTStarting;
                DTControl.DTStarting += DTStarting;
            }            
        }
        /// <summary>
        /// Opens up the configuration dialog, this is a MODAL dialog and will block the thread!
        /// </summary>
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
                _logged_in = true;
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
                if (LoggedIn != null)
                {
                    LoggedIn();
                }
                _logged_in = true;
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

        #endregion

        #region LoggingOut

        bool Logout(object[] Params)
        {
            if (_logged_in)
            {
                _curProfile.Sessions.Last().Logout = EveCom.Session.Now;
            }
            Config.Save();
            LavishScriptAPI.LavishScript.ExecuteCommand("Exit");            
            return true;
        }

        #endregion        

        #endregion
    }

    /// <summary>
    /// Provides access to information about downtime, fires events related to downtime
    /// </summary>
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

        public int DowntimeHour = 11;
        public int DowntimeMinute = 00;
        public int DowntimeLength = 30;
        public DateTime NextDownTimeStart;
        public DateTime NextDownTimeEnd;
        public Logger Log = new Logger("DTControl");
        /// <summary>
        /// True if it's currently scheduled downtime
        /// </summary>
        public bool IsDT = false;

        #endregion

        #region Events

        /// <summary>
        /// Fires when DT ends
        /// </summary>
        public event Action DTEnded;
        /// <summary>
        /// Fires every cycle, the parameter is when the number of minutes till DT starts, stops firing during DT
        /// </summary>
        public event Action<double> DTStarting;
        /// <summary>
        /// Fires once, when DT starts
        /// </summary>
        public event Action DTStarted;

        #endregion

        private void SetNextDT()
        {
            DateTime now = EveCom.Session.Now;
            DateTime todaysDTStart = new DateTime(now.Year, now.Month, now.Day, DowntimeHour, DowntimeMinute, 0);

            if (todaysDTStart.Subtract(now).TotalMinutes > 0)
            {
                //havent had DTend yet , set it to today
                NextDownTimeStart = new DateTime(now.Year, now.Month, now.Day, DowntimeHour, DowntimeMinute, 00);
            }
            else
            {
                NextDownTimeStart = new DateTime(now.Year, now.Month, now.Day, DowntimeHour, DowntimeMinute, 00).AddDays(1);
            }
            Log.Log("Next downtime will be {0}", NextDownTimeStart.ToString("MM/dd/yyyy h:mm tt"));
        }

        private void SetNextDTEnd()
        {
            DateTime now = EveCom.Session.Now;
            DateTime todaysDTEnd = new DateTime(now.Year, now.Month, now.Day, DowntimeHour, DowntimeMinute, 0).AddMinutes(DowntimeLength);
            Log.Log("{0}", todaysDTEnd.Subtract(now).TotalMinutes);
            if (todaysDTEnd.Subtract(now).TotalMinutes > 0)
            {
                //havent had DTend yet , set it to today
                NextDownTimeEnd = new DateTime(now.Year, now.Month, now.Day, DowntimeHour, DowntimeMinute, 00).AddMinutes(DowntimeLength);
            }
            else
            {
                NextDownTimeEnd = new DateTime(now.Year, now.Month, now.Day, DowntimeHour, DowntimeMinute, 00).AddDays(1).AddMinutes(DowntimeLength);               
            }
            Log.Log("Next downtime will end {0}", NextDownTimeEnd.ToString("MM/dd/yyyy h:mm tt"));
        }

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
            //will only happen on initialization
            if (NextDownTimeStart.Ticks == 0)
            {
                SetNextDT();
                SetNextDTEnd();
            }
            //has dt ended
            if (NextDownTimeEnd < EveCom.Session.Now)
            {
                Log.Log("Downtime has ended");
                if (DTEnded != null)
                {
                    DTEnded();
                }
                SetNextDTEnd();
                IsDT = false;  
            }
            //time to work out next DT start?
            if (NextDownTimeStart < EveCom.Session.Now)
            {
                Log.Log("Downtime has begun!");
                SetNextDT();
                if (DTStarted != null)
                {
                    DTStarted();
                }
            }
            //is it downtime right now?
            if (NextDownTimeEnd.Subtract(EveCom.Session.Now).TotalMinutes < DowntimeLength)
            {  
                IsDT = true;
                return false;
            }
            //check for a window, override default DT if there is one
            Window dtWindow = Window.All.FirstOrDefault(a => a.Caption.ToLower().Contains("downtime"));
            if (dtWindow != null)
            {
                //for now just say DT is happening right this second, later parse for the time
                if (DTStarting != null)
                {
                    DTStarting(0);
                    return false;
                }
            }
            //otherwise assume as scheduled
            if (DTStarting != null)
            {
                DTStarting(NextDownTimeStart.Subtract(EveCom.Session.Now).TotalMinutes);
            }
            return false;
        }

        #endregion
    }
}

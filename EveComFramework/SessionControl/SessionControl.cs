using EveComFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InnerSpaceAPI;
using EveCom;

namespace EveComFramework.SessionControl
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

    public class LoginGlobalSettings : Settings
    {
        public LoginGlobalSettings() : base("Login") { }
        /// <summary>
        /// Available userprofiles, keyed by the character name
        /// </summary>
        public SerializableDictionary<string, Profile> Profiles = new SerializableDictionary<string,Profile>();
    }

    public class LoginLocalSettings : Settings
    {
        public int LoginDelta = 10;
        public int LogoutHours = 4;
        public int LogoutDelta = 20;
        public int Downtime = 30;
        public int DowntimeDelta = 10;
    }
    
    public class UIData : State
    {
        #region Variables

        public long CharID { get; set; }
        public string CharName { get; set; }

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

        private UIData() : base()
        {
            QueueState(Update);
        }


        #endregion

        #region States

        bool Update(object[] Params)
        {
            CharID = Me.CharID;
            CharName = Me.Name;
            return false;
        }
        #endregion
    }

    /// <summary>
    /// Sessioncontrol provides interface for logging in and out of Eve and awareness of downtime
    /// </summary>
    public class SessionControl : State
    {
        #region Instantiation

        static SessionControl _Instance;
        public static SessionControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SessionControl();
                }
                return _Instance;
            }
        }

        private SessionControl() : base()
        {
            LoginDelta = random.Next(Config.LoginDelta);
            QueueState(LoginScreen);
            QueueState(CharScreen);
            QueueState(Monitor);
        }

        ~SessionControl()
        {
            if (_curProfile != null)
            {
                using (new EVEFrameLock())
                {
                    _curProfile.Sessions.Last().Logout = EveCom.Session.Now;
                }
                GlobalConfig.Save();
            }
        }

        #endregion

        #region Variables

        public LoginGlobalSettings GlobalConfig = new LoginGlobalSettings();
        public LoginLocalSettings Config = new LoginLocalSettings();
        public Logger Log = new Logger("LoginControl");
        public string characterName = string.Empty;
        DateTime Instanced = DateTime.Now;
        DateTime SessionStart;
        Random random = new Random();
        int DowntimeDelta = 0;
        int LoginDelta = 0;
        int LogoutDelta = 0;

        private Profile _curProfile;

        #endregion

        #region Events

        /// <summary>
        /// Fired when LoginControl thinks it is time to get ready to log out, call PerformLogout afterwards to finish it
        /// </summary>
        public event Action LogOut;

        #endregion

        #region Actions

        public void UpdateCurrentProfile()
        {
            if (GlobalConfig.Profiles.ContainsKey(characterName)) _curProfile = GlobalConfig.Profiles[characterName];
        }

        public void PerformLogout()
        {
            QueueState(Logout);
        }

        /// <summary>
        /// Opens up the configuration dialog, this is a MODAL dialog and will block the thread!
        /// </summary>
        public void Configure()
        {
            UI.SessionControl Configuration = new UI.SessionControl();
            Configuration.ShowDialog();
        }

        public void NewDowntimeDelta()
        {
            DowntimeDelta = random.Next(Config.DowntimeDelta);
        }
        public void NewLoginDelta()
        {
            LoginDelta = random.Next(Config.LoginDelta);
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
            UpdateCurrentProfile();
            if (Session.InSpace || Session.InStation || CharSel.AtCharSel) return true;
            
            if (Login.AtLogin)
            {
                if (Login.Loading || Login.Connecting) return false;
                Window Message = Window.All.FirstOrDefault(a => a.Caption.Contains("There is a new build available"));
                if (Message != null)
                {
                    Message.ClickButton(Window.Button.Yes);
                    return false;
                }
                Message = Window.All.FirstOrDefault(a =>    a.Caption.Contains("A client update is available") ||
                                                            a.Caption.Contains("The client update has been installed.") ||
                                                            a.Caption.Contains("The update has been downloaded.") ||
                                                            a.Caption.Contains("The daily downtime will begin in") ||
                                                            a.Caption.Contains("The connection to the server was closed") ||
                                                            a.Caption.Contains("Unable to connect to the selected server.") ||
                                                            a.Caption.Contains("At any time you can log in to the account management page"));
                if (Message != null)
                {
                    Message.ClickButton(Window.Button.OK);
                    return false;
                }                
                if (Window.All.Any(a => a.Caption.Contains("Account subscription expired") ||
                                        a.Caption.Contains("has been disabled")))
                {
                    Log.Log("|rLogin failed, stopping script");
                    Clear();
                    return true;
                }

                if (_curProfile != null && DateTime.Now > Instanced.AddMinutes(LoginDelta))
                {
                    Log.Log("|oLogging into account");
                    Log.Log(" |g{0}", _curProfile.Username);
                    Login.Connect(_curProfile.Username, _curProfile.Password);
                    InsertState(LoginScreen);
                    WaitFor(5, () => Login.Connecting);
                    return true;
                }
            }
            return false;
        }

        bool CharScreen(object[] Params)
        {
            UpdateCurrentProfile();
            if (Session.InSpace || Session.InStation)
            {
                PlaySession newSession = new PlaySession();
                newSession.Login = Session.Now;
                newSession.Logout = Session.Now.AddMinutes(1);
                if (_curProfile != null)
                {
                    _curProfile.Sessions.Add(newSession);
                    GlobalConfig.Save();
                }
                SessionStart = DateTime.Now.AddMinutes(random.Next(Config.LogoutDelta));
                DowntimeDelta = random.Next(Config.DowntimeDelta);
                LogoutDelta = random.Next(Config.LogoutDelta);
                return true;
            }
            if (CharSel.Loading) return false;

            if (CharSel.AtCharSel && CharSel.Ready)
            {
                if (_curProfile != null)
                {
                    CharSel.CharSlot character = CharSel.Slots.FirstOrDefault(a => a.CharID == _curProfile.CharacterID);
                    Log.Log("|oActivating character");
                    Log.Log(" |g{0}", characterName);
                    character.Activate();
                    InsertState(CharScreen);
                    WaitFor(5, () => CharSel.Loading);
                    return true;
                }
                else
                {
                    Log.Log("|rUnable to find character, check configuration");
                    Clear();
                    return true;
                }
            }

            return false;        
        }

        #endregion

        bool Monitor(object[] Params)
        {
            //close downtime warning windows
            Window dtWindow = Window.All.FirstOrDefault(a => a.Caption != null && a.Caption.ToLower().Contains("downtime"));
            if (dtWindow != null)
            {
                dtWindow.Close();
                return false;
            }

            if (DateTime.Now > SessionStart.AddHours(Config.LogoutHours).AddMinutes(LogoutDelta) ||
                DateTime.Now.AddMinutes(Config.Downtime + DowntimeDelta) > Session.NextDowntime)
            {
                if (LogOut != null)
                {
                    LogOut();
                }
                return true;
            }

            return false;
        }

        #region LoggingOut

        bool Logout(object[] Params)
        {
            if (_curProfile != null)
            {
                _curProfile.Sessions.Last().Logout = EveCom.Session.Now;
                GlobalConfig.Save();
            }
            LavishScriptAPI.LavishScript.ExecuteCommand("Exit");            
            return true;
        }

        #endregion        

        #endregion
    }


}

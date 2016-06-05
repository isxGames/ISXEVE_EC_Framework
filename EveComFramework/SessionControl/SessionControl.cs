#pragma warning disable 1591
using EveComFramework.Core;
using System;
using System.Linq;
using EVE.ISXEVE;
using EveComFramework.KanedaToolkit;
using LavishScriptAPI;

namespace EveComFramework.SessionControl
{

	/// <summary>
	/// Userprofile for an eve account including play sessions, can be serialized
	/// </summary>
	[Serializable]
	public class Profile
	{
		public string Username;
		public string Password;
		public long CharacterID;
	}

	/// <summary>
	/// Global settings for SessionControl class
	/// </summary>
	public class LoginGlobalSettings : Settings
	{
		public LoginGlobalSettings() : base("Login") { }
		/// <summary>
		/// Available userprofiles, keyed by the character name
		/// </summary>
		new public SerializableDictionary<string, Profile> Profiles = new SerializableDictionary<string, Profile>();
	}

	/// <summary>
	/// Profile-based settings for SessionControl class
	/// </summary>
	public class LoginLocalSettings : Settings
	{
		public string Mode = "Duration";
		public int LoginDelta = 0;
		public int LogoutHours = 24;
		public int LogoutDelta = 20;
		public int Downtime = 30;
		public int DowntimeDelta = 10;
		public DateTime PeriodStart = DateTime.Now;
		public DateTime PeriodEnd = DateTime.Now.AddHours(2);
		public SerializableDictionary<long, DateTime> SessionStart = new SerializableDictionary<long, DateTime>();
		public SerializableDictionary<long, bool> Reconnect = new SerializableDictionary<long, bool>();
	}

	/// <summary>
	/// Sessioncontrol provides interface for logging in and out of Eve and awareness of downtime
	/// </summary>
	public class SessionControl : State
	{
		#region Instantiation

		static SessionControl _Instance;
		/// <summary>
		/// Singletoner
		/// </summary>
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

		private SessionControl()
		{
			LoginDelta = random.Next(Config.LoginDelta);
			QueueState(LoginScreen);
			QueueState(CharScreen);
			QueueState(Monitor);
		}

		#endregion

		#region Variables

		/// <summary>
		/// Global config containing all login information
		/// </summary>
		public LoginGlobalSettings GlobalConfig = new LoginGlobalSettings();
		/// <summary>
		/// Config for this class
		/// </summary>
		public LoginLocalSettings Config = new LoginLocalSettings();
		/// <summary>
		/// Log for this class
		/// </summary>
		public Logger Log = new Logger("LoginControl");
		/// <summary>
		/// The character name to work with
		/// </summary>
		public string characterName { get; set; }

		DateTime Instanced = DateTime.Now;
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

		/// <summary>
		/// Sets up _curProfile with data from GlobalConfig
		/// </summary>
		public void UpdateCurrentProfile()
		{
			if (characterName == null) characterName = Cache.Instance.Name;
			if (characterName != null && GlobalConfig.Profiles.ContainsKey(characterName)) _curProfile = GlobalConfig.Profiles[characterName];
		}

		/// <summary>
		/// Perform a logout (closes the client)
		/// </summary>
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

		#endregion

		#region LoggingIn

		bool LoginScreen(object[] Params)
		{
			UpdateCurrentProfile();
			if (Session.InSpace || Session.InStation || CharSel.AtCharSel) return true;

			if (Login.AtLogin)
			{
				if (Login.Loading) return false;
				if (Login.ServerStatus != "Status: OK") return false;
				PopupWindow Message = Window.All.OfType<PopupWindow>().FirstOrDefault(a => a.Message.Contains("A client update is available") ||
															a.Message.Contains("The daily downtime will begin in") ||
															a.Message.Contains("The connection to the server was closed") ||
															a.Message.Contains("Unable to connect to the selected server.") ||
															a.Message.Contains("Connection Failed"));
				if (Message != null)
				{
					EVEFrame.Log("Click");
					Message.ClickButton(Window.Button.OK);
					return false;
				}
				if (Window.All.OfType<PopupWindow>().Any(a => a.Message.Contains("Account subscription expired") ||
										a.Message.Contains("has been disabled")))
				{
					Log.Log("|rLogin failed, stopping script");
					Clear();
					return true;
				}

				if (Login.Connecting) return false;

				if (_curProfile != null)
				{
					if (Config.Mode == "Duration" && DateTime.Now <= Instanced.AddMinutes(LoginDelta)) return false;
					if (Config.Mode == "Period" && DateTime.Now.TimeOfDay < Config.PeriodStart.TimeOfDay) return false;
					if (Config.Mode == "Period" && DateTime.Now.TimeOfDay > Config.PeriodEnd.TimeOfDay)
					{
						Log.Log("|oRun period already complete, closing");
						Clear();
						QueueState(Logout);
					}
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
				if (_curProfile != null)
				{
					if (Config.Reconnect == null || !Config.Reconnect.ContainsKey(_curProfile.CharacterID) || !Config.Reconnect[_curProfile.CharacterID])
					{
						Config.SessionStart.AddOrUpdate(_curProfile.CharacterID, DateTime.Now);
						Config.Reconnect.AddOrUpdate(_curProfile.CharacterID, false);
						Config.Save();
					}
					else
					{
						Config.Reconnect.AddOrUpdate(_curProfile.CharacterID, false);
						Config.Save();
					}
				}
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
					if (character != null)
					{
						Log.Log("|oActivating character");
						Log.Log(" |g{0}", characterName);
						character.Activate();
						InsertState(CharScreen);
						WaitFor(5, () => CharSel.Loading);
						return true;
					}
				}
				Log.Log("|rUnable to find character, check configuration");
				Clear();
				return true;
			}

			return false;
		}

		#endregion

		bool Monitor(object[] Params)
		{
			UpdateCurrentProfile();

			if (Config.Mode == "Period")
			{
				if (DateTime.Now.TimeOfDay > Config.PeriodEnd.TimeOfDay)
				{
					if (LogOut != null)
					{
						LogOut();
					}
					return true;
				}
			}

			if (Config.Mode == "Duration")
			{
				if (_curProfile != null)
				{
					if (DateTime.Now > Config.SessionStart[_curProfile.CharacterID].AddHours(Config.LogoutHours).AddMinutes(LogoutDelta) ||
					Session.Now.AddMinutes(Config.Downtime + DowntimeDelta) > Session.NextDowntime)
					{
						if (LogOut != null)
						{
							LogOut();
						}
						return true;
					}
				}
			}

			return false;
		}

		#region LoggingOut

		bool Logout(object[] Params)
		{
			LavishScript.ExecuteCommand("Exit");
			return true;
		}

		#endregion

		#endregion
	}


}

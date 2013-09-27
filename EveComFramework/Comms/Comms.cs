using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveComFramework.Core;
using EveCom;
using Meebey.SmartIrc4net;

namespace EveComFramework.Comms
{
    #region Settings

    /// <summary>
    /// Settings for the Comms class
    /// </summary>
    public class CommsSettings : Settings
    {
        public bool UseIRC = false;
        public string Server;
        public int Port = 6667;
        public string Prefix = "EveComUser";
        public string SendTo;
    }

    #endregion

    class Comms : State
    {
        #region Instantiation

        static Comms _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static Comms Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Comms();
                }
                return _Instance;
            }
        }

        private Comms() : base()
        {
            DefaultFrequency = 200;
            Random rand = new Random();
            Name = Config.Prefix + "-" + rand.Next(99999999);
            QueueState(Init);
            QueueState(Control);
        }

        #endregion

        #region Variables

        /// <summary>
        /// Config for this module
        /// </summary>
        public CommsSettings Config = new CommsSettings();
        string LastLocal = "";
        string Name;
        Queue<string> ChatQueue = new Queue<string>();
        public static IrcClient irc = new IrcClient();

        #endregion

        #region Actions

        public static void OnQueryMessage(object sender, IrcEventArgs e)
        {
            switch (e.Data.MessageArray[0])
            {
                // debug stuff
                case "gc":
                    GC.Collect();
                    break;
                // typical commands
                case "join":
                    irc.RfcJoin(e.Data.MessageArray[1]);
                    break;
                case "part":
                    irc.RfcPart(e.Data.MessageArray[1]);
                    break;
            }
        }

        public static void OnError(object sender, ErrorEventArgs e) 
        {
            EVEFrame.Log("Error: " + e.ErrorMessage);
        }

        public static void OnRawMessage(object sender, IrcEventArgs e)
        {
            EVEFrame.Log("Received: " + e.Data.RawMessage);
        }


        #endregion

        #region States

        bool Init(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            if (ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Any()) LastLocal = ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().Text;

            if (Config.UseIRC)
            {
                EVEFrame.Log("UseIRC");
                irc.Encoding = System.Text.Encoding.UTF8;
                irc.SendDelay = 200;
                irc.ActiveChannelSyncing = true;
                irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
                irc.OnError += new ErrorEventHandler(OnError);
                irc.OnRawMessage += new IrcEventHandler(OnRawMessage);

                try
                {
                    EVEFrame.Log("Connecting to: " + Config.Server + "  Port: " + Config.Port);
                    irc.Connect(Config.Server, Config.Port);
                }
                catch { EVEFrame.Log("Connect failed"); }
                try
                {
                    EVEFrame.Log("Logging in as nick: " + Name + "  RealName: " + Name);
                    irc.Login(Name, Name);
                }
                catch { EVEFrame.Log("Login failed"); }
                try
                {
                    EVEFrame.Log("Sending message to: " + Config.SendTo);
                    irc.SendMessage(SendType.Message, Config.SendTo, "Testing");
                }
                catch { EVEFrame.Log("Message failed"); }
            }

            return true;
        }

        bool Control(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            if (ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Any())
            {
                if (ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().Text != LastLocal)
                {
                    LastLocal = ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().Text;
                    ChatQueue.Enqueue("<Local> " + LastLocal);
                }
            }



            return false;
        }

        #endregion
    }
}

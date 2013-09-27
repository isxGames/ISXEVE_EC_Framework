using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveComFramework.Core;
using EveCom;
using IrcDotNet;

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

        IrcClient IRC = new IrcClient();

        #endregion

        #region Actions

        void ConnectEvent(EventArgs args)
        {

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

                IrcUserRegistrationInfo reginfo = new IrcUserRegistrationInfo();
                reginfo.NickName = Name;
                reginfo.RealName = Name;
                reginfo.UserName = Name;
                IRC.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
                IRC.Connect(new Uri("irc://" + Config.Server), reginfo);
                IRC.LocalUser.SendMessage(Config.SendTo, "Connected");
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

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
        public string Server = "irc1.lavishsoft.com";
        public int Port = 6667;
        public string SendTo;
        public bool Local = true;
        public bool NPC = false;
        public bool Wallet = true;
        public bool ChatInvite = true;
    }

    #endregion

    public class Comms : State
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
            QueueState(Init);
            QueueState(ConnectIRC);
            QueueState(Blank, 5000);
            QueueState(PostInit);
            QueueState(Control);
        }

        #endregion

        #region Variables

        /// <summary>
        /// Config for this module
        /// </summary>
        public CommsSettings Config = new CommsSettings();
        string LastLocal = "";
        double LastWallet;
        bool ChatInviteSeen = false;

        public Queue<string> ChatQueue = new Queue<string>();

        IrcClient IRC = new IrcClient();

        #endregion

        #region Actions


        #endregion

        #region Events

        public event Action ToggleStop;
        public event Action Start;
        public event Action Skip;

        void PMReceived(object sender, IrcMessageEventArgs e)
        {
            if (e.Source.Name == Config.SendTo)
            {
                if (e.Text.ToLower().StartsWith("togglestop") && ToggleStop != null)
                {
                    ToggleStop();
                }
                if (e.Text.ToLower().StartsWith("start") && Start != null)
                {
                    Start();
                }
                if (e.Text.ToLower().StartsWith("skip") && Skip != null)
                {
                    Skip();
                }
            }
        }

        #endregion

        #region States

        bool Init(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            try
            {
                if (ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Any()) LastLocal = ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().Text;
            }
            catch
            {
                LastLocal = "";
            }
            LastWallet = Wallet.ISK;

            return true;
        }

        bool ConnectIRC(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            if (Config.UseIRC)
            {
                try
                {
                    IrcUserRegistrationInfo reginfo = new IrcUserRegistrationInfo();
                    reginfo.NickName = Me.Name.Replace(" ", string.Empty);
                    reginfo.RealName = Me.Name.Replace(" ", string.Empty);
                    reginfo.UserName = Me.Name.Replace(" ", string.Empty);
                    IRC.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
                    IRC.Connect(new Uri("irc://" + Config.Server), reginfo);
                }
                catch
                {
                    EVEFrame.Log("Connect failed");
                }
            }
            return true;
        }
        bool Blank (object[] Params)
        {
            return true;
        }

        bool PostInit(object[] Params)
        {
            if (!IRC.IsRegistered)
            {
                DislodgeCurState(ConnectIRC);
                InsertState(Blank, 5000);
                return false;
            }
            IRC.LocalUser.MessageReceived += PMReceived;
            IRC.LocalUser.SendMessage(Config.SendTo, "Connected");
            return true;
        }

        bool Control(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            try
            {
                if (Config.Local && ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Any())
                {
                    if (ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().Text != LastLocal)
                    {
                        LastLocal = ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().Text;
                        if (ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().SenderName != "Message" || Config.NPC)
                        {
                            ChatQueue.Enqueue("<Local> " + ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString())).Messages.Last().SenderName + ": " + LastLocal);
                        }
                    }
                }
            }
            catch { }

            if (Config.ChatInvite)
            {
                Window ChatInvite = Window.All.FirstOrDefault(a => a.Name.Contains("ChatInvitation"));
                if (!ChatInviteSeen && ChatInvite != null)
                {
                    ChatQueue.Enqueue("<Comms> !!!!!!!!!!!!!!!!!!!!New Chat Invitation received!!!!!!!!!!!!!!!!!!!!");
                    ChatInviteSeen = true;
                }
                if (ChatInviteSeen && ChatInvite == null)
                {
                    ChatInviteSeen = false;
                }
            }

            if (Config.Wallet && LastWallet != Wallet.ISK)
            {
                double difference = Wallet.ISK - LastWallet;
                LastWallet = Wallet.ISK;
                ChatQueue.Enqueue("<Wallet> " + toISK(LastWallet) + " Delta: " + toISK(difference));
            }

            if (Config.UseIRC)
            {
                if (ChatQueue.Any())
                {
                    IRC.LocalUser.SendMessage(Config.SendTo, ChatQueue.Dequeue());
                }
            }

            return false;
        }

        #endregion


        string toISK(double val)
        {
            if (val > 1000000000) return string.Format("{0:C2}b isk", val / 1000000000);
            if (val > 1000000) return string.Format("{0:C2}m isk", val / 1000000);
            if (val > 1000) return string.Format("{0:C2}k isk", val / 1000);
            return string.Format("{0:C2} isk", val);
        }
    }

}

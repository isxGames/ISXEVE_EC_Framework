using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveComFramework.Core;
using EveCom;
using IrcDotNet;
using EveComFramework.KanedaToolkit;

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
        public bool AllChat = false;
        public bool Wallet = true;
        public bool ChatInvite = true;
        public bool Grid = false;
        public bool LocalTraffic = false;
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

        private Comms()
            : base()
        {
            DefaultFrequency = 200;
            QueueState(Init);
            QueueState(ConnectIRC);
            QueueState(Blank, 5000);
            QueueState(PostInit);
            QueueState(Control);
            NonFleetPlayers.AddNonFleetPlayers();
        }

        #endregion

        #region Variables

        /// <summary>
        /// Config for this module
        /// </summary>
        public CommsSettings Config = new CommsSettings();
        string LastLocal = "";
        Dictionary<string, string> chatMessages = new Dictionary<string, string>();
        double LastWallet;
        bool ChatInviteSeen = false;

        public Queue<string> ChatQueue = new Queue<string>();
        public Queue<string> LocalQueue = new Queue<string>();

        IrcClient IRC = new IrcClient();

        EveComFramework.Targets.Targets NonFleetPlayers = new EveComFramework.Targets.Targets();
        List<Entity> NonFleetMemberOnGrid = new List<Entity>();
        List<Pilot> PilotCache = new List<Pilot>();
        int SolarSystem = -1;

        public ChatChannel LocalChat
        {
            get
            {
                return ChatChannel.All.FirstOrDefault(a => a.ID.Contains(Session.SolarSystemID.ToString()));
            }
        }

        #endregion

        #region Actions


        #endregion

        #region Events

        public event Action ToggleStop;
        public event Action ToggleLogoff;
        public event Action Start;
        public event Action Panic;
        public event Action ClearPanic;
        public event Action Skip;

        void PMReceived(object sender, IrcMessageEventArgs e)
        {
            if (e.Source.Name == Config.SendTo)
            {
                if (e.Text.ToLower().StartsWith("?") || e.Text.ToLower().StartsWith("help"))
                {
                    ChatQueue.Enqueue("---------------Currently supported commands---------------");
                    ChatQueue.Enqueue("? or help - Display this menu!");
                    ChatQueue.Enqueue("Togglestop - Toggles on/off a stop at next opportunity");
                    ChatQueue.Enqueue("Togglelogoff - Toggles logoff on stop");
                    ChatQueue.Enqueue("Start - Starts the bot (ignored if bot is running)");
                    ChatQueue.Enqueue("Skip - Forces the bot to skip the current anomaly");
                    ChatQueue.Enqueue("Local <message> - Relays <message> to local (space must follow Local and don't put the <>!)");
                    ChatQueue.Enqueue("Listlocal or Locallist - Lists pilots currently in local chat");
                    ChatQueue.Enqueue("panic - Trigger panicked flee");
                    ChatQueue.Enqueue("clearpanic - Clear panic");
                    ChatQueue.Enqueue("All commands are not case sensitive!");
                }
                if (e.Text.ToLower().StartsWith("togglestop") && ToggleStop != null)
                {
                    ToggleStop();
                }
                if (e.Text.ToLower().StartsWith("start") && Start != null)
                {
                    Start();
                }
                if (e.Text.ToLower().StartsWith("togglelogoff") && ToggleLogoff != null)
                {
                    ToggleLogoff();
                }
                if (e.Text.ToLower().StartsWith("panic") && Panic != null)
                {
                    Panic();
                }
                if (e.Text.ToLower().StartsWith("clearpanic") && ClearPanic != null)
                {
                    ClearPanic();
                }
                if (e.Text.ToLower().StartsWith("skip") && Skip != null)
                {
                    Skip();
                }
                if (e.Text.ToLower().StartsWith("local "))
                {
                    LocalQueue.Enqueue(e.Text.Remove(0, 6));
                }
                if (e.Text.ToLower().StartsWith("listlocal") || e.Text.ToLower().StartsWith("locallist"))
                {
                    ChatQueue.Enqueue("---------------Local List---------------");
                    EVEFrameUtil.Do(() =>
                    {
                        foreach (Pilot p in Local.Pilots)
                        {
                            ChatQueue.Enqueue(p.Name + " - http://evewho.com/pilot/" + p.Name.Replace(" ", "%20"));
                        }
                    });
                    ChatQueue.Enqueue("----------------End List----------------");
                }
            }
        }

        void Error(object sender, IrcErrorEventArgs e)
        {
            EVEFrame.Log("Error: " + e.Error.Message);
        }


        #endregion

        #region States

        bool Init(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            try
            {
                if (LocalChat.Messages.Any()) LastLocal = LocalChat.Messages.Last().Text;
            }
            catch
            {
                LastLocal = "";
            }
            LastWallet = Wallet.ISK;

            IRC.Error += Error;

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
                    reginfo.NickName = Me.Name.Replace(" ", string.Empty).Replace("'", string.Empty);
                    reginfo.RealName = Me.Name.Replace(" ", string.Empty).Replace("'", string.Empty);
                    reginfo.UserName = Me.Name.Replace(" ", string.Empty).Replace("'", string.Empty);
                    IRC.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
                    IRC.Connect(new Uri("irc://" + Config.Server), reginfo);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
        bool Blank(object[] Params)
        {
            return true;
        }

        bool PostInit(object[] Params)
        {
            if (!IRC.IsConnected)
            {
                DislodgeCurState(ConnectIRC);
                InsertState(Blank, 5000);
                return false;
            }
            IRC.LocalUser.MessageReceived += PMReceived;
            IRC.LocalUser.SendMessage(Config.SendTo, "Connected - type ? or help for instructions");
            return true;
        }

        bool Control(object[] Params)
        {
            if (!Session.Safe || (!Session.InSpace && !Session.InStation)) return false;

            if (Session.SolarSystemID != SolarSystem)
            {
                PilotCache = Local.Pilots;
                SolarSystem = Session.SolarSystemID;
            }

            if (Config.LocalTraffic)
            {
                List<Pilot> newPilots = Local.Pilots.Where(a => !PilotCache.Contains(a)).ToList();
                foreach (Pilot pilot in newPilots)
                {
                    ChatQueue.Enqueue("<Local> New Pilot: " + pilot.Name);
                }
            }

            PilotCache = Local.Pilots;

            try
            {
                if (Config.Local)
                {
                    if (Config.AllChat)
                    {
                        foreach (ChatChannel channel in ChatChannel.All)
                        {
                            try
                            {
                                if (channel.Messages.Count > 0)
                                {
                                    if (!chatMessages.ContainsKey(channel.ID))
                                    {
                                        chatMessages.Add(channel.ID, string.Empty);
                                    }
                                    if (chatMessages.FirstOrDefault(a => a.Key == channel.ID).Value != channel.Messages.Last().Text)
                                    {
                                        chatMessages.AddOrUpdate(channel.ID, channel.Messages.Last().Text);
                                        if (!(channel.Messages.Last().SenderName == "Message" || channel.Messages.Last().SenderName == "EVE System") || Config.NPC)
                                        {
                                            ChatQueue.Enqueue("[Chat] <" + channel.Name + "> " + channel.Messages.Last().SenderName + ": " + channel.Messages.Last().Text);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                EVEFrame.Log(e);
                            }
                        }
                    }
                    else
                    {
                        if (LocalChat.Messages.Any())
                        {
                            if (LocalChat.Messages.Last().Text != LastLocal)
                            {
                                LastLocal = LocalChat.Messages.Last().Text;
                                if (LocalChat.Messages.Last().SenderName != "Message" || Config.NPC)
                                {
                                    ChatQueue.Enqueue("[Chat] <Local> " + LocalChat.Messages.Last().SenderName + ": " + LastLocal);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EVEFrame.Log(e);
            }

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

            if (Session.InSpace && Config.Grid)
            {
                Entity AddNonFleet = NonFleetPlayers.TargetList.FirstOrDefault(a => !NonFleetMemberOnGrid.Contains(a));
                if (AddNonFleet != null)
                {
                    ChatQueue.Enqueue("<Security> Non fleet member on grid: " + AddNonFleet.Name + " - http://evewho.com/pilot/" + AddNonFleet.Name.Replace(" ", "%20"));
                    NonFleetMemberOnGrid.Add(AddNonFleet);
                }
                NonFleetMemberOnGrid = NonFleetPlayers.TargetList.Where(a => NonFleetMemberOnGrid.Contains(a)).ToList();
            }

            if (Config.UseIRC)
            {
                if (ChatQueue.Any())
                {
                    IRC.LocalUser.SendMessage(Config.SendTo, ChatQueue.Dequeue());
                }
            }

            if (LocalQueue.Count > 0)
            {
                LocalChat.Send(LocalQueue.Dequeue());
            }
            return false;
        }

        #endregion


        string toISK(double val)
        {
            if (val > 1000000000) return string.Format("{0:0.00}b isk", val / 1000000000);
            if (val > 1000000) return string.Format("{0:0.00}m isk", val / 1000000);
            if (val > 1000) return string.Format("{0:0.00}k isk", val / 1000);
            return string.Format("{0:0.00} isk", val);
        }
    }

}

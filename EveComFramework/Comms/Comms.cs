using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveComFramework.Core;
using EveCom;
using Sharkbite.Irc;

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

        private Connection connection;

        #endregion

        #region Actions

		public void OnRegistered() 
		{
			//We have to catch errors in our delegates because Thresher purposefully
			//does not handle them for us. Exceptions will cause the library to exit if they are not
			//caught.
			try
			{ 

				//The connection is ready so lets join a channel.
				//We can join any number of channels simultaneously but
				//one will do for now.
				//All commands are sent to IRC using the Sender object
				//from the Connection.
                EVEFrame.Log("Joining test channel");
				connection.Sender.Join("#test459");
			}
			catch( Exception e ) 
			{
				Console.WriteLine("Error in OnRegistered(): " + e ) ;
			}
		}

		public void OnPublic( UserInfo user, string channel, string message )
		{
			//Echo back any public messages
            EVEFrame.Log("OnPublic");
			connection.Sender.PublicMessage( channel,  user.Nick + " said, " + message );
		}

		public void OnPrivate( UserInfo user,  string message )
		{
			//Quit IRC if someone sends us a 'die' message
			if( message == "die" ) 
			{
				connection.Disconnect("Bye");
			}
		}

		public void OnError( ReplyCode code, string message) 
		{
			//All anticipated errors have a numeric code. The custom Thresher ones start at 1000 and
			//can be found in the ErrorCodes class. All the others are determined by the IRC spec
			//and can be found in RFC2812Codes.
			EVEFrame.Log("An error of type " + code + " due to " + message + " has occurred.");
		}

		public void OnDisconnected() 
		{
			//If this disconnection was involutary then you should have received an error
			//message ( from OnError() ) before this was called.
			EVEFrame.Log("Connection to the server has been closed.");
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
                ConnectionArgs cargs = new ConnectionArgs(Name, Config.Server);

                connection = new Connection(cargs, false, false);

			    //Listen for any messages sent to the channel
			    connection.Listener.OnPublic += new PublicMessageEventHandler( OnPublic );

			    //Listen for bot commands sent as private messages
			    connection.Listener.OnPrivate += new PrivateMessageEventHandler( OnPrivate );
	
			    //Listen for notification that an error has ocurred 
			    connection.Listener.OnError += new ErrorMessageEventHandler( OnError );

			    //Listen for notification that we are no longer connected.
			    connection.Listener.OnDisconnected += new DisconnectedEventHandler( OnDisconnected );

                try
                {
                    connection.Connect();
                    EVEFrame.Log("IRC Connected");
                }
			    catch( Exception e ) 
			    {
				    EVEFrame.Log("Error during connection process.");
				    EVEFrame.Log( e );
			    }

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

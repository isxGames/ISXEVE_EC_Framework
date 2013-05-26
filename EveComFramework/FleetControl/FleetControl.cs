using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;
using _Fleet = EveCom.Fleet;


namespace EveComFramework.FleetControl
{
    #region FleetMember class

    [Serializable]
    public class FleetMember
    {
        public long ID { get; set; }
        public bool FleetBooster { get; set; }
        public bool WingBooster { get; set; }
        public bool SquadBooster { get; set; }
    }

    [Serializable]
    public class Fleet
    {
        public string Boss { get; set; }
        public long Commander { get; set; }
        public List<Wing> Wings { get; set; }
    }

    [Serializable]
    public class Wing
    {
        public long Commander { get; set; }
        public List<Squad> Squads { get; set; }
    }

    [Serializable]
    public class Squad
    {
        public long Commander { get; set; }
        public List<FleetMember> Members { get; set; }
    }

    #endregion

    #region Settings

    public class Settings : EveComFramework.Core.Settings
    {

        public SerializableDictionary<string, Fleet> Fleets = new SerializableDictionary<string, Fleet>();
        public string CurrentFleet = "";
    }

    #endregion

    public class FleetControl: EveComFramework.Core.State
    {
        #region Instantiation

        static FleetControl _Instance;
        public static FleetControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new FleetControl();
                }
                return _Instance;
            }
        }

        private FleetControl() : base()
        {
            QueueState(Control);
        }

        #endregion

        #region Variables

        public Settings Config = new Settings();

        #endregion

        #region Actions

        public void Start()
        {
            if (Idle)
            {
                QueueState(Control);
            }

        }

        public void Stop()
        {
            Clear();
        }

        public void Configure()
        {
            UI.FleetControl Configuration = new UI.FleetControl();
            Configuration.Show();
        }

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe || Config.CurrentFleet == "")
            {
                return false;
            }


            if (!Session.InFleet)
            {
                if (Window.All.OfType<PopupWindow>().Any(a => a.Message.Contains(Config.Fleets[Config.CurrentFleet].Boss)))
                {
                    Window.All.OfType<PopupWindow>().FirstOrDefault(a => a.Message.Contains(Config.Fleets[Config.CurrentFleet].Boss)).ClickButton(Window.Button.Yes);
                    return false;
                }

                if (Config.Fleets[Config.CurrentFleet].Boss == Me.Name)
                {
                    _Fleet.CreateFleet();
                    return false;
                }
                return false;
            }

            if (Config.Fleets[Config.CurrentFleet].Boss != Me.Name) return false;

            if (Config.Fleets[Config.CurrentFleet].Commander != _Fleet.Commander.ID && Config.Fleets[Config.CurrentFleet].Commander != -1)
            {
                if (Config.Fleets[Config.CurrentFleet].Commander == Me.CharID)
                {
                    _Fleet.Members.FirstOrDefault(a => a.ID == Me.CharID).Move(role: FleetRole.FleetCommander);
                    return false;
                }
                else if (_Fleet.Members.Any(a => a.ID == Config.Fleets[Config.CurrentFleet].Commander))
                {
                    _Fleet.Members.FirstOrDefault(a => a.ID == Config.Fleets[Config.CurrentFleet].Commander).Move(role: FleetRole.FleetCommander);
                    return false;
                }
                else if (Local.Pilots.Any(a => a.ID == Config.Fleets[Config.CurrentFleet].Commander))
                {
                    _Fleet.Invite(Local.Pilots.FirstOrDefault(a => a.ID == Config.Fleets[Config.CurrentFleet].Commander), role: FleetRole.FleetCommander);
                    return false;
                }
                
            }


            return false;
        }

        #endregion

    }
}

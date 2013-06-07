using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework.Core
{
    public class Cache : State
    {
        #region Instantiation

        static Cache _Instance;
        public static Cache Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Cache();
                }
                return _Instance;
            }
        }

        private Cache() : base()
        {
            QueueState(Control);
        }

        #endregion

        #region Variables

        public string Name { get; set; }
        public string[] Bookmarks { get; set; }
        public string[] FleetMembers { get; set; }
        public string[] CargoItems { get; set; }

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            Name = Me.Name;
            Bookmarks = Bookmark.All.Select(a => a.Title).ToArray();
            if (Session.InFleet) FleetMembers = Fleet.Members.Select(a => a.Name).ToArray();
            if (MyShip.CargoBay != null && MyShip.CargoBay.IsPrimed) CargoItems = MyShip.CargoBay.Items.Distinct().Select(a => a.Type).ToArray();
            return false;
        }

        #endregion
    }
}

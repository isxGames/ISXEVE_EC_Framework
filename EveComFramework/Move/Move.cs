using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework.Move
{
    class Location
    {
        internal enum LocationType
        {
            SolarSystem,
            Station,
            POSStructure
        }
        internal LocationType Type { get; set; }
        internal Bookmark Bookmark { get; set; }
        internal int StationID { get; set; }
        internal int SolarSystem { get; set; }
        internal string ContainerName { get; set; }

        internal Location(LocationType Type, Bookmark Bookmark = null, int Station = 0, int SolarSystem = 0, string ContainerName = null)
        {
            this.Type = Type;
            this.Bookmark = Bookmark;
            this.StationID = Station;
            this.SolarSystem = SolarSystem;
            this.ContainerName = ContainerName;
        }

        public Location Clone()
        {
            return new Location(Type, Bookmark, StationID, SolarSystem, ContainerName);
        }

    }

    public class Move : EveComFramework.Core.State
    {

        #region Instantiation
        static Move _Instance;
        public static Move Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Move();
                }
                return _Instance;
            }
        }

        private Move() : base()
        {

        }

        #endregion

        #region Actions

        public void ToggleAutopilot(bool Activate = true)
        {
            Clear();
            if (Activate)
            {
                using (new EVEFrameLock())
                {
                    if (Route.Path == null || Route.Path[0] == -1)
                    {
                        return;
                    }
                    if (Session.InStation)
                    {
                        QueueState(Undock);
                    }
                    QueueState(AutoPilot);
                }
            }
        }

        public void Bookmark(Bookmark Bookmark, int Distance = 0)
        {
            Clear();
            using (new EVEFrameLock())
            {
                if (Bookmark == null)
                {
                    //LSUI.Update("Move", "Invalid bookmark specified", "r");
                    return;
                }
                if (Session.InStation)
                {
                    if (Session.StationID == Bookmark.ItemID)
                    {
                        return;
                    }
                    else
                    {
                        QueueState(Undock);
                    }
                }
                if (Bookmark.LocationID != Session.SolarSystemID)
                {
                    //LSUI.Update("Move", "Setting course", "o");
                    //LSUI.Update("Move", " " + Bookmark.Title, "-g");
                    Bookmark.SetDestination();
                    QueueState(AutoPilot, 2000);
                }
                if (Bookmark.GroupID == Group.Station && Bookmark.LocationID == Session.SolarSystemID)
                {
                    QueueState(Dock, -1, Entity.All.FirstOrDefault(a => a.ID == Bookmark.ItemID));
                }
                else
                {
                    QueueState(BookmarkWarp, -1, Bookmark, Distance);
                }
            }
        }

        public void Object(Entity Entity, int Distance = 0)
        {
            Clear();
            using (new EVEFrameLock())
            {
                if (Entity == null)
                {
                    //LSUI.Update("Move", "Invalid entity specified", "r");
                    return;
                }
                if (Session.InStation)
                {
                    if (Session.StationID == Entity.ID)
                    {
                        return;
                    }
                    else
                    {
                        QueueState(Undock);
                    }
                }
                if (Entity.GroupID == Group.Station)
                {
                    QueueState(Dock, -1, Entity);
                }
                else
                {
                    QueueState(ObjectWarp, -1, Entity, Distance);
                }
            }

        }

        public void Jump()
        {
            if (Idle)
            {
                QueueState(JumpThroughArray);
            }
        }

        public void Approach(Entity Target, int Distance = 0)
        {
            // If we're not doing anything, just start ApproachState
            if (Idle)
            {
                QueueState(ApproachState, -1, Target, Distance);
            }
            // If we're approaching something else or orbiting something, change to approaching the new target - retain collision information!
            if ((CurState.State == ApproachState && (Entity)CurState.Params[0] != Target) || CurState.State == OrbitState)
            {
                Entity Collision = ((Entity)CurState.Params[3]);

                Clear();
                QueueState(ApproachState, -1, Target, Distance, true, Collision);
            }
        }

        public void Orbit(Entity Target, int Distance = 1000)
        {
            // If we're not doing anything, just start OrbitState
            if (Idle)
            {
                QueueState(OrbitState, -1, Target, Distance);
            }
            // If we're orbiting something else or approaching something, change to orbiting the new target - retain collision information!
            if ((CurState.State == OrbitState && (Entity)CurState.Params[0] != Target) || CurState.State == ApproachState)
            {
                Entity Collision = ((Entity)CurState.Params[3]);

                Clear();
                QueueState(OrbitState, -1, Target, Distance, true, Collision);
            }
        }


        #endregion

        #region States

        bool BookmarkWarp(object[] Params)
        {
            Bookmark Destination = (EveCom.Bookmark)Params[0];
            int Distance = 0;
            if (Params.Count() > 1) { Distance = (int)Params[1]; }

            if (Session.InStation)
            {
                if (Destination.ItemID != Session.StationID)
                {
                    InsertState(BookmarkWarp, -1, Destination, Distance);
                    InsertState(Undock);
                }
                return true;
            }

            if (!Session.InSpace)
            {
                return false;
            }

            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                return false;
            }

            if (Destination.Distance < 150000 && Destination.Distance > 0)
            {
                //LSUI.Update("Debug", "Destination is " + Destination.Distance.ToString() + " m away.");
                return true;
            }

            if (Destination.Exists && Destination.CanWarpTo)
            {
                //LSUI.Update("Move", "Warping", "o");
                //LSUI.Update("Move", " " + Destination.Title + " (" + Distance + " km)", "-g");
                Destination.WarpTo(Distance);
            }

            return true;
        }

        bool ObjectWarp(object[] Params)
        {
            int Distance = 0;
            if (Params.Count() > 1) { Distance = (int)Params[1]; }
            if (((EveCom.Entity)Params[0]).Exists && ((EveCom.Entity)Params[0]).Distance > 150000)
            {
                //LSUI.Update("Move", "Warping", "o");
                //LSUI.Update("Move", " " + ((EveCom.Entity)Params[0]).Name + " (" + Distance + " km)", "-g");
                ((EveCom.Entity)Params[0]).WarpTo(Distance);
            }
            return true;
        }

        bool Undock(object[] Params)
        {
            if (Session.InSpace)
            {
                //LSUI.Update("Move", "Undock complete", "o");
                return true;
            }

            //LSUI.Update("Move", "Undocking", "o");
            //LSUI.Update("Move", " " + Session.StationName, "-g");
            Command.CmdExitStation.Execute();
            InsertState(Undock);
            WaitFor(20, () => Session.InSpace);
            return true;
        }

        bool JumpThroughArray(object[] Params)
        {
            Entity JumpPortalArray = Entity.All.Where(a => a.GroupID == Group.JumpPortalArray).FirstOrDefault();
            if (JumpPortalArray == null)
            {
                //LSUI.Update("Move", "No Jump Portal Array on grid", "r");
                return true;
            }
            if (JumpPortalArray.Distance > 2500)
            {
                InsertState(JumpThroughArray);
                InsertState(ApproachState, -1, JumpPortalArray, 2500);
                return true;
            }
            //LSUI.Update("Move", "Jumping through", "o");
            //LSUI.Update("Move", " " + JumpPortalArray.Name, "-g");
            JumpPortalArray.JumpThroughPortal();
            InsertState(JumpThroughArray);
            int CurSystem = Session.SolarSystemID;
            WaitFor(10, () => Session.SolarSystemID != CurSystem, () => MyShip.ToEntity.Mode == EntityMode.Approaching);
            return true;
        }

        bool ApproachState(object[] Params)
        {
            Entity Target = ((Entity)Params[0]);
            int Distance = (int)Params[1];
            bool Approaching = false;
            if (Params.Count() > 2) { Approaching = (bool)Params[2]; }
            Entity Collision = null;
            if (Params.Count() > 3) { Collision = ((Entity)Params[3]); }


            if (Target == null || !Target.Exists)
            {
                return true;
            }

            if (Target.Distance > Distance)
            {
                // Start approaching our approach target if we're not currently approaching anything
                if (!Approaching || MyShip.ToEntity.Mode == EntityMode.Stopped)
                {
                    //LSUI.Update("Move", "Approaching", "o");
                    //LSUI.Update("Move", " " + Target.Name + " (" + Distance / 1000 + " km)", "-g");
                    Target.Approach();
                    InsertState(ApproachState, -1, Target, Distance, true);
                    WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Approaching);
                }
                // Else, if we're in 5km of a structure and aren't already orbiting a structure, orbit it and set it as our collision target
                else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000) != null
                        && Collision == null)
                {
                    Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000);
                    //LSUI.Update("Move", "Orbiting", "o");
                    //LSUI.Update("Move", " " + Collision.Name + " (10 km)", "-g");
                    Collision.Orbit(10000);
                    InsertState(ApproachState, -1, Target, Distance, true, Collision);
                }
                // Else, if we're in 2km of a structure that isn't our current collision target, change orbit and collision target to it
                else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000) != null
                        && Collision != Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000))
                {
                    Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000);
                    //LSUI.Update("Move", "Orbiting", "o");
                    //LSUI.Update("Move", " " + Collision.Name + "(10 km)", "-g");
                    Collision.Orbit(10000);
                    InsertState(ApproachState, -1, Target, Distance, true, Collision);
                }
                // Else, if we're not within 8km of a structure and we have a collision target (orbiting a structure) change approach back to our approach target
                else if (Entity.All.Where(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 8000).FirstOrDefault() == null
                        && Collision != null)
                {
                    //LSUI.Update("Move", "Approaching", "o");
                    //LSUI.Update("Move", " " + Target.Name + " (" + Distance / 1000 + " km)", "-g");
                    Target.Approach();
                    InsertState(ApproachState, -1, Target, Distance, true);
                }
                else
                {
                    InsertState(ApproachState, -1, Target, Distance, Approaching, Collision);
                }

            }


            return true;
        }

        bool OrbitState(object[] Params)
        {
            Entity Target = ((Entity)Params[0]);
            int Distance = (int)Params[1];
            bool Orbiting = false;
            if (Params.Count() > 2) { Orbiting = (bool)Params[2]; }
            Entity Collision = null;
            if (Params.Count() > 3) { Collision = (Entity)Params[3]; }

            if (Target == null || !Target.Exists)
            {
                return true;
            }

            // Start orbiting our orbit target if we're not currently orbiting anything
            if (!Orbiting || MyShip.ToEntity.Mode == EntityMode.Stopped)
            {
                //LSUI.Update("Move", "Orbiting", "o");
                //LSUI.Update("Move", " " + Target.Name + " (" + Distance / 1000 + " km)", "-g");
                Target.Orbit(Distance);
                InsertState(OrbitState, -1, Target, Distance, true);
                WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Orbiting);
            }
            // Else, if we're in 5km of a structure and aren't already orbiting a structure, orbit it and set it as our collision target
            else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000) != null
                    && Collision == null)
            {
                Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000);
                //LSUI.Update("Move", "Orbiting", "o");
                //LSUI.Update("Move", " " + Collision.Name + "(10 km)", "-g");
                Collision.Orbit(10000);
                InsertState(OrbitState, -1, Target, Distance, true, Collision);
            }
            // Else, if we're in 2km of a structure that isn't our current collision target, change orbit and collision target to it
            else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000) != null
                    && Collision != Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000))
            {
                Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000);
                //LSUI.Update("Move", "Orbiting", "o");
                //LSUI.Update("Move", " " + Collision.Name + "(10 km)", "-g");
                Collision.Orbit(10000);
                InsertState(OrbitState, -1, Target, Distance, true, Collision);
            }
            // Else, if we're not within 8km of a structure and we have a collision target (orbiting a structure) change orbit back to our orbit target
            else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 8000) == null
                    && Collision != null)
            {
                //LSUI.Update("Move", "Orbiting", "o");
                //LSUI.Update("Move", " " + Target.Name + " (" + Distance / 1000 + " km)", "-g");
                Target.Orbit(Distance);
                InsertState(OrbitState, -1, Target, Distance, true);
            }
            else
            {
                InsertState(OrbitState, -1, Target, Distance, Orbiting, Collision);
            }
            return true;

        }

        bool AutoPilot(object[] Params)
        {
            if (Route.Path == null || Route.Path[0] == -1)
            {
                //LSUI.Update("Move", "Route empty, autopilot deactivated", "o");
                return true;
            }
            if (Session.InSpace)
            {
                if (Route.NextWaypoint.GroupID == Group.Stargate)
                {
                    //LSUI.Update("Move", "Jumping through", "o");
                    //LSUI.Update("Move", " " + Route.NextWaypoint.Name, "-g");
                    Route.NextWaypoint.Jump();
                    int CurSystem = Session.SolarSystemID;
                    InsertState(AutoPilot);
                    WaitFor(10, () => Session.SolarSystemID != CurSystem, () => MyShip.ToEntity.Mode != EntityMode.Stopped);
                    return true;
                }
                if (Route.NextWaypoint.GroupID == Group.Station)
                {
                    InsertState(Dock, 500, Route.NextWaypoint);
                    return true;
                }
            }
            return false;
        }


        bool Dock(object[] Params)
        {
            if (Params.Length == 0)
            {
                //LSUI.Update("Move", "Dock call incomplete", "r");
                return true;
            }
            if (Session.InStation)
            {
                //LSUI.Update("Move", "Dock complete", "o");
                return true;
            }

            //LSUI.Update("Move", "Docking", "o");
            //LSUI.Update("Move", " " + ((Entity)Params[0]).Name, "-g");
            ((Entity)Params[0]).Dock();
            InsertState(Dock, -1, Params[0]);
            WaitFor(10, () => Session.InStation, () => MyShip.ToEntity.Mode != EntityMode.Stopped);

            return true;
        }

        #endregion

    }

}

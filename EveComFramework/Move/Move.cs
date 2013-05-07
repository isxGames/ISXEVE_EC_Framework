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

        #region Variables

        public Core.Logger Log = new Core.Logger();

        #endregion

        #region Actions

        public void ToggleAutopilot(bool Activate = true)
        {
            Clear();
            if (Activate)
            {
                QueueState(AutoPilotPrep);
            }
        }

        public void Bookmark(Bookmark Bookmark, int Distance = 0)
        {
            Clear();
            QueueState(BookmarkPrep, -1, Bookmark, Distance);
        }

        public void Object(Entity Entity, int Distance = 0)
        {
            Clear();
            QueueState(ObjectPrep, -1, Entity, Distance);
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
                QueueState(ApproachState, -1, Target, Distance, false);
            }
            // If we're approaching something else or orbiting something, change to approaching the new target - retain collision information!
            if ((CurState.State == ApproachState && (Entity)CurState.Params[0] != Target) || CurState.State == OrbitState)
            {
                if (CurState.Params.Count() > 3)
                {
                    Clear();
                    Entity Collision = ((Entity)CurState.Params[3]);
                    QueueState(ApproachState, -1, Target, Distance, false, Collision);
                }
                else
                {
                    Clear();
                    Entity Collision = ((Entity)CurState.Params[3]);
                    QueueState(ApproachState, -1, Target, Distance, false);
                }
            }
        }

        public void Orbit(Entity Target, int Distance = 1000)
        {
            // If we're not doing anything, just start OrbitState
            if (Idle)
            {
                QueueState(OrbitState, -1, Target, Distance, false);
            }
            // If we're orbiting something else or approaching something, change to orbiting the new target - retain collision information!
            if ((CurState.State == OrbitState && (Entity)CurState.Params[0] != Target) || CurState.State == ApproachState)
            {
                if (CurState.Params.Count() > 3)
                {
                    Clear();
                    Entity Collision = ((Entity)CurState.Params[3]);
                    QueueState(OrbitState, -1, Target, Distance, false, Collision);
                }
                else
                {
                    Clear();
                    QueueState(OrbitState, -1, Target, Distance, false);
                }

            }
        }


        #endregion

        #region States

        bool BookmarkPrep(object[] Params)
        {
            Bookmark Bookmark = (Bookmark)Params[0];
            int Distance = (int)Params[1];

            if (Session.InStation)
            {
                if (Session.StationID == Bookmark.ItemID)
                {
                    return true;
                }
                else
                {
                    QueueState(Undock);
                }
            }
            if (Bookmark.LocationID != Session.SolarSystemID)
            {
                Log.Log("Setting course: {0}", Bookmark.Title);
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
            return true;
        }

        bool BookmarkWarp(object[] Params)
        {
            Bookmark Destination = (Bookmark)Params[0];
            int Distance = (int)Params[1];

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
                return true;
            }
            if (Destination.Exists && Destination.CanWarpTo)
            {
                Log.Log("Warping: {0} ({1} km)", Destination.Title, Distance);
                Destination.WarpTo(Distance);
            }
            return true;
        }

        bool ObjectPrep(object[] Params)
        {
            Entity Entity = (Entity)Params[0];
            int Distance = (int)Params[1];

            if (Session.InStation)
            {
                if (Session.StationID == Entity.ID)
                {
                    return true;
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
            return true;
        }

        bool ObjectWarp(object[] Params)
        {
            Entity Entity = (Entity)Params[0];
            int Distance = (int)Params[1];
            if (Entity.Exists && Entity.Distance > 150000)
            {
                Log.Log("Warping: {0} ({1} km)", Entity.Name, Distance);
                Entity.WarpTo(Distance);
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
            bool Approaching = (bool)Params[2];
            Entity Collision = null;
            if (Params.Count() > 3) { Collision = (Entity)Params[3]; }


            if (Target == null || !Target.Exists)
            {
                return true;
            }

            if (Target.Distance > Distance)
            {
                // Start approaching our approach target if we're not currently approaching anything
                if (!Approaching || (MyShip.ToEntity.Mode != EntityMode.Orbiting && MyShip.ToEntity.Mode != EntityMode.Approaching))
                {
                    Log.Log("Approaching {0}({1} km)", Target.Name, Distance / 1000);
                    Target.Approach();
                    InsertState(ApproachState, -1, Target, Distance, true);
                    WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Approaching);
                }
                // Else, if we're in 5km of a structure and aren't already orbiting a structure, orbit it and set it as our collision target
                else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000) != null
                        && Collision == null)
                {
                    Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000);
                    Log.Log("Orbiting {0}(10 km)", Collision.Name);
                    Collision.Orbit(10000);
                    InsertState(ApproachState, -1, Target, Distance, true, Collision);
                }
                // Else, if we're in 2km of a structure that isn't our current collision target, change orbit and collision target to it
                else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000) != null
                        && Collision != Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000))
                {
                    Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000);
                    Log.Log("Orbiting {0}(10 km)", Collision.Name);
                    Collision.Orbit(10000);
                    InsertState(ApproachState, -1, Target, Distance, true, Collision);
                }
                // Else, if we're not within 8km of a structure and we have a collision target (orbiting a structure) change approach back to our approach target
                else if (Entity.All.Where(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 8000).FirstOrDefault() == null
                        && Collision != null)
                {
                    Log.Log("Approaching {0}({1} km)", Target.Name, Distance / 1000);
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
            bool Orbiting = (bool)Params[2]; 
            Entity Collision = null;
            if (Params.Count() > 3) { Collision = (Entity)Params[3]; }

            if (Target == null || !Target.Exists)
            {
                return true;
            }

            // Start orbiting our orbit target if we're not currently orbiting anything
            if (!Orbiting || MyShip.ToEntity.Mode != EntityMode.Orbiting)
            {
                Log.Log("Orbiting {0}({1} km)", Target.Name, Distance / 1000);
                Target.Orbit(Distance);
                InsertState(OrbitState, -1, Target, Distance, true);
                WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Orbiting);
            }
            // Else, if we're in 5km of a structure and aren't already orbiting a structure, orbit it and set it as our collision target
            else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000) != null
                    && Collision == null)
            {
                Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 5000);
                Log.Log("Orbiting {0}(10 km)", Collision.Name);
                Collision.Orbit(10000);
                InsertState(OrbitState, -1, Target, Distance, true, Collision);
            }
            // Else, if we're in 2km of a structure that isn't our current collision target, change orbit and collision target to it
            else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000) != null
                    && Collision != Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000))
            {
                Collision = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 2000);
                Log.Log("Orbiting {0}(10 km)", Collision.Name);
                Collision.Orbit(10000);
                InsertState(OrbitState, -1, Target, Distance, true, Collision);
            }
            // Else, if we're not within 8km of a structure and we have a collision target (orbiting a structure) change orbit back to our orbit target
            else if (Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Distance <= 8000) == null
                    && Collision != null)
            {
                Log.Log("Orbiting {0}({1} km)", Target.Name, Distance / 1000);
                Target.Orbit(Distance);
                InsertState(OrbitState, -1, Target, Distance, true);
            }
            else
            {
                InsertState(OrbitState, -1, Target, Distance, Orbiting, Collision);
            }
            return true;

        }

        bool AutoPilotPrep(object[] Params)
        {
            if (Route.Path == null || Route.Path[0] == -1)
            {
                return true;
            }
            if (Session.InStation)
            {
                QueueState(Undock);
            }
            QueueState(AutoPilot);
            return true;
        }

        bool AutoPilot(object[] Params)
        {
            if (Route.Path == null || Route.Path[0] == -1)
            {
                Log.Log("Autopilot deactivated");
                return true;
            }
            if (Session.InSpace)
            {
                if (Route.NextWaypoint.GroupID == Group.Stargate)
                {
                    Log.Log("Jumping through to {0}", Route.NextWaypoint.Name);
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
                Log.Log("Dock call incomplete");
                return true;
            }
            if (Session.InStation)
            {
                Log.Log("Dock complete");
                return true;
            }

            Log.Log("Docking at {0}", ((Entity)Params[0]).Name);
            ((Entity)Params[0]).Dock();
            InsertState(Dock, -1, Params[0]);
            WaitFor(10, () => Session.InStation, () => MyShip.ToEntity.Mode != EntityMode.Stopped);

            return true;
        }

        #endregion

    }

}

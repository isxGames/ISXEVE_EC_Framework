#pragma warning disable 1591
using System.Collections.Generic;
using System.Linq;
using EveCom;
using EveComFramework.Core;
using EveComFramework.KanedaToolkit;

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

        internal Location(LocationType Type, Bookmark Bookmark = null, int StationID = 0, int SolarSystem = 0, string ContainerName = null)
        {
            this.Type = Type;
            this.Bookmark = Bookmark;
            this.StationID = StationID;
            this.SolarSystem = SolarSystem;
            this.ContainerName = ContainerName;
        }

        public Location Clone()
        {
            return new Location(Type, Bookmark, StationID, SolarSystem, ContainerName);
        }

    }

    /// <summary>
    /// Settings for the Move class
    /// </summary>
    public class MoveSettings : Settings
    {
        public bool WarpCollisionPrevention = true;
        public decimal WarpCollisionTrigger = 1;
        public decimal WarpCollisionOrbit = 5;
        public bool ApproachCollisionPrevention = true;
        public decimal ApproachCollisionTrigger = .5m;
        public decimal ApproachCollisionOrbit = .6m;
        public bool OrbitCollisionPrevention = true;
        public decimal OrbitCollisionTrigger = 5;
        public decimal OrbitCollisionOrbit = 10;
        public bool InstaWarp = false;
    }

    /// <summary>
    /// This class handles navigation
    /// </summary>
    public class Move : State
    {

        #region Instantiation
        static Move _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
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

        private Move()
        {

        }

        #endregion

        #region Variables

        /// <summary>
        /// The logger for this class
        /// </summary>
        public Logger Log = new Logger("Move");
        public MoveSettings Config = new MoveSettings();
        InstaWarp InstaWarpModule = InstaWarp.Instance;
        #endregion

        #region Actions

        /// <summary>
        /// Toggle on/off the autopilot
        /// </summary>
        /// <param name="Activate">Enable = true</param>
        public void ToggleAutopilot(bool Activate = true)
        {
            Clear();
            if (Activate)
            {
                QueueState(AutoPilotPrep);
            }
        }

        /// <summary>
        /// Warp to a bookmark
        /// </summary>
        /// <param name="Bookmark">The bookmark to warp to</param>
        /// <param name="Distance">The distance to warp at.  Default: 0</param>
        public void Bookmark(Bookmark Bookmark, int Distance = 0)
        {
            Clear();
            QueueState(BookmarkPrep, -1, Bookmark, Distance);
        }

        /// <summary>
        /// Warp to an entity
        /// </summary>
        /// <param name="Entity">The entity to which to warp</param>
        /// <param name="Distance">The distance to warp at.  Default: 0</param>
        public void Object(Entity Entity, int Distance = 0)
        {
            Clear();
            QueueState(ObjectPrep, -1, Entity, Distance);
        }

        /// <summary>
        /// Activate an entity (ex: Jump gate)
        /// </summary>
        /// <param name="Entity"></param>
        public void Activate(Entity Entity)
        {
            Clear();
            QueueState(ActivateEntity, -1, Entity);
        }

        /// <summary>
        /// Jump through an entity (ex: Jump portal array)
        /// </summary>
        public void Jump()
        {
            if (Idle)
            {
                QueueState(JumpThroughArray);
            }
        }

        int LastOrbitDistance;
        /// <summary>
        /// Orbit an entity
        /// </summary>
        /// <param name="Target">The entity to orbit</param>
        /// <param name="Distance">The distance from the entity to orbit</param>
        public void Orbit(Entity Target, int Distance = 1000)
        {
            // If we're not doing anything, just start OrbitState
            if (Idle)
            {
                LastOrbitDistance = Distance;
                QueueState(OrbitState, -1, Target, Distance, false);
                return;
            }
            // If we're orbiting something else or approaching something, change to orbiting the new target - retain collision information!
            if ((CurState.State == OrbitState && (Entity)CurState.Params[0] != Target) || CurState.State == ApproachState)
            {
                Clear();
                LastOrbitDistance = Distance;
                QueueState(OrbitState, -1, Target, Distance, false);
            }

            if (Distance != LastOrbitDistance && LastOrbitDistance != 0 && Distance != 0)
            {
                Clear();
                LastOrbitDistance = Distance;
                QueueState(OrbitState, -1, Target, Distance, false);
            }
        }


        #endregion

        #region States

        bool BookmarkPrep(object[] Params)
        {
            Bookmark Bookmark = (Bookmark)Params[0];
            int Distance = (int)Params[1];

            if (Bookmark == null) return true;

            if (Session.InStation)
            {
                if (Session.StationID == Bookmark.ItemID)
                {
                    return true;
                }
                QueueState(Undock);
                QueueState(BookmarkPrep, -1, Bookmark, Distance);
                return true;
            }
            if (Bookmark.LocationID != Session.SolarSystemID)
            {
                if (Route.Path.Last() != Bookmark.LocationID)
                {
                    Log.Log("|oSetting course");
                    Log.Log(" |-g{0}", Bookmark.Title);
                    Bookmark.SetDestination();
                }
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
            Entity Collision = null;
            if (Params.Count() > 2) Collision = (Entity)Params[2];

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
            if (!Config.WarpCollisionPrevention)
            {
                DoInstaWarp();
                Log.Log("|oWarping");
                Log.Log(" |-g{0} (|w{1} km|-g)", Destination.Title, Distance);
                Destination.WarpTo(Distance);
                InsertState(BookmarkWarp, -1, Destination, Distance);
                WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                return true;
            }

            Entity LCO = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 900));
            Entity LCO2 = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 500));
            if (LCO != null && Collision == null)
            {
                Collision = LCO;
                Log.Log("|oToo close for warp, orbiting");
                Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.WarpCollisionOrbit);
                Collision.Orbit((int)(Config.WarpCollisionOrbit * 1000));
                InsertState(BookmarkWarp, -1, Destination, Distance, Collision);
            }
            // Else, if we're in half trigger of a structure that isn't our current collision target, change orbit and collision target to it
            else if (LCO2 != null && Collision != null && Collision != LCO2)
            {
                Collision = LCO2;
                Log.Log("|oOrbiting");
                Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.WarpCollisionOrbit);
                Collision.Orbit((int)(Config.WarpCollisionOrbit * 1000));
                InsertState(BookmarkWarp, -1, Destination, Distance, Collision);
            }
            else if (LCO == null)
            {
                if (Destination.Exists && Destination.CanWarpTo)
                {
                    DoInstaWarp();
                    Log.Log("|oWarping");
                    Log.Log(" |-g{0} (|w{1} km|-g)", Destination.Title, Distance);
                    Destination.WarpTo(Distance);
                    InsertState(BookmarkWarp, -1, Destination, Distance);
                    WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                }
            }
            else if (Collision == LCO)
            {
                InsertState(BookmarkWarp, -1, Destination, Distance, Collision);
            }

            return true;
        }

        bool ObjectPrep(object[] Params)
        {
            Entity Entity = (Entity)Params[0];
            int Distance = (int)Params[1];

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
            Entity Collision = null;
            if (Params.Count() > 2) Collision = (Entity)Params[2];

            if (!Session.InSpace)
            {
                return true;
            }
            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                return false;
            }
            if (Entity.Distance < 150000 && Entity.Distance > 0)
            {
                return true;
            }
            if (!Config.WarpCollisionPrevention)
            {
                DoInstaWarp();
                Log.Log("|oWarping");
                Log.Log(" |-g{0} (|w{1} km|-g)", Entity.Name, Distance);
                Entity.WarpTo(Distance);
                InsertState(ObjectWarp, -1, Entity, Distance);
                WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                return true;
            }

            Entity LCO = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 900));
            Entity LCO2 = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 500));
            if (LCO != null && Collision == null)
            {
                Collision = LCO;
                Log.Log("|oToo close for warp, orbiting");
                Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.WarpCollisionOrbit);
                Collision.Orbit((int)(Config.WarpCollisionOrbit * 1000));
                InsertState(ObjectWarp, -1, Entity, Distance, Collision);
            }
            // Else, if we're in half trigger of a structure that isn't our current collision target, change orbit and collision target to it
            else if (LCO2 != null && Collision != null && Collision != LCO2)
            {
                Collision = LCO2;
                Log.Log("|oOrbiting");
                Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.WarpCollisionOrbit);
                Collision.Orbit((int)(Config.WarpCollisionOrbit * 1000));
                InsertState(ObjectWarp, -1, Entity, Distance, Collision);
            }
            else if (LCO == null)
            {
                if (Entity.Exists && Entity.Distance > 150000)
                {
                    DoInstaWarp();
                    Log.Log("|oWarping");
                    Log.Log(" |-g{0} (|w{1} km|-g)", Entity.Name, Distance);
                    Entity.WarpTo(Distance);
                    InsertState(ObjectWarp, -1, Entity, Distance);
                    WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                }
            }
            else if (Collision == LCO)
            {
                InsertState(ObjectWarp, -1, Entity, Distance, Collision);
            }

            return true;
        }

        public bool Undock(object[] Params)
        {
            if (Session.InSpace)
            {
                Log.Log("|oUndock complete");
                return true;
            }

            Log.Log("|oUndocking");
            Log.Log(" |-g{0}", Session.StationName);
            Station.Exit();
            InsertState(Undock);
            WaitFor(20, () => Session.InSpace);
            return true;
        }

        bool JumpThroughArray(object[] Params)
        {
            Entity JumpPortalArray = Entity.All.FirstOrDefault(a => a.GroupID == Group.JumpPortalArray);
            if (JumpPortalArray == null)
            {
                Log.Log("|yNo Jump Portal Array on grid");
                return true;
            }
            if (JumpPortalArray.Distance > 2500)
            {
                ApproachTarget = JumpPortalArray;
                ApproachDistance = 2500;
                InsertState(JumpThroughArray);
                InsertState(ApproachState);
                return true;
            }
            Log.Log("|oJumping through");
            Log.Log(" |-g{0}", JumpPortalArray.Name);
            JumpPortalArray.JumpThroughPortal();
            InsertState(JumpThroughArray);
            int CurSystem = Session.SolarSystemID;
            WaitFor(10, () => Session.SolarSystemID != CurSystem, () => MyShip.ToEntity.Mode == EntityMode.Approaching);
            return true;
        }

        bool ActivateEntity(object[] Params)
        {
            Entity Target = (Entity)Params[0];
            if (Target == null || !Target.Exists) return true;
            if (Target.Distance > 2500)
            {
                Clear();
                ApproachTarget = Target;
                ApproachDistance = 2500;
                QueueState(ApproachState);
                QueueState(ActivateEntity, -1, Target);
                return false;
            }
            Log.Log("|oActivating");
            Log.Log(" |-g{0}", Target.Name);

            Target.Activate();

            WaitFor(30, () => MyShip.ToEntity.Mode == EntityMode.Warping);
            return true;
        }

        #region Approach

        /// <summary>
        /// Approach an entity
        /// </summary>
        /// <param name="Target">The entity to approach</param>
        /// <param name="Distance">What distance from the entity to stop at</param>
        public void Approach(Entity Target, int Distance = 1000)
        {
            // If we're not doing anything, just start ApproachState
            if (Idle)
            {
                ApproachTarget = Target;
                ApproachDistance = Distance;
                Approaching = false;
                ApproachCollision = null;
                QueueState(ApproachState);
                return;
            }
            // If we're approaching something else or orbiting something, change to approaching the new target - retain collision information!
            if ((CurState.State == ApproachState && ApproachTarget != Target) || CurState.State == OrbitState)
            {
                ApproachTarget = Target;
                ApproachDistance = Distance;
                Clear();
                QueueState(ApproachState, -1, Target, Distance, false);
            }
        }

        Entity ApproachTarget;
        int ApproachDistance;
        bool Approaching;
        Entity ApproachCollision;

        bool ApproachState(object[] Params)
        {
            if (ApproachTarget == null || !ApproachTarget.Exists || ApproachTarget.Exploded || ApproachTarget.Released)
            {
                return true;
            }

            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                return false;
            }

            if ((ApproachTarget.CategoryID == Category.Asteroid ? ApproachTarget.SurfaceDistance : ApproachTarget.Distance) > ApproachDistance)
            {
                // Start approaching our approach target if we're not currently approaching anything
                if (!Approaching || (MyShip.ToEntity.Mode != EntityMode.Orbiting && MyShip.ToEntity.Mode != EntityMode.Approaching))
                {
                    if (ApproachTarget.SurfaceDistance > 150000 && (ApproachTarget.CategoryID == Category.Asteroid || ApproachTarget.CategoryID == Category.Structure || ApproachTarget.CategoryID == Category.Station || ApproachTarget.GroupID == Group.CargoContainer || ApproachTarget.GroupID == Group.Wreck))
                    {
                        DoInstaWarp();
                        Log.Log("|oWarping");
                        Log.Log(" |-g{0}(|w{1} km|-g)", ApproachTarget.Name, ApproachDistance / 1000);
                        ApproachTarget.WarpTo(ApproachDistance);
                        DislodgeWaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                        return false;
                    }

                    Approaching = true;
                    Log.Log("|oApproaching");
                    Log.Log(" |-g{0}(|w{1} km|-g)", ApproachTarget.Name, ApproachDistance / 1000);
                    ApproachTarget.Approach();
                    DislodgeWaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Approaching);
                    return false;
                }

                if (Config.ApproachCollisionPrevention)
                {
                    // Else, if we're in trigger of a structure and aren't already orbiting a structure, orbit it and set it as our collision target
                    Entity CollisionCheck = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.ApproachCollisionTrigger * 900));
                    if (CollisionCheck != null && ApproachCollision == null)
                    {
                        ApproachCollision = CollisionCheck;
                        Log.Log("|oOrbiting");
                        Log.Log(" |-g{0}(|w{1} km|-g)", ApproachCollision.Name, Config.ApproachCollisionOrbit);
                        ApproachCollision.Orbit((int)(Config.ApproachCollisionOrbit * 1000));
                        return false;
                    }
                    // Else, if we're in half trigger of a structure that isn't our current collision target, change orbit and collision target to it
                    CollisionCheck = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.ApproachCollisionTrigger * 500));
                    if (CollisionCheck != null && CollisionCheck != ApproachCollision)
                    {
                        ApproachCollision = CollisionCheck;
                        Log.Log("|oOrbiting");
                        Log.Log(" |-g{0}(|w{1} km|-g)", ApproachCollision.Name, Config.ApproachCollisionOrbit);
                        ApproachCollision.Orbit((int)(Config.ApproachCollisionOrbit * 1000));
                        return false;
                    }
                    // Else, if we're not within trigger of a structure and we have a collision target (orbiting a structure) change approach back to our approach target
                    CollisionCheck = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.ApproachCollisionTrigger * 900));
                    if (CollisionCheck == null && ApproachCollision != null)
                    {
                        ApproachCollision = null;
                        Log.Log("|oApproaching");
                        Log.Log(" |-g{0}(|w{1} km|-g)", ApproachTarget.Name, ApproachDistance / 1000);
                        ApproachTarget.Approach();
                        return false;
                    }
                }
            }
            else
            {
                if (MyShip.ToEntity.Velocity.Magnitude > 0)
                {
                    Command.CmdStopShip.Execute();
                }

                return true;
            }

            return false;
        }

        #endregion

        bool OrbitState(object[] Params)
        {
            Entity Target = ((Entity)Params[0]);
            int Distance = (int)Params[1];
            bool Orbiting = (bool)Params[2];
            Entity Collision = null;
            if (Params.Count() > 3) { Collision = (Entity)Params[3]; }

            if (Target == null || !Target.Exists || Target.Exploded || Target.Released)
            {
                return true;
            }

            // Start orbiting our orbit target if we're not currently orbiting anything
            if (!Orbiting || MyShip.ToEntity.Mode != EntityMode.Orbiting)
            {
                Log.Log("|oOrbiting");
                Log.Log(" |-g{0}(|w{1} km|-g)", Target.Name, Distance / 1000);
                Target.Orbit(Distance);
                InsertState(OrbitState, -1, Target, Distance, true);
                WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Orbiting);
            }
            else
            {
                Entity LCO = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 900));
                Entity LCO2 = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 500));
                // Else, if we're in trigger of a structure and aren't already orbiting a structure, orbit it and set it as our collision target
                if (Config.OrbitCollisionPrevention)
                {
                    if (LCO != null && Collision == null)
                    {
                        Collision = LCO;
                        Log.Log("|oOrbiting");
                        Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.OrbitCollisionOrbit);
                        Collision.Orbit((int)(Config.OrbitCollisionOrbit * 1000));
                        InsertState(OrbitState, -1, Target, Distance, true, Collision);
                    }
                    // Else, if we're in half trigger of a structure that isn't our current collision target, change orbit and collision target to it
                    else if (LCO2 != null && Collision != null && Collision != LCO2)
                    {
                        Collision = LCO2;
                        Log.Log("|oOrbiting");
                        Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.OrbitCollisionOrbit);
                        Collision.Orbit((int)(Config.OrbitCollisionOrbit * 1000));
                        InsertState(OrbitState, -1, Target, Distance, true, Collision);
                    }
                    // Else, if we're not within 1km of a structure and we have a collision target (orbiting a structure) change orbit back to our orbit target
                    else if (LCO == null && Collision != null)
                    {
                        Log.Log("|oOrbiting");
                        Log.Log(" |-g{0}(|w{1} km|-g)", Target.Name, Distance / 1000);
                        Target.Orbit(Distance);
                        InsertState(OrbitState, -1, Target, Distance, true);
                        WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                    }
                }
                else
                {
                    InsertState(OrbitState, -1, Target, Distance, true, Collision);
                }
            }
            return true;

        }

        readonly Dictionary<int, int> Bubbles = new Dictionary<int, int> {
                        {12200, 26500},
                        {26888, 40000},
                        {12199, 11500},
                        {26890, 17500},
                        {12198, 5000},
                        {26892, 7500},
                        {22778, 20000}
                        };

        readonly List<int> NullificationSubsystems = new List<int>()
        {
            30082,  // Legion
            30092,  // Tengu
            30102,  // Proteus
            30112   // Loki
        };

        bool Bubbled()
        {
            if (MyShip.ToEntity.GroupID == Group.Interceptor) return false;
            if (MyShip.ToEntity.TypeID == 34590) return false; // Victorieux Luxury Yacht
            // @TODO: T3 Nullification Subsystem
            if (!Entity.All.Any(a => Bubbles.Keys.Contains(a.TypeID))) return false;
            if (!Entity.All.Any(a => a.Distance < Bubbles.Values.Max() && Bubbles.Keys.Contains(a.TypeID))) return false;
            return Bubbles.Any(bubble => Entity.All.Any(a => a.TypeID == bubble.Key && a.Distance < bubble.Value));
        }

        bool AutoPilotPrep(object[] Params)
        {
            QueueAutoPilotDeactivation = false;
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

        bool QueueAutoPilotDeactivation;
        public bool SunMidpoint = false;
        bool AutoPilot(object[] Params)
        {
            if (Route.Path == null || Route.Path[0] == -1 || QueueAutoPilotDeactivation)
            {
                QueueAutoPilotDeactivation = false;
                Log.Log("|oAutopilot deactivated");
                return true;
            }

            if (Session.InSpace)
            {
                if (UndockWarp.Instance != null && !UndockWarp.Instance.Idle && UndockWarp.Instance.CurState.ToString() != "WaitStation") return false;
                if (MyShip.ToEntity.Mode == EntityMode.Warping) return false;
                Entity Sun = Entity.All.FirstOrDefault(a => a.GroupID == Group.Sun);
                if (SunMidpoint && Sun != null && Sun.Distance < 1000000000)
                {
                    if (Bubbled())
                    {
                        if (MyShip.Mode == EntityMode.Stopped)
                        {
                            Log.Log("|rBubble detected!");
                            Log.Log("|oAligning to |-g{0}", Sun.Name);
                            Sun.AlignTo();
                            InsertState(AutoPilot);
                            WaitFor(10, () => MyShip.ToEntity.Mode != EntityMode.Stopped);
                            return true;
                        }
                        else
                        {
                            DoInstaWarp();
                            Log.Log("|oWarping");
                            Log.Log(" |-g{0}", Route.NextWaypoint.Name);
                            Route.NextWaypoint.WarpTo();
                            return false;
                        }
                    }
                    DoInstaWarp();
                    Log.Log("|oWarping to |-g{0} |w(|y100 km|w)", Sun.Name);
                    Sun.WarpTo(100000);
                    InsertState(AutoPilot);
                    WaitFor(10, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                    return true;
                }
                if (Route.NextWaypoint.GroupID == Group.Stargate)
                {
                    if (Bubbled() && Route.NextWaypoint.Distance > 2000)
                    {
                        if (MyShip.Mode == EntityMode.Stopped)
                        {
                            Log.Log("|rBubble detected!");
                            Log.Log("|oAligning to |-g{0}", Route.NextWaypoint.Name);
                            Route.NextWaypoint.AlignTo();
                            InsertState(AutoPilot);
                            WaitFor(10, () => MyShip.ToEntity.Mode != EntityMode.Stopped);
                            return true;
                        }
                        else
                        {
                            DoInstaWarp();
                            Log.Log("|oWarping");
                            Log.Log(" |-g{0}", Route.NextWaypoint.Name);
                            Route.NextWaypoint.WarpTo();
                            return false;
                        }
                    }
                    if (!Entity.All.Any(a => a.GroupID == Group.Station && a.Distance < 150000)) DoInstaWarp();
                    Log.Log("|oJumping through to |-g{0}", Route.NextWaypoint.Name);
                    Route.NextWaypoint.Jump();
                    if (Route.Path != null && Route.Waypoints != null)
                    {
                        if (Route.Path.FirstOrDefault() == Route.Waypoints.FirstOrDefault()) QueueAutoPilotDeactivation = true;
                    }
                    int CurSystem = Session.SolarSystemID;
                    InsertState(AutoPilot);
                    WaitFor(10, () => Session.SolarSystemID != CurSystem, () => MyShip.ToEntity.Mode != EntityMode.Stopped);
                    return true;
                }
                if (Route.NextWaypoint.GroupID == Group.Station)
                {
                    if (Bubbled() && Route.NextWaypoint.Distance > 2000)
                    {
                        if (MyShip.Mode == EntityMode.Stopped)
                        {
                            Log.Log("|rBubble detected!");
                            Log.Log("|oAligning to |-g{0}", Route.NextWaypoint.Name);
                            Route.NextWaypoint.AlignTo();
                            InsertState(AutoPilot);
                            WaitFor(10, () => MyShip.ToEntity.Mode != EntityMode.Stopped);
                            return true;
                        }
                        else
                        {
                            DoInstaWarp();
                            Log.Log("|oWarping");
                            Log.Log(" |-g{0}", Route.NextWaypoint.Name);
                            Route.NextWaypoint.WarpTo();
                            return false;
                        }
                    }
                    InsertState(Dock, 500, Route.NextWaypoint);
                    return true;
                }
            }
            return false;
        }


        bool Dock(object[] Params)
        {
            if (!Session.InSpace) return true;

            Entity Target = (Entity)Params[0];
            Entity Collision = null;
            if (Params.Count() > 1) Collision = (Entity)Params[1];

            if (Params.Length == 0)
            {
                Log.Log("|yDock call incomplete");
                return true;
            }
            if (Session.InStation)
            {
                Log.Log("|oDock complete");
                return true;
            }
            if (!Config.WarpCollisionPrevention)
            {
                if (!Entity.All.Any(a => a.GroupID == Group.Station && a.Distance < 150000)) DoInstaWarp();
                Log.Log("|oDocking");
                Log.Log(" |-g{0}", Target.Name);
                Target.Dock();
                InsertState(Dock, -1, Target);
                WaitFor(10, () => Session.InStation, () => MyShip.ToEntity.Mode == EntityMode.Warping);
                return true;
            }

            Entity LCO = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 900));
            Entity LCO2 = Entity.All.FirstOrDefault(a => (a.GroupID == Group.LargeCollidableObject || a.GroupID == Group.LargeCollidableShip || a.GroupID == Group.LargeCollidableStructure || a.CategoryID == Category.Asteroid) && a.Type != "Beacon" && a.SurfaceDistance <= (double)(Config.WarpCollisionTrigger * 500));

            if (LCO != null && Collision == null)
            {
                Collision = LCO;
                Log.Log("|oToo close for warp, orbiting");
                Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.WarpCollisionOrbit);
                Collision.Orbit((int)(Config.WarpCollisionOrbit * 1000));
                InsertState(Dock, -1, Target, Collision);
            }
            // Else, if we're in .2km of a structure that isn't our current collision target, change orbit and collision target to it
            else if (LCO2 != null)
            {
                Collision = LCO2;
                Log.Log("|oOrbiting");
                Log.Log(" |-g{0}(|w{1} km|-g)", Collision.Name, Config.WarpCollisionOrbit);
                Collision.Orbit((int)(Config.WarpCollisionOrbit * 1000));
                InsertState(Dock, -1, Target, Collision);
            }
            else if (LCO == null)
            {
                if (!Entity.All.Any(a => a.GroupID == Group.Station && a.Distance < 150000)) DoInstaWarp();
                Log.Log("|oDocking");
                Log.Log(" |-g{0}", Target.Name);
                Target.Dock();
                InsertState(Dock, -1, Target);
                WaitFor(10, () => Session.InStation, () => MyShip.ToEntity.Mode == EntityMode.Warping);
            }
            else
            {
                InsertState(Dock, -1, Target, Collision);
            }

            return true;
        }

        #endregion

        #region Helper Methods

        void DoInstaWarp()
        {
            if (Config.InstaWarp && MyShip.ToEntity.Mode != EntityMode.Warping)
            {
                InstaWarpModule.Enabled(true);
            }
        }
        #endregion

    }

    class InstaWarp : State
    {
        public Logger Log = new Logger("MoveInstaWarp");
        #region Instantiation
        static InstaWarp _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static InstaWarp Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new InstaWarp();
                }
                return _Instance;
            }
        }

        private InstaWarp()
        {
            DefaultFrequency = 200;
        }
        #endregion
        #region Actions

        public void Enabled(bool val)
        {
            if (val)
            {
                if (Idle)
                {
                    Log.Log("|yDoing InstaWarp");
                    QueueState(Prepare);
                    QueueState(EnablePropmod);
                }
            }
            else
            {
                Clear();
            }
        }
        #endregion
        #region States
        bool Prepare(object[] Params)
        {
            if (!Session.InSpace) return false;
            return !MyShip.ToEntity.Cloaked;
        }

        bool EnablePropmod(object[] Params)
        {
            List<Module> propulsionModules = MyShip.Modules.Where(a => a.GroupID == Group.PropulsionModule && a.IsOnline).ToList();
            if (propulsionModules.Any())
            {
                if (propulsionModules.Any(a => a.AllowsActivate()))
                {
                    Log.Log("|g  InstaWarp turned on the propmod.");
                    propulsionModules.Where(a => a.AllowsActivate()).ForEach(m => m.Activate());
                }
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Settings for the UndockWarp class
    /// </summary>
    public class UndockWarpSettings : Settings
    {
        public string Substring = "Undock";
        public bool Enabled = false;
    }

    /// <summary>
    /// This class automatically performs a warp to a bookmark which contains the configured substring which is in-system and within 200km
    /// </summary>
    public class UndockWarp : State
    {
        #region Instantiation
        static UndockWarp _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static UndockWarp Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UndockWarp();
                }
                return _Instance;
            }
        }

        private UndockWarp()
        {
            DefaultFrequency = 200;
            if (Config.Enabled) QueueState(WaitStation);
        }

        #endregion

        #region Actions

        /// <summary>
        /// Toggle on/off this class
        /// </summary>
        /// <param name="val">Enabled = true</param>
        public void Enabled(bool val)
        {
            if (val)
            {
                if (Idle)
                {
                    QueueState(WaitStation);
                }
            }
            else
            {
                Clear();
            }
        }

        #endregion

        #region Variables

        /// <summary>
        /// The config for this class
        /// </summary>
        public UndockWarpSettings Config = new UndockWarpSettings();

        #endregion

        #region States

        bool Space(object[] Params)
        {
            if (Session.InStation)
            {
                QueueState(Station);
                return true;
            }
            if (Session.InSpace)
            {
                Bookmark undock = Bookmark.All.FirstOrDefault(a => a.Title.Contains(Config.Substring) && a.LocationID == Session.SolarSystemID && a.Distance < 2000000);
                if (undock != null) undock.WarpTo(0);
                QueueState(WaitStation);
                return true;
            }
            return false;
        }

        bool WaitStation(object[] Params)
        {
            if (Session.InStation)
            {
                QueueState(Station);
                return true;
            }
            return false;
        }

        bool Station(object[] Params)
        {
            if (Session.InSpace)
            {
                QueueState(Space);
                return true;
            }
            return false;
        }

        #endregion
    }
}

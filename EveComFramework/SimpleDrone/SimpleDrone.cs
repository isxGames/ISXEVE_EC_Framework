using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;
using EveComFramework.Targets;
using EveComFramework.Security;

namespace EveComFramework.SimpleDrone
{
    #region Settings

    public class LocalSettings : EveComFramework.Core.Settings
    {
        public bool ShortRange = true;
        public bool ShortRangeFrigate = false;
        public bool LongRange = false;
        public bool Sentry = false;
        public bool Fighter = false;
        public bool PrivateTargets = true;
        public int TargetSlots = 2;
    }

    #endregion

    public class SimpleDrone : State
    {
        #region Instantiation

        static SimpleDrone _Instance;
        public static SimpleDrone Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SimpleDrone();
                }
                return _Instance;
            }
        }

        private SimpleDrone() : base()
        {
            Rats.AddPriorityTargets();
            Rats.AddNPCs();
            Rats.AddTargetingMe();

            Rats.Ordering = new RatComparer();            
        }

        #endregion

        #region Variables

        public Core.Logger Console = new Core.Logger("SimpleDrone");
        public LocalSettings Config = new LocalSettings();
        Targets.Targets Rats = new Targets.Targets();
        public Dictionary<long, long> ActiveTargetList = new Dictionary<long, long>();

        #endregion

        #region Actions

        public void Enabled(bool var)
        {
            if (var)
            {
                if (Idle)
                {
                    QueueState(Control);
                }
            }
            else
            {
                Clear();
            }
        }

        public void Configure()
        {
            //UI.DroneControl Configuration = new UI.DroneControl();
            //Configuration.Show();
        }

        #endregion

        #region States

        Entity ActiveTarget;
        Dictionary<Entity, DateTime> TargetCooldown = new Dictionary<Entity, DateTime>();
        bool OutOfTargets = false;
        Dictionary<Drone, DateTime> NextDroneCommand = new Dictionary<Drone, DateTime>();
        bool DroneReady(Drone drone)
        {
            if (!NextDroneCommand.ContainsKey(drone)) return true;
            if (NextDroneCommand[drone] > DateTime.Now) return true;
            return false;
        }

        bool Control(object[] Params)
        {
            if (!Session.InSpace)
            {
                return false;
            }

            // If we're warping and drones are in space, recall them and stop the module
            if (MyShip.ToEntity.Mode == EntityMode.Warping && Drone.AllInSpace.Any())
            {
                Drone.AllInSpace.ReturnToDroneBay();
                return true;
            }

            // Escape for no drone modes selected
            if (!Config.ShortRange && !Config.LongRange && !Config.Sentry)
            {
                return false;
            }

            Entity WarpScrambling = Security.Security.Instance.ValidScramble;

            #region ActiveTarget selection

            Double MaxRange = 0;
            if (Config.ShortRange) MaxRange = 20000;
            if (Config.LongRange) MaxRange = Me.DroneControlDistance;

            if (WarpScrambling != null)
            {
                if (ActiveTarget != WarpScrambling && WarpScrambling.Distance < MaxRange)
                {
                    Console.Log("|rEntity on grid is/was warp scrambling!");
                    Console.Log("|oOveriding current active target");
                    Console.Log(" |-g{0}", WarpScrambling.Name);
                    ActiveTarget = WarpScrambling;
                    return false;
                }
            }

            if (ActiveTarget == null || !ActiveTarget.Exists || ActiveTarget.Exploded || ActiveTarget.Released)
            {
                ActiveTarget = null;
                if (Rats.LockedAndLockingTargetList.Any())
                {
                    if (Config.PrivateTargets)
                    {
                        ActiveTarget = Rats.LockedAndLockingTargetList.FirstOrDefault(a => !ActiveTargetList.ContainsValue(a.ID) && a.Distance < MaxRange);
                        if (Config.Sentry && ActiveTarget == null)
                        {
                            ActiveTarget = Rats.LockedAndLockingTargetList.FirstOrDefault(a => !ActiveTargetList.ContainsValue(a.ID) && a.Distance < Me.DroneControlDistance);
                        }
                        if (Config.Fighter && ActiveTarget == null)
                        {
                            ActiveTarget = Rats.LockedAndLockingTargetList.FirstOrDefault(a => !ActiveTargetList.ContainsValue(a.ID));
                        }
                    }
                    if (ActiveTarget == null && OutOfTargets)
                    {
                        ActiveTarget = Rats.LockedAndLockingTargetList.FirstOrDefault(a =>  a.Distance < MaxRange);
                    }
                    if (Config.Sentry && ActiveTarget == null && OutOfTargets)
                    {
                        ActiveTarget = Rats.LockedAndLockingTargetList.FirstOrDefault(a => !ActiveTargetList.ContainsValue(a.ID) && a.Distance < Me.DroneControlDistance);
                    }
                    if (Config.Fighter && ActiveTarget == null && OutOfTargets)
                    {
                        ActiveTarget = Rats.LockedAndLockingTargetList.FirstOrDefault(a => !ActiveTargetList.ContainsValue(a.ID));
                    }
                    if (ActiveTarget != null)
                    {
                        LavishScriptAPI.LavishScript.ExecuteCommand("relay \"all other\" Event[RatterUpdateActiveTargetList]:Execute[" + Me.CharID + "," + ActiveTarget.ID.ToString() + "]");
                    }
                }
            }

            #endregion

            #region LockManagement

            TargetCooldown = TargetCooldown.Where(a => a.Value >= DateTime.Now).ToDictionary(a => a.Key, a => a.Value);
            Rats.LockedAndLockingTargetList.ForEach(a => { TargetCooldown.AddOrUpdate(a, DateTime.Now.AddSeconds(2)); });
            if (WarpScrambling != null)
            {
                if (!WarpScrambling.LockedTarget && !WarpScrambling.LockingTarget)
                {
                    if (Rats.LockedAndLockingTargetList.Count >= Me.TrueMaxTargetLocks)
                    {
                        if (Rats.LockedTargetList.Any())
                        {
                            Rats.LockedTargetList.FirstOrDefault().UnlockTarget();
                        }
                        return false;
                    }
                    WarpScrambling.LockTarget();
                    return false;
                }
            }
            else
            {
                Entity NewTarget = Rats.UnlockedTargetList.FirstOrDefault(a => !TargetCooldown.ContainsKey(a) && a.Distance < MyShip.MaxTargetRange);
                if (Rats.LockedAndLockingTargetList.Count < Config.TargetSlots &&
                    NewTarget != null &&
                    Entity.All.FirstOrDefault(a => a.IsJamming && a.IsTargetingMe) == null)
                {
                    Console.Log("|oLocking");
                    Console.Log(" |-g{0}", NewTarget.Name);
                    TargetCooldown.AddOrUpdate(NewTarget, DateTime.Now.AddSeconds(2));
                    NewTarget.LockTarget();
                    OutOfTargets = false;
                    return false;
                }
            }
            OutOfTargets = true;

            #endregion

            // Done with target selection and lock management.  If we get this far and nothing is getting ready to be available, recall drones
            if (!Rats.LockedAndLockingTargetList.Any() && Drone.AllInSpace.Any(a => DroneReady(a)))
            {
                Drone.AllInSpace.Where(a => DroneReady(a)).ReturnToDroneBay();
                Drone.AllInSpace.ForEach(a => { NextDroneCommand.AddOrUpdate(a, DateTime.Now.AddSeconds(2)); });
                return false;
            }

            // Make sure ActiveTarget is locked.  If so, make sure it's the active target, if not, return.
            if (ActiveTarget.LockedTarget)
            {
                if (!ActiveTarget.IsActiveTarget)
                {
                    ActiveTarget.MakeActive();
                    return false;
                }
            }
            else
            {
                return false;
            }

            // Handle ShortRange
            if (Config.ShortRange && ActiveTarget.Distance < 20000)
            {
                // If only doing short range frigates
                if (Config.ShortRangeFrigate)
                {
                    // Make sure target is a frigate
                    if (Data.NPCClasses.All.Any(a => a.Key == ActiveTarget.GroupID && (a.Value == "Destroyer" || a.Value == "Frigate")))
                    {
                        // Recall fighters and sentries
                        if (Drone.AllInSpace.Any(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && (b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                        {
                            foreach (Drone d in Drone.AllInSpace.Where(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && (b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                            {
                                d.ReturnToDroneBay();
                                NextDroneCommand.AddOrUpdate(d, DateTime.Now.AddSeconds(2));
                            }
                            return false;
                        }
                        // Send drones to attack
                        else if (Drone.AllInSpace.Any(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                        {
                            foreach (Drone d in Drone.AllInSpace.Where(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                            {
                                d.Attack();
                                NextDroneCommand.AddOrUpdate(d, DateTime.Now.AddSeconds(1));
                            }
                            return false;
                        }
                        // Launch drones
                        if (Drone.AllInSpace.Count() < Me.MaxActiveDrones
                            && Drone.AllInBay.Any(a => Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                        {
                            int capacity = Me.MaxActiveDrones - Drone.AllInSpace.Count();
                            foreach (Drone d in Drone.AllInBay.Where(a => Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                            {
                                d.Launch();
                                NextDroneCommand.AddOrUpdate(d, DateTime.Now.AddSeconds(1));
                                capacity--;
                                if (capacity == 0) return false;
                            }
                            return false;
                        }
                    }
                }
                else
                {
                    // Recall fighters and sentries
                    if (Drone.AllInSpace.Any(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && (b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                    {
                        foreach (Drone d in Drone.AllInSpace.Where(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && (b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                        {
                            d.ReturnToDroneBay();
                            NextDroneCommand.AddOrUpdate(d, DateTime.Now.AddSeconds(2));
                        }
                        return false;
                    }
                    // Send drones to attack
                    else if (Drone.AllInSpace.Any(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                    {
                        foreach (Drone d in Drone.AllInSpace.Where(a => DroneReady(a) && Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                        {
                            d.Attack();
                            NextDroneCommand.AddOrUpdate(d, DateTime.Now.AddSeconds(1));
                        }
                        return false;
                    }
                    // Launch drones
                    if (Drone.AllInSpace.Count() < Me.MaxActiveDrones
                        && Drone.AllInBay.Any(a => Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                    {
                        int capacity = Me.MaxActiveDrones - Drone.AllInSpace.Count();
                        foreach (Drone d in Drone.AllInBay.Where(a => Data.DroneType.All.Any(b => b.ID == a.TypeID && !(b.Group == "Fighters" || b.Group == "Sentry Drones"))))
                        {
                            d.Launch();
                            NextDroneCommand.AddOrUpdate(d, DateTime.Now.AddSeconds(1));
                            capacity--;
                            if (capacity == 0) return false;
                        }
                        return false;
                    }
                }
            }


            return false;
        }

        #endregion
    }
}

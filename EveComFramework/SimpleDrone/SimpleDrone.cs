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
        public bool ShortRangeClear = true;
        public bool LongRangeClear = false;
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
            if (!Config.ShortRangeClear && !Config.LongRangeClear && !Config.Sentry)
            {
                return false;
            }

            Entity WarpScrambling = Security.Security.Instance.ValidScramble;

            #region ActiveTarget selection

            Double MaxRange = 0;
            if (Config.ShortRangeClear) MaxRange = 20000;
            if (Config.LongRangeClear) MaxRange = Me.DroneControlDistance;

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

            return false;
        }

        #endregion
    }
}

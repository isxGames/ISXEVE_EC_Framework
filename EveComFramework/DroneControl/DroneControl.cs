using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Targets;

namespace EveComFramework.DroneControl
{
    #region Settings

    public class DroneControlSettings : EveComFramework.Core.Settings
    {
        public bool CombatDrones = false;
        public bool Sentries = false;
        public int CombatTargetsReserved = 2;
        public int SentryRange = 20;
        public int SentryDistanceLimit = 20;
        public int SentryCountLimit = 3;
        public int CombatTimeout;

        public bool LogisticsDrones = true;
        public int LogiDroneCount = 3;
        public int LogiDroneTargets = 2;

        public bool SalvageDrones = true;
        public int SalvageLockCount = 2;

        public bool MiningDrones = true;
        public int MiningLockCount = 2;
    }

    #endregion

    public class DroneControl : EveComFramework.Core.State
    {
        #region Instantiation

        static DroneControl _Instance;
        public static DroneControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DroneControl();
                }
                return _Instance;
            }
        }

        private DroneControl() : base()
        {
            Rats.AddTargetingMe();
            Rats.AddNPCs();
            RatComparer ratComp = new RatComparer();
            Rats.Ordering = ratComp;
            ratComp.ClassOrder.Add("Sentry");
            ratComp.ClassOrder.Add("Swarm");
            ratComp.ClassOrder.Add("Other");
            ratComp.ClassOrder.Add("Hauler");
            ratComp.ClassOrder.Add("Officer");
            ratComp.ClassOrder.Add("Battleship");
            ratComp.ClassOrder.Add("BattleCruiser");
            ratComp.ClassOrder.Add("Cruiser");
            ratComp.ClassOrder.Add("Destroyer");
            ratComp.ClassOrder.Add("Frigate");


            Roids.AddQuery(ent => ent.CategoryID == Category.Asteroid);
            Wrecks.AddQuery(ent => ent.GroupID == Group.Wreck);
        }

        #endregion

        #region Variables

        public Core.Logger Log = new Core.Logger("DroneControl");
        public DroneControlSettings Config = new DroneControlSettings();
        public Targets.Targets Rats = new Targets.Targets();
        public Targets.Targets Roids = new Targets.Targets();
        public Targets.Targets Wrecks = new Targets.Targets();
        public Core.Busy Busy = Core.Busy.Instance;

        public Dictionary<string, LinkedList<Drone>> BayQueues = new Dictionary<string, LinkedList<Drone>>();

        public Dictionary<Drone, DronePersist> DroneData = new Dictionary<Drone, DronePersist>();

        int MyMaxCombatLocks
        {
            get
            {
                return Math.Min(Me.MaxTargetLocks, Config.CombatTargetsReserved);
            }
        }

        int MyMaxLogiLocks
        {
            get
            {
                return Math.Min(Me.MaxTargetLocks, Config.LogiDroneTargets);
            }
        }

        int MyMaxSalvageLocks
        {
            get
            {
                return Math.Min(Me.MaxTargetLocks, Config.SalvageLockCount);
            }
        }

        int MyMaxMiningLocks
        {
            get
            {
                return Math.Min(Me.MaxTargetLocks, Config.MiningLockCount);
            }
        }


        public class RatComparer : Comparer<Entity>
        {
            public List<string> ClassOrder = new List<string>();

            public override int Compare(Entity x, Entity y)
            {
                if (x == null && y == null)
                    return 0;
                if (x == null)
                    return 1;
                if (y == null)
                    return -1;
                if (x == y)
                    return 0;
                int orderx = 0;
                
                if(Data.NPCClasses.All.ContainsKey(x.GroupID))
                {
                    orderx = ClassOrder.IndexOf(Data.NPCClasses.All[x.GroupID]) + 2;
                }
                else if (x.IsTargetingMe)
                {
                    orderx = 1;
                }
                int ordery = 0;
                

                if(Data.NPCClasses.All.ContainsKey(y.GroupID))
                {
                    ordery = ClassOrder.IndexOf(Data.NPCClasses.All[y.GroupID]) + 2;
                }
                else if (y.IsTargetingMe)
                {
                    ordery = 1;
                }

                if (orderx > ordery)
                    return -1;
                if (orderx < ordery)
                    return 1;
                return 0;
            }
        }
        #endregion

        #region Actions

        public void Start()
        {
            if (Idle)
            {
                QueueState(CombatCheck);
            }
            if (!Idle && CurState.State == Paused)
            {
                ClearCurState();
            }
        }

        public void Stop()
        {
            Clear();
        }

        public void Pause()
        {
            if (!Idle)
            {
                DislodgeCurState(args => true);
                InsertState(Paused);
                InsertState(ResetBusy);
                WaitFor(30, () => Drone.AllInSpace.Count() == 0);
                Drone.AllInSpace.ReturnToDroneBay();
            }
        }

        public void Resume()
        {
            if (!Idle && CurState.State == Paused)
            {
                ClearCurState();
            }
        }

        public void Configure()
        {
            UI.DroneControl Configuration = new UI.DroneControl();
            Configuration.Show();
        }

        #endregion

        #region States

        bool Combat(object[] Params)
        {
            if (!Config.CombatDrones)
            {
                WaitFor(Config.CombatTimeout, () => Rats.TargetList.Count == 0, () => Rats.TargetList.Count > 0);
                QueueState(CombatCheck);
                return true;
            }

            if (!Session.InSpace)
            {
                return false;
            }

            Drone.AllInBay.Union(Drone.AllInSpace).ForEach(drone =>
            {
                if (!DroneData.ContainsKey(drone))
                {
                    string droneGroup = Data.DroneType.All.First(dronetype => dronetype.ID == drone.TypeID).Group;
                    DroneData.Add(drone, new DronePersist() { Group = droneGroup });
                    if (!BayQueues.ContainsKey(droneGroup))
                    {
                        BayQueues.Add(droneGroup, new LinkedList<Drone>());
                    }
                    if (drone.InBay)
                    {
                        BayQueues[droneGroup].AddLast(drone);
                    }
                }
            });

            if (Drone.AllInSpace.Count() == 0)
            {
                Busy.SetDone("DroneControl");
            }

            if (Rats.TargetList.Count == 0)
            {
                WaitFor(30, () => Rats.TargetList.Count > 0);
                QueueState(CombatCheck);
                return true;
            }

            if (MyShip.ToEntity.Mode == EntityMode.Warping && Drone.AllInSpace.Count(drone => drone.State == EntityState.Departing || drone.State == EntityState.Departing_2) > 0)
            {
                Drone.AllInSpace.Where(drone => drone.State != EntityState.Departing && drone.State != EntityState.Departing_2).LimitCommand().ReturnToDroneBay();
                Drone.AllInSpace.Where(drone => drone.GroupID == Group.CombatDrone || drone.GroupID == Group.FighterDrone || drone.GroupID == Group.ElectronicWarfareDrone).ForEach(drone => { DroneData[drone].Target = null; DroneData[drone].Assigned = false; });
                return false;
            }

            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                return false;
            }

            List<Drone> ReturnedDrones = Drone.AllInSpace.Where(drone => drone.ToEntity.ShieldPct == 0 || (drone.ToEntity.ShieldPct < 75 && drone.ToEntity.ShieldPct < DroneData[drone].Health)).Select(drone => { DroneData[drone].Health = drone.ToEntity.ShieldPct; return drone; }).LimitCommand().ToList();

            Drone.AllInSpace.ForEach(drone => { DroneData[drone].Health = drone.ToEntity.ShieldPct; DroneData[drone].Armor = drone.ToEntity.ArmorPct; DroneData[drone].Hull = drone.ToEntity.HullPct; });

            if(ReturnedDrones.Count > 0)
            {
                ReturnedDrones.ReturnToDroneBay();
                ReturnedDrones.ForEach(drone => { DroneData[drone].Assigned = false; DroneData[drone].Target = null; });
                return false;
            }

            if (Rats.GetLocks(MyMaxCombatLocks))
            {
                return false;
            }

            DroneData.Keys.Where(drone => DroneData[drone].Assigned).Where(drone => DroneData[drone].Target == null || !DroneData[drone].Target.Exists || DroneData[drone].Target.Exploded || (!DroneData[drone].Target.LockedTarget && !DroneData[drone].Target.LockingTarget)).ToList().ForEach(drone => { DroneData[drone].Assigned = false; DroneData[drone].Target = null; });

            foreach (Entity rat in Rats.LockedAndLockingTargetList)
            {
                string ratClass = Data.NPCClasses.All.ContainsKey(rat.GroupID) ? Data.NPCClasses.All[rat.GroupID] : "";


                int DroneCount = Me.MaxActiveDrones;

                if (DroneData.Count(data => data.Value.Assigned && data.Value.Target == rat) > DroneCount)
                {
                    DroneData.Where(data => data.Value.Assigned && data.Value.Target == rat).Select(dd => dd.Key).Skip(DroneCount).ToList().ForEach(drone => { DroneData[drone].Assigned = false; DroneData[drone].Target = null; });
                }

                foreach (Drone drone in DroneData.Keys.Where(drone => DroneData[drone].Assigned && DroneData[drone].Target == rat).ToList())
                {
                    if (!IsBestDrone(drone, ratClass, Config.Sentries && rat.Distance > (Config.SentryRange * 1000)))
                    {
                        DroneData[drone].Assigned = false;
                        DroneData[drone].Target = null;
                    }
                }

                while (DroneData.Count(data => data.Value.Assigned && data.Value.Target == rat) < DroneCount && DroneData.Count(data => data.Value.Assigned) < Me.MaxActiveDrones)
                {
                    Drone Best;
                    if (Config.Sentries && rat.Distance > (Config.SentryRange * 1000))
                    {
                        Best = FindDroneByGroup("Sentry Drones") ?? BestDrone(ratClass);
                    }
                    else
                    {
                        Best = BestDrone(ratClass);
                    }
                    if (Best == null)
                    {
                        break;
                    }
                    BayQueues[DroneData[Best].Group].Remove(Best);
                    BayQueues[DroneData[Best].Group].AddLast(Best);
                    DroneData[Best].Assigned = true;
                    DroneData[Best].Target = rat;
                }
            }

            if (DroneData.Keys.Count(drone => DroneData[drone].Assigned) < Me.MaxActiveDrones && Rats.LockedAndLockingTargetList.Count > 0 && Rats.LockedAndLockingTargetList.Count == Rats.TargetList.Count)
            {
                Entity FirstTarget = Rats.LockedTargetList.First();
                string ratClass = Data.NPCClasses.All.ContainsKey(FirstTarget.GroupID) ? Data.NPCClasses.All[FirstTarget.GroupID] : "";
                while (DroneData.Keys.Count(drone => DroneData[drone].Assigned) < Me.MaxActiveDrones)
                {
                    Drone Best;
                    if (Config.Sentries && FirstTarget.Distance > Config.SentryRange*1000)
                    {
                        Best = FindDroneByGroup("Sentry Drones") ?? BestDrone(ratClass);
                    }
                    else
                    {
                        Best = BestDrone(ratClass);
                    }
                    if (Best == null)
                    {
                        break;
                    }
                    BayQueues[DroneData[Best].Group].Remove(Best);
                    BayQueues[DroneData[Best].Group].AddLast(Best);
                    DroneData[Best].Assigned = true;
                    DroneData[Best].Target = FirstTarget;
                }
            }

            if (DroneData.Count(drone => drone.Value.Group == "Sentry Drones") > Config.SentryCountLimit)
            {
                DroneData.Where(drone => drone.Value.Group != "Sentry Drones" && drone.Value.Assigned && drone.Value.Target.Distance > Config.SentryDistanceLimit*1000).Select(drone => drone.Key).ToList().ForEach(drone => { DroneData[drone].Assigned = false; DroneData[drone].Target = null; });
            }

            if (Drone.AllInSpace.Count(drone => !DroneData[drone].Assigned && drone.State != EntityState.Departing && drone.State != EntityState.Departing_2) > Me.MaxActiveDrones - Drone.AllInBay.Count(drone => DroneData[drone].Assigned))
            {
                Drone.AllInSpace.Where(drone => !DroneData[drone].Assigned && drone.State != EntityState.Departing && drone.State != EntityState.Departing_2).ReturnToDroneBay();
                return false;
            }

            if (Drone.AllInBay.Count(drone => DroneData[drone].Assigned) > 0 && Drone.AllInSpace.Count() < Me.MaxActiveDrones)
            {
                Drone.AllInBay.Where(drone => DroneData[drone].Assigned).Take(Me.MaxActiveDrones - Drone.AllInSpace.Count()).Launch();
                Busy.SetBusy("DroneControl", Pause);
                return false;
            }

            if (Drone.AllInSpace.Count(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].Target.LockedTarget) > 0)
            {
                List<Drone> CurTargetLimited = Drone.AllInSpace.Where(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].Target.IsActiveTarget && DroneData[drone].Target.LockedTarget).LimitCommand().ToList();
                if (CurTargetLimited.Count > 0)
                {
                    CurTargetLimited.Attack();
                    return false;
                }
                if (Drone.AllInSpace.Count(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].LastCommandTime < DateTime.Now && DroneData[drone].Target.LockedTarget) > 0)
                {
                    Entity target = DroneData[Drone.AllInSpace.First(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].LastCommandTime < DateTime.Now && DroneData[drone].Target.LockedTarget)].Target;
                    if (!target.IsActiveTarget)
                    {
                        target.MakeActive();
                        return false;
                    }
                }
            }

            return false;
        }

        bool CombatCheck(object[] args)
        {
            if (Rats.TargetList.Count > 0)
            {
                QueueState(Combat);
            }
            else
            {
                Drone.AllInSpace.Where(drone => drone.GroupID == Group.CombatDrone || drone.GroupID == Group.FighterDrone || drone.GroupID == Group.ElectronicWarfareDrone).ReturnToDroneBay();
                Drone.AllInSpace.Where(drone => drone.GroupID == Group.CombatDrone || drone.GroupID == Group.FighterDrone || drone.GroupID == Group.ElectronicWarfareDrone).ForEach(drone => { BayQueues[DroneData[drone].Group].AddLast(drone); DroneData[drone].Target = null; DroneData[drone].Assigned = false; });
                QueueState(myargs => true, 5000);
                QueueState(LogisticsOnDrones);
            }
            if (!Drone.AllInSpace.Any())
            {
                Busy.SetDone("DroneControl");
            }
            return true;
        }

        bool LogisticsOnDrones(object[] args)
        {
            if (!Config.LogisticsDrones)
            {
                QueueState(Salvage);
                return true;
            }

            if (!Session.InSpace)
            {
                return false;
            }

            if (Drone.AllInSpace.Count() == 0)
            {
                Busy.SetDone("DroneControl");
            }

            Drone.AllInBay.Union(Drone.AllInSpace).ForEach(drone =>
            {
                if (!DroneData.ContainsKey(drone))
                {
                    string droneGroup = Data.DroneType.All.First(dronetype => dronetype.ID == drone.TypeID).Group;
                    DroneData.Add(drone, new DronePersist() { Group = droneGroup });
                    if (!BayQueues.ContainsKey(droneGroup))
                    {
                        BayQueues.Add(droneGroup, new LinkedList<Drone>());
                    }
                    if (drone.InBay)
                    {
                        BayQueues[droneGroup].AddLast(drone);
                    }
                }
            });


            if (Rats.TargetList.Count > 0)
            {
                Drone.AllInSpace.Where(drone => drone.GroupID == Group.RepairDrone).ReturnToDroneBay();
                QueueState(Combat);
                return true;
            }

            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                Drone.AllInSpace.Where(drone => drone.State != EntityState.Departing && drone.State != EntityState.Departing_2).LimitCommand().ReturnToDroneBay();
                return false;
            }

            if (DroneData.Count(data => data.Value.Armor < 100) == 0 || DroneData.Count(data => data.Key.Type.Contains("Armor")) == 0)
            {
                Drone.AllInSpace.ReturnToDroneBay();
                QueueState(Salvage);
                return true;
            }

            Drone.AllInSpace.ForEach(drone => { DroneData[drone].Health = drone.ToEntity.ShieldPct; DroneData[drone].Armor = drone.ToEntity.ArmorPct; DroneData[drone].Hull = drone.ToEntity.HullPct; });

            List<Drone> Reppers = Drone.AllInSpace.Where(drone => drone.Type.Contains("Armor")).ToList();

            if (Reppers.Count < Config.LogiDroneCount && Drone.AllInBay.Count(drone => drone.Type.Contains("Armor")) > 0)
            {
                Drone.AllInBay.Where(drone => drone.Type.Contains("Armor")).Take(Config.LogiDroneCount - Reppers.Count).LimitCommand().Launch();
                Busy.SetBusy("DroneControl", Pause);
                return false;
            }

            if (Drone.AllInSpace.Where(drone => DroneData[drone].Armor == 100).Except(Reppers.Take(Config.LogiDroneCount)).Count() > 0)
            {
                Drone.AllInSpace.Where(drone => DroneData[drone].Armor == 100).Except(Reppers.Take(Config.LogiDroneCount)).LimitCommand().ReturnToDroneBay();
                return false;
            }

            List<Drone> Damaged = Drone.AllInSpace.Where(drone => DroneData[drone].Armor < 100).ToList();

            foreach (Drone damageddrone in Damaged)
            {
                if (!damageddrone.ToEntity.LockedTarget || damageddrone.ToEntity.LockingTarget && Entity.Targeting.Count + Entity.Targets.Count < Me.MaxTargetLocks)
                {
                    damageddrone.ToEntity.LockTarget();
                    return false;
                }
            }

            if (Damaged.Count(drone => drone.ToEntity.LockedTarget) > 0)
            {
                Entity firstDrone = Damaged.First(drone => drone.ToEntity.LockedTarget).ToEntity;
                if (!firstDrone.IsActiveTarget)
                {
                    firstDrone.MakeActive();
                    return false;
                }
                else
                {
                    List<Drone> NeedAssigned = Reppers.Where(drone => drone.Target != firstDrone).LimitCommand().ToList();
                    if (NeedAssigned.Count > 0)
                    {
                        NeedAssigned.Repair();
                        return false;
                    }
                }
            }

            if (Damaged.Union(Reppers).Count() < Me.MaxActiveDrones)
            {
                Drone.AllInBay.Where(drone => DroneData[drone].Armor < 100).Take(Me.MaxActiveDrones - Damaged.Union(Reppers).Count()).LimitCommand().Launch();
                Busy.SetBusy("DroneControl", Pause);
                return false;
            }

            return false;
        }

        bool Salvage(object[] args)
        {
            if (!Config.SalvageDrones)
            {
                QueueState(Mining);
                return true;
            }

            if (!Session.InSpace)
            {
                return false;
            }

            if (Drone.AllInSpace.Count() == 0)
            {
                Busy.SetDone("DroneControl");
            }

            Drone.AllInBay.Union(Drone.AllInSpace).ForEach(drone =>
            {
                if (!DroneData.ContainsKey(drone))
                {
                    string droneGroup = Data.DroneType.All.First(dronetype => dronetype.ID == drone.TypeID).Group;
                    DroneData.Add(drone, new DronePersist() { Group = droneGroup });
                    if (!BayQueues.ContainsKey(droneGroup))
                    {
                        BayQueues.Add(droneGroup, new LinkedList<Drone>());
                    }
                    if (drone.InBay)
                    {
                        BayQueues[droneGroup].AddLast(drone);
                    }
                }
            });


            if (Rats.TargetList.Count > 0)
            {
                Drone.AllInSpace.Where(drone => drone.GroupID == Group.MiningDrone || drone.GroupID == Group.SalvageDrone || drone.GroupID == Group.RepairDrone).ReturnToDroneBay();
                QueueState(Combat);
                return true;
            }

            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                return false;
            }

            if (Wrecks.TargetList.Count == 0 || DroneData.Count(data => data.Key.GroupID == Group.SalvageDrone) == 0)
            {
                Drone.AllInSpace.Where(drone => drone.GroupID == Group.SalvageDrone).ReturnToDroneBay();
                QueueState(Mining);
                return true;
            }

            List<Drone> recalls = Drone.AllInSpace.Where(drone => drone.GroupID != Group.SalvageDrone && drone.State != EntityState.Departing && drone.State != EntityState.Departing_2).LimitCommand().ToList();

            if (recalls.Count > 0)
            {
                recalls.ReturnToDroneBay();
                return false;
            }

            if(Drone.AllInSpace.Count(drone => drone.GroupID == Group.SalvageDrone) < Me.MaxActiveDrones && Me.MaxActiveDrones > Drone.AllInSpace.Count() && Drone.AllInBay.Count(drone => drone.GroupID == Group.SalvageDrone) > 0)
            {
                Drone.AllInBay.Where(drone => drone.GroupID == Group.SalvageDrone).Take(Me.MaxActiveDrones - Drone.AllInSpace.Count()).Launch();
                Busy.SetBusy("DroneControl", Pause);
                return false;
            }

            double MaxLocks = MyMaxSalvageLocks;
            if (MaxLocks > Wrecks.TargetList.Count)
            {
                MaxLocks = Wrecks.TargetList.Count;
            }

            double Spread = (double)DroneData.Count(data => data.Key.GroupID == Group.SalvageDrone) / MaxLocks;
            double SpreadMiddle = (double)DroneData.Count(data => data.Key.GroupID == Group.SalvageDrone) % MaxLocks;
            int SpreadCount = 0;
            int CurSpread = (int)Math.Ceiling(Spread);


            DroneData.Keys.Where(drone => DroneData[drone].Assigned).Where(drone => DroneData[drone].Target == null || !DroneData[drone].Target.Exists || DroneData[drone].Target.Exploded).ToList().ForEach(drone => { DroneData[drone].Assigned = false; DroneData[drone].Target = null; });

            foreach (Entity wreck in Wrecks.LockedTargetList)
            {
                SpreadCount++;
                if (SpreadCount > SpreadMiddle)
                {
                    CurSpread = (int)Math.Floor(Spread);
                }
                if (DroneData.Count(data => data.Value.Target == wreck) < CurSpread)
                {
                    List<Drone> free = Drone.AllInSpace.Where(drone => !DroneData[drone].Assigned && drone.State != EntityState.Departing && drone.State != EntityState.Departing_2).Take(CurSpread - DroneData.Count(data => data.Value.Target == wreck)).ToList();
                    if (free.Count > 0)
                    {
                        free.ForEach(drone => { DroneData[drone].Target = wreck; DroneData[drone].Assigned = true; });
                        return false;
                    }
                }

            }

            if (Drone.AllInSpace.Count(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target) > 0)
            {
                List<Drone> CurTargetLimited = Drone.AllInSpace.Where(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].Target.IsActiveTarget).LimitCommand().ToList();
                if (CurTargetLimited.Count > 0)
                {
                    CurTargetLimited.Salvage();
                    return false;
                }
                if (Drone.AllInSpace.Count(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].LastCommandTime < DateTime.Now) > 0)
                {
                    Entity target = DroneData[Drone.AllInSpace.First(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].LastCommandTime < DateTime.Now)].Target;
                    if (!target.IsActiveTarget)
                    {
                        target.MakeActive();
                        return false;
                    }
                }
            }

            Wrecks.GetLocks(MyMaxSalvageLocks);
            return false;
        }

        bool Mining(object[] args)
        {
            if (!Config.MiningDrones)
            {
                QueueState(CombatCheck);
                return true;
            }
            if (!Session.InSpace)
            {
                return false;
            }

            if (Drone.AllInSpace.Count() == 0)
            {
                Busy.SetDone("DroneControl");
            }

            Drone.AllInBay.Union(Drone.AllInSpace).ForEach(drone =>
            {
                if (!DroneData.ContainsKey(drone))
                {
                    string droneGroup = Data.DroneType.All.First(dronetype => dronetype.ID == drone.TypeID).Group;
                    DroneData.Add(drone, new DronePersist() { Group = droneGroup });
                    if (!BayQueues.ContainsKey(droneGroup))
                    {
                        BayQueues.Add(droneGroup, new LinkedList<Drone>());
                    }
                    if (drone.InBay)
                    {
                        BayQueues[droneGroup].AddLast(drone);
                    }
                }
            });


            if (Rats.TargetList.Count > 0)
            {
                Drone.AllInSpace.Where(drone => drone.GroupID == Group.MiningDrone || drone.GroupID == Group.SalvageDrone || drone.GroupID == Group.RepairDrone).ReturnToDroneBay();
                QueueState(Combat);
                return true;
            }

            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                return false;
            }

            double MaxLocks = MyMaxMiningLocks;
            if (MaxLocks > Roids.TargetList.Count)
            {
                MaxLocks = Roids.TargetList.Count;
            }

            double Spread = (double)DroneData.Count(data => data.Key.GroupID == Group.MiningDrone) / MaxLocks;
            double SpreadMiddle = (double)DroneData.Count(data => data.Key.GroupID == Group.MiningDrone) % MaxLocks;
            int SpreadCount = 0;
            int CurSpread = (int)Math.Ceiling(Spread);


            DroneData.Keys.Where(drone => DroneData[drone].Assigned).Where(drone => DroneData[drone].Target == null || !DroneData[drone].Target.Exists || DroneData[drone].Target.Exploded).ToList().ForEach(drone => { DroneData[drone].Assigned = false; DroneData[drone].Target = null; });

            if (Drone.AllInSpace.Count(drone => drone.GroupID == Group.MiningDrone) < Me.MaxActiveDrones && Me.MaxActiveDrones > Drone.AllInSpace.Count() && Drone.AllInBay.Count(drone => drone.GroupID == Group.MiningDrone) > 0)
            {
                Drone.AllInBay.Where(drone => drone.GroupID == Group.MiningDrone).Take(Me.MaxActiveDrones - Drone.AllInSpace.Count()).Launch();
                Busy.SetBusy("DroneControl", Pause);
                return false;
            }

            foreach (Entity roid in Roids.LockedTargetList)
            {
                SpreadCount++;
                if (SpreadCount > SpreadMiddle)
                {
                    CurSpread = (int)Math.Floor(Spread);
                }
                if (DroneData.Count(data => data.Value.Target == roid) < CurSpread)
                {
                    List<Drone> free = Drone.AllInSpace.Where(drone => !DroneData[drone].Assigned && drone.State != EntityState.Departing && drone.State != EntityState.Departing_2).Take(CurSpread - DroneData.Count(data => data.Value.Target == roid)).ToList();
                    if (free.Count > 0)
                    {
                        free.ForEach(drone => { DroneData[drone].Target = roid; DroneData[drone].Assigned = true; });
                        return false;
                    }
                }

            }

            if (Drone.AllInSpace.Count(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target) > 0)
            {
                List<Drone> CurTargetLimited = Drone.AllInSpace.Where(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].Target.IsActiveTarget).LimitCommand().ToList();
                if (CurTargetLimited.Count > 0)
                {
                    CurTargetLimited.MineRepeatedly();
                    return false;
                }
                if (Drone.AllInSpace.Count(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].LastCommandTime < DateTime.Now) > 0)
                {
                    Entity target = DroneData[Drone.AllInSpace.First(drone => DroneData[drone].Assigned && DroneData[drone].Target != drone.Target && DroneData[drone].LastCommandTime < DateTime.Now)].Target;
                    if (!target.IsActiveTarget)
                    {
                        target.MakeActive();
                        return false;
                    }
                }
            }


            return false;
        }

        public bool ResetBusy(object[] args)
        {
            Busy.SetDone("DroneControl");
            return true;
        }

        public bool Paused(object[] args)
        {
            return false;
        }

        #endregion

        public class DronePersist
        {
            public DateTime LastCommandTime;
            public double Health = 100;
            public bool Assigned = false;
            public Entity Target = null;
            public string Group = "";
            public double Armor = 100;
            public double Hull = 100;
        }

        public bool IsBestDrone(Drone drone, string Class, bool Sentry)
        {
            Drone Better;
            if (Sentry)
            {
                Better = FindDroneByGroup("Sentry Drones") ?? BestDrone(Class);
            }
            else
            {
                Better = BestDrone(Class);
            }

            if (Better == null)
            {
                return true;
            }

            if (Sentry)
            {
                if (DroneData[drone].Group == "Sentry Drones")
                {
                    return true;
                }
                else if (DroneData[Better].Group == "Sentry Drones")
                {
                    return false;
                }
            }

            switch (Class)
            {
                case "BattleShip":
                case "BattleCruiser":
                    switch (DroneData[drone].Group)
                    {
                        case "Heavy Attack Drones":
                            return true;
                        case "Medium Scout Drones":
                            return DroneData[Better].Group != "Heavy Attack Drones";
                        case "Light Scout Drones":
                            return DroneData[Better].Group != "Heavy Attack Drones" && DroneData[Better].Group != "Medium Scout Drones";
                    }
                    return true;
                case "Cruiser":
                    switch (DroneData[drone].Group)
                    {
                        case "Medium Scout Drones":
                            return true;
                        case "Heavy Attack Drones":
                            return DroneData[Better].Group != "Medium Scout Drones";
                        case "Light Scout Drones":
                            return DroneData[Better].Group != "Heavy Attack Drones" && DroneData[Better].Group != "Medium Scout Drones";
                    }
                    return true;
                case "Destroyer":
                case "Frigate":
                    switch (DroneData[drone].Group)
                    {
                        case "Light Scout Drones":
                            return true;
                        case "Medium Scout Drones":
                            return DroneData[Better].Group != "Light Scout Drones";
                        case "Heavy Attack Drones":
                            return DroneData[Better].Group != "Light Scout Drones" && DroneData[Better].Group != "Medium Scout Drones";
                    }
                    return true;
                default:
                    switch (DroneData[drone].Group)
                    {
                        case "Medium Scout Drones":
                            return true;
                        case "Heavy Attack Drones":
                            return DroneData[Better].Group != "Medium Scout Drones";
                        case "Light Scout Drones":
                            return DroneData[Better].Group != "Heavy Attack Drones" && DroneData[Better].Group != "Medium Scout Drones";
                    }
                    return true;
            }
        }

        /// <summary>
        /// Finds the best inactive drone to attack a specific class target, in space or in the drone bay.  Preference is given to drones in space. 
        /// </summary>
        /// <param name="Class">Class of ship, such as "BattleShip", "Cruiser", or "Frigate"</param>
        /// <returns>Drone that is best suited to attack that class.  Drone may be in the bay</returns>
        public Drone BestDrone(string Class)
        {
            switch (Class)
            {
                case "BattleShip":
                case "BattleCruiser":
                    return FindDroneByGroup("Heavy Attack Drones") ?? FindDroneByGroup("Medium Scout Drones") ?? FindDroneByGroup("Light Scout Drones");
                case "Cruiser":
                    return FindDroneByGroup("Medium Scout Drones") ?? FindDroneByGroup("Heavy Attack Drones") ?? FindDroneByGroup("Light Scout Drones");
                case "Destroyer":
                case "Frigate":
                    return FindDroneByGroup("Light Scout Drones") ?? FindDroneByGroup("Medium Scout Drones") ?? FindDroneByGroup("Heavy Attack Drones");
                default:
                    return FindDroneByGroup("Medium Scout Drones") ?? FindDroneByGroup("Heavy Attack Drones") ?? FindDroneByGroup("Light Scout Drones");
            }
        }

        public Drone FindDroneByGroup(string Group)
        {
            if (Drone.AllInSpace.Count(drone => DroneData[drone].Group == Group && !DroneData[drone].Assigned && drone.State != EntityState.Departing && drone.State != EntityState.Departing_2) > 0)
            {
                return Drone.AllInSpace.First(drone => DroneData[drone].Group == Group && !DroneData[drone].Assigned && drone.State != EntityState.Departing && drone.State != EntityState.Departing_2);
            }
            if (BayQueues.ContainsKey(Group))
            {
                if (BayQueues[Group].Count(drone => !DroneData[drone].Assigned && drone.InBay) > 0)
                {
                    return BayQueues[Group].First(drone => !DroneData[drone].Assigned && drone.InBay);
                }
            }
            return null;
        }

    }

    public static class LimitDroneCommands
    {
        public static IEnumerable<Drone> LimitCommand(this IEnumerable<Drone> Drones, int Timeout = 5000)
        {
            foreach (Drone drone in Drones)
            {
                if (DroneControl.Instance.DroneData[drone].LastCommandTime > DateTime.Now)
                {
                    continue;
                }
                DroneControl.Instance.DroneData[drone].LastCommandTime = DateTime.Now.AddMilliseconds(Timeout);
                yield return drone;
            }
        }

    }

    public static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> method)
        {
            foreach (T item in items)
            {
                method(item);
            }
        }
    }

}

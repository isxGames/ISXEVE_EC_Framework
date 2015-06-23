#pragma warning disable 1591
using System.Collections.Generic;
using System.Linq;
using EveCom;
using EveComFramework.Core;
using EveComFramework.KanedaToolkit;
using EveComFramework.Move;

namespace EveComFramework.AutoModule
{
    #region Settings

    /// <summary>
    /// Configuration settings for this AutoModule
    /// </summary>
    public class AutoModuleSettings : Settings
    {
        public bool Enabled = false;
        public bool ActiveHardeners = true;
        public bool ShieldBoosters = true;
        public bool ArmorRepairs = true;
        public bool Cloaks = true;
        public bool GangLinks = true;
        public bool SensorBoosters = true;
        public bool TrackingComputers = true;
        public bool ECCMs = true;
        public bool ECMBursts = true;
        public bool DroneControlUnits = true;
        public bool DroneTrackingModules = true;
        public bool AutoTargeters = true;
        public bool PropulsionModules = false;
        public bool PropulsionModulesAlwaysOn = false;
        public bool PropulsionModulesApproaching = false;
        public bool PropulsionModulesOrbiting = false;

        public int CapActiveHardeners = 30;
        public int CapShieldBoosters = 30;
        public int CapArmorRepairs = 30;
        public int CapCloaks = 30;
        public int CapGangLinks = 30;
        public int CapSensorBoosters = 30;
        public int CapTrackingComputers = 30;
        public int CapECCMs = 30;
        public int CapECMBursts = 30;
        public int CapDroneControlUnits = 30;
        public int CapAutoTargeters = 30;
        public int CapPropulsionModules = 30;
        public int CapDroneTrackingModules = 30;

        public int MaxShieldBoosters = 95;
        public int MaxArmorRepairs = 95;
        public int MinShieldBoosters = 80;
        public int MinArmorRepairs = 80;
        public int MinActiveThreshold = 100;
    }

    #endregion

    /// <summary>
    /// This class manages your ships modules intelligently
    /// </summary>
    public class AutoModule : State
    {
        #region Instantiation

        static AutoModule _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static AutoModule Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AutoModule();
                }
                return _Instance;
            }
        }

        private AutoModule()
        {
            DefaultFrequency = 100;
            if (Config.Enabled) QueueState(Control);
        }

        #endregion

        #region Variables

        /// <summary>
        /// Configuration for this module
        /// </summary>
        public AutoModuleSettings Config = new AutoModuleSettings();

        /// <summary>
        /// Set to true to force automodule to decloak you.  Useful for handling non-covops cloaks.
        /// </summary>
        public bool Decloak = false;

        #endregion

        #region Actions

        /// <summary>
        /// Start this module
        /// </summary>
        public void Start()
        {
            if (Idle)
            {
                QueueState(Control);
            }

        }

        /// <summary>
        /// Stop this module
        /// </summary>
        public void Stop()
        {
            Clear();
        }

        /// <summary>
        /// Starts/stops this module
        /// </summary>
        /// <param name="Val">True=Start</param>
        public void Enabled(bool Val)
        {
            if (Val)
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

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if (!Session.InSpace || !Session.Safe)
            {
                return false;
            }

            if (UndockWarp.Instance != null && !UndockWarp.Instance.Idle && UndockWarp.Instance.CurState.ToString() != "WaitStation") return false;

            #region Cloaks

            if (Config.Cloaks)
            {
                Module cloakingDevice = MyShip.Modules.FirstOrDefault(a => a.GroupID == Group.CloakingDevice && a.IsOnline);
                if (cloakingDevice != null)
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapCloaks || Decloak)
                    {
                        if (cloakingDevice.IsActive && !cloakingDevice.IsDeactivating)
                        {
                            cloakingDevice.Deactivate();
                        }
                    }
                }
            }

            if (MyShip.ToEntity.Cloaked)
            {
                return false;
            }

            if (Config.Cloaks)
            {
                Module cloakingDevice = MyShip.Modules.FirstOrDefault(a => a.GroupID == Group.CloakingDevice && a.IsOnline);
                if (cloakingDevice != null)
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapCloaks && !Decloak && !Entity.All.Any(a => a.Distance < 2000 && a.ID != MyShip.ToEntity.ID))
                    {
                        if (!Entity.All.Any(a => a.IsTargetingMe && !a.Released && !a.Exploded))
                        {
                            if (!cloakingDevice.IsActive && !cloakingDevice.IsActivating && !cloakingDevice.IsDeactivating)
                            {
                                cloakingDevice.Activate();
                            }
                            return false;
                        }
                    }
                }
            }

            if (MyShip.Modules.Any(a => a.GroupID == Group.CloakingDevice && a.IsActive && a.IsOnline)) return false;

            #endregion

            #region Shield Boosters

            if (Config.ShieldBoosters)
            {
                List<Module> shieldBoosters = MyShip.Modules.Where(a => a.GroupID == Group.ShieldBooster && a.IsOnline).ToList();
                if (shieldBoosters.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapShieldBoosters && MyShip.ToEntity.ShieldPct <= Config.MinShieldBoosters)
                    {
                        shieldBoosters.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapShieldBoosters || MyShip.ToEntity.ShieldPct > Config.MaxShieldBoosters)
                    {
                        shieldBoosters.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Armor Repairers

            if (Config.ArmorRepairs)
            {
                List<Module> armorRepairers = MyShip.Modules.Where(a => a.GroupID == Group.ArmorRepairUnit && a.IsOnline).ToList();
                if (armorRepairers.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapArmorRepairs && MyShip.ToEntity.ArmorPct <= Config.MinArmorRepairs)
                    {
                        armorRepairers.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapArmorRepairs || MyShip.ToEntity.ArmorPct > Config.MaxArmorRepairs)
                    {
                        armorRepairers.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Active Hardeners

            if (Config.ActiveHardeners)
            {

                if (MyShip.Modules.Any(a => (a.GroupID == Group.DamageControl) && a.IsOnline))
                {
                    MyShip.Modules.Where(a => a.GroupID == Group.DamageControl && a.IsOnline && !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                }

                List<Module> shieldHardeners = MyShip.Modules.Where(a => a.GroupID == Group.ShieldHardener && a.IsOnline).ToList();
                if (shieldHardeners.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapActiveHardeners && MyShip.ToEntity.ShieldPct <= Config.MinActiveThreshold)
                    {
                        shieldHardeners.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if (((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapActiveHardeners || MyShip.ToEntity.ShieldPct > Config.MinActiveThreshold))
                    {
                        shieldHardeners.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                }

                List<Module> armorHardeners = MyShip.Modules.Where(a => (a.GroupID == Group.ArmorHardener || a.GroupID == Group.ArmorResistanceShiftHardener) && a.IsOnline).ToList();
                if (armorHardeners.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapActiveHardeners && MyShip.ToEntity.ArmorPct <= Config.MinActiveThreshold)
                    {
                        armorHardeners.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapActiveHardeners || MyShip.ToEntity.ArmorPct > Config.MinActiveThreshold)
                    {
                        armorHardeners.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Gang Links

            if (Config.GangLinks)
            {
                List<Module> gangLinks = MyShip.Modules.Where(a => a.GroupID == Group.GangCoordinator && a.TypeID != 11014 && a.IsOnline).ToList();
                if (gangLinks.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapGangLinks)
                    {
                        gangLinks.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapGangLinks)
                    {
                        gangLinks.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Sensor Boosters

            if (Config.SensorBoosters)
            {
                List<Module> sensorBoosters = MyShip.Modules.Where(a => a.GroupID == Group.SensorBooster && a.IsOnline).ToList();
                if (sensorBoosters.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapSensorBoosters)
                    {
                        sensorBoosters.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapSensorBoosters)
                    {
                        sensorBoosters.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Tracking Computers

            if (Config.TrackingComputers)
            {
                List<Module> trackingComputers = MyShip.Modules.Where(a => a.GroupID == Group.TrackingComputer && a.IsOnline).ToList();
                if (trackingComputers.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapTrackingComputers)
                    {
                        trackingComputers.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapTrackingComputers)
                    {
                        trackingComputers.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Drone Tracking Modules

            if (Config.DroneTrackingModules)
            {
                List<Module> droneTrackingModules = MyShip.Modules.Where(a => a.GroupID == Group.DroneTrackingModules && a.IsOnline).ToList();
                if (droneTrackingModules.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapDroneTrackingModules)
                    {
                        droneTrackingModules.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapDroneTrackingModules)
                    {
                        droneTrackingModules.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region ECCMs

            if (Config.ECCMs)
            {
                List<Module> ECCM = MyShip.Modules.Where(a => a.GroupID == Group.ECCM && a.IsOnline).ToList();
                if (ECCM.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapECCMs)
                    {
                        ECCM.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapECCMs)
                    {
                        ECCM.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region ECMBursts

            if (Config.ECMBursts)
            {
                List<Module> ECMBursts = MyShip.Modules.Where(a => a.GroupID == Group.ECMBurst && a.IsOnline).ToList();
                if (ECMBursts.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapECMBursts)
                    {
                        ECMBursts.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapECMBursts)
                    {
                        ECMBursts.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Drone Control Units

            if (Config.DroneControlUnits)
            {
                List<Module> droneControlUnits = MyShip.Modules.Where(a => a.GroupID == Group.DroneControlUnit && a.IsOnline).ToList();
                if (droneControlUnits.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapDroneControlUnits)
                    {
                        droneControlUnits.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapDroneControlUnits)
                    {
                        droneControlUnits.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region AutoTargeters

            if (Config.AutoTargeters && MyShip.ToEntity.Mode != EntityMode.Warping)
            {
                List<Module> autoTargeters = MyShip.Modules.Where(a => a.GroupID == Group.AutomatedTargetingSystem && a.IsOnline).ToList();
                if (autoTargeters.Any())
                {
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapAutoTargeters)
                    {
                        autoTargeters.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    }
                    if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapAutoTargeters)
                    {
                        autoTargeters.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    }
                }
            }

            #endregion

            #region Propulsion Modules

            List<Module> propulsionModules = MyShip.Modules.Where(a => a.GroupID == Group.PropulsionModule && a.IsOnline).ToList();

            if (MyShip.ToEntity.Mode == EntityMode.Warping)
            {
                propulsionModules.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                return false;
            }

            if (Config.PropulsionModules && propulsionModules.Any())
            {
                if ((MyShip.Capacitor / MyShip.MaxCapacitor * 100) > Config.CapPropulsionModules &&
                        ((Config.PropulsionModulesApproaching && MyShip.ToEntity.Mode == EntityMode.Approaching) ||
                        (Config.PropulsionModulesOrbiting && MyShip.ToEntity.Mode == EntityMode.Orbiting) ||
                        Config.PropulsionModulesAlwaysOn))
                {
                    propulsionModules.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                }
                if ((!Config.PropulsionModulesAlwaysOn && (MyShip.Capacitor / MyShip.MaxCapacitor * 100) < Config.CapPropulsionModules) ||
                   (!Config.PropulsionModulesAlwaysOn || MyShip.ToEntity.Mode == EntityMode.Stopped || MyShip.ToEntity.Mode == EntityMode.Aligned))
                {
                    propulsionModules.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                }
            }

            #endregion

            return false;
        }

        #endregion
    }

    #region Settings

    /// <summary>
    /// Configuration settings for this Module
    /// </summary>
    public class InstawarpSettings : Settings
    {
        public bool Enabled = false;
    }

    #endregion

    /// <summary>
    /// This class manages one pulse of the first available propulsion module per session.
    /// A new session starts on every solar system change or undock.
    /// </summary>
    public class InstaWarp : State
    {
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
            if (Config.Enabled) QueueState(Control);
        }

        #endregion

        #region Actions

        /// <summary>
        /// Starts/stops this module
        /// </summary>
        /// <param name="Val">True=Start</param>
        public void Enabled(bool Val)
        {
            if (Val)
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

        #endregion

        #region Variables

        /// <summary>
        /// The config for this class
        /// </summary>
        public InstawarpSettings Config = new InstawarpSettings();

        bool CycleComplete = false;

        #endregion

        #region States

        bool Control(object[] Params)
        {
            if (!Session.InSpace || !Session.Safe)
            {
                return false;
            }

            if (Move.UndockWarp.Instance != null && !EveComFramework.Move.UndockWarp.Instance.Idle && EveComFramework.Move.UndockWarp.Instance.CurState.ToString() != "WaitStation") return false;

            if (MyShip.ToEntity.Mode != EntityMode.Warping)
            {
                CycleComplete = false;
                return false;
            }

            if (CycleComplete) return false;

            #region Propulsion Modules

            List<Module> propulsionModules = MyShip.Modules.Where(a => a.GroupID == Group.PropulsionModule && a.IsOnline).ToList();
            if (propulsionModules.Any())
            {
                if (!CycleComplete && propulsionModules.Any(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating))
                {
                    propulsionModules.Where(a => !a.IsActive && !a.IsActivating && !a.IsDeactivating).ForEach(m => m.Activate());
                    return false;
                }
                if (propulsionModules.Any(a => a.IsActive && !a.IsDeactivating))
                {
                    propulsionModules.Where(a => a.IsActive && !a.IsDeactivating).ForEach(m => m.Deactivate());
                    CycleComplete = true;
                }
            }

            #endregion

            return false;
        }
        #endregion
    }
}

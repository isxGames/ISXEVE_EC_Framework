using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EveComFramework.AutoModule;
using EveComFramework.Move;
using EveComFramework.Optimizer;
using EveComFramework.Comms;

namespace EveComFramework.Core
{
    public partial class Configuration : UserControl
    {
        public Configuration()
        {
            InitializeComponent();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                AutoModule.AutoModule AutoModuleInstance = AutoModule.AutoModule.Instance;
                AutoModule.AutoModuleSettings AutoModuleConfig = AutoModule.AutoModule.Instance.Config;
                UndockWarp UndockWarp = UndockWarp.Instance;
                UndockWarpSettings UndockWarpConfig = UndockWarp.Instance.Config;
                InstaWarp InstaWarp = InstaWarp.Instance;
                MoveSettings MoveConfig = EveComFramework.Move.Move.Instance.Config;
                OptimizerSettings OptimizerConfig = Optimizer.Optimizer.Instance.Config;
                InstawarpSettings InstaWarpConfig = InstaWarp.Instance.Config;
                Comms.Comms CommsInstance = Comms.Comms.Instance;
                CommsSettings CommsConfig = Comms.Comms.Instance.Config;
                SimpleDrone.SimpleDrone DroneInstance = SimpleDrone.SimpleDrone.Instance;
                SimpleDrone.LocalSettings DroneConfig = SimpleDrone.SimpleDrone.Instance.Config;

                #region AutoModule

                checkAutoModule.Checked = AutoModuleConfig.Enabled;
                checkAutoModule.CheckedChanged += (s, a) => { AutoModuleConfig.Enabled = checkAutoModule.Checked; AutoModuleInstance.Enabled(AutoModuleConfig.Enabled); AutoModuleConfig.Save(); };

                checkShieldBoosters.Checked = AutoModuleConfig.ShieldBoosters;
                checkShieldBoosters.CheckedChanged += (s, a) => { AutoModuleConfig.ShieldBoosters = checkShieldBoosters.Checked; AutoModuleConfig.Save(); };
                numericShieldCap.Value = AutoModuleConfig.CapShieldBoosters;
                numericShieldCap.ValueChanged += (s, a) => { AutoModuleConfig.CapShieldBoosters = (int)numericShieldCap.Value; AutoModuleConfig.Save(); };
                numericShieldMin.Value = AutoModuleConfig.MinShieldBoosters;
                numericShieldMin.ValueChanged += (s, a) => { AutoModuleConfig.MinShieldBoosters = (int)numericShieldMin.Value; AutoModuleConfig.Save(); };
                numericShieldMax.Value = AutoModuleConfig.MaxShieldBoosters;
                numericShieldMax.ValueChanged += (s, a) => { AutoModuleConfig.MaxShieldBoosters = (int)numericShieldMax.Value; AutoModuleConfig.Save(); };

                checkArmorRepairers.Checked = AutoModuleConfig.ArmorRepairs;
                checkArmorRepairers.CheckedChanged += (s, a) => { AutoModuleConfig.ArmorRepairs = checkArmorRepairers.Checked; AutoModuleConfig.Save(); };
                numericArmorCap.Value = AutoModuleConfig.CapArmorRepairs;
                numericArmorCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericArmorCap.Value; AutoModuleConfig.Save(); };
                numericArmorMin.Value = AutoModuleConfig.MinArmorRepairs;
                numericArmorMin.ValueChanged += (s, a) => { AutoModuleConfig.MinArmorRepairs = (int)numericArmorMin.Value; AutoModuleConfig.Save(); };
                numericArmorMax.Value = AutoModuleConfig.MaxArmorRepairs;
                numericArmorMax.ValueChanged += (s, a) => { AutoModuleConfig.MaxArmorRepairs = (int)numericArmorMax.Value; AutoModuleConfig.Save(); };

                checkActiveHardeners.Checked = AutoModuleConfig.ActiveHardeners;
                checkActiveHardeners.CheckedChanged += (s, a) => { AutoModuleConfig.ActiveHardeners = checkActiveHardeners.Checked; AutoModuleConfig.Save(); };
                numericActiveHardenerCap.Value = AutoModuleConfig.CapActiveHardeners;
                numericActiveHardenerCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericActiveHardenerCap.Value; AutoModuleConfig.Save(); };

                checkCloaks.Checked = AutoModuleConfig.Cloaks;
                checkCloaks.CheckedChanged += (s, a) => { AutoModuleConfig.Cloaks = checkCloaks.Checked; AutoModuleConfig.Save(); };
                numericCloakCap.Value = AutoModuleConfig.CapCloaks;
                numericCloakCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericCloakCap.Value; AutoModuleConfig.Save(); };

                checkGangLinks.Checked = AutoModuleConfig.GangLinks;
                checkGangLinks.CheckedChanged += (s, a) => { AutoModuleConfig.GangLinks = checkGangLinks.Checked; AutoModuleConfig.Save(); };
                numericGangLinkCap.Value = AutoModuleConfig.CapGangLinks;
                numericGangLinkCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericGangLinkCap.Value; AutoModuleConfig.Save(); };

                checkSensorBoosters.Checked = AutoModuleConfig.SensorBoosters;
                checkSensorBoosters.CheckedChanged += (s, a) => { AutoModuleConfig.SensorBoosters = checkSensorBoosters.Checked; AutoModuleConfig.Save(); };
                numericSensorBoosterCap.Value = AutoModuleConfig.CapSensorBoosters;
                numericSensorBoosterCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericSensorBoosterCap.Value; AutoModuleConfig.Save(); };

                checkTrackingComputers.Checked = AutoModuleConfig.TrackingComputers;
                checkTrackingComputers.CheckedChanged += (s, a) => { AutoModuleConfig.TrackingComputers = checkTrackingComputers.Checked; AutoModuleConfig.Save(); };
                numericTrackingComputerCap.Value = AutoModuleConfig.CapTrackingComputers;
                numericTrackingComputerCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericTrackingComputerCap.Value; AutoModuleConfig.Save(); };

                checkECCMs.Checked = AutoModuleConfig.ECCMs;
                checkECCMs.CheckedChanged += (s, a) => { AutoModuleConfig.ECCMs = checkECCMs.Checked; AutoModuleConfig.Save(); };
                numericECCMCap.Value = AutoModuleConfig.CapECCMs;
                numericECCMCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericECCMCap.Value; AutoModuleConfig.Save(); };

                checkDroneControlUnits.Checked = AutoModuleConfig.DroneControlUnits;
                checkDroneControlUnits.CheckedChanged += (s, a) => { AutoModuleConfig.DroneControlUnits = checkDroneControlUnits.Checked; AutoModuleConfig.Save(); };
                numericDroneControlUnitCap.Value = AutoModuleConfig.CapDroneControlUnits;
                numericDroneControlUnitCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericDroneControlUnitCap.Value; AutoModuleConfig.Save(); };

                checkPropulsionModules.Checked = AutoModuleConfig.PropulsionModules;
                checkPropulsionModules.CheckedChanged += (s, a) => { AutoModuleConfig.PropulsionModules = checkPropulsionModules.Checked; AutoModuleConfig.Save(); };
                numericPropulsionModuleCap.Value = AutoModuleConfig.CapPropulsionModules;
                numericPropulsionModuleCap.ValueChanged += (s, a) => { AutoModuleConfig.CapArmorRepairs = (int)numericPropulsionModuleCap.Value; AutoModuleConfig.Save(); };
                checkActivateApproaching.Checked = AutoModuleConfig.PropulsionModulesApproaching;
                checkActivateApproaching.CheckedChanged += (s, a) => { AutoModuleConfig.PropulsionModulesApproaching = checkActivateApproaching.Checked; AutoModuleConfig.Save(); };
                checkActivateOrbiting.Checked = AutoModuleConfig.PropulsionModulesOrbiting;
                checkActivateOrbiting.CheckedChanged += (s, a) => { AutoModuleConfig.PropulsionModulesOrbiting = checkActivateOrbiting.Checked; AutoModuleConfig.Save(); };
                checkAlwaysActive.Checked = AutoModuleConfig.PropulsionModulesAlwaysOn;
                checkAlwaysActive.CheckedChanged += (s, a) => { AutoModuleConfig.PropulsionModulesAlwaysOn = checkAlwaysActive.Checked; AutoModuleConfig.Save(); };

                #endregion

                #region UndockWarp

                checkUndockWarp.Checked = UndockWarpConfig.Enabled;
                checkUndockWarp.CheckedChanged += (s, a) => { UndockWarpConfig.Enabled = checkUndockWarp.Checked; UndockWarp.Enabled(UndockWarpConfig.Enabled); UndockWarpConfig.Save(); };
                textUndockWarp.Text = UndockWarpConfig.Substring;
                textUndockWarp.TextChanged += (s, a) => { UndockWarpConfig.Substring = textUndockWarp.Text; UndockWarpConfig.Save(); };

                #endregion

                #region InstaWarp

                checkInstaWarp.Checked = InstaWarpConfig.Enabled;
                checkInstaWarp.CheckedChanged += (s, a) => { InstaWarpConfig.Enabled = checkInstaWarp.Checked; InstaWarp.Enabled(InstaWarpConfig.Enabled); InstaWarpConfig.Save(); };

                #endregion

                #region Move

                checkWarpCollision.Checked = MoveConfig.WarpCollisionPrevention;
                checkWarpCollision.CheckedChanged += (s, a) => { MoveConfig.WarpCollisionPrevention = checkWarpCollision.Checked; MoveConfig.Save(); };
                numericWarpTrigger.Value = MoveConfig.WarpCollisionTrigger;
                numericWarpTrigger.ValueChanged += (s, a) => { MoveConfig.WarpCollisionTrigger = numericWarpTrigger.Value; MoveConfig.Save(); };
                numericWarpOrbit.Value = MoveConfig.WarpCollisionOrbit;
                numericWarpOrbit.ValueChanged += (s, a) => { MoveConfig.WarpCollisionOrbit = numericWarpOrbit.Value; MoveConfig.Save(); };

                checkApproachCollision.Checked = MoveConfig.ApproachCollisionPrevention;
                checkApproachCollision.CheckedChanged += (s, a) => { MoveConfig.ApproachCollisionPrevention = checkApproachCollision.Checked; MoveConfig.Save(); };
                numericApproachTrigger.Value = MoveConfig.ApproachCollisionTrigger;
                numericApproachTrigger.ValueChanged += (s, a) => { MoveConfig.ApproachCollisionTrigger = numericApproachTrigger.Value; MoveConfig.Save(); };
                numericApproachOrbit.Value = MoveConfig.ApproachCollisionOrbit;
                numericApproachOrbit.ValueChanged += (s, a) => { MoveConfig.ApproachCollisionOrbit = numericApproachOrbit.Value; MoveConfig.Save(); };

                checkOrbitCollision.Checked = MoveConfig.OrbitCollisionPrevention;
                checkOrbitCollision.CheckedChanged += (s, a) => { MoveConfig.OrbitCollisionPrevention = checkOrbitCollision.Checked; MoveConfig.Save(); };
                numericOrbitTrigger.Value = MoveConfig.OrbitCollisionTrigger;
                numericOrbitTrigger.ValueChanged += (s, a) => { MoveConfig.OrbitCollisionTrigger = numericOrbitTrigger.Value; MoveConfig.Save(); };
                numericOrbitOrbit.Value = MoveConfig.OrbitCollisionOrbit;
                numericOrbitOrbit.ValueChanged += (s, a) => { MoveConfig.OrbitCollisionOrbit = numericOrbitOrbit.Value; MoveConfig.Save(); };

                #endregion

                #region Optimizer

                checkDisable3D.Checked = !OptimizerConfig.Enable3D;
                checkDisable3D.CheckedChanged += (s, a) => { OptimizerConfig.Enable3D = !checkDisable3D.Checked; OptimizerConfig.Save(); };
                numericMemoryMax.Value = OptimizerConfig.MaxMemorySize;
                numericMemoryMax.ValueChanged += (s, a) => { OptimizerConfig.MaxMemorySize = numericMemoryMax.Value; OptimizerConfig.Save(); };

                #endregion

                #region IRC

                checkUseIRC.Checked = CommsConfig.UseIRC;
                checkUseIRC.CheckedChanged += (s, a) => { CommsConfig.UseIRC = checkUseIRC.Checked; CommsConfig.Save(); };
                textServer.Text = CommsConfig.Server;
                textServer.TextChanged += (s, a) => { CommsConfig.Server = textServer.Text; CommsConfig.Save(); };
                numericPort.Value = CommsConfig.Port;
                numericPort.ValueChanged += (s, a) => { CommsConfig.Port = (int)Math.Floor(numericPort.Value); CommsConfig.Save(); };
                textSendTo.Text = CommsConfig.SendTo;
                textSendTo.TextChanged += (s, a) => { CommsConfig.SendTo = textSendTo.Text; CommsConfig.Save(); };
                checkLocal.Checked = CommsConfig.Local;
                checkLocal.CheckedChanged += (s, a) => { CommsConfig.Local = checkLocal.Checked; CommsConfig.Save(); };
                checkNPC.Checked = CommsConfig.NPC;
                checkNPC.CheckedChanged += (s, a) => { CommsConfig.NPC = checkNPC.Checked; CommsConfig.Save(); };
                checkWallet.Checked = CommsConfig.Wallet;
                checkWallet.CheckedChanged += (s, a) => { CommsConfig.Wallet = checkWallet.Checked; CommsConfig.Save(); };

                #endregion

                #region Drone Control

                numericDroneTargetSlots.Value = DroneConfig.TargetSlots;
                numericDroneTargetSlots.ValueChanged += (s, a) => { DroneConfig.TargetSlots = (int)Math.Floor(numericDroneTargetSlots.Value); DroneConfig.Save(); };
                checkDronePrivateTargets.Checked = DroneConfig.PrivateTargets;
                checkDronePrivateTargets.CheckedChanged += (s, a) => { DroneConfig.PrivateTargets = checkDronePrivateTargets.Checked; DroneConfig.Save(); };
                switch (DroneConfig.Mode)
                {
                    case SimpleDrone.Mode.AFKHeavy:
                        comboDroneMode.SelectedItem = "AFK Heavy";
                        break;
                    case SimpleDrone.Mode.PointDefense:
                        comboDroneMode.SelectedItem = "Point Defense";
                        break;
                    case SimpleDrone.Mode.Sentry:
                        comboDroneMode.SelectedItem = "Sentry";
                        break;
                }
                comboDroneMode.SelectedIndexChanged += (s, a) =>
                {
                    switch (comboDroneMode.SelectedItem.ToString())
                    {
                        case "AFK Heavy":
                            DroneConfig.Mode = SimpleDrone.Mode.AFKHeavy;
                            break;
                        case "Point Defense":
                            DroneConfig.Mode = SimpleDrone.Mode.PointDefense;
                            break;
                        case "Sentry":
                            DroneConfig.Mode = SimpleDrone.Mode.Sentry;
                            break;
                    }
                    DroneConfig.Save();
                };
                
                #endregion
            }

        }

    }
}

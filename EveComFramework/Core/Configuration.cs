using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EveComFramework.AutoModule;

namespace EveComFramework.Core
{
    public partial class Configuration : UserControl
    {
        AutoModuleSettings AutoModuleConfig;

        public Configuration()
        {
            InitializeComponent();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                AutoModuleConfig = AutoModule.AutoModule.Instance.Config;

                #region AutoModule

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
            }

        }



    }
}

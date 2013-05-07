using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveComFramework.AutoModule.UI
{
    public partial class AutoModule : Form
    {
        internal AutoModuleSettings Config = new AutoModuleSettings();
        
        public AutoModule()
        {
            InitializeComponent();
        }

        private void AutoModule_Load(object sender, EventArgs e)
        {
            MinThreshold.Hide();
            MinThresholdLabel.Hide();
            MaxThreshold.Hide();
            MaxThresholdLabel.Hide();

            foreach (ListViewItem i in Modules.Items)
            {
                switch (i.Text)
                {
                    case "Shield Boosters":
                        i.Checked = true;
                        break;
                    case "Armor Repairers":
                        i.Checked = true;
                        break;
                    case "Active Hardeners":
                        i.Checked = true;
                        break;
                    case "Cloaks":
                        i.Checked = true;
                        break;
                    case "Gang Links":
                        i.Checked = true;
                        break;
                    case "Sensor Boosters":
                        i.Checked = true;
                        break;
                    case "Tracking Computers":
                        i.Checked = true;
                        break;
                    case "ECCMs":
                        i.Checked = true;
                        break;
                    case "Drone Control Units":
                        i.Checked = true;
                        break;
                    case "Propulsion Modules":
                        i.Checked = true;
                        break;
                }
            }

        }

        private void Modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Modules.SelectedItems.Count == 0)
            {
                groupBox1.Hide();
                return;
            }
            groupBox1.Show();
            switch (Modules.SelectedItems[0].Text)
            {
                case "Shield Boosters" :
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapShieldBoosters + "% capacitor";
                    MaxThresholdLabel.Text = "Deactivate if above " + Config.MaxShieldBoosters + "% shields";
                    MinThresholdLabel.Text = "Activate at or below " + Config.MinShieldBoosters + "% shields";
                    CapacitorThreshold.Value = Config.CapShieldBoosters;
                    MaxThreshold.Value = Config.MaxShieldBoosters;
                    MinThreshold.Value = Config.MinShieldBoosters;
                    MaxThreshold.Show();
                    MaxThresholdLabel.Show();
                    MinThreshold.Show();
                    MinThresholdLabel.Show();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Armor Repairers" :
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapArmorRepairs + "% capacitor";
                    MaxThresholdLabel.Text = "Deactivate if above " + Config.MaxArmorRepairs + "% armor";
                    MinThresholdLabel.Text = "Activate at or below " + Config.MinArmorRepairs + "% armor";
                    CapacitorThreshold.Value = Config.CapArmorRepairs;
                    MaxThreshold.Value = Config.MaxArmorRepairs;
                    MinThreshold.Value = Config.MinArmorRepairs;
                    MaxThreshold.Show();
                    MaxThresholdLabel.Show();
                    MinThreshold.Show();
                    MinThresholdLabel.Show();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Active Hardeners":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapActiveHardeners + "% capacitor";
                    CapacitorThreshold.Value = Config.CapActiveHardeners;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Cloaks":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapCloaks + "% capacitor";
                    CapacitorThreshold.Value = Config.CapCloaks;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Gang Links":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapGangLinks + "% capacitor";
                    CapacitorThreshold.Value = Config.CapGangLinks;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Sensor Boosters":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapSensorBoosters + "% capacitor";
                    CapacitorThreshold.Value = Config.CapSensorBoosters;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Tracking Computers":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapTrackingComputers + "% capacitor";
                    CapacitorThreshold.Value = Config.CapTrackingComputers;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "ECCMs":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapECCMs + "% capacitor";
                    CapacitorThreshold.Value = Config.CapECCMs;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Drone Control Units":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapDroneControlUnits + "% capacitor";
                    CapacitorThreshold.Value = Config.CapDroneControlUnits;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Hide();
                    ActivateApproaching.Hide();
                    ActivateOrbiting.Hide();
                    break;
                case "Propulsion Modules":
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapPropulsionModules + "% capacitor";
                    CapacitorThreshold.Value = Config.CapPropulsionModules;
                    MaxThreshold.Hide();
                    MaxThresholdLabel.Hide();
                    MinThreshold.Hide();
                    MinThresholdLabel.Hide();
                    AlwaysActive.Checked = Config.PropulsionModulesAlwaysOn;
                    ActivateApproaching.Checked = Config.PropulsionModulesApproaching;
                    ActivateOrbiting.Checked = Config.PropulsionModulesOrbiting;
                    AlwaysActive.Show();
                    ActivateApproaching.Show();
                    ActivateOrbiting.Show();
                    break;
            }


        }

        private void CapacitorThreshold_Scroll(object sender, EventArgs e)
        {
            if (Modules.SelectedItems.Count == 0)
            {
                groupBox1.Hide();
                return;
            }
            groupBox1.Show();
            switch (Modules.SelectedItems[0].Text)
            {
                case "Shield Boosters":
                    Config.CapShieldBoosters = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapShieldBoosters + "% capacitor";
                    break;
                case "Armor Repairers":
                    Config.CapArmorRepairs = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapArmorRepairs + "% capacitor";
                    break;
                case "Active Hardeners":
                    Config.CapActiveHardeners = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapActiveHardeners + "% capacitor";
                    break;
                case "Cloaks":
                    Config.CapCloaks = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapCloaks + "% capacitor";
                    break;
                case "Gang Links":
                    Config.CapGangLinks = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapGangLinks + "% capacitor";
                    break;
                case "Sensor Boosters":
                    Config.CapSensorBoosters = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapSensorBoosters + "% capacitor";
                    break;
                case "Tracking Computers":
                    Config.CapTrackingComputers = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapTrackingComputers + "% capacitor";
                    break;
                case "ECCMs":
                    Config.CapECCMs = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapECCMs + "% capacitor";
                    break;
                case "Drone Control Units":
                    Config.CapDroneControlUnits = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapDroneControlUnits + "% capacitor";
                    break;
                case "Propulsion Modules":
                    Config.CapPropulsionModules = CapacitorThreshold.Value;
                    Config.Save();
                    CapacitorThresholdLabel.Text = "Only activate if above " + Config.CapPropulsionModules + "% capacitor";
                    break;
            }
        }

        private void MinThreshold_Scroll(object sender, EventArgs e)
        {
            if (Modules.SelectedItems.Count == 0)
            {
                groupBox1.Hide();
                return;
            }
            groupBox1.Show();
            switch (Modules.SelectedItems[0].Text)
            {
                case "Shield Boosters":
                    Config.MinShieldBoosters = MinThreshold.Value;
                    Config.Save();
                    MinThresholdLabel.Text = "Activate at or below " + Config.MinShieldBoosters + "% shield";
                    break;
                case "Armor Repairers":
                    Config.MinArmorRepairs = MinThreshold.Value;
                    Config.Save();
                    MinThresholdLabel.Text = "Activate at or below " + Config.MinArmorRepairs + "% armor";
                    break;
            }
        }

        private void MaxThreshold_Scroll(object sender, EventArgs e)
        {
            if (Modules.SelectedItems.Count == 0)
            {
                groupBox1.Hide();
                return;
            }
            groupBox1.Show();
            switch (Modules.SelectedItems[0].Text)
            {
                case "Shield Boosters":
                    Config.MaxShieldBoosters = MaxThreshold.Value;
                    Config.Save();
                    MaxThresholdLabel.Text = "Deactivate if above " + Config.MaxShieldBoosters + "% shield";
                    break;
                case "Armor Repairers":
                    Config.MaxArmorRepairs = MaxThreshold.Value;
                    Config.Save();
                    MaxThresholdLabel.Text = "Deactivate if above " + Config.MaxArmorRepairs + "% armor";
                    break;
            }
        }

        private void AlwaysActive_CheckedChanged(object sender, EventArgs e)
        {
            Config.PropulsionModulesAlwaysOn = AlwaysActive.Checked;
            Config.Save();
        }

        private void ActivateApproaching_CheckedChanged(object sender, EventArgs e)
        {
            Config.PropulsionModulesApproaching = ActivateApproaching.Checked;
            Config.Save();
        }

        private void ActivateOrbiting_CheckedChanged(object sender, EventArgs e)
        {
            Config.PropulsionModulesOrbiting = ActivateOrbiting.Checked;
            Config.Save();
        }

        private void Modules_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            foreach (ListViewItem checkbox in Modules.CheckedItems)
            {
                switch (checkbox.Text)
                {
                    case "Shield Boosters":
                        Config.ShieldBoosters = true;
                        break;
                    case "Armor Repairers":
                        Config.ArmorRepairs = true;
                        break;
                    case "Active Hardeners":
                        Config.ActiveHardeners = true;
                        break;
                    case "Cloaks":
                        Config.Cloaks = true;
                        break;
                    case "Gang Links":
                        Config.GangLinks = true;
                        break;
                    case "Sensor Boosters":
                        Config.SensorBoosters = true;
                        break;
                    case "Tracking Computers":
                        Config.TrackingComputers = true;
                        break;
                    case "ECCMs":
                        Config.ECCMs = true;
                        break;
                    case "Drone Control Units":
                        Config.DroneControlUnits = true;
                        break;
                    case "Propulsion Modules":
                        Config.PropulsionModules = true;
                        break;
                }
            }
            Config.Save();
        }

    }
}

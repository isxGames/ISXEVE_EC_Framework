using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EveCom;
using InnerSpaceAPI;

namespace EveComFramework.Security.UI
{
    internal partial class Security : Form
    {
        internal SecuritySettings Config = new SecuritySettings();
        string ActiveTrigger;
        List<string> Bookmarks;

        public Security()
        {
            InitializeComponent();
            HookupMouseEnterEvents(this);
        }

        #region Mouseover Controller

        private void HookupMouseEnterEvents(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                childControl.MouseEnter += new EventHandler(Help_MouseEnter);

                // Recurse on this child to get all of its descendents.
                HookupMouseEnterEvents(childControl);
            }
        }

        private void Help_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                Control senderCtl = sender as Control;
                if (!string.IsNullOrEmpty((string)senderCtl.Tag))
                {
                    lblHelpText.Text = (string)senderCtl.Tag;
                }
                else
                {
                    lblHelpText.Text = "Mouseover an element for help";
                }
            }
        }
        
        #endregion

        private void Security_Load(object sender, EventArgs e)
        {

            for (int i = 0; i <= (FleeTypes.Items.Count - 1); i++)
            {
                switch (FleeTypes.Items[i].ToString())
                {
                    case "Flee to closest station in system":
                        if (Config.Types.Contains(FleeType.NearestStation))
                        {
                            FleeTypes.SetItemChecked(i, true);
                        }
                        break;
                    case "Flee to secure bookmark":
                        if (Config.Types.Contains(FleeType.SecureBookmark))
                        {
                            FleeTypes.SetItemChecked(i, true);
                        }
                        break;
                    case "Cycle safe bookmarks":
                        if (Config.Types.Contains(FleeType.SafeBookmarks))
                        {
                            FleeTypes.SetItemChecked(i, true);
                        }
                        break;
                }
            }

            for (int i = 0; i <= (Triggers.Items.Count - 1); i++)
            {
                switch (Triggers.Items[i].ToString())
                {
                    case "In a pod":
                        if (Config.Triggers.Contains(FleeTrigger.Pod))
                        {
                            Triggers.SetItemChecked(i, true);
                        }
                        break;
                    case "Negative standing pilot in local":
                        if (Config.Triggers.Contains(FleeTrigger.NegativeStanding))
                        {
                            Triggers.SetItemChecked(i, true);
                        }
                        break;
                    case "Neutral standing pilot in local":
                        if (Config.Triggers.Contains(FleeTrigger.NeutralStanding))
                        {
                            Triggers.SetItemChecked(i, true);
                        }
                        break;
                    case "Targeted by another player":
                        if (Config.Triggers.Contains(FleeTrigger.Targeted))
                        {
                            Triggers.SetItemChecked(i, true);
                        }
                        break;
                    case "Capacitor low":
                        if (Config.Triggers.Contains(FleeTrigger.CapacitorLow))
                        {
                            Triggers.SetItemChecked(i, true);
                        }
                        break;
                    case "Shield low":
                        if (Config.Triggers.Contains(FleeTrigger.ShieldLow))
                        {
                            Triggers.SetItemChecked(i, true);
                        }
                        break;
                    case "Armor low":
                        if (Config.Triggers.Contains(FleeTrigger.ArmorLow))
                        {
                            Triggers.SetItemChecked(i, true);
                        }
                        break;
                }
            }

            SafeSubstring.Text = Config.SafeSubstring;
            SecureBookmark.Text = Config.SecureBookmark;
            FleeWait.Value = Config.FleeWait;
            lblFleeWait.Text = String.Format("Wait {0} minutes after flee", FleeWait.Value);
        }

        private void SecureBookmark_TextChanged(object sender, EventArgs e)
        {
            Config.SecureBookmark = SecureBookmark.Text;
            Config.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FleeTypes.SelectedIndex > 0)
            {
                FleeTypes.Items.Insert(FleeTypes.SelectedIndex - 1, FleeTypes.SelectedItem);
                FleeTypes.SelectedIndex = (FleeTypes.SelectedIndex - 2);
                FleeTypes.Items.RemoveAt(FleeTypes.SelectedIndex + 2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (FleeTypes.SelectedIndex != -1 && FleeTypes.SelectedIndex < (FleeTypes.Items.Count - 1))
            {
                int IndexToRemove = FleeTypes.SelectedIndex;
                FleeTypes.Items.Insert(FleeTypes.SelectedIndex + 2, FleeTypes.SelectedItem);
                FleeTypes.SelectedIndex = (FleeTypes.SelectedIndex + 2);
                FleeTypes.Items.RemoveAt(IndexToRemove);
            }
        }

        private void SafeSubstring_TextChanged(object sender, EventArgs e)
        {
            Config.SafeSubstring = SafeSubstring.Text;
            Config.Save();
        }

        private void Triggers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Triggers.SelectedItem == null)
            {
                StandingGroup.Hide();
                ThresholdGroup.Hide();
                return;
            }
            switch (Triggers.SelectedItem.ToString())
            {
                case "In a pod":
                    StandingGroup.Hide();
                    ThresholdGroup.Hide();
                    break;
                case "Negative standing pilot in local":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();                   
                    IncludeCorpMembers.Checked = Config.NegativeCorp;
                    IncludeAllianceMembers.Checked = Config.NegativeAlliance;
                    IncludeFleetMembers.Checked = Config.NegativeFleet;
                    break;
                case "Neutral standing pilot in local":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();
                    IncludeCorpMembers.Checked = Config.NeutralCorp;
                    IncludeAllianceMembers.Checked = Config.NeutralAlliance;
                    IncludeFleetMembers.Checked = Config.NeutralFleet;
                    break;
                case "Targeted by another player":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();                   
                    IncludeCorpMembers.Checked = Config.TargetCorp;
                    IncludeAllianceMembers.Checked = Config.TargetAlliance;
                    IncludeFleetMembers.Checked = Config.TargetFleet;
                    break;
                case "Capacitor low":
                    Threshold.Value = Config.CapThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "Shield low":
                    Threshold.Value = Config.ShieldThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "Armor low":
                    Threshold.Value = Config.ArmorThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Armor";
                    StandingGroup.Hide();                   
                    ThresholdGroup.Show();
                    break;
            }
            ActiveTrigger = Triggers.SelectedItem.ToString();
        }


        private void Triggers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<FleeTrigger> build = new List<FleeTrigger>();
            foreach (string i in Triggers.CheckedItems)
            {
                switch (i)
                {
                    case "In a pod":
                        build.Add(FleeTrigger.Pod);
                        break;
                    case "Negative standing pilot in local":
                        build.Add(FleeTrigger.NegativeStanding);
                        break;
                    case "Neutral standing pilot in local":
                        build.Add(FleeTrigger.NeutralStanding);
                        break;
                    case "Targeted by another player":
                        build.Add(FleeTrigger.Targeted);
                        break;
                    case "Capacitor low":
                        build.Add(FleeTrigger.CapacitorLow);
                        break;
                    case "Shield low":
                        build.Add(FleeTrigger.ShieldLow);
                        break;
                    case "Armor low":
                        build.Add(FleeTrigger.ArmorLow);
                        break;
                }
            }
            Config.Triggers = build;
            Config.Save();
        }


        private void Threshold_ValueChanged(object sender, EventArgs e)
        {
            if (sender == Threshold)
            {
                switch (ActiveTrigger)
                {
                    case "CapacitorLow":
                        Config.CapThreshold = Threshold.Value;
                        ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                        break;
                    case "ShieldLow":
                        Config.ShieldThreshold = Threshold.Value;
                        ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                        break;
                    case "ArmorLow":
                        Config.ArmorThreshold = Threshold.Value;
                        ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Armor";
                        break;
                }
                Config.Save();
            }
        }

        private void IncludeCorpMembers_CheckedChanged(object sender, EventArgs e)
        {
            switch (ActiveTrigger)
            {
                case "NegativeStanding":
                    Config.NegativeCorp = IncludeCorpMembers.Checked;
                    break;
                case "NeutralStanding":
                    Config.NeutralCorp = IncludeCorpMembers.Checked;
                    break;
                case "Targeted":
                    Config.TargetCorp = IncludeCorpMembers.Checked;
                    break;
            }
            Config.Save();
        }

        private void IncludeAllianceMembers_CheckedChanged(object sender, EventArgs e)
        {
            switch (ActiveTrigger)
            {
                case "NegativeStanding":
                    Config.NegativeAlliance = IncludeAllianceMembers.Checked;
                    break;
                case "NeutralStanding":
                    Config.NeutralAlliance = IncludeAllianceMembers.Checked;
                    break;
                case "Targeted":
                    Config.TargetAlliance = IncludeAllianceMembers.Checked;
                    break;
            }
            Config.Save();
        }

        private void IncludeFleetMembers_CheckedChanged(object sender, EventArgs e)
        {
            switch (ActiveTrigger)
            {
                case "NegativeStanding":
                    Config.NegativeFleet = IncludeFleetMembers.Checked;
                    break;
                case "NeutralStanding":
                    Config.NeutralFleet = IncludeFleetMembers.Checked;
                    break;
                case "Targeted":
                    Config.TargetFleet = IncludeFleetMembers.Checked;
                    break;
            }
            Config.Save();
        }

        private void FleeTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<FleeType> build = new List<FleeType>();
            foreach (string i in FleeTypes.CheckedItems)
            {
                switch (i)
                {
                    case "Flee to closest station in system":
                        build.Add(FleeType.NearestStation);
                        break;
                    case "Flee to secure bookmark":
                        build.Add(FleeType.SecureBookmark);
                        break;
                    case "Cycle safe bookmarks":
                        build.Add(FleeType.SafeBookmarks);
                        break;
                }
            }
            Config.Types = build;
            Config.Save();
        }

        private void FleeWait_Scroll(object sender, EventArgs e)
        {
            Config.FleeWait = FleeWait.Value;
            lblFleeWait.Text = String.Format("Wait {0} minutes after flee", FleeWait.Value);
            Config.Save();
        }



    }
}

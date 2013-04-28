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

            SafeSubstring.Text = Config.SafeSubstring;
            ActiveTriggers.Items.AddRange(Config.Triggers.Select(a => a.ToString()).ToArray());
            Config.Triggers.ForEach(a => InactiveTriggers.Items.Remove(a.ToString()));
        }

        private void SecureBookmark_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.SecureBookmark = SecureBookmark.SelectedItem.ToString();
            Config.Save();
        }
        private void SecureBookmarkFilter_TextChanged(object sender, EventArgs e)
        {
            SecureBookmark.DataSource = Bookmarks.Where(a => a.Contains(SecureBookmarkFilter.Text)).ToList();
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

        private void ActiveTriggers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveTriggers.SelectedItem == null)
            {
                StandingGroup.Hide();
                ThresholdGroup.Hide();
                return;
            }
            InactiveTriggers.ClearSelected();
            switch (ActiveTriggers.SelectedItem.ToString())
            {
                case "Pod" :
                    StandingGroup.Hide();
                    ThresholdGroup.Hide();
                    break;
                case "NegativeStanding" :
                    ThresholdGroup.Hide();
                    StandingGroup.Show();                   
                    IncludeCorpMembers.Checked = Config.NegativeCorp;
                    IncludeAllianceMembers.Checked = Config.NegativeAlliance;
                    IncludeFleetMembers.Checked = Config.NegativeFleet;
                    break;
                case "NeutralStanding" :
                    ThresholdGroup.Hide();
                    StandingGroup.Show();
                    IncludeCorpMembers.Checked = Config.NeutralCorp;
                    IncludeAllianceMembers.Checked = Config.NeutralAlliance;
                    IncludeFleetMembers.Checked = Config.NeutralFleet;
                    break;
                case "Targeted":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();                   
                    IncludeCorpMembers.Checked = Config.TargetCorp;
                    IncludeAllianceMembers.Checked = Config.TargetAlliance;
                    IncludeFleetMembers.Checked = Config.TargetFleet;
                    break;
                case "CapacitorLow" :
                    Threshold.Value = Config.CapThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "ShieldLow":
                    Threshold.Value = Config.ShieldThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "ArmorLow":
                    Threshold.Value = Config.ArmorThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Armor";
                    StandingGroup.Hide();                   
                    ThresholdGroup.Show();
                    break;
            }
            ActiveTrigger = ActiveTriggers.SelectedItem.ToString();
        }

        private void InactiveTriggers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InactiveTriggers.SelectedItem == null)
            {
                StandingGroup.Hide();
                ThresholdGroup.Hide();
                return;
            }
            ActiveTriggers.ClearSelected();
            switch (InactiveTriggers.SelectedItem.ToString())
            {
                case "Pod":
                    StandingGroup.Hide();
                    ThresholdGroup.Hide();
                    break;
                case "NegativeStanding":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();
                    IncludeCorpMembers.Checked = Config.NegativeCorp;
                    IncludeAllianceMembers.Checked = Config.NegativeAlliance;
                    IncludeFleetMembers.Checked = Config.NegativeFleet;
                    break;
                case "NeutralStanding":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();
                    IncludeCorpMembers.Checked = Config.NeutralCorp;
                    IncludeAllianceMembers.Checked = Config.NeutralAlliance;
                    IncludeFleetMembers.Checked = Config.NeutralFleet;
                    break;
                case "Targeted":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();
                    IncludeCorpMembers.Checked = Config.TargetCorp;
                    IncludeAllianceMembers.Checked = Config.TargetAlliance;
                    IncludeFleetMembers.Checked = Config.TargetFleet;
                    break;
                case "CapacitorLow":
                    Threshold.Value = Config.CapThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "ShieldLow":
                    Threshold.Value = Config.ShieldThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "ArmorLow":
                    Threshold.Value = Config.ArmorThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Armor";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
            }
            ActiveTrigger = InactiveTriggers.SelectedItem.ToString();
        }

        private void InactiveTriggers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ActiveTriggers.Items.Add(InactiveTriggers.SelectedItem);
            InactiveTriggers.Items.Remove(InactiveTriggers.SelectedItem);
            List<FleeTrigger> NewTriggerlist = new List<FleeTrigger>();
            foreach (string s in ActiveTriggers.Items)
            {
                NewTriggerlist.Add((FleeTrigger)Enum.Parse(typeof(FleeTrigger), s));
            }
            Config.Triggers = NewTriggerlist;
            Config.Save();
        }

        private void ActiveTriggers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InactiveTriggers.Items.Add(ActiveTriggers.SelectedItem);
            ActiveTriggers.Items.Remove(ActiveTriggers.SelectedItem);
            List<FleeTrigger> NewTriggerlist = new List<FleeTrigger>();
            foreach (string s in ActiveTriggers.Items)
            {
                NewTriggerlist.Add((FleeTrigger)Enum.Parse(typeof(FleeTrigger), s));
            }
            Config.Triggers = NewTriggerlist;
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

    }
}

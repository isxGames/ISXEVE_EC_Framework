using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EveCom;

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
            
            
            using (new EVEFrameLock())
            {
                Bookmarks = Bookmark.All.Select(a => a.Title).ToList();
            }

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

            SecureBookmark.DataSource = Bookmarks.Where(a => a == Config.SecureBookmark).ToList(); ;
            SecureBookmark.SelectedItem = Config.SecureBookmark;
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
                case "In Pod" :
                    StandingGroup.Hide();
                    ThresholdGroup.Hide();
                    break;
                case "Negative Standing in Local" :
                    ThresholdGroup.Hide();
                    StandingGroup.Show();                   
                    IncludeCorpMembers.Checked = Config.NegativeCorp;
                    IncludeAllianceMembers.Checked = Config.NegativeAlliance;
                    IncludeFleetMembers.Checked = Config.NegativeFleet;
                    break;
                case "Neutral in Local" :
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
                case "Capacitor Low" :
                    Threshold.Value = Config.CapThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "Shield Low":
                    Threshold.Value = Config.ShieldThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "Armor Low":
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
                case "In Pod":
                    StandingGroup.Hide();
                    ThresholdGroup.Hide();
                    break;
                case "Negative Standing in Local":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();
                    IncludeCorpMembers.Checked = Config.NegativeCorp;
                    IncludeAllianceMembers.Checked = Config.NegativeAlliance;
                    IncludeFleetMembers.Checked = Config.NegativeFleet;
                    break;
                case "Neutral in Local":
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
                case "Capacitor Low":
                    Threshold.Value = Config.CapThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "Shield Low":
                    Threshold.Value = Config.ShieldThreshold;
                    ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                    StandingGroup.Hide();
                    ThresholdGroup.Show();
                    break;
                case "Armor Low":
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
            Config.Triggers = ActiveTriggers.Items.Cast<FleeTrigger>().ToList();
            Config.Save();
        }

        private void ActiveTriggers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InactiveTriggers.Items.Add(InactiveTriggers.SelectedItem);
            ActiveTriggers.Items.Remove(InactiveTriggers.SelectedItem);
            Config.Triggers = ActiveTriggers.Items.Cast<FleeTrigger>().ToList();
            Config.Save();
        }

        private void Threshold_ValueChanged(object sender, EventArgs e)
        {
            if (sender == Threshold)
            {
                switch (ActiveTrigger)
                {
                    case "Capacitor Low":
                        Config.CapThreshold = Threshold.Value;
                        ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                        break;
                    case "Shield Low":
                        Config.ShieldThreshold = Threshold.Value;
                        ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                        break;
                    case "Armor Low":
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
                case "Negative Standing in Local":
                    Config.NegativeCorp = IncludeCorpMembers.Checked;
                    break;
                case "Neutral in Local":
                    Config.NeutralCorp = IncludeCorpMembers.Checked;
                    break;
                case "Targeted by another player":
                    Config.TargetCorp = IncludeCorpMembers.Checked;
                    break;
            }
            Config.Save();
        }

        private void IncludeAllianceMembers_CheckedChanged(object sender, EventArgs e)
        {
            switch (ActiveTrigger)
            {
                case "Negative Standing in Local":
                    Config.NegativeAlliance = IncludeAllianceMembers.Checked;
                    break;
                case "Neutral in Local":
                    Config.NeutralAlliance = IncludeAllianceMembers.Checked;
                    break;
                case "Targeted by another player":
                    Config.TargetAlliance = IncludeAllianceMembers.Checked;
                    break;
            }
            Config.Save();
        }

        private void IncludeFleetMembers_CheckedChanged(object sender, EventArgs e)
        {
            switch (ActiveTrigger)
            {
                case "Negative Standing in Local":
                    Config.NegativeFleet = IncludeFleetMembers.Checked;
                    break;
                case "Neutral in Local":
                    Config.NeutralFleet = IncludeFleetMembers.Checked;
                    break;
                case "Targeted by another player":
                    Config.TargetFleet = IncludeFleetMembers.Checked;
                    break;
            }
            Config.Save();
        }

        private void FleeTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.Types = FleeTypes.CheckedItems.Cast<FleeType>().ToList();
            Config.Save();
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using EveCom;
using EveComFramework.Core;
using InnerSpaceAPI;

namespace EveComFramework.Security.UI
{
    internal partial class Security : Form
    {
        
        string ActiveTrigger;
        SecuritySettings Config = EveComFramework.Security.Security.Instance.Config;
        SecurityAudioSettings SpeechConfig = EveComFramework.Security.SecurityAudio.Instance.Config;
        Cache Cache = Cache.Instance;
        SpeechSynthesizer Speech = new SpeechSynthesizer();

        public Security()
        {
            InitializeComponent();
        }

        private void LoadSettings()
        {
            foreach (ListViewItem i in FleeTypes.Items)
            {
                switch (i.Text)
                {
                    case "Flee to closest station":
                        if (Config.Types.Contains(FleeType.NearestStation))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Flee to secure bookmark":
                        if (Config.Types.Contains(FleeType.SecureBookmark))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Cycle safe bookmarks":
                        if (Config.Types.Contains(FleeType.SafeBookmarks))
                        {
                            i.Checked = true;
                        }
                        break;
                }
            }

            foreach (ListViewItem i in Triggers.Items)
            {
                switch (i.Text)
                {
                    case "In a pod":
                        if (Config.Triggers.Contains(FleeTrigger.Pod))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Negative standing pilot in local":
                        if (Config.Triggers.Contains(FleeTrigger.NegativeStanding))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Neutral standing pilot in local":
                        if (Config.Triggers.Contains(FleeTrigger.NeutralStanding))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Neutral to me only":
                        if (Config.Triggers.Contains(FleeTrigger.Paranoid))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Targeted by another player":
                        if (Config.Triggers.Contains(FleeTrigger.Targeted))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Capacitor low":
                        if (Config.Triggers.Contains(FleeTrigger.CapacitorLow))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Shield low":
                        if (Config.Triggers.Contains(FleeTrigger.ShieldLow))
                        {
                            i.Checked = true;
                        }
                        break;
                    case "Armor low":
                        if (Config.Triggers.Contains(FleeTrigger.ArmorLow))
                        {
                            i.Checked = true;
                        }
                        break;
                }
            }

            SafeSubstring.Text = Config.SafeSubstring;
            if (Cache.Bookmarks != null) SecureBookmark.Items.AddRange(Cache.Bookmarks);
            SecureBookmark.Text = Config.SecureBookmark;
            CheckBookmark();
            LoadWhiteList();

            checkAudioBlue.Checked = SpeechConfig.Blue;
            checkAudioFlee.Checked = SpeechConfig.Flee;
            checkAudioGrey.Checked = SpeechConfig.Grey;
            checkAudioRed.Checked = SpeechConfig.Red;
            checkLocal.Checked = SpeechConfig.Local;
            listVoices.Items.Clear();
            listVoices.Items.AddRange(Speech.GetInstalledVoices().Select(a => a.VoiceInfo.Name).ToArray());
            trackRate.Value = SpeechConfig.Rate;
            trackVolume.Value = SpeechConfig.Volume;

            FleeWait.Value = Config.FleeWait;
            lblFleeWait.Text = String.Format("Wait {0} minutes after flee", FleeWait.Value);
        }

        void CheckBookmark()
        {
            if (SecureBookmark.Items.Contains(SecureBookmark.Text))
            {
                SecureBookmarkVerify.Image = Properties.Resources.action_check;
            }
            else
            {
                SecureBookmarkVerify.Image = Properties.Resources.action_delete;
            }
        }

        private void Security_Load(object sender, EventArgs e)
        {
            LoadSettings();
            FleeTypes.ItemChecked += FleeTypes_ItemChecked;
            Triggers.ItemChecked += Triggers_ItemChecked;
        }

        private void SecureBookmark_TextChanged(object sender, EventArgs e)
        {
            Config.SecureBookmark = SecureBookmark.Text;
            CheckBookmark();
            Config.Save();
        }

        private void SafeSubstring_TextChanged(object sender, EventArgs e)
        {
            Config.SafeSubstring = SafeSubstring.Text;
            Config.Save();
        }

        private void Triggers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Triggers.SelectedItems.Count == 0)
            {
                StandingGroup.Hide();
                ThresholdGroup.Hide();
                return;
            }
            switch (Triggers.SelectedItems[0].Text)
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
                case "Neutral to me only":
                    ThresholdGroup.Hide();
                    StandingGroup.Show();
                    IncludeCorpMembers.Checked = Config.ParanoidCorp;
                    IncludeAllianceMembers.Checked = Config.ParanoidAlliance;
                    IncludeFleetMembers.Checked = Config.ParanoidFleet;
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
            ActiveTrigger = Triggers.SelectedItems[0].Text;
        }

        private void Threshold_ValueChanged(object sender, EventArgs e)
        {
            if (sender == Threshold)
            {
                switch (ActiveTrigger)
                {
                    case "Capacitor low":
                        Config.CapThreshold = Threshold.Value;
                        ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Capacitor";
                        break;
                    case "Shield low":
                        Config.ShieldThreshold = Threshold.Value;
                        ThresholdLabel.Text = "Flee if below " + Threshold.Value + " % Shields";
                        break;
                    case "Armor low":
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
                case "Neutral to me only":
                    Config.ParanoidCorp = IncludeCorpMembers.Checked;
                    break;
                case "Negative standing pilot in local":
                    Config.NegativeCorp = IncludeCorpMembers.Checked;
                    break;
                case "Neutral standing pilot in local":
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
                case "Neutral to me only":
                    Config.ParanoidAlliance = IncludeCorpMembers.Checked;
                    break;
                case "Negative standing pilot in local":
                    Config.NegativeAlliance = IncludeAllianceMembers.Checked;
                    break;
                case "Neutral standing pilot in local":
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
                case "Neutral to me only":
                    Config.ParanoidFleet = IncludeCorpMembers.Checked;
                    break;
                case "Negative standing pilot in local":
                    Config.NegativeFleet = IncludeFleetMembers.Checked;
                    break;
                case "Neutral standing pilot in local":
                    Config.NeutralFleet = IncludeFleetMembers.Checked;
                    break;
                case "Targeted by another player":
                    Config.TargetFleet = IncludeFleetMembers.Checked;
                    break;
            }
            Config.Save();
        }


        private void FleeWait_Scroll(object sender, EventArgs e)
        {
            Config.FleeWait = FleeWait.Value;
            lblFleeWait.Text = String.Format("Wait {0} minutes after flee", FleeWait.Value);
            Config.Save();
        }



        private void Triggers_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            List<FleeTrigger> build = new List<FleeTrigger>();
            foreach (ListViewItem i in Triggers.CheckedItems)
            {
                switch (i.Text)
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
                    case "Neutral to me only":
                        build.Add(FleeTrigger.Paranoid);
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
                EVEFrame.Log(i.Text);
            }
            Config.Triggers = build;
            Config.Save();
        }

        private void FleeTypes_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            List<FleeType> build = new List<FleeType>();
            foreach (ListViewItem i in FleeTypes.CheckedItems)
            {
                switch (i.Text)
                {
                    case "Flee to closest station":
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cache.Bookmarks != null)
            {
                SecureBookmark.Items.Clear();
                SecureBookmark.Items.AddRange(Cache.Bookmarks);
            }
            SecureBookmark.Text = Config.SecureBookmark;
            CheckBookmark();
        }

        private void checkAudioFlee_CheckedChanged(object sender, EventArgs e)
        {
            SpeechConfig.Flee = checkAudioFlee.Checked;
            SpeechConfig.Save();
        }

        private void checkAudioRed_CheckedChanged(object sender, EventArgs e)
        {
            SpeechConfig.Red = checkAudioRed.Checked;
            SpeechConfig.Save();
        }

        private void checkAudioBlue_CheckedChanged(object sender, EventArgs e)
        {
            SpeechConfig.Blue = checkAudioBlue.Checked;
            SpeechConfig.Save();
        }

        private void checkAudioGrey_CheckedChanged(object sender, EventArgs e)
        {
            SpeechConfig.Grey = checkAudioGrey.Checked;
            SpeechConfig.Save();
        }

        private void listVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listVoices.SelectedIndex != -1)
            {
                SpeechConfig.Voice = listVoices.SelectedItem.ToString();
                SpeechConfig.Save();
                Speech.Speak(listVoices.SelectedItem.ToString());
            }
        }

        private void trackRate_Scroll(object sender, EventArgs e)
        {
            SpeechConfig.Rate = trackRate.Value;
            SpeechConfig.Save();
            if (listVoices.SelectedIndex != -1)
            {
                Speech.Rate = trackRate.Value;
                Speech.Speak(listVoices.SelectedItem.ToString());
            }
        }

        private void trackVolume_Scroll(object sender, EventArgs e)
        {
            SpeechConfig.Volume = trackVolume.Value;
            SpeechConfig.Save();
            if (listVoices.SelectedIndex != -1)
            {
                Speech.Volume = trackVolume.Value;
                Speech.Speak(listVoices.SelectedItem.ToString());
            }
        }

        private void checkLocal_CheckedChanged(object sender, EventArgs e)
        {
            SpeechConfig.Local = checkLocal.Checked;
            SpeechConfig.Save();
        }

        private void LoadWhiteList()
        {
            listWhiteList.Items.Clear();
            listWhiteList.Items.AddRange(Config.WhiteList.ToArray());
        }

        private void buttonAddWhiteList_Click(object sender, EventArgs e)
        {
            if (textWhiteListPilot.Text != "")
            {
                Config.WhiteList.Add(textWhiteListPilot.Text);
                Config.Save();
                LoadWhiteList();
            }
        }

        private void listWhiteList_KeyUp(object sender, KeyEventArgs e)
        {
            if (listWhiteList.SelectedIndex >= 0)
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    if (Config.WhiteList.Contains(listWhiteList.SelectedItem.ToString()))
                    {
                        Config.WhiteList.Remove(listWhiteList.SelectedItem.ToString());
                        Config.Save();
                        LoadWhiteList();
                    }
                }
            }
        }

    }

}

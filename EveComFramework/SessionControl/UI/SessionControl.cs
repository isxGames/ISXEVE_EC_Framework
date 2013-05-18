using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EveComFramework.Core;

namespace EveComFramework.SessionControl.UI
{
    public partial class SessionControl : Form
    {
        internal LoginGlobalSettings GlobalConfig = EveComFramework.SessionControl.SessionControl.Instance.GlobalConfig;
        internal LoginLocalSettings Config = EveComFramework.SessionControl.SessionControl.Instance.Config;
        UIData UIData = UIData.Instance;

        public SessionControl()
        {
            InitializeComponent();
        }

        private void LoginControl_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            foreach (string characterName in GlobalConfig.Profiles.Keys)
            {
                AddProfile(characterName);
            }
            numLoginDelta.Value = Config.LoginDelta;
            numLogoutHours.Value = Config.LogoutHours;
            numLogoutHoursDelta.Value = Config.LogoutDelta;
            numDowntime.Value = Config.Downtime;
            numDowntimeDelta.Value = Config.DowntimeDelta;
        }


        private void AddProfile(string characterName)
        {
            ListViewItem profileLVI = new ListViewItem(characterName);
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, GlobalConfig.Profiles[characterName].Username));
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, GlobalConfig.Profiles[characterName].Password));
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, GlobalConfig.Profiles[characterName].CharacterID.ToString()));
            profileListView.Items.Add(profileLVI);
        }

        private void addProfileButton_Click(object sender, EventArgs e)
        {
            if (UIData.CharName == null) return;
            Profile p = new Profile();
            p.Username = pUNameBox.Text;
            p.Password = pPasswordBox.Text;
            p.CharacterID = UIData.CharID;
            if (GlobalConfig.Profiles.ContainsKey(UIData.Instance.CharName))
            {
                GlobalConfig.Profiles[UIData.CharName] = p;
            }
            else
            {
                GlobalConfig.Profiles.Add(UIData.CharName, p);
            }
            GlobalConfig.Save();
            AddProfile(UIData.CharName);
        }

        private void removeProfile_Click(object sender, EventArgs e)
        {
            GlobalConfig.Profiles.Remove((string)profileListView.SelectedItems[0].Text);
            profileListView.Items.Remove(profileListView.SelectedItems[0]);
            GlobalConfig.Save();
        }

        private void numLoginDelta_ValueChanged(object sender, EventArgs e)
        {
            Config.LoginDelta = (int)Math.Floor(numLoginDelta.Value);
            Config.Save();
            EveComFramework.SessionControl.SessionControl.Instance.NewLoginDelta();
        }

        private void numLogoutHours_ValueChanged(object sender, EventArgs e)
        {
            Config.LogoutHours = (int)Math.Floor(numLogoutHours.Value);
            Config.Save();
        }

        private void numLogoutHoursDelta_ValueChanged(object sender, EventArgs e)
        {
            Config.LogoutDelta = (int)Math.Floor(numLogoutHoursDelta.Value);
            Config.Save();
        }

        private void numDowntime_ValueChanged(object sender, EventArgs e)
        {
            Config.Downtime = (int)Math.Floor(numDowntimeDelta.Value);
            Config.Save();
        }

        private void numDowntimeDelta_ValueChanged(object sender, EventArgs e)
        {
            Config.DowntimeDelta = (int)Math.Floor(numDowntimeDelta.Value);
            Config.Save();
            EveComFramework.SessionControl.SessionControl.Instance.NewDowntimeDelta();
        }


    }
}

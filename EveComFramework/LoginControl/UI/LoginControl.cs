using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveComFramework.LoginControl.UI
{
    public partial class LoginControl : Form
    {
        internal LoginGlobalSettings Config = EveComFramework.LoginControl.LoginControl.Instance.Config;

        public LoginControl()
        {
            InitializeComponent();
        }

        private void LoginControl_Load(object sender, EventArgs e)
        {
            EveComFramework.LoginControl.UIData.Instance.GotData += UIData_Loaded;
            foreach (string characterName in Config.Profiles.Keys)
            {
                AddProfile(characterName);
            }
            Config.Save();
        }

        void UIData_Loaded()
        {
            if (charnameBox.InvokeRequired)
            {
                charnameBox.Text = EveComFramework.LoginControl.UIData.Instance.CharName;
                pCharIDBox.Text = EveComFramework.LoginControl.UIData.Instance.CharID.ToString();
                addProfileButton.Enabled = true;
            }
            else
            {
                charnameBox.BeginInvoke(new Action(UIData_Loaded));
            }
        }
        private void AddProfile(string characterName)
        {
            ListViewItem profileLVI = new ListViewItem(characterName);
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, Config.Profiles[characterName].Username));
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, Config.Profiles[characterName].Password));
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, Config.Profiles[characterName].CharacterID.ToString()));
            profileListView.Items.Add(profileLVI);
        }

        private void addProfileButton_Click(object sender, EventArgs e)
        {
            Profile p = new Profile();
            p.Username = pUNameBox.Text;
            p.Password = pPasswordBox.Text;
            p.CharacterID = Convert.ToInt64(pCharIDBox.Text);
            Config.Profiles.Add(charnameBox.Text,p);
            AddProfile(charnameBox.Text);
            Config.Save();
        }

        private void removeProfile_Click(object sender, EventArgs e)
        {   
            Config.Profiles.Remove((string)profileListView.SelectedItems[0].Text);
            profileListView.Items.Remove(profileListView.SelectedItems[0]);
            Config.Save();
        }
    }
}

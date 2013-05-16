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
        internal LoginSettings Config = EveComFramework.LoginControl.LoginControl.Instance.Config;

        public LoginControl()
        {
            InitializeComponent();
        }

        private void LoginControl_Load(object sender, EventArgs e)
        {
            foreach (Profile profile in Config.Profiles)
            {
                AddProfile(profile);
            }
            Config.Save();
        }
        private void AddProfile(Profile p)
        {
            ListViewItem profileLVI = new ListViewItem(p.ProfileName);
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, p.Username));
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, p.Password));
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, p.CharacterID.ToString()));
            profileLVI.SubItems.Add(new ListViewItem.ListViewSubItem(profileLVI, p.Bot));
            profileLVI.Tag = p;
            profileListView.Items.Add(profileLVI);
        }

        private void addProfileButton_Click(object sender, EventArgs e)
        {
            Profile p = new Profile();
            p.ProfileName = pNameBox.Text;
            p.Username = pUNameBox.Text;
            p.Password = pPasswordBox.Text;
            p.CharacterID = Convert.ToInt64(pCharIDBox.Text);
            p.Bot = botComboBox.Text;
            Config.Profiles.Add(p);
            AddProfile(p);
            Config.Save();
        }

        private void removeProfile_Click(object sender, EventArgs e)
        {   
            Config.Profiles.Remove((Profile)profileListView.SelectedItems[0].Tag);
            profileListView.Items.Remove(profileListView.SelectedItems[0]);
            Config.Save();
        }
    }
}

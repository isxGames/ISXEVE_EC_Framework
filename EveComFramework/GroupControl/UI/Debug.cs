using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveComFramework.GroupControl.UI
{
    public partial class Debug : Form
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView1.Clear();
            foreach (ActiveMember x in EveComFramework.GroupControl.GroupControl.Instance.CurrentGroup.ActiveMembers)
            {
                ListViewItem y = new ListViewItem();
                y.Text = x.CharacterName;
                y.SubItems.Add(x.Available.ToString());
                y.SubItems.Add(x.Active.ToString());
                y.SubItems.Add(x.InFleet.ToString());
                y.SubItems.Add(x.LeadershipValue.ToString());
                y.SubItems.Add(x.Role.ToString());
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

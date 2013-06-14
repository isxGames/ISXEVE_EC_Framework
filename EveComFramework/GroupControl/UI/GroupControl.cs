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
    public partial class GroupControl : Form
    {
        EveComFramework.GroupControl.GroupControl parent = EveComFramework.GroupControl.GroupControl.Instance;
        string thisProfile = EveComFramework.Core.Config.Instance.DefaultProfile;
        public GroupControl()
        {
            InitializeComponent();
        }

        private void GroupControl_Load(object sender, EventArgs e)
        {
            RefreshTree();
            profileLabel.Text = thisProfile;
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                roleCombo.Items.Add(role.ToString());                
            }
            roleCombo.SelectedItem = roleCombo.Items.Cast<string>().First(a => a == parent.Config.Role.ToString());
            foreach (GroupType gType in Enum.GetValues(typeof(GroupType)))
            {
                comboBox1.Items.Add(gType.ToString());
            }            
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void RefreshTree()
        {
            groupTView.Nodes.Clear();
            foreach (GroupSettings gs in parent.GlobalConfig.Groups)
            {
                TreeNode groupNode = new TreeNode(gs.FriendlyName + "-" + gs.GroupType.ToString());
                groupNode.Tag = gs;                
                foreach (string profile in gs.MemberProfiles)
                {
                    groupNode.Nodes.Add(profile);
                }
                groupTView.Nodes.Add(groupNode);
            }
            groupTView.ExpandAll();
        }
        private void addGroupButton_Click(object sender, EventArgs e)
        {
            parent.GlobalConfig.Groups.Add(new GroupSettings());
            parent.GlobalConfig.Groups.Last().GroupType = (GroupType)Enum.Parse(typeof(GroupType), (string)comboBox1.SelectedItem);
            parent.GlobalConfig.Groups.Last().FriendlyName = groupNameTBox.Text;
            parent.GlobalConfig.Save();
            RefreshTree();
        }

        private void joinButton_Click(object sender, EventArgs e)
        {            
            GroupSettings activeGroup = parent.GlobalConfig.Groups.FirstOrDefault(a => a.MemberProfiles.Any(b => b == thisProfile));
            if (activeGroup != null)
            {
                activeGroup.MemberProfiles.Remove(thisProfile);
            }
            GroupSettings newGroup = (GroupSettings)groupTView.SelectedNode.Tag;
            newGroup.MemberProfiles.Add(thisProfile);
            parent.Config.CurrentGroup = newGroup.ID;
            parent.GlobalConfig.Save();
            parent.Config.Save();
            RefreshTree();               
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (groupTView.SelectedNode.Tag != null)
            {
                GroupSettings toDelete = parent.GlobalConfig.Groups.FirstOrDefault(a => a.MemberProfiles.Any(b => b == thisProfile));
                if (toDelete != null)
                {
                    parent.GlobalConfig.Groups.Remove(toDelete);
                    parent.GlobalConfig.Save();
                }
                RefreshTree();
            }
            else
            {
                GroupSettings parentGroup = (GroupSettings)groupTView.SelectedNode.Parent.Tag;
                parentGroup.MemberProfiles.Remove(groupTView.SelectedNode.Text);
                parent.GlobalConfig.Save();
                RefreshTree();
            }

        }

        private void roleCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Role newRole = (Role)Enum.Parse(typeof(Role), (string)roleCombo.SelectedItem);
            parent.Config.Role = newRole;
            parent.Config.Save();            
        }

        private void groupTView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (groupTView.SelectedNode != null)
            {

                if (groupTView.SelectedNode.Tag != null)
                {
                    deleteButton.Enabled = true;
                    joinButton.Enabled = true;
                }
                else
                {
                    joinButton.Enabled = false;
                }
            }
            else
            {
                deleteButton.Enabled = false;
                joinButton.Enabled = false;
            }
        }

        private void groupNameTBox_TextChanged(object sender, EventArgs e)
        {
            if (groupNameTBox.Text != null)
            {
                addGroupButton.Enabled = true;
                addGroupButton.Text = "Create New Group";
            }
            else
            {
                addGroupButton.Enabled = false;
                addGroupButton.Text = "Enter Group Name First!";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

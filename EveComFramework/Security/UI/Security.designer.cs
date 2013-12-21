namespace EveComFramework.Security.UI
{
    partial class Security
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("In a pod");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Negative standing pilot in local");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Neutral standing pilot in local");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Neutral to me only");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Targeted by another player");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Capacitor low");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Shield low");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Armor low");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Flee to closest station");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Flee to secure bookmark");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("Cycle safe bookmarks");
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ThresholdGroup = new System.Windows.Forms.GroupBox();
            this.ThresholdLabel = new System.Windows.Forms.Label();
            this.Threshold = new System.Windows.Forms.TrackBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Triggers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StandingGroup = new System.Windows.Forms.GroupBox();
            this.IncludeFleetMembers = new System.Windows.Forms.CheckBox();
            this.IncludeAllianceMembers = new System.Windows.Forms.CheckBox();
            this.IncludeCorpMembers = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblFleeWait = new System.Windows.Forms.Label();
            this.FleeWait = new System.Windows.Forms.TrackBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SafeSubstring = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SecureBookmarkVerify = new System.Windows.Forms.PictureBox();
            this.SecureBookmark = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FleeTypes = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.trackVolume = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackRate = new System.Windows.Forms.TrackBar();
            this.listVoices = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkChatInvite = new System.Windows.Forms.CheckBox();
            this.checkLocal = new System.Windows.Forms.CheckBox();
            this.checkAudioGrey = new System.Windows.Forms.CheckBox();
            this.checkAudioBlue = new System.Windows.Forms.CheckBox();
            this.checkAudioRed = new System.Windows.Forms.CheckBox();
            this.checkAudioFlee = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.buttonAddWhiteList = new System.Windows.Forms.Button();
            this.textWhiteListPilot = new System.Windows.Forms.TextBox();
            this.listWhiteList = new System.Windows.Forms.ListBox();
            this.checkGridTraffic = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.ThresholdGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Threshold)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.StandingGroup.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FleeWait)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SecureBookmarkVerify)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRate)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(320, 330);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ThresholdGroup);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.StandingGroup);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(312, 304);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Flee Triggers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ThresholdGroup
            // 
            this.ThresholdGroup.Controls.Add(this.ThresholdLabel);
            this.ThresholdGroup.Controls.Add(this.Threshold);
            this.ThresholdGroup.Location = new System.Drawing.Point(6, 196);
            this.ThresholdGroup.Name = "ThresholdGroup";
            this.ThresholdGroup.Size = new System.Drawing.Size(300, 60);
            this.ThresholdGroup.TabIndex = 3;
            this.ThresholdGroup.TabStop = false;
            this.ThresholdGroup.Text = "Trigger Properties";
            this.ThresholdGroup.Visible = false;
            // 
            // ThresholdLabel
            // 
            this.ThresholdLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdLabel.Location = new System.Drawing.Point(6, 46);
            this.ThresholdLabel.Name = "ThresholdLabel";
            this.ThresholdLabel.Size = new System.Drawing.Size(288, 19);
            this.ThresholdLabel.TabIndex = 5;
            this.ThresholdLabel.Text = "Flee if below 0%";
            this.ThresholdLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Threshold
            // 
            this.Threshold.BackColor = System.Drawing.SystemColors.Control;
            this.Threshold.Location = new System.Drawing.Point(6, 20);
            this.Threshold.Maximum = 100;
            this.Threshold.Name = "Threshold";
            this.Threshold.Size = new System.Drawing.Size(288, 45);
            this.Threshold.TabIndex = 4;
            this.Threshold.Tag = "Use this slider to set the threshold for the corresponding Flee Trigger.";
            this.Threshold.Scroll += new System.EventHandler(this.Threshold_ValueChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Triggers);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(300, 184);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Triggers";
            // 
            // Triggers
            // 
            this.Triggers.AutoArrange = false;
            this.Triggers.CheckBoxes = true;
            this.Triggers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.Triggers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            this.Triggers.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8});
            this.Triggers.Location = new System.Drawing.Point(9, 20);
            this.Triggers.MultiSelect = false;
            this.Triggers.Name = "Triggers";
            this.Triggers.Scrollable = false;
            this.Triggers.Size = new System.Drawing.Size(285, 158);
            this.Triggers.TabIndex = 3;
            this.Triggers.UseCompatibleStateImageBehavior = false;
            this.Triggers.View = System.Windows.Forms.View.Details;
            this.Triggers.SelectedIndexChanged += new System.EventHandler(this.Triggers_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 171;
            // 
            // StandingGroup
            // 
            this.StandingGroup.Controls.Add(this.IncludeFleetMembers);
            this.StandingGroup.Controls.Add(this.IncludeAllianceMembers);
            this.StandingGroup.Controls.Add(this.IncludeCorpMembers);
            this.StandingGroup.Location = new System.Drawing.Point(6, 196);
            this.StandingGroup.Name = "StandingGroup";
            this.StandingGroup.Size = new System.Drawing.Size(300, 88);
            this.StandingGroup.TabIndex = 2;
            this.StandingGroup.TabStop = false;
            this.StandingGroup.Text = "Trigger Properties";
            this.StandingGroup.Visible = false;
            // 
            // IncludeFleetMembers
            // 
            this.IncludeFleetMembers.AutoSize = true;
            this.IncludeFleetMembers.Location = new System.Drawing.Point(6, 66);
            this.IncludeFleetMembers.Name = "IncludeFleetMembers";
            this.IncludeFleetMembers.Size = new System.Drawing.Size(133, 17);
            this.IncludeFleetMembers.TabIndex = 2;
            this.IncludeFleetMembers.Tag = "If this is checked, fleet members will be included in the trigger.";
            this.IncludeFleetMembers.Text = "Include Fleet Members";
            this.IncludeFleetMembers.UseVisualStyleBackColor = true;
            this.IncludeFleetMembers.Click += new System.EventHandler(this.IncludeFleetMembers_CheckedChanged);
            // 
            // IncludeAllianceMembers
            // 
            this.IncludeAllianceMembers.AutoSize = true;
            this.IncludeAllianceMembers.Location = new System.Drawing.Point(6, 43);
            this.IncludeAllianceMembers.Name = "IncludeAllianceMembers";
            this.IncludeAllianceMembers.Size = new System.Drawing.Size(147, 17);
            this.IncludeAllianceMembers.TabIndex = 1;
            this.IncludeAllianceMembers.Tag = "If this is checked, alliance members will be included in the trigger.";
            this.IncludeAllianceMembers.Text = "Include Alliance Members";
            this.IncludeAllianceMembers.UseVisualStyleBackColor = true;
            this.IncludeAllianceMembers.Click += new System.EventHandler(this.IncludeAllianceMembers_CheckedChanged);
            // 
            // IncludeCorpMembers
            // 
            this.IncludeCorpMembers.AutoSize = true;
            this.IncludeCorpMembers.Location = new System.Drawing.Point(6, 20);
            this.IncludeCorpMembers.Name = "IncludeCorpMembers";
            this.IncludeCorpMembers.Size = new System.Drawing.Size(164, 17);
            this.IncludeCorpMembers.TabIndex = 0;
            this.IncludeCorpMembers.Tag = "If this is checked, corporate members will be included in the trigger.";
            this.IncludeCorpMembers.Text = "Include Corporation Members";
            this.IncludeCorpMembers.UseVisualStyleBackColor = true;
            this.IncludeCorpMembers.Click += new System.EventHandler(this.IncludeCorpMembers_CheckedChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(312, 304);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Flee Responses";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblFleeWait);
            this.groupBox4.Controls.Add(this.FleeWait);
            this.groupBox4.Location = new System.Drawing.Point(6, 220);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(296, 69);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Cooldown";
            // 
            // lblFleeWait
            // 
            this.lblFleeWait.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFleeWait.Location = new System.Drawing.Point(7, 46);
            this.lblFleeWait.Name = "lblFleeWait";
            this.lblFleeWait.Size = new System.Drawing.Size(283, 19);
            this.lblFleeWait.TabIndex = 7;
            this.lblFleeWait.Text = "Wait 5 minutes after flee";
            this.lblFleeWait.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FleeWait
            // 
            this.FleeWait.BackColor = System.Drawing.SystemColors.Control;
            this.FleeWait.Location = new System.Drawing.Point(7, 20);
            this.FleeWait.Maximum = 100;
            this.FleeWait.Name = "FleeWait";
            this.FleeWait.Size = new System.Drawing.Size(283, 45);
            this.FleeWait.TabIndex = 6;
            this.FleeWait.Tag = "Use this slider to set the threshold for the corresponding Flee Trigger.";
            this.FleeWait.Value = 5;
            this.FleeWait.Scroll += new System.EventHandler(this.FleeWait_Scroll);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SafeSubstring);
            this.groupBox3.Location = new System.Drawing.Point(6, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(296, 49);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Safe Substring";
            // 
            // SafeSubstring
            // 
            this.SafeSubstring.Location = new System.Drawing.Point(6, 20);
            this.SafeSubstring.Name = "SafeSubstring";
            this.SafeSubstring.Size = new System.Drawing.Size(284, 21);
            this.SafeSubstring.TabIndex = 0;
            this.SafeSubstring.Tag = "Substring to use to identify safe bookmarks.  Any bookmark in-system which contai" +
                "ns this substring will be used as a safe bookmark.";
            this.SafeSubstring.TextChanged += new System.EventHandler(this.SafeSubstring_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SecureBookmarkVerify);
            this.groupBox2.Controls.Add(this.SecureBookmark);
            this.groupBox2.Location = new System.Drawing.Point(6, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 49);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Secure Bookmark";
            // 
            // SecureBookmarkVerify
            // 
            this.SecureBookmarkVerify.Image = global::EveComFramework.Properties.Resources.action_delete;
            this.SecureBookmarkVerify.Location = new System.Drawing.Point(270, 20);
            this.SecureBookmarkVerify.Name = "SecureBookmarkVerify";
            this.SecureBookmarkVerify.Size = new System.Drawing.Size(20, 20);
            this.SecureBookmarkVerify.TabIndex = 3;
            this.SecureBookmarkVerify.TabStop = false;
            // 
            // SecureBookmark
            // 
            this.SecureBookmark.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.SecureBookmark.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.SecureBookmark.FormattingEnabled = true;
            this.SecureBookmark.Location = new System.Drawing.Point(6, 20);
            this.SecureBookmark.Name = "SecureBookmark";
            this.SecureBookmark.Size = new System.Drawing.Size(258, 21);
            this.SecureBookmark.TabIndex = 2;
            this.SecureBookmark.TextChanged += new System.EventHandler(this.SecureBookmark_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FleeTypes);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flee Response";
            // 
            // FleeTypes
            // 
            this.FleeTypes.AutoArrange = false;
            this.FleeTypes.CheckBoxes = true;
            this.FleeTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.FleeTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem9.StateImageIndex = 0;
            listViewItem10.StateImageIndex = 0;
            listViewItem11.StateImageIndex = 0;
            this.FleeTypes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem9,
            listViewItem10,
            listViewItem11});
            this.FleeTypes.Location = new System.Drawing.Point(10, 20);
            this.FleeTypes.Name = "FleeTypes";
            this.FleeTypes.Scrollable = false;
            this.FleeTypes.Size = new System.Drawing.Size(280, 66);
            this.FleeTypes.TabIndex = 2;
            this.FleeTypes.UseCompatibleStateImageBehavior = false;
            this.FleeTypes.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 141;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox7);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(312, 304);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Audio Alerts";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.trackVolume);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.trackRate);
            this.groupBox7.Controls.Add(this.listVoices);
            this.groupBox7.Location = new System.Drawing.Point(106, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 292);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Voices";
            // 
            // trackVolume
            // 
            this.trackVolume.Location = new System.Drawing.Point(6, 241);
            this.trackVolume.Maximum = 100;
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.Size = new System.Drawing.Size(188, 45);
            this.trackVolume.TabIndex = 4;
            this.trackVolume.Value = 100;
            this.trackVolume.Scroll += new System.EventHandler(this.trackVolume_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 225);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Speech Volume";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Speech Rate";
            // 
            // trackRate
            // 
            this.trackRate.Location = new System.Drawing.Point(6, 177);
            this.trackRate.Minimum = -10;
            this.trackRate.Name = "trackRate";
            this.trackRate.Size = new System.Drawing.Size(188, 45);
            this.trackRate.TabIndex = 1;
            this.trackRate.Scroll += new System.EventHandler(this.trackRate_Scroll);
            // 
            // listVoices
            // 
            this.listVoices.FormattingEnabled = true;
            this.listVoices.Location = new System.Drawing.Point(6, 20);
            this.listVoices.Name = "listVoices";
            this.listVoices.Size = new System.Drawing.Size(188, 134);
            this.listVoices.TabIndex = 0;
            this.listVoices.SelectedIndexChanged += new System.EventHandler(this.listVoices_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkGridTraffic);
            this.groupBox6.Controls.Add(this.checkChatInvite);
            this.groupBox6.Controls.Add(this.checkLocal);
            this.groupBox6.Controls.Add(this.checkAudioGrey);
            this.groupBox6.Controls.Add(this.checkAudioBlue);
            this.groupBox6.Controls.Add(this.checkAudioRed);
            this.groupBox6.Controls.Add(this.checkAudioFlee);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(95, 292);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Enabled Alerts";
            // 
            // checkChatInvite
            // 
            this.checkChatInvite.AutoSize = true;
            this.checkChatInvite.Location = new System.Drawing.Point(6, 135);
            this.checkChatInvite.Name = "checkChatInvite";
            this.checkChatInvite.Size = new System.Drawing.Size(77, 17);
            this.checkChatInvite.TabIndex = 5;
            this.checkChatInvite.Text = "Chat Invite";
            this.toolTip1.SetToolTip(this.checkChatInvite, "Perform audio alert when a chat invite is received");
            this.checkChatInvite.UseVisualStyleBackColor = true;
            this.checkChatInvite.CheckedChanged += new System.EventHandler(this.checkChatInvite_CheckedChanged);
            // 
            // checkLocal
            // 
            this.checkLocal.AutoSize = true;
            this.checkLocal.Location = new System.Drawing.Point(6, 112);
            this.checkLocal.Name = "checkLocal";
            this.checkLocal.Size = new System.Drawing.Size(74, 17);
            this.checkLocal.TabIndex = 4;
            this.checkLocal.Text = "Local chat";
            this.toolTip1.SetToolTip(this.checkLocal, "Perform audio alert when a new message is posted in local chat");
            this.checkLocal.UseVisualStyleBackColor = true;
            this.checkLocal.CheckedChanged += new System.EventHandler(this.checkLocal_CheckedChanged);
            // 
            // checkAudioGrey
            // 
            this.checkAudioGrey.AutoSize = true;
            this.checkAudioGrey.Location = new System.Drawing.Point(6, 89);
            this.checkAudioGrey.Name = "checkAudioGrey";
            this.checkAudioGrey.Size = new System.Drawing.Size(71, 17);
            this.checkAudioGrey.TabIndex = 3;
            this.checkAudioGrey.Text = "New Grey";
            this.toolTip1.SetToolTip(this.checkAudioGrey, "Perform audio alert when a new grey appears in local");
            this.checkAudioGrey.UseVisualStyleBackColor = true;
            this.checkAudioGrey.CheckedChanged += new System.EventHandler(this.checkAudioGrey_CheckedChanged);
            // 
            // checkAudioBlue
            // 
            this.checkAudioBlue.AutoSize = true;
            this.checkAudioBlue.Location = new System.Drawing.Point(6, 66);
            this.checkAudioBlue.Name = "checkAudioBlue";
            this.checkAudioBlue.Size = new System.Drawing.Size(70, 17);
            this.checkAudioBlue.TabIndex = 2;
            this.checkAudioBlue.Text = "New Blue";
            this.toolTip1.SetToolTip(this.checkAudioBlue, "Perform audio alert when a new blue appears in local");
            this.checkAudioBlue.UseVisualStyleBackColor = true;
            this.checkAudioBlue.CheckedChanged += new System.EventHandler(this.checkAudioBlue_CheckedChanged);
            // 
            // checkAudioRed
            // 
            this.checkAudioRed.AutoSize = true;
            this.checkAudioRed.Location = new System.Drawing.Point(6, 43);
            this.checkAudioRed.Name = "checkAudioRed";
            this.checkAudioRed.Size = new System.Drawing.Size(67, 17);
            this.checkAudioRed.TabIndex = 1;
            this.checkAudioRed.Text = "New Red";
            this.toolTip1.SetToolTip(this.checkAudioRed, "Perform audio alert when a new red appears in local");
            this.checkAudioRed.UseVisualStyleBackColor = true;
            this.checkAudioRed.CheckedChanged += new System.EventHandler(this.checkAudioRed_CheckedChanged);
            // 
            // checkAudioFlee
            // 
            this.checkAudioFlee.AutoSize = true;
            this.checkAudioFlee.Location = new System.Drawing.Point(6, 20);
            this.checkAudioFlee.Name = "checkAudioFlee";
            this.checkAudioFlee.Size = new System.Drawing.Size(46, 17);
            this.checkAudioFlee.TabIndex = 0;
            this.checkAudioFlee.Text = "Flee";
            this.toolTip1.SetToolTip(this.checkAudioFlee, "Perform audio alert when a flee is triggered");
            this.checkAudioFlee.UseVisualStyleBackColor = true;
            this.checkAudioFlee.CheckedChanged += new System.EventHandler(this.checkAudioFlee_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.buttonAddWhiteList);
            this.tabPage4.Controls.Add(this.textWhiteListPilot);
            this.tabPage4.Controls.Add(this.listWhiteList);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(312, 304);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Whitelist";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // buttonAddWhiteList
            // 
            this.buttonAddWhiteList.Location = new System.Drawing.Point(226, 274);
            this.buttonAddWhiteList.Name = "buttonAddWhiteList";
            this.buttonAddWhiteList.Size = new System.Drawing.Size(80, 23);
            this.buttonAddWhiteList.TabIndex = 2;
            this.buttonAddWhiteList.Text = "Add";
            this.buttonAddWhiteList.UseVisualStyleBackColor = true;
            this.buttonAddWhiteList.Click += new System.EventHandler(this.buttonAddWhiteList_Click);
            // 
            // textWhiteListPilot
            // 
            this.textWhiteListPilot.Location = new System.Drawing.Point(6, 276);
            this.textWhiteListPilot.Name = "textWhiteListPilot";
            this.textWhiteListPilot.Size = new System.Drawing.Size(214, 21);
            this.textWhiteListPilot.TabIndex = 1;
            // 
            // listWhiteList
            // 
            this.listWhiteList.FormattingEnabled = true;
            this.listWhiteList.Location = new System.Drawing.Point(6, 6);
            this.listWhiteList.Name = "listWhiteList";
            this.listWhiteList.Size = new System.Drawing.Size(300, 264);
            this.listWhiteList.TabIndex = 0;
            this.listWhiteList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listWhiteList_KeyUp);
            // 
            // checkGridTraffic
            // 
            this.checkGridTraffic.AutoSize = true;
            this.checkGridTraffic.Location = new System.Drawing.Point(6, 158);
            this.checkGridTraffic.Name = "checkGridTraffic";
            this.checkGridTraffic.Size = new System.Drawing.Size(76, 17);
            this.checkGridTraffic.TabIndex = 6;
            this.checkGridTraffic.Text = "Grid Traffic";
            this.toolTip1.SetToolTip(this.checkGridTraffic, "Perform audio alert when a non fleet member is on grid");
            this.checkGridTraffic.UseVisualStyleBackColor = true;
            this.checkGridTraffic.CheckedChanged += new System.EventHandler(this.checkGridTraffic_CheckedChanged);
            // 
            // Security
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 353);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Security";
            this.Text = "Security";
            this.Load += new System.EventHandler(this.Security_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ThresholdGroup.ResumeLayout(false);
            this.ThresholdGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Threshold)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.StandingGroup.ResumeLayout(false);
            this.StandingGroup.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FleeWait)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SecureBookmarkVerify)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRate)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox SafeSubstring;
        private System.Windows.Forms.GroupBox StandingGroup;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox IncludeFleetMembers;
        private System.Windows.Forms.CheckBox IncludeAllianceMembers;
        private System.Windows.Forms.CheckBox IncludeCorpMembers;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblFleeWait;
        private System.Windows.Forms.TrackBar FleeWait;
        private System.Windows.Forms.ListView Triggers;
        private System.Windows.Forms.GroupBox ThresholdGroup;
        private System.Windows.Forms.Label ThresholdLabel;
        private System.Windows.Forms.TrackBar Threshold;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView FleeTypes;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.PictureBox SecureBookmarkVerify;
        private System.Windows.Forms.ComboBox SecureBookmark;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox checkAudioGrey;
        private System.Windows.Forms.CheckBox checkAudioBlue;
        private System.Windows.Forms.CheckBox checkAudioRed;
        private System.Windows.Forms.CheckBox checkAudioFlee;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ListBox listVoices;
        private System.Windows.Forms.TrackBar trackRate;
        private System.Windows.Forms.TrackBar trackVolume;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkLocal;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button buttonAddWhiteList;
        private System.Windows.Forms.TextBox textWhiteListPilot;
        private System.Windows.Forms.ListBox listWhiteList;
        private System.Windows.Forms.CheckBox checkChatInvite;
        private System.Windows.Forms.CheckBox checkGridTraffic;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
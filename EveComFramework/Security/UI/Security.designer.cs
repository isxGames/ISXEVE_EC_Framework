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
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem("In a pod");
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem("Negative standing pilot in local");
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem("Neutral standing pilot in local");
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem("Neutral to me only");
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem("Targeted by another player");
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem("Capacitor low");
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem("Shield low");
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem("Armor low");
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem("Flee to closest station");
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem("Flee to secure bookmark");
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem("Cycle safe bookmarks");
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkIncludeBroadcastTriggers = new System.Windows.Forms.CheckBox();
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
            this.checkBroadcastTrigger = new System.Windows.Forms.CheckBox();
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
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.trackVolume = new System.Windows.Forms.TrackBar();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.trackRate = new System.Windows.Forms.TrackBar();
            this.listVoices = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkGridTraffic = new System.Windows.Forms.CheckBox();
            this.checkChatInvite = new System.Windows.Forms.CheckBox();
            this.checkLocal = new System.Windows.Forms.CheckBox();
            this.checkAudioGrey = new System.Windows.Forms.CheckBox();
            this.checkAudioBlue = new System.Windows.Forms.CheckBox();
            this.checkAudioRed = new System.Windows.Forms.CheckBox();
            this.checkAudioFlee = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddWhiteList = new System.Windows.Forms.Button();
            this.textWhiteListPilot = new System.Windows.Forms.TextBox();
            this.listWhiteList = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkAlternateStationFlee = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
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
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackRate)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(344, 333);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkIncludeBroadcastTriggers);
            this.tabPage2.Controls.Add(this.ThresholdGroup);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.StandingGroup);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(336, 307);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Flee Triggers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkIncludeBroadcastTriggers
            // 
            this.checkIncludeBroadcastTriggers.AutoSize = true;
            this.checkIncludeBroadcastTriggers.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkIncludeBroadcastTriggers.Location = new System.Drawing.Point(3, 275);
            this.checkIncludeBroadcastTriggers.Name = "checkIncludeBroadcastTriggers";
            this.checkIncludeBroadcastTriggers.Size = new System.Drawing.Size(330, 17);
            this.checkIncludeBroadcastTriggers.TabIndex = 4;
            this.checkIncludeBroadcastTriggers.Text = "Include Broadcast Triggers";
            this.checkIncludeBroadcastTriggers.UseVisualStyleBackColor = true;
            this.checkIncludeBroadcastTriggers.CheckedChanged += new System.EventHandler(this.checkIncludeBroadcastTriggers_CheckedChanged);
            // 
            // ThresholdGroup
            // 
            this.ThresholdGroup.Controls.Add(this.ThresholdLabel);
            this.ThresholdGroup.Controls.Add(this.Threshold);
            this.ThresholdGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.ThresholdGroup.Location = new System.Drawing.Point(3, 187);
            this.ThresholdGroup.Name = "ThresholdGroup";
            this.ThresholdGroup.Size = new System.Drawing.Size(330, 88);
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
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(330, 184);
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
            this.Triggers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Triggers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem12.StateImageIndex = 0;
            listViewItem13.StateImageIndex = 0;
            listViewItem14.StateImageIndex = 0;
            listViewItem15.StateImageIndex = 0;
            listViewItem16.StateImageIndex = 0;
            listViewItem17.StateImageIndex = 0;
            listViewItem18.StateImageIndex = 0;
            listViewItem19.StateImageIndex = 0;
            this.Triggers.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19});
            this.Triggers.Location = new System.Drawing.Point(3, 17);
            this.Triggers.MultiSelect = false;
            this.Triggers.Name = "Triggers";
            this.Triggers.Scrollable = false;
            this.Triggers.Size = new System.Drawing.Size(324, 164);
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
            this.StandingGroup.Location = new System.Drawing.Point(3, 187);
            this.StandingGroup.Name = "StandingGroup";
            this.StandingGroup.Size = new System.Drawing.Size(330, 88);
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
            this.IncludeFleetMembers.Size = new System.Drawing.Size(134, 17);
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
            this.IncludeAllianceMembers.Size = new System.Drawing.Size(148, 17);
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
            this.IncludeCorpMembers.Size = new System.Drawing.Size(166, 17);
            this.IncludeCorpMembers.TabIndex = 0;
            this.IncludeCorpMembers.Tag = "If this is checked, corporate members will be included in the trigger.";
            this.IncludeCorpMembers.Text = "Include Corporation Members";
            this.IncludeCorpMembers.UseVisualStyleBackColor = true;
            this.IncludeCorpMembers.Click += new System.EventHandler(this.IncludeCorpMembers_CheckedChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(336, 307);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Flee Responses";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBroadcastTrigger
            // 
            this.checkBroadcastTrigger.AutoSize = true;
            this.checkBroadcastTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBroadcastTrigger.Location = new System.Drawing.Point(3, 3);
            this.checkBroadcastTrigger.Name = "checkBroadcastTrigger";
            this.checkBroadcastTrigger.Size = new System.Drawing.Size(103, 32);
            this.checkBroadcastTrigger.TabIndex = 4;
            this.checkBroadcastTrigger.Text = "Broadcast Trigger";
            this.checkBroadcastTrigger.UseVisualStyleBackColor = true;
            this.checkBroadcastTrigger.CheckedChanged += new System.EventHandler(this.checkBroadcastTrigger_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblFleeWait);
            this.groupBox4.Controls.Add(this.FleeWait);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 199);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(330, 74);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Cooldown";
            // 
            // lblFleeWait
            // 
            this.lblFleeWait.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFleeWait.Location = new System.Drawing.Point(7, 43);
            this.lblFleeWait.Name = "lblFleeWait";
            this.lblFleeWait.Size = new System.Drawing.Size(320, 23);
            this.lblFleeWait.TabIndex = 7;
            this.lblFleeWait.Text = "Wait 5 minutes after flee";
            this.lblFleeWait.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FleeWait
            // 
            this.FleeWait.BackColor = System.Drawing.SystemColors.Control;
            this.FleeWait.Location = new System.Drawing.Point(7, 18);
            this.FleeWait.Maximum = 100;
            this.FleeWait.Name = "FleeWait";
            this.FleeWait.Size = new System.Drawing.Size(320, 45);
            this.FleeWait.TabIndex = 6;
            this.FleeWait.Tag = "Use this slider to set the threshold for the corresponding Flee Trigger.";
            this.FleeWait.Value = 5;
            this.FleeWait.Scroll += new System.EventHandler(this.FleeWait_Scroll);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SafeSubstring);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 150);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(330, 49);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Safe Substring";
            // 
            // SafeSubstring
            // 
            this.SafeSubstring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SafeSubstring.Location = new System.Drawing.Point(3, 17);
            this.SafeSubstring.Name = "SafeSubstring";
            this.SafeSubstring.Size = new System.Drawing.Size(324, 21);
            this.SafeSubstring.TabIndex = 0;
            this.SafeSubstring.Tag = "Substring to use to identify safe bookmarks.  Any bookmark in-system which contai" +
                "ns this substring will be used as a safe bookmark.";
            this.SafeSubstring.TextChanged += new System.EventHandler(this.SafeSubstring_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SecureBookmarkVerify);
            this.groupBox2.Controls.Add(this.SecureBookmark);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 49);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Secure Bookmark";
            // 
            // SecureBookmarkVerify
            // 
            this.SecureBookmarkVerify.Dock = System.Windows.Forms.DockStyle.Right;
            this.SecureBookmarkVerify.Image = global::EveComFramework.Properties.Resources.action_delete;
            this.SecureBookmarkVerify.Location = new System.Drawing.Point(307, 17);
            this.SecureBookmarkVerify.Name = "SecureBookmarkVerify";
            this.SecureBookmarkVerify.Size = new System.Drawing.Size(20, 29);
            this.SecureBookmarkVerify.TabIndex = 3;
            this.SecureBookmarkVerify.TabStop = false;
            // 
            // SecureBookmark
            // 
            this.SecureBookmark.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.SecureBookmark.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.SecureBookmark.Dock = System.Windows.Forms.DockStyle.Left;
            this.SecureBookmark.FormattingEnabled = true;
            this.SecureBookmark.Location = new System.Drawing.Point(3, 17);
            this.SecureBookmark.Name = "SecureBookmark";
            this.SecureBookmark.Size = new System.Drawing.Size(298, 21);
            this.SecureBookmark.TabIndex = 2;
            this.SecureBookmark.TextChanged += new System.EventHandler(this.SecureBookmark_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FleeTypes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 98);
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
            this.FleeTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FleeTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem20.StateImageIndex = 0;
            listViewItem21.StateImageIndex = 0;
            listViewItem22.StateImageIndex = 0;
            this.FleeTypes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem20,
            listViewItem21,
            listViewItem22});
            this.FleeTypes.Location = new System.Drawing.Point(3, 17);
            this.FleeTypes.Name = "FleeTypes";
            this.FleeTypes.Scrollable = false;
            this.FleeTypes.Size = new System.Drawing.Size(324, 78);
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
            this.tabPage3.Size = new System.Drawing.Size(336, 307);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Audio Alerts";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.groupBox9);
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Controls.Add(this.listVoices);
            this.groupBox7.Location = new System.Drawing.Point(110, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(218, 292);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Voices";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.trackVolume);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox9.Location = new System.Drawing.Point(3, 201);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(212, 65);
            this.groupBox9.TabIndex = 6;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Speech Volume";
            // 
            // trackVolume
            // 
            this.trackVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackVolume.Location = new System.Drawing.Point(3, 17);
            this.trackVolume.Maximum = 100;
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.Size = new System.Drawing.Size(206, 45);
            this.trackVolume.TabIndex = 4;
            this.trackVolume.Value = 100;
            this.trackVolume.Scroll += new System.EventHandler(this.trackVolume_Scroll);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.trackRate);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox8.Location = new System.Drawing.Point(3, 138);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(212, 63);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Speech Rate";
            // 
            // trackRate
            // 
            this.trackRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackRate.Location = new System.Drawing.Point(3, 17);
            this.trackRate.Minimum = -10;
            this.trackRate.Name = "trackRate";
            this.trackRate.Size = new System.Drawing.Size(206, 43);
            this.trackRate.TabIndex = 1;
            this.trackRate.Scroll += new System.EventHandler(this.trackRate_Scroll);
            // 
            // listVoices
            // 
            this.listVoices.Dock = System.Windows.Forms.DockStyle.Top;
            this.listVoices.FormattingEnabled = true;
            this.listVoices.Location = new System.Drawing.Point(3, 17);
            this.listVoices.Name = "listVoices";
            this.listVoices.Size = new System.Drawing.Size(212, 121);
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
            this.groupBox6.Size = new System.Drawing.Size(101, 292);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Enabled Alerts";
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
            this.tabPage4.Controls.Add(this.tableLayoutPanel1);
            this.tabPage4.Controls.Add(this.listWhiteList);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(336, 307);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Whitelist";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.72727F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tableLayoutPanel1.Controls.Add(this.buttonAddWhiteList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textWhiteListPilot, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 254);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(330, 32);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonAddWhiteList
            // 
            this.buttonAddWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddWhiteList.Location = new System.Drawing.Point(242, 3);
            this.buttonAddWhiteList.Name = "buttonAddWhiteList";
            this.buttonAddWhiteList.Size = new System.Drawing.Size(85, 26);
            this.buttonAddWhiteList.TabIndex = 2;
            this.buttonAddWhiteList.Text = "Add";
            this.buttonAddWhiteList.UseVisualStyleBackColor = true;
            this.buttonAddWhiteList.Click += new System.EventHandler(this.buttonAddWhiteList_Click);
            // 
            // textWhiteListPilot
            // 
            this.textWhiteListPilot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textWhiteListPilot.Location = new System.Drawing.Point(3, 3);
            this.textWhiteListPilot.Name = "textWhiteListPilot";
            this.textWhiteListPilot.Size = new System.Drawing.Size(233, 21);
            this.textWhiteListPilot.TabIndex = 1;
            // 
            // listWhiteList
            // 
            this.listWhiteList.Dock = System.Windows.Forms.DockStyle.Top;
            this.listWhiteList.FormattingEnabled = true;
            this.listWhiteList.Location = new System.Drawing.Point(3, 3);
            this.listWhiteList.Name = "listWhiteList";
            this.listWhiteList.Size = new System.Drawing.Size(330, 251);
            this.listWhiteList.TabIndex = 0;
            this.listWhiteList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listWhiteList_KeyUp);
            // 
            // checkAlternateStationFlee
            // 
            this.checkAlternateStationFlee.AutoSize = true;
            this.checkAlternateStationFlee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkAlternateStationFlee.Location = new System.Drawing.Point(112, 3);
            this.checkAlternateStationFlee.Name = "checkAlternateStationFlee";
            this.checkAlternateStationFlee.Size = new System.Drawing.Size(215, 32);
            this.checkAlternateStationFlee.TabIndex = 5;
            this.checkAlternateStationFlee.Text = "Flee to Station for Armor/Shield/Capacitor";
            this.checkAlternateStationFlee.UseVisualStyleBackColor = true;
            this.checkAlternateStationFlee.CheckedChanged += new System.EventHandler(this.checkAlternateStationFlee_CheckedChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel2.Controls.Add(this.checkBroadcastTrigger, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkAlternateStationFlee, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 273);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(330, 38);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // Security
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 333);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Security";
            this.Text = "Security";
            this.Load += new System.EventHandler(this.Security_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackRate)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkLocal;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button buttonAddWhiteList;
        private System.Windows.Forms.TextBox textWhiteListPilot;
        private System.Windows.Forms.ListBox listWhiteList;
        private System.Windows.Forms.CheckBox checkChatInvite;
        private System.Windows.Forms.CheckBox checkGridTraffic;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkIncludeBroadcastTriggers;
        private System.Windows.Forms.CheckBox checkBroadcastTrigger;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox checkAlternateStationFlee;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
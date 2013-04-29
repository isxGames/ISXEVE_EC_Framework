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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SafeSubstring = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SecureBookmark = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.FleeTypes = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.StandingGroup = new System.Windows.Forms.GroupBox();
            this.IncludeFleetMembers = new System.Windows.Forms.CheckBox();
            this.IncludeAllianceMembers = new System.Windows.Forms.CheckBox();
            this.IncludeCorpMembers = new System.Windows.Forms.CheckBox();
            this.ThresholdGroup = new System.Windows.Forms.GroupBox();
            this.ThresholdLabel = new System.Windows.Forms.Label();
            this.Threshold = new System.Windows.Forms.TrackBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Triggers = new System.Windows.Forms.CheckedListBox();
            this.lblHelpText = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblFleeWait = new System.Windows.Forms.Label();
            this.FleeWait = new System.Windows.Forms.TrackBar();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.StandingGroup.SuspendLayout();
            this.ThresholdGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Threshold)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FleeWait)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(320, 330);
            this.tabControl1.TabIndex = 0;
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
            this.groupBox2.Controls.Add(this.SecureBookmark);
            this.groupBox2.Location = new System.Drawing.Point(6, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 49);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Secure Bookmark";
            // 
            // SecureBookmark
            // 
            this.SecureBookmark.Location = new System.Drawing.Point(7, 20);
            this.SecureBookmark.Name = "SecureBookmark";
            this.SecureBookmark.Size = new System.Drawing.Size(283, 21);
            this.SecureBookmark.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.FleeTypes);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flee Response";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(218, 52);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 35);
            this.button3.TabIndex = 5;
            this.button3.Tag = "Move the selected Anomaly down in the priority list";
            this.button3.Text = "↓";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(218, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 33);
            this.button2.TabIndex = 4;
            this.button2.Tag = "Move the selected Anomaly up in the priority list";
            this.button2.Text = "↑";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FleeTypes
            // 
            this.FleeTypes.FormattingEnabled = true;
            this.FleeTypes.Items.AddRange(new object[] {
            "Flee to closest station in system",
            "Flee to secure bookmark",
            "Cycle safe bookmarks",
            "Flee to closest station outside system"});
            this.FleeTypes.Location = new System.Drawing.Point(6, 19);
            this.FleeTypes.Name = "FleeTypes";
            this.FleeTypes.Size = new System.Drawing.Size(206, 68);
            this.FleeTypes.TabIndex = 1;
            this.FleeTypes.Tag = "Use this list to determine how ComBot should react when a Flee Trigger is activat" +
                "ed.";
            this.FleeTypes.SelectedIndexChanged += new System.EventHandler(this.FleeTypes_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ThresholdGroup);
            this.tabPage2.Controls.Add(this.StandingGroup);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(312, 304);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Flee Triggers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // StandingGroup
            // 
            this.StandingGroup.Controls.Add(this.IncludeFleetMembers);
            this.StandingGroup.Controls.Add(this.IncludeAllianceMembers);
            this.StandingGroup.Controls.Add(this.IncludeCorpMembers);
            this.StandingGroup.Location = new System.Drawing.Point(6, 159);
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
            // ThresholdGroup
            // 
            this.ThresholdGroup.Controls.Add(this.ThresholdLabel);
            this.ThresholdGroup.Controls.Add(this.Threshold);
            this.ThresholdGroup.Location = new System.Drawing.Point(6, 159);
            this.ThresholdGroup.Name = "ThresholdGroup";
            this.ThresholdGroup.Size = new System.Drawing.Size(300, 88);
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
            this.groupBox5.Size = new System.Drawing.Size(300, 147);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Inactive Triggers";
            // 
            // Triggers
            // 
            this.Triggers.FormattingEnabled = true;
            this.Triggers.Items.AddRange(new object[] {
            "In a pod",
            "Negative standing pilot in local",
            "Neutral standing pilot in local",
            "Targeted by another player",
            "Capacitor low",
            "Shield low",
            "Armor low"});
            this.Triggers.Location = new System.Drawing.Point(6, 20);
            this.Triggers.Name = "Triggers";
            this.Triggers.Size = new System.Drawing.Size(288, 116);
            this.Triggers.TabIndex = 2;
            this.Triggers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Triggers_ItemCheck);
            this.Triggers.SelectedIndexChanged += new System.EventHandler(this.Triggers_SelectedIndexChanged);
            // 
            // lblHelpText
            // 
            this.lblHelpText.Location = new System.Drawing.Point(9, 345);
            this.lblHelpText.Name = "lblHelpText";
            this.lblHelpText.Size = new System.Drawing.Size(323, 52);
            this.lblHelpText.TabIndex = 3;
            this.lblHelpText.Text = "Mouseover an element for help";
            this.lblHelpText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // Security
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 406);
            this.Controls.Add(this.lblHelpText);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Security";
            this.Text = "Security";
            this.Load += new System.EventHandler(this.Security_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.StandingGroup.ResumeLayout(false);
            this.StandingGroup.PerformLayout();
            this.ThresholdGroup.ResumeLayout(false);
            this.ThresholdGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Threshold)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FleeWait)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox FleeTypes;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblHelpText;
        private System.Windows.Forms.TextBox SafeSubstring;
        private System.Windows.Forms.GroupBox StandingGroup;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox IncludeFleetMembers;
        private System.Windows.Forms.CheckBox IncludeAllianceMembers;
        private System.Windows.Forms.CheckBox IncludeCorpMembers;
        private System.Windows.Forms.GroupBox ThresholdGroup;
        private System.Windows.Forms.Label ThresholdLabel;
        private System.Windows.Forms.TrackBar Threshold;
        private System.Windows.Forms.TextBox SecureBookmark;
        private System.Windows.Forms.CheckedListBox Triggers;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblFleeWait;
        private System.Windows.Forms.TrackBar FleeWait;
    }
}
namespace EveComFramework.DroneControl.UI
{
    partial class DroneControl
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblSentryCountLimit = new System.Windows.Forms.Label();
            this.lblSentryDistanceLimit = new System.Windows.Forms.Label();
            this.SentryCountLimit = new System.Windows.Forms.TrackBar();
            this.SentryDistanceLimit = new System.Windows.Forms.TrackBar();
            this.Sentries = new System.Windows.Forms.CheckBox();
            this.lblSentryRange = new System.Windows.Forms.Label();
            this.SentryRange = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCombatTargetsReserved = new System.Windows.Forms.Label();
            this.lblCombatTimeout = new System.Windows.Forms.Label();
            this.CombatTargetsReserved = new System.Windows.Forms.TrackBar();
            this.CombatDrones = new System.Windows.Forms.CheckBox();
            this.CombatTimeout = new System.Windows.Forms.TrackBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblSalvageLockCount = new System.Windows.Forms.Label();
            this.SalvageLockCount = new System.Windows.Forms.TrackBar();
            this.SalvageDrones = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblMiningDroneTargets = new System.Windows.Forms.Label();
            this.MiningLockCount = new System.Windows.Forms.TrackBar();
            this.MiningDrones = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblLogiDroneCount = new System.Windows.Forms.Label();
            this.LogiDroneCount = new System.Windows.Forms.TrackBar();
            this.lblLogiDroneTargets = new System.Windows.Forms.Label();
            this.LogiDroneTargets = new System.Windows.Forms.TrackBar();
            this.LogisticsDrones = new System.Windows.Forms.CheckBox();
            this.lblHelpText = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SentryCountLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SentryDistanceLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SentryRange)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CombatTargetsReserved)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombatTimeout)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SalvageLockCount)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MiningLockCount)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogiDroneCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogiDroneTargets)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 419);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(752, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Combat Drones";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lblSentryCountLimit);
            this.groupBox6.Controls.Add(this.lblSentryDistanceLimit);
            this.groupBox6.Controls.Add(this.SentryCountLimit);
            this.groupBox6.Controls.Add(this.SentryDistanceLimit);
            this.groupBox6.Controls.Add(this.Sentries);
            this.groupBox6.Controls.Add(this.lblSentryRange);
            this.groupBox6.Controls.Add(this.SentryRange);
            this.groupBox6.Location = new System.Drawing.Point(8, 177);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(363, 158);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sentry Drones";
            // 
            // lblSentryCountLimit
            // 
            this.lblSentryCountLimit.Location = new System.Drawing.Point(177, 121);
            this.lblSentryCountLimit.Name = "lblSentryCountLimit";
            this.lblSentryCountLimit.Size = new System.Drawing.Size(177, 34);
            this.lblSentryCountLimit.TabIndex = 10;
            this.lblSentryCountLimit.Text = "While there are more then 3 sentry drones";
            this.lblSentryCountLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSentryDistanceLimit
            // 
            this.lblSentryDistanceLimit.Location = new System.Drawing.Point(3, 121);
            this.lblSentryDistanceLimit.Name = "lblSentryDistanceLimit";
            this.lblSentryDistanceLimit.Size = new System.Drawing.Size(177, 34);
            this.lblSentryDistanceLimit.TabIndex = 8;
            this.lblSentryDistanceLimit.Text = "Don\'t switch to non-sentry drones farther than 40km";
            this.lblSentryDistanceLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SentryCountLimit
            // 
            this.SentryCountLimit.Location = new System.Drawing.Point(180, 94);
            this.SentryCountLimit.Maximum = 15;
            this.SentryCountLimit.Name = "SentryCountLimit";
            this.SentryCountLimit.Size = new System.Drawing.Size(174, 45);
            this.SentryCountLimit.TabIndex = 9;
            this.SentryCountLimit.TickStyle = System.Windows.Forms.TickStyle.None;
            this.SentryCountLimit.Value = 3;
            this.SentryCountLimit.Scroll += new System.EventHandler(this.SentryCountLimit_Scroll);
            // 
            // SentryDistanceLimit
            // 
            this.SentryDistanceLimit.Location = new System.Drawing.Point(6, 94);
            this.SentryDistanceLimit.Maximum = 100;
            this.SentryDistanceLimit.Name = "SentryDistanceLimit";
            this.SentryDistanceLimit.Size = new System.Drawing.Size(174, 45);
            this.SentryDistanceLimit.TabIndex = 7;
            this.SentryDistanceLimit.TickStyle = System.Windows.Forms.TickStyle.None;
            this.SentryDistanceLimit.Value = 20;
            this.SentryDistanceLimit.Scroll += new System.EventHandler(this.SentryDistanceLimit_Scroll);
            // 
            // Sentries
            // 
            this.Sentries.AutoSize = true;
            this.Sentries.Location = new System.Drawing.Point(11, 20);
            this.Sentries.Name = "Sentries";
            this.Sentries.Size = new System.Drawing.Size(111, 17);
            this.Sentries.TabIndex = 6;
            this.Sentries.Text = "Use sentry drones";
            this.Sentries.UseVisualStyleBackColor = true;
            this.Sentries.CheckedChanged += new System.EventHandler(this.SentryDrones_CheckedChanged);
            // 
            // lblSentryRange
            // 
            this.lblSentryRange.Location = new System.Drawing.Point(5, 70);
            this.lblSentryRange.Name = "lblSentryRange";
            this.lblSentryRange.Size = new System.Drawing.Size(351, 18);
            this.lblSentryRange.TabIndex = 5;
            this.lblSentryRange.Text = "Use sentry drones for targets over 20km away";
            this.lblSentryRange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SentryRange
            // 
            this.SentryRange.Location = new System.Drawing.Point(8, 43);
            this.SentryRange.Maximum = 100;
            this.SentryRange.Name = "SentryRange";
            this.SentryRange.Size = new System.Drawing.Size(348, 45);
            this.SentryRange.TabIndex = 4;
            this.SentryRange.TickStyle = System.Windows.Forms.TickStyle.None;
            this.SentryRange.Value = 20;
            this.SentryRange.Scroll += new System.EventHandler(this.SentryRange_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCombatTargetsReserved);
            this.groupBox2.Controls.Add(this.lblCombatTimeout);
            this.groupBox2.Controls.Add(this.CombatTargetsReserved);
            this.groupBox2.Controls.Add(this.CombatDrones);
            this.groupBox2.Controls.Add(this.CombatTimeout);
            this.groupBox2.Location = new System.Drawing.Point(8, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 165);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // lblCombatTargetsReserved
            // 
            this.lblCombatTargetsReserved.Location = new System.Drawing.Point(6, 70);
            this.lblCombatTargetsReserved.Name = "lblCombatTargetsReserved";
            this.lblCombatTargetsReserved.Size = new System.Drawing.Size(350, 18);
            this.lblCombatTargetsReserved.TabIndex = 3;
            this.lblCombatTargetsReserved.Text = "Use 2 target slots for combat drones";
            this.lblCombatTargetsReserved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCombatTimeout
            // 
            this.lblCombatTimeout.Location = new System.Drawing.Point(8, 121);
            this.lblCombatTimeout.Name = "lblCombatTimeout";
            this.lblCombatTimeout.Size = new System.Drawing.Size(347, 18);
            this.lblCombatTimeout.TabIndex = 8;
            this.lblCombatTimeout.Text = "Switch to utility drones after 10 seconds";
            this.lblCombatTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CombatTargetsReserved
            // 
            this.CombatTargetsReserved.Location = new System.Drawing.Point(9, 43);
            this.CombatTargetsReserved.Name = "CombatTargetsReserved";
            this.CombatTargetsReserved.Size = new System.Drawing.Size(345, 45);
            this.CombatTargetsReserved.TabIndex = 2;
            this.CombatTargetsReserved.TickStyle = System.Windows.Forms.TickStyle.None;
            this.CombatTargetsReserved.Value = 2;
            this.CombatTargetsReserved.Scroll += new System.EventHandler(this.CombatTargetsReserved_Scroll);
            // 
            // CombatDrones
            // 
            this.CombatDrones.AutoSize = true;
            this.CombatDrones.Location = new System.Drawing.Point(9, 20);
            this.CombatDrones.Name = "CombatDrones";
            this.CombatDrones.Size = new System.Drawing.Size(117, 17);
            this.CombatDrones.TabIndex = 1;
            this.CombatDrones.Text = "Use combat drones";
            this.CombatDrones.UseVisualStyleBackColor = true;
            this.CombatDrones.CheckedChanged += new System.EventHandler(this.UseCombatDrones_CheckedChanged);
            // 
            // CombatTimeout
            // 
            this.CombatTimeout.Location = new System.Drawing.Point(11, 94);
            this.CombatTimeout.Maximum = 100;
            this.CombatTimeout.Name = "CombatTimeout";
            this.CombatTimeout.Size = new System.Drawing.Size(344, 45);
            this.CombatTimeout.TabIndex = 7;
            this.CombatTimeout.TickStyle = System.Windows.Forms.TickStyle.None;
            this.CombatTimeout.Value = 20;
            this.CombatTimeout.Scroll += new System.EventHandler(this.CombatTimeout_Scroll);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(752, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Utility Drones";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblSalvageLockCount);
            this.groupBox5.Controls.Add(this.SalvageLockCount);
            this.groupBox5.Controls.Add(this.SalvageDrones);
            this.groupBox5.Location = new System.Drawing.Point(8, 102);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(371, 90);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Salvage Drones";
            // 
            // lblSalvageLockCount
            // 
            this.lblSalvageLockCount.Location = new System.Drawing.Point(5, 66);
            this.lblSalvageLockCount.Name = "lblSalvageLockCount";
            this.lblSalvageLockCount.Size = new System.Drawing.Size(360, 18);
            this.lblSalvageLockCount.TabIndex = 8;
            this.lblSalvageLockCount.Text = "Use 2 target slots for salvage drones";
            this.lblSalvageLockCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SalvageLockCount
            // 
            this.SalvageLockCount.Location = new System.Drawing.Point(8, 39);
            this.SalvageLockCount.Maximum = 12;
            this.SalvageLockCount.Name = "SalvageLockCount";
            this.SalvageLockCount.Size = new System.Drawing.Size(357, 45);
            this.SalvageLockCount.TabIndex = 7;
            this.SalvageLockCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.SalvageLockCount.Value = 2;
            this.SalvageLockCount.Scroll += new System.EventHandler(this.SalvageDroneTargets_Scroll);
            // 
            // SalvageDrones
            // 
            this.SalvageDrones.AutoSize = true;
            this.SalvageDrones.Location = new System.Drawing.Point(8, 16);
            this.SalvageDrones.Name = "SalvageDrones";
            this.SalvageDrones.Size = new System.Drawing.Size(117, 17);
            this.SalvageDrones.TabIndex = 6;
            this.SalvageDrones.Text = "Use salvage drones";
            this.SalvageDrones.UseVisualStyleBackColor = true;
            this.SalvageDrones.CheckedChanged += new System.EventHandler(this.SalvageDrones_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblMiningDroneTargets);
            this.groupBox4.Controls.Add(this.MiningLockCount);
            this.groupBox4.Controls.Add(this.MiningDrones);
            this.groupBox4.Location = new System.Drawing.Point(8, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(371, 90);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mining Drones";
            // 
            // lblMiningDroneTargets
            // 
            this.lblMiningDroneTargets.Location = new System.Drawing.Point(5, 66);
            this.lblMiningDroneTargets.Name = "lblMiningDroneTargets";
            this.lblMiningDroneTargets.Size = new System.Drawing.Size(360, 18);
            this.lblMiningDroneTargets.TabIndex = 8;
            this.lblMiningDroneTargets.Text = "Use 2 target slots for mining drones";
            this.lblMiningDroneTargets.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MiningLockCount
            // 
            this.MiningLockCount.Location = new System.Drawing.Point(8, 39);
            this.MiningLockCount.Maximum = 12;
            this.MiningLockCount.Name = "MiningLockCount";
            this.MiningLockCount.Size = new System.Drawing.Size(357, 45);
            this.MiningLockCount.TabIndex = 7;
            this.MiningLockCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MiningLockCount.Value = 2;
            this.MiningLockCount.Scroll += new System.EventHandler(this.MiningDroneTargets_Scroll);
            // 
            // MiningDrones
            // 
            this.MiningDrones.AutoSize = true;
            this.MiningDrones.Location = new System.Drawing.Point(8, 16);
            this.MiningDrones.Name = "MiningDrones";
            this.MiningDrones.Size = new System.Drawing.Size(113, 17);
            this.MiningDrones.TabIndex = 6;
            this.MiningDrones.Text = "Use mining drones";
            this.MiningDrones.UseVisualStyleBackColor = true;
            this.MiningDrones.CheckedChanged += new System.EventHandler(this.MiningDrones_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblLogiDroneCount);
            this.groupBox3.Controls.Add(this.LogiDroneCount);
            this.groupBox3.Controls.Add(this.lblLogiDroneTargets);
            this.groupBox3.Controls.Add(this.LogiDroneTargets);
            this.groupBox3.Controls.Add(this.LogisticsDrones);
            this.groupBox3.Location = new System.Drawing.Point(385, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(381, 147);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Logistics Drones";
            // 
            // lblLogiDroneCount
            // 
            this.lblLogiDroneCount.Location = new System.Drawing.Point(3, 70);
            this.lblLogiDroneCount.Name = "lblLogiDroneCount";
            this.lblLogiDroneCount.Size = new System.Drawing.Size(372, 18);
            this.lblLogiDroneCount.TabIndex = 5;
            this.lblLogiDroneCount.Text = "Use 2 drone slots for logistics drones";
            this.lblLogiDroneCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogiDroneCount
            // 
            this.LogiDroneCount.Location = new System.Drawing.Point(6, 43);
            this.LogiDroneCount.Name = "LogiDroneCount";
            this.LogiDroneCount.Size = new System.Drawing.Size(369, 45);
            this.LogiDroneCount.TabIndex = 8;
            this.LogiDroneCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.LogiDroneCount.Value = 2;
            this.LogiDroneCount.Scroll += new System.EventHandler(this.LogiDroneCount_Scroll);
            // 
            // lblLogiDroneTargets
            // 
            this.lblLogiDroneTargets.Location = new System.Drawing.Point(3, 121);
            this.lblLogiDroneTargets.Name = "lblLogiDroneTargets";
            this.lblLogiDroneTargets.Size = new System.Drawing.Size(372, 18);
            this.lblLogiDroneTargets.TabIndex = 7;
            this.lblLogiDroneTargets.Text = "Use 2 target slots for logistics drones";
            this.lblLogiDroneTargets.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogiDroneTargets
            // 
            this.LogiDroneTargets.Location = new System.Drawing.Point(6, 94);
            this.LogiDroneTargets.Maximum = 12;
            this.LogiDroneTargets.Name = "LogiDroneTargets";
            this.LogiDroneTargets.Size = new System.Drawing.Size(369, 45);
            this.LogiDroneTargets.TabIndex = 6;
            this.LogiDroneTargets.TickStyle = System.Windows.Forms.TickStyle.None;
            this.LogiDroneTargets.Value = 2;
            this.LogiDroneTargets.Scroll += new System.EventHandler(this.LogiDroneTargets_Scroll);
            // 
            // LogisticsDrones
            // 
            this.LogisticsDrones.AutoSize = true;
            this.LogisticsDrones.Location = new System.Drawing.Point(6, 20);
            this.LogisticsDrones.Name = "LogisticsDrones";
            this.LogisticsDrones.Size = new System.Drawing.Size(119, 17);
            this.LogisticsDrones.TabIndex = 0;
            this.LogisticsDrones.Text = "Use logistics drones";
            this.LogisticsDrones.UseVisualStyleBackColor = true;
            this.LogisticsDrones.CheckedChanged += new System.EventHandler(this.LogisticsDrones_CheckedChanged);
            // 
            // lblHelpText
            // 
            this.lblHelpText.Location = new System.Drawing.Point(12, 434);
            this.lblHelpText.Name = "lblHelpText";
            this.lblHelpText.Size = new System.Drawing.Size(760, 52);
            this.lblHelpText.TabIndex = 8;
            this.lblHelpText.Text = "Mouseover an element for help";
            this.lblHelpText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DroneControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 493);
            this.Controls.Add(this.lblHelpText);
            this.Controls.Add(this.tabControl1);
            this.Name = "DroneControl";
            this.Text = "DroneControl";
            this.Load += new System.EventHandler(this.DroneControl_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SentryCountLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SentryDistanceLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SentryRange)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CombatTargetsReserved)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombatTimeout)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SalvageLockCount)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MiningLockCount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogiDroneCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogiDroneTargets)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblCombatTargetsReserved;
        private System.Windows.Forms.TrackBar CombatTargetsReserved;
        private System.Windows.Forms.CheckBox CombatDrones;
        private System.Windows.Forms.CheckBox Sentries;
        private System.Windows.Forms.Label lblSentryRange;
        private System.Windows.Forms.TrackBar SentryRange;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox LogisticsDrones;
        private System.Windows.Forms.Label lblLogiDroneCount;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblMiningDroneTargets;
        private System.Windows.Forms.TrackBar MiningLockCount;
        private System.Windows.Forms.CheckBox MiningDrones;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblSalvageLockCount;
        private System.Windows.Forms.TrackBar SalvageLockCount;
        private System.Windows.Forms.CheckBox SalvageDrones;
        private System.Windows.Forms.Label lblLogiDroneTargets;
        private System.Windows.Forms.TrackBar LogiDroneTargets;
        private System.Windows.Forms.TrackBar LogiDroneCount;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lblCombatTimeout;
        private System.Windows.Forms.TrackBar CombatTimeout;
        private System.Windows.Forms.Label lblSentryDistanceLimit;
        private System.Windows.Forms.TrackBar SentryDistanceLimit;
        private System.Windows.Forms.TrackBar SentryCountLimit;
        private System.Windows.Forms.Label lblSentryCountLimit;
        private System.Windows.Forms.Label lblHelpText;
    }
}
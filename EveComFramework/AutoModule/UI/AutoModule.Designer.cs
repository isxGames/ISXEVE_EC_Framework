namespace EveComFramework.AutoModule.UI
{
    partial class AutoModule
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Shield Boosters");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Armor Repairers");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Active Hardeners");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Cloaks");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Gang Links");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Sensor Boosters");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Tracking Computers");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("ECCMs");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Drone Control Units");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Propulsion Modules");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("ECMBursts");
            this.CapacitorThresholdLabel = new System.Windows.Forms.Label();
            this.CapacitorThreshold = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AlwaysActive = new System.Windows.Forms.CheckBox();
            this.ActivateOrbiting = new System.Windows.Forms.CheckBox();
            this.ActivateApproaching = new System.Windows.Forms.CheckBox();
            this.MaxThresholdLabel = new System.Windows.Forms.Label();
            this.MaxThreshold = new System.Windows.Forms.TrackBar();
            this.MinThresholdLabel = new System.Windows.Forms.Label();
            this.MinThreshold = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Modules = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.CapacitorThreshold)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinThreshold)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CapacitorThresholdLabel
            // 
            this.CapacitorThresholdLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CapacitorThresholdLabel.Location = new System.Drawing.Point(6, 46);
            this.CapacitorThresholdLabel.Name = "CapacitorThresholdLabel";
            this.CapacitorThresholdLabel.Size = new System.Drawing.Size(309, 19);
            this.CapacitorThresholdLabel.TabIndex = 3;
            this.CapacitorThresholdLabel.Text = "Activate if above 0% capacitor";
            this.CapacitorThresholdLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CapacitorThreshold
            // 
            this.CapacitorThreshold.BackColor = System.Drawing.SystemColors.Control;
            this.CapacitorThreshold.Location = new System.Drawing.Point(6, 20);
            this.CapacitorThreshold.Maximum = 100;
            this.CapacitorThreshold.Name = "CapacitorThreshold";
            this.CapacitorThreshold.Size = new System.Drawing.Size(309, 45);
            this.CapacitorThreshold.TabIndex = 2;
            this.CapacitorThreshold.Tag = "Use this slider to indicate how close ComBot should warp to ratting locations";
            this.CapacitorThreshold.Scroll += new System.EventHandler(this.CapacitorThreshold_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AlwaysActive);
            this.groupBox1.Controls.Add(this.ActivateOrbiting);
            this.groupBox1.Controls.Add(this.ActivateApproaching);
            this.groupBox1.Controls.Add(this.MaxThresholdLabel);
            this.groupBox1.Controls.Add(this.MaxThreshold);
            this.groupBox1.Controls.Add(this.MinThresholdLabel);
            this.groupBox1.Controls.Add(this.MinThreshold);
            this.groupBox1.Controls.Add(this.CapacitorThresholdLabel);
            this.groupBox1.Controls.Add(this.CapacitorThreshold);
            this.groupBox1.Location = new System.Drawing.Point(12, 310);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 171);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thresholds";
            // 
            // AlwaysActive
            // 
            this.AlwaysActive.AutoSize = true;
            this.AlwaysActive.Location = new System.Drawing.Point(9, 141);
            this.AlwaysActive.Name = "AlwaysActive";
            this.AlwaysActive.Size = new System.Drawing.Size(91, 17);
            this.AlwaysActive.TabIndex = 10;
            this.AlwaysActive.Text = "Always active";
            this.AlwaysActive.UseVisualStyleBackColor = true;
            this.AlwaysActive.Visible = false;
            this.AlwaysActive.CheckedChanged += new System.EventHandler(this.AlwaysActive_CheckedChanged);
            // 
            // ActivateOrbiting
            // 
            this.ActivateOrbiting.AutoSize = true;
            this.ActivateOrbiting.Location = new System.Drawing.Point(9, 91);
            this.ActivateOrbiting.Name = "ActivateOrbiting";
            this.ActivateOrbiting.Size = new System.Drawing.Size(131, 17);
            this.ActivateOrbiting.TabIndex = 9;
            this.ActivateOrbiting.Text = "Activate when orbiting";
            this.ActivateOrbiting.UseVisualStyleBackColor = true;
            this.ActivateOrbiting.Visible = false;
            this.ActivateOrbiting.CheckedChanged += new System.EventHandler(this.ActivateOrbiting_CheckedChanged);
            // 
            // ActivateApproaching
            // 
            this.ActivateApproaching.AutoSize = true;
            this.ActivateApproaching.Location = new System.Drawing.Point(9, 68);
            this.ActivateApproaching.Name = "ActivateApproaching";
            this.ActivateApproaching.Size = new System.Drawing.Size(156, 17);
            this.ActivateApproaching.TabIndex = 8;
            this.ActivateApproaching.Text = "Activate when approaching";
            this.ActivateApproaching.UseVisualStyleBackColor = true;
            this.ActivateApproaching.Visible = false;
            this.ActivateApproaching.CheckedChanged += new System.EventHandler(this.ActivateApproaching_CheckedChanged);
            // 
            // MaxThresholdLabel
            // 
            this.MaxThresholdLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxThresholdLabel.Location = new System.Drawing.Point(6, 142);
            this.MaxThresholdLabel.Name = "MaxThresholdLabel";
            this.MaxThresholdLabel.Size = new System.Drawing.Size(309, 19);
            this.MaxThresholdLabel.TabIndex = 7;
            this.MaxThresholdLabel.Text = "Activate if above 0% shields";
            this.MaxThresholdLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MaxThreshold
            // 
            this.MaxThreshold.BackColor = System.Drawing.SystemColors.Control;
            this.MaxThreshold.Location = new System.Drawing.Point(6, 116);
            this.MaxThreshold.Maximum = 100;
            this.MaxThreshold.Name = "MaxThreshold";
            this.MaxThreshold.Size = new System.Drawing.Size(309, 45);
            this.MaxThreshold.TabIndex = 6;
            this.MaxThreshold.Tag = "Use this slider to indicate how close ComBot should warp to ratting locations";
            this.MaxThreshold.Scroll += new System.EventHandler(this.MaxThreshold_Scroll);
            // 
            // MinThresholdLabel
            // 
            this.MinThresholdLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinThresholdLabel.Location = new System.Drawing.Point(6, 94);
            this.MinThresholdLabel.Name = "MinThresholdLabel";
            this.MinThresholdLabel.Size = new System.Drawing.Size(309, 19);
            this.MinThresholdLabel.TabIndex = 5;
            this.MinThresholdLabel.Text = "Deactivate if above 0% shields";
            this.MinThresholdLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MinThreshold
            // 
            this.MinThreshold.BackColor = System.Drawing.SystemColors.Control;
            this.MinThreshold.Location = new System.Drawing.Point(6, 68);
            this.MinThreshold.Maximum = 100;
            this.MinThreshold.Name = "MinThreshold";
            this.MinThreshold.Size = new System.Drawing.Size(309, 45);
            this.MinThreshold.TabIndex = 4;
            this.MinThreshold.Tag = "Use this slider to indicate how close ComBot should warp to ratting locations";
            this.MinThreshold.Scroll += new System.EventHandler(this.MinThreshold_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Modules);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 292);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Modules";
            // 
            // Modules
            // 
            this.Modules.CheckBoxes = true;
            this.Modules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.Modules.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.StateImageIndex = 0;
            listViewItem10.StateImageIndex = 0;
            this.Modules.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10});
            this.Modules.Location = new System.Drawing.Point(9, 20);
            this.Modules.MultiSelect = false;
            this.Modules.Name = "Modules";
            this.Modules.Size = new System.Drawing.Size(306, 252);
            this.Modules.TabIndex = 1;
            this.Modules.UseCompatibleStateImageBehavior = false;
            this.Modules.View = System.Windows.Forms.View.Details;
            this.Modules.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.Modules_ItemChecked);
            this.Modules.SelectedIndexChanged += new System.EventHandler(this.Modules_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // AutoModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 493);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AutoModule";
            this.Text = "AutoModule";
            this.Load += new System.EventHandler(this.AutoModule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CapacitorThreshold)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinThreshold)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label CapacitorThresholdLabel;
        private System.Windows.Forms.TrackBar CapacitorThreshold;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label MaxThresholdLabel;
        private System.Windows.Forms.TrackBar MaxThreshold;
        private System.Windows.Forms.Label MinThresholdLabel;
        private System.Windows.Forms.TrackBar MinThreshold;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox AlwaysActive;
        private System.Windows.Forms.CheckBox ActivateOrbiting;
        private System.Windows.Forms.CheckBox ActivateApproaching;
        private System.Windows.Forms.ListView Modules;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
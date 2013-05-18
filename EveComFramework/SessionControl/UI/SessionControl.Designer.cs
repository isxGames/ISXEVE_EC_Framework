namespace EveComFramework.SessionControl.UI
{
    partial class SessionControl
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
            this.profileListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addProfileButton = new System.Windows.Forms.Button();
            this.pPasswordBox = new System.Windows.Forms.TextBox();
            this.pUNameBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numDowntimeDelta = new System.Windows.Forms.NumericUpDown();
            this.numLogoutHoursDelta = new System.Windows.Forms.NumericUpDown();
            this.numDowntime = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numLogoutHours = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numLoginDelta = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDowntimeDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogoutHoursDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDowntime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogoutHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoginDelta)).BeginInit();
            this.SuspendLayout();
            // 
            // profileListView
            // 
            this.profileListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.profileListView.FullRowSelect = true;
            this.profileListView.Location = new System.Drawing.Point(6, 19);
            this.profileListView.MultiSelect = false;
            this.profileListView.Name = "profileListView";
            this.profileListView.Size = new System.Drawing.Size(577, 180);
            this.profileListView.TabIndex = 0;
            this.profileListView.UseCompatibleStateImageBehavior = false;
            this.profileListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "CharacterName";
            this.columnHeader1.Width = 108;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Username";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Password";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "CharID";
            // 
            // addProfileButton
            // 
            this.addProfileButton.Location = new System.Drawing.Point(491, 14);
            this.addProfileButton.Name = "addProfileButton";
            this.addProfileButton.Size = new System.Drawing.Size(92, 21);
            this.addProfileButton.TabIndex = 2;
            this.addProfileButton.Text = "Create Profile";
            this.addProfileButton.UseVisualStyleBackColor = true;
            this.addProfileButton.Click += new System.EventHandler(this.addProfileButton_Click);
            // 
            // pPasswordBox
            // 
            this.pPasswordBox.Location = new System.Drawing.Point(327, 14);
            this.pPasswordBox.Name = "pPasswordBox";
            this.pPasswordBox.Size = new System.Drawing.Size(158, 21);
            this.pPasswordBox.TabIndex = 3;
            // 
            // pUNameBox
            // 
            this.pUNameBox.Location = new System.Drawing.Point(73, 14);
            this.pUNameBox.Name = "pUNameBox";
            this.pUNameBox.Size = new System.Drawing.Size(184, 21);
            this.pUNameBox.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.profileListView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(589, 237);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profiles";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(6, 205);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(577, 26);
            this.button3.TabIndex = 2;
            this.button3.Text = "Remove Selected Profile";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.removeProfile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.pPasswordBox);
            this.groupBox2.Controls.Add(this.pUNameBox);
            this.groupBox2.Controls.Add(this.addProfileButton);
            this.groupBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.groupBox2.Location = new System.Drawing.Point(12, 255);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(589, 44);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create profile for current character (Must be logged in!)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(263, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Password :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Username :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numDowntimeDelta);
            this.groupBox3.Controls.Add(this.numLogoutHoursDelta);
            this.groupBox3.Controls.Add(this.numDowntime);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.numLogoutHours);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.numLoginDelta);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 305);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(589, 100);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Automation Settings (Linked to profile, not character!)";
            // 
            // numDowntimeDelta
            // 
            this.numDowntimeDelta.Location = new System.Drawing.Point(378, 63);
            this.numDowntimeDelta.Name = "numDowntimeDelta";
            this.numDowntimeDelta.Size = new System.Drawing.Size(44, 21);
            this.numDowntimeDelta.TabIndex = 7;
            this.numDowntimeDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numDowntimeDelta.ValueChanged += new System.EventHandler(this.numDowntimeDelta_ValueChanged);
            // 
            // numLogoutHoursDelta
            // 
            this.numLogoutHoursDelta.Location = new System.Drawing.Point(439, 39);
            this.numLogoutHoursDelta.Name = "numLogoutHoursDelta";
            this.numLogoutHoursDelta.Size = new System.Drawing.Size(44, 21);
            this.numLogoutHoursDelta.TabIndex = 6;
            this.numLogoutHoursDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numLogoutHoursDelta.ValueChanged += new System.EventHandler(this.numLogoutHoursDelta_ValueChanged);
            // 
            // numDowntime
            // 
            this.numDowntime.Location = new System.Drawing.Point(54, 63);
            this.numDowntime.Name = "numDowntime";
            this.numDowntime.Size = new System.Drawing.Size(44, 21);
            this.numDowntime.TabIndex = 5;
            this.numDowntime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numDowntime.ValueChanged += new System.EventHandler(this.numDowntime_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(470, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Log out                  minutes before downtime.  Add a random of up to         " +
                "         minutes.";
            // 
            // numLogoutHours
            // 
            this.numLogoutHours.Location = new System.Drawing.Point(54, 39);
            this.numLogoutHours.Name = "numLogoutHours";
            this.numLogoutHours.Size = new System.Drawing.Size(44, 21);
            this.numLogoutHours.TabIndex = 3;
            this.numLogoutHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numLogoutHours.ValueChanged += new System.EventHandler(this.numLogoutHours_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(531, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Log out                  hours after logging in or starting bot.  Add a random of" +
                " up to                  minutes.";
            // 
            // numLoginDelta
            // 
            this.numLoginDelta.Location = new System.Drawing.Point(248, 15);
            this.numLoginDelta.Name = "numLoginDelta";
            this.numLoginDelta.Size = new System.Drawing.Size(44, 21);
            this.numLoginDelta.TabIndex = 1;
            this.numLoginDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numLoginDelta.ValueChanged += new System.EventHandler(this.numLoginDelta_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Log in after a random amount of time up to                  minutes";
            // 
            // SessionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 415);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionControl";
            this.Text = "SessionControl";
            this.Load += new System.EventHandler(this.LoginControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDowntimeDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogoutHoursDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDowntime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogoutHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoginDelta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView profileListView;
        private System.Windows.Forms.Button addProfileButton;
        private System.Windows.Forms.TextBox pPasswordBox;
        private System.Windows.Forms.TextBox pUNameBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numDowntimeDelta;
        private System.Windows.Forms.NumericUpDown numLogoutHoursDelta;
        private System.Windows.Forms.NumericUpDown numDowntime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numLogoutHours;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numLoginDelta;
        private System.Windows.Forms.Label label1;
    }
}
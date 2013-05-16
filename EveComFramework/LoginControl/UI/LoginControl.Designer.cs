namespace EveComFramework.LoginControl.UI
{
    partial class LoginControl
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
            this.addProfileButton = new System.Windows.Forms.Button();
            this.pPasswordBox = new System.Windows.Forms.TextBox();
            this.pUNameBox = new System.Windows.Forms.TextBox();
            this.pCharIDBox = new System.Windows.Forms.TextBox();
            this.botComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pNameBox = new System.Windows.Forms.TextBox();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // profileListView
            // 
            this.profileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.profileListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.profileListView.FullRowSelect = true;
            this.profileListView.Location = new System.Drawing.Point(3, 16);
            this.profileListView.MultiSelect = false;
            this.profileListView.Name = "profileListView";
            this.profileListView.Size = new System.Drawing.Size(555, 306);
            this.profileListView.TabIndex = 0;
            this.profileListView.UseCompatibleStateImageBehavior = false;
            this.profileListView.View = System.Windows.Forms.View.Details;
            // 
            // addProfileButton
            // 
            this.addProfileButton.Location = new System.Drawing.Point(104, 155);
            this.addProfileButton.Name = "addProfileButton";
            this.addProfileButton.Size = new System.Drawing.Size(196, 26);
            this.addProfileButton.TabIndex = 2;
            this.addProfileButton.Text = "Create Profile";
            this.addProfileButton.UseVisualStyleBackColor = true;
            this.addProfileButton.Click += new System.EventHandler(this.addProfileButton_Click);
            // 
            // pPasswordBox
            // 
            this.pPasswordBox.Location = new System.Drawing.Point(104, 70);
            this.pPasswordBox.Name = "pPasswordBox";
            this.pPasswordBox.Size = new System.Drawing.Size(196, 20);
            this.pPasswordBox.TabIndex = 3;
            // 
            // pUNameBox
            // 
            this.pUNameBox.Location = new System.Drawing.Point(104, 44);
            this.pUNameBox.Name = "pUNameBox";
            this.pUNameBox.Size = new System.Drawing.Size(196, 20);
            this.pUNameBox.TabIndex = 4;
            // 
            // pCharIDBox
            // 
            this.pCharIDBox.Location = new System.Drawing.Point(104, 96);
            this.pCharIDBox.Name = "pCharIDBox";
            this.pCharIDBox.Size = new System.Drawing.Size(196, 20);
            this.pCharIDBox.TabIndex = 5;
            // 
            // botComboBox
            // 
            this.botComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.botComboBox.FormattingEnabled = true;
            this.botComboBox.Location = new System.Drawing.Point(104, 128);
            this.botComboBox.Name = "botComboBox";
            this.botComboBox.Size = new System.Drawing.Size(196, 21);
            this.botComboBox.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.profileListView);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 361);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profiles";
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button3.Location = new System.Drawing.Point(3, 328);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(555, 30);
            this.button3.TabIndex = 2;
            this.button3.Text = "Remove Selected Profile";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.removeProfile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.pNameBox);
            this.groupBox2.Controls.Add(this.pPasswordBox);
            this.groupBox2.Controls.Add(this.pUNameBox);
            this.groupBox2.Controls.Add(this.addProfileButton);
            this.groupBox2.Controls.Add(this.pCharIDBox);
            this.groupBox2.Controls.Add(this.botComboBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 361);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(561, 199);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Profile Creator";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Bot :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Profile CharID :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Profile Password :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Profile Username :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Profile Name :";
            // 
            // pNameBox
            // 
            this.pNameBox.Location = new System.Drawing.Point(104, 18);
            this.pNameBox.Name = "pNameBox";
            this.pNameBox.Size = new System.Drawing.Size(196, 20);
            this.pNameBox.TabIndex = 7;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
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
            // columnHeader5
            // 
            this.columnHeader5.Text = "Bot";
            // 
            // LoginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 562);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LoginControl";
            this.Text = "LoginControl";
            this.Load += new System.EventHandler(this.LoginControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView profileListView;
        private System.Windows.Forms.Button addProfileButton;
        private System.Windows.Forms.TextBox pPasswordBox;
        private System.Windows.Forms.TextBox pUNameBox;
        private System.Windows.Forms.TextBox pCharIDBox;
        private System.Windows.Forms.ComboBox botComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pNameBox;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}
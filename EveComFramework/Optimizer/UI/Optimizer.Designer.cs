namespace EveComFramework.Optimizer.UI
{
    partial class Optimizer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkDisable3D = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericMaxMemorySize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericMaxMemorySize)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkDisable3D);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 41);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Disable 3D Rendering";
            // 
            // checkDisable3D
            // 
            this.checkDisable3D.Location = new System.Drawing.Point(7, 18);
            this.checkDisable3D.Name = "checkDisable3D";
            this.checkDisable3D.Size = new System.Drawing.Size(123, 17);
            this.checkDisable3D.TabIndex = 0;
            this.checkDisable3D.Text = "Enabled";
            this.checkDisable3D.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkDisable3D.UseVisualStyleBackColor = true;
            this.checkDisable3D.CheckedChanged += new System.EventHandler(this.checkDisable3D_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericMaxMemorySize);
            this.groupBox2.Location = new System.Drawing.Point(12, 59);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(136, 48);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Restrict Memory Usage";
            // 
            // numericMaxMemorySize
            // 
            this.numericMaxMemorySize.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericMaxMemorySize.Location = new System.Drawing.Point(7, 19);
            this.numericMaxMemorySize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericMaxMemorySize.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericMaxMemorySize.Name = "numericMaxMemorySize";
            this.numericMaxMemorySize.Size = new System.Drawing.Size(87, 21);
            this.numericMaxMemorySize.TabIndex = 0;
            this.numericMaxMemorySize.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericMaxMemorySize.ValueChanged += new System.EventHandler(this.numericMaxMemorySize_ValueChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(100, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "MB";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Optimizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(157, 120);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Optimizer";
            this.Text = "Optimizer";
            this.Load += new System.EventHandler(this.Optimizer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericMaxMemorySize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkDisable3D;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericMaxMemorySize;
        private System.Windows.Forms.Label label1;
    }
}
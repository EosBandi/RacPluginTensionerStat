namespace MissionPlanner.RACPluginTensionerStat
{
    partial class Settings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nTimeout = new System.Windows.Forms.NumericUpDown();
            this.cbSafetyEnable = new System.Windows.Forms.CheckBox();
            this.nSafetyDelay = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nSafetyForce = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cbSevoNumber = new System.Windows.Forms.ComboBox();
            this.nServoOpen = new System.Windows.Forms.NumericUpDown();
            this.nServoClose = new System.Windows.Forms.NumericUpDown();
            this.tbURL = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSafetyDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSafetyForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nServoOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nServoClose)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tensioner IP address :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tensioner IP timeout :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(177, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Safety Disconnect enable :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Safety Disconnect force :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Safety Disconnect delay :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 336);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Release Close :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 258);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "Release Servo :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 297);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "Release Open :";
            // 
            // nTimeout
            // 
            this.nTimeout.Location = new System.Drawing.Point(193, 65);
            this.nTimeout.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nTimeout.Name = "nTimeout";
            this.nTimeout.Size = new System.Drawing.Size(120, 22);
            this.nTimeout.TabIndex = 2;
            this.nTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbSafetyEnable
            // 
            this.cbSafetyEnable.AutoSize = true;
            this.cbSafetyEnable.Location = new System.Drawing.Point(193, 118);
            this.cbSafetyEnable.Name = "cbSafetyEnable";
            this.cbSafetyEnable.Size = new System.Drawing.Size(18, 17);
            this.cbSafetyEnable.TabIndex = 3;
            this.cbSafetyEnable.UseVisualStyleBackColor = true;
            // 
            // nSafetyDelay
            // 
            this.nSafetyDelay.Location = new System.Drawing.Point(193, 155);
            this.nSafetyDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nSafetyDelay.Name = "nSafetyDelay";
            this.nSafetyDelay.Size = new System.Drawing.Size(120, 22);
            this.nSafetyDelay.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(320, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "mS";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(319, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 17);
            this.label10.TabIndex = 16;
            this.label10.Text = "mS";
            // 
            // nSafetyForce
            // 
            this.nSafetyForce.Location = new System.Drawing.Point(193, 196);
            this.nSafetyForce.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nSafetyForce.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nSafetyForce.Name = "nSafetyForce";
            this.nSafetyForce.Size = new System.Drawing.Size(120, 22);
            this.nSafetyForce.TabIndex = 5;
            this.nSafetyForce.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(320, 196);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 17);
            this.label11.TabIndex = 18;
            this.label11.Text = "Nm";
            // 
            // cbSevoNumber
            // 
            this.cbSevoNumber.FormattingEnabled = true;
            this.cbSevoNumber.Items.AddRange(new object[] {
            "9",
            "10",
            "11",
            "12",
            "13",
            "14"});
            this.cbSevoNumber.Location = new System.Drawing.Point(192, 255);
            this.cbSevoNumber.Name = "cbSevoNumber";
            this.cbSevoNumber.Size = new System.Drawing.Size(121, 24);
            this.cbSevoNumber.TabIndex = 19;
            this.cbSevoNumber.Text = "9";
            // 
            // nServoOpen
            // 
            this.nServoOpen.Location = new System.Drawing.Point(192, 295);
            this.nServoOpen.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nServoOpen.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nServoOpen.Name = "nServoOpen";
            this.nServoOpen.Size = new System.Drawing.Size(120, 22);
            this.nServoOpen.TabIndex = 6;
            this.nServoOpen.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // nServoClose
            // 
            this.nServoClose.Location = new System.Drawing.Point(192, 334);
            this.nServoClose.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nServoClose.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nServoClose.Name = "nServoClose";
            this.nServoClose.Size = new System.Drawing.Size(120, 22);
            this.nServoClose.TabIndex = 7;
            this.nServoClose.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(193, 23);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(215, 22);
            this.tbURL.TabIndex = 1;
            this.tbURL.Text = "http://192.168.0.100/data.xml";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 396);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(this.nServoClose);
            this.Controls.Add(this.nServoOpen);
            this.Controls.Add(this.cbSevoNumber);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.nSafetyForce);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.nSafetyDelay);
            this.Controls.Add(this.cbSafetyEnable);
            this.Controls.Add(this.nTimeout);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Settings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSafetyDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSafetyForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nServoOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nServoClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nTimeout;
        private System.Windows.Forms.CheckBox cbSafetyEnable;
        private System.Windows.Forms.NumericUpDown nSafetyDelay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nSafetyForce;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbSevoNumber;
        private System.Windows.Forms.NumericUpDown nServoOpen;
        private System.Windows.Forms.NumericUpDown nServoClose;
        private System.Windows.Forms.TextBox tbURL;
    }
}
namespace RTT
{
    partial class DebugForm
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
            this.radiovisacom = new System.Windows.Forms.RadioButton();
            this.radiovisa32 = new System.Windows.Forms.RadioButton();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radio_mscomm = new System.Windows.Forms.RadioButton();
            this.radio_SerialPort = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radiovisacom
            // 
            this.radiovisacom.AutoSize = true;
            this.radiovisacom.Location = new System.Drawing.Point(6, 19);
            this.radiovisacom.Name = "radiovisacom";
            this.radiovisacom.Size = new System.Drawing.Size(64, 17);
            this.radiovisacom.TabIndex = 0;
            this.radiovisacom.TabStop = true;
            this.radiovisacom.Text = "visacom";
            this.radiovisacom.UseVisualStyleBackColor = true;
            this.radiovisacom.CheckedChanged += new System.EventHandler(this.radiovisacom_CheckedChanged);
            // 
            // radiovisa32
            // 
            this.radiovisa32.AutoSize = true;
            this.radiovisa32.Location = new System.Drawing.Point(76, 19);
            this.radiovisa32.Name = "radiovisa32";
            this.radiovisa32.Size = new System.Drawing.Size(56, 17);
            this.radiovisa32.TabIndex = 1;
            this.radiovisa32.TabStop = true;
            this.radiovisa32.Text = "visa32";
            this.radiovisa32.UseVisualStyleBackColor = true;
            this.radiovisa32.CheckedChanged += new System.EventHandler(this.radiovisa32_CheckedChanged);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(47, 140);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(61, 23);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(113, 140);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(60, 23);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radiovisacom);
            this.groupBox1.Controls.Add(this.radiovisa32);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 49);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "visa select";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radio_mscomm);
            this.groupBox2.Controls.Add(this.radio_SerialPort);
            this.groupBox2.Location = new System.Drawing.Point(11, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 49);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Serial mode select";
            // 
            // radio_mscomm
            // 
            this.radio_mscomm.AutoSize = true;
            this.radio_mscomm.Location = new System.Drawing.Point(6, 19);
            this.radio_mscomm.Name = "radio_mscomm";
            this.radio_mscomm.Size = new System.Drawing.Size(68, 17);
            this.radio_mscomm.TabIndex = 0;
            this.radio_mscomm.TabStop = true;
            this.radio_mscomm.Text = "MsComm";
            this.radio_mscomm.UseVisualStyleBackColor = true;
            this.radio_mscomm.CheckedChanged += new System.EventHandler(this.radio_mscomm_CheckedChanged);
            // 
            // radio_SerialPort
            // 
            this.radio_SerialPort.AutoSize = true;
            this.radio_SerialPort.Location = new System.Drawing.Point(76, 19);
            this.radio_SerialPort.Name = "radio_SerialPort";
            this.radio_SerialPort.Size = new System.Drawing.Size(70, 17);
            this.radio_SerialPort.TabIndex = 1;
            this.radio_SerialPort.TabStop = true;
            this.radio_SerialPort.Text = "SerialPort";
            this.radio_SerialPort.UseVisualStyleBackColor = true;
            this.radio_SerialPort.CheckedChanged += new System.EventHandler(this.radio_SerialPort_CheckedChanged);
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(185, 175);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebugForm";
            this.Text = "DebugForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radiovisacom;
        private System.Windows.Forms.RadioButton radiovisa32;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radio_mscomm;
        private System.Windows.Forms.RadioButton radio_SerialPort;
    }
}
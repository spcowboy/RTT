namespace RTT
{
    partial class OtherDeviceForm
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
            this.label_Server_Port = new System.Windows.Forms.Label();
            this.textBox_Server_Port = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_du_ip = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_Server_Port
            // 
            this.label_Server_Port.AutoSize = true;
            this.label_Server_Port.Location = new System.Drawing.Point(12, 9);
            this.label_Server_Port.Name = "label_Server_Port";
            this.label_Server_Port.Size = new System.Drawing.Size(87, 13);
            this.label_Server_Port.TabIndex = 18;
            this.label_Server_Port.Text = "ScriptServer Port";
            // 
            // textBox_Server_Port
            // 
            this.textBox_Server_Port.Location = new System.Drawing.Point(105, 6);
            this.textBox_Server_Port.Name = "textBox_Server_Port";
            this.textBox_Server_Port.Size = new System.Drawing.Size(167, 20);
            this.textBox_Server_Port.TabIndex = 19;
            this.textBox_Server_Port.Text = "8001";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Du Ip address";
            // 
            // textBox_du_ip
            // 
            this.textBox_du_ip.Location = new System.Drawing.Point(105, 42);
            this.textBox_du_ip.Name = "textBox_du_ip";
            this.textBox_du_ip.Size = new System.Drawing.Size(167, 20);
            this.textBox_du_ip.TabIndex = 21;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(127, 88);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(69, 28);
            this.button_ok.TabIndex = 22;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(202, 88);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(70, 28);
            this.button_cancel.TabIndex = 23;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // OtherDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 128);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_du_ip);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_Server_Port);
            this.Controls.Add(this.label_Server_Port);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OtherDeviceForm";
            this.Text = "Other Device Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Server_Port;
        private System.Windows.Forms.TextBox textBox_Server_Port;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_du_ip;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
    }
}
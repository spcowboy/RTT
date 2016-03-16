namespace RTT
{
    partial class EVMForm
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
            this.text_timelimit = new System.Windows.Forms.TextBox();
            this.textCount = new System.Windows.Forms.TextBox();
            this.textInterval = new System.Windows.Forms.TextBox();
            this.button_start = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_cpriport = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "TimeLimit(second)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Count";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Interval(second)";
            // 
            // text_timelimit
            // 
            this.text_timelimit.Location = new System.Drawing.Point(172, 6);
            this.text_timelimit.Name = "text_timelimit";
            this.text_timelimit.Size = new System.Drawing.Size(100, 20);
            this.text_timelimit.TabIndex = 3;
            // 
            // textCount
            // 
            this.textCount.Location = new System.Drawing.Point(172, 28);
            this.textCount.Name = "textCount";
            this.textCount.Size = new System.Drawing.Size(100, 20);
            this.textCount.TabIndex = 4;
            // 
            // textInterval
            // 
            this.textInterval.Location = new System.Drawing.Point(172, 50);
            this.textInterval.Name = "textInterval";
            this.textInterval.Size = new System.Drawing.Size(100, 20);
            this.textInterval.TabIndex = 5;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(12, 123);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(98, 23);
            this.button_start.TabIndex = 6;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(172, 123);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(100, 23);
            this.button_stop.TabIndex = 7;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 152);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(260, 23);
            this.progressBar.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "CpriPort";
            // 
            // comboBox_cpriport
            // 
            this.comboBox_cpriport.FormattingEnabled = true;
            this.comboBox_cpriport.Location = new System.Drawing.Point(172, 79);
            this.comboBox_cpriport.Name = "comboBox_cpriport";
            this.comboBox_cpriport.Size = new System.Drawing.Size(100, 21);
            this.comboBox_cpriport.TabIndex = 10;
            // 
            // EVMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 187);
            this.Controls.Add(this.comboBox_cpriport);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.textInterval);
            this.Controls.Add(this.textCount);
            this.Controls.Add(this.text_timelimit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EVMForm";
            this.Text = "EVM";
            this.Load += new System.EventHandler(this.EVMForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox text_timelimit;
        private System.Windows.Forms.TextBox textCount;
        private System.Windows.Forms.TextBox textInterval;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_cpriport;
    }
}
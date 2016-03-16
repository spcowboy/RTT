namespace RTT
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.item_serial_device = new System.Windows.Forms.ToolStripMenuItem();
            this.visaDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.measureMentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rXEVMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sensitivityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mode_Switch = new System.Windows.Forms.ToolStripMenuItem();
            this.upgradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.rruConnButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.DC5767A_ON = new System.Windows.Forms.ToolStripButton();
            this.DC5767A_OFF = new System.Windows.Forms.ToolStripButton();
            this.SACAPTURE = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_capture2 = new System.Windows.Forms.ToolStripButton();
            this.Rumasterswitchbutton = new System.Windows.Forms.ToolStripButton();
            this.Serial2_conn_button = new System.Windows.Forms.ToolStripButton();
            this.serial2_pow_on = new System.Windows.Forms.ToolStripButton();
            this.serial2_pow_off = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_telnet = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.CmdProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.button_lock = new System.Windows.Forms.Button();
            this.button_load = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.comboBox_instrumentprefix = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_terminal_IP = new System.Windows.Forms.Label();
            this.comboBox_terminal_select = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl_display = new System.Windows.Forms.TabControl();
            this.tab_main_display = new System.Windows.Forms.TabPage();
            this.dataDisplayBox = new System.Windows.Forms.RichTextBox();
            this.cmenu_displayfont = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.backColorSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.foreColorSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage_remote = new System.Windows.Forms.TabPage();
            this.Display_remote = new System.Windows.Forms.TextBox();
            this.checkBox_secondary = new System.Windows.Forms.CheckBox();
            this.checkBox_Log = new System.Windows.Forms.CheckBox();
            this.checkBox_debug = new System.Windows.Forms.CheckBox();
            this.SerialpropertyBox = new System.Windows.Forms.GroupBox();
            this.radioC_L2 = new System.Windows.Forms.RadioButton();
            this.radio_LF2 = new System.Windows.Forms.RadioButton();
            this.radio_CR2 = new System.Windows.Forms.RadioButton();
            this.label_serial2baud = new System.Windows.Forms.Label();
            this.label_serial2port = new System.Windows.Forms.Label();
            this.label_rrubaud = new System.Windows.Forms.Label();
            this.label_rruport = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_pause = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoscrollDown = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tag_DC5767A = new System.Windows.Forms.Label();
            this.tag_rumaster = new System.Windows.Forms.Label();
            this.tag_is2 = new System.Windows.Forms.Label();
            this.tag_is1 = new System.Windows.Forms.Label();
            this.tag_rfbox2 = new System.Windows.Forms.Label();
            this.tag_rfbox1 = new System.Windows.Forms.Label();
            this.tag_sg2 = new System.Windows.Forms.Label();
            this.checkBoxHexSend = new System.Windows.Forms.CheckBox();
            this.tag_sg1 = new System.Windows.Forms.Label();
            this.tag_sa = new System.Windows.Forms.Label();
            this.checkBoxHexView = new System.Windows.Forms.CheckBox();
            this.button_clearscreen = new System.Windows.Forms.Button();
            this.InputArea = new System.Windows.Forms.GroupBox();
            this.checkBox_cmdHold = new System.Windows.Forms.CheckBox();
            this.button_sendcommand = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.cmmenu_tab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Newtab = new System.Windows.Forms.ToolStripMenuItem();
            this.ModifyName = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteTab = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.historyBox = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox_script = new System.Windows.Forms.GroupBox();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_rpt = new System.Windows.Forms.TextBox();
            this.textBox_cmd = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl_display.SuspendLayout();
            this.tab_main_display.SuspendLayout();
            this.cmenu_displayfont.SuspendLayout();
            this.tabPage_remote.SuspendLayout();
            this.SerialpropertyBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.InputArea.SuspendLayout();
            this.cmmenu_tab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox_script.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem,
            this.deviceToolStripMenuItem,
            this.measureMentToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1077, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTsFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // openTsFileToolStripMenuItem
            // 
            this.openTsFileToolStripMenuItem.Name = "openTsFileToolStripMenuItem";
            this.openTsFileToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.openTsFileToolStripMenuItem.Text = "Open TsFile";
            this.openTsFileToolStripMenuItem.Click += new System.EventHandler(this.button_load_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.item_serial_device,
            this.visaDeviceToolStripMenuItem,
            this.otherDeviceToolStripMenuItem});
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.deviceToolStripMenuItem.Text = "Setup";
            // 
            // item_serial_device
            // 
            this.item_serial_device.Name = "item_serial_device";
            this.item_serial_device.Size = new System.Drawing.Size(142, 22);
            this.item_serial_device.Text = "Serial Device";
            this.item_serial_device.Click += new System.EventHandler(this.menuSetup_Click);
            // 
            // visaDeviceToolStripMenuItem
            // 
            this.visaDeviceToolStripMenuItem.Name = "visaDeviceToolStripMenuItem";
            this.visaDeviceToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.visaDeviceToolStripMenuItem.Text = "Visa Device";
            this.visaDeviceToolStripMenuItem.Click += new System.EventHandler(this.visaDeviceToolStripMenuItem_Click);
            // 
            // otherDeviceToolStripMenuItem
            // 
            this.otherDeviceToolStripMenuItem.Name = "otherDeviceToolStripMenuItem";
            this.otherDeviceToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.otherDeviceToolStripMenuItem.Text = "Other Device";
            this.otherDeviceToolStripMenuItem.Click += new System.EventHandler(this.otherDeviceToolStripMenuItem_Click);
            // 
            // measureMentToolStripMenuItem
            // 
            this.measureMentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rXEVMToolStripMenuItem,
            this.sensitivityToolStripMenuItem});
            this.measureMentToolStripMenuItem.Name = "measureMentToolStripMenuItem";
            this.measureMentToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.measureMentToolStripMenuItem.Text = "MeasureMent";
            // 
            // rXEVMToolStripMenuItem
            // 
            this.rXEVMToolStripMenuItem.Name = "rXEVMToolStripMenuItem";
            this.rXEVMToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.rXEVMToolStripMenuItem.Text = "Rx-Evm";
            this.rXEVMToolStripMenuItem.Click += new System.EventHandler(this.rXEVMToolStripMenuItem_Click);
            // 
            // sensitivityToolStripMenuItem
            // 
            this.sensitivityToolStripMenuItem.Name = "sensitivityToolStripMenuItem";
            this.sensitivityToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.sensitivityToolStripMenuItem.Text = "Sensitivity";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.Mode_Switch,
            this.upgradeToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // Mode_Switch
            // 
            this.Mode_Switch.Name = "Mode_Switch";
            this.Mode_Switch.Size = new System.Drawing.Size(145, 22);
            this.Mode_Switch.Text = "Mode_Switch";
            this.Mode_Switch.Click += new System.EventHandler(this.visaSwitchToolStripMenuItem_Click);
            // 
            // upgradeToolStripMenuItem
            // 
            this.upgradeToolStripMenuItem.Name = "upgradeToolStripMenuItem";
            this.upgradeToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.upgradeToolStripMenuItem.Text = "Upgrade";
            this.upgradeToolStripMenuItem.Click += new System.EventHandler(this.upgradeToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rruConnButton,
            this.toolStripButton2,
            this.DC5767A_ON,
            this.DC5767A_OFF,
            this.SACAPTURE,
            this.toolStripButton_capture2,
            this.Rumasterswitchbutton,
            this.Serial2_conn_button,
            this.serial2_pow_on,
            this.serial2_pow_off,
            this.toolStripButton_telnet});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1077, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // rruConnButton
            // 
            this.rruConnButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rruConnButton.Image = ((System.Drawing.Image)(resources.GetObject("rruConnButton.Image")));
            this.rruConnButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rruConnButton.Name = "rruConnButton";
            this.rruConnButton.Size = new System.Drawing.Size(23, 22);
            this.rruConnButton.Text = "Connect RRU";
            this.rruConnButton.Click += new System.EventHandler(this.Rru_con_Button_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Script Server";
            this.toolStripButton2.Click += new System.EventHandler(this.Socket_start_Button_Click);
            // 
            // DC5767A_ON
            // 
            this.DC5767A_ON.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DC5767A_ON.Image = ((System.Drawing.Image)(resources.GetObject("DC5767A_ON.Image")));
            this.DC5767A_ON.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DC5767A_ON.Name = "DC5767A_ON";
            this.DC5767A_ON.Size = new System.Drawing.Size(23, 22);
            this.DC5767A_ON.Text = "DC5767A ON";
            this.DC5767A_ON.Click += new System.EventHandler(this.DC5767A_ON_Click);
            // 
            // DC5767A_OFF
            // 
            this.DC5767A_OFF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DC5767A_OFF.Image = ((System.Drawing.Image)(resources.GetObject("DC5767A_OFF.Image")));
            this.DC5767A_OFF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DC5767A_OFF.Name = "DC5767A_OFF";
            this.DC5767A_OFF.Size = new System.Drawing.Size(23, 22);
            this.DC5767A_OFF.Text = "DC5767A OFF";
            this.DC5767A_OFF.Click += new System.EventHandler(this.DC5767A_OFF_Click);
            // 
            // SACAPTURE
            // 
            this.SACAPTURE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SACAPTURE.Image = ((System.Drawing.Image)(resources.GetObject("SACAPTURE.Image")));
            this.SACAPTURE.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SACAPTURE.Name = "SACAPTURE";
            this.SACAPTURE.Size = new System.Drawing.Size(23, 22);
            this.SACAPTURE.Text = "Capture1";
            this.SACAPTURE.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SACAPTURE_MouseDown);
            // 
            // toolStripButton_capture2
            // 
            this.toolStripButton_capture2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_capture2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_capture2.Image")));
            this.toolStripButton_capture2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_capture2.Name = "toolStripButton_capture2";
            this.toolStripButton_capture2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_capture2.Text = "Capture2";
            this.toolStripButton_capture2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStripButton_capture2_MouseDown);
            // 
            // Rumasterswitchbutton
            // 
            this.Rumasterswitchbutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Rumasterswitchbutton.Image = ((System.Drawing.Image)(resources.GetObject("Rumasterswitchbutton.Image")));
            this.Rumasterswitchbutton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Rumasterswitchbutton.Name = "Rumasterswitchbutton";
            this.Rumasterswitchbutton.Size = new System.Drawing.Size(23, 22);
            this.Rumasterswitchbutton.Text = "Rumaster(TCA)";
            this.Rumasterswitchbutton.ToolTipText = "Rumaster(TCA)";
            this.Rumasterswitchbutton.Click += new System.EventHandler(this.Rumasterswitchbutton_Click);
            // 
            // Serial2_conn_button
            // 
            this.Serial2_conn_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Serial2_conn_button.Image = ((System.Drawing.Image)(resources.GetObject("Serial2_conn_button.Image")));
            this.Serial2_conn_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Serial2_conn_button.Name = "Serial2_conn_button";
            this.Serial2_conn_button.Size = new System.Drawing.Size(23, 22);
            this.Serial2_conn_button.Text = "Serial2 Connect";
            this.Serial2_conn_button.Click += new System.EventHandler(this.Serial2_conn_button_Click);
            // 
            // serial2_pow_on
            // 
            this.serial2_pow_on.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.serial2_pow_on.Image = ((System.Drawing.Image)(resources.GetObject("serial2_pow_on.Image")));
            this.serial2_pow_on.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.serial2_pow_on.Name = "serial2_pow_on";
            this.serial2_pow_on.Size = new System.Drawing.Size(23, 22);
            this.serial2_pow_on.Text = "serial2_pow_on";
            this.serial2_pow_on.Click += new System.EventHandler(this.serial2_pow_on_Click);
            // 
            // serial2_pow_off
            // 
            this.serial2_pow_off.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.serial2_pow_off.Image = ((System.Drawing.Image)(resources.GetObject("serial2_pow_off.Image")));
            this.serial2_pow_off.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.serial2_pow_off.Name = "serial2_pow_off";
            this.serial2_pow_off.Size = new System.Drawing.Size(23, 22);
            this.serial2_pow_off.Text = "serial2_pow_off";
            this.serial2_pow_off.Click += new System.EventHandler(this.serial2_pow_off_Click);
            // 
            // toolStripButton_telnet
            // 
            this.toolStripButton_telnet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_telnet.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_telnet.Image")));
            this.toolStripButton_telnet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_telnet.Name = "toolStripButton_telnet";
            this.toolStripButton_telnet.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_telnet.Text = "telnet";
            this.toolStripButton_telnet.Click += new System.EventHandler(this.toolStripButton_telnet_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 692);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1077, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CmdProgressBar
            // 
            this.CmdProgressBar.Name = "CmdProgressBar";
            this.CmdProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // InputBox
            // 
            this.InputBox.AcceptsReturn = true;
            this.InputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputBox.Location = new System.Drawing.Point(191, 19);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(513, 20);
            this.InputBox.TabIndex = 0;
            this.InputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBox_KeyDown);
            this.InputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputBox_KeyPress);
            // 
            // button_lock
            // 
            this.button_lock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_lock.Location = new System.Drawing.Point(717, 180);
            this.button_lock.Name = "button_lock";
            this.button_lock.Size = new System.Drawing.Size(100, 25);
            this.button_lock.TabIndex = 6;
            this.button_lock.Text = "Lock";
            this.toolTip1.SetToolTip(this.button_lock, "Lock TabPages");
            this.button_lock.UseVisualStyleBackColor = true;
            this.button_lock.Click += new System.EventHandler(this.button_lock_Click);
            // 
            // button_load
            // 
            this.button_load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_load.Location = new System.Drawing.Point(717, 118);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(100, 25);
            this.button_load.TabIndex = 7;
            this.button_load.Text = "Load";
            this.toolTip1.SetToolTip(this.button_load, "Load  ts file");
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(717, 149);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(100, 25);
            this.button_save.TabIndex = 8;
            this.button_save.Text = "Save";
            this.toolTip1.SetToolTip(this.button_save, "Save TabPages to ts file");
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // comboBox_instrumentprefix
            // 
            this.comboBox_instrumentprefix.FormattingEnabled = true;
            this.comboBox_instrumentprefix.Location = new System.Drawing.Point(64, 19);
            this.comboBox_instrumentprefix.Name = "comboBox_instrumentprefix";
            this.comboBox_instrumentprefix.Size = new System.Drawing.Size(121, 21);
            this.comboBox_instrumentprefix.TabIndex = 9;
            this.comboBox_instrumentprefix.SelectedIndexChanged += new System.EventHandler(this.comboBox_instrumentprefix_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label_terminal_IP);
            this.groupBox1.Controls.Add(this.comboBox_terminal_select);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tabControl_display);
            this.groupBox1.Controls.Add(this.checkBox_secondary);
            this.groupBox1.Controls.Add(this.checkBox_Log);
            this.groupBox1.Controls.Add(this.checkBox_debug);
            this.groupBox1.Controls.Add(this.SerialpropertyBox);
            this.groupBox1.Controls.Add(this.checkBox_pause);
            this.groupBox1.Controls.Add(this.checkBox_AutoscrollDown);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button_clearscreen);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(854, 413);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Received";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label_terminal_IP
            // 
            this.label_terminal_IP.AutoSize = true;
            this.label_terminal_IP.ForeColor = System.Drawing.Color.Blue;
            this.label_terminal_IP.Location = new System.Drawing.Point(705, 213);
            this.label_terminal_IP.Name = "label_terminal_IP";
            this.label_terminal_IP.Size = new System.Drawing.Size(59, 13);
            this.label_terminal_IP.TabIndex = 20;
            this.label_terminal_IP.Text = "terminal_IP";
            // 
            // comboBox_terminal_select
            // 
            this.comboBox_terminal_select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_terminal_select.FormattingEnabled = true;
            this.comboBox_terminal_select.Location = new System.Drawing.Point(769, 185);
            this.comboBox_terminal_select.Name = "comboBox_terminal_select";
            this.comboBox_terminal_select.Size = new System.Drawing.Size(78, 21);
            this.comboBox_terminal_select.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(705, 188);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Teiminal";
            // 
            // tabControl_display
            // 
            this.tabControl_display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_display.Controls.Add(this.tab_main_display);
            this.tabControl_display.Controls.Add(this.tabPage_remote);
            this.tabControl_display.Location = new System.Drawing.Point(7, 23);
            this.tabControl_display.Name = "tabControl_display";
            this.tabControl_display.SelectedIndex = 0;
            this.tabControl_display.Size = new System.Drawing.Size(685, 377);
            this.tabControl_display.TabIndex = 17;
            // 
            // tab_main_display
            // 
            this.tab_main_display.Controls.Add(this.dataDisplayBox);
            this.tab_main_display.Location = new System.Drawing.Point(4, 22);
            this.tab_main_display.Name = "tab_main_display";
            this.tab_main_display.Padding = new System.Windows.Forms.Padding(3);
            this.tab_main_display.Size = new System.Drawing.Size(677, 351);
            this.tab_main_display.TabIndex = 1;
            this.tab_main_display.Text = "Main";
            this.tab_main_display.UseVisualStyleBackColor = true;
            // 
            // dataDisplayBox
            // 
            this.dataDisplayBox.AcceptsTab = true;
            this.dataDisplayBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataDisplayBox.BackColor = System.Drawing.SystemColors.Window;
            this.dataDisplayBox.ContextMenuStrip = this.cmenu_displayfont;
            this.dataDisplayBox.ForeColor = System.Drawing.Color.Blue;
            this.dataDisplayBox.Location = new System.Drawing.Point(6, 6);
            this.dataDisplayBox.Name = "dataDisplayBox";
            this.dataDisplayBox.ReadOnly = true;
            this.dataDisplayBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.dataDisplayBox.Size = new System.Drawing.Size(665, 339);
            this.dataDisplayBox.TabIndex = 1;
            this.dataDisplayBox.Text = "";
            this.dataDisplayBox.TextChanged += new System.EventHandler(this.dataDisplayBox_TextChanged);
            this.dataDisplayBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataDisplayBox_KeyDown);
            // 
            // cmenu_displayfont
            // 
            this.cmenu_displayfont.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backColorSettingToolStripMenuItem,
            this.foreColorSettingToolStripMenuItem,
            this.fontToolStripMenuItem});
            this.cmenu_displayfont.Name = "cmenu_displayfont";
            this.cmenu_displayfont.Size = new System.Drawing.Size(129, 70);
            // 
            // backColorSettingToolStripMenuItem
            // 
            this.backColorSettingToolStripMenuItem.Name = "backColorSettingToolStripMenuItem";
            this.backColorSettingToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.backColorSettingToolStripMenuItem.Text = "BackColor";
            this.backColorSettingToolStripMenuItem.Click += new System.EventHandler(this.backColorSettingToolStripMenuItem_Click);
            // 
            // foreColorSettingToolStripMenuItem
            // 
            this.foreColorSettingToolStripMenuItem.Name = "foreColorSettingToolStripMenuItem";
            this.foreColorSettingToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.foreColorSettingToolStripMenuItem.Text = "ForeColor";
            this.foreColorSettingToolStripMenuItem.Click += new System.EventHandler(this.foreColorSettingToolStripMenuItem_Click);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // tabPage_remote
            // 
            this.tabPage_remote.Controls.Add(this.Display_remote);
            this.tabPage_remote.Location = new System.Drawing.Point(4, 22);
            this.tabPage_remote.Name = "tabPage_remote";
            this.tabPage_remote.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_remote.Size = new System.Drawing.Size(677, 351);
            this.tabPage_remote.TabIndex = 2;
            this.tabPage_remote.Text = "Remote";
            this.tabPage_remote.UseVisualStyleBackColor = true;
            // 
            // Display_remote
            // 
            this.Display_remote.AcceptsReturn = true;
            this.Display_remote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Display_remote.BackColor = System.Drawing.Color.White;
            this.Display_remote.Location = new System.Drawing.Point(6, 6);
            this.Display_remote.Multiline = true;
            this.Display_remote.Name = "Display_remote";
            this.Display_remote.ReadOnly = true;
            this.Display_remote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Display_remote.Size = new System.Drawing.Size(665, 339);
            this.Display_remote.TabIndex = 0;
            // 
            // checkBox_secondary
            // 
            this.checkBox_secondary.AutoSize = true;
            this.checkBox_secondary.Location = new System.Drawing.Point(134, 0);
            this.checkBox_secondary.Name = "checkBox_secondary";
            this.checkBox_secondary.Size = new System.Drawing.Size(77, 17);
            this.checkBox_secondary.TabIndex = 16;
            this.checkBox_secondary.Text = "Secondary";
            this.checkBox_secondary.UseVisualStyleBackColor = true;
            this.checkBox_secondary.CheckedChanged += new System.EventHandler(this.checkBox_secondary_CheckedChanged);
            // 
            // checkBox_Log
            // 
            this.checkBox_Log.AutoSize = true;
            this.checkBox_Log.Checked = true;
            this.checkBox_Log.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Log.Location = new System.Drawing.Point(454, 0);
            this.checkBox_Log.Name = "checkBox_Log";
            this.checkBox_Log.Size = new System.Drawing.Size(108, 17);
            this.checkBox_Log.TabIndex = 15;
            this.checkBox_Log.Text = "Date_tag_in_Log";
            this.checkBox_Log.UseVisualStyleBackColor = true;
            this.checkBox_Log.CheckedChanged += new System.EventHandler(this.checkBox_Log_CheckedChanged);
            // 
            // checkBox_debug
            // 
            this.checkBox_debug.AutoSize = true;
            this.checkBox_debug.Location = new System.Drawing.Point(390, 0);
            this.checkBox_debug.Name = "checkBox_debug";
            this.checkBox_debug.Size = new System.Drawing.Size(58, 17);
            this.checkBox_debug.TabIndex = 15;
            this.checkBox_debug.Text = "Debug";
            this.checkBox_debug.UseVisualStyleBackColor = true;
            this.checkBox_debug.CheckedChanged += new System.EventHandler(this.checkBox_debug_CheckedChanged);
            // 
            // SerialpropertyBox
            // 
            this.SerialpropertyBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SerialpropertyBox.Controls.Add(this.radioC_L2);
            this.SerialpropertyBox.Controls.Add(this.radio_LF2);
            this.SerialpropertyBox.Controls.Add(this.radio_CR2);
            this.SerialpropertyBox.Controls.Add(this.label_serial2baud);
            this.SerialpropertyBox.Controls.Add(this.label_serial2port);
            this.SerialpropertyBox.Controls.Add(this.label_rrubaud);
            this.SerialpropertyBox.Controls.Add(this.label_rruport);
            this.SerialpropertyBox.Controls.Add(this.label6);
            this.SerialpropertyBox.Controls.Add(this.label5);
            this.SerialpropertyBox.Controls.Add(this.label4);
            this.SerialpropertyBox.Controls.Add(this.label3);
            this.SerialpropertyBox.Controls.Add(this.label2);
            this.SerialpropertyBox.Controls.Add(this.label1);
            this.SerialpropertyBox.Location = new System.Drawing.Point(698, 246);
            this.SerialpropertyBox.Name = "SerialpropertyBox";
            this.SerialpropertyBox.Size = new System.Drawing.Size(150, 154);
            this.SerialpropertyBox.TabIndex = 14;
            this.SerialpropertyBox.TabStop = false;
            this.SerialpropertyBox.Text = "Serial Ports";
            this.SerialpropertyBox.Enter += new System.EventHandler(this.SerialpropertyBox_Enter);
            // 
            // radioC_L2
            // 
            this.radioC_L2.AutoSize = true;
            this.radioC_L2.Location = new System.Drawing.Point(92, 124);
            this.radioC_L2.Name = "radioC_L2";
            this.radioC_L2.Size = new System.Drawing.Size(58, 17);
            this.radioC_L2.TabIndex = 16;
            this.radioC_L2.Text = "CR+LF";
            this.radioC_L2.UseVisualStyleBackColor = true;
            this.radioC_L2.CheckedChanged += new System.EventHandler(this.radio_CR2_CheckedChanged);
            // 
            // radio_LF2
            // 
            this.radio_LF2.AutoSize = true;
            this.radio_LF2.Checked = true;
            this.radio_LF2.Location = new System.Drawing.Point(51, 124);
            this.radio_LF2.Name = "radio_LF2";
            this.radio_LF2.Size = new System.Drawing.Size(37, 17);
            this.radio_LF2.TabIndex = 15;
            this.radio_LF2.TabStop = true;
            this.radio_LF2.Text = "LF";
            this.radio_LF2.UseVisualStyleBackColor = true;
            this.radio_LF2.CheckedChanged += new System.EventHandler(this.radio_CR2_CheckedChanged);
            // 
            // radio_CR2
            // 
            this.radio_CR2.AutoSize = true;
            this.radio_CR2.Location = new System.Drawing.Point(5, 124);
            this.radio_CR2.Name = "radio_CR2";
            this.radio_CR2.Size = new System.Drawing.Size(40, 17);
            this.radio_CR2.TabIndex = 14;
            this.radio_CR2.Text = "CR";
            this.radio_CR2.UseVisualStyleBackColor = true;
            this.radio_CR2.CheckedChanged += new System.EventHandler(this.radio_CR2_CheckedChanged);
            // 
            // label_serial2baud
            // 
            this.label_serial2baud.AutoSize = true;
            this.label_serial2baud.BackColor = System.Drawing.SystemColors.Menu;
            this.label_serial2baud.ForeColor = System.Drawing.Color.Blue;
            this.label_serial2baud.Location = new System.Drawing.Point(57, 108);
            this.label_serial2baud.Name = "label_serial2baud";
            this.label_serial2baud.Size = new System.Drawing.Size(0, 13);
            this.label_serial2baud.TabIndex = 10;
            // 
            // label_serial2port
            // 
            this.label_serial2port.AutoSize = true;
            this.label_serial2port.BackColor = System.Drawing.SystemColors.Menu;
            this.label_serial2port.ForeColor = System.Drawing.Color.Blue;
            this.label_serial2port.Location = new System.Drawing.Point(57, 95);
            this.label_serial2port.Name = "label_serial2port";
            this.label_serial2port.Size = new System.Drawing.Size(0, 13);
            this.label_serial2port.TabIndex = 9;
            // 
            // label_rrubaud
            // 
            this.label_rrubaud.AutoSize = true;
            this.label_rrubaud.BackColor = System.Drawing.SystemColors.Menu;
            this.label_rrubaud.ForeColor = System.Drawing.Color.Blue;
            this.label_rrubaud.Location = new System.Drawing.Point(57, 46);
            this.label_rrubaud.Name = "label_rrubaud";
            this.label_rrubaud.Size = new System.Drawing.Size(0, 13);
            this.label_rrubaud.TabIndex = 8;
            // 
            // label_rruport
            // 
            this.label_rruport.AutoSize = true;
            this.label_rruport.BackColor = System.Drawing.SystemColors.Menu;
            this.label_rruport.ForeColor = System.Drawing.Color.Blue;
            this.label_rruport.Location = new System.Drawing.Point(57, 31);
            this.label_rruport.Name = "label_rruport";
            this.label_rruport.Size = new System.Drawing.Size(0, 13);
            this.label_rruport.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Menu;
            this.label6.Location = new System.Drawing.Point(6, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Baudrate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Menu;
            this.label5.Location = new System.Drawing.Point(6, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Menu;
            this.label4.Location = new System.Drawing.Point(6, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Serial2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Menu;
            this.label3.Location = new System.Drawing.Point(6, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Baudrate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Menu;
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Menu;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "RRU";
            // 
            // checkBox_pause
            // 
            this.checkBox_pause.AutoSize = true;
            this.checkBox_pause.Location = new System.Drawing.Point(328, 0);
            this.checkBox_pause.Name = "checkBox_pause";
            this.checkBox_pause.Size = new System.Drawing.Size(60, 17);
            this.checkBox_pause.TabIndex = 13;
            this.checkBox_pause.Text = "Protect";
            this.checkBox_pause.UseVisualStyleBackColor = true;
            this.checkBox_pause.CheckedChanged += new System.EventHandler(this.checkBox_pause_CheckedChanged);
            // 
            // checkBox_AutoscrollDown
            // 
            this.checkBox_AutoscrollDown.AutoSize = true;
            this.checkBox_AutoscrollDown.Checked = true;
            this.checkBox_AutoscrollDown.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AutoscrollDown.Location = new System.Drawing.Point(217, 0);
            this.checkBox_AutoscrollDown.Name = "checkBox_AutoscrollDown";
            this.checkBox_AutoscrollDown.Size = new System.Drawing.Size(105, 17);
            this.checkBox_AutoscrollDown.TabIndex = 12;
            this.checkBox_AutoscrollDown.Text = "Auto ScrollDown";
            this.checkBox_AutoscrollDown.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tag_DC5767A);
            this.groupBox2.Controls.Add(this.tag_rumaster);
            this.groupBox2.Controls.Add(this.tag_is2);
            this.groupBox2.Controls.Add(this.tag_is1);
            this.groupBox2.Controls.Add(this.tag_rfbox2);
            this.groupBox2.Controls.Add(this.tag_rfbox1);
            this.groupBox2.Controls.Add(this.tag_sg2);
            this.groupBox2.Controls.Add(this.checkBoxHexSend);
            this.groupBox2.Controls.Add(this.tag_sg1);
            this.groupBox2.Controls.Add(this.tag_sa);
            this.groupBox2.Controls.Add(this.checkBoxHexView);
            this.groupBox2.Location = new System.Drawing.Point(698, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 162);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Instrument Status";
            // 
            // tag_DC5767A
            // 
            this.tag_DC5767A.AutoSize = true;
            this.tag_DC5767A.BackColor = System.Drawing.Color.Pink;
            this.tag_DC5767A.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_DC5767A.Location = new System.Drawing.Point(9, 122);
            this.tag_DC5767A.Name = "tag_DC5767A";
            this.tag_DC5767A.Size = new System.Drawing.Size(55, 15);
            this.tag_DC5767A.TabIndex = 8;
            this.tag_DC5767A.Text = "DC5767A";
            this.tag_DC5767A.Click += new System.EventHandler(this.tag_DC5767A_Click);
            // 
            // tag_rumaster
            // 
            this.tag_rumaster.AutoSize = true;
            this.tag_rumaster.BackColor = System.Drawing.Color.Pink;
            this.tag_rumaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_rumaster.Location = new System.Drawing.Point(9, 137);
            this.tag_rumaster.Name = "tag_rumaster";
            this.tag_rumaster.Size = new System.Drawing.Size(30, 15);
            this.tag_rumaster.TabIndex = 7;
            this.tag_rumaster.Text = "TCA";
            // 
            // tag_is2
            // 
            this.tag_is2.AutoSize = true;
            this.tag_is2.BackColor = System.Drawing.Color.Pink;
            this.tag_is2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_is2.Location = new System.Drawing.Point(9, 107);
            this.tag_is2.Name = "tag_is2";
            this.tag_is2.Size = new System.Drawing.Size(33, 15);
            this.tag_is2.TabIndex = 6;
            this.tag_is2.Text = "ISG2";
            this.tag_is2.Click += new System.EventHandler(this.tag_is2_Click);
            // 
            // tag_is1
            // 
            this.tag_is1.AutoSize = true;
            this.tag_is1.BackColor = System.Drawing.Color.Pink;
            this.tag_is1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_is1.Location = new System.Drawing.Point(9, 92);
            this.tag_is1.Name = "tag_is1";
            this.tag_is1.Size = new System.Drawing.Size(33, 15);
            this.tag_is1.TabIndex = 5;
            this.tag_is1.Text = "ISG1";
            this.tag_is1.Click += new System.EventHandler(this.tag_is1_Click);
            // 
            // tag_rfbox2
            // 
            this.tag_rfbox2.AutoSize = true;
            this.tag_rfbox2.BackColor = System.Drawing.Color.Pink;
            this.tag_rfbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_rfbox2.Location = new System.Drawing.Point(9, 77);
            this.tag_rfbox2.Name = "tag_rfbox2";
            this.tag_rfbox2.Size = new System.Drawing.Size(44, 15);
            this.tag_rfbox2.TabIndex = 4;
            this.tag_rfbox2.Text = "RfBox2";
            this.tag_rfbox2.Click += new System.EventHandler(this.tag_rfbox2_Click);
            // 
            // tag_rfbox1
            // 
            this.tag_rfbox1.AutoSize = true;
            this.tag_rfbox1.BackColor = System.Drawing.Color.Pink;
            this.tag_rfbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_rfbox1.Location = new System.Drawing.Point(9, 62);
            this.tag_rfbox1.Name = "tag_rfbox1";
            this.tag_rfbox1.Size = new System.Drawing.Size(44, 15);
            this.tag_rfbox1.TabIndex = 3;
            this.tag_rfbox1.Text = "RfBox1";
            this.tag_rfbox1.Click += new System.EventHandler(this.tag_rfbox1_Click);
            // 
            // tag_sg2
            // 
            this.tag_sg2.AutoSize = true;
            this.tag_sg2.BackColor = System.Drawing.Color.Pink;
            this.tag_sg2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_sg2.Location = new System.Drawing.Point(9, 47);
            this.tag_sg2.Name = "tag_sg2";
            this.tag_sg2.Size = new System.Drawing.Size(30, 15);
            this.tag_sg2.TabIndex = 2;
            this.tag_sg2.Text = "SG2";
            this.tag_sg2.Click += new System.EventHandler(this.tag_sg2_Click);
            // 
            // checkBoxHexSend
            // 
            this.checkBoxHexSend.AutoSize = true;
            this.checkBoxHexSend.Location = new System.Drawing.Point(79, 42);
            this.checkBoxHexSend.Name = "checkBoxHexSend";
            this.checkBoxHexSend.Size = new System.Drawing.Size(73, 17);
            this.checkBoxHexSend.TabIndex = 11;
            this.checkBoxHexSend.Text = "Hex Send";
            this.checkBoxHexSend.UseVisualStyleBackColor = true;
            this.checkBoxHexSend.Visible = false;
            // 
            // tag_sg1
            // 
            this.tag_sg1.AutoSize = true;
            this.tag_sg1.BackColor = System.Drawing.Color.Pink;
            this.tag_sg1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_sg1.Location = new System.Drawing.Point(9, 32);
            this.tag_sg1.Name = "tag_sg1";
            this.tag_sg1.Size = new System.Drawing.Size(30, 15);
            this.tag_sg1.TabIndex = 1;
            this.tag_sg1.Text = "SG1";
            this.tag_sg1.Click += new System.EventHandler(this.tag_sg1_Click);
            // 
            // tag_sa
            // 
            this.tag_sa.AutoSize = true;
            this.tag_sa.BackColor = System.Drawing.Color.Pink;
            this.tag_sa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tag_sa.Location = new System.Drawing.Point(9, 16);
            this.tag_sa.Name = "tag_sa";
            this.tag_sa.Size = new System.Drawing.Size(23, 15);
            this.tag_sa.TabIndex = 0;
            this.tag_sa.Text = "SA";
            this.tag_sa.Click += new System.EventHandler(this.tag_sa_Click);
            // 
            // checkBoxHexView
            // 
            this.checkBoxHexView.AutoSize = true;
            this.checkBoxHexView.Location = new System.Drawing.Point(79, 19);
            this.checkBoxHexView.Name = "checkBoxHexView";
            this.checkBoxHexView.Size = new System.Drawing.Size(70, 17);
            this.checkBoxHexView.TabIndex = 3;
            this.checkBoxHexView.Text = "Hex view";
            this.checkBoxHexView.UseVisualStyleBackColor = true;
            this.checkBoxHexView.Visible = false;
            // 
            // button_clearscreen
            // 
            this.button_clearscreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearscreen.Location = new System.Drawing.Point(637, 0);
            this.button_clearscreen.Name = "button_clearscreen";
            this.button_clearscreen.Size = new System.Drawing.Size(55, 20);
            this.button_clearscreen.TabIndex = 1;
            this.button_clearscreen.Text = "Clear";
            this.toolTip1.SetToolTip(this.button_clearscreen, "Clear Screen");
            this.button_clearscreen.UseVisualStyleBackColor = true;
            this.button_clearscreen.Click += new System.EventHandler(this.button_clearscreen_Click);
            // 
            // InputArea
            // 
            this.InputArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputArea.Controls.Add(this.checkBox_cmdHold);
            this.InputArea.Controls.Add(this.button_sendcommand);
            this.InputArea.Controls.Add(this.comboBox_instrumentprefix);
            this.InputArea.Controls.Add(this.InputBox);
            this.InputArea.Controls.Add(this.button_save);
            this.InputArea.Controls.Add(this.tabControl1);
            this.InputArea.Controls.Add(this.button_load);
            this.InputArea.Controls.Add(this.button_lock);
            this.InputArea.Location = new System.Drawing.Point(13, 478);
            this.InputArea.Name = "InputArea";
            this.InputArea.Size = new System.Drawing.Size(853, 211);
            this.InputArea.TabIndex = 12;
            this.InputArea.TabStop = false;
            this.InputArea.Text = "InputArea";
            // 
            // checkBox_cmdHold
            // 
            this.checkBox_cmdHold.AutoSize = true;
            this.checkBox_cmdHold.Checked = true;
            this.checkBox_cmdHold.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_cmdHold.Location = new System.Drawing.Point(6, 23);
            this.checkBox_cmdHold.Name = "checkBox_cmdHold";
            this.checkBox_cmdHold.Size = new System.Drawing.Size(48, 17);
            this.checkBox_cmdHold.TabIndex = 16;
            this.checkBox_cmdHold.Text = "Hold";
            this.checkBox_cmdHold.UseVisualStyleBackColor = true;
            this.checkBox_cmdHold.CheckedChanged += new System.EventHandler(this.checkBox_cmdHold_CheckedChanged);
            // 
            // button_sendcommand
            // 
            this.button_sendcommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_sendcommand.Location = new System.Drawing.Point(717, 17);
            this.button_sendcommand.Name = "button_sendcommand";
            this.button_sendcommand.Size = new System.Drawing.Size(100, 25);
            this.button_sendcommand.TabIndex = 10;
            this.button_sendcommand.Text = "Send";
            this.toolTip1.SetToolTip(this.button_sendcommand, "Send command to instrument");
            this.button_sendcommand.UseVisualStyleBackColor = true;
            this.button_sendcommand.Click += new System.EventHandler(this.button_sendcommand_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.ContextMenuStrip = this.cmmenu_tab;
            this.tabControl1.Location = new System.Drawing.Point(6, 49);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(698, 156);
            this.tabControl1.TabIndex = 3;
            // 
            // cmmenu_tab
            // 
            this.cmmenu_tab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Newtab,
            this.ModifyName,
            this.DeleteTab});
            this.cmmenu_tab.Name = "cmmenu_tab";
            this.cmmenu_tab.Size = new System.Drawing.Size(145, 70);
            this.cmmenu_tab.Opening += new System.ComponentModel.CancelEventHandler(this.cmmenu_tab_Opening);
            // 
            // Newtab
            // 
            this.Newtab.Name = "Newtab";
            this.Newtab.Size = new System.Drawing.Size(144, 22);
            this.Newtab.Text = "New";
            this.Newtab.Click += new System.EventHandler(this.Newtab_Click);
            // 
            // ModifyName
            // 
            this.ModifyName.Name = "ModifyName";
            this.ModifyName.Size = new System.Drawing.Size(144, 22);
            this.ModifyName.Text = "ModifyName";
            this.ModifyName.Click += new System.EventHandler(this.ModifyName_Click);
            // 
            // DeleteTab
            // 
            this.DeleteTab.Name = "DeleteTab";
            this.DeleteTab.Size = new System.Drawing.Size(144, 22);
            this.DeleteTab.Text = "DeleteTab";
            this.DeleteTab.Click += new System.EventHandler(this.DeleteTab_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.historyBox);
            this.groupBox4.Location = new System.Drawing.Point(872, 59);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(193, 400);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Command History";
            // 
            // historyBox
            // 
            this.historyBox.FormattingEnabled = true;
            this.historyBox.Location = new System.Drawing.Point(6, 19);
            this.historyBox.Name = "historyBox";
            this.historyBox.Size = new System.Drawing.Size(181, 368);
            this.historyBox.TabIndex = 14;
            this.historyBox.SelectedIndexChanged += new System.EventHandler(this.historyBox_SelectedIndexChanged);
            this.historyBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.historyBox_MouseDoubleClick);
            // 
            // groupBox_script
            // 
            this.groupBox_script.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_script.Controls.Add(this.textBox_interval);
            this.groupBox_script.Controls.Add(this.label9);
            this.groupBox_script.Controls.Add(this.button_stop);
            this.groupBox_script.Controls.Add(this.button_start);
            this.groupBox_script.Controls.Add(this.label8);
            this.groupBox_script.Controls.Add(this.label7);
            this.groupBox_script.Controls.Add(this.textBox_rpt);
            this.groupBox_script.Controls.Add(this.textBox_cmd);
            this.groupBox_script.Location = new System.Drawing.Point(872, 465);
            this.groupBox_script.Name = "groupBox_script";
            this.groupBox_script.Size = new System.Drawing.Size(193, 224);
            this.groupBox_script.TabIndex = 14;
            this.groupBox_script.TabStop = false;
            this.groupBox_script.Text = "Script";
            // 
            // textBox_interval
            // 
            this.textBox_interval.Location = new System.Drawing.Point(81, 71);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(100, 20);
            this.textBox_interval.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Interval(ms)";
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(106, 119);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 23);
            this.button_stop.TabIndex = 5;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(6, 119);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 4;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Repeat";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Command";
            // 
            // textBox_rpt
            // 
            this.textBox_rpt.Location = new System.Drawing.Point(81, 45);
            this.textBox_rpt.Name = "textBox_rpt";
            this.textBox_rpt.Size = new System.Drawing.Size(100, 20);
            this.textBox_rpt.TabIndex = 1;
            // 
            // textBox_cmd
            // 
            this.textBox_cmd.Location = new System.Drawing.Point(81, 19);
            this.textBox_cmd.Multiline = true;
            this.textBox_cmd.Name = "textBox_cmd";
            this.textBox_cmd.Size = new System.Drawing.Size(100, 20);
            this.textBox_cmd.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AcceptButton = this.button_sendcommand;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 714);
            this.Controls.Add(this.groupBox_script);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.InputArea);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "RTT";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl_display.ResumeLayout(false);
            this.tab_main_display.ResumeLayout(false);
            this.cmenu_displayfont.ResumeLayout(false);
            this.tabPage_remote.ResumeLayout(false);
            this.tabPage_remote.PerformLayout();
            this.SerialpropertyBox.ResumeLayout(false);
            this.SerialpropertyBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.InputArea.ResumeLayout(false);
            this.InputArea.PerformLayout();
            this.cmmenu_tab.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox_script.ResumeLayout(false);
            this.groupBox_script.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton rruConnButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton DC5767A_ON;
        private System.Windows.Forms.ToolStripButton DC5767A_OFF;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Button button_lock;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.ComboBox comboBox_instrumentprefix;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_clearscreen;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox InputArea;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxHexSend;
        private System.Windows.Forms.Button button_sendcommand;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripButton SACAPTURE;
        private System.Windows.Forms.CheckBox checkBoxHexView;
        private System.Windows.Forms.CheckBox checkBox_AutoscrollDown;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem item_serial_device;
        private System.Windows.Forms.Label tag_rfbox2;
        private System.Windows.Forms.Label tag_rfbox1;
        private System.Windows.Forms.Label tag_sg2;
        private System.Windows.Forms.Label tag_sg1;
        private System.Windows.Forms.Label tag_sa;
        private System.Windows.Forms.Label tag_DC5767A;
        private System.Windows.Forms.Label tag_rumaster;
        private System.Windows.Forms.Label tag_is2;
        private System.Windows.Forms.Label tag_is1;
        private System.Windows.Forms.ListBox historyBox;
        internal System.Windows.Forms.RichTextBox dataDisplayBox;
        private System.Windows.Forms.CheckBox checkBox_pause;
        private System.Windows.Forms.ContextMenuStrip cmmenu_tab;
        private System.Windows.Forms.ToolStripMenuItem Newtab;
        private System.Windows.Forms.ToolStripMenuItem ModifyName;
        private System.Windows.Forms.ToolStripMenuItem DeleteTab;
        private System.Windows.Forms.ContextMenuStrip cmenu_displayfont;
        private System.Windows.Forms.ToolStripMenuItem backColorSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem foreColorSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton Rumasterswitchbutton;
        private System.Windows.Forms.GroupBox SerialpropertyBox;
        private System.Windows.Forms.Label label_serial2baud;
        private System.Windows.Forms.Label label_serial2port;
        private System.Windows.Forms.Label label_rrubaud;
        private System.Windows.Forms.Label label_rruport;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton Serial2_conn_button;
        private System.Windows.Forms.ToolStripProgressBar CmdProgressBar;
        private System.Windows.Forms.ToolStripMenuItem Mode_Switch;
        private System.Windows.Forms.ToolStripMenuItem measureMentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rXEVMToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox_script;
        private System.Windows.Forms.RadioButton radioC_L2;
        private System.Windows.Forms.RadioButton radio_LF2;
        private System.Windows.Forms.RadioButton radio_CR2;
        private System.Windows.Forms.CheckBox checkBox_Log;
        private System.Windows.Forms.CheckBox checkBox_debug;
        private System.Windows.Forms.ToolStripMenuItem upgradeToolStripMenuItem;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_rpt;
        private System.Windows.Forms.TextBox textBox_cmd;
        private System.Windows.Forms.TextBox textBox_interval;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBox_secondary;
        private System.Windows.Forms.ToolStripButton serial2_pow_on;
        private System.Windows.Forms.ToolStripButton serial2_pow_off;
        private System.Windows.Forms.ToolStripMenuItem sensitivityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visaDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherDeviceToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl_display;
        private System.Windows.Forms.TabPage tab_main_display;
        private System.Windows.Forms.ComboBox comboBox_terminal_select;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage_remote;
        private System.Windows.Forms.TextBox Display_remote;
        private System.Windows.Forms.Label label_terminal_IP;
        private System.Windows.Forms.ToolStripButton toolStripButton_telnet;
        private System.Windows.Forms.ToolStripButton toolStripButton_capture2;
        private System.Windows.Forms.CheckBox checkBox_cmdHold;
    }
}


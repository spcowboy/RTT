using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
//using IronPython;
using Ivi.Visa.Interop;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using TigerApplicationServiceClient;
using TSLControlClient.TslControl;
using Tiger.Ruma.WcfInterface;
using Tiger.Ruma;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using MSCommLib;

namespace RTT
{
    public partial class MainForm : Form
    {
        //数据保护模式标记
        bool dataProtect = false;

        //mscomm串口
        MSCommLib.MSCommClass Com_rru = new MSCommClass();
        bool Serial_status = true; //serial = mscomm

        //命令暂存
        bool cmdHold = true;

        //telnet du
        Tcpclient du = null;
        Telnet t = null;

        //path
        string localpath = Application.StartupPath;
        const string mainpath = @"c:\RTT\";
        string logPath = mainpath + @"log\";
        
        string _snapPath = mainpath + @"snapshot\";
        string _rxevmPath = mainpath + @"rxevm\";
        string backuppath = @".\backup\";
        string updatePath = @".\update\";

        //cmd process queue
        bool cmdProcessThread_run = false;
        Queue cmdQueue = new Queue();
        Thread _cmdProcessThread;
        object cmdQueuelock = new object();
        EventWaitHandle _waitcmdQueueEventHandle = new AutoResetEvent(false);
        cmdQueueEventSender cqEventSender = new cmdQueueEventSender();

        //script
        Thread _scriptthread;
        bool script_run = false;
        int script_repeat_times = -1;
        string scrip_cmd = "null";
        int script_interval = 0;

        //sa capture
        Thread _saCapturethread;
        string captureName;
        int captureDelay;

        //
        char SPACER_FLAG_FIR = '#'; //字符串分割符，用于socket字符串传输第一层分割
        char SPACER_FLAG_SEC = '|'; //字符串分割符，用于socket字符串传输第二层分割

        //debug log
        private bool _debug = false;
        private bool _log = false;

        //serialport
        SerialPort _COM_RRU = new SerialPort();
        SerialPort _COM2 = new SerialPort();
        string _com2trans = "\n";
        //bool waitingForreceive = false;

        //ts cmd execute flag
        int _rrusend = 0;
        int _rruresp = 0;



        //lock
        private Object _lock_RRUCOM = new Object();
        private Object _lock_COM2 = new Object();
        private Object _lock_Instrument = new Object();
        

        //socket
        IPEndPoint ipep;//定义一网络端点
        Socket newsock;//定义一个Socket
        const string socketNoresult = "RTT-ACK";
        const string socketNorru = "RRU is not connected.";
        const string socketNoserial2 = "Serial2 is not connected.";

        //rumaster
        bool _TCA_ON = false;
        ApplicationControl tas;
        TslControlClient tsl;
        IRumaControlClient rumaClient;
        Tiger.Ruma.IRumaCpriDataFlow icdf;
        Tiger.Ruma.IRumaCarrierConfig rCarrierConfig;
        Tiger.Ruma.IRumaCpriConfig rCpriConfig;
        Tiger.Ruma.IRumaServerBase rServerBase;
        Tiger.Ruma.IRumaTriggerConfig rTriggerConfig;
        Tiger.Ruma.IRULoader rIRULoader;
        string _cpriport;
        string[] selectedCpriPorts = new string[] { "1A", "1B" };

        int[] rxPortBuffer = new int[] { 256, 256 };

        int[] txIqBandWidth = new int[] { 64, 64 };

        int[] rxIqBandWidth = new int[] { 64, 64 };

        

        //Rumaster Dictionary
        private static readonly Dictionary<string, CpriFlowDirection> flowDirectionDic = new Dictionary<string, CpriFlowDirection>
        {
            {"TX", CpriFlowDirection.TX},
            {"RX", CpriFlowDirection.RX}
        };
        private static readonly Dictionary<string, SampleFrequency> freqDic = new Dictionary<string, SampleFrequency>
        {
            {"Frequency_30_72", SampleFrequency.Frequency_30_72},
            {"Frequency_15_36", SampleFrequency.Frequency_15_36},
            {"Frequency_23_04", SampleFrequency.Frequency_23_04},
            {"Frequency_19_20", SampleFrequency.Frequency_19_20},
            {"Frequency_7_68", SampleFrequency.Frequency_7_68},
            {"Frequency_3_84", SampleFrequency.Frequency_3_84},
            {"Frequency_1_92", SampleFrequency.Frequency_1_92},
            {"Frequency_0_96", SampleFrequency.Frequency_0_96}
        };

        private static readonly Dictionary<string, Technology> technologyDic = new Dictionary<string, Technology>
        {
            {"LTE", Technology.LTE},
            {"GSM", Technology.GSM},
            {"CDMA", Technology.CDMA},
            {"WCDMA_5_BIT", Technology.WCDMA_5_BIT},
            {"WCDMA_7_BIT", Technology.WCDMA_7_BIT}
        };
        private static readonly Dictionary<string, bool> boolDic = new Dictionary<string, bool>
        {
            {"true", true},
            {"false", false}
        };
        private static readonly Dictionary<string, SyncMode> syncModeDic = new Dictionary<string, SyncMode>
        {
            {"FSINFO", SyncMode.FSINFO},
            {"CUSTOM", SyncMode.CUSTOM},
            {"RX_TIMING", SyncMode.RX_TIMING}            
        };
        private static readonly Dictionary<string, LineRate> lineRateDic = new Dictionary<string, LineRate>
        {
            {"LR1_2", LineRate.LR1_2},
            {"LR2_5", LineRate.LR2_5},
            {"LR4_9", LineRate.LR4_9},
            {"LR9_8", LineRate.LR9_8}
        };
        private static readonly Dictionary<string, CpriVersion> cpriVerDic = new Dictionary<string, CpriVersion>
        {
            {"VERSION_1", CpriVersion.VERSION_1},
            {"VERSION_2", CpriVersion.VERSION_2}            
        };

        private static readonly Dictionary<string, LTU> ltuDic = new Dictionary<string, LTU>
        {
            {"INT_REF", LTU.INT_REF},
            {"EXT_REF", LTU.EXT_REF},
            {"APP_REF1", LTU.APP_REF1},
            {"APP_REF2", LTU.APP_REF2},
            {"APP_REF3", LTU.APP_REF3},
            {"APP_REF4", LTU.APP_REF4},
            {"APP_REF5", LTU.APP_REF5},
            {"APP_REF6", LTU.APP_REF6}
        };
        private static readonly Dictionary<string, FlowDataType> flowDataTypeDic = new Dictionary<string, FlowDataType>
        {
            {"NONE", FlowDataType.NONE},
            {"IQ", FlowDataType.IQ},
            {"AXC", FlowDataType.AXC},
            {"ECP", FlowDataType.ECP},
            {"AXC_ECP", FlowDataType.AXC_ECP},
            {"IQ_AXC_ECP", FlowDataType.IQ_AXC_ECP}           
        };
        private static readonly Dictionary<string, FlowStartCondition> flowStartConditionDic = new Dictionary<string, FlowStartCondition>
        {
            {"NONE", FlowStartCondition.NONE},
            {"TRIG_IN", FlowStartCondition.TRIG_IN},
            {"CPRI_TIME", FlowStartCondition.CPRI_TIME},
            {"FIRST_NON_IDLE", FlowStartCondition.FIRST_NON_IDLE},
            {"FSINFO", FlowStartCondition.FSINFO},
            {"FRAME_PRE_START", FlowStartCondition.FRAME_PRE_START},
            {"RADIO_FRAME", FlowStartCondition.RADIO_FRAME}
        };
        private static readonly Dictionary<string, FlowStopCondition> flowStopConditionDic = new Dictionary<string, FlowStopCondition>
        {
            {"NEVER", FlowStopCondition.NEVER},
            {"FLOW_STOP_COND_CPRI_TIME", FlowStopCondition.FLOW_STOP_COND_CPRI_TIME},
            {"FLOW_STOP_COND_TRIG_IN", FlowStopCondition.FLOW_STOP_COND_TRIG_IN},
            {"CPRI_TIME_LENGTH", FlowStopCondition.CPRI_TIME_LENGTH}
        };

        private static readonly Dictionary<string, FlowDataMode> flowDataModeDic = new Dictionary<string, FlowDataMode>
        {
            {"Carrier", FlowDataMode.Carrier},
            {"RAW", FlowDataMode.RAW}
        };
        private static readonly Dictionary<string, ClockInstanceName> clockInstanceNameDic = new Dictionary<string, ClockInstanceName>
        {
            {"CLK_122_0", ClockInstanceName.CLK_122_0},
            {"CLK_122_180", ClockInstanceName.CLK_122_180}
        };
        private static readonly Dictionary<string, CpriTrigSource> cpriTrigSourceDic = new Dictionary<string, CpriTrigSource>
        {
            {"CPC", CpriTrigSource.CPC},
            {"CTT", CpriTrigSource.CTT},
            {"DYNAMIC_GAIN", CpriTrigSource.DYNAMIC_GAIN},
            {"FSINFO_CHANGED", CpriTrigSource.FSINFO_CHANGED},
            {"RXK285", CpriTrigSource.RXK285},
            {"TXK285", CpriTrigSource.TXK285}
        };
        private static readonly Dictionary<string, TriggerStaticOutputLevel> triggerStaticOutputLevelDic = new Dictionary<string, TriggerStaticOutputLevel>
        {
            {"high", TriggerStaticOutputLevel.high},
            {"low", TriggerStaticOutputLevel.low}
        };

        
        //visa
        bool VisaSwitch = true;//true ==visa32
        
        int sesnSA = -1, sesnSG = -1, sesnSG2 = -1, sesnRFBOX = -1, sesnRFBOX2 = -1, sesnIS = -1, sesnIS2 = -1, sesnDC5767A = -1;
        int viSA, viSG, viSG2, viRFBOX, viRFBOX2, viIS, viIS2, viDC5767A, viCapture1, viCapture2;

        //resourcemanager
        
        private Ivi.Visa.Interop.ResourceManager sa_rm;
        private Ivi.Visa.Interop.ResourceManager sg_rm;
        private Ivi.Visa.Interop.ResourceManager sg2_rm;
        private Ivi.Visa.Interop.ResourceManager rfbox_rm;
        private Ivi.Visa.Interop.ResourceManager rfbox2_rm;
        private Ivi.Visa.Interop.ResourceManager is_rm;
        private Ivi.Visa.Interop.ResourceManager is2_rm;
        private Ivi.Visa.Interop.ResourceManager dc5767a_rm;

        //io
        private FormattedIO488 sa_io;
        private FormattedIO488 sg_io;
        private FormattedIO488 sg2_io;
        private FormattedIO488 rfbox_io;
        private FormattedIO488 rfbox2_io;
        private FormattedIO488 is_io;
        private FormattedIO488 is2_io;
        private FormattedIO488 dc5767a_io;

        //session
        private Ivi.Visa.Interop.IMessage sa_sesn;
        private Ivi.Visa.Interop.IMessage sg_sesn;
        private Ivi.Visa.Interop.IMessage sg2_sesn;
        private Ivi.Visa.Interop.IMessage rfbox_sesn;
        private Ivi.Visa.Interop.IMessage rfbox2_sesn;
        private Ivi.Visa.Interop.IMessage is_sesn;
        private Ivi.Visa.Interop.IMessage is2_sesn;
        private Ivi.Visa.Interop.IMessage dc5767a_sesn;
        private Ivi.Visa.Interop.IMessage sesnCapture1;
        private Ivi.Visa.Interop.IMessage sesnCapture2;


        //backcolor
        private string backcolor = "";
        //forecolor
        private string forecolor = "";
        //font
        private string fontstyle = "";

        //TAB INDEX
        private int _tabindex = 1;
        //file name
        private string fName;

        
        
        public Address addr = new Address();
        //private long send_count;
        //private long receive_count;
        private StringBuilder builder = new StringBuilder(4096);
        private StringBuilder printbuilder = new StringBuilder(4096);
        private StringBuilder receivebuilder = new StringBuilder();
        private List<byte> buffer = new List<byte>(4096);
        private Links link;
        private bool Listening = false;//是否没有执行完invoke相关操作  
        private bool Listening_com2 = false;//是否没有执行完invoke相关操作 
        private static bool Closing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke 
        private bool Closing_com2 = false;
        private byte[] binary_data_1 = new byte[9];//AA 44 05 01 02 03 04 05 EA
        
        //socket thread
        private static byte[] result = new byte[1024];
       // private static int myProt = Constant.HOSTPORT;   //端口  
        //static Socket serverSocket;
        private static bool _shouldStop=false;
        Thread _createServer;
        System.Net.Sockets.TcpListener listener;


        //System.Windows.Forms.Timer
        //System.Windows.Forms.Timer socketprocesstimer = new System.Windows.Forms.Timer();
        System.Timers.Timer socketprocesstimer = new System.Timers.Timer();
        System.Timers.Timer _evmtimer = new System.Timers.Timer();
        //System.Windows.Forms.Timer _evmtimer = new System.Windows.Forms.Timer();

        //tag of put to socket result buffer
        private bool socketTag = false;

        //socket result
        private StringBuilder socketbuilder = new StringBuilder();
        delegate void SetTextCallBack(string text);
        delegate void AddhistoryCallBack(string text);

        

        //use in button execute
        static List<string> _buttoncmd = new List<string>();
        Thread _buttoncmdthread;

        //use in ts execute
        static List<string> _tscmd = new List<string>();
        Thread _tsthread;

        

        //textline
        int _textline = 0;

        public MainForm()
        {
            InitializeComponent();
            this.link = new Links();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

            
            
            for (int i = 0; i < Constant.DEVICE_LIST.Length; i++)
            {

                this.comboBox_instrumentprefix.Items.Add(Constant.DEVICE_LIST[i]);
                

            }
            
            string filepath = localpath +@"\default.ts";
            if(File.Exists(filepath))
            {
                
                
                TsFileHelper tfh = new TsFileHelper();
                //get tabs from file
                List<Tabcontent> tabs = tfh.getTabs(filepath);
                //delete all current pages
                this.tabControl1.TabPages.Clear();
                //init tabindex
                this._tabindex = 0;
                //add tabpage from tabs
                if(tabs.Count!=0)
                {
                    foreach (Tabcontent tab in tabs)
                    {
                        this.AddTab(tabControl1,toolTip1,tab,ref _tabindex);
                    }
                }
                else
                {
                    Tabcontent tc = new Tabcontent();
                    this.AddTab(tabControl1, toolTip1, tc, ref _tabindex);
                }
                
            }
            else
            {
                string filePath = System.IO.Path.Combine(localpath, "default.ts");
                File.Create(filePath);
                Tabcontent tc = new Tabcontent();
                this.AddTab(tabControl1, toolTip1, tc, ref _tabindex);
            }


            
            //load config

            Dictionary<string, string> tempaddr = new Dictionary<string, string>();
            ConfigHelper ch = new ConfigHelper();
            tempaddr = ch.GetAddr();
            this.addr.SetAddress(tempaddr,this.link);


            //load color

            this.backcolor = ch.GetConfig("disp_backcolor");
            if (this.backcolor != "")
            {
                this.dataDisplayBox.BackColor = ColorTranslator.FromHtml(this.backcolor);
            }

            this.forecolor = ch.GetConfig("disp_forecolor");
            if (this.forecolor != "")
            {
                this.dataDisplayBox.ForeColor = ColorTranslator.FromHtml(this.forecolor);
            }

            //load font
            FontConverter fc = new FontConverter();
            this.fontstyle = ch.GetConfig("disp_font");
            if (this.fontstyle != "")
            {
                this.dataDisplayBox.Font = (Font)fc.ConvertFromString(this.fontstyle);
            }



            //init instrument status
            //initInstrumentStatus(this.addr, this.addr,true);
            this.initInstrumentStatusbyVisa32(this.addr, this.addr, true);
            foreach (Control ctl in this.SerialpropertyBox.Controls)
            {
                if (ctl.Name == "label_rruport")
                {
                    ctl.Text = this.addr.RRU;
                }
                else if (ctl.Name == "label_rrubaud")
                {
                    ctl.Text = this.addr.Baudrate_rru;
                }
                else if (ctl.Name == "label_serial2port")
                {
                    ctl.Text = this.addr.SERIAL2;
                }
                else if (ctl.Name == "label_serial2baud")
                {
                    ctl.Text = this.addr.Baudrate_com2;
                }
            }

            //ipep
            ipep = new IPEndPoint(IPAddress.Any, int.Parse(this.addr.Server_Port));

            //log
            LogManager.LogFielPrefix = "RTT ";
            //string logPath = System.Environment.CurrentDirectory + @"\log\";
            
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            LogManager.LogPath = logPath;
            LogManager.WriteLog(LogFile.Trace, "RTT start, version is "+ Assembly.GetExecutingAssembly().GetName().Version.ToString());

            //update
            
            if (!Directory.Exists(updatePath))
                Directory.CreateDirectory(updatePath);
            
            if (!Directory.Exists(backuppath))
                Directory.CreateDirectory(backuppath);
            //snapshot
            //_snapPath = System.Environment.CurrentDirectory + @"\snapshot\";

            if (!Directory.Exists(_snapPath))
                Directory.CreateDirectory(_snapPath);

            //rxevm
             
            if (!Directory.Exists(_rxevmPath))
                Directory.CreateDirectory(_rxevmPath);

            //cmd queue process thread
            cmdProcessThread_run = true;
            _cmdProcessThread = new Thread(new ThreadStart(cmdProcessThread));
            _cmdProcessThread.Start();
            
            //cqEventSender.QueueChanged += new cmdQueueEventSender.QueueChangHandler(cmdProcessThread);

            this.Text = "RTT v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.label_terminal_IP.Text = this.addr.DU_IP;
            this.Show();
            //this.InputBox.Focus();
            
        }

        //modify current tab' name
        //add a new tab
        private void AddTab(TabControl tcl,ToolTip tp,Tabcontent tc,ref int tab_index)
        {

            NewTabPage Page;
            TableLayoutPanel layoutPanel = new TableLayoutPanel();
            layoutPanel.RowCount = 4;
            layoutPanel.ColumnCount = 8;
            layoutPanel.Dock = DockStyle.Fill;
            //tc has no buttons,add a empty page
            if (tc.tabname == "")
            {
                Page = new NewTabPage();
                Page.Name = "Page" + tab_index.ToString();
                Page.Text = "tabPage" + tab_index.ToString();
                Page.TabIndex = tab_index;
                Page.Controls.Add(layoutPanel);

                tcl.Controls.Add(Page);
                //layout button
                //int x = 8, y = 10;
                for (int i = 0; i != 32; i++)
                {

                    TabButton tb = new TabButton();
                    tb._index = i;
                    tb.Dock = DockStyle.Fill;
                    tp.SetToolTip(tb, tb.Text);
                    /*Page.Controls.Add(tb);
                    tb.Location = new System.Drawing.Point(x, y);
                    x = x + tb.Width + 7;
                    if (x + tb.Width > Page.Width)
                    {
                        x = 8;
                        y = y + tb.Height + 6;
                    }*/
                    for (int k = 0; k < layoutPanel.RowCount; k++)
                    {
                        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                        for (int j = 0; j < layoutPanel.ColumnCount; j++)
                        {
                            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                            layoutPanel.Controls.Add(tb);
                        }
                    }
                    tb.Click += new System.EventHandler(command_button_Click);
                    tb.MouseDown += new MouseEventHandler(command_button_MouseDown);
                }
            }
            //tc has buttons
            else
            {
                Page = new NewTabPage();
                Page.Name = tc.tabname;
                Page.Text = tc.tabname;
                Page.TabIndex = tab_index;
                Page.Controls.Add(layoutPanel);
                tcl.Controls.Add(Page);
                //layout button
                //int x = 8, y = 10;
                for (int i = 0; i != 32; i++)
                {

                    TabButton tb = new TabButton();
                    tb._data = tc.buttons[i].data;
                    tb._index = i;
                    tb.Name = tc.buttons[i].btnname;
                    tb.Text = tc.buttons[i].btnname;
                    tb.Dock = DockStyle.Fill;
                    tp.SetToolTip(tb, tb.Text);
                    /*Page.Controls.Add(tb);
                    tb.Location = new System.Drawing.Point(x, y);
                    x = x + tb.Width + 7;
                    if (x + tb.Width > Page.Width)
                    {
                        x = 8;
                        y = y + tb.Height + 6;
                    }*/
                    for (int k = 0; k < layoutPanel.RowCount; k++)
                    {
                        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                        for (int j = 0; j < layoutPanel.ColumnCount; j++)
                        {
                            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                            layoutPanel.Controls.Add(tb);
                        }
                    }
                    tb.Click += new System.EventHandler(command_button_Click);
                    tb.MouseDown += new MouseEventHandler(command_button_MouseDown);
                }
            }




            #region 三种设置某个选项卡为当前选项卡的方法
            //this.tabControl1.SelectedIndex = index; 
            //this.tabControl1.SelectedTab = Page;
            //this.tabControl1.SelectTab("Page" + index.ToString()); 
            #endregion

            tab_index++;
        }




        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void button_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            ConfigHelper ch = new ConfigHelper();
            string defaultfilename = ch.GetConfig(Constant.CONFIG_TS_LOAD_PATH);
            sfd.InitialDirectory = defaultfilename;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fName = sfd.FileName;

                //this.dataDisplayBox.AppendText(fName + "\n");
                //init List<Tabcontent> tabs

                //get current tabs in mainform
                List<Tabcontent> tabs = GetCurrentTabs();

               
                TsFileHelper tfh = new TsFileHelper();
                tfh.SaveTab(tabs, fName);



            }
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ConfigHelper ch = new ConfigHelper();
            string defaultfilename = ch.GetConfig(Constant.CONFIG_TS_LOAD_PATH);
            ofd.InitialDirectory = defaultfilename;
            ofd.RestoreDirectory = true;
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fName = ofd.FileName;
                
                
                string fpath = fName.Replace(ofd.SafeFileName, "");
                
                TsFileHelper tfh = new TsFileHelper();
                //delete all current pages
                this.tabControl1.TabPages.Clear();
                //init tabindex
                this._tabindex = 0;
                //get tabs from file
                List<Tabcontent> tabs = tfh.getTabs(fName);
                //var tabs = tfh.getTabs(fName);
                //Thread thd = new Thread(Load_Tabs);
                //thd.Start(tabs);
                //add tabpage from tabs
                foreach (Tabcontent tab in tabs)
                {
                    this.AddTab(tabControl1, toolTip1, tab,ref _tabindex);
                }
                ch.UpdateConfig(Constant.CONFIG_TS_LOAD_PATH, fpath);

            }
        }

        //load tabs
        private void Load_Tabs(object  tbs)
        {
            List<Tabcontent> tabs = (List<Tabcontent>)tbs;
            //add tabpage from tabs
            foreach (Tabcontent tab in tabs)
            {
                this.AddTab(tabControl1, toolTip1, tab, ref _tabindex);
            }

        }
        private void button_lock_Click(object sender, EventArgs e)
        {
            if(this.button_lock.Text == "Lock")
            {
                this.tabControl1.Enabled = false;
                this.button_lock.Text = "Unlock";
            }
            else
            {
                this.tabControl1.Enabled = true;
                this.button_lock.Text = "Lock";
            }
            
        }
        private bool CloseCom(MSCommClass com)
        {
            Closing = true;
            //string command = "";
            //this.command_Process(command);
            while (Listening) Application.DoEvents();
            try
            {
                
                
                if (com.PortOpen==true)
                    com.PortOpen = false;
                
                //Com_rru = null;
                Closing = false;
                return true;
            }
            catch (Exception ex)
            {
                WriteErrorText("Close COM "+com.CommPort+" failed!..."+ex.Message+ex.StackTrace);
                Closing = false;
                return false;
            }



        }
        //初始化并打开串口
        private bool InitComm(MSCommClass com,short PortName,string Baudrate, ToolStripButton MenuButton)
        {
            //com = new MSCommClass();
            com.CommPort = PortName;
            com.Settings = Baudrate + ",N,8,1";
            com.InBufferSize = 4096;
            com.OutBufferSize = 4096;
            com.InputLen = 0;
            com.RThreshold = 1;
            com.RTSEnable = true;
            com.DTREnable = true;
            com.InputMode = InputModeConstants.comInputModeBinary;
            com.OnComm += new DMSCommEvents_OnCommEventHandler(Com_OnComm);

            if(com.PortOpen == false)
            {
                try
                {
                    com.PortOpen = true;
                    WriteTraceText("Init COM "+ PortName + " ...sucess.");
                    MenuButton.CheckState = CheckState.Checked;
                }
                catch(Exception e)
                {
                    WriteTraceText("Init COM " + PortName + " failed!..." + e.Message + e.StackTrace);
                    return false;
                }
            }
            return true;
        }
        //wrapper fun of send to serial
        private void SendToSerial(string cmd, MSCommClass com,SerialPort sp)
        {
            if (Serial_status)
                SendToCom(com, cmd);
            else
                SendToSerialPort(sp,cmd);
        }

        private void SendToCom(MSCommClass com,string msg)
        {
            Thread.Sleep(200);
            WriteDebugText("prepare to send cmd to com...");
            msg += '\r';
            try
            {
                byte[] btsend = Encoding.ASCII.GetBytes(msg);
                if(dataProtect&&com==Com_rru)
                    _rrusend++;
                lock (com)
                {
                    com.Output = btsend;
                }
                
            }
            catch (Exception e)
            {
                WriteTraceText("Send cmd to COM "+com.CommPort+" failed..." + e.Message + e.StackTrace);
            }
            

        }
        private void Com_OnComm()
        {
            if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
            Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
            try
            {
                if (Com_rru.CommEvent == (short)(MSCommLib.OnCommConstants.comEvReceive))
                {

                    //Thread.Sleep(200);
                    //Com_rru.InputLen = 0;
                    byte[] readbuf=new byte[1];
                    lock (Com_rru)
                    {
                        readbuf = (byte[])Com_rru.Input;
                    }
                     
                    char[] buf;
                    if(readbuf.Length>0)
                    {
                        buf = Encoding.ASCII.GetChars(readbuf);
                        for (int i = 0; i != buf.Length; i++)
                        {
                            if (buf[i] == '\n' || buf[i] == '\r')
                            {
                                if (buf[i] == '\n')
                                {
                                    //builder.Append("\n");
                                    //tempstr = builder.ToString();
                                    //if(tempstr.Trim(' ').Trim('\n').Length !=0)
                                    WriteTraceText(builder.ToString());
                                    if (this.socketTag == true)
                                    {
                                        lock (socketbuilder)
                                        {
                                            this.socketbuilder.Append(builder.ToString() + '\n');
                                        }

                                        WriteDebugText("append to socketbuilder:" + builder.ToString());
                                    }
                                    builder.Remove(0, builder.Length);
                                }
                            }
                            else if (buf[i] == '$')
                            {
                                builder.Append(buf[i]);
                                //tempstr = builder.ToString();
                                WriteTraceText(builder.ToString());

                                if (this.socketTag == true)
                                {
                                    lock (socketbuilder)
                                    {
                                        this.socketbuilder.Append(builder.ToString());
                                    }
                                    WriteDebugText("append to socketbuilder:" + builder.ToString());
                                }
                                if (dataProtect)
                                {
                                    _rruresp++;
                                    if (_rruresp > _rrusend)
                                        _rruresp = _rrusend;
                                }

                                //builder.Clear();
                                builder.Remove(0, builder.Length);
                            }
                            else
                            {
                                builder.Append(buf[i]);
                            }
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                WriteErrorText("rru receive error: " + exp.Message + exp.StackTrace);

            }

            finally
            {
                Listening = false;//我用完了，ui可以关闭串口了。  
            }
            
            
            
        }

        //修改串口参数
        private void ModifySerialParameter(SerialPort sp,string PortName, int Baudrate,ToolStripButton SerialConnBtn)
        {
            if (sp.IsOpen == true)
            {
                if (Close_serial_port(sp))
                {
                    SerialConnBtn.CheckState = CheckState.Unchecked;
                    InitSerialPort(sp, PortName, Baudrate,SerialConnBtn);
                }
            }
        }
        private void ModifySerialParameter(MSCommClass com, string PortName, int Baudrate, ToolStripButton SerialConnBtn)
        {
            if (com.PortOpen == true)
            {
                if (CloseCom(com))
                {
                    SerialConnBtn.CheckState = CheckState.Unchecked;
                    if (PortName.Contains("COM"))
                        InitComm(com, short.Parse(PortName.Remove(0, 3)), Baudrate.ToString(), SerialConnBtn);
                    else
                        WriteTraceText("Wrong SerialPortName..." + PortName);
                }
            }
        }
        //初始化并打开串口
        private bool InitSerialPort(SerialPort Com,string PortName,int Baudrate,ToolStripButton MenuButton)
        {
            //设定port,波特率,无检验位，8个数据位，1个停止位
            //Com = new SerialPort(PortName, Baudrate, Parity.None, 8, StopBits.One);
            Com.PortName = PortName;
            Com.BaudRate = Baudrate;
            Com.Parity = Parity.None;
            Com.StopBits = StopBits.One;
            Com.DataBits = 8; 
            Com.ReadBufferSize = 2048;
            Com.ReceivedBytesThreshold = 1;
            Com.NewLine = "\n";
            Com.DtrEnable = true;
            Com.RtsEnable = true;
            Com.ReadTimeout = 2000;
            
            //Com.WriteTimeout = 2000;


            //open serial port

            try
            {

                Com.Open();
                Com.DiscardInBuffer();
                Com.DiscardOutBuffer();
                Com.DataReceived += Com_rru_DataReceived;
                Com.ErrorReceived += Com_rru_ErrorReceived;
                WriteTraceText(PortName + " open, and start Listenning.");

                MenuButton.CheckState = CheckState.Checked;
            }
            catch (Exception ex)
            {
                //Com = null;
                //现实异常信息给客户。  
                WriteErrorText(PortName + " open failed: " + ex.Message+ex.StackTrace);
                return false;
            }
            return true;
        }
        //open serial port
        private bool OpenSerialPort(SerialPort sp, MSCommClass com,string PortName,string Baudrate,ToolStripButton SerialConBtn)
        {
            bool isSucess = false;
            if (Serial_status)
            {
                if (PortName.Contains("COM"))
                {
                    try
                    {
                        isSucess = InitComm(com, short.Parse(PortName.Remove(0, 3)), Baudrate, SerialConBtn);
                    }
                    catch (Exception e)
                    {
                        WriteErrorText("InitComm failed!..."+e.Message+e.StackTrace);
                    }
                    
                }
                    
                else
                {
                    WriteTraceText("Wrong SerialPortName..." + addr.RRU);
                }
                    
            }

            else
                try
                {
                    isSucess = InitSerialPort(sp, PortName, int.Parse(Baudrate), SerialConBtn);
                }
                catch (Exception e)
                {
                    WriteErrorText("InitSerialPort failed!..." + e.Message + e.StackTrace);
                }
            

            return isSucess;
        }
        //close serial port
        private bool CloseSerial(MSCommClass com,SerialPort sp)
        {
            if (Serial_status)
            {
                if (CloseCom(com))
                {
                    this.rruConnButton.CheckState = CheckState.Unchecked;
                    WriteTraceText("COM "+ com.CommPort+" closed.");
                }
            }
            else
            {
                if (Close_serial_port(sp))
                {
                    this.rruConnButton.CheckState = CheckState.Unchecked;
                    WriteTraceText("Serial port "+ sp.PortName+" closed.");
                }
            }
            return true;
        }
        //rru serialport open ,and listen to datareceive when checked state, close when unchecked state
        private void Rru_con_Button_Click(object sender, EventArgs e)
        {
            //rru serialport open ,and listen to datareceive
            if (this.rruConnButton.Checked == false)
            {
                //初始化串口，如果未设置串口则弹出对话框提示用户先设置串口
                if (this.addr.RRU == "")
                {
                    WriteTraceText("Please setup serial port first!");                     
                }
                else
                {
                    OpenSerialPort(_COM_RRU,Com_rru,addr.RRU,addr.Baudrate_rru, rruConnButton);

                }

            }
            //close serial port
            else
            {
                CloseSerial(Com_rru,_COM_RRU);
            }

        }
        private void Close_serial2_port()
        {
            Closing_com2 = true;
            //string command = "";
            //this.command_Process(command);
            while (Listening_com2) Application.DoEvents();
            try
            {
                this._COM2.Close();
                WriteTraceText("close serial 2 port.");
            }
            catch (Exception ex)
            {
                WriteErrorText("Close serial port 2 failed!");

            }
            
            Closing_com2 = false;
            
            
        }

        //close serial port
        private bool Close_serial_port(SerialPort sp)
        {
            Closing = true;
            //string command = "";
            //this.command_Process(command);
            while (Listening) Application.DoEvents();
            try
            {
                if(sp.IsOpen)
                    sp.Close();
                
                //this._COM_RRU = null;
                //WriteTraceText("Ru serial port closed.");
                Closing = false;
                return true;
            }
            catch(Exception ex)
            {
                WriteErrorText("Close Ru serial port failed! "+ex.Message+ex.StackTrace);
                Closing = false;
                return false;
            }
            
            
            
        }

        //error data receive
        private void Com2_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            WriteTraceText(sender.ToString() + " : " + e.ToString());
            
            this._COM2.DiscardInBuffer();
            this._COM2.DiscardOutBuffer();

        }

        private void Com_2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(200);
            if (Closing_com2) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环  
            try
            {
                Listening_com2 = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。  
                //int n = this.link.com_rru.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
                string tempstr = this._COM2.ReadExisting();
                if (tempstr.Trim().Length != 0)
                {
                    //if (checkBox_pause.Checked == false)
                        this.Invoke((EventHandler)(delegate 
                        {
                            
                            this.dataDisplayBox.AppendText(tempstr);
                            if (this.checkBox_AutoscrollDown.Checked)
                            {
                                this.dataDisplayBox.ScrollToCaret();
                            }
                            

                        }));
                    
                    
                }
                
                


            }
            finally
            {
                Listening_com2 = false;//我用完了，ui可以关闭串口了。  
            }
        }

        //error data receive
        private void Com_rru_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            WriteTraceText(sender.ToString() + " : " + e.ToString());
            
            this._COM_RRU.DiscardInBuffer();
            this._COM_RRU.DiscardOutBuffer();
            
        }
        //serial rru datareceive
        private void Com_rru_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //string tempstr;
            if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环  
            try
            {
                Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                byte[] readbuf = new byte[this._COM_RRU.BytesToRead];
                char[] buf;
                int n = 0;
                //lock (_lock_RRUCOM)
                //{
                    //tempstr = this._COM_RRU.ReadExisting();
                    try
                    {
                        n = this._COM_RRU.Read(readbuf, 0, readbuf.Length);
                    }
                    catch(Exception ex)
                    {
                        n = 0;
                        WriteErrorText("Read from RRU failed! "+ex.Message);
                    }
                    
                    
                    //foreach(var b in readbuf)
                    //{
                    //    WriteDebugText("count+" + b);
                    //}
                    
                    
                    if (n>0)
                    {
                        buf = Encoding.ASCII.GetChars(readbuf);
                        for (int i=0; i!= buf.Length; i++)
                        {
                            if(buf[i]=='\n'||buf[i]=='\r')
                            {
                                if(buf[i]=='\n')
                                {
                                    //builder.Append("\n");
                                    //tempstr = builder.ToString();
                                    //if(tempstr.Trim(' ').Trim('\n').Length !=0)
                                    WriteTraceText(builder.ToString());
                                    if (this.socketTag == true)
                                    {
                                        lock(socketbuilder)
                                        {
                                            this.socketbuilder.Append(builder.ToString() + '\n');
                                        }
                                        
                                        WriteDebugText("append to socketbuilder:"+ builder.ToString());
                                    }
                                //builder.Clear();
                                builder.Remove(0, builder.Length);
                            }
                            }
                            else if(buf[i]=='$')
                            {
                                builder.Append(buf[i]);
                                //tempstr = builder.ToString();
                                WriteTraceText(builder.ToString());
                                
                                if (this.socketTag == true)
                                {
                                    lock (socketbuilder)
                                    {
                                        this.socketbuilder.Append(builder.ToString());
                                    }
                                    WriteDebugText("append to socketbuilder:" + builder.ToString());
                                }
                                if(dataProtect)
                                {
                                    _rruresp++;
                                    if (_rruresp > _rrusend)
                                        _rruresp = _rrusend;
                                }

                            //builder.Clear();
                            builder.Remove(0, builder.Length);
                        }
                            else
                            {
                                builder.Append(buf[i]);
                            }
                        }
                        
                    }
                


            }
            catch(Exception exp)
            {
                WriteErrorText("COM_RRU receive error: "+exp.Message);
                
            }    
            
            finally
            {
                Listening = false;//我用完了，ui可以关闭串口了。  
            }
        }
        /*
        private void Com_rru_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(200);
            if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环  
            try  
            {  
                Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。  
                int n = this.link.com_rru.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致  
                byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据  
                //received_count += n;//增加接收计数  
                this.link.com_rru.Read(buf, 0, n);//读取缓冲数据 
                this.link.com_rru.DiscardInBuffer();
                //this.dataDisplayBox.AppendText(n.ToString()+"\n");
                //this.dataDisplayBox.AppendText(Encoding.ASCII.GetString(buf) + "\n");
                //this.dataDisplayBox.AppendText("11111111111\n");
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////  
                //<协议解析>  
                bool data_1_catched = false;//缓存记录数据是否捕获到  
                                            //1.缓存数据  
                buffer.AddRange(buf);  
                //2.完整性判断  
                while (buffer.Count >= 4)//至少要包含头（2字节）+长度（1字节）+校验（1字节）  
                {  
                    //请不要担心使用>=，因为>=已经和>,<,=一样，是独立操作符，并不是解析成>和=2个符号  
                    //2.1 查找数据头  
                    if (buffer[0] == 0xAA && buffer[1] == 0x44)  
                    {  
                        //2.2 探测缓存数据是否有一条数据的字节，如果不够，就不用费劲的做其他验证了  
                        //前面已经限定了剩余长度>=4，那我们这里一定能访问到buffer[2]这个长度  
                        int len = buffer[2];//数据长度  
                        //数据完整判断第一步，长度是否足够  
                        //len是数据段长度,4个字节是while行注释的3部分长度  
                        if (buffer.Count<len + 4) break;//数据不够的时候什么都不做  
                        //这里确保数据长度足够，数据头标志找到，我们开始计算校验  
                        //2.3 校验数据，确认数据正确  
                        //异或校验，逐个字节异或得到校验码  
                        byte checksum = 0;  
                        for (int i = 0; i<len + 3; i++)//len+3表示校验之前的位置  
                        {  
                            checksum ^= buffer[i];  
                        }  
                        if (checksum != buffer[len + 3]) //如果数据校验失败，丢弃这一包数据  
                        {  
                            buffer.RemoveRange(0, len + 4);//从缓存中删除错误数据  
                            continue;//继续下一次循环  
                        }
//至此，已经被找到了一条完整数据。我们将数据直接分析，或是缓存起来一起分析  
//我们这里采用的办法是缓存一次，好处就是如果你某种原因，数据堆积在缓存buffer中  
//已经很多了，那你需要循环的找到最后一组，只分析最新数据，过往数据你已经处理不及时  
//了，就不要浪费更多时间了，这也是考虑到系统负载能够降低。  
                        buffer.CopyTo(0, binary_data_1, 0, len + 4);//复制一条完整数据到具体的数据缓存  
                        data_1_catched = true;  
                        buffer.RemoveRange(0, len + 4);//正确分析一条数据，从缓存中移除数据。  
                    }  
                    else  
                    {  
                        //这里是很重要的，如果数据开始不是头，则删除数据  
                        buffer.RemoveAt(0);  
                    }  
                }  
                //分析数据  
                if (data_1_catched)  
                {  
                    //我们的数据都是定好格式的，所以当我们找到分析出的数据1，就知道固定位置一定是这些数据，我们只要显示就可以了  
                    string data = binary_data_1[3].ToString("X2") + " " + binary_data_1[4].ToString("X2") + " " +
                        binary_data_1[5].ToString("X2") + " " + binary_data_1[6].ToString("X2") + " " +
                        binary_data_1[7].ToString("X2");
                    //更新界面  
                    //this.Invoke((EventHandler)(delegate { this.dataDisplayBox.Text = data; }));  
                }  
                //如果需要别的协议，只要扩展这个data_n_catched就可以了。往往我们协议多的情况下，还会包含数据编号，给来的数据进行  
                //编号，协议优化后就是： 头+编号+长度+数据+校验  
                //</协议解析>  
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////  
                builder.Clear();//清除字符串构造器的内容  
                //因为要访问ui资源，所以需要使用invoke方式同步ui。  
                this.Invoke((EventHandler)(delegate
                {  
                    //判断是否是显示为16禁止  
                    if (checkBoxHexView.Checked)  
                    {  
                        //依次的拼接出16进制字符串  
                        foreach (byte b in buf)  
                        {  
                            builder.Append(b.ToString("X2") + " ");  
                        }  
                    }  
                    else  
                    {
                        //依次判断段落结尾，默认为\n
                        for(int i =0; i!=buf.Length;i++)
                        {
                            if (buf[i] != System.BitConverter.GetBytes(10)[0])
                            {
                                byte[] tempbytes = new byte[1];
                                tempbytes[0] = buf[i];
                                builder.Append(Encoding.ASCII.GetString(tempbytes));
                            }
                                
                            else
                            {
                                string tempstr = builder.ToString();
                                if (tempstr.Trim().Length != 0)
                                {
                                    this.dataDisplayBox.AppendText(tempstr + "\n");
                                    LogManager.WriteLog(LogFile.Trace, tempstr);
                                    //append to socketbuilder
                                    if (this.socketTag == true)
                                        socketbuilder.Append(tempstr + "\n");
                                    builder.Clear();
                                }

                                
                            }
                        }
                        
                        //直接按ASCII规则转换成字符串  
                        
                        //receivebuilder.Append(Encoding.ASCII.GetString(buf));
                        //append to socketbuilder
                        //if (this.socketTag==true)
                            //socketbuilder.Append(Encoding.ASCII.GetString(buf));
                    }
                    //追加的形式添加到文本框末端，并滚动到最后。 
                    //SetText(builder.ToString());
                    //this.dataDisplayBox.AppendText(builder.ToString());
                    
                    
                    if (receivebuilder.ToString().Contains('$'))
                    {
                        LogManager.WriteLog(LogFile.Trace, (receivebuilder.ToString()));
                        receivebuilder.Clear();
                    }
                    else if(receivebuilder.Length>1024)
                    {
                        LogManager.WriteLog(LogFile.Trace, (receivebuilder.ToString()));
                        receivebuilder.Clear();
                    }
                    
                    
                    
//修改接收计数  
//labelGetCount.Text = "Get:" + received_count.ToString();
}));  
            }  
            finally  
            {  
                Listening = false;//我用完了，ui可以关闭串口了。  
            }  
        }

    */
        //send button
        //private async void button_sendcommand_Click(object sender, EventArgs e)
        private  void button_sendcommand_Click(object sender, EventArgs e)
        {
            //sender to rru or device
            if(tabControl_display.SelectedTab == tab_main_display)
            {
                string command = this.InputBox.Text;
                cmdQueue.Enqueue(command);
                _waitcmdQueueEventHandle.Set();
            }
            //send to du
            else if(tabControl_display.SelectedTab == tabPage_remote)
            {
                string command = this.InputBox.Text;
                Addhistory(command);
                
                t.Send(command);
                    
            }
            if(!cmdHold)
                this.InputBox.Clear();
            //this.command_Process(command);



        }

        
        //command from socket
        private string SocketCommandProcess(string command)
        {
            

            string result = " ";
            //cmd$delay\0
            if (command != null)
            {



                WriteDebugText("socket receive: " + command);
                string[] scokcmd = command.Split('$');
                
                if (scokcmd.Length==2)
                    result = this.command_Process(scokcmd[0], true,int.Parse(scokcmd[1]));
                else if(scokcmd.Length == 3)
                    result = this.command_Process(scokcmd[0], true,int.Parse(scokcmd[1]), scokcmd[2]);

            }
            
            return result;
        }

        //in visacom mode
        private void SaCapturebyVisacom(IMessage sesnCapture,string name = "",int delay = 5000)
        {
            
            StringBuilder filename = new StringBuilder();
            if (name != "")
            {
                filename.Append(name).Append(@".png");
            }
            else
            {
                filename.Append(DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")).Append(@".png");
            }
            string cmd = "MMEM:STOR:SCR \"D:\\rttscr.png\"";//;*OPC?
            sesnCapture.WriteString(cmd);
            sesnCapture.WriteString("MMEM:DATA? \"D:\\rttscr.png\"");
            
            //this.sa_sesn.WriteString("CALCulate:DATA?");
            
            byte[] readbuf;
            
            readbuf = sesnCapture.Read(1000000);
            //WriteDebugText(Encoding.ASCII.GetString(readbuf));
            //if(delay>5000)
            //    Thread.Sleep(delay);
            //else
            //    Thread.Sleep(5000);
            sesnCapture.WriteString("MMEM:DEL \"D:\\rttscr.png\"");
            sesnCapture.WriteString("*CLS");
            
            byte[] size = { readbuf[1] };
            
            byte[] newbuf = readbuf.Skip(2+ int.Parse(Encoding.ASCII.GetString(size))).Take(readbuf.Length-9).ToArray();
            
            File.WriteAllBytes(_snapPath + filename.ToString(), newbuf);
        }

        //only in visa32 mode
        private void SaCapture(int vi,string name="", int delay = 5000)
        {
            // SA截图
            //int status = -1;
            StringBuilder filename = new StringBuilder();
            byte[] buf = new byte[1];

            if (name != "")
            {
                filename.Append(name).Append(@".png");
            }
            else
            {
                filename.Append(DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")).Append(@".png");
            }
            string cmdfilename = " \"D:\\" + filename.ToString() + "\"";
            string cmd = "MMEM:STOR:SCR"+ cmdfilename;//;*OPC?
            //WriteTraceText(cmd);
            //buf = Encoding.ASCII.GetBytes(cmd);
            
            int recount;
            visa32.viPrintf(vi, cmd+"\n");//.viWrite(viSA, buf, buf.Length, out recount);
            
            //buf = Encoding.ASCII.GetBytes("MMEM:DATA? \"D:\\rttscr.png\"\n");
            cmd = "MMEM:DATA?"+ cmdfilename;
            visa32.viPrintf(vi, cmd + "\n");//.viWrite(viSA, buf, buf.Length, out recount);
                
            byte[] readbuf = new byte[1000000];
                    


            //int tmep = readbuf.Length;

            visa32.viRead(vi, readbuf, readbuf.Length, out recount);
                    
            //if (delay > 5000)
            //    Thread.Sleep(delay);
            //else
            //    Thread.Sleep(5000);
            cmd = "MMEM:DEL"+cmdfilename;
            visa32.viPrintf(vi, cmd + "\n"); //"MMEM:STOR:SCR \"D:\\" + filename
                        
            cmd = "*CLS";
            visa32.viPrintf(vi, "*CLS\n");
                            
            //int size = System.BitConverter.ToInt32(readbuf, 1);
            byte[] size = { readbuf[1] };
            //WriteText(Encoding.ASCII.GetString(size));
            byte[] newbuf = readbuf.Skip(2 + int.Parse(Encoding.ASCII.GetString(size))).Take(recount - 9).ToArray();
            //byte[] newbuf = readbuf.Take(recount).ToArray();
            try
            {
                File.WriteAllBytes(_snapPath + filename.ToString(), newbuf);
            }
            catch(Exception ex)
            {
                WriteTraceText("write capture picture failed! " + ex.Message);
            }
            
                            
                            
                        
                        
                    
                    
                
                
           
            

            

        }
        //all command process
        private string command_Process(string cmd,bool socket = false,int delay=0,string filename = "")
        {
            

            bool printtag = true;
            string result = "";
            char[] cl = { ':' };
            string sendcmd = "";
            
            Addhistory(cmd);
            LogManager.WriteLog(LogFile.Command, cmd);
            //WriteTraceText(cmd);

            //zhushi
            if (!cmd.StartsWith("#"))
            {



                //根据前缀判断仪器
                if (cmd.Contains("SA."))
                {
                    if (cmd.Contains(Constant.PRIFIX_SA))
                    {

                        sendcmd = cmd.Replace(Constant.PRIFIX_SA, "").TrimStart(cl);
                        if (this.addr.SA != "")
                        {
                            try
                            {
                                if (this.VisaSwitch)  //true == visa32
                                {
                                    result = this.send_to_instrument(cmd, this.addr.SA, sendcmd, ref this.sesnSA, ref this.viSA, delay);
                                }
                                else
                                {
                                    result = this.send_to_instrument(cmd, this.addr.SA, sendcmd, this.sa_sesn, delay);
                                }
                                if (this.tag_sa.BackColor == Color.Pink)
                                {
                                    this.tag_sa.BackColor = Color.SpringGreen;
                                }


                            }
                            catch (Exception e)
                            {
                                this.tag_sa.BackColor = Color.Pink;
                            }
                        }
                        else
                        {
                            WriteTraceText("Please setup SA address first!");

                        }



                    }
                    
                }
                else if (cmd.Contains("Capture1"))
                {
                    WriteTraceText(cmd);
                    if (this.addr.capture1 != string.Empty)
                    {
                        if (this.VisaSwitch) //true == visa32
                        {
                            this.SaCapture(viCapture1, filename,delay);
                        }
                        else
                        {
                            this.SaCapturebyVisacom(sesnCapture1, filename,delay);
                        }
                    }
                    else
                        WriteTraceText("Please input capture1 address first!");
                    
                    

                }
                else if (cmd.Contains("Capture2"))
                {

                    WriteTraceText(cmd);
                    if (this.addr.capture2 != string.Empty)
                    {
                        if (this.VisaSwitch) //true == visa32
                        {
                            this.SaCapture(viCapture2, filename, delay);
                        }
                        else
                        {
                            this.SaCapturebyVisacom(sesnCapture2, filename, delay);
                        }
                    }
                    else
                        WriteTraceText("Please input capture2 address first!");
                    

                }
                else if (cmd.Contains(Constant.PRIFIX_SG))
                {
                    sendcmd = cmd.Replace(Constant.PRIFIX_SG, "").TrimStart(cl);
                    if (this.addr.SG != "")
                    {
                        try
                        {
                            if (this.VisaSwitch)  //true == visa32
                            {
                                result = this.send_to_instrument(cmd, this.addr.SG, sendcmd, ref this.sesnSG, ref this.viSG, delay);

                            }
                            else
                            {
                                result = this.send_to_instrument(cmd, this.addr.SG, sendcmd, this.sg_sesn, delay);
                            }
                            if (this.tag_sg1.BackColor == Color.Pink)
                            {
                                this.tag_sg1.BackColor = Color.SpringGreen;
                            }


                        }
                        catch (Exception e)
                        {
                            this.tag_sg1.BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        WriteTraceText("Please setup SG address first!");

                    }

                }
                else if (cmd.Contains(Constant.PRIFIX_SG2))
                {
                    sendcmd = cmd.Replace(Constant.PRIFIX_SG2, "").TrimStart(cl);
                    if (this.addr.SG2 != "")
                    {
                        try
                        {
                            if (this.VisaSwitch)  //true == visa32
                            {
                                result = this.send_to_instrument(cmd, this.addr.SG2, sendcmd, ref this.sesnSG2, ref this.viSG2, delay);
                            }
                            else
                            {
                                result = this.send_to_instrument(cmd, this.addr.SG2, sendcmd, this.sg2_sesn, delay);
                            }
                            if (this.tag_sg2.BackColor == Color.Pink)
                            {
                                this.tag_sg2.BackColor = Color.SpringGreen;
                            }


                        }
                        catch (Exception e)
                        {
                            this.tag_sg2.BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        WriteTraceText("Please setup SG2 address first!");

                    }

                }
                else if (cmd.Contains(Constant.PRIFIX_IS1))
                {
                    sendcmd = cmd.Replace(Constant.PRIFIX_IS1, "").TrimStart(cl);
                    if (this.addr.IS1 != "")
                    {
                        try
                        {
                            if (this.VisaSwitch)  //true == visa32
                            {
                                result = this.send_to_instrument(cmd, this.addr.IS1, sendcmd, ref this.sesnIS, ref this.viIS, delay);
                            }
                            else
                            {
                                result = this.send_to_instrument(cmd, this.addr.IS1, sendcmd, this.is_sesn, delay);
                            }
                            if (this.tag_is1.BackColor == Color.Pink)
                            {
                                this.tag_is1.BackColor = Color.SpringGreen;
                            }


                        }
                        catch (Exception e)
                        {
                            this.tag_is1.BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        WriteTraceText("Please setup IS1 address first!");
                    }

                }
                else if (cmd.Contains(Constant.PRIFIX_IS2))
                {
                    sendcmd = cmd.Replace(Constant.PRIFIX_IS2, "").TrimStart(cl);
                    if (this.addr.IS2 != "")
                    {
                        try
                        {
                            if (this.VisaSwitch)  //true == visa32
                            {
                                result = this.send_to_instrument(cmd, this.addr.IS2, sendcmd, ref this.sesnIS2, ref this.viIS2, delay);
                            }
                            else
                            {
                                result = this.send_to_instrument(cmd, this.addr.IS2, sendcmd, this.is2_sesn, delay);
                            }
                            if (this.tag_is2.BackColor == Color.Pink)
                            {
                                this.tag_is2.BackColor = Color.SpringGreen;
                            }


                        }
                        catch (Exception e)
                        {
                            this.tag_is2.BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        WriteTraceText("Please setup IS2 address first!");
                    }

                }
                else if (cmd.Contains(Constant.PRIFIX_RFBOX))
                {
                    sendcmd = cmd.Replace(Constant.PRIFIX_RFBOX, "");
                    if (this.addr.RFBOX != "")
                    {
                        try
                        {
                            if (this.VisaSwitch)  //true == visa32
                            {
                                result = this.send_to_instrument(cmd, this.addr.RFBOX, sendcmd, ref this.sesnRFBOX, ref this.viRFBOX, delay);
                            }
                            else
                            {
                                result = this.send_to_instrument(cmd, this.addr.RFBOX, sendcmd, this.rfbox_sesn, delay);
                            }
                            if (this.tag_rfbox1.BackColor == Color.Pink)
                            {
                                this.tag_rfbox1.BackColor = Color.SpringGreen;
                            }


                        }
                        catch (Exception e)
                        {
                            this.tag_rfbox1.BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        WriteTraceText("Please setup RFBOX address first!");
                    }

                }
                else if (cmd.Contains(Constant.PRIFIX_RFBOX2))
                {
                    sendcmd = cmd.Replace(Constant.PRIFIX_RFBOX2, "");
                    if (this.addr.RFBOX2 != "")
                    {
                        try
                        {
                            if (this.VisaSwitch)  //true == visa32
                            {
                                result = this.send_to_instrument(cmd, this.addr.RFBOX2, sendcmd, ref this.sesnRFBOX2, ref this.viRFBOX2, delay);
                            }
                            else
                            {
                                result = this.send_to_instrument(cmd, this.addr.RFBOX2, sendcmd, this.rfbox2_sesn, delay);
                            }
                            if (this.tag_rfbox2.BackColor == Color.Pink)
                            {
                                this.tag_rfbox2.BackColor = Color.SpringGreen;
                            }



                        }
                        catch (Exception e)
                        {
                            this.tag_rfbox2.BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        WriteTraceText("Please setup RFBOX2 address first!");
                    }

                }
                else if (cmd.Contains(Constant.PRIFIX_DC5767A))
                {
                    sendcmd = cmd.Replace(Constant.PRIFIX_DC5767A, "").TrimStart(cl);
                    if (this.addr.DC5767A != "")
                    {
                        try
                        {
                            if (this.VisaSwitch)  //true == visa32
                            {
                                this.send_to_instrument(cmd, this.addr.DC5767A, sendcmd, ref this.sesnDC5767A, ref this.viDC5767A);
                            }
                            else
                            {
                                this.send_to_instrument(cmd, this.addr.DC5767A, sendcmd, this.dc5767a_sesn, delay);
                            }
                            if (this.tag_DC5767A.BackColor == Color.Pink)
                            {
                                this.tag_DC5767A.BackColor = Color.SpringGreen;
                            }
                             

                        }
                        catch (Exception e)
                        {
                            this.tag_DC5767A.BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        WriteTraceText("Please setup DC5767A address first!");
                    }

                }
                else if (cmd.Contains(Constant.PRIFIX_RUMASTER))
                {
                    WriteTraceText(cmd);
                    sendcmd = cmd.Replace(Constant.PRIFIX_RUMASTER, "").TrimStart(cl);
                    if (this.tag_rumaster.BackColor != Color.Pink)
                    {
                        try
                        {
                            if (sendcmd.Contains("setIQfile"))
                            {
                                string[] cmds = sendcmd.Split('#');
                                if (cmds.Length >= 3)
                                {
                                    this.RumasterSetIQfile(cmds[1], cmds[2]);

                                }


                        }
                        else if (sendcmd.Contains("setCPCfile"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 3)
                            {
                                this.RumasterSetCPCfile(cmds[1], cmds[2]);
                            }
                        }
                        else if (sendcmd.Contains("startplayback"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 3)
                            {

                                if (cmds[2] == "all")
                                {
                                    Tiger.Ruma.FlowDataType[] flows = { Tiger.Ruma.FlowDataType.AXC, Tiger.Ruma.FlowDataType.IQ };
                                    this.RumasterStartPlayBack(cmds[1], flows);
                                }
                                else if (cmds[2] == "axc")
                                {
                                    Tiger.Ruma.FlowDataType[] flows = { Tiger.Ruma.FlowDataType.AXC };
                                    this.RumasterStartPlayBack(cmds[1], flows);
                                }
                                else if (cmds[2] == "iq")
                                {
                                    Tiger.Ruma.FlowDataType[] flows = { Tiger.Ruma.FlowDataType.IQ };
                                    this.RumasterStartPlayBack(cmds[1], flows);
                                }
                                else
                                {
                                    WriteTraceText("Rumaster Commands format error!");
                                }
                            }
                            else
                            {
                                WriteTraceText("Rumaster Commands format error!");
                            }
                        }
                        else if (sendcmd.Contains("stopplayback"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 3)
                            {
                                if (cmds[2] == "all")
                                {
                                    Tiger.Ruma.FlowDataType[] flows = { Tiger.Ruma.FlowDataType.AXC, Tiger.Ruma.FlowDataType.IQ };
                                    this.RumasterStopPlayBack(cmds[1], flows);
                                }
                                else if (cmds[2] == "axc")
                                {
                                    Tiger.Ruma.FlowDataType[] flows = { Tiger.Ruma.FlowDataType.AXC };
                                    this.RumasterStopPlayBack(cmds[1], flows);
                                }
                                else if (cmds[2] == "iq")
                                {
                                    Tiger.Ruma.FlowDataType[] flows = { Tiger.Ruma.FlowDataType.IQ };
                                    this.RumasterStopPlayBack(cmds[1], flows);
                                }
                                else
                                {
                                    WriteTraceText("Rumaster Commands format error!");
                                }
                            }
                            else
                            {
                                WriteTraceText("Rumaster Commands format error!");
                            }

                        }
                        else if (sendcmd.Contains("CpcFileSetLoopLengthFromIQFile"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 4)
                            {
                                this.RumasterCpcFileSetLoopLengthFromIQFile(cmds[1], cmds[2], cmds[3]);
                            }
                            else
                            {
                                WriteTraceText("Please check parameters first!");
                            }
                        }
                        else if (sendcmd.Contains("GetStartStopCondition"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 4)
                            {
                                result = this.RumasterGetStartStopCondition(cmds[1], cmds[2], cmds[3]);
                            }
                        }
                        else if (sendcmd.Contains("SetStartStopCondition"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 8)
                            {
                                this.RumasterSetStartStopCondition(cmds[1], cmds[2], cmds[3], cmds[4], cmds[5], cmds[6], cmds[7]);
                            }
                        }
                        else if (sendcmd.Contains("GetFlowDataMode"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 4)
                            {
                                result = this.RumasterGetFlowDataMode(cmds[1], cmds[2], cmds[3]);
                            }
                        }
                        else if (sendcmd.Contains("SetFlowDataMode"))
                        {
                            string[] cmds = sendcmd.Split('#');
                            if (cmds.Length >= 5)
                            {
                                this.RumasterSetFlowDataMode(cmds[1], cmds[2], cmds[3], cmds[4]);
                            }
                        }
                        else if (sendcmd.Contains("DeleteAllCarriers"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 3)
                            {
                                this.RumasterDeleteAllCarriers(cmds[1], cmds[2]);
                            }
                        }
                        else if (sendcmd.Contains("DeleteCarrier"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 4)
                            {
                                this.RumasterDeleteCarrier(cmds[1], cmds[2], cmds[3]);
                            }
                        }
                        else if (sendcmd.Contains("AddCarrier"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 7)
                            {
                                this.RumasterAddCarrier(cmds[1], cmds[2], cmds[3], cmds[4], cmds[5], cmds[6]);
                            }
                        }
                        else if (sendcmd.Contains("GetCarrierConfig"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 4)
                            {
                                result = this.RumasterGetCarrierConfig(cmds[1], cmds[2], cmds[3]);
                            }
                        }
                        else if (sendcmd.Contains("SetCarrierConfig"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 14)
                            {
                                this.RumasterSetCarrierConfig(cmds[1], cmds[2], cmds[3], cmds[4], cmds[5], cmds[6], cmds[7], cmds[8], cmds[9], cmds[10], cmds[11], cmds[12], cmds[13]);
                            }
                        }
                        else if (sendcmd.Contains("SetCpriConfig"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 4)
                            {
                                this.RumasterSetCpriConfig(cmds[1], cmds[2], cmds[3]);
                            }
                        }
                        else if (sendcmd.Contains("GetCpriConfig"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 3)
                            {
                                result = this.RumasterGetCpriConfig(cmds[1], cmds[2]);
                            }
                        }
                        else if (sendcmd.Contains("GetPllRef"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 3)
                            {
                                result = this.RumasterGetPllRef(cmds[1], cmds[2]);
                            }
                        }
                        else if (sendcmd.Contains("SetPllRef"))
                        {
                            string[] cmds = sendcmd.Split(SPACER_FLAG_FIR);
                            if (cmds.Length >= 3)
                            {
                                this.RumasterSetPllRef(cmds[1], cmds[2]);
                            }
                        }     
                        else if (sendcmd.Contains("rxevmcaptureOnce"))
                        {
                            string[] cmds = sendcmd.Split('#');     //rxevmcapture#cpriport#filename(option)
                            string _tempfilename;
                            if (cmds.Length >= 2)
                            {
                                try
                                {

                                        // fill in with port, direction , data tyep and mode, ok?ok
                                        this.icdf.SetFlowDataMode(cmds[1], CpriFlowDirection.RX, FlowDataType.IQ, FlowDataMode.RAW);
                                        this.icdf.StartCapture(cmds[1], Tiger.Ruma.FlowDataType.IQ);
                                        Thread.Sleep(20);
                                        this.icdf.StopCapture(cmds[1], Tiger.Ruma.FlowDataType.IQ);
                                        if (cmds.Length == 2)
                                        {
                                            _tempfilename = DateTime.Now.ToString("HH_mm_ss") + ".cul";
                                        }
                                        else
                                        {
                                            _tempfilename = cmds[2] + ".cul";
                                        }
                                        this.icdf.ExportAllCapturedData(cmds[1], @"c:\RTT\rxevm\" + _tempfilename, "", Tiger.Ruma.ExportFormat.Cul, Tiger.Ruma.UmtsType.LTE);
                                        WriteTraceText("rxevm capture : " + _tempfilename + " on cpri " + cmds[1]);
                                        result = _tempfilename;
                                    }
                                    catch (Exception exp)
                                    {
                                        WriteTraceText(exp.Message + "RX_EVM_Capture failed!");
                                    }

                                }
                                else
                                {
                                    WriteTraceText("Please command,rxevmcaptureOnce#cpriport");
                                }
                            }
                            else if (sendcmd.Contains("rxevmcaptureStart"))
                            {
                                string[] cmds = sendcmd.Split('#');     //rxevmcapture#cpriport#interval
                                if (cmds.Length >= 3)
                                {
                                    try
                                    {
                                        if (int.Parse(cmds[2]) > 2000)
                                        {
                                            _cpriport = cmds[1];
                                            _evmtimer.Interval = int.Parse(cmds[2]);
                                            _evmtimer.Enabled = true;
                                            _evmtimer.Elapsed += new System.Timers.ElapsedEventHandler(_evmtimer_Elapsed);
                                            WriteTraceText("EvmCapture start! Interval is : " + cmds[2] + " ms ");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        WriteTraceText("please check interval value" + e.Message);
                                    }

                                }
                                else
                                {
                                    WriteTraceText("Please command,rxevmcaptureStart#cpriport#interval(ms)");
                                }

                            }
                            else if (sendcmd.Contains("rxevmcaptureStop"))
                            {
                                string[] cmds = sendcmd.Split('#');     //rxevmcapture#cpriport
                                if (cmds.Length >= 1)
                                {

                                _evmtimer.Stop();
                                WriteTraceText("EvmCapture stop !");
                            }
                        }
						else if (sendcmd.Contains("RULoader"))
						{
							string[] cmds = sendcmd.Split('#');
							if (sendcmd.Contains("UpgradeRU"))
                            {                                
                                if (cmds.Length >= 5)
                                {
                                    this.RumasterUpgradeRU(cmds[1],cmds[2],cmds[3],cmds[4]);
                                }
                            }
							else if(sendcmd.Contains("UpgradeRUStatus"))
                            { 
                            	result = this.RumasterUpgradeRUStatus();                            
                            }
							else if(sendcmd.Contains("RuHwInfo"))
                            {                                
                                if (cmds.Length >= 3)
                                {
                                    result=this.RumasterRuHwInfo(cmds[1],cmds[2]);
                                }
                            }
							else if(sendcmd.Contains("RuSwInfo"))
                            {                                
                                if (cmds.Length >= 3)
                                {
                                    result=this.RumasterRuSwInfo(cmds[1],cmds[2]);
                                }
                            }
							else if(sendcmd.Contains("AsynchronousUpgradeRU"))
                            {                                
                                if (cmds.Length >= 5)
                                {
                                    result=this.RumasterAsynchronousUpgradeRU(cmds[1],cmds[2],cmds[3],cmds[4]);
                                }
                            }
							else if(sendcmd.Contains("IsLinkRuUP"))
                            {                                
                                if (cmds.Length >= 2)
                                {
                                    result=this.RumasterIsLinkRuUP(cmds[1]);
                                }
                            }
							else if(sendcmd.Contains("IsLinkRuUP2"))
                            {                                
                                if (cmds.Length >= 3)
                                {
                                    result=this.RumasterIsLinkRuUP2(cmds[1], cmds[2]);
                                }
                            }
							else if(sendcmd.Contains("DeleteRuSector"))
                            {                                
                                if (cmds.Length >= 4)
                                {
                                    result=this.RumasterDeleteRuSector(cmds[1],cmds[2],cmds[3]);
                                }
                            }
							else if(sendcmd.Contains("RestartRU"))
                            {                                
                                if (cmds.Length >= 4)
                                {
                                    result=this.RumasterRestartRU(cmds[1],cmds[2],cmds[3]);
                                }
                            }

						}
                        else if (sendcmd.Contains("Trigger"))
                        {
							string[] cmds = sendcmd.Split('#');
							if (sendcmd.Contains("SetupClockSource"))
                            {                                
                                if (cmds.Length >= 3)
                                {
                                    this.RumasterSetupClockTriggerSource(cmds[1],cmds[2]);
                                }
                            }
							else if(sendcmd.Contains("SetupGammaSource"))
                            {                                
                                if (cmds.Length >= 2)
                                {
                                    this.RumasterSetupGammaTriggerSource(cmds[1]);
                                }
                            }
							else if(sendcmd.Contains("SetCpriSource"))
                            {                                
                                if (cmds.Length >= 4)
                                {
                                    this.RumasterSetCpriTriggerSource(cmds[1],cmds[2],cmds[3]);
                                }
                            }
							else if(sendcmd.Contains("SetupGsmFramesync"))
                            {                                
                                if (cmds.Length >= 9)
                                {
                                    this.RumasterSetupGsmFramesyncTrigger(cmds[1],cmds[2],cmds[3],cmds[4],cmds[5],cmds[6],cmds[7],cmds[8]);
                                }
                            }
							else if(sendcmd.Contains("SetupCtt"))
                            {                                
                                if (cmds.Length >= 6)
                                {
                                    this.RumasterSetupCttTrigger(cmds[1],cmds[2],cmds[3],cmds[4],cmds[5]);
                                }
                            }
							else if(sendcmd.Contains("SetupAdjustment"))
                            {                                
                                if (cmds.Length >= 5)
                                {
                                    this.RumasterSetupTriggerAdjustment(cmds[1],cmds[2],cmds[3],cmds[4]);
                                }
                            }
							else if(sendcmd.Contains("SetupStatic"))
                            {                                
                                if (cmds.Length >= 4)
                                {
                                    this.RumasterSetupStaticTrigger(cmds[1],cmds[2],cmds[3]);
                                }
                            }
							else if(sendcmd.Contains("GetStaticOutput"))
                            {                                
                                if (cmds.Length >= 2)
                                {
                                    result=this.RumasterGetTriggerStaticOutput(cmds[1]);
                                }
                            }
							else if(sendcmd.Contains("GetAdjustment"))
                            {                                
                                if (cmds.Length >= 2)
                                {
                                    result=this.RumasterGetTriggerAdjustment(cmds[1]);
                                }
                            }
                               
                        }

                    }
                    catch (Exception e)
                    {
                        this.tag_rumaster.BackColor = Color.Pink;
                        WriteErrorText("Cpriflowdata start/stop failed! " + e.Message);
                    }
                }
                else
                {
                    WriteTraceText("Please start rumaster first!");
                }

                }
                else if (cmd.Contains("Process.Delay"))
                {
                    WriteTraceText(cmd);
                    sendcmd = cmd.Replace("Process.Delay", "").TrimStart('(').TrimEnd(')');
                    try
                    {
                        Thread.Sleep(int.Parse(sendcmd));
                    }
                    catch
                    {
                        WriteTraceText("error on : " + cmd);
                    }
                }
                else if(cmd.Contains("Getprint.time"))
                {
                    WriteTraceText(cmd+" "+delay.ToString());
                    
                    printtag = false;
                    WriteDebugText("Getprint started." );
                    this.socketTag = true;
                    lock (socketbuilder)
                    {
                        //socketbuilder.Clear();
                        socketbuilder.Remove(0, socketbuilder.Length);
                    }
                    Thread.Sleep(delay);
                    WriteDebugText("Getprint sleep for delay " + delay.ToString());
                    this.socketTag = false;
                    string tempstr;
                    lock (socketbuilder)
                    {
                         tempstr = this.socketbuilder.ToString();
                    }
                    WriteDebugText("Getprint finished,length of result is "+ tempstr.Length);

                    result = tempstr;
                    
                    lock (socketbuilder)
                    {
                        //this.socketbuilder.Clear();
                        socketbuilder.Remove(0, socketbuilder.Length);
                    }
                    WriteDebugText("Getprint finished,return.");
                }
                else if (cmd.Contains("SERIAL2."))          //serial 2
                {
                    sendcmd = cmd.Replace("SERIAL2.", "").TrimStart(cl);
                    this.send_to_serial2(sendcmd);
                }
                else if (cmd.Contains("ts."))
                {
                    WriteTraceText(cmd);
                    printtag = false;

                    WriteTraceText(cmd);
                    //LogManager.WriteLog(LogFile.Trace, cmd);
                    string buttonname = cmd.Replace("ts.", "");

                    //get current tabs in mainform
                    foreach (NewTabPage ntp in this.tabControl1.TabPages)
                    {
                        foreach (Control item in ntp.Controls)
                        {
                            if (item is TableLayoutPanel)
                            {
                                TableLayoutPanel layoutPanel = item as TableLayoutPanel;
                                foreach (Control btn in layoutPanel.Controls)
                                {
                                    if (btn is TabButton)
                                    {

                                        TabButton button = btn as TabButton;
                                        if (button.Name == buttonname)
                                        {
                                            
                                            if (button._data != null)
                                            {
                                                foreach (string tempcmd in button._data)
                                                {
                                                    if (tempcmd != "")
                                                    {
                                                        if(socket)
                                                        {
                                                            command_Process(tempcmd);
                                                        }
                                                        else
                                                        {
                                                            cmdQueue.Enqueue(tempcmd);
                                                            _waitcmdQueueEventHandle.Set();
                                                        }
                                                        

                                                    }

                                                }
                                            }
                                            
                                            
                                            if (delay != 0)
                                                Thread.Sleep(delay);
                                            else
                                                Thread.Sleep(1000);
                                            //_tsthread.Join();
                                        }

                                    }
                                }
                            }

                        }


                    }

                }
                //send to rru
                else
                {
                    if (this.rruConnButton.CheckState == CheckState.Checked)
                    {


                        printtag = false;
                        WriteDebugText("into send to rru process...");

                        //delay do not equal to 0, need to return result,start timer and return result
                        if (delay != 0)
                        {
                            lock (socketbuilder)
                            {
                                //this.socketbuilder.Clear();
                                socketbuilder.Remove(0, socketbuilder.Length);
                            }
                            this.socketTag = true;
                            
                            
                            //数据完整性保护模式
                            if(dataProtect)
                            {
                                for (int i = 0; i != 4; i++)
                                {
                                    if (_rrusend == _rruresp)
                                    {
                                        if (_rrusend > 1000)
                                        {
                                            _rrusend = 0;
                                            _rruresp = 0;
                                        }
                                        SendToSerial(cmd, Com_rru,_COM_RRU);
                                        
                                        break;
                                    }
                                    Thread.Sleep(100);
                                    if (i == 3)
                                    {
                                        WriteDebugText("socket rru sync failed:" + cmd);
                                        WriteDebugText("socket rru re-sync with :" + cmd);

                                        SendToSerial(cmd, Com_rru, _COM_RRU);

                                        break;
                                    }

                                }
                                Thread.Sleep(delay);
                                for (int i = 0; i < 5; i++)
                                {
                                    string tempstr;
                                    lock (socketbuilder)
                                    {
                                         tempstr = this.socketbuilder.ToString();
                                    }
                                    
                                    if (tempstr.TrimEnd().EndsWith("$"))
                                    {
                                        result = tempstr;
                                        this.socketTag = false;
                                        lock (socketbuilder)
                                        {
                                            //this.socketbuilder.Clear();
                                            socketbuilder.Remove(0, socketbuilder.Length);
                                        }
                                        
                                        WriteDebugText("resp result is: " + result);
                                        break;
                                    }
                                    else
                                    {
                                        if (i < 4)
                                        {
                                            WriteDebugText("thread sleep: " + i.ToString());
                                            Thread.Sleep(600 * i);
                                            continue;
                                        }
                                        else
                                        {

                                            WriteDebugText("socket result from rru:: can't find $ in result.");
                                            lock (socketbuilder)
                                            {
                                                result = this.socketbuilder.ToString();
                                            }
                                            
                                            WriteDebugText("socket result from rru: " + result);
                                            if (result.Length == 0)
                                                result = socketNoresult;
                                            this.socketTag = false;
                                            lock (socketbuilder)
                                            {
                                                //this.socketbuilder.Clear();
                                                socketbuilder.Remove(0, socketbuilder.Length);
                                            }
                                            break;
                                        }

                                    }

                                }
                            }
                            //速度模式
                            else
                            {
                                SendToSerial(cmd, Com_rru, _COM_RRU);
                                WriteDebugText("Socket fast send to rru: " + cmd);
                                Thread.Sleep(delay);
                                WriteDebugText("Socket fast send to rru and sleep delay: " + delay.ToString());
                                lock (socketbuilder)
                                {
                                    result = this.socketbuilder.ToString();
                                }
                                
                                WriteDebugText("Socket result from rru: " + result);
                                if (result.Length == 0)
                                    result = socketNoresult;
                                this.socketTag = false;
                                lock (socketbuilder)
                                {
                                    // this.socketbuilder.Clear();
                                    socketbuilder.Remove(0, socketbuilder.Length);
                                }
                            }
                            

                        }
                        else
                        {
                            //数据完整性保护模式
                            if (dataProtect)
                            {
                                for (int i = 0; i != 4; i++)
                                {

                                    if (_rrusend == _rruresp)
                                    {
                                        if (_rrusend > 1000)
                                        {
                                            _rrusend = 0;
                                            _rruresp = 0;
                                        }
                                        SendToSerial(cmd, Com_rru, _COM_RRU);

                                        break;
                                    }
                                    Thread.Sleep(100);
                                    if (i == 3)
                                    {
                                        WriteDebugText("rru sync failed:" + cmd);
                                        WriteDebugText("rru re-sync with :" + cmd);

                                        SendToSerial(cmd, Com_rru, _COM_RRU);

                                        break;
                                    }

                                }
                            }
                            //速度模式
                            else
                            {
                                SendToSerial(cmd, Com_rru, _COM_RRU);
                            }

                            result = socketNoresult;
                        }
                    }
                    else
                    {
                        printtag = false;
                        WriteTraceText("please connect rru first.");
                        result = socketNorru;
                    }

                }
                //print toscreen and log
                //string logmsg = result.Trim();
                if (printtag)
                {


                    if (result != string.Empty)
                    {
                        WriteTraceText(result);

                    }
                    else
                    {
                        result = socketNoresult;

                    }
                }
            }
            WriteDebugText("result is: " + result);
            return result;
        }
        //rumaster start
        private void RumasterStartPlayBack(string cpriport,Tiger.Ruma.FlowDataType[] flows)
        {
            try
            {
                
                icdf.StartPlayBack(cpriport, flows);
            }
            catch
            {
                WriteTraceText("Rumaster startplayback failed!");
                
            }
            
        }
        //rumaster stop
        private void RumasterStopPlayBack(string cpriport,Tiger.Ruma.FlowDataType[] flows)
        {
            
            try
            {
                
                icdf.StopPlayBack(cpriport, flows);
            }
            catch
            {
                WriteTraceText("Rumaster stopplayback failed!");
                
            }
        }
        //rumaster switch CpcFileSetLoopLengthFromIQFile
        private void RumasterCpcFileSetLoopLengthFromIQFile(string cpriport, string filepath,string iqfile)
        {
            WriteTraceText("RumasterCpcFileSetLoopLengthFromIQFile : cpc-" + filepath+",IQ-"+ iqfile);

            try
            {

                icdf.CpcFilesClearAll(cpriport);
                icdf.CpcFileAdd(cpriport, filepath);
                icdf.CpcSetAxcMode(cpriport, Tiger.Ruma.TxAxcMode.CPC_FILES);
                icdf.CpcFileSetLoopLengthFromIQFile(cpriport, filepath, iqfile);
                icdf.CpcFileSetLoopLength(cpriport, filepath, 2);
                icdf.CpcFileSetCurrent(cpriport, filepath);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster set CPC file failed!--" + e.Message);

            }

        }
        //rumaster switch IQ file
        private void RumasterSetIQfile(string cpriport,string filepath)
        {
            WriteTraceText("RumasterSetIQfile : " + filepath);
            
            try
            {
                
                icdf.IQFileClearAll();
                icdf.IQFileAdd(cpriport, filepath);
                icdf.IQFileSetCurrentByName(cpriport, filepath);
            }
            catch(Exception e)
            {
                WriteTraceText("Rumaster set IQ file failed!--"+e.Message);
                
            }
            
        }
        //rumaster switch CPC file
        private void RumasterSetCPCfile(string cpriport, string filepath)
        {
            WriteTraceText("RumasterSetCPCfile : " + filepath);
            
            try
            {
                
                icdf.CpcFilesClearAll(cpriport);
                icdf.CpcFileAdd(cpriport, filepath);
                icdf.CpcSetAxcMode(cpriport, Tiger.Ruma.TxAxcMode.CPC_FILES);
                icdf.CpcFileSetLoopLength(cpriport, filepath, 2);
                icdf.CpcFileSetCurrent(cpriport, filepath);
            }
            catch(Exception e)
            {
                WriteTraceText("Rumaster set CPC file failed!--" + e.Message);
            }
        }
        private void RumasterSetFlowDataMode(string cpriport, string flowdirection, string flowdatatype,string flowdatamode)
        {
            
            try
            {
                icdf.SetFlowDataMode(cpriport, flowDirectionDic[flowdirection], flowDataTypeDic[flowdatatype],flowDataModeDic[flowdatamode]);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Set FlowDataMode!--" + e.Message);
            }
        }
        private string RumasterGetFlowDataMode(string cpriport, string flowdirection, string flowdatatype)
        {
            string data="";
            try
            {
                data=icdf.GetFlowDataMode(cpriport, flowDirectionDic[flowdirection], flowDataTypeDic[flowdatatype]).ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Get FlowDataMode!--" + e.Message);
            }
            return data;
        }

        private string RumasterGetStartStopCondition(string cpriport, string flowdirection, string flowdatatype)
        {
            string data = "";
            try
            {
                FlowStartCondition startCondition;
                ushort[] startConditionPara;
                FlowStopCondition stopCondition;
                ushort[] stopConditionPara;
                icdf.GetStartStopCondition(cpriport, flowDirectionDic[flowdirection], flowDataTypeDic[flowdatatype],out startCondition, out startConditionPara,out  stopCondition,out stopConditionPara);
                string startConditionParameters = "";
                int startparalen = startConditionPara.Length;
                if (startparalen > 0)
                {
                    for (int i = startparalen - 1; i >0; i--)
                    {                        
                        startConditionParameters = startConditionParameters + startConditionPara[i] + SPACER_FLAG_SEC;
                    }
                    startConditionParameters = startConditionParameters + startConditionPara[0];
                }

                string stopConditionParameters = "";
                int stopparalen = stopConditionPara.Length;
                if (stopparalen > 0)
                {
                    if (stopCondition.ToString() == "CPRI_TIME_LENGTH")
                    {
                        stopparalen--;
                    }
                    for (int i = stopparalen - 1; i>0; i--)
                    {
                        stopConditionParameters = stopConditionParameters + stopConditionPara[i] + SPACER_FLAG_SEC;
                    }
                    stopConditionParameters = stopConditionParameters + stopConditionPara[0];
                }


                data = "startCondition=" + startCondition.ToString() + SPACER_FLAG_FIR + "startConditionParameters=" + startConditionParameters + SPACER_FLAG_FIR + "stopCondition=" + stopCondition.ToString() + SPACER_FLAG_FIR + "stopConditionParameters=" + stopConditionParameters;

            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Get StartStop Condition!--" + e.Message);
            }

            return data;
        }
        private void RumasterSetStartStopCondition(string cpriport, string flowdirection ,string flowdatatype ,string startCondition,string startConditionPara , string stopCondition, string stopConditionPara)
        {
            try
            {                
                ushort[] startpara;
                string[] startparastr = startConditionPara.Split(SPACER_FLAG_SEC);
                ushort startparalen = 4;
                if (startCondition == "CPRI_TIME")
                {
                    startparalen = 4;                     
                }
                else if (startCondition == "FRAME_PRE_START")
                {
                    startparalen = 2;                   
                }
                startpara = new ushort[startparalen];
                if (startparastr.Length == startparalen)
                {
                    for (int i = 0; i < startparalen; i++)
                    {
                        startpara[i] = ushort.Parse(startparastr[startparalen - 1 - i]);
                    }
                }
                else
                {
                    if ((startCondition == "CPRI_TIME") || (startCondition == "FRAME_PRE_START"))
                    {
                        WriteErrorText("startConditionPara Error");
                    }
                }

                ushort[] stoppara;
                string[] stopparastr = stopConditionPara.Split(SPACER_FLAG_SEC);
                ushort stopparalen = 3;
                stoppara = new ushort[stopparalen];
                if (stopCondition == "CPRI_TIME_LENGTH")
                { 
                    if ( stopparastr.Length == stopparalen)
                    {
                        for (int i = 0; i < stopparalen; i++)
                        {
                            stoppara[i] = ushort.Parse(stopparastr[stopparalen-1-i]);
                        }
                    }
                    else
                    {
                        WriteErrorText("stopConditionPara Error");
                    }
                }

                icdf.SetStartStopCondition(cpriport, flowDirectionDic[flowdirection],flowDataTypeDic[flowdatatype],flowStartConditionDic[startCondition], startpara,flowStopConditionDic[stopCondition], stoppara);
            }
            catch (Exception e)  
            {
                WriteTraceText("Rumaster Set StartStop Condition!--" + e.Message);
            }
        }
        private void RumasterDeleteAllCarriers(string cpriport, string flowdirection)
        {
            try
            {
                rCarrierConfig.DeleteAllCarriers(cpriport, flowDirectionDic[flowdirection]);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Delete All Carriers!--" + e.Message);
            }
        }
        private void RumasterDeleteCarrier(string cpriport, string flowdirection, string carrierindex)
        {
            try
            {
                rCarrierConfig.DeleteCarrier(cpriport, flowDirectionDic[flowdirection], byte.Parse(carrierindex));
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Delete Carriers!--" + e.Message);
            }
        }
        

        private void RumasterAddCarrier(string cpriport, string flowdirection,string carrierid, string axccontainer, string frequency, string technology)
        {           
            try
            {                
                byte carrierIndex= byte.Parse("0");
                string consistencyWarning="none";    
                rCarrierConfig.AddCarrier(cpriport, flowDirectionDic[flowdirection], byte.Parse(carrierid), uint.Parse(axccontainer), freqDic[frequency], technologyDic[technology], out carrierIndex,out  consistencyWarning);
                          
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Add Carrier failed!--" + e.Message);
            }
        }
        private string showCarrierData(CarrierData data)
        {
            //#cpriPort#flowDirection#axcContainerGroupLength#bfPeriod#carrierId#carrierNumber#enabled#fsInfo#gain#frequency##axcContainer#syncMode#technology 
            string strdata = "";
            
            string carrierNumber = data.carrierNumber.ToString();           
            strdata = strdata + "Number=" + carrierNumber + SPACER_FLAG_SEC;
            if (data.enabled == true)
            {                
                strdata = strdata + "Enabled=" + "true" + SPACER_FLAG_SEC;
            }
            else if (data.enabled == false)
            {               
                strdata = strdata + "Enabled=" + "false" + SPACER_FLAG_SEC;
            }           
            string carrierId = data.carrierId.ToString();           
            strdata = strdata + "Id=" + carrierId + SPACER_FLAG_SEC;
            string tech = data.technology.ToString();            
            strdata = strdata + "Technology=" + tech + SPACER_FLAG_SEC;
            string Frequency = data.sampleFrequency.ToString();
            strdata = strdata + "SampleFrequency=" + Frequency + SPACER_FLAG_SEC;

            string startAxcContainer = data.startAxcContainer.ToString();
            strdata = strdata + "AxcContainer=" + startAxcContainer + SPACER_FLAG_SEC;

            Gain gaindata = data.gain;

            if (gaindata.GainEnable == true)
            {
                strdata = strdata + "GainEnable=" + "true" + SPACER_FLAG_SEC;
            }
            else if (gaindata.GainEnable == false)
            {
                strdata = strdata + "GainEnable=" + "false" + SPACER_FLAG_SEC;
            }
           
            string gaindb = gaindata.GainDb.ToString();
            strdata = strdata + "GainDb=" + gaindb + SPACER_FLAG_SEC;
            string gainfact = gaindata.GainFact.ToString();
            strdata = strdata + "GainFact=" + gainfact + SPACER_FLAG_SEC;


            FsInfo fsinfodata = data.fsInfo;
            string addr = fsinfodata.Addr.ToString();
            strdata = strdata + "FsInfoAddress=" + addr + SPACER_FLAG_SEC;
            string hf = fsinfodata.Hf.ToString();
            strdata = strdata + "FsInfoHF=" + hf + SPACER_FLAG_SEC;
            string bf = fsinfodata.Bf.ToString();
            strdata = strdata + "FsInfoBF=" + bf + SPACER_FLAG_SEC;
            string bfoffset = fsinfodata.BfOffset.ToString();
            strdata = strdata + "FsInfoBfOffset=" + bfoffset + SPACER_FLAG_SEC;


            string axcContainerGroupLength = data.axcContainerGroupLength.ToString();
            strdata = strdata + "axcContainerGroupLength=" + axcContainerGroupLength + SPACER_FLAG_SEC;
            string bfPeriod = data.bfPeriod.ToString();
            strdata = strdata + "bfPeriod=" + bfPeriod + SPACER_FLAG_SEC;

            string syncmode = data.syncMode.ToString();
            strdata = strdata + "syncMode=" + syncmode;

            //WriteTraceText(strdata);
            return strdata;
            
        }
        private string  RumasterGetCarrierConfig(string cpriport, string flowdirection, string carrierindex)
        {
            string getdata = "";

            try
            {
                CarrierData data = rCarrierConfig.GetCarrierConfig(cpriport, flowDirectionDic[flowdirection], byte.Parse(carrierindex));                
                getdata=showCarrierData(data);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Get Carrier Config failed!--" + e.Message);
            }
            return getdata;
        }
        private void RumasterSetCarrierConfig(string cpriport, string flowdirection, string carriernumber, string enabled, string carrierid, string technology, string frequency, string axccontainer, string gain, string fsinfo,string axcContainerGroupLength,string bfPeriod,string syncmode)
        {

           
            try
            {     
                
                          
                FsInfo fsinfoData = new FsInfo();
                string[] fsinfostr = fsinfo.Split(SPACER_FLAG_SEC);
                if (fsinfostr.Length >= 4)
                {
                    fsinfoData.Addr = byte.Parse(fsinfostr[0]);
                    fsinfoData.Hf = byte.Parse(fsinfostr[1]);
                    fsinfoData.Bf = byte.Parse(fsinfostr[2]);                   
                    fsinfoData.BfOffset = byte.Parse(fsinfostr[3]);                    
                }
                

                Gain  gainData = new Gain();
                string[] gainstr = gain.Split(SPACER_FLAG_SEC);
                if (fsinfostr.Length >= 3)
                {
                    gainData.GainEnable = boolDic[gainstr[0]];
                    gainData.GainDb = double.Parse(gainstr[1]);
                    gainData.GainFact = double.Parse(gainstr[2]);                    
                }

                CarrierData data = new CarrierData();
                data.axcContainerGroupLength = byte.Parse(axcContainerGroupLength);
                data.bfPeriod = byte.Parse(bfPeriod);
                data.carrierId = byte.Parse(carrierid);
                data.carrierNumber = byte.Parse(carriernumber);
                data.enabled = boolDic[enabled];
                data.sampleFrequency = freqDic[frequency];
                data.technology = technologyDic[technology];
                data.startAxcContainer = byte.Parse(axccontainer);
                data.syncMode = syncModeDic[syncmode];
                data.fsInfo = fsinfoData;
                data.gain = gainData;

                
                rCarrierConfig.SetCarrierConfig(cpriport, flowDirectionDic[flowdirection], data);
                showCarrierData(data);


            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Set Carrier Config failed!--" + e.Message);
            }
        }


        private void RumasterSetCpriConfig(string paraname,string cpriport,string paravalue)
        {           
            try
            {
                switch(paraname)
                {
                    case "LineRate":
                        rCpriConfig.SetLineRate(cpriport, lineRateDic[paravalue]);
                        break;
                    case "CPRIVer":
                        rCpriConfig.SetCpriVersion(cpriport, cpriVerDic[paravalue]);
                        break;
                    case "ScrambSeed":
                        rCpriConfig.SetScramblingSeed(cpriport,uint.Parse(paravalue));                    
                        break;
                    default:
                        WriteErrorText("Undefined Rumaster Set CPRI Config parament:  " + paraname);
                        break;
                }
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Set CPRI Config: "+paraname +" failed!--" + e.Message);
            }           
        }
        private string RumasterGetCpriConfig(string paraname, string cpriport)
        {
            string data = "";
            try
            {
                
                switch (paraname)
                {
                    
                    case "LineRate":
                        data = rCpriConfig.GetLineRate(cpriport).ToString();
                        break;
                    case "CPRIVer":
                        data = rCpriConfig.GetCpriVersion(cpriport).ToString();
                        break;
                    case "ScrambSeed":
                        data = rCpriConfig.GetScramblingSeed(cpriport).ToString();
                        break;
                    default:
                        WriteErrorText("Undefined Rumaster Set CPRI Config parament:  " + paraname);
                        break;
                }

            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Set CPRI Config: " + paraname + " failed!--" + e.Message);
            }
            return data;
        }
        private string  RumasterGetPllRef(string hwSn, string reflock)
        {
            string data = "";
            try
            {
                bool locked = boolDic[reflock];
                data = rServerBase.GetPllRef(hwSn, ref locked).ToString();               
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Get PllRef  failed!--" + e.Message);
            }
            return data;
        }
        private void RumasterSetPllRef(string hwSn, string ltustr)
        {           
            try
            {               
                rServerBase.SetPllRef(hwSn, ltuDic[ltustr]);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Set PllRef  failed!--" + e.Message);
            }           
        }
        private void RumasterSetupGammaTriggerSource(string triggerPort)
        {
            try
            {
                rTriggerConfig.SetupGammaTriggerSource(triggerPort);
            }
            catch (Exception e)
            {
                WriteTraceText("Setup Gamma Trigger Source Failed!--" + e.Message);
            }
        }
        private void RumasterSetupClockTriggerSource(string triggerPort, string instanceName)
        {            
            try
            {
                rTriggerConfig.SetupClockTriggerSource(triggerPort, clockInstanceNameDic[instanceName]);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Setup Clock Trigger Source  Failed!--" + e.Message);
            }
        }
        private void RumasterSetCpriTriggerSource(string triggerPort, string cpriport, string cpriTriggerSource)
        {
            try
            {
                rTriggerConfig.SetCpriTriggerSource(triggerPort, cpriport, cpriTrigSourceDic[cpriTriggerSource]);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Set Cpri Trigger Source Failed!--" + e.Message);
            }
        }
        private void RumasterSetupGsmFramesyncTrigger(string triggerPort, string cpriport, string hyperframe, string basicFrameOffset,string word,string basicframe,string pulseLengthIn13MHzCycles, string pulseOffset)
        {
            try
            {
                rTriggerConfig.SetupGsmFramesyncTrigger(triggerPort, cpriport, uint.Parse(hyperframe), uint.Parse(basicFrameOffset),uint.Parse(word), uint.Parse(basicframe), uint.Parse(pulseLengthIn13MHzCycles), uint.Parse(pulseOffset));
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Set Cpri Trigger Source Failed!--" + e.Message);
            }
        }
        private void RumasterSetupTriggerAdjustment(string triggerPort,string pulseOffset,string pulseWidth,string finePhaseAdjust)
        {
            try
            {
                rTriggerConfig.SetupTriggerAdjustment(triggerPort,int.Parse(pulseOffset),uint.Parse(pulseWidth),int.Parse(finePhaseAdjust));
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Setup Trigger Adjustment Failed!--" + e.Message);
            }
        }
        private void RumasterSetupStaticTrigger(string triggerPort, string level, string enabled)
        {
            try
            {
                rTriggerConfig.SetupStaticTrigger(triggerPort, triggerStaticOutputLevelDic[level], boolDic[enabled]);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Setup Static Trigger Failed!--" + e.Message);
            }
        }

        private string RumasterGetTriggerStaticOutput(string triggerPort)
        {
            TriggerStaticOutputLevel level = TriggerStaticOutputLevel.low;
            bool enabled = false;
            string data = "";

            try
            {
                rTriggerConfig.GetStaticOutput(triggerPort, out level, out enabled);
                data = "leve=" + level.ToString() + SPACER_FLAG_SEC + "enabled=" + enabled.ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Setup Static Trigger Failed!--" + e.Message);
            }
           
            return data;
        }
        private string RumasterGetTriggerAdjustment(string triggerPort)
        {            
            int pulseOffset = 0;
            uint pulseWidth = 0;
            int finePhaseAdjust = 0;
            string data = "";
            try
            {
                rTriggerConfig.GetTriggerAdjustment(triggerPort, out pulseOffset, out pulseWidth, out finePhaseAdjust);
                data = "pulseOffset=" + pulseOffset.ToString() + SPACER_FLAG_SEC + "pulseWidth=" + pulseWidth.ToString() + SPACER_FLAG_SEC + "finePhaseAdjust=" + finePhaseAdjust.ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Get Trigger Adjustment Failed!--" + e.Message);
            }

            return data;
        }
        private void RumasterSetupCttTrigger(string triggerPort, string cpriPort, string filename,string useCttEnableTriggerSettings, string cttEnableTriggerArray)
        {
            
            string[] triggerArraystr = cttEnableTriggerArray.Split(SPACER_FLAG_SEC);
            int strlen = triggerArraystr.Length;
            bool[] triggerArray = new bool[strlen];
            for (int i = 0; i < strlen; i++)
            {
                triggerArray[i] = boolDic[triggerArraystr[i]];
            }
            try
            {
                rTriggerConfig.SetupCttTrigger(triggerPort, cpriPort, filename,boolDic[useCttEnableTriggerSettings], triggerArray);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Setup Ctt Trigger Failed!--" + e.Message);
            }
        }
		private void RumasterUpgradeRU(string filename, string port, string physPos, string restart)
        {
            try
            {
                rIRULoader.UpgradeRU(filename,ulong.Parse(port),ulong.Parse(physPos),boolDic[restart]);
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Upgrade RU Failed!--" + e.Message);
            }
        }
		
		private string RumasterUpgradeRUStatus()
        {            
            int statePercent = 0;            
            string data = "";
            try
            {
                rIRULoader.UpgradeRUStatus(out statePercent);
                data = "statePercent=" + statePercent.ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Get Upgrade RU Status Failed!--" + e.Message);
            }

            return data;
        }
        private string RumasterRuHwInfo(string port, string physPos)
        {
            string data = "";
            try
            {
                data = rIRULoader.RuHwInfo(ulong.Parse(port), ulong.Parse(physPos));
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Ru HwInfo Failed!--" + e.Message);
            }
            return data;
        }
        private string RumasterAsynchronousUpgradeRU(string filename,string port, string physPos,string restart)
        {
            string data = "";
            try
            {
                data = rIRULoader.AsynchronousUpgradeRU(filename, ulong.Parse(port), ulong.Parse(physPos),boolDic[restart]).ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Asynchronous Upgrade RU Failed!--" + e.Message);
            }
            return data;
        }
        private string RumasterIsLinkRuUP(string port)
        {
            string data = "";
            try
            {
                data = rIRULoader.IsLinkRuUP(ulong.Parse(port)).ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Is Link Ru UP Failed!--" + e.Message);
            }
            return data;
        }

        private string RumasterIsLinkRuUP2(string port,string physicalPosition)
        {
            string data = "";
            try
            {
                data = rIRULoader.IsLinkRuUP2(ulong.Parse(port), ulong.Parse(physicalPosition)).ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Is Link Ru UP2 Failed!--" + e.Message);
            }
            return data;
        }

        private string RumasterDeleteRuSector(string radioPid, string port, string physPos)
        {
            string data = "";
            try
            {
                data = rIRULoader.DeleteRuSector(radioPid, ulong.Parse(port), ulong.Parse(physPos)).ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Delete Ru Sector Failed!--" + e.Message);
            }
            return data;
        }

        private string RumasterRestartRU(string radioPid,string port, string physPos)
        {
            string data = "";
            try
            {
                data = rIRULoader.RestartRU(radioPid,ulong.Parse(port), ulong.Parse(physPos)).ToString();
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Restart RU Failed!--" + e.Message);
            }
            return data;
        }

        private string RumasterRuSwInfo(string port,string physPos)
        {
            string data = "";
            try
            {
                data=rIRULoader.RuSwInfo(long.Parse(port),ulong.Parse(physPos));                
            }
            catch (Exception e)
            {
                WriteTraceText("Rumaster Ru SwInfo Failed!--" + e.Message);
            }
            return data;
        }

        //timer tick
        //private void socketprocesstimer_Tick(object sender,EventArgs e)
        private void socketprocesstimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //time up to return
            
            this.socketprocesstimer.Stop();
            this.socketTag = false;
            
        }

        //to serial 2
        private void send_to_serial2(string cmd = "")
        {
            string cmdneedsend = cmd;

            cmd += this._com2trans;


            Thread.Sleep(200);
            //定义一个变量，记录发送了几个字节  
            int n = 0;
            //16进制发送  
            if (checkBoxHexSend.Checked)
            {
                if (this._COM2 != null)
                {
                    if (this._COM2.IsOpen)
                    {
                        //我们不管规则了。如果写错了一些，我们允许的，只用正则得到有效的十六进制数 
                        //cmdneedsend += "\r";
                        MatchCollection mc = Regex.Matches(cmd, @"(?i)[/da-f]{2}");
                        List<byte> buf = new List<byte>();//填充到这个临时列表中  
                                                          //依次添加到列表中  
                        foreach (Match m in mc)
                        {
                            buf.Add(byte.Parse(m.Value));
                        }
                        //转换列表为数组后发送  
                        lock (_lock_COM2)
                        {
                            this._COM2.Write(buf.ToArray(), 0, buf.Count);
                        }
                        //记录发送的字节数  
                        n = buf.Count;
                    }
                }
                else
                {
                    WriteTraceText("Please connect serial 2 first.");
                    
                }

            }
            else//unicode编码直接发送  
            {
                
                if (this._COM2 != null)
                {
                    if (this._COM2.IsOpen)
                    {

            
                        
                        lock (_lock_COM2)
                        {
                            
                            this._COM2.Write(cmd);
                            WriteDebugText("write to com_2: " + cmdneedsend);
                            
                        }

            

                    }

                }
                    
                else
                {
                    WriteTraceText("Please connect serial 2 first.");
                    
                }

    

    }
    
}
        //send to serialport
        private void SendToSerialPort(SerialPort sp,string cmd = "")
        {
            Thread.Sleep(200);
            WriteDebugText("Prepare to send cmd to "+ sp.PortName);
            string cmdneedsend;
            if (cmd != "")
                cmdneedsend = cmd;
            else
                cmdneedsend = InputBox.Text;

            cmdneedsend = cmdneedsend.Replace("\r", "").Replace("\n", "").TrimStart('\r').TrimStart('\n');



            //定义一个变量，记录发送了几个字节  
            int n = 0;
            //16进制发送  
            if (checkBoxHexSend.Checked)
            {
                //我们不管规则了。如果写错了一些，我们允许的，只用正则得到有效的十六进制数 

                MatchCollection mc = Regex.Matches(cmdneedsend, @"(?i)[/da-f]{2}");
                List<byte> buf = new List<byte>();//填充到这个临时列表中  
                //依次添加到列表中  
                foreach (Match m in mc)
                {
                    buf.Add(byte.Parse(m.Value));
                }
                //转换列表为数组后发送
                //lock(_lock_RRUCOM)
                //{
                sp.Write(buf.ToArray(), 0, buf.Count);
                //}

                //记录发送的字节数  
                n = buf.Count;
            }
            else//unicode编码直接发送  
            {

                
                if (sp.IsOpen)
                {
                    //lock (_lock_RRUCOM)
                    //{
                    try
                    {
                        lock (sp)
                        {
                            sp.Write(cmdneedsend + '\r');
                        }

                        if (dataProtect && sp==_COM_RRU)
                            _rrusend++;
                        WriteDebugText("Write to " + sp.PortName + " : " + cmdneedsend);
                        //waitingForreceive = true;
                    }
                    catch (Exception e)
                    {

                        WriteTraceText(cmd);
                        WriteErrorText("Write to "+sp.PortName+" failed:" + e.Message + e.StackTrace);
                    }
                }
                else
                {
                    WriteTraceText(cmd);
                    WriteTraceText("Please connect "+sp.PortName+" first.");
                 }

            }

        }
        //to rru 
        private void send_to_rru(string cmd="",int delay = 0)
        {
            Thread.Sleep(200);
            WriteDebugText("prepare to send cmd...");
            string cmdneedsend;
            if (cmd != "")
                cmdneedsend = cmd;
            else
                cmdneedsend = InputBox.Text;

            cmdneedsend = cmdneedsend.Replace("\r","").Replace("\n","").TrimStart('\r').TrimStart('\n');
            
            
            
            //定义一个变量，记录发送了几个字节  
            int n = 0;
            //16进制发送  
            if (checkBoxHexSend.Checked)
            {
                //我们不管规则了。如果写错了一些，我们允许的，只用正则得到有效的十六进制数 

                MatchCollection mc = Regex.Matches(cmdneedsend, @"(?i)[/da-f]{2}");
                List<byte> buf = new List<byte>();//填充到这个临时列表中  
                //依次添加到列表中  
                foreach (Match m in mc)
                {
                    buf.Add(byte.Parse(m.Value));
                }
                //转换列表为数组后发送
                //lock(_lock_RRUCOM)
                //{
                    this._COM_RRU.Write(buf.ToArray(), 0, buf.Count);
                //}
                
                //记录发送的字节数  
                n = buf.Count;
            }
            else//unicode编码直接发送  
            {
                
                if (this._COM_RRU != null)
                {
                    if(this._COM_RRU.IsOpen)
                    {
                        
                        
                        //lock (_lock_RRUCOM)
                        //{
                            try
                            {
                                lock(this._COM_RRU)
                                {
                                    this._COM_RRU.Write(cmdneedsend + '\r');
                                }
                                
                                if(dataProtect)
                                    _rrusend++;
                                WriteDebugText("write to com_rru: " + cmdneedsend);
                                //waitingForreceive = true;
                            }
                            catch(Exception e)
                            {
                                
                                WriteTraceText(cmd);
                                WriteErrorText("write to com_rru failed:" + e.Message + e.StackTrace);
                            }
                            
                            

                                
                            

                        //}
                        
                        


                    }
                    
                }
                    
                else
                {
                    WriteTraceText(cmd);
                    WriteTraceText("Please connect rru first.");
                    
                }
                    
                

            }
            
        }

        //send cmd to instrument by visa32
        private string send_to_instrument(string fullcmd,string address, string cmd,  ref int session,ref int vi,int delay = 0)
        {
            WriteTraceText(fullcmd);
            cmd += '\n';
            int status = -1;
            string strRd = "";
            string instrumentAddress = address;
            //byte[] buf;
            //string cmd = "*IDN?\n";
            //buf = Encoding.ASCII.GetBytes(cmd);
           // int retcount;


            //lock
            //this.WriteTraceText("\n");
            //lock (_lock_Instrument)
            //{
                for (int i = 0; i < 5; i++)
                {
                
                    status = visa32.viPrintf(vi, cmd);
                    //status = visa32.viWrite(vi, buf, buf.Length, out retcount);
                    if (status < visa32.VI_SUCCESS)
                    {
                        visa32.viClear(vi);
                        if (i != 4)
                        {
                            Thread.Sleep(200);
                            continue;

                        }
                        else
                        {
                            WriteErrorText("write to instrument byvisa32 error : " + status.ToString());
                            throw new Exception(status.ToString());
                        }
                        

                    }
                    else
                        break;


                }
                
                
                if (delay != 0&&delay>500)
                    Thread.Sleep(delay);
                else
                    Thread.Sleep(500);
                if (cmd.Contains("?"))
                {

                    for (int i = 0; i < 6; i++)
                    {
                        //visa32.viClear(vi);
                        status = visa32.viRead(vi, out strRd, 4069);
                        
                        if (status < visa32.VI_SUCCESS)
                        {
                            visa32.viClear(vi);
                            if (i != 4)
                            {
                                Thread.Sleep(i * 400);
                                continue;

                            }
                            else
                            {
                                WriteErrorText("read from instrument byvisa32 error : " + status.ToString());
                                if (vi == this.viSA)
                                    this.CheckSessionbyVisa32(this.addr.SA, this.tag_sa, ref this.sesnSA, ref this.viSA);
                                else
                                    throw new Exception(status.ToString());
                                status = visa32.viRead(vi, out strRd, 4069);
                                if (status < visa32.VI_SUCCESS)
                                {
                                    throw new Exception(status.ToString());
                                }
                                else
                                    break;
                            }
                            
                        }
                        else
                            break;
                        
                        
                    }
                    

                    

                }
            //}

            /*try
            {
                visa32.viGetDefaultRM(out _session);
                status = visa32.viOpen(_session, instrumentAddress, 0, 1000, out _vi);
                if (status == 0)
                {

                    visa32.viClear(vi);
                    status = visa32.viWrite(_vi, buf, buf.Length, out retcount);
                    if (status != 0)
                    {
                        //SetText(status.ToString());

                        throw new Exception(status.ToString());

                    }
                }
                else
                {
                    //SetText(status.ToString());

                    throw new Exception(status.ToString());
                }
            }
            catch
            {
                throw new Exception(status.ToString());
            }
            if (cmd.Contains("?"))
            {
                if (delay != 0)
                    Thread.Sleep(delay);

                byte[] readbuf = new byte[2048];

                status = visa32.viRead(_vi, readbuf, readbuf.Length, out retcount);

                if (retcount != 0)
                {
                    strRd = Encoding.ASCII.GetString(readbuf, 0, retcount - 1);
                    visa32.viClear(_vi);
                }
                else
                {
                    visa32.viClear(_vi);
                    SetText("read instrument error : " + status.ToString());

                    throw new Exception(status.ToString());
                }



            }*/
            ///////////

            strRd = strRd.TrimEnd().TrimEnd('\n');
            return strRd;
        }

        //send to instrument by visacom
        private string send_to_instrument(string fullcmd,string address,string cmd, IMessage ioDmm,int delay = 0)
        {


            WriteTraceText(fullcmd);
            //IVisaSession basesession = null;
            //IMessage talksession = null;
            
            string strRd = "";
            string instrumentAddress = address;
            //lock
            //this.WriteTraceText("\n");
            //lock (_lock_Instrument)
            //{



                    ioDmm.Clear();
                    lock (ioDmm)
                    {
                        ioDmm.WriteString(cmd);
                    }
                    
                    

                    if (delay != 0 && delay > 500)
                        Thread.Sleep(delay);
                    else
                        Thread.Sleep(500);
                    if (cmd.Contains("?"))
                    {
                        for (int i = 0; i < 5; i++)
                        {

                            try
                            {

                                lock (ioDmm)
                                {
                                    strRd = ioDmm.ReadString(4069);
                                }
                        
                                break;
                            }
                            catch (Exception exp)
                            {
                                ioDmm.Clear();
                                if (i != 9)
                                {
                                    Thread.Sleep(200);
                                    continue;

                                }
                                else 
                                {
                                    WriteErrorText("write/read to instrumen by visacom error: " + exp.Message);
                                    throw new Exception(exp.Message);
                                }
                                
                            }
                            


                        }
                        
                        


                    }


                
                
            //}
            strRd = strRd.TrimEnd('\n');
            return strRd;
        }

        private void ListenUDPClientConnect()
        {
            int recv;
            byte[] data ;
            string recvStr = "";
            string response = "";
            byte[] resbyte;

            
            newsock.Bind(ipep);//Socket与本地的一个终结点相关联
            //Console.WriteLine("Waiting for a client.....");
            
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);//定义要发送的计算机的地址
            EndPoint Remote = (EndPoint)(sender);//
            while (!_shouldStop)
            {
                try
                {
                    // Send back a response.
                    data = new byte[1024];
                    recv = newsock.ReceiveFrom(data, ref Remote);


                    if (recv != 0)
                    {
                        recvStr = Encoding.ASCII.GetString(data, 0, recv);
                        //string[] scokcmd = recvStr.Split('$');
                        //if (int.Parse(scokcmd[1]) != 0)
                        //{
                        response = this.SocketCommandProcess(recvStr);
                            //Task<string> t = new Task<string>(() => SocketCommandProcess(recvStr));
                            //t.Start();
                            //t.Wait(60000);
                            //response = t.Result;
                            //if (response==string.Empty|| response==null)
                            //{
                            //    response = socketNoresult;
                            //}
                        //}
                        //else
                        //{
                        //    Task t = new Task(() => SocketCommandProcess(recvStr));
                        //    t.Start();
                        //    response = socketNoresult;
                        //}

                        //response = recvStr.ToUpper();
                        resbyte = Encoding.ASCII.GetBytes(response);
                        newsock.SendTo(resbyte, resbyte.Length, SocketFlags.None, Remote);
                        WriteDebugText("socket response : " + response);
                        //this.Invoke((EventHandler)(delegate
                        //{
                            //LogManager.WriteLog(LogFile.Error, "socket response : "+response);
                        //}));
                     }
                }
                catch
                {
                    break;
                }
                
  
                
                
            }
            
        }
        //socket server thread
        private void Socket_start_Button_Click(object sender, EventArgs e)
        {

            //start socket thread
            if(this.toolStripButton2.Checked==false)
            {

                //_createServer = new Thread(new ThreadStart(ListenClientConnect));
                newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _createServer = new Thread(new ThreadStart(ListenUDPClientConnect));
                _createServer.Start();
                _shouldStop = false;

                //Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
                //通过Clientsoket发送数据  
                WriteTraceText("Script server started.");
                
                this.toolStripButton2.CheckState = CheckState.Checked;
                //Console.ReadLine();
            }
            //stop socket thread
            else
            {
                //this.RequestStopthread();
                //this.RequestStopUDPthread();
                try
                {
                    _createServer.Abort();
                    newsock.Close();
                    this.toolStripButton2.CheckState = CheckState.Unchecked;
                    WriteTraceText("Script server stoped.");
                }
                catch
                {
                    WriteTraceText("Script server failed.");
                }
                
                
                
                
            }
            
        }
        //stop udp server
        private void RequestStopUDPthread()
        {
            _shouldStop = true;
            byte[] data = new byte[1024];
            try
            {
                
                _shouldStop = true;
                //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                string welcome = "";
                data = Encoding.ASCII.GetBytes(welcome);
                server.SendTo(data, data.Length, SocketFlags.None, ipep);
                


            }
            catch (Exception e)
            {
                
                WriteErrorText("RequestStopUDP error！" + e);
                
                return;
            }

        }
        private void RequestStopthread()
        {
            try
            {
                _shouldStop = true;
                var msg = "";
                //设定服务器IP地址 
                
                TcpClient client = new TcpClient("127.0.0.1", 8001);
                
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
                
                NetworkStream stream = client.GetStream();
                
                stream.Write(data, 0, data.Length);
                
                stream.Close();
                client.Close();
                
                
            }
            catch(Exception e)
            {
                //Console.WriteLine("连接服务器失败，请按回车键退出！");
                WriteErrorText("连接服务器失败，请按回车键退出！" + e.Message);
                //this.dataDisplayBox.AppendText("连接服务器失败，请按回车键退出！"+e+"\n");
                return;
            }
            
        }

        //set cmdprogressbar
        public void SetCmdprogress(int value)
        {
            this.Invoke((EventHandler)(delegate
            {

                this.CmdProgressBar.Value = value;
                

            }));
            
        }
        //=========================================================================================================
        //write log to logfile and write print to remote display
        //
        //
        //=========================================================================================================
        public void WriteText_to_remote(string text)
        {
            this.Invoke((EventHandler)(delegate
            {

                this.Display_remote.AppendText(text);
                if (this.checkBox_AutoscrollDown.Checked)
                {
                    this.Display_remote.ScrollToCaret();
                }

            }));
        }
        
        
        public void WriteTraceText_to_remote(string text)
        {
            this.Invoke((EventHandler)(delegate
            {

                this.Display_remote.AppendText(text + '\n');
                if (this.checkBox_AutoscrollDown.Checked)
                {
                    this.Display_remote.ScrollToCaret();
                }
                LogManager.WriteLog(LogFile.Trace, text);

            }));
        }
        
        //=========================================================================================================
        //write log to logfile and write print to main display
        //
        //
        //=========================================================================================================
        public void WriteText(string text)
        {
            this.Invoke((EventHandler)(delegate
            {

                this.dataDisplayBox.AppendText(text);
                if (this.checkBox_AutoscrollDown.Checked)
                {
                    this.dataDisplayBox.ScrollToCaret();
                }

            }));
        }

        public void WriteTraceNoText(string text)
        {
            this.Invoke((EventHandler)(delegate
            {


                LogManager.WriteLog(LogFile.Trace, text);

            }));
        }
        
        public void WriteDebugText(string text)
        {
            if(this._debug)
            {
                this.Invoke((EventHandler)(delegate
                {

                    LogManager.WriteLog(LogFile.Debug, text);

                }));
            }
            
        }

        //display
        public void WriteTraceText(string text)
        {
            this.Invoke((EventHandler)(delegate
            {

                this.dataDisplayBox.AppendText(text+'\n');
                if (this.checkBox_AutoscrollDown.Checked)
                {
                    this.dataDisplayBox.ScrollToCaret();
                }
                LogManager.WriteLog(LogFile.Trace, text);

            }));
        }

        public void WriteErrorText(string text)
        {
            this.Invoke((EventHandler)(delegate
            {

                this.dataDisplayBox.AppendText(text + '\n');
                if (this.checkBox_AutoscrollDown.Checked)
                {
                    this.dataDisplayBox.ScrollToCaret();
                }
                LogManager.WriteLog(LogFile.Error, text);

            }));
        }

        public void WriteErrorNoText(string text)
        {
            this.Invoke((EventHandler)(delegate
            {

                
                LogManager.WriteLog(LogFile.Error, text);

            }));
        }

        //write to displaybox in thread
        public void SetText(string text)
        {
            if (dataDisplayBox.InvokeRequired)
            {
                SetTextCallBack st = new SetTextCallBack(SetText);
                this.Invoke(st, new object[] { text });

            }
            else
            {
                dataDisplayBox.AppendText(text + "\n");
                if (this.checkBox_AutoscrollDown.Checked)
                {
                    this.dataDisplayBox.ScrollToCaret();
                }

            }
        }
        //add to historybox in thread
        private void Addhistory(string text)
        {
            if (historyBox.InvokeRequired)
            {
                AddhistoryCallBack st = new AddhistoryCallBack(Addhistory);
                this.Invoke(st, new object[] { text });

            }
            else
            {
                historyBox.Items.Add(text);
                historyBox.SelectedIndex = historyBox.Items.Count - 1;
                
            }
        }
        //button execute process
        private void commandListExecute()
        {
            if(_buttoncmd!=null)
            {
                foreach (string cmd in _buttoncmd)
                {
                    if (cmd != "")
                    {
                        
                        command_Process(cmd);
  
                    }
                }
            }
            _buttoncmd = null;

        }

        //
        private void scriptExecute()
        {
            if(script_repeat_times ==-1)
            {
                while(script_run)
                {
                    string[] cmdlist = scrip_cmd.Split('\n');
                    foreach(string cmd in cmdlist)
                    {
                        command_Process(cmd);
                    }
                    
                    if (script_interval != 0)
                        Thread.Sleep(script_interval);
                }
            }
            else
            {
                for(int i=0; i!=script_repeat_times;i++)
                {
                    if (!script_run)
                        break;
                    string[] cmdlist = scrip_cmd.Split('\n');
                    foreach (string cmd in cmdlist)
                    {
                        command_Process(cmd);
                    }
                    
                    if (script_interval != 0)
                        Thread.Sleep(script_interval);
                    
                }
            }
        }

        //sa capture thread
        //private void saCaptureExecute()
        //{
        //    if (this.addr.SA != "")
        //    {
        //        if (this.VisaSwitch)   //only in visa32
        //            this.SaCapture(captureName, captureDelay);
        //        else
        //            this.SaCapturebyVisacom(captureName, captureDelay);
        //    }

        //    else
        //        WriteTraceText("Please setup SA address first!");

        //}

        //ts execute process
        private void tsListExecute()
        {
            if (_tscmd != null)
            {
                foreach (string cmd in _tscmd)
                {
                    if (cmd != "")
                    {
                        
                        command_Process(cmd);
  
                    }
                    
                }
            }
            _tscmd = null;

        }
        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private void ListenClientConnect()
        {

            listener = new System.Net.Sockets.TcpListener(IPAddress.Parse("127.0.0.1"), 8001);
            listener.Start();
            try
            {
                
                // Buffer for reading data
                Byte[] recvBytes = new Byte[256];
                String recvStr = null;
                String response = null;
                // Enter the listening loop.
                while (!_shouldStop)
                {
                    
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = listener.AcceptTcpClient();
                    client.ReceiveTimeout = 3000;
                    client.SendTimeout = 3000;

                    recvStr = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    //SetText("tag");
                    // Loop to receive all the data sent by the client.
                    try
                    {
                        while ((i = stream.Read(recvBytes, 0, recvBytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            recvStr = System.Text.Encoding.ASCII.GetString(recvBytes, 0, i);

                            // Send back a response.

                            if (recvStr.Length != 0)
                            {

                                response = this.SocketCommandProcess(recvStr);

                                //response = recvStr.ToUpper();
                                byte[] bs = Encoding.ASCII.GetBytes(response);
                                stream.Write(bs, 0, bs.Length);  //返回信息给客户端
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        WriteErrorText("socket error: "+e.Message);
                        //LogManager.WriteLog(LogFile.Trace,e.Message);
                        continue;
                    }
                    
                    

                    // Shutdown and end connection
                    client.Close();
                }
            }
            
            catch(Exception e)
            {
                WriteErrorText("socket listen error: " + e.Message);
            }
            finally
            {
                // Stop listening for new clients.
                if(listener!=null)
                    listener.Stop();
            }
            
        }

        

        //Command buttons event handller
        private void command_button_Click(object sender, EventArgs e)
        {
            if(sender is TabButton)
            {
                MouseEventArgs Mouse_e = (MouseEventArgs)e; 
                //left key
                if(Mouse_e.Button == MouseButtons.Left)
                {
                    TabButton button = sender as TabButton;
                    List<string> data = button._data;
                    
                    if (data != null)
                    {
                        foreach (string cmd in data)
                        {
                            if (cmd != "")
                            {
                                //sender to rru or device
                                if (tabControl_display.SelectedTab == tab_main_display)
                                {
                                    cmdQueue.Enqueue(cmd);
                                    _waitcmdQueueEventHandle.Set();
                                    WriteDebugText(cmd);
                                }
                                //send to du
                                else if (tabControl_display.SelectedTab == tabPage_remote)
                                {
                                    
                                    Addhistory(cmd);
                                    
                                    t.Send(cmd);
                                }
                                

                            }
                        }
                    }
                   
                }
                
                
            }
        }

        //command buttons data edit
        private void command_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TabButton button = sender as TabButton;
                ButtonDataForm bdfm = new ButtonDataForm(button.Name,button._data);

                if (bdfm.ShowDialog() == DialogResult.OK)
                {
                    
                    button._data = bdfm._buttondata;
                    button.Name = bdfm._buttonname;
                    button.Text = bdfm._buttonname;
                }
            }  
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //terminate cmdprocessthread
            cmdProcessThread_run = false;
            cmdQueue.Enqueue("");
            _waitcmdQueueEventHandle.Set();
            _cmdProcessThread.Abort();

            //stop RU Master
            //if(_TCA_ON)
            //    RumaControlClientFactory.StopDefaultTool();

            //save log
            //if (this._log)
                //LogManager.WriteLog(LogFile.Log, this.dataDisplayBox.Text);



            //save button ts file as default ts file
            string localpath = Application.StartupPath;
            string filepath = localpath + @"\default.ts";
            //get current tabs in mainform
            List<Tabcontent> tabs = GetCurrentTabs();

            
            

            TsFileHelper tfh = new TsFileHelper();
            tfh.SaveTab(tabs, filepath);

            //close session
            this.closVisa32sesn();
            this.closeVisacomsesn();
            
            
            
            
            //close serial port 
            

            if (Close_serial_port(_COM_RRU))
            {
                //this._COM_RRU.Dispose();
                this._COM_RRU = null;
            }
            
            
            if(CloseCom(Com_rru))
            {
                Com_rru = null;
            }
            
            //close serial 2 
            if (this._COM2 != null)
            {
                Close_serial2_port();
                
                this._COM2 = null;
            }
            
            //save color
            if (this.backcolor!="")
            {
                ConfigHelper ch = new ConfigHelper();
                ch.UpdateConfig("disp_backcolor", this.backcolor);
            }
            if (this.forecolor != "")
            {
                ConfigHelper ch = new ConfigHelper();
                ch.UpdateConfig("disp_forecolor", this.forecolor);
            }
            //save font
            if (this.fontstyle != "")
            {
                ConfigHelper ch = new ConfigHelper();
                ch.UpdateConfig("disp_font", this.fontstyle);
            }

            //close socket
            if(this.toolStripButton2.Checked==true)
            {
                try
                {
                    this._createServer.Abort();
                    newsock.Close();
                }
                catch
                {
                    WriteTraceText("Stop socket server failed.");
                }
                //this.RequestStopthread();
                //this._createServer.Join();
                //this._createServer = null;
            }

            

        }

        private void closeVisacomsesn()
        {
            if (this.sa_sesn!=null)
            {
                this.sa_sesn.Clear();
                this.sa_sesn.Close();
                this.sa_sesn = null;
                
            }
            if (this.sg_sesn != null)
            {
                this.sg_sesn.Clear();
                this.sg_sesn.Close();
                this.sg_sesn = null;
            }
            if (this.sg2_sesn != null)
            {
                this.sg2_sesn.Clear();
                this.sg2_sesn.Close();
                this.sg2_sesn = null;
            }
            if (this.rfbox_sesn != null)
            {
                this.rfbox_sesn.Clear();
                this.rfbox_sesn.Close();
                this.rfbox_sesn = null;
            }
            if (this.rfbox2_sesn != null)
            {
                this.rfbox2_sesn.Clear();
                this.rfbox2_sesn.Close();
                this.rfbox2_sesn = null;
            }
            if (this.is_sesn != null)
            {
                this.is_sesn.Clear();
                this.is_sesn.Close();
                this.is_sesn = null;
            }
            if (this.is2_sesn != null)
            {
                this.is2_sesn.Clear();
                this.is2_sesn.Close();
                this.is2_sesn = null;
            }
            if (this.dc5767a_sesn != null)
            {
                this.dc5767a_sesn.Clear();
                this.dc5767a_sesn.Close();
                this.dc5767a_sesn = null;
            }
        }

        private void closVisa32sesn()
        {
            if (this.sesnSA != -1)
            {
                visa32.viClear(this.viSA);
                visa32.viClose(this.viSA);
                visa32.viClose(this.sesnSA);
                this.sesnSA = -1;
            }
            if (this.sesnSG != -1)
            {
                visa32.viClear(this.viSG);
                visa32.viClose(this.viSG);
                visa32.viClose(this.sesnSG);
                this.sesnSG = -1;
            }
            if (this.sesnSG2 != -1)
            {
                visa32.viClear(this.viSG2);
                visa32.viClose(this.viSG2);
                visa32.viClose(this.sesnSG2);
                this.sesnSG2 = -1;
            }
            if (this.sesnRFBOX != -1)
            {
                visa32.viClear(this.viRFBOX);
                visa32.viClose(this.viRFBOX);
                visa32.viClose(this.sesnRFBOX);
                this.sesnRFBOX = -1;
            }
            if (this.sesnRFBOX2 != -1)
            {
                visa32.viClear(this.viRFBOX2);
                visa32.viClose(this.viRFBOX2);
                visa32.viClose(this.sesnRFBOX2);
                this.sesnRFBOX2 = -1;
            }
            if (this.sesnIS != -1)
            {
                visa32.viClear(this.viIS);
                visa32.viClose(this.viIS);
                visa32.viClose(this.sesnIS);
                this.sesnIS = -1;
            }
            if (this.sesnIS2 != -1)
            {
                visa32.viClear(this.viIS2);
                visa32.viClose(this.viIS2);
                visa32.viClose(this.sesnIS2);
                this.sesnIS2 = -1;
            }
            if (this.sesnDC5767A != -1)
            {
                visa32.viClear(this.viDC5767A);
                visa32.viClose(this.viDC5767A);
                visa32.viClose(this.sesnDC5767A);
                this.sesnDC5767A = -1;
            }
            
        }

        private List<Tabcontent> GetCurrentTabs()
        {
            List<Tabcontent> tabs = new List<Tabcontent>();

            //get current tabs in mainform
            foreach (NewTabPage ntp in this.tabControl1.TabPages)
            {
                Tabcontent tab = new Tabcontent();
                tab.tabname = ntp.Name;

                foreach (Control item in ntp.Controls)
                {
                    if (item is TableLayoutPanel)
                    {
                        TableLayoutPanel layoutPanel = item as TableLayoutPanel;
                        foreach (Control btitem in layoutPanel.Controls)
                        {
                            if (btitem is TabButton)
                            {
                                TabButton button = btitem as TabButton;
                                Buttontype bt = new Buttontype();
                                bt.btnname = button.Name;
                                bt.btnnumber = button._index;
                                bt.data = button._data;
                                tab.buttons[button._index] = bt;
                            }
                        }
                    }

                }
                tabs.Add(tab);
            }

            return tabs;
        }

        private void button_clearscreen_Click(object sender, EventArgs e)
        {
            this.dataDisplayBox.Clear();
            this.Display_remote.Clear();
        }

        private void menuSetup_Click(object sender, EventArgs e)
        {
            Address oldaddr = (Address)this.addr.Clone();
            SerialPortSetupForm sf = new SerialPortSetupForm(this.addr);
            //SetupForm sf = new SetupForm(this.addr);
            if(sf.ShowDialog() == DialogResult.OK)
            {
                if(sf.localaddr.RRU != "")
                {
                    this.addr.RRU = sf.localaddr.RRU;
                    this.addr.Baudrate_rru = sf.localaddr.Baudrate_rru;
                }
                if(sf.localaddr.SERIAL2 != "")
                {
                    this.addr.Baudrate_com2 = sf.localaddr.Baudrate_com2;
                    this.addr.SERIAL2 = sf.localaddr.SERIAL2;
                }
                
                

                foreach (Control ctl in this.SerialpropertyBox.Controls)
                {
                    if(ctl.Name == "label_rruport")
                    {
                        ctl.Text = this.addr.RRU;
                    }
                    else if (ctl.Name == "label_rrubaud")
                    {
                        ctl.Text = this.addr.Baudrate_rru;
                    }
                    else if (ctl.Name == "label_serial2port")
                    {
                        ctl.Text = this.addr.SERIAL2;
                    }
                    else if (ctl.Name == "label_serial2baud")
                    {
                        ctl.Text = this.addr.Baudrate_com2;
                    }
                }
                if (this.addr.RRU != ""&& oldaddr.RRU!= this.addr.RRU)
                {

                    if (Serial_status)
                        ModifySerialParameter(Com_rru, addr.RRU, int.Parse(addr.Baudrate_rru), rruConnButton);
                    else
                        ModifySerialParameter(_COM_RRU, addr.RRU, int.Parse(addr.Baudrate_rru), rruConnButton);

                    
                     
                }

                if(oldaddr.Baudrate_rru != addr.Baudrate_rru)
                {
                    if(Serial_status)
                        ModifySerialParameter(Com_rru, addr.RRU, int.Parse(addr.Baudrate_rru), rruConnButton);
                    else
                        ModifySerialParameter(_COM_RRU, addr.RRU, int.Parse(addr.Baudrate_rru), rruConnButton);
                }

                if (this.addr.SERIAL2 != "" && oldaddr.SERIAL2 != this.addr.SERIAL2)
                {
                    if (this._COM2.IsOpen == true)
                    {
                        if (Close_serial_port(_COM2))
                        {
                            this._COM2.PortName = this.addr.SERIAL2;
                            this._COM2.BaudRate = int.Parse(this.addr.Baudrate_com2);
                            try
                            {
                                this._COM2.Open();
                                this._COM2.DiscardOutBuffer();
                                this._COM2.DiscardInBuffer();
                            }
                            catch (Exception exp_rru)
                            {
                                WriteErrorText("Serial port 2 init error! please check serial infomation first. " + exp_rru.Message);
                            }
                        }
                        else
                            WriteErrorText("Close serial port 2 failed! Please stop serial port listening and re-setup serial port first!");
                        

                    }
                    
                }

                //save address and port to config file

                ConfigHelper ch = new ConfigHelper();
                Dictionary<string, string> adds = this.addr.UpdateAddress();
                ch.UpdateAddr(adds);
                
                sf.Close();
            }
        }

        ////check and open(close) session by visa32
        private void CheckSessionbyVisa32(string addr, Label instrtag, ref int instrsession, ref int vi)
        {
            int status = -1;
            instrtag.Visible = true;

            visa32.viOpenDefaultRM(out instrsession);
            status = visa32.viOpen(instrsession, addr, visa32.VI_NULL, visa32.VI_NULL, out vi);
                        
            if (status >=visa32.VI_SUCCESS)
            {
                instrtag.BackColor = Color.SpringGreen;
                visa32.viSetAttribute(vi, visa32.VI_ATTR_WR_BUF_OPER_MODE, visa32.VI_FLUSH_ON_ACCESS);
                visa32.viSetAttribute(vi,visa32.VI_ATTR_RD_BUF_OPER_MODE,visa32.VI_FLUSH_ON_ACCESS);
                //visa32.viSetAttribute(vi, visa32.VI_ATTR_TMO_VALUE,1000); 
            }
            else
            {
                WriteTraceText("Please re-check the address :"+ addr+ status.ToString());
                instrtag.BackColor = Color.Pink;
            }

               
        }
        //check and open(close) session
        private void CheckSession(string addr,Label instrtag, ResourceManager rm, ref IMessage sesn)
        {
            
            
            instrtag.Visible = true;
            try
            {
                if (rm == null)
                {
                    
                    rm = new Ivi.Visa.Interop.ResourceManager();

                    sesn = (IMessage)rm.Open(addr, AccessMode.NO_LOCK, 0);
                    sesn.Timeout = 2000;
                    
                    instrtag.BackColor = Color.SpringGreen;
                    
                    
                }



            }
            catch (System.Runtime.InteropServices.COMException exp)
            {
                instrtag.BackColor = Color.Pink;
                


            }
      
        }

        //init instrument
        private void initInstrumentStatus(Address addr,Address oldaddr,bool init=false)
        {
            if (addr.SA != string.Empty)
            {
                this.tag_sa.Visible = true;
                if (addr.SA != oldaddr.SA || init)
                    this.CheckSession(addr.SA,  this.tag_sa, this.sa_rm, ref this.sa_sesn);
                
            }
            else
            {
                this.tag_sa.Visible = false;
            }
            if (addr.SG != string.Empty)
            {
                this.tag_sg1.Visible = true;
                if (addr.SG != oldaddr.SG || init)
                    this.CheckSession(addr.SG,  this.tag_sg1, this.sg_rm, ref this.sg_sesn);
            }
            else
            {
                this.tag_sg1.Visible = false;
            }
            if (addr.SG2 != string.Empty)
            {
                this.tag_sg2.Visible = true;
                if (addr.SG2 != oldaddr.SG2 || init)
                    this.CheckSession(addr.SG2,  this.tag_sg2, this.sg2_rm, ref this.sg2_sesn);
            }
            else
            {
                this.tag_sg2.Visible = false;
            }
            if (addr.RFBOX != string.Empty)
            {
                this.tag_rfbox1.Visible = true;
                if (addr.RFBOX != oldaddr.RFBOX || init)
                    this.CheckSession(addr.RFBOX,  this.tag_rfbox1, this.rfbox_rm, ref this.rfbox_sesn);
            }
            else
            {
                this.tag_rfbox1.Visible = false;
            }
            if (addr.RFBOX2 != string.Empty)
            {
                this.tag_rfbox2.Visible = true;
                if (addr.RFBOX2 != oldaddr.RFBOX2 || init)
                    this.CheckSession(addr.RFBOX2,  this.tag_rfbox2, this.rfbox2_rm, ref this.rfbox2_sesn);
            }
            else
            {
                this.tag_rfbox2.Visible = false;
            }
            if (addr.IS1 != string.Empty)
            {
                this.tag_is1.Visible = true;
                if (addr.IS1 != oldaddr.IS1 || init)
                    this.CheckSession(addr.IS1,  this.tag_is1, this.is_rm, ref this.is_sesn);
            }
            else
            {
                this.tag_is1.Visible = false;
            }
            if (addr.IS2 != string.Empty)
            {
                this.tag_is2.Visible = true;
                if (addr.IS2 != oldaddr.IS2 || init)
                    this.CheckSession(addr.IS2,  this.tag_is2, this.is2_rm, ref this.is2_sesn);
            }
            else
            {
                this.tag_is2.Visible = false;
            }
            if (addr.DC5767A != string.Empty)
            {
                this.tag_DC5767A.Visible = true;
                if (addr.DC5767A != oldaddr.DC5767A || init)
                    this.CheckSession(addr.DC5767A,  this.tag_DC5767A, this.dc5767a_rm, ref this.dc5767a_sesn);
            }
            else
            {
                this.tag_DC5767A.Visible = false;
            }
            
            
            
            
            
        }
        ////init instrument
        private void initInstrumentStatusbyVisa32(Address addr, Address oldaddr, bool init = false)
        {
            if(addr.SA!=string.Empty)
            {
                this.tag_sa.Visible = true;
                if(addr.SA != oldaddr.SA || init)
                    this.CheckSessionbyVisa32(addr.SA, this.tag_sa, ref this.sesnSA, ref this.viSA);
            }
            else
            {
                this.tag_sa.Visible = false;
            }
            if (addr.SG != string.Empty)
            {
                this.tag_sg1.Visible = true;
                if (addr.SG != oldaddr.SG || init)
                    this.CheckSessionbyVisa32(addr.SG, this.tag_sg1, ref this.sesnSG, ref this.viSG);
            }
            else
            {
                this.tag_sg1.Visible = false;
            }
            if (addr.SG2 != string.Empty)
            {
                this.tag_sg2.Visible = true;
                if (addr.SG2 != oldaddr.SG2 || init)
                    this.CheckSessionbyVisa32(addr.SG2, this.tag_sg2, ref this.sesnSG2, ref this.viSG2);
            }
            else
            {
                this.tag_sg2.Visible = false;
            }
            if (addr.RFBOX != string.Empty)
            {
                this.tag_rfbox1.Visible = true;
                if (addr.RFBOX != oldaddr.RFBOX || init)
                    this.CheckSessionbyVisa32(addr.RFBOX, this.tag_rfbox1, ref this.sesnRFBOX, ref this.viRFBOX);
            }
            else
            {
                this.tag_rfbox1.Visible = false;
            }
            if (addr.RFBOX2 != string.Empty)
            {
                this.tag_rfbox2.Visible = true;
                if (addr.RFBOX2 != oldaddr.RFBOX2 || init)
                    this.CheckSessionbyVisa32(addr.RFBOX2, this.tag_rfbox2, ref this.sesnRFBOX2, ref this.viRFBOX2);
            }
            else
            {
                this.tag_rfbox2.Visible = false;
            }
            if (addr.IS1 != string.Empty)
            {
                this.tag_is1.Visible = true;
                if (addr.IS1 != oldaddr.IS1 || init)
                    this.CheckSessionbyVisa32(addr.IS1, this.tag_is1, ref this.sesnIS, ref this.viIS);
            }
            else
            {
                this.tag_is1.Visible = false;
            }
            if (addr.IS2 != string.Empty)
            {
                this.tag_is2.Visible = true;
                if (addr.IS2 != oldaddr.IS2 || init)
                    this.CheckSessionbyVisa32(addr.IS2, this.tag_is2, ref this.sesnIS2, ref this.viIS2);
            }
            else
            {
                this.tag_is2.Visible = false;
            }
            if (addr.DC5767A != string.Empty)
            {
                this.tag_DC5767A.Visible = true;
                if (addr.DC5767A != oldaddr.DC5767A || init)
                    this.CheckSessionbyVisa32(addr.DC5767A, this.tag_DC5767A, ref this.sesnDC5767A, ref this.viDC5767A);
            }
            else
            {
                this.tag_DC5767A.Visible = false;
            }

            //this.CheckSession(addr.RUMASTER, oldaddr.RUMASTER, this.tag_rumaster, this.rumaster_session, init);
            
            
        }

        private void DC5767A_ON_Click(object sender, EventArgs e)
        {
            string cmd = "DC5767A.OUTPut ON";
            this.command_Process(cmd);
        }

        private void DC5767A_OFF_Click(object sender, EventArgs e)
        {
            string cmd = "DC5767A.OUTPut OFF";
            this.command_Process(cmd);
        }

        private void SACAPTURE_Click(object sender, EventArgs e)
        {
            
            if (this.addr.capture1 != string.Empty)
            {
                if (this.VisaSwitch) //true == visa32
                {
                    this.SaCapture(viCapture1);
                }
                else
                {
                    this.SaCapturebyVisacom(sesnCapture1);
                }
            }
            else
                WriteTraceText("Please input capture1 address first!");
            
        }

        

        private void dataDisplayBox_TextChanged(object sender, EventArgs e)
        {
            
            
            if (this.dataDisplayBox.Lines.Length > 600)
            {
                //if(this._log)
                    //LogManager.WriteLog(LogFile.Log, this.dataDisplayBox.Text);
                this.dataDisplayBox.Clear();
            }
            
        }

        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //TextBox tb = sender as TextBox;
            
            
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            
            
            if (e.KeyCode == Keys.Up)
            {
                
                if(this.historyBox.Items.Count!=0&&historyBox.SelectedIndex>0)
                {
                    this.historyBox.SelectedIndex--;
                    if (this.historyBox.SelectedItem!=null)
                        this.InputBox.Text = this.historyBox.SelectedItem.ToString();
                    
                }
                
            }
            if(e.KeyCode == Keys.Down)
            {
                
                if (this.historyBox.Items.Count != 0&&historyBox.SelectedIndex<historyBox.Items.Count-1)
                {
                    this.historyBox.SelectedIndex++;
                    if (this.historyBox.SelectedItem != null)
                        this.InputBox.Text = this.historyBox.SelectedItem.ToString();
                    
                }
            }
        }

        //rumaster stasrtup
        private void Rumasterswitchbutton_Click(object sender, EventArgs e)
        {
            
                // Connect the the TigerApplication service using a local or remote IP address
                tas = new ApplicationControl("127.0.0.1");

                // Get a list with all started applications. If no application is running - start a new application
                string[] tsls = tas.GetTslList();
                if (tsls.Length == 0)
                {
                    tas.StartSpecifiedTsl(@"C:\Program Files\Ericsson\TCA\TSL.exe");
                    tsls = tas.GetTslList();
                }

                // Get attached hardware devices
                tsl = new TslControlClient(tsls[0]);
                string[] hwSnrs = tsl.GetHws();
                if (hwSnrs.Length == 0)
                {
                    
                    throw new Exception("No hardwares detected!");
                    

                }
                else
                {
                    string[] uris = tsl.GetServiceList(ToolType.ID_RUMA);
                    string toolUri;
                    WriteTraceText("uris is :"+ uris.Length.ToString());
                    if (uris.Length == 1)
                    {
                        toolUri = uris[0];
                    }
                    else if (uris.Length > 1)
                    {
                        toolUri = uris[uris.Length - 1];
                    }
                    else
                    {
                        toolUri = tsl.StartService(ToolType.ID_RUMA, hwSnrs[0]);
                    }

                    this.rumaClient = RumaControlClientFactory.Create(toolUri);
                    _TCA_ON = true;
                    this.icdf = this.rumaClient.CpriDataFlow;
                    this.rCarrierConfig = this.rumaClient.CarrierConfig;
                    this.rCpriConfig = this.rumaClient.CpriConfig;
                    this.rServerBase = this.rumaClient.PlatformUtilities;
                    this.tag_rumaster.BackColor = Color.SpringGreen;
                    this.rTriggerConfig = this.rumaClient.TriggerConfig;
                    this.rIRULoader = this.rumaClient.OoM.RULoader;


                }
            // Retrieve all running tool services. If no tool service is running - start a new tool
                
            //RUMA instance created, now start tool must be executed and resource needs to be allocated.

            //rumaClient.RuMaUtilities.SetCustomStartupParametersVee(selectedCpriPorts,

            //                                                    trigger1,

            //                                                    trigger2,

            //                                                    trigger3,

            //                                                    trigger4,

            //                                                    rxPortBuffer,

            //                                                    rxIqBandWidth,

            //                                                    txIqBandWidth,

            //                                                    totalRxBufferSize,

            //                                                   totalTxBufferSize,

            //                                                    allocateAuxPort,

            //                                                    allocateDebugPort);


            
                
                
                
            
            
        }

        private void SerialpropertyBox_Enter(object sender, EventArgs e)
        {

        }

        //serial2 connect
        private void Serial2_conn_button_Click(object sender, EventArgs e)
        {
            //rru serialport open ,and listen to datareceive
            if (this.Serial2_conn_button.Checked == false)
            {
                
                //初始化串口，如果未设置串口则弹出对话框提示用户先设置串口
                if (this.addr.SERIAL2 == "")
                {
                    WriteTraceText("Please setup serial 2 port first!");
                    
                }
                else
                {
                    //设定port,波特率,无检验位，8个数据位，1个停止位
                    this._COM2 = new SerialPort(this.addr.SERIAL2, int.Parse(this.addr.Baudrate_com2), Parity.None, 8, StopBits.One);
                    this._COM2.ReadBufferSize = 2048;
                    this._COM2.ReceivedBytesThreshold = 1;
                    this._COM2.NewLine = "\n";
                    this._COM2.DtrEnable = true;
                    this._COM2.RtsEnable = true;



                    //open serial port

                    try
                    {

                        this._COM2.Open();
                        this._COM2.DataReceived += Com_2_DataReceived;
                        this._COM2.ErrorReceived += Com2_ErrorReceived;
                        WriteTraceText("Serialport 2 open ,and listen to datareceive.");
                        
                        this.Serial2_conn_button.CheckState = CheckState.Checked;
                    }
                    catch (Exception ex)
                    {
                        this._COM2 = null;
                        //现实异常信息给客户。  
                        
                        WriteErrorText("Serial port 2 open failed: " + ex.Message);
                    }

                }

            }
            //close serial port
            else
            {
                Close_serial2_port();
                this.Serial2_conn_button.CheckState = CheckState.Unchecked;
            }
        }

        private void visaSwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugForm df = new DebugForm(this.VisaSwitch,Serial_status);
            if (df.ShowDialog() == DialogResult.OK)
            {
                //this.dataDisplayBox.AppendText(df.visastatus+"\n");
                if(df.visastatus=="visacom")
                {
                    this.VisaSwitch = false; //false ==visacom
                    closVisa32sesn();
                    initInstrumentStatus(this.addr,this.addr,true);
                    WriteTraceText("switch to "+df.visastatus);
                }
                else
                {
                    this.VisaSwitch = true;  //true == visa32
                    closeVisacomsesn();
                    initInstrumentStatusbyVisa32(this.addr, this.addr, true);
                    WriteTraceText("switch to " + df.visastatus);
                }
                if (df.SerialMode == "MsComm")
                {
                    this.Serial_status = true; //true ==MsComm
                    
                    if(_COM_RRU.IsOpen)
                    {
                        if(Close_serial_port(_COM_RRU))
                        {
                            this.rruConnButton.CheckState = CheckState.Unchecked;
                            if (addr.RRU.Contains("COM"))
                                InitComm(Com_rru, short.Parse(addr.RRU.Remove(0, 3)), addr.Baudrate_rru, rruConnButton);
                            else
                                WriteTraceText("Wrong SerialPortName..." + addr.RRU);
                                
                        }

                    }
                   
                    
                    
                    WriteTraceText("switch to " + df.SerialMode);
                }
                else
                {
                    this.Serial_status = false;  //false == serialport
                    
                    if (Com_rru.PortOpen == true)
                    {
                        if(CloseCom(Com_rru))
                        {
                            this.rruConnButton.CheckState = CheckState.Unchecked;
                            InitSerialPort(_COM_RRU, addr.RRU, int.Parse(this.addr.Baudrate_rru),this.rruConnButton);
                        }
                    }
                    
                    
                    
                    WriteTraceText("switch to " + df.SerialMode);
                }
            }
        }

        //open capture RXEVM data form
        private void rXEVMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.rumaClient!=null)
            {
                EVMForm ef = new EVMForm(this.rumaClient, this.icdf);
                ef.Show();
            }
            else
            {
                MessageBox.Show("Please start Rumaster(TCA) first!");
            }
            
        }

        private void button_executepy_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();


            ofd.InitialDirectory = @"c:\";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //fName = ofd.FileName;
                ////execute python script
                //var engine = IronPython.Hosting.Python.CreateEngine();
                //var scope = engine.CreateScope();
                //var sourceCode = engine.CreateScriptSourceFromFile(fName);
                //ICollection<string> Paths = engine.GetSearchPaths();
                //Paths.Add(@"C:\receive");
                //engine.SetSearchPaths(Paths);
                //var actual = sourceCode.Compile().Execute<string>(scope);
                



            }
        }

        private void radio_CR2_CheckedChanged(object sender, EventArgs e)
        {
            
            if(radio_LF2.Checked)
                this._com2trans = "\r";
            if(radio_CR2.Checked)
                this._com2trans = "\n";
            if(radioC_L2.Checked)
                this._com2trans = "\r\n";

        }

        

        private void historyBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InputBox.Text = historyBox.SelectedItem.ToString();
        }
        //==========================================================================================
        //refresh instruments connect status
        //==========================================================================================
        private void tag_sa_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(this.addr.SA, this.tag_sa, ref this.sesnSA, ref this.viSA);
            }
            else
            {
                this.CheckSession(this.addr.SA, this.tag_sa, this.sa_rm, ref this.sa_sesn);
            }
        }

        private void tag_sg1_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(addr.SG, this.tag_sg1, ref this.sesnSG, ref this.viSG);
            }
            else
            {
                this.CheckSession(addr.SG, this.tag_sg1, this.sg_rm, ref this.sg_sesn);
            }
        }

        private void tag_sg2_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(this.addr.SG2, this.tag_sg2, ref this.sesnSG2, ref this.viSG2);
            }
            else
            {
                this.CheckSession(this.addr.SG2, this.tag_sg2, this.sg2_rm, ref this.sg2_sesn);
            }
        }

        private void tag_rfbox1_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(this.addr.RFBOX, this.tag_rfbox1, ref this.sesnRFBOX, ref this.viRFBOX);
            }
            else
            {
                this.CheckSession(this.addr.RFBOX, this.tag_rfbox1, this.rfbox_rm, ref this.rfbox_sesn);
            }
        }

        private void tag_rfbox2_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(this.addr.RFBOX2, this.tag_rfbox2, ref this.sesnRFBOX2, ref this.viRFBOX2);
            }
            else
            {
                this.CheckSession(this.addr.RFBOX2, this.tag_rfbox2, this.rfbox2_rm, ref this.rfbox2_sesn);
            }
        }

        private void tag_is1_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(this.addr.IS1, this.tag_is1, ref this.sesnIS, ref this.viIS);
            }
            else
            {
                this.CheckSession(this.addr.IS1, this.tag_is1, this.is_rm, ref this.is_sesn);
            }
        }

        private void tag_is2_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(this.addr.IS2, this.tag_is2, ref this.sesnIS2, ref this.viIS2);
            }
            else
            {
                this.CheckSession(this.addr.IS2, this.tag_is2, this.is2_rm, ref this.is2_sesn);
            }
        }

        private void tag_DC5767A_Click(object sender, EventArgs e)
        {
            if (this.VisaSwitch) //true == visa32
            {
                this.CheckSessionbyVisa32(this.addr.DC5767A, this.tag_DC5767A, ref this.sesnDC5767A, ref this.viDC5767A);
            }
            else
            {
                this.CheckSession(this.addr.DC5767A, this.tag_DC5767A, this.dc5767a_rm, ref this.dc5767a_sesn);
            }
        }

        private void checkBox_debug_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_debug.Checked)
            {
                this._debug = true;
                WriteTraceText("Debug On.");
            }
            else
            {
                this._debug = false;
                WriteTraceText("Debug Off.");
            }
        }

        private void checkBox_Log_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Log.Checked)
            {
                this._log = true;
                LogManager.dateTag = true;
                WriteTraceText("Date tag On.");
            }
            else
            {
                this._log = false;
                LogManager.dateTag = false;
                WriteTraceText("Date tag Off.");
            }
        }

        //Update to latest version
        private void upgradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool running = false;
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                //MessageBox.Show(p.ProcessName);
                if (p.ProcessName.ToLower() == "rttupdate")
                {
                    running = true;
                    break;
                }
            }
            if(!running)
            {
                Process.Start(Application.StartupPath + @"\RTTUpdate.exe");
            }
            else
            {
                WriteTraceText("update is running!");
            }
            
        }
        //============================================Script===============================================
        private void button_start_Click(object sender, EventArgs e)
        {
            
            
            foreach (Control ctl in this.groupBox_script.Controls)
            {
                if (ctl.Name == "textBox_cmd")
                {
                    if (ctl.Text != "")
                        scrip_cmd = ctl.Text;
                    else
                        scrip_cmd = "";
                }
                else if (ctl.Name == "textBox_rpt")
                {
                    if (ctl.Text != "")
                    {
                        try
                        {
                            script_repeat_times = int.Parse(ctl.Text);
                        }
                        catch(Exception ex)
                        {
                            WriteErrorText("Please input number into repeat time!");
                        }
                    }
                        
                    else
                        script_repeat_times = -1;

                }
                else if(ctl.Name == "textBox_interval")
                {
                    if(ctl.Text!="")
                    {
                        script_interval = int.Parse(ctl.Text);
                    }
                    
                }
                
            }
            if(scrip_cmd=="")
            {
                WriteErrorText("Please input correct command into command textbox!");
            }
            else
            {
                
                script_run = true;

                
                _scriptthread = new Thread(new ThreadStart(scriptExecute));
                _scriptthread.Start();
                
                
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            script_run = false;
            _scriptthread.Abort();

        }


        //==================================================================================================
        private void comboBox_instrumentprefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_instrumentprefix.SelectedItem.ToString() != "")
            {
                string command = this.InputBox.Text;


                switch (comboBox_instrumentprefix.SelectedIndex)
                {

                    //rru
                    case 0:
                        InputBox.Text = "";
                        //this.send_to_rru();

                        break;
                    //sa
                    case 1:
                        InputBox.Text = Constant.PRIFIX_SA + ":";
                        
                        break;
                    //sg1
                    case 2:
                        InputBox.Text = Constant.PRIFIX_SG + ":";
                        
                        break;
                    //sg2
                    case 3:
                        InputBox.Text = Constant.PRIFIX_SG2 + ":";
                        
                        break;
                    //rfbox1
                    case 4:
                        InputBox.Text = Constant.PRIFIX_RFBOX;
                        
                        break;
                    //rfbox2
                    case 5:
                        InputBox.Text = Constant.PRIFIX_RFBOX2;
                        
                        break;
                    //dc5767a
                    case 6:
                        InputBox.Text = Constant.PRIFIX_DC5767A;
                        
                        break;
                    //is1
                    case 7:
                        InputBox.Text = Constant.PRIFIX_IS1+":";
                        
                        break;
                    //is2
                    case 8:
                        InputBox.Text = Constant.PRIFIX_IS2 + ":";
                        
                        break;
                    //rumaster
                    case 10:
                        InputBox.Text = Constant.PRIFIX_RUMASTER;
                        
                        break;
                    //SERIAL2
                    case 9:
                        InputBox.Text = "SERIAL2.";
                        break;

                    default:

                        break;

                }
            }
        }

        //exchange log path
        private void checkBox_secondary_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_secondary.Checked)
            {
                _snapPath = localpath + @"\snapshot\";
                if (!Directory.Exists(_snapPath))
                    Directory.CreateDirectory(_snapPath);

                _rxevmPath = localpath + @"\rxevm\";
                if (!Directory.Exists(_rxevmPath))
                    Directory.CreateDirectory(_rxevmPath);

                logPath = localpath + @"\log\";
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);
                LogManager.LogPath = logPath;
            }
            else
            {
                _snapPath = mainpath + @"snapshot\";
                if (!Directory.Exists(_snapPath))
                    Directory.CreateDirectory(_snapPath);

                _rxevmPath = mainpath + @"rxevm\";
                if (!Directory.Exists(_rxevmPath))
                    Directory.CreateDirectory(_rxevmPath);

                logPath = mainpath + @"log\";
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);
                LogManager.LogPath = logPath;
            }
        }

        private void serial2_pow_on_Click(object sender, EventArgs e)
        {
            command_Process("DC5767A.OUT1");
        }

        private void serial2_pow_off_Click(object sender, EventArgs e)
        {
            command_Process("DC5767A.OUT0");
        }


        //open Visa Device setup form
        private void visaDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Address oldaddr = (Address)this.addr.Clone();
            VisaDeviceSetupForm sf = new VisaDeviceSetupForm(this.addr);
            if (sf.ShowDialog() == DialogResult.OK)
            {
                

                this.addr.SA = sf.localaddr.SA;
                this.addr.SG = sf.localaddr.SG;
                this.addr.SG2 = sf.localaddr.SG2;
                this.addr.RFBOX = sf.localaddr.RFBOX;
                this.addr.RFBOX2 = sf.localaddr.RFBOX2;
                this.addr.IS1 = sf.localaddr.IS1;
                this.addr.IS2 = sf.localaddr.IS2;
                this.addr.DC5767A = sf.localaddr.DC5767A;
                this.addr.capture1 = sf.localaddr.capture1;
                this.addr.capture2 = sf.localaddr.capture2;

                if (this.VisaSwitch) //true == visa32
                {
                    this.initInstrumentStatusbyVisa32(this.addr, oldaddr);
                }
                else
                {
                    this.initInstrumentStatus(this.addr, oldaddr);
                }

                
                //SA
                if (this.addr.capture1 == Constant.VISADEVICE_LIST[0])
                    viCapture1 = viSA;
                //SG1
                else if (this.addr.capture1 == Constant.VISADEVICE_LIST[1])
                    viCapture1 = viSG;
                //SG2
                else if (this.addr.capture1 == Constant.VISADEVICE_LIST[2])
                    viCapture1 = viSG2;
                //RFBOX1
                else if (this.addr.capture1 == Constant.VISADEVICE_LIST[3])
                    viCapture1 = viRFBOX;
                //RFBOX2
                else if (this.addr.capture1 == Constant.VISADEVICE_LIST[4])
                    viCapture1 = viRFBOX2;
                //DC5767A
                else if (this.addr.capture1 == Constant.VISADEVICE_LIST[5])
                    viCapture1 = viDC5767A;
                //ISG1
                else if (this.addr.capture1 == Constant.VISADEVICE_LIST[6])
                    viCapture1 = viIS;
                //ISG2
                else if (this.addr.capture1 == Constant.VISADEVICE_LIST[7])
                    viCapture1 = viIS2;
                
                
                //SA
                if (this.addr.capture2 == Constant.VISADEVICE_LIST[0])
                    viCapture2 = viSA;
                //SG1
                else if (this.addr.capture2 == Constant.VISADEVICE_LIST[1])
                    viCapture2 = viSG;
                //SG2
                else if (this.addr.capture2 == Constant.VISADEVICE_LIST[2])
                    viCapture2 = viSG2;
                //RFBOX1
                else if (this.addr.capture2 == Constant.VISADEVICE_LIST[3])
                    viCapture2 = viRFBOX;
                //RFBOX2
                else if (this.addr.capture2 == Constant.VISADEVICE_LIST[4])
                    viCapture2 = viRFBOX2;
                //DC5767A
                else if (this.addr.capture2 == Constant.VISADEVICE_LIST[5])
                    viCapture2 = viDC5767A;
                //ISG1
                else if (this.addr.capture2 == Constant.VISADEVICE_LIST[6])
                    viCapture2 = viIS;
                //ISG2
                else if (this.addr.capture2 == Constant.VISADEVICE_LIST[7])
                    viCapture2 = viIS2;
                //save address and port to config file

                ConfigHelper ch = new ConfigHelper();
                Dictionary<string, string> adds = this.addr.UpdateAddress();
                ch.UpdateAddr(adds);

                sf.Close();
            }
        }
        //not use
        private void toolStripButton_capture2_Click(object sender, EventArgs e)
        {
            if (this.addr.capture2 != string.Empty)
            {
                if (this.VisaSwitch) //true == visa32
                {
                    this.SaCapture(viCapture2);
                }
                else
                {
                    this.SaCapturebyVisacom(sesnCapture1);
                }
            }
            else
                WriteTraceText("Please input capture2 address first!");
        }
        private void capture1(string filename = "")
        {
            if (this.addr.capture1 != string.Empty)
            {
                if (this.VisaSwitch) //true == visa32
                {
                    this.SaCapture(viCapture1, filename);
                }
                else
                {
                    this.SaCapturebyVisacom(sesnCapture1, filename);
                }
            }
            else
                WriteTraceText("Please input capture1 address first!");
        }
        private void capture2(string filename="")
        {
            if (this.addr.capture2 != string.Empty)
            {
                if (this.VisaSwitch) //true == visa32
                {
                    this.SaCapture(viCapture2,filename);
                }
                else
                {
                    this.SaCapturebyVisacom(sesnCapture2,filename);
                }
            }
            else
                WriteTraceText("Please input capture2 address first!");
        }

        private void toolStripButton_capture2_MouseDown(object sender, MouseEventArgs e)
        {
            //按鼠标右键，弹出菜单   
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                CaptureFilenameConfirmForm cf = new CaptureFilenameConfirmForm();
                if(cf.ShowDialog() == DialogResult.OK)
                {
                    
                    capture2(cf.filename);
                }
            }
            else if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
               
                capture2();
            }
        }

        private void SACAPTURE_MouseDown(object sender, MouseEventArgs e)
        {
            //按鼠标右键，弹出菜单   
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                CaptureFilenameConfirmForm cf = new CaptureFilenameConfirmForm();
                if (cf.ShowDialog() == DialogResult.OK)
                {
                    
                    capture1(cf.filename);
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                
                capture1();
            }
        }

        //数据保护模式
        private void checkBox_pause_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_pause.Checked)
            {
                dataProtect = true;
                WriteTraceText("dataProtect is true");
            }
            else
            {
                dataProtect = false;
                WriteTraceText("dataProtect is false");
            }
                
        }
        //cmdhold
        private void checkBox_cmdHold_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_cmdHold.Checked)
            {
                cmdHold = true;
                WriteTraceText("cmdHold is true");
            }
            else
            {
                cmdHold = false;
                WriteTraceText("cmdHold is false");
            }
        }

        private void otherDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Address oldaddr = (Address)this.addr.Clone();
            OtherDeviceForm sf = new OtherDeviceForm(this.addr);
            if (sf.ShowDialog() == DialogResult.OK)
            {
                
                this.addr.Server_Port = sf.localaddr.Server_Port;
                if(sf.localaddr.DU_IP != String.Empty)
                {
                    label_terminal_IP.Text = sf.localaddr.DU_IP;
                    this.addr.DU_IP = sf.localaddr.DU_IP;
                }
                //save address and port to config file

                ConfigHelper ch = new ConfigHelper();
                Dictionary<string, string> adds = this.addr.UpdateAddress();
                ch.UpdateAddr(adds);

                sf.Close();
            }
        }

        //telnet
        //private async void toolStripButton_telnet_Click(object sender, EventArgs e)
        private  void toolStripButton_telnet_Click(object sender, EventArgs e)
        {
            //du = new Tcpclient(this.addr.DU_IP, 23);
            //string print = await du.Read();


            t = new Telnet();

            t.writereceive = WriteTraceText_to_remote;
            t.doSocket(this.addr.DU_IP, 23);
            //WriteTraceText_to_remote(print);
            //Telnet p1 = new Telnet("147.128.108.73", "hwg", "123456");
            //p1.Connect();

        }

        private void historyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.historyBox.Items.Count > 100)
            {
                this.historyBox.Items.Remove(historyBox.Items[0]);
                //this.historyBox.Items.Clear();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void cmmenu_tab_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Newtab_Click(object sender, EventArgs e)
        {
            NewTabPage Page;
            //tc has no buttons,add a empty page
            
            Page = new NewTabPage();
            Page.Name = "Page" + _tabindex.ToString();
            Page.Text = "tabPage" + _tabindex.ToString();
            Page.TabIndex = _tabindex;

            TableLayoutPanel layoutPanel = new TableLayoutPanel();
            layoutPanel.RowCount = 4;
            layoutPanel.ColumnCount = 8;
            layoutPanel.Dock = DockStyle.Fill;
            Page.Controls.Add(layoutPanel);
            this.tabControl1.Controls.Add(Page);
            //layout button
            //int x = 8, y = 10;
            for (int i = 0; i != 32; i++)
            {

                TabButton tb = new TabButton();
                tb._index = i;
                toolTip1.SetToolTip(tb, tb.Text);
                tb.Dock = DockStyle.Fill;
                for (int k = 0; k < layoutPanel.RowCount; k++)
                {
                    layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    for (int j = 0; j < layoutPanel.ColumnCount; j++)
                    {
                        layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                        layoutPanel.Controls.Add(tb);
                    }
                }
                tb.Click += new System.EventHandler(command_button_Click);
                tb.MouseDown += new MouseEventHandler(command_button_MouseDown);
            }
            
        }

        private void ModifyName_Click(object sender, EventArgs e)
        {
            ModifyTabnameform mtf = new ModifyTabnameform();
            if(mtf.ShowDialog() == DialogResult.OK)
            {
                if(mtf.Tabname!=null)
                {
                    this.tabControl1.SelectedTab.Text = mtf.Tabname;
                    this.tabControl1.SelectedTab.Name = mtf.Tabname;
                }
            }
        }

        private void DeleteTab_Click(object sender, EventArgs e)
        {
            //消息框中需要显示哪些按钮，此处显示“确定”和“取消”

            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;

            //"确定要退出吗？"是对话框的显示信息，"退出系统"是对话框的标题

            //默认情况下，如MessageBox.Show("确定要退出吗？")只显示一个“确定”按钮。
            DialogResult dr = MessageBox.Show("Confirm to delete Tab : "+ this.tabControl1.SelectedTab.Name+" ?", "Delete Tab", messButton);

            if (dr == DialogResult.OK)//如果点击“确定”按钮

            {

                this.tabControl1.TabPages.Remove(this.tabControl1.SelectedTab);

            }

            else//如果点击“取消”按钮

            {

 

            }
            
        }

        private void backColorSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if(cd.ShowDialog() == DialogResult.OK)
            {
                this.dataDisplayBox.BackColor = cd.Color;
                this.backcolor = ColorTranslator.ToHtml(cd.Color);
            }
        }

        private void foreColorSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.dataDisplayBox.ForeColor = cd.Color;
                this.forecolor = ColorTranslator.ToHtml(cd.Color);
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog cd = new FontDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.dataDisplayBox.Font = cd.Font;
                FontConverter fc = new FontConverter();
                this.fontstyle = fc.ConvertToInvariantString(cd.Font);
                //this.forecolor = ColorTranslator.ToHtml(cd.Color);
            }
        }

        //do capture evm data
        void _evmtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //WriteDebugText("rxevm capture tick start");
            this.EVMRXCapture(_cpriport);
            //WriteDebugText("rxevm capture tick fin");
        }

        

        //rxevm
        private void EVMRXCapture(string capturecpriport)
        {

            
            try
            {
                this.icdf.StartCapture(capturecpriport, Tiger.Ruma.FlowDataType.IQ);
                Thread.Sleep(20);
                this.icdf.StopCapture(capturecpriport, Tiger.Ruma.FlowDataType.IQ);
                
                string filename = DateTime.Now.ToString("HH_mm_ss") + ".cul";
                this.icdf.ExportAllCapturedData(capturecpriport, @"c:\RTT\rxevm\" + filename, "", Tiger.Ruma.ExportFormat.Cul, Tiger.Ruma.UmtsType.LTE);
                WriteTraceText("rxevm capture : " + filename +" on cpri "+ capturecpriport);
            }
            catch (Exception exp)
            {
                WriteTraceText(exp.Message + "RX_EVM_Capture failed!");
                
            }
        }

        private void dataDisplayBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)//用户是否按下了Ctrl键
            {
                if (e.KeyCode == Keys.C)
                {
                    if(rruConnButton.Checked)
                        SendToSerial(((char)(3)).ToString() + '\r',Com_rru,_COM_RRU);
                    
                }
            }
            else if(e.KeyCode == Keys.F5)  //f5 debug print switch
            {
                if(this._debug)
                {
                    this._debug = false;
                    WriteTraceText("Debug Off.");
                }
                else
                {
                    this._debug = true;
                    WriteTraceText("Debug On.");
                }
            }
            
        }
        //==============================================cmd process queue solution=============================
        private void cmdProcessThread()
        {
            while(true)
            {
                _waitcmdQueueEventHandle.WaitOne();
                if(!cmdProcessThread_run)
                {
                    break;
                }
                while (cmdQueue.Count > 0)
                {
                    Thread.Sleep(350);
                    string cmd = cmdQueue.Dequeue().ToString();
                    WriteDebugText("cmdProcessThread process cmd:" + cmd);

                    command_Process(cmd);
                    
                }
            }
            
            
        }
        

        
    }

    public class Address : ICloneable
    {
        public string RRU = "";
        public string Baudrate_rru = "";
        public string SERIAL2 = "";
        public string Baudrate_com2 = "";
        public string DC5767A = "";
        public string SA = "";
        public string SG = "";
        public string SG2 = "";
        public string RFBOX = "";
        public string RFBOX2 = "";
        public string DU_IP = "";

        public string IS1 = "";
        public string IS2 = "";
        public string Server_Port = "";
        public string capture1 = "";
        public string capture2 = "";

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public void SetAddress(Dictionary<string, string> addrdic,Links link)
        {
            //ConfigHelper ch = new ConfigHelper();
            //Dictionary<string, string> addrdic = ch.GetAddr();
            this.DC5767A = addrdic[Constant.DEVICE_NAME_AGILENT5767A];
            this.IS1 = addrdic[Constant.DEVICE_NAME_INTERFERENCE_SIGNAL_1];
            this.IS2 = addrdic[Constant.DEVICE_NAME_INTERFERENCE_SIGNAL_2];
            this.RFBOX = addrdic[Constant.DEVICE_NAME_RFBOX];
            this.RFBOX2 = addrdic[Constant.DEVICE_NAME_RFBOX2];
            
            this.SA = addrdic[Constant.DEVICE_NAME_SIGNALANALYZER];
            this.SG = addrdic[Constant.DEVICE_NAME_SIGNALGENERATOR];
            this.SG2 = addrdic[Constant.DEVICE_NAME_SIGNALGENERATOR2];
            this.RRU = addrdic["port_rru"];
            this.Baudrate_rru = addrdic["baudrate_rru"];
            this.Server_Port = addrdic["server_port"];
            this.DU_IP = addrdic["Du_ip"];
            //if(this.RRU!=""&&this.Baudrate_rru!="")
            //{
            //    link.set_port1(this.RRU, this.Baudrate_rru, "", "8", "1");
            //}


            //this.SERIAL2 = addrdic[Constant.DEVICE_NAME_DC_DH1716A];
        }

        public Dictionary<string, string> UpdateAddress()
        {
            Dictionary<string, string> addrdic = new Dictionary<string, string>();
            addrdic[Constant.DEVICE_NAME_AGILENT5767A] = this.DC5767A;
            addrdic[Constant.DEVICE_NAME_INTERFERENCE_SIGNAL_1]=this.IS1;
            addrdic[Constant.DEVICE_NAME_INTERFERENCE_SIGNAL_2]=this.IS2;
            addrdic[Constant.DEVICE_NAME_RFBOX]=this.RFBOX;
            addrdic[Constant.DEVICE_NAME_RFBOX2]=this.RFBOX2;
            
            addrdic[Constant.DEVICE_NAME_SIGNALANALYZER]=this.SA;
            addrdic[Constant.DEVICE_NAME_SIGNALGENERATOR]= this.SG;
            addrdic[Constant.DEVICE_NAME_SIGNALGENERATOR2]= this.SG2;
            addrdic["port_rru"] = this.RRU;
            addrdic["baudrate_rru"] = this.Baudrate_rru;
            addrdic["server_port"] = this.Server_Port;
            addrdic["Du_ip"] = this.DU_IP;
            //addrdic[Constant.DEVICE_NAME_DC_DH1716A]= this.SERIAL2;
            return addrdic;
        }
    }


    //extend button
    public partial class TabButton : Button
    {
        
        //buttontype 
        //public Buttontype bt = new Buttontype();
        public int _index = 0;
        public List<string> _data = new List<string>();
        
    }

    //extend tabpage
    public partial class NewTabPage : TabPage
    {
        //tabcontent
        Tabcontent tc = new Tabcontent();
    }

    public class Links
    {
        public SerialPort com_rru;
        public SerialPort com_2;

        public string port_rru = "";
        public string baudrate_rru = "";
        public string parity_rru = "";
        public string stopbits_rru = "";
        public string databits_rru = "";

        public string port_2 = "";
        public string baudrate_2 = "";
        public string parity_2 = "";
        public string stopbits_2 = "";
        public string databits_2 = "";

        public bool port1_hasset = false;
        public bool port2_hasset = false;

        public void set_port1(string port, string baudrate, string parity, string stopbits, string databits)
        {
            this.port_rru = port;
            this.baudrate_rru = baudrate;
            this.parity_rru = parity;
            this.stopbits_rru = stopbits;
            this.databits_rru = databits;
            this.port1_hasset = true;
        }
        public void init_port1(Dictionary<string,string> portatt)
        {
            
            
                this.port_rru = portatt["port_rru"];
                this.baudrate_rru = portatt["baudrate_rru"];
                this.parity_rru = portatt["parity_rru"];
                this.stopbits_rru = portatt["stopbits_rru"];
                this.databits_rru = portatt["databits_rru"];
                this.port1_hasset = true;
            
            
        }

        public Dictionary<string, string> get_port1()
        {

            Dictionary<string, string> portatt = new Dictionary<string, string>();
            portatt["port_rru"] = this.port_rru;
            portatt["baudrate_rru"] = this.baudrate_rru;
            portatt["parity_rru"] = this.parity_rru;
            portatt["stopbits_rru"] = this.stopbits_rru;
            portatt["databits_rru"] = this.databits_rru;
            
            return portatt;


        }

        public void set_port2(string port, string baudrate, string parity, string stopbits, string databits)
        {
            
            
                this.port_2 = port;
                this.baudrate_2 = baudrate;
                this.parity_2 = parity;
                this.stopbits_2 = stopbits;
                this.databits_2 = databits;
                this.port2_hasset = true;
            
                
        }



    }


}

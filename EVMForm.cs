using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using TigerApplicationServiceClient;
using TSLControlClient.TslControl;
using Tiger.Ruma.WcfInterface;
using System.ServiceModel;
using System.Threading;

namespace RTT
{
    public partial class EVMForm : Form
    {
        Tiger.Ruma.WcfInterface.IRumaControlClient rumaclient;
        Tiger.Ruma.IRumaCpriDataFlow Rumacdf;
        System.Windows.Forms.Timer evmtimer = new System.Windows.Forms.Timer();
        string capturecpriport = null;
        int timelimit = 0;
        int Interval = 0;
        int count = 0;

        bool _isstart = false;

        int ticknumber = 0;

        public EVMForm(Tiger.Ruma.WcfInterface.IRumaControlClient ruma, Tiger.Ruma.IRumaCpriDataFlow IRumacdf)
        {
            InitializeComponent();
            this.rumaclient = ruma;
            this.Rumacdf = IRumacdf;
            evmtimer.Tick += new EventHandler(evmtimer_tick);

        }
        //flush all parameter
        private void reflushAllParameter()
        {
            timelimit = 0;
            Interval = 0;
            count = 0;
            capturecpriport = null;
            ticknumber = 0;
        }
        //do capture evm data
        private void evmtimer_tick(object sender, EventArgs e)
        {
            //this.progressBar.Value = 10;
            if(count!=0)
            {
                
                if(ticknumber< count)
                {
                    if (timelimit != 0)
                    {
                        if ((ticknumber + 1) * Interval < timelimit)
                        {
                            //do evm capture
                            //this.progressBar.Value = 30;
                            this.EVMRXCapture();
                            ticknumber++;
                            //this.progressBar.Value = 100;
                        }
                        else
                        {
                            this.evmtimer.Stop();
                            reflushAllParameter();
                            _isstart = false;
                            MessageBox.Show("Time up! Capture finished!");
                        }
                    }
                    else
                    {
                        //do evm capture
                        //this.progressBar.Value = 30;
                        this.EVMRXCapture();
                        ticknumber++;
                        //this.progressBar.Value = 100;
                    }
                    
                }
                else
                {
                    ticknumber = 0;
                    this.evmtimer.Stop();
                    reflushAllParameter();
                    _isstart = false;
                    MessageBox.Show("Count is over! Capture finished!");
                }
            }
            else
            {
                if (timelimit != 0)
                {
                    if ((ticknumber + 1) * Interval < timelimit)
                    {
                        //do evm capture
                        this.progressBar.Value = 30;
                        this.EVMRXCapture();
                        ticknumber++;
                        this.progressBar.Value = 100;
                    }
                    else
                    {
                        this.progressBar.Value = 100;
                        this.evmtimer.Stop();
                        reflushAllParameter();
                        _isstart = false;
                        MessageBox.Show("Time up! Capture finished!");
                    }
                }
                else
                {
                    //do evm capture
                    this.progressBar.Value = 30;
                    this.EVMRXCapture();
                    ticknumber++;
                    this.progressBar.Value = 100;
                }


            }

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if(_isstart == false)
            {
                if (this.text_timelimit.Text != "")
                {
                    try
                    {
                        timelimit = int.Parse(this.text_timelimit.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Input numeric type to timelimit please!");
                    }
                }

                if (this.textCount.Text != "")
                {
                    try
                    {
                        count = int.Parse(this.textCount.Text);

                    }
                    catch
                    {
                        MessageBox.Show("Input numeric type to timelimit please!");
                    }
                }

                if (this.textInterval.Text == "")
                {
                    MessageBox.Show("Input interval please!");
                }
                else
                {
                    try
                    {
                        this.Interval = int.Parse(this.textInterval.Text);
                        //start capture rxevm data
                        if (comboBox_cpriport.SelectedItem != null)
                        {
                            capturecpriport = comboBox_cpriport.SelectedItem.ToString();
                            evmtimer.Interval = this.Interval * 1000; //ms
                            ticknumber = 0;
                            this.evmtimer.Start();
                            _isstart = true;
                        }
                        else
                        {
                            MessageBox.Show("Please confirm the cpriport infomation!");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Please confirm the interval infomation!");
                    }


                }
            }
            
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            this.evmtimer.Stop();
            reflushAllParameter();
            _isstart = false;
        }

        private void EVMRXCapture()
        {

            //this.progressBar.Value = 40;
            try
            {
                Rumacdf.SetFlowDataMode(capturecpriport, Tiger.Ruma.CpriFlowDirection.RX, Tiger.Ruma.FlowDataType.IQ, Tiger.Ruma.FlowDataMode.RAW);
                Rumacdf.StartCapture(capturecpriport, Tiger.Ruma.FlowDataType.IQ);
                Thread.Sleep(20);
                Rumacdf.StopCapture(capturecpriport, Tiger.Ruma.FlowDataType.IQ);
                this.progressBar.Value = 60;
                string filename = DateTime.Now.ToString("HH_mm_ss")+".cul";
                Rumacdf.ExportAllCapturedData(capturecpriport, @"c:\RTT\rxevm\"+ filename, "", Tiger.Ruma.ExportFormat.Cul, Tiger.Ruma.UmtsType.LTE);
                this.progressBar.Value = 80;
            }
            catch(Exception exp)
            {
                this.evmtimer.Stop();
                reflushAllParameter();
                _isstart = false;
                MessageBox.Show(exp.Message+"RX_EVM_Capture failed!");
                
            }
        }

        private void EVMForm_Load(object sender, EventArgs e)
        {
            string[] cpriports = new string[10];
            
            cpriports = rumaclient.CpriConfig.GetAllocatedCpriPorts();
            foreach (string port in cpriports)
            {
                this.comboBox_cpriport.Items.Add(port);
            }
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 100;
        }
    }
}

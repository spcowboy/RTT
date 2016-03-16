using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTT
{
    public partial class DebugForm : Form
    {
        public string visastatus = "visa32";
        public string SerialMode = "SerialPort";
        public DebugForm(bool status,bool serialstatus)
        {
            InitializeComponent();
            if (status)
                //visastatus = "visa32";
                radiovisa32.Select();
            else
                //visastatus = "visacom";
                radiovisacom.Select();
            if (serialstatus)
                //serialstatus = "mscomm";
                radio_mscomm.Select();
            else
                //serialstatus = "SerialPort";
                radio_SerialPort.Select();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void radiovisacom_CheckedChanged(object sender, EventArgs e)
        {
            this.visastatus = radiovisacom.Text;
        }

        private void radiovisa32_CheckedChanged(object sender, EventArgs e)
        {
            this.visastatus = radiovisa32.Text;
        }

        private void radio_mscomm_CheckedChanged(object sender, EventArgs e)
        {
            SerialMode = radio_mscomm.Text;
        }

        private void radio_SerialPort_CheckedChanged(object sender, EventArgs e)
        {
            SerialMode = radio_SerialPort.Text;
        }
    }
}

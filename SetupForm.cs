using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace RTT
{
    public partial class SetupForm : Form
    {
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

        
        public string SA = "";
        public string SG = "";
        public string SG2 = "";
        public string RFBOX = "";
        public string RFBOX2 = "";
        public string IS1 = "";
        public string IS2 = "";
        public string DC5767A = "";
        public string Du_ip = "";

        public Address localaddr = new Address();
        public SetupForm(Address addr)
        {
            InitializeComponent();

            if (addr != null)
            {
                this.localaddr = addr;
                this.textBox1.Text = this.localaddr.SA;
                this.textBox2.Text = this.localaddr.SG;
                this.textBox3.Text = this.localaddr.SG2;
                this.textBox4.Text = this.localaddr.RFBOX;
                this.textBox5.Text = this.localaddr.RFBOX2;
                this.textBox6.Text = this.localaddr.IS1;
                this.textBox7.Text = this.localaddr.IS2;
                this.textBox_du_ip.Text = this.localaddr.DU_IP;
                this.textBox9.Text = this.localaddr.DC5767A;
                this.textBox_Server_Port.Text = this.localaddr.Server_Port;

            }
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            string[] portlist = SerialPort.GetPortNames();

            for (int i = 0; i < portlist.Length; i++)

            {

                this.comboBox1.Items.Add(portlist[i]);
                this.comboBox10.Items.Add(portlist[i]);

            }

            for (int i = 0; i != Constant.BAUD_RATES.Length; i++)
            {
                this.comboBox2.Items.Add(Constant.BAUD_RATES[i]);
                this.comboBox9.Items.Add(Constant.BAUD_RATES[i]);
            }

            for (int i = 0; i != Constant.PARITY.Length; i++)
            {
                this.comboBox3.Items.Add(Constant.PARITY[i]);
                this.comboBox8.Items.Add(Constant.PARITY[i]);
            }

            for (int i = 0; i != Constant.DATA_BITS.Length; i++)
            {
                this.comboBox4.Items.Add(Constant.DATA_BITS[i]);
                this.comboBox7.Items.Add(Constant.DATA_BITS[i]);
            }

            for (int i = 0; i != Constant.STOP_BITS.Length; i++)
            {
                this.comboBox5.Items.Add(Constant.STOP_BITS[i]);
                this.comboBox6.Items.Add(Constant.STOP_BITS[i]);
            }
            //rru
            /*if (this.comboBox1.Items.Contains(this.localaddr.RRU))
                this.comboBox1.SelectedText = this.localaddr.RRU;
            if (this.comboBox2.Items.Contains(this.localaddr.Baudrate_rru))
                this.comboBox2.SelectedText = this.localaddr.Baudrate_rru;
            else*/
            this.comboBox2.SelectedIndex = 1;
            
            this.comboBox3.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 4;
            this.comboBox5.SelectedIndex = 0;

            //serial2
            

            this.comboBox9.SelectedIndex = 1;
            this.comboBox8.SelectedIndex = 0;
            this.comboBox7.SelectedIndex = 4;
            this.comboBox6.SelectedIndex = 0;
            
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void button_ok_Click_1(object sender, EventArgs e)
        {

            localaddr.SA = this.textBox1.Text;


            localaddr.SG = this.textBox2.Text;


            localaddr.SG2 = this.textBox3.Text;


            localaddr.RFBOX = this.textBox4.Text;


            localaddr.RFBOX2 = this.textBox5.Text;

            localaddr.IS1 = this.textBox6.Text;

            localaddr.IS2 = this.textBox7.Text;

            localaddr.Server_Port = this.textBox_Server_Port.Text;

            localaddr.DC5767A = this.textBox9.Text;
            localaddr.DU_IP = this.textBox_du_ip.Text;
            
            
            if (comboBox1.SelectedItem != null)
            {
                localaddr.RRU = comboBox1.SelectedItem.ToString();
                //this.port_rru = comboBox1.SelectedItem.ToString();
                localaddr.Baudrate_rru = comboBox2.SelectedItem.ToString();
                //this.baudrate_rru = comboBox2.SelectedItem.ToString();
                //this.parity_rru = comboBox3.SelectedItem.ToString();
                //this.databits_rru = comboBox4.SelectedItem.ToString();
                //this.stopbits_rru = comboBox5.SelectedItem.ToString();
            }

            if (comboBox10.SelectedItem != null)
            {
                localaddr.SERIAL2 = comboBox10.SelectedItem.ToString();
                //this.port_2 = comboBox10.SelectedItem.ToString();
                localaddr.Baudrate_com2 = comboBox9.SelectedItem.ToString();
                //this.baudrate_2 = comboBox9.SelectedItem.ToString();
                //this.parity_2 = comboBox8.SelectedItem.ToString();
                //this.databits_2 = comboBox7.SelectedItem.ToString();
                //this.stopbits_2 = comboBox6.SelectedItem.ToString();
            }
            
            
            this.DialogResult = DialogResult.OK;
            
        }
    }
}

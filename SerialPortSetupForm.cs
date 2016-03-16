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
    public partial class SerialPortSetupForm : Form
    {
        //public string port_rru = "";
        //public string baudrate_rru = "";
        //public string parity_rru = "";
        //public string stopbits_rru = "";
        //public string databits_rru = "";

        //public string port_2 = "";
        //public string baudrate_2 = "";
        //public string parity_2 = "";
        //public string stopbits_2 = "";
        //public string databits_2 = "";

        public Address localaddr = new Address();
        public SerialPortSetupForm(Address addr)
        {
            InitializeComponent();
            if (addr != null)
            {
                this.localaddr = addr;
            }

        }

        

        

        
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SerialPortSetupForm_Load(object sender, EventArgs e)
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
            if (this.comboBox1.Items.Contains(this.localaddr.RRU))
                this.comboBox1.SelectedText = this.localaddr.RRU;
            if (this.comboBox2.Items.Contains(this.localaddr.Baudrate_rru))
                this.comboBox2.SelectedText = this.localaddr.Baudrate_rru;
            else
                this.comboBox2.SelectedIndex = 1;

            this.comboBox3.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 4;
            this.comboBox5.SelectedIndex = 0;

            //serial2
            if (this.comboBox10.Items.Contains(this.localaddr.SERIAL2))
                this.comboBox10.SelectedText = this.localaddr.SERIAL2;
            if (this.comboBox9.Items.Contains(this.localaddr.Baudrate_com2))
                this.comboBox9.SelectedText = this.localaddr.Baudrate_com2;
            else
                this.comboBox9.SelectedIndex = 1;
            this.comboBox8.SelectedIndex = 0;
            this.comboBox7.SelectedIndex = 4;
            this.comboBox6.SelectedIndex = 0;

            //======
            
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != null)
            {
                localaddr.RRU = comboBox1.SelectedItem.ToString();
                
            }
            else if(comboBox1.Text != null)
            {
                localaddr.RRU = comboBox1.Text;
                
            }
            if (comboBox2.SelectedItem != null)
            {
                
                localaddr.Baudrate_rru = comboBox2.SelectedItem.ToString();
                
            }
            else if (comboBox2.Text != null)
            {
                
                localaddr.Baudrate_rru = comboBox2.Text;
            }
            if (comboBox10.SelectedItem != null)
            {
                localaddr.SERIAL2 = comboBox10.SelectedItem.ToString();
                
            }
            else if(comboBox10.Text != null)
            {
                localaddr.SERIAL2 = comboBox10.Text;
                
            }
            if (comboBox9.SelectedItem != null)
            {

                localaddr.Baudrate_com2 = comboBox9.SelectedItem.ToString();
                
            }
            else if (comboBox9.Text != null)
            {

                localaddr.Baudrate_com2 = comboBox9.Text;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }
    }
}

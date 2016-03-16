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
    public partial class VisaDeviceSetupForm : Form
    {
        public Address localaddr = new Address();
        public VisaDeviceSetupForm(Address addr)
        {
            InitializeComponent();
            if(addr!=null)
            {
                this.localaddr = addr;
                this.textBox1.Text = this.localaddr.SA;
                this.textBox2.Text = this.localaddr.SG;
                this.textBox3.Text = this.localaddr.SG2;
                this.textBox4.Text = this.localaddr.RFBOX;
                this.textBox5.Text = this.localaddr.RFBOX2;
                this.textBox6.Text = this.localaddr.IS1;
                this.textBox7.Text = this.localaddr.IS2;
                
                this.textBox9.Text = this.localaddr.DC5767A;
                for(int i=0; i!= Constant.VISADEVICE_LIST.Length; i++)
                    this.comboBox_capture1.Items.Add(Constant.VISADEVICE_LIST[i]);
                if (this.localaddr.capture1 != string.Empty)
                    this.comboBox_capture1.SelectedText = this.localaddr.capture1;
                else
                    this.comboBox_capture1.SelectedIndex = 0;
                for (int i = 0; i != Constant.VISADEVICE_LIST.Length; i++)
                    this.comboBox_capture2.Items.Add(Constant.VISADEVICE_LIST[i]);
                if (this.localaddr.capture2!=string.Empty)
                    this.comboBox_capture2.SelectedText = this.localaddr.capture2;

            }
        }

        private void InstrumentSetupForm_Load(object sender, EventArgs e)
        {
            
        }
        private void button_cancel_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            
            this.localaddr.SA = this.textBox1.Text;
            
            
            this.localaddr.SG = this.textBox2.Text;
            
            this.localaddr.SG2 = this.textBox3.Text;
            
            this.localaddr.RFBOX = this.textBox4.Text;
            
            this.localaddr.RFBOX2 = this.textBox5.Text;
            
            this.localaddr.IS1 = this.textBox6.Text;
            
            this.localaddr.IS2 = this.textBox7.Text;
            
            this.localaddr.DC5767A = this.textBox9.Text;
            if (comboBox_capture1.SelectedItem != null)
            {
                localaddr.capture1 = comboBox_capture1.SelectedItem.ToString();

            }
            else if (comboBox_capture1.Text != null)
            {
                localaddr.capture1 = comboBox_capture1.Text;

            }
            if (comboBox_capture2.SelectedItem != null)
            {
                localaddr.capture2 = comboBox_capture2.SelectedItem.ToString();

            }
            else if (comboBox_capture2.Text != null)
            {
                localaddr.capture2 = comboBox_capture2.Text;

            }
            
            
            this.DialogResult = DialogResult.OK;
            
        }
    }
}

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
    public partial class OtherDeviceForm : Form
    {
        public Address localaddr = new Address();
        public OtherDeviceForm(Address addr)
        {
            InitializeComponent();
            if (addr != null)
            {
                this.textBox_Server_Port.Text = addr.Server_Port;
                this.textBox_du_ip.Text = addr.DU_IP;
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            localaddr.Server_Port = this.textBox_Server_Port.Text;
            localaddr.DU_IP = this.textBox_du_ip.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

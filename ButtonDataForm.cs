using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RTT
{
    public partial class ButtonDataForm : Form
    {
        public string _buttonname = "";
        public List<string> _buttondata = new List<string>();
        public ButtonDataForm(string name,List<string> data)
        {
            InitializeComponent();
            this._buttonname = name;
            this.textBox1.Text = name;
            this._buttondata = data;
            foreach(string cmd in data)
            {
                this.richTextBox1.AppendText(cmd + "\n");
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!=null)
            _buttonname = textBox1.Text;
            if(richTextBox1.Text!=null)
            {
                if (richTextBox1.Text.Contains("\n"))
                {
                    string[] textarray = Regex.Split(richTextBox1.Text, "\n", RegexOptions.IgnoreCase);
                    _buttondata = new List<string>(textarray);
                }
                else
                    _buttondata.Add(richTextBox1.Text);
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttoncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            this._buttonname = "";
            this.textBox1.Clear();
            this._buttondata.Clear();
            this.richTextBox1.Clear();
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            StringBuilder Cliptxt = new StringBuilder();
            Cliptxt.Append(_buttonname).Append("!");
            if (textBox1.Text != null)
                _buttonname = textBox1.Text;
            if (richTextBox1.Text != null)
            {
                if (richTextBox1.Text.Contains("\n"))
                {
                    string[] textarray = Regex.Split(richTextBox1.Text, "\n", RegexOptions.IgnoreCase);
                    for(int i = 0; i!= textarray.Length; i++)
                    {
                        if (i != textarray.Length - 1)
                        {
                            Cliptxt.Append(textarray[i]).Append("@");
                        }
                        else
                            Cliptxt.Append(textarray[i]);

                    }
                    _buttondata = new List<string>(textarray);
                }
                else
                    _buttondata.Add(richTextBox1.Text);
            }
            
            
            Clipboard.SetDataObject(Cliptxt.ToString());
        }

        private void button_paste_Click(object sender, EventArgs e)
        {

            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                string tempstr = (String)iData.GetData(DataFormats.Text);
                string[] tempstrarryall = tempstr.Split('!');
                if(tempstrarryall!=null&& tempstrarryall.Length>1)
                {
                    
                    this.textBox1.Text = tempstrarryall[0];
                    if (tempstrarryall[1].Contains('@'))
                    {
                        string[] tempstrarrydata = tempstrarryall[1].Split('@');
                        if(tempstrarrydata!=null&& tempstrarrydata.Length>1)
                        {
                            for(int i = 0; i!= tempstrarrydata.Length; i++)
                            {
                                this.richTextBox1.AppendText(tempstrarrydata[i] + "\n");
                            }
                        }
                    }
                    else
                    {
                        this.richTextBox1.AppendText(tempstrarryall[1] + "\n");
                    }
                        

                }
                    

            }
        }
    }
}

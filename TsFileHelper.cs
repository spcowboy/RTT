using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace RTT
{
    class Tabcontent
    {
        public string tabname = "";
        public Buttontype[] buttons = new Buttontype[32];
        public Tabcontent(string tabname)
        {
            this.tabname = tabname;
            for (int i = 0; i != 32; i++)
            {
                buttons[i] = new Buttontype(i, "");
            }

        }
        public Tabcontent()
        {
            this.tabname = "";
            for (int i = 0; i != 32; i++)
            {
                buttons[i] = new Buttontype(i, "");
            }

        }
    }
    class Buttontype
    {
        public int btnnumber=-1;
        public string btnname = "";
        public List<string> data = new List<string>();
        public Buttontype(int btnnumber=-1, string btnname="")
        {
            this.btnname = btnname;
            this.btnnumber = btnnumber;
        }
        /*public Buttontype()
        {
            this.btnname = "";
            this.btnnumber = 0;
            this.data = new List<string>();

        }*/
    }
    class Cmddocument
    {
        //public Dictionary<int, Tabcontent> tabs = new Dictionary<int, Tabcontent>();
        public List<Tabcontent> tabs = new List<Tabcontent>();
    }
    class TsFileHelper
    {
        //save cmddoc to file
        public void SaveTab(List<Tabcontent> tabs, string path)
        {

            
            List<string> alllines = new List<string>();
            for (int i = 0; i != tabs.Count; i++)
            {
                Tabcontent tab = tabs[i];
                alllines.Add("<tab {" + tab.tabname + "}>");
                for (int j = 0; j != tab.buttons.Length; j++)
                {

                    Buttontype btn = tab.buttons[j];
                    if (btn.btnnumber!=-1)
                    {
                        alllines.Add("<button {" + btn.btnnumber + "} {" + btn.btnname + "}>");
                        alllines.Add("<data>");
                        for (int k = 0; k != btn.data.Count; k++)
                        {
                            alllines.Add(btn.data[k]);
                        }
                        alllines.Add("</data>");
                        alllines.Add("</button>");
                    }
                    
                }
                alllines.Add("</tab>");
            }
            File.WriteAllLines(path, alllines.ToArray());
            //File.WriteAllText(path, alllines.ToString());
            /*
            if (!File.Exists(path))
            {
                StreamWriter sw = File.AppendText(path);
                File.a
                File.WriteAllLines(path, alllines.ToArray());
            }
            else
            {
                File.WriteAllLines(path, alllines.ToArray());
            }
             */   
        }

        //get cmddoc from file
        public List<Tabcontent> getTabs(string path)
        {
            //Cmddocument cmddoc = new Cmddocument();
            List<Tabcontent> tabs = new List<Tabcontent>();
            if (File.Exists(path))
            {
                string[] alllines;
                alllines = File.ReadAllLines(path);
                //int i = 0;
                int tabno = 0;
                int tabstart = 0;
                for (int i = 0; i != alllines.Length;)
                {
                    string line = alllines[i];
                    Tabcontent tab = new Tabcontent("");
                    if (line.StartsWith("<tab"))
                    {
                        tabstart = i;

                        string tabname = line.Substring(line.IndexOf("{") + 1, line.IndexOf("}") - line.IndexOf("{") - 1);
                        tab = new Tabcontent(tabname);
                        int j = 0;
                        i++;
                        for (int newtab = i; newtab != alllines.Length; newtab = i)
                        {
                            string newline = alllines[i];
                            if (newline.StartsWith("</tab"))
                            {
                                tabs.Add(tab);
                                //cmddoc.[tabno] = tab;
                                tabno++;
                                i++;
                                break;
                            }
                            else if (newline.StartsWith("<button"))
                            {
                                int btnstart = newtab;
                                Buttontype btn = new Buttontype(int.Parse(newline.Substring(newline.IndexOf("{") + 1, newline.IndexOf("}") - newline.IndexOf("{") - 1)), newline.Substring(newline.LastIndexOf("{") + 1, newline.LastIndexOf("}") - newline.LastIndexOf("{") - 1));
                                i++;
                                for (int newbtn = i; newbtn != alllines.Length; newbtn = i)
                                {
                                    string newbtnline = alllines[i];
                                    if (newbtnline.StartsWith("</button"))
                                    {
                                        i++;
                                        break;
                                    }
                                    else if (newbtnline.StartsWith("<data") || newbtnline.StartsWith("</data"))
                                    {
                                        i++;
                                        continue;
                                    }
                                    else
                                    {
                                        if (!newbtnline.Contains("<icon>") && !newbtnline.Contains("</icon>") && !newbtnline.Contains("<desc>") && !newbtnline.Contains("</desc>"))
                                        {
                                            btn.data.Add(newbtnline);
                                            i++;
                                        }
                                        else
                                        {
                                            i++;
                                        }
                                    }
                                }
                                //tab.buttons[j] = btn;
                                tab.buttons[btn.btnnumber] = btn;
                                j++;
                            }
                        }
                    }
                    else
                    {
                        i++;
                        continue;
                    }

                }
            }

            return tabs;
        }
    }
}

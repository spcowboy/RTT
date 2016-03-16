using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTT
{
    /// <summary>
    /// 重写的TabControl控件 带关闭按钮
    /// </summary>
    public class MyTabControl : TabControl
    {
        private int iconWidth = 16;
        private int iconHeight = 16;
        private Image icon = null;
        private Brush biaocolor = Brushes.Silver; //选项卡的背景色
        //private Form_paint father;//父窗口，即绘图界面，为的是当选项卡全关后调用父窗口的dispose事件关闭父窗口
        //private AxMicrosoft.Office.Interop.VisOcx.AxDrawingControl axDrawingControl1;
        public MyTabControl()
            : base()
        {
            //this.axDrawingControl1 = axDrawingControl;
            this.ItemSize = new Size(50, 25); //设置选项卡标签的大小,可改变高不可改变宽 
            //this.Appearance = TabAppearance.Buttons; //选项卡的显示模式
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            //icon = Properties.Resources.close.ToBitmap();
            iconWidth = icon.Width; iconHeight = icon.Height;
        }
        /// <summary>
        /// 设置父窗口
        /// </summary>
        /// <param name="fp">画图窗口</param>
        //public void setFather(Form_paint fp)
        //{
        //    this.father = fp;
        //}
        /// <summary>
        /// 重写的绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)//重写绘制事件。
        {
            Graphics g = e.Graphics;
            Rectangle r = GetTabRect(e.Index);
            if (e.Index == this.SelectedIndex)    //当前选中的Tab页，设置不同的样式以示选中
            {
                Brush selected_color = Brushes.Gold; //选中的项的背景色
                g.FillRectangle(selected_color, r); //改变选项卡标签的背景色
                string title = this.TabPages[e.Index].Text + "   ";
                g.DrawString(title, this.Font, new SolidBrush(Color.Black), new PointF(r.X + 3, r.Y + 6));//PointF选项卡标题的位置
                r.Offset(r.Width - iconWidth - 3, 2);
                g.DrawImage(icon, new Point(r.X - 2, r.Y + 2));//选项卡上的图标的位置 fntTab = new System.Drawing.Font(e.Font, FontStyle.Bold);
            }
            else//非选中的
            {
                g.FillRectangle(biaocolor, r); //改变选项卡标签的背景色
                string title = this.TabPages[e.Index].Text + "   ";
                g.DrawString(title, this.Font, new SolidBrush(Color.Black), new PointF(r.X + 3, r.Y + 6));//PointF选项卡标题的位置
                r.Offset(r.Width - iconWidth - 3, 2);
                g.DrawImage(icon, new Point(r.X - 2, r.Y + 2));//选项卡上的图标的位置
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            #region 左键判断是否在关闭区域
            if (e.Button == MouseButtons.Left)
            {
                Point p = e.Location;
                Rectangle r = GetTabRect(this.SelectedIndex);
                r.Offset(r.Width - iconWidth - 3, 2);
                r.Width = iconWidth;
                r.Height = iconHeight;
                if (r.Contains(p)) //点击特定区域时才发生
                {
                    string temp = this.SelectedTab.Text;
                    if (temp[temp.Length - 5] == '*')//有变化才保存
                    {
                        //确认是否保存VSD文档到ft_doc_Path
                        DialogResult response = MessageBox.Show("是否保存故障树" + this.SelectedTab.Name + "到图形文件", "请确认", MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (response == System.Windows.Forms.DialogResult.Yes)//确认保存
                        {
                            //axDrawingControl1.Document.SaveAs(GlobalVariables.ft_doc_Path + axDrawingControl1.Document.Title + ".vsd");//保存当前文档到文件夹
                            string datetime = DateTime.Now.ToString();//获取当前时间
                            //helpTool.saveVsdDB(axDrawingControl1.Document.Title, datetime);//保存vsd文档到数据库
                            //helpTool.setDatetimeToXml(axDrawingControl1.Document.Title, datetime);//如果信息已存在则将xml中的日期更新，如果不存在直接插入

                            this.SelectedTab.Text = temp.Substring(0, temp.Length - 5) + "   ";//保存后取消星号标志,还原为没变化的时候的样式
                        }
                        else if (response == System.Windows.Forms.DialogResult.Cancel)//点击取消或者关闭
                        {
                            return;//直接退出，撤销这次关闭程序的事件。
                        }
                    }
                    //if (this.TabCount == 1)//是最后一个选项卡，直接关闭父界面，即画图界面
                    //{
                    //    father.DisposeForTabControl(true);
                    //}
                    //else//不是最后一个
                    //{
                        this.TabPages.Remove(this.SelectedTab);
                    //}
                }
            }
            #endregion
            #region 右键 选中
            else if (e.Button == MouseButtons.Right)    //  右键选中
            {
                for (int i = 0; i < this.TabPages.Count; i++)
                {
                    TabPage tp = this.TabPages[i];
                    if (this.GetTabRect(i).Contains(new Point(e.X, e.Y)))
                    {
                        this.SelectedTab = tp;
                        break;
                    }
                }
            }
            #endregion
            #region 中键 选中 关闭
            else if (e.Button == MouseButtons.Middle)//鼠标中键关闭
            {
                for (int i = 0; i < this.TabPages.Count; i++)
                {
                    TabPage tp = this.TabPages[i];
                    if (this.GetTabRect(i).Contains(new Point(e.X, e.Y)))//找到后，关闭
                    {
                        this.SelectedTab = tp;
                        string temp = tp.Text;
                        if (temp[temp.Length - 5] == '*')//有变化才保存
                        {
                            //确认是否保存VSD文档到ft_doc_Path
                            DialogResult response = MessageBox.Show("是否保存故障树" + this.SelectedTab.Name + "到图形文件", "请确认", MessageBoxButtons.YesNoCancel,
                                             MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (response == System.Windows.Forms.DialogResult.Yes)//确认保存
                            {
                                //axDrawingControl1.Document.SaveAs(GlobalVariables.ft_doc_Path + axDrawingControl1.Document.Title + ".vsd");//保存当前文档到文件夹
                                string datetime = DateTime.Now.ToString();//获取当前时间
                                //helpTool.saveVsdDB(axDrawingControl1.Document.Title, datetime);//保存vsd文档到数据库
                                //helpTool.setDatetimeToXml(axDrawingControl1.Document.Title, datetime);//如果信息已存在则将xml中的日期更新，如果不存在直接插入

                                this.SelectedTab.Text = temp.Substring(0, temp.Length - 5) + "   ";//保存后取消星号标志,还原为没变化的时候的样式
                            }
                            else if (response == System.Windows.Forms.DialogResult.Cancel)//点击取消或者关闭
                            {
                                return;//直接退出，撤销这次关闭程序的事件。
                            }
                        }
                        //if (this.TabCount == 1)//是最后一个选项卡，直接关闭父界面，即画图界面
                        //{
                           // father.DisposeForTabControl(true);
                        //}
                        //else//不是最后一个
                        //{
                            this.TabPages.Remove(this.SelectedTab);
                        //}

                        break;
                    }
                }
            }
            #endregion
        }
    }
}

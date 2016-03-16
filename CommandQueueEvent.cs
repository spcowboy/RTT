using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Collections;

namespace RTT
{
    
        public class CommandQueueEvent : EventArgs
        {
            public readonly char KeyToRaiseEvent;
            
        }

        public class cmdQueueEventSender
        {
        //用event 关键字声明事件对象
            public delegate void QueueChangHandler(object sender, CommandQueueEvent e);
            public event QueueChangHandler QueueChanged;
            
            public void OnQueueChanged()
            {
                if (this.QueueChanged != null)//判断事件是否有处理函数 
                {
                    
                    this.QueueChanged(this, new CommandQueueEvent());
                }
            }
    }
        

        

    
}


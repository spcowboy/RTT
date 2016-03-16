using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace RTT
{
    public class LogManager
    {
        
        private static string logPath = string.Empty;
        public static bool dateTag = true;

        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = System.Environment.CurrentDirectory + @"\log\" ;
                    
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFielPrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix
        {
            get { return logFielPrefix; }
            set { logFielPrefix = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string msg)
        {
            try
            {
                if(logFile == LogFile.Log.ToString()|| logFile == LogFile.Command.ToString())
                {
                    System.IO.StreamWriter sw = System.IO.File.AppendText(
                    LogPath + LogFielPrefix + logFile + " " +
                    DateTime.Now.ToString("yyyyMMddHHMM") + ".Log"
                    );
                    if(dateTag)
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg);
                    else
                        sw.WriteLine(msg);
                    sw.Close();
                }
                else
                {
                    System.IO.StreamWriter sw = System.IO.File.AppendText(
                    LogPath + LogFielPrefix + LogFile.Trace.ToString() + " " +
                    DateTime.Now.ToString("yyyyMMddHHMM") + ".Log"
                    );
                    if (dateTag)
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + logFile +" - "+ msg);
                    else
                        sw.WriteLine(logFile + " - " + msg);
                    sw.Close();
                }
                
            }
            catch
            { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(LogFile logFile, string msg)
        {
               //do not print debug log
             WriteLog(logFile.ToString(), msg);
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Log,
        Trace,
        Warning,
        Debug,
        Error,
        Command
    }
}

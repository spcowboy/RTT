using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Data;

namespace RTT
{
    enum Verbs
    {
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255
    }

    enum Options
    {
        SGA = 3
    }

    class Tcpclient
    {
        TcpClient tcpSocket = null;
        StringBuilder sb = new StringBuilder();
        int TimeOutMs = 500;
        byte[] m_byBuff = new byte[100000];
        public ManualResetEvent connectDone = new ManualResetEvent(false);

        public Tcpclient(String Hostname, int Port, string username = "", string pw = "")
        {
            tcpSocket = new TcpClient(Hostname, Port);


            //connectDone.Reset();
            //异步回调
            //AsyncCallback recieveData = new AsyncCallback(OnRecievedData);
            //Task t = await TcpClient.ConnectAsync(Hostname, Port);

            // Wait here until the callback processes the connection.
            //connectDone.WaitOne();
        }



        //public async Task<string> Login(string Username, string Password, int LoginTimeOutMs)
        //{
        //    int oldTimeOutMs = TimeOutMs;
        //    TimeOutMs = LoginTimeOutMs;
        //    string s = await Read();
        //    //if (!s.TrimEnd().EndsWith(":"))
        //    //throw new Exception("Failed to connect : no login prompt");
        //    //WriteLine(Username);

        //    //s += Read();
        //    //if (!s.TrimEnd().EndsWith(":"))
        //    //throw new Exception("Failed to connect : no password prompt");
        //    //WriteLine(Password);

        //    //s += Read();
        //    TimeOutMs = oldTimeOutMs;

        //    return s;
        //}

        public void DisConnect()
        {
            if (tcpSocket != null)
            {
                if (tcpSocket.Connected)
                {
                    tcpSocket.Client.Disconnect(true);
                }
            }
        }

        public void WriteLine(string cmd)
        {
            Write(cmd + "\r\n");
        }

        public void Write(string cmd)
        {
            if (!tcpSocket.Connected) return;
            byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));
            tcpSocket.GetStream().Write(buf, 0, buf.Length);
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: writeline ")); 
        }

        //public async Task<string> Read()
        //{
        //    if (!tcpSocket.Connected) return null;
        //    sb.Clear();
        //    do
        //    {
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: start read "));
        //        ParseTelnet(sb);
        //        System.Threading.Thread.Sleep(TimeOutMs);
        //    } while (tcpSocket.Available > 0);
        //    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: finish read "));
        //    //Console.Write(ConvertToGB2312(sb.ToString()));
        //    return ConvertToGB2312(sb.ToString());
        //}

        public bool IsConnected
        {
            get { return tcpSocket.Connected; }
        }

        void ParseTelnet(StringBuilder sb)
        {
            while (tcpSocket.Available > 0)
            {
                int input = tcpSocket.GetStream().ReadByte();
                switch (input)
                {
                    case -1:
                        break;
                    case (int)Verbs.IAC:
                        // interpret as command  
                        int inputverb = tcpSocket.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int)Verbs.IAC:
                                //literal IAC = 255 escaped, so append char 255 to string  
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.DO:
                            case (int)Verbs.DONT:
                            case (int)Verbs.WILL:
                            case (int)Verbs.WONT:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)  
                                int inputoption = tcpSocket.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                if (inputoption == (int)Options.SGA)
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                                else
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                                tcpSocket.GetStream().WriteByte((byte)inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append((char)input);
                        break;
                }
                //sb.Append(tcpSocket.GetStream().Read(m_byBuff, 0, m_byBuff.Length));
                //break;
            }
        }

        private string ConvertToGB2312(string str_origin)
        {
            char[] chars = str_origin.ToCharArray();
            byte[] bytes = new byte[chars.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                int c = (int)chars[i];
                bytes[i] = (byte)c;
            }
            Encoding Encoding_GB2312 = Encoding.GetEncoding("GB2312");
            string str_converted = Encoding_GB2312.GetString(bytes);
            return str_converted;
        }
    }

}

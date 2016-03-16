using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace RTT
{
    class Constant
    {
        //config appsetting name
        public const string CONFIG_TS_LOAD_PATH = "ts_loadpath";
        public const string CONFIG_TS_SAVE_PATH = "ts_savepath";

        //command prefix string
        public const string PRIFIX_SA = "SA.Command";
        public const string PRIFIX_SG = "SG.Command";
        public const string PRIFIX_SG2 = "SG2.Command";
        public const string PRIFIX_IS1 = "ISG1.Command";
        public const string PRIFIX_IS2 = "ISG2.Command";
        public const string PRIFIX_RFBOX = "RF-Box.";
        public const string PRIFIX_RFBOX2 = "RF-Box2.";
        public const string PRIFIX_DC5767A = "DC5767A.";
        public const string PRIFIX_RUMASTER = "Rumaster.";


        //Instument name                            
        public const string DEVICE_NAME_RRU = "RRU";
        public const string DEVICE_NAME_SIGNALANALYZER = "SIGNALANALYZER";
        public const string DEVICE_NAME_SIGNALGENERATOR = "SIGNALGENERATOR";
        public const string DEVICE_NAME_SIGNALGENERATOR2 = "SIGNALGENERATOR2";
        public const string DEVICE_NAME_RFBOX = "RFBOX";
        public const string DEVICE_NAME_RFBOX2 = "RFBOX2";
        public const string DEVICE_NAME_AGILENT5767A = "DC5767A";
        public const string DEVICE_NAME_RuMaster = "RuMaster";
        public const string DEVICE_NAME_INTERFERENCE_SIGNAL_1 = "INTERFERENCE_SIGNAL_1";
        public const string DEVICE_NAME_INTERFERENCE_SIGNAL_2 = "INTERFERENCE_SIGNAL_2";
        public const string DEVICE_NAME_DC_DH1716A = "DC1716A";

        // Baudrate list
        public static string[] BAUD_RATES = { "2400", "9600", "19200", "38400", "57600", "115200", "230400", "460800", "921600" };
        //Data Bits
        public static string[] DATA_BITS = { "4", "5", "6", "7", "8" };
        //Parity
        public static string[] PARITY = { "None", "Odd", "Even", "1", "0" };
        //Stop Bits
        public static string[] STOP_BITS = { "1", "1.5", "2" };
        //send device list
        public static string[] DEVICE_LIST = { "RRU", "SA", "SG1", "SG2", "RFBOX1", "RFBOX2", "DC5767A", "ISG1", "ISG2", "SERIAL2", "RUMASTER" };

        //Visadevice list
        public static string[] VISADEVICE_LIST = { "SA", "SG1", "SG2", "RFBOX1", "RFBOX2", "DC5767A", "ISG1", "ISG2" };
        public enum VISADEVICE_ENUM
        {
            SA = 1,
            SG = 2,
            SG2 = 3,
            RFBOX1 = 4,
            RFBOX2 = 5,
            DC5767A = 6,
            ISG1 = 7,
            ISG2 = 8

        }
        //socket server port
        public const int HOSTPORT = 8001;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using ConfigurationTool;

namespace SLXW
{
    public class SysPrm
    {

        public static int m_Port1 = 1;
        public static int m_Port2 = 2;
        public static int m_Port3 = 3;
        public static int m_Port4 = 4;



  
        public static void ReadPrm()
        {
      

            m_Port1 = Configure.ReadConfig("Sys", "m_Port1 ", 0);
            m_Port2 = Configure.ReadConfig("Sys", "m_Port2 ", 0);
            m_Port3 = Configure.ReadConfig("Sys", "m_Port3 ", 0);
            m_Port4 = Configure.ReadConfig("Sys", "m_Port4 ", 0);
 



        }
        public static void WritePrm()
        {


            Configure.WriteConfig("Sys", "m_Port1 ", m_Port1);
            Configure.WriteConfig("Sys", "m_Port2 ", m_Port2);
            Configure.WriteConfig("Sys", "m_Port3 ", m_Port3);
            Configure.WriteConfig("Sys", "m_Port4 ", m_Port4);

        }

    }
}

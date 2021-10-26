using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using Lib.DB;
using System.IO;

namespace HGApp
{
    internal class Barcode
    {
        private Barcode() { }

        private static Barcode _Instance;
        private static readonly object locker = new object();

        public string m_sYear = "";
        public string m_sDay = "";
        public string m_sSerial = "";
        public string m_sBarcode = "";
        public string m_sVendorNo = "";
        public string m_sPartNo = "";
        public string m_sModelName = "";

        public static Barcode GetInstance()
        {
            if (_Instance == null)
            {
                lock (locker)
                {
                    if (_Instance == null)
                    {
                        _Instance = new Barcode();
                    }
                }
            }
            return _Instance; 
        }
        public string ModeName()
        {
            CCfg cfg = new CCfg();

            m_sModelName = Path.GetFileNameWithoutExtension(cfg.m_sModelPath).Replace(".ezd", "");
            return m_sModelName;
        }
        public string Year()
        {
            string sYear = DateTime.Now.Year.ToString();
            m_sYear = sYear.Substring(sYear.Length - 2, 2);
            return m_sYear;
        }

        public string Day()
        {
            TimeSpan tmSpan = DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1);
            m_sDay = string.Format("{0:D3}", tmSpan.Days + 1);

            CCfg cfg = new CCfg();
            if (cfg.m_sDya != m_sDay)
            {
                cfg.m_sDya = m_sDay;
                cfg.m_nSerial = 1;
                cfg.SaveToFile();
            }
            return m_sDay;
        }

        public string Serial(bool bAdd=true)
        {
            CCfg cfg = new CCfg();
            m_sSerial = string.Format("{0:D4}", cfg.m_nSerial);
            if(bAdd)
            {
                cfg.m_nSerial++;
                cfg.SaveToFile();
            }
          
            return m_sSerial;
        }
        public void SetSerial(int nNumber)
        {
            CCfg cfg = new CCfg();
            cfg.m_nSerial= nNumber;
            m_sSerial = string.Format("{0:D4}", cfg.m_nSerial);
            cfg.SaveToFile();   
        }

        public bool GetBarcode(ref string sBarcode,bool bAddSerial = true)
        {
            CCfg cfg = new CCfg();

            if (cfg.GetVendorNo(ref m_sVendorNo) && cfg.GetPartNo(ref m_sPartNo))
            {
                // P05458472S180020001V31158
                m_sBarcode = "P" + m_sPartNo + "S" + Year() + Day() + Serial(bAddSerial) + "V" + m_sVendorNo;
                sBarcode = m_sBarcode;
                return true;
            }
            return false;
        }
    }
}

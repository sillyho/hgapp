using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using Lib.DB;

namespace HGApp
{
    internal class CCfg : CCfgMgr
    {
        public CCfg(bool bLoadNow = true)
            : base("HGLaser.cfg")
        {
            if (bLoadNow)
            { LoadFromFile(); }
        }

        public override void LoadFromFile()
        {
            _LoadFromFileInternal(this);
        }

        public override void SaveToFile()
        {
            _SaveToFileInternal(this);
        }

        public string m_sModelPath = "";
        public string m_sVendorNo = "";
        public string m_sPartNo = "";
        public int m_nPartNoLength = 8;
        public int m_nSerial = 1;
        public string m_sDya = "0";
        public string m_sPassWord = "";

        public int m_MarkOutput = 0;
        public int m_MarkFinished = 1;
        public int m_MarkFinishedPluse = 200;
        public bool m_ScanBar = false;

        public bool GetVendorNo(ref string sVendor)
        {


            sVendor = m_sVendorNo;
            
            return true;
        }

        public bool GetPartNo(ref string sPartNo)
        {
            if (m_sPartNo.Length > m_nPartNoLength)
            {
                Log.Instance().AddDetails("零件号代号长度大于设定长度。", false);
                return false;
            }

            sPartNo = m_sPartNo.PadLeft(m_nPartNoLength, '0');
            return true;
        }


        [CategoryAttribute("Ezcad")]
        public string 模板路径
        {
            get { return m_sModelPath; }
            set { m_sModelPath = value; }
        }

        [CategoryAttribute("Barcode")]
        public string 供应商代号
        {
            get { return m_sVendorNo; }
            set { m_sVendorNo = value; }
        }

        [CategoryAttribute("Barcode")]
        public string 零件号
        {
            get { return m_sPartNo; }
            set { m_sPartNo = value; }
        }

        [CategoryAttribute("Barcode")]
        public int 零件号长度
        {
            get { return m_nPartNoLength; }
            set { m_nPartNoLength = value; }
        }
        [CategoryAttribute("IO")]
        public int 标刻中输出
        {
            get { return m_MarkOutput; }
            set { m_MarkOutput = value; }
        }
         [CategoryAttribute("IO")]
        public int 标刻完成输出
        {
            get { return m_MarkFinished; }
            set { m_MarkFinished = value; }
        }
         [CategoryAttribute("IO")]
        public int 完成输出脉宽
        {
            get { return m_MarkFinishedPluse; }
            set { m_MarkFinishedPluse = value; }
        }
         public bool 完成后扫码
         {
             get { return m_ScanBar; }
             set { m_ScanBar = value; }
         }

        private string sPath = null;
        private UIFilenameEditor.FileDialogType eDialogType = UIFilenameEditor.FileDialogType.LoadFileDialog;
        private bool bUseFilePath = false;

        private void BuildAttributes_FilePath()
        {
            ArrayList attrs = new ArrayList();
            UIFilenameEditor.FileDialogFilterAttribute FilterAttribute = new UIFilenameEditor.FileDialogFilterAttribute(sPath);
            UIFilenameEditor.SaveFileAttribute SaveDialogAttribute = new UIFilenameEditor.SaveFileAttribute();
            Attribute[] attrArray;
            attrs.Add(FilterAttribute);
            if (eDialogType == UIFilenameEditor.FileDialogType.SaveFileDialog)
            {
                attrs.Add(SaveDialogAttribute);
            }
            attrArray = (System.Attribute[])attrs.ToArray(typeof(Attribute));
            oCustomAttributes = new AttributeCollection(attrArray);
        }

        [NonSerialized()]
        protected AttributeCollection oCustomAttributes = null;

    }

}
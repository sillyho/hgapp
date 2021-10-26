using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Lib;
using Lib.EzCad;
using SYS_DB;
using SLXW;
using ConfigurationTool;
namespace HGApp
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
        }
        public bool IsStopRunning
        {
            get { return m_bStopRun; }
        }
        private bool m_bStopRun = true;
        public bool m_bManualRun = false;
        private string _sBarcode = "";
        private bool m_bLock = false;

        System.Windows.Forms.Timer timerUpdate = new System.Windows.Forms.Timer();


        private Dictionary<string, string> m_Dict = new Dictionary<string, string>();
        private void FormHome_Load(object sender, EventArgs e)
        {
            MarkEzdEx.InitialCard(Handle, true);
            MarkEzdEx.PictureBoxPreview = PictureBoxPrev;
            Log.Instance().AddDetails("Ezcad 初始化完成", true);
            ModelFileExsit();

            InitEzd();



            timerUpdate.Tick += new EventHandler(UpdateBarcodeInfo);
            timerUpdate.Enabled = true;
            timerUpdate.Interval = 1000;

            this.ActiveControl = textBarcode;
            textBarcode.KeyUp += new KeyEventHandler(FormHome_KeyUp);

        }
        private void InitEzd()
        {
            CCfg cfg = new CCfg();
            if(MarkEzdEx.LoadEzdFile(cfg.m_sModelPath, false)!=Lmc1ErrCode.LMC1_ERR_SUCCESS)
            {
                Log.Instance().AddDetails("加载模板失败:"+ cfg.m_sModelPath, false);

                return;
            }
            
            MarkEzdEx.ShowPreview();
            string strmodename = Path.GetFileNameWithoutExtension(cfg.m_sModelPath).Replace(".ezd", "");
            ChangeSerial(strmodename);
            cfg = new CCfg();
            string strCode = MarkEzdEx.GetTextByName("BARCODE");
            if (strCode.Length != 25)
            {
                Log.Instance().AddDetails("模板中Code对象长度不为25", false);
                return ;
            }

            //供应商代号
            string strvendor = strCode.Substring(20, 5);

            //零件代号
            string strpartcode = strCode.Substring(1, 8);
           
            this.Invoke((EventHandler)(delegate
            {
                LabelVendor.Text = strvendor;
                LabelPart.Text = strpartcode;
                label_file.Text = strmodename;
            }));
            cfg.m_sPartNo = strpartcode;
            cfg.m_sVendorNo = strvendor;
            cfg.m_nPartNoLength = 8;
            cfg.SaveToFile();
            Barcode.GetInstance().m_sModelName = strmodename;
            Barcode.GetInstance().m_sPartNo = strpartcode;
            Barcode.GetInstance().m_sVendorNo = strvendor;
  
            Barcode.GetInstance().GetBarcode(ref strCode,false);
            this.Invoke((EventHandler)(delegate
            {
                LabelSerial.Text = Barcode.GetInstance().m_sSerial;
                LabelYear.Text = Barcode.GetInstance().m_sYear;
                LabelDay.Text = Barcode.GetInstance().m_sDay;
                LabelBarcode.Text = strCode;
            }));

        }
        private int m_MarkingIO = 0;
        private int m_MarkFinish = 0;
        private int m_MarkFinishPluse = 0;
        private bool m_bScanBar = false;
        private bool ModelFileExsit()
        {
            CCfg cfg = new CCfg();
            if (!File.Exists(cfg.m_sModelPath))
            {
                string sInfo = string.Format("模板文件{0}不存在!", cfg.m_sModelPath);
                Log.Instance().AddDetails(sInfo, false);
                return false;
            }
            return true;
        }
        public bool Start()
        {
            m_Dict.Clear();

            DataTable dt = new DataTable();
            if (!AccessHelper.ReadData(String.Format("select * from io_model "), ref dt))
            {
                Log.Instance().AddDetails("查询数据库失败!", false);
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                m_Dict.Add(dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString());
            }
            if (m_Dict.Count <= 0)
            {
                Log.Instance().AddDetails("请先配置IO模板!", false);
                return false;
            }

            CCfg cfg = new CCfg();
            cfg.LoadFromFile();

            m_MarkingIO = cfg.标刻中输出;
            m_MarkFinish = cfg.标刻完成输出;
            m_MarkFinishPluse = cfg.完成输出脉宽;
            m_bScanBar = cfg.完成后扫码;

            m_bStopRun = false;
            new Thread(WorkThread) { IsBackground = true }.Start();
            Log.Instance().AddDetails("系统开始等待工作", true);
            return true;


        }

        public void Stop()
        {
            m_bStopRun = true;
        }

        private void WorkThread()
        {
            int[] nStatus = new int[16];
            string strCurNumStr;

            string strTest = "";
            while (!m_bStopRun)
            {
                strTest = "";
                if (MarkEzdEx.ReadPort(ref nStatus)!=Lmc1ErrCode.LMC1_ERR_SUCCESS)
                {
                    Log.Instance().AddDetails("读取板卡IO失败!", false);
                    break;
                }
                for(int i=0;i<16;i++)
                {
                    strTest += nStatus[i].ToString();
                }
                //

                this.Invoke((EventHandler)(delegate
                {
                    label9.Text = strTest;
                }));
                if (nStatus[4]!=0)//打标端口为4  固定
                {
                    AutoWork();
                }
                else
                {
                    strCurNumStr = String.Format("{0}{1}{2}{3}", nStatus[SysPrm.m_Port1], nStatus[SysPrm.m_Port2], nStatus[SysPrm.m_Port3], nStatus[SysPrm.m_Port4]);
          
                    if (m_Dict.Keys.Contains(strCurNumStr))
                    {
                        Log.Instance().AddDetails(String.Format("端口信息:{0}对应模板{1}:", strCurNumStr, m_Dict[strCurNumStr]), true);

              
                        this.Invoke((EventHandler)(delegate
                        {
                            FormChangeEzd fce = new FormChangeEzd("收到模板切换信号，是否将模板切换到:" + m_Dict[strCurNumStr]);
                            if(fce.ShowDialog()==DialogResult.OK)
                            {
                                ChangeEzd(m_Dict[strCurNumStr]);
                            }
                        }));

                    }
                }
               
                Thread.Sleep(80);
            }
        }
     
        
        private bool ChangeEzd(string strEzd)
        {

            if ( MarkEzdEx.LoadEzdFile(strEzd, false)!= Lmc1ErrCode.LMC1_ERR_SUCCESS)
            {
                Log.Instance().AddDetails("加载模板失败:"+strEzd, false);
                return false;
            }
            MarkEzdEx.ShowPreview(PictureBoxPrev);

            string strCode = MarkEzdEx.GetTextByName("BARCODE");
        
            if (strCode.Length!=25)
            {
                Log.Instance().AddDetails("模板中BARCODE对象长度不为25", false);
                return false;
                //P03779762212978888V31158
            }
            //供应商代号
            string strvendor = strCode.Substring(20, 5);
         
            //零件代号
            string strpartcode = strCode.Substring(1, 8);
            string strmodename = Path.GetFileNameWithoutExtension(strEzd).Replace(".ezd", "");
            this.Invoke((EventHandler)(delegate
            {
                LabelVendor.Text = strvendor;
                LabelPart.Text = strpartcode;
                label_file.Text = strmodename;
            }));

            CCfg cfg = new CCfg();
            cfg.m_sPartNo = strpartcode;
            
            cfg.m_sVendorNo = strvendor;
            cfg.m_sModelPath = strEzd;
            
            cfg.m_nPartNoLength = 8;

            cfg.SaveToFile();
            Barcode.GetInstance().m_sModelName = strmodename;
            Barcode.GetInstance().m_sPartNo = strpartcode;
            Barcode.GetInstance().m_sVendorNo = strvendor;

            //初始化序列号
            ChangeSerial(strmodename);
            Barcode.GetInstance().GetBarcode(ref strCode,false);
            this.Invoke((EventHandler)(delegate
            {
                LabelSerial.Text = Barcode.GetInstance().m_sSerial;
                LabelYear.Text = Barcode.GetInstance().m_sYear;
                LabelDay.Text = Barcode.GetInstance().m_sDay;
                LabelBarcode.Text = strCode;
            }));

            return true;
        }
        private void UpdateSerial(string strmodelName,int nSerial)
        {
            Configure.WriteConfig("NUMBER", strmodelName, nSerial);
        }
        private void ChangeSerial(string strmodelName)
        {
            string strSaveTime = Configure.ReadConfig("SERIAL", "TIME", "");
            string strCurTime = DateTime.Now.ToString("yyyyMMdd");
            if(strSaveTime!=strCurTime)
            {
                Log.Instance().AddDetails("日期改变，序列号重置", false);
                Configure.WriteConfig("SERIAL", "TIME", strCurTime);

                Barcode.GetInstance().SetSerial(1);
                //配置文件全部写成1
                Dictionary<string, string> Dict = Configure.ReadKeys("NUMBER");
                foreach(KeyValuePair<string,string>kp in Dict)
                {
                    Configure.WriteConfig("NUMBER", kp.Key, 1);
                }
                return;
            }
            int nOld = Configure.ReadConfig("NUMBER", strmodelName, 0);
            if(nOld ==0)//没有改项
            {
                Barcode.GetInstance().SetSerial(1);
                Configure.WriteConfig("NUMBER", strmodelName, 1);
            }
            else
            {
                Barcode.GetInstance().SetSerial(nOld);
  

            }
        }
        private void Run()
        {
            var fm = new FormThread("正在执行");
            fm.ShowDialog(AutoWork);
        }

        private void AutoWork()
        {
            // 1. 等待开始信号
            m_bManualRun = false;
            Log.Instance().AddDetails("接收到开始信号", true);

            // 2.打标
            if (!Mark())
            {
                Log.Instance().AddDetails("打标失败！", false);
                return;
            }
            if (m_bScanBar)
            {
                // 3.扫码
                string sBarcode = FormInput.GetInput("请扫描二维码", "", false);
                if (sBarcode != _sBarcode)
                {
                    FormUser frm = new FormUser();
                    if (sBarcode == "")
                    {
                        Log.Instance().AddDetails("二维码无法识别", false);
                        frm.ShowDialog();
                        return;
                    }
                    Log.Instance().AddDetails("打标二维码和扫描二维码不同", false);
                    frm.ShowDialog();
                    return;
                }

                Log.Instance().AddDetails("二维码扫描OK", true);
            }
            
            return;
        }

        public bool Mark()
        {
            CCfg cfg = new CCfg();
            string sPartNo = cfg.m_sPartNo;

            this.Invoke((EventHandler)(delegate
            {
                LabelSerial.Text = Barcode.GetInstance().Serial(false);

            }));

            //1.加载打标模板
            if (Lmc1ErrCode.LMC1_ERR_SUCCESS != MarkEzdEx.LoadEzdFile(cfg.m_sModelPath, false))
            {
                Log.Instance().AddDetails("Ezcad 模板加载失败！", false);
                return false;
            }

            // 2. 修改实体内容
            bool bRtn = Barcode.GetInstance().GetBarcode(ref _sBarcode);
            if(!bRtn)
            {
                Log.Instance().AddDetails("生成二维码失败！", false);
                return false;
            }
            this.Invoke((EventHandler)(delegate
            {
                LabelBarcode.Text = _sBarcode;

            }));
            string sMsg = string.Format("打标的二维码内容为：{0}", _sBarcode);
            Log.Instance().AddDetails(sMsg, true);
            cfg = new CCfg();
            UpdateSerial(Path.GetFileNameWithoutExtension(cfg.m_sModelPath).Replace(".ezd", ""), cfg.m_nSerial);

            if (Lmc1ErrCode.LMC1_ERR_SUCCESS != MarkEzdEx.ChangeTextByName("BARCODE", _sBarcode))
            {
                Log.Instance().AddDetails("Ezcad 修改实体内容失败！", false);
                return false;
            }
            Log.Instance().AddDetails(String.Format("替换文本完成"), true);

            MarkEzdEx.ShowPreview();
            Log.Instance().AddDetails(String.Format("开始标刻"), true);

            MarkEzdEx.OutputPort(m_MarkingIO, true);
            //// 3. 打标实体
            if (Lmc1ErrCode.LMC1_ERR_SUCCESS != MarkEzdEx.Mark())
            {
                Log.Instance().AddDetails("Ezcad 打标失败！", false);
                MarkEzdEx.OutputPort(m_MarkingIO, false);
                MarkEzdEx.OutputPort(m_MarkFinish, true);
                Thread.Sleep(m_MarkFinishPluse);
                MarkEzdEx.OutputPort(m_MarkFinish, false);

                return false;
            }
            MarkEzdEx.OutputPort(m_MarkingIO, false);

            MarkEzdEx.OutputPort(m_MarkFinish, true);
            Thread.Sleep(m_MarkFinishPluse);
            MarkEzdEx.OutputPort(m_MarkFinish, false);
            Log.Instance().AddDetails(String.Format("标刻完成"), true);

            return true;
        }

        private void UpdateBarcodeInfo(object sender, EventArgs e)
        {
            timerUpdate.Enabled = false;
            //LabelVendor.Text = Barcode.GetInstance().m_sVendorNo;
            //LabelPart.Text = Barcode.GetInstance().m_sPartNo;
            //LabelYear.Text = Barcode.GetInstance().m_sYear;
            //LabelDay.Text = Barcode.GetInstance().m_sDay;
            //LabelSerial.Text = Barcode.GetInstance().m_sSerial;
            //LabelBarcode.Text = Barcode.GetInstance().m_sBarcode;
            //label_file.Text = Barcode.GetInstance().ModeName();
            timerUpdate.Enabled = true;
            textBarcode.Focus();
        }

        private void FormHome_Login()
        {
            m_bLock = true;
            FormUser frm = new FormUser();
            DialogResult r = frm.ShowDialog();
            if (r == DialogResult.OK)
            {
                m_bLock = false;
            }
        }

        private void FormHome_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender == textBarcode && e.KeyValue == 13)     // 换行
            {
                string sBarcode = textBarcode.Text.Replace("\r\n", "");
                if (sBarcode == "")
                    return;

                CCfg cfg = new CCfg();
                if (!sBarcode.Contains(cfg.m_sPartNo))
                {
                    Log.Instance().AddDetails("产品类型错误，件号不匹配。", false);
                    MessageBox.Show("该产品类型错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBarcode.Text = "";
                    FormHome_Login(); 
                }
                else
                {
                    textBarcode.Text = "";
                    Log.Instance().AddDetails("产品类型正确！", true);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeEzd(@"C:\Users\Administrator\Desktop\Untitled.ezd");
        }
    }
}

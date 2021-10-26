using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lib;
using Lib.EzCad;
using SLXW;
using SYS_DB;

namespace HGApp
{
    public partial class FormMain : Form
    {
        private FormHome m_formHome = new FormHome();
        private List<Form> m_listForm = new List<Form>();

        public FormMain()
        {
            InitializeComponent();
            Log.Instance().WriteToFile = true;
        }

        private void SwitchForm(Form form)
        {
            foreach (Form f in m_listForm)
            {
                if (f == form)
                    f.Show();
                else 
                    f.Hide();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if(!AccessHelper.ConnectToDatabase())
            {
                MessageBox.Show("连接本地数据库data.mdb失败!",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SysPrm.ReadPrm();

            Log.Instance().TopLevel = false;
            Log.Instance().FormBorderStyle = FormBorderStyle.None;
            Log.Instance().Dock = DockStyle.Fill;
            PanelLog.Controls.Clear();
            PanelLog.Controls.Add(Log.Instance());
            Log.Instance().Show();

            m_listForm.Clear();
            m_listForm.Add(m_formHome);
            foreach (Form f in m_listForm)
            {
                f.TopLevel = false;
                PanelWnd.Controls.Add(f);
                f.Show();
            }

            SwitchForm(m_formHome);
            BtnRun.Enabled = true;
            BtnSetParam.Enabled = true;
            BtnStop.Enabled = false;

            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LabelClock.Text = DateTime.Now.ToString();
        }

        private void BtnSetParam_Click(object sender, EventArgs e)
        {
            FormUser frm = new FormUser();
            DialogResult r = frm.ShowDialog();
            if (r == DialogResult.OK)
            {
                FormSetting form = new FormSetting();
                form.ShowDialog();
            }
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            m_formHome.Start();
            BtnRun.Enabled = false;
            BtnSetParam.Enabled = false;
            BtnStop.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            m_formHome.Stop();
            BtnRun.Enabled = true;
            BtnSetParam.Enabled = true;
            BtnStop.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_formHome.m_bManualRun = true;
            string sBarcode = "";
            Barcode.GetInstance().GetBarcode(ref sBarcode);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormUser frm = new FormUser();
            frm.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CCfg cfg = new CCfg();
            string sPartNo = "";
            cfg.GetPartNo(ref sPartNo);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //string sbarcode = "";
            //Barcode.GetInstance().GetBarcode(ref sbarcode);
            m_formHome.Mark();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!m_formHome.IsStopRunning)
            {
                MessageBox.Show("正在运行中，请停止后再关闭程序!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Cancel = true;
            }
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using Lib;
using Lib.DB;

namespace HGApp
{
    public partial class Log : Form
    {
        private object _lockObj = new object();
        private bool _bWriteToFile = false;
        private bool _HideWhenClose = true;

        public static Log _Log;
        private static readonly object locker = new object();

        private Log() { InitializeComponent();  }

        public static Log Instance()
        {
            if (_Log == null)
            {
                lock (locker)
                {
                    if (_Log == null)
                    {
                        _Log = new Log();
                    }
                }
            }
            return _Log;
        }

        public bool HideWhenClose
        {
            get { return _HideWhenClose; }
            set { _HideWhenClose = value; }
        }

        /// <summary>
        /// 是否写入文件
        /// </summary>
        public bool WriteToFile
        {
            get { return _bWriteToFile; }
            set { _bWriteToFile = value; }
        }

        /*
        public FormLog()
        {
            //InitializeComponent();
            //ClientSize = new Size(SystemInformation.WorkingArea.Width, 142);
            //Location = new System.Drawing.Point(0, SystemInformation.WorkingArea.Height - Height);
            //listView_Details.Location = new Point(0, 0);
            //listView_Details.Size = ClientSize;
        }
         * */

        public void AddDetails(string strDetails, bool bSuccess)
        {
            if (bSuccess)
            {
                AddDetails(strDetails, Color.Green);
            }
            else
            {
                AddDetails(strDetails, Color.Red);
            }
        }

        private void _add(string strDetails, Color clr)
        {
            if (WriteToFile)
            {
                /*Lib.DB.*/simpleLog log = new /*DB.*/simpleLog("details", ".txt");
                log.Write(strDetails);
            }

            ListViewItem lvi = new ListViewItem();
            lvi.ForeColor = clr;
            lvi.Text = DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("000");
            lvi.SubItems.Add(strDetails);

            listView_Details.Items.Add(lvi);
            lvi.EnsureVisible();
            if (listView_Details.Items.Count > 10000)
            {
                listView_Details.Items.Clear();
            }
        }

        public void AddDetails(string strDetails, Color clr)
        {
            lock (_lockObj)
            {
                if (listView_Details.InvokeRequired)
                {
                    listView_Details.Invoke((EventHandler)delegate
                    {
                        _add(strDetails, clr);
                    });
                }
                else
                {
                    _add(strDetails, clr);
                }
            }
        }

        private void FormDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = HideWhenClose;
        }

        private void FormLog_Load(object sender, EventArgs e)
        {

        }
    }
}
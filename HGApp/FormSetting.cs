using Lib.EzCad;
using MarkEzd;
using SLXW;
using SYS_DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HGApp
{
    public partial class FormSetting : DevComponents.DotNetBar.Office2007Form
    {
        private CCfg _cfg = new CCfg();

        public FormSetting()
        {
            InitializeComponent();

            propertyGrid1.PropertySort = PropertySort.Categorized;

            _cfg.LoadFromFile();
            propertyGrid1.SelectedObject = _cfg;
        }
        private bool InitDb()
        {

            dataGridView1.Rows.Clear();
            DataTable dt = new DataTable();
            if (!AccessHelper.ReadData(String.Format("select * from io_model"), ref dt))
            {
                return false;
            }

            foreach (DataRow dr in dt.Rows)
            {
                int nIndedx = dataGridView1.Rows.Add();
                dataGridView1.Rows[nIndedx].Cells[0].Value = dr.ItemArray[0].ToString();
                dataGridView1.Rows[nIndedx].Cells[1].Value = dr.ItemArray[1].ToString(); ;
            }
            return true;

        }
        private void FormSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            _cfg.SaveToFile();
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {

            SysPrm.ReadPrm();

            checkBox1.Text = "端口" + SysPrm.m_Port1.ToString();
            checkBox2.Text = "端口" + SysPrm.m_Port2.ToString();
            checkBox3.Text = "端口" + SysPrm.m_Port3.ToString();
            checkBox4.Text = "端口" + SysPrm.m_Port4.ToString();

            InitDb();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;
            string strDelete = String.Format("delete * from io_model where 数值='{0}'",
                dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            if (AccessHelper.DeleteData(strDelete))
            {
                InitDb();
            }
        }

        private void 标刻ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;

            string strMark = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

            new Thread(() =>
            {
                MarkEzdEx.LoadEzdFile(strMark, false);
                MarkEzdEx.ShowPreview(pictureBox1);
                MarkEzdEx.Mark();
   

            }).Start();
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = null;
            openFileDialog.Filter = "ezd文件|*.ezd|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
                string str = textBox1.Text;
                MarkEzdEx.LoadEzdFile( str,false);
                MarkEzdEx.ShowPreview(pictureBox1);
            }
        }

        private void button_edit_Click(object sender, EventArgs e)
        {

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("打标文件不存在", "重复", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            string strNumStr = String.Format("{0}{1}{2}{3}", checkBox1.Checked ? "1" : "0",
               checkBox2.Checked ? "1" : "0", checkBox3.Checked ? "1" : "0", checkBox4.Checked ? "1" : "0");
            if(strNumStr=="0000")
            {
                MessageBox.Show("请设置IO", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            string strSql = String.Format("select * from io_model where 数值='{0}'", strNumStr);
            DataTable dt = new DataTable();
            if (!AccessHelper.ReadData(strSql, ref dt))
            {
                return;
            }
            if (dt.Rows.Count > 0)
            {
                if (MessageBox.Show("已经存在该IO配置模板,是否覆盖?", "重复", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                {
                    return;
                }
                AccessHelper.UpdateData(String.Format("update io_model set 模板='{0}' where 数值='{1}'", textBox1.Text, strNumStr));
            }
            else
            {

                AccessHelper.InsertData(String.Format("insert into io_model values('{0}','{1}')", strNumStr, textBox1.Text));
            }
            InitDb();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;
            string strnumStr = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            checkBox1.Checked = strnumStr.Substring(0, 1) == "1";
            checkBox2.Checked = strnumStr.Substring(1, 1) == "1";
            checkBox3.Checked = strnumStr.Substring(2, 1) == "1";
            checkBox4.Checked = strnumStr.Substring(3, 1) == "1";
   
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string strEzd = textBox1.Text;
            MarkEzdEx.LoadEzdFile( strEzd,false);
            MarkEzdEx.ShowPreview(pictureBox1);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HGApp
{
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
        }

        private bool m_bEditPsw = false;

        private void btnOK_Click(object sender, EventArgs e)
        {
             CCfg cfg = new CCfg();
            if (m_bEditPsw)
            {
                m_bEditPsw = false;
                cfg.m_sPassWord = textPsw2.Text;
                cfg.SaveToFile();
                this.Close();
            }
            else 
            {
                if (cfg.m_sPassWord == textPsw1.Text)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.None;
                    MessageBox.Show("密码错误请重新输入!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            CCfg cfg = new CCfg();
            if(textPsw1.Text != cfg.m_sPassWord)
            {
                MessageBox.Show("密码错误请重新输入!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                textPsw2.Enabled = true;
                textPsw2.Focus();
                m_bEditPsw = true;
            }
        }

        private void FormUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == textPsw1 && e.KeyValue == 13)     // 换行
            {
                btnOK_Click(sender, e);
            }
        }
    }
}

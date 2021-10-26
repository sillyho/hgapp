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
    public partial class FormInput : Form
    {
        private bool isPwd = false;

        public static string GetInput(string strTitle, string strDefault, bool Pwd)
        {
            FormInput input = new FormInput(strDefault)
            {
                isPwd = Pwd,
                Text = strTitle
            };
            input.ShowDialog();
            return input.textBoxInput.Text;
        }

        public FormInput(string strDefaultInput)
        {
            InitializeComponent();
            textBoxInput.Text = strDefaultInput;
            TopMost = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void FormInput_Shown(object sender, EventArgs e)
        {
            textBoxInput.Focus();
        }

        private void FormInput_Load(object sender, EventArgs e)
        {
            if (isPwd)
            {
                textBoxInput.Multiline = false;
                textBoxInput.PasswordChar = '*';
            }
        }
    }
}
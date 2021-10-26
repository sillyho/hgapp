namespace HGApp
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.BtnSetParam = new System.Windows.Forms.Button();
            this.BtnRun = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.PanelWnd = new System.Windows.Forms.Panel();
            this.PanelLog = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelClock = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnSetParam
            // 
            this.BtnSetParam.FlatAppearance.BorderSize = 0;
            this.BtnSetParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSetParam.Image = global::HGApp.Properties.Resources.Set;
            this.BtnSetParam.Location = new System.Drawing.Point(14, 11);
            this.BtnSetParam.Name = "BtnSetParam";
            this.BtnSetParam.Size = new System.Drawing.Size(75, 73);
            this.BtnSetParam.TabIndex = 0;
            this.BtnSetParam.UseVisualStyleBackColor = true;
            this.BtnSetParam.Click += new System.EventHandler(this.BtnSetParam_Click);
            // 
            // BtnRun
            // 
            this.BtnRun.FlatAppearance.BorderSize = 0;
            this.BtnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRun.Image = global::HGApp.Properties.Resources.Run;
            this.BtnRun.Location = new System.Drawing.Point(628, 12);
            this.BtnRun.Name = "BtnRun";
            this.BtnRun.Size = new System.Drawing.Size(75, 73);
            this.BtnRun.TabIndex = 0;
            this.BtnRun.UseVisualStyleBackColor = true;
            this.BtnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.FlatAppearance.BorderSize = 0;
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStop.Image = global::HGApp.Properties.Resources.Stop;
            this.BtnStop.Location = new System.Drawing.Point(709, 12);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 73);
            this.BtnStop.TabIndex = 0;
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // PanelWnd
            // 
            this.PanelWnd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelWnd.Location = new System.Drawing.Point(-1, 91);
            this.PanelWnd.Name = "PanelWnd";
            this.PanelWnd.Size = new System.Drawing.Size(801, 379);
            this.PanelWnd.TabIndex = 1;
            // 
            // PanelLog
            // 
            this.PanelLog.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PanelLog.Location = new System.Drawing.Point(-1, 467);
            this.PanelLog.Name = "PanelLog";
            this.PanelLog.Size = new System.Drawing.Size(801, 195);
            this.PanelLog.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.label1.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(800, 87);
            this.label1.TabIndex = 3;
            this.label1.Text = " 扫码激光打标系统";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelClock
            // 
            this.LabelClock.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.LabelClock.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabelClock.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.LabelClock.Location = new System.Drawing.Point(297, 66);
            this.LabelClock.Name = "LabelClock";
            this.LabelClock.Size = new System.Drawing.Size(250, 22);
            this.LabelClock.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 20);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ClientSize = new System.Drawing.Size(795, 661);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LabelClock);
            this.Controls.Add(this.PanelLog);
            this.Controls.Add(this.PanelWnd);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnRun);
            this.Controls.Add(this.BtnSetParam);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HG-APP(20211023V1.00)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnSetParam;
        private System.Windows.Forms.Button BtnRun;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Panel PanelWnd;
        private System.Windows.Forms.Panel PanelLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelClock;
        private System.Windows.Forms.Button button1;
    }
}


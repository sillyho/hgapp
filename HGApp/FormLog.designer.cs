namespace HGApp
{
    partial class Log
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Log));
            this.listView_Details = new System.Windows.Forms.ListView();
            this.时间 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.消息 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView_Details
            // 
            this.listView_Details.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_Details.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.时间,
            this.消息});
            this.listView_Details.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_Details.FullRowSelect = true;
            this.listView_Details.Location = new System.Drawing.Point(0, 0);
            this.listView_Details.Margin = new System.Windows.Forms.Padding(0);
            this.listView_Details.Name = "listView_Details";
            this.listView_Details.Size = new System.Drawing.Size(914, 145);
            this.listView_Details.TabIndex = 0;
            this.listView_Details.UseCompatibleStateImageBehavior = false;
            this.listView_Details.View = System.Windows.Forms.View.Details;
            // 
            // 时间
            // 
            this.时间.Text = "时间";
            this.时间.Width = 147;
            // 
            // 消息
            // 
            this.消息.Text = "消息";
            this.消息.Width = 1000;
            // 
            // Log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(914, 145);
            this.Controls.Add(this.listView_Details);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 540);
            this.Name = "Log";
            this.Text = "DetailsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDetails_FormClosing);
            this.Load += new System.EventHandler(this.FormLog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_Details;
        private System.Windows.Forms.ColumnHeader 时间;
        private System.Windows.Forms.ColumnHeader 消息;
    }
}
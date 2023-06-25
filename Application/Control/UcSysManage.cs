namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    using LTP.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class UcSysManage : UserControl
    {
        private INIFile cfgfile;
        private CheckBox chkLoginfo;
        private string cmcfgfile = (Application.StartupPath + @"\cmcfg.ini");
        private IContainer components;
        private GroupBox groupBox_DBO;
        private GroupBox groupBox1;
        private RadioButton radbtnDBO_SP;
        private RadioButton radbtnDBO_SQL;

        public UcSysManage()
        {
            this.InitializeComponent();
            if (File.Exists(this.cmcfgfile))
            {
                this.cfgfile = new INIFile(this.cmcfgfile);
                if (this.cfgfile.IniReadValue("dbo", "dbosp").Trim() == "1")
                {
                    this.radbtnDBO_SP.Checked = true;
                }
                else
                {
                    this.radbtnDBO_SQL.Checked = true;
                }
                if (this.cfgfile.IniReadValue("loginfo", "save").Trim() == "1")
                {
                    this.chkLoginfo.Checked = true;
                }
                else
                {
                    this.chkLoginfo.Checked = false;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox_DBO = new GroupBox();
            this.radbtnDBO_SP = new RadioButton();
            this.radbtnDBO_SQL = new RadioButton();
            this.groupBox1 = new GroupBox();
            this.chkLoginfo = new CheckBox();
            this.groupBox_DBO.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox_DBO.Controls.Add(this.radbtnDBO_SP);
            this.groupBox_DBO.Controls.Add(this.radbtnDBO_SQL);
            this.groupBox_DBO.Location = new Point(12, 5);
            this.groupBox_DBO.Name = "groupBox_DBO";
            this.groupBox_DBO.Size = new Size(0x156, 0x37);
            this.groupBox_DBO.TabIndex = 0;
            this.groupBox_DBO.TabStop = false;
            this.groupBox_DBO.Text = "DBO方式";
            this.radbtnDBO_SP.AutoSize = true;
            this.radbtnDBO_SP.Location = new Point(0xa4, 20);
            this.radbtnDBO_SP.Name = "radbtnDBO_SP";
            this.radbtnDBO_SP.Size = new Size(0x3b, 0x10);
            this.radbtnDBO_SP.TabIndex = 0;
            this.radbtnDBO_SP.Text = "SP方式";
            this.radbtnDBO_SP.UseVisualStyleBackColor = true;
            this.radbtnDBO_SQL.AutoSize = true;
            this.radbtnDBO_SQL.Checked = true;
            this.radbtnDBO_SQL.Location = new Point(0x23, 20);
            this.radbtnDBO_SQL.Name = "radbtnDBO_SQL";
            this.radbtnDBO_SQL.Size = new Size(0x47, 0x10);
            this.radbtnDBO_SQL.TabIndex = 0;
            this.radbtnDBO_SQL.TabStop = true;
            this.radbtnDBO_SQL.Text = "默认方式";
            this.radbtnDBO_SQL.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.chkLoginfo);
            this.groupBox1.Location = new Point(12, 0x43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x156, 0x41);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "软件设置";
            this.chkLoginfo.AutoSize = true;
            this.chkLoginfo.Checked = true;
            this.chkLoginfo.CheckState = CheckState.Checked;
            this.chkLoginfo.Location = new Point(0x17, 0x15);
            this.chkLoginfo.Name = "chkLoginfo";
            this.chkLoginfo.Size = new Size(0x60, 0x10);
            this.chkLoginfo.TabIndex = 0;
            this.chkLoginfo.Text = "记录错误日志";
            this.chkLoginfo.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.groupBox_DBO);
            base.Name = "UcSysManage";
            base.Size = new Size(0x170, 0x10b);
            this.groupBox_DBO.ResumeLayout(false);
            this.groupBox_DBO.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public void SaveDBO()
        {
            this.cfgfile = new INIFile(this.cmcfgfile);
            if (this.radbtnDBO_SP.Checked)
            {
                this.cfgfile.IniWriteValue("dbo", "dbosp", "1");
            }
            else
            {
                this.cfgfile.IniWriteValue("dbo", "dbosp", "0");
            }
            if (this.chkLoginfo.Checked)
            {
                this.cfgfile.IniWriteValue("loginfo", "save", "1");
            }
            else
            {
                this.cfgfile.IniWriteValue("loginfo", "save", "0");
            }
        }
    }
}


namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    using Flextronics.Applications.ApplicationFactory;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UcCSSet : UserControl
    {
        private DALTypeAddIn cm_daltype = new DALTypeAddIn();
        private IContainer components;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        public Label label1;
        public Label label2;
        public Label lblDbHelperName;
        public Label lblNamepace;
        public RadioButton radbtn_Frame_F3;
        public RadioButton radbtn_Frame_One;
        public RadioButton radbtn_Frame_S3;
        private ModuleSettings setting;
        public TextBox txtDbHelperName;
        public TextBox txtNamepace;
        public TextBox txtProcPrefix;
        public TextBox txtProjectName;

        public UcCSSet()
        {
            this.InitializeComponent();
            this.setting = ModuleConfig.GetSettings();
            string appFrame = this.setting.AppFrame;
            if (appFrame != null)
            {
                if (!(appFrame == "One"))
                {
                    if (appFrame == "S3")
                    {
                        this.radbtn_Frame_S3.Checked = true;
                    }
                    else if (appFrame == "F3")
                    {
                        this.radbtn_Frame_F3.Checked = true;
                    }
                }
                else
                {
                    this.radbtn_Frame_One.Checked = true;
                }
            }
            this.groupBox5.Controls.Add(this.cm_daltype);
            this.cm_daltype.Location = new Point(10, 0x12);
            this.cm_daltype.SetSelectedDALType(this.setting.DALType);
            this.txtDbHelperName.Text = this.setting.DbHelperName;
            this.txtNamepace.Text = this.setting.Namepace;
            this.txtProjectName.Text = this.setting.ProjectName;
            this.txtProcPrefix.Text = this.setting.ProcPrefix;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetDALType()
        {
            string appGuid = "";
            appGuid = this.cm_daltype.AppGuid;
            if ((appGuid == "") && (appGuid == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return "";
            }
            return appGuid;
        }

        private void InitializeComponent()
        {
            this.lblNamepace = new Label();
            this.lblDbHelperName = new Label();
            this.txtNamepace = new TextBox();
            this.txtDbHelperName = new TextBox();
            this.groupBox6 = new GroupBox();
            this.radbtn_Frame_One = new RadioButton();
            this.radbtn_Frame_F3 = new RadioButton();
            this.radbtn_Frame_S3 = new RadioButton();
            this.groupBox5 = new GroupBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.txtProjectName = new TextBox();
            this.txtProcPrefix = new TextBox();
            this.groupBox6.SuspendLayout();
            base.SuspendLayout();
            this.lblNamepace.AutoSize = true;
            this.lblNamepace.Location = new Point(0x1b, 0x11);
            this.lblNamepace.Name = "lblNamepace";
            this.lblNamepace.Size = new Size(0x59, 12);
            this.lblNamepace.TabIndex = 0;
            this.lblNamepace.Text = "顶级命名空间：";
            this.lblDbHelperName.AutoSize = true;
            this.lblDbHelperName.Location = new Point(0x1b, 0x29);
            this.lblDbHelperName.Name = "lblDbHelperName";
            this.lblDbHelperName.Size = new Size(0x59, 12);
            this.lblDbHelperName.TabIndex = 0;
            this.lblDbHelperName.Text = "数据访问基类：";
            this.txtNamepace.Location = new Point(0x7e, 13);
            this.txtNamepace.Name = "txtNamepace";
            this.txtNamepace.Size = new Size(0x8f, 0x15);
            this.txtNamepace.TabIndex = 2;
            this.txtDbHelperName.Location = new Point(0x7e, 0x25);
            this.txtDbHelperName.Name = "txtDbHelperName";
            this.txtDbHelperName.Size = new Size(0x8f, 0x15);
            this.txtDbHelperName.TabIndex = 2;
            this.groupBox6.Controls.Add(this.radbtn_Frame_One);
            this.groupBox6.Controls.Add(this.radbtn_Frame_F3);
            this.groupBox6.Controls.Add(this.radbtn_Frame_S3);
            this.groupBox6.Location = new Point(13, 0x71);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(360, 0x2c);
            this.groupBox6.TabIndex = 0x34;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "默认架构方式";
            this.radbtn_Frame_One.AutoSize = true;
            this.radbtn_Frame_One.Location = new Point(0x15, 0x12);
            this.radbtn_Frame_One.Name = "radbtn_Frame_One";
            this.radbtn_Frame_One.Size = new Size(0x47, 0x10);
            this.radbtn_Frame_One.TabIndex = 0;
            this.radbtn_Frame_One.Text = "单类结构";
            this.radbtn_Frame_One.UseVisualStyleBackColor = true;
            this.radbtn_Frame_F3.AutoSize = true;
            this.radbtn_Frame_F3.Checked = true;
            this.radbtn_Frame_F3.Location = new Point(0xcb, 0x12);
            this.radbtn_Frame_F3.Name = "radbtn_Frame_F3";
            this.radbtn_Frame_F3.Size = new Size(0x5f, 0x10);
            this.radbtn_Frame_F3.TabIndex = 0;
            this.radbtn_Frame_F3.TabStop = true;
            this.radbtn_Frame_F3.Text = "工厂模式三层";
            this.radbtn_Frame_F3.UseVisualStyleBackColor = true;
            this.radbtn_Frame_S3.AutoSize = true;
            this.radbtn_Frame_S3.Location = new Point(0x70, 0x12);
            this.radbtn_Frame_S3.Name = "radbtn_Frame_S3";
            this.radbtn_Frame_S3.Size = new Size(0x47, 0x10);
            this.radbtn_Frame_S3.TabIndex = 0;
            this.radbtn_Frame_S3.Text = "简单三层";
            this.radbtn_Frame_S3.UseVisualStyleBackColor = true;
            this.groupBox5.Location = new Point(13, 0xa1);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(360, 0x39);
            this.groupBox5.TabIndex = 0x33;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "数据层代码形式";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x33, 0x41);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目名称：";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x1b, 0x59);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "存储过程前缀：";
            this.txtProjectName.AcceptsReturn = true;
            this.txtProjectName.Location = new Point(0x7e, 0x3d);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new Size(0x8f, 0x15);
            this.txtProjectName.TabIndex = 2;
            this.txtProcPrefix.Location = new Point(0x7e, 0x55);
            this.txtProcPrefix.Name = "txtProcPrefix";
            this.txtProcPrefix.Size = new Size(0x8f, 0x15);
            this.txtProcPrefix.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
//            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox6);
            base.Controls.Add(this.groupBox5);
            base.Controls.Add(this.txtProcPrefix);
            base.Controls.Add(this.txtDbHelperName);
            base.Controls.Add(this.txtProjectName);
            base.Controls.Add(this.txtNamepace);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.lblDbHelperName);
            base.Controls.Add(this.lblNamepace);
            base.Name = "UcCSSet";
            base.Size = new Size(0x17d, 0x111);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}


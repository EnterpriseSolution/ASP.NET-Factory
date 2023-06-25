namespace Codematic
{
    using Maticsoft.AddInManager;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class FormAbout : Form
    {
        private IContainer components;
        private DataSet dsAddin;
        private Label label1;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label lblDesc;
        private Label lblVer;
        private ListBox listBox1;
        private Panel panel1;

        public FormAbout()
        {
            this.InitializeComponent();
        }

        private void BindListbox()
        {
            try
            {
                this.dsAddin = new AddIn().GetAddInList();
                this.listBox1.Items.Clear();
                if ((this.dsAddin != null) && (this.dsAddin.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow row in this.dsAddin.Tables[0].Rows)
                    {
                        string str = row["Guid"].ToString();
                        string str2 = row["Name"].ToString();
                        string str3 = row["Version"].ToString();
                        this.listBox1.Items.Add(str2 + "  " + str3 + " " + str);
                    }
                }
            }
            catch (SystemException exception)
            {
                string message = exception.Message;
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            this.lblVer.Text = "版本：" + Application.ProductVersion + " (Build 081005)";
            this.BindListbox();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblVer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDesc = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Web Matrix";
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.BackColor = System.Drawing.Color.Transparent;
            this.lblVer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblVer.Location = new System.Drawing.Point(12, 98);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(58, 13);
            this.lblVer.TabIndex = 0;
            this.lblVer.Text = "版本：1.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Copyright(C) 2009   All Rights Reserved.";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(12, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(508, 40);
            this.label4.TabIndex = 0;
            this.label4.Text = "警告; 本计算机程序受版权法和国际条约保护。如未经授权而擅自复制或传播本程序(或其中任何部分)，将受到严厉的民事或刑事制裁，并将在法律许可的最大限度内受到起诉。";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(12, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "已安装的产品(&I):";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(14, 153);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(506, 56);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(12, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "产品详细信息:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblDesc);
            this.panel1.Location = new System.Drawing.Point(14, 237);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(506, 46);
            this.panel1.TabIndex = 3;
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(3, 3);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(0, 13);
            this.lblDesc.TabIndex = 0;
            // 
            // FormAbout
            // 
            this.ClientSize = new System.Drawing.Size(542, 365);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblVer);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于 Web Matrix";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                string str = this.listBox1.SelectedItem.ToString();
                int startIndex = str.LastIndexOf(" ");
                string str2 = str.Substring(startIndex).Trim();
                DataRow[] rowArray = this.dsAddin.Tables[0].Select("Guid='" + str2 + "'");
                if (rowArray.Length > 0)
                {
                    this.lblDesc.Text = rowArray[0]["Decription"].ToString();
                }
            }
        }
    }
}


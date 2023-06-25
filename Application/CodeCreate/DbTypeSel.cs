namespace Flextronics.Application.ApplicationFactory
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DbTypeSel : Form
    {
        private Button btn_Cancel;
        private Button btn_Next;
        private IContainer components;
        public string dbtype = "SQL2000";
        private GroupBox groupBox1;
        private RadioButton radbtn_dbtype_Access;
        private RadioButton radbtn_dbtype_MySQL;
        private RadioButton radbtn_dbtype_Oracle;
        private RadioButton radbtn_dbtype_SQL2000;

        public DbTypeSel()
        {
            this.InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (this.radbtn_dbtype_SQL2000.Checked)
            {
                this.dbtype = "SQL2000";
            }
            if (this.radbtn_dbtype_Oracle.Checked)
            {
                this.dbtype = "Oracle";
            }
            if (this.radbtn_dbtype_MySQL.Checked)
            {
                this.dbtype = "MySQL";
            }
            if (this.radbtn_dbtype_Access.Checked)
            {
                this.dbtype = "OleDb";
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radbtn_dbtype_MySQL = new System.Windows.Forms.RadioButton();
            this.radbtn_dbtype_Oracle = new System.Windows.Forms.RadioButton();
            this.radbtn_dbtype_SQL2000 = new System.Windows.Forms.RadioButton();
            this.radbtn_dbtype_Access = new System.Windows.Forms.RadioButton();
            this.btn_Next = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radbtn_dbtype_MySQL);
            this.groupBox1.Controls.Add(this.radbtn_dbtype_Oracle);
            this.groupBox1.Controls.Add(this.radbtn_dbtype_SQL2000);
            this.groupBox1.Controls.Add(this.radbtn_dbtype_Access);
            this.groupBox1.Location = new System.Drawing.Point(10, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据源类型";
            // 
            // radbtn_dbtype_MySQL
            // 
            this.radbtn_dbtype_MySQL.AutoSize = true;
            this.radbtn_dbtype_MySQL.Location = new System.Drawing.Point(24, 84);
            this.radbtn_dbtype_MySQL.Name = "radbtn_dbtype_MySQL";
            this.radbtn_dbtype_MySQL.Size = new System.Drawing.Size(63, 17);
            this.radbtn_dbtype_MySQL.TabIndex = 5;
            this.radbtn_dbtype_MySQL.TabStop = true;
            this.radbtn_dbtype_MySQL.Text = " MySQL";
            this.radbtn_dbtype_MySQL.UseVisualStyleBackColor = true;
            // 
            // radbtn_dbtype_Oracle
            // 
            this.radbtn_dbtype_Oracle.Location = new System.Drawing.Point(24, 52);
            this.radbtn_dbtype_Oracle.Name = "radbtn_dbtype_Oracle";
            this.radbtn_dbtype_Oracle.Size = new System.Drawing.Size(104, 24);
            this.radbtn_dbtype_Oracle.TabIndex = 4;
            this.radbtn_dbtype_Oracle.Text = " Oracle";
            // 
            // radbtn_dbtype_SQL2000
            // 
            this.radbtn_dbtype_SQL2000.Checked = true;
            this.radbtn_dbtype_SQL2000.Location = new System.Drawing.Point(24, 20);
            this.radbtn_dbtype_SQL2000.Name = "radbtn_dbtype_SQL2000";
            this.radbtn_dbtype_SQL2000.Size = new System.Drawing.Size(160, 24);
            this.radbtn_dbtype_SQL2000.TabIndex = 2;
            this.radbtn_dbtype_SQL2000.TabStop = true;
            this.radbtn_dbtype_SQL2000.Text = " SQL Server";
            // 
            // radbtn_dbtype_Access
            // 
            this.radbtn_dbtype_Access.Location = new System.Drawing.Point(24, 108);
            this.radbtn_dbtype_Access.Name = "radbtn_dbtype_Access";
            this.radbtn_dbtype_Access.Size = new System.Drawing.Size(104, 24);
            this.radbtn_dbtype_Access.TabIndex = 3;
            this.radbtn_dbtype_Access.Text = " OleDb";
            // 
            // btn_Next
            // 
            this.btn_Next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Next.Location = new System.Drawing.Point(96, 176);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(75, 25);
            this.btn_Next.TabIndex = 1;
            this.btn_Next.Text = "下一步";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(188, 176);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 25);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取  消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // DbTypeSel
            // 
            this.ClientSize = new System.Drawing.Size(290, 213);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbTypeSel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择数据库类型";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}


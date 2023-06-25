namespace Codematic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    

    public class DbOption : Form
    {
        private Button btn_Exit;
        private Button btnOk;
        private Container components;
        private GroupBox groupBox4;
        private RadioButton radbtn_dbtype_Access;
        private RadioButton radbtn_dbtype_Oracle;
        private RadioButton radbtn_dbtype_SQL;
        private ModuleSettings setting = new ModuleSettings();

        public DbOption()
        {
            this.InitializeComponent();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
        }

        private void DbOption_Load(object sender, EventArgs e)
        {
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
            this.btnOk = new Button();
            this.btn_Exit = new Button();
            this.groupBox4 = new GroupBox();
            this.radbtn_dbtype_Oracle = new RadioButton();
            this.radbtn_dbtype_SQL = new RadioButton();
            this.radbtn_dbtype_Access = new RadioButton();
            this.groupBox4.SuspendLayout();
            base.SuspendLayout();
   //         this.btnOk._Image = null;
            this.btnOk.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
    //        this.btnOk.DefaultScheme = false;
            //this.btnOk.DialogResult = DialogResult.None;
            this.btnOk.Image = null;
            this.btnOk.Location = new Point(0x70, 0xb0);
            this.btnOk.Name = "btnOk";
      //      this.btnOk.Scheme = Button.Schemes.Blue;
            this.btnOk.Size = new Size(0x4b, 0x1a);
            this.btnOk.TabIndex = 0x2c;
            this.btnOk.Text = "确  定";
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
        //    this.btn_Exit._Image = null;
            this.btn_Exit.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
      //      this.btn_Exit.DefaultScheme = false;
            this.btn_Exit.DialogResult = DialogResult.None;
            this.btn_Exit.Image = null;
            this.btn_Exit.Location = new Point(200, 0xb0);
            this.btn_Exit.Name = "btn_Exit";
      //      this.btn_Exit.Scheme = Button.Schemes.Blue;
            this.btn_Exit.Size = new Size(0x4b, 0x1a);
            this.btn_Exit.TabIndex = 0x2b;
            this.btn_Exit.Text = "取  消";
            this.btn_Exit.Click += new EventHandler(this.btn_Exit_Click);
            this.groupBox4.Controls.Add(this.radbtn_dbtype_Oracle);
            this.groupBox4.Controls.Add(this.radbtn_dbtype_SQL);
            this.groupBox4.Controls.Add(this.radbtn_dbtype_Access);
            this.groupBox4.Location = new Point(8, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(280, 0x90);
            this.groupBox4.TabIndex = 0x2f;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "选择数据源类型";
            this.radbtn_dbtype_Oracle.Location = new Point(0x18, 0x40);
            this.radbtn_dbtype_Oracle.Name = "radbtn_dbtype_Oracle";
            this.radbtn_dbtype_Oracle.TabIndex = 1;
            this.radbtn_dbtype_Oracle.Text = " Oracle";
            this.radbtn_dbtype_SQL.Checked = true;
            this.radbtn_dbtype_SQL.Location = new Point(0x18, 0x20);
            this.radbtn_dbtype_SQL.Name = "radbtn_dbtype_SQL";
            this.radbtn_dbtype_SQL.Size = new Size(160, 0x18);
            this.radbtn_dbtype_SQL.TabIndex = 0;
            this.radbtn_dbtype_SQL.TabStop = true;
            this.radbtn_dbtype_SQL.Text = " SQL Server ";
            this.radbtn_dbtype_Access.Location = new Point(0x18, 0x60);
            this.radbtn_dbtype_Access.Name = "radbtn_dbtype_Access";
            this.radbtn_dbtype_Access.TabIndex = 1;
            this.radbtn_dbtype_Access.Text = " OleDb";
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x124, 0xd7);
            base.Controls.Add(this.groupBox4);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.btn_Exit);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DbOption";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "数据源设置";
            base.Load += new EventHandler(this.DbOption_Load);
            this.groupBox4.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}


namespace Codematic
{
    using LTP.CodeBuild;
    using LTP.DBFactory;
    using LTP.IDBO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;
    //

    public class DataExport : Form
    {
        private Button btn_Add;
        private Button btn_Addlist;
        private Button btn_Cancle;
        private Button btn_Creat;
        private Button btn_Del;
        private Button btn_Dellist;
        private Button btn_TargetFold;
        private CodeBuilders cb;
        private ComboBox cmbDB;
        private Container components;
        private string DbName = "master";
        private IDbObject dbobj;
        private DbSettings dbset;
        private IDbScriptBuilder dsb;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelNum;
        private Label lblServer;
        private ListBox listTable1;
        private ListBox listTable2;
        private Thread mythread;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private TextBox txtTargetFolder;

        public DataExport(string longservername, string dbname)
        {
            this.InitializeComponent();
            this.DbName = dbname;
            this.dbset = DbConfig.GetSetting(longservername);
            this.dbobj = DBOMaker.CreateDbObj(this.dbset.DbType);
            this.dbobj.DbConnectStr = this.dbset.ConnectStr;
            this.cb = new CodeBuilders(this.dbobj);
            this.dsb = DBOMaker.CreateScript(this.dbset.DbType);
            this.lblServer.Text = this.dbset.Server;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            int count = this.listTable1.SelectedItems.Count;
            ListBox.SelectedObjectCollection selectedItems = this.listTable1.SelectedItems;
            for (int i = 0; i < count; i++)
            {
                this.listTable2.Items.Add(this.listTable1.SelectedItems[i]);
            }
            for (int j = 0; j < count; j++)
            {
                if (this.listTable1.SelectedItems.Count > 0)
                {
                    this.listTable1.Items.Remove(this.listTable1.SelectedItems[0]);
                }
            }
            this.IsHasItem();
        }

        private void btn_Addlist_Click(object sender, EventArgs e)
        {
            int count = this.listTable1.Items.Count;
            for (int i = 0; i < count; i++)
            {
                this.listTable2.Items.Add(this.listTable1.Items[i]);
            }
            this.listTable1.Items.Clear();
            this.IsHasItem();
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            if ((this.mythread != null) && this.mythread.IsAlive)
            {
                this.mythread.Abort();
            }
            base.Close();
        }

        private void btn_Creat_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtTargetFolder.Text == "")
                {
                    MessageBox.Show("请选择保存文件路径！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.mythread = new Thread(new ThreadStart(this.ThreadWork));
                    this.mythread.Start();
                }
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog(exception);
                MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            int count = this.listTable2.SelectedItems.Count;
            ListBox.SelectedObjectCollection selectedItems = this.listTable2.SelectedItems;
            for (int i = 0; i < count; i++)
            {
                this.listTable1.Items.Add(this.listTable2.SelectedItems[i]);
            }
            for (int j = 0; j < count; j++)
            {
                if (this.listTable2.SelectedItems.Count > 0)
                {
                    this.listTable2.Items.Remove(this.listTable2.SelectedItems[0]);
                }
            }
            this.IsHasItem();
        }

        private void btn_Dellist_Click(object sender, EventArgs e)
        {
            int count = this.listTable2.Items.Count;
            for (int i = 0; i < count; i++)
            {
                this.listTable1.Items.Add(this.listTable2.Items[i]);
            }
            this.listTable2.Items.Clear();
            this.IsHasItem();
        }

        private void btn_TargetFold_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "保存当前脚本";
            dialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.txtTargetFolder.Text = dialog.FileName;
            }
        }

        private void cmbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.cmbDB.Text;
            List<string> tables = this.dbobj.GetTables(text);
            this.listTable1.Items.Clear();
            this.listTable2.Items.Clear();
            if (tables.Count > 0)
            {
                foreach (string str2 in tables)
                {
                    this.listTable1.Items.Add(str2);
                }
            }
            this.IsHasItem();
        }

        private void DbToWord_Load(object sender, EventArgs e)
        {
            string dbName = "master";
            string dbType = this.dbobj.DbType;
            if (dbType != null)
            {
                if (!(dbType == "SQL2000") && !(dbType == "SQL2005"))
                {
                    if ((dbType == "Oracle") || (dbType == "OleDb"))
                    {
                        dbName = this.dbset.DbName;
                    }
                    else if (dbType == "MySQL")
                    {
                        dbName = "mysql";
                    }
                }
                else
                {
                    dbName = "master";
                }
            }
            if ((this.dbset.DbName == "") || (this.dbset.DbName == dbName))
            {
                List<string> dBList = this.dbobj.GetDBList();
                if ((dBList != null) && (dBList.Count > 0))
                {
                    foreach (string str2 in dBList)
                    {
                        this.cmbDB.Items.Add(str2);
                    }
                }
            }
            else
            {
                this.cmbDB.Items.Add(this.DbName);
            }
            this.btn_Creat.Enabled = false;
            this.cmbDB.Text = this.DbName;
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
            ResourceManager manager = new ResourceManager(typeof(DbToScript));
            this.groupBox1 = new GroupBox();
            this.cmbDB = new ComboBox();
            this.lblServer = new Label();
            this.label1 = new Label();
            this.label3 = new Label();
            this.groupBox2 = new GroupBox();
            this.progressBar1 = new ProgressBar();
            this.labelNum = new Label();
            this.btn_Addlist = new Button();
            this.btn_Add = new Button();
            this.btn_Del = new Button();
            this.btn_Dellist = new Button();
            this.listTable2 = new ListBox();
            this.listTable1 = new ListBox();
            this.btn_Creat = new Button();
            this.btn_Cancle = new Button();
            this.groupBox3 = new GroupBox();
            this.txtTargetFolder = new TextBox();
            this.btn_TargetFold = new Button();
            this.label2 = new Label();
            this.progressBar2 = new ProgressBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.cmbDB);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1d0, 0x38);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据库";
            this.cmbDB.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDB.Location = new Point(0x128, 0x18);
            this.cmbDB.Name = "cmbDB";
            this.cmbDB.Size = new Size(0x98, 20);
            this.cmbDB.TabIndex = 2;
            this.cmbDB.SelectedIndexChanged += new EventHandler(this.cmbDB_SelectedIndexChanged);
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new Point(0x68, 0x1a);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new Size(0x2a, 0x11);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "label2";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 0x1a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4f, 0x11);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前服务器：";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xd8, 0x1a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4f, 0x11);
            this.label3.TabIndex = 0;
            this.label3.Text = "选择数据库：";
            this.groupBox2.Controls.Add(this.progressBar2);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.labelNum);
            this.groupBox2.Controls.Add(this.btn_Addlist);
            this.groupBox2.Controls.Add(this.btn_Add);
            this.groupBox2.Controls.Add(this.btn_Del);
            this.groupBox2.Controls.Add(this.btn_Dellist);
            this.groupBox2.Controls.Add(this.listTable2);
            this.groupBox2.Controls.Add(this.listTable1);
            this.groupBox2.Location = new Point(8, 0x70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x1d0, 0xe0);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择表";
            this.progressBar1.Location = new Point(0x30, 0xc0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(0x188, 20);
            this.progressBar1.TabIndex = 10;
            this.labelNum.Location = new Point(0x10, 0xbf);
            this.labelNum.Name = "labelNum";
            this.labelNum.Size = new Size(0x23, 0x16);
            this.labelNum.TabIndex = 9;
            this.labelNum.TextAlign = ContentAlignment.MiddleCenter;
            this.btn_Addlist.Enabled = false;
            this.btn_Addlist.FlatStyle = FlatStyle.Popup;
            this.btn_Addlist.Location = new Point(0xd0, 0x20);
            this.btn_Addlist.Name = "btn_Addlist";
            this.btn_Addlist.Size = new Size(40, 0x17);
            this.btn_Addlist.TabIndex = 7;
            this.btn_Addlist.Text = ">>";
            this.btn_Addlist.Click += new EventHandler(this.btn_Addlist_Click);
            this.btn_Add.Enabled = false;
            this.btn_Add.FlatStyle = FlatStyle.Popup;
            this.btn_Add.Location = new Point(0xd0, 0x40);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new Size(40, 0x17);
            this.btn_Add.TabIndex = 8;
            this.btn_Add.Text = ">";
            this.btn_Add.Click += new EventHandler(this.btn_Add_Click);
            this.btn_Del.Enabled = false;
            this.btn_Del.FlatStyle = FlatStyle.Popup;
            this.btn_Del.Location = new Point(0xd0, 0x60);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new Size(40, 0x17);
            this.btn_Del.TabIndex = 5;
            this.btn_Del.Text = "<";
            this.btn_Del.Click += new EventHandler(this.btn_Del_Click);
            this.btn_Dellist.Enabled = false;
            this.btn_Dellist.FlatStyle = FlatStyle.Popup;
            this.btn_Dellist.Location = new Point(0xd0, 0x80);
            this.btn_Dellist.Name = "btn_Dellist";
            this.btn_Dellist.Size = new Size(40, 0x17);
            this.btn_Dellist.TabIndex = 6;
            this.btn_Dellist.Text = "<<";
            this.btn_Dellist.Click += new EventHandler(this.btn_Dellist_Click);
            this.listTable2.ItemHeight = 12;
            this.listTable2.Location = new Point(0x120, 0x18);
            this.listTable2.Name = "listTable2";
            this.listTable2.Size = new Size(0x98, 0x94);
            this.listTable2.TabIndex = 1;
            this.listTable2.DoubleClick += new EventHandler(this.listTable2_DoubleClick);
            this.listTable1.ItemHeight = 12;
            this.listTable1.Location = new Point(0x10, 0x18);
            this.listTable1.Name = "listTable1";
            this.listTable1.SelectionMode = SelectionMode.MultiExtended;
            this.listTable1.Size = new Size(0x98, 0x94);
            this.listTable1.TabIndex = 0;
            this.listTable1.DoubleClick += new EventHandler(this.listTable1_DoubleClick);
        //    this.btn_Creat._Image = null;
            this.btn_Creat.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
      //      this.btn_Creat.DefaultScheme = false;
            this.btn_Creat.DialogResult = DialogResult.None;
            this.btn_Creat.Image = null;
            this.btn_Creat.Location = new Point(240, 0x158);
            this.btn_Creat.Name = "btn_Creat";
        //    this.btn_Creat.Scheme = Button.Schemes.Blue;
            this.btn_Creat.Size = new Size(0x4b, 0x1a);
            this.btn_Creat.TabIndex = 0x2a;
            this.btn_Creat.Text = "生  成";
            this.btn_Creat.Click += new EventHandler(this.btn_Creat_Click);
           // this.btn_Cancle._Image = null;
            this.btn_Cancle.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
        //    this.btn_Cancle.DefaultScheme = false;
            this.btn_Cancle.DialogResult = DialogResult.None;
            this.btn_Cancle.Image = null;
            this.btn_Cancle.Location = new Point(0x158, 0x158);
            this.btn_Cancle.Name = "btn_Cancle";
        //    this.btn_Cancle.Scheme = Button.Schemes.Blue;
            this.btn_Cancle.Size = new Size(0x4b, 0x1a);
            this.btn_Cancle.TabIndex = 0x2a;
            this.btn_Cancle.Text = "取  消";
            this.btn_Cancle.Click += new EventHandler(this.btn_Cancle_Click);
            this.groupBox3.Controls.Add(this.txtTargetFolder);
            this.groupBox3.Controls.Add(this.btn_TargetFold);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new Point(8, 0x40);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x1d0, 0x30);
            this.groupBox3.TabIndex = 0x2b;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "保存";
            this.txtTargetFolder.BorderStyle = BorderStyle.FixedSingle;
            this.txtTargetFolder.Location = new Point(0x48, 0x10);
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.Size = new Size(0x120, 0x15);
            this.txtTargetFolder.TabIndex = 0x2f;
            this.txtTargetFolder.Text = "";
        //    this.btn_TargetFold._Image = null;
            this.btn_TargetFold.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
          //  this.btn_TargetFold.DefaultScheme = false;
            this.btn_TargetFold.DialogResult = DialogResult.None;
            this.btn_TargetFold.Image = null;
            this.btn_TargetFold.Location = new Point(0x178, 15);
            this.btn_TargetFold.Name = "btn_TargetFold";
          //  this.btn_TargetFold.Scheme = Button.Schemes.Blue;
            this.btn_TargetFold.Size = new Size(0x39, 0x17);
            this.btn_TargetFold.TabIndex = 0x30;
            this.btn_TargetFold.Text = "选择...";
            this.btn_TargetFold.Click += new EventHandler(this.btn_TargetFold_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 0x12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x36, 0x11);
            this.label2.TabIndex = 0x31;
            this.label2.Text = "文件名：";
            this.progressBar2.Location = new Point(0x30, 0xb2);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new Size(0x188, 10);
            this.progressBar2.TabIndex = 11;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(480, 0x17e);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.btn_Creat);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.btn_Cancle);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DbToScript";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "生成SQL脚本";
            base.Load += new EventHandler(this.DbToWord_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void IsHasItem()
        {
            if (this.listTable1.Items.Count > 0)
            {
                this.btn_Add.Enabled = true;
                this.btn_Addlist.Enabled = true;
            }
            else
            {
                this.btn_Add.Enabled = false;
                this.btn_Addlist.Enabled = false;
            }
            if (this.listTable2.Items.Count > 0)
            {
                this.btn_Del.Enabled = true;
                this.btn_Dellist.Enabled = true;
                this.btn_Creat.Enabled = true;
            }
            else
            {
                this.btn_Del.Enabled = false;
                this.btn_Dellist.Enabled = false;
                this.btn_Creat.Enabled = false;
            }
        }

        private void listTable1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listTable1.SelectedItem != null)
            {
                this.listTable2.Items.Add(this.listTable1.SelectedItem);
                this.listTable1.Items.Remove(this.listTable1.SelectedItem);
                this.IsHasItem();
            }
        }

        private void listTable2_DoubleClick(object sender, EventArgs e)
        {
            if (this.listTable2.SelectedItem != null)
            {
                this.listTable1.Items.Add(this.listTable2.SelectedItem);
                this.listTable2.Items.Remove(this.listTable2.SelectedItem);
                this.IsHasItem();
            }
        }

        private void ThreadWork()
        {
            try
            {
                this.Text = "正在生成脚本...";
                this.Cursor = Cursors.WaitCursor;
                this.btn_Creat.Enabled = false;
                this.btn_Cancle.Enabled = false;
                string text = this.cmbDB.Text;
                string text1 = "数据库名：" + text;
                int count = this.listTable2.Items.Count;
                string filename = this.txtTargetFolder.Text;
                this.progressBar1.Maximum = count;
                this.labelNum.Text = "0";
                for (int i = 0; i < count; i++)
                {
                    this.listTable2.SelectedIndex = i;
                    string tablename = this.listTable2.Items[i].ToString();
                    this.dsb.CreateTabScript(text, tablename, filename, this.progressBar2);
                    this.progressBar1.Value = i + 1;
                    this.labelNum.Text = (i + 1).ToString();
                }
                this.btn_Creat.Enabled = true;
                this.btn_Cancle.Enabled = true;
                this.Text = "脚本全部生成成功！";
                this.Cursor = Cursors.Default;
                MessageBox.Show("脚本全部生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog(exception);
                this.btn_Creat.Enabled = true;
                this.btn_Cancle.Enabled = true;
                this.Text = "脚本生成失败！";
                this.Cursor = Cursors.Default;
                MessageBox.Show("脚本生成失败！请检查表名是否规范或其他问题导致。(" + exception.Message + ")", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}


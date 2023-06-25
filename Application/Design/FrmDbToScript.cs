using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Flextronics.Applications.Library.Schema;
using Flextronics.Applications.Library.Utility;

namespace Flextronics.Applications.ApplicationFactory
{
   
    public class FrmDbToScript : Form
    {
        private Button btn_Add;
        private Button btn_Addlist;
        private Button btn_Cancle;
        private Button btn_Creat;
        private Button btn_Del;
        private Button btn_Dellist;
        private Button btn_TargetFold;
        private ComboBox cmbDB;
        private Container components;
        private string DbName;
        private IDbObject dbobj;
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
        private PictureBox pictureBox1;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private TextBox txtTargetFolder;

        public FrmDbToScript(string longservername)
        {
            this.DbName = "master";
            this.InitializeComponent();
            this.dsb = ObjectHelper.CreatDsb(longservername);
            this.dbobj = ObjectHelper.CreatDbObj(longservername);
            int index = longservername.IndexOf("(");
            string str = longservername.Substring(0, index);
            this.lblServer.Text = str;
            List<string> dBList = this.dbobj.GetDBList();
            if ((dBList != null) && (dBList.Count > 0))
            {
                foreach (string str2 in dBList)
                {
                    this.cmbDB.Items.Add(str2);
                }
            }
            this.btn_Creat.Enabled = false;
            if (this.cmbDB.Items.Count > 0)
            {
                this.cmbDB.SelectedIndex = 0;
            }
        }

        public FrmDbToScript(string longservername, string dbname)
        {
            this.DbName = "master";
            this.InitializeComponent();
            this.DbName = dbname;
            this.dsb = ObjectHelper.CreatDsb(longservername);
            this.dbobj = ObjectHelper.CreatDbObj(longservername);
            this.lblServer.Text = longservername;
            List<string> dBList = this.dbobj.GetDBList();
            if ((dBList != null) && (dBList.Count > 0))
            {
                foreach (string str in dBList)
                {
                    this.cmbDB.Items.Add(str);
                }
            }
            this.btn_Creat.Enabled = false;
            this.cmbDB.Text = this.DbName;
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
                    this.DbName = this.cmbDB.Text;
                    this.pictureBox1.Visible = true;
                    this.mythread = new Thread(new ThreadStart(this.ThreadWork));
                    this.mythread.Start();
                }
            }
            catch (Exception exception)
            {
                Log.WriteLog(exception);
                MessageBox.Show(exception.Message, "完成", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            this.DbName = this.cmbDB.Text;
            List<string> tables = this.dbobj.GetTables(this.DbName);
            this.listTable1.Items.Clear();
            this.listTable2.Items.Clear();
            if (tables.Count > 0)
            {
                foreach (string str in tables)
                {
                    this.listTable1.Items.Add(str);
                }
            }
            this.IsHasItem();
        }

        private void DbToWord_Load(object sender, EventArgs e)
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmDbToScript));
            this.groupBox1 = new GroupBox();
            this.cmbDB = new ComboBox();
            this.lblServer = new Label();
            this.label1 = new Label();
            this.label3 = new Label();
            this.groupBox2 = new GroupBox();
            this.progressBar2 = new ProgressBar();
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
            this.pictureBox1 = new PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.cmbDB);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1f7, 0x38);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据库";
            this.cmbDB.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDB.Location = new Point(0x146, 0x18);
            this.cmbDB.Name = "cmbDB";
            this.cmbDB.Size = new Size(160, 20);
            this.cmbDB.TabIndex = 2;
            this.cmbDB.SelectedIndexChanged += new EventHandler(this.cmbDB_SelectedIndexChanged);
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new Point(0x57, 0x1a);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new Size(0x29, 12);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "label2";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(9, 0x1a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前服务器：";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xf6, 0x1a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4d, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "选择数据库：";
            this.groupBox2.Controls.Add(this.pictureBox1);
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
            this.groupBox2.Size = new Size(0x1f7, 0xe0);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择表";
            this.progressBar2.Location = new Point(0x10, 0xb2);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new Size(0x1ac, 10);
            this.progressBar2.TabIndex = 11;
            this.progressBar1.Location = new Point(0x10, 0xc0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(0x1ac, 20);
            this.progressBar1.TabIndex = 10;
            this.labelNum.Location = new Point(450, 0xbf);
            this.labelNum.Name = "labelNum";
            this.labelNum.Size = new Size(0x23, 0x16);
            this.labelNum.TabIndex = 9;
            this.labelNum.TextAlign = ContentAlignment.MiddleCenter;
            this.btn_Addlist.Enabled = false;
            this.btn_Addlist.FlatStyle = FlatStyle.Popup;
            this.btn_Addlist.Location = new Point(240, 0x1f);
            this.btn_Addlist.Name = "btn_Addlist";
            this.btn_Addlist.Size = new Size(40, 0x17);
            this.btn_Addlist.TabIndex = 7;
            this.btn_Addlist.Text = ">>";
            this.btn_Addlist.Click += new EventHandler(this.btn_Addlist_Click);
            this.btn_Add.Enabled = false;
            this.btn_Add.FlatStyle = FlatStyle.Popup;
            this.btn_Add.Location = new Point(240, 0x3d);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new Size(40, 0x17);
            this.btn_Add.TabIndex = 8;
            this.btn_Add.Text = ">";
            this.btn_Add.Click += new EventHandler(this.btn_Add_Click);
            this.btn_Del.Enabled = false;
            this.btn_Del.FlatStyle = FlatStyle.Popup;
            this.btn_Del.Location = new Point(240, 0x5b);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new Size(40, 0x17);
            this.btn_Del.TabIndex = 5;
            this.btn_Del.Text = "<";
            this.btn_Del.Click += new EventHandler(this.btn_Del_Click);
            this.btn_Dellist.Enabled = false;
            this.btn_Dellist.FlatStyle = FlatStyle.Popup;
            this.btn_Dellist.Location = new Point(240, 0x79);
            this.btn_Dellist.Name = "btn_Dellist";
            this.btn_Dellist.Size = new Size(40, 0x17);
            this.btn_Dellist.TabIndex = 6;
            this.btn_Dellist.Text = "<<";
            this.btn_Dellist.Click += new EventHandler(this.btn_Dellist_Click);
            this.listTable2.ItemHeight = 12;
            this.listTable2.Location = new Point(0x13a, 0x15);
            this.listTable2.Name = "listTable2";
            this.listTable2.Size = new Size(0xac, 0x94);
            this.listTable2.TabIndex = 1;
            this.listTable2.DoubleClick += new EventHandler(this.listTable2_DoubleClick);
            this.listTable1.ItemHeight = 12;
            this.listTable1.Location = new Point(0x10, 0x15);
            this.listTable1.Name = "listTable1";
            this.listTable1.SelectionMode = SelectionMode.MultiExtended;
            this.listTable1.Size = new Size(0xb7, 0x94);
            this.listTable1.TabIndex = 0;
            this.listTable1.DoubleClick += new EventHandler(this.listTable1_DoubleClick);
         //   this.btn_Creat._Image = null;
            this.btn_Creat.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
          //  this.btn_Creat.DefaultScheme = false;
            this.btn_Creat.Image = null;
            this.btn_Creat.Location = new Point(0x134, 0x158);
            this.btn_Creat.Name = "btn_Creat";
          //  this.btn_Creat.Scheme = Button.Schemes.Blue;
            this.btn_Creat.Size = new Size(0x4b, 0x1a);
            this.btn_Creat.TabIndex = 0x2a;
            this.btn_Creat.Text = "生  成";
            this.btn_Creat.Click += new EventHandler(this.btn_Creat_Click);
        //    this.btn_Cancle._Image = null;
            this.btn_Cancle.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
      //      this.btn_Cancle.DefaultScheme = false;
            this.btn_Cancle.Image = null;
            this.btn_Cancle.Location = new Point(0x19c, 0x158);
            this.btn_Cancle.Name = "btn_Cancle";
          //  this.btn_Cancle.Scheme = Button.Schemes.Blue;
            this.btn_Cancle.Size = new Size(0x4b, 0x1a);
            this.btn_Cancle.TabIndex = 0x2a;
            this.btn_Cancle.Text = "取  消";
            this.btn_Cancle.Click += new EventHandler(this.btn_Cancle_Click);
            this.groupBox3.Controls.Add(this.txtTargetFolder);
            this.groupBox3.Controls.Add(this.btn_TargetFold);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new Point(8, 0x40);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x1f7, 0x30);
            this.groupBox3.TabIndex = 0x2b;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "保存";
            this.txtTargetFolder.BorderStyle = BorderStyle.FixedSingle;
            this.txtTargetFolder.Location = new Point(0x48, 0x10);
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.Size = new Size(0x15f, 0x15);
            this.txtTargetFolder.TabIndex = 0x2f;
          //  this.btn_TargetFold._Image = null;
            this.btn_TargetFold.BackColor = Color.FromArgb(0, 0xec, 0xe9, 0xd8);
       //     this.btn_TargetFold.DefaultScheme = false;
            this.btn_TargetFold.Image = null;
            this.btn_TargetFold.Location = new Point(0x1ad, 14);
            this.btn_TargetFold.Name = "btn_TargetFold";
         //   this.btn_TargetFold.Scheme = Button.Schemes.Blue;
            this.btn_TargetFold.Size = new Size(0x39, 0x17);
            this.btn_TargetFold.TabIndex = 0x30;
            this.btn_TargetFold.Text = "选择...";
            this.btn_TargetFold.Click += new EventHandler(this.btn_TargetFold_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 0x12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 0x31;
            this.label2.Text = "文件名：";
            this.pictureBox1.Location = new Point(0xf6, 0x93);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x1c, 0x1c);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0x35;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x20b, 0x17e);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.btn_Creat);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.btn_Cancle);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DbToScript";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "生成SQL数据库脚本";
            base.Load += new EventHandler(this.DbToWord_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((ISupportInitialize) this.pictureBox1).EndInit();
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

        public void SetBtnDisable()
        {
            if (this.btn_Creat.InvokeRequired)
            {
                SetBtnDisableCallback method = new SetBtnDisableCallback(this.SetBtnDisable);
                base.Invoke(method, null);
            }
            else
            {
                this.btn_Creat.Enabled = false;
                this.btn_Cancle.Enabled = false;
            }
        }

        public void SetBtnEnable()
        {
            if (this.btn_Creat.InvokeRequired)
            {
                SetBtnEnableCallback method = new SetBtnEnableCallback(this.SetBtnEnable);
                base.Invoke(method, null);
            }
            else
            {
                this.btn_Creat.Enabled = true;
                this.btn_Cancle.Enabled = true;
            }
        }

        public void SetlblStatuText(string text)
        {
            if (this.labelNum.InvokeRequired)
            {
                SetlblStatuCallback method = new SetlblStatuCallback(this.SetlblStatuText);
                base.Invoke(method, new object[] { text });
            }
            else
            {
                this.labelNum.Text = text;
            }
        }

        public void SetprogressBar1Max(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1MaxCallback method = new SetProBar1MaxCallback(this.SetprogressBar1Max);
                base.Invoke(method, new object[] { val });
            }
            else
            {
                this.progressBar1.Maximum = val;
            }
        }

        public void SetprogressBar1Val(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1ValCallback method = new SetProBar1ValCallback(this.SetprogressBar1Val);
                base.Invoke(method, new object[] { val });
            }
            else
            {
                this.progressBar1.Value = val;
            }
        }

        private void ThreadWork()
        {
            try
            {
                this.SetBtnDisable();
                int count = this.listTable2.Items.Count;
                string text = this.txtTargetFolder.Text;
                this.SetprogressBar1Max(count);
                this.SetprogressBar1Val(1);
                this.SetlblStatuText("0");
                for (int i = 0; i < count; i++)
                {
                    string tablename = this.listTable2.Items[i].ToString();
                    this.dsb.Fieldlist = new List<ColumnInfo>();//new List<Codematic.Library.ColumnInfo>(); 
                    this.dsb.CreateTabScript(this.DbName, tablename, text, this.progressBar2);
                    this.SetprogressBar1Val(i + 1);
                    this.SetlblStatuText((i + 1).ToString());
                }
                this.SetBtnEnable();
                MessageBox.Show("脚本全部生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception exception)
            {
                Log.WriteLog(exception);
                this.SetBtnEnable();
                MessageBox.Show("脚本生成失败！请检查表名是否规范或其他问题导致。(" + exception.Message + ")", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private delegate void SetBtnDisableCallback();

        private delegate void SetBtnEnableCallback();

        private delegate void SetlblStatuCallback(string text);

        private delegate void SetProBar1MaxCallback(int val);

        private delegate void SetProBar1ValCallback(int val);
    }
}


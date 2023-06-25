namespace Codematic
{
    using LTP.CodeBuild;
    using LTP.CodeHelper;
    using LTP.IDBO;
    //using LTP.TextEditor;
    using LTP.Utility;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    public class CodeTemplate : Form
    {
        private Button btn_Run;
        private Button btn_SelAll;
        private Button btn_SelClear;
        private Button btn_SelI;
        private Button btn_SetKey;
        private IContainer components;
        private string dbname;
        private IDbObject dbobj;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ImageList imglistView;
        private ListBox list_KeyField;
        private ListView listView1;
        private MainForm mainfrm;
        private Panel panel1;
        private string servername;
        private Splitter splitter1;
        private ToolStripStatusLabel StatusLabel1;
        private StatusStrip statusStrip1;
        private TabControl tabControl1;
        private string tablename;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Thread thread;
        //private TextEditorControl txtCode;
        //private TextEditorControl txtTemplate;

        private RichTextBox txtCode;
        private RichTextBox txtTemplate; 

        public CodeTemplate()
        {
            this.InitializeComponent();
        }

        public CodeTemplate(Form mdiParentForm)
        {
            this.InitializeComponent();
            this.mainfrm = (MainForm) mdiParentForm;
            this.CreatView();
            this.mainfrm.toolBtn_Run.Visible = true;
            if (((DbView) Application.OpenForms["DbView"]) != null)
            {
                try
                {
                    this.thread = new Thread(new ThreadStart(this.Showlistview));
                    this.thread.Start();
                }
                catch
                {
                }
            }
        }

        private void BindlistViewCol(string Dbname, string TableName)
        {
            List<LTP.CodeHelper.ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(Dbname, TableName);
            if ((columnInfoList != null) && (columnInfoList.Count > 0))
            {
                this.listView1.Items.Clear();
                this.list_KeyField.Items.Clear();
                foreach (LTP.CodeHelper.ColumnInfo info in columnInfoList)
                {
                    string colorder = info.Colorder;
                    string columnName = info.ColumnName;
                    string typeName = info.TypeName;
                    string length = info.Length;
                    string preci = info.Preci;
                    string scale = info.Scale;
                    string defaultVal = info.DefaultVal;
                    string deText = info.DeText;
                    string text = info.IsIdentity ? "√" : "";
                    string str8 = info.IsPK ? "√" : "";
                    string str9 = info.cisNull ? "√" : "";
                    ListViewItem item = new ListViewItem(colorder, 0);
                    item.ImageIndex = 4;
                    item.SubItems.Add(columnName);
                    item.SubItems.Add(typeName);
                    item.SubItems.Add(length);
                    item.SubItems.Add(scale);
                    item.SubItems.Add(text);
                    if ((str8 == "√") && (str9.Trim() == ""))
                    {
                        this.list_KeyField.Items.Add(columnName + "(" + typeName + ")");
                    }
                    else
                    {
                        str8 = "";
                    }
                    item.SubItems.Add(str8);
                    item.SubItems.Add(str9);
                    item.SubItems.Add(defaultVal);
                    this.listView1.Items.AddRange(new ListViewItem[] { item });
                }
            }
            this.btn_SelAll_Click(null, null);
        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            try
            {
                this.Run();
            }
            catch
            {
            }
        }

        private void btn_SelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                }
            }
        }

        private void btn_SelClear_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                item.Checked = false;
            }
        }

        private void btn_SelI_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void btn_SetKey_Click(object sender, EventArgs e)
        {
            this.list_KeyField.Items.Clear();
            foreach (ListViewItem item in this.listView1.Items)
            {
                if (item.Checked)
                {
                    this.list_KeyField.Items.Add(item.SubItems[1].Text + "(" + item.SubItems[2].Text + ")");
                }
            }
        }

        private void CreatView()
        {
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.LargeImageList = this.imglistView;
            this.listView1.SmallImageList = this.imglistView;
            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;
            this.listView1.Columns.Add("序号", 60, HorizontalAlignment.Center);
            this.listView1.Columns.Add("列名", 110, HorizontalAlignment.Left);
            this.listView1.Columns.Add("数据类型", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("长度", 40, HorizontalAlignment.Left);
            this.listView1.Columns.Add("小数", 40, HorizontalAlignment.Left);
            this.listView1.Columns.Add("标识", 40, HorizontalAlignment.Center);
            this.listView1.Columns.Add("主键", 40, HorizontalAlignment.Center);
            this.listView1.Columns.Add("允许空", 60, HorizontalAlignment.Center);
            this.listView1.Columns.Add("默认值", 100, HorizontalAlignment.Left);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<LTP.CodeHelper.ColumnInfo> GetFieldlist()
        {
            DataRow[] rowArray;
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(this.dbobj.GetColumnInfoList(this.dbname, this.tablename));
            StringPlus plus = new StringPlus();
            foreach (ListViewItem item in this.listView1.Items)
            {
                if (item.Checked)
                {
                    plus.Append("'" + item.SubItems[1].Text + "',");
                }
            }
            plus.DelLastComma();
            if (columnInfoDt == null)
            {
                return null;
            }
            if (plus.Value.Length > 0)
            {
                rowArray = columnInfoDt.Select("ColumnName in (" + plus.Value + ")", "colorder asc");
            }
            else
            {
                rowArray = columnInfoDt.Select();
            }
            List<LTP.CodeHelper.ColumnInfo> list2 = new List<LTP.CodeHelper.ColumnInfo>();
            foreach (DataRow row in rowArray)
            {
                string str = row["Colorder"].ToString();
                string str2 = row["ColumnName"].ToString();
                string str3 = row["TypeName"].ToString();
                string str4 = row["IsIdentity"].ToString();
                string str5 = row["IsPK"].ToString();
                string str6 = row["Length"].ToString();
                string str7 = row["Preci"].ToString();
                string str8 = row["Scale"].ToString();
                string str9 = row["cisNull"].ToString();
                string str10 = row["DefaultVal"].ToString();
                string str11 = row["DeText"].ToString();
                LTP.CodeHelper.ColumnInfo info = new LTP.CodeHelper.ColumnInfo();
                info.Colorder = str;
                info.ColumnName = str2;
                info.TypeName = str3;
                info.IsIdentity = str4 == "√";
                info.IsPK = str5 == "√";
                info.Length = str6;
                info.Preci = str7;
                info.Scale = str8;
                info.cisNull = str9 == "√";
                info.DefaultVal = str10;
                info.DeText = str11;
                list2.Add(info);
            }
            return list2;
        }

        private List<LTP.CodeHelper.ColumnInfo> GetKeyFields()
        {
            DataRow[] rowArray;
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(this.dbobj.GetColumnInfoList(this.dbname, this.tablename));
            StringPlus plus = new StringPlus();
            foreach (object obj2 in this.list_KeyField.Items)
            {
                plus.Append("'" + obj2.ToString() + "',");
            }
            plus.DelLastComma();
            if (columnInfoDt == null)
            {
                return null;
            }
            if (plus.Value.Length > 0)
            {
                rowArray = columnInfoDt.Select("ColumnName in (" + plus.Value + ")", "colorder asc");
            }
            else
            {
                rowArray = columnInfoDt.Select();
            }
            List<LTP.CodeHelper.ColumnInfo> list2 = new List<LTP.CodeHelper.ColumnInfo>();
            foreach (DataRow row in rowArray)
            {
                string str = row["Colorder"].ToString();
                string str2 = row["ColumnName"].ToString();
                string str3 = row["TypeName"].ToString();
                string str4 = row["IsIdentity"].ToString();
                string str5 = row["IsPK"].ToString();
                string str6 = row["Length"].ToString();
                string str7 = row["Preci"].ToString();
                string str8 = row["Scale"].ToString();
                string str9 = row["cisNull"].ToString();
                string str10 = row["DefaultVal"].ToString();
                string str11 = row["DeText"].ToString();
                LTP.CodeHelper.ColumnInfo item = new LTP.CodeHelper.ColumnInfo();
                item.Colorder = str;
                item.ColumnName = str2;
                item.TypeName = str3;
                item.IsIdentity = str4 == "√";
                item.IsPK = str5 == "√";
                item.Length = str6;
                item.Preci = str7;
                item.Scale = str8;
                item.cisNull = str9 == "√";
                item.DefaultVal = str10;
                item.DeText = str11;
                list2.Add(item);
            }
            return list2;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(CodeTemplate));
            this.statusStrip1 = new StatusStrip();
            this.StatusLabel1 = new ToolStripStatusLabel();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.panel1 = new Panel();
            this.groupBox1 = new GroupBox();
            this.txtTemplate = new RichTextBox();
            this.groupBox2 = new GroupBox();
            this.list_KeyField = new ListBox();
            this.btn_SetKey = new Button();
            this.btn_SelClear = new Button();
            this.btn_SelI = new Button();
            this.btn_SelAll = new Button();
            this.splitter1 = new Splitter();
            this.listView1 = new ListView();
            this.tabPage2 = new TabPage();
            this.txtCode = new RichTextBox(); 
            this.imglistView = new ImageList(this.components);
            this.btn_Run = new Button();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            base.SuspendLayout();
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.StatusLabel1 });
            this.statusStrip1.Location = new Point(0, 0x1b7);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x254, 0x16);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new Size(0, 0x11);
            this.tabControl1.Alignment = TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Cursor = Cursors.Hand;
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.ImageList = this.imglistView;
            this.tabControl1.ItemSize = new Size(0x3a, 0x13);
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x254, 0x1b7);
            this.tabControl1.TabIndex = 1;
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.RightToLeft = RightToLeft.No;
            this.tabPage1.Size = new Size(0x24c, 0x19c);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "模版";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(3, 0x97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x246, 0x102);
            this.panel1.TabIndex = 11;
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.txtTemplate);
            this.groupBox1.Location = new Point(3, 0x38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x23c, 0xca);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "模版";
            this.txtTemplate.Dock = DockStyle.Fill;
            this.txtTemplate.Location = new Point(3, 0x11);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new Size(0x236, 0xb6);
            this.txtTemplate.TabIndex = 8;
            this.txtTemplate.Text = "";
            this.txtTemplate.MouseUp += new MouseEventHandler(this.txtTemplate_MouseUp);
            //this.txtTemplate.IsIconBarVisible = false;
            //this.txtTemplate.ShowInvalidLines = false;
            //this.txtTemplate.ShowSpaces = false;
            //this.txtTemplate.ShowTabs = false;
            //this.txtTemplate.ShowEOLMarkers = false;
            //this.txtTemplate.ShowVRuler = false;
            //this.txtTemplate.Language = TextEditorControlBase.Languages.XML;
            //this.txtTemplate.Encoding = Encoding.Default;
            this.txtTemplate.Font = new Font("新宋体", 9f);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.list_KeyField);
            this.groupBox2.Controls.Add(this.btn_SetKey);
            this.groupBox2.Controls.Add(this.btn_SelClear);
            this.groupBox2.Controls.Add(this.btn_SelI);
            this.groupBox2.Controls.Add(this.btn_SelAll);
            this.groupBox2.Location = new Point(3, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x23c, 0x34);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.list_KeyField.FormattingEnabled = true;
            this.list_KeyField.ItemHeight = 12;
            this.list_KeyField.Location = new Point(0x1b3, 14);
            this.list_KeyField.Name = "list_KeyField";
            this.list_KeyField.Size = new Size(0x81, 0x1c);
            this.list_KeyField.TabIndex = 1;
            this.btn_SetKey.Location = new Point(0x141, 0x11);
            this.btn_SetKey.Name = "btn_SetKey";
            this.btn_SetKey.Size = new Size(0x6c, 0x17);
            this.btn_SetKey.TabIndex = 0;
            this.btn_SetKey.Text = "主键(条件)字段";
            this.btn_SetKey.UseVisualStyleBackColor = true;
            this.btn_SetKey.Click += new EventHandler(this.btn_SetKey_Click);
            this.btn_SelClear.Location = new Point(0xb2, 0x11);
            this.btn_SelClear.Name = "btn_SelClear";
            this.btn_SelClear.Size = new Size(0x4b, 0x17);
            this.btn_SelClear.TabIndex = 0;
            this.btn_SelClear.Text = "清空";
            this.btn_SelClear.UseVisualStyleBackColor = true;
            this.btn_SelClear.Click += new EventHandler(this.btn_SelClear_Click);
            this.btn_SelI.Location = new Point(0x61, 0x11);
            this.btn_SelI.Name = "btn_SelI";
            this.btn_SelI.Size = new Size(0x4b, 0x17);
            this.btn_SelI.TabIndex = 0;
            this.btn_SelI.Text = "反选";
            this.btn_SelI.UseVisualStyleBackColor = true;
            this.btn_SelI.Click += new EventHandler(this.btn_SelI_Click);
            this.btn_SelAll.Location = new Point(0x10, 0x11);
            this.btn_SelAll.Name = "btn_SelAll";
            this.btn_SelAll.Size = new Size(0x4b, 0x17);
            this.btn_SelAll.TabIndex = 0;
            this.btn_SelAll.Text = "全选";
            this.btn_SelAll.UseVisualStyleBackColor = true;
            this.btn_SelAll.Click += new EventHandler(this.btn_SelAll_Click);
            this.splitter1.Dock = DockStyle.Top;
            this.splitter1.Location = new Point(3, 0x94);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(0x246, 3);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            this.listView1.Dock = DockStyle.Top;
            this.listView1.Location = new Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x246, 0x91);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.tabPage2.Controls.Add(this.txtCode);
            this.tabPage2.ImageIndex = 2;
            this.tabPage2.Location = new Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0x24c, 0x19c);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "代码";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.txtCode.Dock = DockStyle.Fill;
            this.txtCode.Location = new Point(3, 3);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new Size(0x246, 0x196);
            this.txtCode.TabIndex = 0;
            this.txtCode.Text = "";
            //this.txtCode.IsIconBarVisible = false;
            //this.txtCode.ShowInvalidLines = false;
            //this.txtCode.ShowSpaces = false;
            //this.txtCode.ShowTabs = false;
            //this.txtCode.ShowEOLMarkers = false;
            //this.txtCode.ShowVRuler = false;
            //this.txtCode.Language = TextEditorControlBase.Languages.CSHARP;
            //this.txtCode.Encoding = Encoding.Default;
            this.txtCode.Font = new Font("新宋体", 9f);
            //this.imglistView.ImageStream = (ImageListStreamer) manager.GetObject("imglistView.ImageStream");
            //this.imglistView.TransparentColor = Color.Transparent;
            //this.imglistView.Images.SetKeyName(0, "fild2.gif");
            //this.imglistView.Images.SetKeyName(1, "xml.ico");
            //this.imglistView.Images.SetKeyName(2, "file.ico");
            this.btn_Run.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btn_Run.Location = new Point(0x179, 0x1b7);
            this.btn_Run.Name = "btn_Run";
            this.btn_Run.Size = new Size(0x4b, 0x17);
            this.btn_Run.TabIndex = 13;
            this.btn_Run.Text = "生成";
            this.btn_Run.UseVisualStyleBackColor = true;
            this.btn_Run.Visible = false;
            this.btn_Run.Click += new EventHandler(this.btn_Run_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x254, 0x1cd);
            base.Controls.Add(this.btn_Run);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.statusStrip1);
           // base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "CodeTemplate";
            this.Text = "模版代码生成";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void Run()
        {
            this.StatusLabel1.Text = "正在生成...";
            string filename = @"Template\temp.xslt";
            try
            {
                string text = this.txtTemplate.Text;
                if (text.Trim() == "")
                {
                    MessageBox.Show("模版内容为空，请先在模版管理器里选择模版！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.StatusLabel1.Text = "就绪";
                    return;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(text);
                document.Save(filename);
            }
            catch (Exception exception)
            {
                this.StatusLabel1.Text = "模版格式有误：" + exception.Message;
                return;
            }
            string code = "";
            try
            {
                code = new BuilderTemp(this.dbobj, this.dbname, this.tablename, this.GetFieldlist(), this.GetKeyFields(), filename).GetCode();
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                string path = @"Template\temp.xml";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception exception2)
            {
                this.StatusLabel1.Text = "代码转换失败！" + exception2.Message;
                return;
            }
            this.txtCode.Text = code;
            this.tabControl1.SelectedIndex = 1;
            this.StatusLabel1.Text = "生成成功。";
        }

        public void SetListView(DbView dbviewfrm)
        {
            TreeNode selectedNode = dbviewfrm.treeView1.SelectedNode;
            if (selectedNode != null)
            {
                string str = selectedNode.Tag.ToString();
                if (str == null)
                {
                    goto Label_011F;
                }
                if (!(str == "table") && !(str == "view"))
                {
                    if (str == "column")
                    {
                        this.servername = selectedNode.Parent.Parent.Parent.Parent.Text;
                        this.dbname = selectedNode.Parent.Parent.Parent.Text;
                        this.tablename = selectedNode.Parent.Text;
                        this.dbobj = ObjHelper.CreatDbObj(this.servername);
                        this.BindlistViewCol(this.dbname, this.tablename);
                        return;
                    }
                    goto Label_011F;
                }
                this.servername = selectedNode.Parent.Parent.Parent.Text;
                this.dbname = selectedNode.Parent.Parent.Text;
                this.tablename = selectedNode.Text;
                this.dbobj = ObjHelper.CreatDbObj(this.servername);
                this.BindlistViewCol(this.dbname, this.tablename);
            }
            return;
        Label_011F:
            this.listView1.Items.Clear();
        }

        public void SettxtTemplate(string Filename)
        {
            StreamReader reader = new StreamReader(Filename, Encoding.Default);
            string str = reader.ReadToEnd();
            reader.Close();
            this.txtTemplate.Text = str;
        }

        private void Showlistview()
        {
            DbView dbviewfrm = (DbView) Application.OpenForms["DbView"];
            if (dbviewfrm.treeView1.InvokeRequired)
            {
                SetListCallback method = new SetListCallback(this.Showlistview);
                dbviewfrm.Invoke(method, null);
            }
            else
            {
                this.SetListView(dbviewfrm);
            }
        }

        private void txtTemplate_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                sender.ToString();
            }
        }

        private delegate void SetListCallback();
    }
}


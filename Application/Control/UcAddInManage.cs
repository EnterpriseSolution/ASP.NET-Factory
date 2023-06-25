namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    using Flextronics.Applications.ApplicationFactory;
    using Maticsoft.AddInManager;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class UcAddInManage : UserControl
    {
        private AddIn addin = new AddIn();
        private Button btn_Add;
        private Button btnBrow;
        private UcCodeView codeview;
        private IContainer components;
        private ContextMenu contextMenu1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private ListView listView1;
        private MenuItem menu_ShowMain;
        private TabControl tabControlAddIn;
        private TabPage tabPage_Add;
        private TabPage tabPage_Code;
        private TabPage tabPage_List;
        private TextBox txtAssembly;
        private TextBox txtClassname;
        private TextBox txtDecr;
        private TextBox txtFile;
        private TextBox txtName;
        private TextBox txtVersion;

        public UcAddInManage()
        {
            this.InitializeComponent();
            this.codeview = new UcCodeView();
            this.tabPage_Code.Controls.Add(this.codeview);
            this.CreatView();
            this.BindlistView();
            this.LoadAddinFile();
        }

        private void BindlistView()
        {
            DataSet addInList = this.addin.GetAddInList();
            if ((addInList != null) && (addInList.Tables.Count > 0))
            {
                DataTable table = addInList.Tables[0];
                if (table != null)
                {
                    this.listView1.Items.Clear();
                    foreach (DataRow row in table.Rows)
                    {
                        string text = row["Name"].ToString();
                        string str2 = row["Decription"].ToString();
                        string str3 = row["Assembly"].ToString();
                        row["Classname"].ToString();
                        string str4 = row["Version"].ToString();
                        ListViewItem item = new ListViewItem(row["Guid"].ToString(), 0);
                        item.SubItems.Add(text);
                        item.SubItems.Add(str3);
                        item.SubItems.Add(str4);
                        item.SubItems.Add(str2);
                        this.listView1.Items.AddRange(new ListViewItem[] { item });
                    }
                }
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.txtFile.Text;
                if (File.Exists(text))
                {
                    int num = text.LastIndexOf(@"\");
                    if (num > 1)
                    {
                        File.Copy(text, Application.StartupPath + @"\" + text.Substring(num + 1), true);
                    }
                    if (((this.txtClassname.Text.Trim() == "") || (this.txtAssembly.Text.Trim() == "")) || (this.txtName.Text.Trim() == ""))
                    {
                        MessageBox.Show(this, "组件信息不完整，请认真填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        this.addin.Guid = Guid.NewGuid().ToString().ToUpper();
                        this.addin.Classname = this.txtClassname.Text;
                        this.addin.Assembly = this.txtAssembly.Text;
                        this.addin.Decription = this.txtDecr.Text;
                        this.addin.Name = this.txtName.Text;
                        this.addin.Version = this.txtVersion.Text;
                        this.addin.AddAddIn();
                        this.BindlistView();
                        this.LoadAddinFile();
                        MessageBox.Show(this, "组件添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.tabControlAddIn.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "组件添加失败！\r\n请手工复制该文件至安装目录，并修改配置文件即可。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Log.WriteLog(exception);
            }
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "打开sql脚本文件";
                dialog.Filter = "DLL Files (*.dll)|*.dll|All files (*.*)|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = dialog.FileName;
                    this.txtFile.Text = fileName;
                    Assembly assembly = Assembly.LoadFile(fileName);
                    AssemblyName name = assembly.GetName();
                    Version version = name.Version;
                    this.txtAssembly.Text = name.Name;
                    this.txtVersion.Text = version.Major + "." + version.MajorRevision;
                    bool flag = false;
                    foreach (System.Type type in assembly.GetTypes())
                    {
                        if (this.IsValidPlugin(type))
                        {
                            flag = true;
                            this.txtClassname.Text = type.FullName;
                        }
                    }
                    if (!flag)
                    {
                        MessageBox.Show(this, "非标准代码生成插件，请重新选择或改写文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "加载组件失败！\r\n请检查该组件是否符合接口标准或文件是否正确。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Log.WriteLog(exception);
            }
        }

        private void CreatView()
        {
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            this.listView1.FullRowSelect = true;
            this.listView1.Columns.Add("编号", 60, HorizontalAlignment.Left);
            this.listView1.Columns.Add("名称", 150, HorizontalAlignment.Left);
            this.listView1.Columns.Add("程序集", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("版本", 60, HorizontalAlignment.Left);
            this.listView1.Columns.Add("说明", 150, HorizontalAlignment.Left);
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
            this.tabControlAddIn = new TabControl();
            this.tabPage_Add = new TabPage();
            this.btnBrow = new Button();
            this.btn_Add = new Button();
            this.txtFile = new TextBox();
            this.txtVersion = new TextBox();
            this.txtClassname = new TextBox();
            this.txtAssembly = new TextBox();
            this.txtDecr = new TextBox();
            this.txtName = new TextBox();
            this.label6 = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.tabPage_List = new TabPage();
            this.listView1 = new ListView();
            this.contextMenu1 = new ContextMenu();
            this.menu_ShowMain = new MenuItem();
            this.tabPage_Code = new TabPage();
            this.tabPage_Add.SuspendLayout();
            this.tabPage_List.SuspendLayout();
            base.SuspendLayout();
            this.tabControlAddIn.Controls.Add(this.tabPage_Add);
            this.tabControlAddIn.Controls.Add(this.tabPage_List);
            this.tabControlAddIn.Controls.Add(this.tabPage_Code);
            this.tabControlAddIn.Dock = DockStyle.Fill;
            this.tabControlAddIn.Location = new Point(0, 0);
            this.tabControlAddIn.Name = "tabControlAddIn";
            this.tabControlAddIn.SelectedIndex = 0;
            this.tabControlAddIn.Size = new Size(0x170, 0x10b);
            this.tabControlAddIn.TabIndex = 0;
            this.tabPage_Add.Controls.Add(this.btnBrow);
            this.tabPage_Add.Controls.Add(this.btn_Add);
            this.tabPage_Add.Controls.Add(this.txtFile);
            this.tabPage_Add.Controls.Add(this.txtVersion);
            this.tabPage_Add.Controls.Add(this.txtClassname);
            this.tabPage_Add.Controls.Add(this.txtAssembly);
            this.tabPage_Add.Controls.Add(this.txtDecr);
            this.tabPage_Add.Controls.Add(this.txtName);
            this.tabPage_Add.Controls.Add(this.label6);
            this.tabPage_Add.Controls.Add(this.label5);
            this.tabPage_Add.Controls.Add(this.label4);
            this.tabPage_Add.Controls.Add(this.label3);
            this.tabPage_Add.Controls.Add(this.label2);
            this.tabPage_Add.Controls.Add(this.label1);
            this.tabPage_Add.Location = new Point(4, 0x15);
            this.tabPage_Add.Name = "tabPage_Add";
            this.tabPage_Add.Padding = new Padding(3);
            this.tabPage_Add.Size = new Size(360, 0xf2);
            this.tabPage_Add.TabIndex = 1;
            this.tabPage_Add.Text = "增加组件";
            this.tabPage_Add.UseVisualStyleBackColor = true;
            this.btnBrow.Location = new Point(0x10c, 0x10);
            this.btnBrow.Name = "btnBrow";
            this.btnBrow.Size = new Size(0x21, 0x17);
            this.btnBrow.TabIndex = 3;
            this.btnBrow.Text = "...";
            this.btnBrow.UseVisualStyleBackColor = true;
            this.btnBrow.Click += new EventHandler(this.btnBrow_Click);
            this.btn_Add.Location = new Point(0x53, 0xaf);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new Size(0x4b, 0x17);
            this.btn_Add.TabIndex = 2;
            this.btn_Add.Text = "添加";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new EventHandler(this.btn_Add_Click);
            this.txtFile.Location = new Point(0x53, 0x12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new Size(0xb6, 0x15);
            this.txtFile.TabIndex = 1;
            this.txtVersion.Location = new Point(0x53, 0x8f);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new Size(0xb6, 0x15);
            this.txtVersion.TabIndex = 1;
            this.txtClassname.Location = new Point(0x53, 0x76);
            this.txtClassname.Name = "txtClassname";
            this.txtClassname.Size = new Size(0xb6, 0x15);
            this.txtClassname.TabIndex = 1;
            this.txtAssembly.Location = new Point(0x53, 0x5d);
            this.txtAssembly.Name = "txtAssembly";
            this.txtAssembly.Size = new Size(0xb6, 0x15);
            this.txtAssembly.TabIndex = 1;
            this.txtDecr.Location = new Point(0x53, 0x44);
            this.txtDecr.Name = "txtDecr";
            this.txtDecr.Size = new Size(0xb6, 0x15);
            this.txtDecr.TabIndex = 1;
            this.txtName.Location = new Point(0x53, 0x2b);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(0xb6, 0x15);
            this.txtName.TabIndex = 1;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(12, 0x15);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x41, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "选择文件：";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x24, 0x92);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x29, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "版本：";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x24, 0x79);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x29, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "类名：";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x18, 0x60);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x35, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "程序集：";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x24, 0x47);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "说明：";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x24, 0x2e);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称：";
            this.tabPage_List.Controls.Add(this.listView1);
            this.tabPage_List.Location = new Point(4, 0x15);
            this.tabPage_List.Name = "tabPage_List";
            this.tabPage_List.Padding = new Padding(3);
            this.tabPage_List.Size = new Size(360, 0xf2);
            this.tabPage_List.TabIndex = 0;
            this.tabPage_List.Text = "组件列表";
            this.tabPage_List.UseVisualStyleBackColor = true;
            this.listView1.ContextMenu = this.contextMenu1;
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.Location = new Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x162, 0xec);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.menu_ShowMain });
            this.menu_ShowMain.Index = 0;
            this.menu_ShowMain.Text = "删除";
            this.menu_ShowMain.Click += new EventHandler(this.menu_ShowMain_Click);
            this.tabPage_Code.Location = new Point(4, 0x15);
            this.tabPage_Code.Name = "tabPage_Code";
            this.tabPage_Code.Size = new Size(360, 0xf2);
            this.tabPage_Code.TabIndex = 2;
            this.tabPage_Code.Text = "编辑文件";
            this.tabPage_Code.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.Controls.Add(this.tabControlAddIn);
            base.Name = "UcAddInManage";
            base.Size = new Size(0x170, 0x10b);
            this.tabControlAddIn.ResumeLayout(false);
            this.tabPage_Add.ResumeLayout(false);
            this.tabPage_Add.PerformLayout();
            this.tabPage_List.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool IsValidPlugin(System.Type t)
        {
            foreach (System.Type type in t.GetInterfaces())
            {
                if (((type.FullName == "LTP.IBuilder.IBuilderDAL") || (type.FullName == "LTP.IBuilder.IBuilderDALTran")) || (type.FullName == "LTP.IBuilder.IBuilderBLL"))
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadAddinFile()
        {
            string strContent = this.addin.LoadFile();
            this.codeview.SettxtContent("XML", strContent);
        }

        private void menu_ShowMain_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listView1.SelectedItems.Count < 1)
                {
                    MessageBox.Show(this, "请先选择数据项！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    int num = 0;
                    foreach (ListViewItem item in this.listView1.SelectedItems)
                    {
                        string text = item.SubItems[0].Text;
                        this.addin.DeleteAddIn(text);
                        this.listView1.Items.Remove(this.listView1.SelectedItems[0]);
                        num++;
                    }
                    this.BindlistView();
                    this.LoadAddinFile();
                    MessageBox.Show(this, num + "项已删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "删除失败，请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Log.WriteLog(exception);
            }
        }
    }
}


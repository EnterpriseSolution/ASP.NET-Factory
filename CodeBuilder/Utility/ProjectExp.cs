namespace Flextronics.Applications.ApplicationFactory
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;
    

    //public class ProjectExp : Form
    //{
    //    private Button btn_Cancle;
    //    private Button btn_ProFold;
    //    private Button btn_Set;
    //    private Button btn_TargetFold;
    //    private Button btnExp;
    //    private IContainer components;
    //    private ArrayList filearrlist1 = new ArrayList();
    //    private ArrayList filearrlist2 = new ArrayList();
    //    private ArrayList folderarrlist = new ArrayList();
    //    private GroupBox groupBox1;
    //    private GroupBox groupBox2;
    //    private ImageList imageList1;
    //    private Label label1;
    //    private Label label2;
    //    private ListView listView1;
    //    private ProgressBar progressBar1;
    //    public static ProSettings setting = new ProSettings();
    //    private StatusBar statusBar1;
    //    private StatusBarPanel statusBarPanel1;
    //    private TextBox textBox1;
    //    private TextBox textBox2;

    //    public ProjectExp()
    //    {
    //        this.InitializeComponent();
    //    }

    //    private void AddListItem(string SourceDirectory)
    //    {
    //        DirectoryInfo info = new DirectoryInfo(SourceDirectory);
    //        if (info.Exists)
    //        {
    //            DirectoryInfo[] directories = info.GetDirectories();
    //            for (int i = 0; i < directories.Length; i++)
    //            {
    //                ListViewItem item = new ListViewItem("", 0);
    //                string name = directories[i].Name;
    //                item.SubItems.Add(name);
    //                item.Checked = true;
    //                this.listView1.Items.AddRange(new ListViewItem[] { item });
    //            }
    //        }
    //    }

    //    private void btn_Cancle_Click(object sender, EventArgs e)
    //    {
    //        base.Close();
    //    }

    //    private void btn_ProFold_Click(object sender, EventArgs e)
    //    {
    //        FolderBrowserDialog dialog = new FolderBrowserDialog();
    //        if (dialog.ShowDialog(this) == DialogResult.OK)
    //        {
    //            this.textBox1.Text = dialog.SelectedPath;
    //            this.listView1.Items.Clear();
    //            this.AddListItem(this.textBox1.Text.Trim());
    //        }
    //    }

    //    private void btn_Set_Click(object sender, EventArgs e)
    //    {
    //        ProjectSet set = new ProjectSet();
    //        if (set.ShowDialog(this) == DialogResult.OK)
    //        {
    //            this.LoadData();
    //        }
    //    }

    //    private void btn_TargetFold_Click(object sender, EventArgs e)
    //    {
    //        FolderBrowserDialog dialog = new FolderBrowserDialog();
    //        if (dialog.ShowDialog(this) == DialogResult.OK)
    //        {
    //            this.textBox2.Text = dialog.SelectedPath;
    //        }
    //    }

    //    private void btnExp_Click(object sender, EventArgs e)
    //    {
    //        string path = this.textBox1.Text.Trim();
    //        string str2 = this.textBox2.Text.Trim();
    //        if (path == "")
    //        {
    //            MessageBox.Show("请选择项目！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //        }
    //        else if (str2 == "")
    //        {
    //            MessageBox.Show("请选择输出目录！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //        }
    //        else
    //        {
    //            try
    //            {
    //                DirectoryInfo info = new DirectoryInfo(path);
    //                DirectoryInfo info2 = new DirectoryInfo(str2);
    //                if (!info.Exists)
    //                {
    //                    MessageBox.Show("源目录不存在！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //                    return;
    //                }
    //                if (!info2.Exists)
    //                {
    //                    try
    //                    {
    //                        info2.Create();
    //                    }
    //                    catch
    //                    {
    //                        MessageBox.Show("目标目录不存在！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //                        return;
    //                    }
    //                }
    //            }
    //            catch
    //            {
    //                MessageBox.Show("目录信息有误！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //                return;
    //            }
    //            setting.SourceDirectory = path;
    //            setting.TargetDirectory = str2;
    //            ProConfig.SaveSettings(setting);
    //            if ((path != "") && (str2 != ""))
    //            {
    //                this.GetSelFolder();
    //                this.CopyDirectory(path, str2);
    //            }
    //            this.progressBar1.Value = 0;
    //            this.statusBarPanel1.Text = "就绪";
    //            MessageBox.Show("项目成功导出！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //        }
    //    }

    //    public void CopyDirectory(string SourceDirectory, string TargetDirectory)
    //    {
    //        DirectoryInfo info = new DirectoryInfo(SourceDirectory);
    //        DirectoryInfo info2 = new DirectoryInfo(TargetDirectory);
    //        if (info.Exists)
    //        {
    //            if (!info2.Exists)
    //            {
    //                info2.Create();
    //            }
    //            FileInfo[] files = info.GetFiles();
    //            int length = files.Length;
    //            this.progressBar1.Maximum = length;
    //            for (int i = 0; i < length; i++)
    //            {
    //                this.progressBar1.Value = i;
    //                if (setting.Mode == "Filter")
    //                {
    //                    if (this.IsHasitem(files[i].Extension, this.filearrlist1))
    //                    {
    //                        File.Copy(files[i].FullName, info2.FullName + @"\" + files[i].Name, true);
    //                    }
    //                }
    //                else if (!this.IsHasitem(files[i].Extension, this.filearrlist2))
    //                {
    //                    File.Copy(files[i].FullName, info2.FullName + @"\" + files[i].Name, true);
    //                }
    //                this.statusBarPanel1.Text = files[i].FullName;
    //            }
    //            DirectoryInfo[] directories = info.GetDirectories();
    //            for (int j = 0; j < directories.Length; j++)
    //            {
    //                if ((directories[j].Parent.FullName != this.textBox1.Text.Trim()) || this.IsHasitem(directories[j].FullName, this.folderarrlist))
    //                {
    //                    this.CopyDirectory(directories[j].FullName, info2.FullName + @"\" + directories[j].Name);
    //                }
    //            }
    //        }
    //    }

    //    private void CreatListView()
    //    {
    //        this.listView1.Columns.Clear();
    //        this.listView1.Items.Clear();
    //        this.listView1.View = View.Details;
    //        this.listView1.GridLines = true;
    //        this.listView1.FullRowSelect = true;
    //        this.listView1.CheckBoxes = true;
    //        this.listView1.Columns.Add("导出", 50, HorizontalAlignment.Left);
    //        this.listView1.Columns.Add("文件夹名称", 200, HorizontalAlignment.Left);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing && (this.components != null))
    //        {
    //            this.components.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    private void GetSelFolder()
    //    {
    //        string str = this.textBox1.Text.Trim();
    //        foreach (ListViewItem item in this.listView1.Items)
    //        {
    //            if (item.Checked)
    //            {
    //                string str2 = str + @"\" + item.SubItems[1].Text;
    //                this.folderarrlist.Add(str2);
    //            }
    //        }
    //    }

    //    private void InitializeComponent()
    //    {
    //        this.components = new Container();
    //        ResourceManager manager = new ResourceManager(typeof(ProjectExp));
    //        this.groupBox1 = new GroupBox();
    //        this.textBox2 = new TextBox();
    //        this.btn_ProFold = new Button();
    //        this.textBox1 = new TextBox();
    //        this.label1 = new Label();
    //        this.label2 = new Label();
    //        this.btn_TargetFold = new Button();
    //        this.btnExp = new Button();
    //        this.statusBar1 = new StatusBar();
    //        this.statusBarPanel1 = new StatusBarPanel();
    //        this.btn_Set = new Button();
    //        this.groupBox2 = new GroupBox();
    //        this.listView1 = new ListView();
    //        this.imageList1 = new ImageList(this.components);
    //        this.progressBar1 = new ProgressBar();
    //        this.btn_Cancle = new Button();
    //        this.groupBox1.SuspendLayout();
    //        this.statusBarPanel1.BeginInit();
    //        this.groupBox2.SuspendLayout();
    //        base.SuspendLayout();
    //        this.groupBox1.Controls.Add(this.textBox2);
    //        this.groupBox1.Controls.Add(this.btn_ProFold);
    //        this.groupBox1.Controls.Add(this.textBox1);
    //        this.groupBox1.Controls.Add(this.label1);
    //        this.groupBox1.Controls.Add(this.label2);
    //        this.groupBox1.Controls.Add(this.btn_TargetFold);
    //        this.groupBox1.Location = new Point(8, 8);
    //        this.groupBox1.Name = "groupBox1";
    //        this.groupBox1.Size = new Size(440, 0x58);
    //        this.groupBox1.TabIndex = 0;
    //        this.groupBox1.TabStop = false;
    //        this.groupBox1.Text = "路径";
    //        this.textBox2.BorderStyle = BorderStyle.FixedSingle;
    //        this.textBox2.Location = new Point(80, 0x34);
    //        this.textBox2.Name = "textBox2";
    //        this.textBox2.Size = new Size(0x120, 0x15);
    //        this.textBox2.TabIndex = 1;
    //        this.textBox2.Text = "";
    //      //  this.btn_ProFold._Image = null;
    //        this.btn_ProFold.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
    //     //   this.btn_ProFold.DefaultScheme = false;
    //        this.btn_ProFold.DialogResult = DialogResult.None;
    //        this.btn_ProFold.Image = null;
    //        this.btn_ProFold.Location = new Point(0x170, 0x15);
    //        this.btn_ProFold.Name = "btn_ProFold";
    //      //  this.btn_ProFold.Scheme = Button.Schemes.Blue;
    //        this.btn_ProFold.Size = new Size(0x39, 0x17);
    //        this.btn_ProFold.TabIndex = 0x2b;
    //        this.btn_ProFold.Text = "选择...";
    //        this.btn_ProFold.Click += new EventHandler(this.btn_ProFold_Click);
    //        this.textBox1.BorderStyle = BorderStyle.FixedSingle;
    //        this.textBox1.Location = new Point(80, 0x16);
    //        this.textBox1.Name = "textBox1";
    //        this.textBox1.Size = new Size(0x120, 0x15);
    //        this.textBox1.TabIndex = 1;
    //        this.textBox1.Text = "";
    //        this.label1.AutoSize = true;
    //        this.label1.Location = new Point(0x10, 0x18);
    //        this.label1.Name = "label1";
    //        this.label1.Size = new Size(0x42, 0x11);
    //        this.label1.TabIndex = 0;
    //        this.label1.Text = "选择项目：";
    //        this.label2.AutoSize = true;
    //        this.label2.Location = new Point(0x10, 0x36);
    //        this.label2.Name = "label2";
    //        this.label2.Size = new Size(0x42, 0x11);
    //        this.label2.TabIndex = 0;
    //        this.label2.Text = "输出目录：";
    //     //   this.btn_TargetFold._Image = null;
    //        this.btn_TargetFold.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
    //       // this.btn_TargetFold.DefaultScheme = false;
    //        this.btn_TargetFold.DialogResult = DialogResult.None;
    //        this.btn_TargetFold.Image = null;
    //        this.btn_TargetFold.Location = new Point(0x170, 0x33);
    //        this.btn_TargetFold.Name = "btn_TargetFold";
    //     //   this.btn_TargetFold.Scheme = Button.Schemes.Blue;
    //        this.btn_TargetFold.Size = new Size(0x39, 0x17);
    //        this.btn_TargetFold.TabIndex = 0x2b;
    //        this.btn_TargetFold.Text = "选择...";
    //        this.btn_TargetFold.Click += new EventHandler(this.btn_TargetFold_Click);
    //    //    this.btnExp._Image = null;
    //        this.btnExp.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
    //     //   this.btnExp.DefaultScheme = false;
    //        this.btnExp.DialogResult = DialogResult.None;
    //        this.btnExp.Image = null;
    //        this.btnExp.Location = new Point(0xc0, 0x116);
    //        this.btnExp.Name = "btnExp";
    //     //   this.btnExp.Scheme = Button.Schemes.Blue;
    //        this.btnExp.Size = new Size(0x4b, 0x1a);
    //        this.btnExp.TabIndex = 0x2a;
    //        this.btnExp.Text = "导出项目";
    //        this.btnExp.Click += new EventHandler(this.btnExp_Click);
    //        this.statusBar1.Location = new Point(0, 0x13f);
    //        this.statusBar1.Name = "statusBar1";
    //        this.statusBar1.Panels.AddRange(new StatusBarPanel[] { this.statusBarPanel1 });
    //        this.statusBar1.ShowPanels = true;
    //        this.statusBar1.Size = new Size(0x1c8, 0x16);
    //        this.statusBar1.TabIndex = 0x2b;
    //        this.statusBar1.Text = "statusBar1";
    //        this.statusBarPanel1.AutoSize = StatusBarPanelAutoSize.Spring;
    //        this.statusBarPanel1.Text = "就绪";
    //        this.statusBarPanel1.Width = 440;
    //    //    this.btn_Set._Image = null;
    //        this.btn_Set.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
    //    //    this.btn_Set.DefaultScheme = false;
    //        this.btn_Set.DialogResult = DialogResult.None;
    //        this.btn_Set.Image = null;
    //        this.btn_Set.Location = new Point(0x48, 0x116);
    //        this.btn_Set.Name = "btn_Set";
    //     //   this.btn_Set.Scheme = Button.Schemes.Blue;
    //        this.btn_Set.Size = new Size(0x4b, 0x1a);
    //        this.btn_Set.TabIndex = 0x2a;
    //        this.btn_Set.Text = "设  置";
    //        this.btn_Set.Click += new EventHandler(this.btn_Set_Click);
    //        this.groupBox2.Controls.Add(this.listView1);
    //        this.groupBox2.Location = new Point(8, 0x68);
    //        this.groupBox2.Name = "groupBox2";
    //        this.groupBox2.Size = new Size(440, 160);
    //        this.groupBox2.TabIndex = 0x2c;
    //        this.groupBox2.TabStop = false;
    //        this.groupBox2.Text = "目录信息";
    //        this.listView1.Dock = DockStyle.Fill;
    //        this.listView1.Location = new Point(3, 0x11);
    //        this.listView1.Name = "listView1";
    //        this.listView1.Size = new Size(0x1b2, 140);
    //        this.listView1.SmallImageList = this.imageList1;
    //        this.listView1.TabIndex = 0;
    //        this.imageList1.ImageSize = new Size(0x10, 0x10);
    //        this.imageList1.ImageStream = (ImageListStreamer) manager.GetObject("imageList1.ImageStream");
    //        this.imageList1.TransparentColor = Color.Transparent;
    //        this.progressBar1.Location = new Point(0, 0x137);
    //        this.progressBar1.Name = "progressBar1";
    //        this.progressBar1.Size = new Size(440, 10);
    //        this.progressBar1.TabIndex = 0x2d;
    //   //     this.btn_Cancle._Image = null;
    //        this.btn_Cancle.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
    //     //   this.btn_Cancle.DefaultScheme = false;
    //        this.btn_Cancle.DialogResult = DialogResult.Cancel;
    //        this.btn_Cancle.Image = null;
    //        this.btn_Cancle.Location = new Point(0x138, 0x116);
    //        this.btn_Cancle.Name = "btn_Cancle";
    //     //   this.btn_Cancle.Scheme = Button.Schemes.Blue;
    //        this.btn_Cancle.Size = new Size(0x4b, 0x1a);
    //        this.btn_Cancle.TabIndex = 0x2e;
    //        this.btn_Cancle.Text = "取  消";
    //        this.btn_Cancle.Click += new EventHandler(this.btn_Cancle_Click);
    //        this.AutoScaleBaseSize = new Size(6, 14);
    //        base.ClientSize = new Size(0x1c8, 0x155);
    //        base.Controls.Add(this.btn_Cancle);
    //        base.Controls.Add(this.progressBar1);
    //        base.Controls.Add(this.groupBox2);
    //        base.Controls.Add(this.statusBar1);
    //        base.Controls.Add(this.btnExp);
    //        base.Controls.Add(this.groupBox1);
    //        base.Controls.Add(this.btn_Set);
    //        base.Icon = (Icon) manager.GetObject("$this.Icon");
    //        base.MaximizeBox = false;
    //        base.MinimizeBox = false;
    //        base.Name = "ProjectExp";
    //        base.StartPosition = FormStartPosition.CenterScreen;
    //        this.Text = "WEB项目发布";
    //        base.Load += new EventHandler(this.ProjectExp_Load);
    //        this.groupBox1.ResumeLayout(false);
    //        this.statusBarPanel1.EndInit();
    //        this.groupBox2.ResumeLayout(false);
    //        base.ResumeLayout(false);
    //    }

    //    private bool IsHasitem(string str, ArrayList arrlist)
    //    {
    //        for (int i = 0; i < arrlist.Count; i++)
    //        {
    //            if (arrlist[i].ToString() == str)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    private void LoadData()
    //    {
    //        setting = ProConfig.GetSettings();
    //        if (setting != null)
    //        {
    //            foreach (string str in setting.FileExt.Split(new char[] { ';' }))
    //            {
    //                if (str.Trim() != "")
    //                {
    //                    this.filearrlist1.Add(str);
    //                }
    //            }
    //            foreach (string str2 in setting.FileExtDel.Split(new char[] { ';' }))
    //            {
    //                if (str2.Trim() != "")
    //                {
    //                    this.filearrlist2.Add(str2);
    //                }
    //            }
    //            this.textBox1.Text = setting.SourceDirectory;
    //            this.textBox2.Text = setting.TargetDirectory;
    //        }
    //    }

    //    private void ProjectExp_Load(object sender, EventArgs e)
    //    {
    //        this.LoadData();
    //        this.CreatListView();
    //        if (this.textBox1.Text.Trim() != "")
    //        {
    //            this.listView1.Items.Clear();
    //            this.AddListItem(this.textBox1.Text.Trim());
    //        }
    //    }
    //}
}


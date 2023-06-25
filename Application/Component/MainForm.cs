namespace Codematic
{
  
    using Crownwood.Magic.Common;
    using Crownwood.Magic.Controls;
    using Crownwood.Magic.Docking;
    using Crownwood.Magic.Menus;
    using LTP.IDBO;
    using LTP.Utility;
    //using LTPTextEditor.Editor;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
   // using LTP.SplashScrForm;
    //using UpdateApp;

    public class MainForm : Form
    {
        #region 
        private AppSettings appsettings;
        private INIFile cfgfile;
        private string cmcfgfile = (Application.StartupPath + @"\cmcfg.ini");
        private IContainer components;
        private DockingManager DBdockManager;
        private DockingManager dockManager;
        private FrmSearch frmSearch;
        private ImageList leftViewImgs;
        private MenuCommand menuCommand1;
        private MenuCommand menuCommand2;
        private MenuCommand menuCommand3;
        private MenuStrip MenuMain;
        public Mutex mutex;
        private object[] persistedSearchItems;
        public static ModuleSettings setting = new ModuleSettings();
        private StatusStrip statusBar;
        public ToolStripStatusLabel lblViewInfo;
        public ToolStripStatusLabel StatusLabel2;
        public ToolStripStatusLabel StatusLabel3;
        //public Crownwood.Magic.Controls.TabControl tabControlMain;
        private Thread threadUpdate;
        private ImageList toolBarImgs;
        private ToolStrip ToolbarMain;
        private ToolStripButton toolBtn_CreatCode;
        private ToolStripButton toolBtn_DbView;
        private ToolStripButton toolBtn_New;
        public ToolStripButton toolBtn_Run;
        private ToolStripButton toolBtn_SQL;
        public ToolStripButton toolBtn_SQLExe;
        public ToolStripComboBox toolComboBox_DB;
        public ToolStripComboBox toolComboBox_Table;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripSeparator toolStripMenuItem13;
        private ToolStripSeparator toolStripMenuItem15;
        private ToolStripSeparator toolStripMenuItem16;
        public ToolStripSeparator toolStripMenuItem17;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator5;
        private ImageList viewImgs;
        private ToolStripMenuItem 帮助HToolStripMenuItem;
        private ToolStripMenuItem 保存脚本ToolStripMenuItem;
        private ToolStripMenuItem 保存为ToolStripMenuItem;
        private ToolStripMenuItem 编辑ToolStripMenuItem;
        private ToolStripMenuItem 表数据ToolStripMenuItem;
        public ToolStripMenuItem 查询QToolStripMenuItem;
        private ToolStripMenuItem 查询分析器ToolStripMenuItem;
        private ToolStripMenuItem 查找ToolStripMenuItem;
        public ToolStripMenuItem 查找下一个ToolStripMenuItem;
        private ToolStripMenuItem 窗口WToolStripMenuItem;
        private ToolStripMenuItem 存储过程ToolStripMenuItem;
        private ToolStripMenuItem 打开ToolStripMenuItem;
        private ToolStripMenuItem 打开脚本ToolStripMenuItem;
        private ToolStripMenuItem 代码生成器ToolStripMenuItem;
        public ToolStripMenuItem 导出文件ToolStripMenuItem;
        public ToolStripMenuItem 对象定义ToolStripMenuItem;
        private ToolStripMenuItem 访问Maticsoft站点NToolStripMenuItem;
        private ToolStripMenuItem 服务器资源管理器SToolStripMenuItem;
        private ToolStripMenuItem 复制ToolStripMenuItem;
        private ToolStripMenuItem 工具TToolStripMenuItem;
        private ToolStripMenuItem 关闭CToolStripMenuItem;
        private ToolStripMenuItem 关闭所有文档LToolStripMenuItem;
        private ToolStripMenuItem 关于CodematicAToolStripMenuItem;
        private ToolStripMenuItem 恢复ToolStripMenuItem;
        private ToolStripMenuItem 恢复ZToolStripMenuItem;
        private ToolStripMenuItem 脚本片断管理ToolStripMenuItem;
        private ToolStripMenuItem 论坛交流ToolStripMenuItem;
        private ToolStripMenuItem 模版代码生成器ToolStripMenuItem;
        private ToolStripMenuItem 全选AToolStripMenuItem;
        public ToolStripMenuItem 生成ToolStripMenuItem;
        public ToolStripMenuItem 生成存储过程ToolStripMenuItem;
        public ToolStripMenuItem 生成数据脚本ToolStripMenuItem;
        private ToolStripMenuItem 视图VToolStripMenuItem;
        private ToolStripMenuItem 数据脚本ToolStripMenuItem;
        private ToolStripMenuItem 替换ToolStripMenuItem;
        private ToolStripMenuItem 停止查询ToolStripMenuItem;
        private ToolStripMenuItem 退出;
        private ToolStripMenuItem 显示结果窗口ToolStripMenuItem;
        private ToolStripMenuItem 新建ToolStripMenuItem;
        private ToolStripMenuItem 选项OToolStripMenuItem;
        private ToolStripMenuItem 验证当前查询ToolStripMenuItem;
        private ToolStripMenuItem 运行当前查询ToolStripMenuItem;
        private ToolStripMenuItem 粘贴ToolStripMenuItem;
        private ToolStripMenuItem 重置窗口布局ToolStripMenuItem;
        private ToolStripMenuItem 主题ToolStripMenuItem;
        private ToolStripMenuItem 转到定义ToolStripMenuItem;
        private ToolStripMenuItem 转到对象引用ToolStripMenuItem;
        public Crownwood.Magic.Controls.TabControl tabControlMain;
        private ToolStripMenuItem 转到行ToolStripMenuItem;

        #endregion 


        Content databaseView;
        public MainForm()
        {
            this.InitializeComponent();
                  

            this.mutex = new Mutex(false, "SINGLE_INSTANCE_MUTEX");
            if (!this.mutex.WaitOne(0, false))
            {
                this.mutex.Close();
                this.mutex = null;
            }
            //this.Text = "动软.NET代码生成器  V" + Application.ProductVersion;
            this.dockManager = new DockingManager(this, VisualStyle.IDE);
            this.dockManager.OuterControl = this.statusBar;
            this.dockManager.InnerControl = this.tabControlMain;
            this.tabControlMain.IDEPixelBorder = true;
            this.tabControlMain.IDEPixelArea = true;
            #region
            //关闭解决方案和类视图管理窗口
            //Content c = new Content(this.dockManager);
            //c.Control = new SolutionExplorer();
            //Size size = c.Control.Size;
            //c.Title = "解决方案资源管理器";
            //c.FullTitle = "解决方案资源管理器";
            //c.AutoHideSize = size;
            //c.DisplaySize = size;
            //c.ImageList = this.viewImgs;
            //c.ImageIndex = 0;
            //c.PropertyChanged += new Content.PropChangeHandler(this.PropChange);
            //Content content2 = new Content(this.dockManager);
            //content2.Control = new ClassView();
            //Size size2 = content2.Control.Size;
            //content2.Title = "类视图";
            //content2.FullTitle = "类视图";
            //content2.AutoHideSize = size2;
            //content2.DisplaySize = size2;
            //content2.ImageList = this.viewImgs;
            //content2.ImageIndex = 1;
            //this.dockManager.Contents.Add(c);
            //WindowContent wc = this.dockManager.AddContentWithState(c, Crownwood.Magic.Docking.State.DockRight);
            //this.dockManager.Contents.Add(content2);
            //this.dockManager.AddContentToWindowContent(content2, wc);
            //end
            #endregion 
            this.DBdockManager = new DockingManager(this, VisualStyle.IDE);
            this.DBdockManager.OuterControl = this.statusBar;
            this.DBdockManager.InnerControl = this.tabControlMain;
            Content databaseView = new Content(this.DBdockManager);
            //databaseView = new Content(this.DBdockManager);
            databaseView.Control = new DbView(this);
            Size size3 = databaseView.Control.Size;
            databaseView.Title = "数据库视图";
            databaseView.FullTitle = "数据库视图";
            databaseView.AutoHideSize = size3;
            databaseView.DisplaySize = size3;
            databaseView.ImageList = this.leftViewImgs;
            databaseView.ImageIndex = 0;
            #region
            //关闭模板管理窗口
            //Content content5 = new Content(this.DBdockManager);
            //content5.Control = new TempView();
            //Size size4 = content5.Control.Size;
            //content5.Title = "模版管理";
            //content5.FullTitle = "模版管理";
            //content5.AutoHideSize = size4;
            //content5.DisplaySize = size4;
            //content5.ImageList = this.leftViewImgs;
            //content5.ImageIndex = 1;
            this.DBdockManager.Contents.Add(databaseView);
            WindowContent content6 = this.DBdockManager.AddContentWithState(databaseView, Crownwood.Magic.Docking.State.DockLeft);
            //this.DBdockManager.Contents.Add(content5);
            //this.DBdockManager.AddContentToWindowContent(content5, content6);
            this.DBdockManager.AddContentToWindowContent(databaseView, content6);
            //end
            #endregion 
            this.appsettings = AppConfig.GetSettings();
            string appStart = this.appsettings.AppStart;
            if (appStart != null)
            {
                if (!(appStart == "startuppage"))
                {
                    if (!(appStart == "blank") && (appStart == "homepage"))
                    {
                        //string str = "首页";
                        //string url = "http://www.maticsoft.com";
                        //if ((this.appsettings.HomePage != null) && (this.appsettings.HomePage != ""))
                        //{
                        //    url = this.appsettings.HomePage;
                        //}
                        //Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
                        //page.Title = str;
                        //page.Control = new IEView(this, url);
                        //this.tabControlMain.TabPages.Add(page);
                        //this.tabControlMain.SelectedTab = page;
                    }
                }
                else
                {
                    try
                    {
                        this.LoadStartPage();
                    }
                    catch (Exception exception)
                    {
                        LogInfo.WriteLog(exception);
                    }
                }
            }
            this.tabControlMain.MouseUp += new MouseEventHandler(this.OnMouseUpTabPage);
            if (!this.IsHasChecked())
            {
                try
                {
                    this.threadUpdate = new Thread(new ThreadStart(this.ProcUpdate));
                    this.threadUpdate.Start();
                }
                catch (Exception exception2)
                {
                    LogInfo.WriteLog(exception2);
                    MessageBox.Show(exception2.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public void AddNewTabPage(Control control, string Title)
        {
            if (this.tabControlMain.InvokeRequired)
            {
                AddNewTabPageCallback method = new AddNewTabPageCallback(this.AddNewTabPage);
                base.Invoke(method, new object[] { control, Title });
            }
            else
            {
                Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
                page.Title = Title;
                page.Control = control;
                this.tabControlMain.TabPages.Add(page);
                this.tabControlMain.SelectedTab = page;
            }
        }

        public void AddSinglePage(Control control, string Title)
        {
            if (!this.tabControlMain.Visible)
            {
                this.tabControlMain.Visible = true;
            }
            bool flag = false;
            Crownwood.Magic.Controls.TabPage page = null;
            foreach (Crownwood.Magic.Controls.TabPage page2 in this.tabControlMain.TabPages)
            {
                if (page2.Control.Name == control.Name)
                {
                    flag = true;
                    page = page2;
                }
            }
            if (!flag)
            {
                this.AddNewTabPage(control, Title);
            }
            else
            {
                this.tabControlMain.SelectedTab = page;
            }
        }

        public void AddTabPage(string pageTitle, Control ctrForm)
        {
            if (!this.tabControlMain.Visible)
            {
                this.tabControlMain.Visible = true;
            }
            Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
            page.Title = pageTitle;
            page.Control = ctrForm;
            this.tabControlMain.TabPages.Add(page);
            this.tabControlMain.SelectedTab = page;
        }

        public void CheckDbServer()
        {
            if (FormCommon.GetDbViewSelServer() == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void CheckMarker()
        {
            this.cfgfile.IniWriteValue("update", "today", DateTime.Today.ToString("yyyyMMdd"));
        }

        private void c代码转换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void dB脚本生成器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dbViewSelServer = FormCommon.GetDbViewSelServer();
            if (dbViewSelServer == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                new DbToScript(dbViewSelServer).ShowDialog(this);
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

        private bool ExistPage(string CtrName)
        {
            bool flag = false;
            if (!this.tabControlMain.Visible)
            {
                this.tabControlMain.Visible = true;
            }
            foreach (Crownwood.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
            {
                string name = page.Control.Name;
                if (page.Control.Name == CtrName)
                {
                    flag = true;
                }
            }
            return flag;
        }

        private void frmSearch_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                this.persistedSearchItems = this.frmSearch.SearchItems;
            }
            catch
            {
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MenuMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.保存为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.退出 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复ZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.全选AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查找下一个ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.替换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.转到行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.服务器资源管理器SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.运行当前查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.验证当前查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.脚本片断管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.转到定义ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.转到对象引用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成数据脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成存储过程ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.存储过程ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.表数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
            this.对象定义ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询分析器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.代码生成器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模版代码生成器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选项OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.窗口WToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示结果窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.关闭所有文档LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置窗口布局ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.主题ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.论坛交流ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.访问Maticsoft站点NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.关于CodematicAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolbarMain = new System.Windows.Forms.ToolStrip();
            this.toolBtn_New = new System.Windows.Forms.ToolStripButton();
            this.toolBtn_DbView = new System.Windows.Forms.ToolStripButton();
            this.toolBtn_SQL = new System.Windows.Forms.ToolStripButton();
            this.toolBtn_CreatCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtn_SQLExe = new System.Windows.Forms.ToolStripButton();
            this.toolBtn_Run = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolComboBox_DB = new System.Windows.Forms.ToolStripComboBox();
            this.toolComboBox_Table = new System.Windows.Forms.ToolStripComboBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.lblViewInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.viewImgs = new System.Windows.Forms.ImageList(this.components);
            this.toolBarImgs = new System.Windows.Forms.ImageList(this.components);
            this.leftViewImgs = new System.Windows.Forms.ImageList(this.components);
            this.tabControlMain = new Crownwood.Magic.Controls.TabControl();
            this.MenuMain.SuspendLayout();
            this.ToolbarMain.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuMain
            // 
            this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.视图VToolStripMenuItem,
            this.查询QToolStripMenuItem,
            this.生成ToolStripMenuItem,
            this.工具TToolStripMenuItem,
            this.窗口WToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.MenuMain.Location = new System.Drawing.Point(0, 0);
            this.MenuMain.Name = "MenuMain";
            this.MenuMain.Size = new System.Drawing.Size(922, 24);
            this.MenuMain.TabIndex = 5;
            this.MenuMain.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.打开ToolStripMenuItem,
            this.关闭CToolStripMenuItem,
            this.toolStripMenuItem3,
            this.保存为ToolStripMenuItem,
            this.toolStripMenuItem5,
            this.退出});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(57, 20);
            this.toolStripMenuItem1.Text = "文件(&F)";
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.新建ToolStripMenuItem.Text = "新建(&N)";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.数据库连接SToolStripMenuItem_Click);
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.打开ToolStripMenuItem.Text = "打开(&O)";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 关闭CToolStripMenuItem
            // 
            this.关闭CToolStripMenuItem.Name = "关闭CToolStripMenuItem";
            this.关闭CToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.关闭CToolStripMenuItem.Text = "关闭(&C)";
            this.关闭CToolStripMenuItem.Click += new System.EventHandler(this.关闭CToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(133, 6);
            // 
            // 保存为ToolStripMenuItem
            // 
            this.保存为ToolStripMenuItem.Name = "保存为ToolStripMenuItem";
            this.保存为ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.保存为ToolStripMenuItem.Text = "保存为(&S)...";
            this.保存为ToolStripMenuItem.Click += new System.EventHandler(this.保存为ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(133, 6);
            // 
            // 退出
            // 
            this.退出.Name = "退出";
            this.退出.Size = new System.Drawing.Size(136, 22);
            this.退出.Text = "退出(&X)";
            this.退出.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.恢复ZToolStripMenuItem,
            this.toolStripMenuItem16,
            this.编辑ToolStripMenuItem,
            this.复制ToolStripMenuItem,
            this.粘贴ToolStripMenuItem,
            this.恢复ToolStripMenuItem,
            this.toolStripMenuItem6,
            this.全选AToolStripMenuItem,
            this.toolStripMenuItem13,
            this.查找ToolStripMenuItem,
            this.查找下一个ToolStripMenuItem,
            this.替换ToolStripMenuItem,
            this.转到行ToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(57, 20);
            this.toolStripMenuItem2.Text = "编辑(&E)";
            // 
            // 恢复ZToolStripMenuItem
            // 
            this.恢复ZToolStripMenuItem.Name = "恢复ZToolStripMenuItem";
            this.恢复ZToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.恢复ZToolStripMenuItem.Text = "恢复(&Z)";
            this.恢复ZToolStripMenuItem.Click += new System.EventHandler(this.恢复ZToolStripMenuItem_Click);
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(131, 6);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.编辑ToolStripMenuItem.Text = "剪切(&T)";
            this.编辑ToolStripMenuItem.Click += new System.EventHandler(this.剪切ToolStripMenuItem_Click);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.复制ToolStripMenuItem.Text = "复制(&C)";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 粘贴ToolStripMenuItem
            // 
            this.粘贴ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.粘贴ToolStripMenuItem.Name = "粘贴ToolStripMenuItem";
            this.粘贴ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.粘贴ToolStripMenuItem.Text = "粘贴(&P)";
            this.粘贴ToolStripMenuItem.Click += new System.EventHandler(this.粘贴ToolStripMenuItem_Click);
            // 
            // 恢复ToolStripMenuItem
            // 
            this.恢复ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.恢复ToolStripMenuItem.Name = "恢复ToolStripMenuItem";
            this.恢复ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.恢复ToolStripMenuItem.Text = "删除(&D)";
            this.恢复ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(131, 6);
            // 
            // 全选AToolStripMenuItem
            // 
            this.全选AToolStripMenuItem.Name = "全选AToolStripMenuItem";
            this.全选AToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.全选AToolStripMenuItem.Text = "全选(&A)";
            this.全选AToolStripMenuItem.Click += new System.EventHandler(this.全选AToolStripMenuItem_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(131, 6);
            // 
            // 查找ToolStripMenuItem
            // 
            this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
            this.查找ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.查找ToolStripMenuItem.Text = "查找...";
            this.查找ToolStripMenuItem.Click += new System.EventHandler(this.查找ToolStripMenuItem_Click);
            // 
            // 查找下一个ToolStripMenuItem
            // 
            this.查找下一个ToolStripMenuItem.Name = "查找下一个ToolStripMenuItem";
            this.查找下一个ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.查找下一个ToolStripMenuItem.Text = "查找下一个";
            this.查找下一个ToolStripMenuItem.Click += new System.EventHandler(this.查找下一个ToolStripMenuItem_Click);
            // 
            // 替换ToolStripMenuItem
            // 
            this.替换ToolStripMenuItem.Name = "替换ToolStripMenuItem";
            this.替换ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.替换ToolStripMenuItem.Text = "替换";
            this.替换ToolStripMenuItem.Click += new System.EventHandler(this.替换ToolStripMenuItem_Click);
            // 
            // 转到行ToolStripMenuItem
            // 
            this.转到行ToolStripMenuItem.Name = "转到行ToolStripMenuItem";
            this.转到行ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.转到行ToolStripMenuItem.Text = "转到行...";
            this.转到行ToolStripMenuItem.Click += new System.EventHandler(this.转到行ToolStripMenuItem_Click);
            // 
            // 视图VToolStripMenuItem
            // 
            this.视图VToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.服务器资源管理器SToolStripMenuItem});
            this.视图VToolStripMenuItem.Name = "视图VToolStripMenuItem";
            this.视图VToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.视图VToolStripMenuItem.Text = "视图(&V)";
            this.视图VToolStripMenuItem.Click += new System.EventHandler(this.视图VToolStripMenuItem_Click);
            // 
            // 服务器资源管理器SToolStripMenuItem
            // 
            this.服务器资源管理器SToolStripMenuItem.Checked = true;
            this.服务器资源管理器SToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.服务器资源管理器SToolStripMenuItem.Name = "服务器资源管理器SToolStripMenuItem";
            this.服务器资源管理器SToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.服务器资源管理器SToolStripMenuItem.Text = "服务器资源管理器(&S)";
            this.服务器资源管理器SToolStripMenuItem.Click += new System.EventHandler(this.服务器资源管理器SToolStripMenuItem_Click);
            // 
            // 查询QToolStripMenuItem
            // 
            this.查询QToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开脚本ToolStripMenuItem,
            this.保存脚本ToolStripMenuItem,
            this.toolStripMenuItem11,
            this.运行当前查询ToolStripMenuItem,
            this.停止查询ToolStripMenuItem,
            this.验证当前查询ToolStripMenuItem,
            this.toolStripMenuItem12,
            this.脚本片断管理ToolStripMenuItem,
            this.转到定义ToolStripMenuItem,
            this.转到对象引用ToolStripMenuItem});
            this.查询QToolStripMenuItem.Name = "查询QToolStripMenuItem";
            this.查询QToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.查询QToolStripMenuItem.Text = "查询(&Q)";
            this.查询QToolStripMenuItem.Visible = false;
            this.查询QToolStripMenuItem.Click += new System.EventHandler(this.查询QToolStripMenuItem_Click);
            // 
            // 打开脚本ToolStripMenuItem
            // 
            this.打开脚本ToolStripMenuItem.Name = "打开脚本ToolStripMenuItem";
            this.打开脚本ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.打开脚本ToolStripMenuItem.Text = "打开脚本...";
            this.打开脚本ToolStripMenuItem.Click += new System.EventHandler(this.打开脚本ToolStripMenuItem_Click);
            // 
            // 保存脚本ToolStripMenuItem
            // 
            this.保存脚本ToolStripMenuItem.Name = "保存脚本ToolStripMenuItem";
            this.保存脚本ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.保存脚本ToolStripMenuItem.Text = "保存脚本...";
            this.保存脚本ToolStripMenuItem.Click += new System.EventHandler(this.保存脚本ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(143, 6);
            // 
            // 运行当前查询ToolStripMenuItem
            // 
            this.运行当前查询ToolStripMenuItem.Name = "运行当前查询ToolStripMenuItem";
            this.运行当前查询ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.运行当前查询ToolStripMenuItem.Text = "运行当前查询";
            this.运行当前查询ToolStripMenuItem.Click += new System.EventHandler(this.运行当前查询ToolStripMenuItem_Click);
            // 
            // 停止查询ToolStripMenuItem
            // 
            this.停止查询ToolStripMenuItem.Name = "停止查询ToolStripMenuItem";
            this.停止查询ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.停止查询ToolStripMenuItem.Text = "停止查询";
            this.停止查询ToolStripMenuItem.Click += new System.EventHandler(this.停止查询ToolStripMenuItem_Click);
            // 
            // 验证当前查询ToolStripMenuItem
            // 
            this.验证当前查询ToolStripMenuItem.Name = "验证当前查询ToolStripMenuItem";
            this.验证当前查询ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.验证当前查询ToolStripMenuItem.Text = "验证当前查询";
            this.验证当前查询ToolStripMenuItem.Click += new System.EventHandler(this.验证当前查询ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(143, 6);
            // 
            // 脚本片断管理ToolStripMenuItem
            // 
            this.脚本片断管理ToolStripMenuItem.Name = "脚本片断管理ToolStripMenuItem";
            this.脚本片断管理ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.脚本片断管理ToolStripMenuItem.Text = "脚本片断管理";
            // 
            // 转到定义ToolStripMenuItem
            // 
            this.转到定义ToolStripMenuItem.Name = "转到定义ToolStripMenuItem";
            this.转到定义ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.转到定义ToolStripMenuItem.Text = "转到定义";
            this.转到定义ToolStripMenuItem.Click += new System.EventHandler(this.转到定义ToolStripMenuItem_Click);
            // 
            // 转到对象引用ToolStripMenuItem
            // 
            this.转到对象引用ToolStripMenuItem.Name = "转到对象引用ToolStripMenuItem";
            this.转到对象引用ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.转到对象引用ToolStripMenuItem.Text = "转到对象引用";
            this.转到对象引用ToolStripMenuItem.Click += new System.EventHandler(this.转到对象引用ToolStripMenuItem_Click);
            // 
            // 生成ToolStripMenuItem
            // 
            this.生成ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.生成数据脚本ToolStripMenuItem,
            this.生成存储过程ToolStripMenuItem,
            this.导出文件ToolStripMenuItem,
            this.toolStripMenuItem17,
            this.对象定义ToolStripMenuItem});
            this.生成ToolStripMenuItem.Name = "生成ToolStripMenuItem";
            this.生成ToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.生成ToolStripMenuItem.Text = "生成(&C)";
            this.生成ToolStripMenuItem.Visible = false;
            // 
            // 生成数据脚本ToolStripMenuItem
            // 
            this.生成数据脚本ToolStripMenuItem.Name = "生成数据脚本ToolStripMenuItem";
            this.生成数据脚本ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.生成数据脚本ToolStripMenuItem.Text = "生成数据脚本";
            this.生成数据脚本ToolStripMenuItem.Click += new System.EventHandler(this.生成数据脚本ToolStripMenuItem_Click);
            // 
            // 生成存储过程ToolStripMenuItem
            // 
            this.生成存储过程ToolStripMenuItem.Name = "生成存储过程ToolStripMenuItem";
            this.生成存储过程ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.生成存储过程ToolStripMenuItem.Text = "生成存储过程";
            this.生成存储过程ToolStripMenuItem.Click += new System.EventHandler(this.生成存储过程ToolStripMenuItem_Click);
            // 
            // 导出文件ToolStripMenuItem
            // 
            this.导出文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.存储过程ToolStripMenuItem,
            this.数据脚本ToolStripMenuItem,
            this.表数据ToolStripMenuItem});
            this.导出文件ToolStripMenuItem.Name = "导出文件ToolStripMenuItem";
            this.导出文件ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.导出文件ToolStripMenuItem.Text = "导出文件";
            // 
            // 存储过程ToolStripMenuItem
            // 
            this.存储过程ToolStripMenuItem.Name = "存储过程ToolStripMenuItem";
            this.存储过程ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.存储过程ToolStripMenuItem.Text = "存储过程";
            this.存储过程ToolStripMenuItem.Click += new System.EventHandler(this.存储过程ToolStripMenuItem_Click);
            // 
            // 数据脚本ToolStripMenuItem
            // 
            this.数据脚本ToolStripMenuItem.Name = "数据脚本ToolStripMenuItem";
            this.数据脚本ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.数据脚本ToolStripMenuItem.Text = "数据脚本";
            this.数据脚本ToolStripMenuItem.Click += new System.EventHandler(this.数据脚本ToolStripMenuItem_Click);
            // 
            // 表数据ToolStripMenuItem
            // 
            this.表数据ToolStripMenuItem.Name = "表数据ToolStripMenuItem";
            this.表数据ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.表数据ToolStripMenuItem.Text = "表数据";
            this.表数据ToolStripMenuItem.Click += new System.EventHandler(this.表数据ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size(143, 6);
            // 
            // 对象定义ToolStripMenuItem
            // 
            this.对象定义ToolStripMenuItem.Name = "对象定义ToolStripMenuItem";
            this.对象定义ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.对象定义ToolStripMenuItem.Text = "对象定义";
            this.对象定义ToolStripMenuItem.Click += new System.EventHandler(this.对象定义ToolStripMenuItem_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查询分析器ToolStripMenuItem,
            this.代码生成器ToolStripMenuItem,
            this.模版代码生成器ToolStripMenuItem,
            this.选项OToolStripMenuItem});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.工具TToolStripMenuItem.Text = "工具(&T)";
            this.工具TToolStripMenuItem.Click += new System.EventHandler(this.工具TToolStripMenuItem_Click);
            // 
            // 查询分析器ToolStripMenuItem
            // 
            this.查询分析器ToolStripMenuItem.Name = "查询分析器ToolStripMenuItem";
            this.查询分析器ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.查询分析器ToolStripMenuItem.Text = "查询分析器";
            this.查询分析器ToolStripMenuItem.Click += new System.EventHandler(this.查询分析器ToolStripMenuItem_Click);
            // 
            // 代码生成器ToolStripMenuItem
            // 
            this.代码生成器ToolStripMenuItem.Name = "代码生成器ToolStripMenuItem";
            this.代码生成器ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.代码生成器ToolStripMenuItem.Text = "代码生成器";
            this.代码生成器ToolStripMenuItem.Click += new System.EventHandler(this.代码生成器ToolStripMenuItem_Click);
            // 
            // 模版代码生成器ToolStripMenuItem
            // 
            this.模版代码生成器ToolStripMenuItem.Name = "模版代码生成器ToolStripMenuItem";
            this.模版代码生成器ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.模版代码生成器ToolStripMenuItem.Text = "模版代码生成器";
            this.模版代码生成器ToolStripMenuItem.Click += new System.EventHandler(this.模版代码生成器ToolStripMenuItem_Click);
            // 
            // 选项OToolStripMenuItem
            // 
            this.选项OToolStripMenuItem.Name = "选项OToolStripMenuItem";
            this.选项OToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.选项OToolStripMenuItem.Text = "选项(&O)...";
            this.选项OToolStripMenuItem.Visible = false;
            this.选项OToolStripMenuItem.Click += new System.EventHandler(this.选项OToolStripMenuItem_Click);
            // 
            // 窗口WToolStripMenuItem
            // 
            this.窗口WToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示结果窗口ToolStripMenuItem,
            this.toolStripMenuItem8,
            this.关闭所有文档LToolStripMenuItem,
            this.重置窗口布局ToolStripMenuItem});
            this.窗口WToolStripMenuItem.Name = "窗口WToolStripMenuItem";
            this.窗口WToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.窗口WToolStripMenuItem.Text = "窗口(&W)";
            this.窗口WToolStripMenuItem.Visible = false;
            this.窗口WToolStripMenuItem.Click += new System.EventHandler(this.窗口WToolStripMenuItem_Click);
            // 
            // 显示结果窗口ToolStripMenuItem
            // 
            this.显示结果窗口ToolStripMenuItem.Name = "显示结果窗口ToolStripMenuItem";
            this.显示结果窗口ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.显示结果窗口ToolStripMenuItem.Text = "显示结果窗口";
            this.显示结果窗口ToolStripMenuItem.Click += new System.EventHandler(this.显示结果窗口ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(158, 6);
            // 
            // 关闭所有文档LToolStripMenuItem
            // 
            this.关闭所有文档LToolStripMenuItem.Name = "关闭所有文档LToolStripMenuItem";
            this.关闭所有文档LToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.关闭所有文档LToolStripMenuItem.Text = "关闭所有文档(&L)";
            this.关闭所有文档LToolStripMenuItem.Click += new System.EventHandler(this.关闭所有文档LToolStripMenuItem_Click);
            // 
            // 重置窗口布局ToolStripMenuItem
            // 
            this.重置窗口布局ToolStripMenuItem.Name = "重置窗口布局ToolStripMenuItem";
            this.重置窗口布局ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.重置窗口布局ToolStripMenuItem.Text = "重置窗口布局(&R)";
            this.重置窗口布局ToolStripMenuItem.Click += new System.EventHandler(this.重置窗口布局ToolStripMenuItem_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主题ToolStripMenuItem,
            this.论坛交流ToolStripMenuItem,
            this.访问Maticsoft站点NToolStripMenuItem,
            this.toolStripMenuItem15,
            this.关于CodematicAToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            this.帮助HToolStripMenuItem.Visible = false;
            this.帮助HToolStripMenuItem.Click += new System.EventHandler(this.帮助HToolStripMenuItem_Click);
            // 
            // 主题ToolStripMenuItem
            // 
            this.主题ToolStripMenuItem.Name = "主题ToolStripMenuItem";
            this.主题ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.主题ToolStripMenuItem.Text = "在线帮助";
            this.主题ToolStripMenuItem.Click += new System.EventHandler(this.主题ToolStripMenuItem_Click);
            // 
            // 论坛交流ToolStripMenuItem
            // 
            this.论坛交流ToolStripMenuItem.Name = "论坛交流ToolStripMenuItem";
            this.论坛交流ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.论坛交流ToolStripMenuItem.Text = "论坛交流";
            this.论坛交流ToolStripMenuItem.Click += new System.EventHandler(this.论坛交流ToolStripMenuItem_Click);
            // 
            // 访问Maticsoft站点NToolStripMenuItem
            // 
            this.访问Maticsoft站点NToolStripMenuItem.Name = "访问Maticsoft站点NToolStripMenuItem";
            this.访问Maticsoft站点NToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.访问Maticsoft站点NToolStripMenuItem.Text = "访问官方站点";
            this.访问Maticsoft站点NToolStripMenuItem.Click += new System.EventHandler(this.访问Maticsoft站点NToolStripMenuItem_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(165, 6);
            // 
            // 关于CodematicAToolStripMenuItem
            // 
            this.关于CodematicAToolStripMenuItem.Name = "关于CodematicAToolStripMenuItem";
            this.关于CodematicAToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.关于CodematicAToolStripMenuItem.Text = "关于Web Matrix(&A)";
            this.关于CodematicAToolStripMenuItem.Click += new System.EventHandler(this.关于CodematicAToolStripMenuItem_Click);
            // 
            // ToolbarMain
            // 
            this.ToolbarMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtn_New,
            this.toolBtn_DbView,
            this.toolBtn_SQL,
            this.toolBtn_CreatCode,
            this.toolStripSeparator3,
            this.toolBtn_SQLExe,
            this.toolBtn_Run,
            this.toolStripSeparator5,
            this.toolComboBox_DB,
            this.toolComboBox_Table});
            this.ToolbarMain.Location = new System.Drawing.Point(0, 24);
            this.ToolbarMain.Name = "ToolbarMain";
            this.ToolbarMain.Size = new System.Drawing.Size(922, 25);
            this.ToolbarMain.TabIndex = 6;
            this.ToolbarMain.Text = "toolStrip1";
            // 
            // toolBtn_New
            // 
            this.toolBtn_New.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolBtn_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolBtn_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_New.Name = "toolBtn_New";
            this.toolBtn_New.Size = new System.Drawing.Size(71, 22);
            this.toolBtn_New.Text = "数据库连接";
            this.toolBtn_New.Click += new System.EventHandler(this.数据库连接SToolStripMenuItem_Click);
            // 
            // toolBtn_DbView
            // 
            this.toolBtn_DbView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolBtn_DbView.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolBtn_DbView.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtn_DbView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_DbView.Name = "toolBtn_DbView";
            this.toolBtn_DbView.Size = new System.Drawing.Size(83, 22);
            this.toolBtn_DbView.Text = "数据库浏览器";
            this.toolBtn_DbView.Click += new System.EventHandler(this.toolBtn_DbView_Click);
            // 
            // toolBtn_SQL
            // 
            this.toolBtn_SQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolBtn_SQL.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtn_SQL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_SQL.Name = "toolBtn_SQL";
            this.toolBtn_SQL.Size = new System.Drawing.Size(71, 22);
            this.toolBtn_SQL.Text = "查询分析器";
            this.toolBtn_SQL.Click += new System.EventHandler(this.toolBtn_SQL_Click);
            // 
            // toolBtn_CreatCode
            // 
            this.toolBtn_CreatCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolBtn_CreatCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_CreatCode.Name = "toolBtn_CreatCode";
            this.toolBtn_CreatCode.Size = new System.Drawing.Size(71, 22);
            this.toolBtn_CreatCode.Text = "代码生成器";
            this.toolBtn_CreatCode.Click += new System.EventHandler(this.toolBtn_CreatCode_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolBtn_SQLExe
            // 
            this.toolBtn_SQLExe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolBtn_SQLExe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_SQLExe.Name = "toolBtn_SQLExe";
            this.toolBtn_SQLExe.Size = new System.Drawing.Size(68, 22);
            this.toolBtn_SQLExe.Text = "执行SQL(&X)";
            this.toolBtn_SQLExe.Visible = false;
            this.toolBtn_SQLExe.Click += new System.EventHandler(this.toolBtn_SQLExe_Click);
            // 
            // toolBtn_Run
            // 
            this.toolBtn_Run.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolBtn_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_Run.Name = "toolBtn_Run";
            this.toolBtn_Run.Size = new System.Drawing.Size(73, 22);
            this.toolBtn_Run.Text = "生成代码(&B)";
            this.toolBtn_Run.Visible = false;
            this.toolBtn_Run.Click += new System.EventHandler(this.toolBtn_Run_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolComboBox_DB
            // 
            this.toolComboBox_DB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolComboBox_DB.Name = "toolComboBox_DB";
            this.toolComboBox_DB.Size = new System.Drawing.Size(125, 25);
            this.toolComboBox_DB.Visible = false;
            this.toolComboBox_DB.SelectedIndexChanged += new System.EventHandler(this.toolComboBox_DB_SelectedIndexChanged);
            // 
            // toolComboBox_Table
            // 
            this.toolComboBox_Table.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolComboBox_Table.Name = "toolComboBox_Table";
            this.toolComboBox_Table.Size = new System.Drawing.Size(130, 25);
            this.toolComboBox_Table.Visible = false;
            this.toolComboBox_Table.SelectedIndexChanged += new System.EventHandler(this.toolComboBox_Table_SelectedIndexChanged);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblViewInfo,
            this.StatusLabel2,
            this.StatusLabel3});
            this.statusBar.Location = new System.Drawing.Point(0, 424);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(922, 22);
            this.statusBar.TabIndex = 9;
            this.statusBar.Text = "statusStrip1";
            // 
            // lblViewInfo
            // 
            this.lblViewInfo.AutoSize = false;
            this.lblViewInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblViewInfo.Name = "lblViewInfo";
            this.lblViewInfo.Size = new System.Drawing.Size(250, 17);
            this.lblViewInfo.Text = "就绪";
            this.lblViewInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblViewInfo.Click += new System.EventHandler(this.StatusLabel1_Click_1);
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.AutoSize = false;
            this.StatusLabel2.Name = "StatusLabel2";
            this.StatusLabel2.Size = new System.Drawing.Size(150, 17);
            this.StatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusLabel3
            // 
            this.StatusLabel3.Name = "StatusLabel3";
            this.StatusLabel3.Size = new System.Drawing.Size(507, 17);
            this.StatusLabel3.Spring = true;
            this.StatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.StatusLabel3.Click += new System.EventHandler(this.StatusLabel3_Click);
            // 
            // viewImgs
            // 
            this.viewImgs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.viewImgs.ImageSize = new System.Drawing.Size(16, 16);
            this.viewImgs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolBarImgs
            // 
            this.toolBarImgs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.toolBarImgs.ImageSize = new System.Drawing.Size(16, 16);
            this.toolBarImgs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // leftViewImgs
            // 
            this.leftViewImgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("leftViewImgs.ImageStream")));
            this.leftViewImgs.TransparentColor = System.Drawing.Color.Transparent;
            this.leftViewImgs.Images.SetKeyName(0, "Main.Image.png");
            // 
            // tabControlMain
            // 
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
            this.tabControlMain.Location = new System.Drawing.Point(0, 49);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.Size = new System.Drawing.Size(922, 375);
            this.tabControlMain.TabIndex = 11;
            this.tabControlMain.SelectionChanged += new System.EventHandler(this.tabControlMain_SelectionChanged);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(922, 446);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.ToolbarMain);
            this.Controls.Add(this.MenuMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuMain;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".NET代码生成器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MenuMain.ResumeLayout(false);
            this.MenuMain.PerformLayout();
            this.ToolbarMain.ResumeLayout(false);
            this.ToolbarMain.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private bool IsHasChecked()
        {
            if (!System.IO.File.Exists(this.cmcfgfile))
            {
                return false;
            }
            this.cfgfile = new INIFile(this.cmcfgfile);
            return (this.cfgfile.IniReadValue("update", "today") == DateTime.Today.ToString("yyyyMMdd"));
        }

        private void LoadStartPage()
        {
            string startUpPage = this.appsettings.StartUpPage;
            //this.SetStatusText("正在加载起始页...");
            //this.AddSinglePage(new StartPageForm(this, startUpPage), "起始页");
            //this.SetStatusText("完成");
        }

        private void OnCloseTabPage(Crownwood.Magic.Controls.TabPage page)
        {
            string name = page.Control.Name;
            if (name != null)
            {
                if (!(name == "DbQuery"))
                {
                    if (((!(name == "DbBrowser") && !(name == "StartPageForm")) && !(name == "CodeMaker")) && (name == "CodeTemplate"))
                    {
                        this.toolBtn_Run.Visible = false;
                    }
                }
                else
                {
                    this.toolBtn_SQLExe.Visible = false;
                    this.查询QToolStripMenuItem.Visible = false;
                }
            }
            page.Control.Dispose();
        }

        protected void OnColseSelected(object sender, EventArgs e)
        {
            if (this.tabControlMain.TabPages.Count > 0)
            {
                this.OnCloseTabPage(this.tabControlMain.SelectedTab);
                this.tabControlMain.TabPages.Remove(this.tabControlMain.SelectedTab);
                if (this.tabControlMain.TabPages.Count == 0)
                {
                    this.tabControlMain.Visible = false;
                }
            }
        }

        protected void OnColseUnSelected(object sender, EventArgs e)
        {
            if (this.tabControlMain.TabPages.Count > 0)
            {
                ArrayList list = new ArrayList();
                foreach (Crownwood.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    if (page != this.tabControlMain.SelectedTab)
                    {
                        list.Add(page);
                    }
                }
                foreach (Crownwood.Magic.Controls.TabPage page2 in list)
                {
                    this.tabControlMain.TabPages.Remove(page2);
                }
                if (this.tabControlMain.TabPages.Count == 0)
                {
                    this.tabControlMain.Visible = false;
                }
            }
        }

        protected void OnMouseUpTabPage(object sender, MouseEventArgs e)
        {
            if (((this.tabControlMain.TabPages.Count > 0) && (e.Button == MouseButtons.Right)) && this.tabControlMain.SelectedTab.Selected)
            {
                new MenuControl();
                MenuCommand command = new MenuCommand("保存(&S)", new EventHandler(this.OnSaveSelected));
                MenuCommand command2 = new MenuCommand("关闭(&C)", new EventHandler(this.OnColseSelected));
                MenuCommand command3 = new MenuCommand("除此之外全部关闭(&A)", new EventHandler(this.OnColseUnSelected));
                PopupMenu menu = new PopupMenu();
                menu.MenuCommands.AddRange(new MenuCommand[] { command, command2, command3 });
                menu.TrackPopup(this.tabControlMain.PointToScreen(new Point(e.X, e.Y)));
            }
            if (((this.tabControlMain.TabPages.Count > 0) && (e.Button == MouseButtons.Left)) && this.tabControlMain.SelectedTab.Selected)
            {
                this.toolBtn_SQLExe.Visible = false;
                this.toolBtn_Run.Visible = false;
                this.查询QToolStripMenuItem.Visible = false;
                string name = this.tabControlMain.SelectedTab.Control.Name;
                if (name != null)
                {
                    if (!(name == "DbQuery"))
                    {
                        if (((!(name == "DbBrowser") && !(name == "StartPageForm")) && !(name == "CodeMaker")) && (name == "CodeTemplate"))
                        {
                            this.toolBtn_Run.Visible = true;
                        }
                    }
                    else
                    {
                        this.toolBtn_SQLExe.Visible = true;
                        this.查询QToolStripMenuItem.Visible = true;
                    }
                }
            }
        }

        protected void OnSaveSelected(object sender, EventArgs e)
        {
        }

        private void ProcUpdate()
        {
            try
            {
                //Codematic.UpServer.UpServer server = new Codematic.UpServer.UpServer();
                //decimal num = decimal.Parse(UpdateConfig.GetSettings().Version);
                //decimal num2 = decimal.Parse(server.GetVersion());
                //this.CheckMarker();
                //if ((num < num2) && (MessageBox.Show(this, "程序发现新版本，你想现在升级吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes))
                //{
                //    Process.Start(Application.StartupPath + @"\UpdateApp.exe");
                //    base.Close();
                //    Application.Exit();
                //}
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog("ProcUpdate():" + exception.Message);
            }
        }

        public void PropChange(Content obj, Content.Property prop)
        {
        }

        private void SendSetup()
        {
            try
            {
                WebClient client = new WebClient();
                string address = "http://www.maticsoft.com/setup.aspx";
                NameValueCollection data = new NameValueCollection();
                data.Add("SoftName", "Codematic");
                data.Add("Version", Application.ProductVersion);
                data.Add("SQLinfo", "ee-ee-ff-ds");
                byte[] bytes = client.UploadValues(address, "POST", data);
                Encoding.Default.GetString(bytes);
                client.Dispose();
                this.appsettings.Setup = true;
                AppConfig.SaveSettings(this.appsettings);
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog(exception);
            }
        }

        public void SetStatusText(string text)
        {
            this.lblViewInfo.Text = text;
        }

        private void StatusLabel1_Click(object sender, EventArgs e)
        {
        }

        private void toolBtn_CreatCode_Click(object sender, EventArgs e)
        {
            this.CheckDbServer();
            this.AddSinglePage(new CodeMaker(), "代码生成器");
        }

        private void toolBtn_CreatTempCode_Click(object sender, EventArgs e)
        {
            this.CheckDbServer();
            this.AddSinglePage(new CodeTemplate(this), "模版代码生成器");
        }

        private void toolBtn_DbView_Click(object sender, EventArgs e)
        {
            this.AddSinglePage(new DbBrowser(), "摘要");
        }

        private void toolBtn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void toolBtn_OutCode_Click(object sender, EventArgs e)
        {
            string dbViewSelServer = FormCommon.GetDbViewSelServer();
            if (dbViewSelServer == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                new CodeExport(dbViewSelServer).ShowDialog(this);
            }
        }

        private void toolBtn_Run_Click(object sender, EventArgs e)
        {
            if (this.tabControlMain.SelectedTab.Control.Name == "CodeTemplate")
            {
                ((CodeTemplate) this.tabControlMain.SelectedTab.Control).Run();
            }
        }

        private void toolBtn_SQL_Click(object sender, EventArgs e)
        {
            this.AddSinglePage(new DbQuery(this, ""), "查询分析器");
            this.toolBtn_SQLExe.Visible = true;
        }

        private void toolBtn_SQLExe_Click(object sender, EventArgs e)
        {
            if (this.tabControlMain.SelectedTab.Control.Name == "DbQuery")
            {
                ((DbQuery) this.tabControlMain.SelectedTab.Control).RunCurrentQuery();
            }
        }

        private void toolBtn_Web_Click(object sender, EventArgs e)
        {
            //new ProjectExp().Show();
        }

        private void toolBtn_Word_Click(object sender, EventArgs e)
        {
            string dbViewSelServer = FormCommon.GetDbViewSelServer();
            if (dbViewSelServer == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                //new DbToWord(dbViewSelServer).Show();
            }
        }

        private void toolComboBox_DB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dbViewSelServer = FormCommon.GetDbViewSelServer();
            if (dbViewSelServer == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                IDbObject obj2 = ObjHelper.CreatDbObj(dbViewSelServer);
                string text = this.toolComboBox_DB.Text;
                DataTable tabViews = obj2.GetTabViews(text);
                this.toolComboBox_Table.Items.Clear();
                if (tabViews != null)
                {
                    foreach (DataRow row in tabViews.Rows)
                    {
                        string item = row["name"].ToString();
                        this.toolComboBox_Table.Items.Add(item);
                    }
                    if (this.toolComboBox_Table.Items.Count > 0)
                    {
                        this.toolComboBox_Table.SelectedIndex = 0;
                    }
                }
                this.StatusLabel3.Text = "当前库:" + text;
            }
        }

        private void toolComboBox_Table_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void wEB项目发布ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new ProjectExp().Show();
        }

        private void 保存脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "保存当前查询";
                dialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = dialog.FileName;
                    string text = this.ActiveDbQuery.txtContent.Text;
                    StreamWriter writer = new StreamWriter(fileName, false, Encoding.Default);
                    writer.Write(text);
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private void 保存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "保存当前查询";
                dialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = dialog.FileName;
                    string text = this.ActiveDbQuery.txtContent.Text;
                    StreamWriter writer = new StreamWriter(fileName, false, Encoding.Default);
                    writer.Write(text);
                    writer.Flush();
                    writer.Close();
                }
            }
            if (this.ActiveCodeMaker != null)
            {
                CodeMaker activeCodeMaker = this.ActiveCodeMaker;
                SaveFileDialog dialog2 = new SaveFileDialog();
                dialog2.Title = "保存当前代码";
                string str3 = "";
                if (activeCodeMaker.codeview.txtContent_CS.Visible)
                {
                    dialog2.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                    str3 = activeCodeMaker.codeview.txtContent_CS.Text;
                }
                if (activeCodeMaker.codeview.txtContent_SQL.Visible)
                {
                    dialog2.Filter = "SQL files (*.sql)|*.cs|All files (*.*)|*.*";
                    str3 = activeCodeMaker.codeview.txtContent_SQL.Text;
                }
                if (activeCodeMaker.codeview.txtContent_Web.Visible)
                {
                    dialog2.Filter = "Aspx files (*.aspx)|*.cs|All files (*.*)|*.*";
                    str3 = activeCodeMaker.codeview.txtContent_Web.Text;
                }
                if (dialog2.ShowDialog(this) == DialogResult.OK)
                {
                    StreamWriter writer2 = new StreamWriter(dialog2.FileName, false, Encoding.Default);
                    writer2.Write(str3);
                    writer2.Flush();
                    writer2.Close();
                }
            }
            if (this.ActiveCodeEditor != null)
            {
                SaveFileDialog dialog3 = new SaveFileDialog();
                dialog3.Title = "保存当前代码";
                dialog3.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                if (dialog3.ShowDialog(this) == DialogResult.OK)
                {
                    string path = dialog3.FileName;
                    string str6 = this.ActiveCodeEditor.txtContent.Text;
                    StreamWriter writer3 = new StreamWriter(path, false, Encoding.Default);
                    writer3.Write(str6);
                    writer3.Flush();
                    writer3.Close();
                }
            }
        }

        private void 表数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 查询分析器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddSinglePage(new DbQuery(this, ""), "查询分析器");
            this.toolBtn_SQLExe.Visible = true;
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.frmSearch = new FrmSearch(this.ActiveDbQuery);
                this.frmSearch.Closing += new CancelEventHandler(this.frmSearch_Closing);
                this.frmSearch.SearchItems = this.persistedSearchItems;
                this.frmSearch.TopMost = true;
                this.frmSearch.Show();
                this.frmSearch.Focus();
            }
        }

        private void 查找下一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ActiveDbQuery.FindNext();
        }

        private void 窗口WToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 存储过程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 打开脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "打开sql脚本文件";
                dialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    StreamReader reader = new StreamReader(dialog.FileName, Encoding.Default);
                    string str2 = reader.ReadToEnd();
                    reader.Close();
                    this.ActiveDbQuery.txtContent.Text = str2;
                }
            }
        }

        private void 代码生成器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CheckDbServer();
            this.AddSinglePage(new CodeMaker(), "代码生成");
        }

        private void 代码自动输出器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dbViewSelServer = FormCommon.GetDbViewSelServer();
            if (dbViewSelServer == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                new CodeExport(dbViewSelServer).ShowDialog(this);
            }
        }

        private void 对象定义ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 访问Maticsoft站点NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    new Process();
            //    Process.Start("IExplore.exe", "http://www.maticsoft.com");
            //}
            //catch
            //{
            //    MessageBox.Show("请访问：http://www.maticsoft.com", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
        }

        private void 服务器资源管理器SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Content c = this.DBdockManager.Contents["数据库视图"];
            if (this.服务器资源管理器SToolStripMenuItem.Checked)
            {
                this.DBdockManager.HideContent(c);
                this.服务器资源管理器SToolStripMenuItem.Checked = false;
            }
            else
            {
                this.DBdockManager.ShowContent(c);
                this.服务器资源管理器SToolStripMenuItem.Checked = true;
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.Copy();
            }
        }

        private void 关闭CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControlMain.TabPages.Count > 0)
            {
                this.OnCloseTabPage(this.tabControlMain.SelectedTab);
                this.tabControlMain.TabPages.Remove(this.tabControlMain.SelectedTab);
                if (this.tabControlMain.TabPages.Count == 0)
                {
                    this.tabControlMain.Visible = false;
                }
            }
        }

        private void 关闭所有文档LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControlMain.TabPages.Count > 0)
            {
                ArrayList list = new ArrayList();
                foreach (Crownwood.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    list.Add(page);
                }
                foreach (Crownwood.Magic.Controls.TabPage page2 in list)
                {
                    this.tabControlMain.TabPages.Remove(page2);
                }
                if (this.tabControlMain.TabPages.Count == 0)
                {
                    this.tabControlMain.Visible = false;
                }
            }
        }

        private void 关于CodematicAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormAbout().ShowDialog(this);
        }

        private void 恢复ZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.Undo();
            }
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.Cut();
            }
        }

    

        private void 解决方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Content c = this.dockManager.Contents["解决方案资源管理器"];
            //if (this.解决方案ToolStripMenuItem.Checked)
            //{
            //    this.dockManager.HideContent(c);
            //    this.解决方案ToolStripMenuItem.Checked = false;
            //}
            //else
            //{
            //    this.dockManager.ShowContent(c);
            //    this.解决方案ToolStripMenuItem.Checked = true;
            //}
        }

        private void 类视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Content c = this.dockManager.Contents["类视图"];
            //if (this.类视图ToolStripMenuItem.Checked)
            //{
            //    this.dockManager.HideContent(c);
            //    this.类视图ToolStripMenuItem.Checked = false;
            //}
            //else
            //{
            //    this.dockManager.ShowContent(c);
            //    this.类视图ToolStripMenuItem.Checked = true;
            //}
        }

        private void 论坛交流ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    new Process();
            //    Process.Start("IExplore.exe", "http://bbs.maticsoft.com");
            //}
            //catch
            //{
            //    MessageBox.Show("请访问：http://bbs.maticsoft.com", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
        }

        private void 模版代码生成器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CheckDbServer();
            this.AddSinglePage(new CodeTemplate(this), "模版代码生成器");
        }

        private void 模版管理器TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Content c = this.DBdockManager.Contents["模版管理"];
            //if (this.模版管理器TToolStripMenuItem.Checked)
            //{
            //    this.DBdockManager.HideContent(c);
            //    this.模版管理器TToolStripMenuItem.Checked = false;
            //}
            //else
            //{
            //    this.DBdockManager.ShowContent(c);
            //    this.模版管理器TToolStripMenuItem.Checked = true;
            //}
        }

        private void 起始页GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoadStartPage();
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Crownwood.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
            {
                if (page.Selected && (page.Control.Name == "DbQuery"))
                {
                    foreach (Control control in page.Control.Controls)
                    {
                        if ((control.ProductName == "LTPTextEditor") && (control.Name == "txtContent"))
                        {
                           // TextEditorControlWrapper wrapper = (TextEditorControlWrapper) control;
                            RichTextBox wrapper = (RichTextBox)control;
                            wrapper.Select(0, wrapper.Text.Length);
                        }
                    }
                    continue;
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.Cut();
            }
        }

        private void 生成存储过程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 生成数据脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 生成数据库文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dbViewSelServer = FormCommon.GetDbViewSelServer();
            if (dbViewSelServer == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                //new DbToWord(dbViewSelServer).Show();
            }
        }

        private void 数据脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 数据库管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddSinglePage(new DbBrowser(), "摘要");
        }

        private void 数据库连接SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new DbView(this).RegServer();
            Content c = this.DBdockManager.Contents["数据库视图"];
            if (c != null)
            {
                DbView form = c.Control as DbView;
                form.RegServer();
            }
        }

        private void 替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                FrmSearch search = new FrmSearch(this.ActiveDbQuery, true);
                search.Closing += new CancelEventHandler(this.frmSearch_Closing);
                search.SearchItems = this.persistedSearchItems;
                search.Show();
                search.Focus();
            }
        }

        private void 停止查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

     

        private void 显示结果窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

     

        private void 选项OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OptionFrm(this).Show();
        }

        private void 验证当前查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.miValidateCurrentQuery_Click(sender, e);
            }
        }

        private void 运行当前查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.RunCurrentQuery();
            }
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.Paste();
            }
        }

        private void 重置窗口布局ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Content c = this.dockManager.Contents["解决方案资源管理器"];
            //this.dockManager.ShowContent(c);
            //Content content2 = this.dockManager.Contents["类视图"];
            //this.dockManager.ShowContent(content2);
            //this.dockManager.Contents.Add(c);
            //WindowContent wc = this.dockManager.AddContentWithState(c, Crownwood.Magic.Docking.State.DockRight);
            //this.dockManager.Contents.Add(content2);
            //this.dockManager.AddContentToWindowContent(content2, wc);
            Content content4 = this.DBdockManager.Contents["数据库视图"];
            this.DBdockManager.ShowContent(content4);
            this.DBdockManager.Contents.Add(content4);
            this.DBdockManager.AddContentWithState(content4, Crownwood.Magic.Docking.State.DockLeft);
        }

        private void 主题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    new Process();
            //    Process.Start("IExplore.exe", "http://help.maticsoft.com");
            //}
            //catch
            //{
            //    MessageBox.Show("请访问：http://www.maticsoft.com", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
        }

        private void 转到定义ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.GoToDefenition();
            }
        }

        private void 转到对象引用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.GoToReferenceObject();
            }
        }

        private void 转到行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveDbQuery != null)
            {
                this.ActiveDbQuery.GoToLine();
            }
        }

        private CodeEditor ActiveCodeEditor
        {
            get
            {
                foreach (Crownwood.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    if (page.Selected && (page.Control.Name == "CodeEditor"))
                    {
                        foreach (Control control in page.Control.Controls)
                        {
                            if ((control.ProductName == "LTP.TextEditor") && (control.Name == "txtContent"))
                            {
                                return (CodeEditor) page.Control;
                            }
                        }
                        continue;
                    }
                }
                return null;
            }
            set
            {
                this.ActiveCodeEditor = value;
            }
        }

        private CodeMaker ActiveCodeMaker
        {
            get
            {
                foreach (Crownwood.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    if (page.Selected && (page.Control.Name == "CodeMaker"))
                    {
                        foreach (Control control in page.Control.Controls)
                        {
                            if (control.Name == "tabControl1")
                            {
                                return (CodeMaker) page.Control;
                            }
                        }
                        continue;
                    }
                }
                return null;
            }
            set
            {
                this.ActiveCodeMaker = value;
            }
        }

        private DbQuery ActiveDbQuery
        {
            get
            {
                foreach (Crownwood.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    if (page.Selected && (page.Control.Name == "DbQuery"))
                    {
                        foreach (Control control in page.Control.Controls)
                        {
                            if ((control.ProductName == "LTPTextEditor") && (control.Name == "txtContent"))
                            {
                                return (DbQuery) page.Control;
                            }
                        }
                        continue;
                    }
                }
                return null;
            }
            set
            {
                this.ActiveDbQuery = value;
            }
        }

        private delegate void AddNewTabPageCallback(Control control, string Title);

        private delegate void SetStatusCallback(string text);

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            //Content c = dockManager.Contents["数据库视图"];
            //if (c == null)
            //{
            //    //LoadDatabaseExlporer();
            //    //c = dockManager.Contents["数据库视图"];
            //}
            //if (this.toolStripMenuItem18.Checked)
            //{
            //    this.dockManager.HideContent(c);               

            //    this.toolStripMenuItem18.Checked = false;
            //}
            //else
            //{
            //    this.dockManager.ShowContent(c);            
            //    this.toolStripMenuItem18.Checked = true;
            //}

            Content content4 = this.DBdockManager.Contents["数据库视图"];
            if (content4 == null)
            {
                content4 = new Content(this.DBdockManager);
                content4.Control = new DbView(this);
                Size size3 = content4.Control.Size;
                content4.Title = "数据库视图";
                content4.FullTitle = "数据库视图";
                content4.AutoHideSize = size3;
                content4.DisplaySize = size3;
                content4.ImageList = this.leftViewImgs;
                content4.ImageIndex = 0;

                this.DBdockManager.Contents.Add(content4);
                WindowContent content6 = this.DBdockManager.AddContentWithState(content4, Crownwood.Magic.Docking.State.DockLeft);
                this.DBdockManager.AddContentToWindowContent(content4, content6);

            }
            this.DBdockManager.ShowContent(content4);
            this.DBdockManager.Contents.Add(content4);
            this.DBdockManager.AddContentWithState(content4, Crownwood.Magic.Docking.State.DockLeft);
        }    
        
        private void LoadDatabaseExlporer()
        {
            Content databaseView = new Content(this.DBdockManager);            
            databaseView.Control = new DbView(this);
            Size size3 = databaseView.Control.Size;
            databaseView.Title = "数据库视图";
            databaseView.FullTitle = "数据库视图";
            databaseView.AutoHideSize = size3;
            databaseView.DisplaySize = size3;
            databaseView.ImageList = this.leftViewImgs;
            databaseView.ImageIndex = 0;

            this.DBdockManager.Contents.Add(databaseView);
            WindowContent content6 = this.DBdockManager.AddContentWithState(databaseView, Crownwood.Magic.Docking.State.DockLeft);           
            this.DBdockManager.AddContentToWindowContent(databaseView, content6);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //SplashScreen.ShowSplashScreen();
            //Application.DoEvents();
            //SplashScreen.SetStatus("应用程序正在启动...");            
            //SplashScreen.CloseForm();

            //SplashForm frm = new SplashForm();
            //Application.DoEvents();
            //frm.ShowDialog();
            //frm.Close(); 

        }

        private void tabControlMain_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void 帮助HToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 工具TToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 视图VToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 查询QToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void StatusLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void StatusLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}


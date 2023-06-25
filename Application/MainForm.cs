using Junxian.Magic.Common;
using Junxian.Magic.Controls;
using Junxian.Magic.Docking;
using Junxian.Magic.Menus;
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

using Application = System.Windows.Forms.Application;
using Flextronics.Applications.ApplicationFactory;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.Schema;
using WebMatrix.Design;

namespace Flextronics.Applications.ApplicationFactory
{
  
    public  partial class MainForm :  Form   // KryptonForm  
    {

        #region
        private AppSettings appsettings;
        private INIFile cfgfile;
        private string cmcfgfile = (Application.StartupPath + @"\cmcfg.ini");
        private IContainer components;
        private DockingManager DBdockManager;
        private DockingManager dockManager;
        // private FrmSearch frmSearch;
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
        //public Junxian.Magic.Controls.TabControl tabControlMain;
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
        private ToolStripSeparator toolStripMenuItem15;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator5;
        private ImageList viewImgs;
        private ToolStripMenuItem 帮助HToolStripMenuItem;
        private ToolStripMenuItem 保存脚本ToolStripMenuItem;
        private ToolStripMenuItem 保存为ToolStripMenuItem;
        public ToolStripMenuItem 查询QToolStripMenuItem;
        private ToolStripMenuItem 查询分析器ToolStripMenuItem;
        private ToolStripMenuItem 窗口WToolStripMenuItem;
        private ToolStripMenuItem 打开ToolStripMenuItem;
        private ToolStripMenuItem 打开脚本ToolStripMenuItem;
        private ToolStripMenuItem 代码生成器ToolStripMenuItem;
        private ToolStripMenuItem 访问Maticsoft站点NToolStripMenuItem;
        private ToolStripMenuItem 服务器资源管理器SToolStripMenuItem;
        private ToolStripMenuItem 工具TToolStripMenuItem;
        private ToolStripMenuItem 关闭CToolStripMenuItem;
        private ToolStripMenuItem 关闭所有文档LToolStripMenuItem;
        private ToolStripMenuItem 关于CodematicAToolStripMenuItem;
        private ToolStripMenuItem 脚本片断管理ToolStripMenuItem;
        private ToolStripMenuItem 论坛交流ToolStripMenuItem;
        private ToolStripMenuItem 模版代码生成器ToolStripMenuItem;
        private ToolStripMenuItem 视图VToolStripMenuItem;
        private ToolStripMenuItem 停止查询ToolStripMenuItem;
        private ToolStripMenuItem 退出;
        private ToolStripMenuItem 显示结果窗口ToolStripMenuItem;
        private ToolStripMenuItem 新建ToolStripMenuItem;
        private ToolStripMenuItem 选项OToolStripMenuItem;
        private ToolStripMenuItem 验证当前查询ToolStripMenuItem;
        private ToolStripMenuItem 运行当前查询ToolStripMenuItem;
        private ToolStripMenuItem 重置窗口布局ToolStripMenuItem;
        private ToolStripMenuItem 主题ToolStripMenuItem;
        private ToolStripMenuItem 转到定义ToolStripMenuItem;
        private ToolStripMenuItem 转到对象引用ToolStripMenuItem;
        private ToolStripMenuItem 查询分析器ToolStripMenuItem1;
        private ToolStripMenuItem 代码生成器ToolStripMenuItem1;
        private ToolStripMenuItem 工具ToolStripMenuItem;
        public ToolStripMenuItem wCFSupportToolStripMenuItem;
        public Junxian.Magic.Controls.TabControl tabControlMain;

        #endregion 

        //Content databaseView;
        public MainForm()
        {
            this.InitializeComponent();

            ToolStripManager.Renderer = new Office2007Renderer();
            this.mutex = new Mutex(false, "SINGLE_INSTANCE_MUTEX");
            if (!this.mutex.WaitOne(0, false))
            {
                this.mutex.Close();
                this.mutex = null;
            }
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
            //WindowContent wc = this.dockManager.AddContentWithState(c, Junxian.Magic.Docking.State.DockRight);
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
            WindowContent content6 = this.DBdockManager.AddContentWithState(databaseView, Junxian.Magic.Docking.State.DockLeft);
            //this.DBdockManager.Contents.Add(content5);
            //this.DBdockManager.AddContentToWindowContent(content5, content6);
            this.DBdockManager.AddContentToWindowContent(databaseView, content6);
            //end
            #endregion 

            Junxian.Magic.Controls.TabPage page = new Junxian.Magic.Controls.TabPage();
            page.Title = "首页";
            page.Control = new FrmStart();
            this.tabControlMain.TabPages.Add(page);
            this.tabControlMain.SelectedTab = page;

            this.appsettings = AppConfig.GetSettings();
            string appStart = this.appsettings.AppStart;
            //if (appStart != null)
            //{
            //    if (!(appStart == "startuppage"))
            //    {
            //        if (!(appStart == "blank") && (appStart == "homepage"))
            //        {
            //            //string str = "首页";
            //            //string url = "http://www.maticsoft.com";
            //            //if ((this.appsettings.HomePage != null) && (this.appsettings.HomePage != ""))
            //            //{
            //            //    url = this.appsettings.HomePage;
            //            //}
                      
            //        }
            //    }
            //    else
            //    {
                   
            //    }
            //}
            this.tabControlMain.MouseUp += new MouseEventHandler(this.OnMouseUpTabPage);
           
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
                Junxian.Magic.Controls.TabPage page = new Junxian.Magic.Controls.TabPage();
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
            Junxian.Magic.Controls.TabPage page = null;
            foreach (Junxian.Magic.Controls.TabPage page2 in this.tabControlMain.TabPages)
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
            Junxian.Magic.Controls.TabPage page = new Junxian.Magic.Controls.TabPage();
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
                new FrmDbToScript(dbViewSelServer).ShowDialog(this);
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
            foreach (Junxian.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
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
            //try
            //{
            //    this.persistedSearchItems = this.frmSearch.SearchItems;
            //}
            //catch
            //{
            //}
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
            this.视图VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.服务器资源管理器SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询分析器ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.代码生成器ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wCFSupportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tabControlMain = new Junxian.Magic.Controls.TabControl();
            this.MenuMain.SuspendLayout();
            this.ToolbarMain.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuMain
            // 
            this.MenuMain.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.视图VToolStripMenuItem,
            this.查询QToolStripMenuItem,
            this.工具TToolStripMenuItem,
            this.窗口WToolStripMenuItem,
            this.帮助HToolStripMenuItem,
            this.工具ToolStripMenuItem});
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
            this.toolStripMenuItem1.Size = new System.Drawing.Size(55, 20);
            this.toolStripMenuItem1.Text = "文件(&F)";
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.新建ToolStripMenuItem.Text = "新建(&N)";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.数据库连接SToolStripMenuItem_Click);
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.打开ToolStripMenuItem.Text = "打开(&O)";
            this.打开ToolStripMenuItem.Visible = false;
            // 
            // 关闭CToolStripMenuItem
            // 
            this.关闭CToolStripMenuItem.Name = "关闭CToolStripMenuItem";
            this.关闭CToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.关闭CToolStripMenuItem.Text = "关闭(&C)";
            this.关闭CToolStripMenuItem.Click += new System.EventHandler(this.关闭CToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(128, 6);
            // 
            // 保存为ToolStripMenuItem
            // 
            this.保存为ToolStripMenuItem.Name = "保存为ToolStripMenuItem";
            this.保存为ToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.保存为ToolStripMenuItem.Text = "保存为(&S)...";
            this.保存为ToolStripMenuItem.Visible = false;
            this.保存为ToolStripMenuItem.Click += new System.EventHandler(this.保存为ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(128, 6);
            // 
            // 退出
            // 
            this.退出.Name = "退出";
            this.退出.Size = new System.Drawing.Size(131, 22);
            this.退出.Text = "退出(&X)";
            this.退出.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 视图VToolStripMenuItem
            // 
            this.视图VToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.服务器资源管理器SToolStripMenuItem,
            this.查询分析器ToolStripMenuItem1,
            this.代码生成器ToolStripMenuItem1});
            this.视图VToolStripMenuItem.Name = "视图VToolStripMenuItem";
            this.视图VToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.视图VToolStripMenuItem.Text = "视图(&V)";
            // 
            // 服务器资源管理器SToolStripMenuItem
            // 
            this.服务器资源管理器SToolStripMenuItem.Checked = true;
            this.服务器资源管理器SToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.服务器资源管理器SToolStripMenuItem.Name = "服务器资源管理器SToolStripMenuItem";
            this.服务器资源管理器SToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.服务器资源管理器SToolStripMenuItem.Text = "服务器资源管理器(&S)";
            this.服务器资源管理器SToolStripMenuItem.Click += new System.EventHandler(this.服务器资源管理器SToolStripMenuItem_Click);
            // 
            // 查询分析器ToolStripMenuItem1
            // 
            this.查询分析器ToolStripMenuItem1.Name = "查询分析器ToolStripMenuItem1";
            this.查询分析器ToolStripMenuItem1.Size = new System.Drawing.Size(182, 22);
            this.查询分析器ToolStripMenuItem1.Text = "查询分析器";
            this.查询分析器ToolStripMenuItem1.Click += new System.EventHandler(this.查询分析器ToolStripMenuItem_Click);
            // 
            // 代码生成器ToolStripMenuItem1
            // 
            this.代码生成器ToolStripMenuItem1.Name = "代码生成器ToolStripMenuItem1";
            this.代码生成器ToolStripMenuItem1.Size = new System.Drawing.Size(182, 22);
            this.代码生成器ToolStripMenuItem1.Text = "代码生成器";
            this.代码生成器ToolStripMenuItem1.Click += new System.EventHandler(this.代码生成器ToolStripMenuItem_Click);
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
            this.查询QToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.查询QToolStripMenuItem.Text = "查询(&Q)";
            this.查询QToolStripMenuItem.Visible = false;
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
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查询分析器ToolStripMenuItem,
            this.代码生成器ToolStripMenuItem,
            this.模版代码生成器ToolStripMenuItem,
            this.选项OToolStripMenuItem});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.工具TToolStripMenuItem.Text = "工具(&T)";
            this.工具TToolStripMenuItem.Visible = false;
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
            this.模版代码生成器ToolStripMenuItem.Visible = false;
            this.模版代码生成器ToolStripMenuItem.Click += new System.EventHandler(this.模版代码生成器ToolStripMenuItem_Click);
            // 
            // 选项OToolStripMenuItem
            // 
            this.选项OToolStripMenuItem.Name = "选项OToolStripMenuItem";
            this.选项OToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.选项OToolStripMenuItem.Text = "选项(&O)...";
            this.选项OToolStripMenuItem.Visible = false;
            // 
            // 窗口WToolStripMenuItem
            // 
            this.窗口WToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示结果窗口ToolStripMenuItem,
            this.toolStripMenuItem8,
            this.关闭所有文档LToolStripMenuItem,
            this.重置窗口布局ToolStripMenuItem});
            this.窗口WToolStripMenuItem.Name = "窗口WToolStripMenuItem";
            this.窗口WToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.窗口WToolStripMenuItem.Text = "窗口(&W)";
            this.窗口WToolStripMenuItem.Visible = false;
            // 
            // 显示结果窗口ToolStripMenuItem
            // 
            this.显示结果窗口ToolStripMenuItem.Name = "显示结果窗口ToolStripMenuItem";
            this.显示结果窗口ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.显示结果窗口ToolStripMenuItem.Text = "显示结果窗口";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(156, 6);
            // 
            // 关闭所有文档LToolStripMenuItem
            // 
            this.关闭所有文档LToolStripMenuItem.Name = "关闭所有文档LToolStripMenuItem";
            this.关闭所有文档LToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.关闭所有文档LToolStripMenuItem.Text = "关闭所有文档(&L)";
            this.关闭所有文档LToolStripMenuItem.Click += new System.EventHandler(this.关闭所有文档LToolStripMenuItem_Click);
            // 
            // 重置窗口布局ToolStripMenuItem
            // 
            this.重置窗口布局ToolStripMenuItem.Name = "重置窗口布局ToolStripMenuItem";
            this.重置窗口布局ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
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
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            this.帮助HToolStripMenuItem.Visible = false;
            // 
            // 主题ToolStripMenuItem
            // 
            this.主题ToolStripMenuItem.Name = "主题ToolStripMenuItem";
            this.主题ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.主题ToolStripMenuItem.Text = "在线帮助";
            // 
            // 论坛交流ToolStripMenuItem
            // 
            this.论坛交流ToolStripMenuItem.Name = "论坛交流ToolStripMenuItem";
            this.论坛交流ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.论坛交流ToolStripMenuItem.Text = "论坛交流";
            // 
            // 访问Maticsoft站点NToolStripMenuItem
            // 
            this.访问Maticsoft站点NToolStripMenuItem.Name = "访问Maticsoft站点NToolStripMenuItem";
            this.访问Maticsoft站点NToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.访问Maticsoft站点NToolStripMenuItem.Text = "访问官方站点";
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(167, 6);
            // 
            // 关于CodematicAToolStripMenuItem
            // 
            this.关于CodematicAToolStripMenuItem.Name = "关于CodematicAToolStripMenuItem";
            this.关于CodematicAToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.关于CodematicAToolStripMenuItem.Text = "关于Web Matrix(&A)";
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wCFSupportToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // wCFSupportToolStripMenuItem
            // 
            this.wCFSupportToolStripMenuItem.Name = "wCFSupportToolStripMenuItem";
            this.wCFSupportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.wCFSupportToolStripMenuItem.Text = "WCF Support";
            this.wCFSupportToolStripMenuItem.Click += new System.EventHandler(this.wCFSupportToolStripMenuItem_Click);
            // 
            // ToolbarMain
            // 
            this.ToolbarMain.Font = new System.Drawing.Font("Segoe UI", 8.25F);
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
            this.toolBtn_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtn_New.Image = global::WebMatrix.Properties.Resources.toolbtn_AddServer_Image;
            this.toolBtn_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_New.Name = "toolBtn_New";
            this.toolBtn_New.Size = new System.Drawing.Size(23, 22);
            this.toolBtn_New.Text = "数据库连接";
            this.toolBtn_New.Click += new System.EventHandler(this.数据库连接SToolStripMenuItem_Click);
            // 
            // toolBtn_DbView
            // 
            this.toolBtn_DbView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtn_DbView.Image = global::WebMatrix.Properties.Resources.toolBtn_DbView_Image;
            this.toolBtn_DbView.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolBtn_DbView.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtn_DbView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_DbView.Name = "toolBtn_DbView";
            this.toolBtn_DbView.Size = new System.Drawing.Size(23, 22);
            this.toolBtn_DbView.Text = "数据库浏览器";
            this.toolBtn_DbView.Click += new System.EventHandler(this.toolBtn_DbView_Click);
            // 
            // toolBtn_SQL
            // 
            this.toolBtn_SQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtn_SQL.Image = global::WebMatrix.Properties.Resources._128_0;
            this.toolBtn_SQL.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtn_SQL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_SQL.Name = "toolBtn_SQL";
            this.toolBtn_SQL.Size = new System.Drawing.Size(23, 22);
            this.toolBtn_SQL.Text = "查询分析器";
            this.toolBtn_SQL.Click += new System.EventHandler(this.toolBtn_SQL_Click);
            // 
            // toolBtn_CreatCode
            // 
            this.toolBtn_CreatCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtn_CreatCode.Image = global::WebMatrix.Properties.Resources.toolBtn_NewProject_Image;
            this.toolBtn_CreatCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_CreatCode.Name = "toolBtn_CreatCode";
            this.toolBtn_CreatCode.Size = new System.Drawing.Size(23, 22);
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
            this.toolBtn_SQLExe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtn_SQLExe.Image = global::WebMatrix.Properties.Resources.ToolStripMenuItem_Image;
            this.toolBtn_SQLExe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_SQLExe.Name = "toolBtn_SQLExe";
            this.toolBtn_SQLExe.Size = new System.Drawing.Size(23, 22);
            this.toolBtn_SQLExe.Text = "执行SQL(&X)";
            this.toolBtn_SQLExe.Visible = false;
            this.toolBtn_SQLExe.Click += new System.EventHandler(this.toolBtn_SQLExe_Click);
            // 
            // toolBtn_Run
            // 
            this.toolBtn_Run.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtn_Run.Image = global::WebMatrix.Properties.Resources.toolBtn_Run_Image;
            this.toolBtn_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn_Run.Name = "toolBtn_Run";
            this.toolBtn_Run.Size = new System.Drawing.Size(23, 22);
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
            this.statusBar.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblViewInfo,
            this.StatusLabel2,
            this.StatusLabel3});
            this.statusBar.Location = new System.Drawing.Point(0, 424);
            this.statusBar.Name = "statusBar";
            this.statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
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
            // 
            // viewImgs
            // 
            this.viewImgs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.viewImgs.ImageSize = new System.Drawing.Size(16, 16);
            this.viewImgs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolBarImgs
            // 
            this.toolBarImgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolBarImgs.ImageStream")));
            this.toolBarImgs.TransparentColor = System.Drawing.Color.Transparent;
            this.toolBarImgs.Images.SetKeyName(0, "Add.ico");
            this.toolBarImgs.Images.SetKeyName(1, "Windows Magnifier.ico");
            this.toolBarImgs.Images.SetKeyName(2, "Monitor.ico");
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
            this.tabControlMain.IDE2005Style = Junxian.Magic.Controls.IDE2005Style.Enhanced;
            this.tabControlMain.Location = new System.Drawing.Point(0, 49);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.OfficeDockSides = false;
            this.tabControlMain.ShowDropSelect = false;
            this.tabControlMain.Size = new System.Drawing.Size(922, 375);
            this.tabControlMain.Style = Junxian.Magic.Common.VisualStyle.IDE2005;
            this.tabControlMain.TabIndex = 11;
            this.tabControlMain.TextTips = true;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(922, 446);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.ToolbarMain);
            this.Controls.Add(this.MenuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuMain;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASP.NET 应用代码生成器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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

      

        private void OnCloseTabPage(Junxian.Magic.Controls.TabPage page)
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
                foreach (Junxian.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    if (page != this.tabControlMain.SelectedTab)
                    {
                        list.Add(page);
                    }
                }
                foreach (Junxian.Magic.Controls.TabPage page2 in list)
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

      

        public void PropChange(Content obj, Content.Property prop)
        {
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
            bool WCF = wCFSupportToolStripMenuItem.Checked;
            this.AddSinglePage(new FrmCodeMaker(this), "代码生成器");
        }

        private void toolBtn_CreatTempCode_Click(object sender, EventArgs e)
        {
            this.CheckDbServer();
            this.AddSinglePage(new FrmCodeTemplate2(this), "模版代码生成器");
        }

        private void toolBtn_DbView_Click(object sender, EventArgs e)
        {
            this.AddSinglePage(new DbBrowser(), "数据库");
        }

        private void toolBtn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void toolBtn_OutCode_Click(object sender, EventArgs e)
        {
            //string dbViewSelServer = FormCommon.GetDbViewSelServer();
            //if (dbViewSelServer == "")
            //{
            //    MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
            //else
            //{
            //    new CodeExport(dbViewSelServer).ShowDialog(this);
            //}
        }

        private void toolBtn_Run_Click(object sender, EventArgs e)
        {
            if (this.tabControlMain.SelectedTab.Control.Name == "CodeTemplate")
            {
                ((FrmCodeTemplate) this.tabControlMain.SelectedTab.Control).Run();
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

        private void toolComboBox_DB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dbViewSelServer = FormCommon.GetDbViewSelServer();
            if (dbViewSelServer == "")
            {
                MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                IDbObject obj2 = ObjectHelper.CreatDbObj(dbViewSelServer);
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
                //this.StatusLabel3.Text = "当前库:" + text;
                this.StatusLabel3.Text = text;
            }
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
                //FrmCodeMaker activeCodeMaker = this.ActiveCodeMaker;
                //SaveFileDialog dialog2 = new SaveFileDialog();
                //dialog2.Title = "保存当前代码";
                //string str3 = "";
                //if (activeCodeMaker.codeview.txtContent_CS.Visible)
                //{
                //    dialog2.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                //    str3 = activeCodeMaker.codeview.txtContent_CS.Text;
                //}
                //if (activeCodeMaker.codeview.txtContent_SQL.Visible)
                //{
                //    dialog2.Filter = "SQL files (*.sql)|*.cs|All files (*.*)|*.*";
                //    str3 = activeCodeMaker.codeview.txtContent_SQL.Text;
                //}
                //if (activeCodeMaker.codeview.txtContent_Web.Visible)
                //{
                //    dialog2.Filter = "Aspx files (*.aspx)|*.cs|All files (*.*)|*.*";
                //    str3 = activeCodeMaker.codeview.txtContent_Web.Text;
                //}
                //if (dialog2.ShowDialog(this) == DialogResult.OK)
                //{
                //    StreamWriter writer2 = new StreamWriter(dialog2.FileName, false, Encoding.Default);
                //    writer2.Write(str3);
                //    writer2.Flush();
                //    writer2.Close();
                //}
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
        private void 查询分析器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddSinglePage(new DbQuery(this, ""), "查询分析器");
            this.toolBtn_SQLExe.Visible = true;
        }
        private void 查找下一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.ActiveDbQuery.FindNext();
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
            this.AddSinglePage(new FrmCodeMaker(this), "代码生成");
        }

        private void 代码自动输出器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string dbViewSelServer = FormCommon.GetDbViewSelServer();
            //if (dbViewSelServer == "")
            //{
            //    MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
            //else
            //{
            //    new CodeExport(dbViewSelServer).ShowDialog(this);
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
                foreach (Junxian.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    list.Add(page);
                }
                foreach (Junxian.Magic.Controls.TabPage page2 in list)
                {
                    this.tabControlMain.TabPages.Remove(page2);
                }
                if (this.tabControlMain.TabPages.Count == 0)
                {
                    this.tabControlMain.Visible = false;
                }
            }
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
   
        private void 模版代码生成器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CheckDbServer();
            this.AddSinglePage(new FrmCodeTemplate2(this), "模版代码生成器");
        }
           
        private void 起始页GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Junxian.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
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
      
        private void 数据库管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddSinglePage(new DbBrowser(), "摘要");
        }

        private void 数据库连接SToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            Content c = this.DBdockManager.Contents["数据库视图"];
            if (c != null)
            {
                DbView form = c.Control as DbView;
                form.RegServer();
            }
        }       
      
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            Content content4 = this.DBdockManager.Contents["数据库视图"];
            this.DBdockManager.ShowContent(content4);
            this.DBdockManager.Contents.Add(content4);
            this.DBdockManager.AddContentWithState(content4, Junxian.Magic.Docking.State.DockLeft);
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
                foreach (Junxian.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
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

        private FrmCodeMaker ActiveCodeMaker
        {
            get
            {
                foreach (Junxian.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
                {
                    if (page.Selected && (page.Control.Name == "FrmCodeMaker"))
                    {
                        foreach (Control control in page.Control.Controls)
                        {
                            if (control.Name == "tabControl1")
                            {
                                return (FrmCodeMaker) page.Control;
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
                foreach (Junxian.Magic.Controls.TabPage page in this.tabControlMain.TabPages)
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
                WindowContent content6 = this.DBdockManager.AddContentWithState(content4, Junxian.Magic.Docking.State.DockLeft);
                this.DBdockManager.AddContentToWindowContent(content4, content6);

            }
            this.DBdockManager.ShowContent(content4);
            this.DBdockManager.Contents.Add(content4);
            this.DBdockManager.AddContentWithState(content4, Junxian.Magic.Docking.State.DockLeft);
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
            WindowContent content6 = this.DBdockManager.AddContentWithState(databaseView, Junxian.Magic.Docking.State.DockLeft);           
            this.DBdockManager.AddContentToWindowContent(databaseView, content6);
        }

        private void toolComboBox_Table_SelectedIndexChanged(object sender, EventArgs e)
        {
            string db = toolComboBox_DB.Text;
            string table = toolComboBox_Table.Text;            
            FrmCodeMaker activeCodeMaker = ActiveCodeMaker;
            if(activeCodeMaker!=null)
            {
                activeCodeMaker.BindlistViewCol(db, table);
                activeCodeMaker.dbname = db;
                activeCodeMaker.tablename = table;
            }
        }

        private void wCFSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wCFSupportToolStripMenuItem.Checked)
                wCFSupportToolStripMenuItem.Checked = false;
            else if (wCFSupportToolStripMenuItem.Checked == false)
                wCFSupportToolStripMenuItem.Checked = true;
        }

    

    }

    public partial class MainForm
    {
        
    }
}


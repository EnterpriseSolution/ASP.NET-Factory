namespace Codematic
{
    using Crownwood.Magic.Controls;
    using LTP.CodeBuild;
    using LTP.CodeHelper;
    using LTP.DBFactory;
    using LTP.IDBO;
  //  using LTP.SplashScrForm;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using WebMatrix.Component;

    public class DbView : Form
    {
        private ToolStripMenuItem adfsToolStripMenuItem;
        private IContainer components;
        private IDbObject dbobj;
        private ContextMenuStrip DbTreeContextMenu;
        public static bool isMdb;
       // private LoginMySQL loginMysql = new LoginMySQL();
      //  private LoginMySQL loginMysql;

        private LoginForm logo = new LoginForm();
      //  private LoginOledb logoOledb = new LoginOledb();
   //     private LoginOra logoOra = new LoginOra();
        private bool m_bLayoutCalled;
        private MainForm mainfrm;
        private string path = Application.StartupPath;
        private TreeNode serverlistNode;
        private ModuleSettings setting;
        private ToolStripButton toolbtn_AddServer;
        private ToolStripButton toolbtn_Connect;
        private ToolStripButton toolbtn_Refrush;
        private ToolStripButton toolbtn_unConnect;
        private ToolStrip toolStrip1;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private TreeNode TreeClickNode;
        private ImageList treeImgs;
        public TreeView treeView1;
        private ImageList imageList1;
        private ToolStripMenuItem 添加服务器ToolStripMenuItem;

        public DbView(Form mdiParentForm)
        {
            this.mainfrm = (MainForm) mdiParentForm;
            this.InitializeComponent();
            this.treeView1.ExpandAll();
            //DbView_Load(this,null);
        }

        private void AddSinglePage(Control control, string Title)
        {
            if (!this.mainfrm.tabControlMain.Visible)
            {
                this.mainfrm.tabControlMain.Visible = true;
            }
            bool flag = false;
            Crownwood.Magic.Controls.TabPage page = null;
            foreach (Crownwood.Magic.Controls.TabPage page2 in this.mainfrm.tabControlMain.TabPages)
            {
                if (page2.Control.Name == control.Name)
                {
                    flag = true;
                    page = page2;
                }
            }
            if (!flag)
            {
                this.AddTabPage(Title, control);
            }
            else
            {
                this.mainfrm.tabControlMain.SelectedTab = page;
            }
        }

        private void AddTabPage(string pageTitle, Control ctrForm)
        {
            if (!this.mainfrm.tabControlMain.Visible)
            {
                this.mainfrm.tabControlMain.Visible = true;
            }
            Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
            page.Title = pageTitle;
            page.Control = ctrForm;
            this.mainfrm.tabControlMain.TabPages.Add(page);
            this.mainfrm.tabControlMain.SelectedTab = page;
        }

        private void AddTabPage(string pageTitle, Control ctrForm, MainForm mainfrm)
        {
            if (!mainfrm.tabControlMain.Visible)
            {
                mainfrm.tabControlMain.Visible = true;
            }
            Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
            page.Title = pageTitle;
            page.Control = ctrForm;
            mainfrm.tabControlMain.TabPages.Add(page);
            mainfrm.tabControlMain.SelectedTab = page;
        }

        private void ConnectServer(TreeNode serverNode, string dbtype, string ServerIp, string DbName, bool ConnectSimple)
        {
            IDbObject obj2 = DBOMaker.CreateDbObj(dbtype);
            //this.mainfrm.StatusLabel1.Text = "加载数据库树...";
            //SplashScreen.ShowSplashScreen();
            //Application.DoEvents();
            //SplashScreen.SetStatus("加载数据库树...");
            DbSettings settings = DbConfig.GetSetting(dbtype, ServerIp, DbName);
            obj2.DbConnectStr = settings.ConnectStr;
            serverNode.Nodes.Clear();
            if ((dbtype == "SQL2000") || (dbtype == "SQL2005"))
            {
                try
                {
                    if ((settings.DbName == "master") || (settings.DbName == ""))
                    {
                        List<string> dBList = obj2.GetDBList();
                        if (dBList.Count > 0)
                        {
                            this.mainfrm.toolComboBox_DB.Items.Clear();
                            foreach (string str in dBList)
                            {
                                TreeNode node = new TreeNode(str);
                                node.ImageIndex = 0;
                                node.SelectedImageIndex = 0;
                                node.Tag = "db";
                                serverNode.Nodes.Add(node);
                                this.mainfrm.toolComboBox_DB.Items.Add(str);
                            }
                            if (this.mainfrm.toolComboBox_DB.Items.Count > 0)
                            {
                                this.mainfrm.toolComboBox_DB.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        TreeNode node2 = new TreeNode(settings.DbName);
                        node2.ImageIndex = 0;
                        node2.SelectedImageIndex = 0;
                        node2.Tag = "db";
                        serverNode.Nodes.Add(node2);
                        this.mainfrm.toolComboBox_DB.Items.Clear();
                        this.mainfrm.toolComboBox_DB.Items.Add(settings.DbName);
                        DataTable tabViews = obj2.GetTabViews(settings.DbName);
                        if (tabViews != null)
                        {
                            this.mainfrm.toolComboBox_Table.Items.Clear();
                            foreach (DataRow row in tabViews.Rows)
                            {
                                string item = row["name"].ToString();
                                this.mainfrm.toolComboBox_Table.Items.Add(item);
                            }
                            if (this.mainfrm.toolComboBox_Table.Items.Count > 0)
                            {
                                this.mainfrm.toolComboBox_Table.SelectedIndex = 0;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    MessageBox.Show(this, "连接服务器失败！请检查服务器是否已经启动或工作正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            #region 
            if (dbtype == "Oracle")
            {
                TreeNode node3 = new TreeNode(ServerIp);
                node3.ImageIndex = 2;
                node3.SelectedImageIndex = 2;
                node3.Tag = "db";
                serverNode.Nodes.Add(node3);
                this.mainfrm.toolComboBox_DB.Items.Add(ServerIp);
                this.mainfrm.toolComboBox_DB.SelectedIndex = 0;
                DataTable table2 = obj2.GetTabViews(ServerIp);
                if (table2 != null)
                {
                    this.mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row2 in table2.Rows)
                    {
                        string str3 = row2["name"].ToString();
                        this.mainfrm.toolComboBox_Table.Items.Add(str3);
                    }
                    if (this.mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        this.mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }
            }
            if (dbtype == "MySQL")
            {
                try
                {
                    string dbName = settings.DbName;
                    switch (dbName)
                    {
                        case "mysql":
                        case "":
                        {
                            List<string> list2 = obj2.GetDBList();
                            if (list2.Count > 0)
                            {
                                this.mainfrm.toolComboBox_DB.Items.Clear();
                                foreach (string str5 in list2)
                                {
                                    TreeNode node4 = new TreeNode(str5);
                                    node4.ImageIndex = 2;
                                    node4.SelectedImageIndex = 2;
                                    node4.Tag = "db";
                                    serverNode.Nodes.Add(node4);
                                    this.mainfrm.toolComboBox_DB.Items.Add(str5);
                                }
                                if (this.mainfrm.toolComboBox_DB.Items.Count > 0)
                                {
                                    this.mainfrm.toolComboBox_DB.SelectedIndex = 0;
                                }
                            }
                            goto Label_05E4;
                        }
                    }
                    TreeNode node5 = new TreeNode(dbName);
                    node5.ImageIndex = 2;
                    node5.SelectedImageIndex = 2;
                    node5.Tag = "db";
                    serverNode.Nodes.Add(node5);
                    this.mainfrm.toolComboBox_DB.Items.Clear();
                    this.mainfrm.toolComboBox_DB.Items.Add(dbName);
                    DataTable table3 = obj2.GetTabViews(dbName);
                    if (table3 != null)
                    {
                        this.mainfrm.toolComboBox_Table.Items.Clear();
                        foreach (DataRow row3 in table3.Rows)
                        {
                            row3["name"].ToString();
                            this.mainfrm.toolComboBox_Table.Items.Add(dbName);
                        }
                    }
                }
                catch (Exception exception2)
                {
                    LogInfo.WriteLog(exception2);
                    MessageBox.Show(this, "连接服务器失败！请检查服务器是否已经启动或工作正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
        Label_05E4:
            if (dbtype == "OleDb")
            {
                string text = ServerIp.Substring(ServerIp.LastIndexOf(@"\") + 1);
                TreeNode node6 = new TreeNode(text);
                node6.ImageIndex = 2;
                node6.SelectedImageIndex = 2;
                node6.Tag = "db";
                serverNode.Nodes.Add(node6);
                this.mainfrm.toolComboBox_DB.Items.Add(text);
                this.mainfrm.toolComboBox_DB.SelectedIndex = 0;
                DataTable table4 = obj2.GetTabViews(text);
                if (table4 != null)
                {
                    this.mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row4 in table4.Rows)
                    {
                        string str7 = row4["name"].ToString();
                        this.mainfrm.toolComboBox_Table.Items.Add(str7);
                    }
                    if (this.mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        this.mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }
            }
            #endregion 

            serverNode.ExpandAll();
            foreach (TreeNode node7 in serverNode.Nodes)
            {
                string str8 = node7.Text;
                this.mainfrm.lblViewInfo.Text = "加载数据库 " + str8 + "...";
                //SplashScreen.SetStatus(" 加载数据库 " + str8 + "...");
                TreeNode node8 = new TreeNode("表");
                node8.ImageIndex = 3;
                node8.SelectedImageIndex = 3;
                node8.Tag = "tableroot";
                node7.Nodes.Add(node8);
                TreeNode node9 = new TreeNode("视图");
                node9.ImageIndex = 3;
                node9.SelectedImageIndex = 3;
                node9.Tag = "viewroot";
                node7.Nodes.Add(node9);
                TreeNode node10 = new TreeNode("存储过程");
                node10.ImageIndex = 3;
                node10.SelectedImageIndex = 3;
                node10.Tag = "procroot";
                node7.Nodes.Add(node10);
                try
                {
                    List<string> tables = obj2.GetTables(str8);
                    if (tables.Count > 0)
                    {
                        foreach (string str9 in tables)
                        {
                            //SplashScreen.SetStatus(" 加载数据库信息 " + str9);
                            TreeNode node11 = new TreeNode(str9);
                            node11.ImageIndex = 4;
                            node11.SelectedImageIndex = 4;
                            node11.Tag = "table";
                            node8.Nodes.Add(node11);
                            if (!ConnectSimple)
                            {
                                List<LTP.CodeHelper.ColumnInfo> columnList = obj2.GetColumnList(str8, str9);
                                if ((columnList != null) && (columnList.Count > 0))
                                {
                                    foreach (LTP.CodeHelper.ColumnInfo info in columnList)
                                    {
                                        string columnName = info.ColumnName;
                                        string typeName = info.TypeName;
                                        TreeNode node12 = new TreeNode(columnName + "[" + typeName + "]");
                                        node12.ImageIndex = 7;
                                        node12.SelectedImageIndex = 7;
                                        node12.Tag = "column";
                                        node11.Nodes.Add(node12);
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception3)
                {
                    LogInfo.WriteLog(exception3);
                    MessageBox.Show(this, "获取数据库" + str8 + "的表信息失败：" + exception3.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                try
                {
                    DataTable vIEWs = obj2.GetVIEWs(str8);
                    if (vIEWs != null)
                    {
                        foreach (DataRow row5 in vIEWs.Select("", "name ASC"))
                        {
                            string str12 = row5["name"].ToString();
                           // SplashScreen.SetStatus("加载数据库信息 " + str12);
                            TreeNode node13 = new TreeNode(str12);
                            node13.ImageIndex = 4;
                            node13.SelectedImageIndex = 4;
                            node13.Tag = "view";
                            node9.Nodes.Add(node13);
                            if (!ConnectSimple)
                            {
                                List<LTP.CodeHelper.ColumnInfo> list5 = obj2.GetColumnList(str8, str12);
                                if ((list5 != null) && (list5.Count > 0))
                                {
                                    foreach (LTP.CodeHelper.ColumnInfo info2 in list5)
                                    {
                                        string str13 = info2.ColumnName;
                                        string str14 = info2.TypeName;
                                        TreeNode node14 = new TreeNode(str13 + "[" + str14 + "]");
                                        node14.ImageIndex = 7;
                                        node14.SelectedImageIndex = 7;
                                        node14.Tag = "column";
                                        node13.Nodes.Add(node14);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception4)
                {
                    LogInfo.WriteLog(exception4);
                    MessageBox.Show(this, "获取数据库" + str8 + "的视图信息失败：" + exception4.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                try
                {
                    DataTable procs = obj2.GetProcs(str8);
                    if (procs != null)
                    {
                        foreach (DataRow row6 in procs.Select("", "name ASC"))
                        {
                            string str15 = row6["name"].ToString();
                         //   SplashScreen.SetStatus("加载数据库信息 " + str15);
                            TreeNode node15 = new TreeNode(str15);
                            node15.ImageIndex = 4;
                            node15.SelectedImageIndex = 4;
                            node15.Tag = "proc";
                            node10.Nodes.Add(node15);
                            if (!ConnectSimple)
                            {
                                List<LTP.CodeHelper.ColumnInfo> list6 = obj2.GetColumnList(str8, str15);
                                if ((list6 != null) && (list6.Count > 0))
                                {
                                    foreach (LTP.CodeHelper.ColumnInfo info3 in list6)
                                    {
                                        string str16 = info3.ColumnName;
                                        string str17 = info3.TypeName;
                                        TreeNode node16 = new TreeNode(str16 + "[" + str17 + "]");
                                        node16.ImageIndex = 9;
                                        node16.SelectedImageIndex = 9;
                                        node16.Tag = "column";
                                        node15.Nodes.Add(node16);
                                    }
                                }
                            }
                        }
                    }
                    continue;
                }
                catch (Exception exception5)
                {
                    LogInfo.WriteLog(exception5);
                    MessageBox.Show(this, "获取数据库" + str8 + "的视图信息失败：" + exception5.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    continue;
                }
            }
            //SplashScreen.CloseForm();
            foreach (TreeNode node17 in this.serverlistNode.Nodes)
            {
                if (node17.Text == ServerIp)
                {
                    this.treeView1.SelectedNode = node17;
                }
            }
            this.mainfrm.lblViewInfo.Text = "就绪";
        }

        private void CreatMenu(string NodeType)
        {
            this.DbTreeContextMenu.Items.Clear();
            switch (NodeType)
            {
                case "serverlist":
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Name = "添加服务器Item";
                    item.Text = "添加服务器";
                    item.Click += new EventHandler(this.添加服务器Item_Click);
                    ToolStripMenuItem item2 = new ToolStripMenuItem();
                    item2.Name = "备份服务器配置Item";
                    item2.Text = "备份服务器配置";
                    item2.Click += new EventHandler(this.备份服务器配置Item_Click);
                    ToolStripMenuItem item3 = new ToolStripMenuItem();
                    item3.Name = "导入服务器配置Item";
                    item3.Text = "导入服务器配置";
                    item3.Click += new EventHandler(this.导入服务器配置Item_Click);
                    ToolStripMenuItem item4 = new ToolStripMenuItem();
                    item4.Name = "刷新Item";
                    item4.Text = "刷新";
                    item4.Click += new EventHandler(this.刷新Item_Click);
                    ToolStripMenuItem item5 = new ToolStripMenuItem();
                    item5.Name = "属性Item";
                    item5.Text = "属性";
                    item5.Click += new EventHandler(this.属性Item_Click);
                    this.DbTreeContextMenu.Items.AddRange(new ToolStripItem[] { item, item2, item3, item4 });
                    return;
                }
                case "server":
                {
                    ToolStripMenuItem item6 = new ToolStripMenuItem();
                    item6.Name = "连接服务器Item";
                    item6.Text = "连接服务器";
                    item6.Click += new EventHandler(this.连接服务器Item_Click);
                    ToolStripMenuItem item7 = new ToolStripMenuItem();
                    item7.Name = "注销服务器Item";
                    item7.Text = "注销服务器";
                    item7.Click += new EventHandler(this.注销服务器Item_Click);
                    ToolStripMenuItem item8 = new ToolStripMenuItem();
                    item8.Name = "属性Item";
                    item8.Text = "刷新";
                    item8.Click += new EventHandler(this.server属性Item_Click);
                    this.DbTreeContextMenu.Items.AddRange(new ToolStripItem[] { item6, item7, item8 });
                    return;
                }
                case "db":
                {
                    ToolStripMenuItem item9 = new ToolStripMenuItem();
                    item9.Name = "新建查询Item";
                    item9.Text = "新建查询";
                    item9.Click += new EventHandler(this.新建查询Item_Click);
                    ToolStripSeparator separator = new ToolStripSeparator();
                    separator.Name = "Separator1";
                    ToolStripMenuItem item10 = new ToolStripMenuItem();
                    item10.Name = "生成存储过程Item";
                    item10.Text = "生成存储过程";
                    item10.Click += new EventHandler(this.生成存储过程dbItem_Click);
                    ToolStripMenuItem item11 = new ToolStripMenuItem();
                    item11.Name = "生成数据脚本Item";
                    item11.Text = "生成数据脚本";
                    item11.Click += new EventHandler(this.生成数据脚本dbItem_Click);
                    ToolStripMenuItem item12 = new ToolStripMenuItem();
                    item12.Name = "导出文件Item";
                    item12.Text = "导出文件";
                    ToolStripMenuItem item13 = new ToolStripMenuItem();
                    item13.Name = "存储过程Item";
                    item13.Text = "存储过程";
                    item13.Click += new EventHandler(this.存储过程dbItem_Click);
                    ToolStripMenuItem item14 = new ToolStripMenuItem();
                    item14.Name = "数据脚本Item";
                    item14.Text = "数据脚本";
                    item14.Click += new EventHandler(this.数据脚本dbItem_Click);
                    ToolStripMenuItem item15 = new ToolStripMenuItem();
                    item15.Name = "表数据Item";
                    item15.Text = "表数据";
                    item15.Click += new EventHandler(this.表数据dbItem_Click);
                    item12.DropDownItems.AddRange(new ToolStripItem[] { item13, item14, item15 });
                    ToolStripSeparator separator2 = new ToolStripSeparator();
                    separator.Name = "Separator2";
                    ToolStripMenuItem item16 = new ToolStripMenuItem();
                    item16.Name = "父子表代码生成Item";
                    item16.Text = "父子表代码生成";
                    item16.Click += new EventHandler(this.父子表代码生成dbItem_Click);
                    ToolStripMenuItem item17 = new ToolStripMenuItem();
                    item17.Name = "自动输出代码Item";
                    item17.Text = "代码批量生成";
                    item17.Click += new EventHandler(this.自动输出代码dbItem_Click);
                    ToolStripSeparator separator3 = new ToolStripSeparator();
                    separator.Name = "Separatordb2";
                    ToolStripMenuItem item18 = new ToolStripMenuItem();
                    item18.Name = "刷新Item";
                    item18.Text = "刷新";
                    item18.Click += new EventHandler(this.刷新dbItem_Click);
                    this.DbTreeContextMenu.Items.AddRange(new ToolStripItem[] { item9, separator, item10, item11, item12, separator2, item16, item17, separator3, item18 });
                    return;
                }
                case "tableroot":
                case "viewroot":
                case "procroot":
                case "column":
                {
                    ToolStripMenuItem item19 = new ToolStripMenuItem();
                    item19.Name = "生成SQL语句Item";
                    item19.Text = "生成SQL语句到";
                    ToolStripMenuItem item20 = new ToolStripMenuItem();
                    item20.Name = "SELECTItem";
                    item20.Text = "SELECT(&S)";
                    item19.Click += new EventHandler(item20_Click);
                    item20.Click += new EventHandler(item20_Click);
                    this.DbTreeContextMenu.Items.AddRange(new ToolStripItem[] { item19, item20});
                    return;                   
                }
                case "table":
                {
                    ToolStripMenuItem item19 = new ToolStripMenuItem();
                    item19.Name = "生成SQL语句Item";
                    item19.Text = "生成SQL语句到";
                    ToolStripMenuItem item20 = new ToolStripMenuItem();
                    item20.Name = "SELECTItem";
                    item20.Text = "SELECT(&S)";
                    item20.Click += new EventHandler(this.SELECTItem_Click);
                    ToolStripMenuItem item21 = new ToolStripMenuItem();
                    item21.Name = "UPDATEItem";
                    item21.Text = "UPDATE(&U)";
                    item21.Click += new EventHandler(this.UPDATEItem_Click);
                    ToolStripMenuItem item22 = new ToolStripMenuItem();
                    item22.Name = "DELETEItem";
                    item22.Text = "DELETE(&D)";
                    item22.Click += new EventHandler(this.DELETEItem_Click);
                    ToolStripMenuItem item23 = new ToolStripMenuItem();
                    item23.Name = "INSERTItem";
                    item23.Text = "INSERT(&I)";
                    item23.Click += new EventHandler(this.INSERTItem_Click);
                    item19.DropDownItems.AddRange(new ToolStripItem[] { item20, item21, item22, item23 });
                    ToolStripMenuItem item24 = new ToolStripMenuItem();
                    item24.Name = "查看表数据tabItem";
                    item24.Text = "浏览表数据";
                    item24.Click += new EventHandler(this.查看表数据tabItem_Click);
                    ToolStripMenuItem item25 = new ToolStripMenuItem();
                    item25.Name = "生成数据脚本tabItem";
                    item25.Text = "生成数据脚本";
                    item25.Click += new EventHandler(this.生成数据脚本tabItem_Click);
                    ToolStripMenuItem item26 = new ToolStripMenuItem();
                    item26.Name = "生成存储过程tabItem";
                    item26.Text = "生成存储过程";
                    item26.Click += new EventHandler(this.生成存储过程tabItem_Click);
                    ToolStripMenuItem item27 = new ToolStripMenuItem();
                    item27.Name = "导出文件tabItem";
                    item27.Text = "导出文件";
                    ToolStripMenuItem item28 = new ToolStripMenuItem();
                    item28.Name = "存储过程tabItem";
                    item28.Text = "存储过程";
                    item28.Click += new EventHandler(this.存储过程tabItem_Click);
                    ToolStripMenuItem item29 = new ToolStripMenuItem();
                    item29.Name = "数据脚本tabItem";
                    item29.Text = "数据脚本";
                    item29.Click += new EventHandler(this.数据脚本tabItem_Click);
                    ToolStripMenuItem item30 = new ToolStripMenuItem();
                    item30.Name = "表数据tabItem";
                    item30.Text = "表数据";
                    item30.Click += new EventHandler(this.表数据tabItem_Click);
                    item27.DropDownItems.AddRange(new ToolStripItem[] { item28, item29, item30 });
                    ToolStripSeparator separator4 = new ToolStripSeparator();
                    separator4.Name = "Separator1";
                    ToolStripMenuItem item31 = new ToolStripMenuItem();
                    item31.Name = "代码生成Item";
                    item31.Text = "代码生成器";
                    item31.Click += new EventHandler(this.代码生成Item_Click);
                    ToolStripMenuItem item32 = new ToolStripMenuItem();
                    item32.Name = "模版代码生成Item";
                    item32.Text = "模版代码生成";
                    item32.Click += new EventHandler(this.模版代码生成Item_Click);
                    ToolStripMenuItem item33 = new ToolStripMenuItem();
                    item33.Name = "生成单类结构Item";
                    item33.Text = "生成单类结构";
                    item33.Click += new EventHandler(this.生成单类结构Items_Click);
                    ToolStripMenuItem item34 = new ToolStripMenuItem();
                    item34.Name = "生成ModelItem";
                    item34.Text = "生成Model";
                    item34.Click += new EventHandler(this.生成ModelItem_Click);
                    ToolStripSeparator separator5 = new ToolStripSeparator();
                    separator5.Name = "Separator3";
                    ToolStripMenuItem item35 = new ToolStripMenuItem();
                    item35.Name = "简单三层Item";
                    item35.Text = "简单三层";
                    ToolStripMenuItem item36 = new ToolStripMenuItem();
                    item36.Name = "生成DALS3Item";
                    item36.Text = "生成DAL";
                    item36.Click += new EventHandler(this.生成DALS3Item_Click);
                    ToolStripMenuItem item37 = new ToolStripMenuItem();
                    item37.Name = "生成BLLS3Item";
                    item37.Text = "生成BLL";
                    item37.Click += new EventHandler(this.生成BLLS3Item_Click);
                    ToolStripMenuItem item38 = new ToolStripMenuItem();
                    item38.Name = "生成全部S3";
                    item38.Text = "生成全部";
                    item38.Click += new EventHandler(this.生成全部S3Item_Click);
                    item35.DropDownItems.AddRange(new ToolStripItem[] { item36, item37, item38 });
                    ToolStripMenuItem item39 = new ToolStripMenuItem();
                    item39.Name = "工厂模式三层Item";
                    item39.Text = "工厂模式三层";
                    ToolStripMenuItem item40 = new ToolStripMenuItem();
                    item40.Name = "生成DALF3Item";
                    item40.Text = "生成DAL";
                    item40.Click += new EventHandler(this.生成DALF3Item_Click);
                    ToolStripMenuItem item41 = new ToolStripMenuItem();
                    item41.Name = "生成IDALItem";
                    item41.Text = "生成DAL";
                    item41.Click += new EventHandler(this.生成IDALItem_Click);
                    ToolStripMenuItem item42 = new ToolStripMenuItem();
                    item42.Name = "生成DALFactoryItem";
                    item42.Text = "生成DALFactory";
                    item42.Click += new EventHandler(this.生成DALFactoryItem_Click);
                    ToolStripMenuItem item43 = new ToolStripMenuItem();
                    item43.Name = "生成BLLF3Item";
                    item43.Text = "生成BLL";
                    item43.Click += new EventHandler(this.生成BLLF3Item_Click);
                    ToolStripMenuItem item44 = new ToolStripMenuItem();
                    item44.Name = "生成全部F3Item";
                    item44.Text = "生成全部";
                    item44.Click += new EventHandler(this.生成全部F3Item_Click);
                    item39.DropDownItems.AddRange(new ToolStripItem[] { item40, item41, item42, item43, item44 });
                    ToolStripSeparator separator6 = new ToolStripSeparator();
                    separator6.Name = "Separator5";
                    ToolStripMenuItem item45 = new ToolStripMenuItem();
                    item45.Name = "生成页面Item";
                    item45.Text = "生成页面";
                    item45.Click += new EventHandler(this.生成页面Item_Click);
                    ToolStripSeparator separator7 = new ToolStripSeparator();
                    separator7.Name = "Separator2";
                    //ToolStripMenuItem item46 = new ToolStripMenuItem();
                    //item46.Name = "自动输出代码Item";
                    //item46.Text = "代码批量生成";
                    //item46.Click += new EventHandler(this.自动输出代码Item_Click);
                    ToolStripSeparator separator8 = new ToolStripSeparator();
                    separator8.Name = "Separator4";
                    ToolStripMenuItem item47 = new ToolStripMenuItem();
                    item47.Name = "重命名tabItem";
                    item47.Text = "重命名";
                    item47.Click += new EventHandler(this.重命名tabItem_Click);
                    ToolStripMenuItem item48 = new ToolStripMenuItem();
                    item48.Name = "删除tabItem";
                    item48.Text = "删除";
                    item48.Click += new EventHandler(this.删除tabItem_Click);
                    this.DbTreeContextMenu.Items.AddRange(new ToolStripItem[] { item19, item24, item25, item26, item27, separator4, item31, 
                        //item46, 
                        item32, separator8, item47, item48 });
                    return;
                }
                case "view":
                {
                    ToolStripMenuItem item49 = new ToolStripMenuItem();
                    item49.Name = "脚本Item";
                    item49.Text = "脚本";
                    ToolStripMenuItem item50 = new ToolStripMenuItem();
                    item50.Name = "SELECTItem";
                    item50.Text = "SELECT(&S)";
                    ToolStripMenuItem item51 = new ToolStripMenuItem();
                    item51.Name = "AlterItem";
                    item51.Text = "ALTER(&U)";
                    ToolStripMenuItem item52 = new ToolStripMenuItem();
                    item52.Name = "DropItem";
                    item52.Text = "DROP(&D)";
                    item49.DropDownItems.AddRange(new ToolStripItem[] { item50, item51, item52 });
                    ToolStripMenuItem item53 = new ToolStripMenuItem();
                    item53.Name = "对象定义Item";
                    item53.Text = "对象定义";
                    item53.Click += new EventHandler(this.对象定义Item_Click);
                    ToolStripMenuItem item54 = new ToolStripMenuItem();
                    item54.Name = "查看表数据tabItem";
                    item54.Text = "浏览表数据";
                    item54.Click += new EventHandler(this.查看表数据tabItem_Click);
                    ToolStripSeparator separator9 = new ToolStripSeparator();
                    separator9.Name = "Separatorv1";
                    ToolStripMenuItem item55 = new ToolStripMenuItem();
                    item55.Name = "代码生成Item";
                    item55.Text = "代码生成器";
                    item55.Click += new EventHandler(this.代码生成Item_Click);
                    this.DbTreeContextMenu.Items.AddRange(new ToolStripItem[] { item49, item53, item54, separator9, item55 });
                    return;
                }
                case "proc":
                {
                    ToolStripMenuItem item56 = new ToolStripMenuItem();
                    item56.Name = "脚本Item";
                    item56.Text = "脚本";
                    ToolStripMenuItem item57 = new ToolStripMenuItem();
                    item57.Name = "AlterItem";
                    item57.Text = "ALTER(&U)";
                    ToolStripMenuItem item58 = new ToolStripMenuItem();
                    item58.Name = "DropItem";
                    item58.Text = "DROP(&D)";
                    ToolStripMenuItem item59 = new ToolStripMenuItem();
                    item59.Name = "EXECItem";
                    item59.Text = "EXEC(&I)";
                    item56.DropDownItems.AddRange(new ToolStripItem[] { item57, item58, item59 });
                    ToolStripMenuItem item60 = new ToolStripMenuItem();
                    item60.Name = "对象定义Item";
                    item60.Text = "对象定义";
                    item60.Click += new EventHandler(this.对象定义Item_Click);
                    this.DbTreeContextMenu.Items.AddRange(new ToolStripItem[] { item56, item60 });
                    break;
                }
                default:
                    return;
            }
        }

        void item20_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void CreatTree(string dbtype, string ServerIp, string constr, string Dbname)
        {
            this.dbobj = DBOMaker.CreateDbObj(dbtype);
            TreeNode node = new TreeNode(this.GetserverNodeText(ServerIp, dbtype, Dbname));
            node.Tag = "server";
            this.serverlistNode.Nodes.Add(node);
            node.ImageIndex = 1;
            node.SelectedImageIndex = 1;
            this.treeView1.SelectedNode = node;
            this.mainfrm.lblViewInfo.Text = "加载数据库树...";
            //SplashScreen.SetStatus("加载数据库树...");
            this.dbobj.DbConnectStr = constr;
            if ((dbtype == "SQL2000") || (dbtype == "SQL2005"))
            {
                try
                {
                    if ((this.logo.dbname == "master") || (this.logo.dbname == ""))
                    {
                        List<string> dBList = this.dbobj.GetDBList();
                        if ((dBList != null) && (dBList.Count > 0))
                        {
                            this.mainfrm.toolComboBox_DB.Items.Clear();
                            foreach (string str in dBList)
                            {
                                TreeNode node2 = new TreeNode(str);
                                node2.ImageIndex = 2;
                                node2.SelectedImageIndex = 2;
                                node2.Tag = "db";
                                node.Nodes.Add(node2);
                                this.mainfrm.toolComboBox_DB.Items.Add(str);
                            }
                            if (this.mainfrm.toolComboBox_DB.Items.Count > 0)
                            {
                                this.mainfrm.toolComboBox_DB.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        string dbname = this.logo.dbname;
                        TreeNode node3 = new TreeNode(dbname);
                        node3.ImageIndex = 2;
                        node3.SelectedImageIndex = 2;
                        node3.Tag = "db";
                        node.Nodes.Add(node3);
                        this.mainfrm.toolComboBox_DB.Items.Clear();
                        this.mainfrm.toolComboBox_DB.Items.Add(dbname);
                        DataTable tabViews = this.dbobj.GetTabViews(dbname);
                        if (tabViews != null)
                        {
                            this.mainfrm.toolComboBox_Table.Items.Clear();
                            foreach (DataRow row in tabViews.Rows)
                            {
                                row["name"].ToString();
                                this.mainfrm.toolComboBox_Table.Items.Add(dbname);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    throw new Exception("获取数据库失败：" + exception.Message);
                }
            }
            #region 
            if (dbtype == "Oracle")
            {
                string text = ServerIp;
                TreeNode node4 = new TreeNode(text);
                node4.ImageIndex = 2;
                node4.SelectedImageIndex = 2;
                node4.Tag = "db";
                node.Nodes.Add(node4);
                this.mainfrm.toolComboBox_DB.Items.Add(text);
                DataTable table2 = this.dbobj.GetTabViews(text);
                if (table2 != null)
                {
                    this.mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row2 in table2.Rows)
                    {
                        row2["name"].ToString();
                        this.mainfrm.toolComboBox_Table.Items.Add(text);
                    }
                    if (this.mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        this.mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }
            }
            if (dbtype == "MySQL")
            {
                #region 
                //    try
            //    {
            //        if ((this.loginMysql.dbname == "mysql") || (this.loginMysql.dbname == ""))
            //        {
            //            List<string> list2 = this.dbobj.GetDBList();
            //            if ((list2 != null) && (list2.Count > 0))
            //            {
            //                this.mainfrm.toolComboBox_DB.Items.Clear();
            //                foreach (string str4 in list2)
            //                {
            //                    TreeNode node5 = new TreeNode(str4);
            //                    node5.ImageIndex = 2;
            //                    node5.SelectedImageIndex = 2;
            //                    node5.Tag = "db";
            //                    node.Nodes.Add(node5);
            //                    this.mainfrm.toolComboBox_DB.Items.Add(str4);
            //                }
            //                if (this.mainfrm.toolComboBox_DB.Items.Count > 0)
            //                {
            //                    this.mainfrm.toolComboBox_DB.SelectedIndex = 0;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            string str5 = this.loginMysql.dbname;
            //            TreeNode node6 = new TreeNode(str5);
            //            node6.ImageIndex = 2;
            //            node6.SelectedImageIndex = 2;
            //            node6.Tag = "db";
            //            node.Nodes.Add(node6);
            //            this.mainfrm.toolComboBox_DB.Items.Clear();
            //            this.mainfrm.toolComboBox_DB.Items.Add(str5);
            //            DataTable table3 = this.dbobj.GetTabViews(str5);
            //            if (table3 != null)
            //            {
            //                this.mainfrm.toolComboBox_Table.Items.Clear();
            //                foreach (DataRow row3 in table3.Rows)
            //                {
            //                    row3["name"].ToString();
            //                    this.mainfrm.toolComboBox_Table.Items.Add(str5);
            //                }
            //                if (this.mainfrm.toolComboBox_Table.Items.Count > 0)
            //                {
            //                    this.mainfrm.toolComboBox_Table.SelectedIndex = 0;
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exception2)
            //    {
            //        LogInfo.WriteLog(exception2);
            //        throw new Exception("获取数据库失败：" + exception2.Message);
                //    }
                #endregion
            }
            if (dbtype == "OleDb")
            {
                string str6 = ServerIp.Substring(ServerIp.LastIndexOf(@"\") + 1);
                TreeNode node7 = new TreeNode(str6);
                node7.ImageIndex = 2;
                node7.SelectedImageIndex = 2;
                node7.Tag = "db";
                node.Nodes.Add(node7);
                this.mainfrm.toolComboBox_DB.Items.Add(str6);
                DataTable table4 = this.dbobj.GetTabViews(str6);
                if (table4 != null)
                {
                    this.mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row4 in table4.Rows)
                    {
                        row4["name"].ToString();
                        this.mainfrm.toolComboBox_Table.Items.Add(str6);
                    }
                    if (this.mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        this.mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }
            }
            node.ExpandAll();
            #endregion 

            foreach (TreeNode node8 in node.Nodes)
            {
                string dbName = node8.Text;
                this.mainfrm.lblViewInfo.Text = "加载数据库 " + dbName + "...";
               // SplashScreen.SetStatus("加载数据库 " + dbName + "...");
                TreeNode node9 = new TreeNode("表");
                node9.ImageIndex = 3;
                node9.SelectedImageIndex = 3;
                node9.Tag = "tableroot";
                node8.Nodes.Add(node9);
                TreeNode node10 = new TreeNode("视图");
                node10.ImageIndex = 3;
                node10.SelectedImageIndex = 3;
                node10.Tag = "viewroot";
                node8.Nodes.Add(node10);
                TreeNode node11 = new TreeNode("存储过程");
                node11.ImageIndex = 3;
                node11.SelectedImageIndex = 3;
                node11.Tag = "procroot";
                node8.Nodes.Add(node11);
                try
                {
                    List<string> tables = this.dbobj.GetTables(dbName);
                    if (tables.Count > 0)
                    {
                        foreach (string str8 in tables)
                        {
                            //SplashScreen.SetStatus("加载数据库信息 " + str8);
                            TreeNode node12 = new TreeNode(str8);
                            node12.ImageIndex = 4;
                            node12.SelectedImageIndex = 4;
                            node12.Tag = "table";
                            node9.Nodes.Add(node12);
                            if (!this.logo.chk_Simple.Checked)
                            {
                                List<LTP.CodeHelper.ColumnInfo> columnList = this.dbobj.GetColumnList(dbName, str8);
                                if ((columnList != null) && (columnList.Count > 0))
                                {
                                    foreach (LTP.CodeHelper.ColumnInfo info in columnList)
                                    {
                                        string columnName = info.ColumnName;
                                        string typeName = info.TypeName;
                                        TreeNode node13 = new TreeNode(columnName + "[" + typeName + "]");
                                        node13.ImageIndex = 7;
                                        node13.SelectedImageIndex = 7;
                                        node13.Tag = "column";
                                        node12.Nodes.Add(node13);
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception3)
                {
                    LogInfo.WriteLog(exception3);
                    MessageBox.Show(this, "获取数据库" + dbName + "的表信息失败：" + exception3.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                try
                {
                    DataTable vIEWs = this.dbobj.GetVIEWs(dbName);
                    if (vIEWs != null)
                    {
                        foreach (DataRow row5 in vIEWs.Select("", "name ASC"))
                        {
                            string str11 = row5["name"].ToString();
                            //SplashScreen.SetStatus("加载数据库信息 " + str11);
                            TreeNode node14 = new TreeNode(str11);
                            node14.ImageIndex = 4;
                            node14.SelectedImageIndex = 4;
                            node14.Tag = "view";
                            node10.Nodes.Add(node14);
                            if (!this.logo.chk_Simple.Checked)
                            {
                                List<LTP.CodeHelper.ColumnInfo> list5 = this.dbobj.GetColumnList(dbName, str11);
                                if ((list5 != null) && (list5.Count > 0))
                                {
                                    foreach (LTP.CodeHelper.ColumnInfo info2 in list5)
                                    {
                                        string str12 = info2.ColumnName;
                                        string str13 = info2.TypeName;
                                        TreeNode node15 = new TreeNode(str12 + "[" + str13 + "]");
                                        node15.ImageIndex = 7;
                                        node15.SelectedImageIndex = 7;
                                        node15.Tag = "column";
                                        node14.Nodes.Add(node15);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception4)
                {
                    LogInfo.WriteLog(exception4);
                    MessageBox.Show(this, "获取数据库" + dbName + "的视图信息失败：" + exception4.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                try
                {
                    DataTable procs = this.dbobj.GetProcs(dbName);
                    if (procs != null)
                    {
                        foreach (DataRow row6 in procs.Select("", "name ASC"))
                        {
                            string str14 = row6["name"].ToString();
                           // SplashScreen.SetStatus("加载数据库信息 " + str14);
                            TreeNode node16 = new TreeNode(str14);
                            node16.ImageIndex = 4;
                            node16.SelectedImageIndex = 4;
                            node16.Tag = "proc";
                            node11.Nodes.Add(node16);
                            if (!this.logo.chk_Simple.Checked)
                            {
                                List<LTP.CodeHelper.ColumnInfo> list6 = this.dbobj.GetColumnList(dbName, str14);
                                if ((list6 != null) && (list6.Count > 0))
                                {
                                    foreach (LTP.CodeHelper.ColumnInfo info3 in list6)
                                    {
                                        string str15 = info3.ColumnName;
                                        string str16 = info3.TypeName;
                                        TreeNode node17 = new TreeNode(str15 + "[" + str16 + "]");
                                        node17.ImageIndex = 9;
                                        node17.SelectedImageIndex = 9;
                                        node17.Tag = "column";
                                        node16.Nodes.Add(node17);
                                    }
                                }
                            }
                        }
                    }
                    continue;
                }
                catch (Exception exception5)
                {
                    LogInfo.WriteLog(exception5);
                    MessageBox.Show(this, "获取数据库" + dbName + "的视图信息失败：" + exception5.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    continue;
                }
            }
            foreach (TreeNode node18 in this.treeView1.Nodes)
            {
                if (node18.Text == ServerIp)
                {
                    this.treeView1.SelectedNode = node18;
                }
            }
            this.mainfrm.lblViewInfo.Text = "就绪";
            //treeView1.Refresh();
        }

        private void DbView_Layout(object sender, LayoutEventArgs e)
        {
            if (!this.m_bLayoutCalled)
            {
                this.m_bLayoutCalled = true;
                //SplashScreen.CloseForm();
                base.Activate();
            }
        }

        private void DbView_Load(object sender, EventArgs e)
        {
            this.LoadServer();
            this.setting = ModuleConfig.GetSettings();
            this.mainfrm = (MainForm) Application.OpenForms["MainForm"];
        }

        private void DELETEItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = this.TreeClickNode.Parent.Parent.Text;
                string tablename = this.TreeClickNode.Text;
                string sQLDelete = ObjHelper.CreatDsb(text).GetSQLDelete(dbname, tablename);
                string pageTitle = tablename + "查询.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, sQLDelete));
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

        private string GetConstr(string ServerIp, string txtConstr)
        {
            string str = "";
            if (ServerIp != "")
            {
                str = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ServerIp + ";Persist Security Info=False";
                if (ServerIp.ToLower().IndexOf(".mdb") > 0)
                {
                    isMdb = true;
                    return str;
                }
                isMdb = false;
                return str;
            }
            str = txtConstr;
            if (str.ToLower().IndexOf(".mdb") > 0)
            {
                isMdb = true;
                return str;
            }
            isMdb = false;
            return str;
        }

        public string GetLongServername()
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null)
            {
                return "";
            }
            switch (selectedNode.Tag.ToString())
            {
                case "serverlist":
                    return "";

                case "server":
                    return selectedNode.Text;

                case "db":
                    return selectedNode.Parent.Text;

                case "tableroot":
                case "viewroot":
                    return selectedNode.Parent.Parent.Text;

                case "table":
                case "view":
                    return selectedNode.Parent.Parent.Parent.Text;

                case "column":
                    return selectedNode.Parent.Parent.Parent.Parent.Text;
            }
            return "";
        }

        private string GetserverNodeText(string servername, string dbtype, string dbname)
        {
            string str = servername + "(" + dbtype + ")";
            if ((dbname.Trim() != "") && (dbname.Trim() != "master"))
            {
                str = str + "(" + dbname + ")";
            }
            return str;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbView));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("ID", 7, 7);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("table1", 4, 4, new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("表", 4, 4, new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点4", 7, 7);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("view1", 6, 6, new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("视图", 4, 4, new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("master", 2, 2, new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("127.0.0.1", 1, 1, new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("服务器", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode8});
            this.treeImgs = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.DbTreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adfsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbtn_AddServer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbtn_Connect = new System.Windows.Forms.ToolStripButton();
            this.toolbtn_unConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbtn_Refrush = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.DbTreeContextMenu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeImgs
            // 
            this.treeImgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImgs.ImageStream")));
            this.treeImgs.TransparentColor = System.Drawing.Color.Transparent;
            this.treeImgs.Images.SetKeyName(0, "Server.ico");
            this.treeImgs.Images.SetKeyName(1, "Computer.ico");
            this.treeImgs.Images.SetKeyName(2, "Database.ico");
            this.treeImgs.Images.SetKeyName(3, "Database.ico");
            this.treeImgs.Images.SetKeyName(4, "6841_1.ico");
            this.treeImgs.Images.SetKeyName(5, "6841_2.ico");
            this.treeImgs.Images.SetKeyName(6, "4531_3.ico");
            this.treeImgs.Images.SetKeyName(7, "Table.ico");
            this.treeImgs.Images.SetKeyName(8, "Solution.ico");
            this.treeImgs.Images.SetKeyName(9, "Table.ico");
            this.treeImgs.Images.SetKeyName(10, "Tables.ico");
            this.treeImgs.Images.SetKeyName(11, "Views.ico");
            this.treeImgs.Images.SetKeyName(12, "Tables.ico");
            this.treeImgs.Images.SetKeyName(13, "Project.ico");
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.ContextMenuStrip = this.DbTreeContextMenu;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.treeImgs;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 7;
            treeNode1.Name = "ID";
            treeNode1.SelectedImageIndex = 7;
            treeNode1.Text = "ID";
            treeNode2.ImageIndex = 4;
            treeNode2.Name = "table1";
            treeNode2.SelectedImageIndex = 4;
            treeNode2.Text = "table1";
            treeNode3.ImageIndex = 4;
            treeNode3.Name = "表";
            treeNode3.SelectedImageIndex = 4;
            treeNode3.Text = "表";
            treeNode4.ImageIndex = 7;
            treeNode4.Name = "节点4";
            treeNode4.SelectedImageIndex = 7;
            treeNode4.Text = "节点4";
            treeNode5.ImageIndex = 6;
            treeNode5.Name = "view1";
            treeNode5.SelectedImageIndex = 6;
            treeNode5.Text = "view1";
            treeNode6.ImageIndex = 4;
            treeNode6.Name = "视图";
            treeNode6.SelectedImageIndex = 4;
            treeNode6.Text = "视图";
            treeNode7.ImageIndex = 2;
            treeNode7.Name = "master";
            treeNode7.SelectedImageIndex = 2;
            treeNode7.Text = "master";
            treeNode8.ImageIndex = 1;
            treeNode8.Name = "";
            treeNode8.SelectedImageIndex = 1;
            treeNode8.Text = "127.0.0.1";
            treeNode9.ImageIndex = 0;
            treeNode9.Name = "";
            treeNode9.SelectedImageIndex = 0;
            treeNode9.Text = "服务器";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(244, 343);
            this.treeView1.TabIndex = 2;
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            // 
            // DbTreeContextMenu
            // 
            this.DbTreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加服务器ToolStripMenuItem,
            this.toolStripMenuItem1});
            this.DbTreeContextMenu.Name = "DbTreeContextMenu";
            this.DbTreeContextMenu.Size = new System.Drawing.Size(135, 32);
            this.DbTreeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.DbTreeContextMenu_Opening);
            // 
            // 添加服务器ToolStripMenuItem
            // 
            this.添加服务器ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adfsToolStripMenuItem});
            this.添加服务器ToolStripMenuItem.Name = "添加服务器ToolStripMenuItem";
            this.添加服务器ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.添加服务器ToolStripMenuItem.Text = "添加服务器";
            // 
            // adfsToolStripMenuItem
            // 
            this.adfsToolStripMenuItem.Name = "adfsToolStripMenuItem";
            this.adfsToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.adfsToolStripMenuItem.Text = "adfs";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(131, 6);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbtn_AddServer,
            this.toolStripSeparator2,
            this.toolbtn_Connect,
            this.toolbtn_unConnect,
            this.toolStripSeparator1,
            this.toolbtn_Refrush});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(244, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolbtn_AddServer
            // 
            this.toolbtn_AddServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtn_AddServer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolbtn_AddServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtn_AddServer.Name = "toolbtn_AddServer";
            this.toolbtn_AddServer.Size = new System.Drawing.Size(35, 22);
            this.toolbtn_AddServer.Text = "注册";
            this.toolbtn_AddServer.ToolTipText = "新增服务器注册";
            this.toolbtn_AddServer.Click += new System.EventHandler(this.toolbtn_AddServer_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolbtn_Connect
            // 
            this.toolbtn_Connect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtn_Connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtn_Connect.Name = "toolbtn_Connect";
            this.toolbtn_Connect.Size = new System.Drawing.Size(35, 22);
            this.toolbtn_Connect.Text = "连接";
            this.toolbtn_Connect.ToolTipText = "连接服务器";
            this.toolbtn_Connect.Click += new System.EventHandler(this.toolbtn_Connect_Click);
            // 
            // toolbtn_unConnect
            // 
            this.toolbtn_unConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtn_unConnect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolbtn_unConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtn_unConnect.Name = "toolbtn_unConnect";
            this.toolbtn_unConnect.Size = new System.Drawing.Size(35, 22);
            this.toolbtn_unConnect.Text = "断开";
            this.toolbtn_unConnect.ToolTipText = "断开服务器";
            this.toolbtn_unConnect.Click += new System.EventHandler(this.toolbtn_unConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolbtn_Refrush
            // 
            this.toolbtn_Refrush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolbtn_Refrush.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolbtn_Refrush.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtn_Refrush.Name = "toolbtn_Refrush";
            this.toolbtn_Refrush.Size = new System.Drawing.Size(35, 22);
            this.toolbtn_Refrush.Text = "刷新";
            this.toolbtn_Refrush.ToolTipText = "刷新";
            this.toolbtn_Refrush.Click += new System.EventHandler(this.toolbtn_Refrush_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // DbView
            // 
            this.ClientSize = new System.Drawing.Size(244, 368);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "DbView";
            this.Text = "DbView";
            this.Load += new System.EventHandler(this.DbView_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.DbView_Layout);
            this.DbTreeContextMenu.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void INSERTItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = this.TreeClickNode.Parent.Parent.Text;
                string tablename = this.TreeClickNode.Text;
                string sQLInsert = ObjHelper.CreatDsb(text).GetSQLInsert(dbname, tablename);
                string pageTitle = tablename + "查询.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, sQLInsert));
            }
        }

        private void LoadServer()
        {
            this.treeView1.Nodes.Clear();
            this.serverlistNode = new TreeNode("服务器");
            this.serverlistNode.Tag = "serverlist";
            this.serverlistNode.ImageIndex = 0;
            this.serverlistNode.SelectedImageIndex = 0;
            this.treeView1.Nodes.Add(this.serverlistNode);
            DbSettings[] settingsArray = DbConfig.GetSettings();
            if (settingsArray != null)
            {
                foreach (DbSettings settings in settingsArray)
                {
                    string server = settings.Server;
                    string dbType = settings.DbType;
                    string dbName = settings.DbName;
                    TreeNode node = new TreeNode(this.GetserverNodeText(server, dbType, dbName));
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    node.Tag = "server";
                    this.serverlistNode.Nodes.Add(node);
                }
                this.serverlistNode.Expand();
            }
        }

        private void LoginServer()
        {
            this.mainfrm = (MainForm) Application.OpenForms["MainForm"];
            if (this.logo.ShowDialog(this) == DialogResult.OK)
            {
                //SplashScreen.ShowSplashScreen();
                Application.DoEvents();
                //SplashScreen.SetStatus("正在进行验证...");
                string text = this.logo.comboBoxServer.Text;
                string dbname = this.logo.dbname;
                string constr = this.logo.constr;
                string selVer = this.logo.GetSelVer();
                try
                {
                    //SplashScreen.SetStatus("加载数据库树...");
                    this.mainfrm.lblViewInfo.Text = "正在验证和连接服务器.....";
                    this.CreatTree(selVer, text, constr, dbname);
                    this.mainfrm.lblViewInfo.Text = "就绪";
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    MessageBox.Show(this, exception.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                //SplashScreen.SetStatus("引导系统模块...");
            }
        }

        private void LoginServerMySQL()
        {
            //this.mainfrm = (MainForm) Application.OpenForms["MainForm"];
            //if (this.loginMysql.ShowDialog(this) == DialogResult.OK)
            //{
            //    //SplashScreen.ShowSplashScreen();
            //    //Application.DoEvents();
            //    //SplashScreen.SetStatus("正在进行验证...");
            //    string text = this.loginMysql.comboBoxServer.Text;
            //    string constr = this.loginMysql.constr;
            //    string dbname = this.loginMysql.dbname;
            //    try
            //    {
            //        //SplashScreen.SetStatus("加载数据库树...");
            //        this.mainfrm.lblViewInfo.Text = "正在验证和连接服务器.....";
            //        this.CreatTree("MySQL", text, constr, dbname);
            //        this.mainfrm.lblViewInfo.Text = "就绪";
            //    }
            //    catch (Exception exception)
            //    {
            //        LogInfo.WriteLog(exception);
            //        MessageBox.Show(this, exception.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        return;
            //    }
            //    //SplashScreen.SetStatus("引导系统模块...");
            //}
        }

        private void LoginServerOledb()
        {
            //switch (this.logoOledb.ShowDialog(this))
            //{
            //    case DialogResult.OK:
            //    {
            //       //SplashScreen.ShowSplashScreen();
            //       // Application.DoEvents();
            //       // SplashScreen.SetStatus("正在进行验证...");
            //        string text = this.logoOledb.txtServer.Text;
            //        string text1 = this.logoOledb.txtUser.Text;
            //        string text2 = this.logoOledb.txtPass.Text;
            //        string txtConstr = this.logoOledb.txtConstr.Text;
            //        string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + ";Persist Security Info=False";
            //        this.GetConstr(text, txtConstr);
            //        try
            //        {
            //           // SplashScreen.SetStatus("加载数据库树...");
            //            this.mainfrm.lblViewInfo.Text = "正在验证和连接服务器.....";
            //            this.CreatTree("OleDb", text, constr, "");
            //            this.mainfrm.lblViewInfo.Text = "就绪";
            //        }
            //        catch (Exception exception)
            //        {
            //            LogInfo.WriteLog(exception);
            //            MessageBox.Show(this, exception.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //            break;
            //        }
            //       // SplashScreen.SetStatus("引导系统模块...");
            //        break;
            //    }
            //    case DialogResult.Cancel:
            //        break;

            //    default:
            //        return;
            //}
        }

        private void LoginServerOra()
        {
            //this.mainfrm = (MainForm) Application.OpenForms["MainForm"];
            //if (this.logoOra.ShowDialog(this) == DialogResult.OK)
            //{
            //   // SplashScreen.ShowSplashScreen();
            //    Application.DoEvents();
            //   // SplashScreen.SetStatus("正在进行验证...");
            //    string text = this.logoOra.txtServer.Text;
            //    string constr = this.logoOra.constr;
            //    string dbname = this.logoOra.dbname;
            //    try
            //    {
            //       // SplashScreen.SetStatus("加载数据库树...");
            //        this.mainfrm.lblViewInfo.Text = "正在验证和连接服务器.....";
            //        this.CreatTree("Oracle", text, constr, dbname);
            //        this.mainfrm.lblViewInfo.Text = "就绪";
            //    }
            //    catch (Exception exception)
            //    {
            //        LogInfo.WriteLog(exception);
            //        MessageBox.Show(this, exception.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        return;
            //    }
            //   // SplashScreen.SetStatus("引导系统模块...");
            //}
        }

        public void RegServer()
        {
            try
            {
                //TODO 20090921 用于选择数据库类型
                //DbTypeSel sel = new DbTypeSel();
                //if (sel.ShowDialog(this) != DialogResult.OK)
                //{
                //    goto Label_00A3;
                //}
                //string dbtype = sel.dbtype;
                FrmDataSource sel = new FrmDataSource(); 
                if (sel.ShowDialog(this) != DialogResult.OK)
                {
                    goto Label_00A3;
                }
                string dbtype = sel.dbtype;
                //end
                if (dbtype == null)
                {
                    goto Label_008A;
                }
                if (!(dbtype == "SQL2000") && !(dbtype == "SQL2005"))
                {
                    if (dbtype == "Oracle")
                    {
                        goto Label_0072;
                    }
                    if (dbtype == "OleDb")
                    {
                        goto Label_007A;
                    }
                    if (dbtype == "MySQL")
                    {
                        goto Label_0082;
                    }
                    goto Label_008A;
                }
                this.LoginServer();
                goto Label_0090;
            Label_0072:
                this.LoginServerOra();
                goto Label_0090;
            Label_007A:
                this.LoginServerOledb();
                goto Label_0090;
            Label_0082:
                this.LoginServerMySQL();
                goto Label_0090;
            Label_008A:
                this.LoginServer();
            Label_0090:
                if (this.serverlistNode != null)
                {
                    this.serverlistNode.Expand();
                }
            Label_00A3:
                //SplashScreen.CloseForm();
                int i=0;
            }
            catch
            {
                MessageBox.Show("连接服务器失败，请关闭后重新打开再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void SELECTItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = this.TreeClickNode.Parent.Parent.Text;
                string tablename = this.TreeClickNode.Text;
                string sQLSelect = ObjHelper.CreatDsb(text).GetSQLSelect(dbname, tablename);
                string pageTitle = tablename + "查询.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, sQLSelect));
            }
        }

        private void server属性Item_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                DbSettings setting = DbConfig.GetSetting(this.TreeClickNode.Text);
                string server = setting.Server;
                string dbType = setting.DbType;
                string dbName = setting.DbName;
                bool connectSimple = setting.ConnectSimple;
                try
                {
                    this.ConnectServer(this.TreeClickNode, dbType, server, dbName, connectSimple);
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    MessageBox.Show("连接服务器失败：" + exception.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void toolbtn_AddServer_Click(object sender, EventArgs e)
        {
            this.RegServer();
        }

        private void toolbtn_Connect_Click(object sender, EventArgs e)
        {
            if (((this.TreeClickNode != null) && (this.TreeClickNode.Tag.ToString() == "server")) && !((this.TreeClickNode.Tag.ToString() == "server") & (this.TreeClickNode.Nodes.Count > 0)))
            {
                DbSettings setting = DbConfig.GetSetting(this.TreeClickNode.Text);
                string server = setting.Server;
                string dbType = setting.DbType;
                string dbName = setting.DbName;
                bool connectSimple = setting.ConnectSimple;
                try
                {
                    this.ConnectServer(this.TreeClickNode, dbType, server, dbName, connectSimple);
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    MessageBox.Show("连接服务器失败：" + exception.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void toolbtn_Refrush_Click(object sender, EventArgs e)
        {
        }

        private void toolbtn_unConnect_Click(object sender, EventArgs e)
        {
            if ((this.TreeClickNode != null) && (this.TreeClickNode.Tag.ToString() == "server"))
            {
                try
                {
                    if ((this.TreeClickNode.Tag.ToString() == "server") & (this.TreeClickNode.Nodes.Count > 0))
                    {
                        this.TreeClickNode.Nodes.Clear();
                    }
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    MessageBox.Show("操作失败：" + exception.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode selectedNode = this.treeView1.SelectedNode;
                string text = selectedNode.Text;
                string str2 = selectedNode.Tag.ToString().ToLower();
                this.mainfrm = (MainForm) Application.OpenForms["MainForm"];
                DbBrowser browser = (DbBrowser) Application.OpenForms["DbBrowser"];
                if (browser != null)
                {
                    this.mainfrm.lblViewInfo.Text = "正在检索数据库.....";
                    browser.SetListView(this);
                }
                CodeMaker maker = (CodeMaker) Application.OpenForms["CodeMaker"];
                if (maker != null)
                {
                    this.mainfrm.lblViewInfo.Text = "正在检索数据库.....";
                    maker.SetListView(this);
                }
                CodeMakerM rm = (CodeMakerM) Application.OpenForms["CodeMakerM"];
                if (rm != null)
                {
                    this.mainfrm.lblViewInfo.Text = "正在检索数据库.....";
                    rm.SetListView(this);
                }
                CodeTemplate template = (CodeTemplate) Application.OpenForms["CodeTemplate"];
                if (template != null)
                {
                    this.mainfrm.lblViewInfo.Text = "正在检索数据库.....";
                    template.SetListView(this);
                }
                switch (str2)
                {
                    case "serverlist":
                        this.mainfrm.toolComboBox_DB.Items.Clear();
                        this.mainfrm.toolComboBox_Table.Items.Clear();
                        this.mainfrm.toolComboBox_DB.Visible = false;
                        this.mainfrm.toolComboBox_Table.Visible = false;
                        this.mainfrm.生成ToolStripMenuItem.Visible = false;
                        break;

                    case "server":
                        this.mainfrm.toolComboBox_DB.Visible = true;
                        this.mainfrm.toolComboBox_Table.Visible = false;
                        this.mainfrm.生成ToolStripMenuItem.Visible = false;
                        break;

                    case "db":
                        this.mainfrm.toolComboBox_DB.Visible = true;
                        this.mainfrm.toolComboBox_Table.Visible = true;
                        this.mainfrm.toolComboBox_DB.Text = selectedNode.Parent.Text;
                        this.mainfrm.生成ToolStripMenuItem.Visible = false;
                        break;

                    case "tableroot":
                    case "viewroot":
                        this.mainfrm.toolComboBox_DB.Visible = true;
                        this.mainfrm.toolComboBox_Table.Visible = true;
                        this.mainfrm.toolComboBox_DB.Text = selectedNode.Parent.Text;
                        this.mainfrm.生成ToolStripMenuItem.Visible = false;
                        break;

                    case "table":
                        this.mainfrm.toolComboBox_DB.Visible = true;
                        this.mainfrm.toolComboBox_Table.Visible = true;
                        this.mainfrm.toolComboBox_DB.Text = selectedNode.Parent.Parent.Text;
                        this.mainfrm.toolComboBox_Table.Text = text;
                        this.mainfrm.生成ToolStripMenuItem.Visible = true;
                        this.mainfrm.对象定义ToolStripMenuItem.Visible = false;
                        this.mainfrm.toolStripMenuItem17.Visible = false;
                        this.mainfrm.生成存储过程ToolStripMenuItem.Visible = true;
                        this.mainfrm.生成数据脚本ToolStripMenuItem.Visible = true;
                        break;

                    case "view":
                        this.mainfrm.toolComboBox_DB.Visible = true;
                        this.mainfrm.toolComboBox_Table.Visible = true;
                        this.mainfrm.toolComboBox_DB.Text = selectedNode.Parent.Parent.Text;
                        this.mainfrm.toolComboBox_Table.Text = text;
                        this.mainfrm.生成ToolStripMenuItem.Visible = true;
                        this.mainfrm.对象定义ToolStripMenuItem.Visible = true;
                        this.mainfrm.toolStripMenuItem17.Visible = true;
                        this.mainfrm.生成存储过程ToolStripMenuItem.Visible = false;
                        this.mainfrm.生成数据脚本ToolStripMenuItem.Visible = false;
                        break;

                    case "proc":
                        this.mainfrm.生成ToolStripMenuItem.Visible = true;
                        this.mainfrm.对象定义ToolStripMenuItem.Visible = true;
                        this.mainfrm.toolStripMenuItem17.Visible = true;
                        this.mainfrm.生成存储过程ToolStripMenuItem.Visible = false;
                        this.mainfrm.生成数据脚本ToolStripMenuItem.Visible = false;
                        break;

                    default:
                        this.mainfrm.生成ToolStripMenuItem.Visible = false;
                        break;
                }
                this.mainfrm.lblViewInfo.Text = "就绪";
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog(exception);
                MessageBox.Show(exception.Message);
            }
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode item = (TreeNode) e.Item;
            if (((item.Tag.ToString() == "table") || (item.Tag.ToString() == "view")) || (item.Tag.ToString() == "column"))
            {
                base.DoDragDrop(item, DragDropEffects.Copy);
            }
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Point pt = new Point(e.X, e.Y);
                this.TreeClickNode = this.treeView1.GetNodeAt(pt);
                this.treeView1.SelectedNode = this.TreeClickNode;
                if (this.TreeClickNode != null)
                {
                    this.CreatMenu(this.TreeClickNode.Tag.ToString());
                    if (e.Button == MouseButtons.Right)
                    {
                        this.DbTreeContextMenu.Show(this.treeView1, pt);
                    }
                }
                else
                {
                    this.DbTreeContextMenu.Items.Clear();
                }
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog(exception);
            }
        }

        private void UPDATEItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = this.TreeClickNode.Parent.Parent.Text;
                string tablename = this.TreeClickNode.Text;
                string sQLUpdate = ObjHelper.CreatDsb(text).GetSQLUpdate(dbname, tablename);
                string pageTitle = tablename + "查询.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, sQLUpdate));
            }
        }

        private void 备份服务器配置Item_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "保存服务器配置";
            dialog.Filter = "DB Serverlist(*.config)|*.config|All files (*.*)|*.*";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                DbConfig.GetSettingDs().WriteXml(fileName);
            }
        }

        private void 表数据dbItem_Click(object sender, EventArgs e)
        {
        }

        private void 表数据tabItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string text1 = this.TreeClickNode.Parent.Parent.Text;
                string text2 = this.TreeClickNode.Text;
                ObjHelper.CreatDbObj(text);
            }
        }

        private void 查看表数据tabItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbName = this.TreeClickNode.Parent.Parent.Text;
                string tabName = this.TreeClickNode.Text;
                DataList ctrForm = new DataList(ObjHelper.CreatDbObj(text), dbName, tabName);
                this.AddTabPage(tabName, ctrForm, this.mainfrm);
            }
        }

        private void 存储过程dbItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "保存当前脚本";
            dialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            if ((dialog.ShowDialog(this) == DialogResult.OK) && (this.TreeClickNode != null))
            {
                string text = this.TreeClickNode.Parent.Text;
                string dbname = this.TreeClickNode.Text;
                string pROCCode = ObjHelper.CreatDsb(text).GetPROCCode(dbname);
                StreamWriter writer = new StreamWriter(dialog.FileName, false, Encoding.Default);
                writer.Write(pROCCode);
                writer.Flush();
                writer.Close();
                MessageBox.Show("脚本生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void 存储过程tabItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "保存当前脚本";
            dialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            if ((dialog.ShowDialog(this) == DialogResult.OK) && (this.TreeClickNode != null))
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = this.TreeClickNode.Parent.Parent.Text;
                string tablename = this.TreeClickNode.Text;
                IDbScriptBuilder builder = ObjHelper.CreatDsb(text);
                builder.DbName = dbname;
                builder.TableName = tablename;
                builder.ProjectName = this.setting.ProjectName;
                builder.ProcPrefix = this.setting.ProcPrefix;
                builder.Keys = new List<LTP.CodeHelper.ColumnInfo>();
                builder.Fieldlist = new List<LTP.CodeHelper.ColumnInfo>();
                string pROCCode = builder.GetPROCCode(dbname, tablename);
                StreamWriter writer = new StreamWriter(dialog.FileName, false, Encoding.Default);
                writer.Write(pROCCode);
                writer.Flush();
                writer.Close();
                MessageBox.Show("脚本生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void 代码生成dbItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode.Parent.Text != "")
            {
                this.mainfrm.AddSinglePage(new CodeMaker(), "代码生成器");
            }
        }

        private void 代码生成Item_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode.Parent.Parent.Parent.Text != "")
            {
                this.mainfrm.AddSinglePage(new CodeMaker(), "代码生成器");
            }
        }

        private void 导入服务器配置Item_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择服务器配置文件";
            dialog.Filter = "DB Serverlist(*.config)|*.config|All files (*.*)|*.*";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string fileName = dialog.FileName;
                    DataSet set = new DataSet();
                    if (File.Exists(fileName))
                    {
                        set.ReadXml(fileName);
                        string str2 = Application.StartupPath + @"\DbSetting.config";
                        set.WriteXml(str2);
                    }
                    this.LoadServer();
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    MessageBox.Show("读取配置文件失败！", "提示");
                }
            }
        }

        private void 对象定义Item_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbName = this.TreeClickNode.Parent.Parent.Text;
                string objName = this.TreeClickNode.Text;
                string objectInfo = ObjHelper.CreatDbObj(text).GetObjectInfo(dbName, objName);
                string pageTitle = objName + "定义.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, objectInfo));
            }
        }

        private void 父子表代码生成dbItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Text;
                string dbname = this.TreeClickNode.Text;
                if (text != "")
                {
                    this.mainfrm.AddSinglePage(new CodeMakerM(dbname), "父子表代码生成");
                }
            }
        }

        private void 连接服务器Item_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                DbSettings setting = DbConfig.GetSetting(this.TreeClickNode.Text);
                string server = setting.Server;
                string dbType = setting.DbType;
                string dbName = setting.DbName;
                bool connectSimple = setting.ConnectSimple;
                try
                {
                    this.ConnectServer(this.TreeClickNode, dbType, server, dbName, connectSimple);
                }
                catch (Exception exception)
                {
                    LogInfo.WriteLog(exception);
                    MessageBox.Show("连接服务器失败，请关闭后重新打开再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void 模版代码生成Item_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode.Parent.Parent.Parent.Text != "")
            {
                this.mainfrm.AddSinglePage(new CodeTemplate(this.mainfrm), "模版代码生成器");
            }
        }

        private void 删除tabItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbName = this.TreeClickNode.Parent.Parent.Text;
                string tableName = this.TreeClickNode.Text;
                IDbObject obj2 = ObjHelper.CreatDbObj(text);
                if (MessageBox.Show(this, "你确认要删除改表吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (obj2.DeleteTable(dbName, tableName))
                    {
                        this.TreeClickNode.Remove();
                        MessageBox.Show(this, "表" + tableName + "删除成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show(this, "表" + tableName + "删除失败，请稍候重试！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void 生成BLLF3Item_Click(object sender, EventArgs e)
        {
        }

        private void 生成BLLS3Item_Click(object sender, EventArgs e)
        {
        }

        private void 生成DALF3Item_Click(object sender, EventArgs e)
        {
        }

        private void 生成DALFactoryItem_Click(object sender, EventArgs e)
        {
        }

        private void 生成DALS3Item_Click(object sender, EventArgs e)
        {
        }

        private void 生成IDALItem_Click(object sender, EventArgs e)
        {
        }

        private void 生成ModelItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string str2 = this.TreeClickNode.Parent.Parent.Text;
                string str3 = this.TreeClickNode.Text;
                CodeBuilders builders = ObjHelper.CreatCB(text);
                builders.DbName = str2;
                builders.TableName = str3;
                string strSQL = builders.GetCodeFrameS3Model();
                string pageTitle = str3;
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, strSQL));
            }
        }

        private void 生成存储过程dbItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Text;
                string dbname = this.TreeClickNode.Text;
                string pROCCode = ObjHelper.CreatDsb(text).GetPROCCode(dbname);
                string pageTitle = dbname + "存储过程.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, pROCCode));
            }
        }

        private void 生成存储过程tabItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = this.TreeClickNode.Parent.Parent.Text;
                string tablename = this.TreeClickNode.Text;
                IDbScriptBuilder builder = ObjHelper.CreatDsb(text);
                builder.DbName = dbname;
                builder.TableName = tablename;
                builder.ProjectName = this.setting.ProjectName;
                builder.ProcPrefix = this.setting.ProcPrefix;
                builder.Keys = new List<LTP.CodeHelper.ColumnInfo>();
                builder.Fieldlist = new List<LTP.CodeHelper.ColumnInfo>();
                string pROCCode = builder.GetPROCCode(dbname, tablename);
                string pageTitle = tablename + "存储过程.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, pROCCode));
            }
        }

        private void 生成单类结构Items_Click(object sender, EventArgs e)
        {
        }

        private void 生成全部F3Item_Click(object sender, EventArgs e)
        {
        }

        private void 生成全部S3Item_Click(object sender, EventArgs e)
        {
        }

        private void 生成数据脚本dbItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Text;
                string dbname = this.TreeClickNode.Text;
                new DbToScript(text, dbname).ShowDialog(this);
            }
        }

        private void 生成数据脚本tabItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "如果该表数据量较大，直接生成将需要比较长的时间，\r\n确实需要直接生成吗？\r\n(建议采用脚本生成器生成。)", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if ((result == DialogResult.Yes) && (this.TreeClickNode != null))
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = this.TreeClickNode.Parent.Parent.Text;
                string tablename = this.TreeClickNode.Text;
                IDbScriptBuilder builder = ObjHelper.CreatDsb(text);
                builder.Fieldlist = new List<LTP.CodeHelper.ColumnInfo>();
                string strSQL = builder.CreateTabScript(dbname, tablename);
                string pageTitle = tablename + "脚本.sql";
                this.AddTabPage(pageTitle, new DbQuery(this.mainfrm, strSQL));
            }
            if ((result == DialogResult.No) && (this.TreeClickNode != null))
            {
                string longservername = this.TreeClickNode.Parent.Parent.Parent.Text;
                string str7 = this.TreeClickNode.Parent.Parent.Text;
                string text1 = this.TreeClickNode.Text;
                new DbToScript(longservername, str7).ShowDialog(this);
            }
        }

        private void 生成页面Item_Click(object sender, EventArgs e)
        {
        }

        private void 数据脚本dbItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Text;
                string dbname = this.TreeClickNode.Text;
                new DbToScript(text, dbname).ShowDialog(this);
            }
        }

        private void 数据脚本tabItem_Click(object sender, EventArgs e)
        {
            string text = this.TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = this.TreeClickNode.Parent.Parent.Text;
            new DbToScript(text, dbname).ShowDialog(this);
        }

        private void 刷新dbItem_Click(object sender, EventArgs e)
        {
            TreeNode treeClickNode = this.TreeClickNode;
            DbSettings setting = DbConfig.GetSetting(treeClickNode.Parent.Text);
            string server = setting.Server;
            string dbType = setting.DbType;
            treeClickNode.Nodes.Clear();
            IDbObject obj2 = DBOMaker.CreateDbObj(dbType);
            obj2.DbConnectStr = setting.ConnectStr;
            string text = treeClickNode.Text;
            this.mainfrm.lblViewInfo.Text = "加载数据库" + text + "...";
            TreeNode node = new TreeNode("表");
            node.ImageIndex = 3;
            node.SelectedImageIndex = 3;
            node.Tag = "tableroot";
            treeClickNode.Nodes.Add(node);
            TreeNode node3 = new TreeNode("视图");
            node3.ImageIndex = 3;
            node3.SelectedImageIndex = 3;
            node3.Tag = "viewroot";
            treeClickNode.Nodes.Add(node3);
            TreeNode node4 = new TreeNode("存储过程");
            node4.ImageIndex = 3;
            node4.SelectedImageIndex = 3;
            node4.Tag = "procroot";
            treeClickNode.Nodes.Add(node4);
            try
            {
                List<string> tables = obj2.GetTables(text);
                if (tables.Count > 0)
                {
                    foreach (string str4 in tables)
                    {
                        TreeNode node5 = new TreeNode(str4);
                        node5.ImageIndex = 4;
                        node5.SelectedImageIndex = 4;
                        node5.Tag = "table";
                        node.Nodes.Add(node5);
                        List<LTP.CodeHelper.ColumnInfo> columnList = obj2.GetColumnList(text, str4);
                        if ((columnList != null) && (columnList.Count > 0))
                        {
                            foreach (LTP.CodeHelper.ColumnInfo info in columnList)
                            {
                                string columnName = info.ColumnName;
                                string typeName = info.TypeName;
                                TreeNode node6 = new TreeNode(columnName + "[" + typeName + "]");
                                node6.ImageIndex = 7;
                                node6.SelectedImageIndex = 7;
                                node6.Tag = "column";
                                node5.Nodes.Add(node6);
                            }
                            continue;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog(exception);
                MessageBox.Show(this, "获取数据库" + text + "的表信息失败：" + exception.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            try
            {
                DataTable vIEWs = obj2.GetVIEWs(text);
                if (vIEWs != null)
                {
                    foreach (DataRow row in vIEWs.Select("", "name ASC"))
                    {
                        string str7 = row["name"].ToString();
                        TreeNode node7 = new TreeNode(str7);
                        node7.ImageIndex = 4;
                        node7.SelectedImageIndex = 4;
                        node7.Tag = "view";
                        node3.Nodes.Add(node7);
                        List<LTP.CodeHelper.ColumnInfo> list3 = obj2.GetColumnList(text, str7);
                        if ((list3 != null) && (list3.Count > 0))
                        {
                            foreach (LTP.CodeHelper.ColumnInfo info2 in list3)
                            {
                                string str8 = info2.ColumnName;
                                string str9 = info2.TypeName;
                                TreeNode node8 = new TreeNode(str8 + "[" + str9 + "]");
                                node8.ImageIndex = 7;
                                node8.SelectedImageIndex = 7;
                                node8.Tag = "column";
                                node7.Nodes.Add(node8);
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                LogInfo.WriteLog(exception2);
                MessageBox.Show(this, "获取数据库" + text + "的视图信息失败：" + exception2.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            try
            {
                DataTable procs = obj2.GetProcs(text);
                if (procs != null)
                {
                    foreach (DataRow row2 in procs.Select("", "name ASC"))
                    {
                        string str10 = row2["name"].ToString();
                        TreeNode node9 = new TreeNode(str10);
                        node9.ImageIndex = 4;
                        node9.SelectedImageIndex = 4;
                        node9.Tag = "proc";
                        node4.Nodes.Add(node9);
                        List<LTP.CodeHelper.ColumnInfo> list4 = obj2.GetColumnList(text, str10);
                        if ((list4 != null) && (list4.Count > 0))
                        {
                            foreach (LTP.CodeHelper.ColumnInfo info3 in list4)
                            {
                                string str11 = info3.ColumnName;
                                string str12 = info3.TypeName;
                                TreeNode node10 = new TreeNode(str11 + "[" + str12 + "]");
                                node10.ImageIndex = 9;
                                node10.SelectedImageIndex = 9;
                                node10.Tag = "column";
                                node9.Nodes.Add(node10);
                            }
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                LogInfo.WriteLog(exception3);
                MessageBox.Show(this, "获取数据库" + text + "的视图信息失败：" + exception3.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this.mainfrm.lblViewInfo.Text = "就绪";
        }

        private void 刷新Item_Click(object sender, EventArgs e)
        {
            this.LoadServer();
        }

        private void 添加服务器Item_Click(object sender, EventArgs e)
        {
            this.RegServer();
        }

        private void 新建查询Item_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Text;
                string pageTitle = this.TreeClickNode.Parent.Text + "." + text + "查询.sql";
                MainForm mainfrm = (MainForm) Application.OpenForms["MainForm"];
                this.AddTabPage(pageTitle, new DbQuery(mainfrm, ""), mainfrm);
                mainfrm.toolComboBox_DB.Text = text;
            }
        }

        private void 重命名tabItem_Click(object sender, EventArgs e)
        {
            if (this.TreeClickNode != null)
            {
                string text = this.TreeClickNode.Parent.Parent.Parent.Text;
                string dbName = this.TreeClickNode.Parent.Parent.Text;
                string oldName = this.TreeClickNode.Text;
                IDbObject obj2 = ObjHelper.CreatDbObj(text);
                RenameFrm frm = new RenameFrm();
                frm.txtName.Text = oldName;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    string newName = frm.txtName.Text.Trim();
                    if (obj2.RenameTable(dbName, oldName, newName))
                    {
                        this.TreeClickNode.Text = newName;
                        MessageBox.Show(this, "表名修改成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show(this, "表名修改失败，请稍候重试！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void 属性Item_Click(object sender, EventArgs e)
        {
        }

        private void 注销服务器Item_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.TreeClickNode != null)
                {
                    DbSettings setting = DbConfig.GetSetting(this.TreeClickNode.Text);
                    if (setting != null)
                    {
                        DbConfig.DelSetting(setting.DbType, setting.Server, setting.DbName);
                    }
                    this.serverlistNode.Nodes.Remove(this.TreeClickNode);
                }
            }
            catch
            {
                MessageBox.Show("注销服务器失败，请关闭后重新打开再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void 自动输出代码dbItem_Click(object sender, EventArgs e)
        {
            string text = this.TreeClickNode.Parent.Text;
            if (text != "")
            {
                new CodeExport(text).ShowDialog(this);
            }
        }

        private void 自动输出代码Item_Click(object sender, EventArgs e)
        {
            string text = this.TreeClickNode.Parent.Parent.Parent.Text;
            if (text != "")
            {
                new CodeExport(text).ShowDialog(this);
            }
        }

        private void DbTreeContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}


namespace Codematic
{
    using LTP.CodeHelper;
    using LTP.DBFactory;
    using LTP.IDBO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DbBrowser : Form
    {
        private IContainer components;
        private IDbObject dbobj;
        private GroupBox groupBox1;
        private ImageList imglistDB;
        private ImageList imglistView;
        private Label lblNum;
        private Label lblViewInfo;
        //private ListView listView1;
        private PictureBox pictureBox1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSplitButton toolStripSplitButton1;
        private ToolStripMenuItem 列表ToolStripMenuItem;
        private ImageList imageList1;
        private ListView listView1;
        private ToolStripMenuItem 详细信息ToolStripMenuItem;

        public DbBrowser()
        {
            this.InitializeComponent();
            DbView dbviewfrm = (DbView) Application.OpenForms["DbView"];
            this.SetListView(dbviewfrm);
        }

        private void BindlistViewCol(string Dbname, string TableName)
        {
            this.SetListViewMenu("colum");
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.LargeImageList = this.imglistView;
            this.listView1.SmallImageList = this.imglistView;
            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            this.listView1.FullRowSelect = true;
            this.listView1.Columns.Add("序号", 60, HorizontalAlignment.Left);
            this.listView1.Columns.Add("列名", 110, HorizontalAlignment.Left);
            this.listView1.Columns.Add("数据类型", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("长度", 40, HorizontalAlignment.Left);
            this.listView1.Columns.Add("小数", 40, HorizontalAlignment.Left);
            this.listView1.Columns.Add("标识", 40, HorizontalAlignment.Center);
            this.listView1.Columns.Add("主键", 40, HorizontalAlignment.Center);
            this.listView1.Columns.Add("允许空", 60, HorizontalAlignment.Center);
            this.listView1.Columns.Add("默认值", 100, HorizontalAlignment.Left);
            List<ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(Dbname, TableName);
            if ((columnInfoList != null) && (columnInfoList.Count > 0))
            {
                foreach (ColumnInfo info in columnInfoList)
                {
                    string str10;
                    string colorder = info.Colorder;
                    string columnName = info.ColumnName;
                    string typeName = info.TypeName;
                    string length = info.Length;
                    if (((str10 = typeName) != null) && (((str10 == "varchar") || (str10 == "nvarchar")) || (((str10 == "char") || (str10 == "nchar")) || (str10 == "varbinary"))))
                    {
                        length = CodeCommon.GetDataTypeLenVal(typeName, length);
                    }
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
                    if ((str8 != "√") || (str9.Trim() != ""))
                    {
                        str8 = "";
                    }
                    item.SubItems.Add(str8);
                    item.SubItems.Add(str9);
                    item.SubItems.Add(defaultVal);
                    this.listView1.Items.AddRange(new ListViewItem[] { item });
                }
            }
        }

        private void BindlistViewTab(string Dbname, string SelNodeType)
        {
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.LargeImageList = this.imglistView;
            this.listView1.SmallImageList = this.imglistView;
            this.listView1.View = View.Details;
            this.listView1.FullRowSelect = true;
            this.listView1.Columns.Add("名称", 250, HorizontalAlignment.Left);
            this.listView1.Columns.Add("所有者", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("类型", 60, HorizontalAlignment.Left);
            this.listView1.Columns.Add("创建日期", 200, HorizontalAlignment.Left);
            List<TableInfo> tablesInfo = null;
            string str5 = SelNodeType;
            if (str5 != null)
            {
                if (!(str5 == "db"))
                {
                    if (str5 == "tableroot")
                    {
                        tablesInfo = this.dbobj.GetTablesInfo(Dbname);
                    }
                    else if (str5 == "viewroot")
                    {
                        tablesInfo = this.dbobj.GetVIEWsInfo(Dbname);
                    }
                    else if (str5 == "procroot")
                    {
                        tablesInfo = this.dbobj.GetProcInfo(Dbname);
                    }
                }
                else
                {
                    tablesInfo = this.dbobj.GetTabViewsInfo(Dbname);
                }
            }
            if ((tablesInfo != null) && (tablesInfo.Count > 0))
            {
                foreach (TableInfo info in tablesInfo)
                {
                    string str3;
                    ListViewItem item = new ListViewItem(info.TabName, 0);
                    string tabUser = info.TabUser;
                    item.SubItems.Add(tabUser);
                    switch (info.TabType.Trim())
                    {
                        case "S":
                            str3 = "系统";
                            break;

                        case "U":
                            str3 = "用户";
                            item.ImageIndex = 2;
                            break;

                        case "TABLE":
                            str3 = "表";
                            item.ImageIndex = 2;
                            break;

                        case "V":
                        case "VIEW":
                            str3 = "视图";
                            item.ImageIndex = 3;
                            break;

                        case "P":
                            str3 = "存储过程";
                            item.ImageIndex = 5;
                            break;

                        default:
                            str3 = "系统";
                            break;
                    }
                    item.SubItems.Add(str3);
                    string tabDate = info.TabDate;
                    item.SubItems.Add(tabDate);
                    this.listView1.Items.AddRange(new ListViewItem[] { item });
                }
            }
        }

        private void CreatDbObj(string servername)
        {
            DbSettings setting = DbConfig.GetSetting(servername);
            this.dbobj = DBOMaker.CreateDbObj(setting.DbType);
            this.dbobj.DbConnectStr = setting.ConnectStr;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbBrowser));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "toolbtn_AddServer.Image.png");
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(292, 273);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // DbBrowser
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.listView1);
            this.Name = "DbBrowser";
            this.ResumeLayout(false);

        }

        public void SetListView(DbView dbviewfrm)
        {
            TreeNode selectedNode = dbviewfrm.treeView1.SelectedNode;
            if (selectedNode != null)
            {
                string servername = "";
                switch (selectedNode.Tag.ToString())
                {
                    case "server":
                        servername = selectedNode.Text;
                        this.CreatDbObj(servername);
                        //this.lblViewInfo.Text = " 服务器：" + servername;                        
                       // this.lblNum.Text = selectedNode.Nodes.Count.ToString() + "项";
                        this.listView1.Columns.Clear();
                        this.listView1.Items.Clear();
                        //this.listView1.LargeImageList = this.imglistDB;
                        this.listView1.View = View.LargeIcon;
                        foreach (TreeNode node2 in selectedNode.Nodes)
                        {
                            string text = node2.Text;
                            ListViewItem item = new ListViewItem(text, 0);
                            item.SubItems.Add(text);
                            item.ImageIndex = 0;
                            this.listView1.Items.AddRange(new ListViewItem[] { item });
                        }
                        this.SetListViewMenu("db");
                        return;

                    case "db":
                        servername = selectedNode.Parent.Text;
                        this.CreatDbObj(servername);
                        //this.lblViewInfo.Text = " 数据库：" + selectedNode.Text;
                       // this.lblNum.Text = selectedNode.Nodes.Count.ToString() + "项";
                        this.SetListViewMenu("table");
                        this.BindlistViewTab(selectedNode.Text, selectedNode.Tag.ToString());
                        return;

                    case "tableroot":
                    case "viewroot":
                    case "procroot":
                    {
                        servername = selectedNode.Parent.Parent.Text;
                        string dbname = selectedNode.Parent.Text;
                        this.CreatDbObj(servername);
                        //this.lblViewInfo.Text = " 数据库：" + dbname;
                        //this.lblNum.Text = selectedNode.Nodes.Count.ToString() + "项";
                        this.SetListViewMenu("table");
                        this.BindlistViewTab(dbname, selectedNode.Tag.ToString());
                        return;
                    }
                    case "table":
                    case "view":
                    {
                        servername = selectedNode.Parent.Parent.Parent.Text;
                        string str4 = selectedNode.Parent.Parent.Text;
                        string tableName = selectedNode.Text;
                        this.CreatDbObj(servername);
                        //this.lblViewInfo.Text = " 表：" + tableName;
                        //this.lblNum.Text = selectedNode.Nodes.Count.ToString() + "项";
                        this.SetListViewMenu("column");
                        this.BindlistViewCol(str4, tableName);
                        return;
                    }
                    case "proc":
                    {
                        servername = selectedNode.Parent.Parent.Parent.Text;
                        string text1 = selectedNode.Parent.Parent.Text;
                        string str6 = selectedNode.Text;
                        this.CreatDbObj(servername);
                        //this.lblViewInfo.Text = " 存储过程：" + str6;
                        //this.lblNum.Text = selectedNode.Nodes.Count.ToString() + "项";
                        this.listView1.Items.Clear();
                        return;
                    }
                    case "column":
                        servername = selectedNode.Parent.Parent.Parent.Parent.Text;
                        return;
                }
            }
        }

        private void SetListViewMenu(string itemType)
        {
            string str;
            if ((((str = itemType.ToLower()) != null) && !(str == "server")) && (((str == "db") || (str == "table")) || !(str == "column")))
            {
            }
        }
    }
}


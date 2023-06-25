using Flextronics.Applications.ApplicationFactory.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Flextronics.Applications.Library.Schema;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.CodeBuilder;

namespace Flextronics.Applications.ApplicationFactory
{
   

    public class CodeMakerM : Form
    {
        private Button btn_Next;
        private Button btn_Ok;
        private Button btn_SelAll;
        private Button btn_SelAll2;
        private Button btn_SelI;
        private Button btn_SelI2;
        private CheckBox chk_CS_Add;
        private CheckBox chk_CS_Delete;
        private CheckBox chk_CS_Exists;
        private CheckBox chk_CS_GetList;
        private CheckBox chk_CS_GetModel;
        private CheckBox chk_CS_GetModelByCache;
        private CheckBox chk_CS_Update;
        private CheckBox chk_DB_Add;
        private CheckBox chk_DB_Delete;
        private CheckBox chk_DB_Exists;
        private CheckBox chk_DB_GetList;
        private CheckBox chk_DB_GetMaxID;
        private CheckBox chk_DB_GetModel;
        private CheckBox chk_DB_Update;
        private CheckBox chk_Web_Add;
        private CheckBox chk_Web_HasKey;
        private CheckBox chk_Web_Show;
        private CheckBox chk_Web_Update;
        private DALTypeAddIn cm_blltype;
        private DALTypeAddIn cm_daltype;
        private ComboBox cmbox_PField;
        private ComboBox cmbox_PTab;
        private ComboBox cmbox_SField;
        private ComboBox cmbox_STab;
        private UcCodeView codeview;
        private IContainer components;
        private string dbname;
        private IDbObject dbobj;
        private GroupBox groupBox_AppType;
        private GroupBox groupBox_DALType;
        private GroupBox groupBox_DB;
        private GroupBox groupBox_F3;
        private GroupBox groupBox_FrameSel;
        private GroupBox groupBox_Method;
        private GroupBox groupBox_Parameter;
        private GroupBox groupBox_Type;
        private GroupBox groupBox_Web;
        private GroupBox groupBox1;
        private ImageList imglistDB;
        private ImageList imgListTabpage;
        private ImageList imglistView;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ListView listView1;
        private ListView listView2;
        private Panel panel1;
        private Panel panel2;
        private RadioButton radbtn_AppType_Web;
        private RadioButton radbtn_AppType_Winform;
        private RadioButton radbtn_DB_DDL;
        private RadioButton radbtn_DB_Proc;
        private RadioButton radbtn_F3_BLL;
        private RadioButton radbtn_F3_DAL;
        private RadioButton radbtn_F3_DALFactory;
        private RadioButton radbtn_F3_IDAL;
        private RadioButton radbtn_F3_Model;
        private RadioButton radbtn_Frame_F3;
        private RadioButton radbtn_Frame_One;
        private RadioButton radbtn_Frame_S3;
        private RadioButton radbtn_Type_CS;
        private RadioButton radbtn_Type_DB;
        private RadioButton radbtn_Type_Web;
        private RadioButton radbtn_Web_Aspx;
        private RadioButton radbtn_Web_AspxCS;
        private string servername;
        private ModuleSettings setting;
        private TabControl tabControl1;
        private string tablename;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Thread thread;
        private ToolStripButton toolStripButton2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSplitButton toolStripSplitButton1;
        private TextBox txtClassName;
        private TextBox txtClassName2;
        private TextBox txtNameSpace;
        private TextBox txtNameSpace2;
        private TextBox txtProcPrefix;
        private TextBox txtProjectName;
        private TextBox txtTabname;
        private ToolStripMenuItem 列表ToolStripMenuItem;
        private ImageList imageList1;
        private ToolStripMenuItem 详细信息ToolStripMenuItem;

        public CodeMakerM(string Dbname)
        {
            this.InitializeComponent();
            this.dbname = Dbname;
            this.codeview = new UcCodeView();
            this.tabPage2.Controls.Add(this.codeview);
            this.SetListViewMenu("colum");
            this.CreatView();
            this.SetFormConfig();
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

        private void BindlistViewCol1(string Dbname, string TableName)
        {
            List<ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(Dbname, TableName);
            if ((columnInfoList != null) && (columnInfoList.Count > 0))
            {
                this.listView1.Items.Clear();
                this.cmbox_PField.Items.Clear();
                foreach (ColumnInfo info in columnInfoList)
                {
                    string colorder = info.Colorder;
                    string columnName = info.ColumnName;
                    string typeName = info.TypeName;
                    this.cmbox_PField.Items.Add(columnName);
                    ListViewItem item = new ListViewItem(colorder, 0);
                    item.Checked = true;
                    item.ImageIndex = -1;
                    item.SubItems.Add(columnName);
                    item.SubItems.Add(typeName);
                    this.listView1.Items.AddRange(new ListViewItem[] { item });
                }
                if (this.cmbox_PField.Items.Count > 0)
                {
                    this.cmbox_PField.SelectedIndex = 0;
                }
            }
            this.txtTabname.Text = TableName;
            this.txtClassName.Text = TableName;
        }

        private void BindlistViewCol2(string Dbname, string TableName)
        {
            List<ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(Dbname, TableName);
            if ((columnInfoList != null) && (columnInfoList.Count > 0))
            {
                this.listView2.Items.Clear();
                this.cmbox_SField.Items.Clear();
                foreach (ColumnInfo info in columnInfoList)
                {
                    string colorder = info.Colorder;
                    string columnName = info.ColumnName;
                    string typeName = info.TypeName;
                    this.cmbox_SField.Items.Add(columnName);
                    ListViewItem item = new ListViewItem(colorder, 0);
                    item.Checked = true;
                    item.ImageIndex = -1;
                    item.SubItems.Add(columnName);
                    item.SubItems.Add(typeName);
                    this.listView2.Items.AddRange(new ListViewItem[] { item });
                }
                if (this.cmbox_SField.Items.Count > 0)
                {
                    this.cmbox_SField.SelectedIndex = 0;
                }
            }
            this.txtTabname.Text = TableName;
            this.txtClassName2.Text = TableName;
        }

        private void BindTablist(string Dbname)
        {
            List<TableInfo> tablesInfo = this.dbobj.GetTablesInfo(Dbname);
            if ((tablesInfo != null) && (tablesInfo.Count > 0))
            {
                foreach (TableInfo info in tablesInfo)
                {
                    string tabName = info.TabName;
                    this.cmbox_PTab.Items.Add(tabName);
                    this.cmbox_STab.Items.Add(tabName);
                }
            }
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (this.listView1.CheckedItems.Count < 1)
            {
                MessageBox.Show("没有任何可以生成的项！", "请选择", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    if (this.radbtn_Type_DB.Checked)
                    {
                        this.CreatDB();
                    }
                    if (this.radbtn_Type_CS.Checked)
                    {
                        this.CreatCS();
                    }
                    if (this.radbtn_Type_Web.Checked)
                    {
                        this.CreatWeb();
                    }
                }
                catch
                {
                    MessageBox.Show("代码生成失败，请关闭后重新打开再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (this.radbtn_Frame_One.Checked)
                {
                    this.setting.AppFrame = "One";
                }
                if (this.radbtn_Frame_S3.Checked)
                {
                    this.setting.AppFrame = "S3";
                }
                if (this.radbtn_Frame_F3.Checked)
                {
                    this.setting.AppFrame = "F3";
                }
                this.setting.DALType = this.GetDALType();
                this.setting.BLLType = this.GetBLLType();
                this.setting.ProjectName = this.txtProjectName.Text;
                this.setting.Namepace = this.txtNameSpace.Text;
                this.setting.Folder = this.txtNameSpace2.Text;
                this.setting.ProcPrefix = this.txtProcPrefix.Text;
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

        private void btn_SelAll2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView2.Items)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                }
            }
        }

        private void btn_SelI_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void btn_SelI2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView2.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void cmbox_PTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.cmbox_PTab.SelectedItem != null) && (this.cmbox_PTab.Text != "System.Data.DataRowView"))
            {
                string text = this.cmbox_PTab.Text;
                this.BindlistViewCol1(this.dbname, text);
            }
        }

        private void cmbox_STab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.cmbox_STab.SelectedItem != null) && (this.cmbox_STab.Text != "System.Data.DataRowView"))
            {
                string text = this.cmbox_STab.Text;
                this.BindlistViewCol2(this.dbname, text);
            }
        }

        private void CodeMaker_Load(object sender, EventArgs e)
        {
        }

        private void CreatCS()
        {
            if (this.radbtn_Frame_One.Checked)
            {
                this.CreatCsOne();
            }
            if (this.radbtn_Frame_S3.Checked)
            {
                this.CreatCsS3();
            }
            if (this.radbtn_Frame_F3.Checked)
            {
                this.CreatCsF3();
            }
        }

        private void CreatCsF3()
        {
            if (this.radbtn_F3_Model.Checked)
            {
                this.CreatCsF3Model();
            }
            if (this.radbtn_F3_DAL.Checked)
            {
                this.CreatCsF3DAL();
            }
            if (this.radbtn_F3_IDAL.Checked)
            {
                this.CreatCsF3IDAL();
            }
            if (this.radbtn_F3_DALFactory.Checked)
            {
                this.CreatCsF3DALFactory();
            }
            if (this.radbtn_F3_BLL.Checked)
            {
                this.CreatCsF3BLL();
            }
        }

        private void CreatCsF3BLL()
        {
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlistP(), this.GetKeyFieldsP(), nameSpace, folder, this.setting.DbHelperName);
            string bLLType = this.GetBLLType();
            string strContent = ef.GetBLLCode(bLLType, false, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetModelByCache.Checked, this.chk_CS_GetList.Checked, this.chk_CS_GetList.Checked);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsF3DAL()
        {
            string text = this.txtProcPrefix.Text;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string tableNameParent = this.cmbox_PTab.Text;
            string tableNameSon = this.cmbox_STab.Text;
            string modelNameParent = this.txtClassName.Text;
            string modelNameSon = this.txtClassName2.Text;
            string modelName = this.txtClassName.Text;
            if (modelName == "")
            {
                modelName = this.tablename;
            }
            string modelSpaceParent = nameSpace + ".Model." + modelNameParent;
            string modelSpaceSon = nameSpace + ".Model." + modelNameSon;
            if (folder != "")
            {
                nameSpace = nameSpace + "." + folder;
                modelSpaceParent = nameSpace + ".Model." + folder + "." + modelNameParent;
                modelSpaceSon = nameSpace + ".Model." + folder + "." + modelNameSon;
            }
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlistP(), this.GetKeyFieldsP(), nameSpace, folder, this.setting.DbHelperName);
            string dALType = this.GetDALType();
            string strContent = ef.GetDALCodeTran(dALType, false, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, text, tableNameParent, tableNameSon, modelNameParent, modelNameSon, this.GetFieldlistP(), this.GetFieldlistS(), this.GetKeyFieldsP(), this.GetKeyFieldsS(), modelSpaceParent, modelSpaceSon);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsF3DALFactory()
        {
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
            string dALFactoryCode = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlistP(), this.GetKeyFieldsP(), nameSpace, folder, this.setting.DbHelperName).GetDALFactoryCode();
            this.SettxtContent("CS", dALFactoryCode);
        }

        private void CreatCsF3IDAL()
        {
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
            string strContent = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlistP(), this.GetKeyFieldsP(), nameSpace, folder, this.setting.DbHelperName).GetIDALCode(false, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, this.chk_CS_GetList.Checked);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsF3Model()
        {
            string text = this.txtProcPrefix.Text;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string tableNameParent = this.cmbox_PTab.Text;
            string tableNameSon = this.cmbox_STab.Text;
            string modelNameParent = this.txtClassName.Text;
            string modelNameSon = this.txtClassName2.Text;
            string strContent = new BuilderFrameF3(this.dbobj, this.dbname, nameSpace, folder, this.setting.DbHelperName).GetModelCode(tableNameParent, modelNameParent, this.GetFieldlistP(), tableNameSon, modelNameSon, this.GetFieldlistS());
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsOne()
        {
            string text = this.txtProcPrefix.Text;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            if (folder.Trim() != "")
            {
                nameSpace = nameSpace + "." + folder;
            }
            string modelName = this.txtClassName.Text;
            if (modelName == "")
            {
                modelName = this.tablename;
            }
            BuilderFrameOne one = new BuilderFrameOne(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlistP(), this.GetKeyFieldsP(), nameSpace, folder, this.setting.DbHelperName);
            string dALType = this.GetDALType();
            string strContent = one.GetCode(dALType, false, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, text);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsS3()
        {
            if (this.radbtn_F3_Model.Checked)
            {
                this.CreatCsS3Model();
            }
            if (this.radbtn_F3_DAL.Checked)
            {
                this.CreatCsS3DAL();
            }
            if (this.radbtn_F3_BLL.Checked)
            {
                this.CreatCsS3BLL();
            }
        }

        private void CreatCsS3BLL()
        {
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
            BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlistP(), this.GetKeyFieldsP(), nameSpace, folder, this.setting.DbHelperName);
            string bLLType = this.GetBLLType();
            string strContent = es.GetBLLCode(bLLType, false, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetModelByCache.Checked, this.chk_CS_GetList.Checked);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsS3DAL()
        {
            string text = this.txtProcPrefix.Text;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string tableNameParent = this.cmbox_PTab.Text;
            string tableNameSon = this.cmbox_STab.Text;
            string modelNameParent = this.txtClassName.Text;
            string modelNameSon = this.txtClassName2.Text;
            string modelName = this.txtClassName.Text;
            if (modelName == "")
            {
                modelName = this.tablename;
            }
            string modelSpaceParent = nameSpace + ".Model." + modelNameParent;
            string modelSpaceSon = nameSpace + ".Model." + modelNameSon;
            if (folder != "")
            {
                nameSpace = nameSpace + "." + folder;
                modelSpaceParent = nameSpace + ".Model." + folder + "." + modelNameParent;
                modelSpaceSon = nameSpace + ".Model." + folder + "." + modelNameSon;
            }
            BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlistP(), this.GetKeyFieldsP(), nameSpace, folder, this.setting.DbHelperName);
            string dALType = this.GetDALType();
            string strContent = es.GetDALCodeTran(dALType, false, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, text, tableNameParent, tableNameSon, modelNameParent, modelNameSon, this.GetFieldlistP(), this.GetFieldlistS(), this.GetKeyFieldsP(), this.GetKeyFieldsS(), modelSpaceParent, modelSpaceSon);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsS3Model()
        {
            string text = this.txtProcPrefix.Text;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string tableNameParent = this.cmbox_PTab.Text;
            string tableNameSon = this.cmbox_STab.Text;
            string modelNameParent = this.txtClassName.Text;
            string modelNameSon = this.txtClassName2.Text;
            string strContent = new BuilderFrameS3(this.dbobj, this.dbname, nameSpace, folder, this.setting.DbHelperName).GetModelCode(tableNameParent, modelNameParent, this.GetFieldlistP(), tableNameSon, modelNameSon, this.GetFieldlistS());
            this.SettxtContent("CS", strContent);
        }

        private void CreatDB()
        {
            if (this.radbtn_DB_Proc.Checked)
            {
                this.CreatDBProc();
            }
            else
            {
                this.CreatDBScript();
            }
        }

        private void CreatDBProc()
        {
            string text = this.txtProjectName.Text;
            string text1 = this.txtNameSpace.Text;
            string text2 = this.txtNameSpace2.Text;
            string text3 = this.txtClassName.Text;
            string str2 = this.txtProcPrefix.Text;
            string str3 = this.txtTabname.Text;
            bool maxid = this.chk_DB_GetMaxID.Checked;
            bool ishas = this.chk_DB_Exists.Checked;
            bool add = this.chk_DB_Add.Checked;
            bool update = this.chk_DB_Update.Checked;
            bool delete = this.chk_DB_Delete.Checked;
            bool getModel = this.chk_DB_GetModel.Checked;
            bool list = this.chk_DB_GetList.Checked;
            IDbScriptBuilder builder = ObjectHelper.CreatDsb(this.servername);
            builder.DbName = this.dbname;
            builder.TableName = str3;
            builder.ProjectName = text;
            builder.ProcPrefix = str2;
            builder.Keys = this.GetKeyFieldsP();
            builder.Fieldlist = this.GetFieldlistP();
            string strContent = builder.GetPROCCode(maxid, ishas, add, update, delete, getModel, list);
            this.SettxtContent("SQL", strContent);
        }

        private void CreatDBScript()
        {
            this.Cursor = Cursors.WaitCursor;
            IDbScriptBuilder builder = ObjectHelper.CreatDsb(this.servername);
            builder.Fieldlist = this.GetFieldlistP();
            string strContent = builder.CreateTabScript(this.dbname, this.tablename);
            this.SettxtContent("SQL", strContent);
            this.Cursor = Cursors.Default;
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
            this.listView1.Columns.Add("选", 0x1c, HorizontalAlignment.Left);
            this.listView1.Columns.Add("列名", 0x69, HorizontalAlignment.Left);
            this.listView1.Columns.Add("数据类型", 80, HorizontalAlignment.Left);
            this.listView2.Columns.Clear();
            this.listView2.Items.Clear();
            this.listView2.LargeImageList = this.imglistView;
            this.listView2.SmallImageList = this.imglistView;
            this.listView2.View = View.Details;
            this.listView2.GridLines = true;
            this.listView2.CheckBoxes = true;
            this.listView2.FullRowSelect = true;
            this.listView2.Columns.Add("选", 0x1c, HorizontalAlignment.Left);
            this.listView2.Columns.Add("列名", 0x69, HorizontalAlignment.Left);
            this.listView2.Columns.Add("数据类型", 80, HorizontalAlignment.Left);
        }

        private void CreatWeb()
        {
            string str = this.txtNameSpace.Text.Trim();
            string str2 = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = str;
            web.Fieldlist = this.GetFieldlistP();
            web.Keys = this.GetKeyFieldsP();
            web.ModelName = text;
            web.Folder = str2;
            if (this.radbtn_Web_Aspx.Checked)
            {
                string strContent = web.GetWebHtmlCode(this.chk_Web_HasKey.Checked, this.chk_Web_Add.Checked, this.chk_Web_Update.Checked, this.chk_Web_Show.Checked, true);
                this.SettxtContent("Aspx", strContent);
            }
            else
            {
                string str5 = web.GetWebCode(this.chk_Web_HasKey.Checked, this.chk_Web_Add.Checked, this.chk_Web_Update.Checked, this.chk_Web_Show.Checked, true);
                this.SettxtContent("CS", str5);
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

        private string GetBLLType()
        {
            string appGuid = "";
            appGuid = this.cm_blltype.AppGuid;
            if ((appGuid == "") && (appGuid == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return "";
            }
            return appGuid;
        }

        private string GetDALType()
        {
            string appGuid = "";
            appGuid = this.cm_daltype.AppGuid;
            if (!(appGuid == "") && !(appGuid == "System.Data.DataRowView"))
            {
                return appGuid;
            }
            MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return "";
        }

        private List<ColumnInfo> GetFieldlistP()
        {
            DataRow[] rowArray;
            string text = this.cmbox_PTab.Text;
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(this.dbobj.GetColumnInfoList(this.dbname, text));
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
            List<ColumnInfo> list2 = new List<ColumnInfo>();
            foreach (DataRow row in rowArray)
            {
                string str2 = row["Colorder"].ToString();
                string str3 = row["ColumnName"].ToString();
                string str4 = row["TypeName"].ToString();
                string str5 = row["IsIdentity"].ToString();
                string str6 = row["IsPK"].ToString();
                string str7 = row["Length"].ToString();
                string str8 = row["Preci"].ToString();
                string str9 = row["Scale"].ToString();
                string str10 = row["cisNull"].ToString();
                string str11 = row["DefaultVal"].ToString();
                string str12 = row["DeText"].ToString();
                ColumnInfo info = new ColumnInfo();
                info.Colorder = str2;
                info.ColumnName = str3;
                info.TypeName = str4;
                info.IsIdentity = str5 == "√";
                info.IsPK = str6 == "√";
                info.Length = str7;
                info.Preci = str8;
                info.Scale = str9;
                info.cisNull = str10 == "√";
                info.DefaultVal = str11;
                info.DeText = str12;
                list2.Add(info);
            }
            return list2;
        }

        private List<ColumnInfo> GetFieldlistS()
        {
            DataRow[] rowArray;
            string text = this.cmbox_STab.Text;
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(this.dbobj.GetColumnInfoList(this.dbname, text));
            StringPlus plus = new StringPlus();
            foreach (ListViewItem item in this.listView2.Items)
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
            List<ColumnInfo> list2 = new List<ColumnInfo>();
            foreach (DataRow row in rowArray)
            {
                string str2 = row["Colorder"].ToString();
                string str3 = row["ColumnName"].ToString();
                string str4 = row["TypeName"].ToString();
                string str5 = row["IsIdentity"].ToString();
                string str6 = row["IsPK"].ToString();
                string str7 = row["Length"].ToString();
                string str8 = row["Preci"].ToString();
                string str9 = row["Scale"].ToString();
                string str10 = row["cisNull"].ToString();
                string str11 = row["DefaultVal"].ToString();
                string str12 = row["DeText"].ToString();
                ColumnInfo info = new ColumnInfo();
                info.Colorder = str2;
                info.ColumnName = str3;
                info.TypeName = str4;
                info.IsIdentity = str5 == "√";
                info.IsPK = str6 == "√";
                info.Length = str7;
                info.Preci = str8;
                info.Scale = str9;
                info.cisNull = str10 == "√";
                info.DefaultVal = str11;
                info.DeText = str12;
                list2.Add(info);
            }
            return list2;
        }

        private List<ColumnInfo> GetKeyFieldsP()
        {
            DataRow[] rowArray;
            string text = this.cmbox_PTab.Text;
            List<ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(this.dbname, text);
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(columnInfoList);
            StringPlus plus = new StringPlus();
            plus.Append("'" + this.cmbox_PField.Text + "'");
            if ((columnInfoList == null) || (columnInfoList.Count <= 0))
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
            List<ColumnInfo> list2 = new List<ColumnInfo>();
            foreach (DataRow row in rowArray)
            {
                string str2 = row["Colorder"].ToString();
                string str3 = row["ColumnName"].ToString();
                string str4 = row["TypeName"].ToString();
                string str5 = row["IsIdentity"].ToString();
                string str6 = row["IsPK"].ToString();
                string str7 = row["Length"].ToString();
                string str8 = row["Preci"].ToString();
                string str9 = row["Scale"].ToString();
                string str10 = row["cisNull"].ToString();
                string str11 = row["DefaultVal"].ToString();
                string str12 = row["DeText"].ToString();
                ColumnInfo item = new ColumnInfo();
                item.Colorder = str2;
                item.ColumnName = str3;
                item.TypeName = str4;
                item.IsIdentity = str5 == "√";
                item.IsPK = str6 == "√";
                item.Length = str7;
                item.Preci = str8;
                item.Scale = str9;
                item.cisNull = str10 == "√";
                item.DefaultVal = str11;
                item.DeText = str12;
                list2.Add(item);
            }
            return list2;
        }

        private List<ColumnInfo> GetKeyFieldsS()
        {
            DataRow[] rowArray;
            string text = this.cmbox_STab.Text;
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(this.dbobj.GetColumnInfoList(this.dbname, text));
            StringPlus plus = new StringPlus();
            plus.Append("'" + this.cmbox_SField.Text + "'");
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
            List<ColumnInfo> list2 = new List<ColumnInfo>();
            foreach (DataRow row in rowArray)
            {
                string str2 = row["Colorder"].ToString();
                string str3 = row["ColumnName"].ToString();
                string str4 = row["TypeName"].ToString();
                string str5 = row["IsIdentity"].ToString();
                string str6 = row["IsPK"].ToString();
                string str7 = row["Length"].ToString();
                string str8 = row["Preci"].ToString();
                string str9 = row["Scale"].ToString();
                string str10 = row["cisNull"].ToString();
                string str11 = row["DefaultVal"].ToString();
                string str12 = row["DeText"].ToString();
                ColumnInfo item = new ColumnInfo();
                item.Colorder = str2;
                item.ColumnName = str3;
                item.TypeName = str4;
                item.IsIdentity = str5 == "√";
                item.IsPK = str6 == "√";
                item.Length = str7;
                item.Preci = str8;
                item.Scale = str9;
                item.cisNull = str10 == "√";
                item.DefaultVal = str11;
                item.DeText = str12;
                list2.Add(item);
            }
            return list2;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeMakerM));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "toolbtn_Connect.Image.png");
            // 
            // CodeMakerM
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "CodeMakerM";
            this.ResumeLayout(false);

        }

        private void radbtn_DBSel_Click(object sender, EventArgs e)
        {
            if (this.radbtn_DB_Proc.Checked)
            {
                this.chk_DB_GetMaxID.Visible = true;
                this.chk_DB_Exists.Visible = true;
                this.chk_DB_Add.Visible = true;
                this.chk_DB_Update.Visible = true;
                this.chk_DB_Delete.Visible = true;
                this.chk_DB_GetModel.Visible = true;
                this.chk_DB_GetList.Visible = true;
                this.txtProcPrefix.Visible = true;
                this.txtTabname.Visible = true;
                this.label5.Visible = true;
                this.label6.Visible = true;
                this.label7.Visible = true;
            }
            else
            {
                this.chk_DB_GetMaxID.Visible = false;
                this.chk_DB_Exists.Visible = false;
                this.chk_DB_Add.Visible = false;
                this.chk_DB_Update.Visible = false;
                this.chk_DB_Delete.Visible = false;
                this.chk_DB_GetModel.Visible = false;
                this.chk_DB_GetList.Visible = false;
                this.txtProcPrefix.Visible = false;
                this.txtTabname.Visible = false;
                this.label5.Visible = false;
                this.label6.Visible = false;
                this.label7.Visible = false;
            }
        }

        private void radbtn_F3_Click(object sender, EventArgs e)
        {
            if (this.radbtn_F3_Model.Checked)
            {
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_DAL.Checked)
            {
                this.groupBox_DALType.Visible = true;
                this.cm_daltype.Visible = true;
                this.cm_blltype.Visible = false;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_IDAL.Checked)
            {
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_DALFactory.Checked)
            {
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_AppType.Visible = true;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_BLL.Checked)
            {
                this.groupBox_DALType.Visible = true;
                this.cm_daltype.Visible = false;
                this.cm_blltype.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
        }

        private void radbtn_Frame_Click(object sender, EventArgs e)
        {
            if (this.radbtn_Frame_One.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_F3.Visible = false;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_Frame_S3.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_F3.Visible = true;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
                this.radbtn_F3_IDAL.Visible = false;
                this.radbtn_F3_DALFactory.Visible = false;
                this.radbtn_F3_Click(sender, e);
            }
            if (this.radbtn_Frame_F3.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_F3.Visible = true;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = true;
                this.groupBox_Web.Visible = false;
                this.radbtn_F3_IDAL.Visible = true;
                this.radbtn_F3_DALFactory.Visible = true;
                this.radbtn_F3_Click(sender, e);
            }
        }

        private void radbtn_Type_Click(object sender, EventArgs e)
        {
            if (this.radbtn_Type_DB.Checked)
            {
                this.groupBox_DB.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_FrameSel.Visible = false;
                this.groupBox_F3.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_Type_CS.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_AppType.Visible = true;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_FrameSel.Visible = true;
                this.groupBox_F3.Visible = true;
                this.groupBox_Web.Visible = false;
                this.radbtn_Frame_Click(sender, e);
            }
            if (this.radbtn_Type_Web.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_AppType.Visible = false;
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_FrameSel.Visible = false;
                this.groupBox_F3.Visible = false;
                this.groupBox_Web.Visible = true;
            }
        }

        private void SetFormConfig()
        {
            this.radbtn_Type_Click(null, null);
            //this.setting = ModuleConfig.GetSettings();
            this.txtProjectName.Text = this.setting.ProjectName;
            this.txtNameSpace.Text = this.setting.Namepace;
            this.txtNameSpace2.Text = this.setting.Folder;
            this.txtProcPrefix.Text = this.setting.ProcPrefix;
            string appFrame = this.setting.AppFrame;
            if (appFrame != null)
            {
                if (!(appFrame == "One"))
                {
                    if (appFrame == "S3")
                    {
                        this.radbtn_Frame_S3.Checked = true;
                    }
                    else if (appFrame == "F3")
                    {
                        this.radbtn_Frame_F3.Checked = true;
                    }
                }
                else
                {
                    this.radbtn_Frame_One.Checked = true;
                }
            }
            this.radbtn_Type_Click(null, null);
            this.radbtn_Frame_Click(null, null);
            this.radbtn_F3_Click(null, null);
            this.cm_daltype = new DALTypeAddIn("LTP.IBuilder.IBuilderDALTran");
            this.cm_daltype.Title = "DAL";
            this.groupBox_DALType.Controls.Add(this.cm_daltype);
            this.cm_daltype.Location = new Point(30, 0x12);
            this.cm_daltype.SetSelectedDALType(this.setting.DALType.Trim());
            this.cm_blltype = new DALTypeAddIn("LTP.IBuilder.IBuilderBLL");
            this.cm_blltype.Title = "BLL";
            this.groupBox_DALType.Controls.Add(this.cm_blltype);
            this.cm_blltype.Location = new Point(30, 0x12);
            this.cm_blltype.SetSelectedDALType(this.setting.BLLType.Trim());
            this.tabControl1.SelectedIndex = 0;
        }

        public void SetListView(DbView dbviewfrm)
        {
            TreeNode selectedNode = dbviewfrm.treeView1.SelectedNode;
            if (selectedNode != null)
            {
                string str = selectedNode.Tag.ToString();
                if (str == null)
                {
                    goto Label_01AE;
                }
                if (!(str == "table"))
                {
                    if (str == "view")
                    {
                        this.servername = selectedNode.Parent.Parent.Parent.Text;
                        this.dbname = selectedNode.Parent.Parent.Text;
                        this.tablename = selectedNode.Text;
                        this.dbobj = ObjectHelper.CreatDbObj(this.servername);
                        this.BindTablist(this.dbname);
                        return;
                    }
                    if (str == "column")
                    {
                        this.servername = selectedNode.Parent.Parent.Parent.Parent.Text;
                        this.dbname = selectedNode.Parent.Parent.Parent.Text;
                        this.dbobj = ObjectHelper.CreatDbObj(this.servername);
                        this.BindTablist(this.dbname);
                        return;
                    }
                    if (str == "db")
                    {
                        this.servername = selectedNode.Parent.Text;
                        this.dbname = selectedNode.Text;
                        this.dbobj = ObjectHelper.CreatDbObj(this.servername);
                        this.BindTablist(this.dbname);
                        return;
                    }
                    goto Label_01AE;
                }
                this.servername = selectedNode.Parent.Parent.Parent.Text;
                this.dbname = selectedNode.Parent.Parent.Text;
                this.tablename = selectedNode.Text;
                this.dbobj = ObjectHelper.CreatDbObj(this.servername);
                this.BindTablist(this.dbname);
            }
            return;
        Label_01AE:
            this.listView1.Items.Clear();
            this.listView2.Items.Clear();
        }

        private void SetListViewMenu(string itemType)
        {
            string str;
            if ((((str = itemType.ToLower()) != null) && !(str == "server")) && (((str == "db") || (str == "table")) || !(str == "column")))
            {
            }
        }

        private void SettxtContent(string Type, string strContent)
        {
            this.codeview.SettxtContent(Type, strContent);
            this.tabControl1.SelectedIndex = 2;
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

        private delegate void SetListCallback();
    }
}


using Flextronics.Applications.ApplicationFactory.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Flextronics.Applications.ApplicationFactory;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.Schema;
using Flextronics.Applications.Library.CodeBuilder;
using System.Collections;

namespace Flextronics.Applications.ApplicationFactory
{
    public class FrmCodeMaker : Form // KryptonForm
    {
        public string dbname;
        public string tablename;
        private string servername;

        private IDbObject dbobj;

        private DALTypeAddIn cm_blltype;
        private DALTypeAddIn cm_daltype;
        //public UcCodeView codeview;

        private bool WCFSupport;

        public CodeEditor code;

        private TabPage tabPage1;
        private TabPage tabPage2;
        private MainForm mainfrm;

        #region 
        private Button btn_Ok;
        private Button btn_SelAll;
        private Button btn_SelClear;
        private Button btn_SelI;
        private Button btn_SetKey;
        private CheckBox chk_CS_Add;
        private CheckBox chk_CS_Delete;
        private CheckBox chk_CS_Exists;
        private CheckBox chk_CS_GetList;
        private CheckBox chk_CS_GetMaxID;
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
      
        private IContainer components;
       
        private GroupBox groupBox_AppType;
        private GroupBox groupBox_DALType;
        private GroupBox groupBox_DB;
        private GroupBox groupBox_F3;
        private GroupBox groupBox_FrameSel;
        private GroupBox groupBox_Method;
        private GroupBox groupBox_Parameter;
        private GroupBox groupBox_Select;
        private GroupBox groupBox_Type;
        private GroupBox groupBox_Web;
        private ImageList imglistDB;
        private ImageList imgListTabpage;
        private ImageList imglistView;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label lblkeycount;
        private ListBox list_KeyField;
        private ListView listFields;
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
        private Splitter splitter1;
        private TabControl tabControl1;
  
        private Thread thread;
        private ToolStripButton toolStripButton2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSplitButton toolStripSplitButton1;
        private TextBox txtClassName;
        private TextBox txtNameSpace;
        private TextBox txtNameSpace2;
        private TextBox txtProcPrefix;
        private TextBox txtProjectName;
        private TextBox txtTabname;
        private ToolStripMenuItem 列表ToolStripMenuItem;
        private ToolStripMenuItem 详细信息ToolStripMenuItem;
        #endregion

        public FrmCodeMaker(Form mdiParentForm)
        {
            this.InitializeComponent();

            this.list_KeyField.Height = 32;
            //codeview = new UcCodeView();
            //codeview.Dock = DockStyle.Fill;
            //tabPage2.Controls.Add(codeview);

            code = new CodeEditor();
            code.Dock = DockStyle.Fill;
            tabPage2.Controls.Add(code);

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
            this.mainfrm = (MainForm)mdiParentForm;
            WCFSupport = mainfrm.wCFSupportToolStripMenuItem.Checked;
        }

        //显示数据库结构
        public void BindlistViewCol(string Dbname, string TableName)
        {
            this.chk_CS_GetMaxID.Checked = true;
            List<ColumnInfo> columnInfoList = dbobj.GetColumnInfoList(Dbname, TableName);
            if ((columnInfoList != null) && (columnInfoList.Count > 0))
            {
                this.listFields.Items.Clear();
                this.list_KeyField.Items.Clear();
                this.chk_CS_GetMaxID.Enabled = true;
                foreach (ColumnInfo info in columnInfoList)
                {
                    string colorder = info.Colorder;
                    string columnName = info.ColumnName;
                    string typeName = info.TypeName;
                    string length = info.Length;
                    if (typeName == "nvarchar")
                    {
                        length = (int.Parse(length) / 2).ToString();
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
                    if ((str8 == "√") && (str9.Trim() == ""))
                    {
                        this.list_KeyField.Items.Add(columnName);
                        if (text == "√")
                        {
                            this.chk_CS_GetMaxID.Checked = false;
                            this.chk_CS_GetMaxID.Enabled = false;
                            this.chk_DB_GetMaxID.Checked = false;
                            this.chk_DB_GetMaxID.Enabled = false;
                        }
                    }
                    else
                    {
                        str8 = "";
                        if (text == "√")
                        {
                            this.list_KeyField.Items.Add(columnName);
                            this.chk_CS_GetMaxID.Checked = false;
                            this.chk_CS_GetMaxID.Enabled = false;
                            this.chk_DB_GetMaxID.Checked = false;
                            this.chk_DB_GetMaxID.Enabled = false;
                        }
                    }
                    item.SubItems.Add(str8);
                    item.SubItems.Add(str9);
                    item.SubItems.Add(defaultVal);
                    this.listFields.Items.AddRange(new ListViewItem[] { item });
                }
            }
            this.btn_SelAll_Click(null, null);
            this.txtTabname.Text = TableName;
            this.txtClassName.Text = TableName;
            this.lblkeycount.Text = this.list_KeyField.Items.Count.ToString() + "个主键";
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            bool WCF = mainfrm.wCFSupportToolStripMenuItem.Checked;
            if (this.listFields.CheckedItems.Count < 1)
            {
                MessageBox.Show("没有任何可以生成的项！", "请选择", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if ((this.list_KeyField.Items.Count != 0) || (MessageBox.Show("没有主键字段和条件字段，你确认要继续生成？", "主键提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No))
            {
                if (this.radbtn_Type_DB.Checked)
                {
                    CreatDB();
                }
                if (this.radbtn_Type_CS.Checked)
                {
                    CreatCS();
                }
                if (this.radbtn_Type_Web.Checked)
                {
                    CreatWeb();
                }
            }
        }

        private void btn_SelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listFields.Items)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                }
            }
        }

        private void btn_SelClear_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listFields.Items)
            {
                item.Checked = false;
            }
        }

        private void btn_SelI_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listFields.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void btn_SetKey_Click(object sender, EventArgs e)
        {
            this.list_KeyField.Items.Clear();
            foreach (ListViewItem item in this.listFields.Items)
            {
                if (item.Checked)
                {
                    this.list_KeyField.Items.Add(item.SubItems[1].Text);
                }
            }
            this.lblkeycount.Text = this.list_KeyField.Items.Count.ToString() + "个主键";
        }
    
        #region  代码生成入口

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
            web.Fieldlist = this.GetFieldlist();
            web.Keys = this.GetKeyFields();
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

        #endregion 

        #region  数据库代码

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
            builder.Keys = this.GetKeyFields();
            builder.Fieldlist = this.GetFieldlist();
            string strContent = builder.GetPROCCode(maxid, ishas, add, update, delete, getModel, list);
            this.SettxtContent("SQL", strContent);
        }

        private void CreatDBScript()
        {
            this.Cursor = Cursors.WaitCursor;
            IDbScriptBuilder builder = ObjectHelper.CreatDsb(this.servername);
            builder.Fieldlist = this.GetFieldlist();
            string strContent = builder.CreateTabScript(this.dbname, this.tablename);
            this.SettxtContent("SQL", strContent);
            this.Cursor = Cursors.Default;
        }

        #endregion 

        #region 应用程序类代码

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
            //BuilderFrameOne one = new BuilderFrameOne(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName);
            BuilderFrameOne one = new BuilderFrameOne(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName");

            string dALType = this.GetDALType();
            string strContent = one.GetCode(dALType, this.chk_CS_GetMaxID.Checked, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, text);
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

        #endregion 

        private void CreatCsF3BLL()
        {
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
            //BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName);
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName");

            string bLLType = this.GetBLLType();
            string strContent = ef.GetBLLCode(bLLType, this.chk_CS_GetMaxID.Checked, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetModelByCache.Checked, this.chk_CS_GetList.Checked, this.chk_CS_GetList.Checked);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsF3DAL()
        {
            string text = this.txtProcPrefix.Text;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string modelName = this.txtClassName.Text;
            if (modelName == "")
            {
                modelName = this.tablename;
            }
           // BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName);
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName");

            string dALType = this.GetDALType();
            string strContent = ef.GetDALCode(dALType, this.chk_CS_GetMaxID.Checked, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, text);
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
           // string dALFactoryCode = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName).GetDALFactoryCode();
            string dALFactoryCode = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName").GetDALFactoryCode();

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
           // string strContent = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName).GetIDALCode(this.chk_CS_GetMaxID.Checked, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, this.chk_CS_GetList.Checked);
            string strContent = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName").GetIDALCode(this.chk_CS_GetMaxID.Checked, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, this.chk_CS_GetList.Checked);

            this.SettxtContent("CS", strContent);
        }

        private void CreatCsF3Model()
        {
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
           // string modelCode = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName).GetModelCode();
            string modelCode = new BuilderFrameF3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName").GetModelCode();

            this.SettxtContent("CS", modelCode);
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
            //BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName);
            BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName",WCFSupport);

            string bLLType = this.GetBLLType();
            string strContent = es.GetBLLCode(bLLType, this.chk_CS_GetMaxID.Checked, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetModelByCache.Checked, this.chk_CS_GetList.Checked);
            this.SettxtContent("CS", strContent);
        }

        private void CreatCsS3DAL()
        {
            string text = this.txtProcPrefix.Text;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string modelName = this.txtClassName.Text;
            if (modelName == "")
            {
                modelName = this.tablename;
            }
            modelName = modelName + "DAL";
           // BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName);
            BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, modelName, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName",WCFSupport);

            string dALType = this.GetDALType();
            string strContent = es.GetDALCode(dALType, this.chk_CS_GetMaxID.Checked, this.chk_CS_Exists.Checked, this.chk_CS_Add.Checked, this.chk_CS_Update.Checked, this.chk_CS_Delete.Checked, this.chk_CS_GetModel.Checked, this.chk_CS_GetList.Checked, text);
            this.SettxtContent("CS", strContent);
        }
        //生成三层的实体类代码
        private void CreatCsS3Model()
        {
            bool WCF = mainfrm.wCFSupportToolStripMenuItem.Checked;
            string nameSpace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string text = this.txtClassName.Text;
            if (text == "")
            {
                text = this.tablename;
            }
            text = text + "Entity";
           // string modelCode = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, this.setting.DbHelperName).GetModelCode();
            BuilderFrameS3 cs = new BuilderFrameS3(this.dbobj, this.dbname, this.tablename, text, this.GetFieldlist(), this.GetKeyFields(), nameSpace, folder, "DbHelperName", WCF);
            string modelCode=cs.GetModelCode();
            //end
            this.SettxtContent("CS", modelCode);
        }
      
       private void CreatView()
        {
            this.listFields.Columns.Clear();
            this.listFields.Items.Clear();
            this.listFields.LargeImageList = this.imglistView;
            this.listFields.SmallImageList = this.imglistView;
            this.listFields.View = View.Details;
            this.listFields.GridLines = true;
            this.listFields.CheckBoxes = true;
            this.listFields.FullRowSelect = true;
            this.listFields.Columns.Add("序号", 60, HorizontalAlignment.Center);
            this.listFields.Columns.Add("列名", 110, HorizontalAlignment.Left);
            this.listFields.Columns.Add("数据类型", 80, HorizontalAlignment.Left);
            this.listFields.Columns.Add("长度", 40, HorizontalAlignment.Left);
            this.listFields.Columns.Add("小数", 40, HorizontalAlignment.Left);
            this.listFields.Columns.Add("标识", 40, HorizontalAlignment.Center);
            this.listFields.Columns.Add("主键", 40, HorizontalAlignment.Center);
            this.listFields.Columns.Add("允许空", 60, HorizontalAlignment.Center);
            this.listFields.Columns.Add("默认值", 100, HorizontalAlignment.Left);
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
            if ((appGuid == "") && (appGuid == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return "";
            }
            return appGuid;
        }

        private List<ColumnInfo> GetFieldlist()
        {
            DataRow[] rowArray;
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(this.dbobj.GetColumnInfoList(this.dbname, this.tablename));
            StringPlus plus = new StringPlus();
            foreach (ListViewItem item in this.listFields.Items)
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
                rowArray = columnInfoDt.Select("ColumnName in (" + plus.Value + ")", "   colorder ASC");
            }
            else
            {
                rowArray = columnInfoDt.Select();
            }
            //ANDY 需要重新创建一个表，来排列顺序
            //只 需要保证 colorder为整数类型，不是字符串，上面的代码起作用
            //DataTable tblTemp = columnInfoDt.Clone();
            //foreach (DataRow drow in rowArray)
            //    tblTemp.ImportRow(drow);
            //DataView dv = new DataView(tblTemp);
            //dv.Sort = "    colorder ASC ";
            //DataTable table = dv.Table.Clone();
            //IEnumerator rators = dv.GetEnumerator();
            //while (rators.MoveNext())
            //{
            //    object obj = rators.Current;
            //    table.ImportRow(((DataRowView)obj).Row);
            //}     
            //end
            List<ColumnInfo> list2 = new List<ColumnInfo>();
            foreach (DataRow row in  rowArray)
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
                ColumnInfo info = new ColumnInfo();
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

        private List<ColumnInfo> GetKeyFields()
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
            List<ColumnInfo> list2 = new List<ColumnInfo>();
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
                ColumnInfo item = new ColumnInfo();
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.groupBox_Web = new System.Windows.Forms.GroupBox();
            this.chk_Web_Show = new System.Windows.Forms.CheckBox();
            this.chk_Web_Update = new System.Windows.Forms.CheckBox();
            this.chk_Web_HasKey = new System.Windows.Forms.CheckBox();
            this.chk_Web_Add = new System.Windows.Forms.CheckBox();
            this.radbtn_Web_AspxCS = new System.Windows.Forms.RadioButton();
            this.radbtn_Web_Aspx = new System.Windows.Forms.RadioButton();
            this.groupBox_AppType = new System.Windows.Forms.GroupBox();
            this.radbtn_AppType_Winform = new System.Windows.Forms.RadioButton();
            this.radbtn_AppType_Web = new System.Windows.Forms.RadioButton();
            this.groupBox_Method = new System.Windows.Forms.GroupBox();
            this.chk_CS_GetList = new System.Windows.Forms.CheckBox();
            this.chk_CS_GetModelByCache = new System.Windows.Forms.CheckBox();
            this.chk_CS_GetModel = new System.Windows.Forms.CheckBox();
            this.chk_CS_Delete = new System.Windows.Forms.CheckBox();
            this.chk_CS_Update = new System.Windows.Forms.CheckBox();
            this.chk_CS_Add = new System.Windows.Forms.CheckBox();
            this.chk_CS_Exists = new System.Windows.Forms.CheckBox();
            this.chk_CS_GetMaxID = new System.Windows.Forms.CheckBox();
            this.groupBox_DALType = new System.Windows.Forms.GroupBox();
            this.groupBox_F3 = new System.Windows.Forms.GroupBox();
            this.radbtn_F3_BLL = new System.Windows.Forms.RadioButton();
            this.radbtn_F3_DALFactory = new System.Windows.Forms.RadioButton();
            this.radbtn_F3_IDAL = new System.Windows.Forms.RadioButton();
            this.radbtn_F3_DAL = new System.Windows.Forms.RadioButton();
            this.radbtn_F3_Model = new System.Windows.Forms.RadioButton();
            this.groupBox_FrameSel = new System.Windows.Forms.GroupBox();
            this.radbtn_Frame_F3 = new System.Windows.Forms.RadioButton();
            this.radbtn_Frame_S3 = new System.Windows.Forms.RadioButton();
            this.radbtn_Frame_One = new System.Windows.Forms.RadioButton();
            this.groupBox_DB = new System.Windows.Forms.GroupBox();
            this.chk_DB_GetList = new System.Windows.Forms.CheckBox();
            this.chk_DB_GetModel = new System.Windows.Forms.CheckBox();
            this.chk_DB_Delete = new System.Windows.Forms.CheckBox();
            this.chk_DB_Update = new System.Windows.Forms.CheckBox();
            this.chk_DB_Add = new System.Windows.Forms.CheckBox();
            this.chk_DB_Exists = new System.Windows.Forms.CheckBox();
            this.chk_DB_GetMaxID = new System.Windows.Forms.CheckBox();
            this.txtTabname = new System.Windows.Forms.TextBox();
            this.txtProcPrefix = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.radbtn_DB_DDL = new System.Windows.Forms.RadioButton();
            this.radbtn_DB_Proc = new System.Windows.Forms.RadioButton();
            this.groupBox_Type = new System.Windows.Forms.GroupBox();
            this.radbtn_Type_Web = new System.Windows.Forms.RadioButton();
            this.radbtn_Type_CS = new System.Windows.Forms.RadioButton();
            this.radbtn_Type_DB = new System.Windows.Forms.RadioButton();
            this.groupBox_Parameter = new System.Windows.Forms.GroupBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.txtNameSpace2 = new System.Windows.Forms.TextBox();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_Select = new System.Windows.Forms.GroupBox();
            this.btn_SetKey = new System.Windows.Forms.Button();
            this.list_KeyField = new System.Windows.Forms.ListBox();
            this.btn_SelClear = new System.Windows.Forms.Button();
            this.btn_SelI = new System.Windows.Forms.Button();
            this.btn_SelAll = new System.Windows.Forms.Button();
            this.lblkeycount = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listFields = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imgListTabpage = new System.Windows.Forms.ImageList(this.components);
            this.imglistDB = new System.Windows.Forms.ImageList(this.components);
            this.imglistView = new System.Windows.Forms.ImageList(this.components);
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.详细信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox_Web.SuspendLayout();
            this.groupBox_AppType.SuspendLayout();
            this.groupBox_Method.SuspendLayout();
            this.groupBox_F3.SuspendLayout();
            this.groupBox_FrameSel.SuspendLayout();
            this.groupBox_DB.SuspendLayout();
            this.groupBox_Type.SuspendLayout();
            this.groupBox_Parameter.SuspendLayout();
            this.groupBox_Select.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imgListTabpage;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 641);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(616, 614);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Design";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(211)))), ((int)(((byte)(241)))));
            this.panel2.Controls.Add(this.btn_Ok);
            this.panel2.Controls.Add(this.groupBox_Web);
            this.panel2.Controls.Add(this.groupBox_AppType);
            this.panel2.Controls.Add(this.groupBox_Method);
            this.panel2.Controls.Add(this.groupBox_DALType);
            this.panel2.Controls.Add(this.groupBox_F3);
            this.panel2.Controls.Add(this.groupBox_FrameSel);
            this.panel2.Controls.Add(this.groupBox_DB);
            this.panel2.Controls.Add(this.groupBox_Type);
            this.panel2.Controls.Add(this.groupBox_Parameter);
            this.panel2.Controls.Add(this.groupBox_Select);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(610, 555);
            this.panel2.TabIndex = 2;
            // 
            // btn_Ok
            // 
            this.btn_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Ok.Location = new System.Drawing.Point(514, 495);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(75, 23);
            this.btn_Ok.TabIndex = 7;
            this.btn_Ok.Text = "Execute";
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // groupBox_Web
            // 
            this.groupBox_Web.Controls.Add(this.chk_Web_Show);
            this.groupBox_Web.Controls.Add(this.chk_Web_Update);
            this.groupBox_Web.Controls.Add(this.chk_Web_HasKey);
            this.groupBox_Web.Controls.Add(this.chk_Web_Add);
            this.groupBox_Web.Controls.Add(this.radbtn_Web_AspxCS);
            this.groupBox_Web.Controls.Add(this.radbtn_Web_Aspx);
            this.groupBox_Web.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Web.Location = new System.Drawing.Point(0, 478);
            this.groupBox_Web.Name = "groupBox_Web";
            this.groupBox_Web.Size = new System.Drawing.Size(610, 67);
            this.groupBox_Web.TabIndex = 6;
            this.groupBox_Web.TabStop = false;
            this.groupBox_Web.Text = "Web页面";
            // 
            // chk_Web_Show
            // 
            this.chk_Web_Show.AutoSize = true;
            this.chk_Web_Show.Checked = true;
            this.chk_Web_Show.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Web_Show.Location = new System.Drawing.Point(292, 45);
            this.chk_Web_Show.Name = "chk_Web_Show";
            this.chk_Web_Show.Size = new System.Drawing.Size(62, 17);
            this.chk_Web_Show.TabIndex = 3;
            this.chk_Web_Show.Text = "显示页";
            this.chk_Web_Show.UseVisualStyleBackColor = true;
            // 
            // chk_Web_Update
            // 
            this.chk_Web_Update.AutoSize = true;
            this.chk_Web_Update.Checked = true;
            this.chk_Web_Update.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Web_Update.Location = new System.Drawing.Point(217, 45);
            this.chk_Web_Update.Name = "chk_Web_Update";
            this.chk_Web_Update.Size = new System.Drawing.Size(62, 17);
            this.chk_Web_Update.TabIndex = 3;
            this.chk_Web_Update.Text = "修改页";
            this.chk_Web_Update.UseVisualStyleBackColor = true;
            // 
            // chk_Web_HasKey
            // 
            this.chk_Web_HasKey.AutoSize = true;
            this.chk_Web_HasKey.Checked = true;
            this.chk_Web_HasKey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Web_HasKey.Location = new System.Drawing.Point(55, 45);
            this.chk_Web_HasKey.Name = "chk_Web_HasKey";
            this.chk_Web_HasKey.Size = new System.Drawing.Size(74, 17);
            this.chk_Web_HasKey.TabIndex = 3;
            this.chk_Web_HasKey.Text = "包括主键";
            this.chk_Web_HasKey.UseVisualStyleBackColor = true;
            // 
            // chk_Web_Add
            // 
            this.chk_Web_Add.AutoSize = true;
            this.chk_Web_Add.Checked = true;
            this.chk_Web_Add.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Web_Add.Location = new System.Drawing.Point(142, 45);
            this.chk_Web_Add.Name = "chk_Web_Add";
            this.chk_Web_Add.Size = new System.Drawing.Size(62, 17);
            this.chk_Web_Add.TabIndex = 3;
            this.chk_Web_Add.Text = "增加页";
            this.chk_Web_Add.UseVisualStyleBackColor = true;
            // 
            // radbtn_Web_AspxCS
            // 
            this.radbtn_Web_AspxCS.AutoSize = true;
            this.radbtn_Web_AspxCS.Location = new System.Drawing.Point(158, 20);
            this.radbtn_Web_AspxCS.Name = "radbtn_Web_AspxCS";
            this.radbtn_Web_AspxCS.Size = new System.Drawing.Size(76, 17);
            this.radbtn_Web_AspxCS.TabIndex = 2;
            this.radbtn_Web_AspxCS.Text = "Web Code";
            this.radbtn_Web_AspxCS.UseVisualStyleBackColor = true;
            // 
            // radbtn_Web_Aspx
            // 
            this.radbtn_Web_Aspx.AutoSize = true;
            this.radbtn_Web_Aspx.Checked = true;
            this.radbtn_Web_Aspx.Location = new System.Drawing.Point(56, 20);
            this.radbtn_Web_Aspx.Name = "radbtn_Web_Aspx";
            this.radbtn_Web_Aspx.Size = new System.Drawing.Size(76, 17);
            this.radbtn_Web_Aspx.TabIndex = 1;
            this.radbtn_Web_Aspx.TabStop = true;
            this.radbtn_Web_Aspx.Text = "Web Page";
            this.radbtn_Web_Aspx.UseVisualStyleBackColor = true;
            // 
            // groupBox_AppType
            // 
            this.groupBox_AppType.Controls.Add(this.radbtn_AppType_Winform);
            this.groupBox_AppType.Controls.Add(this.radbtn_AppType_Web);
            this.groupBox_AppType.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_AppType.Location = new System.Drawing.Point(0, 438);
            this.groupBox_AppType.Name = "groupBox_AppType";
            this.groupBox_AppType.Size = new System.Drawing.Size(610, 40);
            this.groupBox_AppType.TabIndex = 6;
            this.groupBox_AppType.TabStop = false;
            this.groupBox_AppType.Text = "应用系统类型";
            // 
            // radbtn_AppType_Winform
            // 
            this.radbtn_AppType_Winform.AutoSize = true;
            this.radbtn_AppType_Winform.Location = new System.Drawing.Point(186, 17);
            this.radbtn_AppType_Winform.Name = "radbtn_AppType_Winform";
            this.radbtn_AppType_Winform.Size = new System.Drawing.Size(91, 17);
            this.radbtn_AppType_Winform.TabIndex = 2;
            this.radbtn_AppType_Winform.Text = "WinForm系统";
            this.radbtn_AppType_Winform.UseVisualStyleBackColor = true;
            // 
            // radbtn_AppType_Web
            // 
            this.radbtn_AppType_Web.AutoSize = true;
            this.radbtn_AppType_Web.Checked = true;
            this.radbtn_AppType_Web.Location = new System.Drawing.Point(84, 17);
            this.radbtn_AppType_Web.Name = "radbtn_AppType_Web";
            this.radbtn_AppType_Web.Size = new System.Drawing.Size(72, 17);
            this.radbtn_AppType_Web.TabIndex = 1;
            this.radbtn_AppType_Web.TabStop = true;
            this.radbtn_AppType_Web.Text = "Web系统";
            this.radbtn_AppType_Web.UseVisualStyleBackColor = true;
            // 
            // groupBox_Method
            // 
            this.groupBox_Method.Controls.Add(this.chk_CS_GetList);
            this.groupBox_Method.Controls.Add(this.chk_CS_GetModelByCache);
            this.groupBox_Method.Controls.Add(this.chk_CS_GetModel);
            this.groupBox_Method.Controls.Add(this.chk_CS_Delete);
            this.groupBox_Method.Controls.Add(this.chk_CS_Update);
            this.groupBox_Method.Controls.Add(this.chk_CS_Add);
            this.groupBox_Method.Controls.Add(this.chk_CS_Exists);
            this.groupBox_Method.Controls.Add(this.chk_CS_GetMaxID);
            this.groupBox_Method.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Method.Location = new System.Drawing.Point(0, 396);
            this.groupBox_Method.Name = "groupBox_Method";
            this.groupBox_Method.Size = new System.Drawing.Size(610, 42);
            this.groupBox_Method.TabIndex = 5;
            this.groupBox_Method.TabStop = false;
            this.groupBox_Method.Text = "方法选择";
            // 
            // chk_CS_GetList
            // 
            this.chk_CS_GetList.AutoSize = true;
            this.chk_CS_GetList.Checked = true;
            this.chk_CS_GetList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_GetList.Location = new System.Drawing.Point(534, 18);
            this.chk_CS_GetList.Name = "chk_CS_GetList";
            this.chk_CS_GetList.Size = new System.Drawing.Size(59, 17);
            this.chk_CS_GetList.TabIndex = 8;
            this.chk_CS_GetList.Text = "GetList";
            this.chk_CS_GetList.UseVisualStyleBackColor = true;
            // 
            // chk_CS_GetModelByCache
            // 
            this.chk_CS_GetModelByCache.AutoSize = true;
            this.chk_CS_GetModelByCache.Checked = true;
            this.chk_CS_GetModelByCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_GetModelByCache.Location = new System.Drawing.Point(420, 18);
            this.chk_CS_GetModelByCache.Name = "chk_CS_GetModelByCache";
            this.chk_CS_GetModelByCache.Size = new System.Drawing.Size(115, 17);
            this.chk_CS_GetModelByCache.TabIndex = 9;
            this.chk_CS_GetModelByCache.Text = "GetModelByCache";
            this.chk_CS_GetModelByCache.UseVisualStyleBackColor = true;
            // 
            // chk_CS_GetModel
            // 
            this.chk_CS_GetModel.AutoSize = true;
            this.chk_CS_GetModel.Checked = true;
            this.chk_CS_GetModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_GetModel.Location = new System.Drawing.Point(348, 18);
            this.chk_CS_GetModel.Name = "chk_CS_GetModel";
            this.chk_CS_GetModel.Size = new System.Drawing.Size(72, 17);
            this.chk_CS_GetModel.TabIndex = 9;
            this.chk_CS_GetModel.Text = "GetModel";
            this.chk_CS_GetModel.UseVisualStyleBackColor = true;
            // 
            // chk_CS_Delete
            // 
            this.chk_CS_Delete.AutoSize = true;
            this.chk_CS_Delete.Checked = true;
            this.chk_CS_Delete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_Delete.Location = new System.Drawing.Point(288, 18);
            this.chk_CS_Delete.Name = "chk_CS_Delete";
            this.chk_CS_Delete.Size = new System.Drawing.Size(57, 17);
            this.chk_CS_Delete.TabIndex = 10;
            this.chk_CS_Delete.Text = "Delete";
            this.chk_CS_Delete.UseVisualStyleBackColor = true;
            // 
            // chk_CS_Update
            // 
            this.chk_CS_Update.AutoSize = true;
            this.chk_CS_Update.Checked = true;
            this.chk_CS_Update.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_Update.Location = new System.Drawing.Point(228, 18);
            this.chk_CS_Update.Name = "chk_CS_Update";
            this.chk_CS_Update.Size = new System.Drawing.Size(61, 17);
            this.chk_CS_Update.TabIndex = 7;
            this.chk_CS_Update.Text = "Update";
            this.chk_CS_Update.UseVisualStyleBackColor = true;
            // 
            // chk_CS_Add
            // 
            this.chk_CS_Add.AutoSize = true;
            this.chk_CS_Add.Checked = true;
            this.chk_CS_Add.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_Add.Location = new System.Drawing.Point(186, 18);
            this.chk_CS_Add.Name = "chk_CS_Add";
            this.chk_CS_Add.Size = new System.Drawing.Size(45, 17);
            this.chk_CS_Add.TabIndex = 4;
            this.chk_CS_Add.Text = "Add";
            this.chk_CS_Add.UseVisualStyleBackColor = true;
            // 
            // chk_CS_Exists
            // 
            this.chk_CS_Exists.AutoSize = true;
            this.chk_CS_Exists.Checked = true;
            this.chk_CS_Exists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_Exists.Location = new System.Drawing.Point(125, 18);
            this.chk_CS_Exists.Name = "chk_CS_Exists";
            this.chk_CS_Exists.Size = new System.Drawing.Size(53, 17);
            this.chk_CS_Exists.TabIndex = 5;
            this.chk_CS_Exists.Text = "Exists";
            this.chk_CS_Exists.UseVisualStyleBackColor = true;
            // 
            // chk_CS_GetMaxID
            // 
            this.chk_CS_GetMaxID.AutoSize = true;
            this.chk_CS_GetMaxID.Checked = true;
            this.chk_CS_GetMaxID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_CS_GetMaxID.Location = new System.Drawing.Point(56, 18);
            this.chk_CS_GetMaxID.Name = "chk_CS_GetMaxID";
            this.chk_CS_GetMaxID.Size = new System.Drawing.Size(74, 17);
            this.chk_CS_GetMaxID.TabIndex = 6;
            this.chk_CS_GetMaxID.Text = "GetMaxID";
            this.chk_CS_GetMaxID.UseVisualStyleBackColor = true;
            // 
            // groupBox_DALType
            // 
            this.groupBox_DALType.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_DALType.Location = new System.Drawing.Point(0, 354);
            this.groupBox_DALType.Name = "groupBox_DALType";
            this.groupBox_DALType.Size = new System.Drawing.Size(610, 42);
            this.groupBox_DALType.TabIndex = 4;
            this.groupBox_DALType.TabStop = false;
            this.groupBox_DALType.Text = "代码模板组件类型";
            // 
            // groupBox_F3
            // 
            this.groupBox_F3.Controls.Add(this.radbtn_F3_BLL);
            this.groupBox_F3.Controls.Add(this.radbtn_F3_DALFactory);
            this.groupBox_F3.Controls.Add(this.radbtn_F3_IDAL);
            this.groupBox_F3.Controls.Add(this.radbtn_F3_DAL);
            this.groupBox_F3.Controls.Add(this.radbtn_F3_Model);
            this.groupBox_F3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_F3.Location = new System.Drawing.Point(0, 314);
            this.groupBox_F3.Name = "groupBox_F3";
            this.groupBox_F3.Size = new System.Drawing.Size(610, 40);
            this.groupBox_F3.TabIndex = 4;
            this.groupBox_F3.TabStop = false;
            this.groupBox_F3.Text = "代码类型";
            // 
            // radbtn_F3_BLL
            // 
            this.radbtn_F3_BLL.AutoSize = true;
            this.radbtn_F3_BLL.Location = new System.Drawing.Point(342, 18);
            this.radbtn_F3_BLL.Name = "radbtn_F3_BLL";
            this.radbtn_F3_BLL.Size = new System.Drawing.Size(44, 17);
            this.radbtn_F3_BLL.TabIndex = 0;
            this.radbtn_F3_BLL.Text = "BLL";
            this.radbtn_F3_BLL.UseVisualStyleBackColor = true;
            this.radbtn_F3_BLL.Click += new System.EventHandler(this.radbtn_F3_Click);
            // 
            // radbtn_F3_DALFactory
            // 
            this.radbtn_F3_DALFactory.AutoSize = true;
            this.radbtn_F3_DALFactory.Location = new System.Drawing.Point(244, 18);
            this.radbtn_F3_DALFactory.Name = "radbtn_F3_DALFactory";
            this.radbtn_F3_DALFactory.Size = new System.Drawing.Size(81, 17);
            this.radbtn_F3_DALFactory.TabIndex = 0;
            this.radbtn_F3_DALFactory.Text = "DALFactory";
            this.radbtn_F3_DALFactory.UseVisualStyleBackColor = true;
            this.radbtn_F3_DALFactory.Click += new System.EventHandler(this.radbtn_F3_Click);
            // 
            // radbtn_F3_IDAL
            // 
            this.radbtn_F3_IDAL.AutoSize = true;
            this.radbtn_F3_IDAL.Location = new System.Drawing.Point(182, 18);
            this.radbtn_F3_IDAL.Name = "radbtn_F3_IDAL";
            this.radbtn_F3_IDAL.Size = new System.Drawing.Size(49, 17);
            this.radbtn_F3_IDAL.TabIndex = 0;
            this.radbtn_F3_IDAL.Text = "IDAL";
            this.radbtn_F3_IDAL.UseVisualStyleBackColor = true;
            this.radbtn_F3_IDAL.Click += new System.EventHandler(this.radbtn_F3_Click);
            // 
            // radbtn_F3_DAL
            // 
            this.radbtn_F3_DAL.AutoSize = true;
            this.radbtn_F3_DAL.Location = new System.Drawing.Point(126, 18);
            this.radbtn_F3_DAL.Name = "radbtn_F3_DAL";
            this.radbtn_F3_DAL.Size = new System.Drawing.Size(46, 17);
            this.radbtn_F3_DAL.TabIndex = 0;
            this.radbtn_F3_DAL.Text = "DAL";
            this.radbtn_F3_DAL.UseVisualStyleBackColor = true;
            this.radbtn_F3_DAL.Click += new System.EventHandler(this.radbtn_F3_Click);
            // 
            // radbtn_F3_Model
            // 
            this.radbtn_F3_Model.AutoSize = true;
            this.radbtn_F3_Model.Checked = true;
            this.radbtn_F3_Model.Location = new System.Drawing.Point(58, 18);
            this.radbtn_F3_Model.Name = "radbtn_F3_Model";
            this.radbtn_F3_Model.Size = new System.Drawing.Size(54, 17);
            this.radbtn_F3_Model.TabIndex = 0;
            this.radbtn_F3_Model.TabStop = true;
            this.radbtn_F3_Model.Text = "Model";
            this.radbtn_F3_Model.UseVisualStyleBackColor = true;
            this.radbtn_F3_Model.Click += new System.EventHandler(this.radbtn_F3_Click);
            // 
            // groupBox_FrameSel
            // 
            this.groupBox_FrameSel.Controls.Add(this.radbtn_Frame_F3);
            this.groupBox_FrameSel.Controls.Add(this.radbtn_Frame_S3);
            this.groupBox_FrameSel.Controls.Add(this.radbtn_Frame_One);
            this.groupBox_FrameSel.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_FrameSel.Location = new System.Drawing.Point(0, 274);
            this.groupBox_FrameSel.Name = "groupBox_FrameSel";
            this.groupBox_FrameSel.Size = new System.Drawing.Size(610, 40);
            this.groupBox_FrameSel.TabIndex = 4;
            this.groupBox_FrameSel.TabStop = false;
            this.groupBox_FrameSel.Text = "架构选择";
            // 
            // radbtn_Frame_F3
            // 
            this.radbtn_Frame_F3.AutoSize = true;
            this.radbtn_Frame_F3.Location = new System.Drawing.Point(263, 16);
            this.radbtn_Frame_F3.Name = "radbtn_Frame_F3";
            this.radbtn_Frame_F3.Size = new System.Drawing.Size(97, 17);
            this.radbtn_Frame_F3.TabIndex = 0;
            this.radbtn_Frame_F3.Text = "工厂模式三层";
            this.radbtn_Frame_F3.UseVisualStyleBackColor = true;
            this.radbtn_Frame_F3.Visible = false;
            this.radbtn_Frame_F3.Click += new System.EventHandler(this.radbtn_Frame_Click);
            // 
            // radbtn_Frame_S3
            // 
            this.radbtn_Frame_S3.AutoSize = true;
            this.radbtn_Frame_S3.Checked = true;
            this.radbtn_Frame_S3.Location = new System.Drawing.Point(159, 16);
            this.radbtn_Frame_S3.Name = "radbtn_Frame_S3";
            this.radbtn_Frame_S3.Size = new System.Drawing.Size(49, 17);
            this.radbtn_Frame_S3.TabIndex = 0;
            this.radbtn_Frame_S3.TabStop = true;
            this.radbtn_Frame_S3.Text = "四层";
            this.radbtn_Frame_S3.UseVisualStyleBackColor = true;
            this.radbtn_Frame_S3.Click += new System.EventHandler(this.radbtn_Frame_Click);
            // 
            // radbtn_Frame_One
            // 
            this.radbtn_Frame_One.AutoSize = true;
            this.radbtn_Frame_One.Location = new System.Drawing.Point(58, 16);
            this.radbtn_Frame_One.Name = "radbtn_Frame_One";
            this.radbtn_Frame_One.Size = new System.Drawing.Size(73, 17);
            this.radbtn_Frame_One.TabIndex = 0;
            this.radbtn_Frame_One.Text = "单类结构";
            this.radbtn_Frame_One.UseVisualStyleBackColor = true;
            this.radbtn_Frame_One.Visible = false;
            this.radbtn_Frame_One.Click += new System.EventHandler(this.radbtn_Frame_Click);
            // 
            // groupBox_DB
            // 
            this.groupBox_DB.Controls.Add(this.chk_DB_GetList);
            this.groupBox_DB.Controls.Add(this.chk_DB_GetModel);
            this.groupBox_DB.Controls.Add(this.chk_DB_Delete);
            this.groupBox_DB.Controls.Add(this.chk_DB_Update);
            this.groupBox_DB.Controls.Add(this.chk_DB_Add);
            this.groupBox_DB.Controls.Add(this.chk_DB_Exists);
            this.groupBox_DB.Controls.Add(this.chk_DB_GetMaxID);
            this.groupBox_DB.Controls.Add(this.txtTabname);
            this.groupBox_DB.Controls.Add(this.txtProcPrefix);
            this.groupBox_DB.Controls.Add(this.label6);
            this.groupBox_DB.Controls.Add(this.label7);
            this.groupBox_DB.Controls.Add(this.label5);
            this.groupBox_DB.Controls.Add(this.radbtn_DB_DDL);
            this.groupBox_DB.Controls.Add(this.radbtn_DB_Proc);
            this.groupBox_DB.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_DB.Location = new System.Drawing.Point(0, 172);
            this.groupBox_DB.Name = "groupBox_DB";
            this.groupBox_DB.Size = new System.Drawing.Size(610, 102);
            this.groupBox_DB.TabIndex = 3;
            this.groupBox_DB.TabStop = false;
            this.groupBox_DB.Text = "数据库脚本";
            // 
            // chk_DB_GetList
            // 
            this.chk_DB_GetList.AutoSize = true;
            this.chk_DB_GetList.Checked = true;
            this.chk_DB_GetList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_DB_GetList.Location = new System.Drawing.Point(486, 77);
            this.chk_DB_GetList.Name = "chk_DB_GetList";
            this.chk_DB_GetList.Size = new System.Drawing.Size(59, 17);
            this.chk_DB_GetList.TabIndex = 3;
            this.chk_DB_GetList.Text = "GetList";
            this.chk_DB_GetList.UseVisualStyleBackColor = true;
            // 
            // chk_DB_GetModel
            // 
            this.chk_DB_GetModel.AutoSize = true;
            this.chk_DB_GetModel.Checked = true;
            this.chk_DB_GetModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_DB_GetModel.Location = new System.Drawing.Point(412, 77);
            this.chk_DB_GetModel.Name = "chk_DB_GetModel";
            this.chk_DB_GetModel.Size = new System.Drawing.Size(72, 17);
            this.chk_DB_GetModel.TabIndex = 3;
            this.chk_DB_GetModel.Text = "GetModel";
            this.chk_DB_GetModel.UseVisualStyleBackColor = true;
            // 
            // chk_DB_Delete
            // 
            this.chk_DB_Delete.AutoSize = true;
            this.chk_DB_Delete.Checked = true;
            this.chk_DB_Delete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_DB_Delete.Location = new System.Drawing.Point(350, 77);
            this.chk_DB_Delete.Name = "chk_DB_Delete";
            this.chk_DB_Delete.Size = new System.Drawing.Size(57, 17);
            this.chk_DB_Delete.TabIndex = 3;
            this.chk_DB_Delete.Text = "Delete";
            this.chk_DB_Delete.UseVisualStyleBackColor = true;
            // 
            // chk_DB_Update
            // 
            this.chk_DB_Update.AutoSize = true;
            this.chk_DB_Update.Checked = true;
            this.chk_DB_Update.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_DB_Update.Location = new System.Drawing.Point(288, 77);
            this.chk_DB_Update.Name = "chk_DB_Update";
            this.chk_DB_Update.Size = new System.Drawing.Size(61, 17);
            this.chk_DB_Update.TabIndex = 3;
            this.chk_DB_Update.Text = "Update";
            this.chk_DB_Update.UseVisualStyleBackColor = true;
            // 
            // chk_DB_Add
            // 
            this.chk_DB_Add.AutoSize = true;
            this.chk_DB_Add.Checked = true;
            this.chk_DB_Add.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_DB_Add.Location = new System.Drawing.Point(244, 77);
            this.chk_DB_Add.Name = "chk_DB_Add";
            this.chk_DB_Add.Size = new System.Drawing.Size(45, 17);
            this.chk_DB_Add.TabIndex = 3;
            this.chk_DB_Add.Text = "Add";
            this.chk_DB_Add.UseVisualStyleBackColor = true;
            // 
            // chk_DB_Exists
            // 
            this.chk_DB_Exists.AutoSize = true;
            this.chk_DB_Exists.Checked = true;
            this.chk_DB_Exists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_DB_Exists.Location = new System.Drawing.Point(182, 77);
            this.chk_DB_Exists.Name = "chk_DB_Exists";
            this.chk_DB_Exists.Size = new System.Drawing.Size(53, 17);
            this.chk_DB_Exists.TabIndex = 3;
            this.chk_DB_Exists.Text = "Exists";
            this.chk_DB_Exists.UseVisualStyleBackColor = true;
            // 
            // chk_DB_GetMaxID
            // 
            this.chk_DB_GetMaxID.AutoSize = true;
            this.chk_DB_GetMaxID.Checked = true;
            this.chk_DB_GetMaxID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_DB_GetMaxID.Location = new System.Drawing.Point(108, 77);
            this.chk_DB_GetMaxID.Name = "chk_DB_GetMaxID";
            this.chk_DB_GetMaxID.Size = new System.Drawing.Size(74, 17);
            this.chk_DB_GetMaxID.TabIndex = 3;
            this.chk_DB_GetMaxID.Text = "GetMaxID";
            this.chk_DB_GetMaxID.UseVisualStyleBackColor = true;
            // 
            // txtTabname
            // 
            this.txtTabname.Location = new System.Drawing.Point(201, 49);
            this.txtTabname.Name = "txtTabname";
            this.txtTabname.Size = new System.Drawing.Size(75, 20);
            this.txtTabname.TabIndex = 2;
            // 
            // txtProcPrefix
            // 
            this.txtProcPrefix.Location = new System.Drawing.Point(108, 48);
            this.txtProcPrefix.Name = "txtProcPrefix";
            this.txtProcPrefix.Size = new System.Drawing.Size(75, 20);
            this.txtProcPrefix.TabIndex = 2;
            this.txtProcPrefix.Text = "UP_";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "存储过程方法：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(187, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "存储过程前缀：";
            // 
            // radbtn_DB_DDL
            // 
            this.radbtn_DB_DDL.AutoSize = true;
            this.radbtn_DB_DDL.Location = new System.Drawing.Point(169, 20);
            this.radbtn_DB_DDL.Name = "radbtn_DB_DDL";
            this.radbtn_DB_DDL.Size = new System.Drawing.Size(97, 17);
            this.radbtn_DB_DDL.TabIndex = 0;
            this.radbtn_DB_DDL.Text = "数据创建脚本";
            this.radbtn_DB_DDL.UseVisualStyleBackColor = true;
            this.radbtn_DB_DDL.Click += new System.EventHandler(this.radbtn_DBSel_Click);
            // 
            // radbtn_DB_Proc
            // 
            this.radbtn_DB_Proc.AutoSize = true;
            this.radbtn_DB_Proc.Checked = true;
            this.radbtn_DB_Proc.Location = new System.Drawing.Point(58, 20);
            this.radbtn_DB_Proc.Name = "radbtn_DB_Proc";
            this.radbtn_DB_Proc.Size = new System.Drawing.Size(73, 17);
            this.radbtn_DB_Proc.TabIndex = 0;
            this.radbtn_DB_Proc.TabStop = true;
            this.radbtn_DB_Proc.Text = "存储过程";
            this.radbtn_DB_Proc.UseVisualStyleBackColor = true;
            this.radbtn_DB_Proc.Click += new System.EventHandler(this.radbtn_DBSel_Click);
            // 
            // groupBox_Type
            // 
            this.groupBox_Type.Controls.Add(this.radbtn_Type_Web);
            this.groupBox_Type.Controls.Add(this.radbtn_Type_CS);
            this.groupBox_Type.Controls.Add(this.radbtn_Type_DB);
            this.groupBox_Type.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Type.Location = new System.Drawing.Point(0, 128);
            this.groupBox_Type.Name = "groupBox_Type";
            this.groupBox_Type.Size = new System.Drawing.Size(610, 44);
            this.groupBox_Type.TabIndex = 2;
            this.groupBox_Type.TabStop = false;
            this.groupBox_Type.Text = "类型";
            // 
            // radbtn_Type_Web
            // 
            this.radbtn_Type_Web.AutoSize = true;
            this.radbtn_Type_Web.Location = new System.Drawing.Point(242, 20);
            this.radbtn_Type_Web.Name = "radbtn_Type_Web";
            this.radbtn_Type_Web.Size = new System.Drawing.Size(73, 17);
            this.radbtn_Type_Web.TabIndex = 0;
            this.radbtn_Type_Web.Text = "应用程序";
            this.radbtn_Type_Web.UseVisualStyleBackColor = true;
            this.radbtn_Type_Web.Click += new System.EventHandler(this.radbtn_Type_Click);
            // 
            // radbtn_Type_CS
            // 
            this.radbtn_Type_CS.AutoSize = true;
            this.radbtn_Type_CS.Checked = true;
            this.radbtn_Type_CS.Location = new System.Drawing.Point(152, 20);
            this.radbtn_Type_CS.Name = "radbtn_Type_CS";
            this.radbtn_Type_CS.Size = new System.Drawing.Size(61, 17);
            this.radbtn_Type_CS.TabIndex = 0;
            this.radbtn_Type_CS.TabStop = true;
            this.radbtn_Type_CS.Text = "源代码";
            this.radbtn_Type_CS.UseVisualStyleBackColor = true;
            this.radbtn_Type_CS.Click += new System.EventHandler(this.radbtn_Type_Click);
            // 
            // radbtn_Type_DB
            // 
            this.radbtn_Type_DB.AutoSize = true;
            this.radbtn_Type_DB.Location = new System.Drawing.Point(58, 20);
            this.radbtn_Type_DB.Name = "radbtn_Type_DB";
            this.radbtn_Type_DB.Size = new System.Drawing.Size(85, 17);
            this.radbtn_Type_DB.TabIndex = 0;
            this.radbtn_Type_DB.Text = "数据库脚本";
            this.radbtn_Type_DB.UseVisualStyleBackColor = true;
            this.radbtn_Type_DB.Click += new System.EventHandler(this.radbtn_Type_Click);
            // 
            // groupBox_Parameter
            // 
            this.groupBox_Parameter.Controls.Add(this.txtClassName);
            this.groupBox_Parameter.Controls.Add(this.txtNameSpace2);
            this.groupBox_Parameter.Controls.Add(this.txtNameSpace);
            this.groupBox_Parameter.Controls.Add(this.txtProjectName);
            this.groupBox_Parameter.Controls.Add(this.label3);
            this.groupBox_Parameter.Controls.Add(this.label4);
            this.groupBox_Parameter.Controls.Add(this.label2);
            this.groupBox_Parameter.Controls.Add(this.label1);
            this.groupBox_Parameter.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Parameter.Location = new System.Drawing.Point(0, 54);
            this.groupBox_Parameter.Name = "groupBox_Parameter";
            this.groupBox_Parameter.Size = new System.Drawing.Size(610, 74);
            this.groupBox_Parameter.TabIndex = 1;
            this.groupBox_Parameter.TabStop = false;
            this.groupBox_Parameter.Text = "参数";
            this.groupBox_Parameter.Visible = false;
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(382, 43);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(100, 20);
            this.txtClassName.TabIndex = 1;
            // 
            // txtNameSpace2
            // 
            this.txtNameSpace2.Location = new System.Drawing.Point(382, 20);
            this.txtNameSpace2.Name = "txtNameSpace2";
            this.txtNameSpace2.Size = new System.Drawing.Size(100, 20);
            this.txtNameSpace2.TabIndex = 1;
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(111, 43);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(100, 20);
            this.txtNameSpace.TabIndex = 1;
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(111, 20);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(100, 20);
            this.txtProjectName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(309, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "类名：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(291, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "二级命名空间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "顶级命名空间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目名称：";
            // 
            // groupBox_Select
            // 
            this.groupBox_Select.Controls.Add(this.btn_SetKey);
            this.groupBox_Select.Controls.Add(this.list_KeyField);
            this.groupBox_Select.Controls.Add(this.btn_SelClear);
            this.groupBox_Select.Controls.Add(this.btn_SelI);
            this.groupBox_Select.Controls.Add(this.btn_SelAll);
            this.groupBox_Select.Controls.Add(this.lblkeycount);
            this.groupBox_Select.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Select.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Select.Name = "groupBox_Select";
            this.groupBox_Select.Size = new System.Drawing.Size(610, 54);
            this.groupBox_Select.TabIndex = 0;
            this.groupBox_Select.TabStop = false;
            this.groupBox_Select.Text = "操作";
            // 
            // btn_SetKey
            // 
            this.btn_SetKey.Location = new System.Drawing.Point(293, 20);
            this.btn_SetKey.Name = "btn_SetKey";
            this.btn_SetKey.Size = new System.Drawing.Size(121, 23);
            this.btn_SetKey.TabIndex = 2;
            this.btn_SetKey.Text = "添加主键(条件)字段";
            this.btn_SetKey.UseVisualStyleBackColor = true;
            this.btn_SetKey.Visible = false;
            this.btn_SetKey.Click += new System.EventHandler(this.btn_SetKey_Click);
            // 
            // list_KeyField
            // 
            this.list_KeyField.FormattingEnabled = true;
            this.list_KeyField.Location = new System.Drawing.Point(422, 23);
            this.list_KeyField.Name = "list_KeyField";
            this.list_KeyField.Size = new System.Drawing.Size(129, 17);
            this.list_KeyField.TabIndex = 1;
            this.list_KeyField.Visible = false;
            // 
            // btn_SelClear
            // 
            this.btn_SelClear.Location = new System.Drawing.Point(169, 20);
            this.btn_SelClear.Name = "btn_SelClear";
            this.btn_SelClear.Size = new System.Drawing.Size(70, 23);
            this.btn_SelClear.TabIndex = 0;
            this.btn_SelClear.Text = "清空(&C)";
            this.btn_SelClear.UseVisualStyleBackColor = true;
            this.btn_SelClear.Click += new System.EventHandler(this.btn_SelClear_Click);
            // 
            // btn_SelI
            // 
            this.btn_SelI.Location = new System.Drawing.Point(93, 20);
            this.btn_SelI.Name = "btn_SelI";
            this.btn_SelI.Size = new System.Drawing.Size(70, 23);
            this.btn_SelI.TabIndex = 0;
            this.btn_SelI.Text = "反选(&I)";
            this.btn_SelI.UseVisualStyleBackColor = true;
            this.btn_SelI.Click += new System.EventHandler(this.btn_SelI_Click);
            // 
            // btn_SelAll
            // 
            this.btn_SelAll.Location = new System.Drawing.Point(17, 20);
            this.btn_SelAll.Name = "btn_SelAll";
            this.btn_SelAll.Size = new System.Drawing.Size(70, 23);
            this.btn_SelAll.TabIndex = 0;
            this.btn_SelAll.Text = "全选(&A)";
            this.btn_SelAll.UseVisualStyleBackColor = true;
            this.btn_SelAll.Click += new System.EventHandler(this.btn_SelAll_Click);
            // 
            // lblkeycount
            // 
            this.lblkeycount.AutoSize = true;
            this.lblkeycount.Location = new System.Drawing.Point(550, 23);
            this.lblkeycount.Name = "lblkeycount";
            this.lblkeycount.Size = new System.Drawing.Size(49, 13);
            this.lblkeycount.TabIndex = 1;
            this.lblkeycount.Text = "0个主键";
            this.lblkeycount.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(3, 53);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(610, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listFields);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(610, 50);
            this.panel1.TabIndex = 0;
            // 
            // listFields
            // 
            this.listFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFields.Location = new System.Drawing.Point(0, 0);
            this.listFields.Name = "listFields";
            this.listFields.Size = new System.Drawing.Size(610, 50);
            this.listFields.TabIndex = 0;
            this.listFields.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(616, 614);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Source ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // imgListTabpage
            // 
            this.imgListTabpage.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgListTabpage.ImageSize = new System.Drawing.Size(16, 16);
            this.imgListTabpage.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imglistDB
            // 
            this.imglistDB.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imglistDB.ImageSize = new System.Drawing.Size(16, 16);
            this.imglistDB.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imglistView
            // 
            this.imglistView.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imglistView.ImageSize = new System.Drawing.Size(16, 16);
            this.imglistView.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 23);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.列表ToolStripMenuItem,
            this.详细信息ToolStripMenuItem});
            this.toolStripSplitButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(64, 23);
            this.toolStripSplitButton1.Text = "列表";
            // 
            // 列表ToolStripMenuItem
            // 
            this.列表ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.列表ToolStripMenuItem.Name = "列表ToolStripMenuItem";
            this.列表ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.列表ToolStripMenuItem.Text = "列表";
            // 
            // 详细信息ToolStripMenuItem
            // 
            this.详细信息ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.详细信息ToolStripMenuItem.Name = "详细信息ToolStripMenuItem";
            this.详细信息ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.详细信息ToolStripMenuItem.Text = "详细信息";
            // 
            // FrmCodeMaker
            // 
            this.ClientSize = new System.Drawing.Size(624, 641);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmCodeMaker";
            this.Text = "CodeMaker";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox_Web.ResumeLayout(false);
            this.groupBox_Web.PerformLayout();
            this.groupBox_AppType.ResumeLayout(false);
            this.groupBox_AppType.PerformLayout();
            this.groupBox_Method.ResumeLayout(false);
            this.groupBox_Method.PerformLayout();
            this.groupBox_F3.ResumeLayout(false);
            this.groupBox_F3.PerformLayout();
            this.groupBox_FrameSel.ResumeLayout(false);
            this.groupBox_FrameSel.PerformLayout();
            this.groupBox_DB.ResumeLayout(false);
            this.groupBox_DB.PerformLayout();
            this.groupBox_Type.ResumeLayout(false);
            this.groupBox_Type.PerformLayout();
            this.groupBox_Parameter.ResumeLayout(false);
            this.groupBox_Parameter.PerformLayout();
            this.groupBox_Select.ResumeLayout(false);
            this.groupBox_Select.PerformLayout();
            this.panel1.ResumeLayout(false);
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
            this.panel1.Height = 150;
            this.radbtn_Type_Click(null, null);
           // this.setting = ModuleConfig.GetSettings();
            //this.txtProjectName.Text = this.setting.ProjectName;
            //this.txtNameSpace.Text = this.setting.Namepace;
            //this.txtNameSpace2.Text = this.setting.Folder;
            //this.txtProcPrefix.Text = this.setting.ProcPrefix;
            //string appFrame = this.setting.AppFrame;
            string appFrame = string.Empty;
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
           // this.cm_daltype = new DALTypeAddIn("LTP.IBuilder.IBuilderDAL");
            this.cm_daltype = new DALTypeAddIn("LTP.IBuilder.IBuilderDAL");

            this.cm_daltype.Title = "DAL";
            this.groupBox_DALType.Controls.Add(this.cm_daltype);
            //TODO: 需要调试的代码
            this.cm_daltype.Location = new Point(30, 0x12);
            //this.cm_daltype.SetSelectedDALType(this.setting.DALType.Trim());
            this.cm_blltype = new DALTypeAddIn("LTP.IBuilder.IBuilderBLL");
            this.cm_blltype.Title = "BLL";
            this.groupBox_DALType.Controls.Add(this.cm_blltype);
            this.cm_blltype.Location = new Point(30, 0x12);
            //this.cm_blltype.SetSelectedDALType(this.setting.BLLType.Trim());
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
                    goto Label_01A7;
                }
                if (!(str == "table"))
                {
                    if (str == "view")
                    {
                        this.servername = selectedNode.Parent.Parent.Parent.Text;
                        this.dbname = selectedNode.Parent.Parent.Text;
                        this.tablename = selectedNode.Text;
                        dbobj = ObjectHelper.CreatDbObj(this.servername);
                        this.SetListViewMenu("column");
                        this.BindlistViewCol(this.dbname, this.tablename);
                        return;
                    }
                    if (str == "column")
                    {
                        this.servername = selectedNode.Parent.Parent.Parent.Parent.Text;
                        this.dbname = selectedNode.Parent.Parent.Parent.Text;
                        this.tablename = selectedNode.Parent.Text;
                        this.dbobj = ObjectHelper.CreatDbObj(this.servername);
                        this.SetListViewMenu("column");
                        this.BindlistViewCol(this.dbname, this.tablename);
                        return;
                    }
                    goto Label_01A7;
                }
                this.servername = selectedNode.Parent.Parent.Parent.Text;
                this.dbname = selectedNode.Parent.Parent.Text;
                this.tablename = selectedNode.Text;
                this.dbobj = ObjectHelper.CreatDbObj(this.servername);
                this.SetListViewMenu("column");
                this.BindlistViewCol(this.dbname, this.tablename);
            }
            return;
        Label_01A7:
            this.listFields.Items.Clear();
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
            code.txtContent.Text = strContent;   
            this.tabControl1.SelectedIndex = 1;
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

            //在主表的表变化时，需要更新ListView
            MainForm frm = (MainForm)Application.OpenForms["MainForm"];

            
        }

        private delegate void SetListCallback();
    }
}


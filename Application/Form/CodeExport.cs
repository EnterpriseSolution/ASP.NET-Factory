namespace Codematic
{
    using WebMatrix.Properties;
    using Codematic.UserControls;
    using LTP.CodeBuild;
    using LTP.CodeHelper;
    using LTP.DBFactory;
    using LTP.IDBO;
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
    

    public class CodeExport : Form
    {
        private Button btn_Add;
        private Button btn_Addlist;
        private Button btn_Cancle;
        private Button btn_Del;
        private Button btn_Dellist;
        private Button btn_Export;
        private Button btn_TargetFold;
        private CodeBuilders cb;
        private INIFile cfgfile;
        private DALTypeAddIn cm_blltype;
        private DALTypeAddIn cm_daltype;
        private ComboBox cmbDB;
        private string cmcfgfile = (Application.StartupPath + @"\cmcfg.ini");
        private IContainer components;
        private string dbname = "";
        private IDbObject dbobj;
        private DbSettings dbset;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label labelNum;
        private Label lblServer;
        private ListBox listTable1;
        private ListBox listTable2;
        private Thread mythread;
        private PictureBox pictureBox1;
        private ProgressBar progressBar1;
        private RadioButton radBtn_F3;
        private RadioButton radBtn_One;
        private RadioButton radBtn_S3;
        private ModuleSettings setting;
        private TextBox txtDbHelper;
        private TextBox txtFolder;
        private TextBox txtNamespace;
        private TextBox txtTabNamepre;
        private TextBox txtTargetFolder;
        private ImageList imageList1;
        private VSProject vsp = new VSProject();

        public CodeExport(string longservername)
        {
            this.InitializeComponent();
            this.dbset = DbConfig.GetSetting(longservername);
            this.dbobj = DBOMaker.CreateDbObj(this.dbset.DbType);
            this.dbobj.DbConnectStr = this.dbset.ConnectStr;
            this.cb = new CodeBuilders(this.dbobj);
            this.lblServer.Text = this.dbset.Server;
        }

        private void AddClassFile(string ProjectFile, string classFileName, string ProType)
        {
            if (File.Exists(ProjectFile))
            {
                switch (ProType)
                {
                    case "2003":
                        this.vsp.AddClass2003(ProjectFile, classFileName);
                        return;

                    case "2005":
                        this.vsp.AddClass2005(ProjectFile, classFileName);
                        return;
                }
                this.vsp.AddClass(ProjectFile, classFileName);
            }
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
            base.Close();
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

        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtTargetFolder.Text.Trim() == "")
                {
                    MessageBox.Show("目标文件夹为空！");
                }
                else
                {
                    this.cfgfile.IniWriteValue("Project", "lastpath", this.txtTargetFolder.Text.Trim());
                    this.dbname = this.cmbDB.Text;
                    this.pictureBox1.Visible = true;
                    this.mythread = new Thread(new ThreadStart(this.ThreadWork));
                    this.mythread.Start();
                }
            }
            catch (Exception exception)
            {
                LogInfo.WriteLog(exception);
                MessageBox.Show(exception.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btn_TargetFold_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.txtTargetFolder.Text = dialog.SelectedPath;
            }
        }

        private void cmbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.cmbDB.Text;
            List<string> tables = this.dbobj.GetTables(text);
            this.listTable1.Items.Clear();
            this.listTable2.Items.Clear();
            if (tables.Count > 0)
            {
                foreach (string str2 in tables)
                {
                    this.listTable1.Items.Add(str2);
                }
            }
            this.IsHasItem();
        }

        private void CodeExport_Load(object sender, EventArgs e)
        {
            string str7;
            string dbName = "master";
            string dbType = this.dbobj.DbType;
            if (dbType != null)
            {
                if (!(dbType == "SQL2000") && !(dbType == "SQL2005"))
                {
                    if ((dbType == "Oracle") || (dbType == "OleDb"))
                    {
                        dbName = this.dbset.DbName;
                    }
                    else if (dbType == "MySQL")
                    {
                        dbName = "mysql";
                    }
                }
                else
                {
                    dbName = "master";
                }
            }
            if ((this.dbset.DbName == "") || (this.dbset.DbName == dbName))
            {
                List<string> dBList = this.dbobj.GetDBList();
                if ((dBList != null) && (dBList.Count > 0))
                {
                    foreach (string str2 in dBList)
                    {
                        this.cmbDB.Items.Add(str2);
                    }
                }
            }
            else
            {
                this.cmbDB.Items.Add(this.dbset.DbName);
            }
            if (this.cmbDB.Items.Count > 0)
            {
                this.cmbDB.SelectedIndex = 0;
            }
            else
            {
                List<string> tables = this.dbobj.GetTables("");
                this.listTable1.Items.Clear();
                this.listTable2.Items.Clear();
                if (tables.Count > 0)
                {
                    foreach (string str3 in tables)
                    {
                        this.listTable1.Items.Add(str3);
                    }
                }
            }
            this.btn_Export.Enabled = false;
            this.setting = ModuleConfig.GetSettings();
            string appFrame = this.setting.AppFrame;
            if (appFrame != null)
            {
                if (!(appFrame == "One"))
                {
                    if (appFrame == "S3")
                    {
                        this.radBtn_S3.Checked = true;
                    }
                    else if (appFrame == "F3")
                    {
                        this.radBtn_F3.Checked = true;
                    }
                }
                else
                {
                    this.radBtn_One.Checked = true;
                }
            }
            this.cm_blltype = new DALTypeAddIn("LTP.IBuilder.IBuilderBLL");
            this.cm_blltype.Title = "BLL";
            this.groupBox5.Controls.Add(this.cm_blltype);
            this.cm_blltype.Location = new Point(30, 40);
            this.cm_blltype.SetSelectedDALType(this.setting.BLLType.Trim());
            this.cm_daltype = new DALTypeAddIn("LTP.IBuilder.IBuilderDAL");
            this.cm_daltype.Title = "DAL";
            this.groupBox5.Controls.Add(this.cm_daltype);
            this.cm_daltype.Location = new Point(30, 0x40);
            this.cm_daltype.SetSelectedDALType(this.setting.DALType);
            this.txtDbHelper.Text = this.setting.DbHelperName;
            if ((this.setting.DbHelperName == "DbHelperSQL") && ((str7 = this.dbobj.DbType) != null))
            {
                if (!(str7 == "SQL2000") && !(str7 == "SQL2005"))
                {
                    if (str7 == "Oracle")
                    {
                        this.txtDbHelper.Text = "DbHelperOra";
                    }
                    else if (str7 == "MySQL")
                    {
                        this.txtDbHelper.Text = "DbHelperMySQL";
                    }
                    else if (str7 == "OleDb")
                    {
                        this.txtDbHelper.Text = "DbHelperOleDb";
                    }
                }
                else
                {
                    this.txtDbHelper.Text = "DbHelperSQL";
                }
            }
            this.txtFolder.Text = this.setting.Folder;
            this.txtNamespace.Text = this.setting.Namepace;
            if (File.Exists(this.cmcfgfile))
            {
                this.cfgfile = new INIFile(this.cmcfgfile);
                string str4 = this.cfgfile.IniReadValue("Project", "lastpath");
                if (str4.Trim() != "")
                {
                    this.txtTargetFolder.Text = str4;
                }
            }
        }

        private void CreatCS()
        {
            if (this.radBtn_One.Checked)
            {
                this.CreatCsOne();
            }
            if (this.radBtn_S3.Checked)
            {
                this.CreatCsS3();
            }
            if (this.radBtn_F3.Checked)
            {
                this.CreatCsF3();
            }
        }

        private void CreatCsF3()
        {
            string text = this.txtTargetFolder.Text;
            this.FolderCheck(text);
            string folder = text + @"\Model";
            if (this.cb.Folder != "")
            {
                folder = folder + @"\" + this.cb.Folder;
            }
            this.FolderCheck(folder);
            string filename = folder + @"\" + this.cb.ModelName + ".cs";
            string strCode = this.cb.GetCodeFrameS3Model();
            this.WriteFile(filename, strCode);
            this.AddClassFile(folder + @"\Model.csproj", this.cb.ModelName + ".cs", "");
            string dbType = this.dbobj.DbType;
            if ((this.dbobj.DbType == "SQL2000") || (this.dbobj.DbType == "SQL2005"))
            {
                dbType = "SQLServer";
            }
            string str6 = text + @"\" + dbType + "DAL";
            if (this.cb.Folder != "")
            {
                str6 = str6 + @"\" + this.cb.Folder;
            }
            this.FolderCheck(str6);
            string str7 = str6 + @"\" + this.cb.ModelName + ".cs";
            string str8 = this.cb.GetCodeFrameF3DAL(this.GetDALType(), true, true, true, true, true, true, true);
            this.WriteFile(str7, str8);
            this.AddClassFile(str6 + @"\" + dbType + "DAL.csproj", this.cb.ModelName + ".cs", "");
            string str9 = text + @"\DALFactory";
            this.FolderCheck(str9);
            string path = str9 + @"\DataAccess.cs";
            string strContent = this.cb.GetCodeFrameF3DALFactory();
            if (File.Exists(path))
            {
                if (File.ReadAllText(path).IndexOf("class DataAccess") > 0)
                {
                    strContent = this.cb.GetCodeFrameF3DALFactoryMethod();
                    this.vsp.AddMethodToClass(path, strContent);
                }
                else
                {
                    strContent = this.cb.GetCodeFrameF3DALFactory();
                    StreamWriter writer = new StreamWriter(path, true, Encoding.Default);
                    writer.Write(strContent);
                    writer.Flush();
                    writer.Close();
                }
            }
            else
            {
                strContent = this.cb.GetCodeFrameF3DALFactory();
                this.WriteFile(path, strContent);
            }
            string str13 = text + @"\IDAL";
            if (this.cb.Folder != "")
            {
                str13 = str13 + @"\" + this.cb.Folder;
            }
            this.FolderCheck(str13);
            string str14 = str13 + @"\I" + this.cb.ModelName + ".cs";
            string str15 = this.cb.GetCodeFrameF3IDAL(true, true, true, true, true, true, true, true);
            this.WriteFile(str14, str15);
            this.AddClassFile(str13 + @"\IDAL.csproj", "I" + this.cb.ModelName + ".cs", "");
            string str16 = text + @"\BLL";
            if (this.cb.Folder != "")
            {
                str16 = str16 + @"\" + this.cb.Folder;
            }
            this.FolderCheck(str16);
            string str17 = str16 + @"\" + this.cb.ModelName + ".cs";
            string bLLType = this.GetBLLType();
            string str19 = this.cb.GetCodeFrameF3BLL(bLLType, true, true, true, true, true, true, true, true);
            this.WriteFile(str17, str19);
            this.AddClassFile(str16 + @"\BLL.csproj", this.cb.ModelName + ".cs", "");
            string str20 = text + @"\Web";
            if (this.cb.Folder != "")
            {
                str20 = str20 + @"\" + this.cb.Folder;
            }
            this.FolderCheck(str20);
            this.FolderCheck(str20 + @"\" + this.cb.ModelName);
            string str21 = "";
            string str22 = str20 + @"\" + this.cb.ModelName + @"\Add.aspx";
            string str23 = str20 + @"\" + this.cb.ModelName + @"\Add.aspx.cs";
            string str24 = str20 + @"\" + this.cb.ModelName + @"\Add.aspx.designer.cs";
            string str25 = Application.StartupPath + @"\Template\web\Add.aspx";
            string str26 = Application.StartupPath + @"\Template\web\Add.aspx.cs";
            string str27 = Application.StartupPath + @"\Template\web\Add.aspx.designer.cs";
            if (File.Exists(str25))
            {
                using (StreamReader reader = new StreamReader(str25, Encoding.Default))
                {
                    string addAspx = this.cb.GetAddAspx();
                    str21 = reader.ReadToEnd().Replace(".Demo.Add", "." + this.cb.ModelName + ".Add").Replace("<$$AddAspx$$>", addAspx);
                    reader.Close();
                }
                this.WriteFile(str22, str21);
            }
            if (File.Exists(str26))
            {
                using (StreamReader reader2 = new StreamReader(str26, Encoding.Default))
                {
                    string addAspxCs = this.cb.GetAddAspxCs();
                    str21 = reader2.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$AddAspxCs$$>", addAspxCs);
                    reader2.Close();
                }
                this.WriteFile(str23, str21);
            }
            if (File.Exists(str27))
            {
                using (StreamReader reader3 = new StreamReader(str27, Encoding.Default))
                {
                    string addDesigner = this.cb.GetAddDesigner();
                    str21 = reader3.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$AddDesigner$$>", addDesigner);
                    reader3.Close();
                }
                this.WriteFile(str24, str21);
            }
            str22 = str20 + @"\" + this.cb.ModelName + @"\Modify.aspx";
            str23 = str20 + @"\" + this.cb.ModelName + @"\Modify.aspx.cs";
            str24 = str20 + @"\" + this.cb.ModelName + @"\Modify.aspx.designer.cs";
            str25 = Application.StartupPath + @"\Template\web\Modify.aspx";
            str26 = Application.StartupPath + @"\Template\web\Modify.aspx.cs";
            str27 = Application.StartupPath + @"\Template\web\Modify.aspx.designer.cs";
            if (File.Exists(str25))
            {
                using (StreamReader reader4 = new StreamReader(str25, Encoding.Default))
                {
                    string updateAspx = this.cb.GetUpdateAspx();
                    str21 = reader4.ReadToEnd().Replace(".Demo.Modify", "." + this.cb.ModelName + ".Modify").Replace("<$$ModifyAspx$$>", updateAspx);
                    reader4.Close();
                }
                this.WriteFile(str22, str21);
            }
            if (File.Exists(str26))
            {
                using (StreamReader reader5 = new StreamReader(str26, Encoding.Default))
                {
                    string updateAspxCs = this.cb.GetUpdateAspxCs();
                    str21 = reader5.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ModifyAspxCs$$>", updateAspxCs);
                    reader5.Close();
                }
                this.WriteFile(str23, str21);
            }
            if (File.Exists(str27))
            {
                using (StreamReader reader6 = new StreamReader(str27, Encoding.Default))
                {
                    string updateDesigner = this.cb.GetUpdateDesigner();
                    str21 = reader6.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ModifyDesigner$$>", updateDesigner);
                    reader6.Close();
                }
                this.WriteFile(str24, str21);
            }
            str22 = str20 + @"\" + this.cb.ModelName + @"\Show.aspx";
            str23 = str20 + @"\" + this.cb.ModelName + @"\Show.aspx.cs";
            str24 = str20 + @"\" + this.cb.ModelName + @"\Show.aspx.designer.cs";
            str25 = Application.StartupPath + @"\Template\web\Show.aspx";
            str26 = Application.StartupPath + @"\Template\web\Show.aspx.cs";
            str27 = Application.StartupPath + @"\Template\web\Show.aspx.designer.cs";
            if (File.Exists(str25))
            {
                using (StreamReader reader7 = new StreamReader(str25, Encoding.Default))
                {
                    string showAspx = this.cb.GetShowAspx();
                    str21 = reader7.ReadToEnd().Replace(".Demo.Show", "." + this.cb.ModelName + ".Show").Replace("<$$ShowAspx$$>", showAspx);
                    reader7.Close();
                }
                this.WriteFile(str22, str21);
            }
            if (File.Exists(str26))
            {
                using (StreamReader reader8 = new StreamReader(str26, Encoding.Default))
                {
                    string showAspxCs = this.cb.GetShowAspxCs();
                    str21 = reader8.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ShowAspxCs$$>", showAspxCs);
                    reader8.Close();
                }
                this.WriteFile(str23, str21);
            }
            if (File.Exists(str27))
            {
                using (StreamReader reader9 = new StreamReader(str27, Encoding.Default))
                {
                    string showDesigner = this.cb.GetShowDesigner();
                    str21 = reader9.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ShowDesigner$$>", showDesigner);
                    reader9.Close();
                }
                this.WriteFile(str24, str21);
            }
        }

        private void CreatCsOne()
        {
            string str = this.txtNamespace.Text.Trim();
            string str2 = this.txtFolder.Text.Trim();
            if (str2.Trim() != "")
            {
                this.cb.NameSpace = str + "." + str2;
                this.cb.Folder = str2;
            }
            string strCode = this.cb.GetCodeFrameOne(this.GetDALType(), true, true, true, true, true, true, true);
            string text = this.txtTargetFolder.Text;
            this.FolderCheck(text);
            string folder = text + @"\Class";
            this.FolderCheck(folder);
            string filename = folder + @"\" + this.cb.ModelName + ".cs";
            this.WriteFile(filename, strCode);
        }

        private void CreatCsS3()
        {
            string text = this.txtTargetFolder.Text;
            this.FolderCheck(text);
            string folder = text + @"\Model";
            if (this.cb.Folder != "")
            {
                folder = folder + @"\" + this.cb.Folder;
            }
            this.FolderCheck(folder);
            string filename = folder + @"\" + this.cb.ModelName + ".cs";
            string strCode = this.cb.GetCodeFrameS3Model();
            this.WriteFile(filename, strCode);
            this.AddClassFile(folder + @"\Model.csproj", this.cb.ModelName + ".cs", "");
            string str5 = text + @"\DAL";
            if (this.cb.Folder != "")
            {
                str5 = str5 + @"\" + this.cb.Folder;
            }
            this.FolderCheck(str5);
            string str6 = str5 + @"\" + this.cb.ModelName + ".cs";
            string str7 = this.cb.GetCodeFrameS3DAL(this.GetDALType(), true, true, true, true, true, true, true);
            this.WriteFile(str6, str7);
            this.AddClassFile(str5 + @"\DAL.csproj", this.cb.ModelName + ".cs", "");
            string str8 = text + @"\BLL";
            if (this.cb.Folder != "")
            {
                str8 = str8 + @"\" + this.cb.Folder;
            }
            this.FolderCheck(str8);
            string str9 = str8 + @"\" + this.cb.ModelName + ".cs";
            string bLLType = this.GetBLLType();
            string str11 = this.cb.GetCodeFrameS3BLL(bLLType, true, true, true, true, true, true, true, true);
            this.WriteFile(str9, str11);
            this.AddClassFile(str8 + @"\BLL.csproj", this.cb.ModelName + ".cs", "");
            string str12 = text + @"\Web";
            if (this.cb.Folder != "")
            {
                str12 = str12 + @"\" + this.cb.Folder;
            }
            this.FolderCheck(str12);
            this.FolderCheck(str12 + @"\" + this.cb.ModelName);
            string str13 = "";
            string str14 = str12 + @"\" + this.cb.ModelName + @"\Add.aspx";
            string str15 = str12 + @"\" + this.cb.ModelName + @"\Add.aspx.cs";
            string str16 = str12 + @"\" + this.cb.ModelName + @"\Add.aspx.designer.cs";
            string path = Application.StartupPath + @"\Template\web\Add.aspx";
            string str18 = Application.StartupPath + @"\Template\web\Add.aspx.cs";
            string str19 = Application.StartupPath + @"\Template\web\Add.aspx.designer.cs";
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path, Encoding.Default))
                {
                    string addAspx = this.cb.GetAddAspx();
                    str13 = reader.ReadToEnd().Replace(".Demo.Add", "." + this.cb.ModelName + ".Add").Replace("<$$AddAspx$$>", addAspx);
                    reader.Close();
                }
                this.WriteFile(str14, str13);
            }
            if (File.Exists(str18))
            {
                using (StreamReader reader2 = new StreamReader(str18, Encoding.Default))
                {
                    string addAspxCs = this.cb.GetAddAspxCs();
                    str13 = reader2.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$AddAspxCs$$>", addAspxCs);
                    reader2.Close();
                }
                this.WriteFile(str15, str13);
            }
            if (File.Exists(str19))
            {
                using (StreamReader reader3 = new StreamReader(str19, Encoding.Default))
                {
                    string addDesigner = this.cb.GetAddDesigner();
                    str13 = reader3.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$AddDesigner$$>", addDesigner);
                    reader3.Close();
                }
                this.WriteFile(str16, str13);
            }
            str14 = str12 + @"\" + this.cb.ModelName + @"\Modify.aspx";
            str15 = str12 + @"\" + this.cb.ModelName + @"\Modify.aspx.cs";
            str16 = str12 + @"\" + this.cb.ModelName + @"\Modify.aspx.designer.cs";
            path = Application.StartupPath + @"\Template\web\Modify.aspx";
            str18 = Application.StartupPath + @"\Template\web\Modify.aspx.cs";
            str19 = Application.StartupPath + @"\Template\web\Modify.aspx.designer.cs";
            if (File.Exists(path))
            {
                using (StreamReader reader4 = new StreamReader(path, Encoding.Default))
                {
                    string updateAspx = this.cb.GetUpdateAspx();
                    str13 = reader4.ReadToEnd().Replace(".Demo.Modify", "." + this.cb.ModelName + ".Modify").Replace("<$$ModifyAspx$$>", updateAspx);
                    reader4.Close();
                }
                this.WriteFile(str14, str13);
            }
            if (File.Exists(str18))
            {
                using (StreamReader reader5 = new StreamReader(str18, Encoding.Default))
                {
                    string updateAspxCs = this.cb.GetUpdateAspxCs();
                    str13 = reader5.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ModifyAspxCs$$>", updateAspxCs);
                    reader5.Close();
                }
                this.WriteFile(str15, str13);
            }
            if (File.Exists(str19))
            {
                using (StreamReader reader6 = new StreamReader(str19, Encoding.Default))
                {
                    string updateDesigner = this.cb.GetUpdateDesigner();
                    str13 = reader6.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ModifyDesigner$$>", updateDesigner);
                    reader6.Close();
                }
                this.WriteFile(str16, str13);
            }
            str14 = str12 + @"\" + this.cb.ModelName + @"\Show.aspx";
            str15 = str12 + @"\" + this.cb.ModelName + @"\Show.aspx.cs";
            str16 = str12 + @"\" + this.cb.ModelName + @"\Show.aspx.designer.cs";
            path = Application.StartupPath + @"\Template\web\Show.aspx";
            str18 = Application.StartupPath + @"\Template\web\Show.aspx.cs";
            str19 = Application.StartupPath + @"\Template\web\Show.aspx.designer.cs";
            if (File.Exists(path))
            {
                using (StreamReader reader7 = new StreamReader(path, Encoding.Default))
                {
                    string showAspx = this.cb.GetShowAspx();
                    str13 = reader7.ReadToEnd().Replace(".Demo.Show", "." + this.cb.ModelName + ".Show").Replace("<$$ShowAspx$$>", showAspx);
                    reader7.Close();
                }
                this.WriteFile(str14, str13);
            }
            if (File.Exists(str18))
            {
                using (StreamReader reader8 = new StreamReader(str18, Encoding.Default))
                {
                    string showAspxCs = this.cb.GetShowAspxCs();
                    str13 = reader8.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ShowAspxCs$$>", showAspxCs);
                    reader8.Close();
                }
                this.WriteFile(str15, str13);
            }
            if (File.Exists(str19))
            {
                using (StreamReader reader9 = new StreamReader(str19, Encoding.Default))
                {
                    string showDesigner = this.cb.GetShowDesigner();
                    str13 = reader9.ReadToEnd().Replace(".Demo", "." + this.cb.ModelName).Replace("<$$ShowDesigner$$>", showDesigner);
                    reader9.Close();
                }
                this.WriteFile(str16, str13);
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

        private void FolderCheck(string Folder)
        {
            DirectoryInfo info = new DirectoryInfo(Folder);
            if (!info.Exists)
            {
                info.Create();
            }
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeExport));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "toolStrip1.BackgroundImage.gif");
            // 
            // CodeExport
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "CodeExport";
            this.ResumeLayout(false);

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
                this.btn_Export.Enabled = true;
            }
            else
            {
                this.btn_Del.Enabled = false;
                this.btn_Dellist.Enabled = false;
                this.btn_Export.Enabled = false;
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
            if (this.btn_Export.InvokeRequired)
            {
                SetBtnDisableCallback method = new SetBtnDisableCallback(this.SetBtnDisable);
                base.Invoke(method, null);
            }
            else
            {
                this.btn_Export.Enabled = false;
                this.btn_Cancle.Enabled = false;
            }
        }

        public void SetBtnEnable()
        {
            if (this.btn_Export.InvokeRequired)
            {
                SetBtnEnableCallback method = new SetBtnEnableCallback(this.SetBtnEnable);
                base.Invoke(method, null);
            }
            else
            {
                this.btn_Export.Enabled = true;
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
            this.SetBtnDisable();
            string str = this.txtNamespace.Text.Trim();
            string str2 = this.txtFolder.Text.Trim();
            int count = this.listTable2.Items.Count;
            this.SetprogressBar1Max(count);
            this.SetprogressBar1Val(1);
            this.SetlblStatuText("0");
            this.cb.DbName = this.dbname;
            if (str != "")
            {
                this.cb.NameSpace = str;
                this.setting.Namepace = str;
            }
            this.cb.Folder = str2;
            this.setting.Folder = str2;
            this.cb.DbHelperName = this.txtDbHelper.Text.Trim();
            this.cb.ProcPrefix = this.setting.ProcPrefix;
            this.setting.DbHelperName = this.txtDbHelper.Text.Trim();
            ModuleConfig.SaveSettings(this.setting);
            for (int i = 0; i < count; i++)
            {
                string tableName = this.listTable2.Items[i].ToString();
                this.cb.TableName = tableName;
                this.cb.ModelName = tableName;
                string str4 = this.txtTabNamepre.Text.Trim();
                if ((str4 != "") && tableName.StartsWith(str4))
                {
                    this.cb.ModelName = tableName.Substring(str4.Length);
                }
                DataTable keyName = this.dbobj.GetKeyName(this.dbname, tableName);
                List<LTP.CodeHelper.ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(this.dbname, tableName);
                this.cb.Fieldlist = columnInfoList;
                this.cb.Keys = CodeCommon.GetColumnInfos(keyName);
                this.CreatCS();
                this.SetprogressBar1Val(i + 1);
                this.SetlblStatuText((i + 1).ToString());
            }
            this.SetBtnEnable();
            MessageBox.Show(this, "文档生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void WriteFile(string Filename, string strCode)
        {
            StreamWriter writer = new StreamWriter(Filename, false, Encoding.Default);
            writer.Write(strCode);
            writer.Flush();
            writer.Close();
        }

        private delegate void SetBtnDisableCallback();

        private delegate void SetBtnEnableCallback();

        private delegate void SetlblStatuCallback(string text);

        private delegate void SetProBar1MaxCallback(int val);

        private delegate void SetProBar1ValCallback(int val);
    }
}


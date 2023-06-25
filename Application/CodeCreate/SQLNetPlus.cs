using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Runtime.InteropServices;
using System.Xml;

namespace CSharpWinApp
{

    public class Config
{
    // Fields
    private string sPath = "";
    private XmlDocument xDoc = null;

    // Methods
    public Config(string s_Path)
    {
        this.sPath = s_Path;
        this.xDoc = new XmlDocument();
        this.xDoc.Load(s_Path);
    }

    private XmlElement getElement(string elemName)
    {
        try
        {
            return (XmlElement) this.getXmlNode().SelectSingleNode("//add[@key='" + elemName + "']");
        }
        catch
        {
            return null;
        }
    }

    public string GetValue(string keyName)
    {
        XmlElement element = this.getElement(keyName);
        if (element != null)
        {
            return element.GetAttribute("value");
        }
        return "";
    }

    private XmlNode getXmlNode()
    {
        return this.xDoc.SelectSingleNode("//appSettings");
    }

    public bool SetValue(string keyName, string keyValue)
    {
        try
        {
            XmlNode node = this.getXmlNode();
            XmlElement element = this.getElement(keyName);
            if (element != null)
            {
                element.SetAttribute("value", keyValue);
                this.xDoc.Save(this.sPath);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public bool SetValue(string keyName, string keyValue, bool isCreate)
    {
        try
        {
            XmlNode node = this.getXmlNode();
            XmlElement element = this.getElement(keyName);
            if (element != null)
            {
                element.SetAttribute("value", keyValue);
            }
            else
            {
                XmlElement newChild = this.xDoc.CreateElement("add");
                newChild.SetAttribute("key", keyName);
                newChild.SetAttribute("value", keyValue);
                node.AppendChild(newChild);
            }
            this.xDoc.Save(this.sPath);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
    public class DataAccess
{
    // Fields
    private SqlConnection conn = null;
    private string strconn = "";

    // Methods
    public DataAccess(string configPath)
    {
        try
        {
            Config config = new Config(configPath);
            this.strconn = "server=" + config.GetValue("RemoteSQLServerUri") + ";uid=" + config.GetValue("RemoteSQLServerUser") + ";pwd=" + config.GetValue("RemoteSQLServerPWD") + ";database=" + config.GetValue("RemoteSQLServerDB");
            this.conn = new SqlConnection(this.strconn);
        }
        catch
        {
        }
    }

    public DataSet GetDataSet(string tablename)
    {
        if (this.conn == null)
        {
            return null;
        }
        DataSet dataSet = new DataSet();
        new SqlDataAdapter("select * from " + tablename, this.conn).Fill(dataSet);
        return dataSet;
    }

    public DataSet GetDataSet(string name, bool show)
    {
        DataSet set;
        if (this.conn == null)
        {
            return null;
        }
        string selectCommandText = "";
        if (!show)
        {
            selectCommandText = "select t1.name,t3.name as datatype,t1.isnullable,t1.length,t1.isoutparam from syscolumns t1 ";
            selectCommandText = (selectCommandText + " join systypes t3 on t1.xusertype=t3.xusertype " + " join sysobjects t4 on t1.id=t4.id ") + " where t4.name='" + name + "'";
            goto Label_00C3;
        }
        selectCommandText = "select [name] from sysobjects where ";
        switch (name)
        {
            case "table":
                selectCommandText = selectCommandText + "type='U'";
                goto Label_0085;

            case "view":
                selectCommandText = selectCommandText + "type='V'";
                break;

            case "proc":
                selectCommandText = selectCommandText + "type='P'";
                break;
        }
    Label_0085:
        selectCommandText = selectCommandText + " AND status >= 0 order by [name]";
    Label_00C3:
        set = new DataSet();
        new SqlDataAdapter(selectCommandText, this.conn).Fill(set);
        return set;
    }

    public string getTypeString(string typeName)
    {
        switch (typeName.ToLower())
        {
            case "bigint":
                return "System.Int32";

            case "bit":
                return "System.Boolean";

            case "char":
                return "System.String";

            case "datetime":
                return "System.DateTime";

            case "decimal":
                return "System.Decimal";

            case "float":
                return "System.Single";

            case "int":
                return "System.Int32";

            case "money":
                return "System.Decimal";

            case "nchar":
                return "System.String";

            case "ntext":
                return "System.String";

            case "numeric":
                return "System.Decimal";

            case "nvarchar":
                return "System.String";

            case "real":
                return "System.Single";

            case "smalldatetime":
                return "System.DateTime";

            case "smallint":
                return "System.Int32";

            case "smallmoney":
                return "System.Decimal";

            case "tinyint":
                return "System.Int32";

            case "varchar":
                return "System.String";
        }
        return typeName;
    }
}

 

 

	/// <summary>
	/// SQLNetPlus 的摘要说明。
	/// </summary>
	public class SQLNetPlus : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu SysMenu;
		private System.Windows.Forms.MenuItem MenuConfigDB;
		private System.Windows.Forms.MenuItem MenuConnectDB;
		private System.Windows.Forms.MenuItem MenuCloseFrm;
		private System.Windows.Forms.RadioButton rbtnTable;
		private System.Windows.Forms.RadioButton rbtnView;
		private System.Windows.Forms.RadioButton rbtnProc;
		private System.Windows.Forms.ListBox list;
		private System.Windows.Forms.TextBox txtCode;
		private System.Windows.Forms.Button btnBuildCode;
		private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.TextBox txtClassName;
        private IContainer components;

		#region 系统生成代码


        public SQLNetPlus()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.SysMenu = new System.Windows.Forms.MainMenu(this.components);
            this.MenuConfigDB = new System.Windows.Forms.MenuItem();
            this.MenuConnectDB = new System.Windows.Forms.MenuItem();
            this.MenuCloseFrm = new System.Windows.Forms.MenuItem();
            this.rbtnTable = new System.Windows.Forms.RadioButton();
            this.rbtnView = new System.Windows.Forms.RadioButton();
            this.rbtnProc = new System.Windows.Forms.RadioButton();
            this.list = new System.Windows.Forms.ListBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnBuildCode = new System.Windows.Forms.Button();
            this.lblClassName = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SysMenu
            // 
            this.SysMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuConfigDB,
            this.MenuConnectDB,
            this.MenuCloseFrm});
            // 
            // MenuConfigDB
            // 
            this.MenuConfigDB.Index = 0;
            this.MenuConfigDB.Text = "数据库配置";
            this.MenuConfigDB.Click += new System.EventHandler(this.MenuConfigDB_Click);
            // 
            // MenuConnectDB
            // 
            this.MenuConnectDB.Index = 1;
            this.MenuConnectDB.Text = "连接数据库";
            this.MenuConnectDB.Click += new System.EventHandler(this.MenuConnectDB_Click);
            // 
            // MenuCloseFrm
            // 
            this.MenuCloseFrm.Index = 3;
            this.MenuCloseFrm.Text = "退出系统";
            this.MenuCloseFrm.Click += new System.EventHandler(this.MenuCloseFrm_Click);
            // 
            // rbtnTable
            // 
            this.rbtnTable.Checked = true;
            this.rbtnTable.Location = new System.Drawing.Point(7, 5);
            this.rbtnTable.Name = "rbtnTable";
            this.rbtnTable.Size = new System.Drawing.Size(66, 22);
            this.rbtnTable.TabIndex = 1;
            this.rbtnTable.TabStop = true;
            this.rbtnTable.Text = "用户表";
            this.rbtnTable.CheckedChanged += new System.EventHandler(this.RadioClick);
            // 
            // rbtnView
            // 
            this.rbtnView.Location = new System.Drawing.Point(154, 5);
            this.rbtnView.Name = "rbtnView";
            this.rbtnView.Size = new System.Drawing.Size(68, 22);
            this.rbtnView.TabIndex = 2;
            this.rbtnView.Text = "视图";
            this.rbtnView.CheckedChanged += new System.EventHandler(this.RadioClick);
            // 
            // rbtnProc
            // 
            this.rbtnProc.Location = new System.Drawing.Point(67, 5);
            this.rbtnProc.Name = "rbtnProc";
            this.rbtnProc.Size = new System.Drawing.Size(87, 22);
            this.rbtnProc.TabIndex = 3;
            this.rbtnProc.Text = "存储过程";
            this.rbtnProc.CheckedChanged += new System.EventHandler(this.RadioClick);
            // 
            // list
            // 
            this.list.Location = new System.Drawing.Point(7, 30);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(160, 433);
            this.list.TabIndex = 4;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(173, 29);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCode.Size = new System.Drawing.Size(491, 434);
            this.txtCode.TabIndex = 5;
            // 
            // btnBuildCode
            // 
            this.btnBuildCode.Location = new System.Drawing.Point(536, 1);
            this.btnBuildCode.Name = "btnBuildCode";
            this.btnBuildCode.Size = new System.Drawing.Size(63, 21);
            this.btnBuildCode.TabIndex = 6;
            this.btnBuildCode.Text = "生成代码";
            this.btnBuildCode.Click += new System.EventHandler(this.btnBuildCode_Click);
            // 
            // lblClassName
            // 
            this.lblClassName.Location = new System.Drawing.Point(385, 7);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(40, 15);
            this.lblClassName.TabIndex = 7;
            this.lblClassName.Text = "类名：";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(431, 3);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(84, 20);
            this.txtClassName.TabIndex = 8;
            this.txtClassName.Text = "test";
            // 
            // SQLNetPlus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(684, 473);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.lblClassName);
            this.Controls.Add(this.btnBuildCode);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.list);
            this.Controls.Add(this.rbtnProc);
            this.Controls.Add(this.rbtnView);
            this.Controls.Add(this.rbtnTable);
            this.Menu = this.SysMenu;
            this.Name = "SQLNetPlus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entity Class Generator";
            this.Load += new System.EventHandler(this.SQLNetPlus_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new SQLNetPlus());
		}
	

		//数据库配置
		private void MenuConfigDB_Click(object sender, System.EventArgs e)
		{
			ConfigDB f = new ConfigDB();
			f.ShowDialog();
		}

		//退出系统
		private void MenuCloseFrm_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		//窗体加载事件
		private void SQLNetPlus_Load(object sender, System.EventArgs e)
		{
		}

		//连接数据库，在此把数据库表、存储过程、视图全部列出来
		private void MenuConnectDB_Click(object sender, System.EventArgs e)
		{
			RadioClick(null,null);
		}

		#region 用户表、存储过程、视图的转换显示
		//用户表、存储过程、视图的转换显示
		private void RadioClick(object sender, System.EventArgs e)
		{
			list.Items.Clear();
			string name = "table";
			if(rbtnTable.Checked == true)
				name = "table";
			else if(rbtnView.Checked == true)
				name = "view";
			else if(rbtnProc.Checked == true)
				name = "proc";
			
			DataSet ds = getDataSet(name);

			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				list.Items.Add(ds.Tables[0].Rows[i]["Name"]);
			}
		}

		private DataSet getDataSet(string name)
		{
			DataAccess da = getDataAccess();
			DataSet ds = da.GetDataSet(name,true);
			return ds;
		}

		private DataAccess getDataAccess()
		{
			string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
			string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
			string configPath = assemblyDirPath + "\\AppConfig.config";
			DataAccess da = new DataAccess(configPath);
			return da;
		}

		#endregion

		//生成代码按钮
		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnBuildCode_Click(object sender, System.EventArgs e)
		{
			if(txtClassName.Text == "")
			{
				MessageBox.Show("请输入类的名称");
				return;
			}
			string name = "table";
			if(rbtnTable.Checked == true)
				name = "table";
			else if(rbtnView.Checked == true)
				name = "view";
			else if(rbtnProc.Checked == true)
				name = "proc";
			string sql  = "";
			if(list.SelectedItem != null)
				GetGeneralCode(list.SelectedItem.ToString(),name);
		}

		private void GetGeneralCode(string selectName,string name)
		{
			CSharpCodeProvider provider = new CSharpCodeProvider();
			ICodeGenerator generator = provider.CreateGenerator();
			StreamWriter writer = new StreamWriter(@"c:\" + txtClassName.Text + ".cs",false);
			CodeCompileUnit unit = new CodeCompileUnit();
			CodeNamespace nspace = new CodeNamespace("SQLNetPlus"); //添加名称空间
			nspace.Imports.Add(new CodeNamespaceImport("System"));
			CodeTypeDeclaration info = new CodeTypeDeclaration(txtClassName.Text);
			
			CodeConstructor ct = new CodeConstructor();

			DataAccess da = getDataAccess();
			DataSet ds = da.GetDataSet(selectName,false);

			if(name != "proc")
			{
				ct.Attributes=MemberAttributes.Public;
				ct.Statements.Add(new CodeSnippetStatement("this.conStr=System.Configuration.ConfigurationSettings.AppSettings[\"DBConnStr\"];")); //内容
				ct.Statements.Add(new CodeSnippetStatement("if( this.conStr==null || this.conStr.Length==0)"));            //内容
				ct.Statements.Add(new CodeSnippetStatement("throw new Exception();"));

				info.Members.Add(ct);

				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					//私有成员变量，直接以字段名(包括s_)作变量名
					CodeMemberField f = new CodeMemberField();
					f.Name="s_" + ds.Tables[0].Rows[i]["Name"];
					f.Type = new CodeTypeReference((da.getTypeString(ds.Tables[0].Rows[i]["datatype"].ToString())));
					info.Members.Add(f);
					//公共属性，以字段名作属性名
					CodeMemberProperty p = new CodeMemberProperty();
					p.Name=ds.Tables[0].Rows[i]["Name"].ToString();
					p.Type=f.Type;
					p.Attributes = MemberAttributes.Public|MemberAttributes.Final;
					p.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),f.Name)));
					p.SetStatements.Add(new CodeAssignStatement(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),f.Name),
						new CodePropertySetValueReferenceExpression()));
					info.Members.Add(p);
				}
			}
			else
			{
				ct.Attributes=MemberAttributes.Public;
				info.Members.Add(ct);

				//添加方法
				CodeMemberMethod m = new CodeMemberMethod();
				m.ReturnType = new CodeTypeReference("System.Data.DataSet");
				m.Name = "ExecProc_" + selectName;
				m.Attributes = MemberAttributes.Public|MemberAttributes.Final;
				int rows = ds.Tables[0].Rows.Count;
				CodeSnippetStatement codebody = null;
				codebody = new CodeSnippetStatement("SqlParameter[] parms =new SqlParameter[" + (rows+1).ToString() + "];");
				m.Statements.Add(codebody);
				for(int i=0;i<rows;i++)
				{
					string paramName = ds.Tables[0].Rows[i]["name"].ToString(); 
					string paramName1 = "p_" + paramName.Substring(1,paramName.Length-1);
					CodeTypeReference paramType = new CodeTypeReference((da.getTypeString(ds.Tables[0].Rows[i]["datatype"].ToString())));
					CodeParameterDeclarationExpression parameters = null;
					parameters = new CodeParameterDeclarationExpression(paramType, paramName1);
					m.Parameters.Add(parameters);
					string paramOutput = ds.Tables[0].Rows[i]["isoutparam"].ToString();
					codebody = new CodeSnippetStatement("parms[" + i.ToString() + "]=new SqlParameter(\"" + paramName + "\", " + paramName1 + ");");
					m.Statements.Add(codebody); 
					if(paramOutput == "1")
					{
						codebody = new CodeSnippetStatement("parms[" + i.ToString() + "].Direction=ParameterDirection.Output;");
						m.Statements.Add(codebody);
					}
				}
				codebody = new CodeSnippetStatement("parms[" + rows.ToString() + "]=new SqlParameter(\"RETURN_VALUE\", SqlDbType.Int);");
				m.Statements.Add(codebody); 

				codebody = new CodeSnippetStatement("parms[" + rows.ToString() + "].Direction=ParameterDirection.ReturnValue;");
				m.Statements.Add(codebody); 

				codebody = new CodeSnippetStatement("return ExecuteDataset(\"" + selectName + "\",parms);");
				m.Statements.Add(codebody); 

				info.Members.Add(m);

			}
			nspace.Types.Add(info);
			unit.Namespaces.Add(nspace);                
			generator.GenerateCodeFromCompileUnit(unit,writer,new CodeGeneratorOptions());
			writer.Close();
			StreamReader sr = new StreamReader(@"c:\" + txtClassName.Text + ".cs",System.Text.Encoding.GetEncoding("GB2312"));
			txtCode.Text = sr.ReadToEnd();
			sr.Close();
		}

		//联系方式
		private void MenuContact_Click(object sender, System.EventArgs e)
		{
			//MessageBox.Show("我的联系方式，http://fineboy.cnblogs.com");
		}
	}
}

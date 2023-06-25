using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;

namespace CSharpWinApp
{
	/// <summary>
	/// ConfigDB 的摘要说明。
	/// </summary>
	public class ConfigDB : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblServerIP;
		private System.Windows.Forms.Label lblAccount;
		private System.Windows.Forms.TextBox txtServerIP;
		private System.Windows.Forms.TextBox txtAccount;
		private System.Windows.Forms.Label lblPwd;
		private System.Windows.Forms.TextBox txtAcctPwd;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblDBName;
		private System.Windows.Forms.TextBox txtDBName;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConfigDB()
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.lblDBName = new System.Windows.Forms.Label();
            this.txtAcctPwd = new System.Windows.Forms.TextBox();
            this.lblPwd = new System.Windows.Forms.Label();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.lblAccount = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDBName);
            this.groupBox1.Controls.Add(this.lblDBName);
            this.groupBox1.Controls.Add(this.txtAcctPwd);
            this.groupBox1.Controls.Add(this.lblPwd);
            this.groupBox1.Controls.Add(this.txtAccount);
            this.groupBox1.Controls.Add(this.lblAccount);
            this.groupBox1.Controls.Add(this.txtServerIP);
            this.groupBox1.Controls.Add(this.lblServerIP);
            this.groupBox1.Location = new System.Drawing.Point(7, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 141);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MSSQL Server服务器设置";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(73, 45);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(84, 20);
            this.txtDBName.TabIndex = 7;
            this.txtDBName.Text = "PUBS";
            // 
            // lblDBName
            // 
            this.lblDBName.Location = new System.Drawing.Point(7, 52);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(66, 15);
            this.lblDBName.TabIndex = 6;
            this.lblDBName.Text = "数据库名称：";
            // 
            // txtAcctPwd
            // 
            this.txtAcctPwd.Location = new System.Drawing.Point(73, 104);
            this.txtAcctPwd.Name = "txtAcctPwd";
            this.txtAcctPwd.Size = new System.Drawing.Size(84, 20);
            this.txtAcctPwd.TabIndex = 5;
            this.txtAcctPwd.Text = "123456";
            // 
            // lblPwd
            // 
            this.lblPwd.Location = new System.Drawing.Point(13, 111);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(60, 15);
            this.lblPwd.TabIndex = 4;
            this.lblPwd.Text = "登录密码：";
            // 
            // txtAccount
            // 
            this.txtAccount.Location = new System.Drawing.Point(73, 74);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(84, 20);
            this.txtAccount.TabIndex = 3;
            this.txtAccount.Text = "sa";
            // 
            // lblAccount
            // 
            this.lblAccount.Location = new System.Drawing.Point(13, 82);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(60, 15);
            this.lblAccount.TabIndex = 2;
            this.lblAccount.Text = "登录帐户：";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(135, 15);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(83, 20);
            this.txtServerIP.TabIndex = 1;
            this.txtServerIP.Text = "127.0.0.1";
            // 
            // lblServerIP
            // 
            this.lblServerIP.Location = new System.Drawing.Point(7, 22);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(133, 15);
            this.lblServerIP.TabIndex = 0;
            this.lblServerIP.Text = "SQL Server 服务器IP地址：";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(118, 171);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(46, 21);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(179, 171);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(46, 21);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "退出";
            // 
            // ConfigDB
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(254, 209);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置数据库服务器";
            this.Load += new System.EventHandler(this.ConfigDB_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		//确定配置
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			OpraterConfig(true);
		}

		private void ConfigDB_Load(object sender, System.EventArgs e)
		{
			OpraterConfig(false);
		}

		#region " 操作配置文件的方法 "
		private void OpraterConfig(bool isset)
		{
			try
			{
				string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
				string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
				string configFilePath = assemblyDirPath + "\\AppConfig.config";

    
				Config config = new Config(configFilePath);
				if(isset)
				{
					config.SetValue("RemoteSQLServerUri",txtServerIP.Text,true);
					config.SetValue("RemoteSQLServerUser",txtAccount.Text,true);
					config.SetValue("RemoteSQLServerPWD",txtAcctPwd.Text,true);
					config.SetValue("RemoteSQLServerDB",txtDBName.Text,true);
					MessageBox.Show("设置成功");
					btnOK.Enabled = false;
				}
				else
				{
					txtServerIP.Text = config.GetValue("RemoteSQLServerUri");
					txtAccount.Text = config.GetValue("RemoteSQLServerUser");
					txtAcctPwd.Text = config.GetValue("RemoteSQLServerPWD");
					txtDBName.Text = config.GetValue("RemoteSQLServerDB");
				}
			}
			catch
			{
				btnOK.Enabled = true;
			}
		}
		#endregion

	}
}

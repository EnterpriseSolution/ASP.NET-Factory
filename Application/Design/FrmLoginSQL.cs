using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.Schema;
using WebMatrix.Component;


namespace Flextronics.Applications.ApplicationFactory
{

    public class LoginForm : Form
    {
        //private Button btn_Cancel;
        //private Button btn_ConTest;
        //private Button btn_Ok;
        private Button btn_Cancel;
        private Button btn_ConTest;
        private Button btn_Ok;

        public CheckBox chk_Simple;
        public ComboBox cmbDBlist;
        public ComboBox comboBox_Verified;
        public ComboBox comboBoxServer;
        private IContainer components;
        public string constr;
        public string dbname = "master";
        private DbSettings dbobj = new DbSettings();
        public Label label1;
        public Label label2;
        public Label label3;
        private Label label4;
        public Label label6;
        private PictureBox pictureBox1;
        private ToolTip toolTip1;
        public TextBox txtPass;
        public TextBox txtUser;

        public LoginForm()
        {
            this.InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btn_ConTest_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.comboBoxServer.Text.Trim();
                string str2 = this.txtUser.Text.Trim();
                string str3 = this.txtPass.Text.Trim();
                if ((str2 == "") || (str == ""))
                {
                    MessageBox.Show(this, "服务器或用户名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    if (this.GetSelVerified() == "Windows")
                    {
                        this.constr = "Integrated Security=SSPI;Data Source=" + str + ";Initial Catalog=master";
                    }
                    else if (str3 == "")
                    {
                        this.constr = "user id=" + str2 + ";initial catalog=master;data source=" + str;
                    }
                    else
                    {
                        this.constr = "user id=" + str2 + ";password=" + str3 + ";initial catalog=master;data source=" + str;
                    }
                    try
                    {
                        this.Text = "正在连接服务器，请稍候...";
                        IDbObject obj2 = DBOMaker.CreateDbObj(this.GetSelVer());
                        obj2.DbConnectStr = this.constr;
                        List<string> dBList = obj2.GetDBList();
                        this.cmbDBlist.Enabled = true;
                        this.cmbDBlist.Items.Clear();
                        this.cmbDBlist.Items.Add("全部库");
                        if ((dBList != null) && (dBList.Count > 0))
                        {
                            foreach (string str5 in dBList)
                            {
                                this.cmbDBlist.Items.Add(str5);
                            }
                        }
                        this.cmbDBlist.SelectedIndex = 0;
                        this.Text = "连接服务器成功！";
                    }
                    catch (Exception exception)
                    {
                        Log.WriteLog(exception);
                        this.Text = "连接服务器或获取数据信息失败！";
                        string text = "连接服务器或获取数据信息失败！\r\n";
                        text = (text + "请检查服务器地址或用户名密码是否正确！\r\n") + "如果连接失败，服务器名可以用 “(local)”或是“.” 或者“机器名” 试一下！\r\n" + "如果需要查看帮助文件以帮助您解决问题，请点“确定”，否则点“取消”";
                        //if (MessageBox.Show(this, text, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) == DialogResult.OK)
                        //{
                        //    try
                        //    {
                        //        new Process();
                        //        Process.Start("IExplore.exe", "http://help.maticsoft.com");
                        //    }
                        //    catch
                        //    {
                        //        MessageBox.Show("请访问：http://www.maticsoft.com", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        //    }
                        //}
                    }
                }
            }
            catch (Exception exception2)
            {
                Log.WriteLog(exception2);
                //MessageBox.Show(this, exception2.Message, StringResources.Application, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                string str = this.comboBoxServer.Text.Trim();
                string str2 = this.txtUser.Text.Trim();
                string str3 = this.txtPass.Text.Trim();
                if ((str2 == "") || (str == ""))
                {
                    MessageBox.Show(this, "服务器或用户名不能为空！", "Application", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    if (this.cmbDBlist.SelectedIndex > 0)
                    {
                        this.dbname = this.cmbDBlist.Text;
                    }
                    else
                    {
                        this.dbname = "master";
                    }
                    if (this.GetSelVerified() == "Windows")
                    {
                        this.constr = "Integrated Security=SSPI;Data Source=" + str + ";Initial Catalog=" + this.dbname;
                    }
                    else if (str3 == "")
                    {
                        this.constr = "user id=" + str2 + ";initial catalog=" + this.dbname + ";data source=" + str;
                    }
                    else
                    {
                        this.constr = "user id=" + str2 + ";password=" + str3 + ";initial catalog=" + this.dbname + ";data source=" + str;
                    }
                    constr = constr + "; Connection Timeout=10";
                    SqlConnection connection = new SqlConnection(this.constr);
                    //connection.ConnectionTimeout = 10; Connection Timeout=30
                    try
                    {
                        this.Text = "正在连接服务器，请稍候...";
                        connection.Open();
                    }
                    catch (Exception exception)
                    {
                        this.Text = "连接服务器失败！";
                        Log.WriteLog(exception);
                        MessageBox.Show(this, "连接服务器失败！请检查服务器地址或用户名密码是否正确！", "Application", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    this.Text = "连接服务器成功！";
                    if (this.dbobj == null)
                    {
                        this.dbobj = new DbSettings();
                    }
                    string selVer = this.GetSelVer();
                    this.dbobj.DbType = selVer;
                    this.dbobj.Server = str;
                    this.dbobj.ConnectStr = this.constr;
                    this.dbobj.DbName = this.dbname;
                    this.dbobj.ConnectSimple = this.chk_Simple.Checked;
                    if (!DbConfig.AddSettings(this.dbobj))
                    {
                        MessageBox.Show(this, "该服务器已经存在！请更换服务器地址或检查输入是否正确！", "Application", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show(this, exception2.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Log.WriteLog(exception2);
            }
        }

        private void comboBox_Verified_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.GetSelVerified() == "Windows")
            {
                this.label2.Enabled = false;
                this.label3.Enabled = false;
                this.txtUser.Enabled = false;
                this.txtPass.Enabled = false;
            }
            else
            {
                this.label2.Enabled = true;
                this.label3.Enabled = true;
                this.txtUser.Enabled = true;
                this.txtPass.Enabled = true;
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

        public string GetSelVer()
        {
            //直接获取为SQL 2000的版本，通用性强
            //switch (this.comboBoxServerVer.Text)
            //{
            //    case "SQL Server2000":
            //        return "SQL2000";

            //    case "SQL Server2005":
            //        return "SQL2005";
            //}
            //return "SQL2005";
            return "SQL2000";
        }

        public string GetSelVerified()
        {
            //if (this.comboBox_Verified.SelectedItem.ToString() == "Windows Authentication")
            //{
            //    return "Windows";
            //}
            //return "SQL";
            if(comboBox_Verified.SelectedIndex==0)
                return "Windows";
            return "SQL";
        }

        private string GetSQLVer(string connectionString)
        {
            string str3;
            string cmdText = "select serverproperty('productversion')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    try
                    {
                        connection.Open();
                        object objA = command.ExecuteScalar();
                        if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
                        {
                            return "";
                        }
                        string str2 = objA.ToString().Trim();
                        if (str2.Length > 1)
                        {
                            return str2.Substring(0, 1);
                        }
                        str3 = "";
                    }
                    catch (SqlException exception)
                    {
                        connection.Close();
                        Log.WriteLog(exception);
                        throw exception;
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }
            }
            return str3;
        }

        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDBlist = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chk_Simple = new System.Windows.Forms.CheckBox();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_ConTest = new System.Windows.Forms.Button();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_Verified = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbDBlist
            // 
            this.cmbDBlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBlist.Enabled = false;
            this.cmbDBlist.Location = new System.Drawing.Point(120, 211);
            this.cmbDBlist.Name = "cmbDBlist";
            this.cmbDBlist.Size = new System.Drawing.Size(256, 21);
            this.cmbDBlist.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Database：";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(152, 154);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(224, 20);
            this.txtUser.TabIndex = 3;
            this.txtUser.Text = "sa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User name：";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(152, 184);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(224, 20);
            this.txtPass.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(79, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password：";
            // 
            // chk_Simple
            // 
            this.chk_Simple.AutoSize = true;
            this.chk_Simple.Checked = true;
            this.chk_Simple.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Simple.Location = new System.Drawing.Point(120, 243);
            this.chk_Simple.Name = "chk_Simple";
            this.chk_Simple.Size = new System.Drawing.Size(98, 17);
            this.chk_Simple.TabIndex = 22;
            this.chk_Simple.Text = "高效连接模式";
            this.chk_Simple.UseVisualStyleBackColor = true;
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btn_Ok.Location = new System.Drawing.Point(225, 277);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(66, 26);
            this.btn_Ok.TabIndex = 19;
            this.btn_Ok.Text = "OK";
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(309, 277);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(67, 26);
            this.btn_Cancel.TabIndex = 20;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_ConTest
            // 
            this.btn_ConTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.btn_ConTest.Location = new System.Drawing.Point(120, 277);
            this.btn_ConTest.Name = "btn_ConTest";
            this.btn_ConTest.Size = new System.Drawing.Size(80, 26);
            this.btn_ConTest.TabIndex = 19;
            this.btn_ConTest.Text = "Connect";
            this.btn_ConTest.UseVisualStyleBackColor = false;
            this.btn_ConTest.Click += new System.EventHandler(this.btn_ConTest_Click);
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Location = new System.Drawing.Point(120, 90);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(256, 21);
            this.comboBoxServer.TabIndex = 21;
            this.comboBoxServer.Text = "127.0.0.1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Authentication：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox_Verified
            // 
            this.comboBox_Verified.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Verified.FormattingEnabled = true;
            this.comboBox_Verified.Items.AddRange(new object[] {
            "Windows  Authentication",
            "SQL Server Authentication"});
            this.comboBox_Verified.Location = new System.Drawing.Point(120, 124);
            this.comboBox_Verified.Name = "comboBox_Verified";
            this.comboBox_Verified.Size = new System.Drawing.Size(256, 21);
            this.comboBox_Verified.TabIndex = 21;
            this.comboBox_Verified.SelectedIndexChanged += new System.EventHandler(this.comboBox_Verified_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(396, 82);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btn_Ok;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(396, 318);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.chk_Simple);
            this.Controls.Add(this.comboBox_Verified);
            this.Controls.Add(this.comboBoxServer);
            this.Controls.Add(this.cmbDBlist);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.btn_ConTest);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Server";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //this.toolTip1.SetToolTip(this.txtUser, "请保证该用户具有每个数据库的访问权！");
           // this.comboBoxServerVer.SelectedIndex = 0;
          //  this.comboBox_Verified.SelectedIndex = 0;

            comboBox_Verified.SelectedIndex = 0;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}


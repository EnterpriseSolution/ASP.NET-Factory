namespace Flextronics.Application.ApplicationFactory
{
    using Flextronics.Application.ApplicationFactory.UserControls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class OptionFrm : Form
    {
        private AppSettings appsettings;
        private Button btn_Cancel;
        private Button btn_Ok;
        private IContainer components;
        private GroupBox groupBox1;
        private MainForm mainForm;
        private UcOptionEditor optionEditor;
        private UcOptionsEnviroments optionsEnviroments;
        private UcOptionsQuerySettings optionsQuerySettings;
        private UcOptionStartUp optionStartUp;
        private ModuleSettings setting;
        private TreeView treeView1;
        private UcAddInManage ucAddin;
        private UcCSSet uccsset;
        private UcDatatype ucDatatype;
        private UcSysManage ucSysmanage;
        private Panel UserControlContainer;

        public OptionFrm(MainForm mainform)
        {
            this.InitializeComponent();
            this.mainForm = mainform;
            this.appsettings = AppConfig.GetSettings();
            this.setting = ModuleConfig.GetSettings();
            this.optionsEnviroments = new UcOptionsEnviroments();
            this.optionEditor = new UcOptionEditor();
            this.optionsQuerySettings = new UcOptionsQuerySettings();
            this.optionStartUp = new UcOptionStartUp(this.appsettings);
            this.uccsset = new UcCSSet();
            this.ucAddin = new UcAddInManage();
            this.ucDatatype = new UcDatatype();
            this.ucSysmanage = new UcSysManage();
        }

        private void ActivateOptionControl(UserControl optionControl)
        {
            foreach (UserControl control in this.UserControlContainer.Controls)
            {
                control.Hide();
            }
            optionControl.Show();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            switch (this.optionStartUp.cmb_StartUpItem.SelectedIndex)
            {
                case 0:
                    this.appsettings.AppStart = "startuppage";
                    this.appsettings.StartUpPage = this.optionStartUp.txtStartUpPage.Text;
                    break;

                case 1:
                    this.appsettings.AppStart = "blank";
                    break;

                case 2:
                    this.appsettings.AppStart = "homepage";
                    this.appsettings.HomePage = this.optionStartUp.txtStartUpPage.Text;
                    break;
            }
            AppConfig.SaveSettings(this.appsettings);
            if (this.uccsset.radbtn_Frame_One.Checked)
            {
                this.setting.AppFrame = "One";
            }
            if (this.uccsset.radbtn_Frame_S3.Checked)
            {
                this.setting.AppFrame = "S3";
            }
            if (this.uccsset.radbtn_Frame_F3.Checked)
            {
                this.setting.AppFrame = "F3";
            }
            this.setting.DALType = this.uccsset.GetDALType();
            this.setting.Namepace = this.uccsset.txtNamepace.Text.Trim();
            this.setting.DbHelperName = this.uccsset.txtDbHelperName.Text.Trim();
            this.setting.ProjectName = this.uccsset.txtProjectName.Text.Trim();
            this.setting.ProcPrefix = this.uccsset.txtProcPrefix.Text.Trim();
            ModuleConfig.SaveSettings(this.setting);
            this.ucDatatype.SaveIni();
            this.ucSysmanage.SaveDBO();
            base.Close();
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
            this.treeView1 = new TreeView();
            this.groupBox1 = new GroupBox();
            this.btn_Cancel = new Button();
            this.btn_Ok = new Button();
            this.UserControlContainer = new Panel();
            base.SuspendLayout();
            this.treeView1.Location = new Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new Size(0xca, 0x114);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
            this.groupBox1.Location = new Point(0xe9, 0x116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x180, 4);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.btn_Cancel.Location = new Point(0x21e, 0x125);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(0x4b, 0x17);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.btn_Ok.Location = new Point(0x1b0, 0x125);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new Size(0x4b, 0x17);
            this.btn_Ok.TabIndex = 3;
            this.btn_Ok.Text = "确定";
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new EventHandler(this.btn_Ok_Click);
            this.UserControlContainer.Location = new Point(0xe9, 12);
            this.UserControlContainer.Name = "UserControlContainer";
            this.UserControlContainer.Size = new Size(0x180, 260);
            this.UserControlContainer.TabIndex = 4;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
//            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x279, 0x148);
            base.Controls.Add(this.UserControlContainer);
            base.Controls.Add(this.btn_Ok);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.treeView1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "OptionFrm";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "选项";
            base.Load += new EventHandler(this.OptionFrm_Load);
            base.ResumeLayout(false);
        }

        private void InitTreeView()
        {
            TreeNode node = new TreeNode("环境", 0, 1);
            TreeNode node2 = new TreeNode("代码参数", 0, 1);
            TreeNode node3 = new TreeNode("组件管理", 0, 1);
            TreeNode node4 = new TreeNode("系统管理", 0, 1);
            new TreeNode("编辑", 2, 3);
            new TreeNode("设置", 2, 3);
            TreeNode node5 = new TreeNode("启动", 2, 3);
            node.Nodes.Add(node5);
            new TreeNode("数据库脚本", 2, 3);
            TreeNode node6 = new TreeNode("代码生成", 2, 3);
            new TreeNode("Web页面", 2, 3);
            TreeNode node7 = new TreeNode("字段类型映射", 2, 3);
            node2.Nodes.Add(node6);
            node2.Nodes.Add(node7);
            this.treeView1.Nodes.Add(node);
            this.treeView1.Nodes.Add(node2);
            this.treeView1.Nodes.Add(node3);
            this.treeView1.Nodes.Add(node4);
            node.Expand();
            node2.Expand();
            this.UserControlContainer.Controls.Add(this.optionsEnviroments);
            this.UserControlContainer.Controls.Add(this.optionEditor);
            this.UserControlContainer.Controls.Add(this.optionsQuerySettings);
            this.UserControlContainer.Controls.Add(this.optionStartUp);
            this.UserControlContainer.Controls.Add(this.uccsset);
            this.UserControlContainer.Controls.Add(this.ucAddin);
            this.UserControlContainer.Controls.Add(this.ucDatatype);
            this.UserControlContainer.Controls.Add(this.ucSysmanage);
            this.ActivateOptionControl(this.optionsEnviroments);
        }

        private void OptionFrm_Load(object sender, EventArgs e)
        {
            this.InitTreeView();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode != null)
            {
                switch (selectedNode.Text)
                {
                    case "环境":
                        this.ActivateOptionControl(this.optionsEnviroments);
                        return;

                    case "编辑":
                        this.ActivateOptionControl(this.optionEditor);
                        return;

                    case "设置":
                        this.ActivateOptionControl(this.optionsQuerySettings);
                        return;

                    case "启动":
                        this.ActivateOptionControl(this.optionStartUp);
                        return;

                    case "代码参数":
                    case "代码生成":
                        this.ActivateOptionControl(this.uccsset);
                        return;

                    case "字段类型映射":
                        this.ActivateOptionControl(this.ucDatatype);
                        return;

                    case "组件管理":
                    case "DAL代码插件":
                        this.ActivateOptionControl(this.ucAddin);
                        return;

                    case "系统管理":
                        this.ActivateOptionControl(this.ucSysmanage);
                        return;

                    default:
                        return;
                }
            }
        }
    }
}


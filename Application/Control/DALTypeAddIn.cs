using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Flextronics.Applications.Library.PlugIn;

namespace Flextronics.Applications.ApplicationFactory.UserControls
{ 

    public class DALTypeAddIn : UserControl
    {
        private string _guid;
        private string _name;
        //private ComboBox cmbox_DALType;
        private IContainer components;
        private Label lblTip;
        private ComboBox cmbox_DALType;
        private Label lblTitle;

        public DALTypeAddIn()
        {
            this._name = "";
            this._guid = "";
            this.InitializeComponent();
            //TODO 修改Plun In的方式
            //DataSet addInList = new AddIn().GetAddInList();
            //if (addInList != null)
            //{
            //    int count = addInList.Tables[0].Rows.Count;
            //    if (count > 0)
            //    {
            //        this.cmbox_DALType.DataSource = addInList.Tables[0].DefaultView;
            //        this.cmbox_DALType.ValueMember = "Guid";
            //        this.cmbox_DALType.DisplayMember = "Name";
            //    }
            //    if (count == 1)
            //    {
            //        this._guid = addInList.Tables[0].Rows[0]["Guid"].ToString();
            //        this._name = addInList.Tables[0].Rows[0]["Name"].ToString();
            //    }
            //}
            BindCombox();
            //end
        }

        public DALTypeAddIn(string InterfaceName)
        {
            this._name = "";
            this._guid = "";
            this.InitializeComponent();
            //DataSet addInList = new AddIn().GetAddInList(InterfaceName);
            //if (addInList != null)
            //{
            //    int count = addInList.Tables[0].Rows.Count;
            //    if (count > 0)
            //    {
            //        this.cmbox_DALType.DataSource = addInList.Tables[0].DefaultView;
            //        this.cmbox_DALType.ValueMember = "Guid";
            //        this.cmbox_DALType.DisplayMember = "Name";
            //    }
            //    if (count == 1)
            //    {
            //        this._guid = addInList.Tables[0].Rows[0]["Guid"].ToString();
            //        this._name = addInList.Tables[0].Rows[0]["Name"].ToString();
            //    }
            //}
            BindCombox();
        }

        void BindCombox()
        {
            PlugInEntityCollection list = PlugInManager.BuildPlugIn();
            if (list != null)
            {
                cmbox_DALType.DataSource = list;
                cmbox_DALType.ValueMember = "GUID";
                cmbox_DALType.DisplayMember = "Decription";

                _guid = list[0].GUID;
                _name = list[0].Name;
            }
        }

        private void cmbox_DALType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbox_DALType.SelectedItem != null)
            {
                PlugInEntity item = cmbox_DALType.SelectedItem as PlugInEntity;
                //this._name = cmbox_DALType.Text;
                //this._guid = cmbox_DALType.SelectedValue.ToString();
                if (item != null)
                {
                    _name = item.Name;
                    _guid = item.GUID;
                    AddIn @in = new AddIn(_guid);
                    this.lblTip.Text = @in.Decription;
                }
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

        private void InitializeComponent()
        {
            this.lblTip = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.cmbox_DALType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(257, 4);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(103, 13);
            this.lblTip.TabIndex = 1;
            this.lblTip.Text = "请先选择代码类型";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(0, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(28, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "DAL";
            // 
            // cmbox_DALType
            // 
            this.cmbox_DALType.FormattingEnabled = true;
            this.cmbox_DALType.Location = new System.Drawing.Point(43, 1);
            this.cmbox_DALType.Name = "cmbox_DALType";
            this.cmbox_DALType.Size = new System.Drawing.Size(188, 21);
            this.cmbox_DALType.TabIndex = 3;
            this.cmbox_DALType.SelectedIndexChanged += new System.EventHandler(this.cmbox_DALType_SelectedIndexChanged);
            // 
            // DALTypeAddIn
            // 
            this.Controls.Add(this.cmbox_DALType);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTip);
            this.Name = "DALTypeAddIn";
            this.Size = new System.Drawing.Size(375, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void SetSelectedDALType(string appguid)
        {
            //for (int i = 0; i < this.cmbox_DALType.Items.Count; i++)
            //{
            //    DataRow row = (this.cmbox_DALType.Items[i] as DataRowView).Row;
            //    if (row[this.cmbox_DALType.ValueMember].ToString() == appguid)
            //    {
            //        this.cmbox_DALType.SelectedIndex = i;
            //        this._guid = appguid;
            //        this._name = row[this.cmbox_DALType.DisplayMember].ToString();
            //        AddIn @in = new AddIn(this._guid);
            //        this.lblTip.Text = @in.Decription;
            //        return;
            //    }
            //}
            for (int i = 0; i < this.cmbox_DALType.Items.Count; i++)
            {
                PlugInEntity row = this.cmbox_DALType.Items[i] as PlugInEntity;
                if(appguid==row.GUID)
                {
                    cmbox_DALType.SelectedIndex = i;
                    _guid = row.GUID;
                    _name = row.Name;
                    AddIn @in = new AddIn(_guid);
                    this.lblTip.Text = @in.Decription;
                    return;
                }               
            }
        }

        public string AppGuid
        {
            get
            {
                return this._guid;
            }
            set
            {
                this._guid = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string Title
        {
            get
            {
                return this.lblTitle.Text;
            }
            set
            {
                this.lblTitle.Text = value;
            }
        }
    }
}


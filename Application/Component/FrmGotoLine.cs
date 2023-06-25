namespace Codematic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmGotoLine : Form
    {
        private DbQuery _frmQuery;
        private Button btnCancel;
        private Button btnOk;
        private Container components;
        private Label lblText;
        private TextBox txtLine;

        public FrmGotoLine(DbQuery frmQuery, int currentPosition, int lastPosition)
        {
            this.InitializeComponent();
            this._frmQuery = frmQuery;
            this.lblText.Text = "Line number(1-" + lastPosition.ToString() + ")";
            this.txtLine.Text = currentPosition.ToString();
            this.txtLine.SelectAll();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int line = Convert.ToInt32(this.txtLine.Text);
            this._frmQuery.GoToLine(line);
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
            this.lblText = new Label();
            this.txtLine = new TextBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.lblText.AutoSize = true;
            this.lblText.Location = new Point(10, 11);
            this.lblText.Name = "lblText";
            this.lblText.Size = new Size(0x41, 12);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "转到行号：";
            this.txtLine.Location = new Point(10, 30);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new Size(0xb6, 0x15);
            this.txtLine.TabIndex = 1;
            this.btnOk.FlatStyle = FlatStyle.System;
            this.btnOk.Location = new Point(0x23, 0x3d);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x4b, 0x19);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatStyle = FlatStyle.System;
            this.btnCancel.Location = new Point(0x74, 0x3d);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x19);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            base.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xcb, 0x60);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.txtLine);
            base.Controls.Add(this.lblText);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FrmGotoLine";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "跳到指定行";
            base.TopMost = true;
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}


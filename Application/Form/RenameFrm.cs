namespace Codematic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    

    public class RenameFrm : Form
    {
        private Button btn_Cancel;
        private Button btn_OK;
        private Container components;
        private Label label1;
        public TextBox txtName;

        public RenameFrm()
        {
            this.InitializeComponent();
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
            this.btn_OK = new Button();
            this.btn_Cancel = new Button();
            this.txtName = new TextBox();
            this.label1 = new Label();
            base.SuspendLayout();
         //   this.btn_OK._Image = null;
            this.btn_OK.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
        //    this.btn_OK.DefaultScheme = false;
            this.btn_OK.DialogResult = DialogResult.OK;
            this.btn_OK.Image = null;
            this.btn_OK.Location = new Point(70, 0x37);
            this.btn_OK.Name = "btn_OK";
         //   this.btn_OK.Scheme = Button.Schemes.Blue;
            this.btn_OK.Size = new Size(0x41, 0x19);
            this.btn_OK.TabIndex = 50;
            this.btn_OK.Text = "确 定";
         //   this.btn_Cancel._Image = null;
            this.btn_Cancel.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
       //     this.btn_Cancel.DefaultScheme = false;
            this.btn_Cancel.DialogResult = DialogResult.Cancel;
            this.btn_Cancel.Image = null;
            this.btn_Cancel.Location = new Point(0xa6, 0x37);
            this.btn_Cancel.Name = "btn_Cancel";
         //   this.btn_Cancel.Scheme = Button.Schemes.Blue;
            this.btn_Cancel.Size = new Size(0x41, 0x19);
            this.btn_Cancel.TabIndex = 0x31;
            this.btn_Cancel.Text = "取 消";
            this.txtName.Location = new Point(110, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(0x9a, 0x15);
            this.txtName.TabIndex = 0x30;
            this.txtName.Text = "";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(14, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4f, 0x11);
            this.label1.TabIndex = 0x2f;
            this.label1.Text = "输入新名称：";
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x124, 0x5f);
            base.Controls.Add(this.btn_OK);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.txtName);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "RenameFrm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "新名称";
            base.ResumeLayout(false);
        }
    }
}


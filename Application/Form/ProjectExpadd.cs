namespace Codematic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    

    public class ProjectExpadd : Form
    {
        private Button btn_Cancel;
        private Button btn_OK;
        private Container components;
        private Label label1;
        public TextBox textBox1;

        public ProjectExpadd()
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
            this.label1 = new Label();
            this.textBox1 = new TextBox();
            this.btn_OK = new Button();
            this.btn_Cancel = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5b, 0x11);
            this.label1.TabIndex = 0;
            this.label1.Text = "添加文件类型：";
            this.textBox1.Location = new Point(0x68, 0x10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xa8, 0x15);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "";
        //    this.btn_OK._Image = null;
            this.btn_OK.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
       //     this.btn_OK.DefaultScheme = false;
            this.btn_OK.DialogResult = DialogResult.OK;
            this.btn_OK.Image = null;
            this.btn_OK.Location = new Point(0x40, 0x38);
            this.btn_OK.Name = "btn_OK";
        //    this.btn_OK.Scheme = Button.Schemes.Blue;
            this.btn_OK.Size = new Size(0x41, 0x19);
            this.btn_OK.TabIndex = 0x2e;
            this.btn_OK.Text = "确 定";
          //  this.btn_Cancel._Image = null;
            this.btn_Cancel.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
         //   this.btn_Cancel.DefaultScheme = false;
            this.btn_Cancel.DialogResult = DialogResult.Cancel;
            this.btn_Cancel.Image = null;
            this.btn_Cancel.Location = new Point(160, 0x38);
            this.btn_Cancel.Name = "btn_Cancel";
        //    this.btn_Cancel.Scheme = Button.Schemes.Blue;
            this.btn_Cancel.Size = new Size(0x41, 0x19);
            this.btn_Cancel.TabIndex = 0x2d;
            this.btn_Cancel.Text = "取 消";
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x124, 0x5d);
            base.Controls.Add(this.btn_OK);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ProjectExpadd";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "添加文件类型";
            base.ResumeLayout(false);
        }
    }
}


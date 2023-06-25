namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UcOptionsQuerySettings : UserControl
    {
        public CheckBox chbReadOnlyOutput;
        public CheckBox chbRunWithIOStat;
        public CheckBox chbShowCommentHeader;
        private CheckBox checkBox1;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtDiffPercent;

        public UcOptionsQuerySettings()
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
            this.chbShowCommentHeader = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.txtDiffPercent = new TextBox();
            this.label3 = new Label();
            this.label1 = new Label();
            this.label2 = new Label();
            this.checkBox1 = new CheckBox();
            this.chbRunWithIOStat = new CheckBox();
            this.chbReadOnlyOutput = new CheckBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.chbShowCommentHeader.FlatStyle = FlatStyle.System;
            this.chbShowCommentHeader.Location = new Point(8, 0x38);
            this.chbShowCommentHeader.Name = "chbShowCommentHeader";
            this.chbShowCommentHeader.Size = new Size(280, 0x10);
            this.chbShowCommentHeader.TabIndex = 7;
            this.chbShowCommentHeader.Text = "Show document header window";
            this.groupBox1.Controls.Add(this.txtDiffPercent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Enabled = false;
            this.groupBox1.FlatStyle = FlatStyle.System;
            this.groupBox1.Location = new Point(8, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x138, 0xa8);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information message";
            this.txtDiffPercent.Location = new Point(0x90, 0x38);
            this.txtDiffPercent.Name = "txtDiffPercent";
            this.txtDiffPercent.Size = new Size(0x20, 0x15);
            this.txtDiffPercent.TabIndex = 5;
            this.txtDiffPercent.Text = "101";
            this.label3.Location = new Point(0x10, 0x38);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x80, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "Differencial percentage:";
            this.label1.Location = new Point(0x10, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(280, 0x20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enabling this option will inform the user about ineffective query plans. ";
            this.label2.Location = new Point(0x10, 0x58);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x108, 0x48);
            this.label2.TabIndex = 3;
            this.label2.Text = "Setting the differential percentage will affect when the alarm is given. 101% (default) means the alarm will be set off when the table scan exceed the rowcount. ";
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.FlatStyle = FlatStyle.System;
            this.checkBox1.Location = new Point(0x157, 120);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x121, 0x10);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Use upper case on reserved words recognizion";
            this.chbRunWithIOStat.FlatStyle = FlatStyle.System;
            this.chbRunWithIOStat.Location = new Point(8, 30);
            this.chbRunWithIOStat.Name = "chbRunWithIOStat";
            this.chbRunWithIOStat.Size = new Size(0xe2, 0x10);
            this.chbRunWithIOStat.TabIndex = 5;
            this.chbRunWithIOStat.Text = "Run query with IO statistics";
            this.chbReadOnlyOutput.AutoSize = true;
            this.chbReadOnlyOutput.FlatStyle = FlatStyle.System;
            this.chbReadOnlyOutput.Location = new Point(8, 8);
            this.chbReadOnlyOutput.Name = "chbReadOnlyOutput";
            this.chbReadOnlyOutput.Size = new Size(0x72, 0x11);
            this.chbReadOnlyOutput.TabIndex = 8;
            this.chbReadOnlyOutput.Text = "自动检测新版本";
            base.Controls.Add(this.chbReadOnlyOutput);
            base.Controls.Add(this.chbShowCommentHeader);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.chbRunWithIOStat);
            base.Name = "UcOptionsQuerySettings";
            base.Size = new Size(720, 0x270);
            base.Load += new EventHandler(this.UcOptionsQuerySettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void UcOptionsQuerySettings_Load(object sender, EventArgs e)
        {
        }
    }
}


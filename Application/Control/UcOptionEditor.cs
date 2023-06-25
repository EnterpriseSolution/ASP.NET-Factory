namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UcOptionEditor : UserControl
    {
        private Button btnSelectFont;
        public CheckBox chbShowEOLMarkers;
        public CheckBox chbShowLineNumbers;
        public CheckBox chbShowMatchingBrackets;
        public CheckBox chbShowSpaces;
        public CheckBox chbShowTabs;
        private Container components;
        public Font font;
        private GroupBox groupBox1;
        private Label label1;
        private Label lblFont;

        public UcOptionEditor()
        {
            this.InitializeComponent();
        }

        private void btnSelectFont_Click(object sender, EventArgs e)
        {
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
            this.chbShowLineNumbers = new CheckBox();
            this.chbShowEOLMarkers = new CheckBox();
            this.chbShowMatchingBrackets = new CheckBox();
            this.chbShowSpaces = new CheckBox();
            this.chbShowTabs = new CheckBox();
            this.label1 = new Label();
            this.lblFont = new Label();
            this.btnSelectFont = new Button();
            this.groupBox1 = new GroupBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.chbShowLineNumbers.CheckState = CheckState.Checked;
            this.chbShowLineNumbers.FlatStyle = FlatStyle.System;
            this.chbShowLineNumbers.Location = new Point(8, 8);
            this.chbShowLineNumbers.Name = "chbShowLineNumbers";
            this.chbShowLineNumbers.Size = new Size(0xf8, 0x10);
            this.chbShowLineNumbers.TabIndex = 0;
            this.chbShowLineNumbers.Text = "Show line numbers";
            this.chbShowEOLMarkers.FlatStyle = FlatStyle.System;
            this.chbShowEOLMarkers.Location = new Point(8, 0x20);
            this.chbShowEOLMarkers.Name = "chbShowEOLMarkers";
            this.chbShowEOLMarkers.Size = new Size(0xf8, 0x10);
            this.chbShowEOLMarkers.TabIndex = 1;
            this.chbShowEOLMarkers.Text = "Show EOL markers";
            this.chbShowMatchingBrackets.FlatStyle = FlatStyle.System;
            this.chbShowMatchingBrackets.Location = new Point(8, 0x38);
            this.chbShowMatchingBrackets.Name = "chbShowMatchingBrackets";
            this.chbShowMatchingBrackets.Size = new Size(0xf8, 0x10);
            this.chbShowMatchingBrackets.TabIndex = 2;
            this.chbShowMatchingBrackets.Text = "Show matching brackets";
            this.chbShowSpaces.FlatStyle = FlatStyle.System;
            this.chbShowSpaces.Location = new Point(8, 80);
            this.chbShowSpaces.Name = "chbShowSpaces";
            this.chbShowSpaces.Size = new Size(0xf8, 0x10);
            this.chbShowSpaces.TabIndex = 3;
            this.chbShowSpaces.Text = "Show spaces";
            this.chbShowTabs.FlatStyle = FlatStyle.System;
            this.chbShowTabs.Location = new Point(8, 0x68);
            this.chbShowTabs.Name = "chbShowTabs";
            this.chbShowTabs.Size = new Size(0xf8, 0x10);
            this.chbShowTabs.TabIndex = 4;
            this.chbShowTabs.Text = "Show tabs";
            this.label1.FlatStyle = FlatStyle.System;
            this.label1.Location = new Point(0x10, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x10);
            this.label1.TabIndex = 5;
            this.label1.Text = "Font:";
            this.lblFont.BackColor = Color.White;
            this.lblFont.FlatStyle = FlatStyle.System;
            this.lblFont.Location = new Point(0x38, 0x18);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new Size(0xa8, 0x10);
            this.lblFont.TabIndex = 6;
            this.lblFont.Text = "Curier";
            this.btnSelectFont.FlatStyle = FlatStyle.System;
            this.btnSelectFont.Location = new Point(0xe8, 0x18);
            this.btnSelectFont.Name = "btnSelectFont";
            this.btnSelectFont.Size = new Size(0x20, 0x10);
            this.btnSelectFont.TabIndex = 7;
            this.btnSelectFont.Text = "...";
            this.btnSelectFont.Click += new EventHandler(this.btnSelectFont_Click);
            this.groupBox1.Controls.Add(this.lblFont);
            this.groupBox1.Controls.Add(this.btnSelectFont);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(8, 0xa8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(280, 0x40);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font settings";
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.chbShowTabs);
            base.Controls.Add(this.chbShowSpaces);
            base.Controls.Add(this.chbShowMatchingBrackets);
            base.Controls.Add(this.chbShowEOLMarkers);
            base.Controls.Add(this.chbShowLineNumbers);
            base.Name = "UcOptionEditor";
            base.Size = new Size(0x138, 0xf8);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void txtTabIndent_TextChanged(object sender, EventArgs e)
        {
        }
    }
}


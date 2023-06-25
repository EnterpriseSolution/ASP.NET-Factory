namespace Codematic
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class FrmSearch : Form
    {
        private DbQuery _frmQuery;
        private int _lastPos;
        private Button btn_Cancel;
        public Button btn_FindNext;
        private Button btnReplace;
        private Button btnReplaceAll;
        private ComboBox cboWord;
        private CheckBox chbMatchCase;
        private CheckBox chbMatchWholeWord;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private MainForm mainForm;
        private Panel panel1;
        private RadioButton rb_Down;
        private RadioButton rb_Up;
        private TextBox txtReplacewith;

        public FrmSearch(DbQuery frmQuery)
        {
            this.InitializeComponent();
            string currentWord = frmQuery.txtContent.Text;  // GetCurrentWord();
            this.cboWord.Text = currentWord;
            this._frmQuery = frmQuery;
            base.Height = 0x98;
            this.btnReplace.Visible = false;
            this.btnReplaceAll.Visible = false;
            this.panel1.Location = new Point(8, 0x30);
            this.mainForm = (MainForm) frmQuery.MdiParentForm;
            this.mainForm.查找下一个ToolStripMenuItem.Visible = true;
            base.Focus();
        }

        public FrmSearch(DbQuery frmQuery, bool replace)
        {
            this.InitializeComponent();
            this.Text = "Replace";
            this._frmQuery = frmQuery;
            if (!replace)
            {
                base.Height = 0x98;
            }
            base.Focus();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btn_FindNext_Click(object sender, EventArgs e)
        {
            string text = this.cboWord.Text;
            try
            {
                Regex regex;
                if (this.chbMatchWholeWord.Checked)
                {
                    text = @"\b" + text + @"\b";
                }
                text = text.Replace("[", @"\[");
                text = text.Replace("(", @"\(");
                if (this.chbMatchCase.Checked)
                {
                    regex = new Regex(text);
                }
                else
                {
                    regex = new Regex(text, RegexOptions.IgnoreCase);
                }
                this._lastPos = this._frmQuery.Find(regex, this._lastPos);
                if (!this.cboWord.Items.Contains(this.cboWord.Text))
                {
                    this.cboWord.Items.Add(this.cboWord.Text);
                }
            }
            catch
            {
                MessageBox.Show("Unable to search for [" + text + "].)");
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            Regex regex;
            string text = this.cboWord.Text;
            if (this.chbMatchWholeWord.Checked)
            {
                text = @"\b" + text + @"\b";
            }
            if (this.chbMatchCase.Checked)
            {
                regex = new Regex(text);
            }
            else
            {
                regex = new Regex(text, RegexOptions.IgnoreCase);
            }
            this._lastPos = this._frmQuery.Replace(regex, this._lastPos, this.txtReplacewith.Text);
            if (!this.cboWord.Items.Contains(this.cboWord.Text))
            {
                this.cboWord.Items.Add(this.cboWord.Text);
            }
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            try
            {
                Regex regex;
                string text = this.cboWord.Text;
                if (this.chbMatchWholeWord.Checked)
                {
                    text = @"\b" + text + @"\b";
                }
                if (this.chbMatchCase.Checked)
                {
                    regex = new Regex(text);
                }
                else
                {
                    regex = new Regex(text, RegexOptions.IgnoreCase);
                }
                this._frmQuery.ReplaceAll(regex, this.txtReplacewith.Text);
                if (!this.cboWord.Items.Contains(this.cboWord.Text))
                {
                    this.cboWord.Items.Add(this.cboWord.Text);
                }
            }
            catch (Exception exception)
            {
                throw exception;
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

        public void FindNext()
        {
        }

        private void FrmSearch_Load(object sender, EventArgs e)
        {
            base.TopLevel = true;
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.chbMatchCase = new CheckBox();
            this.btn_FindNext = new Button();
            this.btn_Cancel = new Button();
            this.chbMatchWholeWord = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.rb_Down = new RadioButton();
            this.rb_Up = new RadioButton();
            this.txtReplacewith = new TextBox();
            this.label2 = new Label();
            this.btnReplaceAll = new Button();
            this.btnReplace = new Button();
            this.panel1 = new Panel();
            this.cboWord = new ComboBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x13, 0x15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "查找内容：";
            this.chbMatchCase.AutoSize = true;
            this.chbMatchCase.Location = new Point(10, 0x23);
            this.chbMatchCase.Name = "chbMatchCase";
            this.chbMatchCase.Size = new Size(0x54, 0x10);
            this.chbMatchCase.TabIndex = 8;
            this.chbMatchCase.Text = "区分大小写";
            this.btn_FindNext.Enabled = false;
            this.btn_FindNext.FlatStyle = FlatStyle.System;
            this.btn_FindNext.Location = new Point(0x163, 0x11);
            this.btn_FindNext.Name = "btn_FindNext";
            this.btn_FindNext.Size = new Size(0x60, 0x1a);
            this.btn_FindNext.TabIndex = 2;
            this.btn_FindNext.Text = "查找下一个(&F)";
            this.btn_FindNext.Click += new EventHandler(this.btn_FindNext_Click);
            this.btn_Cancel.DialogResult = DialogResult.Cancel;
            this.btn_Cancel.FlatStyle = FlatStyle.System;
            this.btn_Cancel.Location = new Point(0x163, 0x34);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(0x60, 0x1a);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "取消(&C)";
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.chbMatchWholeWord.AutoSize = true;
            this.chbMatchWholeWord.Location = new Point(10, 9);
            this.chbMatchWholeWord.Name = "chbMatchWholeWord";
            this.chbMatchWholeWord.Size = new Size(0x54, 0x10);
            this.chbMatchWholeWord.TabIndex = 7;
            this.chbMatchWholeWord.Text = "匹配整个词";
            this.groupBox1.Controls.Add(this.rb_Down);
            this.groupBox1.Controls.Add(this.rb_Up);
            this.groupBox1.Location = new Point(0xb1, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x9a, 60);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "方向";
            this.groupBox1.Visible = false;
            this.rb_Down.Checked = true;
            this.rb_Down.Location = new Point(0x4d, 0x1a);
            this.rb_Down.Name = "rb_Down";
            this.rb_Down.Size = new Size(0x43, 0x11);
            this.rb_Down.TabIndex = 10;
            this.rb_Down.TabStop = true;
            this.rb_Down.Text = "向下";
            this.rb_Up.Location = new Point(0x13, 0x1a);
            this.rb_Up.Name = "rb_Up";
            this.rb_Up.Size = new Size(0x30, 0x11);
            this.rb_Up.TabIndex = 9;
            this.rb_Up.Text = "向上";
            this.txtReplacewith.Location = new Point(0x6a, 0x2c);
            this.txtReplacewith.Name = "txtReplacewith";
            this.txtReplacewith.Size = new Size(240, 0x15);
            this.txtReplacewith.TabIndex = 4;
            this.txtReplacewith.TextChanged += new EventHandler(this.txt_replacewith_TextChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x1f, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "替换为：";
            this.btnReplaceAll.Enabled = false;
            this.btnReplaceAll.FlatStyle = FlatStyle.System;
            this.btnReplaceAll.Location = new Point(0x163, 0x79);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new Size(0x60, 0x19);
            this.btnReplaceAll.TabIndex = 7;
            this.btnReplaceAll.Text = "替换所有";
            this.btnReplaceAll.Click += new EventHandler(this.btnReplaceAll_Click);
            this.btnReplace.Enabled = false;
            this.btnReplace.FlatStyle = FlatStyle.System;
            this.btnReplace.Location = new Point(0x163, 0x56);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new Size(0x60, 0x1a);
            this.btnReplace.TabIndex = 6;
            this.btnReplace.Text = "替换(&R)";
            this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.chbMatchWholeWord);
            this.panel1.Controls.Add(this.chbMatchCase);
            this.panel1.Location = new Point(10, 0x4e);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x159, 0x44);
            this.panel1.TabIndex = 11;
            this.cboWord.Location = new Point(0x6a, 0x11);
            this.cboWord.Name = "cboWord";
            this.cboWord.Size = new Size(240, 20);
            this.cboWord.TabIndex = 1;
            this.cboWord.TextChanged += new EventHandler(this.txt_word_TextChanged);
            base.AcceptButton = this.btn_FindNext;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.CancelButton = this.btn_Cancel;
            base.ClientSize = new Size(460, 0x9a);
            base.Controls.Add(this.cboWord);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.btnReplace);
            base.Controls.Add(this.btnReplaceAll);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.txtReplacewith);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.btn_FindNext);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FrmSearch";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "查找";
            base.TopMost = true;
            base.Load += new EventHandler(this.FrmSearch_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void txt_replacewith_TextChanged(object sender, EventArgs e)
        {
            this.btnReplace.Enabled = (this.cboWord.Text.Length > 0) & (this.txtReplacewith.Text.Length > 0);
            this.btnReplaceAll.Enabled = this.btnReplace.Enabled;
        }

        private void txt_word_TextChanged(object sender, EventArgs e)
        {
            this.btn_FindNext.Enabled = this.cboWord.Text.Length > 0;
        }

        public object[] SearchItems
        {
            get
            {
                ArrayList list = new ArrayList(this.cboWord.Items);
                return list.ToArray();
            }
            set
            {
                if (value != null)
                {
                    this.cboWord.Items.AddRange(value);
                }
            }
        }
    }
}


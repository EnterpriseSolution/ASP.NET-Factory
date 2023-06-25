namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    //using LTP.TextEditor;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class UcCodeView : UserControl
    {
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem menu_Save;
        //public TextEditorControl txtContent_CS;
        //public TextEditorControl txtContent_SQL;
        //public TextEditorControl txtContent_Web;
        //private TextEditorControl txtContent_XML;
        //20090623 this control cause many problems
        public RichTextBox txtContent_CS;
        public RichTextBox txtContent_SQL;
        public RichTextBox txtContent_Web;
        private RichTextBox txtContent_XML;


        public UcCodeView()
        {
            this.InitializeComponent();
            this.SettxtContent("CS", "");
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
            this.components = new System.ComponentModel.Container();
            this.txtContent_Web = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.txtContent_CS = new System.Windows.Forms.RichTextBox();
            this.txtContent_SQL = new System.Windows.Forms.RichTextBox();
            this.txtContent_XML = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtContent_Web
            // 
            this.txtContent_Web.ContextMenuStrip = this.contextMenuStrip1;
            this.txtContent_Web.Font = new System.Drawing.Font("NSimSun", 9F);
            this.txtContent_Web.Location = new System.Drawing.Point(321, 214);
            this.txtContent_Web.Name = "txtContent_Web";
            this.txtContent_Web.Size = new System.Drawing.Size(200, 200);
            this.txtContent_Web.TabIndex = 0;
            this.txtContent_Web.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Save});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 26);
            // 
            // menu_Save
            // 
            this.menu_Save.Name = "menu_Save";
            this.menu_Save.Size = new System.Drawing.Size(112, 22);
            this.menu_Save.Text = "保存(&S)";
            this.menu_Save.Click += new System.EventHandler(this.menu_Save_Click);
            // 
            // txtContent_CS
            // 
            this.txtContent_CS.ContextMenuStrip = this.contextMenuStrip1;
            this.txtContent_CS.Font = new System.Drawing.Font("NSimSun", 9F);
            this.txtContent_CS.Location = new System.Drawing.Point(219, 8);
            this.txtContent_CS.Name = "txtContent_CS";
            this.txtContent_CS.Size = new System.Drawing.Size(200, 200);
            this.txtContent_CS.TabIndex = 0;
            this.txtContent_CS.Text = "";
            // 
            // txtContent_SQL
            // 
            this.txtContent_SQL.ContextMenuStrip = this.contextMenuStrip1;
            this.txtContent_SQL.Font = new System.Drawing.Font("NSimSun", 9F);
            this.txtContent_SQL.Location = new System.Drawing.Point(3, 3);
            this.txtContent_SQL.Name = "txtContent_SQL";
            this.txtContent_SQL.Size = new System.Drawing.Size(200, 200);
            this.txtContent_SQL.TabIndex = 0;
            this.txtContent_SQL.Text = "";
            // 
            // txtContent_XML
            // 
            this.txtContent_XML.ContextMenuStrip = this.contextMenuStrip1;
            this.txtContent_XML.Font = new System.Drawing.Font("NSimSun", 9F);
            this.txtContent_XML.Location = new System.Drawing.Point(3, 3);
            this.txtContent_XML.Name = "txtContent_XML";
            this.txtContent_XML.Size = new System.Drawing.Size(200, 200);
            this.txtContent_XML.TabIndex = 0;
            this.txtContent_XML.Text = "";
            // 
            // UcCodeView
            // 
            this.Controls.Add(this.txtContent_Web);
            this.Controls.Add(this.txtContent_CS);
            this.Controls.Add(this.txtContent_SQL);
            this.Controls.Add(this.txtContent_XML);
            this.Name = "UcCodeView";
            this.Size = new System.Drawing.Size(932, 524);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void menu_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "保存当前代码";
            string text = "";
            if (this.txtContent_CS.Visible)
            {
                dialog.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                text = this.txtContent_CS.Text;
            }
            if (this.txtContent_SQL.Visible)
            {
                dialog.Filter = "SQL files (*.sql)|*.cs|All files (*.*)|*.*";
                text = this.txtContent_SQL.Text;
            }
            if (this.txtContent_Web.Visible)
            {
                dialog.Filter = "Aspx files (*.aspx)|*.cs|All files (*.*)|*.*";
                text = this.txtContent_Web.Text;
            }
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(dialog.FileName, false, Encoding.Default);
                writer.Write(text);
                writer.Flush();
                writer.Close();
            }
        }

        public void SettxtContent(string Type, string strContent)
        {
            string str = Type;
            if (str != null)
            {
                if (!(str == "SQL"))
                {
                    if (!(str == "CS"))
                    {
                        if (!(str == "Aspx"))
                        {
                            if (str == "XML")
                            {
                                this.txtContent_SQL.Visible = false;
                                this.txtContent_CS.Visible = false;
                                this.txtContent_Web.Visible = false;
                                this.txtContent_XML.Visible = true;
                                this.txtContent_XML.Dock = DockStyle.Fill;
                                this.txtContent_XML.Text = strContent;
                            }
                            return;
                        }
                        this.txtContent_SQL.Visible = false;
                        this.txtContent_CS.Visible = false;
                        this.txtContent_XML.Visible = false;
                        this.txtContent_Web.Visible = true;
                        this.txtContent_Web.Dock = DockStyle.Fill;
                        this.txtContent_Web.Text = strContent;
                        return;
                    }
                }
                else
                {
                    this.txtContent_SQL.Visible = true;
                    this.txtContent_CS.Visible = false;
                    this.txtContent_Web.Visible = false;
                    this.txtContent_XML.Visible = false;
                    this.txtContent_SQL.Dock = DockStyle.Fill;
                    this.txtContent_SQL.Text = strContent;
                    return;
                }
                this.txtContent_SQL.Visible = false;
                this.txtContent_CS.Visible = true;
                this.txtContent_Web.Visible = false;
                this.txtContent_XML.Visible = false;
                this.txtContent_CS.Dock = DockStyle.Fill;
                this.txtContent_CS.Text = strContent;
            }
        }
    }
}


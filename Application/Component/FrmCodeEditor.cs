namespace Codematic
{
  //  using LTP.TextEditor;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class CodeEditor : Form
    {
        private IContainer components;
        //public TextEditorControl txtContent;
        public RichTextBox txtContent; 

        public CodeEditor()
        {
            this.InitializeComponent();
        }

        public CodeEditor(string tempFile, string FileType)
        {
            this.InitializeComponent();
            //switch (FileType)
            //{
            //    case "cs":
            //        this.txtContent.Language = TextEditorControlBase.Languages.CSHARP;
            //        break;

            //    case "vb":
            //        this.txtContent.Language = TextEditorControlBase.Languages.VBNET;
            //        break;

            //    case "html":
            //        this.txtContent.Language = TextEditorControlBase.Languages.HTML;
            //        break;

            //    case "sql":
            //        this.txtContent.Language = TextEditorControlBase.Languages.SQL;
            //        break;

            //    case "cpp":
            //        this.txtContent.Language = TextEditorControlBase.Languages.CPP;
            //        break;

            //    case "js":
            //        this.txtContent.Language = TextEditorControlBase.Languages.JavaScript;
            //        break;

            //    case "java":
            //        this.txtContent.Language = TextEditorControlBase.Languages.Java;
            //        break;

            //    case "xml":
            //        this.txtContent.Language = TextEditorControlBase.Languages.XML;
            //        break;

            //    case "txt":
            //        this.txtContent.Language = TextEditorControlBase.Languages.XML;
            //        break;
            //}
            StreamReader reader = new StreamReader(tempFile, Encoding.Default);
            string str = reader.ReadToEnd();
            reader.Close();
            this.txtContent.Text = str;
        }

        public CodeEditor(string strCode, string FileType, string temp)
        {
            this.InitializeComponent();
            //switch (FileType)
            //{
            //    case "cs":
            //        this.txtContent.Language = TextEditorControlBase.Languages.CSHARP;
            //        break;

            //    case "vb":
            //        this.txtContent.Language = TextEditorControlBase.Languages.VBNET;
            //        break;

            //    case "html":
            //        this.txtContent.Language = TextEditorControlBase.Languages.HTML;
            //        break;

            //    case "sql":
            //        this.txtContent.Language = TextEditorControlBase.Languages.SQL;
            //        break;

            //    case "cpp":
            //        this.txtContent.Language = TextEditorControlBase.Languages.CPP;
            //        break;

            //    case "js":
            //        this.txtContent.Language = TextEditorControlBase.Languages.JavaScript;
            //        break;

            //    case "java":
            //        this.txtContent.Language = TextEditorControlBase.Languages.Java;
            //        break;

            //    case "xml":
            //        this.txtContent.Language = TextEditorControlBase.Languages.XML;
            //        break;

            //    case "txt":
            //        this.txtContent.Language = TextEditorControlBase.Languages.XML;
            //        break;
            //}
            this.txtContent.Text = strCode;
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
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Font = new System.Drawing.Font("NSimSun", 9F);
            this.txtContent.Location = new System.Drawing.Point(0, 0);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(712, 306);
            this.txtContent.TabIndex = 0;
            this.txtContent.Text = "";
            // 
            // CodeEditor
            // 
            this.ClientSize = new System.Drawing.Size(712, 306);
            this.Controls.Add(this.txtContent);
            this.Name = "CodeEditor";
            this.Text = "CodeEditor";
            this.ResumeLayout(false);

        }
    }
}


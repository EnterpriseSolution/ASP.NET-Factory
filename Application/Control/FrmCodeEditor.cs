using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Flextronics.Applications.ApplicationFactory.UserControls;
using ICSharpCode.TextEditor.Document;
using System.Collections.Generic;

namespace Flextronics.Applications.ApplicationFactory
{

    public class CodeEditor : UserControl
    {
        public ICSharpCode.TextEditor.TextEditorControl txtContent;
        private IContainer components;

        public CodeEditor()
        {
            this.InitializeComponent();
            setting();
        }

        public CodeEditor(string tempFile, string FileType)
        {
            this.InitializeComponent();
            setting();

            StreamReader reader = new StreamReader(tempFile, Encoding.Default);
            string str = reader.ReadToEnd();
            reader.Close();
            this.txtContent.Text = str;
           
        }

        public CodeEditor(string strCode, string FileType, string temp)
        {
            this.InitializeComponent();
            setting();
            this.txtContent.Text = strCode;           
        }
        void setting()
        {
            // ICSharpCode.TextEditor doesn't have any built-in code folding
            // strategies, so I've included a simple one. Apparently, the
            // foldings are not updated automatically, so in this demo the user
            // cannot add or remove folding regions after loading the file.
            txtContent.Document.FoldingManager.FoldingStrategy = new RegionFoldingStrategy();
            txtContent.Document.FoldingManager.UpdateFoldings(null, null);

            txtContent.ShowEOLMarkers = false;
            txtContent.ShowHRuler = false;
            txtContent.ShowInvalidLines = false;
            txtContent.ShowMatchingBracket = true;
            txtContent.ShowSpaces = false;
            txtContent.ShowTabs = false;
            txtContent.ShowVRuler = false;
            txtContent.AllowCaretBeyondEOL = false;
            txtContent.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            txtContent.Encoding = Encoding.Default;
          
            txtContent.HorizontalScroll.Enabled = false;
            txtContent.HorizontalScroll.Visible = false;
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
            this.txtContent = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.IsReadOnly = false;
            this.txtContent.Location = new System.Drawing.Point(0, 0);
            this.txtContent.Name = "txtContent";
            this.txtContent.ShowVRuler = false;
            this.txtContent.Size = new System.Drawing.Size(712, 306);
            this.txtContent.TabIndex = 0;
            // 
            // CodeEditor
            // 
            this.Controls.Add(this.txtContent);
            this.Name = "CodeEditor";
            this.Size = new System.Drawing.Size(712, 306);
            this.ResumeLayout(false);

        }
    }
    public class RegionFoldingStrategy : IFoldingStrategy
    {
        /// <summary>
        /// Generates the foldings for our document.
        /// </summary>
        /// <param name="document">The current document.</param>
        /// <param name="fileName">The filename of the document.</param>
        /// <param name="parseInformation">Extra parse information, not used in this sample.</param>
        /// <returns>A list of FoldMarkers.</returns>
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker> list = new List<FoldMarker>();

            Stack<int> startLines = new Stack<int>();

            // Create foldmarkers for the whole document, enumerate through every line.
            for (int i = 0; i < document.TotalNumberOfLines; i++)
            {
                var seg = document.GetLineSegment(i);
                int offs, end = document.TextLength;
                char c;
                for (offs = seg.Offset; offs < end && ((c = document.GetCharAt(offs)) == ' ' || c == '\t'); offs++)
                { }
                if (offs == end)
                    break;
                int spaceCount = offs - seg.Offset;

                // now offs points to the first non-whitespace char on the line
                if (document.GetCharAt(offs) == '#')
                {
                    string text = document.GetText(offs, seg.Length - spaceCount);
                    if (text.StartsWith("#region"))
                        startLines.Push(i);
                    if (text.StartsWith("#endregion") && startLines.Count > 0)
                    {
                        // Add a new FoldMarker to the list.
                        int start = startLines.Pop();
                        list.Add(new FoldMarker(document, start,
                            document.GetLineSegment(start).Length,
                            i, spaceCount + "#endregion".Length));
                    }
                }
            }

            return list;
        }
    }
}


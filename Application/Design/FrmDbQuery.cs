//using ICSharpCode.TextEditor.Document;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using Flextronics.Applications.Library.Schema;
using Flextronics.Applications.Library.Utility;


namespace Flextronics.Applications.ApplicationFactory
{
  
    public class DbQuery : Form
    {
        private IAsyncResult _asyncResult;
        private bool _canceled;
        private Exception _currentException;
        private TimeSpan _currentExecutionTime;
        private TreeNode _dragObject;
        private int _dragPos;
        private Regex _findNextRegex;
        private int _findNextStartPos;
        private string _OrginalName;
        private static ArrayList _sqlInfoMessages = new ArrayList();
        private Hashtable Aliases = new Hashtable();
        private ArrayList AliasList = new ArrayList();
        private ContextMenu cmDragAndDrp;
        private ContextMenu cmShortcutMeny;
        private IContainer components;
        private ContextMenu contextMenu1;
        public string DatabaseName = "";
        //private DataGridView dataGridView1;
        private Timer ExecutionTimer;
        private int firstPos;
        private ImageList imageList1;
        private ImageList imageList2;
        public bool IsActive;
        private int keyPressCount;
        private int lastPos;
        private string m_fileName = string.Empty;
        private bool m_resetText = true;
        private MainForm mainfrm;
        public Form MdiParentForm;
        private MenuItem menuItemGroupBy;
        private MenuItem menuItemJoin;
        private MenuItem menuItemLeftOuterJoin;
        private MenuItem menuItemObjectName;
        private MenuItem menuItemOrderBy;
        private MenuItem menuItemRightOuterJoin;
        private MenuItem menuItemSelect1;
        private MenuItem menuItemSelect2;
        private MenuItem menuItemSplitter;
        private MenuItem menuItemWhere;
        private PageSetupDialog pageSetupDialog;
        private PrintDialog printDialog;
        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;
        private ArrayList ReservedWords = new ArrayList();
        private Splitter splitter1;
        private XmlDocument sqlReservedWords = new XmlDocument();
        private StatusStrip statusStrip1;
      //  public SyntaxReader syntaxReader;
        public TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolTip toolTip1;
        private ToolTip ttParamenterInfo;
        //public RichTextBox txtContent; 
        private ImageList imageList3;
        private DataGridView dataGridView1;
        public RichTextBox txtContent;
        private Splitter splitter2;
        public RichTextBox txtInfo;

        public Stream GetSnippetFile()
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream("WebMatrix.Other.Snippets.xml");
            Debug.Assert(stream != null, "Unable to load  for type '" + GetType().FullName + "'");
            return stream;
        }

        public DbQuery(Form mdiParentForm, string strSQL)
        {
            this.InitializeComponent();
            this.MdiParentForm = mdiParentForm;

          
          //  txtContent = new RichTextBox();
         //   txtContent.Dock = DockStyle.Fill;
         //   Controls.Add(txtContent);
            //this.tabControl1.Visible = false;
            //this.txtContent.ActiveTextAreaControl.TextArea.MouseUp += new MouseEventHandler(this.qcTextEditor_MouseUp);
            //this.txtContent.ActiveTextAreaControl.TextArea.DragDrop += new DragEventHandler(this.TextArea_DragDrop);
            //this.txtContent.ActiveTextAreaControl.TextArea.DragEnter += new DragEventHandler(this.TextArea_DragEnter);
            //this.txtContent.ActiveTextAreaControl.TextArea.Click += new EventHandler(this.TextArea_Click);
            //this.txtContent.ActiveTextAreaControl.TextArea.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextArea_KeyPress);
            //this.txtContent.ActiveTextAreaControl.TextArea.KeyUp += new KeyEventHandler(this.TextArea_KeyUp);
            this.mainfrm = (MainForm) mdiParentForm;
            this.mainfrm.toolBtn_SQLExe.Visible = true;
           // this.mainfrm.查询QToolStripMenuItem.Visible = true;
            this.txtContent.Text = strSQL;

              //TODO 自动生成默认的SQL
            txtContent.Text =string.Format(" SELECT *  FROM {0}.dbo.{1} ", mainfrm.toolComboBox_DB.Text,mainfrm.toolComboBox_Table.Text);
         

        }

        public void AddRevisionCommentSection()
        {
            if (this.txtContent.Text.IndexOf("</member>", 0) >= 1)
            {
                int length = this.txtContent.Text.LastIndexOf("</revision>") + 11;
                this.txtContent.Text = this.txtContent.Text.Substring(0, length) + "\n\t<revision author='" + SystemInformation.UserName.ToString() + "' date='" + DateTime.Now.ToString() + "'>Altered</revision>" + this.txtContent.Text.Substring(length);
                this.txtContent.Refresh();
            }
        }

        public void AddToRecentObjects(string objectName)
        {
        }

        public void ComplementHeader()
        {
        }

        public void Copy()
        {
            this.txtContent.Copy();
        }

        public void CreateInsertStatement()
        {
        }

        public void CreateInsertStatement(string SQLstring)
        {
        }

        public void CreateUpdateStatement()
        {
        }

        public void CreateUpdateStatement(string SQLstring)
        {
        }

        public void Cut()
        {
            this.txtContent.Cut();
        }

        private void DbQuery_FormClosing(object sender, FormClosingEventArgs e)
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

        public void ExecuteSql()
        {
            MainForm mdiParentForm = (MainForm) this.MdiParentForm;
            DataSet set = new DataSet();
            try
            {
                string longServername = ((DbView) Application.OpenForms["DbView"]).GetLongServername();
                if (longServername.Length < 1)
                {
                   // this.toolStripStatusLabel1.Text = "没有选择任何服务器！";
                    return;
                }
                //string text = mdiParentForm.toolComboBox_DB.Text;
                //if (text.Length < 1)
                //{
                //   // this.toolStripStatusLabel1.Text = "没有选择可执行的数据库！";
                //    return;
                //}
                //this.toolStripStatusLabel1.Text = "正在进行批查询......";
                string sQLString = this.txtContent.Text;
                if (this.txtContent.SelectedText.Length > 1)
                {
                    sQLString = this.txtContent.SelectedText;
                }
                else
                {
                    sQLString = this.txtContent.Text;
                }
                if (sQLString.Trim() == "")
                {
                   // this.toolStripStatusLabel1.Text = "查询语句为空！";  
                    mdiParentForm.lblViewInfo.Text = "查询语句为空";
                    return;
                }
                string text = sQLString;
                IDbObject obj2 = ObjectHelper.CreatDbObj(longServername);
               // this.txtInfo.Text = "";
                StringBuilder builder = new StringBuilder();
                if (!sQLString.Trim().StartsWith("select") && !sQLString.Trim().StartsWith("SELECT"))
                {
                    if (sQLString.IndexOf("GO\r\n") > 0)
                    {
                        foreach (string str4 in sQLString.Split(new string[] { "GO\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (str4.Trim() != "")
                            {
                                obj2.ExecuteSql(text, str4.Trim());
                                builder.Append("命令成功完成!\r\n");
                            }
                        }
                    }
                    else
                    {
                        builder.Append("命令成功完成（所影响的行数为 " + obj2.ExecuteSql(text, sQLString).ToString() + " 行）");
                    }
                    this.tabControl1.SelectedIndex = 1;
                }
                else
                {
                    set = obj2.Query(text, sQLString);
                    if (set.Tables.Count > 0)
                    {
                        foreach (DataTable table in set.Tables)
                        {
                            builder.Append("（所影响的行数为 " + table.Rows.Count.ToString() + " 行）\r\n");
                        }
                    }
                    //this.tabControl1.SelectedIndex = 0;
                }
            //    this.txtInfo.Text = builder.ToString();
              //  this.toolStripStatusLabel1.Text = "命令已成功完成。";
            }
            catch (Exception exception)
            {
                Log.WriteLog(exception);
                // this.toolStripStatusLabel1.Text = "命令执行失败！";
                //this.txtInfo.Text = exception.Message;
                mainfrm.lblViewInfo.Text = "命令执行失败";
                this.tabControl1.SelectedIndex = 1;
            }
          //  this.tabControl1.Visible = true;
            this.dataGridView1.BackColor = Color.FromArgb(0x94, 0xb6, 0xed);
            if (set.Tables.Count > 1)
            {
                this.dataGridView1.DataSource = set;
            }
            else if (set.Tables.Count == 1)
            {
                this.dataGridView1.DataSource = set.Tables[0];
            }
        }

        private void ExecutionTimer_Tick(object sender, EventArgs e)
        {
        }

        public int Find(Regex regex, int startPos)
        {
            string input = this.txtContent.Text.Substring(startPos);
            Match match = regex.Match(input);
            if (!match.Success)
            {
                MessageBox.Show("没有找到指定文本.", "Codematic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return 0;
            }
            //int lineNumberForOffset = this.txtContent.Document.GetLineNumberForOffset(match.Index + startPos);
            //this.txtContent.ActiveTextAreaControl.TextArea.ScrollTo(lineNumberForOffset);
            //this.txtContent.Select(match.Index + startPos, match.Length);
            //this._findNextRegex = regex;
            //this._findNextStartPos = match.Index + startPos;
            //this.txtContent.SetPosition((match.Index + match.Length) + startPos);
            //return ((match.Index + match.Length) + startPos);
            return 0;
        }

        public void FindNext()
        {
            if (this._findNextRegex != null)
            {
                this.Find(this._findNextRegex, this._findNextStartPos + 1);
            }
        }

        public string GetAliasTableName(string alias)
        {
            return null;
        }

        public void GetCreateTablesScriptFromXMLFile()
        {
        }

        public string GetCurrentWord()
        {
            return this.txtContent.Text;// GetCurrentWord();
        }

        public void GetInsertScriptFromXMLFile()
        {
        }

        public void GetXmlDocFile()
        {
        }

        public void GoToDefenition()
        {
        }

        public void GoToLine()
        {
          //  int currentPosition = this.txtContent.GetLineFromCharIndex(this.txtContent.SelectionStart) + 1;
         //   new FrmGotoLine(this, currentPosition, this.txtContent.Document.LineSegmentCollection.Count).Show();
        }

        public void GoToLine(int line)
        {
            //int offset = this.txtContent.Document.GetLineSegment(line - 1).Offset;
            //int length = this.txtContent.Document.GetLineSegment(line - 1).Length;
            //this.txtContent.ActiveTextAreaControl.TextArea.ScrollTo(line - 1);
            //if (length == 0)
            //{
            //    length++;
            //}
            //this.txtContent.Select(offset, length);
           // this.txtContent.SetLine(line - 1);
        }

        public void GoToReferenceAny()
        {
        }

        public void GoToReferenceObject()
        {
            this.Cursor = Cursors.WaitCursor;
            string currentWord = this.txtContent.Text;//GetCurrentWord();
            if (currentWord.IndexOf(".") > -1)
            {
                currentWord = currentWord.Substring(currentWord.IndexOf(".") + 1);
            }
            if (currentWord.Length == 0)
            {
                MessageBox.Show("The referenced object was not found", "Go to reference", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Cursor = Cursors.Default;
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList3
            // 
            this.imageList3.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList3.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 218);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(642, 154);
            this.dataGridView1.TabIndex = 0;
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(0, 0);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(642, 218);
            this.txtContent.TabIndex = 1;
            this.txtContent.Text = "";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 215);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(642, 3);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // DbQuery
            // 
            this.ClientSize = new System.Drawing.Size(642, 372);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.dataGridView1);
            this.Name = "DbQuery";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        public void InsertHeader()
        {
        }

        private void menuItemGroupBy_Click(object sender, EventArgs e)
        {
        }

        private void menuItemJoin_Click(object sender, EventArgs e)
        {
            string text = this._dragObject.Parent.Parent.Parent.Text;
            string dbname = this._dragObject.Parent.Parent.Text;
            string tablename = this._dragObject.Text;
            string sQLUpdate = ObjectHelper.CreatDsb(text).GetSQLUpdate(dbname, tablename);
           // this.txtContent.ActiveTextAreaControl.TextArea.InsertString(sQLUpdate);
        }

        private void menuItemLeftOuterJoin_Click(object sender, EventArgs e)
        {
            string text = this._dragObject.Parent.Parent.Parent.Text;
            string dbname = this._dragObject.Parent.Parent.Text;
            string tablename = this._dragObject.Text;
            string sQLDelete = ObjectHelper.CreatDsb(text).GetSQLDelete(dbname, tablename);
            //this.txtContent.ActiveTextAreaControl.TextArea.InsertString(sQLDelete);
        }

        private void menuItemObjectName_Click(object sender, EventArgs e)
        {
            string text = this._dragObject.Text;
           // this.txtContent.ActiveTextAreaControl.TextArea.InsertString(text);
        }

        private void menuItemOrderBy_Click(object sender, EventArgs e)
        {
        }

        private void menuItemRightOuterJoin_Click(object sender, EventArgs e)
        {
            string text = this._dragObject.Parent.Parent.Parent.Text;
            string dbname = this._dragObject.Parent.Parent.Text;
            string tablename = this._dragObject.Text;
            string sQLInsert = ObjectHelper.CreatDsb(text).GetSQLInsert(dbname, tablename);
          //  this.txtContent.ActiveTextAreaControl.TextArea.InsertString(sQLInsert);
        }

        private void menuItemSelect1_Click(object sender, EventArgs e)
        {
            string str = "SELECT *\nFROM\t" + this._dragObject.Text + "\n";
         //   this.txtContent.ActiveTextAreaControl.TextArea.InsertString(str);
        }

        private void menuItemSelect2_Click(object sender, EventArgs e)
        {
            string text = this._dragObject.Parent.Parent.Parent.Text;
            string dbname = this._dragObject.Parent.Parent.Text;
            string tablename = this._dragObject.Text;
            string sQLSelect = ObjectHelper.CreatDsb(text).GetSQLSelect(dbname, tablename);
         //   this.txtContent.ActiveTextAreaControl.TextArea.InsertString(sQLSelect);
        }

        private void menuItemWhere_Click(object sender, EventArgs e)
        {
        }

     

        private void miAllSel_Click(object sender, EventArgs e)
        {
            this.txtContent.Select(0, this.txtContent.Text.Length);
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            this.Copy();
        }

        private void miCut_Click(object sender, EventArgs e)
        {
            this.txtContent.Cut();
        }

        private void miGoToAnyRererence_Click(object sender, EventArgs e)
        {
            this.GoToReferenceAny();
        }

        private void miGoToDefinision_Click(object sender, EventArgs e)
        {
            this.GoToDefenition();
        }

        private void miGoToRererence_Click(object sender, EventArgs e)
        {
            this.GoToReferenceObject();
        }

        private void miMakeCurrentQueryCS_Click(object sender, EventArgs e)
        {
            try
            {
              //  this.toolStripStatusLabel1.Text = "正在生成......";
                string text = this.txtContent.Text;
                if (this.txtContent.SelectedText.Length > 1)
                {
                    text = this.txtContent.SelectedText;
                }
                else
                {
                    text = this.txtContent.Text;
                }
                if (text.Trim() == "")
                {
                   // this.toolStripStatusLabel1.Text = "查询语句为空！";
                }
                else
                {
                    StringPlus plus = new StringPlus();
                    plus.AppendLine("StringBuilder strSql=new StringBuilder();");
                    if (text.IndexOf("\r\n") > 0)
                    {
                        foreach (string str2 in text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (str2.Trim() != "")
                            {
                                plus.AppendLine("strSql.Append(\"" + str2 + " \");");
                            }
                        }
                    }
                    else
                    {
                        plus.AppendLine("strSql.Append(\"" + text + " \");");
                    }
                    this.mainfrm.AddTabPage("Class1.cs", new CodeEditor(plus.Value, "cs", ""));
                   // this.toolStripStatusLabel1.Text = "成功完成。";
                }
            }
            catch (Exception exception)
            {
                Log.WriteLog(exception);
                //this.toolStripStatusLabel1.Text = "执行失败！";
            }
        }

        private void miMakeCurrentQuerySQL_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm mdiParentForm = (MainForm) this.MdiParentForm;
                string longServername = ((DbView) Application.OpenForms["DbView"]).GetLongServername();
                if (longServername.Length < 1)
                {
                    //this.toolStripStatusLabel1.Text = "没有选择任何服务器！";
                }
                else
                {
                    string text = mdiParentForm.toolComboBox_DB.Text;
                    if (text.Length < 1)
                    {
                       // this.toolStripStatusLabel1.Text = "没有选择可执行的数据库！";
                    }
                    else
                    {
                      //  this.toolStripStatusLabel1.Text = "正在生成......";
                        string selectedText = this.txtContent.Text;
                        if (this.txtContent.SelectedText.Length > 1)
                        {
                            selectedText = this.txtContent.SelectedText;
                        }
                        else
                        {
                            selectedText = this.txtContent.Text;
                        }
                        if (selectedText.Trim() == "")
                        {
                           // this.toolStripStatusLabel1.Text = "查询语句为空！";
                        }
                        else
                        {
                            string strCode = ObjectHelper.CreatDsb(longServername).CreateTabScriptBySQL(text, selectedText.Trim());
                            this.mainfrm.AddTabPage("SQL1.sql", new CodeEditor(strCode, "sql", ""));
                           // this.toolStripStatusLabel1.Text = "成功完成。";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.WriteLog(exception);
               // this.toolStripStatusLabel1.Text = "执行失败！";
            }
        }

        private void miOptions_Click(object sender, EventArgs e)
        {
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            this.Paste();
        }

        private void miRunCurrentQuery_Click(object sender, EventArgs e)
        {
            this.RunCurrentQuery();
        }

        private void miSnippet_Click(object sender, EventArgs e)
        {
          //  string text = ((SnippetMenuItem) sender).statement.Replace(@"\n", "\n").Replace(@"\t", "\t");
          //  int selectionStart = this.txtContent.SelectionStart;
          ////  this.txtContent.Document.Replace(selectionStart, 0, text);
          //  if (text.IndexOf("{}") > -1)
          //  {
          //      selectionStart = (selectionStart + text.IndexOf("{}")) + 1;
          //  }
          //  this.txtContent.SetPosition(selectionStart);
          //  this.txtContent.Refresh();
        }

        public void miValidateCurrentQuery_Click(object sender, EventArgs e)
        {
            this.txtContent.ResumeLayout();
            string content = this.Content;
            if (this.txtContent.SelectedText.Length > 0)
            {
                string newValue = "SET NOEXEC ON;" + this.txtContent.SelectedText + ";SET NOEXEC OFF;";
                int length = newValue.Length;
                int index = this.Content.IndexOf(this.txtContent.SelectedText);
                if (((this.Content.IndexOf("SET NOEXEC ON", 0) < 0) && (index >= 0)) && (length > 0))
                {
                    this.Content = this.Content.Replace(this.txtContent.SelectedText, newValue);
                    this.txtContent.Select(index, length);
                }
            }
            else
            {
                this.Content = "SET NOEXEC ON;" + this.Content + ";SET NOEXEC OFF;\n\n";
            }
            this.RunQuery();
            this.Content = content;
            this.txtContent.ResumeLayout();
        }

        public void Paste()
        {
            this.txtContent.Paste();
        }

        public void PrintOutPut(bool preview)
        {
        }

        public void PrintPageSetUp()
        {
        }

        public void PrintStatement(bool preview)
        {
        }

        private void qcTextEditor_KeyPressEvent(object sender, KeyEventArgs e)
        {
            e.ToString();
         //   this.txtContent.ActiveTextAreaControl.Caret.Line.ToString();
        //    this.txtContent.ActiveTextAreaControl.Caret.Column.ToString();
            if (e.Alt && (e.KeyValue == 0x58))
            {
                e.Handled = false;
                this.RunQuery();
            }
            else
            {
                Keys keyCode = e.KeyCode;
                if (e.KeyCode == Keys.F5)
                {
                    this.RunQuery();
                }
                if (((((Control.ModifierKeys & Keys.Control) == Keys.Control) && (e.KeyValue == 0x20)) || (e.KeyValue == 190)) && (e.KeyValue != 190))
                {
                    e.Handled = true;
                }
                if (((Control.ModifierKeys & Keys.Control) != Keys.Control) || (e.KeyValue != 0x43))
                {
                    int keyValue = e.KeyValue;
                }
            }
        }

        private void qcTextEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string text=this.txtContent.Text; //GetCurrentWord();
                IDataObject dataObject = Clipboard.GetDataObject();
                MenuItem item = null;
                MenuItem item2 = null;
                MenuItem item3 = null;
                MenuItem item4 = null;
                MenuItem item5 = new MenuItem("复制 (&C)");
                MenuItem item6 = new MenuItem("剪切 (&T)");
                MenuItem item7 = new MenuItem("粘贴 (&P)");
                MenuItem item8 = new MenuItem("全选 (&A)");
                new MenuItem("-");
                MenuItem item9 = new MenuItem("转到定义 (&G)");
                MenuItem item10 = new MenuItem("转到对象引用 (&R)");
                MenuItem item11 = new MenuItem("转到所有对象引用");
                MenuItem item12 = new MenuItem("-");
                if (this.txtContent.SelectedText.Length > 0)
                {
                    item = new MenuItem("运行当前选择");
                    item2 = new MenuItem("验证当前选择");
                    item3 = new MenuItem("生成当前选择SQL语句的拼接代码");
                    item4 = new MenuItem("生成当前选择查询结果的数据脚本");
                }
                else
                {
                    item = new MenuItem("运行当前查询");
                    item2 = new MenuItem("验证当前查询");
                    item3 = new MenuItem("生成当前查询SQL语句的拼接代码");
                    item4 = new MenuItem("生成当前查询结果的数据脚本");
                }
                MenuItem item13 = new MenuItem("-");
                MenuItem item14 = new MenuItem("选项 (&O)");
                MenuItem item15 = new MenuItem("-");
                MenuItem item16 = new MenuItem("脚本片断");
                MenuItem item17 = new MenuItem("增加到脚本片断");
                item5.Click += new EventHandler(this.miCopy_Click);
                item6.Click += new EventHandler(this.miCut_Click);
                item7.Click += new EventHandler(this.miPaste_Click);
                item8.Click += new EventHandler(this.miAllSel_Click);
                item9.Click += new EventHandler(this.miGoToDefinision_Click);
                item10.Click += new EventHandler(this.miGoToRererence_Click);
                item11.Click += new EventHandler(this.miGoToAnyRererence_Click);
                item.Click += new EventHandler(this.miRunCurrentQuery_Click);
                item2.Click += new EventHandler(this.miValidateCurrentQuery_Click);
                item3.Click += new EventHandler(this.miMakeCurrentQueryCS_Click);
                item4.Click += new EventHandler(this.miMakeCurrentQuerySQL_Click);
                item14.Click += new EventHandler(this.miOptions_Click);
                if (!dataObject.GetDataPresent(DataFormats.Text))
                {
                    item7.Enabled = false;
                }
                this.cmShortcutMeny.MenuItems.Clear();
                this.cmShortcutMeny.MenuItems.Add(item5);
                this.cmShortcutMeny.MenuItems.Add(item6);
                this.cmShortcutMeny.MenuItems.Add(item7);
                this.cmShortcutMeny.MenuItems.Add(item8);
                this.cmShortcutMeny.MenuItems.Add(item12);
                this.cmShortcutMeny.MenuItems.Add(item);
                this.cmShortcutMeny.MenuItems.Add(item13);
                this.cmShortcutMeny.MenuItems.Add(item3);
                this.cmShortcutMeny.MenuItems.Add(item4);
                this.cmShortcutMeny.MenuItems.Add(item15);
                this.cmShortcutMeny.MenuItems.Add(item16);
                XmlDocument document = new XmlDocument();
                //TODO 变成资源文件
                //document.Load(Application.StartupPath + @"\Snippets.xml");
                document.Load(GetSnippetFile());
                //end
                
                XmlNodeList elementsByTagName = document.GetElementsByTagName("snippets");
                if (this.txtContent.SelectedText.Length > 1)
                {
                    item16.MenuItems.Add(item17);
                }
                foreach (XmlNode node in elementsByTagName[0].ChildNodes)
                {
                    SnippetMenuItem item18 = new SnippetMenuItem();
                    item18.Text = node.Attributes["name"].Value;
                    item18.statement = node.InnerText;
                    item18.Click += new EventHandler(this.miSnippet_Click);
                    item16.MenuItems.Add(item18);
                }
                this.cmShortcutMeny.Show(this.txtContent, new Point(e.X, e.Y));
            }
        }

        public void RefreshLineRangeColor(int firstLine, int toLine)
        {
        }

        public int Replace(Regex regex, int startPos, string replaceWith)
        {
            //if (this.txtContent.SelectedText.Length > 0)
            //{
            //    int offset = this.txtContent.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
            //    int length = this.txtContent.SelectedText.Length;
            //    this.txtContent.Document.Replace(offset, length, replaceWith);
            //    return this.Find(regex, length + offset);
            //}
            //string input = this.txtContent.Text.Substring(startPos);
            //Match match = regex.Match(input);
            //if (!match.Success)
            //{
            //    MessageBox.Show("没有找到指定文本.", "Codematic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    return 0;
            //}
            //this.txtContent.Document.Replace(match.Index + startPos, match.Length, replaceWith);
            //return ((match.Index + match.Length) + startPos);
            return 0;
        }

        public void ReplaceAll(Regex regex, string replaceWith)
        {
            string text = this.txtContent.Text;
            this.txtContent.Text = regex.Replace(this.txtContent.Text, replaceWith);
        }

        public void RunCurrentQuery()
        {
            try
            {
                this.RunQuery();
            }
            catch (Exception exception)
            {
                Log.WriteLog(exception);
            }
        }

        public void RunQuery()
        {
            this.ExecuteSql();
        }

        public void RunQueryLine()
        {
            Point positionFromCharIndex = this.txtContent.GetPositionFromCharIndex(this.txtContent.SelectionStart);
            positionFromCharIndex.X = 0;
            int charIndexFromPosition = this.txtContent.GetCharIndexFromPosition(positionFromCharIndex);
            int index = this.txtContent.Text.IndexOf("\n", charIndexFromPosition);
            if (index == -1)
            {
                index = this.txtContent.Text.Length;
            }
            this.txtContent.Select(charIndexFromPosition, index - charIndexFromPosition);
            this.RunQuery();
        }

        public void SaveAs(string path)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists && ((info.Attributes & FileAttributes.ReadOnly) != 0))
                {
                    if (MessageBox.Show("Overwrite read-only file?", path, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }
                    info.Attributes -= 1;
                    info.Delete();
                }
                this.txtContent.SaveFile(path);
                this.FileName = path;
            }
            catch (Exception exception)
            {
                Log.WriteLog(exception);
                MessageBox.Show(string.Format("Errors Ocurred\n{0}", exception.Message), "Save Error", MessageBoxButtons.OK);
            }
        }

        public void SendToOutPutWindow()
        {
        }

        public void SetDatabaseConnection(string dbName, IDbConnection conn)
        {
        }

        private void SetDragAndDropContextMenu(TreeNode node)
        {
            foreach (MenuItem item in this.cmDragAndDrp.MenuItems)
            {
                item.Visible = false;
            }
            this.menuItemObjectName.Visible = true;
            this.menuItemObjectName.Text = node.Text;
            this.menuItemSplitter.Visible = true;
            if ((node.Tag.ToString() == "table") || (node.Tag.ToString() == "view"))
            {
                this.menuItemSelect1.Visible = true;
                this.menuItemSelect2.Visible = true;
                this.menuItemJoin.Visible = true;
                this.menuItemLeftOuterJoin.Visible = true;
                this.menuItemRightOuterJoin.Visible = true;
            }
            else
            {
                bool flag1 = node.Tag.ToString() == "column";
            }
        }

        private void SetDragAndDropMenuIcons()
        {
        }

        public void StopCurrentExecution()
        {
            this.txtContent.Enabled = true;
        }

        private void TextArea_Click(object sender, EventArgs e)
        {
          //  this.txtContent.ActiveTextAreaControl.Caret.Line.ToString();
         //   this.txtContent.ActiveTextAreaControl.Caret.Column.ToString();
        }

        private void TextArea_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Forms.TreeNode", false) != null)
            {
                TreeNode data = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode", false);
                if (((data.Tag.ToString() == "table") || (data.Tag.ToString() == "view")) || (data.Tag.ToString() == "column"))
                {
                    Rectangle rectangle = this.txtContent.RectangleToClient(this.txtContent.ClientRectangle);
                    Point position = new Point(e.X + rectangle.X, e.Y + rectangle.Y);
                    this._dragPos = this.txtContent.GetCharIndexFromPosition(position);
                    this._dragObject = data;
                    string text = data.Text;
                    this.SetDragAndDropContextMenu(data);
                    this.cmDragAndDrp.Show(this.txtContent, position);
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] strArray = (string[]) e.Data.GetData(DataFormats.FileDrop);
                try
                {
                    string path = strArray[0];
                    if (strArray.Length <= 1)
                    {
                        MainForm mdiParentForm = (MainForm) this.MdiParentForm;
                        Path.GetFileName(path);
                        string str2 = "";
                        StreamReader reader = new StreamReader(path);
                        str2 = reader.ReadToEnd();
                        reader.Close();
                        reader = null;
                        this.Content = str2;
                    }
                }
                catch (Exception exception)
                {
                    Log.WriteLog(exception);
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void TextArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("System.Windows.Forms.TreeNode", false) != null)
            {
                TreeNode data = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode", false);
                if ((data.Tag.ToString() == "table") || (data.Tag.ToString() == "view"))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else if (data.Tag.ToString() == "column")
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TextArea_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;
            if (((keyChar != '\b') && (keyChar != '\r')) && (keyChar != '\x001b'))
            {
                this.keyPressCount++;
                if (this.Text.IndexOf("*") <= 0)
                {
                    this.Text = this.Text + "*";
                }
            }
        }

        private void TextArea_KeyUp(object sender, KeyEventArgs e)
        {
            if ((((e.KeyCode == Keys.Tab) || (e.KeyCode == Keys.Delete)) || ((e.KeyCode == Keys.Back) || (e.KeyCode == Keys.Return))) && (this.Text.IndexOf("*") <= 0))
            {
                this.Text = this.Text + "*";
            }
        }

        public void Undo()
        {
          //  this.txtContent.UndoAction();
        }

        public bool ClosingCanceled
        {
            get
            {
                return this._canceled;
            }
        }

        public string Content
        {
            get
            {
                return this.txtContent.Text;
            }
            set
            {
                this.txtContent.Text = value;
                this.txtContent.Refresh();
            }
        }

        public Font EditorFont
        {
            get
            {
                return this.txtContent.Font;
            }
            set
            {
                this.txtContent.Font = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_fileName;
            }
            set
            {
                if (value != string.Empty)
                {
                    string str = value;
                    this.Text = str;
                }
                this.m_fileName = value;
            }
        }

        private class Alias
        {
            public string AliasName;
            public string TableName;

            public Alias(string alias, string table)
            {
                this.AliasName = alias;
                this.TableName = table;
            }
        }

        public class SnippetMenuItem : MenuItem
        {
            public string statement = "";
        }
    }
}


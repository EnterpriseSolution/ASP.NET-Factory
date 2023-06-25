using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ICSharpCode.TextEditor;

namespace LTPTextEditor
{
    public class SyntaxReader
    {
        // Fields
        private Color _commentColor = Color.Green;
        private Color _compareColor = Color.PaleVioletRed;
        private Color _defaultColor = Color.Black;
        private int _differencialPercentage = 0x65;
        private bool _hideStartPage = true;
        private Color _keyWordColor = Color.Blue;
        private int _oleCommentColor = 0x8000;
        private int _oleCompareColor = 0x9370db;
        private int _oleDefaultColor = -9999997;
        private int _oleKeyWordColor = 0xff0000;
        private int _oleOperatorColor = 0x808080;
        private int _oleStringColor = 0xff;
        private Color _operatorColor = Color.Gray;
        private bool _runWithIOStatistics = true;
        private Settings _settings;
        private bool _showFrmDocumentHeader = true;
        private Color _stringColor = Color.Red;
        private ArrayList Compares = new ArrayList();
        public Font EditorFont;
        private ArrayList Functions = new ArrayList();
        private ArrayList Keywords = new ArrayList();
        private ArrayList Operands = new ArrayList();
        public string ReservedWordsRegExPath = @"QUERYCOMMANDER\s|";
        private Hashtable sqlReservedWords = new Hashtable();
        public XmlNodeList xmlNodeList;
        public XmlDocument xmlReservedWords;

        // Methods
        public SyntaxReader()
        {
            this.LoadXMLDocuments();
        }

        public void FillArrays()
        {
        }

        public Color GetColor(string word)
        {
            if (this.sqlReservedWords.Contains(word))
            {
                switch (this.sqlReservedWords[word].ToString())
                {
                    case "keyword":
                        return this._keyWordColor;

                    case "operator":
                        return this._operatorColor;

                    case "compare":
                        return this._compareColor;
                }
            }
            return Color.Black;
        }

        public int GetColorRef(string word)
        {
            if (!this.sqlReservedWords.Contains(word))
            {
                return -9999997;
            }
            switch (this.sqlReservedWords[word].ToString())
            {
                case "keyword":
                    return this._oleKeyWordColor;

                case "operator":
                    return this._oleOperatorColor;

                case "compare":
                    return this._oleCompareColor;
            }
            return this._oleDefaultColor;
        }

        public bool IsFunction(string s)
        {
            return (this.Functions.BinarySearch(s) >= 0);
        }

        public bool IsKeyword(string s)
        {
            return (this.Keywords.BinarySearch(s) >= 0);
        }

        public bool IsReservedWord(string word)
        {
            if (word == null)
            {
                return false;
            }
            return this.sqlReservedWords.Contains(word.ToUpper());
        }

        private void LoadXMLDocuments()
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsApplication1.QCTextEditor.SQLReservedWords.xml");
            this.xmlReservedWords = new XmlDocument();
            this.xmlReservedWords.Load(manifestResourceStream);
            this.xmlNodeList = this.xmlReservedWords.GetElementsByTagName("SQLReservedWords");
            ArrayList list = new ArrayList();
            foreach (XmlNode node in this.xmlNodeList[0].ChildNodes)
            {
                if (this.sqlReservedWords.Contains(node.Name))
                {
                    list.Add(node.Name);
                }
                else
                {
                    this.sqlReservedWords.Add(node.Name, node.Attributes["type"].Value);
                    this.ReservedWordsRegExPath = this.ReservedWordsRegExPath + node.Name + @"\s|";
                }
            }
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextReader textReader = new StreamReader(path);
            this._settings = (Settings)serializer.Deserialize(textReader);
            textReader.Close();
            this._hideStartPage = this._settings.HideStartPage;
            this._keyWordColor = Color.FromName(this._settings.keyWordColor);
            this._commentColor = Color.FromName(this._settings.commentColor);
            this._compareColor = Color.FromName(this._settings.compareColor);
            this._defaultColor = Color.FromName(this._settings.defaultColor);
            this._operatorColor = Color.FromName(this._settings.operatorColor);
            this._stringColor = Color.FromName(this._settings.stringColor);
            this._oleCommentColor = this._settings.Ole_commentColor;
            this._oleCompareColor = this._settings.Ole_compareColor;
            this._oleDefaultColor = this._settings.Ole_defaultColor;
            this._oleKeyWordColor = this._settings.Ole_keyWordColor;
            this._oleOperatorColor = this._settings.Ole_operatorColor;
            this._oleStringColor = this._settings.Ole_stringColor;
            this.EditorFont = new Font(this._settings.fontFamily, this._settings.fontSize, this._settings.fontStyle, this._settings.fontGraphicsUnit);
            this._differencialPercentage = this._settings.DifferencialPercentage;
            this._runWithIOStatistics = this._settings.RunWithIOStatistics;
            this._showFrmDocumentHeader = this._settings.ShowFrmDocumentHeader;
            if ((this._differencialPercentage == 0) && !this._runWithIOStatistics)
            {
                this._runWithIOStatistics = true;
                this._differencialPercentage = 0x65;
            }
        }

        public void Save()
        {
            this._settings.keyWordColor = this._keyWordColor.Name;
            this._settings.operatorColor = this._operatorColor.Name;
            this._settings.compareColor = this._compareColor.Name;
            this._settings.commentColor = this._commentColor.Name;
            this._settings.stringColor = this._stringColor.Name;
            this._settings.defaultColor = this._defaultColor.Name;
            this._settings.Ole_commentColor = this._oleCommentColor;
            this._settings.Ole_compareColor = this._oleCompareColor;
            this._settings.Ole_defaultColor = this._oleDefaultColor;
            this._settings.Ole_keyWordColor = this._oleKeyWordColor;
            this._settings.Ole_operatorColor = this._oleOperatorColor;
            this._settings.Ole_stringColor = this._oleStringColor;
            this._settings.RunWithIOStatistics = this._runWithIOStatistics;
            this._settings.DifferencialPercentage = this._differencialPercentage;
            this._settings.HideStartPage = this._hideStartPage;
            this._settings.ShowFrmDocumentHeader = this._showFrmDocumentHeader;
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, this._settings);
            textWriter.Close();
            this.LoadXMLDocuments();
        }

        public void Save(Font f)
        {
            this._settings.keyWordColor = this._keyWordColor.Name;
            this._settings.operatorColor = this._operatorColor.Name;
            this._settings.compareColor = this._compareColor.Name;
            this._settings.commentColor = this._commentColor.Name;
            this._settings.stringColor = this._stringColor.Name;
            this._settings.defaultColor = this._defaultColor.Name;
            this._settings.Ole_commentColor = this._oleCommentColor;
            this._settings.Ole_compareColor = this._oleCompareColor;
            this._settings.Ole_defaultColor = this._oleDefaultColor;
            this._settings.Ole_keyWordColor = this._oleKeyWordColor;
            this._settings.Ole_operatorColor = this._oleOperatorColor;
            this._settings.Ole_stringColor = this._oleStringColor;
            this._settings.fontFamily = f.FontFamily.Name;
            this._settings.fontGraphicsUnit = f.Unit;
            this._settings.fontSize = f.Size;
            this._settings.fontStyle = f.Style;
            this._settings.RunWithIOStatistics = this._runWithIOStatistics;
            this._settings.DifferencialPercentage = this._differencialPercentage;
            this._settings.HideStartPage = this._hideStartPage;
            this._settings.ShowFrmDocumentHeader = this._showFrmDocumentHeader;
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, this._settings);
            textWriter.Close();
            this.LoadXMLDocuments();
        }

        // Properties
        public int color_comment
        {
            get
            {
                return this._oleCommentColor;
            }
            set
            {
                this._oleCommentColor = value;
            }
        }

        public int color_compare
        {
            get
            {
                return this._oleCompareColor;
            }
            set
            {
                this._oleCompareColor = value;
            }
        }

        public int color_default
        {
            get
            {
                return this._oleDefaultColor;
            }
            set
            {
                this._oleDefaultColor = value;
            }
        }

        public int color_keyword
        {
            get
            {
                return this._oleKeyWordColor;
            }
            set
            {
                this._oleKeyWordColor = value;
            }
        }

        public int color_operator
        {
            get
            {
                return this._oleOperatorColor;
            }
            set
            {
                this._oleOperatorColor = value;
            }
        }

        public int color_string
        {
            get
            {
                return this._oleStringColor;
            }
            set
            {
                this._oleStringColor = value;
            }
        }

        public Color CommentColor
        {
            get
            {
                return this._commentColor;
            }
            set
            {
                this._commentColor = value;
            }
        }

        public Color CompareColor
        {
            get
            {
                return this._compareColor;
            }
            set
            {
                this._compareColor = value;
            }
        }

        public Color DefaultColor
        {
            get
            {
                return this._defaultColor;
            }
            set
            {
                this._defaultColor = value;
            }
        }

        public int DifferencialPercentage
        {
            get
            {
                return this._differencialPercentage;
            }
            set
            {
                this._differencialPercentage = value;
            }
        }

        public bool HideStartPage
        {
            get
            {
                return this._hideStartPage;
            }
            set
            {
                this._hideStartPage = value;
            }
        }

        public Color KeyWordColor
        {
            get
            {
                return this._keyWordColor;
            }
            set
            {
                this._keyWordColor = value;
            }
        }

        public Color OperatorColor
        {
            get
            {
                return this._operatorColor;
            }
            set
            {
                this._operatorColor = value;
            }
        }

        public bool RunWithIOStatistics
        {
            get
            {
                return this._runWithIOStatistics;
            }
            set
            {
                this._runWithIOStatistics = value;
            }
        }

        public bool ShowFrmDocumentHeader
        {
            get
            {
                return this._showFrmDocumentHeader;
            }
            set
            {
                this._showFrmDocumentHeader = value;
            }
        }

        public Color StringColor
        {
            get
            {
                return this._stringColor;
            }
            set
            {
                this._stringColor = value;
            }
        }

        // Nested Types
        [Serializable]
        public class Settings
        {
            // Fields
            public string commentColor;
            public string compareColor;
            public string defaultColor;
            public int DifferencialPercentage;
            public string fontFamily;
            public GraphicsUnit fontGraphicsUnit;
            public float fontSize;
            public FontStyle fontStyle;
            public bool HideStartPage;
            public string keyWordColor;
            public int Ole_commentColor;
            public int Ole_compareColor;
            public int Ole_defaultColor;
            public int Ole_keyWordColor;
            public int Ole_operatorColor;
            public int Ole_stringColor;
            public string operatorColor;
            public bool RunWithIOStatistics;
            public bool ShowFrmDocumentHeader;
            public string stringColor;
        }
    }

}

namespace LTPTextEditor.Editor
{
    [Serializable]
    public class TextEditorControlWrapper : TextEditorControl
    {
        // Fields
        private WordAndPosition[] _buffer = new WordAndPosition[0x30d40];
        public InfoMessageCollection _infoMessages = new InfoMessageCollection();

        // Events
        public event KeyPressEventHandler KeyPressEvent;

        public event MYMouseRButtonUpEventHandler RMouseUpEvent;

        // Methods
        public TextEditorControlWrapper()
        {
            base.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
        }

        public void AddInfoMessages(Point StartPoint, InfoMessage.MessageType MessageType, double PercentPositionFromTop, string Message)
        {
            int num = this._infoMessages.Add(new InfoMessage(StartPoint, MessageType, PercentPositionFromTop, Message));
            base.Controls.Add(this._infoMessages[num].Picture);
        }

        public void ClearInfoMessages()
        {
            this._infoMessages.Clear();
        }

        public void Copy()
        {
            this.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(null, null);
        }

        public void Cut()
        {
            this.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(null, null);
        }

        public int Find(string str, int start, RichTextBoxFinds options)
        {
            return 1;
        }

        public int GetCharIndexForTableDefenition(string tableName)
        {
            this._buffer.Initialize();
            int index = 0;
            string text = this.Text;
            Regex regex = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            for (Match match = regex.Match(text); match.Success; match = match.NextMatch())
            {
                if (((match.Value.ToUpper() == tableName.ToUpper()) && (this._buffer[index - 1].Word.ToUpper() == "FROM")) || ((match.Value.ToUpper() == tableName.ToUpper()) && (this._buffer[index - 1].Word.ToUpper() == "JOIN")))
                {
                    return match.Index;
                }
                this._buffer[index].Word = match.Value;
                this._buffer[index].Position = match.Index;
                this._buffer[index].Length = match.Length;
                index++;
            }
            return -1;
        }

        public int GetCharIndexFromPosition(Point position)
        {
            return 1;
        }

        public string GetCurrentWord()
        {
            int offset = this.ActiveTextAreaControl.Caret.Offset;
            string wordAt = TextUtilities.GetWordAt(base.Document, offset);
            if ((wordAt.Length == 0) && (this.Text.Length > (offset - 1)))
            {
                wordAt = TextUtilities.GetWordAt(base.Document, offset - 1);
            }
            return wordAt.Trim();
        }

        public int GetLineFromCharIndex(int offSet)
        {
            return base.Document.GetLineNumberForOffset(offSet);
        }

        public Point GetPositionFromCharIndex(int position)
        {
            Point screenPosition;
            try
            {
                screenPosition = this.ActiveTextAreaControl.Caret.ScreenPosition;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return screenPosition;
        }

        public string GetText(int startPos, int length)
        {
            return base.Document.TextBufferStrategy.GetText(startPos, length);
        }

        public ArrayList GetVariables(string Stringmatch)
        {
            ArrayList list = new ArrayList();
            string text = this.Text;
            string pattern = @"\100+\w+";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            for (Match match = regex.Match(text); match.Success; match = match.NextMatch())
            {
                if (!list.Contains(match.Value))
                {
                    list.Add(match.Value);
                }
            }
            return list;
        }

        public void GoToLine(int line)
        {
        }

        private void InitializeComponent()
        {
            base.textAreaPanel.Name = "textAreaPanel";
            base.textAreaPanel.Size = new Size(0x368, 760);
            base.Name = "TextEditorControlWrapper";
            base.Size = new Size(0x368, 760);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            bool flag = true;
            switch (keyData)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    return flag;
            }
            return base.IsInputKey(keyData);
        }

        public string Mark(int startPos, int length)
        {
            Point positionFromCharIndex = this.GetPositionFromCharIndex(startPos);
            Point endPosition = this.GetPositionFromCharIndex(startPos + length);
            this.ActiveTextAreaControl.TextArea.SelectionManager.SetSelection(positionFromCharIndex, endPosition);
            return base.Document.TextBufferStrategy.GetText(startPos, length);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        public void Paste()
        {
            this.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(null, null);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            this.RaiseKeyPressEvent(keyData);
            return base.ProcessDialogKey(keyData);
        }

        private void RaiseKeyPressEvent(Keys keydata)
        {
            KeyEventArgs args = new KeyEventArgs(keydata);
            this.KeyPressEvent(this, args);
        }

        private void RaiseRMouseUpEvent()
        {
            if (this.RMouseUpEvent != null)
            {
                int x = this.ActiveTextAreaControl.Caret.ScreenPosition.X;
                int y = this.ActiveTextAreaControl.Caret.ScreenPosition.Y;
                MouseEventArgs args = new MouseEventArgs(MouseButtons.Right, 1, x, y, 0);
                this.RMouseUpEvent(this, args);
            }
        }

        public void Select(int startPos, int length)
        {
            Point startPosition = this.ActiveTextAreaControl.TextArea.Document.OffsetToPosition(startPos);
            Point endPosition = this.ActiveTextAreaControl.TextArea.Document.OffsetToPosition(startPos + length);
            this.ActiveTextAreaControl.TextArea.SelectionManager.SetSelection(startPosition, endPosition);
        }

        public void SetHighLightingStragegy(string token)
        {
            base.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(token);
        }

        public void SetLine(int line)
        {
            TextArea textArea = this.ActiveTextAreaControl.TextArea;
            textArea.Caret.Column = 0;
            textArea.Caret.Line = line;
            textArea.Caret.UpdateCaretPosition();
        }

        public void SetPosition(int pos)
        {
            this.ActiveTextAreaControl.TextArea.Caret.Position = base.Document.OffsetToPosition(pos);
        }

        public void SetReseveredWordsToUpperCase()
        {
            Regex regex = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            SyntaxReader reader = new SyntaxReader();
            for (Match match = regex.Match(this.Text); match.Success; match = match.NextMatch())
            {
                if (reader.IsReservedWord(match.Value.ToUpper()))
                {
                    base.Document.Replace(match.Index, match.Length, match.Value.ToUpper());
                }
            }
        }

        public void SetSelectionUnderlineColor(object underlineColor)
        {
        }

        public void SetSelectionUnderlineStyle(object underlineStyle)
        {
        }

        public void UndoAction()
        {
            base.Undo();
        }

        // Properties
        public string CurrentWord
        {
            get
            {
                int offset = TextUtilities.FindPrevWordStart(base.Document, this.SelectionStart);
                string wordAt = TextUtilities.GetWordAt(base.Document, offset);
                if (wordAt.IndexOf(".") > 0)
                {
                    return wordAt.Substring(wordAt.IndexOf(".") + 1);
                }
                return wordAt;
            }
        }

        public string SelectedText
        {
            get
            {
                return this.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
            }
            set
            {
            }
        }

        public int SelectionStart
        {
            get
            {
                return this.ActiveTextAreaControl.Caret.Offset;
            }
            set
            {
            }
        }

        // Nested Types
        public class Action
        {
            // Fields
            public int Position;
            public string SelectedText;
            public string Value;

            // Methods
            public Action(int Position, string Value, string SelectedText)
            {
                this.Position = Position;
                this.Value = Value;
                this.SelectedText = SelectedText;
            }
        }

        public class API
        {
            // Methods
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern int SendMessage(IntPtr hWnd, int wmsg, int wParam, int lParam);
            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern int SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref TextEditorControlWrapper.Messages.CHARFORMAT lp);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref TextEditorControlWrapper.Messages.PARAFORMAT lp);
        }

        private class ColorPosition
        {
            // Fields
            public int EndPosition;
            public int StartPosition;
            public ColorType Type;

            // Methods
            public ColorPosition(ColorType type, int startPosition, int endPosition)
            {
                this.StartPosition = startPosition;
                this.EndPosition = endPosition;
                this.Type = type;
            }

            // Nested Types
            public enum ColorType
            {
                String,
                Comment
            }
        }

        private class ColorPositionCollection : ArrayList
        {
            // Methods
            public int Add(TextEditorControlWrapper.ColorPosition colorPosition)
            {
                return base.Add(colorPosition);
            }
        }

        public class DebugEventArgs : EventArgs
        {
            // Fields
            public string method;

            // Methods
            public DebugEventArgs(string method)
            {
                this.method = method;
            }
        }

        public class InfoMessage
        {
            // Fields
            public double PercentPositionFormTop;
            public PictureBox Picture = new PictureBox();
            public Point StartPoint;

            // Methods
            public InfoMessage(Point startPoint, MessageType messageType, double percentPositionFormTop, string Message)
            {
                this.StartPoint = startPoint;
                this.Picture.Location = startPoint;
                this.Picture.SizeMode = PictureBoxSizeMode.StretchImage;
                this.Picture.Size = new Size(15, 15);
                this.Picture.BackColor = Color.Transparent;
                this.PercentPositionFormTop = percentPositionFormTop;
                ToolTip tip = new ToolTip();
                tip.ShowAlways = true;
                tip.SetToolTip(this.Picture, Message);
                switch (messageType)
                {
                    case MessageType.Info:
                        this.Picture.Image = Image.FromStream(this.CopyEmbeddedResource("WindowsApplication1.Embedded.infomessage.gif"));
                        return;

                    case MessageType.Warning:
                        this.Picture.Image = Image.FromStream(this.CopyEmbeddedResource("WindowsApplication1.Embedded.infowarning.gif"));
                        return;

                    case MessageType.Exception:
                        this.Picture.Image = Image.FromStream(this.CopyEmbeddedResource("WindowsApplication1.Embedded.infowarning.gif"));
                        return;
                }
                this.Picture.Image = Image.FromStream(this.CopyEmbeddedResource("WindowsApplication1.Embedded.infomessage.gif"));
            }

            private Stream CopyEmbeddedResource(string resource)
            {
                return Assembly.GetEntryAssembly().GetManifestResourceStream(resource);
            }

            // Nested Types
            public enum MessageType
            {
                Info,
                Warning,
                Exception
            }
        }

        public class InfoMessageCollection : CollectionBase
        {
            // Methods
            public virtual int Add(TextEditorControlWrapper.InfoMessage newInfoMessage)
            {
                return base.List.Add(newInfoMessage);
            }

            // Properties
            public virtual TextEditorControlWrapper.InfoMessage this[int Index]
            {
                get
                {
                    return (TextEditorControlWrapper.InfoMessage)base.List[Index];
                }
            }
        }

        public delegate void KeyPressEventHandler(object sender, KeyEventArgs args);

        public class Messages
        {
            // Fields
            public const int CFM_UNDERLINETYPE = 0x800000;
            public const int EM_GETCHARFORMAT = 0x43a;
            public const int EM_GETOLEINTERFACE = 0x43c;
            public const int EM_GETPARAFORMAT = 0x43d;
            public const int EM_GETTEXTLENGTHEX = 0x45f;
            public const int EM_OUTLINE = 0x4dc;
            public const int EM_SETCHARFORMAT = 0x444;
            public const int EM_SETEVENTMASK = 0x431;
            public const int EM_SETPARAFORMAT = 0x447;
            public const int EM_SETTYPOGRAPHYOPTIONS = 0x4ca;
            public const int EM_STOPGROUPTYPING = 0x458;
            public const int EM_UNDO = 0x304;
            public const int PFM_ALIGNMENT = 8;
            public const int SCF_SELECTION = 1;
            public const int TO_ADVANCEDTYPOGRAPHY = 1;
            public const short WM_KEYDOWN = 0x100;
            public const short WM_KEYUP = 0x101;
            public const short WM_PAINT = 15;
            public const int WM_RBUTTONUP = 0x215;
            public const int WM_SETREDRAW = 11;
            public const int WM_USER = 0x400;
            public const int WM_VSCROLL = 0x115;

            // Nested Types
            [StructLayout(LayoutKind.Sequential)]
            public struct CHARFORMAT
            {
                public int cbSize;
                public uint dwMask;
                public uint dwEffects;
                public int yHeight;
                public int yOffset;
                public int crTextColor;
                public byte bCharSet;
                public byte bPitchAndFamily;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
                public char[] szFaceName;
                public short wWeight;
                public short sSpacing;
                public int crBackColor;
                public int LCID;
                public uint dwReserved;
                public short sStyle;
                public short wKerning;
                public byte bUnderlineType;
                public byte bAnimation;
                public byte bRevAuthor;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct PARAFORMAT
            {
                public int cbSize;
                public uint dwMask;
                public short wNumbering;
                public short wReserved;
                public int dxStartIndent;
                public int dxRightIndent;
                public int dxOffset;
                public short wAlignment;
                public short cTabCount;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
                public int[] rgxTabs;
                public int dySpaceBefore;
                public int dySpaceAfter;
                public int dyLineSpacing;
                public short sStyle;
                public byte bLineSpacingRule;
                public byte bOutlineLevel;
                public short wShadingWeight;
                public short wShadingStyle;
                public short wNumberingStart;
                public short wNumberingStyle;
                public short wNumberingTab;
                public short wBorderSpace;
                public short wBorderWidth;
                public short wBorders;
            }
        }

        public class MouseUpEventArgs : EventArgs
        {
            // Fields
            public MouseButtons Button;
            public int X;
            public int Y;

            // Methods
            public MouseUpEventArgs(MouseButtons Button, int X, int Y)
            {
                this.Button = Button;
                this.X = X;
                this.Y = Y;
            }
        }

        public delegate void MYMouseRButtonUpEventHandler(object sender, MouseEventArgs args);

        [StructLayout(LayoutKind.Sequential)]
        private struct WordAndPosition
        {
            public string Word;
            public int Position;
            public int Length;
            public override string ToString()
            {
                return string.Concat(new object[] { "Word = ", this.Word, ", Position = ", this.Position, ", Length = ", this.Length, "\n" });
            }
        }
    }

}
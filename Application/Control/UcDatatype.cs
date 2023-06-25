using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Flextronics.Applications.ApplicationFactory.UserControls
{

    public class UcDatatype : UserControl
    {
        private IContainer components;
        //TODO 数据类型
        private string filename = (Application.StartupPath + @"\datatype.ini");
        //private string filename ="WebMatrix.Other.datatype.ini";

        private GroupBox groupBox1;
        private ToolTip toolTip1;
        private RichTextBox txtIniDatatype;

        string GetDataTypeIni(string file)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(file);
            Debug.Assert(stream != null, "Unable to load  for type '" + GetType().FullName + "'");
            int len = (int)stream.Length;
            byte[] bytes = new byte[len];
            stream.Read(bytes, 0, len);
            string entries = Encoding.Default.GetString(bytes);
            return entries;
        }
        public UcDatatype()
        {
            this.InitializeComponent();
            //TODO 数据类型
            //StreamReader reader = new StreamReader(this.filename, Encoding.Default);
             //string str = reader.ReadToEnd();
           // string str = GetDataTypeIni(this.filename);
            //reader.Close();

            StreamReader reader = new StreamReader(this.filename, Encoding.Default);
            string str = reader.ReadToEnd();            
            this.txtIniDatatype.Text = str;
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
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(UcDatatype));
            this.groupBox1 = new GroupBox();
            this.txtIniDatatype = new RichTextBox();
            this.toolTip1 = new ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.txtIniDatatype);
            this.groupBox1.Location = new Point(10, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x15f, 0xfc);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "字段类型映射";
            this.txtIniDatatype.Dock = DockStyle.Fill;
            this.txtIniDatatype.Location = new Point(3, 0x11);
            this.txtIniDatatype.Name = "txtIniDatatype";
            this.txtIniDatatype.Size = new Size(0x159, 0xe8);
            this.txtIniDatatype.TabIndex = 0;
            this.txtIniDatatype.Text = "";
           // this.toolTip1.SetToolTip(this.txtIniDatatype, manager.GetString("txtIniDatatype.ToolTip"));
            this.toolTip1.ToolTipTitle = "[DbToCS] 段，是数据库类型和C#类型的对应关系。";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.Controls.Add(this.groupBox1);
            base.Name = "UcDatatype";
            base.Size = new Size(0x170, 0x10b);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void SaveIni()
        {
            //TODO 数据类型 不支持写，保存
            //string[] lines = this.txtIniDatatype.Lines;
            //StreamWriter writer = new StreamWriter(this.filename, false, Encoding.Default);
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    if (lines[i].Trim() != "")
            //    {
            //        writer.WriteLine(lines[i]);
            //    }
            //}
            //writer.Close();
        }
    }
}


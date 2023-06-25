namespace Codematic
{
    using LTP.IDBO;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class DataList : Form
    {
        private IContainer components;
        private DataGridView dataGridView1;
        private string dbname;
        private IDbObject dbobj;
        private Thread mythread;
        private ToolStripStatusLabel StatusLabel_Count;
        private ToolStripStatusLabel StatusLabel_dbname;
        private ToolStripStatusLabel StatusLabel_rowcol;
        private ToolStripStatusLabel StatusLabel_time;
        private ToolStripStatusLabel StatusLabel_Tip;
        private StatusStrip statusStrip1;
        private string tabname;
        private System.Windows.Forms.Timer timer1;
        private int times;

        public DataList(IDbObject idbobj, string dbName, string tabName)
        {
            this.InitializeComponent();
            try
            {
                this.dbobj = idbobj;
                this.dbname = dbName;
                this.tabname = tabName;
                this.StatusLabel_dbname.Text = "库:" + this.dbname + "，表:" + this.tabname + "  ";
                this.mythread = new Thread(new ThreadStart(this.ThreadWork));
                this.mythread.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void BindData()
        {
            try
            {
                DataTable tabData = this.dbobj.GetTabData(this.dbname, this.tabname);
                this.dataGridView1.DataSource = tabData;
                this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                this.StatusLabel_Count.Text = tabData.Rows.Count + "行 ";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void BindDataList()
        {
            if (this.dataGridView1.InvokeRequired)
            {
                SetListCallback method = new SetListCallback(this.BindDataList);
                base.Invoke(method, null);
            }
            else
            {
                this.BindData();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string str = string.Format("{0},{1}", this.dataGridView1.CurrentCell.RowIndex, this.dataGridView1.CurrentCell.ColumnIndex);
            this.StatusLabel_rowcol.Text = str;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetTimestr(int times)
        {
            int num = 0;
            int num2 = 0;
            if (times > 60)
            {
                if (times > 0xe10)
                {
                    num = times / 0xe10;
                    num2 = (times - (num * 0xe10)) / 60;
                }
                else
                {
                    num2 = times / 60;
                }
            }
            int num3 = (times - (num * 0xe10)) - (num2 * 60);
            return (num.ToString("{00}") + ":" + num2.ToString("{00}") + ":" + num3.ToString("{00}"));
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.dataGridView1 = new DataGridView();
            this.statusStrip1 = new StatusStrip();
            this.StatusLabel_Tip = new ToolStripStatusLabel();
            this.StatusLabel_dbname = new ToolStripStatusLabel();
            this.StatusLabel_time = new ToolStripStatusLabel();
            this.StatusLabel_Count = new ToolStripStatusLabel();
            this.StatusLabel_rowcol = new ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((ISupportInitialize) this.dataGridView1).BeginInit();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 0x17;
            this.dataGridView1.Size = new Size(0x1ca, 0x152);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.StatusLabel_Tip, this.StatusLabel_dbname, this.StatusLabel_time, this.StatusLabel_Count, this.StatusLabel_rowcol });
            this.statusStrip1.Location = new Point(0, 0x152);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x1ca, 0x16);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.StatusLabel_Tip.BorderStyle = Border3DStyle.SunkenInner;
            this.StatusLabel_Tip.Name = "StatusLabel_Tip";
            this.StatusLabel_Tip.Size = new Size(0xdb, 0x11);
            this.StatusLabel_Tip.Spring = true;
            this.StatusLabel_Tip.Text = "就绪";
            this.StatusLabel_Tip.TextAlign = ContentAlignment.MiddleLeft;
            this.StatusLabel_dbname.Name = "StatusLabel_dbname";
            this.StatusLabel_dbname.Size = new Size(0x35, 0x11);
            this.StatusLabel_dbname.Text = "数据库：";
            this.StatusLabel_dbname.TextAlign = ContentAlignment.MiddleLeft;
            this.StatusLabel_time.Name = "StatusLabel_time";
            this.StatusLabel_time.Size = new Size(0x2f, 0x11);
            this.StatusLabel_time.Text = "0:00:00";
            this.StatusLabel_time.TextAlign = ContentAlignment.MiddleLeft;
            this.StatusLabel_Count.Name = "StatusLabel_Count";
            this.StatusLabel_Count.Size = new Size(0x17, 0x11);
            this.StatusLabel_Count.Text = "0行";
            this.StatusLabel_Count.TextAlign = ContentAlignment.MiddleLeft;
            this.StatusLabel_rowcol.AutoSize = false;
            this.StatusLabel_rowcol.Name = "StatusLabel_rowcol";
            this.StatusLabel_rowcol.Size = new Size(70, 0x11);
            this.StatusLabel_rowcol.Text = "行0，列0";
            this.timer1.Interval = 0x3e8;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1ca, 360);
            base.Controls.Add(this.dataGridView1);
            base.Controls.Add(this.statusStrip1);
            base.Name = "DataList";
            this.Text = "DataList";
            ((ISupportInitialize) this.dataGridView1).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ThreadWork()
        {
            try
            {
                this.timer1.Enabled = true;
                this.StatusLabel_Tip.Text = "正在查询，请稍候...";
                this.BindDataList();
                this.timer1.Enabled = false;
                this.StatusLabel_Tip.Text = "完成";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.times++;
            this.StatusLabel_time.Text = this.GetTimestr(this.times) + "  ";
        }

        private delegate void SetListCallback();
    }
}


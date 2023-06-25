namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    using Flextronics.Applications.ApplicationFactory;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UcOptionStartUp : UserControl
    {
        public CheckBox chb_isTimespan;
        public ComboBox cmb_StartUpItem;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label4;
        public NumericUpDown num_Time;
        private AppSettings settings;
        public TextBox txtStartUpPage;

        public UcOptionStartUp(AppSettings setting)
        {
            this.InitializeComponent();
            this.settings = setting;
        }

        private void cmb_StartUpItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cmb_StartUpItem.SelectedIndex)
            {
                case 0:
                    this.label1.Text = "启动页新闻频道(RSS)(&S):";
                    return;

                case 1:
                    break;

                case 2:
                    this.label1.Text = "主页地址(&H):";
                    break;

                default:
                    return;
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

        private void InitializeComponent()
        {
            this.chb_isTimespan = new CheckBox();
            this.label4 = new Label();
            this.cmb_StartUpItem = new ComboBox();
            this.label1 = new Label();
            this.txtStartUpPage = new TextBox();
            this.num_Time = new NumericUpDown();
            this.label2 = new Label();
            this.num_Time.BeginInit();
            base.SuspendLayout();
            this.chb_isTimespan.FlatStyle = FlatStyle.System;
            this.chb_isTimespan.Location = new Point(10, 0x5e);
            this.chb_isTimespan.Name = "chb_isTimespan";
            this.chb_isTimespan.Size = new Size(0x108, 0x18);
            this.chb_isTimespan.TabIndex = 10;
            this.chb_isTimespan.Text = "下载内容的时间间隔(&D)：";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "启动时(&P):";
            this.cmb_StartUpItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmb_StartUpItem.FormattingEnabled = true;
            this.cmb_StartUpItem.Items.AddRange(new object[] { "显示起始页", "显示空环境", "打开主页" });
            this.cmb_StartUpItem.Location = new Point(10, 0x17);
            this.cmb_StartUpItem.Name = "cmb_StartUpItem";
            this.cmb_StartUpItem.Size = new Size(0x157, 20);
            this.cmb_StartUpItem.TabIndex = 13;
            this.cmb_StartUpItem.SelectedIndexChanged += new EventHandler(this.cmb_StartUpItem_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x33);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x8f, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "启动页新闻频道(RSS)(&S):";
            this.txtStartUpPage.Location = new Point(10, 0x43);
            this.txtStartUpPage.Name = "txtStartUpPage";
            this.txtStartUpPage.Size = new Size(0x157, 0x15);
            this.txtStartUpPage.TabIndex = 14;
            //this.txtStartUpPage.Text = "http://ltp.cnblogs.com/Rss.aspx";
            //this.txtStartUpPage.Text = System.Configuration.ConfigurationManager.AppSettings["StartUpPage"];
            this.num_Time.Location = new Point(0x19, 0x75);
            int[] bits = new int[4];
            bits[0] = 360;
            this.num_Time.Maximum = new decimal(bits);
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.num_Time.Minimum = new decimal(numArray2);
            this.num_Time.Name = "num_Time";
            this.num_Time.Size = new Size(40, 0x15);
            this.num_Time.TabIndex = 15;
            int[] numArray3 = new int[4];
            numArray3[0] = 60;
            this.num_Time.Value = new decimal(numArray3);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x47, 0x79);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2f, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "分钟(&M)";
            base.Controls.Add(this.num_Time);
            base.Controls.Add(this.txtStartUpPage);
            base.Controls.Add(this.cmb_StartUpItem);
            base.Controls.Add(this.chb_isTimespan);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label4);
            base.Name = "UcOptionStartUp";
            base.Size = new Size(0x174, 0xb5);
            base.Load += new EventHandler(this.UcOptionStartUp_Load);
            this.num_Time.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void UcOptionStartUp_Load(object sender, EventArgs e)
        {
            string appStart = this.settings.AppStart;
            if (appStart != null)
            {
                if (!(appStart == "startuppage"))
                {
                    if (appStart == "blank")
                    {
                        this.cmb_StartUpItem.SelectedIndex = 1;
                    }
                    else if (appStart == "homepage")
                    {
                        this.cmb_StartUpItem.SelectedIndex = 2;
                    }
                }
                else
                {
                    this.cmb_StartUpItem.SelectedIndex = 0;
                }
            }
            this.txtStartUpPage.Text = this.settings.StartUpPage;
        }
    }
}


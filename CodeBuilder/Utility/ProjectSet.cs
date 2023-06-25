namespace Flextronics.Applications.ApplicationFactory
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    

   // public class ProjectSet : Form
   // {
   //     private Button btn_Add1;
   //     private Button btn_Add2;
   //     private Button btn_Cancel;
   //     private Button btn_Del1;
   //     private Button btn_Del2;
   //     private Button btn_OK;
   //     private Container components;
   //     private GroupBox groupbox2;
   //     private ListBox listBox1;
   //     private ListBox listBox2;
   //     private RadioButton radioButton1;
   //     private RadioButton radioButton2;
   //     public static ProSettings setting = new ProSettings();

   //     public ProjectSet()
   //     {
   //         this.InitializeComponent();
   //     }

   //     private void btn_Add1_Click(object sender, EventArgs e)
   //     {
   //         ProjectExpadd expadd = new ProjectExpadd();
   //         if (expadd.ShowDialog(this) == DialogResult.OK)
   //         {
   //             string item = expadd.textBox1.Text.Trim();
   //             this.listBox1.Items.Add(item);
   //         }
   //     }

   //     private void btn_Add2_Click(object sender, EventArgs e)
   //     {
   //         ProjectExpadd expadd = new ProjectExpadd();
   //         if (expadd.ShowDialog(this) == DialogResult.OK)
   //         {
   //             string item = expadd.textBox1.Text.Trim();
   //             this.listBox2.Items.Add(item);
   //         }
   //     }

   //     private void btn_Del1_Click(object sender, EventArgs e)
   //     {
   //         if (this.listBox1.SelectedItem != null)
   //         {
   //             this.listBox1.Items.Remove(this.listBox1.SelectedItem);
   //         }
   //     }

   //     private void btn_Del2_Click(object sender, EventArgs e)
   //     {
   //         if (this.listBox2.SelectedItem != null)
   //         {
   //             this.listBox2.Items.Remove(this.listBox2.SelectedItem);
   //         }
   //     }

   //     private void btn_OK_Click(object sender, EventArgs e)
   //     {
   //         this.SaveSet();
   //     }

   //     protected override void Dispose(bool disposing)
   //     {
   //         if (disposing && (this.components != null))
   //         {
   //             this.components.Dispose();
   //         }
   //         base.Dispose(disposing);
   //     }

   //     private void InitializeComponent()
   //     {
   //         this.groupbox2 = new GroupBox();
   //         this.btn_Add1 = new Button();
   //         this.listBox1 = new ListBox();
   //         this.radioButton1 = new RadioButton();
   //         this.radioButton2 = new RadioButton();
   //         this.listBox2 = new ListBox();
   //         this.btn_Del1 = new Button();
   //         this.btn_Add2 = new Button();
   //         this.btn_Del2 = new Button();
   //         this.btn_OK = new Button();
   //         this.btn_Cancel = new Button();
   //         this.groupbox2.SuspendLayout();
   //         base.SuspendLayout();
   //         this.groupbox2.Controls.Add(this.btn_Add1);
   //         this.groupbox2.Controls.Add(this.listBox1);
   //         this.groupbox2.Controls.Add(this.radioButton1);
   //         this.groupbox2.Controls.Add(this.radioButton2);
   //         this.groupbox2.Controls.Add(this.listBox2);
   //         this.groupbox2.Controls.Add(this.btn_Del1);
   //         this.groupbox2.Controls.Add(this.btn_Add2);
   //         this.groupbox2.Controls.Add(this.btn_Del2);
   //         this.groupbox2.Location = new Point(8, 8);
   //         this.groupbox2.Name = "groupbox2";
   //         this.groupbox2.Size = new Size(0x1c0, 0x88);
   //         this.groupbox2.TabIndex = 2;
   //         this.groupbox2.TabStop = false;
   //         this.groupbox2.Text = "文件规则";
   //     //    this.btn_Add1._Image = null;
   //         this.btn_Add1.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
   //     //    this.btn_Add1.DefaultScheme = false;
   //         this.btn_Add1.DialogResult = DialogResult.None;
   //         this.btn_Add1.Image = null;
   //         this.btn_Add1.Location = new Point(160, 40);
   //         this.btn_Add1.Name = "btn_Add1";
   //      //   this.btn_Add1.Scheme = Button.Schemes.Blue;
   //         this.btn_Add1.Size = new Size(0x39, 0x17);
   //         this.btn_Add1.TabIndex = 0x2c;
   //         this.btn_Add1.Text = "增加...";
   //         this.btn_Add1.Click += new EventHandler(this.btn_Add1_Click);
   //         this.listBox1.ItemHeight = 12;
   //         this.listBox1.Location = new Point(80, 0x10);
   //         this.listBox1.Name = "listBox1";
   //         this.listBox1.Size = new Size(0x48, 0x70);
   //         this.listBox1.TabIndex = 1;
   //         this.radioButton1.Location = new Point(0x10, 0x18);
   //         this.radioButton1.Name = "radioButton1";
   //         this.radioButton1.Size = new Size(0x40, 0x18);
   //         this.radioButton1.TabIndex = 0;
   //         this.radioButton1.Text = "筛选法";
   //         this.radioButton1.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
   //         this.radioButton2.Checked = true;
   //         this.radioButton2.Location = new Point(240, 0x18);
   //         this.radioButton2.Name = "radioButton2";
   //         this.radioButton2.Size = new Size(0x40, 0x18);
   //         this.radioButton2.TabIndex = 0;
   //         this.radioButton2.TabStop = true;
   //         this.radioButton2.Text = "排除法";
   //         this.radioButton2.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
   //         this.listBox2.ItemHeight = 12;
   //         this.listBox2.Location = new Point(0x130, 0x10);
   //         this.listBox2.Name = "listBox2";
   //         this.listBox2.Size = new Size(0x48, 0x70);
   //         this.listBox2.TabIndex = 1;
   //      //   this.btn_Del1._Image = null;
   //         this.btn_Del1.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
   //      //   this.btn_Del1.DefaultScheme = false;
   //         this.btn_Del1.DialogResult = DialogResult.None;
   //         this.btn_Del1.Image = null;
   //         this.btn_Del1.Location = new Point(160, 0x48);
   //         this.btn_Del1.Name = "btn_Del1";
   //    //     this.btn_Del1.Scheme = Button.Schemes.Blue;
   //         this.btn_Del1.Size = new Size(0x39, 0x17);
   //         this.btn_Del1.TabIndex = 0x2c;
   //         this.btn_Del1.Text = "移 除";
   //         this.btn_Del1.Click += new EventHandler(this.btn_Del1_Click);
   //   //      this.btn_Add2._Image = null;
   //         this.btn_Add2.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
   //  //       this.btn_Add2.DefaultScheme = false;
   //         this.btn_Add2.DialogResult = DialogResult.None;
   //         this.btn_Add2.Image = null;
   //         this.btn_Add2.Location = new Point(0x180, 40);
   //         this.btn_Add2.Name = "btn_Add2";
   //  //       this.btn_Add2.Scheme = Button.Schemes.Blue;
   //         this.btn_Add2.Size = new Size(0x39, 0x17);
   //         this.btn_Add2.TabIndex = 0x2c;
   //         this.btn_Add2.Text = "增加...";
   //         this.btn_Add2.Click += new EventHandler(this.btn_Add2_Click);
   ////         this.btn_Del2._Image = null;
   //         this.btn_Del2.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
   //  //       this.btn_Del2.DefaultScheme = false;
   //         this.btn_Del2.DialogResult = DialogResult.None;
   //         this.btn_Del2.Image = null;
   //         this.btn_Del2.Location = new Point(0x180, 0x48);
   //         this.btn_Del2.Name = "btn_Del2";
   // //        this.btn_Del2.Scheme = Button.Schemes.Blue;
   //         this.btn_Del2.Size = new Size(0x39, 0x17);
   //         this.btn_Del2.TabIndex = 0x2c;
   //         this.btn_Del2.Text = "移 除";
   //         this.btn_Del2.Click += new EventHandler(this.btn_Del2_Click);
   //  //       this.btn_OK._Image = null;
   //         this.btn_OK.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
   //  //       this.btn_OK.DefaultScheme = false;
   //         this.btn_OK.DialogResult = DialogResult.OK;
   //         this.btn_OK.Image = null;
   //         this.btn_OK.Location = new Point(0xe8, 160);
   //         this.btn_OK.Name = "btn_OK";
   //  //       this.btn_OK.Scheme = Button.Schemes.Blue;
   //         this.btn_OK.Size = new Size(0x41, 0x19);
   //         this.btn_OK.TabIndex = 0x30;
   //         this.btn_OK.Text = "确 定";
   //         this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
   //   //      this.btn_Cancel._Image = null;
   //         this.btn_Cancel.BackColor = Color.FromArgb(0, 0xd4, 0xd0, 200);
   //  //       this.btn_Cancel.DefaultScheme = false;
   //         this.btn_Cancel.DialogResult = DialogResult.Cancel;
   //         this.btn_Cancel.Image = null;
   //         this.btn_Cancel.Location = new Point(0x148, 160);
   //         this.btn_Cancel.Name = "btn_Cancel";
   ////         this.btn_Cancel.Scheme = Button.Schemes.Blue;
   //         this.btn_Cancel.Size = new Size(0x41, 0x19);
   //         this.btn_Cancel.TabIndex = 0x2f;
   //         this.btn_Cancel.Text = "取 消";
   //         this.AutoScaleBaseSize = new Size(6, 14);
   //         base.ClientSize = new Size(0x1d2, 0xc7);
   //         base.Controls.Add(this.btn_OK);
   //         base.Controls.Add(this.btn_Cancel);
   //         base.Controls.Add(this.groupbox2);
   //         base.FormBorderStyle = FormBorderStyle.FixedDialog;
   //         base.MaximizeBox = false;
   //         base.MinimizeBox = false;
   //         base.Name = "ProjectSet";
   //         base.StartPosition = FormStartPosition.CenterParent;
   //         this.Text = "设置规则";
   //         base.Load += new EventHandler(this.ProjectSet_Load);
   //         this.groupbox2.ResumeLayout(false);
   //         base.ResumeLayout(false);
   //     }

   //     private void LoadData()
   //     {
   //         setting = ProConfig.GetSettings();
   //         if (setting.Mode == "Filter")
   //         {
   //             this.radioButton1.Checked = true;
   //         }
   //         else
   //         {
   //             this.radioButton2.Checked = true;
   //         }
   //         foreach (string str in setting.FileExt.Split(new char[] { ';' }))
   //         {
   //             if (str.Trim() != "")
   //             {
   //                 this.listBox1.Items.Add(str);
   //             }
   //         }
   //         foreach (string str2 in setting.FileExtDel.Split(new char[] { ';' }))
   //         {
   //             if (str2.Trim() != "")
   //             {
   //                 this.listBox2.Items.Add(str2);
   //             }
   //         }
   //     }

   //     private void ProjectSet_Load(object sender, EventArgs e)
   //     {
   //         this.listBox1.Enabled = false;
   //         this.btn_Add1.Enabled = false;
   //         this.btn_Del1.Enabled = false;
   //         this.LoadData();
   //     }

   //     private void radioButton_CheckedChanged(object sender, EventArgs e)
   //     {
   //         if (this.radioButton1.Checked)
   //         {
   //             this.listBox1.Enabled = true;
   //             this.btn_Add1.Enabled = true;
   //             this.btn_Del1.Enabled = true;
   //             this.listBox2.Enabled = false;
   //             this.btn_Add2.Enabled = false;
   //             this.btn_Del2.Enabled = false;
   //         }
   //         else
   //         {
   //             this.listBox2.Enabled = true;
   //             this.btn_Add2.Enabled = true;
   //             this.btn_Del2.Enabled = true;
   //             this.listBox1.Enabled = false;
   //             this.btn_Add1.Enabled = false;
   //             this.btn_Del1.Enabled = false;
   //         }
   //     }

   //     private void SaveSet()
   //     {
   //         if (this.radioButton1.Checked)
   //         {
   //             setting.Mode = "Filter";
   //         }
   //         else
   //         {
   //             setting.Mode = "Del";
   //         }
   //         if (this.listBox1.Items.Count > 0)
   //         {
   //             string str = "";
   //             for (int i = 0; i < this.listBox1.Items.Count; i++)
   //             {
   //                 str = str + this.listBox1.Items[i].ToString() + ";";
   //             }
   //             setting.FileExt = str.Substring(0, str.Length - 1);
   //         }
   //         if (this.listBox2.Items.Count > 0)
   //         {
   //             string str2 = "";
   //             for (int j = 0; j < this.listBox2.Items.Count; j++)
   //             {
   //                 str2 = str2 + this.listBox2.Items[j].ToString() + ";";
   //             }
   //             setting.FileExtDel = str2.Substring(0, str2.Length - 1);
   //         }
   //         ProConfig.SaveSettings(setting);
   //     }
   // }
}


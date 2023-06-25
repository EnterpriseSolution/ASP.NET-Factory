using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Flextronics.Applications.ApplicationFactory
{  

    public class FrmPropertyForm : Form
    {
        private ComboBox comboBox1;
        private IContainer components;
        private PropertyGrid propertyGrid1;

        public FrmPropertyForm()
        {
            this.InitializeComponent();
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
            this.comboBox1 = new ComboBox();
            this.propertyGrid1 = new PropertyGrid();
            base.SuspendLayout();
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(0, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x124, 20);
            this.comboBox1.TabIndex = 0;
            this.propertyGrid1.Dock = DockStyle.Fill;
            this.propertyGrid1.Location = new Point(0, 20);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new Size(0x124, 290);
            this.propertyGrid1.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.ClientSize = new Size(0x124, 310);
            base.Controls.Add(this.propertyGrid1);
            base.Controls.Add(this.comboBox1);
            base.Name = "PropertyForm";
            this.Text = "属性";
            base.ResumeLayout(false);
        }
    }
}


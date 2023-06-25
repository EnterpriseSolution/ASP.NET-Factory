namespace Flextronics.Applications.ApplicationFactory.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UcDBSet : UserControl
    {
        private IContainer components;

        public UcDBSet()
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
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.Name = "UcDBSet";
            base.Size = new Size(0x170, 0xf8);
            base.ResumeLayout(false);
        }
    }
}


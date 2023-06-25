namespace Codematic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using System.Threading;

    public class SplashForm : Form
    {
        private Container components;
        internal Label Label1;
        internal PictureBox pbLogo;

        public SplashForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.SuspendLayout();
            // 
            // SplashForm
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(531, 175);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "SplashForm";
            this.Opacity = 0.86;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.ResumeLayout(false);

        }

        protected override void OnLoad(System.EventArgs e)
        {         
            Thread.Sleep(8000);
            base.OnLoad(e);
        }

        public void SplashForm_Load(object sender, EventArgs e)
        {

        }

    }
}


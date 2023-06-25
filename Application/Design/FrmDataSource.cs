using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebMatrix.Component
{
    public partial class FrmDataSource : Form
    {
        public FrmDataSource()
        {
            InitializeComponent();
        }

        public string dbtype
        { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            dbtype = "SQL2000";

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}

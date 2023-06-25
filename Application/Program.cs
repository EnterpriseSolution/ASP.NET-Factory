//<?xml version="1.0" encoding="utf-8"?>
//<configuration>
//  <appSettings>
//    <add key="RemoteSQLServerUri" value="127.0.0.1" />
//    <add key="RemoteSQLServerUser" value="sa" />
//    <add key="RemoteSQLServerPWD" value="123456" />
//    <add key="RemoteSQLServerDB" value="pubs" />
//  </appSettings>
//</configuration>

namespace Flextronics.Applications.ApplicationFactory
{
    using System;
    using System.Windows.Forms;
    using System.Threading;
    using Flextronics.Applications.Library.Utility;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
                   
            ThreadExceptionHandler handler = new ThreadExceptionHandler();           
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            MainForm mainForm = new MainForm();
            if (mainForm.mutex != null)
            {                
                Application.Run(mainForm);
            }
            else
            {
                MessageBox.Show(mainForm, "程序已经有一个实例在运行！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }      

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Application.Exit();
        }
    }
}


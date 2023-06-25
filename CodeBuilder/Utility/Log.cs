using System;
using System.IO;
using System.Windows.Forms;

namespace Flextronics.Applications.Library.Utility
{
  
    public static class Log
    {
        private static string cmcfgfile = (Application.StartupPath + @"\cmcfg.ini");
        private static string logfilename = (Application.StartupPath + @"\Log.txt");

        public static void WriteLog(Exception ex)
        {
            if (File.Exists(cmcfgfile))
            {
                INIFile file = new INIFile(cmcfgfile);
                if (file.IniReadValue("loginfo", "save").Trim() == "1")
                {
                    StreamWriter writer = new StreamWriter(logfilename, true);
                    writer.WriteLine(DateTime.Now.ToString() + ":");
                    writer.WriteLine("错误信息：" + ex.Message);
                    writer.WriteLine("Stack Trace:" + ex.StackTrace);
                    writer.WriteLine("");
                    writer.Close();
                }
            }
        }

        public static void WriteLog(string loginfo)
        {
            if (File.Exists(cmcfgfile))
            {
                INIFile file = new INIFile(cmcfgfile);
                if (file.IniReadValue("loginfo", "save").Trim() == "1")
                {
                    StreamWriter writer = new StreamWriter(logfilename, true);
                    writer.WriteLine(DateTime.Now.ToString() + ":" + loginfo);
                    writer.Close();
                }
            }
        }
    }
}


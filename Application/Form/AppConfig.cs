namespace Codematic
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class AppConfig
    {
        public static AppSettings GetSettings()
        {
            AppSettings settings = null;
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            try
            {
                //FileStream stream = new FileStream(Application.StartupPath + @"\appconfig.config", FileMode.Open);
                Stream stream = typeof(AppConfig).Assembly.GetManifestResourceStream("WebMatrix.Other.appconfig.config");
                settings = (AppSettings) serializer.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                settings = new AppSettings();
            }
            return settings;
        }

        public static void SaveSettings(AppSettings data)
        {
            //string path = Application.StartupPath + @"\appconfig.config";
            //XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            //FileStream stream = new FileStream(path, FileMode.Create);
            //serializer.Serialize((Stream) stream, data);
            //stream.Close();
        }
    }
}


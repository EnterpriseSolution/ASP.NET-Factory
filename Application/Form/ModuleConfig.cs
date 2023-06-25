namespace Codematic
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class ModuleConfig
    {
        
        public static ModuleSettings GetSettings()
        {
            ModuleSettings settings = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ModuleSettings));
            try
            {
                //Stream stream =Type.GetType("ModuleConfig").Assembly.GetManifestResourceStream("WebMatrix.Other.Config.xml");
                //TODO 20090729 改变存储方案

                Stream stream = typeof(ModuleConfig).Assembly.GetManifestResourceStream("WebMatrix.Other.Config.xml");


                //Debug.Assert(stream != null, "Unable to load  for type '" + GetType().FullName + "'");
                //int len = (int)stream.Length;
                //byte[] bytes = new byte[len];
                //stream.Read(bytes, 0, len);
                //string entries = Encoding.GetEncoding("utf-8").GetString(bytes);
                //return entries;

                //FileStream stream = new FileStream(Application.StartupPath + @"\config.xml", FileMode.Open);
                //end
                settings = (ModuleSettings) serializer.Deserialize(stream);
                stream.Close();
            }
            catch(Exception  ex)
            {
                string e = ex.ToString();
                settings = new ModuleSettings();
            }
            return settings;
        }

        public static void SaveSettings(ModuleSettings data)
        {
            //关闭保存功能
            //string path = Application.StartupPath + @"\config.xml";
            //XmlSerializer serializer = new XmlSerializer(typeof(ModuleSettings));
            //FileStream stream = new FileStream(path, FileMode.Create);
            //serializer.Serialize((Stream) stream, data);
            //stream.Close();
        }
    }
}


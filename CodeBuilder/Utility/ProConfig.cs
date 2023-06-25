namespace Flextronics.Applications.ApplicationFactory
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    //public class ProConfig
    //{
    //    public static ProSettings GetSettings()
    //    {
    //        ProSettings settings = null;
    //        XmlSerializer serializer = new XmlSerializer(typeof(ProSettings));
    //        try
    //        {
    //            string path = "ProConfig.config";
    //            FileStream stream = new FileStream(path, FileMode.Open);
    //            settings = (ProSettings) serializer.Deserialize(stream);
    //            stream.Close();
    //        }
    //        catch
    //        {
    //            settings = new ProSettings();
    //        }
    //        return settings;
    //    }

    //    public static void SaveSettings(ProSettings data)
    //    {
    //        string path = "ProConfig.config";
    //        XmlSerializer serializer = new XmlSerializer(typeof(ProSettings));
    //        FileStream stream = new FileStream(path, FileMode.Create);
    //        serializer.Serialize((Stream) stream, data);
    //        stream.Close();
    //    }
    //}
}


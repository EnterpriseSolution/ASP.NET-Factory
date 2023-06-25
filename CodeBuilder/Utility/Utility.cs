using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Runtime.Remoting.Messaging;

namespace Flextronics.Applications.Library.Utility
{

    public class Cache
    {
        // Fields
        protected Hashtable _Cache = new Hashtable();
        protected object _LockObj = new object();

        // Methods
        public virtual void Clear()
        {
            lock (this._Cache.SyncRoot)
            {
                this._Cache.Clear();
            }
        }

        public virtual void DelObject(object key)
        {
            lock (this._Cache.SyncRoot)
            {
                this._Cache.Remove(key);
            }
        }

        public virtual object GetObject(object key)
        {
            if (this._Cache.ContainsKey(key))
            {
                return this._Cache[key];
            }
            return null;
        }

        private void Results(IAsyncResult ar)
        {
            ((EventSaveCache)((AsyncResult)ar).AsyncDelegate).EndInvoke(ar);
        }

        public void SaveCache(object key, object value)
        {
            new EventSaveCache(this.SetCache).BeginInvoke(key, value, new AsyncCallback(this.Results), null);
        }

        protected virtual void SetCache(object key, object value)
        {
            lock (this._LockObj)
            {
                if (!this._Cache.ContainsKey(key))
                {
                    this._Cache.Add(key, value);
                }
            }
        }

        // Properties
        public int Count
        {
            get
            {
                return this._Cache.Count;
            }
        }
    }


    public sealed class ConfigHelper
    {
        // Methods
        public static bool GetConfigBool(string key)
        {
            bool flag = false;
            string configString = GetConfigString(key);
            if ((configString != null) && (string.Empty != configString))
            {
                try
                {
                    flag = bool.Parse(configString);
                }
                catch (FormatException)
                {
                }
            }
            return flag;
        }

        public static decimal GetConfigDecimal(string key)
        {
            decimal num = 0M;
            string configString = GetConfigString(key);
            if ((configString != null) && (string.Empty != configString))
            {
                try
                {
                    num = decimal.Parse(configString);
                }
                catch (FormatException)
                {
                }
            }
            return num;
        }

        public static int GetConfigInt(string key)
        {
            int num = 0;
            string configString = GetConfigString(key);
            if ((configString != null) && (string.Empty != configString))
            {
                try
                {
                    num = int.Parse(configString);
                }
                catch (FormatException)
                {
                }
            }
            return num;
        }

        public static string GetConfigString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }


    internal delegate void EventSaveCache(object key, object value);

    public class INIFile
    {
        // Fields
        public string path;

        // Methods
        public INIFile(string INIPath)
        {
            this.path = INIPath;
        }

        private string[] ByteToString(byte[] sectionByte)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(sectionByte).Split(new char[1]);
        }

        public void ClearAllSection()
        {
            this.IniWriteValue(null, null, null);
        }

        public void ClearSection(string Section)
        {
            this.IniWriteValue(Section, null, null);
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, byte[] retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder retVal = new StringBuilder(0xff);
            GetPrivateProfileString(Section, Key, "", retVal, 0xff, this.path);
            return retVal.ToString();
        }

        public string[] IniReadValues()
        {
            byte[] sectionByte = this.IniReadValues(null, null);
            return this.ByteToString(sectionByte);
        }

        public string[] IniReadValues(string Section)
        {
            byte[] sectionByte = this.IniReadValues(Section, null);
            return this.ByteToString(sectionByte);
        }

        public byte[] IniReadValues(string section, string key)
        {
            byte[] retVal = new byte[0xff];
            GetPrivateProfileString(section, key, "", retVal, 0xff, this.path);
            return retVal;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    }


    public class StringPlus
    {
        // Fields
        private StringBuilder str = new StringBuilder();

        // Methods
        public string Append(string Text)
        {
            this.str.Append(Text);
            return this.str.ToString();
        }

        public string AppendLine()
        {
            this.str.Append("\r\n");
            return this.str.ToString();
        }

        public string AppendLine(string Text)
        {
            this.str.Append(Text + "\r\n");
            return this.str.ToString();
        }

        public string AppendSpace(int SpaceNum, string Text)
        {
            this.str.Append(this.Space(SpaceNum));
            this.str.Append(Text);
            return this.str.ToString();
        }

        public string AppendSpaceLine(int SpaceNum, string Text)
        {
            this.str.Append(this.Space(SpaceNum));
            this.str.Append(Text);
            this.str.Append("\r\n");
            return this.str.ToString();
        }

        public void DelLastChar(string strchar)
        {
            string str = this.str.ToString();
            int length = str.LastIndexOf(strchar);
            if (length > 0)
            {
                this.str = new StringBuilder();
                this.str.Append(str.Substring(0, length));
            }
        }

        public void DelLastComma()
        {
            string str = this.str.ToString();
            int length = str.LastIndexOf(",");
            if (length > 0)
            {
                this.str = new StringBuilder();
                this.str.Append(str.Substring(0, length));
            }
        }

        public void Remove(int Start, int Num)
        {
            this.str.Remove(Start, Num);
        }

        public string Space(int SpaceNum)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < SpaceNum; i++)
            {
                builder.Append("\t");
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            return this.str.ToString();
        }

        // Properties
        public string Value
        {
            get
            {
                return this.str.ToString();
            }
        }
    }


    public class VSProject
    {
        // Methods
        public void AddClass(string filename, string classname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            string name = document.DocumentElement.FirstChild.Name;
            if (name != null)
            {
                if (!(name == "CSHARP"))
                {
                    if (!(name == "PropertyGroup"))
                    {
                        return;
                    }
                }
                else
                {
                    this.AddClass2003(filename, classname);
                    return;
                }
                this.AddClass2005(filename, classname);
            }
        }

        public void AddClass2003(string filename, string classname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.DocumentElement.ChildNodes)
            {
                foreach (XmlElement element2 in element)
                {
                    if (element2.Name == "Files")
                    {
                        foreach (XmlElement element3 in element2)
                        {
                            if (element3.Name == "Include")
                            {
                                XmlElement newChild = document.CreateElement("File", document.DocumentElement.NamespaceURI);
                                newChild.SetAttribute("RelPath", classname);
                                newChild.SetAttribute("SubType", "Code");
                                newChild.SetAttribute("BuildAction", "Compile");
                                element3.AppendChild(newChild);
                                break;
                            }
                        }
                        continue;
                    }
                }
            }
            document.Save(filename);
        }

        public void AddClass2005(string filename, string classname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.DocumentElement.ChildNodes)
            {
                if (element.Name == "ItemGroup")
                {
                    string innerText = element.ChildNodes[0].InnerText;
                    if (element.ChildNodes[0].Name == "Compile")
                    {
                        XmlElement newChild = document.CreateElement("Compile", document.DocumentElement.NamespaceURI);
                        newChild.SetAttribute("Include", classname);
                        element.AppendChild(newChild);
                        break;
                    }
                }
            }
            document.Save(filename);
        }

        public void AddClass2005Aspx(string filename, string aspxname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.DocumentElement.ChildNodes)
            {
                if (element.Name == "ItemGroup")
                {
                    string innerText = element.ChildNodes[0].InnerText;
                    if (element.ChildNodes[0].Name == "Compile")
                    {
                        XmlElement newChild = document.CreateElement("Compile", document.DocumentElement.NamespaceURI);
                        newChild.SetAttribute("Include", aspxname);
                        element.AppendChild(newChild);
                        break;
                    }
                }
            }
            document.Save(filename);
        }

        public void AddMethodToClass(string ClassFile, string strContent)
        {
            if (File.Exists(ClassFile))
            {
                string str = File.ReadAllText(ClassFile, Encoding.Default);
                if (str.IndexOf(" class ") > 0)
                {
                    int num = str.LastIndexOf("}");
                    int num2 = str.Substring(0, num - 1).LastIndexOf("}");
                    string str4 = str.Substring(0, num2 - 1) + "\r\n" + strContent + "\r\n}\r\n}";
                    StreamWriter writer = new StreamWriter(ClassFile, false, Encoding.Default);
                    writer.Write(str4);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }


}
 

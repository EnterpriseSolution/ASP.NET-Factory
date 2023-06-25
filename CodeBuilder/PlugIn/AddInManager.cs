//DataSet将内容写成XML字符串，再用DataSet.ReadXML读回将会报“路径中具有非法字符串的错误”。 
//解决的方案是：
////采用XMLTextWriter将DataSet的内容写入到Stream中
//System.IO.MemorySystem stream = new System.IO.MemorySystem();
//XMLTextWriter writer = new XMLTextWriter(stream,Encoding.UTF8);
//ds.Table[0].WriteXML(writer);
////将流stream转换成字符串
//int count = (int)stream.Length;
//byte[] arr = new byte[count];
//stream.Seek(0,System.IO.SeekOrigin.Begin);
//stream.Read(arr,0,count);
//UTF8Encoding utf = new UTF8Encoding();
//string xmlstr = utf.GetString(arr).Trim();
////再用DataSet.ReadXML读出
//DataSet ds1 = new DataSet();
//ds1.ReadXML(new System.IO.StringReader(xmlstr));

//CodeDAL.xml
//<?xml version="1.0" encoding="utf-8" ?>
//<Builders>
//  <Builder>
//    <Guid>94F2CF68-CECB-47b5-81D9-68BF5806ACF5</Guid>
//    <Name>Maticsoft BuilderDALSQL</Name>
//    <Decription>基于SQL方式</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>LTP.BuilderDALSQL.BuilderDAL</Classname>
//    <Version>2.1.0</Version>
//  </Builder>
//  <Builder>
//    <Guid>94F2CF68-CECB-47b5-81D9-68BF5806ACF2</Guid>
//    <Name>Maticsoft BuilderDALParam</Name>
//    <Decription>基于Parameter方式</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>LTP.BuilderDALParam.BuilderDAL</Classname>
//    <Version>2.1.0</Version>
//  </Builder>
//  <Builder>
//    <Guid>94F2CF68-CECB-47b5-81D9-68BF5806ACF1</Guid>
//    <Name>Maticsoft BuilderDALProc</Name>
//    <Decription>基于存储过程方式</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>LTP.BuilderDALProc.BuilderDAL</Classname>
//    <Version>2.1.0</Version>
//  </Builder>
//  <Builder>
//    <Guid>7CBA9B17-2D38-4D25-9B02-115487B38B9F</Guid>
//    <Name>BuilderDALELParam</Name>
//    <Decription>基于企业库Param方式</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>LTP.BuilderDALELParam.BuilderDAL</Classname>
//    <Version>2.0</Version>
//  </Builder>
//  <Builder>
//    <Guid>7DF51581-8A67-4524-ADBD-C272FD39E7D8</Guid>
//    <Name>BuilderDALELSQL</Name>
//    <Decription>基于企业库SQL方式</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>LTP.BuilderDALELSQL.BuilderDAL</Classname>
//    <Version>2.0</Version>
//  </Builder>
//  <Builder>
//    <Guid>6232952B-0598-4FE7-8E4A-DBBBE86B8676</Guid>
//    <Name>BuilderDAELLProc</Name>
//    <Decription>基于企业库Proc方式</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>LTP.BuilderDALELProc.BuilderDAL</Classname>
//    <Version>2.0</Version>
//  </Builder>
//  <Builder>
//    <Guid>490036AC-B954-474D-83AF-9739C126B332</Guid>
//    <Name>BuilderDALTranParam</Name>
//    <Decription>父子表生成</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>LTP.BuilderDALTranParam.BuilderDAL</Classname>
//    <Version>2.0</Version>
//  </Builder>
//  <Builder>
//    <Guid>18E25976-05E4-40BA-9227-5C339670E0F7</Guid>
//    <Name>BuilderBLLComm</Name>
//    <Decription>普通的业务层生成</Decription>
//    <Assembly>Library</Assembly>
//    <Classname>Flextronics.Applications.Library.BuilderBLL</Classname>
//    <Version>2.0</Version>
//  </Builder>
//</Builders>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Flextronics.Applications.Library.Utility;
using System.Reflection;

namespace Flextronics.Applications.Library.PlugIn
{
    public class PlugInEntity
    {
        public PlugInEntity() { }
        public PlugInEntity(string guid, string name, string description, string assembly, string className, string version)
        {
            GUID = guid;
            Name = name;
            Decription = description;
            Assembly = assembly;
            ClassName = className;
            Version = version;
        }
        private string _GUID;

        public string GUID
        {
            get { return _GUID; }
            set { _GUID = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


        private string _Decription;

        public string Decription
        {
            get { return _Decription; }
            set { _Decription = value; }
        }
        private string _Assembly;

        public string Assembly
        {
            get { return _Assembly; }
            set { _Assembly = value; }
        }
        private string _ClassName;

        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
        private string _Version;

        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

     
    }

    public class PlugInEntityCollection: List<PlugInEntity>
    {

    }

    public class PlugInManager
    {
        public static PlugInEntityCollection BuildPlugIn()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            string library = assem.GetName().Name; 
            string version="4.2";

            string sql="Flextronics.Applications.Library.CodeBuilder.SQL.";
            string enterpriseLibrary="Flextronics.Applications.Library.CodeBuilder.EnterpriseLibrary.";

            PlugInEntityCollection list = new PlugInEntityCollection();
            //Andy 先暂时关闭这些生成方式
            PlugInEntity p;
            //PlugInEntity p=new PlugInEntity("94F2CF68-CECB-47b5-81D9-68BF5806ACF5","BuilderDALSQL","基于SQL方式",library,sql+"BuilderDALSQL",version);
            //list.Add(p);
            //p=new PlugInEntity("94F2CF68-CECB-47b5-81D9-68BF5806ACF2","BuilderDALParam","基于Parameter方式",library,sql+"BuilderDALParam",version);
            //list.Add(p);
            //p=new PlugInEntity("94F2CF68-CECB-47b5-81D9-68BF5806ACF1","BuilderDALProcedure","基于存储过程方式",library,sql+"BuilderDALProcedure",version);
            //list.Add(p);

            p=new PlugInEntity("7DF51581-8A67-4524-ADBD-C272FD39E7D8","BuilderDALSQL","基于企业库SQL方式",library,enterpriseLibrary+"BuilderDALSQL",version);
            list.Add(p);
            p=new PlugInEntity("7CBA9B17-2D38-4D25-9B02-115487B38B9F","BuilderDALParam","基于企业库Param方式",library,enterpriseLibrary+"BuilderDALParam",version);
            list.Add(p);
            //p=new PlugInEntity("6232952B-0598-4FE7-8E4A-DBBBE86B8676","BuilderDALProcedure","基于企业库Proc方式",library,enterpriseLibrary+"BuilderDALProcedure",version);
            //list.Add(p);

            //p=new PlugInEntity("490036AC-B954-474D-83AF-9739C126B332","BuilderDALTranParam","父子表生成",library,"Flextronics.Applications.Library.CodeBuilder.BuilderDALTranParam",version);
            //list.Add(p);
            p = new PlugInEntity("18E25976-05E4-40BA-9227-5C339670E0F7", "BuilderBLL", "业务接口层生成", library, "Flextronics.Applications.Library.CodeBuilder.BuilderBLL", version);
            list.Add(p);                     
      
            return list;
        }
    }

    public class AddIn
    {
     
        private string _assembly;
        private string _classname;
        private string _desc;
        private string _guid;
        private string _name;
        private string _version;
        private Cache cache;
        private string fileAddin;

        //string GetAddInFile()
        //{
        //    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CodeDAL.xml");
        //}
        // Methods
        public AddIn()
        {
            this.fileAddin = Application.StartupPath + @"\CodeDAL.addin";
            this.cache = new Cache();
        }


        public AddIn(string AssemblyGuid)
        {
            #region
            //this.fileAddin = Application.StartupPath + @"\CodeDAL.addin";
            //this.cache = new Cache();
            //if (this.cache.GetObject(AssemblyGuid) == null)
            //{
            //    try
            //    {
            //        object addIn = this.GetAddIn(AssemblyGuid);
            //        if (addIn != null)
            //        {
            //            this.cache.SaveCache(AssemblyGuid, addIn);
            //            DataRow row = (DataRow)addIn;
            //            this._guid = row["Guid"].ToString();
            //            this._name = row["Name"].ToString();
            //            this._desc = row["Decription"].ToString();
            //            this._assembly = row["Assembly"].ToString();
            //            this._classname = row["Classname"].ToString();
            //            this._version = row["Version"].ToString();
            //        }
            //    }
            //    catch (Exception exception)
            //    {
            //        string message = exception.Message;
            //    }
            //}
#endregion
            PlugInEntityCollection list = GetAddInList();           
            foreach (PlugInEntity entity in list)
            {
                if (entity.GUID == AssemblyGuid)
                {
                    _guid = entity.GUID;
                    _name =entity.Name;
                    _desc = entity.Decription;
                    _assembly = entity.Assembly;
                    _classname = entity.ClassName;
                    _version = entity.Version;
                }
            }
        }

        //public void AddAddIn()
        //{
        //    DataSet set = new DataSet();         
        //    string str = GetAddInFile();         
        //    set.ReadXml(str);

        //    if (set.Tables.Count > 0)
        //    {
        //        DataRow row = set.Tables[0].NewRow();
        //        row["Guid"] = this._guid;
        //        row["Name"] = this._name;
        //        row["Decription"] = this._desc;
        //        row["Assembly"] = this._assembly;
        //        row["Classname"] = this._classname;
        //        row["Version"] = this._version;
        //        set.Tables[0].Rows.Add(row);

        //        XmlTextWriter writer = new XmlTextWriter(this.fileAddin, Encoding.Default);
        //        writer.WriteStartDocument();
        //        set.WriteXml(writer);
        //        writer.Close();
        //        //}
        //    }
        //}

        //public void DeleteAddIn(string AssemblyGuid)
        //{
        //    DataSet addInList = this.GetAddInList();
        //    if (addInList.Tables.Count > 0)
        //    {
        //        addInList.Tables[0].Select("Guid='" + AssemblyGuid + "'")[0].Delete();
        //        addInList.WriteXml(this.fileAddin);
        //    }
        //}

        private PlugInEntity GetAddIn(string AssemblyGuid)
        {
            //DataSet addInList = this.GetAddInList();
            //if (addInList.Tables.Count > 0)
            //{
            //    DataRow[] rowArray = addInList.Tables[0].Select("Guid='" + AssemblyGuid + "'");
            //    if (rowArray.Length > 0)
            //    {
            //        return rowArray[0];
            //    }
            //}
            //return null;
            PlugInEntityCollection list = GetAddInList();
            PlugInEntity  p=new PlugInEntity();
            foreach (PlugInEntity entity in list)
            {
                if (entity.GUID ==AssemblyGuid)
                {
                    p = entity;
                    break;
                }
            }
            return p;
        }

        //public DataRow GetAddInByCache(string AssemblyGuid)
        //{
        //    object addIn = this.cache.GetObject(AssemblyGuid);
        //    if (addIn == null)
        //    {
        //        try
        //        {
        //            addIn = this.GetAddIn(AssemblyGuid);
        //            this.cache.SaveCache(AssemblyGuid, addIn);
        //        }
        //        catch (Exception exception)
        //        {
        //            string message = exception.Message;
        //        }
        //    }
        //    return (DataRow)addIn;
        //}

        public PlugInEntityCollection GetAddInList()
        {
           PlugInEntityCollection  list=  PlugInManager.BuildPlugIn();
           return list;
            #region 
            //try
            //{
            //    DataSet set = new DataSet();              
            //    string str = GetAddInFile();           
            //    set.ReadXml(str);
            //    if (set.Tables.Count > 0)
            //    {
            //        return set;
            //    }               
            //    return null;
            //}
            //catch (SystemException exception)
            //{
            //    string message = exception.Message;
            //    return null;
            //}
            #endregion
        }

        public DataSet GetAddInList(string InterfaceName)
        {
            try
            {
                DataSet set = new DataSet();
               // string str = GetAddInFile();
                string str = "";

                set.ReadXml(str);
                if (set.Tables.Count > 0)
                {
                    List<DataRow> list = new List<DataRow>();
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        Type[] types = System.Reflection.Assembly.Load(row["Assembly"].ToString()).GetTypes();
                        bool flag = false;
                        foreach (Type type in types)
                        {
                            foreach (Type type2 in type.GetInterfaces())
                            {
                                if (type2.FullName == InterfaceName)
                                {
                                    flag = true;
                                }
                            }
                        }
                        if (!flag)
                        {
                            list.Add(row);
                        }
                    }
                    foreach (DataRow row2 in list)
                    {
                        set.Tables[0].Rows.Remove(row2);
                    }
                    return set;
                }
                //}
                return null;

            }
            catch (SystemException exception)
            {
                string message = exception.Message;
                return null;
            }
        }

        //public string LoadFile()
        //{
        //    return GetAddInFile();
        //}

        #region 
        public string Assembly
        {
            get
            {
                return this._assembly;
            }
            set
            {
                this._assembly = value;
            }
        }

        public string Classname
        {
            get
            {
                return this._classname;
            }
            set
            {
                this._classname = value;
            }
        }

        public string Decription
        {
            get
            {
                return this._desc;
            }
            set
            {
                this._desc = value;
            }
        }

        public string Guid
        {
            get
            {
                return this._guid;
            }
            set
            {
                this._guid = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }
        #endregion
    } 

}

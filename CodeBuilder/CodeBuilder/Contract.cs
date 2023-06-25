using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Reflection;
using Flextronics.Applications.Library.Schema;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.PlugIn;

namespace Flextronics.Applications.Library.CodeBuilder
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IBuilderAttribute : Attribute
    {
        // Fields
        private string _assembly;
        private string _classname;
        private string _desc;
        private string _guid;
        private string _name;
        private string _version;

        // Properties
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
    }
    public interface IBuilderBLL
    {
        // Methods
        string CreatBLLADD();
        string CreatBLLDelete();
        string CreatBLLExists();
        string CreatBLLGetList();
        string CreatBLLGetMaxID();
        string CreatBLLGetModel();
        string CreatBLLUpdate();
        string GetBLLCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List);

        // Properties
        string BLLpath { get; set; }
        string DALSpace { get; set; }
        string DbType { get; set; }
        string Factorypath { get; set; }
        List<ColumnInfo> Fieldlist { get; set; }
        string IClass { get; set; }
        string IDALpath { get; set; }
        bool IsHasIdentity { get; set; }
        List<ColumnInfo> Keys { get; set; }
        string ModelName { get; set; }
        string Modelpath { get; set; }
        string ModelSpace { get; set; }
        string NameSpace { get; set; }
    }
    public interface IBuilderDAL
    {
        // Methods
        string CreatAdd();
        string CreatDelete();
        string CreatExists();
        string CreatGetList();
        string CreatGetMaxID();
        string CreatGetModel();
        string CreatUpdate();
        string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List);

        // Properties
        string DALpath { get; set; }
        string DbHelperName { get; set; }
        string DbName { get; set; }
        IDbObject DbObject { get; set; }
        List<ColumnInfo> Fieldlist { get; set; }
        string Folder { get; set; }
        string IClass { get; set; }
        string IDALpath { get; set; }
        List<ColumnInfo> Keys { get; set; }
        string ModelName { get; set; }
        string Modelpath { get; set; }
        string ModelSpace { get; set; }
        string NameSpace { get; set; }
        string ProcPrefix { get; set; }
        string TableName { get; set; }
    }
    public interface IBuilderDALTran
    {
        // Methods
        string CreatAdd();
        string CreatDelete();
        string CreatExists();
        string CreatGetList();
        string CreatGetMaxID();
        string CreatGetModel();
        string CreatUpdate();
        string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List);

        // Properties
        string DALpath { get; set; }
        string DbHelperName { get; set; }
        string DbName { get; set; }
        IDbObject DbObject { get; set; }
        List<ColumnInfo> FieldlistParent { get; set; }
        List<ColumnInfo> FieldlistSon { get; set; }
        string Folder { get; set; }
        string IClass { get; set; }
        string IDALpath { get; set; }
        List<ColumnInfo> KeysParent { get; set; }
        List<ColumnInfo> KeysSon { get; set; }
        string ModelNameParent { get; set; }
        string ModelNameSon { get; set; }
        string Modelpath { get; set; }
        string ModelSpaceParent { get; set; }
        string ModelSpaceSon { get; set; }
        string NameSpace { get; set; }
        string ProcPrefix { get; set; }
        string TableNameParent { get; set; }
        string TableNameSon { get; set; }
    }
    public interface IBuilderModel
    {
        // Methods
        string CreatModel();
        string CreatModelMethod();

        // Properties
        List<ColumnInfo> Fieldlist { get; set; }
        string ModelName { get; set; }
        string Modelpath { get; set; }
        string NameSpace { get; set; }
    }
    public interface IBuilderWeb
    {
        // Methods
        string CreatDeleteForm();
        string CreatSearchForm();
        string GetAddAspx();
        string GetAddAspxCs();
        string GetAddDesigner();
        string GetShowAspx();
        string GetShowAspxCs();
        string GetShowDesigner();
        string GetUpdateAspx();
        string GetUpdateAspxCs();
        string GetUpdateDesigner();
        string GetUpdateShowAspxCs();
        string GetWebCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm);
        string GetWebHtmlCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm);

        // Properties
        List<ColumnInfo> Fieldlist { get; set; }
        string Folder { get; set; }
        List<ColumnInfo> Keys { get; set; }
        string ModelName { get; set; }
        string NameSpace { get; set; }
    }

    public class BuilderFactory
    {
        // Fields
        private static Cache cache = new Cache();

        // Methods
        public static IBuilderBLL CreateBLLObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn @in = new AddIn(AssemblyGuid);
                string assembly = @in.Assembly;
                string classname = @in.Classname;
                return (IBuilderBLL)CreateObject(assembly, classname);
            }
            catch (SystemException exception)
            {
                string message = exception.Message;
                return null;
            }
        }

        public static IBuilderDAL CreateDALObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn @in = new AddIn(AssemblyGuid);
                string assembly = @in.Assembly;
                string classname = @in.Classname;
                return (IBuilderDAL)CreateObject(assembly, classname);
            }
            catch (SystemException exception)
            {
                string message = exception.Message;
                return null;
            }
        }

        public static IBuilderDALTran CreateDALTranObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn @in = new AddIn(AssemblyGuid);
                string assembly = @in.Assembly;
                string classname = @in.Classname;
                return (IBuilderDALTran)CreateObject(assembly, classname);
            }
            catch (SystemException exception)
            {
                string message = exception.Message;
                return null;
            }
        }

        public static IBuilderModel CreateModelObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn @in = new AddIn(AssemblyGuid);
                string assembly = @in.Assembly;
                string classname = @in.Classname;
                return (IBuilderModel)CreateObject(assembly, classname);
            }
            catch (SystemException exception)
            {
                string message = exception.Message;
                return null;
            }
        }

        private static object CreateObject(string path, string TypeName)
        {
            object obj2 = cache.GetObject(TypeName);
            if (obj2 == null)
            {
                try
                {
                    obj2 = Assembly.Load(path).CreateInstance(TypeName);
                    cache.SaveCache(TypeName, obj2);
                }
                catch (Exception exception)
                {
                    string message = exception.Message;
                }
            }
            return obj2;
        }

        public static IBuilderWeb CreateWebObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn @in = new AddIn(AssemblyGuid);
                string assembly = @in.Assembly;
                string classname = @in.Classname;
                return (IBuilderWeb)CreateObject(assembly, classname);
            }
            catch (SystemException exception)
            {
                string message = exception.Message;
                return null;
            }
        }
    }
    
    public class BuilderFrame
    {
        // Fields
        private string _bllpath;
        private string _bllspace;
        private string _dalpath;
        private string _dalspace;
        private string _dbhelperName;
        private string _dbname;
        protected string _dbtype;
        private string _factoryclass;
        private List<ColumnInfo> _fieldlist;
        private string _folder;
        private string _idalpath;
        protected string _key = "ID";
        private List<ColumnInfo> _keys;
        protected string _keyType = "int";
        private string _modelname;
        private string _modelpath;
        private string _modelspace;
        private string _namespace = "Maticsoft";
        private string _tablename;
        protected IDbObject dbobj;

        // Methods
        public string GetFieldslist(DataTable dt)
        {
            StringPlus plus = new StringPlus();
            foreach (DataRow row in dt.Rows)
            {
                plus.Append("[" + row["ColumnName"].ToString() + "],");
            }
            plus.DelLastComma();
            return plus.Value;
        }

        public string GetkeyParalist(Hashtable Keys)
        {
            StringPlus plus = new StringPlus();
            foreach (DictionaryEntry entry in Keys)
            {
                plus.Append(CodeCommon.DbTypeToCS(entry.Value.ToString()) + " " + entry.Key.ToString() + ",");
            }
            if (plus.Value.IndexOf(",") > 0)
            {
                plus.DelLastComma();
            }
            return plus.Value;
        }

        public string GetkeyWherelist(Hashtable Keys)
        {
            StringPlus plus = new StringPlus();
            int num = 0;
            foreach (DictionaryEntry entry in Keys)
            {
                num++;
                if (CodeCommon.IsAddMark(entry.Value.ToString()))
                {
                    plus.Append(entry.Key.ToString() + "='\"+" + entry.Key.ToString() + "+\"'\"");
                }
                else
                {
                    plus.Append(entry.Key.ToString() + "=\"+" + entry.Key.ToString() + "+\"");
                    if (num == Keys.Count)
                    {
                        plus.Append("\"");
                    }
                }
                plus.Append(" and ");
            }
            if (plus.Value.IndexOf("and") > 0)
            {
                plus.DelLastChar("and");
            }
            return plus.Value;
        }

        public string GetkeyWherelistProc(Hashtable Keys)
        {
            StringPlus plus = new StringPlus();
            foreach (DictionaryEntry entry in Keys)
            {
                plus.Append(entry.Key.ToString() + "=@" + entry.Key.ToString());
                plus.Append(" and ");
            }
            if (plus.Value.IndexOf("and") > 0)
            {
                plus.DelLastChar("and");
            }
            return plus.Value;
        }

        public string Space(int num)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                builder.Append("\t");
            }
            return builder.ToString();
        }

        // Properties
        public string BLLpath
        {
            get
            {
                //string str = this._namespace + ".BLL";
                //if (this._folder.Trim() != "")
                //{
                //    str = str + "." + this._folder;
                //}
                //return str;

                string path = "Service";
                if (_namespace.Length > 0)
                    path = _namespace + "." + path;
                if (_folder.Length > 0)
                    path = path + "." + _folder;
                return path;                    
            }
            set
            {
                this._bllpath = value;
            }
        }

        public string BLLSpace
        {
            get
            {
                //this._bllspace = this._namespace + ".BLL";
                //if (this._folder.Trim() != "")
                //{
                //    this._bllspace = this._bllspace + "." + this._folder;
                //}
                //this._bllspace = this._bllspace + "." + this._modelname;
                //return this._bllspace;

                string path = "Service";
                if (_namespace.Length > 0)
                    path = _namespace + "." + path;
                if (_folder.Length > 0)
                    path = path + "." + _folder;
                path = path + "." + _modelname;
                return path;     
            }
        }

        public string DALpath
        {
            get
            {
                //string str = this._dbtype;
                //if ((this._dbtype == "SQL2000") || (this._dbtype == "SQL2005"))
                //{
                //    str = "SQLServer";
                //}
                //this._dalpath = this._namespace + "." + str + "DAL";
                //if (this._folder.Trim() != "")
                //{
                //    this._dalpath = this._dalpath + "." + this._folder;
                //}
                //return this._dalpath;

                string str = this._dbtype;
                if ((this._dbtype == "SQL2000") || (this._dbtype == "SQL2005"))
                {
                    str = "SQLServer";
                }                
                _dalpath = this._namespace + "." + str + "DAL";
                if (this._folder.Trim() != "")
                {
                    this._dalpath = this._dalpath + "." + this._folder;
                }
                return this._dalpath;           

            }
            set
            {
                this._dalpath = value;
            }
        }

        public string DALSpace
        {
            get
            {
                string str = this._dbtype;
                if ((this._dbtype == "SQL2000") || (this._dbtype == "SQL2005"))
                {
                    str = "SQLServer";
                }
                this._dalspace = this.NameSpace + "." + str + "DAL";
                if (this.Folder.Trim() != "")
                {
                    this._dalspace = this._dalspace + "." + this.Folder;
                }
                this._dalspace = this._dalspace + "." + this.ModelName;
                return this._dalspace;
            }
        }

        public string DbHelperName
        {
            get
            {
                return this._dbhelperName;
            }
            set
            {
                this._dbhelperName = value;
            }
        }

        public string DbName
        {
            get
            {
                return this._dbname;
            }
            set
            {
                this._dbname = value;
            }
        }

        public string DbParaDbType
        {
            get
            {
                switch (this.dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                        return "SqlDbType";

                    case "Oracle":
                        return "OracleType";

                    case "MySQL":
                        return "MySqlDbType";

                    case "OleDb":
                        return "OleDbType";
                }
                return "SqlDbType";
            }
        }

        public string DbParaHead
        {
            get
            {
                switch (this.dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                        return "Sql";

                    case "Oracle":
                        return "Oracle";

                    case "MySQL":
                        return "MySql";

                    case "OleDb":
                        return "OleDb";
                }
                return "Sql";
            }
        }

        public string Factorypath
        {
            get
            {
                return (this._namespace + ".DALFactory");
            }
        }

        public List<ColumnInfo> Fieldlist
        {
            get
            {
                return this._fieldlist;
            }
            set
            {
                this._fieldlist = value;
            }
        }

        public string Fields
        {
            get
            {
                StringPlus plus = new StringPlus();
                foreach (object obj2 in this.Fieldlist)
                {
                    plus.Append("'" + obj2.ToString() + "',");
                }
                plus.DelLastComma();
                return plus.Value;
            }
        }

        public string Fieldstrlist
        {
            get
            {
                StringPlus plus = new StringPlus();
                foreach (ColumnInfo info in this.Fieldlist)
                {
                    plus.Append(info.ColumnName + ",");
                }
                plus.DelLastComma();
                return plus.Value;
            }
        }

        public string Folder
        {
            get
            {
                return this._folder;
            }
            set
            {
                this._folder = value;
            }
        }

        public string IClass
        {
            get
            {
                return ("I" + this._modelname);
            }
        }

        public string IDALpath
        {
            get
            {
                this._idalpath = this._namespace + ".IDAL";
                if (this._folder.Trim() != "")
                {
                    this._idalpath = this._idalpath + "." + this._folder;
                }
                return this._idalpath;
            }
        }

        public bool IsHasIdentity
        {
            get
            {
                bool flag = false;
                if (this.Keys.Count > 0)
                {
                    foreach (ColumnInfo info in this.Keys)
                    {
                        if (info.IsIdentity)
                        {
                            flag = true;
                        }
                    }
                }
                return flag;
            }
        }

        public string Key
        {
            get
            {
                foreach (ColumnInfo info in this._keys)
                {
                    this._key = info.ColumnName;
                    this._keyType = info.TypeName;
                    if (info.IsIdentity)
                    {
                        this._key = info.ColumnName;
                        this._keyType = CodeCommon.DbTypeToCS(info.TypeName);
                        break;
                    }
                }
                return this._key;
            }
        }

        public List<ColumnInfo> Keys
        {
            get
            {
                return this._keys;
            }
            set
            {
                this._keys = value;
            }
        }

        public string ModelName
        {
            get
            {
                return this._modelname;
            }
            set
            {
                this._modelname = value;
            }
        }

        public string Modelpath
        {
            get
            {
                //重写生成空间
                //this._modelpath = this._namespace + ".Model";
                //if (this._folder.Trim() != "")
                //{
                //    this._modelpath = this._modelpath + "." + this._folder;
                //}
                //return this._modelpath;
                string path = "BusinessEntity";
                if (_namespace.Length > 0)
                    path = _namespace + "." + path;
                if (_folder.Length > 0)
                    path = _folder +"."+ path;
                return path;
            }
            set
            {
                this._modelpath = value;
            }
        }

        public string ModelSpace
        {
            get
            {
                //this._modelspace = this._namespace + ".Model";
                //if (this._folder.Trim() != "")
                //{
                //    this._modelspace = this._modelspace + "." + this._folder;
                //}
                //this._modelspace = this._modelspace + "." + this._modelname;
                //return this._modelspace;

                string path = "BusinessEntity";
                if (_namespace.Length > 0)
                    path = _namespace + "." + path;
                if (_folder.Length > 0)
                    path = _folder + "." + path;
                return path;
            }
            set
            {
                this._modelspace = value;
            }
        }

        public string NameSpace
        {
            get
            {
                return this._namespace;
            }
            set
            {
                this._namespace = value;
            }
        }

        public string TableName
        {
            get
            {
                return this._tablename;
            }
            set
            {
                this._tablename = value;
            }
        }
    }

    public class BuilderFrameF3 : BuilderFrame
    {
        // Fields
        private IBuilderBLL ibll;
        private IBuilderDAL idal;
        private IBuilderDALTran idaltran;

        // Methods
        public BuilderFrameF3(IDbObject idbobj, string dbName, string nameSpace, string folder, string dbHelperName)
        {
            base.dbobj = idbobj;
            base._dbtype = idbobj.DbType;
            base.DbName = dbName;
            base.NameSpace = nameSpace;
            base.DbHelperName = dbHelperName;
            base.Folder = folder;
        }

        public BuilderFrameF3(IDbObject idbobj, string dbName, string tableName, string modelName, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string nameSpace, string folder, string dbHelperName)
        {
            base.dbobj = idbobj;
            base._dbtype = idbobj.DbType;
            base.DbName = dbName;
            base.TableName = tableName;
            base.ModelName = modelName;
            base.NameSpace = nameSpace;
            base.DbHelperName = dbHelperName;
            base.Folder = folder;
            base.Fieldlist = fieldlist;
            base.Keys = keys;
            foreach (ColumnInfo info in keys)
            {
                base._key = info.ColumnName;
                base._keyType = info.TypeName;
                if (info.IsIdentity)
                {
                    base._key = info.ColumnName;
                    base._keyType = CodeCommon.DbTypeToCS(info.TypeName);
                    break;
                }
            }
        }

        public string GetBLLCode(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List, bool ListProc)
        {
            this.ibll = BuilderFactory.CreateBLLObj(AssemblyGuid);
            if (this.ibll == null)
            {
                return "请选择有效的业务层代码组件类型！";
            }
            this.ibll.Fieldlist = base.Fieldlist;
            this.ibll.Keys = base.Keys;
            this.ibll.NameSpace = base.NameSpace;
            this.ibll.ModelSpace = base.ModelSpace;
            this.ibll.ModelName = base.ModelName;
            this.ibll.Modelpath = base.Modelpath;
            this.ibll.BLLpath = base.BLLpath;
            this.ibll.Factorypath = base.Factorypath;
            this.ibll.IDALpath = base.IDALpath;
            this.ibll.IClass = base.IClass;
            this.ibll.DALSpace = "";
            this.ibll.IsHasIdentity = base.IsHasIdentity;
            this.ibll.DbType = base.dbobj.DbType;
            return this.ibll.GetBLLCode(Maxid, Exists, Add, Update, Delete, GetModel, GetModelByCache, List);
        }

        public string GetDALCode(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, string procPrefix)
        {
            this.idal = BuilderFactory.CreateDALObj(AssemblyGuid);
            if (this.idal == null)
            {
                return "请选择有效的数据层代码组件类型！";
            }
            this.idal.DbObject = base.dbobj;
            this.idal.DbName = base.DbName;
            this.idal.TableName = base.TableName;
            this.idal.ModelName = base.ModelName;
            this.idal.NameSpace = base.NameSpace;
            this.idal.DbHelperName = base.DbHelperName;
            this.idal.Folder = base.Folder;
            this.idal.Fieldlist = base.Fieldlist;
            this.idal.Keys = base.Keys;
            this.idal.Modelpath = base.Modelpath;
            this.idal.ModelSpace = base.ModelSpace;
            this.idal.DALpath = base.DALpath;
            this.idal.IDALpath = base.IDALpath;
            this.idal.IClass = base.IClass;
            this.idal.ProcPrefix = procPrefix;
            return this.idal.GetDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);
        }

        public string GetDALCodeTran(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, string procPrefix, string tableNameParent, string tableNameSon, string modelNameParent, string modelNameSon, List<ColumnInfo> fieldlistParent, List<ColumnInfo> fieldlistSon, List<ColumnInfo> keysParent, List<ColumnInfo> keysSon, string modelSpaceParent, string modelSpaceSon)
        {
            this.idaltran = BuilderFactory.CreateDALTranObj(AssemblyGuid);
            if (this.idaltran == null)
            {
                return "请选择有效的数据层代码组件类型！";
            }
            this.idaltran.DbObject = base.dbobj;
            this.idaltran.DbName = base.DbName;
            this.idaltran.TableNameParent = tableNameParent;
            this.idaltran.TableNameSon = tableNameSon;
            this.idaltran.ModelNameParent = modelNameParent;
            this.idaltran.ModelNameSon = modelNameSon;
            this.idaltran.NameSpace = base.NameSpace;
            this.idaltran.DbHelperName = base.DbHelperName;
            this.idaltran.Folder = base.Folder;
            this.idaltran.FieldlistParent = fieldlistParent;
            this.idaltran.FieldlistSon = fieldlistSon;
            this.idaltran.KeysParent = keysParent;
            this.idaltran.KeysSon = keysSon;
            this.idaltran.Modelpath = base.Modelpath;
            this.idaltran.ModelSpaceParent = modelSpaceParent;
            this.idaltran.ModelSpaceSon = modelSpaceSon;
            this.idaltran.DALpath = base.DALpath;
            this.idaltran.IDALpath = base.IDALpath;
            this.idaltran.IClass = base.IClass;
            this.idaltran.ProcPrefix = procPrefix;
            return this.idaltran.GetDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);
        }

        public string GetDALFactoryCode()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("using System;\r\n");
            builder.Append("using System.Reflection;\r\n");
            builder.Append("using System.Configuration;\r\n");
            builder.Append("using " + base.IDALpath + ";\r\n");
            builder.Append("namespace " + base.Factorypath + "\r\n");
            builder.Append("{\r\n");
            builder.Append(base.Space(1) + "/// <summary>\r\n");
            builder.Append(base.Space(1) + "/// 抽象工厂模式创建DAL。\r\n");
            builder.Append(base.Space(1) + "/// web.config 需要加入配置：(利用工厂模式+反射机制+缓存机制,实现动态创建不同的数据层对象接口)  \r\n");
            builder.Append(base.Space(1) + "/// DataCache类在导出代码的文件夹里\r\n");
            builder.Append(base.Space(1) + "/// <appSettings>  \r\n");
            builder.Append(base.Space(1) + "/// <add key=\"DAL\" value=\"" + base.DALpath + "\" /> (这里的命名空间根据实际情况更改为自己项目的命名空间)\r\n");
            builder.Append(base.Space(1) + "/// </appSettings> \r\n");
            builder.Append(base.Space(1) + "/// </summary>\r\n");
            builder.Append(base.Space(1) + "public sealed class DataAccess\r\n");
            builder.Append(base.Space(1) + "{\r\n");
            builder.Append(base.Space(2) + "private static readonly string AssemblyPath = ConfigurationManager.AppSettings[\"DAL\"];\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 创建对象或从缓存获取\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            builder.Append(base.Space(2) + "public static object CreateObject(string AssemblyPath,string ClassNamespace)\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "object objType = DataCache.GetCache(ClassNamespace);//从缓存读取\r\n");
            builder.Append(base.Space(3) + "if (objType == null)\r\n");
            builder.Append(base.Space(3) + "{\r\n");
            builder.Append(base.Space(4) + "try\r\n");
            builder.Append(base.Space(4) + "{\r\n");
            builder.Append(base.Space(5) + "objType = Assembly.Load(AssemblyPath).CreateInstance(ClassNamespace);//反射创建\r\n");
            builder.Append(base.Space(5) + "DataCache.SetCache(ClassNamespace, objType);// 写入缓存\r\n");
            builder.Append(base.Space(4) + "}\r\n");
            builder.Append(base.Space(4) + "catch\r\n");
            builder.Append(base.Space(4) + "{}\r\n");
            builder.Append(base.Space(3) + "}\r\n");
            builder.Append(base.Space(3) + "return objType;\r\n");
            builder.Append(base.Space(2) + "}\r\n");
            builder.Append(this.GetDALFactoryMethodCode());
            builder.Append(base.Space(1) + "}\r\n");
            builder.Append("}\r\n");
            builder.Append("\r\n");
            return builder.ToString();
        }

        public string GetDALFactoryMethodCode()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 创建" + base.ModelName + "数据层接口");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public static " + base.IDALpath + "." + base.IClass + " Create" + base.ModelName + "()");
            plus.AppendSpaceLine(2, "{\r\n");
            if (base.Folder != "")
            {
                plus.AppendSpaceLine(3, "string ClassNamespace = AssemblyPath +\"." + base.Folder + "." + base.ModelName + "\";");
            }
            else
            {
                plus.AppendSpaceLine(3, "string ClassNamespace = AssemblyPath +\"." + base.ModelName + "\";");
            }
            plus.AppendSpaceLine(3, "object objType=CreateObject(AssemblyPath,ClassNamespace);");
            plus.AppendSpaceLine(3, "return (" + base.IDALpath + "." + base.IClass + ")objType;");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        public string GetIDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, bool ListProc)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("using System;\r\n");
            builder.Append("using System.Data;\r\n");
            builder.Append("namespace " + base.IDALpath + "\r\n");
            builder.Append("{\r\n");
            builder.Append("\t/// <summary>\r\n");
            builder.Append("\t/// 接口层" + base.IClass + " 的摘要说明。\r\n");
            builder.Append("\t/// </summary>\r\n");
            builder.Append("\tpublic interface " + base.IClass + "\r\n");
            builder.Append("\t{\r\n");
            builder.Append(base.Space(2) + "#region  成员方法\r\n");
            if (Maxid && (base.Keys.Count > 0))
            {
                foreach (ColumnInfo info in base.Keys)
                {
                    if ((CodeCommon.DbTypeToCS(info.TypeName) == "int") && info.IsPK)
                    {
                        builder.Append(base.Space(2) + "/// <summary>\r\n");
                        builder.Append(base.Space(2) + "/// 得到最大ID\r\n");
                        builder.Append(base.Space(2) + "/// </summary>\r\n");
                        builder.Append("\t\tint GetMaxId();\r\n");
                        break;
                    }
                }
            }
            if (Exists && (base.Keys.Count > 0))
            {
                builder.Append(base.Space(2) + "/// <summary>\r\n");
                builder.Append(base.Space(2) + "/// 是否存在该记录\r\n");
                builder.Append(base.Space(2) + "/// </summary>\r\n");
                builder.Append("\t\tbool Exists(" + CodeCommon.GetInParameter(base.Keys) + ");\r\n");
            }
            if (Add)
            {
                builder.Append(base.Space(2) + "/// <summary>\r\n");
                builder.Append(base.Space(2) + "/// 增加一条数据\r\n");
                builder.Append(base.Space(2) + "/// </summary>\r\n");
                string str = "void";
                if (base.IsHasIdentity)
                {
                    str = "int";
                }
                builder.Append("\t\t" + str + " Add(" + base.ModelSpace + " model);\r\n");
            }
            if (Update)
            {
                builder.Append(base.Space(2) + "/// <summary>\r\n");
                builder.Append(base.Space(2) + "/// 更新一条数据\r\n");
                builder.Append(base.Space(2) + "/// </summary>\r\n");
                builder.Append("\t\tvoid Update(" + base.ModelSpace + " model);\r\n");
            }
            if (Delete)
            {
                builder.Append(base.Space(2) + "/// <summary>\r\n");
                builder.Append(base.Space(2) + "/// 删除一条数据\r\n");
                builder.Append(base.Space(2) + "/// </summary>\r\n");
                builder.Append("\t\tvoid Delete(" + CodeCommon.GetInParameter(base.Keys) + ");\r\n");
            }
            if (GetModel)
            {
                builder.Append(base.Space(2) + "/// <summary>\r\n");
                builder.Append(base.Space(2) + "/// 得到一个对象实体\r\n");
                builder.Append(base.Space(2) + "/// </summary>\r\n");
                builder.Append("\t\t" + base.ModelSpace + " GetModel(" + CodeCommon.GetInParameter(base.Keys) + ");\r\n");
            }
            if (List)
            {
                builder.Append(base.Space(2) + "/// <summary>\r\n");
                builder.Append(base.Space(2) + "/// 获得数据列表\r\n");
                builder.Append(base.Space(2) + "/// </summary>\r\n");
                builder.Append("\t\tDataSet GetList(string strWhere);\r\n");
            }
            if (ListProc)
            {
                builder.Append(base.Space(2) + "/// <summary>\r\n");
                builder.Append(base.Space(2) + "/// 根据分页获得数据列表\r\n");
                builder.Append(base.Space(2) + "/// </summary>\r\n");
                builder.Append("//\t\tDataSet GetList(int PageSize,int PageIndex,string strWhere);\r\n");
            }
            builder.Append(base.Space(2) + "#endregion  成员方法\r\n");
            builder.Append("\t}\r\n");
            builder.Append("}\r\n");
            return builder.ToString();
        }

        public string GetModelCode()
        {
            BuilderModel model = new BuilderModel();
            model.ModelName = base.ModelName;
            model.NameSpace = base.NameSpace;
            model.Fieldlist = base.Fieldlist;
            model.Modelpath = base.Modelpath;
            return model.CreatModel();
        }

        public string GetModelCode(string tableNameParent, string modelNameParent, List<ColumnInfo> FieldlistP, string tableNameSon, string modelNameSon, List<ColumnInfo> FieldlistS)
        {
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            if (modelNameSon == "")
            {
                modelNameSon = tableNameSon;
            }
            StringPlus plus = new StringPlus();
            new StringPlus();
            new StringPlus();
            plus.AppendLine("using System;");
            plus.AppendLine("using System.Collections.Generic;");
            plus.AppendLine("namespace " + base.Modelpath);
            plus.AppendLine("{");
            BuilderModelT lt = new BuilderModelT();
            lt.ModelName = modelNameParent;
            lt.NameSpace = base.NameSpace;
            lt.Fieldlist = FieldlistP;
            lt.Modelpath = base.Modelpath;
            lt.ModelNameSon = modelNameSon;
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + modelNameParent + " 。(属性说明自动提取数据库字段的描述信息)");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "public class " + modelNameParent);
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "public " + modelNameParent + "()");
            plus.AppendSpaceLine(2, "{}");
            plus.AppendLine(lt.CreatModelMethodT());
            plus.AppendSpaceLine(1, "}");
            BuilderModel model = new BuilderModel();
            model.ModelName = modelNameSon;
            model.NameSpace = base.NameSpace;
            model.Fieldlist = FieldlistS;
            model.Modelpath = base.Modelpath;
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + modelNameSon + " 。(属性说明自动提取数据库字段的描述信息)");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "public class " + modelNameSon);
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "public " + modelNameSon + "()");
            plus.AppendSpaceLine(2, "{}");
            plus.AppendLine(model.CreatModelMethod());
            plus.AppendSpaceLine(1, "}");
            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }
    }

    public class BuilderFrameOne : BuilderFrame
    {
        // Fields
        private string _procprefix;
        private INIFile cfgfile;
        private string cmcfgfile = (Application.StartupPath + @"\cmcfg.ini");

        // Methods
        public BuilderFrameOne(IDbObject idbobj, string dbName, string tableName, string modelName, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string nameSpace, string folder, string dbHelperName)
        {
            base.dbobj = idbobj;
            base.DbName = dbName;
            base.TableName = tableName;
            base.ModelName = modelName;
            base.NameSpace = nameSpace;
            base.Folder = folder;
            base.DbHelperName = dbHelperName;
            base._dbtype = idbobj.DbType;
            base.Fieldlist = fieldlist;
            base.Keys = keys;
            foreach (ColumnInfo info in keys)
            {
                base._key = info.ColumnName;
                base._keyType = info.TypeName;
                if (info.IsIdentity)
                {
                    base._key = info.ColumnName;
                    base._keyType = CodeCommon.DbTypeToCS(info.TypeName);
                    break;
                }
            }
        }

        private string CreatAddParam()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            StringBuilder builder3 = new StringBuilder();
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 增加一条数据\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            string str = "void";
            if (base.IsHasIdentity)
            {
                str = "int";
            }
            builder.Append(base.Space(2) + "public " + str + " Add()\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"insert into " + base.TableName + "(\");\r\n");
            builder2.Append(base.Space(3) + "strSql.Append(\"");
            int num = 0;
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                bool isIdentity = info.IsIdentity;
                string length = info.Length;
                if (info.IsIdentity)
                {
                    str = CodeCommon.DbTypeToCS(typeName);
                }
                else
                {
                    plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@" + columnName + "\", " + base.DbParaDbType + "." + CodeCommon.DbTypeLength(base._dbtype, typeName, length) + "),");
                    builder2.Append(columnName + ",");
                    builder3.Append("@" + columnName + ",");
                    plus2.AppendSpaceLine(3, string.Concat(new object[] { "parameters[", num, "].Value = ", columnName, ";" }));
                    num++;
                }
            }
            builder2.Remove(builder2.Length - 1, 1);
            builder3.Remove(builder3.Length - 1, 1);
            if (plus.Value.IndexOf(",") > 0)
            {
                plus.DelLastComma();
            }
            builder2.Append(")\");\r\n");
            builder.Append(builder2.ToString());
            builder.Append(base.Space(3) + "strSql.Append(\" values (\");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"" + builder3.ToString() + ")\");\r\n");
            if (base.IsHasIdentity)
            {
                builder.Append(CodeCommon.Space(3) + "strSql.Append(\";select @@IDENTITY\");\r\n");
            }
            builder.Append(base.Space(3) + "" + base.DbParaHead + "Parameter[] parameters = {\r\n");
            builder.Append(plus.Value);
            builder.Append("};\r\n");
            builder.Append(plus2.Value + "\r\n");
            if (base.IsHasIdentity)
            {
                builder.Append(CodeCommon.Space(3) + "object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);\r\n");
                builder.Append(CodeCommon.Space(3) + "if (obj == null)\r\n");
                builder.Append(CodeCommon.Space(3) + "{\r\n");
                builder.Append(CodeCommon.Space(4) + "return 1;\r\n");
                builder.Append(CodeCommon.Space(3) + "}\r\n");
                builder.Append(CodeCommon.Space(3) + "else\r\n");
                builder.Append(CodeCommon.Space(3) + "{\r\n");
                builder.Append(CodeCommon.Space(4) + "return Convert.ToInt32(obj);\r\n");
                builder.Append(CodeCommon.Space(3) + "}\r\n");
            }
            else
            {
                builder.Append(CodeCommon.Space(3) + "" + base.DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);\r\n");
            }
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        private string CreatAddProc()
        {
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "///  增加一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            string str = "void";
            if (base.IsHasIdentity)
            {
                str = "int";
            }
            plus.AppendSpaceLine(2, "public " + str + " Add()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "int rowsAffected;");
            plus.AppendSpaceLine(3, "" + base.DbParaHead + "Parameter[] parameters = {");
            int num = 0;
            int num2 = 0;
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                bool isIdentity = info.IsIdentity;
                string length = info.Length;
                plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@" + columnName + "\", " + base.DbParaDbType + "." + CodeCommon.DbTypeLength(base._dbtype, typeName, length) + "),");
                if (info.IsIdentity)
                {
                    num = num2;
                    plus2.AppendSpaceLine(3, "parameters[" + num2 + "].Direction = ParameterDirection.Output;");
                    num2++;
                }
                else
                {
                    plus2.AppendSpaceLine(3, string.Concat(new object[] { "parameters[", num2, "].Value = ", columnName, ";" }));
                    num2++;
                }
            }
            if (plus.Value.IndexOf(",") > 0)
            {
                plus.DelLastComma();
            }
            plus.AppendLine("};");
            plus.AppendLine(plus2.Value);
            plus.AppendSpaceLine(3, "" + base.DbHelperName + ".RunProcedure(\"UP_" + base.TableName + "_ADD\",parameters,out rowsAffected);");
            if (base.IsHasIdentity)
            {
                plus.AppendSpaceLine(3, string.Concat(new object[] { base._key, "= (", base._keyType, ")parameters[", num, "].Value;" }));
            }
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatAddSQL()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            StringBuilder builder3 = new StringBuilder();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 增加一条数据\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            string str = "void";
            if (base.IsHasIdentity)
            {
                str = "int";
            }
            builder.Append(base.Space(2) + "public " + str + " Add()\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"insert into [" + base.TableName + "](\");\r\n");
            builder2.Append(base.Space(3) + "strSql.Append(\"");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                bool isIdentity = info.IsIdentity;
                if (info.IsIdentity)
                {
                    str = CodeCommon.DbTypeToCS(typeName);
                }
                else
                {
                    builder2.Append(columnName + ",");
                    if (CodeCommon.IsAddMark(typeName.Trim()))
                    {
                        builder3.Append(base.Space(3) + "strSql.Append(\"'\"+" + columnName + "+\"',\");\r\n");
                        continue;
                    }
                    builder3.Append(base.Space(3) + "strSql.Append(\"\"+" + columnName + "+\",\");\r\n");
                }
            }
            builder2.Remove(builder2.Length - 1, 1);
            builder3.Remove(builder3.Length - 6, 1);
            builder2.Append("\");\r\n");
            builder.Append(builder2.ToString());
            builder.Append(base.Space(3) + "strSql.Append(\")\");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\" values (\");\r\n");
            builder.Append(builder3.ToString());
            builder.Append(base.Space(3) + "strSql.Append(\")\");\r\n");
            if (base.IsHasIdentity)
            {
                builder.Append(CodeCommon.Space(3) + "strSql.Append(\";select @@IDENTITY\");\r\n");
                builder.Append(CodeCommon.Space(3) + "object obj = DbHelperSQL.GetSingle(strSql.ToString());\r\n");
                builder.Append(CodeCommon.Space(3) + "if (obj == null)\r\n");
                builder.Append(CodeCommon.Space(3) + "{\r\n");
                builder.Append(CodeCommon.Space(4) + "return 1;\r\n");
                builder.Append(CodeCommon.Space(3) + "}\r\n");
                builder.Append(CodeCommon.Space(3) + "else\r\n");
                builder.Append(CodeCommon.Space(3) + "{\r\n");
                builder.Append(CodeCommon.Space(4) + "return Convert.ToInt32(obj);\r\n");
                builder.Append(CodeCommon.Space(3) + "}\r\n");
            }
            else
            {
                builder.Append(CodeCommon.Space(3) + "" + base.DbHelperName + ".ExecuteSql(strSql.ToString());\r\n");
            }
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        private string CreatConstructorParam()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 得到一个对象实体\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            builder.Append(base.Space(2) + "public " + base.ModelName + "(" + CodeCommon.GetInParameter(base.Keys) + ")\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"select ");
            builder.Append(base.Fieldstrlist + " \");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\" FROM " + base.TableName + " \");\r\n");
            if (this.GetWhereExpression(base.Keys).Length > 0)
            {
                builder.Append(base.Space(3) + "strSql.Append(\" where " + this.GetWhereExpression(base.Keys) + "\");\r\n");
            }
            else
            {
                builder.Append(base.Space(3) + "//strSql.Append(\" where 条件);\r\n");
            }
            builder.AppendLine(this.GetPreParameter(base.Keys));
            builder.Append(base.Space(3) + "DataSet ds=" + base.DbHelperName + ".Query(strSql.ToString(),parameters);\r\n");
            builder.Append(base.Space(3) + "if(ds.Tables[0].Rows.Count>0)\r\n");
            builder.Append(base.Space(3) + "{\r\n");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                switch (CodeCommon.DbTypeToCS(info.TypeName))
                {
                    case "int":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "decimal":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "DateTime":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "string":
                        {
                            builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            continue;
                        }
                    case "bool":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=true;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(5) + "else\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=false;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(4) + "}\r\n\r\n");
                            continue;
                        }
                    case "byte[]":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                }
                builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
            }
            builder.Append(base.Space(3) + "}\r\n");
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        private string CreatConstructorProc()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 得到一个对象实体");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + base.ModelName + "(" + CodeCommon.GetInParameter(base.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendLine(this.GetPreParameter(base.Keys));
            plus.AppendSpaceLine(3, "DataSet ds= " + base.DbHelperName + ".RunProcedure(\"UP_" + base.TableName + "_GetModel\",parameters,\"ds\");");
            plus.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
            plus.AppendSpaceLine(3, "{");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                switch (CodeCommon.DbTypeToCS(info.TypeName))
                {
                    case "int":
                        {
                            plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(4, "{");
                            plus.AppendSpaceLine(5, "" + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(4, "}");
                            continue;
                        }
                    case "decimal":
                        {
                            plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(4, "{");
                            plus.AppendSpaceLine(5, "" + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(4, "}");
                            continue;
                        }
                    case "DateTime":
                        {
                            plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(4, "{\r\n");
                            plus.AppendSpaceLine(5, "" + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(4, "}");
                            continue;
                        }
                    case "string":
                        {
                            plus.AppendSpaceLine(4, "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                            continue;
                        }
                    case "bool":
                        {
                            plus.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            plus.Append(base.Space(4) + "{\r\n");
                            plus.Append(base.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))\r\n");
                            plus.Append(base.Space(5) + "{\r\n");
                            plus.Append(base.Space(6) + "" + columnName + "=true;\r\n");
                            plus.Append(base.Space(5) + "}\r\n");
                            plus.Append(base.Space(5) + "else\r\n");
                            plus.Append(base.Space(5) + "{\r\n");
                            plus.Append(base.Space(6) + "" + columnName + "=false;\r\n");
                            plus.Append(base.Space(5) + "}\r\n");
                            plus.Append(base.Space(4) + "}\r\n\r\n");
                            continue;
                        }
                    case "byte[]":
                        {
                            plus.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            plus.Append(base.Space(4) + "{\r\n");
                            plus.Append(base.Space(5) + "" + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            plus.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                }
                plus.AppendSpaceLine(4, "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
            }
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatConstructorSQL()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 得到一个对象实体\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            builder.Append(base.Space(2) + "public " + base.ModelName + "(" + CodeCommon.GetInParameter(base.Keys) + ")\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"select  \");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"" + base.Fieldstrlist + " \");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\" from " + base.TableName + " \");\r\n");
            if (CodeCommon.GetWhereExpression(base.Keys).Length > 0)
            {
                builder.Append(base.Space(3) + "strSql.Append(\" where " + CodeCommon.GetWhereExpression(base.Keys) + ");\r\n");
            }
            else
            {
                builder.Append(base.Space(3) + "//strSql.Append(\" where 条件);\r\n");
            }
            builder.Append(base.Space(3) + "DataSet ds=" + base.DbHelperName + ".Query(strSql.ToString());\r\n");
            builder.Append(base.Space(3) + "if(ds.Tables[0].Rows.Count>0)\r\n");
            builder.Append(base.Space(3) + "{\r\n");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                switch (CodeCommon.DbTypeToCS(info.TypeName))
                {
                    case "int":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "decimal":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "DateTime":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "string":
                        {
                            builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            continue;
                        }
                    case "bool":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=true;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(5) + "else\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=false;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(4) + "}\r\n\r\n");
                            continue;
                        }
                    case "byte[]":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                }
                builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
            }
            builder.Append(base.Space(3) + "}\r\n");
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        private string CreatDeleteParam()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 删除一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public void Delete(" + CodeCommon.GetInParameter(base.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            if (base.dbobj.DbType != "OleDb")
            {
                plus.AppendSpaceLine(3, "strSql.Append(\"delete " + base.TableName + " \");");
            }
            else
            {
                plus.AppendSpaceLine(3, "strSql.Append(\"delete from " + base.TableName + " \");");
            }
            if (this.GetWhereExpression(base.Keys).Length > 0)
            {
                plus.AppendSpace(3, "strSql.Append(\" where " + this.GetWhereExpression(base.Keys) + "\");\r\n");
            }
            else
            {
                plus.AppendSpace(3, "//strSql.Append(\" where 条件);\r\n");
            }
            plus.AppendLine(this.GetPreParameter(base.Keys));
            plus.AppendSpaceLine(3, "" + base.DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatDeleteProc()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 删除一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public void Delete(" + CodeCommon.GetInParameter(base.Keys) + ")");
            plus.AppendSpaceLine(2, "{\r\n");
            plus.AppendSpaceLine(3, "int rowsAffected;");
            plus.AppendLine(this.GetPreParameter(base.Keys));
            plus.AppendSpaceLine(3, "" + base.DbHelperName + ".RunProcedure(\"UP_" + base.TableName + "_Delete\",parameters,out rowsAffected);");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatDeleteSQL()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 删除一条数据\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            builder.Append(base.Space(2) + "public void Delete(" + CodeCommon.GetInParameter(base.Keys) + ")\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            if (base._dbtype != "OleDb")
            {
                builder.Append(base.Space(3) + "strSql.Append(\"delete " + base.TableName + " \");\r\n");
            }
            else
            {
                builder.Append(base.Space(3) + "strSql.Append(\"delete from " + base.TableName + " \");\r\n");
            }
            if (CodeCommon.GetWhereExpression(base.Keys).Length > 0)
            {
                builder.Append(base.Space(3) + "strSql.Append(\" where " + CodeCommon.GetWhereExpression(base.Keys) + ");\r\n");
            }
            else
            {
                builder.Append(base.Space(3) + "//strSql.Append(\" where 条件);\r\n");
            }
            builder.Append(base.Space(3) + "\t" + base.DbHelperName + ".ExecuteSql(strSql.ToString());\r\n");
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        private string CreatExistsParam()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 是否存在该记录");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public bool Exists(" + CodeCommon.GetInParameter(base.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            plus.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + base.TableName + "\");");
            if (this.GetWhereExpression(base.Keys).Length > 0)
            {
                plus.AppendSpaceLine(3, "strSql.Append(\" where " + this.GetWhereExpression(base.Keys) + "\");\r\n");
            }
            else
            {
                plus.AppendSpaceLine(3, "//strSql.Append(\" where 条件);\r\n");
            }
            plus.AppendLine(this.GetPreParameter(base.Keys));
            plus.Append(CodeCommon.Space(3) + "return " + base.DbHelperName + ".Exists(strSql.ToString(),parameters);\r\n");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatExistsProc()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 是否存在该记录");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public bool Exists(" + CodeCommon.GetInParameter(base.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "int rowsAffected;");
            plus.AppendLine(this.GetPreParameter(base.Keys));
            plus.AppendSpaceLine(3, "int result= " + base.DbHelperName + ".RunProcedure(\"UP_" + base.TableName + "_Exists\",parameters,out rowsAffected);");
            plus.AppendSpaceLine(3, "if(result==1)");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "return true;");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(3, "else");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "return false;");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatExistsSQL()
        {
            StringPlus plus = new StringPlus();
            if (base.Keys.Count > 0)
            {
                plus.AppendLine("");
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 是否存在该记录");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public bool Exists(" + CodeCommon.GetInParameter(base.Keys) + ")");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                plus.AppendSpace(3, "strSql.Append(\"select count(1) from " + base.TableName);
                plus.AppendSpaceLine(0, " where " + CodeCommon.GetWhereExpression(base.Keys) + " );");
                plus.AppendSpaceLine(3, "return " + base.DbHelperName + ".Exists(strSql.ToString());");
                plus.AppendSpace(2, "}");
            }
            return plus.ToString();
        }

        private string CreatGetListByPageProc()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/*");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 分页获取数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "" + base.DbParaHead + "Parameter[] parameters = {");
            plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@tblName\", " + base.DbParaDbType + ".VarChar, 255),");
            plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@fldName\", " + base.DbParaDbType + ".VarChar, 255),");
            plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@PageSize\", " + base.DbParaDbType + "." + CodeCommon.CSToProcType(base._dbtype, "int") + "),");
            plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@PageIndex\", " + base.DbParaDbType + "." + CodeCommon.CSToProcType(base._dbtype, "int") + "),");
            plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@IsReCount\", " + base.DbParaDbType + "." + CodeCommon.CSToProcType(base._dbtype, "bit") + "),");
            plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@OrderType\", " + base.DbParaDbType + "." + CodeCommon.CSToProcType(base._dbtype, "bit") + "),");
            plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@strWhere\", " + base.DbParaDbType + ".VarChar,1000),");
            plus.AppendSpaceLine(5, "};");
            plus.AppendSpaceLine(3, "parameters[0].Value = \"" + base.TableName + "\";");
            plus.AppendSpaceLine(3, "parameters[1].Value = \"" + base._key + "\";");
            plus.AppendSpaceLine(3, "parameters[2].Value = PageSize;");
            plus.AppendSpaceLine(3, "parameters[3].Value = PageIndex;");
            plus.AppendSpaceLine(3, "parameters[4].Value = 0;");
            plus.AppendSpaceLine(3, "parameters[5].Value = 0;");
            plus.AppendSpaceLine(3, "parameters[6].Value = strWhere;\t");
            plus.AppendSpaceLine(3, "return " + base.DbHelperName + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
            plus.AppendSpaceLine(2, "}*/");
            return plus.Value;
        }

        private string CreatGetListParam()
        {
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(base.dbobj.GetColumnInfoList(base.DbName, base.TableName));
            StringPlus plus = new StringPlus();
            string fieldslist = base.GetFieldslist(columnInfoDt);
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获得数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            plus.AppendSpace(3, "strSql.Append(\"select ");
            plus.AppendLine(fieldslist + " \");");
            plus.AppendSpaceLine(3, "strSql.Append(\" FROM " + base.TableName + " \");");
            plus.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(3, "return " + base.DbHelperName + ".Query(strSql.ToString());");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatGetListProc()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DataSet GetList()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return " + base.DbHelperName + ".RunProcedure(\"UP_" + base.TableName + "_GetList\",parameters,\"ds\");");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatGetListSQL()
        {
            DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(base.dbobj.GetColumnList(base.DbName, base.TableName));
            StringPlus plus = new StringPlus();
            string fieldslist = base.GetFieldslist(columnInfoDt);
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获得数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            plus.AppendSpace(3, "strSql.Append(\"select ");
            plus.AppendLine(fieldslist + " \");");
            plus.AppendSpaceLine(3, "strSql.Append(\" FROM " + base.TableName + " \");");
            plus.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            plus.AppendSpaceLine(3, "{\r\n");
            plus.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(3, "return " + base.DbHelperName + ".Query(strSql.ToString());");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatGetMaxIDParam()
        {
            StringPlus plus = new StringPlus();
            if (base.Keys.Count > 0)
            {
                string columnName = "";
                foreach (ColumnInfo info in base.Keys)
                {
                    if (CodeCommon.DbTypeToCS(info.TypeName) == "int")
                    {
                        columnName = info.ColumnName;
                        if (info.IsPK)
                        {
                            plus.AppendLine("");
                            plus.AppendSpaceLine(2, "/// <summary>");
                            plus.AppendSpaceLine(2, "/// 得到最大ID");
                            plus.AppendSpaceLine(2, "/// </summary>");
                            plus.AppendSpaceLine(2, "public int GetMaxId()");
                            plus.AppendSpaceLine(2, "{\r\n");
                            plus.AppendSpaceLine(2, "return " + base.DbHelperName + ".GetMaxID(\"" + columnName + "\", \"" + base.TableName + "\"); ");
                            plus.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return plus.ToString();
        }

        private string CreatGetMaxIDProc()
        {
            StringPlus plus = new StringPlus();
            if (base.Keys.Count > 0)
            {
                string columnName = "";
                foreach (ColumnInfo info in base.Keys)
                {
                    if (CodeCommon.DbTypeToCS(info.TypeName) == "int")
                    {
                        columnName = info.ColumnName;
                        if (info.IsPK)
                        {
                            plus.AppendLine("");
                            plus.AppendSpaceLine(2, "/// <summary>");
                            plus.AppendSpaceLine(2, "/// 得到最大ID");
                            plus.AppendSpaceLine(2, "/// </summary>");
                            plus.AppendSpaceLine(2, "public int GetMaxId()");
                            plus.AppendSpaceLine(2, "{\r\n");
                            plus.AppendSpaceLine(2, "return " + base.DbHelperName + ".GetMaxID(\"" + columnName + "\", \"" + base.TableName + "\"); ");
                            plus.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return plus.ToString();
        }

        private string CreatGetMaxIDSQL()
        {
            StringPlus plus = new StringPlus();
            if (base.Keys.Count > 0)
            {
                string columnName = "";
                foreach (ColumnInfo info in base.Keys)
                {
                    if (CodeCommon.DbTypeToCS(info.TypeName) == "int")
                    {
                        columnName = info.ColumnName;
                        if (info.IsPK)
                        {
                            plus.AppendLine("");
                            plus.AppendSpaceLine(2, "/// <summary>");
                            plus.AppendSpaceLine(2, "/// 得到最大ID");
                            plus.AppendSpaceLine(2, "/// </summary>");
                            plus.AppendSpaceLine(2, "public int GetMaxId()");
                            plus.AppendSpaceLine(2, "{\r\n");
                            plus.AppendSpaceLine(2, "return " + base.DbHelperName + ".GetMaxID(\"" + columnName + "\", \"" + base.TableName + "\"); ");
                            plus.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return plus.ToString();
        }

        private string CreatGetModelParam()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 得到一个对象实体\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            builder.Append(base.Space(2) + "public void GetModel(" + CodeCommon.GetInParameter(base.Keys) + ")\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"select ");
            builder.Append(base.Fieldstrlist + " \");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\" FROM " + base.TableName + " \");\r\n");
            if (this.GetWhereExpression(base.Keys).Length > 0)
            {
                builder.Append(base.Space(3) + "strSql.Append(\" where " + this.GetWhereExpression(base.Keys) + "\");\r\n");
            }
            else
            {
                builder.Append(base.Space(3) + "//strSql.Append(\" where 条件);\r\n");
            }
            builder.AppendLine(this.GetPreParameter(base.Keys));
            builder.Append(base.Space(3) + "DataSet ds=" + base.DbHelperName + ".Query(strSql.ToString(),parameters);\r\n");
            builder.Append(base.Space(3) + "if(ds.Tables[0].Rows.Count>0)\r\n");
            builder.Append(base.Space(3) + "{\r\n");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                switch (CodeCommon.DbTypeToCS(info.TypeName))
                {
                    case "int":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "decimal":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "DateTime":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "string":
                        {
                            builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            continue;
                        }
                    case "bool":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=true;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(5) + "else\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=false;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(4) + "}\r\n\r\n");
                            continue;
                        }
                    case "byte[]":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                }
                builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
            }
            builder.Append(base.Space(3) + "}\r\n");
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        private string CreatGetModelProc()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 得到一个对象实体");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public void GetModel(" + CodeCommon.GetInParameter(base.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendLine(this.GetPreParameter(base.Keys));
            plus.AppendSpaceLine(3, "DataSet ds= " + base.DbHelperName + ".RunProcedure(\"UP_" + base.TableName + "_GetModel\",parameters,\"ds\");");
            plus.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
            plus.AppendSpaceLine(3, "{");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                switch (CodeCommon.DbTypeToCS(info.TypeName))
                {
                    case "int":
                        {
                            plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(4, "{");
                            plus.AppendSpaceLine(5, "" + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(4, "}");
                            continue;
                        }
                    case "decimal":
                        {
                            plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(4, "{");
                            plus.AppendSpaceLine(5, "" + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(4, "}");
                            continue;
                        }
                    case "DateTime":
                        {
                            plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(4, "{\r\n");
                            plus.AppendSpaceLine(5, "" + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(4, "}");
                            continue;
                        }
                    case "string":
                        {
                            plus.AppendSpaceLine(4, "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                            continue;
                        }
                    case "bool":
                        {
                            plus.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            plus.Append(base.Space(4) + "{\r\n");
                            plus.Append(base.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))\r\n");
                            plus.Append(base.Space(5) + "{\r\n");
                            plus.Append(base.Space(6) + "" + columnName + "=true;\r\n");
                            plus.Append(base.Space(5) + "}\r\n");
                            plus.Append(base.Space(5) + "else\r\n");
                            plus.Append(base.Space(5) + "{\r\n");
                            plus.Append(base.Space(6) + "" + columnName + "=false;\r\n");
                            plus.Append(base.Space(5) + "}\r\n");
                            plus.Append(base.Space(4) + "}\r\n\r\n");
                            continue;
                        }
                    case "byte[]":
                        {
                            plus.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            plus.Append(base.Space(4) + "{\r\n");
                            plus.Append(base.Space(5) + "" + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            plus.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                }
                plus.AppendSpaceLine(4, "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
            }
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatGetModelSQL()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 得到一个对象实体\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            builder.Append(base.Space(2) + "public void GetModel(" + CodeCommon.GetInParameter(base.Keys) + ")\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"select  \");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"" + base.Fieldstrlist + " \");\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\" from " + base.TableName + " \");\r\n");
            if (CodeCommon.GetWhereExpression(base.Keys).Length > 0)
            {
                builder.Append(base.Space(3) + "strSql.Append(\" where " + CodeCommon.GetWhereExpression(base.Keys) + ");\r\n");
            }
            else
            {
                builder.Append(base.Space(3) + "//strSql.Append(\" where 条件);\r\n");
            }
            builder.Append(base.Space(3) + "DataSet ds=" + base.DbHelperName + ".Query(strSql.ToString());\r\n");
            builder.Append(base.Space(3) + "if(ds.Tables[0].Rows.Count>0)\r\n");
            builder.Append(base.Space(3) + "{\r\n");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                switch (CodeCommon.DbTypeToCS(info.TypeName))
                {
                    case "int":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "decimal":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "DateTime":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                    case "string":
                        {
                            builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            continue;
                        }
                    case "bool":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=true;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(5) + "else\r\n");
                            builder.Append(base.Space(5) + "{\r\n");
                            builder.Append(base.Space(6) + "" + columnName + "=false;\r\n");
                            builder.Append(base.Space(5) + "}\r\n");
                            builder.Append(base.Space(4) + "}\r\n\r\n");
                            continue;
                        }
                    case "byte[]":
                        {
                            builder.Append(base.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")\r\n");
                            builder.Append(base.Space(4) + "{\r\n");
                            builder.Append(base.Space(5) + "" + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
                            builder.Append(base.Space(4) + "}\r\n");
                            continue;
                        }
                }
                builder.Append(base.Space(4) + "" + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();\r\n");
            }
            builder.Append(base.Space(3) + "}\r\n");
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        private string CreatUpdateParam()
        {
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus();
            StringPlus plus3 = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 更新一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public void Update()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            plus.AppendSpaceLine(3, "strSql.Append(\"update " + base.TableName + " set \");");
            int num = 0;
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                string length = info.Length;
                bool isIdentity = info.IsIdentity;
                bool isPK = info.IsPK;
                plus2.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@" + columnName + "\", " + base.DbParaDbType + "." + CodeCommon.DbTypeLength(base._dbtype, typeName, length) + "),");
                plus3.AppendSpaceLine(3, string.Concat(new object[] { "parameters[", num, "].Value = ", columnName, ";" }));
                num++;
                if ((!info.IsIdentity && !info.IsPK) && !base.Keys.Contains(info))
                {
                    plus.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=@" + columnName + ",\");");
                }
            }
            if (plus.Value.IndexOf(",") > 0)
            {
                plus.DelLastComma();
            }
            plus.AppendLine("\");");
            if (this.GetWhereExpression(base.Keys).Length > 0)
            {
                plus.AppendSpace(3, "strSql.Append(\" where " + this.GetWhereExpression(base.Keys) + "\");\r\n");
            }
            else
            {
                plus.AppendSpace(3, "//strSql.Append(\" where 条件);\r\n");
            }
            plus.AppendSpaceLine(3, "" + base.DbParaHead + "Parameter[] parameters = {");
            if (plus2.Value.IndexOf(",") > 0)
            {
                plus2.DelLastComma();
            }
            plus.Append(plus2.Value);
            plus.AppendLine("};");
            plus.AppendLine(plus3.Value);
            plus.AppendSpaceLine(3, "" + base.DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        private string CreatUpdateProc()
        {
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "///  更新一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public void Update()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "int rowsAffected;");
            plus.AppendSpaceLine(3, "" + base.DbParaHead + "Parameter[] parameters = {");
            int num = 0;
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                string length = info.Length;
                plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"@" + columnName + "\", " + base.DbParaDbType + "." + CodeCommon.DbTypeLength(base._dbtype, typeName, length) + "),");
                plus2.AppendSpaceLine(3, string.Concat(new object[] { "parameters[", num, "].Value = ", columnName, ";" }));
                num++;
            }
            if (plus.Value.IndexOf(",") > 0)
            {
                plus.DelLastComma();
            }
            plus.AppendLine("};");
            plus.AppendLine(plus2.Value);
            plus.AppendSpaceLine(3, "" + base.DbHelperName + ".RunProcedure(\"UP_" + base.TableName + "_Update\",parameters,out rowsAffected);");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        private string CreatUpdateSQL()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\r\n");
            builder.Append(base.Space(2) + "/// <summary>\r\n");
            builder.Append(base.Space(2) + "/// 更新一条数据\r\n");
            builder.Append(base.Space(2) + "/// </summary>\r\n");
            builder.Append(base.Space(2) + "public void Update()\r\n");
            builder.Append(base.Space(2) + "{\r\n");
            builder.Append(base.Space(3) + "StringBuilder strSql=new StringBuilder();\r\n");
            builder.Append(base.Space(3) + "strSql.Append(\"update " + base.TableName + " set \");\r\n");
            foreach (ColumnInfo info in base.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                string length = info.Length;
                bool isIdentity = info.IsIdentity;
                bool isPK = info.IsPK;
                if ((!info.IsIdentity && !info.IsPK) && !base.Keys.Contains(info))
                {
                    if (CodeCommon.IsAddMark(typeName.Trim()))
                    {
                        builder.Append(base.Space(3) + "strSql.Append(\"" + columnName + "='\"+" + columnName + "+\"',\");\r\n");
                    }
                    else
                    {
                        builder.Append(base.Space(3) + "strSql.Append(\"" + columnName + "=\"+" + columnName + "+\",\");\r\n");
                    }
                }
            }
            builder.Remove(builder.Length - 6, 1);
            builder.Append(base.Space(3) + "strSql.Append(\" where " + CodeCommon.GetWhereExpression(base.Keys) + "\");\r\n");
            builder.Append(base.Space(3) + "" + base.DbHelperName + ".ExecuteSql(strSql.ToString());\r\n");
            builder.Append(base.Space(2) + "}");
            return builder.ToString();
        }

        public string GetCode(string DALtype, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, string procPrefix)
        {
            this.cfgfile = new INIFile(this.cmcfgfile);
            DALtype = this.cfgfile.IniReadValue("BuilderOne", DALtype.Trim());
            this.ProcPrefix = procPrefix;
            StringPlus plus = new StringPlus();
            plus.AppendLine("using System;");
            plus.AppendLine("using System.Data;");
            plus.AppendLine("using System.Text;");
            string dbType = base.dbobj.DbType;
            if (dbType != null)
            {
                if (!(dbType == "SQL2005"))
                {
                    if (dbType == "SQL2000")
                    {
                        plus.AppendLine("using System.Data.SqlClient;");
                    }
                    else if (dbType == "Oracle")
                    {
                        plus.AppendLine("using System.Data.OracleClient;");
                    }
                    else if (dbType == "MySQL")
                    {
                        plus.AppendLine("using MySql.Data.MySqlClient;");
                    }
                    else if (dbType == "OleDb")
                    {
                        plus.AppendLine("using System.Data.OleDb;");
                    }
                }
                else
                {
                    plus.AppendLine("using System.Data.SqlClient;");
                }
            }
            //plus.AppendLine("using Maticsoft.DBUtility;//请先添加引用");
            plus.AppendLine("namespace " + base.NameSpace);
            plus.AppendLine("{");
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 类" + base.ModelName + "。");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "public class " + base.ModelName);
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "public " + base.ModelName + "()");
            plus.AppendSpaceLine(2, "{}");
            BuilderModel model = new BuilderModel();
            model.ModelName = base.ModelName;
            model.NameSpace = base.NameSpace;
            model.Fieldlist = base.Fieldlist;
            model.Modelpath = base.Modelpath;
            plus.AppendLine(model.CreatModelMethod());
            plus.AppendLine("");
            plus.AppendSpaceLine(2, "#region  成员方法");
            string str2 = DALtype;
            if (str2 != null)
            {
                if (!(str2 == "sql"))
                {
                    if (str2 == "Param")
                    {
                        plus.Append(this.CreatConstructorParam() + "\r\n");
                        if (Maxid)
                        {
                            plus.Append(this.CreatGetMaxIDParam() + "\r\n");
                        }
                        if (Exists)
                        {
                            plus.Append(this.CreatExistsParam() + "\r\n");
                        }
                        if (Add)
                        {
                            plus.Append(this.CreatAddParam() + "\r\n");
                        }
                        if (Update)
                        {
                            plus.Append(this.CreatUpdateParam() + "\r\n");
                        }
                        if (Delete)
                        {
                            plus.Append(this.CreatDeleteParam() + "\r\n");
                        }
                        if (GetModel)
                        {
                            plus.Append(this.CreatGetModelParam() + "\r\n");
                        }
                        if (List)
                        {
                            plus.Append(this.CreatGetListParam() + "\r\n");
                        }
                        goto Label_049F;
                    }
                    if (str2 == "Proc")
                    {
                        plus.Append(this.CreatConstructorProc() + "\r\n");
                        if (Maxid)
                        {
                            plus.Append(this.CreatGetMaxIDProc() + "\r\n");
                        }
                        if (Exists)
                        {
                            plus.Append(this.CreatExistsProc() + "\r\n");
                        }
                        if (Add)
                        {
                            plus.Append(this.CreatAddProc() + "\r\n");
                        }
                        if (Update)
                        {
                            plus.Append(this.CreatUpdateProc() + "\r\n");
                        }
                        if (Delete)
                        {
                            plus.Append(this.CreatDeleteProc() + "\r\n");
                        }
                        if (GetModel)
                        {
                            plus.Append(this.CreatGetModelProc() + "\r\n");
                        }
                        if (List)
                        {
                            plus.Append(this.CreatGetListProc() + "\r\n");
                        }
                        if (List)
                        {
                            plus.Append(this.CreatGetListByPageProc() + "\r\n");
                        }
                        goto Label_049F;
                    }
                }
                else
                {
                    plus.AppendLine(this.CreatConstructorSQL());
                    if (Maxid)
                    {
                        plus.AppendLine(this.CreatGetMaxIDSQL());
                    }
                    if (Exists)
                    {
                        plus.AppendLine(this.CreatExistsSQL());
                    }
                    if (Add)
                    {
                        plus.AppendLine(this.CreatAddSQL());
                    }
                    if (Update)
                    {
                        plus.AppendLine(this.CreatUpdateSQL());
                    }
                    if (Delete)
                    {
                        plus.AppendLine(this.CreatDeleteSQL());
                    }
                    if (GetModel)
                    {
                        plus.AppendLine(this.CreatGetModelSQL());
                    }
                    if (List)
                    {
                        plus.AppendLine(this.CreatGetListSQL());
                    }
                    goto Label_049F;
                }
            }
            plus.AppendSpaceLine(2, "//暂不支持该方式。\r\n");
        Label_049F:
            plus.AppendSpaceLine(2, "#endregion  成员方法");
            plus.AppendSpaceLine(1, "}");
            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }

        public string GetPreParameter(List<ColumnInfo> keys)
        {
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus();
            plus.AppendSpaceLine(3, "" + base.DbParaHead + "Parameter[] parameters = {");
            int num = 0;
            foreach (ColumnInfo info in keys)
            {
                plus.AppendSpaceLine(5, "new " + base.DbParaHead + "Parameter(\"" + this.preParameter + "" + info.ColumnName + "\", " + base.DbParaDbType + "." + CodeCommon.DbTypeLength(base.dbobj.DbType, info.TypeName, "") + "),");
                plus2.AppendSpaceLine(3, "parameters[" + num.ToString() + "].Value = " + info.ColumnName + ";");
                num++;
            }
            plus.DelLastComma();
            plus.AppendLine("};");
            plus.Append(plus2.Value);
            return plus.Value;
        }

        public string GetWhereExpression(List<ColumnInfo> keys)
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in keys)
            {
                plus.Append(info.ColumnName + "=" + this.preParameter + info.ColumnName + " and ");
            }
            plus.DelLastChar("and");
            return plus.Value;
        }

        // Properties
        public string preParameter
        {
            get
            {
                switch (base.dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                        return "@";

                    case "Oracle":
                        return ":";
                }
                return "@";
            }
        }

        public string ProcPrefix
        {
            get
            {
                return this._procprefix;
            }
            set
            {
                this._procprefix = value;
            }
        }
    }

    public class BuilderFrameS3 : BuilderFrame
    {
        // Fields
        private IBuilderBLL ibll;
        private IBuilderDAL idal;
        private IBuilderDALTran idaltran;
        private bool WCFSupport;
        // Methods
        public BuilderFrameS3(IDbObject idbobj, string dbName, string nameSpace, string folder, string dbHelperName)
        {
            base.dbobj = idbobj;
            base._dbtype = idbobj.DbType;
            base.DbName = dbName;
            base.NameSpace = nameSpace;
            base.DbHelperName = dbHelperName;
            base.Folder = folder;            
        }

        public BuilderFrameS3(IDbObject idbobj, string dbName, string tableName, string modelName, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string nameSpace, string folder, string dbHelperName,bool WCF)
        {
            base.dbobj = idbobj;
            base._dbtype = idbobj.DbType;
            base.DbName = dbName;
            base.TableName = tableName;
            base.ModelName = modelName;
            base.NameSpace = nameSpace;
            base.DbHelperName = dbHelperName;
            base.Folder = folder;
            base.Fieldlist = fieldlist;
            base.Keys = keys;
            foreach (ColumnInfo info in keys)
            {
                base._key = info.ColumnName;
                base._keyType = info.TypeName;
                if (info.IsIdentity)
                {
                    base._key = info.ColumnName;
                    base._keyType = CodeCommon.DbTypeToCS(info.TypeName);
                    break;
                }
            }
            WCFSupport = WCF;
        }

        public string GetBLLCode(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
        {
            this.ibll = BuilderFactory.CreateBLLObj(AssemblyGuid);
            if (this.ibll == null)
            {
                return "请选择有效的业务层代码组件类型！";
            }
            this.ibll.Fieldlist = base.Fieldlist;
            this.ibll.Keys = base.Keys;
            this.ibll.NameSpace = base.NameSpace;
            this.ibll.ModelSpace = base.ModelSpace;
            this.ibll.ModelName = base.ModelName;
            this.ibll.Modelpath = base.Modelpath;
            this.ibll.BLLpath = base.BLLpath;
            this.ibll.Factorypath = "";
            this.ibll.IDALpath = "";
            this.ibll.IClass = "";
            this.ibll.DALSpace = this.DALSpace;
            this.ibll.IsHasIdentity = base.IsHasIdentity;
            this.ibll.DbType = base.dbobj.DbType;
            return this.ibll.GetBLLCode(Maxid, Exists, Add, Update, Delete, GetModel, GetModelByCache, List);
        }

        public string GetDALCode(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, string procPrefix)
        {
            this.idal = BuilderFactory.CreateDALObj(AssemblyGuid);
            if (this.idal == null)
            {
                return "请选择有效的数据层代码组件类型！";
            }
            this.idal.DbObject = base.dbobj;
            this.idal.DbName = base.DbName;
            this.idal.TableName = base.TableName;
            this.idal.ModelName = base.ModelName;
            this.idal.NameSpace = base.NameSpace;
            this.idal.DbHelperName = base.DbHelperName;
            this.idal.Folder = base.Folder;
            this.idal.Fieldlist = base.Fieldlist;
            this.idal.Keys = base.Keys;
            this.idal.Modelpath = base.Modelpath;
            this.idal.ModelSpace = base.ModelSpace;
            this.idal.DALpath = this.DALpath;
            this.idal.IDALpath = "";
            this.idal.IClass = "";
            this.idal.ProcPrefix = procPrefix;
            return this.idal.GetDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);
        }

        public string GetDALCodeTran(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, string procPrefix, string tableNameParent, string tableNameSon, string modelNameParent, string modelNameSon, List<ColumnInfo> fieldlistParent, List<ColumnInfo> fieldlistSon, List<ColumnInfo> keysParent, List<ColumnInfo> keysSon, string modelSpaceParent, string modelSpaceSon)
        {
            this.idaltran = BuilderFactory.CreateDALTranObj(AssemblyGuid);
            if (this.idaltran == null)
            {
                return "请选择有效的数据层代码组件类型！";
            }
            this.idaltran.DbObject = base.dbobj;
            this.idaltran.DbName = base.DbName;
            this.idaltran.TableNameParent = tableNameParent;
            this.idaltran.TableNameSon = tableNameSon;
            this.idaltran.ModelNameParent = modelNameParent;
            this.idaltran.ModelNameSon = modelNameSon;
            this.idaltran.NameSpace = base.NameSpace;
            this.idaltran.DbHelperName = base.DbHelperName;
            this.idaltran.Folder = base.Folder;
            this.idaltran.FieldlistParent = fieldlistParent;
            this.idaltran.FieldlistSon = fieldlistSon;
            this.idaltran.KeysParent = keysParent;
            this.idaltran.KeysSon = keysSon;
            this.idaltran.Modelpath = base.Modelpath;
            this.idaltran.ModelSpaceParent = modelSpaceParent;
            this.idaltran.ModelSpaceSon = modelSpaceSon;
            this.idaltran.DALpath = this.DALpath;
            this.idaltran.IDALpath = "";
            this.idaltran.IClass = "";
            this.idaltran.ProcPrefix = procPrefix;
            return this.idaltran.GetDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);
        }

        public string GetModelCode()
        {
            BuilderModel model = new BuilderModel();
            model.ModelName = base.ModelName;
            model.NameSpace = base.NameSpace;
            model.Fieldlist = base.Fieldlist;
            //model.Modelpath = base.Modelpath;
            model.Modelpath = "BusinessEntity";
            model.WCFSupport = WCFSupport;
            //end
            return model.CreatModel();
        }

        public string GetModelCode(string tableNameParent, string modelNameParent, List<ColumnInfo> FieldlistP, string tableNameSon, string modelNameSon, List<ColumnInfo> FieldlistS)
        {
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            if (modelNameSon == "")
            {
                modelNameSon = tableNameSon;
            }
            StringPlus plus = new StringPlus();
            new StringPlus();
            new StringPlus();
            plus.AppendLine("using System;");
            plus.AppendLine("using System.Collections.Generic;");
            plus.AppendLine("namespace " + base.Modelpath);
            plus.AppendLine("{");
            BuilderModelT lt = new BuilderModelT();
            lt.ModelName = modelNameParent;
            lt.NameSpace = base.NameSpace;
            lt.Fieldlist = FieldlistP;
            lt.Modelpath = base.Modelpath;
            lt.ModelNameSon = modelNameSon;
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + modelNameParent + " 。(属性说明自动提取数据库字段的描述信息)");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "public class " + modelNameParent);
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "public " + modelNameParent + "()");
            plus.AppendSpaceLine(2, "{}");
            plus.AppendLine(lt.CreatModelMethodT());
            plus.AppendSpaceLine(1, "}");
            BuilderModel model = new BuilderModel();
            model.ModelName = modelNameSon;
            model.NameSpace = base.NameSpace;
            model.Fieldlist = FieldlistS;
            model.Modelpath = base.Modelpath;
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + modelNameSon + " 。(属性说明自动提取数据库字段的描述信息)");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "public class " + modelNameSon);
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "public " + modelNameSon + "()");
            plus.AppendSpaceLine(2, "{}");
            plus.AppendLine(model.CreatModelMethod());
            plus.AppendSpaceLine(1, "}");
            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }

        // Properties
        public string DALpath
        {
            get
            {
                string str = base.NameSpace + ".DAL";
                if (base.Folder.Trim() != "")
                {
                    str = str + "." + base.Folder;
                }
                return str;
            }
        }

        public string DALSpace
        {
            get
            {
                string str = base.NameSpace + ".DAL";
                if (base.Folder.Trim() != "")
                {
                    str = str + "." + base.Folder;
                }
                return (str + "." + base.ModelName);
            }
        }
    }

    public class BuilderTemp
    {
        // Fields
        private string _dbname;
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys;
        private string _tablename;
        protected IDbObject dbobj;
        private string strXslt = "";

        // Methods
        public BuilderTemp(IDbObject idbobj, string dbName, string tableName, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string strxslt)
        {
            this.dbobj = idbobj;
            this.DbName = dbName;
            this.TableName = tableName;
            this.Fieldlist = fieldlist;
            this.Keys = keys;
            this.Fieldlist = fieldlist;
            this.Keys = keys;
            this.strXslt = strxslt;
        }

        public string GetCode()
        {
            StringWriter writer = new StringWriter();
            if (this.Fieldlist.Count > 0)
            {
                XslTransform transform = new XslTransform();
                transform.Load(this.strXslt);
                transform.Transform((IXPathNavigable)this.GetXml2(), null, (TextWriter)writer, null);
            }
            return writer.ToString();
        }

        private XmlDocument GetXml(DataRow[] dtrows)
        {
            Stream w = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(w, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Schema");
            writer.WriteStartElement("TableName");
            writer.WriteAttributeString("value", "Authors");
            writer.WriteEndElement();
            writer.WriteStartElement("FIELDS");
            foreach (DataRow row in dtrows)
            {
                string str = row["ColumnName"].ToString();
                string str2 = row["TypeName"].ToString();
                writer.WriteStartElement("FIELD");
                writer.WriteAttributeString("Name", str);
                writer.WriteAttributeString("Type", str2);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            TextReader txtReader = new StringReader(writer.ToString());
            XmlDocument document = new XmlDocument();
            document.Load(txtReader);
            return document;
        }

        private XmlDocument GetXml2()
        {
            string filename = @"Template\temp.xml";
            XmlTextWriter writer = new XmlTextWriter(filename, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Schema");
            writer.WriteStartElement("TableName");
            writer.WriteAttributeString("value", this.TableName);
            writer.WriteEndElement();
            writer.WriteStartElement("FIELDS");
            foreach (ColumnInfo info in this.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                string length = info.Length;
                bool isIdentity = info.IsIdentity;
                bool isPK = info.IsPK;
                string deText = info.DeText;
                string defaultVal = info.DefaultVal;
                writer.WriteStartElement("FIELD");
                writer.WriteAttributeString("Name", columnName);
                writer.WriteAttributeString("Type", CodeCommon.DbTypeToCS(typeName));
                writer.WriteAttributeString("Desc", deText);
                writer.WriteAttributeString("defaultVal", defaultVal);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteStartElement("PrimaryKeys");
            foreach (ColumnInfo info2 in this.Keys)
            {
                string str6 = info2.ColumnName;
                string dbtype = info2.TypeName;
                string text2 = info2.Length;
                bool flag3 = info2.IsIdentity;
                bool flag4 = info2.IsPK;
                string str8 = info2.DeText;
                string str9 = info2.DefaultVal;
                writer.WriteStartElement("FIELD");
                writer.WriteAttributeString("Name", str6);
                writer.WriteAttributeString("Type", CodeCommon.DbTypeToCS(dbtype));
                writer.WriteAttributeString("Desc", str8);
                writer.WriteAttributeString("defaultVal", str9);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            return document;
        }

        // Properties
        public string DbName
        {
            get
            {
                return this._dbname;
            }
            set
            {
                this._dbname = value;
            }
        }

        public List<ColumnInfo> Fieldlist
        {
            get
            {
                return this._fieldlist;
            }
            set
            {
                this._fieldlist = value;
            }
        }

        public string Fields
        {
            get
            {
                StringPlus plus = new StringPlus();
                foreach (object obj2 in this.Fieldlist)
                {
                    plus.Append("'" + obj2.ToString() + "',");
                }
                plus.DelLastComma();
                return plus.Value;
            }
        }

        public List<ColumnInfo> Keys
        {
            get
            {
                return this._keys;
            }
            set
            {
                this._keys = value;
            }
        }

        public string TableName
        {
            get
            {
                return this._tablename;
            }
            set
            {
                this._tablename = value;
            }
        }
    }

    public class CodeBuilders
    {
        // Fields
        private string _bllpath;
        private string _bllspace;
        private string _dalpath;
        private string _dbconnectStr;
        private string _dbhelperName;
        private string _dbname;
        private string _dbtype;
        private string _factoryclass;
        private List<ColumnInfo> _fieldlist;
        private string _folder;
        private string _idalpath;
        private List<ColumnInfo> _keys;
        private string _modelname;
        private string _modelpath;
        private string _modelspace;
        private string _namespace;
        private string _procprefix;
        private string _tablename;
        private IDbObject dbobj;

        // Methods
        public CodeBuilders(IDbObject idbobj)
        {
            string str;
            this._namespace = "Maticsoft";
            this._dbhelperName = "DbHelperSQL";
            this.dbobj = idbobj;
            this.DbType = idbobj.DbType;
            if ((this._dbhelperName == "") && ((str = this.DbType) != null))
            {
                if (!(str == "SQL2000") && !(str == "SQL2005"))
                {
                    if (!(str == "Oracle"))
                    {
                        if (!(str == "MySQL"))
                        {
                            if (str == "OleDb")
                            {
                                this._dbhelperName = "DbHelperOleDb";
                            }
                            return;
                        }
                        this._dbhelperName = "DbHelperMySQL";
                        return;
                    }
                }
                else
                {
                    this._dbhelperName = "DbHelperSQL";
                    return;
                }
                this._dbhelperName = "DbHelperOra";
            }
        }

        public string GetAddAspx()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            return web.GetAddAspx();
        }

        public string GetAddAspxCs()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            string addAspxCs = web.GetAddAspxCs();
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "protected void Page_LoadComplete(object sender, EventArgs e)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "(Master.FindControl(\"lblTitle\") as Label).Text = \"信息添加\";");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "protected void btnAdd_Click(object sender, EventArgs e)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, addAspxCs);
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string GetAddDesigner()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            return web.GetAddDesigner();
        }

        public string GetCodeFrameF3BLL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
        {
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName);
            return ef.GetBLLCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, GetModelByCache, List, List);
        }

        public string GetCodeFrameF3DAL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName);
            return ef.GetDALCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, List, this.ProcPrefix);
        }

        public string GetCodeFrameF3DALFactory()
        {
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName);
            return ef.GetDALFactoryCode();
        }

        public string GetCodeFrameF3DALFactoryMethod()
        {
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName);
            return ef.GetDALFactoryMethodCode();
        }

        public string GetCodeFrameF3IDAL(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, bool ListProc)
        {
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName);
            return ef.GetIDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List, ListProc);
        }

        public string GetCodeFrameF3Model()
        {
            BuilderFrameF3 ef = new BuilderFrameF3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName);
            return ef.GetModelCode();
        }

        public string GetCodeFrameOne(string DALtype, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            BuilderFrameOne one = new BuilderFrameOne(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName);
            return one.GetCode(DALtype, Maxid, Exists, Add, Update, Delete, GetModel, List, this.ProcPrefix);
        }

        public string GetCodeFrameS3BLL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List,bool WCFSupport)
        {
            BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName,WCFSupport);
            return es.GetBLLCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, GetModelByCache, List);
        }

        public string GetCodeFrameS3DAL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List,bool WCFSupport)
        {
            BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName, WCFSupport);
            return es.GetDALCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, List, this.ProcPrefix);
        }

        public string GetCodeFrameS3Model(bool WCFSupport)
        {
            BuilderFrameS3 es = new BuilderFrameS3(this.dbobj, this.DbName, this.TableName, this.ModelName, this.Fieldlist, this.Keys, this.NameSpace, this.Folder, this.DbHelperName, WCFSupport);
            return es.GetModelCode();
        }

        public string GetShowAspx()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            return web.GetShowAspx();
        }

        public string GetShowAspxCs()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            string showAspxCs = web.GetShowAspxCs();
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "protected void Page_LoadComplete(object sender, EventArgs e)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "(Master.FindControl(\"lblTitle\") as Label).Text = \"详细信息\";");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "protected void Page_Load(object sender, EventArgs e)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "if (!Page.IsPostBack)");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null || Request.Params[\"id\"].Trim() != \"\")");
            plus.AppendSpaceLine(4, "{");
            plus.AppendSpaceLine(5, "string id = Request.Params[\"id\"];");
            if (this._keys.Count > 0)
            {
                plus.AppendSpaceLine(5, "//ShowInfo(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
            }
            else
            {
                plus.AppendSpaceLine(5, "ShowInfo();");
            }
            plus.AppendSpaceLine(4, "}");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, showAspxCs);
            return plus.ToString();
        }

        public string GetShowDesigner()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            return web.GetShowDesigner();
        }

        public string GetUpdateAspx()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            return web.GetUpdateAspx();
        }

        public string GetUpdateAspxCs()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            string updateAspxCs = web.GetUpdateAspxCs();
            string updateShowAspxCs = web.GetUpdateShowAspxCs();
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "protected void Page_LoadComplete(object sender, EventArgs e)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "(Master.FindControl(\"lblTitle\") as Label).Text = \"信息修改\";");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "protected void Page_Load(object sender, EventArgs e)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "if (!Page.IsPostBack)");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null || Request.Params[\"id\"].Trim() != \"\")");
            plus.AppendSpaceLine(4, "{");
            plus.AppendSpaceLine(5, "string id = Request.Params[\"id\"];");
            if (this._keys.Count > 0)
            {
                plus.AppendSpaceLine(5, "//ShowInfo(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
            }
            else
            {
                plus.AppendSpaceLine(5, "ShowInfo();");
            }
            plus.AppendSpaceLine(4, "}");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(3, updateShowAspxCs);
            plus.AppendSpaceLine(2, "protected void btnAdd_Click(object sender, EventArgs e)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, updateAspxCs);
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string GetUpdateDesigner()
        {
            BuilderWeb web = new BuilderWeb();
            web.NameSpace = this.NameSpace;
            web.Fieldlist = this.Fieldlist;
            web.Keys = this.Keys;
            web.ModelName = this.ModelName;
            web.Folder = this.Folder;
            return web.GetUpdateDesigner();
        }

        // Properties
        public string BLLpath
        {
            get
            {
                string str = this._namespace + ".BLL";
                if (this._folder.Trim() != "")
                {
                    str = str + "." + this._folder;
                }
                return str;
            }
            set
            {
                this._bllpath = value;
            }
        }

        public string BLLSpace
        {
            get
            {
                this._bllspace = this._namespace + ".BLL";
                if (this._folder.Trim() != "")
                {
                    this._bllspace = this._bllspace + "." + this._folder;
                }
                this._bllspace = this._bllspace + "." + this._modelname;
                return this._bllspace;
            }
        }

        public string DALpath
        {
            get
            {
                string str = this._namespace + "." + this._dbtype + "DAL";
                if (this._folder.Trim() != "")
                {
                    str = str + "." + this._folder;
                }
                return str;
            }
            set
            {
                this._dalpath = value;
            }
        }

        public string DbConnectStr
        {
            get
            {
                return this._dbconnectStr;
            }
            set
            {
                this._dbconnectStr = value;
            }
        }

        public string DbHelperName
        {
            get
            {
                return this._dbhelperName;
            }
            set
            {
                this._dbhelperName = value;
            }
        }

        public string DbName
        {
            get
            {
                return this._dbname;
            }
            set
            {
                this._dbname = value;
            }
        }

        public string DbType
        {
            get
            {
                return this._dbtype;
            }
            set
            {
                this._dbtype = value;
            }
        }

        public string FactoryClass
        {
            get
            {
                this._factoryclass = this._namespace + ".DALFactory";
                if (this._folder.Trim() != "")
                {
                    this._factoryclass = this._factoryclass + "." + this._folder;
                }
                this._factoryclass = this._factoryclass + "." + this._modelname;
                return this._factoryclass;
            }
        }

        public string Factorypath
        {
            get
            {
                string str = this._namespace + ".DALFactory";
                if (this._folder.Trim() != "")
                {
                    str = str + "." + this._folder;
                }
                return str;
            }
        }

        public List<ColumnInfo> Fieldlist
        {
            get
            {
                return this._fieldlist;
            }
            set
            {
                this._fieldlist = value;
            }
        }

        public string Folder
        {
            get
            {
                return this._folder;
            }
            set
            {
                this._folder = value;
            }
        }

        public string IClass
        {
            get
            {
                return ("I" + this._modelname);
            }
        }

        public string IDALpath
        {
            get
            {
                this._idalpath = this._namespace + ".IDAL";
                if (this._folder.Trim() != "")
                {
                    this._idalpath = this._idalpath + "." + this._folder;
                }
                return this._idalpath;
            }
        }

        public bool IsHasIdentity
        {
            get
            {
                bool flag = false;
                if (this.Keys.Count > 0)
                {
                    foreach (ColumnInfo info in this.Keys)
                    {
                        if (info.IsIdentity)
                        {
                            flag = true;
                        }
                    }
                }
                return flag;
            }
        }

        public List<ColumnInfo> Keys
        {
            get
            {
                return this._keys;
            }
            set
            {
                this._keys = value;
            }
        }

        public string ModelName
        {
            get
            {
                return this._modelname;
            }
            set
            {
                this._modelname = value;
            }
        }

        public string Modelpath
        {
            get
            {
                this._modelpath = this._namespace + ".Model";
                if (this._folder.Trim() != "")
                {
                    this._modelpath = this._modelpath + "." + this._folder;
                }
                return this._modelpath;
            }
            set
            {
                this._modelpath = value;
            }
        }

        public string ModelSpace
        {
            get
            {
                this._modelspace = this._namespace + ".Model";
                if (this._folder.Trim() != "")
                {
                    this._modelspace = this._modelspace + "." + this._folder;
                }
                this._modelspace = this._modelspace + "." + this._modelname;
                return this._modelspace;
            }
            set
            {
                this._modelspace = value;
            }
        }

        public string NameSpace
        {
            get
            {
                return this._namespace;
            }
            set
            {
                this._namespace = value;
            }
        }

        public string ProcPrefix
        {
            get
            {
                return this._procprefix;
            }
            set
            {
                this._procprefix = value;
            }
        }

        public string TableName
        {
            get
            {
                return this._tablename;
            }
            set
            {
                this._tablename = value;
            }
        }
    }

}
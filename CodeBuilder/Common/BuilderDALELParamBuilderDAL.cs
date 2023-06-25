using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using LTP.Utility;
using LTP.IDBO;
using LTP.CodeHelper;
namespace LTP.BuilderDALELParam
{
    /// <summary>
    /// 数据访问层代码构造器（Parameter方式）
    /// </summary>
    public class BuilderDAL : LTP.IBuilder.IBuilderDAL
    {

        #region 私有变量
        protected string _key = "ID";//标识列，或主键字段		
        protected string _keyType = "int";//标识列，或主键字段类型        
        #endregion

        #region 公有属性
        IDbObject dbobj;
        private string _dbname;
        private string _tablename;
        private string _modelname; //model类名
        private string _dalname;//dal类名    
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys; // 主键或条件字段列表        
        private string _namespace; //顶级命名空间名
        private string _folder; //所在文件夹
        private string _dbhelperName;//数据库访问类名           
        private string _modelpath;
        private string _dalpath;
        private string _idalpath;
        private string _iclass;
        private string _procprefix;

        public IDbObject DbObject
        {
            set { dbobj = value; }
            get { return dbobj; }
        }
        /// <summary>
        /// 库名
        /// </summary>
        public string DbName
        {
            set { _dbname = value; }
            get { return _dbname; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }

        /// <summary>
        /// 选择要生成的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// 主键或条件字段的集合
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }
        /// <summary>
        /// 顶级命名空间名
        /// </summary>
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        /// <summary>
        /// 所在文件夹
        /// </summary>
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }

        /*============================*/

        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        /// <summary>
        /// 类名
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// 实体类的整个命名空间 + 类名，即等于 Modelpath+ModelName
        /// </summary>
        public string ModelSpace
        {
            get { return Modelpath + "." + ModelName; }
        }
        /*============================*/

        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        public string DALpath
        {
            set { _dalpath = value; }
            get
            {
                return _dalpath;
            }
        }
        public string DALName
        {
            set { _dalname = value; }
            get { return _dalname; }
        }
        /*============================*/


        /// <summary>
        /// 接口的命名空间
        /// </summary>
        public string IDALpath
        {
            set { _idalpath = value; }
            get
            {
                return _idalpath;
            }
        }
        /// <summary>
        /// 接口类名
        /// </summary>
        public string IClass
        {
            set { _iclass = value; }
            get { return _iclass; }
        }
        /*============================*/

        /// <summary>
        /// 数据库访问类名
        /// </summary>
        public string DbHelperName
        {
            set { _dbhelperName = value; }
            get { return _dbhelperName; }
        }
        /// <summary>
        /// 存储过程前缀 
        /// </summary>       
        public string ProcPrefix
        {
            set { _procprefix = value; }
            get { return _procprefix; }
        }
        #endregion

        #region 构造属性
                
        /// <summary>
        /// 所选字段的 select 列表
        /// </summary>
        public string Fieldstrlist
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (ColumnInfo obj in Fieldlist)
                {
                    _fields.Append(obj.ColumnName + ",");
                }
                _fields.DelLastComma();
                return _fields.Value;
            }
        }
       
        /// <summary>
        ///  不同数据库字段类型
        /// </summary>
        public string DbParaDbType
        {
            get
            {
                return "DbType";
            }
        }

        /// <summary>
        /// 存储过程参数 调用符号@
        /// </summary>
        public string preParameter
        {
            get
            {               
               return "@";
            }
        }

        /// <summary>
        /// 列中是否有标识列
        /// </summary>
        public bool IsHasIdentity
        {
            get
            {
                bool isid = false;
                if (_keys.Count > 0)
                {
                    foreach (ColumnInfo key in _keys)
                    {
                        if (key.IsIdentity)
                        {
                            isid = true;
                        }
                    }
                }
                return isid;
            }
        }
        private string KeysNullTip
        {
            get
            {
                if (_keys.Count == 0)
                {
                    return "//该表无主键信息，请自定义主键/条件字段";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region 构造函数

        public BuilderDAL()
        {
        }
        public BuilderDAL(IDbObject idbobj)
        {
            dbobj = idbobj;
        }

        public BuilderDAL(IDbObject idbobj, string dbname, string tablename, string modelname, string dalName, 
            List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string namepace,
            string folder, string dbherlpername, string modelpath, string modelspace,
            string dalpath, string idalpath, string iclass)
        {
            dbobj = idbobj;
            _dbname = dbname;
            _tablename = tablename;
            _modelname = modelname;
            _dalname = dalName;
            _namespace = namepace;
            _folder = folder;
            _dbhelperName = dbherlpername;
            _modelpath = modelpath;           
            _dalpath = dalpath;
            _idalpath = idalpath;
            _iclass = iclass;
            Fieldlist = fieldlist;
            Keys = keys;
            foreach (ColumnInfo key in _keys)
            {
                _key = key.ColumnName;
                _keyType = key.TypeName;
                if (key.IsIdentity)
                {
                    _key = key.ColumnName;
                    _keyType = CodeCommon.DbTypeToCS(key.TypeName);
                    break;
                }
            }
        }

        #endregion
       
        #region  根据列信息 得到参数的列表
        
        /// <summary>
        /// 得到Where条件语句 - Parameter方式 (例如：用于Exists  Delete  GetModel 的where)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetWhereExpression(List<ColumnInfo> keys)
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo key in keys)
            {
                strclass.Append(key.ColumnName + "=" + preParameter + key.ColumnName + " and ");
            }
            strclass.DelLastChar("and");
            return strclass.Value;
        }

        /// <summary>
        /// 生成sql语句中的参数列表(例如：用于Add  Exists  Update Delete  GetModel 的参数传入)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetPreParameter(List<ColumnInfo> keys)
        {
            StringPlus strclass = new StringPlus();  
            foreach (ColumnInfo key in keys)
            {
                strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + key.ColumnName + "\", DbType." +CSToProcType(key.TypeName) + "," + key.ColumnName + ");");
            }       
            return strclass.Value;
        }

         #endregion
        
        #region 数据层(整个类)
        /// <summary>
        /// 得到整个类的代码
        /// </summary>     
        public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using Microsoft.Practices.EnterpriseLibrary.Data;");
            strclass.AppendLine("using Microsoft.Practices.EnterpriseLibrary.Data.Sql;");
            strclass.AppendLine("using System.Data.Common;");
            if (IDALpath != "")
            {
                strclass.AppendLine("using " + IDALpath + ";");
            }
            strclass.AppendLine("using Maticsoft.DBUtility;//请先添加引用");
            strclass.AppendLine("namespace " + DALpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 数据访问类" + DALName + "。");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpace(1, "public class " + DALName);
            if (IClass != "")
            {
                strclass.Append(":" + IClass);
            }
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + DALName + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendSpaceLine(2, "#region  成员方法");

            #region  方法代码
            if (Maxid)
            {
                strclass.AppendLine(CreatGetMaxID());
            }
            if (Exists)
            {
                strclass.AppendLine(CreatExists());
            }
            if (Add)
            {
                strclass.AppendLine(CreatAdd());
            }
            if (Update)
            {
                strclass.AppendLine(CreatUpdate());
            }
            if (Delete)
            {
                strclass.AppendLine(CreatDelete());
            }
            if (GetModel)
            {
                strclass.AppendLine(CreatGetModel());
            }
            if (List)
            {
                strclass.AppendLine(CreatGetList());
                strclass.AppendLine(CreatGetListByPageProc());
                strclass.AppendLine(CreatGetListArray());
                strclass.AppendLine(CreatReaderBind());
               
            }
            #endregion

            strclass.AppendSpaceLine(2, "#endregion  成员方法");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }

        #endregion

        #region 数据层(使用Parameter实现)

        /// <summary>
        /// 得到最大ID的方法代码
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatGetMaxID()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                string keyname = "";
                foreach (ColumnInfo obj in _keys)
                {
                    if (CodeCommon.DbTypeToCS(obj.TypeName) == "int")
                    {
                        keyname = obj.ColumnName;
                        if (obj.IsPK)
                        {
                            strclass.AppendLine("");
                            strclass.AppendSpaceLine(2, "/// <summary>");
                            strclass.AppendSpaceLine(2, "/// 得到最大ID");
                            strclass.AppendSpaceLine(2, "/// </summary>");
                            strclass.AppendSpaceLine(2, "public int GetMaxId()");
                            strclass.AppendSpaceLine(2, "{" );
                            strclass.AppendSpaceLine(3, "string strsql = \"select max(" + keyname + ")+1 from " + _tablename + "\";");
                            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
                            strclass.AppendSpaceLine(3, "object obj = db.ExecuteScalar(CommandType.Text, strsql);");
                            strclass.AppendSpaceLine(3, "if (obj != null && obj != DBNull.Value)");
                            strclass.AppendSpaceLine(3, "{");
                            strclass.AppendSpaceLine(4, "return int.Parse(obj.ToString());");
                            strclass.AppendSpaceLine(3, "}");
                            strclass.AppendSpaceLine(3, "return 1;");                           
                            strclass.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return strclass.ToString();
        }

        /// <summary>
        /// 得到Exists方法的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatExists()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// 是否存在该记录");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Exists(" +LTP.CodeHelper.CodeCommon.GetInParameter(Keys) + ")");
                strclass.AppendSpaceLine(2, "{");

                strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
                strclass.AppendSpaceLine(3, "StringBuilder strSql = new StringBuilder();");
                strclass.AppendSpace(3, "strSql.Append(\"select count(1) from " + _tablename);
                strclass.AppendLine(" where " + GetWhereExpression(Keys) + "\");");
                strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());");                
                strclass.Append(GetPreParameter(Keys));
                strclass.AppendSpaceLine(3, "int cmdresult;");
                strclass.AppendSpaceLine(3, "object obj = db.ExecuteScalar(dbCommand);");

                strclass.AppendSpaceLine(3, "if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "cmdresult = 0;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "cmdresult = int.Parse(obj.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "if (cmdresult == 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return false;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return true;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");              
            }            
            return strclass.Value;
        }

        /// <summary>
        /// 得到Add()的代码
        /// </summary>        
        public string CreatAdd()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            StringPlus strclass3 = new StringPlus();
            StringPlus strclass4 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>" );
            strclass.AppendSpaceLine(2, "/// 增加一条数据" );
            strclass.AppendSpaceLine(2, "/// </summary>" );
            string strretu = "void";
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005") && (IsHasIdentity))
            {
                strretu ="int";
            }
            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.AppendLine(strFun);            
            strclass.AppendSpaceLine(2, "{" );
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();" );
            strclass.AppendSpaceLine(3, "strSql.Append(\"insert into " + _tablename + "(\");" );
            strclass1.AppendSpace(3, "strSql.Append(\"");          
            foreach (ColumnInfo field in Fieldlist)
                {
                    string columnName = field.ColumnName;
                    string columnType = field.TypeName;
                    bool IsIdentity = field.IsIdentity;
                    string Length = field.Length;
                    if (field.IsIdentity)
                    {                        
                        continue;
                    }        
                    strclass1.Append(columnName + ",");   
                    strclass2.Append(preParameter + columnName + ",");
                    strclass3.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(columnType) + ", model." + columnName + ");");  
                }
           
            //去掉最后的逗号
            strclass1.DelLastComma();
            strclass2.DelLastComma();
            //strclass3.DelLastComma();
            strclass1.AppendLine(")\");");
            strclass.AppendLine(strclass1.ToString());
            strclass.AppendSpaceLine(3, "strSql.Append(\" values (\");" );
            strclass.AppendSpaceLine(3, "strSql.Append(\"" + strclass2.ToString() + ")\");" );
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\";select @@IDENTITY\");");
            }

            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());");  
            strclass.Append(strclass3.Value);
            //重新定义方法头
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "int result;");
                strclass.AppendSpaceLine(3, "object obj = db.ExecuteScalar(dbCommand);");
                strclass.AppendSpaceLine(3, "if(!int.TryParse(obj.ToString(),out result))");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return 0;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "return result;");
            }
            else
            {
                strclass.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);");          
            }
            strclass.AppendSpace(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// 得到Update（）的代码
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatUpdate()
        {            
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 更新一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"update " + _tablename + " set \");");
            //int n = 0;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;
                bool IsIdentity = field.IsIdentity;
                bool isPK = field.IsPK;

                strclass1.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(columnType) + ", model." + columnName + ");");

                if (field.IsIdentity || field.IsPK || (Keys.Contains(field)))
                {
                    continue;
                }
                strclass.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=" + preParameter + columnName + ",\");");
            }
            

            //去掉最后的逗号			
            strclass.DelLastComma();
            strclass.AppendLine("\");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + GetWhereExpression(Keys) + "\");");


            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());"); 


            strclass.Append(strclass1.Value);
            strclass.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);\r\n"); 
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        /// <summary>
        /// 得到Delete的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDelete()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 删除一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Delete(" + LTP.CodeHelper.CodeCommon.GetInParameter(Keys) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            //if (dbobj.DbType != "OleDb")
            //{
            //    strclass.AppendSpaceLine(3, "strSql.Append(\"delete " + _tablename + " \");");
            //}
            //else
            //{
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
            //}
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + GetWhereExpression(Keys) + "\");");

            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());"); 



            strclass.Append(GetPreParameter(Keys));

            strclass.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);\r\n"); 
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }

        /// <summary>
        /// 得到GetModel()的代码
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatGetModel()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.Append("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 得到一个对象实体");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(" + LTP.CodeHelper.CodeCommon.GetInParameter(Keys) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"select " + Fieldstrlist + " from " + _tablename + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + GetWhereExpression(Keys) + "\");");


            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());");

            strclass.Append(GetPreParameter(Keys));

            strclass.AppendSpaceLine(3, "" + ModelSpace + " model=null;");


            strclass.AppendSpaceLine(3, "using (IDataReader dataReader = db.ExecuteReader(dbCommand))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "if(dataReader.Read())");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "model=ReaderBind(dataReader);");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3,"return model;");
            strclass.AppendSpaceLine(2, "}");


            return strclass.Value;
        }

        /// <summary>
        /// 得到GetList()的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetList()
        {
            StringPlus strclass = new StringPlus();         
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(Fieldstrlist + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "return db.ExecuteDataSet(CommandType.Text, strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }



        /// <summary>
        /// 得到GetList()的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetListByPageProc()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/*");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 分页获取数据列表");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3,"Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3,"DbCommand dbCommand = db.GetStoredProcCommand(\"UP_GetRecordByPage\");");
            strclass.AppendSpaceLine(3,"db.AddInParameter(dbCommand, \"tblName\", DbType.AnsiString, \""+TableName+"\");");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"fldName\", DbType.AnsiString, \"" + _key + "\");");
            strclass.AppendSpaceLine(3,"db.AddInParameter(dbCommand, \"PageSize\", DbType.Int32, PageSize);");
            strclass.AppendSpaceLine(3,"db.AddInParameter(dbCommand, \"PageIndex\", DbType.Int32, PageIndex);");
            strclass.AppendSpaceLine(3,"db.AddInParameter(dbCommand, \"IsReCount\", DbType.Boolean, 0);");
            strclass.AppendSpaceLine(3,"db.AddInParameter(dbCommand, \"OrderType\", DbType.Boolean, 0);");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"strWhere\", DbType.AnsiString, strWhere);");
            strclass.AppendSpaceLine(3, "return db.ExecuteDataSet(dbCommand);");
            strclass.AppendSpaceLine(2, "}*/");
            return strclass.Value;
        }

        #region  生成对象实体绑定数据

        /// <summary>
        /// 生成对象实体绑定数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string CreatReaderBind()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 对象实体绑定数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " ReaderBind(IDataReader dataReader)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, ModelSpace + " model=new " + ModelSpace + "();");
            
            bool isobj = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;

                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(int)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "long":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(long)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "decimal":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");

                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(decimal)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(DateTime)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "string":
                        {
                            strclass1.AppendSpaceLine(3, "model." + columnName + "=dataReader[\"" + columnName + "\"].ToString();");
                        }
                        break;
                    case "bool":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");

                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(bool)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");

                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(byte[])ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "Guid":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "= new Guid(ojb.ToString());");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    default:
                        strclass1.AppendSpaceLine(3, "model." + columnName + "=dataReader[\"" + columnName + "\"].ToString();\r\n");
                        break;
                }
            }
            if (isobj)
            {
                strclass.AppendSpaceLine(3, "object ojb; ");
            }            
            strclass.Append(strclass1.ToString());
            strclass.AppendSpaceLine(3, "return model;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }


        public string CreatGetListArray()
        {
            string strList = "List<" + ModelSpace + ">";
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表（比DataSet效率高，推荐使用）");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + strList + " GetListArray(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(Fieldstrlist + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, strList + " list = new " + strList + "();");
            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "while (dataReader.Read())");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "list.Add(ReaderBind(dataReader));");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return list;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }


        #endregion

        #endregion

        #region CSToProcType
        /// <summary>
        /// 企业库数据库字段对应
        /// </summary>
        /// <param name="cstype"></param>
        /// <returns></returns>
        private static string CSToProcType(string cstype)
        {
            string ProcType = cstype;
            switch (cstype.Trim().ToLower())
            {
                
                case "string":
                case "nvarchar":                
                case "nchar":                
                case "ntext":
                    ProcType = "String";
                    break;
                case "text":
                case "char":
                case "varchar":
                    ProcType = "AnsiString";
                    break;
                case "datetime":
                case "smalldatetime":
                    ProcType = "DateTime";
                    break;
                case "smallint":
                    ProcType = "Int16";
                    break;
                case "tinyint":
                    ProcType = "Byte";
                    break;
                case "int":
                    ProcType = "Int32";
                    break;
                case "bigint":
                case "long":
                    ProcType = "Int64";
                    break;
                case "float":
                    ProcType = "Double";
                    break;
                case "real":
                case "numeric":
                case "decimal":
                    ProcType = "Decimal";
                    break;
                case "money":
                case "smallmoney":
                    ProcType = "Currency";
                    break;
                case "bool":
                case "bit":
                    ProcType = "Boolean";
                    break;
                case "binary":
                case "varbinary":
                    ProcType = "Binary";
                    break;
                case "image":
                    ProcType = "Image";
                    break;
                case "uniqueidentifier":
                    ProcType = "Guid";
                    break;
                case "timestamp":
                    ProcType = "String";
                    break;
                default:
                    ProcType = "String";
                    break;
            }
            return ProcType;
        }

        #endregion

    }
}

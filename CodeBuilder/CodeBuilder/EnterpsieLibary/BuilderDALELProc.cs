using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.Schema;


namespace Flextronics.Applications.Library.CodeBuilder.EnterpriseLibrary
{
    public class BuilderDALProcedure : IBuilderDAL
{
#region  Fields
    private string _dalpath;
    private string _dbhelperName;
    private string _dbname;
    private List<ColumnInfo> _fieldlist;
    private string _folder;
    private string _iclass;
    private string _idalpath;
    protected string _key;
    private List<ColumnInfo> _keys;
    protected string _keyType;
    private string _modelname;
    private string _modelpath;
    private string _modelspace;
    private string _namespace;
    private string _procprefix;
    private string _tablename;
    private IDbObject dbobj;
#endregion 
    // Methods
    public BuilderDALProcedure()
    {
        this._key = "ID";
        this._keyType = "int";
    }

    public BuilderDALProcedure(IDbObject idbobj)
    {
        this._key = "ID";
        this._keyType = "int";
        this.dbobj = idbobj;
    }

    public BuilderDALProcedure(IDbObject idbobj, string dbname, string tablename, string modelname, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string namepace, string folder, string dbherlpername, string modelpath, string modelspace, string dalpath, string idalpath, string iclass)
    {
        this._key = "ID";
        this._keyType = "int";
        this.dbobj = idbobj;
        this._dbname = dbname;
        this._tablename = tablename;
        this._modelname = modelname;
        this._namespace = namepace;
        this._folder = folder;
        this._dbhelperName = dbherlpername;
        this._modelpath = modelpath;
        this._modelspace = modelspace;
        this._dalpath = dalpath;
        this._idalpath = idalpath;
        this._iclass = iclass;
        this.Fieldlist = fieldlist;
        this.Keys = keys;
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
    }

    public string CreatAdd()
    {
        if (this.ModelSpace == "")
        {
            this.ModelSpace = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "///  增加一条数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        string str = "void";
        if (this.IsHasIdentity)
        {
            str = "int";
        }
        string text = CodeCommon.Space(2) + "public " + str + " Add(" + this.ModelSpace + " model)";
        plus.AppendLine(text);
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
        plus.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + this.ProcPrefix + this._tablename + "_ADD\");");
        string str3 = string.Empty;
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isIdentity = info.IsIdentity;
            string length = info.Length;
            if (info.IsIdentity)
            {
                plus.AppendSpaceLine(3, "db.AddOutParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(typeName) + ", " + length + ");");
                str3 = columnName;
            }
            else
            {
                plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(typeName) + ", model." + columnName + ");");
            }
        }
        plus.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);");
        if (this.IsHasIdentity)
        {
            plus.AppendSpaceLine(3, "return (" + this._keyType + ")db.GetParameterValue(dbCommand, \"" + str3 + "\");");
        }
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatDelete()
    {
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 删除一条数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public void Delete(" + CodeCommon.GetInParameter(this.Keys) + ")");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
        plus.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + this.ProcPrefix + this._tablename + "_Delete\");");
        plus.AppendLine(this.GetPreParameter(this.Keys));
        plus.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatExists()
    {
        StringPlus plus = new StringPlus();
        if (this._keys.Count > 0)
        {
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 是否存在该记录");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public bool Exists(" + CodeCommon.GetInParameter(this.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            plus.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + this.ProcPrefix + this._tablename + "_Exists\");");
            plus.Append(this.GetPreParameter(this.Keys));
            plus.AppendSpaceLine(3, "int result;");
            plus.AppendSpaceLine(3, "object obj = db.ExecuteScalar(dbCommand);");
            plus.AppendSpaceLine(3, "int.TryParse(obj.ToString(),out result);");
            plus.AppendSpaceLine(3, "if(result==1)");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "return true;");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(3, "else");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "return false;");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(2, "}");
        }
        return plus.Value;
    }

    public string CreatGetList()
    {
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 获得数据列表");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpace(3, "strSql.Append(\"select ");
        plus.AppendLine(this.Fieldstrlist + " \");");
        plus.AppendSpaceLine(3, "strSql.Append(\" FROM " + this.TableName + " \");");
        plus.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
        plus.AppendSpaceLine(3, "{");
        plus.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
        plus.AppendSpaceLine(3, "return db.ExecuteDataSet(CommandType.Text, strSql.ToString());");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatGetListArray()
    {
        string str = "List<" + this.ModelSpace + ">";
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 获得数据列表（比DataSet效率高，推荐使用）");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public " + str + " GetListArray(string strWhere)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpace(3, "strSql.Append(\"select ");
        plus.AppendLine(this.Fieldstrlist + " \");");
        plus.AppendSpaceLine(3, "strSql.Append(\" FROM " + this.TableName + " \");");
        plus.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
        plus.AppendSpaceLine(3, "{");
        plus.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpaceLine(3, str + " list = new " + str + "();");
        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
        plus.AppendSpaceLine(3, "using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))");
        plus.AppendSpaceLine(3, "{");
        plus.AppendSpaceLine(4, "while (dataReader.Read())");
        plus.AppendSpaceLine(4, "{");
        plus.AppendSpaceLine(5, "list.Add(ReaderBind(dataReader));");
        plus.AppendSpaceLine(4, "}");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpaceLine(3, "return list;");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatGetListByPageProc()
    {
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(2, "/*");
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 分页获取数据列表");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
        plus.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"UP_GetRecordByPage\");");
        plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"tblName\", DbType.AnsiString, \"" + this.TableName + "\");");
        plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"fldName\", DbType.AnsiString, \"" + this._key + "\");");
        plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"PageSize\", DbType.Int32, PageSize);");
        plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"PageIndex\", DbType.Int32, PageIndex);");
        plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"IsReCount\", DbType.Boolean, 0);");
        plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"OrderType\", DbType.Boolean, 0);");
        plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"strWhere\", DbType.AnsiString, strWhere);");
        plus.AppendSpaceLine(3, "return db.ExecuteDataSet(dbCommand);");
        plus.AppendSpaceLine(2, "}*/");
        return plus.Value;
    }

    public string CreatGetMaxID()
    {
        StringPlus plus = new StringPlus();
        if (this._keys.Count > 0)
        {
            string columnName = "";
            foreach (ColumnInfo info in this._keys)
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
                        plus.AppendSpaceLine(2, "{");
                        plus.AppendSpaceLine(3, "string strsql = \"select max(" + columnName + ")+1 from " + this._tablename + "\";");
                        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
                        plus.AppendSpaceLine(3, "object obj = db.ExecuteScalar(CommandType.Text, strsql);");
                        plus.AppendSpaceLine(3, "if (obj != null && obj != DBNull.Value)");
                        plus.AppendSpaceLine(3, "{");
                        plus.AppendSpaceLine(4, "return int.Parse(obj.ToString());");
                        plus.AppendSpaceLine(3, "}");
                        plus.AppendSpaceLine(3, "return 1;");
                        plus.AppendSpaceLine(2, "}");
                        break;
                    }
                }
            }
        }
        return plus.ToString();
    }

    public string CreatGetModel()
    {
        if (this.ModelSpace == "")
        {
            this.ModelSpace = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 得到一个对象实体");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public " + this.ModelSpace + " GetModel(" + CodeCommon.GetInParameter(this.Keys) + ")");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
        plus.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + this.ProcPrefix + this._tablename + "_GetModel\");");
        plus.AppendLine(this.GetPreParameter(this.Keys));
        plus.AppendSpaceLine(3, "" + this.ModelSpace + " model=null;");
        plus.AppendSpaceLine(3, "using (IDataReader dataReader = db.ExecuteReader(dbCommand))");
        plus.AppendSpaceLine(3, "{");
        plus.AppendSpaceLine(4, "if(dataReader.Read())");
        plus.AppendSpaceLine(4, "{");
        plus.AppendSpaceLine(5, "model=ReaderBind(dataReader);");
        plus.AppendSpaceLine(4, "}");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpaceLine(3, "return model;");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatReaderBind()
    {
        if (this.ModelSpace == "")
        {
            this.ModelSpace = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        plus.AppendLine("");
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 对象实体绑定数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public " + this.ModelSpace + " ReaderBind(IDataReader dataReader)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, this.ModelSpace + " model=new " + this.ModelSpace + "();");
        bool flag = false;
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isIdentity = info.IsIdentity;
            string length = info.Length;
            switch (CodeCommon.DbTypeToCS(typeName))
            {
                case "int":
                {
                    flag = true;
                    plus2.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                    plus2.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                    plus2.AppendSpaceLine(3, "{");
                    plus2.AppendSpaceLine(4, "model." + columnName + "=(int)ojb;");
                    plus2.AppendSpaceLine(3, "}");
                    continue;
                }
                case "long":
                {
                    flag = true;
                    plus2.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                    plus2.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                    plus2.AppendSpaceLine(3, "{");
                    plus2.AppendSpaceLine(4, "model." + columnName + "=(long)ojb;");
                    plus2.AppendSpaceLine(3, "}");
                    continue;
                }
                case "decimal":
                {
                    flag = true;
                    plus2.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                    plus2.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                    plus2.AppendSpaceLine(3, "{");
                    plus2.AppendSpaceLine(4, "model." + columnName + "=(decimal)ojb;");
                    plus2.AppendSpaceLine(3, "}");
                    continue;
                }
                case "DateTime":
                {
                    flag = true;
                    plus2.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                    plus2.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                    plus2.AppendSpaceLine(3, "{");
                    plus2.AppendSpaceLine(4, "model." + columnName + "=(DateTime)ojb;");
                    plus2.AppendSpaceLine(3, "}");
                    continue;
                }
                case "string":
                {
                    plus2.AppendSpaceLine(3, "model." + columnName + "=dataReader[\"" + columnName + "\"].ToString();");
                    continue;
                }
                case "bool":
                {
                    flag = true;
                    plus2.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                    plus2.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                    plus2.AppendSpaceLine(3, "{");
                    plus2.AppendSpaceLine(4, "model." + columnName + "=(bool)ojb;");
                    plus2.AppendSpaceLine(3, "}");
                    continue;
                }
                case "byte[]":
                {
                    flag = true;
                    plus2.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                    plus2.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                    plus2.AppendSpaceLine(3, "{");
                    plus2.AppendSpaceLine(4, "model." + columnName + "=(byte[])ojb;");
                    plus2.AppendSpaceLine(3, "}");
                    continue;
                }
                case "Guid":
                {
                    flag = true;
                    plus2.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                    plus2.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                    plus2.AppendSpaceLine(3, "{");
                    plus2.AppendSpaceLine(4, "model." + columnName + "= new Guid(ojb.ToString());");
                    plus2.AppendSpaceLine(3, "}");
                    continue;
                }
            }
            plus2.AppendSpaceLine(3, "model." + columnName + "=dataReader[\"" + columnName + "\"].ToString();\r\n");
        }
        if (flag)
        {
            plus.AppendSpaceLine(3, "object ojb; ");
        }
        plus.Append(plus2.ToString());
        plus.AppendSpaceLine(3, "return model;");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatUpdate()
    {
        if (this.ModelSpace == "")
        {
            this.ModelSpace = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "///  更新一条数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public void Update(" + this.ModelSpace + " model)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
        plus.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + this.ProcPrefix + this._tablename + "_Update\");");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string length = info.Length;
            plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(typeName) + ", model." + columnName + ");");
        }
        plus.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    private static string CSToProcType(string cstype)
    {
        string str = cstype;
        switch (cstype.Trim().ToLower())
        {
            case "string":
            case "nvarchar":
            case "nchar":
            case "ntext":
                return "String";

            case "text":
            case "char":
            case "varchar":
                return "AnsiString";

            case "datetime":
            case "smalldatetime":
                return "DateTime";

            case "smallint":
                return "Int16";

            case "tinyint":
                return "Byte";

            case "int":
                return "Int32";

            case "bigint":
            case "long":
                return "Int64";

            case "float":
                return "Double";

            case "real":
            case "numeric":
            case "decimal":
                return "Decimal";

            case "money":
            case "smallmoney":
                return "Currency";

            case "bool":
            case "bit":
                return "Boolean";

            case "binary":
            case "varbinary":
                return "Binary";

            case "image":
                return "Image";

            case "uniqueidentifier":
                return "Guid";

            case "timestamp":
                return "String";
        }
        return "String";
    }

    public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine("using System;");
        plus.AppendLine("using System.Data;");
        plus.AppendLine("using System.Text;");
        plus.AppendLine("using System.Collections.Generic;");
        plus.AppendLine("using Microsoft.Practices.EnterpriseLibrary.Data;");
        plus.AppendLine("using Microsoft.Practices.EnterpriseLibrary.Data.Sql;");
        plus.AppendLine("using System.Data.Common;");
        if (this.IDALpath != "")
        {
            plus.AppendLine("using " + this.IDALpath + ";");
        }
        //plus.AppendLine("using Maticsoft.DBUtility;//请先添加引用");
        //plus.AppendLine("namespace " + this.DALpath);
        plus.AppendLine("namespace  Service ");

        plus.AppendLine("{");
        plus.AppendSpaceLine(1, "/// <summary>");
        plus.AppendSpaceLine(1, "/// 数据访问类" + this.ModelName + "。");
        plus.AppendSpaceLine(1, "/// </summary>");
        plus.AppendSpace(1, "public class " + this.ModelName);
        if (this.IClass != "")
        {
            plus.Append(":" + this.IClass);
        }
        plus.AppendLine("");
        plus.AppendSpaceLine(1, "{");
        plus.AppendSpaceLine(2, "public " + this.ModelName + "()");
        plus.AppendSpaceLine(2, "{}");
        plus.AppendSpaceLine(2, "#region  成员方法");
        if (Maxid)
        {
            plus.AppendLine(this.CreatGetMaxID());
        }
        if (Exists)
        {
            plus.AppendLine(this.CreatExists());
        }
        if (Add)
        {
            plus.AppendLine(this.CreatAdd());
        }
        if (Update)
        {
            plus.AppendLine(this.CreatUpdate());
        }
        if (Delete)
        {
            plus.AppendLine(this.CreatDelete());
        }
        if (GetModel)
        {
            plus.AppendLine(this.CreatGetModel());
        }
        if (List)
        {
            plus.AppendLine(this.CreatGetList());
            plus.AppendLine(this.CreatGetListByPageProc());
            plus.AppendLine(this.CreatGetListArray());
            plus.AppendLine(this.CreatReaderBind());
        }
        plus.AppendSpaceLine(2, "#endregion  成员方法");
        plus.AppendSpaceLine(1, "}");
        plus.AppendLine("}");
        plus.AppendLine("");
        return plus.ToString();
    }

    public string GetPreParameter(List<ColumnInfo> keys)
    {
        StringPlus plus = new StringPlus();
        foreach (ColumnInfo info in keys)
        {
            plus.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + info.ColumnName + "\", DbType." + CSToProcType(info.TypeName) + "," + info.ColumnName + ");");
        }
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
    public string DALpath
    {
        get
        {
            return this._dalpath;
        }
        set
        {
            this._dalpath = value;
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

    public IDbObject DbObject
    {
        get
        {
            return this.dbobj;
        }
        set
        {
            this.dbobj = value;
        }
    }

    public string DbParaDbType
    {
        get
        {
            return "DbType";
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
            foreach (ColumnInfo info in this.Fieldlist)
            {
                plus.Append("'" + info.ColumnName + "',");
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
            return this._iclass;
        }
        set
        {
            this._iclass = value;
        }
    }

    public string IDALpath
    {
        get
        {
            return this._idalpath;
        }
        set
        {
            this._idalpath = value;
        }
    }

    public bool IsHasIdentity
    {
        get
        {
            bool flag = false;
            if (this._keys.Count > 0)
            {
                foreach (ColumnInfo info in this._keys)
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

    public string preParameter
    {
        get
        {
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
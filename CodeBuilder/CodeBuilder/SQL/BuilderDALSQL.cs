using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.Schema;

namespace Flextronics.Applications.Library.CodeBuilder.SQL
{
    public class BuilderDALSQL : IBuilderDAL
{
#region  Fields
    private string _dalpath;
    private string _dbhelperName;
    private string _dbname;
    private List<ColumnInfo> _fieldlist;
    private string _folder;
    private string _iclass;
    private string _idalpath;
    private List<ColumnInfo> _keys;
    private string _modelname;
    private string _modelpath;
    private string _modelspace;
    private string _namespace;
    private string _procprefix;
    private string _tablename;
    private IDbObject dbobj;
#endregion

    // Methods
    public BuilderDALSQL()
    {
    }

    public BuilderDALSQL(IDbObject idbobj)
    {
        this.dbobj = idbobj;
    }

    public BuilderDALSQL(IDbObject idbobj, string dbname, string tablename, string modelname, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string namepace, string folder, string dbherlpername, string modelpath, string modelspace, string dalpath, string idalpath, string iclass)
    {
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
    }

    public string CreatAdd()
    {
        if (this.ModelSpace == "")
        {
            this.ModelSpace = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        plus.AppendLine("");
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 增加一条数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        string str = "void";
        if (((this.dbobj.DbType == "SQL2000") || (this.dbobj.DbType == "SQL2005")) && this.IsHasIdentity)
        {
            str = "int";
        }
        string text = CodeCommon.Space(2) + "public " + str + " Add(" + this.ModelSpace + " model)";
        plus.AppendLine(text);
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpaceLine(3, "StringBuilder strSql1=new StringBuilder();");
        plus.AppendSpaceLine(3, "StringBuilder strSql2=new StringBuilder();");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            if (!info.IsIdentity)
            {
                plus.AppendSpaceLine(3, "if (model." + columnName + " != null)");
                plus.AppendSpaceLine(3, "{");
                plus.AppendSpaceLine(4, "strSql1.Append(\"" + columnName + ",\");");
                if (CodeCommon.IsAddMark(typeName.Trim()))
                {
                    plus.AppendSpaceLine(4, "strSql2.Append(\"'\"+model." + columnName + "+\"',\");");
                }
                else
                {
                    plus.AppendSpaceLine(4, "strSql2.Append(\"\"+model." + columnName + "+\",\");");
                }
                plus.AppendSpaceLine(3, "}");
            }
        }
        plus.AppendSpaceLine(3, "strSql.Append(\"insert into " + this.TableName + "(\");");
        plus.AppendSpaceLine(3, "strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));");
        plus.AppendSpaceLine(3, "strSql.Append(\")\");");
        plus.AppendSpaceLine(3, "strSql.Append(\" values (\");");
        plus.AppendSpaceLine(3, "strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));");
        plus.AppendSpaceLine(3, "strSql.Append(\")\");");
        if (((this.dbobj.DbType == "SQL2000") || (this.dbobj.DbType == "SQL2005")) && this.IsHasIdentity)
        {
            plus.AppendSpaceLine(3, "strSql.Append(\";select @@IDENTITY\");");
            plus.AppendSpaceLine(3, "object obj = " + this.DbHelperName + ".GetSingle(strSql.ToString());");
            plus.AppendSpaceLine(3, "if (obj == null)");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "return 1;");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(3, "else");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "return Convert.ToInt32(obj);");
            plus.AppendSpaceLine(3, "}");
        }
        else
        {
            plus.AppendSpaceLine(3, "" + this.DbHelperName + ".ExecuteSql(strSql.ToString());");
        }
        plus.AppendSpace(2, "}");
        return plus.ToString();
    }

    public string CreatDelete()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine("");
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 删除一条数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public void Delete(" + CodeCommon.GetInParameter(this.Keys) + ")");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        if (this.dbobj.DbType != "OleDb")
        {
            plus.AppendSpaceLine(3, "strSql.Append(\"delete " + this._tablename + " \");");
        }
        else
        {
            plus.AppendSpaceLine(3, "strSql.Append(\"delete from " + this._tablename + " \");");
        }
        plus.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereExpression(this.Keys) + "\" );");
        plus.AppendSpaceLine(3, this.DbHelperName + ".ExecuteSql(strSql.ToString());");
        plus.AppendSpace(2, "}");
        return plus.ToString();
    }

    public string CreatExists()
    {
        StringPlus plus = new StringPlus();
        if (this._keys.Count > 0)
        {
            plus.AppendLine("");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 是否存在该记录");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public bool Exists(" + CodeCommon.GetInParameter(this.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            plus.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + this._tablename + "\");");
            plus.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereExpression(this.Keys) + "\");");
            plus.AppendSpaceLine(3, "return " + this.DbHelperName + ".Exists(strSql.ToString());");
            plus.AppendSpace(2, "}");
        }
        return plus.ToString();
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
        plus.AppendSpaceLine(3, "return " + this.DbHelperName + ".Query(strSql.ToString());");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatGetListByPageProc()
    {
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(2, "/*");
        plus.AppendSpaceLine(2, "*/");
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
                        plus.AppendSpaceLine(2, "return " + this.DbHelperName + ".GetMaxID(\"" + columnName + "\", \"" + this._tablename + "\"); ");
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
        plus.AppendLine();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 得到一个对象实体");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public " + this.ModelSpace + " GetModel(" + CodeCommon.GetInParameter(this.Keys) + ")");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpace(3, "strSql.Append(\"select ");
        if ((this.dbobj.DbType == "SQL2005") || (this.dbobj.DbType == "SQL2000"))
        {
            plus.Append(" top 1 ");
        }
        plus.AppendLine(" \");");
        plus.AppendSpaceLine(3, "strSql.Append(\" " + this.Fieldstrlist + " \");");
        plus.AppendSpaceLine(3, "strSql.Append(\" from " + this._tablename + " \");");
        plus.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereExpression(this.Keys) + "\" );");
        plus.AppendSpaceLine(3, this.ModelSpace + " model=new " + this.ModelSpace + "();");
        plus.AppendSpaceLine(3, "DataSet ds=" + this.DbHelperName + ".Query(strSql.ToString());");
        plus.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
        plus.AppendSpaceLine(3, "{");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            switch (CodeCommon.DbTypeToCS(info.TypeName))
            {
                case "int":
                {
                    plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(4, "{");
                    plus.AppendSpaceLine(5, "model." + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                    plus.AppendSpaceLine(4, "}");
                    continue;
                }
                case "decimal":
                {
                    plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(4, "{");
                    plus.AppendSpaceLine(5, "model." + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                    plus.AppendSpaceLine(4, "}");
                    continue;
                }
                case "float":
                {
                    plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(4, "{");
                    plus.AppendSpaceLine(5, "model." + columnName + "=float.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                    plus.AppendSpaceLine(4, "}");
                    continue;
                }
                case "DateTime":
                {
                    plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(4, "{");
                    plus.AppendSpaceLine(5, "model." + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                    plus.AppendSpaceLine(4, "}");
                    continue;
                }
                case "string":
                {
                    plus.AppendSpaceLine(4, "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                    continue;
                }
                case "bool":
                {
                    plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(4, "{");
                    plus.AppendSpaceLine(5, "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "model." + columnName + "=true;");
                    plus.AppendSpaceLine(5, "}");
                    plus.AppendSpaceLine(5, "else");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "model." + columnName + "=false;");
                    plus.AppendSpaceLine(5, "}");
                    plus.AppendSpaceLine(4, "}");
                    continue;
                }
                case "byte[]":
                {
                    plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(4, "{");
                    plus.AppendSpaceLine(5, "model." + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"];");
                    plus.AppendSpaceLine(4, "}");
                    continue;
                }
                case "Guid":
                {
                    plus.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(4, "{");
                    plus.AppendSpaceLine(5, "model." + columnName + "=new Guid(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                    plus.AppendSpaceLine(4, "}");
                    continue;
                }
            }
            plus.AppendSpaceLine(4, "//model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
        }
        plus.AppendSpaceLine(4, "return model;");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpaceLine(3, "else");
        plus.AppendSpaceLine(3, "{");
        plus.AppendSpaceLine(4, "return null;");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpace(2, "}");
        return plus.ToString();
    }

    public string CreatUpdate()
    {
        if (this.ModelSpace == "")
        {
            this.ModelSpace = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        plus.AppendLine("");
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 更新一条数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public void Update(" + this.ModelSpace + " model)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpaceLine(3, "strSql.Append(\"update " + this._tablename + " set \");");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string length = info.Length;
            bool isIdentity = info.IsIdentity;
            bool isPK = info.IsPK;
            if ((!info.IsIdentity && !info.IsPK) && !this.Keys.Contains(info))
            {
                if (CodeCommon.IsAddMark(typeName.Trim()))
                {
                    plus.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "='\"+model." + columnName + "+\"',\");");
                }
                else
                {
                    plus.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=\"+model." + columnName + "+\",\");");
                }
            }
        }
        plus.Remove(plus.Value.Length - 6, 1);
        plus.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetModelWhereExpression(this.Keys) + "\");");
        plus.AppendSpaceLine(3, "" + this.DbHelperName + ".ExecuteSql(strSql.ToString());");
        plus.AppendSpace(2, "}");
        return plus.ToString();
    }

    public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine("using System;");
        plus.AppendLine("using System.Data;");
        plus.AppendLine("using System.Text;");
        string dbType = this.dbobj.DbType;
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
        if (this.IDALpath != "")
        {
            plus.AppendLine("using " + this.IDALpath + ";");
        }
        //plus.AppendLine("using Maticsoft.DBUtility;//请先添加引用");
       // plus.AppendLine("namespace " + this.DALpath);
        plus.AppendLine("namespace  Service " );
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
        }
        plus.AppendSpaceLine(2, "#endregion  成员方法");
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
            switch (this.dbobj.DbType)
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
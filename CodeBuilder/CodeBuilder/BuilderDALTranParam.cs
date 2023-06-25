using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextronics.Applications.Library.Utility;
using Flextronics.Applications.Library.Schema;

namespace Flextronics.Applications.Library.CodeBuilder
{
    /// <summary>
    /// 父子表生成
    /// </summary>
    public class BuilderDALTranParam : IBuilderDALTran
{
    // Fields
    private string _dalpath;
    private string _dbhelperName;
    private string _dbname;
    private List<ColumnInfo> _fieldlistparent;
    private List<ColumnInfo> _fieldlistson;
    private string _folder;
    private string _iclass;
    private string _idalpath;
    protected string _key;
    private List<ColumnInfo> _keysparent;
    private List<ColumnInfo> _keysson;
    protected string _keyType;
    private string _modelnameparent;
    private string _modelnameson;
    private string _modelpath;
    private string _modelspaceparent;
    private string _modelspaceson;
    private string _namespace;
    private string _procprefix;
    private string _tablenameparent;
    private string _tablenameson;
    private IDbObject dbobj;

    // Methods
    public BuilderDALTranParam()
    {
        this._key = "ID";
        this._keyType = "int";
    }

    public BuilderDALTranParam(IDbObject idbobj)
    {
        this._key = "ID";
        this._keyType = "int";
        this.dbobj = idbobj;
    }

    public BuilderDALTranParam(IDbObject idbobj, string dbname, string tablename, string modelname, List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string namepace, string folder, string dbherlpername, string modelpath, string modelspace, string dalpath, string idalpath, string iclass)
    {
        this._key = "ID";
        this._keyType = "int";
        this.dbobj = idbobj;
        this._dbname = dbname;
        this._tablenameparent = tablename;
        this._modelnameparent = modelname;
        this._namespace = namepace;
        this._folder = folder;
        this._dbhelperName = dbherlpername;
        this._modelpath = modelpath;
        this._modelspaceparent = modelspace;
        this._dalpath = dalpath;
        this._idalpath = idalpath;
        this._iclass = iclass;
        this.FieldlistParent = fieldlist;
        this.KeysParent = keys;
        foreach (ColumnInfo info in this._keysparent)
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
        if (this.ModelSpaceParent == "")
        {
            this.ModelSpaceParent = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        StringPlus plus3 = new StringPlus();
        StringPlus plus4 = new StringPlus();
        StringPlus plus5 = new StringPlus();
        plus.AppendLine();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 增加一条数据,及其子表数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        string str = "void";
        if (((this.dbobj.DbType == "SQL2000") || (this.dbobj.DbType == "SQL2005")) && this.IsHasIdentity)
        {
            str = "int";
        }
        string text = CodeCommon.Space(2) + "public " + str + " Add(" + this.ModelSpaceParent + " model)";
        plus.AppendLine(text);
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpaceLine(3, "strSql.Append(\"insert into " + this._tablenameparent + "(\");");
        plus2.AppendSpace(3, "strSql.Append(\"");
        int num = 0;
        int num2 = 0;
        foreach (ColumnInfo info in this.FieldlistParent)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isIdentity = info.IsIdentity;
            string length = info.Length;
            if (!info.IsIdentity)
            {
                plus4.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + columnName + "\", " + this.DbParaDbType + "." + CodeCommon.DbTypeLength(this.dbobj.DbType, typeName, length) + "),");
                plus2.Append(columnName + ",");
                plus3.Append(this.preParameter + columnName + ",");
                plus5.AppendSpaceLine(3, string.Concat(new object[] { "parameters[", num, "].Value = model.", columnName, ";" }));
                num++;
            }
        }
        if (((this.dbobj.DbType == "SQL2000") || (this.dbobj.DbType == "SQL2005")) && this.IsHasIdentity)
        {
            num2 = num;
            plus4.AppendSpaceLine(5, "new SqlParameter(\"@ReturnValue\",SqlDbType.Int),");
            plus5.AppendSpaceLine(3, "parameters[" + num2.ToString() + "].Direction = ParameterDirection.Output;");
        }
        plus2.DelLastComma();
        plus3.DelLastComma();
        plus4.DelLastComma();
        plus2.AppendLine(")\");");
        plus.Append(plus2.ToString());
        plus.AppendSpaceLine(3, "strSql.Append(\" values (\");");
        plus.AppendSpaceLine(3, "strSql.Append(\"" + plus3.ToString() + ")\");");
        if (((this.dbobj.DbType == "SQL2000") || (this.dbobj.DbType == "SQL2005")) && this.IsHasIdentity)
        {
            plus.AppendSpaceLine(3, "strSql.Append(\";set @ReturnValue= @@IDENTITY\");");
        }
        plus.AppendSpaceLine(3, "" + this.DbParaHead + "Parameter[] parameters = {");
        plus.Append(plus4.Value);
        plus.AppendLine("};");
        plus.AppendLine(plus5.Value);
        plus.AppendSpaceLine(3, "List<CommandInfo> sqllist = new List<CommandInfo>();");
        plus.AppendSpaceLine(3, "CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);");
        plus.AppendSpaceLine(3, "sqllist.Add(cmd);");
        plus.AppendSpaceLine(3, "StringBuilder strSql2;");
        plus.AppendSpaceLine(3, "foreach (" + this.ModelSpaceSon + " models in model." + this.ModelNameSon + "s)");
        plus.AppendSpaceLine(3, "{");
        StringPlus plus6 = new StringPlus();
        StringPlus plus7 = new StringPlus();
        StringPlus plus8 = new StringPlus();
        StringPlus plus9 = new StringPlus();
        plus.AppendSpaceLine(4, "strSql2=new StringBuilder();");
        plus.AppendSpaceLine(4, "strSql2.Append(\"insert into " + this._tablenameson + "(\");");
        plus6.AppendSpace(4, "strSql2.Append(\"");
        int num3 = 0;
        foreach (ColumnInfo info2 in this.FieldlistSon)
        {
            string str6 = info2.ColumnName;
            string datatype = info2.TypeName;
            bool flag2 = info2.IsIdentity;
            string str8 = info2.Length;
            if (!info2.IsIdentity)
            {
                plus8.AppendSpaceLine(6, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + str6 + "\", " + this.DbParaDbType + "." + CodeCommon.DbTypeLength(this.dbobj.DbType, datatype, str8) + "),");
                plus6.Append(str6 + ",");
                plus7.Append(this.preParameter + str6 + ",");
                plus9.AppendSpaceLine(4, string.Concat(new object[] { "parameters2[", num3, "].Value = models.", str6, ";" }));
                num3++;
            }
        }
        plus6.DelLastComma();
        plus7.DelLastComma();
        plus8.DelLastComma();
        plus6.AppendLine(")\");");
        plus.Append(plus6.ToString());
        plus.AppendSpaceLine(4, "strSql2.Append(\" values (\");");
        plus.AppendSpaceLine(4, "strSql2.Append(\"" + plus7.ToString() + ")\");");
        plus.AppendSpaceLine(4, "" + this.DbParaHead + "Parameter[] parameters2 = {");
        plus.Append(plus8.Value);
        plus.AppendLine("};");
        plus.AppendLine(plus9.Value);
        plus.AppendSpaceLine(4, "cmd = new CommandInfo(strSql2.ToString(), parameters2);");
        plus.AppendSpaceLine(4, "sqllist.Add(cmd);");
        plus.AppendSpaceLine(3, "}");
        if (((this.dbobj.DbType == "SQL2000") || (this.dbobj.DbType == "SQL2005")) && this.IsHasIdentity)
        {
            plus.AppendSpaceLine(3, this.DbHelperName + ".ExecuteSqlTranWithIndentity(sqllist);");
            plus.AppendSpaceLine(3, string.Concat(new object[] { "return (", this._keyType, ")parameters[", num2, "].Value;" }));
        }
        else
        {
            plus.AppendSpaceLine(3, "" + this.DbHelperName + ".ExecuteSqlTran(sqllist);");
        }
        plus.AppendSpace(2, "}");
        return plus.ToString();
    }

    public string CreatDelete()
    {
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 删除一条数据，及子表所有相关数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public void Delete(" + CodeCommon.GetInParameter(this.KeysParent) + ")");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "List<CommandInfo> sqllist = new List<CommandInfo>();");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        if (this.dbobj.DbType != "OleDb")
        {
            plus.AppendSpaceLine(3, "strSql.Append(\"delete " + this._tablenameparent + " \");");
        }
        else
        {
            plus.AppendSpaceLine(3, "strSql.Append(\"delete from " + this._tablenameparent + " \");");
        }
        plus.AppendSpaceLine(3, "strSql.Append(\" where " + this.GetWhereExpression(this.KeysParent) + "\");");
        plus.AppendLine(this.GetPreParameter(this.KeysParent));
        plus.AppendSpaceLine(3, "CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);");
        plus.AppendSpaceLine(3, "sqllist.Add(cmd);");
        plus.AppendSpaceLine(3, "StringBuilder strSql2=new StringBuilder();");
        if (this.dbobj.DbType != "OleDb")
        {
            plus.AppendSpaceLine(3, "strSql2.Append(\"delete " + this._tablenameson + " \");");
        }
        else
        {
            plus.AppendSpaceLine(3, "strSql2.Append(\"delete from " + this._tablenameson + " \");");
        }
        plus.AppendSpaceLine(3, "strSql2.Append(\" where " + this.GetWhereExpression(this.KeysSon) + "\");");
        plus.AppendLine(this.GetPreParameter(this.KeysSon, "2"));
        plus.AppendSpaceLine(3, "cmd = new CommandInfo(strSql2.ToString(), parameters2);");
        plus.AppendSpaceLine(3, "sqllist.Add(cmd);");
        plus.AppendSpaceLine(3, "" + this.DbHelperName + ".ExecuteSqlTran(sqllist);");
        plus.AppendSpaceLine(2, "}");
        return plus.Value;
    }

    public string CreatExists()
    {
        StringPlus plus = new StringPlus();
        if (this._keysparent.Count > 0)
        {
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 是否存在该记录");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public bool Exists(" + CodeCommon.GetInParameter(this.KeysParent) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            plus.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + this._tablenameparent + "\");");
            plus.AppendSpaceLine(3, "strSql.Append(\" where " + this.GetWhereExpression(this.KeysParent) + "\");");
            plus.AppendLine(this.GetPreParameter(this.KeysParent));
            plus.AppendSpaceLine(3, "return " + this.DbHelperName + ".Exists(strSql.ToString(),parameters);");
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
        plus.AppendSpaceLine(3, "strSql.Append(\" FROM " + this.TableNameParent + " \");");
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
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 分页获取数据列表");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "" + this.DbParaHead + "Parameter[] parameters = {");
        plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "tblName\", " + this.DbParaDbType + ".VarChar, 255),");
        plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "fldName\", " + this.DbParaDbType + ".VarChar, 255),");
        plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "PageSize\", " + this.DbParaDbType + "." + CodeCommon.CSToProcType(this.dbobj.DbType, "int") + "),");
        plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "PageIndex\", " + this.DbParaDbType + "." + CodeCommon.CSToProcType(this.dbobj.DbType, "int") + "),");
        plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "IsReCount\", " + this.DbParaDbType + "." + CodeCommon.CSToProcType(this.dbobj.DbType, "bit") + "),");
        plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "OrderType\", " + this.DbParaDbType + "." + CodeCommon.CSToProcType(this.dbobj.DbType, "bit") + "),");
        plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "strWhere\", " + this.DbParaDbType + ".VarChar,1000),");
        plus.AppendSpaceLine(5, "};");
        plus.AppendSpaceLine(3, "parameters[0].Value = \"" + this.TableNameParent + "\";");
        plus.AppendSpaceLine(3, "parameters[1].Value = \"" + this._keysparent + "\";");
        plus.AppendSpaceLine(3, "parameters[2].Value = PageSize;");
        plus.AppendSpaceLine(3, "parameters[3].Value = PageIndex;");
        plus.AppendSpaceLine(3, "parameters[4].Value = 0;");
        plus.AppendSpaceLine(3, "parameters[5].Value = 0;");
        plus.AppendSpaceLine(3, "parameters[6].Value = strWhere;\t");
        plus.AppendSpaceLine(3, "return " + this.DbHelperName + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
        plus.AppendSpaceLine(2, "}*/");
        return plus.Value;
    }

    public string CreatGetMaxID()
    {
        StringPlus plus = new StringPlus();
        if (this._keysparent.Count > 0)
        {
            string columnName = "";
            foreach (ColumnInfo info in this._keysparent)
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
                        plus.AppendSpaceLine(2, "return " + this.DbHelperName + ".GetMaxID(\"" + columnName + "\", \"" + this._tablenameparent + "\"); ");
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
        if (this.ModelSpaceParent == "")
        {
            this.ModelSpaceParent = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        plus.AppendLine();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 得到一个对象实体");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public " + this.ModelSpaceParent + " GetModel(" + CodeCommon.GetInParameter(this.KeysParent) + ")");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpaceLine(3, "strSql.Append(\"select " + this.Fieldstrlist + " from " + this._tablenameparent + " \");");
        plus.AppendSpaceLine(3, "strSql.Append(\" where " + this.GetWhereExpression(this.KeysParent) + "\");");
        plus.AppendLine(this.GetPreParameter(this.KeysParent));
        plus.AppendSpaceLine(3, "" + this.ModelSpaceParent + " model=new " + this.ModelSpaceParent + "();");
        plus.AppendSpaceLine(3, "DataSet ds=" + this.DbHelperName + ".Query(strSql.ToString(),parameters);");
        plus.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
        plus.AppendSpaceLine(3, "{");
        plus.AppendSpaceLine(4, "#region  父表信息");
        foreach (ColumnInfo info in this.FieldlistParent)
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
        plus.AppendSpaceLine(4, "#endregion  父表信息end");
        plus.AppendLine();
        plus.AppendSpaceLine(4, "#region  子表信息");
        plus.AppendSpaceLine(4, "StringBuilder strSql2=new StringBuilder();");
        plus.AppendSpaceLine(4, "strSql2.Append(\"select " + this.FieldstrlistSon + " from " + this._tablenameson + " \");");
        plus.AppendSpaceLine(4, "strSql2.Append(\" where " + this.GetWhereExpression(this.KeysSon) + "\");");
        plus.AppendLine(this.GetPreParameter(this.KeysParent, "2"));
        plus.AppendSpaceLine(4, "DataSet ds2=" + this.DbHelperName + ".Query(strSql2.ToString(),parameters2);");
        plus.AppendSpaceLine(4, "if(ds2.Tables[0].Rows.Count>0)");
        plus.AppendSpaceLine(4, "{");
        plus.AppendSpaceLine(5, "#region  子表字段信息");
        plus.AppendSpaceLine(5, "int i = ds2.Tables[0].Rows.Count;");
        plus.AppendSpaceLine(5, "List<" + this.ModelSpaceSon + "> models = new List<" + this.ModelSpaceSon + ">();");
        plus.AppendSpaceLine(5, this.ModelSpaceSon + " modelt;");
        plus.AppendSpaceLine(5, "for (int n = 0; n < i; n++)");
        plus.AppendSpaceLine(5, "{");
        plus.AppendSpaceLine(6, "modelt = new " + this.ModelSpaceSon + "();");
        foreach (ColumnInfo info2 in this.FieldlistSon)
        {
            string str3 = info2.ColumnName;
            switch (CodeCommon.DbTypeToCS(info2.TypeName))
            {
                case "int":
                {
                    plus.AppendSpaceLine(5, "if(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "modelt." + str3 + "=int.Parse(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString());");
                    plus.AppendSpaceLine(5, "}");
                    continue;
                }
                case "decimal":
                {
                    plus.AppendSpaceLine(5, "if(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "modelt." + str3 + "=decimal.Parse(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString());");
                    plus.AppendSpaceLine(5, "}");
                    continue;
                }
                case "DateTime":
                {
                    plus.AppendSpaceLine(5, "if(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "modelt." + str3 + "=DateTime.Parse(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString());");
                    plus.AppendSpaceLine(5, "}");
                    continue;
                }
                case "string":
                {
                    plus.AppendSpaceLine(5, "modelt." + str3 + "=ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString();");
                    continue;
                }
                case "bool":
                {
                    plus.AppendSpaceLine(5, "if(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "if((ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString()==\"1\")||(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString().ToLower()==\"true\"))");
                    plus.AppendSpaceLine(6, "{");
                    plus.AppendSpaceLine(7, "modelt." + str3 + "=true;");
                    plus.AppendSpaceLine(6, "}");
                    plus.AppendSpaceLine(6, "else");
                    plus.AppendSpaceLine(6, "{");
                    plus.AppendSpaceLine(7, "modelt." + str3 + "=false;");
                    plus.AppendSpaceLine(6, "}");
                    plus.AppendSpaceLine(5, "}");
                    continue;
                }
                case "byte[]":
                {
                    plus.AppendSpaceLine(5, "if(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "modelt." + str3 + "=(byte[])ds2.Tables[0].Rows[0][\"" + str3 + "\"];");
                    plus.AppendSpaceLine(5, "}");
                    continue;
                }
                case "Guid":
                {
                    plus.AppendSpaceLine(5, "if(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString()!=\"\")");
                    plus.AppendSpaceLine(5, "{");
                    plus.AppendSpaceLine(6, "modelt." + str3 + "=new Guid(ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString());");
                    plus.AppendSpaceLine(5, "}");
                    continue;
                }
            }
            plus.AppendSpaceLine(5, "modelt." + str3 + "=ds2.Tables[0].Rows[0][\"" + str3 + "\"].ToString();");
        }
        plus.AppendSpaceLine(6, "models.Add(modelt);");
        plus.AppendSpaceLine(5, "}");
        plus.AppendSpaceLine(5, "model." + this.ModelNameSon + "s = models;");
        plus.AppendSpaceLine(5, "#endregion  子表字段信息end");
        plus.AppendSpaceLine(4, "}");
        plus.AppendSpaceLine(4, "#endregion  子表信息end");
        plus.AppendLine();
        plus.AppendSpaceLine(4, "return model;");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpaceLine(3, "else");
        plus.AppendSpaceLine(3, "{");
        plus.AppendSpaceLine(4, "return null;");
        plus.AppendSpaceLine(3, "}");
        plus.AppendSpaceLine(2, "}");
        return plus.ToString();
    }

    public string CreatUpdate()
    {
        if (this.ModelSpaceParent == "")
        {
            this.ModelSpaceParent = "ModelClassName";
        }
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        StringPlus plus3 = new StringPlus();
        plus.AppendSpaceLine(2, "/// <summary>");
        plus.AppendSpaceLine(2, "/// 更新一条数据");
        plus.AppendSpaceLine(2, "/// </summary>");
        plus.AppendSpaceLine(2, "public void Update(" + this.ModelSpaceParent + " model)");
        plus.AppendSpaceLine(2, "{");
        plus.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
        plus.AppendSpaceLine(3, "strSql.Append(\"update " + this._tablenameparent + " set \");");
        int num = 0;
        foreach (ColumnInfo info in this.FieldlistParent)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string length = info.Length;
            bool isIdentity = info.IsIdentity;
            bool isPK = info.IsPK;
            plus2.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + columnName + "\", " + this.DbParaDbType + "." + CodeCommon.DbTypeLength(this.dbobj.DbType, typeName, length) + "),");
            plus3.AppendSpaceLine(3, string.Concat(new object[] { "parameters[", num, "].Value = model.", columnName, ";" }));
            num++;
            if ((!info.IsIdentity && !info.IsPK) && !this.KeysParent.Contains(info))
            {
                plus.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=" + this.preParameter + columnName + ",\");");
            }
        }
        plus.DelLastComma();
        plus.AppendLine("\");");
        plus.AppendSpaceLine(3, "strSql.Append(\" where " + this.GetWhereExpression(this.KeysParent) + "\");");
        plus.AppendSpaceLine(3, "" + this.DbParaHead + "Parameter[] parameters = {");
        plus2.DelLastComma();
        plus.Append(plus2.Value);
        plus.AppendLine("};");
        plus.AppendLine(plus3.Value);
        plus.AppendSpaceLine(3, "" + this.DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
        plus.AppendSpaceLine(2, "}");
        return plus.ToString();
    }

    public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine("using System;");
        plus.AppendLine("using System.Data;");
        plus.AppendLine("using System.Text;");
        plus.AppendLine("using System.Collections.Generic;");
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
        //plus.AppendLine("namespace " + this.DALpath);
        plus.AppendLine("namespace  Service ");
        plus.AppendLine("{");
        plus.AppendSpaceLine(1, "/// <summary>");
        plus.AppendSpaceLine(1, "/// 数据访问类" + this.ModelNameParent + "。");
        plus.AppendSpaceLine(1, "/// </summary>");
        plus.AppendSpace(1, "public class " + this.ModelNameParent);
        if (this.IClass != "")
        {
            plus.Append(":" + this.IClass);
        }
        plus.AppendLine("");
        plus.AppendSpaceLine(1, "{");
        plus.AppendSpaceLine(2, "public " + this.ModelNameParent + "()");
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

    public string GetPreParameter(List<ColumnInfo> keys)
    {
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        plus.AppendSpaceLine(3, "" + this.DbParaHead + "Parameter[] parameters = {");
        int num = 0;
        foreach (ColumnInfo info in keys)
        {
            plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "" + info.ColumnName + "\", " + this.DbParaDbType + "." + CodeCommon.DbTypeLength(this.dbobj.DbType, info.TypeName, "") + "),");
            plus2.AppendSpaceLine(3, "parameters[" + num.ToString() + "].Value = " + info.ColumnName + ";");
            num++;
        }
        plus.DelLastComma();
        plus.AppendLine("};");
        plus.Append(plus2.Value);
        return plus.Value;
    }

    public string GetPreParameter(List<ColumnInfo> keys, string numPara)
    {
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        plus.AppendSpaceLine(3, "" + this.DbParaHead + "Parameter[] parameters" + numPara + " = {");
        int num = 0;
        foreach (ColumnInfo info in keys)
        {
            plus.AppendSpaceLine(5, "new " + this.DbParaHead + "Parameter(\"" + this.preParameter + "" + info.ColumnName + "\", " + this.DbParaDbType + "." + CodeCommon.DbTypeLength(this.dbobj.DbType, info.TypeName, "") + "),");
            plus2.AppendSpaceLine(3, "parameters" + numPara + "[" + num.ToString() + "].Value = " + info.ColumnName + ";");
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

                case "OleDb":
                    return "OleDb";
            }
            return "Sql";
        }
    }

    public List<ColumnInfo> FieldlistParent
    {
        get
        {
            return this._fieldlistparent;
        }
        set
        {
            this._fieldlistparent = value;
        }
    }

    public List<ColumnInfo> FieldlistSon
    {
        get
        {
            return this._fieldlistson;
        }
        set
        {
            this._fieldlistson = value;
        }
    }

    public string Fieldstrlist
    {
        get
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in this.FieldlistParent)
            {
                plus.Append(info.ColumnName + ",");
            }
            plus.DelLastComma();
            return plus.Value;
        }
    }

    public string FieldstrlistSon
    {
        get
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in this.FieldlistSon)
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
            if (this._keysparent.Count > 0)
            {
                foreach (ColumnInfo info in this._keysparent)
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

    public List<ColumnInfo> KeysParent
    {
        get
        {
            return this._keysparent;
        }
        set
        {
            this._keysparent = value;
        }
    }

    public List<ColumnInfo> KeysSon
    {
        get
        {
            return this._keysson;
        }
        set
        {
            this._keysson = value;
        }
    }

    public string ModelNameParent
    {
        get
        {
            return this._modelnameparent;
        }
        set
        {
            this._modelnameparent = value;
        }
    }

    public string ModelNameSon
    {
        get
        {
            return this._modelnameson;
        }
        set
        {
            this._modelnameson = value;
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

    public string ModelSpaceParent
    {
        get
        {
            return this._modelspaceparent;
        }
        set
        {
            this._modelspaceparent = value;
        }
    }

    public string ModelSpaceSon
    {
        get
        {
            return this._modelspaceson;
        }
        set
        {
            this._modelspaceson = value;
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

    public string TableNameParent
    {
        get
        {
            return this._tablenameparent;
        }
        set
        {
            this._tablenameparent = value;
        }
    }

    public string TableNameSon
    {
        get
        {
            return this._tablenameson;
        }
        set
        {
            this._tablenameson = value;
        }
    }
}
 

}
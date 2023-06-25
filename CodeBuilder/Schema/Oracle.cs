using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Flextronics.Applications.Library.Utility;

namespace Flextronics.Applications.Library.Schema
{
    public class OracleDbObject : IDbObject
{
    // Fields
    private string _dbconnectStr;
    private OracleConnection connect;

    // Methods
    public OracleDbObject()
    {
        this.connect = new OracleConnection();
    }

    public OracleDbObject(string DbConnectStr)
    {
        this.connect = new OracleConnection();
        this._dbconnectStr = DbConnectStr;
        this.connect.ConnectionString = DbConnectStr;
    }

    public OracleDbObject(bool SSPI, string server, string User, string Pass)
    {
        this.connect = new OracleConnection();
        this.connect = new OracleConnection();
        this._dbconnectStr = "Data Source=" + server + "; user id=" + User + ";password=" + Pass;
        this.connect.ConnectionString = this._dbconnectStr;
    }

    public bool DeleteTable(string DbName, string TableName)
    {
        try
        {
            string sQLString = "DROP TABLE " + TableName + "";
            this.ExecuteSql(DbName, sQLString);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public OracleDataReader ExecuteReader(string strSQL)
    {
        OracleDataReader reader2;
        try
        {
            this.OpenDB();
            reader2 = new OracleCommand(strSQL, this.connect).ExecuteReader();
        }
        catch (OracleException exception)
        {
            throw new Exception(exception.Message);
        }
        return reader2;
    }

    public int ExecuteSql(string DbName, string SQLString)
    {
        this.OpenDB();
        OracleCommand command = new OracleCommand(SQLString, this.connect);
        command.CommandText = SQLString;
        return command.ExecuteNonQuery();
    }

    public List<ColumnInfo> GetColumnInfoList(string DbName, string TableName)
    {
        List<ColumnInfo> list2;
        try
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ");
            builder.Append("COL.COLUMN_ID as colorder,");
            builder.Append("COL.COLUMN_NAME as ColumnName,");
            builder.Append("COL.DATA_TYPE as TypeName, ");
            builder.Append("DECODE(COL.DATA_TYPE, 'NUMBER',COL.DATA_PRECISION, COL.DATA_LENGTH) as Length,");
            builder.Append("COL.DATA_PRECISION as Preci,");
            builder.Append("COL.DATA_SCALE as Scale,");
            builder.Append("'' as IsIdentity,");
            builder.Append("case when PKCOL.COLUMN_POSITION >0  then '√'else '' end as isPK,");
            builder.Append("case when COL.NULLABLE='Y'  then '√'else '' end as cisNull, ");
            builder.Append("COL.DATA_DEFAULT as defaultVal, ");
            builder.Append("CCOM.COMMENTS as deText,");
            builder.Append("COL.NUM_DISTINCT as NUM_DISTINCT ");
            builder.Append(" FROM USER_TAB_COLUMNS COL,USER_COL_COMMENTS CCOM, ");
            builder.Append(" ( SELECT AA.TABLE_NAME, AA.INDEX_NAME, AA.COLUMN_NAME, AA.COLUMN_POSITION");
            builder.Append(" FROM USER_IND_COLUMNS AA, USER_CONSTRAINTS BB");
            builder.Append(" WHERE BB.CONSTRAINT_TYPE = 'P'");
            builder.Append(" AND AA.TABLE_NAME = BB.TABLE_NAME");
            builder.Append(" AND AA.INDEX_NAME = BB.CONSTRAINT_NAME");
            builder.Append(" AND AA.TABLE_NAME IN ('" + TableName + "')");
            builder.Append(") PKCOL");
            builder.Append(" WHERE COL.TABLE_NAME = CCOM.TABLE_NAME");
            builder.Append(" AND COL.COLUMN_NAME = CCOM.COLUMN_NAME");
            builder.Append(" AND COL.TABLE_NAME ='" + TableName + "'");
            builder.Append(" AND COL.COLUMN_NAME = PKCOL.COLUMN_NAME(+)");
            builder.Append(" AND COL.TABLE_NAME = PKCOL.TABLE_NAME(+)");
            builder.Append(" ORDER BY COL.COLUMN_ID ");
            List<ColumnInfo> list = new List<ColumnInfo>();
            OracleDataReader reader = this.ExecuteReader(builder.ToString());
            while (reader.Read())
            {
                ColumnInfo item = new ColumnInfo();
                item.Colorder = reader.GetValue(0).ToString();
                item.ColumnName = reader.GetValue(1).ToString();
                item.TypeName = reader.GetValue(2).ToString();
                item.Length = reader.GetValue(3).ToString();
                item.Preci = reader.GetValue(4).ToString();
                item.Scale = reader.GetValue(5).ToString();
                item.IsIdentity = reader.GetValue(6).ToString() == "√";
                item.IsPK = reader.GetValue(7).ToString() == "√";
                item.cisNull = reader.GetValue(8).ToString() == "√";
                item.DefaultVal = reader.GetValue(9).ToString();
                item.DeText = reader.GetValue(10).ToString();
                list.Add(item);
            }
            reader.Close();
            list2 = list;
        }
        catch (Exception exception)
        {
            throw new Exception("获取列数据失败" + exception.Message);
        }
        return list2;
    }

    public List<ColumnInfo> GetColumnList(string DbName, string TableName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select ");
        builder.Append("COLUMN_ID as colorder,");
        builder.Append("COLUMN_NAME as ColumnName,");
        builder.Append("DATA_TYPE as TypeName ");
        builder.Append(" from USER_TAB_COLUMNS ");
        builder.Append(" where TABLE_NAME='" + TableName + "'");
        builder.Append(" order by COLUMN_ID");
        List<ColumnInfo> list = new List<ColumnInfo>();
        OracleDataReader reader = this.ExecuteReader(builder.ToString());
        while (reader.Read())
        {
            ColumnInfo item = new ColumnInfo();
            item.Colorder = reader.GetValue(0).ToString();
            item.ColumnName = reader.GetString(1);
            item.TypeName = reader.GetString(2);
            item.Length = "";
            item.Preci = "";
            item.Scale = "";
            item.IsPK = false;
            item.cisNull = false;
            item.DefaultVal = "";
            item.IsIdentity = false;
            item.DeText = "";
            list.Add(item);
        }
        reader.Close();
        return list;
    }

    public List<string> GetDBList()
    {
        return null;
    }

    public DataTable GetKeyName(string DbName, string TableName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select ");
        builder.Append("COL.COLUMN_ID as colorder,");
        builder.Append("COL.COLUMN_NAME as ColumnName,");
        builder.Append("COL.DATA_TYPE as TypeName, ");
        builder.Append("COL.DATA_LENGTH as Length,");
        builder.Append("COL.DATA_PRECISION as Preci,");
        builder.Append("COL.DATA_SCALE as Scale,");
        builder.Append("'' as IsIdentity,");
        builder.Append("case when PKCOL.COLUMN_POSITION >0  then '√'else '' end as isPK,");
        builder.Append("case when COL.NULLABLE='Y'  then '√'else '' end as cisNull, ");
        builder.Append("COL.DATA_DEFAULT as defaultVal, ");
        builder.Append("CCOM.COMMENTS as deText,");
        builder.Append("COL.NUM_DISTINCT as NUM_DISTINCT ");
        builder.Append(" FROM USER_TAB_COLUMNS COL,USER_COL_COMMENTS CCOM, ");
        builder.Append(" ( SELECT AA.TABLE_NAME, AA.INDEX_NAME, AA.COLUMN_NAME, AA.COLUMN_POSITION");
        builder.Append(" FROM USER_IND_COLUMNS AA, USER_CONSTRAINTS BB");
        builder.Append(" WHERE BB.CONSTRAINT_TYPE = 'P'");
        builder.Append(" AND AA.TABLE_NAME = BB.TABLE_NAME");
        builder.Append(" AND AA.INDEX_NAME = BB.CONSTRAINT_NAME");
        builder.Append(" AND AA.TABLE_NAME IN ('" + TableName + "')");
        builder.Append(") PKCOL");
        builder.Append(" WHERE COL.TABLE_NAME = CCOM.TABLE_NAME");
        builder.Append(" AND PKCOL.COLUMN_POSITION >0");
        builder.Append(" AND COL.COLUMN_NAME = CCOM.COLUMN_NAME");
        builder.Append(" AND COL.TABLE_NAME ='" + TableName + "'");
        builder.Append(" AND COL.COLUMN_NAME = PKCOL.COLUMN_NAME(+)");
        builder.Append(" AND COL.TABLE_NAME = PKCOL.TABLE_NAME(+)");
        builder.Append(" ORDER BY COL.COLUMN_ID ");
        return this.Query("", builder.ToString()).Tables[0];
    }

    public string GetObjectInfo(string DbName, string objName)
    {
        return null;
    }

    public List<TableInfo> GetProcInfo(string DbName)
    {
        return null;
    }

    public DataTable GetProcs(string DbName)
    {
        return null;
    }

    public object GetSingle(string DbName, string SQLString)
    {
        try
        {
            this.OpenDB();
            object objA = new OracleCommand(SQLString, this.connect).ExecuteScalar();
            if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
            {
                return null;
            }
            return objA;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetTabData(string DbName, string TableName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select * from " + TableName + "");
        return this.Query("", builder.ToString()).Tables[0];
    }

    public DataTable GetTabDataBySQL(string DbName, string strSQL)
    {
        return this.Query("", strSQL).Tables[0];
    }

    public List<string> GetTables(string DbName)
    {
        string strSQL = "select TNAME name from tab where TABTYPE='TABLE'";
        List<string> list = new List<string>();
        OracleDataReader reader = this.ExecuteReader(strSQL);
        while (reader.Read())
        {
            list.Add(reader.GetString(0));
        }
        reader.Close();
        return list;
    }

    public List<TableInfo> GetTablesInfo(string DbName)
    {
        string strSQL = "select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab where TABTYPE='TABLE'";
        List<TableInfo> list = new List<TableInfo>();
        OracleDataReader reader = this.ExecuteReader(strSQL);
        while (reader.Read())
        {
            TableInfo item = new TableInfo();
            item.TabName = reader.GetString(0);
            item.TabDate = reader.GetValue(3).ToString();
            item.TabType = reader.GetString(2);
            item.TabUser = reader.GetString(1);
            list.Add(item);
        }
        reader.Close();
        return list;
    }

    public DataTable GetTabViews(string DbName)
    {
        string sQLString = "select TNAME name,TABTYPE type from tab ";
        return this.Query("", sQLString).Tables[0];
    }

    public List<TableInfo> GetTabViewsInfo(string DbName)
    {
        string strSQL = "select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab ";
        List<TableInfo> list = new List<TableInfo>();
        OracleDataReader reader = this.ExecuteReader(strSQL);
        while (reader.Read())
        {
            TableInfo item = new TableInfo();
            item.TabName = reader.GetString(0);
            item.TabDate = reader.GetValue(3).ToString();
            item.TabType = reader.GetString(2);
            item.TabUser = reader.GetString(1);
            list.Add(item);
        }
        reader.Close();
        return list;
    }

    public string GetVersion()
    {
        return "";
    }

    public DataTable GetVIEWs(string DbName)
    {
        string sQLString = "select TNAME name from tab where TABTYPE='VIEW'";
        return this.Query("", sQLString).Tables[0];
    }

    public List<TableInfo> GetVIEWsInfo(string DbName)
    {
        string strSQL = "select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab where TABTYPE='VIEW'";
        List<TableInfo> list = new List<TableInfo>();
        OracleDataReader reader = this.ExecuteReader(strSQL);
        while (reader.Read())
        {
            TableInfo item = new TableInfo();
            item.TabName = reader.GetString(0);
            item.TabDate = reader.GetValue(3).ToString();
            item.TabType = reader.GetString(2);
            item.TabUser = reader.GetString(1);
            list.Add(item);
        }
        reader.Close();
        return list;
    }

    public void OpenDB()
    {
        try
        {
            if (this.connect.ConnectionString == "")
            {
                this.connect.ConnectionString = this._dbconnectStr;
            }
            if (this.connect.ConnectionString != this._dbconnectStr)
            {
                this.connect.Close();
                this.connect.ConnectionString = this._dbconnectStr;
            }
            if (this.connect.State == ConnectionState.Closed)
            {
                this.connect.Open();
            }
        }
        catch
        {
        }
    }

    public DataSet Query(string DbName, string SQLString)
    {
        DataSet dataSet = new DataSet();
        try
        {
            this.OpenDB();
            new OracleDataAdapter(SQLString, this.connect).Fill(dataSet, "ds");
        }
        catch (OracleException exception)
        {
            throw new Exception(exception.Message);
        }
        return dataSet;
    }

    public bool RenameTable(string DbName, string OldName, string NewName)
    {
        try
        {
            string sQLString = "RENAME " + OldName + " TO " + NewName + "";
            this.ExecuteSql(DbName, sQLString);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Properties
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

    public string DbType
    {
        get
        {
            return "Oracle";
        }
    }
}


    public class OracleDbScriptBuilder : IDbScriptBuilder
{
    // Fields
    private string _dbconnectStr;
    private string _dbname;
    private List<ColumnInfo> _fieldlist;
    protected string _key = "ID";
    private List<ColumnInfo> _keys;
    protected string _keyType = "int";
    private string _procprefix;
    private string _projectname;
    private string _tablename;
    private OracleDbObject dbobj = new OracleDbObject();

    // Methods
    public string CreateDBTabScript(string dbname)
    {
        this.dbobj.DbConnectStr = this.DbConnectStr;
        StringPlus plus = new StringPlus();
        List<string> tables = this.dbobj.GetTables(dbname);
        if (tables.Count > 0)
        {
            foreach (string str in tables)
            {
                plus.AppendLine(this.CreateTabScript(dbname, str));
            }
        }
        return plus.Value;
    }

    public string CreateTabScript(string dbname, string tablename)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        DataTable columnInfoDt = CodeCommon.GetColumnInfoDt(this.dbobj.GetColumnInfoList(dbname, tablename));
        StringPlus plus = new StringPlus();
        string str = "";
        new StringPlus();
        Hashtable hashtable = new Hashtable();
        StringPlus plus2 = new StringPlus();
        plus.AppendLine("");
        plus.AppendLine("CREATE TABLE [" + tablename + "] (");
        if (columnInfoDt != null)
        {
            if (this.Fieldlist.Count > 0)
            {
                columnInfoDt.Select("ColumnName in (" + this.Fields + ")", "colorder asc");
            }
            else
            {
                columnInfoDt.Select();
            }
            foreach (DataRow row in columnInfoDt.Rows)
            {
                string key = row["ColumnName"].ToString();
                string str3 = row["TypeName"].ToString();
                string str4 = row["Length"].ToString();
                string str5 = row["Preci"].ToString();
                string str6 = row["Scale"].ToString();
                row["isPK"].ToString();
                string str7 = row["cisNull"].ToString();
                string str8 = row["defaultVal"].ToString();
                plus.Append("[" + key + "] [" + str3 + "] ");
                string str13 = str3.Trim();
                if (str13 != null)
                {
                    if ((!(str13 == "CHAR") && !(str13 == "VARCHAR2")) && (!(str13 == "NCHAR") && !(str13 == "NVARCHAR2")))
                    {
                        if (str13 == "NUMBER")
                        {
                            goto Label_020C;
                        }
                    }
                    else
                    {
                        plus.Append(" (" + str4 + ")");
                    }
                }
                goto Label_0249;
            Label_020C:;
                plus.Append(" (" + str5 + "," + str6 + ")");
            Label_0249:
                if (str7 == "√")
                {
                    plus.Append(" NULL");
                }
                else
                {
                    plus.Append(" NOT NULL");
                }
                if (str8 != "")
                {
                    plus.Append(" DEFAULT " + str8);
                }
                plus.AppendLine(",");
                hashtable.Add(key, str3);
                plus2.Append("[" + key + "],");
                if (str == "")
                {
                    str = key;
                }
            }
        }
        plus.DelLastComma();
        plus2.DelLastComma();
        plus.AppendLine(")");
        plus.AppendLine("");
        DataTable tabData = this.dbobj.GetTabData(dbname, tablename);
        if (tabData != null)
        {
            foreach (DataRow row2 in tabData.Rows)
            {
                StringPlus plus3 = new StringPlus();
                StringPlus plus4 = new StringPlus();
                foreach (string str9 in plus2.Value.Split(new char[] { ',' }))
                {
                    string str10 = str9.Substring(1, str9.Length - 2);
                    string columnType = "";
                    foreach (DictionaryEntry entry in hashtable)
                    {
                        if (entry.Key.ToString() == str10)
                        {
                            columnType = entry.Value.ToString();
                        }
                    }
                    string str12 = "";
                    string str14 = columnType;
                    if (str14 == null)
                    {
                        goto Label_047C;
                    }
                    if (!(str14 == "BLOB"))
                    {
                        if (str14 == "bit")
                        {
                            goto Label_044D;
                        }
                        goto Label_047C;
                    }
                    byte[] bytes = (byte[]) row2[str10];
                    str12 = CodeCommon.ToHexString(bytes);
                    goto Label_0491;
                Label_044D:
                    str12 = (row2[str10].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_0491;
                Label_047C:
                    str12 = row2[str10].ToString().Trim();
                Label_0491:
                    if (str12 != "")
                    {
                        if (CodeCommon.IsAddMark(columnType))
                        {
                            plus4.Append("'" + str12 + "',");
                        }
                        else
                        {
                            plus4.Append(str12 + ",");
                        }
                        plus3.Append("[" + str10 + "],");
                    }
                }
                plus3.DelLastComma();
                plus4.DelLastComma();
                plus.Append("INSERT [" + tablename + "] (");
                plus.Append(plus3.Value);
                plus.Append(") VALUES ( ");
                plus.Append(plus4.Value);
                plus.AppendLine(")");
            }
        }
        return plus.Value;
    }

    public void CreateTabScript(string dbname, string tablename, string filename, ProgressBar progressBar)
    {
        StreamWriter writer = new StreamWriter(filename, true, Encoding.Default);
        this.dbobj.DbConnectStr = this._dbconnectStr;
        List<ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(dbname, tablename);
        StringPlus plus = new StringPlus();
        string str = "";
        new StringPlus();
        Hashtable hashtable = new Hashtable();
        StringPlus plus2 = new StringPlus();
        plus.AppendLine("");
        plus.AppendLine("CREATE TABLE [" + tablename + "] (");
        if ((columnInfoList != null) && (columnInfoList.Count > 0))
        {
            foreach (ColumnInfo info in columnInfoList)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                bool isIdentity = info.IsIdentity;
                string length = info.Length;
                string preci = info.Preci;
                string scale = info.Scale;
                bool isPK = info.IsPK;
                bool cisNull = info.cisNull;
                string defaultVal = info.DefaultVal;
                plus.Append("[" + columnName + "] [" + typeName + "] ");
                string str12 = typeName.Trim();
                if (str12 != null)
                {
                    if ((!(str12 == "CHAR") && !(str12 == "VARCHAR2")) && (!(str12 == "NCHAR") && !(str12 == "NVARCHAR2")))
                    {
                        if (str12 == "NUMBER")
                        {
                            goto Label_0194;
                        }
                    }
                    else
                    {
                        plus.Append(" (" + length + ")");
                    }
                }
                goto Label_01D1;
            Label_0194:;
                plus.Append(" (" + preci + "," + scale + ")");
            Label_01D1:
                if (cisNull)
                {
                    plus.Append(" NULL");
                }
                else
                {
                    plus.Append(" NOT NULL");
                }
                if (defaultVal != "")
                {
                    plus.Append(" DEFAULT " + defaultVal);
                }
                plus.AppendLine(",");
                hashtable.Add(columnName, typeName);
                plus2.Append("[" + columnName + "],");
                if (str == "")
                {
                    str = columnName;
                }
            }
        }
        plus.DelLastComma();
        plus2.DelLastComma();
        plus.AppendLine(")");
        plus.AppendLine("");
        if (str != "")
        {
            plus.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD  CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  NONCLUSTERED ( [" + str + "] )");
        }
        writer.Write(plus.Value);
        DataTable tabData = this.dbobj.GetTabData(dbname, tablename);
        if (tabData != null)
        {
            int num = 0;
            progressBar.Maximum = tabData.Rows.Count;
            foreach (DataRow row in tabData.Rows)
            {
                progressBar.Value = num;
                num++;
                StringPlus plus3 = new StringPlus();
                StringPlus plus4 = new StringPlus();
                StringPlus plus5 = new StringPlus();
                foreach (string str8 in plus2.Value.Split(new char[] { ',' }))
                {
                    string str9 = str8.Substring(1, str8.Length - 2);
                    string columnType = "";
                    foreach (DictionaryEntry entry in hashtable)
                    {
                        if (entry.Key.ToString() == str9)
                        {
                            columnType = entry.Value.ToString();
                        }
                    }
                    string str11 = "";
                    string str13 = columnType;
                    if (str13 == null)
                    {
                        goto Label_0481;
                    }
                    if (!(str13 == "BLOB"))
                    {
                        if (str13 == "bit")
                        {
                            goto Label_0452;
                        }
                        goto Label_0481;
                    }
                    byte[] bytes = (byte[]) row[str9];
                    str11 = CodeCommon.ToHexString(bytes);
                    goto Label_0496;
                Label_0452:
                    str11 = (row[str9].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_0496;
                Label_0481:
                    str11 = row[str9].ToString().Trim();
                Label_0496:
                    if (str11 != "")
                    {
                        if (CodeCommon.IsAddMark(columnType))
                        {
                            plus5.Append("'" + str11 + "',");
                        }
                        else
                        {
                            plus5.Append(str11 + ",");
                        }
                        plus4.Append("[" + str9 + "],");
                    }
                }
                plus4.DelLastComma();
                plus5.DelLastComma();
                plus3.Append("INSERT [" + tablename + "] (");
                plus3.Append(plus4.Value);
                plus3.Append(") VALUES ( ");
                plus3.Append(plus5.Value);
                plus3.AppendLine(")");
                writer.Write(plus3.Value);
            }
        }
        writer.Flush();
        writer.Close();
    }

    public string CreateTabScriptBySQL(string dbname, string strSQL)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        string str = "TableName";
        StringPlus plus = new StringPlus();
        DataTable tabDataBySQL = this.dbobj.GetTabDataBySQL(dbname, strSQL);
        if (tabDataBySQL != null)
        {
            DataColumnCollection columns = tabDataBySQL.Columns;
            foreach (DataRow row in tabDataBySQL.Rows)
            {
                StringPlus plus2 = new StringPlus();
                StringPlus plus3 = new StringPlus();
                foreach (DataColumn column in columns)
                {
                    string columnName = column.ColumnName;
                    string name = column.DataType.Name;
                    bool autoIncrement = column.AutoIncrement;
                    string str4 = "";
                    string str5 = name.ToLower();
                    if (str5 == null)
                    {
                        goto Label_0148;
                    }
                    if ((!(str5 == "binary") && !(str5 == "byte[]")) && !(str5 == "blob"))
                    {
                        if ((str5 == "bit") || (str5 == "boolean"))
                        {
                            goto Label_0119;
                        }
                        goto Label_0148;
                    }
                    byte[] bytes = (byte[]) row[columnName];
                    str4 = CodeCommon.ToHexString(bytes);
                    goto Label_015D;
                Label_0119:
                    str4 = (row[columnName].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_015D;
                Label_0148:
                    str4 = row[columnName].ToString().Trim();
                Label_015D:
                    if (str4 != "")
                    {
                        if (CodeCommon.IsAddMark(name))
                        {
                            plus3.Append("'" + str4 + "',");
                        }
                        else
                        {
                            plus3.Append(str4 + ",");
                        }
                        plus2.Append("" + columnName + ",");
                    }
                }
                plus2.DelLastComma();
                plus3.DelLastComma();
                plus.Append("INSERT " + str + " (");
                plus.Append(plus2.Value);
                plus.Append(") VALUES ( ");
                plus.Append(plus3.Value);
                plus.AppendLine(")");
            }
        }
        return plus.Value;
    }

    public string CreatPROCADD()
    {
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        StringPlus plus3 = new StringPlus();
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：增加一条记录 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_ADD (");
        plus.AppendLine(this.GetInParameter(this.Keys, true));
        plus.AppendLine(")");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isIdentity = info.IsIdentity;
            bool isPK = info.IsPK;
            string length = info.Length;
            string preci = info.Preci;
            string scale = info.Scale;
            plus2.Append(columnName + ",");
            plus3.Append("" + columnName + "_in ,");
        }
        plus2.DelLastComma();
        plus3.DelLastComma();
        plus.AppendLine(" AS ");
        plus.AppendLine("BEGIN");
        plus.AppendSpaceLine(1, "INSERT INTO " + this._tablename + "(");
        plus.AppendSpaceLine(1, plus2.Value);
        plus.AppendSpaceLine(1, ")VALUES(");
        plus.AppendSpaceLine(1, plus3.Value);
        plus.AppendSpaceLine(1, ");");
        plus.AppendLine("COMMIT;");
        plus.AppendLine("END;");
        return plus.Value;
    }

    public string CreatPROCDelete()
    {
        StringPlus plus = new StringPlus();
        new StringPlus();
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：删除一条记录 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_Delete");
        plus.AppendLine(this.GetInParameter(this.Keys, false));
        plus.AppendLine("BEGIN");
        plus.AppendLine(" AS ");
        plus.AppendSpaceLine(1, "DELETE " + this._tablename);
        plus.AppendSpaceLine(1, " WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendLine("COMMIT;");
        plus.AppendLine("END;");
        return plus.Value;
    }

    public string CreatPROCGetList()
    {
        StringPlus plus = new StringPlus();
        new StringPlus();
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：查询记录信息 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_GetList");
        plus.AppendLine(" AS ");
        plus.AppendLine("BEGIN");
        plus.AppendSpaceLine(1, "SELECT ");
        plus.AppendSpaceLine(1, this.Fieldstrlist);
        plus.AppendSpaceLine(1, " FROM " + this._tablename);
        plus.AppendLine("COMMIT;");
        plus.AppendLine("END;");
        return plus.Value;
    }

    public string CreatPROCGetMaxID()
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
                        plus.AppendLine("------------------------------------");
                        plus.AppendLine("--用途：得到主键字段最大值 ");
                        plus.AppendLine("--项目名称：" + this.ProjectName);
                        plus.AppendLine("--说明：");
                        plus.AppendLine("--时间：" + DateTime.Now.ToString());
                        plus.AppendLine("------------------------------------");
                        plus.AppendLine("CREATE OR REPLACE  PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_GetMaxId (");
                        plus.AppendLine(")");
                        plus.AppendLine("IS");
                        plus.AppendLine("TempID Number;");
                        plus.AppendLine("BEGIN");
                        plus.AppendSpaceLine(1, "SELECT max(" + columnName + ") into TempID FROM " + this._tablename);
                        plus.AppendSpaceLine(1, "IF NVL(TempID) then");
                        plus.AppendSpaceLine(2, "RETURN 1;");
                        plus.AppendSpaceLine(1, "ELSE");
                        plus.AppendSpaceLine(2, "RETURN TempID;");
                        plus.AppendSpaceLine(1, "end IF;");
                        plus.AppendLine("END;");
                        break;
                    }
                }
            }
        }
        return plus.ToString();
    }

    public string CreatPROCGetModel()
    {
        StringPlus plus = new StringPlus();
        new StringPlus();
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：得到实体对象的详细信息 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_GetModel");
        plus.AppendLine(this.GetInParameter(this.Keys, false));
        plus.AppendLine(" AS ");
        plus.AppendLine("BEGIN");
        plus.AppendSpaceLine(1, "SELECT ");
        plus.AppendSpaceLine(1, this.Fieldstrlist);
        plus.AppendSpaceLine(1, " FROM " + this._tablename);
        plus.AppendSpaceLine(1, " WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendLine("COMMIT;");
        plus.AppendLine("END;");
        return plus.Value;
    }

    public string CreatPROCIsHas()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：是否已经存在 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_Exists (");
        plus.AppendLine(this.GetInParameter(this.Keys, false));
        plus.AppendLine(")");
        plus.AppendLine("AS");
        plus.AppendLine("TempID Number;");
        plus.AppendLine("BEGIN");
        plus.AppendSpaceLine(1, "SELECT count(1) into TempID  FROM " + this._tablename + " WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendSpaceLine(1, "IF TempID = 0 then");
        plus.AppendSpaceLine(2, "RETURN 0;");
        plus.AppendSpaceLine(1, "ELSE");
        plus.AppendSpaceLine(2, "RETURN 1;");
        plus.AppendSpaceLine(1, "end IF;");
        plus.AppendLine("END;");
        plus.AppendLine();
        return plus.Value;
    }

    public string CreatPROCUpdate()
    {
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：修改一条记录 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_Update");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isIdentity = info.IsIdentity;
            bool isPK = info.IsPK;
            string length = info.Length;
            string preci = info.Preci;
            string scale = info.Scale;
            switch (typeName.ToLower())
            {
                case "decimal":
                case "numeric":
                    plus.AppendLine("" + columnName + "_in " + typeName + "(" + preci + "," + scale + "),");
                    break;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    plus.AppendLine("" + columnName + "_in " + typeName + "(" + length + "),");
                    break;

                default:
                    plus.AppendLine("" + columnName + "_in " + typeName + ",");
                    break;
            }
            if (!this.IsKeys(columnName))
            {
                plus2.Append("" + columnName + " = " + columnName + "_in ,");
            }
        }
        plus.DelLastComma();
        plus2.DelLastComma();
        plus.AppendLine("");
        plus.AppendLine(" AS ");
        plus.AppendLine("BEGIN");
        plus.AppendSpaceLine(1, "UPDATE " + this._tablename + " SET ");
        plus.AppendSpaceLine(1, plus2.Value);
        plus.AppendSpaceLine(1, "WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendLine("");
        plus.AppendLine("COMMIT;");
        plus.AppendLine("END;");
        return plus.Value;
    }

    public string GetInParameter(List<ColumnInfo> fieldlist, bool output)
    {
        StringPlus plus = new StringPlus();
        foreach (ColumnInfo info in fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isIdentity = info.IsIdentity;
            bool isPK = info.IsPK;
            string length = info.Length;
            string preci = info.Preci;
            string scale = info.Scale;
            string str6 = " ";
            if (isIdentity && output)
            {
                str6 = " out ";
            }
            switch (typeName.ToLower())
            {
                case "decimal":
                case "numeric":
                    plus.Append("" + columnName + "_in" + str6 + typeName + "(" + preci + "," + scale + ")");
                    break;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    plus.Append("" + columnName + "_in" + str6 + typeName + "(" + length + ")");
                    break;

                default:
                    plus.Append("" + columnName + "_in" + str6 + typeName);
                    break;
            }
            if (!isIdentity || !output)
            {
                plus.AppendLine(",");
            }
        }
        plus.DelLastComma();
        return plus.Value;
    }

    public string GetPROCCode(string dbname)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        DataTable tabViews = this.dbobj.GetTabViews(dbname);
        StringPlus plus = new StringPlus();
        if (tabViews != null)
        {
            foreach (DataRow row in tabViews.Rows)
            {
                string tablename = row["name"].ToString();
                plus.AppendLine(this.GetPROCCode(dbname, tablename));
            }
        }
        return plus.Value;
    }

    public string GetPROCCode(string dbname, string tablename)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        this.Fieldlist = this.dbobj.GetColumnInfoList(dbname, tablename);
        DataTable keyName = this.dbobj.GetKeyName(dbname, tablename);
        this.DbName = dbname;
        this.TableName = tablename;
        this.Keys = CodeCommon.GetColumnInfos(keyName);
        foreach (ColumnInfo info in this.Keys)
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
        return this.GetPROCCode(true, true, true, true, true, true, true);
    }

    public string GetPROCCode(bool Maxid, bool Ishas, bool Add, bool Update, bool Delete, bool GetModel, bool List)
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine("/******************************************************************");
        plus.AppendLine("* 表名：" + this._tablename);
        plus.AppendLine("* 时间：" + DateTime.Now.ToString());
        plus.AppendLine("* Made by Codematic");
        plus.AppendLine("******************************************************************/");
        plus.AppendLine("");
        if (Maxid)
        {
            plus.AppendLine(this.CreatPROCGetMaxID());
        }
        if (Ishas)
        {
            plus.AppendLine(this.CreatPROCIsHas());
        }
        if (Add)
        {
            plus.AppendLine(this.CreatPROCADD());
        }
        if (Update)
        {
            plus.AppendLine(this.CreatPROCUpdate());
        }
        if (Delete)
        {
            plus.AppendLine(this.CreatPROCDelete());
        }
        if (GetModel)
        {
            plus.AppendLine(this.CreatPROCGetModel());
        }
        if (List)
        {
            plus.AppendLine(this.CreatPROCGetList());
        }
        return plus.Value;
    }

    public string GetSQLDelete(string dbname, string tablename)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        this.DbName = dbname;
        this.TableName = tablename;
        StringPlus plus = new StringPlus();
        plus.AppendLine("delete from " + tablename);
        plus.Append(" where  <搜索条件>");
        return plus.Value;
    }

    public string GetSQLInsert(string dbname, string tablename)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        List<ColumnInfo> columnList = this.dbobj.GetColumnList(dbname, tablename);
        this.DbName = dbname;
        this.TableName = tablename;
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        plus.AppendLine("INSERT INTO " + tablename + " ( ");
        if ((columnList != null) && (columnList.Count > 0))
        {
            foreach (ColumnInfo info in columnList)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                plus.AppendLine("[" + columnName + "] ,");
                if (CodeCommon.IsAddMark(typeName))
                {
                    plus2.Append("'" + columnName + "',");
                }
                else
                {
                    plus2.Append(columnName + ",");
                }
            }
            plus.DelLastComma();
            plus2.DelLastComma();
        }
        plus.Append(") VALUES (" + plus2.Value + ")");
        return plus.Value;
    }

    public string GetSQLSelect(string dbname, string tablename)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        List<ColumnInfo> columnList = this.dbobj.GetColumnList(dbname, tablename);
        this.DbName = dbname;
        this.TableName = tablename;
        StringPlus plus = new StringPlus();
        plus.Append("select ");
        if ((columnList != null) && (columnList.Count > 0))
        {
            foreach (ColumnInfo info in columnList)
            {
                plus.Append("[" + info.ColumnName + "],");
            }
            plus.DelLastComma();
        }
        plus.Append(" from " + tablename);
        return plus.Value;
    }

    public string GetSQLUpdate(string dbname, string tablename)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        List<ColumnInfo> columnList = this.dbobj.GetColumnList(dbname, tablename);
        this.DbName = dbname;
        this.TableName = tablename;
        StringPlus plus = new StringPlus();
        plus.AppendLine("update " + tablename + " set ");
        if ((columnList != null) && (columnList.Count > 0))
        {
            foreach (ColumnInfo info in columnList)
            {
                string columnName = info.ColumnName;
                plus.AppendLine("[" + columnName + "] = <" + columnName + ">,");
            }
            plus.DelLastComma();
        }
        plus.Append(" where <搜索条件>");
        return plus.Value;
    }

    public string GetWhereExpression(List<ColumnInfo> keys)
    {
        StringPlus plus = new StringPlus();
        foreach (ColumnInfo info in keys)
        {
            plus.Append(info.ColumnName + "= " + info.ColumnName + "_in and ");
        }
        plus.DelLastChar("and");
        return plus.Value;
    }

    public bool IsKeys(string columnName)
    {
        bool flag = false;
        foreach (ColumnInfo info in this.Keys)
        {
            if (info.ColumnName.Trim() == columnName.Trim())
            {
                flag = true;
            }
        }
        return flag;
    }

    // Properties
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

    public string ProjectName
    {
        get
        {
            return this._projectname;
        }
        set
        {
            this._projectname = value;
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
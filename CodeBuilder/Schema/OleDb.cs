using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Flextronics.Applications.Library.Utility;

namespace Flextronics.Applications.Library.Schema
{
    public class OleDbObject : IDbObject
{
    // Fields
    private string _dbconnectStr;
    private OleDbConnection connect;

    // Methods
    public OleDbObject()
    {
        this.connect = new OleDbConnection();
    }

    public OleDbObject(string DbConnectStr)
    {
        this.connect = new OleDbConnection();
        this._dbconnectStr = DbConnectStr;
        this.connect.ConnectionString = DbConnectStr;
    }

    public OleDbObject(bool SSPI, string server, string User, string Pass)
    {
        this.connect = new OleDbConnection();
        this.connect = new OleDbConnection();
        this._dbconnectStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + server + ";Persist Security Info=False";
        this.connect.ConnectionString = this._dbconnectStr;
    }

    public bool DeleteTable(string DbName, string TableName)
    {
        try
        {
            string text1 = "DROP TABLE " + TableName + "";
            this.ExecuteSql(DbName, TableName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public OleDbDataReader ExecuteReader(string strSQL)
    {
        OleDbDataReader reader2;
        try
        {
            this.OpenDB();
            reader2 = new OleDbCommand(strSQL, this.connect).ExecuteReader();
        }
        catch (OleDbException exception)
        {
            throw new Exception(exception.Message);
        }
        return reader2;
    }

    public int ExecuteSql(string DbName, string SQLString)
    {
        this.OpenDB();
        OleDbCommand command = new OleDbCommand(SQLString, this.connect);
        command.CommandText = SQLString;
        return command.ExecuteNonQuery();
    }

    public List<ColumnInfo> GetColumnInfoList(string DbName, string TableName)
    {
        this.OpenDB();
        object[] restrictions = new object[4];
        restrictions[2] = TableName;
        DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);
        List<ColumnInfo> list = new List<ColumnInfo>();
        foreach (DataRow row in oleDbSchemaTable.Rows)
        {
            ColumnInfo item = new ColumnInfo();
            item.Colorder = row[6].ToString();
            item.ColumnName = row[3].ToString();
            string str = row[11].ToString();
            string str2 = row[11].ToString();
            if (str2 != null)
            {
                if (!(str2 == "3"))
                {
                    if (str2 == "5")
                    {
                        goto Label_00F8;
                    }
                    if (str2 == "6")
                    {
                        goto Label_0101;
                    }
                    if (str2 == "7")
                    {
                        goto Label_010A;
                    }
                    if (str2 == "11")
                    {
                        goto Label_0113;
                    }
                    if (str2 == "130")
                    {
                        goto Label_011C;
                    }
                }
                else
                {
                    str = "int";
                }
            }
            goto Label_0123;
        Label_00F8:
            str = "float";
            goto Label_0123;
        Label_0101:
            str = "money";
            goto Label_0123;
        Label_010A:
            str = "datetime";
            goto Label_0123;
        Label_0113:
            str = "bool";
            goto Label_0123;
        Label_011C:
            str = "varchar";
        Label_0123:
            item.TypeName = str;
            item.Length = row[13].ToString();
            item.Preci = row[15].ToString();
            item.Scale = row[0x10].ToString();
            if (row[10].ToString().ToLower() == "true")
            {
                item.cisNull = false;
            }
            else
            {
                item.cisNull = true;
            }
            item.DefaultVal = row[8].ToString();
            item.DeText = "";
            item.IsPK = false;
            item.IsIdentity = false;
            list.Add(item);
        }
        return list;
    }

    public DataTable GetColumnInfoListSP(string DbName, string ViewName)
    {
        return null;
    }

    public List<ColumnInfo> GetColumnList(string DbName, string TableName)
    {
        return this.GetColumnInfoList(DbName, TableName);
    }

    public List<string> GetDBList()
    {
        return null;
    }

    public DataTable GetKeyName(string DbName, string TableName)
    {
        try
        {
            this.OpenDB();
            DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Key_Column_Usage, new object[4]);
            return this.Key2Colum(oleDbSchemaTable, TableName);
        }
        catch (Exception exception)
        {
            string message = exception.Message;
            return null;
        }
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
            object objA = new OleDbCommand(SQLString, this.connect).ExecuteScalar();
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
        builder.Append("select * from [" + TableName + "]");
        return this.Query("", builder.ToString()).Tables[0];
    }

    public DataTable GetTabDataBySQL(string DbName, string strSQL)
    {
        return this.Query("", strSQL).Tables[0];
    }

    public List<string> GetTables(string DbName)
    {
        this.OpenDB();
        object[] restrictions = new object[4];
        restrictions[3] = "TABLE";
        DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
        List<string> list = new List<string>();
        foreach (DataRow row in oleDbSchemaTable.Rows)
        {
            list.Add(row[2].ToString());
        }
        return list;
    }

    public List<TableInfo> GetTablesInfo(string DbName)
    {
        this.OpenDB();
        object[] restrictions = new object[4];
        restrictions[3] = "TABLE";
        DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
        List<TableInfo> list = new List<TableInfo>();
        foreach (DataRow row in oleDbSchemaTable.Rows)
        {
            TableInfo item = new TableInfo();
            item.TabName = row[2].ToString();
            item.TabUser = "dbo";
            item.TabType = row[3].ToString();
            item.TabDate = row[6].ToString();
            list.Add(item);
        }
        return list;
    }

    public DataTable GetTabViews(string DbName)
    {
        this.OpenDB();
        DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        return this.Tab2Tab(oleDbSchemaTable);
    }

    public List<TableInfo> GetTabViewsInfo(string DbName)
    {
        this.OpenDB();
        DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        List<TableInfo> list = new List<TableInfo>();
        foreach (DataRow row in oleDbSchemaTable.Rows)
        {
            TableInfo item = new TableInfo();
            item.TabName = row[2].ToString();
            item.TabUser = "dbo";
            item.TabType = row[3].ToString();
            item.TabDate = row[6].ToString();
            list.Add(item);
        }
        return list;
    }

    public string GetVersion()
    {
        return "";
    }

    public DataTable GetVIEWs(string DbName)
    {
        this.OpenDB();
        object[] restrictions = new object[4];
        restrictions[3] = "VIEW";
        DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
        return this.Tab2Tab(oleDbSchemaTable);
    }

    public List<TableInfo> GetVIEWsInfo(string DbName)
    {
        this.OpenDB();
        object[] restrictions = new object[4];
        restrictions[3] = "VIEW";
        DataTable oleDbSchemaTable = this.connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
        List<TableInfo> list = new List<TableInfo>();
        foreach (DataRow row in oleDbSchemaTable.Rows)
        {
            TableInfo item = new TableInfo();
            item.TabName = row[2].ToString();
            item.TabUser = "dbo";
            item.TabType = row[3].ToString();
            item.TabDate = row[6].ToString();
            list.Add(item);
        }
        return list;
    }

    private DataTable Key2Colum(DataTable sTable, string TableName)
    {
        DataTable table = new DataTable();
        table.Columns.Add("colorder");
        table.Columns.Add("ColumnName");
        table.Columns.Add("TypeName");
        table.Columns.Add("Length");
        table.Columns.Add("Preci");
        table.Columns.Add("Scale");
        table.Columns.Add("IsIdentity");
        table.Columns.Add("isPK");
        table.Columns.Add("cisNull");
        table.Columns.Add("defaultVal");
        table.Columns.Add("deText");
        int num = 0;
        foreach (DataRow row in sTable.Rows)
        {
            if ((row[5].ToString() == TableName) && (row[2].ToString() == "PrimaryKey"))
            {
                string str3 = row[6].ToString();
                DataRow row2 = table.NewRow();
                row2["colorder"] = row[9].ToString();
                foreach (DataRow row3 in CodeCommon.GetColumnInfoDt(this.GetColumnList(null, TableName)).Select("ColumnName='" + str3 + "'"))
                {
                    row2["ColumnName"] = str3;
                    row2["TypeName"] = row3["TypeName"];
                    row2["Length"] = row3["Length"];
                    row2["Preci"] = row3["Preci"];
                    row2["Scale"] = row3["Scale"];
                    row2["IsIdentity"] = row3["IsIdentity"];
                    row2["isPK"] = row3["isPK"];
                    row2["cisNull"] = row3["cisNull"];
                    row2["defaultVal"] = row3["defaultVal"];
                    row2["deText"] = row3["deText"];
                }
                table.Rows.Add(row2);
                num++;
            }
        }
        return table;
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
            new OleDbDataAdapter(SQLString, this.connect).Fill(dataSet, "ds");
        }
        catch (OleDbException exception)
        {
            throw new Exception(exception.Message);
        }
        return dataSet;
    }

    public bool RenameTable(string DbName, string OldName, string NewName)
    {
        return false;
    }

    private DataTable Tab2Colum(DataTable sTable)
    {
        DataTable table = new DataTable();
        table.Columns.Add("colorder");
        table.Columns.Add("ColumnName");
        table.Columns.Add("TypeName");
        table.Columns.Add("Length");
        table.Columns.Add("Preci");
        table.Columns.Add("Scale");
        table.Columns.Add("IsIdentity");
        table.Columns.Add("isPK");
        table.Columns.Add("cisNull");
        table.Columns.Add("defaultVal");
        table.Columns.Add("deText");
        int num = 0;
        foreach (DataRow row in sTable.Select("", "ORDINAL_POSITION asc"))
        {
            DataRow row2 = table.NewRow();
            row2["colorder"] = row[6].ToString();
            row2["ColumnName"] = row[3].ToString();
            string str = row[11].ToString();
            string str2 = row[11].ToString();
            if (str2 != null)
            {
                if (!(str2 == "3"))
                {
                    if (str2 == "5")
                    {
                        goto Label_019B;
                    }
                    if (str2 == "6")
                    {
                        goto Label_01A4;
                    }
                    if (str2 == "7")
                    {
                        goto Label_01AD;
                    }
                    if (str2 == "11")
                    {
                        goto Label_01B6;
                    }
                    if (str2 == "130")
                    {
                        goto Label_01BF;
                    }
                }
                else
                {
                    str = "int";
                }
            }
            goto Label_01C6;
        Label_019B:
            str = "float";
            goto Label_01C6;
        Label_01A4:
            str = "money";
            goto Label_01C6;
        Label_01AD:
            str = "datetime";
            goto Label_01C6;
        Label_01B6:
            str = "bool";
            goto Label_01C6;
        Label_01BF:
            str = "varchar";
        Label_01C6:
            row2["TypeName"] = str;
            row2["Length"] = row[13].ToString();
            row2["Preci"] = row[15].ToString();
            row2["Scale"] = row[0x10].ToString();
            row2["IsIdentity"] = "";
            row2["isPK"] = "";
            if (row[10].ToString().ToLower() == "true")
            {
                row2["cisNull"] = "";
            }
            else
            {
                row2["cisNull"] = "√";
            }
            row2["defaultVal"] = row[8].ToString();
            row2["deText"] = "";
            table.Rows.Add(row2);
            num++;
        }
        return table;
    }

    private DataTable Tab2Tab(DataTable sTable)
    {
        DataTable table = new DataTable();
        table.Columns.Add("name");
        table.Columns.Add("cuser");
        table.Columns.Add("type");
        table.Columns.Add("dates");
        foreach (DataRow row in sTable.Rows)
        {
            DataRow row2 = table.NewRow();
            row2["name"] = row[2].ToString();
            row2["cuser"] = "dbo";
            row2["type"] = row[3].ToString();
            row2["dates"] = row[6].ToString();
            table.Rows.Add(row2);
        }
        return table;
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
            return "OleDb";
        }
    }
}


    public class OleDbScriptBuilder : IDbScriptBuilder
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
    private OleDbObject dbobj = new OleDbObject();

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
                            goto Label_01D4;
                        }
                    }
                    else
                    {
                        plus.Append(" (" + str4 + ")");
                    }
                }
                goto Label_0211;
            Label_01D4:;
                plus.Append(" (" + str5 + "," + str6 + ")");
            Label_0211:
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
                        goto Label_0444;
                    }
                    if (!(str14 == "BLOB"))
                    {
                        if (str14 == "bit")
                        {
                            goto Label_0415;
                        }
                        goto Label_0444;
                    }
                    byte[] bytes = (byte[]) row2[str10];
                    str12 = CodeCommon.ToHexString(bytes);
                    goto Label_0459;
                Label_0415:
                    str12 = (row2[str10].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_0459;
                Label_0444:
                    str12 = row2[str10].ToString().Trim();
                Label_0459:
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
                    if ((!(str12 == "char") && !(str12 == "varchar")) && (!(str12 == "nchar") && !(str12 == "nvarchar")))
                    {
                        if (str12 == "float")
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
                        goto Label_048F;
                    }
                    if (!(str13 == "binary"))
                    {
                        if ((str13 == "bit") || (str13 == "bool"))
                        {
                            goto Label_0460;
                        }
                        goto Label_048F;
                    }
                    byte[] bytes = (byte[]) row[str9];
                    str11 = CodeCommon.ToHexString(bytes);
                    goto Label_04A4;
                Label_0460:
                    str11 = (row[str9].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_04A4;
                Label_048F:
                    str11 = row[str9].ToString().Trim();
                Label_04A4:
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
        plus.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[");
        plus.Append("" + this.ProcPrefix + "" + this._tablename + "_ADD");
        plus.AppendLine("]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        plus.AppendLine("drop procedure [dbo].[" + this.ProcPrefix + "" + this._tablename + "_ADD]");
        plus.AppendLine("GO");
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：增加一条记录 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_ADD");
        plus.AppendLine(this.GetInParameter(this.Keys, true));
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isIdentity = info.IsIdentity;
            bool isPK = info.IsPK;
            string length = info.Length;
            string preci = info.Preci;
            string scale = info.Scale;
            if (isIdentity)
            {
                this._key = columnName;
                this._keyType = typeName;
            }
            else
            {
                plus2.Append("[" + columnName + "],");
                plus3.Append("@" + columnName + ",");
            }
        }
        plus2.DelLastComma();
        plus3.DelLastComma();
        plus.AppendLine("");
        plus.AppendLine(" AS ");
        plus.AppendSpaceLine(1, "INSERT INTO " + this._tablename + "(");
        plus.AppendSpaceLine(1, plus2.Value);
        plus.AppendSpaceLine(1, ")VALUES(");
        plus.AppendSpaceLine(1, plus3.Value);
        plus.AppendSpaceLine(1, ")");
        if (this.IsHasIdentity)
        {
            plus.AppendSpaceLine(1, "SET @" + this._key + " = @@IDENTITY");
        }
        plus.AppendLine("");
        plus.AppendLine("GO");
        return plus.Value;
    }

    public string CreatPROCDelete()
    {
        StringPlus plus = new StringPlus();
        plus.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[");
        plus.Append("" + this.ProcPrefix + "" + this._tablename + "_Delete");
        plus.AppendLine("]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        plus.AppendLine("drop procedure [dbo].[" + this.ProcPrefix + "" + this._tablename + "_Delete]");
        plus.AppendLine("GO");
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：删除一条记录 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_Delete");
        plus.AppendLine(this.GetInParameter(this.Keys, false));
        plus.AppendLine(" AS ");
        plus.AppendSpaceLine(1, "DELETE " + this._tablename);
        plus.AppendSpaceLine(1, " WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendLine("");
        plus.AppendLine("GO");
        return plus.Value;
    }

    public string CreatPROCGetList()
    {
        StringPlus plus = new StringPlus();
        plus.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[");
        plus.Append("" + this.ProcPrefix + "" + this._tablename + "_GetList");
        plus.AppendLine("]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        plus.AppendLine("drop procedure [dbo].[" + this.ProcPrefix + "" + this._tablename + "_GetList]");
        plus.AppendLine("GO");
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：查询记录信息 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_GetList");
        plus.AppendLine(" AS ");
        plus.AppendSpaceLine(1, "SELECT ");
        plus.AppendSpaceLine(1, this.Fieldstrlist);
        plus.AppendSpaceLine(1, " FROM " + this._tablename);
        plus.AppendLine("");
        plus.AppendLine("GO");
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
                        plus.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[");
                        plus.Append("" + this.ProcPrefix + "" + this._tablename + "_GetMaxId");
                        plus.AppendLine("]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
                        plus.AppendLine("drop procedure [dbo].[" + this.ProcPrefix + "" + this._tablename + "_GetMaxId]");
                        plus.AppendLine("GO");
                        plus.AppendLine("------------------------------------");
                        plus.AppendLine("--用途：得到主键字段最大值 ");
                        plus.AppendLine("--项目名称：" + this.ProjectName);
                        plus.AppendLine("--说明：");
                        plus.AppendLine("--时间：" + DateTime.Now.ToString());
                        plus.AppendLine("------------------------------------");
                        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_GetMaxId");
                        plus.AppendLine("AS");
                        plus.AppendSpaceLine(1, "DECLARE @TempID int");
                        plus.AppendSpaceLine(1, "SELECT @TempID = max([" + columnName + "])+1 FROM " + this._tablename);
                        plus.AppendSpaceLine(1, "IF @TempID IS NULL");
                        plus.AppendSpaceLine(2, "RETURN 1");
                        plus.AppendSpaceLine(1, "ELSE");
                        plus.AppendSpaceLine(2, "RETURN @TempID");
                        plus.AppendLine("");
                        plus.AppendLine("GO");
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
        plus.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[");
        plus.Append("" + this.ProcPrefix + "" + this._tablename + "_GetModel");
        plus.AppendLine("]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        plus.AppendLine("drop procedure [dbo].[" + this.ProcPrefix + "" + this._tablename + "_GetModel]");
        plus.AppendLine("GO");
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：得到实体对象的详细信息 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_GetModel");
        plus.AppendLine(this.GetInParameter(this.Keys, false));
        plus.AppendLine(" AS ");
        plus.AppendSpaceLine(1, "SELECT ");
        plus.AppendSpaceLine(1, this.Fieldstrlist);
        plus.AppendSpaceLine(1, " FROM " + this._tablename);
        plus.AppendSpaceLine(1, " WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendLine("");
        plus.AppendLine("GO");
        return plus.Value;
    }

    public string CreatPROCIsHas()
    {
        StringPlus plus = new StringPlus();
        plus.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[");
        plus.Append("" + this.ProcPrefix + "" + this._tablename + "_Exists");
        plus.AppendLine("]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        plus.AppendLine("drop procedure [dbo].[" + this.ProcPrefix + "" + this._tablename + "_Exists]");
        plus.AppendLine("GO");
        plus.AppendLine("------------------------------------");
        plus.AppendLine("--用途：是否已经存在 ");
        plus.AppendLine("--项目名称：" + this.ProjectName);
        plus.AppendLine("--说明：");
        plus.AppendLine("--时间：" + DateTime.Now.ToString());
        plus.AppendLine("------------------------------------");
        plus.AppendLine("CREATE PROCEDURE " + this.ProcPrefix + "" + this._tablename + "_Exists");
        plus.AppendLine(this.GetInParameter(this.Keys, false));
        plus.AppendLine("AS");
        plus.AppendSpaceLine(1, "DECLARE @TempID int");
        plus.AppendSpaceLine(1, "SELECT @TempID = count(1) FROM " + this._tablename + " WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendSpaceLine(1, "IF @TempID = 0");
        plus.AppendSpaceLine(2, "RETURN 0");
        plus.AppendSpaceLine(1, "ELSE");
        plus.AppendSpaceLine(2, "RETURN 1");
        plus.AppendLine("");
        plus.AppendLine("GO");
        return plus.Value;
    }

    public string CreatPROCUpdate()
    {
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        plus.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[");
        plus.Append("" + this.ProcPrefix + "" + this._tablename + "_Update");
        plus.AppendLine("]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        plus.AppendLine("drop procedure [dbo].[" + this.ProcPrefix + "" + this._tablename + "_Update]");
        plus.AppendLine("GO");
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
                    plus.AppendLine("@" + columnName + " " + typeName + "(" + preci + "," + scale + "),");
                    break;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    plus.AppendLine("@" + columnName + " " + typeName + "(" + length + "),");
                    break;

                default:
                    plus.AppendLine("@" + columnName + " " + typeName + ",");
                    break;
            }
            if (!isIdentity && !isPK)
            {
                plus2.Append("[" + columnName + "] = @" + columnName + ",");
            }
        }
        plus.DelLastComma();
        plus2.DelLastComma();
        plus.AppendLine();
        plus.AppendLine(" AS ");
        plus.AppendSpaceLine(1, "UPDATE " + this._tablename + " SET ");
        plus.AppendSpaceLine(1, plus2.Value);
        plus.AppendSpaceLine(1, "WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendLine();
        plus.AppendLine("GO");
        return plus.Value;
    }

    public string GetInParameter(List<ColumnInfo> keys, bool output)
    {
        StringPlus plus = new StringPlus();
        foreach (ColumnInfo info in this.Keys)
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
                    plus.AppendLine("@" + columnName + " " + typeName + "(" + preci + "," + scale + ")");
                    break;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    plus.AppendLine("@" + columnName + " " + typeName + "(" + length + ")");
                    break;

                default:
                    plus.AppendLine("@" + columnName + " " + typeName);
                    break;
            }
            if (isIdentity && output)
            {
                plus.AppendLine(" output,");
            }
            else
            {
                plus.AppendLine(",");
            }
        }
        plus.DelLastComma();
        return plus.Value;
    }

    public string GetPROCCode(string dbname)
    {
        this.dbobj.DbConnectStr = this.DbConnectStr;
        StringPlus plus = new StringPlus();
        List<string> tables = this.dbobj.GetTables(dbname);
        if (tables.Count > 0)
        {
            foreach (string str in tables)
            {
                plus.AppendLine(this.GetPROCCode(dbname, str));
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
            plus.Append(info.ColumnName + "=@" + info.ColumnName + " and ");
        }
        plus.DelLastChar("and");
        return plus.Value;
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

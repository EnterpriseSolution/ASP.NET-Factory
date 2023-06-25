using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Flextronics.Applications.Library.Utility;

namespace Flextronics.Applications.Library.Schema
{
    public class SQLServer2005DbObject : IDbObject
{
    // Fields
    private string _dbconnectStr;
    private INIFile cfgfile;
    private string cmcfgfile;
    private SqlConnection connect;
    private bool isdbosp;

    // Methods
    public SQLServer2005DbObject()
    {
        this.cmcfgfile = Application.StartupPath + @"\cmcfg.ini";
        this.connect = new SqlConnection();
        this.IsDboSp();
    }

    public SQLServer2005DbObject(string DbConnectStr)
    {
        this.cmcfgfile = Application.StartupPath + @"\cmcfg.ini";
        this.connect = new SqlConnection();
        this._dbconnectStr = DbConnectStr;
        this.connect.ConnectionString = DbConnectStr;
    }

    public SQLServer2005DbObject(bool SSPI, string Ip, string User, string Pass)
    {
        this.cmcfgfile = Application.StartupPath + @"\cmcfg.ini";
        this.connect = new SqlConnection();
        this.connect = new SqlConnection();
        if (SSPI)
        {
            this._dbconnectStr = "Integrated Security=SSPI;Data Source=" + Ip + ";Initial Catalog=master";
        }
        else if (Pass == "")
        {
            this._dbconnectStr = "user id=" + User + ";initial catalog=master;data source=" + Ip + ";Connect Timeout=30";
        }
        else
        {
            this._dbconnectStr = "user id=" + User + ";password=" + Pass + ";initial catalog=master;data source=" + Ip + ";Connect Timeout=30";
        }
        this.connect.ConnectionString = this._dbconnectStr;
    }

    private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
    {
        SqlCommand command = new SqlCommand(storedProcName, connection);
        command.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter parameter in parameters)
        {
            if (parameter != null)
            {
                if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
                command.Parameters.Add(parameter);
            }
        }
        return command;
    }

    public DataTable CreateColumnTable()
    {
        DataTable table = new DataTable();
        DataColumn column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "colorder";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "ColumnName";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "TypeName";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "Length";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "Preci";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "Scale";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "IsIdentity";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "isPK";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "cisNull";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "defaultVal";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "deText";
        table.Columns.Add(column);
        return table;
    }

    public bool DeleteTable(string DbName, string TableName)
    {
        try
        {
            SqlCommand command = this.OpenDB(DbName);
            command.CommandText = "DROP TABLE [" + TableName + "]";
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public SqlDataReader ExecuteReader(string DbName, string strSQL)
    {
        SqlDataReader reader2;
        try
        {
            this.OpenDB(DbName);
            reader2 = new SqlCommand(strSQL, this.connect).ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (SqlException exception)
        {
            throw exception;
        }
        return reader2;
    }

    public int ExecuteSql(string DbName, string SQLString)
    {
        SqlCommand command = this.OpenDB(DbName);
        command.CommandText = SQLString;
        return command.ExecuteNonQuery();
    }

    public List<ColumnInfo> GetColumnInfoList(string DbName, string TableName)
    {
        if (this.isdbosp)
        {
            return this.GetColumnInfoListSP(DbName, TableName);
        }
        StringBuilder builder = new StringBuilder();
        builder.Append("SELECT ");
        builder.Append("colorder=C.column_id,");
        builder.Append("ColumnName=C.name,");
        builder.Append("TypeName=T.name, ");
        builder.Append("Length=CASE WHEN T.name='nchar' THEN C.max_length/2 WHEN T.name='nvarchar' THEN C.max_length/2 ELSE C.max_length END,");
        builder.Append("Preci=C.precision, ");
        builder.Append("Scale=C.scale, ");
        builder.Append("IsIdentity=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,");
        builder.Append("isPK=ISNULL(IDX.PrimaryKey,N''),");
        builder.Append("Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END, ");
        builder.Append("IndexName=ISNULL(IDX.IndexName,N''), ");
        builder.Append("IndexSort=ISNULL(IDX.Sort,N''), ");
        builder.Append("Create_Date=O.Create_Date, ");
        builder.Append("Modify_Date=O.Modify_date, ");
        builder.Append("cisNull=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, ");
        builder.Append("defaultVal=ISNULL(D.definition,N''), ");
        builder.Append("deText=ISNULL(PFD.[value],N'') ");
        builder.Append("FROM sys.columns C ");
        builder.Append("INNER JOIN sys.objects O ");
        builder.Append("ON C.[object_id]=O.[object_id] ");
        builder.Append("AND (O.type='U' or O.type='V') ");
        builder.Append("AND O.is_ms_shipped=0 ");
        builder.Append("INNER JOIN sys.types T ");
        builder.Append("ON C.user_type_id=T.user_type_id ");
        builder.Append("LEFT JOIN sys.default_constraints D ");
        builder.Append("ON C.[object_id]=D.parent_object_id ");
        builder.Append("AND C.column_id=D.parent_column_id ");
        builder.Append("AND C.default_object_id=D.[object_id] ");
        builder.Append("LEFT JOIN sys.extended_properties PFD ");
        builder.Append("ON PFD.class=1  ");
        builder.Append("AND C.[object_id]=PFD.major_id  ");
        builder.Append("AND C.column_id=PFD.minor_id ");
        builder.Append("LEFT JOIN sys.extended_properties PTB ");
        builder.Append("ON PTB.class=1 ");
        builder.Append("AND PTB.minor_id=0  ");
        builder.Append("AND C.[object_id]=PTB.major_id ");
        builder.Append("LEFT JOIN ");
        builder.Append("( ");
        builder.Append("SELECT  ");
        builder.Append("IDXC.[object_id], ");
        builder.Append("IDXC.column_id, ");
        builder.Append("Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') ");
        builder.Append("WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, ");
        builder.Append("PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, ");
        builder.Append("IndexName=IDX.Name ");
        builder.Append("FROM sys.indexes IDX ");
        builder.Append("INNER JOIN sys.index_columns IDXC ");
        builder.Append("ON IDX.[object_id]=IDXC.[object_id] ");
        builder.Append("AND IDX.index_id=IDXC.index_id ");
        builder.Append("LEFT JOIN sys.key_constraints KC ");
        builder.Append("ON IDX.[object_id]=KC.[parent_object_id] ");
        builder.Append("AND IDX.index_id=KC.unique_index_id ");
        builder.Append("INNER JOIN  ");
        builder.Append("( ");
        builder.Append("SELECT [object_id], Column_id, index_id=MIN(index_id) ");
        builder.Append("FROM sys.index_columns ");
        builder.Append("GROUP BY [object_id], Column_id ");
        builder.Append(") IDXCUQ ");
        builder.Append("ON IDXC.[object_id]=IDXCUQ.[object_id] ");
        builder.Append("AND IDXC.Column_id=IDXCUQ.Column_id ");
        builder.Append("AND IDXC.index_id=IDXCUQ.index_id ");
        builder.Append(") IDX ");
        builder.Append("ON C.[object_id]=IDX.[object_id] ");
        builder.Append("AND C.column_id=IDX.column_id  ");
        builder.Append("WHERE O.name=N'" + TableName + "' ");
        builder.Append("ORDER BY O.name,C.column_id  ");
        List<ColumnInfo> list = new List<ColumnInfo>();
        SqlDataReader reader = this.ExecuteReader(DbName, builder.ToString());
        while (reader.Read())
        {
            ColumnInfo item = new ColumnInfo();
            item.Colorder = reader.GetValue(0).ToString();
            item.ColumnName = reader.GetString(1);
            item.TypeName = reader.GetString(2);
            item.Length = reader.GetValue(3).ToString();
            item.Preci = reader.GetValue(4).ToString();
            item.Scale = reader.GetValue(5).ToString();
            item.IsIdentity = reader.GetString(6) == "√";
            item.IsPK = reader.GetString(7) == "√";
            item.cisNull = reader.GetString(13) == "√";
            item.DefaultVal = reader.GetString(14);
            item.DeText = reader.GetString(15);
            list.Add(item);
        }
        reader.Close();
        return list;
    }

    public List<ColumnInfo> GetColumnInfoListSP(string DbName, string TableName)
    {
        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@table_name", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_owner", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_qualifier", SqlDbType.NVarChar, 0x180), new SqlParameter("@column_name", SqlDbType.VarChar, 100) };
        parameters[0].Value = TableName;
        parameters[1].Value = null;
        parameters[2].Value = null;
        parameters[3].Value = null;
        DataSet set = this.RunProcedure(DbName, "sp_columns", parameters, "ds");
        if (set.Tables.Count <= 0)
        {
            return null;
        }
        DataTable table = set.Tables[0];
        int count = table.Rows.Count;
        DataTable dt = this.CreateColumnTable();
        for (int i = 0; i < count; i++)
        {
            DataRow row = dt.NewRow();
            row["colorder"] = table.Rows[i]["ORDINAL_POSITION"];
            row["ColumnName"] = table.Rows[i]["COLUMN_NAME"];
            string str = table.Rows[i]["TYPE_NAME"].ToString().Trim();
            row["TypeName"] = (str == "int identity") ? "int" : str;
            row["Length"] = table.Rows[i]["LENGTH"];
            row["Preci"] = table.Rows[i]["PRECISION"];
            row["Scale"] = table.Rows[i]["SCALE"];
            row["IsIdentity"] = (str == "int identity") ? "√" : "";
            row["isPK"] = "";
            row["cisNull"] = (table.Rows[i]["NULLABLE"].ToString().Trim() == "1") ? "√" : "";
            row["defaultVal"] = table.Rows[i]["COLUMN_DEF"];
            row["deText"] = table.Rows[i]["REMARKS"];
            dt.Rows.Add(row);
        }
        return CodeCommon.GetColumnInfos(dt);
    }

    public List<ColumnInfo> GetColumnList(string DbName, string TableName)
    {
        try
        {
            if (this.isdbosp)
            {
                return this.GetColumnListSP(DbName, TableName);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("Select ");
            builder.Append("a.colorder as colorder,");
            builder.Append("a.name as ColumnName,");
            builder.Append("b.name as TypeName ");
            builder.Append(" from syscolumns a, systypes b, sysobjects c ");
            builder.Append(" where a.xtype = b.xusertype ");
            builder.Append("and a.id = c.id ");
            builder.Append("and c.name ='" + TableName + "'");
            builder.Append(" order by a.colorder");
            List<ColumnInfo> list = new List<ColumnInfo>();
            SqlDataReader reader = this.ExecuteReader(DbName, builder.ToString());
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
        catch (Exception)
        {
            return null;
        }
    }

    public List<ColumnInfo> GetColumnListSP(string DbName, string TableName)
    {
        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@table_name", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_owner", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_qualifier", SqlDbType.NVarChar, 0x180), new SqlParameter("@column_name", SqlDbType.VarChar, 100) };
        parameters[0].Value = TableName;
        parameters[1].Value = null;
        parameters[2].Value = null;
        parameters[3].Value = null;
        DataSet set = this.RunProcedure(DbName, "sp_columns", parameters, "ds");
        if (set.Tables.Count <= 0)
        {
            return null;
        }
        DataTable table = set.Tables[0];
        int count = table.Rows.Count;
        DataTable dt = this.CreateColumnTable();
        for (int i = 0; i < count; i++)
        {
            DataRow row = dt.NewRow();
            row["colorder"] = table.Rows[i]["ORDINAL_POSITION"];
            row["ColumnName"] = table.Rows[i]["COLUMN_NAME"];
            string str = table.Rows[i]["TYPE_NAME"].ToString().Trim();
            row["TypeName"] = (str == "int identity") ? "int" : str;
            row["Length"] = table.Rows[i]["LENGTH"];
            row["Preci"] = table.Rows[i]["PRECISION"];
            row["Scale"] = table.Rows[i]["SCALE"];
            row["IsIdentity"] = (str == "int identity") ? "√" : "";
            row["isPK"] = "";
            row["cisNull"] = (table.Rows[i]["NULLABLE"].ToString().Trim() == "1") ? "√" : "";
            row["defaultVal"] = table.Rows[i]["COLUMN_DEF"];
            row["deText"] = table.Rows[i]["REMARKS"];
            dt.Rows.Add(row);
        }
        return CodeCommon.GetColumnInfos(dt);
    }

    public List<string> GetDBList()
    {
        List<string> list = new List<string>();
        string strSQL = "select name from sysdatabases order by name";
        SqlDataReader reader = this.ExecuteReader("master", strSQL);
        while (reader.Read())
        {
            list.Add(reader.GetString(0));
        }
        reader.Close();
        return list;
    }

    public DataTable GetKeyName(string DbName, string TableName)
    {
        DataTable table = this.CreateColumnTable();
        foreach (DataRow row in CodeCommon.GetColumnInfoDt(this.GetColumnInfoList(DbName, TableName)).Select(" isPK='√' or IsIdentity='√' "))
        {
            DataRow row2 = table.NewRow();
            row2["colorder"] = row["colorder"];
            row2["ColumnName"] = row["ColumnName"];
            row2["TypeName"] = row["TypeName"];
            row2["Length"] = row["Length"];
            row2["Preci"] = row["Preci"];
            row2["Scale"] = row["Scale"];
            row2["IsIdentity"] = row["IsIdentity"];
            row2["isPK"] = row["isPK"];
            row2["cisNull"] = row["cisNull"];
            row2["defaultVal"] = row["defaultVal"];
            row2["deText"] = row["deText"];
            table.Rows.Add(row2);
        }
        return table;
    }

    public string GetObjectInfo(string DbName, string objName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select b.text ");
        builder.Append("from sysobjects a, syscomments b  ");
        builder.Append("where a.xtype='p' and a.id = b.id  ");
        builder.Append(" and a.name= '" + objName + "'");
        return this.GetSingle(DbName, builder.ToString()).ToString();
    }

    public List<TableInfo> GetProcInfo(string DbName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select sysobjects.[name] name,sysusers.name cuser,");
        builder.Append("sysobjects.xtype type,sysobjects.crdate dates ");
        builder.Append("from sysobjects,sysusers ");
        builder.Append("where sysusers.uid=sysobjects.uid ");
        builder.Append("and sysobjects.xtype='P' ");
        builder.Append("order by sysobjects.[name] ");
        List<TableInfo> list = new List<TableInfo>();
        SqlDataReader reader = this.ExecuteReader(DbName, builder.ToString());
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

    public DataTable GetProcs(string DbName)
    {
        string sQLString = "select [name] from sysobjects where xtype='P'and [name]<>'dtproperties' order by [name]";
        return this.Query(DbName, sQLString).Tables[0];
    }

    public object GetSingle(string DbName, string SQLString)
    {
        try
        {
            SqlCommand command = this.OpenDB(DbName);
            command.CommandText = SQLString;
            object objA = command.ExecuteScalar();
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
        return this.Query(DbName, builder.ToString()).Tables[0];
    }

    public DataTable GetTabDataBySQL(string DbName, string strSQL)
    {
        return this.Query(DbName, strSQL).Tables[0];
    }

    public List<string> GetTables(string DbName)
    {
        if (this.isdbosp)
        {
            return this.GetTablesSP(DbName);
        }
        string strSQL = "select [name] from sysobjects where xtype='U'and [name]<>'dtproperties' order by [name]";
        List<string> list = new List<string>();
        SqlDataReader reader = this.ExecuteReader(DbName, strSQL);
        while (reader.Read())
        {
            list.Add(reader.GetString(0));
        }
        reader.Close();
        return list;
    }

    public List<TableInfo> GetTablesInfo(string DbName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select sysobjects.[name] name,sysusers.name cuser,");
        builder.Append("sysobjects.xtype type,sysobjects.crdate dates ");
        builder.Append("from sysobjects,sysusers ");
        builder.Append("where sysusers.uid=sysobjects.uid ");
        builder.Append("and sysobjects.xtype='U' ");
        builder.Append("and  sysobjects.[name]<>'dtproperties' ");
        builder.Append("order by sysobjects.[name] ");
        List<TableInfo> list = new List<TableInfo>();
        SqlDataReader reader = this.ExecuteReader(DbName, builder.ToString());
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

    public List<string> GetTablesSP(string DbName)
    {
        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@table_name", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_owner", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_qualifier", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_type", SqlDbType.VarChar, 100) };
        parameters[0].Value = null;
        parameters[1].Value = null;
        parameters[2].Value = null;
        parameters[3].Value = "'TABLE'";
        DataSet set = this.RunProcedure(DbName, "sp_tables", parameters, "ds");
        List<string> list = new List<string>();
        if (set.Tables.Count <= 0)
        {
            return null;
        }
        DataTable table = set.Tables[0];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            list.Add(table.Rows[i]["TABLE_NAME"].ToString());
        }
        return list;
    }

    public DataTable GetTabViews(string DbName)
    {
        if (this.isdbosp)
        {
            return this.GetTabViewsSP(DbName);
        }
        StringBuilder builder = new StringBuilder();
        builder.Append("select [name],sysobjects.xtype type from sysobjects ");
        builder.Append("where (xtype='U' or xtype='V' or xtype='P') ");
        builder.Append("and [name]<>'dtproperties' and [name]<>'syssegments' and [name]<>'sysconstraints' ");
        builder.Append("order by xtype,[name]");
        return this.Query(DbName, builder.ToString()).Tables[0];
    }

    public List<TableInfo> GetTabViewsInfo(string DbName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select sysobjects.[name] name,sysusers.name cuser,");
        builder.Append("sysobjects.xtype type,sysobjects.crdate dates ");
        builder.Append("from sysobjects,sysusers ");
        builder.Append("where sysusers.uid=sysobjects.uid ");
        builder.Append("and (sysobjects.xtype='U' or sysobjects.xtype='V' or sysobjects.xtype='P') ");
        builder.Append("and sysobjects.[name]<>'dtproperties' and sysobjects.[name]<>'syssegments' and sysobjects.[name]<>'sysconstraints'  ");
        builder.Append("order by sysobjects.xtype,sysobjects.[name] ");
        List<TableInfo> list = new List<TableInfo>();
        SqlDataReader reader = this.ExecuteReader(DbName, builder.ToString());
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

    public DataTable GetTabViewsSP(string DbName)
    {
        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@table_name", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_owner", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_qualifier", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_type", SqlDbType.VarChar, 100) };
        parameters[0].Value = null;
        parameters[1].Value = null;
        parameters[2].Value = null;
        parameters[3].Value = "'TABLE','VIEW'";
        DataSet set = this.RunProcedure(DbName, "sp_tables", parameters, "ds");
        if (set.Tables.Count > 0)
        {
            DataTable table = set.Tables[0];
            table.Columns["TABLE_QUALIFIER"].ColumnName = "db";
            table.Columns["TABLE_OWNER"].ColumnName = "cuser";
            table.Columns["TABLE_NAME"].ColumnName = "name";
            table.Columns["TABLE_TYPE"].ColumnName = "type";
            table.Columns["REMARKS"].ColumnName = "remarks";
            return table;
        }
        return null;
    }

    public string GetVersion()
    {
        try
        {
            string sQLString = "execute master..sp_msgetversion ";
            return this.Query("master", sQLString).Tables[0].Rows[0][0].ToString();
        }
        catch
        {
            return "";
        }
    }

    public DataTable GetVIEWs(string DbName)
    {
        if (this.isdbosp)
        {
            return this.GetVIEWsSP(DbName);
        }
        string sQLString = "select [name] from sysobjects where xtype='V' and [name]<>'syssegments' and [name]<>'sysconstraints' order by [name]";
        return this.Query(DbName, sQLString).Tables[0];
    }

    public List<TableInfo> GetVIEWsInfo(string DbName)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("select sysobjects.[name] name,sysusers.name cuser,");
        builder.Append("sysobjects.xtype type,sysobjects.crdate dates ");
        builder.Append("from sysobjects,sysusers ");
        builder.Append("where sysusers.uid=sysobjects.uid ");
        builder.Append("and sysobjects.xtype='V' ");
        builder.Append("and sysobjects.[name]<>'syssegments' and sysobjects.[name]<>'sysconstraints'  ");
        builder.Append("order by sysobjects.[name] ");
        List<TableInfo> list = new List<TableInfo>();
        SqlDataReader reader = this.ExecuteReader(DbName, builder.ToString());
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

    public DataTable GetVIEWsSP(string DbName)
    {
        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@table_name", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_owner", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_qualifier", SqlDbType.NVarChar, 0x180), new SqlParameter("@table_type", SqlDbType.VarChar, 100) };
        parameters[0].Value = null;
        parameters[1].Value = null;
        parameters[2].Value = null;
        parameters[3].Value = "'VIEW'";
        DataSet set = this.RunProcedure(DbName, "sp_tables", parameters, "ds");
        if (set.Tables.Count > 0)
        {
            DataTable table = set.Tables[0];
            table.Columns["TABLE_QUALIFIER"].ColumnName = "db";
            table.Columns["TABLE_OWNER"].ColumnName = "cuser";
            table.Columns["TABLE_NAME"].ColumnName = "name";
            table.Columns["TABLE_TYPE"].ColumnName = "type";
            table.Columns["REMARKS"].ColumnName = "remarks";
            return table;
        }
        return null;
    }

    private bool IsDboSp()
    {
        if (File.Exists(this.cmcfgfile))
        {
            this.cfgfile = new INIFile(this.cmcfgfile);
            if (this.cfgfile.IniReadValue("dbo", "dbosp").Trim() == "1")
            {
                this.isdbosp = true;
            }
        }
        return this.isdbosp;
    }

    private SqlCommand OpenDB(string DbName)
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
            SqlCommand command = new SqlCommand();
            command.Connection = this.connect;
            if (this.connect.State == ConnectionState.Closed)
            {
                this.connect.Open();
            }
            command.CommandText = "use [" + DbName + "]";
            command.ExecuteNonQuery();
            return command;
        }
        catch (Exception exception)
        {
            string message = exception.Message;
            return null;
        }
    }

    public DataSet Query(string DbName, string SQLString)
    {
        DataSet dataSet = new DataSet();
        try
        {
            this.OpenDB(DbName);
            new SqlDataAdapter(SQLString, this.connect).Fill(dataSet, "ds");
        }
        catch (SqlException exception)
        {
            throw new Exception(exception.Message);
        }
        return dataSet;
    }

    public bool RenameTable(string DbName, string OldName, string NewName)
    {
        try
        {
            SqlCommand command = this.OpenDB(DbName);
            command.CommandText = "EXEC sp_rename '" + OldName + "', '" + NewName + "'";
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public DataSet RunProcedure(string DbName, string storedProcName, IDataParameter[] parameters, string tableName)
    {
        this.OpenDB(DbName);
        DataSet dataSet = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = this.BuildQueryCommand(this.connect, storedProcName, parameters);
        adapter.Fill(dataSet, tableName);
        return dataSet;
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
            return "SQL2005";
        }
    }
}


    public class SQLServer2005DbScriptBuilder : IDbScriptBuilder
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
    private SQLServer2005DbObject dbobj = new SQLServer2005DbObject();

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
        plus.AppendLine("if exists (select * from sysobjects where id = OBJECT_ID('[" + tablename + "]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) ");
        plus.AppendLine("DROP TABLE [" + tablename + "]");
        string str = "";
        bool flag = false;
        new StringPlus();
        Hashtable hashtable = new Hashtable();
        StringPlus plus2 = new StringPlus();
        plus.AppendLine("");
        plus.AppendLine("CREATE TABLE [" + tablename + "] (");
        if (columnInfoDt != null)
        {
            DataRow[] rowArray;
            if (this.Fieldlist.Count > 0)
            {
                rowArray = columnInfoDt.Select("ColumnName in (" + this.Fields + ")", "colorder asc");
            }
            else
            {
                rowArray = columnInfoDt.Select();
            }
            foreach (DataRow row in rowArray)
            {
                string key = row["ColumnName"].ToString();
                string str3 = row["TypeName"].ToString();
                string str4 = row["IsIdentity"].ToString();
                string length = row["Length"].ToString();
                string str6 = row["Preci"].ToString();
                string str7 = row["Scale"].ToString();
                string str8 = row["isPK"].ToString();
                string str9 = row["cisNull"].ToString();
                string str10 = row["defaultVal"].ToString();
                plus.Append("[" + key + "] [" + str3 + "] ");
                if (str4 == "√")
                {
                    flag = true;
                    plus.Append(" IDENTITY (1, 1) ");
                }
                switch (str3.Trim())
                {
                    case "varchar":
                    case "char":
                    case "nchar":
                    case "binary":
                    case "nvarchar":
                    case "varbinary":
                        plus.Append(" (" + CodeCommon.GetDataTypeLenVal(str3.Trim(), length) + ")");
                        break;

                    case "decimal":
                    case "numeric":
                        plus.Append(" (" + str6 + "," + str7 + ")");
                        break;
                }
                if (str9 == "√")
                {
                    plus.Append(" NULL");
                }
                else
                {
                    plus.Append(" NOT NULL");
                }
                if (str10 != "")
                {
                    plus.Append(" DEFAULT " + str10);
                }
                plus.AppendLine(",");
                hashtable.Add(key, str3);
                plus2.Append("[" + key + "],");
                if ((str8 == "√") && (str == ""))
                {
                    str = key;
                }
            }
        }
        plus.DelLastComma();
        plus2.DelLastComma();
        plus.AppendLine(")");
        plus.AppendLine("");
        if (str != "")
        {
            plus.AppendLine("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD  CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  NONCLUSTERED ( [" + str + "] )");
        }
        if (flag)
        {
            plus.AppendLine("SET IDENTITY_INSERT [" + tablename + "] ON");
            plus.AppendLine("");
        }
        DataTable tabData = this.dbobj.GetTabData(dbname, tablename);
        if (tabData != null)
        {
            foreach (DataRow row2 in tabData.Rows)
            {
                StringPlus plus3 = new StringPlus();
                StringPlus plus4 = new StringPlus();
                foreach (string str12 in plus2.Value.Split(new char[] { ',' }))
                {
                    string str13 = str12.Substring(1, str12.Length - 2);
                    string columnType = "";
                    foreach (DictionaryEntry entry in hashtable)
                    {
                        if (entry.Key.ToString() == str13)
                        {
                            columnType = entry.Value.ToString();
                        }
                    }
                    string str15 = "";
                    string str17 = columnType;
                    if (str17 == null)
                    {
                        goto Label_05C8;
                    }
                    if (!(str17 == "binary"))
                    {
                        if (str17 == "bit")
                        {
                            goto Label_0599;
                        }
                        goto Label_05C8;
                    }
                    byte[] bytes = (byte[]) row2[str13];
                    str15 = CodeCommon.ToHexString(bytes);
                    goto Label_05DD;
                Label_0599:
                    str15 = (row2[str13].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_05DD;
                Label_05C8:
                    str15 = row2[str13].ToString().Trim();
                Label_05DD:
                    if (str15 != "")
                    {
                        if (CodeCommon.IsAddMark(columnType))
                        {
                            plus4.Append("'" + str15 + "',");
                        }
                        else
                        {
                            plus4.Append(str15 + ",");
                        }
                        plus3.Append("[" + str13 + "],");
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
        if (flag)
        {
            plus.AppendLine("");
            plus.AppendLine("SET IDENTITY_INSERT [" + tablename + "] OFF");
        }
        return plus.Value;
    }

    public void CreateTabScript(string dbname, string tablename, string filename, ProgressBar progressBar)
    {
        StreamWriter writer = new StreamWriter(filename, true, Encoding.Default);
        this.dbobj.DbConnectStr = this._dbconnectStr;
        List<ColumnInfo> columnInfoList = this.dbobj.GetColumnInfoList(dbname, tablename);
        StringPlus plus = new StringPlus();
        plus.AppendLine("if exists (select * from sysobjects where id = OBJECT_ID('[" + tablename + "]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) ");
        plus.AppendLine("DROP TABLE [" + tablename + "]");
        string str = "";
        bool flag = false;
        new StringPlus();
        Hashtable hashtable = new Hashtable();
        StringPlus plus2 = new StringPlus();
        plus.AppendLine("");
        plus.AppendLine("CREATE TABLE [" + tablename + "] (");
        if ((columnInfoList != null) && (columnInfoList.Count > 0))
        {
            int num = 0;
            progressBar.Maximum = columnInfoList.Count;
            foreach (ColumnInfo info in columnInfoList)
            {
                num++;
                progressBar.Value = num;
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
                if (isIdentity)
                {
                    flag = true;
                    plus.Append(" IDENTITY (1, 1) ");
                }
                switch (typeName.Trim())
                {
                    case "varchar":
                    case "char":
                    case "nchar":
                    case "binary":
                    case "nvarchar":
                    case "varbinary":
                        plus.Append(" (" + CodeCommon.GetDataTypeLenVal(typeName.Trim(), length) + ")");
                        break;

                    case "decimal":
                    case "numeric":
                        plus.Append(" (" + preci + "," + scale + ")");
                        break;
                }
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
                if (isPK && (str == ""))
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
            plus.AppendLine("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD  CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  NONCLUSTERED ( [" + str + "] )");
        }
        if (flag)
        {
            plus.AppendLine("SET IDENTITY_INSERT [" + tablename + "] ON");
            plus.AppendLine("");
        }
        writer.Write(plus.Value);
        DataTable tabData = this.dbobj.GetTabData(dbname, tablename);
        if (tabData != null)
        {
            int num2 = 0;
            progressBar.Maximum = tabData.Rows.Count;
            foreach (DataRow row in tabData.Rows)
            {
                num2++;
                progressBar.Value = num2;
                StringPlus plus3 = new StringPlus();
                StringPlus plus4 = new StringPlus();
                StringPlus plus5 = new StringPlus();
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
                        goto Label_058D;
                    }
                    if (!(str14 == "binary"))
                    {
                        if (str14 == "bit")
                        {
                            goto Label_055E;
                        }
                        goto Label_058D;
                    }
                    byte[] bytes = (byte[]) row[str10];
                    str12 = CodeCommon.ToHexString(bytes);
                    goto Label_05A2;
                Label_055E:
                    str12 = (row[str10].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_05A2;
                Label_058D:
                    str12 = row[str10].ToString().Trim();
                Label_05A2:
                    if (str12 != "")
                    {
                        if (CodeCommon.IsAddMark(columnType))
                        {
                            plus5.Append("'" + str12 + "',");
                        }
                        else
                        {
                            plus5.Append(str12 + ",");
                        }
                        plus4.Append("[" + str10 + "],");
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
        if (flag)
        {
            StringPlus plus6 = new StringPlus();
            plus6.AppendLine("");
            plus6.AppendLine("SET IDENTITY_INSERT [" + tablename + "] OFF");
            writer.Write(plus6.Value);
        }
        writer.Flush();
        writer.Close();
    }

    public string CreateTabScriptBySQL(string dbname, string strSQL)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        bool flag = false;
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
                    if (column.AutoIncrement)
                    {
                        flag = true;
                    }
                    string str4 = "";
                    string str5 = name.ToLower();
                    if (str5 == null)
                    {
                        goto Label_0141;
                    }
                    if (!(str5 == "binary") && !(str5 == "byte[]"))
                    {
                        if ((str5 == "bit") || (str5 == "boolean"))
                        {
                            goto Label_0112;
                        }
                        goto Label_0141;
                    }
                    byte[] bytes = (byte[]) row[columnName];
                    str4 = CodeCommon.ToHexString(bytes);
                    goto Label_0156;
                Label_0112:
                    str4 = (row[columnName].ToString().ToLower() == "true") ? "1" : "0";
                    goto Label_0156;
                Label_0141:
                    str4 = row[columnName].ToString().Trim();
                Label_0156:
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
                        plus2.Append("[" + columnName + "],");
                    }
                }
                plus2.DelLastComma();
                plus3.DelLastComma();
                plus.Append("INSERT [" + str + "] (");
                plus.Append(plus2.Value);
                plus.Append(") VALUES ( ");
                plus.Append(plus3.Value);
                plus.AppendLine(")");
            }
        }
        StringPlus plus4 = new StringPlus();
        if (flag)
        {
            plus4.AppendLine("SET IDENTITY_INSERT [" + str + "] ON");
            plus4.AppendLine("");
        }
        plus4.AppendLine(plus.Value);
        if (flag)
        {
            plus4.AppendLine("");
            plus4.AppendLine("SET IDENTITY_INSERT [" + str + "] OFF");
        }
        return plus4.Value;
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
        plus.AppendLine(this.GetInParameter(this.Fieldlist, true));
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
        plus.AppendSpaceLine(1, "INSERT INTO [" + this._tablename + "](");
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
        plus.AppendSpaceLine(1, "DELETE [" + this._tablename + "]");
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
        plus.AppendSpaceLine(1, " FROM [" + this._tablename + "]");
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
                        plus.AppendSpaceLine(1, "SELECT @TempID = max([" + columnName + "])+1 FROM [" + this._tablename + "]");
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
        plus.AppendSpaceLine(1, " FROM [" + this._tablename + "]");
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
        plus.AppendSpaceLine(1, "SELECT @TempID = count(1) FROM [" + this._tablename + "] WHERE " + this.GetWhereExpression(this.Keys));
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
                case "char":
                case "nchar":
                case "binary":
                case "nvarchar":
                case "varbinary":
                {
                    string dataTypeLenVal = CodeCommon.GetDataTypeLenVal(typeName.Trim(), length);
                    plus.AppendLine("@" + columnName + " " + typeName + "(" + dataTypeLenVal + "),");
                    break;
                }
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
        plus.AppendSpaceLine(1, "UPDATE [" + this._tablename + "] SET ");
        plus.AppendSpaceLine(1, plus2.Value);
        plus.AppendSpaceLine(1, "WHERE " + this.GetWhereExpression(this.Keys));
        plus.AppendLine();
        plus.AppendLine("GO");
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
            switch (typeName.ToLower())
            {
                case "decimal":
                case "numeric":
                    plus.Append("@" + columnName + " " + typeName + "(" + preci + "," + scale + ")");
                    break;

                case "char":
                case "varchar":
                case "varbinary":
                case "binary":
                case "nchar":
                case "nvarchar":
                    plus.Append("@" + columnName + " " + typeName + "(" + CodeCommon.GetDataTypeLenVal(typeName.ToLower(), length) + ")");
                    break;

                default:
                    plus.Append("@" + columnName + " " + typeName);
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
        plus.AppendLine("delete from [" + tablename + "]");
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
        plus.AppendLine("INSERT INTO [" + tablename + "] ( ");
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
        plus.Append(" from [" + tablename + "]");
        return plus.Value;
    }

    public string GetSQLUpdate(string dbname, string tablename)
    {
        this.dbobj.DbConnectStr = this._dbconnectStr;
        List<ColumnInfo> columnList = this.dbobj.GetColumnList(dbname, tablename);
        this.DbName = dbname;
        this.TableName = tablename;
        StringPlus plus = new StringPlus();
        plus.AppendLine("update [" + tablename + "] set ");
        if ((columnList != null) && (columnList.Count > 0))
        {
            foreach (ColumnInfo info in columnList)
            {
                plus.AppendLine("[" + info.ColumnName + "] = <" + info.ColumnName + ">,");
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

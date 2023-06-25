using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Flextronics.Applications.Library.Utility;

namespace Flextronics.Applications.Library.Schema
{
    public interface IDbObject
    {
        // Methods
        bool DeleteTable(string DbName, string TableName);
        int ExecuteSql(string DbName, string SQLString);
        List<ColumnInfo> GetColumnInfoList(string DbName, string TableName);
        List<ColumnInfo> GetColumnList(string DbName, string TableName);
        List<string> GetDBList();
        DataTable GetKeyName(string DbName, string TableName);
        string GetObjectInfo(string DbName, string objName);
        List<TableInfo> GetProcInfo(string DbName);
        DataTable GetProcs(string DbName);
        DataTable GetTabData(string DbName, string TableName);
        List<string> GetTables(string DbName);
        List<TableInfo> GetTablesInfo(string DbName);
        DataTable GetTabViews(string DbName);
        List<TableInfo> GetTabViewsInfo(string DbName);
        string GetVersion();
        DataTable GetVIEWs(string DbName);
        List<TableInfo> GetVIEWsInfo(string DbName);
        DataSet Query(string DbName, string SQLString);
        bool RenameTable(string DbName, string OldName, string NewName);

        // Properties
        string DbConnectStr { get; set; }
        string DbType { get; }
    }

    public interface IDbScriptBuilder
    {
        // Methods
        string CreateDBTabScript(string dbname);
        string CreateTabScript(string dbname, string tablename);
        void CreateTabScript(string dbname, string tablename, string filename, ProgressBar progressBar);
        string CreateTabScriptBySQL(string dbname, string strSQL);
        string CreatPROCADD();
        string CreatPROCDelete();
        string CreatPROCGetList();
        string CreatPROCGetMaxID();
        string CreatPROCGetModel();
        string CreatPROCIsHas();
        string CreatPROCUpdate();
        string GetPROCCode(string dbname);
        string GetPROCCode(string dbname, string tablename);
        string GetPROCCode(bool Maxid, bool Ishas, bool Add, bool Update, bool Delete, bool GetModel, bool List);
        string GetSQLDelete(string dbname, string tablename);
        string GetSQLInsert(string dbname, string tablename);
        string GetSQLSelect(string dbname, string tablename);
        string GetSQLUpdate(string dbname, string tablename);

        // Properties
        string DbConnectStr { get; set; }
        string DbName { get; set; }
        List<ColumnInfo> Fieldlist { get; set; }
        string Fields { get; }
        List<ColumnInfo> Keys { get; set; }
        string ProcPrefix { get; set; }
        string ProjectName { get; set; }
        string TableName { get; set; }
    }
}

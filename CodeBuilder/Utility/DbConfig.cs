using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Flextronics.Applications.Library.Utility
{

    public class DbConfig
    {
        private static string fileName = (Application.StartupPath + @"\Setting.config");

        public static bool AddSettings(DbSettings dbobj)
        {
            try
            {
                DataSet set = new DataSet();
                if (!File.Exists(fileName))
                {
                    DataTable table = CreateDataTable();
                    DataRow row = table.NewRow();
                    row["DbType"] = dbobj.DbType;
                    row["Server"] = dbobj.Server;
                    row["ConnectStr"] = dbobj.ConnectStr;
                    row["DbName"] = dbobj.DbName;
                    row["ConnectSimple"] = dbobj.ConnectSimple;
                    table.Rows.Add(row);
                    set.Tables.Add(table);
                }
                else
                {
                    set.ReadXml(fileName);
                    if (set.Tables.Count > 0)
                    {
                        if (set.Tables[0].Select("DbType='" + dbobj.DbType + "' and Server='" + dbobj.Server + "' and DbName='" + dbobj.DbName + "'").Length > 0)
                        {
                            return false;
                        }
                        DataRow row2 = set.Tables[0].NewRow();
                        row2["DbType"] = dbobj.DbType;
                        row2["Server"] = dbobj.Server;
                        row2["ConnectStr"] = dbobj.ConnectStr;
                        row2["DbName"] = dbobj.DbName;
                        row2["ConnectSimple"] = dbobj.ConnectSimple;
                        set.Tables[0].Rows.Add(row2);
                    }
                    else
                    {
                        DataTable table2 = CreateDataTable();
                        DataRow row3 = table2.NewRow();
                        row3["DbType"] = dbobj.DbType;
                        row3["Server"] = dbobj.Server;
                        row3["ConnectStr"] = dbobj.ConnectStr;
                        row3["DbName"] = dbobj.DbName;
                        row3["ConnectSimple"] = dbobj.ConnectSimple;
                        table2.Rows.Add(row3);
                        set.Tables.Add(table2);
                    }
                }
                set.WriteXml(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable CreateDataTable()
        {
            DataTable table = new DataTable("DBServer");
            DataColumn column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DbType";
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Server";
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ConnectStr";
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DbName";
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.Boolean");
            column.ColumnName = "ConnectSimple";
            table.Columns.Add(column);
            return table;
        }

        public static void DelSetting(string DbType, string Serverip, string DbName)
        {
            try
            {
                DataSet set = new DataSet();
                if (File.Exists(fileName))
                {
                    set.ReadXml(fileName);
                    if (set.Tables.Count > 0)
                    {
                        string filterExpression = "DbType='" + DbType + "' and Server='" + Serverip + "'";
                        if ((DbName.Trim() != "") && (DbName.Trim() != "master"))
                        {
                            filterExpression = filterExpression + " and DbName='" + DbName + "'";
                        }
                        DataRow[] rowArray = set.Tables[0].Select(filterExpression);
                        if (rowArray.Length > 0)
                        {
                            set.Tables[0].Rows.Remove(rowArray[0]);
                        }
                        set.Tables[0].AcceptChanges();
                    }
                }
                set.WriteXml(fileName);
            }
            catch
            {
            }
        }

        public static DbSettings GetSetting(string loneServername)
        {
            //bug  (local)(SQL2000)(pubs)
            string dbType = "SQL2000";
            int index = loneServername.IndexOf("(");
            string serverip = loneServername.Substring(0, index);
            int num2 = loneServername.IndexOf(")", index);
            dbType = loneServername.Substring(index + 1, (num2 - index) - 1);
            string dbName = "";
            if (loneServername.Length > (num2 + 1))
            {
                dbName = loneServername.Substring(num2 + 2).Replace(")", "");
            }
            return GetSetting(dbType, serverip, dbName);
        }

        public static DbSettings GetSetting(string DbType, string Serverip, string DbName)
        {
            try
            {
                DbSettings settings = null;
                DataSet set = new DataSet();
                if (File.Exists(fileName))
                {
                    set.ReadXml(fileName);
                    if (set.Tables.Count > 0)
                    {
                        string filterExpression = "DbType='" + DbType + "' and Server='" + Serverip + "'";
                        if (DbName.Trim() != "")
                        {
                            filterExpression = filterExpression + " and DbName='" + DbName + "'";
                        }
                        DataRow[] rowArray = set.Tables[0].Select(filterExpression);
                        if (rowArray.Length > 0)
                        {
                            settings = new DbSettings();
                            settings.DbType = rowArray[0]["DbType"].ToString();
                            settings.Server = rowArray[0]["Server"].ToString();
                            settings.ConnectStr = rowArray[0]["ConnectStr"].ToString();
                            settings.DbName = rowArray[0]["DbName"].ToString();
                            if ((set.Tables[0].Columns.Contains("ConnectSimple") && (rowArray[0]["ConnectSimple"] != null)) && (rowArray[0]["ConnectSimple"].ToString().Length > 0))
                            {
                                settings.ConnectSimple = bool.Parse(rowArray[0]["ConnectSimple"].ToString());
                            }
                        }
                    }
                }
                return settings;
            }
            catch
            {
                return null;
            }
        }

        public static DataSet GetSettingDs()
        {
            try
            {
                DataSet set = new DataSet();
                if (File.Exists(fileName))
                {
                    set.ReadXml(fileName);
                }
                return set;
            }
            catch
            {
                return null;
            }
        }

        public static DbSettings[] GetSettings()
        {
            try
            {
                DataSet set = new DataSet();
                ArrayList list = new ArrayList();
                if (File.Exists(fileName))
                {
                    set.ReadXml(fileName);
                    if (set.Tables.Count > 0)
                    {
                        foreach (DataRow row in set.Tables[0].Rows)
                        {
                            DbSettings settings = new DbSettings();
                            settings.DbType = row["DbType"].ToString();
                            settings.Server = row["Server"].ToString();
                            settings.ConnectStr = row["ConnectStr"].ToString();
                            settings.DbName = row["DbName"].ToString();
                            if ((set.Tables[0].Columns.Contains("ConnectSimple") && (row["ConnectSimple"] != null)) && (row["ConnectSimple"].ToString().Length > 0))
                            {
                                settings.ConnectSimple = bool.Parse(row["ConnectSimple"].ToString());
                            }
                            list.Add(settings);
                        }
                    }
                }
                return (DbSettings[]) list.ToArray(typeof(DbSettings));
            }
            catch
            {
                return null;
            }
        }

        public static void UpdateSettings(DbSettings dbobj)
        {
            try
            {
                DataSet set = new DataSet();
                if (!File.Exists(fileName))
                {
                    DataTable table = CreateDataTable();
                    DataRow row = table.NewRow();
                    row["DbType"] = dbobj.DbType;
                    row["Server"] = dbobj.Server;
                    row["ConnectStr"] = dbobj.ConnectStr;
                    row["DbName"] = dbobj.DbName;
                    row["ConnectSimple"] = dbobj.ConnectSimple;
                    table.Rows.Add(row);
                    set.Tables.Add(table);
                }
                else
                {
                    set.ReadXml(fileName);
                    if (set.Tables.Count > 0)
                    {
                        DataRow[] rowArray = set.Tables[0].Select("DbType='" + dbobj.DbType + "' and Server='" + dbobj.Server + "' and DbName='" + dbobj.DbName + "'");
                        if (rowArray.Length > 0)
                        {
                            rowArray[0]["DbType"] = dbobj.DbType;
                            rowArray[0]["Server"] = dbobj.Server;
                            rowArray[0]["ConnectStr"] = dbobj.ConnectStr;
                            rowArray[0]["DbName"] = dbobj.DbName;
                            rowArray[0]["ConnectSimple"] = dbobj.ConnectSimple;
                        }
                        else
                        {
                            DataRow row2 = set.Tables[0].NewRow();
                            row2["DbType"] = dbobj.DbType;
                            row2["Server"] = dbobj.Server;
                            row2["ConnectStr"] = dbobj.ConnectStr;
                            row2["DbName"] = dbobj.DbName;
                            row2["ConnectSimple"] = dbobj.ConnectSimple;
                            set.Tables[0].Rows.Add(row2);
                        }
                    }
                    else
                    {
                        DataTable table2 = CreateDataTable();
                        DataRow row3 = table2.NewRow();
                        row3["DbType"] = dbobj.DbType;
                        row3["Server"] = dbobj.Server;
                        row3["ConnectStr"] = dbobj.ConnectStr;
                        row3["DbName"] = dbobj.DbName;
                        row3["ConnectSimple"] = dbobj.ConnectSimple;
                        table2.Rows.Add(row3);
                        set.Tables.Add(table2);
                    }
                }
                set.WriteXml(fileName);
            }
            catch
            {
                throw new Exception("保存配置信息失败！");
            }
        }
    }
}


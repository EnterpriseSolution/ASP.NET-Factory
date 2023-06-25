//datatype.ini
//[DbToCS]
//varchar=string
//varchar2=string
//nvarchar=string
//nvarchar2=string
//char=string
//nchar=string
//text=string
//ntext=string
//string=string
//date=DateTime
//datetime=DateTime
//smalldatetime=DateTime
//smallint=int
//int=int
//number=int
//bigint=int
//tinyint=int
//float=decimal
//numeric=decimal
//decimal=decimal
//money=decimal
//smallmoney=decimal
//real=decimal
//bit=bool
//binary=byte[]
//varbinary=byte[]
//image=byte[]
//raw=byte[]
//long=byte[]
//long raw=byte[]
//blob=byte[]
//bfile=byte[]
//uniqueidentifier=Guid
//integer=int
//double=decimal
//enum=Enum
//timestamp=DateTime


//[ToSQLProc]
//varchar=VarChar
//string=VarChar
//nvarchar=NVarChar
//char=Char
//nchar=NChar
//text=Text
//ntext=NText
//datetime=DateTime
//smalldatetime=SmallDateTime
//smallint=SmallInt
//tinyint=TinyInt
//int=Int
//bigint=BigInt
//float=Float
//real=Real
//numeric=Decimal
//decimal=Decimal
//money=Money
//smallmoney=SmallMoney
//bool=Bit
//bit=Bit
//binary=Binary
//varbinary=VarBinary
//image=Image
//uniqueidentifier=UniqueIdentifier
//timestamp=Timestamp


//[ToOraProc]
//char=Char
//varchar2=VarChar
//string=VarChar
//nvarchar2=NVarChar
//nchar=NChar
//long=LongVarChar
//number=Number
//int=Number
//date=DateTime
//raw=Raw
//long raw=LongRaw
//blob=Blob
//bit=Clob
//clob=Clob
//nclob=NClob
//bfile=BFile


//[ToMySQLProc]
//binary=Binary
//bool=Bit
//bit=Bit
//blob=Blob
//double=Double
//Date=DateTime
//datetime=DateTime
//numeric=Decimal
//decimal=Decimal
//float=Float
//enum=Enum
//geometry=Geometry
//longBlob=LongBlob
//longText=LongText
//varchar=VarChar
//string=String
//char=Char
//text=Text
//longtext=LongText
//time=Time
//SmallInt=Int32
//TinyInt=Int32
//timestamp=Timestamp
//tinyText=TinyText
//tinyBlob=TinyBlob
//int=Int32
//varbinary=VarBinary
//varstring=VarString
//year=Year
//varchar=VarChar


//[ToOleDbProc]
//varchar=VarChar
//string=VarChar
//nvarchar=LongVarChar
//char=Char
//nchar=NChar
//text=LongVarChar
//ntext=LongVarChar
//datetime=Date
//smalldatetime=Date
//smallint=SmallInt
//tinyint=TinyInt
//int=Integer
//bigint=BigInt
//money=Decimal
//smallmoney=Decimal
//float=Decimal
//numeric=Decimal
//decimal=Decimal
//bool=Boolean
//bit=Bit
//binary=Binary

//[IsAddMark]
//nvarchar=true
//nchar=true
//ntext=true
//varchar=true
//varchar2=true
//nvarchar2=true
//char=true
//clob=true
//string=true
//text=true
//date=true
//datetime=true
//smalldatetime=true
//uniqueidentifier=true

//[isValueType]
//int=true
//Int32=true
//Int16=true
//Int64=true
//DateTime=true
//decimal=true
//Decimal=true



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace Flextronics.Applications.Library.Utility
{

    public class  CSharpTypeMappingManager
    {
        static Hashtable tbl;
        static CSharpTypeMappingManager()
        {
            tbl = new Hashtable();
            tbl.Add("varchar", "string");
            tbl.Add("varchar2","string");
            tbl.Add("nvarchar","string");
            tbl.Add("nvarchar2","string");
            tbl.Add("char","string");
            tbl.Add("nchar","string");
            tbl.Add("text","string");
            tbl.Add("ntext","string");
            tbl.Add("string","string");
            tbl.Add("date","DateTime");
            tbl.Add("datetime","DateTime");
            tbl.Add("smalldatetime","DateTime");
            tbl.Add("smallint","int");
            tbl.Add("int","int");
            tbl.Add("number","int");
            tbl.Add("bigint","int");
            tbl.Add("tinyint","int");
            tbl.Add("float","decimal");
            tbl.Add("numeric","decimal");
            tbl.Add("decimal","decimal");
            tbl.Add("money","decimal");
            tbl.Add("smallmoney","decimal");
            tbl.Add("real","decimal");
            tbl.Add("bit","bool");
            tbl.Add("binary","byte[]");
            tbl.Add("varbinary","byte[]");
            tbl.Add("image","byte[]");
            tbl.Add("raw","byte[]");
            tbl.Add("long","byte[]");
            tbl.Add("long raw","byte[]");
            tbl.Add("blob","byte[]");
            tbl.Add("bfile","byte[]");
            tbl.Add("uniqueidentifier","Guid");
            tbl.Add("integer","int");
            tbl.Add("double","decimal");
            tbl.Add("enum","Enum");
            tbl.Add("timestamp","DateTime");
            
        }
        public static string Target(string type)
        {
           return  tbl[type].ToString();
        }
    }
    public class  ProcedureTypeMappingManager
    {
        static Hashtable tbl;
        static ProcedureTypeMappingManager()
        {
            tbl = new Hashtable();
            tbl.Add("varchar", "VarChar");
            tbl.Add("string", "VarChar");
            tbl.Add("nvarchar", "NVarChar");
            tbl.Add("char", "Char");
            tbl.Add("nchar", "NChar");
            tbl.Add("text", "Text");
            tbl.Add("ntext", "NText");
            tbl.Add("datetime", "DateTime");
            tbl.Add("smalldatetime", "SmallDateTime");
            tbl.Add("smallint", "SmallInt");
            tbl.Add("tinyint", "TinyInt");
            tbl.Add(" int", "Int");
            tbl.Add(" bigint", "BigInt");
            tbl.Add(" float", "Float");
            tbl.Add("real", "Real");
            tbl.Add("numeric", "Decimal");
            tbl.Add(" decimal", "Decimal");
            tbl.Add(" money", "Money");
            tbl.Add(" smallmoney", "SmallMoney");
            tbl.Add(" bool", "Bit");
            tbl.Add(" bit", "Bit");
            tbl.Add(" binary", "Binary");
            tbl.Add(" varbinary", "VarBinary");
            tbl.Add("  image", "Image");
            tbl.Add(" uniqueidentifier", "UniqueIdentifier");
            tbl.Add("timestamp", "Timestamp");

        }
        
        public static string Target(string type)
        {
            return tbl[type].ToString();
        }
    }
    public class AddMaskMappingManager
    {
        static Hashtable tbl;
        static AddMaskMappingManager()
        {
            tbl = new Hashtable();
            tbl.Add("nvarchar", "true ");
            tbl.Add("nchar", "true");
            tbl.Add("ntext", "true");
            tbl.Add("varchar", "true");
            tbl.Add("varchar2", "true");
            tbl.Add("nvarchar2", "true");
            tbl.Add("char", "true");
            tbl.Add("clob", "true");
            tbl.Add("string", "true");
            tbl.Add("text", "true");
            tbl.Add("date", "true");
            tbl.Add("datetime", "true");
            tbl.Add("smalldatetime", "true");
            tbl.Add("uniqueidentifier", "true");
        }

        public static string Target(string type)
        {
            return tbl[type].ToString();
        }
    }
    public class  ValueTypeMappingManager
    {
        static Hashtable tbl;
        static ValueTypeMappingManager()
        {
            tbl = new Hashtable();
            tbl.Add("int","true");
            tbl.Add("Int32","true");
            tbl.Add("Int16","true");
            tbl.Add("Int64","true");
            tbl.Add("DateTime","true");
            tbl.Add("decimal","true");
            tbl.Add("Decimal","true");         
        }

        public static string Target(string type)
        {
            return tbl[type].ToString();
        }
    }  
  
    public class CodeCommon
    {
        // Fields
        private static INIFile datatype;
        private static string datatypefile = (Application.StartupPath + @"\datatype.ini");
        private static char[] hexDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string CSToProcType(string DbType, string cstype)
        {
            string sourceCodeType = cstype;
            string databaseType = DbType;
            if (databaseType == null)
            {
                return sourceCodeType;
            }
            if (!(databaseType == "SQL2000") && !(databaseType == "SQL2005"))
            {
                if (databaseType != "Oracle")
                {
                    if (databaseType == "MySQL")
                    {
                        return CSToProcTypeMySQL(cstype);
                    }
                    if (databaseType != "OleDb")
                    {
                        return sourceCodeType;
                    }
                    return CSToProcTypeOleDb(cstype);
                }
            }
            else
            {
                return CSToProcTypeSQL(cstype);
            }
            return CSToProcTypeOra(cstype);
        }

        private static string CSToProcTypeMySQL(string cstype)
        {
            string str = cstype;
            if (!File.Exists(datatypefile))
            {
                return str;
            }
            datatype = new INIFile(datatypefile);
            string str2 = datatype.IniReadValue("ToMySQLProc", cstype.ToLower().Trim());
            if (str2 == "")
            {
                return cstype.ToLower().Trim();
            }
            return str2;
        }

        private static string CSToProcTypeOleDb(string cstype)
        {
            string str = cstype;
            if (!File.Exists(datatypefile))
            {
                return str;
            }
            datatype = new INIFile(datatypefile);
            string str2 = datatype.IniReadValue("ToOleDbProc", cstype.ToLower().Trim());
            if (str2 == "")
            {
                return cstype.ToLower().Trim();
            }
            return str2;
        }

        private static string CSToProcTypeOra(string cstype)
        {
            string str = cstype;
            if (!File.Exists(datatypefile))
            {
                return str;
            }
            datatype = new INIFile(datatypefile);
            string str2 = datatype.IniReadValue("ToOraProc", cstype.ToLower().Trim());
            if (str2 == "")
            {
                return cstype.ToLower().Trim();
            }
            return str2;
        }

        private static string CSToProcTypeSQL(string cstype)
        {
            //string str = cstype;
            //if (!File.Exists(datatypefile))
            //{
            //    return str;
            //}
            //datatype = new INIFile(datatypefile);
            //string str2 = datatype.IniReadValue("ToSQLProc", cstype.ToLower().Trim());
            //if (str2 == "")
            //{
            //    return cstype.ToLower().Trim();
            //}
            //return str2;

            string target=ProcedureTypeMappingManager.Target(cstype);
            return target;
        }

        public static string DbTypeLength(string dbtype, string datatype, string Length)
        {
            string str = "";
            string str2 = dbtype;
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "SQL2000") && !(str2 == "SQL2005"))
            {
                if (str2 != "Oracle")
                {
                    if (str2 == "MySQL")
                    {
                        return DbTypeLengthMySQL(datatype, Length);
                    }
                    if (str2 != "OleDb")
                    {
                        return str;
                    }
                    return DbTypeLengthOleDb(datatype, Length);
                }
            }
            else
            {
                return DbTypeLengthSQL(dbtype, datatype, Length);
            }
            return DbTypeLengthOra(datatype, Length);
        }

        private static string DbTypeLengthMySQL(string datatype, string Length)
        {
            string str = "";
            switch (datatype.Trim().ToLower())
            {
                case "number":
                    str = "4";
                    break;

                case "varchar2":
                    if (!(Length == ""))
                    {
                        str = Length;
                        break;
                    }
                    str = "50";
                    break;

                case "char":
                    if (!(Length == ""))
                    {
                        str = Length;
                        break;
                    }
                    str = "50";
                    break;

                case "date":
                case "nchar":
                case "nvarchar2":
                case "long":
                case "long raw":
                case "bfile":
                case "blob":
                    break;

                default:
                    str = Length;
                    break;
            }
            if (str != "")
            {
                return (CSToProcType("MySQL", datatype) + "," + str);
            }
            return CSToProcType("MySQL", datatype);
        }

        private static string DbTypeLengthOleDb(string datatype, string Length)
        {
            string str = "";
            switch (datatype.Trim())
            {
                case "int":
                    str = "4";
                    break;

                case "varchar":
                    if (!(Length == ""))
                    {
                        str = Length;
                        break;
                    }
                    str = "50";
                    break;

                case "char":
                    if (!(Length == ""))
                    {
                        str = Length;
                        break;
                    }
                    str = "50";
                    break;

                case "bit":
                    str = "1";
                    break;

                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "binary":
                case "smallint":
                case "bigint":
                    str = Length;
                    break;

                case "image ":
                case "datetime":
                case "smalldatetime":
                case "nchar":
                case "nvarchar":
                case "ntext":
                case "text":
                    break;

                default:
                    str = Length;
                    break;
            }
            if (str != "")
            {
                return (CSToProcType("OleDb", datatype) + "," + str);
            }
            return CSToProcType("OleDb", datatype);
        }

        private static string DbTypeLengthOra(string datatype, string Length)
        {
            string str = "";
            switch (datatype.Trim().ToLower())
            {
                case "number":
                    str = "4";
                    break;

                case "varchar2":
                    if (!(Length == ""))
                    {
                        str = Length;
                        break;
                    }
                    str = "50";
                    break;

                case "char":
                    if (!(Length == ""))
                    {
                        str = Length;
                        break;
                    }
                    str = "50";
                    break;

                case "date":
                case "nchar":
                case "nvarchar2":
                case "long":
                case "long raw":
                case "bfile":
                case "blob":
                    break;

                default:
                    str = Length;
                    break;
            }
            if (str != "")
            {
                return (CSToProcType("Oracle", datatype) + "," + str);
            }
            return CSToProcType("Oracle", datatype);
        }

        private static string DbTypeLengthSQL(string dbtype, string datatype, string Length)
        {
            string dataTypeLenVal = GetDataTypeLenVal(datatype, Length);
            if (dataTypeLenVal != "")
            {
                return (CSToProcType(dbtype, datatype) + "," + dataTypeLenVal);
            }
            return CSToProcType(dbtype, datatype);
        }

        public static string DbTypeToCS(string dbtype)
        {
            //string str = "string";
            //if (!File.Exists(datatypefile))
            //{
            //    return str;
            //}
            //datatype = new INIFile(datatypefile);
            //string str2 = datatype.IniReadValue("DbToCS", dbtype.ToLower().Trim());
            //if (str2 == "")
            //{
            //    return dbtype.ToLower().Trim();
            //}
            //return str2;

            string target = CSharpTypeMappingManager.Target(dbtype);
            return target;
        }

        public static DataTable GetColumnInfoDt(List<ColumnInfo> collist)
        {
            DataTable table = new DataTable();
            table.Columns.Add("colorder",typeof(Int32));
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
            foreach (ColumnInfo info in collist)
            {
                DataRow row = table.NewRow();
                row["colorder"] = info.Colorder;
                row["ColumnName"] = info.ColumnName;
                row["TypeName"] = info.TypeName;
                row["Length"] = info.Length;
                row["Preci"] = info.Preci;
                row["Scale"] = info.Scale;
                row["IsIdentity"] = info.IsIdentity ? "√" : "";
                row["isPK"] = info.IsPK ? "√" : "";
                row["cisNull"] = info.cisNull ? "√" : "";
                row["defaultVal"] = info.DefaultVal;
                row["deText"] = info.DeText;
                table.Rows.Add(row);
            }
            return table;
        }

        public static List<ColumnInfo> GetColumnInfos(DataTable dt)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            if (dt == null)
            {
                return null;
            }
            foreach (DataRow row in dt.Rows)
            {
                string str = row["Colorder"].ToString();
                string str2 = row["ColumnName"].ToString();
                string str3 = row["TypeName"].ToString();
                string str4 = row["IsIdentity"].ToString();
                string str5 = row["IsPK"].ToString();
                string str6 = row["Length"].ToString();
                string str7 = row["Preci"].ToString();
                string str8 = row["Scale"].ToString();
                string str9 = row["cisNull"].ToString();
                string str10 = row["DefaultVal"].ToString();
                string str11 = row["DeText"].ToString();
                ColumnInfo item = new ColumnInfo();
                item.Colorder = str;
                item.ColumnName = str2;
                item.TypeName = str3;
                item.IsIdentity = str4 == "√";
                item.IsPK = str5 == "√";
                item.Length = str6;
                item.Preci = str7;
                item.Scale = str8;
                item.cisNull = str9 == "√";
                item.DefaultVal = str10;
                item.DeText = str11;
                list.Add(item);
            }
            return list;
        }

        public static string GetDataTypeLenVal(string datatype, string Length)
        {
            string str = "";
            switch (datatype.Trim())
            {
                case "int":
                    return "4";

                case "char":
                    if (!(Length.Trim() == ""))
                    {
                        return Length;
                    }
                    return "10";

                case "nchar":
                    str = Length;
                    if (Length.Trim() == "")
                    {
                        str = "10";
                    }
                    return str;

                case "varchar":
                    str = Length;
                    if (!(Length.Trim() == ""))
                    {
                        if (int.Parse(Length.Trim()) < 1)
                        {
                            str = "";
                        }
                        return str;
                    }
                    return "50";

                case "nvarchar":
                    str = Length;
                    if (!(Length.Trim() == ""))
                    {
                        if (int.Parse(Length.Trim()) < 1)
                        {
                            str = "";
                        }
                        return str;
                    }
                    return "50";

                case "varbinary":
                    str = Length;
                    if (!(Length.Trim() == ""))
                    {
                        if (int.Parse(Length.Trim()) < 1)
                        {
                            str = "";
                        }
                        return str;
                    }
                    return "50";

                case "bit":
                    return "1";

                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "binary":
                case "smallint":
                case "bigint":
                    return Length;

                case "image ":
                case "datetime":
                case "smalldatetime":
                case "ntext":
                case "text":
                    return str;
            }
            return Length;
        }

        public static string GetFieldstrlist(List<ColumnInfo> keys)
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in keys)
            {
                plus.Append(info.ColumnName + ",");
            }
            plus.DelLastComma();
            return plus.Value;
        }

        public static string GetFieldstrlistAdd(List<ColumnInfo> keys)
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in keys)
            {
                plus.Append(info.ColumnName + "+");
            }
            plus.DelLastChar("+");
            return plus.Value;
        }

        public static string GetInParameter(List<ColumnInfo> keys)
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in keys)
            {
                plus.Append(DbTypeToCS(info.TypeName) + " " + info.ColumnName + ",");
            }
            plus.DelLastComma();
            return plus.Value;
        }

        public static string GetModelWhereExpression(List<ColumnInfo> keys)
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in keys)
            {
                if (IsAddMark(info.TypeName))
                {
                    plus.Append(info.ColumnName + "='\"+ Entity." + info.ColumnName + "+\"' and ");
                }
                else
                {
                    plus.Append(info.ColumnName + "=\"+ Entity." + info.ColumnName + "+\" and ");
                }
            }
            plus.DelLastChar("and");
            return plus.Value;
        }

        public static string GetWhereExpression(List<ColumnInfo> keys)
        {
            StringPlus plus = new StringPlus();
            foreach (ColumnInfo info in keys)
            {
                if (IsAddMark(info.TypeName))
                {
                    plus.Append(info.ColumnName + "='\"+" + info.ColumnName + "+\"' and ");
                }
                else
                {
                    plus.Append(info.ColumnName + "=\"+" + info.ColumnName + "+\" and ");
                }
            }
            plus.DelLastChar("and");
            return plus.Value;
        }

        public static bool IsAddMark(string columnType)
        {
            //bool flag = false;
            //if (File.Exists(datatypefile))
            //{
            //    datatype = new INIFile(datatypefile);
            //    if (datatype.IniReadValue("IsAddMark", columnType.Trim().ToLower()) != "")
            //    {
            //        flag = true;
            //    }
            //}
            //return flag;
            try
            {
                string target = AddMaskMappingManager.Target(columnType);
                return target == "true" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public static bool isValueType(string cstype)
        {
            //bool flag = false;
            //if (File.Exists(datatypefile))
            //{
            //    datatype = new INIFile(datatypefile);
            //    if (datatype.IniReadValue("isValueType", cstype.Trim()) == "true")
            //    {
            //        flag = true;
            //    }
            //}
            //return flag;
            try
            {
                string target = ValueTypeMappingManager.Target(cstype);
                return target == "true" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public static string Space(int num)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                builder.Append("\t");
            }
            return builder.ToString();
        }

        public static string ToHexString(byte[] bytes)
        {
            char[] chArray = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int num2 = bytes[i];
                chArray[i * 2] = hexDigits[num2 >> 4];
                chArray[(i * 2) + 1] = hexDigits[num2 & 15];
            }
            string str = new string(chArray);
            return ("0x" + str.Substring(0, bytes.Length));
        }
    }


    public class CodeKey
    {
        // Fields
        private bool _isIdentity;
        private bool _isPK;
        private string _keyName;
        private string _keyType;

        // Properties
        public bool IsIdentity
        {
            get
            {
                return this._isIdentity;
            }
            set
            {
                this._isIdentity = value;
            }
        }

        public bool IsPK
        {
            get
            {
                return this._isPK;
            }
            set
            {
                this._isPK = value;
            }
        }

        public string KeyName
        {
            get
            {
                return this._keyName;
            }
            set
            {
                this._keyName = value;
            }
        }

        public string KeyType
        {
            get
            {
                return this._keyType;
            }
            set
            {
                this._keyType = value;
            }
        }
    }
    public class CodeKeyMaker
    {
        // Methods
        public static string GetParameter(List<CodeKey> keys)
        {
            StringPlus plus = new StringPlus();
            foreach (CodeKey key in keys)
            {
                plus.Append(CodeCommon.DbTypeToCS(key.KeyType) + " " + key.KeyName + ",");
            }
            plus.DelLastComma();
            return plus.Value;
        }
    }
    public class ColumnInfo
    {
        // Fields
        private bool _cisNull;
        private string _colorder;
        private string _columnName;
        private string _defaultVal;
        private string _deText;
        private bool _isIdentity;
        private bool _ispk;
        private string _length;
        private string _preci;
        private string _scale;
        private string _typeName;

        public override string ToString()
        {
            return  _typeName + ":" + _columnName;
        }
        // Properties
        public bool cisNull
        {
            get
            {
                return this._cisNull;
            }
            set
            {
                this._cisNull = value;
            }
        }

        public string Colorder
        {
            get
            {
                return this._colorder;
            }
            set
            {
                this._colorder = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        public string DefaultVal
        {
            get
            {
                return this._defaultVal;
            }
            set
            {
                this._defaultVal = value;
            }
        }

        public string DeText
        {
            get
            {
                return this._deText;
            }
            set
            {
                this._deText = value;
            }
        }

        public bool IsIdentity
        {
            get
            {
                return this._isIdentity;
            }
            set
            {
                this._isIdentity = value;
            }
        }

        public bool IsPK
        {
            get
            {
                return this._ispk;
            }
            set
            {
                this._ispk = value;
            }
        }

        public string Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
            }
        }

        public string Preci
        {
            get
            {
                return this._preci;
            }
            set
            {
                this._preci = value;
            }
        }

        public string Scale
        {
            get
            {
                return this._scale;
            }
            set
            {
                this._scale = value;
            }
        }

        public string TypeName
        {
            get
            {
                return this._typeName;
            }
            set
            {
                this._typeName = value;
            }
        }
    }
    public class TableInfo
    {
        // Fields
        private string _tabDate;
        private string _tabName;
        private string _tabType;
        private string _tabUser;

        public override string ToString()
        {
            return _tabType + "," + _tabName;
        }
        // Properties
        public string TabDate
        {
            get
            {
                return this._tabDate;
            }
            set
            {
                this._tabDate = value;
            }
        }

        public string TabName
        {
            get
            {
                return this._tabName;
            }
            set
            {
                this._tabName = value;
            }
        }

        public string TabType
        {
            get
            {
                return this._tabType;
            }
            set
            {
                this._tabType = value;
            }
        }

        public string TabUser
        {
            get
            {
                return this._tabUser;
            }
            set
            {
                this._tabUser = value;
            }
        }
    }

}
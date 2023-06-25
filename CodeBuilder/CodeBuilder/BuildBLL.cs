using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextronics.Applications.Library.Utility;

namespace Flextronics.Applications.Library.CodeBuilder
{
    public class BuilderBLL : IBuilderBLL
    {
        // Fields
        private string _bllpath;
        private string _dalspace;
        private string _factorypath;
        private List<ColumnInfo> _fieldlist;
        private string _iclass;
        private string _idalpath;
        protected string _key;
        private List<ColumnInfo> _keys;
        protected string _keyType;
        private string _modelname;
        private string _modelpath;
        private string _modelspace;
        private string _namespace;
        private string dbType;
        private bool isHasIdentity;

        // Methods
        public BuilderBLL()
        {
            this._key = "ID";
            this._keyType = "int";
        }

        public BuilderBLL(List<ColumnInfo> keys, string modelspace)
        {
            this._key = "ID";
            this._keyType = "int";
            this._modelspace = modelspace;
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

        public string CreatBLLADD()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 增加一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            string str = "void";
            if (((this.DbType == "SQL2000") || (this.DbType == "SQL2005")) && this.IsHasIdentity)
            {
                str = "int ";
            }
            //plus.AppendSpaceLine(2, "public " + str + " Add(" + this.ModelSpace + " model)");
            plus.AppendSpaceLine(2, "public " + str + " Add(" + ModelName+"Entity" + " model)");

            plus.AppendSpaceLine(2, "{");
            if (str == "void")
            {
                plus.AppendSpaceLine(3, "dal.Add(model);");
            }
            else
            {
                plus.AppendSpaceLine(3, "return dal.Add(model);");
            }
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string CreatBLLDelete()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 删除一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public void Delete(" + CodeCommon.GetInParameter(this.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, this.KeysNullTip);
            plus.AppendSpaceLine(3, "dal.Delete(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string CreatBLLExists()
        {
            StringPlus plus = new StringPlus();
            if (this._keys.Count > 0)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 是否存在该记录");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public bool Exists(" + CodeCommon.GetInParameter(this.Keys) + ")");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return dal.Exists(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
                plus.AppendSpaceLine(2, "}");
            }
            return plus.ToString();
        }

        public string CreatBLLGetAllList()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获得数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DataSet GetAllList()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return GetList(\"\");");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string CreatBLLGetList()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获得数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return dal.GetList(strWhere);");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获得数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            //ModelName+"Entity"      plus.AppendSpaceLine(2, "public List<" + this.ModelSpace + "> GetModelList(string strWhere)");
            plus.AppendSpaceLine(2, "public List<" + ModelName + "Entity" + "> GetModelList(string strWhere)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "DataSet ds = dal.GetList(strWhere);");
            //plus.AppendSpaceLine(3, "List<" + this.ModelSpace + "> modelList = new List<" + this.ModelSpace + ">();");
            plus.AppendSpaceLine(3, "List<" + ModelName + "Entity" + "> modelList = new List<" + ModelName + "Entity" + ">();");

            plus.AppendSpaceLine(3, "int rowsCount = ds.Tables[0].Rows.Count;");
            plus.AppendSpaceLine(3, "if (rowsCount > 0)");
            plus.AppendSpaceLine(3, "{");
            //plus.AppendSpaceLine(4, this.ModelSpace + " model;");
            plus.AppendSpaceLine(4, ModelName + "Entity" + " model;");
            plus.AppendSpaceLine(4, "for (int n = 0; n < rowsCount; n++)");
            plus.AppendSpaceLine(4, "{");
            //plus.AppendSpaceLine(5, "model = new " + this.ModelSpace + "();");
            plus.AppendSpaceLine(5, "model = new " + ModelName + "Entity" + "();");

            foreach (ColumnInfo info in this.Fieldlist)
            {
                string columnName = info.ColumnName;
                switch (CodeCommon.DbTypeToCS(info.TypeName))
                {
                    case "int":
                        {
                            plus.AppendSpaceLine(5, "if(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(5, "{");
                            plus.AppendSpaceLine(6, "model." + columnName + "=int.Parse(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(5, "}");
                            continue;
                        }
                    case "decimal":
                        {
                            plus.AppendSpaceLine(5, "if(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(5, "{");
                            plus.AppendSpaceLine(6, "model." + columnName + "=decimal.Parse(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(5, "}");
                            continue;
                        }
                    case "float":
                        {
                            plus.AppendSpaceLine(5, "if(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(5, "{");
                            plus.AppendSpaceLine(6, "model." + columnName + "=float.Parse(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(5, "}");
                            continue;
                        }
                    case "DateTime":
                        {
                            plus.AppendSpaceLine(5, "if(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(5, "{");
                            plus.AppendSpaceLine(6, "model." + columnName + "=DateTime.Parse(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(5, "}");
                            continue;
                        }
                    case "string":
                        {
                            plus.AppendSpaceLine(5, "model." + columnName + "=ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString();");
                            continue;
                        }
                    case "bool":
                        {
                            plus.AppendSpaceLine(5, "if(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(5, "{");
                            plus.AppendSpaceLine(6, "if((ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            plus.AppendSpaceLine(6, "{");
                            plus.AppendSpaceLine(6, "model." + columnName + "=true;");
                            plus.AppendSpaceLine(6, "}");
                            plus.AppendSpaceLine(6, "else");
                            plus.AppendSpaceLine(6, "{");
                            plus.AppendSpaceLine(7, "model." + columnName + "=false;");
                            plus.AppendSpaceLine(6, "}");
                            plus.AppendSpaceLine(5, "}");
                            continue;
                        }
                    case "byte[]":
                        {
                            plus.AppendSpaceLine(5, "if(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(5, "{");
                            plus.AppendSpaceLine(6, "model." + columnName + "=(byte[])ds.Tables[0].Rows[n][\"" + columnName + "\"];");
                            plus.AppendSpaceLine(5, "}");
                            continue;
                        }
                    case "Guid":
                        {
                            plus.AppendSpaceLine(5, "if(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            plus.AppendSpaceLine(5, "{");
                            plus.AppendSpaceLine(6, "model." + columnName + "=new Guid(ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            plus.AppendSpaceLine(5, "}");
                            continue;
                        }
                }
                plus.AppendSpaceLine(5, "//model." + columnName + "=ds.Tables[0].Rows[n][\"" + columnName + "\"].ToString();");
            }
            plus.AppendSpaceLine(5, "modelList.Add(model);");
            plus.AppendSpaceLine(4, "}");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(3, "return modelList;");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string CreatBLLGetListByPage()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获得数据列表");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return dal.GetList(PageSize,PageIndex,strWhere);");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string CreatBLLGetMaxID()
        {
            StringPlus plus = new StringPlus();
            if (this._keys.Count > 0)
            {
                foreach (ColumnInfo info in this._keys)
                {
                    if (CodeCommon.DbTypeToCS(info.TypeName) == "int")
                    {
                        string columnName = info.ColumnName;
                        if (info.IsPK)
                        {
                            plus.AppendLine("");
                            plus.AppendSpaceLine(2, "/// <summary>");
                            plus.AppendSpaceLine(2, "/// 得到最大ID");
                            plus.AppendSpaceLine(2, "/// </summary>");
                            plus.AppendSpaceLine(2, "public int GetMaxId()");
                            plus.AppendSpaceLine(2, "{");
                            plus.AppendSpaceLine(3, "return dal.GetMaxId();");
                            plus.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return plus.ToString();
        }

        public string CreatBLLGetModel()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 得到一个对象实体");
            plus.AppendSpaceLine(2, "/// </summary>");
            //plus.AppendSpaceLine(2, "public " + this.ModelSpace + " GetModel(" + CodeCommon.GetInParameter(this.Keys) + ")");
            plus.AppendSpaceLine(2, "public " + this.ModelName+"Entity" + " GetModel(" + CodeCommon.GetInParameter(this.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, this.KeysNullTip);
            plus.AppendSpaceLine(3, "return dal.GetModel(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string CreatBLLGetModelByCache(string ModelName)
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 得到一个对象实体，从缓存中。");
            plus.AppendSpaceLine(2, "/// </summary>");
            //  ModelName+"Entity"    plus.AppendSpaceLine(2, "public " + this.ModelSpace + " GetModelByCache(" + CodeCommon.GetInParameter(this.Keys) + ")");
            plus.AppendSpaceLine(2, "public " + ModelName + "Entity" + " GetModelByCache(" + CodeCommon.GetInParameter(this.Keys) + ")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, this.KeysNullTip);
            string str = "";
            if (this.Keys.Count > 0)
            {
                str = "+ " + CodeCommon.GetFieldstrlistAdd(this.Keys);
            }
            plus.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-\" " + str + ";");
            plus.AppendSpaceLine(3, "object objModel = LTP.Common.DataCache.GetCache(CacheKey);");
            plus.AppendSpaceLine(3, "if (objModel == null)");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "try");
            plus.AppendSpaceLine(4, "{");
            plus.AppendSpaceLine(5, "objModel = dal.GetModel(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
            plus.AppendSpaceLine(5, "if (objModel != null)");
            plus.AppendSpaceLine(5, "{");
            plus.AppendSpaceLine(6, "int ModelCache = LTP.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            plus.AppendSpaceLine(6, "LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            plus.AppendSpaceLine(5, "}");
            plus.AppendSpaceLine(4, "}");
            plus.AppendSpaceLine(4, "catch{}");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(3, "return (" + this.ModelSpace + ")objModel;");
            plus.AppendSpaceLine(2, "}");
            return plus.Value;
        }

        public string CreatBLLUpdate()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 更新一条数据");
            plus.AppendSpaceLine(2, "/// </summary>");
            //ModelName+"Entity"  plus.AppendSpaceLine(2, "public void Update(" + this.ModelSpace + " model)");
            plus.AppendSpaceLine(2, "public void Update(" + ModelName + "Entity" + " model)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "dal.Update(model);");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        public string GetBLLCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
        {
            StringPlus plus = new StringPlus();
            plus.AppendLine("using System;");
            plus.AppendLine("using System.Data;");
            plus.AppendLine("using System.Collections.Generic;");
            //if (GetModelByCache)
            //{
            //    plus.AppendLine("using LTP.Common;");
            //}
            plus.AppendLine("using " + this.Modelpath + ";");
            if ((this.Factorypath != "") && (this.Factorypath != null))
            {
                plus.AppendLine("using " + this.Factorypath + ";");
            }
            if ((this.IDALpath != "") && (this.IDALpath != null))
            {
                plus.AppendLine("using " + this.IDALpath + ";");
            }
            plus.AppendLine("namespace " + this.BLLpath);
            plus.AppendLine("{");
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 业务逻辑类" + this.ModelName + "Service");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "public class " + this.ModelName + "Service");
            plus.AppendSpaceLine(1, "{");
            //if ((this.DALSpace != "") && (this.DALSpace != null))
            //{
            //    plus.AppendSpaceLine(2, "private readonly " + this.DALSpace + " dal=new " + this.DALSpace + "();");
            //}
            //if ((this.IClass != "") && (this.IClass != null))
            //{
            //    plus.AppendSpaceLine(2, "private readonly " + this.IClass + " dal=DataAccess.Create" + this.ModelName + "();");
            //}
            plus.AppendSpaceLine(2, "private readonly " + this.ModelName+"DAL" + " dal=new " + this.ModelName + "DAL();");
            plus.AppendSpaceLine(2, "public " + this.ModelName + "Service()");
            plus.AppendSpaceLine(2, "{}");
            plus.AppendSpaceLine(2, "#region  成员方法");
            if (Maxid && (this.Keys.Count > 0))
            {
                foreach (ColumnInfo info in this.Keys)
                {
                    if ((CodeCommon.DbTypeToCS(info.TypeName) == "int") && info.IsPK)
                    {
                        plus.AppendLine(this.CreatBLLGetMaxID());
                        break;
                    }
                }
            }
            if (Exists)
            {
                plus.AppendLine(this.CreatBLLExists());
            }
            if (Add)
            {
                plus.AppendLine(this.CreatBLLADD());
            }
            if (Update)
            {
                plus.AppendLine(this.CreatBLLUpdate());
            }
            if (Delete)
            {
                plus.AppendLine(this.CreatBLLDelete());
            }
            if (GetModel)
            {
                plus.AppendLine(this.CreatBLLGetModel());
            }
            //if (GetModelByCache)
            //{
            //    plus.AppendLine(this.CreatBLLGetModelByCache(this.ModelName));
            //}
            if (List)
            {
                plus.AppendLine(this.CreatBLLGetList());
                plus.AppendLine(this.CreatBLLGetAllList());
                plus.AppendLine(this.CreatBLLGetListByPage());
            }
            plus.AppendSpaceLine(2, "#endregion  成员方法");
            plus.AppendSpaceLine(1, "}");
            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }

        // Properties
        public string BLLpath
        {
            get
            {
                return this._bllpath;
            }
            set
            {
                this._bllpath = value;
            }
        }

        public string DALSpace
        {
            get
            {
                return this._dalspace;
            }
            set
            {
                this._dalspace = value;
            }
        }

        public string DbType
        {
            get
            {
                return this.dbType;
            }
            set
            {
                this.dbType = value;
            }
        }

        public string Factorypath
        {
            get
            {
                return this._factorypath;
            }
            set
            {
                this._factorypath = value;
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
                return this.isHasIdentity;
            }
            set
            {
                this.isHasIdentity = value;
            }
        }

        public string Key
        {
            get
            {
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
                return this._key;
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

        private string KeysNullTip
        {
            get
            {
                if (this._keys.Count == 0)
                {
                    return "//该表无主键信息，请自定义主键/条件字段";
                }
                return "";
            }
        }

        public string ModelName
        {
            get
            {
                //return this._modelname+"Service"; //接口的地方放Service
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
    }

}

 



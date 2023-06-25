using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextronics.Applications.Library.Utility;


namespace Flextronics.Applications.Library.CodeBuilder
{
    public class BuilderModel : IBuilderModel
    {
     
        protected List<ColumnInfo> _fieldlist;
        protected string _modelname = "";
        protected string _modelpath = "";
        protected string _namespace = "Flextronics";

        public bool WCFSupport;

        public string CreatModel()
        {
            StringPlus plus = new StringPlus();
            plus.AppendLine("using System;");
            plus.AppendLine("namespace " + this.Modelpath);
            plus.AppendLine("{");
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + this._modelname + "");
           
            plus.AppendSpaceLine(1, "/// </summary>");
            //加上WCF的支持
            if(WCFSupport)
              plus.AppendSpaceLine(1, "[DataContract]");
            plus.AppendSpaceLine(1, "public class " + this._modelname);
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "public " + this._modelname + "() {    }  ");
            //plus.AppendSpaceLine(2, "{}");
            plus.AppendLine(this.CreatModelMethod());
            plus.AppendSpaceLine(1, "}");
            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }

        public string CreatModelMethod()
        {
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus(); //字段
            StringPlus plus3 = new StringPlus(); //属性
            //plus.AppendSpaceLine(2, "#region Model");
            foreach (ColumnInfo info in this.Fieldlist)
            {
                string columnName = info.ColumnName;
                string typeName = info.TypeName;
                bool isIdentity = info.IsIdentity;
                bool isPK = info.IsPK;
                bool cisNull = info.cisNull;
                string deText = info.DeText;
                typeName = CodeCommon.DbTypeToCS(typeName);
                string str4 = "";
                //Andy  这里判断是值类型的加?,表示可以为空
                //if ((CodeCommon.isValueType(typeName) && !isIdentity) && (!isPK && cisNull))
                //{
                //    str4 = "?";
                //}
                //end
                plus2.AppendSpaceLine(2, "private " + typeName + str4 + " _" + columnName.ToLower() + ";");


                //20090921 暂时去掉注释的生成,同时为支持WCF
                //plus3.AppendSpaceLine(2, "/// <summary>");
                //plus3.AppendSpaceLine(2, "/// " + deText);
                //plus3.AppendSpaceLine(2, "/// </summary>");
                if(WCFSupport)
                   plus3.AppendSpaceLine(2, "[DataMember]");
                
                plus3.AppendSpaceLine(2, "public " + typeName + str4 + " " + columnName);
                plus3.AppendSpaceLine(2, "{");
                plus3.AppendSpaceLine(3, "set{ _" + columnName.ToLower() + "=value;}");
                plus3.AppendSpaceLine(3, "get{return _" + columnName.ToLower() + ";}");
                plus3.AppendSpaceLine(2, "}");
            }
            plus.Append(plus2.Value);
            plus.Append(System.Environment.NewLine);
            plus.Append(plus3.Value);           
            return plus.ToString();
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
               // _modelpath = "BusinessEntity";
                return this._modelpath;
            }
            set
            {
                this._modelpath = value;
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


    public class BuilderModelT : BuilderModel
    {
        // Fields
        private string _modelnameson = "";

        // Methods
        public string CreatModelMethodT()
        {
            StringPlus plus = new StringPlus();
            plus.AppendLine(base.CreatModelMethod());
            plus.AppendSpaceLine(2, "private List<" + this.ModelNameSon + "> _" + this.ModelNameSon.ToLower() + "s;");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 子类 ");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public List<" + this.ModelNameSon + "> " + this.ModelNameSon + "s");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "set{ _" + this.ModelNameSon.ToLower() + "s=value;}");
            plus.AppendSpaceLine(3, "get{return _" + this.ModelNameSon.ToLower() + "s;}");
            plus.AppendSpaceLine(2, "}");
            return plus.ToString();
        }

        // Properties
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
    }

}
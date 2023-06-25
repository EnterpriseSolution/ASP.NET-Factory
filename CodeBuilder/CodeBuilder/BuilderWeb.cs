using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextronics.Applications.Library.Utility;


namespace Flextronics.Applications.Library.CodeBuilder
{
    public class BuilderWeb
{
    // Fields
    private string _bllspace = "";
    protected List<ColumnInfo> _fieldlist;
    private string _folder = "";
    protected string _key = "ID";
    protected List<ColumnInfo> _keys;
    protected string _keyType = "int";
    protected string _modelname;
    private string _modelspace = "";
    protected string _namespace = "Maticsoft";

    // Methods
    public string CreatDeleteForm()
    {
        StringPlus plus = new StringPlus();
        plus.AppendSpaceLine(1, "if(!Page.IsPostBack)");
        plus.AppendSpaceLine(1, "{");
        plus.AppendSpaceLine(2, this.BLLSpace + " bll=new " + this.BLLSpace + "();");
        switch (this._keyType.Trim())
        {
            case "int":
            case "smallint":
            case "float":
            case "numeric":
            case "decimal":
            case "datetime":
            case "smalldatetime":
                plus.AppendSpaceLine(2, this._keyType + " " + this._key + "=" + this._keyType + ".Parse(Request.Params[\"id\"]);");
                break;

            default:
                plus.AppendSpaceLine(2, "string " + this._key + "=Request.Params[\"id\"];");
                break;
        }
        plus.AppendSpaceLine(2, "bll.Delete(" + this._key + ");");
        plus.AppendSpaceLine(2, "Response.Redirect(\"index.aspx\");");
        plus.AppendSpaceLine(1, "}");
        return plus.Value;
    }

    public string CreatSearchForm()
    {
        StringPlus plus = new StringPlus();
        return plus.Value;
    }

    public string GetAddAspx()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine();
        plus.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            if (isPK || isIdentity)
            {
                continue;
            }
            if (deText.Trim() == "")
            {
                deText = columnName;
            }
            plus.AppendSpaceLine(1, "<tr>");
            plus.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
            plus.AppendSpaceLine(2, deText);
            plus.AppendSpaceLine(1, "</td>");
            plus.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
            string str4 = typeName.Trim().ToLower();
            if (str4 == null)
            {
                goto Label_0160;
            }
            if (!(str4 == "datetime") && !(str4 == "smalldatetime"))
            {
                if (str4 == "bit")
                {
                    goto Label_0121;
                }
                goto Label_0160;
            }
            plus.AppendSpaceLine(2, "<INPUT onselectstart=\"return false;\" onkeypress=\"return false\" id=\"txt" + columnName + "\" onfocus=\"setday(this)\"");
            plus.AppendSpaceLine(2, " readOnly type=\"text\" size=\"10\" name=\"Text1\" runat=\"server\">");
            goto Label_0178;
        Label_0121:;
            plus.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");
            goto Label_0178;
        Label_0160:
            plus.AppendSpaceLine(2, "<asp:TextBox id=\"txt" + columnName + "\" runat=\"server\" Width=\"200px\"></asp:TextBox>");
        Label_0178:
            plus.AppendSpaceLine(1, "</td></tr>");
        }
        plus.AppendSpaceLine(1, "<tr>");
        plus.AppendSpaceLine(1, "<td height=\"25\" colspan=\"2\"><div align=\"center\">");
        plus.AppendSpaceLine(2, "<asp:Button ID=\"btnAdd\" runat=\"server\" Text=\"\x00b7 提交 \x00b7\" OnClick=\"btnAdd_Click\" ></asp:Button>");
        plus.AppendSpaceLine(2, "<asp:Button ID=\"btnCancel\" runat=\"server\" Text=\"\x00b7 重填 \x00b7\" OnClick=\"btnCancel_Click\" ></asp:Button>");
        plus.AppendSpaceLine(1, "</div></td></tr>");
        plus.AppendLine("</table>");
        return plus.ToString();
    }

    public string GetAddAspxCs()
    {
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        StringPlus plus3 = new StringPlus();
        StringPlus plus4 = new StringPlus();
        plus.AppendLine();
        //ANDY 不需要这些代码，CS/BS各种验证都不相同
        //plus.AppendSpaceLine(1, "string strErr=\"\";");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            #region 
            if (!isPK && !isIdentity)
            {
                switch (CodeCommon.DbTypeToCS(typeName.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                        plus2.AppendSpaceLine(1, "int " + columnName + "=int.Parse(txt" + columnName + ".Text);");
                        plus3.AppendSpaceLine(1, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(2, "strErr+=\"" + columnName + "不是数字！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;

                    case "float":
                    case "numeric":
                    case "decimal":
                        plus2.AppendSpaceLine(1, "decimal " + columnName + "=decimal.Parse(txt" + columnName + ".Text);");
                        plus3.AppendSpaceLine(1, "if(!PageValidate.IsDecimal(txt" + columnName + ".Text))");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(2, "strErr+=\"" + columnName + "不是数字！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;

                    case "datetime":
                    case "smalldatetime":
                        plus2.AppendSpaceLine(1, "DateTime " + columnName + "=DateTime.Parse(txt" + columnName + ".Text);");
                        plus3.AppendSpaceLine(1, "if(!PageValidate.IsDateTime(txt" + columnName + ".Text))");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(1, "strErr+=\"" + columnName + "不是时间格式！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;

                    case "bool":
                        plus2.AppendSpaceLine(1, "bool " + columnName + "=chk" + columnName + ".Checked;");
                        break;

                    case "byte[]":
                        plus2.AppendSpaceLine(1, "byte[] " + columnName + "= new UnicodeEncoding().GetBytes(txt" + columnName + ".Text);");
                        break;

                    default:
                        plus2.AppendSpaceLine(1, "string " + columnName + "=txt" + columnName + ".Text;");
                        plus3.AppendSpaceLine(1, "if(txt" + columnName + ".Text ==\"\")");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(2, "strErr+=\"" + columnName + "不能为空！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;
                }
                plus4.AppendSpaceLine(1, "entity." + columnName + "=" + columnName + ";");
            }
            #endregion 
        }
        //plus.AppendLine(plus3.ToString());
        //plus.AppendSpaceLine(1, "if(strErr!=\"\")");
        //plus.AppendSpaceLine(1, "{");
        //plus.AppendSpaceLine(2, "MessageBox.Show(this,strErr);");
        //plus.AppendSpaceLine(2, "return;");
        //plus.AppendSpaceLine(1, "}");

        plus.AppendLine(plus2.ToString());
        plus.AppendSpaceLine(1, this.ModelSpace + "  entity=new " + this.ModelSpace + "();");
      
        plus.AppendLine(plus4.ToString());
        plus.AppendSpaceLine(1, this.BLLSpace + " bll=new " + this.BLLSpace + "();");
        plus.AppendSpaceLine(1, "bll.Add(entity);");
        return plus.Value;
    }

    public string GetAddDesigner()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine();
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            if (!isPK && !isIdentity)
            {
                string str3 = CodeCommon.DbTypeToCS(typeName.Trim().ToLower()).ToLower();
                if (str3 == null)
                {
                    goto Label_00D4;
                }
                if (!(str3 == "datetime") && !(str3 == "smalldatetime"))
                {
                    if (str3 == "bool")
                    {
                        goto Label_00BA;
                    }
                    goto Label_00D4;
                }
                plus.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
            }
            continue;
        Label_00BA:
            plus.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
            continue;
        Label_00D4:
            plus.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
        }
        plus.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnAdd;");
        plus.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
        return plus.ToString();
    }

    public string GetShowAspx()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine();
        plus.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string str4;
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            if (deText.Trim() == "")
            {
                deText = columnName;
            }
            plus.AppendSpaceLine(1, "<tr>");
            plus.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
            plus.AppendSpaceLine(2, deText);
            plus.AppendSpaceLine(1, "</td>");
            plus.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
            if (((str4 = typeName.Trim()) != null) && (str4 == "bit"))
            {
                plus.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");
            }
            else
            {
                plus.AppendSpaceLine(2, "<asp:Label id=\"lbl" + columnName + "\" runat=\"server\"></asp:Label>");
            }
            plus.AppendSpaceLine(1, "</td></tr>");
        }
        plus.AppendLine("</table>");
        return plus.ToString();
    }

    public string GetShowAspxCs()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine();
        string key = this.Key;
        plus.AppendSpaceLine(1, "private void ShowInfo(" + CodeCommon.GetInParameter(this.Keys) + ")");
        plus.AppendSpaceLine(1, "{");
        plus.AppendSpaceLine(2, this.BLLSpace + " bll=new " + this.BLLSpace + "();");
        plus.AppendSpaceLine(2, this.ModelSpace + " model=bll.GetModel(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            if (!isPK && !isIdentity)
            {
                switch (CodeCommon.DbTypeToCS(typeName.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                    case "float":
                    case "numeric":
                    case "decimal":
                    case "datetime":
                    case "smalldatetime":
                    {
                        plus.AppendSpaceLine(2, "lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        continue;
                    }
                    case "bool":
                    {
                        plus.AppendSpaceLine(2, "chk" + columnName + ".Checked=model." + columnName + ";");
                        continue;
                    }
                    case "byte[]":
                    {
                        plus.AppendSpaceLine(2, "lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        continue;
                    }
                }
                plus.AppendSpaceLine(2, "lbl" + columnName + ".Text=model." + columnName + ";");
            }
        }
        plus.AppendLine();
        plus.AppendSpaceLine(1, "}");
        return plus.ToString();
    }

    public string GetShowDesigner()
    {
        StringPlus plus = new StringPlus();
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string str4;
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            if (info.DeText.Trim() == "")
            {
                string str3 = columnName;
            }
            if (((str4 = CodeCommon.DbTypeToCS(typeName.Trim().ToLower()).ToLower()) != null) && (str4 == "bool"))
            {
                plus.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
            }
            else
            {
                plus.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Label lbl" + columnName + ";");
            }
        }
        return plus.ToString();
    }

    public string GetUpdateAspx()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine("");
        plus.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            if (deText.Trim() == "")
            {
                deText = columnName;
            }
            if (isPK || isIdentity)
            {
                plus.AppendSpaceLine(1, "<tr>");
                plus.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
                plus.AppendSpaceLine(2, deText);
                plus.AppendSpaceLine(1, "</td>");
                plus.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
                plus.AppendSpaceLine(2, "<asp:label id=\"lbl" + columnName + "\" runat=\"server\"></asp:label>");
                plus.AppendSpaceLine(1, "</td></tr>");
                continue;
            }
            plus.AppendSpaceLine(1, "<tr>");
            plus.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\">");
            plus.AppendSpaceLine(2, deText);
            plus.AppendSpaceLine(1, "</td>");
            plus.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\">");
            string str4 = typeName.Trim();
            if (str4 == null)
            {
                goto Label_01C2;
            }
            if (!(str4 == "datetime") && !(str4 == "smalldatetime"))
            {
                if (str4 == "bit")
                {
                    goto Label_0183;
                }
                goto Label_01C2;
            }
            plus.AppendSpaceLine(2, "<INPUT onselectstart=\"return false;\" onkeypress=\"return false\" id=\"txt" + columnName + "\" onfocus=\"setday(this)\"");
            plus.AppendSpaceLine(2, " readOnly type=\"text\" size=\"10\" name=\"Text1\" runat=\"server\">");
            goto Label_01DA;
        Label_0183:;
            plus.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");
            goto Label_01DA;
        Label_01C2:
            plus.AppendSpaceLine(2, "<asp:TextBox id=\"txt" + columnName + "\" runat=\"server\" Width=\"200px\"></asp:TextBox>");
        Label_01DA:
            plus.AppendSpaceLine(1, "</td></tr>");
        }
        plus.AppendSpaceLine(1, "<tr>");
        plus.AppendSpaceLine(1, "<td height=\"25\" colspan=\"2\"><div align=\"center\">");
        plus.AppendSpaceLine(2, "<asp:Button ID=\"btnAdd\" runat=\"server\" Text=\"\x00b7 提交 \x00b7\" OnClick=\"btnAdd_Click\" ></asp:Button>");
        plus.AppendSpaceLine(2, "<asp:Button ID=\"btnCancel\" runat=\"server\" Text=\"\x00b7 取消 \x00b7\" OnClick=\"btnCancel_Click\" ></asp:Button>");
        plus.AppendSpaceLine(1, "</div></td></tr>");
        plus.AppendLine("</table>");
        return plus.Value;
    }

    public string GetUpdateAspxCs()
    {
        StringPlus plus = new StringPlus();
        StringPlus plus2 = new StringPlus();
        StringPlus plus3 = new StringPlus();
        StringPlus plus4 = new StringPlus();
        plus.AppendLine();
        //plus.AppendSpaceLine(1, "string strErr=\"\";");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            if (!isPK && !isIdentity)
            {
                switch (CodeCommon.DbTypeToCS(typeName.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                        plus2.AppendSpaceLine(1, "int " + columnName + "=int.Parse(txt" + columnName + ".Text);");
                        plus3.AppendSpaceLine(1, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(2, "strErr+=\"" + columnName + "不是数字！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;

                    case "float":
                    case "numeric":
                    case "decimal":
                        plus2.AppendSpaceLine(1, "decimal " + columnName + "=decimal.Parse(txt" + columnName + ".Text);");
                        plus3.AppendSpaceLine(1, "if(!PageValidate.IsDecimal(txt" + columnName + ".Text))");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(2, "strErr+=\"" + columnName + "不是数字！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;

                    case "datetime":
                    case "smalldatetime":
                        plus2.AppendSpaceLine(1, "DateTime " + columnName + "=DateTime.Parse(txt" + columnName + ".Text);");
                        plus3.AppendSpaceLine(1, "if(!PageValidate.IsDateTime(txt" + columnName + ".Text))");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(2, "strErr+=\"" + columnName + "不是时间格式！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;

                    case "bool":
                        plus2.AppendSpaceLine(1, "bool " + columnName + "=chk" + columnName + ".Checked;");
                        break;

                    case "byte[]":
                        plus2.AppendSpaceLine(1, "byte[] " + columnName + "= new UnicodeEncoding().GetBytes(txt" + columnName + ".Text);");
                        break;

                    default:
                        plus2.AppendSpaceLine(1, "string " + columnName + "=txt" + columnName + ".Text;");
                        plus3.AppendSpaceLine(1, "if(txt" + columnName + ".Text ==\"\")");
                        plus3.AppendSpaceLine(1, "{");
                        plus3.AppendSpaceLine(2, "strErr+=\"" + columnName + "不能为空！\\\\n\";\t");
                        plus3.AppendSpaceLine(1, "}");
                        break;
                }
                plus4.AppendSpaceLine(1, "model." + columnName + "=" + columnName + ";");
            }
        }
        //plus.AppendLine(plus3.ToString());
        //plus.AppendSpaceLine(1, "if(strErr!=\"\")");
        //plus.AppendSpaceLine(1, "{");
        //plus.AppendSpaceLine(2, "MessageBox.Show(this,strErr);");
        //plus.AppendSpaceLine(2, "return;");
        //plus.AppendSpaceLine(1, "}");
        plus.AppendLine(plus2.ToString());
        plus.AppendLine();
        plus.AppendSpaceLine(1, this.ModelSpace + " model=new " + this.ModelSpace + "();");
        plus.AppendLine(plus4.ToString());
        plus.AppendSpaceLine(1, this.BLLSpace + " bll=new " + this.BLLSpace + "();");
        plus.AppendSpaceLine(1, "bll.Update(model);");
        return plus.ToString();
    }

    public string GetUpdateDesigner()
    {
        StringPlus plus = new StringPlus();
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            if (deText.Trim() == "")
            {
                deText = columnName;
            }
            if (isPK || isIdentity)
            {
                plus.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Label lbl" + columnName + ";");
            }
            else
            {
                string str4 = CodeCommon.DbTypeToCS(typeName.Trim().ToLower()).ToLower();
                if (str4 == null)
                {
                    goto Label_00FB;
                }
                if (!(str4 == "datetime") && !(str4 == "smalldatetime"))
                {
                    if (str4 == "bool")
                    {
                        goto Label_00E1;
                    }
                    goto Label_00FB;
                }
                plus.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
            }
            continue;
        Label_00E1:
            plus.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
            continue;
        Label_00FB:
            plus.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
        }
        plus.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnAdd;");
        plus.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
        return plus.Value;
    }

    public string GetUpdateShowAspxCs()
    {
        StringPlus plus = new StringPlus();
        plus.AppendLine();
        string key = this.Key;
        plus.AppendSpaceLine(1, "private void ShowInfo(" + CodeCommon.GetInParameter(this.Keys) + ")");
        plus.AppendSpaceLine(1, "{");
        plus.AppendSpaceLine(2, this.BLLSpace + " bll=new " + this.BLLSpace + "();");
        plus.AppendSpaceLine(2, this.ModelSpace + " entity=bll.GetModel(" + CodeCommon.GetFieldstrlist(this.Keys) + ");");
        foreach (ColumnInfo info in this.Fieldlist)
        {
            string columnName = info.ColumnName;
            string typeName = info.TypeName;
            string deText = info.DeText;
            bool isPK = info.IsPK;
            bool isIdentity = info.IsIdentity;
            switch (CodeCommon.DbTypeToCS(typeName.Trim().ToLower()).ToLower())
            {
                case "int":
                case "smallint":
                case "float":
                case "numeric":
                case "decimal":
                case "datetime":
                case "smalldatetime":
                {
                    if (!isPK && !isIdentity)
                    {
                        break;
                    }
                    plus.AppendSpaceLine(2, "lbl" + columnName + ".Text=entity." + columnName + ".ToString();");
                    continue;
                }
                case "bool":
                {
                    plus.AppendSpaceLine(2, "chk" + columnName + ".Checked=entity." + columnName + ";");
                    continue;
                }
                case "byte[]":
                {
                    plus.AppendSpaceLine(2, "txt" + columnName + ".Text=entity." + columnName + ".ToString();");
                    continue;
                }
                default:
                    goto Label_02BE;
            }
            plus.AppendSpaceLine(2, "txt" + columnName + ".Text=entity." + columnName + ".ToString();");
            continue;
        Label_02BE:
            if (isPK || isIdentity)
            {
                plus.AppendSpaceLine(2, "lbl" + columnName + ".Text=entity." + columnName + ";");
                continue;
            }
            plus.AppendSpaceLine(2, "txt" + columnName + ".Text=entity." + columnName + ";");
        }
        plus.AppendLine();
        plus.AppendSpaceLine(1, "}");
        return plus.Value;
    }

    public string GetWebCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
    {
        StringPlus plus = new StringPlus();
        if (AddForm)
        {
            plus.AppendLine("  /******************************增加窗体代码********************************/");
            plus.AppendLine(this.GetAddAspxCs());
        }
        if (UpdateForm)
        {
            plus.AppendLine("  /******************************修改窗体代码********************************/");
            plus.AppendLine("  /*修改代码-显示 */");
            plus.AppendLine(this.GetUpdateShowAspxCs());
            plus.AppendLine("  /*修改代码-提交更新 */");
            plus.AppendLine(this.GetUpdateAspxCs());
        }
        if (ShowForm)
        {
            plus.AppendLine("  /******************************显示窗体代码********************************/");
            plus.AppendLine(this.GetShowAspxCs());
        }
        return plus.Value;
    }

    public string GetWebHtmlCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
    {
        StringPlus plus = new StringPlus();
        if (AddForm)
        {
            plus.AppendLine(" <!--******************************增加页面代码********************************-->");
            plus.AppendLine(this.GetAddAspx());
        }
        if (UpdateForm)
        {
            plus.AppendLine(" <!--******************************修改页面代码********************************-->");
            plus.AppendLine(this.GetUpdateAspx());
        }
        if (ShowForm)
        {
            plus.AppendLine("  <!--******************************显示页面代码********************************-->");
            plus.AppendLine(this.GetShowAspx());
        }
        return plus.ToString();
    }

    // Properties
    private string BLLSpace
    {
        get
        {
            //this._bllspace = this._namespace + ".BLL";
            //if (this._folder.Trim() != "")
            //{
            //    this._bllspace = this._bllspace + "." + this._folder;
            //}
            //this._bllspace = this._bllspace + "." + this._modelname;
            //return this._bllspace;
            return _modelname + "DAL"; //或DAL //Service
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

    protected string Key
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

    public string ModelSpace
    {
        get
        {
            //this._modelspace = this._namespace + ".Model";
            //if (this._folder.Trim() != "")
            //{
            //    this._modelspace = this._modelspace + "." + this._folder;
            //}
            //this._modelspace = this._modelspace + "." + this._modelname;
            //return this._modelspace;
            return _modelname + "Entity";
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
using System;

namespace DbDataTypeMapping
{
	/// <summary>
	/// DbDataTypeMapping 的摘要说明。
	/// </summary>
	public class TypeMapping
	{
		#region private fields
		private string _columnName	=string.Empty;
		private string _dbTypeName	=string.Empty;
		private string _dotNetTypeName	=string.Empty;
		#endregion

		#region constructor
		public TypeMapping()
		{
		}
		#endregion

		#region properties
		/// <summary>
		/// 列名
		/// </summary>
		public string ColumnName
		{
			get{return this._columnName;}
			set{this._columnName=value;}
		}
		/// <summary>
		/// 数据库类型名
		/// </summary>
		public string DbTypeName
		{
			get{return this._dbTypeName;}
			set{this._dbTypeName=value;}
		}
		/// <summary>
		/// 映射到.Net中的类型名
		/// </summary>
		public string DotNetTypeName
		{
			get{return this._dotNetTypeName;}
			set{this._dotNetTypeName=value;}
		}
		#endregion
	}
}

using System;

namespace DbDataTypeMapping
{
	/// <summary>
	/// Table 的摘要说明。
	/// </summary>
	public class Table
	{
		#region private fields
		private string _tableName	=string.Empty;
		private int _id	=0;
		#endregion

		#region constructor
		public Table()
		{
		}
		#endregion

		#region properties
		/// <summary>
		/// 数据库表名
		/// </summary>
		public string TableName
		{
			get{return this._tableName;}
			set{this._tableName=value;}
		}
		/// <summary>
		/// 数据库对象ID
		/// </summary>
		public int Id
		{
			get{return this._id;}
			set{this._id=value;}
		}
		/// <summary>
		/// 重写ToString()方法
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this._tableName;
		}

		#endregion
	}
}

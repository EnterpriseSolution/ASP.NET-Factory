using System;

namespace DbDataTypeMapping
{
	/// <summary>
	/// Table ��ժҪ˵����
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
		/// ���ݿ����
		/// </summary>
		public string TableName
		{
			get{return this._tableName;}
			set{this._tableName=value;}
		}
		/// <summary>
		/// ���ݿ����ID
		/// </summary>
		public int Id
		{
			get{return this._id;}
			set{this._id=value;}
		}
		/// <summary>
		/// ��дToString()����
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this._tableName;
		}

		#endregion
	}
}

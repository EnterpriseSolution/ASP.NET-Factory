using System;

namespace DbDataTypeMapping
{
	/// <summary>
	/// DbDataTypeMapping ��ժҪ˵����
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
		/// ����
		/// </summary>
		public string ColumnName
		{
			get{return this._columnName;}
			set{this._columnName=value;}
		}
		/// <summary>
		/// ���ݿ�������
		/// </summary>
		public string DbTypeName
		{
			get{return this._dbTypeName;}
			set{this._dbTypeName=value;}
		}
		/// <summary>
		/// ӳ�䵽.Net�е�������
		/// </summary>
		public string DotNetTypeName
		{
			get{return this._dotNetTypeName;}
			set{this._dotNetTypeName=value;}
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;

namespace Pulsar.SQL
{
	/// <summary>
	/// ����� ����������� ������ SQL ������ (������� SqlDataReader).
	/// </summary>
	public sealed class PulsarSqlReader : DynamicObject, IPulsarDataReaderViewer, IDataReader   //
	{
		internal SqlDataReader r = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ���������� �������� ���� ������. ���� �������� DBNull, ���������� null.
		/// </summary>
		/// <param name="valueName">��� ���� ������.</param>
		/// <returns></returns>
		public object this[string valueName]
		{
			get 
			{
				return this[r.GetOrdinal(valueName)];
			}
		}
		/// <summary>
		/// ���������� �������� ���� ������. ���� �������� DBNull, ���������� null.
		/// </summary>
		/// <param name="valueOrdinal">���������� ����� ���� ������.</param>
		/// <returns></returns>
		public object this[int valueOrdinal]
		{
			get 
			{
				if(r.GetDataTypeName(valueOrdinal).ToLower() == "timestamp")
					return r.IsDBNull(valueOrdinal) ? SqlTimeStamp.Empty : new SqlTimeStamp((byte[])r[valueOrdinal]);
				return r.IsDBNull(valueOrdinal) ? null : r[valueOrdinal];
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, �������� �� PulsarSqlReader ������.
		/// </summary>
		public bool HasRows
		{
			get { return r.HasRows; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ����� ������.
		/// </summary>
		public int FieldCount
		{
			get { return r.FieldCount; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ������� ����� ������.
		/// </summary>
		public int VisibleFieldCount
		{
			get { return r.VisibleFieldCount; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ ��������� ������ ������.
		/// </summary>
		public IPulsarDataReaderViewer Viewer
		{
			get { return (IPulsarDataReaderViewer)this; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, �������� �� ����� ��������.
		/// </summary>
		public bool IsClosed
		{
			get { return r.IsClosed; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		private PulsarSqlReader () { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="r">SqlDataReader���������� ������.</param>
		public PulsarSqlReader(SqlDataReader r)
		{
			this.r = r;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ��������� �������� ���� ������ �� NULL.
		/// </summary>
		/// <param name="valueName">��� ���� ������.</param>
		/// <returns></returns>
		public bool IsDBNull(string valueName)
		{
			return r.IsDBNull(r.GetOrdinal(valueName));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� �������� ���� ������ �� NULL.
		/// </summary>
		/// <param name="ordinal">���������� ����� ����.</param>
		/// <returns></returns>
		public bool IsDBNull(int ordinal)
		{
			return r.IsDBNull(ordinal);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������ ��������� ������ �� ������.
		/// </summary>
		/// <returns></returns>
		public bool Read()
		{
			return r.Read();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� �����.
		/// </summary>
		public void Close()
		{
			r.Close();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ��� ���� �� ��� ����������� ������.
		/// </summary>
		/// <param name="ordinal">���������� ����� ����.</param>
		/// <returns>��� ����.</returns>
		public string GetName(int ordinal)
		{
			return r.GetName(ordinal);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ��� ����.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Type GetFieldType(int i)
		{
			return r.GetFieldType(i);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// �������� ��������� ������������ ����� �����.
		/// </summary>
		/// <returns></returns>
		public bool NextResult()
		{
			return r.NextResult();
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << Operators >>
		/// <summary>
		/// ����������� � ���� SqlDataReader.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static implicit operator SqlDataReader(PulsarSqlReader r)
		{
			return r.r;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����������� �� ���� SqlDataReader.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static implicit operator PulsarSqlReader(SqlDataReader r)
		{
			return new PulsarSqlReader(r);
		}
		#endregion << Operators >>
		//-------------------------------------------------------------------------------------
		#region IDataReader Members
		int IDataReader.Depth
		{
			get { return r.Depth; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataTable GetSchemaTable()
		{
			return r.GetSchemaTable();
		}
		//-------------------------------------------------------------------------------------
		int IDataReader.RecordsAffected
		{
			get { return r.RecordsAffected; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			if(r == null)
				return;
			if(r.IsClosed == false)
				r.Close();
			r.Dispose();
			r = null;
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IDataRecord Members
		bool IDataRecord.GetBoolean(int i)
		{
			return r.GetBoolean(i);
		}
		//-------------------------------------------------------------------------------------
		byte IDataRecord.GetByte(int i)
		{
			return r.GetByte(i);
		}
		//-------------------------------------------------------------------------------------
		long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return r.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}
		//-------------------------------------------------------------------------------------
		char IDataRecord.GetChar(int i)
		{
			return r.GetChar(i);
		}
		//-------------------------------------------------------------------------------------
		long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return r.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}
		//-------------------------------------------------------------------------------------
		IDataReader IDataRecord.GetData(int i)
		{
			return r.GetData(i);
		}
		//-------------------------------------------------------------------------------------
		string IDataRecord.GetDataTypeName(int i)
		{
			return r.GetDataTypeName(i);
		}
		//-------------------------------------------------------------------------------------
		DateTime IDataRecord.GetDateTime(int i)
		{
			return r.GetDateTime(i);
		}
		//-------------------------------------------------------------------------------------
		decimal IDataRecord.GetDecimal(int i)
		{
			return r.GetDecimal(i);
		}
		//-------------------------------------------------------------------------------------
		double IDataRecord.GetDouble(int i)
		{
			return r.GetDouble( i);
		}
		//-------------------------------------------------------------------------------------
		float IDataRecord.GetFloat(int i)
		{
			return r.GetFloat(i);
		}
		//-------------------------------------------------------------------------------------
		Guid IDataRecord.GetGuid(int i)
		{
			return r.GetGuid(i);
		}
		//-------------------------------------------------------------------------------------
		short IDataRecord.GetInt16(int i)
		{
			return r.GetInt16(i);
		}
		//-------------------------------------------------------------------------------------
		int IDataRecord.GetInt32(int i)
		{
			return r.GetInt32(i);
		}
		//-------------------------------------------------------------------------------------
		long IDataRecord.GetInt64(int i)
		{
			return r.GetInt64(i);
		}
		//-------------------------------------------------------------------------------------
		int IDataRecord.GetOrdinal(string name)
		{
			return r.GetOrdinal(name);
		}
		//-------------------------------------------------------------------------------------
		string IDataRecord.GetString(int i)
		{
			return r.GetString(i);
		}
		//-------------------------------------------------------------------------------------
		object IDataRecord.GetValue(int i)
		{
			return r.GetValue(i);
		}
		//-------------------------------------------------------------------------------------
		int IDataRecord.GetValues(object[] values)
		{
			return r.GetValues(values);
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region << Dynamic Methods >>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = this[binder.Name];
			return true;
		}
		#endregion << Dynamic Methods >>
		//-------------------------------------------------------------------------------------
						
	}
	//**************************************************************************************
	#region << public interface IPulsarDataReaderViewer >>
	/// <summary>
	/// ��������� ��������� ������ ������ PulsarSqlReader.
	/// </summary>
	public interface IPulsarDataReaderViewer
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ���������� �������� ���� ������.
		/// </summary>
		/// <param name="valueName">��� ���� ������.</param>
		/// <returns></returns>
		object this[string valueName] { get; }
		/// <summary>
		/// ���������� �������� ���� ������.
		/// </summary>
		/// <param name="valueOrdinal">���������� ����� ���� ������.</param>
		/// <returns></returns>
		object this[int valueOrdinal] { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, �������� �� PulsarSqlReader ������.
		/// </summary>
		bool HasRows { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ����� ������.
		/// </summary>
		int FieldCount { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ������� ����� ������.
		/// </summary>
		int VisibleFieldCount { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, �������� �� ����� ��������.
		/// </summary>
		bool IsClosed { get; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ��������� �������� ���� ������ �� NULL.
		/// </summary>
		/// <param name="valueName">��� ���� ������.</param>
		/// <returns></returns>
		bool IsDBNull(string valueName);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� �������� ���� ������ �� NULL.
		/// </summary>
		/// <param name="ordinal">���������� ����� ����.</param>
		/// <returns></returns>
		bool IsDBNull(int ordinal);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ��� ���� �� ��� ����������� ������.
		/// </summary>
		/// <param name="ordinal">���������� ����� ����.</param>
		/// <returns>��� ����.</returns>
		string GetName(int ordinal);
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public interface IPulsarDataReaderViewer >>

}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;

namespace Pulsar.SQL
{
	/// <summary>
	/// Класс построчного чтения SQL данных (враппер SqlDataReader).
	/// </summary>
	public sealed class PulsarSqlReader : DynamicObject, IPulsarDataReaderViewer, IDataReader   //
	{
		internal SqlDataReader r = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает значение поля строки. Если значение DBNull, возвращает null.
		/// </summary>
		/// <param name="valueName">Имя поля строки.</param>
		/// <returns></returns>
		public object this[string valueName]
		{
			get 
			{
				return this[r.GetOrdinal(valueName)];
			}
		}
		/// <summary>
		/// Возвращает значение поля строки. Если значение DBNull, возвращает null.
		/// </summary>
		/// <param name="valueOrdinal">Порядковый номер поля строки.</param>
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
		/// Определяет, содержит ли PulsarSqlReader строки.
		/// </summary>
		public bool HasRows
		{
			get { return r.HasRows; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает количесто полей ридера.
		/// </summary>
		public int FieldCount
		{
			get { return r.FieldCount; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает количесто видимых полей ридера.
		/// </summary>
		public int VisibleFieldCount
		{
			get { return r.VisibleFieldCount; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает объект просмотра данных ридера.
		/// </summary>
		public IPulsarDataReaderViewer Viewer
		{
			get { return (IPulsarDataReaderViewer)this; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, является ли ридер закрытым.
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
		/// Конструктор по умолчанию.
		/// </summary>
		private PulsarSqlReader () { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="r">SqlDataReaderсодержащий строку.</param>
		public PulsarSqlReader(SqlDataReader r)
		{
			this.r = r;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Проверяет значение поля строки на NULL.
		/// </summary>
		/// <param name="valueName">Имя поля строки.</param>
		/// <returns></returns>
		public bool IsDBNull(string valueName)
		{
			return r.IsDBNull(r.GetOrdinal(valueName));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Проверяет значение поля строки на NULL.
		/// </summary>
		/// <param name="ordinal">Порядковый номер поля.</param>
		/// <returns></returns>
		public bool IsDBNull(int ordinal)
		{
			return r.IsDBNull(ordinal);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Читает следующую строку из ридера.
		/// </summary>
		/// <returns></returns>
		public bool Read()
		{
			return r.Read();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Закрывает ридер.
		/// </summary>
		public void Close()
		{
			r.Close();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает имя поля по его порядковому номеру.
		/// </summary>
		/// <param name="ordinal">Порядковый номер поля.</param>
		/// <returns>Имя поля.</returns>
		public string GetName(int ordinal)
		{
			return r.GetName(ordinal);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает тип поля.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Type GetFieldType(int i)
		{
			return r.GetFieldType(i);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Выберает следующий возвращенный набор строк.
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
		/// Преобразует к типу SqlDataReader.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static implicit operator SqlDataReader(PulsarSqlReader r)
		{
			return r.r;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Преобразует из типа SqlDataReader.
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
	/// Интерфейс просмотра данных строки PulsarSqlReader.
	/// </summary>
	public interface IPulsarDataReaderViewer
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает значение поля строки.
		/// </summary>
		/// <param name="valueName">Имя поля строки.</param>
		/// <returns></returns>
		object this[string valueName] { get; }
		/// <summary>
		/// Возвращает значение поля строки.
		/// </summary>
		/// <param name="valueOrdinal">Порядковый номер поля строки.</param>
		/// <returns></returns>
		object this[int valueOrdinal] { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, содержит ли PulsarSqlReader строки.
		/// </summary>
		bool HasRows { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает количесто полей ридера.
		/// </summary>
		int FieldCount { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает количесто видимых полей ридера.
		/// </summary>
		int VisibleFieldCount { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, является ли ридер закрытым.
		/// </summary>
		bool IsClosed { get; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Проверяет значение поля строки на NULL.
		/// </summary>
		/// <param name="valueName">Имя поля строки.</param>
		/// <returns></returns>
		bool IsDBNull(string valueName);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Проверяет значение поля строки на NULL.
		/// </summary>
		/// <param name="ordinal">Порядковый номер поля.</param>
		/// <returns></returns>
		bool IsDBNull(int ordinal);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает имя поля по его порядковому номеру.
		/// </summary>
		/// <param name="ordinal">Порядковый номер поля.</param>
		/// <returns>Имя поля.</returns>
		string GetName(int ordinal);
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public interface IPulsarDataReaderViewer >>

}

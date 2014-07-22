using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

using Pulsar.Reflection.Dynamic;

namespace Pulsar.SQL
{
	/// <summary>
	/// Класс реализации IDataReader	для чтения перечисляемых коллекций.
	/// </summary>
	public class EnumeratorDataReader : IDataReader
	{
		IEnumerator	_en = null;
		private List<ValuesPair<string,object>> _columns = new List<ValuesPair<string,object>>();
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="enums">Перечисление.</param>
		/// <param name="persistentValues">Значения столбцов, постоянные для каждой строки.</param>
		/// <param name="columnNames">Имена значений столбцов.</param>
		public EnumeratorDataReader(IEnumerable enums, ParamsDic persistentValues, IEnumerable<string> columnNames) :
		 this(enums.GetEnumerator(), persistentValues, columnNames)
		{
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="enums">Перечисление.</param>
		/// <param name="persistentValues">Значения столбцов, постоянные для каждой строки.</param>
		/// <param name="columnNames">Имена значений столбцов.</param>
		public EnumeratorDataReader(IEnumerable enums, Type itemType, ParamsDic persistentValues, IEnumerable<string> columnNames) :
		 this(enums.GetEnumerator(), itemType, persistentValues, columnNames)
		{
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="enumer">Перечислитель.</param>
		/// <param name="persistentValues">Значения столбцов, постоянные для каждой строки.</param>
		/// <param name="columnNames">Имена значений столбцов.</param>
		public EnumeratorDataReader(IEnumerator enumer, ParamsDic persistentValues, IEnumerable<string> columnNames)
		{
			if(enumer == null)
				throw new ArgumentNullException("enumer");
			_en = enumer;
			Type t = enumer.GetType();
			if(t.IsGenericType)
				t = t.GetGenericArguments()[0];
			else
				t = typeof(object);
			if(columnNames == null)
			{
				if(persistentValues != null)
					foreach(string s in persistentValues.Params)
						_columns.Add(new ValuesPair<string, object>(s, persistentValues[s]));
				foreach(PropertyInfo pi in t.GetProperties())
					_columns.Add(new ValuesPair<string, object>(pi.Name, ReflectionHelper.CreateCallLambdaFunc(pi.GetGetMethod())));
			}
			else
				foreach(string s in columnNames)
					if(persistentValues != null && persistentValues.ContainsKey(s))
						_columns.Add(new ValuesPair<string, object>(s, persistentValues[s]));
					else
						_columns.Add(new ValuesPair<string, object>(s,
							ReflectionHelper.CreateCallLambdaFunc(t.GetProperty(s).GetGetMethod())));
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="enumer">Перечислитель.</param>
		/// <param name="persistentValues">Значения столбцов, постоянные для каждой строки.</param>
		/// <param name="columnNames">Имена значений столбцов.</param>
		public EnumeratorDataReader(IEnumerator enumer, Type itemType, ParamsDic persistentValues, IEnumerable<string> columnNames)
		{
			if(enumer == null)
				throw new ArgumentNullException("enumer");
			_en = enumer;
			if(columnNames == null)
			{
				if(persistentValues != null)
					foreach(string s in persistentValues.Params)
						_columns.Add(new ValuesPair<string, object>(s, persistentValues[s]));
				foreach(PropertyInfo pi in itemType.GetProperties())
					_columns.Add(new ValuesPair<string, object>(pi.Name, ReflectionHelper.CreateCallLambdaFunc(pi.GetGetMethod())));
			}
			else
				foreach(string s in columnNames)
					if(persistentValues != null && persistentValues.ContainsKey(s))
						_columns.Add(new ValuesPair<string,object>(s, persistentValues[s]));
					else
						_columns.Add(new ValuesPair<string, object>(s,
							ReflectionHelper.CreateCallLambdaFunc(itemType.GetProperty(s).GetGetMethod())));
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Перезапускает чтение без обновления столбцов.
		/// </summary>
		public void Reset(IEnumerable enums, ParamsDic persistentValues)
		{
			if(enums == null)
			 throw new ArgumentNullException("enums");
			Reset(enums.GetEnumerator(), persistentValues);
		}
		/// <summary>
		/// Перезапускает чтение без обновления столбцов.
		/// </summary>
		public void Reset(IEnumerator enums, ParamsDic persistentValues)
		{
			_en = enums;
			//_en.Reset();
			if(persistentValues != null)
			 foreach(var i in _columns)
				 if(i.Value2 is Delegate == false && persistentValues.ContainsKey(i.Value1))
					 i.Value2 = persistentValues[i.Value1];
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#pragma warning disable 1591
		#region IDataReader Members
		public void Close()
		{
			_en = null;
		}
		public int Depth
		{
			get { return 0; }
		}
		public DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}
		public bool IsClosed
		{
			get { return _en == null; }
		}
		public bool NextResult()
		{
			return false;
		}
		public bool Read()
		{
			return _en.MoveNext();
		}
		public int RecordsAffected
		{
			get { return -1; }
		}
		public void Dispose()
		{
			_en = null;
			_columns.Clear();
		}
		public int FieldCount
		{
			get { return _columns.Count;  }
		}
		public bool GetBoolean(int i)
		{
			return (bool)this[i];
		}
		public byte GetByte(int i)
		{
			return (byte)this[i];
		}
		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}
		public char GetChar(int i)
		{
			return (char)this[i];
		}
		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}
		public IDataReader GetData(int i)
		{
			return (IDataReader)this[i];
		}
		public string GetDataTypeName(int i)
		{
			object o = this[i];
			return o == null ? null : o.GetType().Name ;
		}
		public DateTime GetDateTime(int i)
		{
			return (DateTime)this[i];
		}
		public decimal GetDecimal(int i)
		{
			return (decimal)this[i];
		}
		public double GetDouble(int i)
		{
			return (double)this[i];
		}
		public Type GetFieldType(int i)
		{
			object o = this[i];
			return o == null ? null : o.GetType() ;
		}
		public float GetFloat(int i)
		{
			return (float)this[i];
		}
		public Guid GetGuid(int i)
		{
			return (Guid)this[i];
		}
		public short GetInt16(int i)
		{
			return (short)this[i];
		}
		public int GetInt32(int i)
		{
			return (int)this[i];
		}
		public long GetInt64(int i)
		{
			return (long)this[i];
		}
		public string GetName(int i)
		{
			return _columns[i].Value1;
		}
		public int GetOrdinal(string name)
		{
			for(int a = 0; a < _columns.Count; a++)
				if(_columns[a].Value1 == name)
					return a;
			return -1;
		}
		public string GetString(int i)
		{
			return (string)this[i];
		}
		public object GetValue(int i)
		{
			return this[i];
		}
		public int GetValues(object[] values)
		{
			for(int a = 0; a < _columns.Count; a++)
				values[a] = this[a];
			return _columns.Count;
		}
		public bool IsDBNull(int i)
		{
			return this[i] is DBNull;
		}
		public object this[string name]
		{
			get 
			{ 
				for(int i = 0; i < _columns.Count; i++)
					if(_columns[i].Value1 == name)
						return this[i];
				return null;
			}
		}
		public object this[int i]
		{
			get 
			{ 
				if(_columns[i].Value2 is Delegate)
					return ((Func<object, object>)_columns[i].Value2)(_en.Current);
				else
					return ((ValuesPair<string,object>)_columns[i]).Value2;
			}
		}
		#endregion
		#pragma warning restore 1591
	}						

}

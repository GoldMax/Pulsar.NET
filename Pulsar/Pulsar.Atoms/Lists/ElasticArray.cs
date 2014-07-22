using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс одномерного массива, реализующего интерфейсы списков
	/// </summary>
	public class ElasticArray<T> : IList<T>, IList, IDisposable
	{
		private T[] _arr = null;	 	 		 
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет размер массива
		/// </summary>
		public int Length
		{
			get { return _arr.Length; }
			set
			{
				if (value == _arr.Length)
					return;
				T[] aa = new T[value];
				Array.Copy(_arr, aa, value);
				_arr = aa;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get { return _arr[index]; }
			set
			{
				_arr[index] = value;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public ElasticArray()
		{
			_arr = new T[0];
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ElasticArray(int length)
		{
			_arr = new T[length];
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ElasticArray(IEnumerable<T> vals)
		{
			_arr = new T[vals.Count()];
			int i = 0;
			foreach (var a in vals)
				_arr[i++] = a;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ElasticArray(T value)
		{
			_arr = new T[1];
			_arr[0] = value;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#pragma warning disable
		#region IList
		#region IList<T> Members
		public int IndexOf(T item)
		{
			for (int a = 0; a < _arr.Length; a++)
				if (Object.Equals(_arr[a], item))
					return a;
			return -1;
		}
		public void Insert(int index, T item)
		{
			if (index < 0 || index > _arr.Length)
				throw new ArgumentOutOfRangeException("index");
			if (index == _arr.Length)
			{
				Add(item);
				return;
			}
			T[] arr = new T[_arr.Length + 1];
			Array.Copy(_arr, arr, index);
			arr[index] = item;
			Array.Copy(_arr, index, arr, index + 1, _arr.Length - index);
			_arr = arr;
		}
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= _arr.Length)
				throw new ArgumentOutOfRangeException("index");
			T[] arr = new T[_arr.Length - 1];
			Array.Copy(_arr, arr, index);
			Array.Copy(_arr, index + 1, arr, index, _arr.Length - index - 1);
			_arr = arr;
		}
		#endregion
		#region ICollection<T> Members
		public void Add(T item)
		{
			T[] arr = new T[_arr.Length + 1];
			Array.Copy(_arr, arr, _arr.Length);
			arr[arr.Length - 1] = item;
			_arr = arr;
		}
		public void AddRange(IEnumerable<T> items)
		{
			if(items == null)
				throw new ArgumentNullException("item");
			int count = items.Count();
			if(count == 0)
				return;
			int pos = _arr.Length;
			Array.Resize(ref _arr, _arr.Length + count);
			foreach(T i in items)
			 _arr[pos++] = i;
		}
		/// <summary>
		/// Устанавливает все элементы массива в значения по умолчанию.
		/// </summary>
		public void Clear()
		{
			for (int a = 0; a < _arr.Length; a++)
				_arr[a] = default(T);
		}
		public bool Contains(T item)
		{
			for (int a = 0; a < _arr.Length; a++)
				if (Object.Equals(_arr[a], item))
					return true;
			return false;
		}
		public void CopyTo(T[] array, int arrayIndex)
		{
			_arr.CopyTo(array, arrayIndex);
		}
		int ICollection<T>.Count
		{
			get { return _arr.Length; }
		}
		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index == -1)
				return false;
			RemoveAt(index);
			return true;
		}
		#endregion
		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			for (int a = 0; a < _arr.Length; a++)
				yield return _arr[a];
		}
		#endregion
		#region IList Members
		int IList.Add(object value)
		{
			Add((T)value);
			return IndexOf((T)value);
		}
		void IList.Clear()
		{
			Clear();
		}
		bool IList.Contains(object value)
		{
			return Contains((T)value);
		}
		int IList.IndexOf(object value)
		{
			return IndexOf((T)value);
		}
		void IList.Insert(int index, object value)
		{
			Insert(index, (T)value);
		}
		bool IList.IsFixedSize
		{
			get { return false; }
		}
		bool IList.IsReadOnly
		{
			get { return false; }
		}
		void IList.Remove(object value)
		{
			Remove((T)value);
		}
		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (T)value; }
		}
		#endregion
		#region ICollection Members
		void ICollection.CopyTo(Array array, int index)
		{
			_arr.CopyTo(array, index);
		}
		int ICollection.Count
		{
			get { return _arr.Length; }
		}
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		object ICollection.SyncRoot
		{
			get { return _arr; }
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _arr.GetEnumerator();
		}
		#endregion
		#endregion IList
		#pragma warning restore
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (_arr.Length == 0)
				return "(нет значения)";
			StringBuilder sb = new StringBuilder();
			for (int a = 0; a < (_arr.Length > 10 ? 10 : _arr.Length); a++)
				sb.AppendFormat(",{0}", _arr[a].ToString());
			if (sb.Length > 0)
				sb.Remove(0, 1);
			if (_arr.Length > 10)
				sb.Append(", ...");
			return sb.ToString();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли текущий массив все элементы узазанного массива.
		/// </summary>
		/// <param name="arr">Проверяемый массив.</param>
		/// <returns></returns>
		public bool Contains(ElasticArray<T> arr)
		{
			if (arr == null || arr.Length == 0)
				return false;
			for (int b = 0; b < arr.Length; b++)
			{
				bool has = false;
				for (int a = 0; a < _arr.Length; a++)
					if (Object.Equals(_arr[a], arr[b]))
					{
						has = true;
						break;
					}
				if (has == false)
					return false;
			}
			return true;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Меняет местами 2 элемента массива.
		/// </summary>
		/// <param name="pos1">Позиция первого элемента</param>
		/// <param name="pos2">Позиция второго элемента</param>
		public void SwapItems(int pos1, int pos2)
		{
			T x = _arr[pos1];
			_arr[pos1] = _arr[pos2];
			_arr[pos2] = x;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Увеличивает ёмкость массива.
		/// </summary>
		/// <param name="len">Новое значение емкости</param>
		public void SetCapacity(int len)
		{
			if(len < _arr.Length)
			 throw new Exception("Новое значение длины не может быть меньше текущего!");
			if(len == _arr.Length)
			 return;
			T[] arr = new T[len];
			Array.Copy(_arr, arr, _arr.Length);
			_arr = arr;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << Static Methods >>
		/// <summary>
		/// Сравнивает два ElasticArray 
		/// </summary>
		/// <param name="arr1"></param>
		/// <param name="arr2"></param>
		/// <returns></returns>
		public static bool Compare(ElasticArray<T> arr1, ElasticArray<T> arr2)
		{
			if (arr1 == null && arr2 == null)
				return true;
			if (arr1 == null || arr2 == null)
				return false;
			if (arr1.Length != arr2.Length)
				return false;
			for (int a = 0; a < arr1.Length; a++)
				if (Object.Equals(arr1[a], arr2[a]) == false)
					return false;
			return true;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Явное приведение к массиву.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static implicit operator T[](ElasticArray<T> x)
		{
		 if(x == null)
			 return null;
			return x._arr;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Неявное приведение к ElasticArray.
		/// </summary>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static explicit operator ElasticArray<T>(T[] arr)
		{
			return new ElasticArray<T>(arr);
		}
		#endregion << Static Methods >>
		//-------------------------------------------------------------------------------------
		#region IDisposable Members
		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if(_arr != null)
			{
				Array.Clear(_arr, 0, _arr.Length);
				_arr = null;
			}
		}

		#endregion
	}
}

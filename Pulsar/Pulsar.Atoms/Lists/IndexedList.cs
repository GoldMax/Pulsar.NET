using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Pulsar.Reflection.Dynamic;

namespace Pulsar
{
	public static class Primes
	{
		internal static readonly int[] primes = new int[]
		{
			7,17,29,59,89,131,197,293,431,521,631,761,919,1103,1327,1597,1931,2333,2801,3371,4049,4861,5839,7013, 
			8419,10103,12143,14591,17519,21023,25229,30293,36353,43627,52361,62851,75431,90523,108631,130363,156437, 
			187751,225307,270371,324449,389357,467237,560689,672827,807403,968897,1162687,1395263,1674319,2009191,
			2411033,2893249,3471899,4166287,4999559,5999471,7199369
		};
		internal static bool IsPrime(int candidate)
		{
			if((candidate & 1) != 0)
			{
				int num = (int)Math.Sqrt((double)candidate);
				for(int i = 3; i <= num; i += 2)
				{
					if(candidate % i == 0)
					{
						return false;
					}
				}
				return true;
			}
			return candidate == 2;
		}
		internal static int GetPrime(int min)
		{
			if(min < 0)
				throw new ArgumentException();
			for(int i = 0; i < primes.Length; i++)
			{
				int num = primes[i];
				if(num > min)
					return num;
			}
			for(int j = min | 1; j < 2147483647; j += 2)
				if(IsPrime(j))
					return j;
			return min;
		}
	}
	/// <summary>
	/// Класс индексированного списка. Null в качестве элементов не допускается.
	/// </summary>
	/// <typeparam name="TIndex">Тип элементов индекса. 
	/// Должен совпадать с типом индексируемого свойства.</typeparam>
	/// <typeparam name="T">Тип элеметов списка.</typeparam>
	public class IndexedList<T, TIndex> : CollectionChangeNotify, IPList, IList<T>, IList, ISerializable
	{
		private delegate TIndex GetRefValue(ref T item);

		// IndexedPropertyName
		private string _pn = null;
		private uint _ver = 0;
		// IsUniqueIndex
		private readonly bool _si = true;

		[NonSerialized]
		private int _count = 0;
		/// <summary>
		/// Массив элементов
		/// </summary>
		[NonSerialized]
		protected T[] _arr = null;

		[NonSerialized]
		private Delegate _getFunc = null;
		[NonSerialized]
		ElasticArray<ValuesPair<uint, UInt>>[] _index = null;
		[NonSerialized]
		private bool _isChangeNotify = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Имя индексируемого свойства.
		/// </summary>
		public string IndexedPropertyName
		{
			get { return _pn; }
		}
		/// <summary>
		/// Определяет, является ли индекс	уникальным.
		/// </summary>
		public bool IsUniqueIndex
		{
			get { return _si; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		private IndexedList()
		{
			if(typeof(T).GetInterface("IObjectChangeNotify", false) != null)
				_isChangeNotify = true;
			//_si = typeof(T).IsValueType;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="propName">Имя индексируемого свойства.</param>
		public IndexedList(string propName)
			: this()
		{
			_pn = propName;
			_getFunc = ReflectionHelper.GetPropertyGetMethod<T>(propName);
			_arr = new T[3];
			_index = new ElasticArray<ValuesPair<uint, UInt>>[3];
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="propName">Имя индексируемого свойства.</param>
		/// <param name="capacity">Ёмкость списка.</param>
		public IndexedList(string propName, int capacity)
			: this()
		{
			_pn = propName;
			_getFunc = ReflectionHelper.GetPropertyGetMethod<T>(propName);
			_arr = new T[Primes.GetPrime(capacity)];
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="propName">Имя индексируемого свойства.</param>
		/// <param name="isUniqueIndex">Определяет, является ли индекс	уникальным.</param>
		public IndexedList(string propName, bool isUniqueIndex)
			: this()
		{
			_pn = propName;
			_getFunc = ReflectionHelper.GetPropertyGetMethod<T>(propName);
			_arr = new T[3];
			_index = new ElasticArray<ValuesPair<uint, UInt>>[3];
			_si = isUniqueIndex;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="propName">Имя индексируемого свойства.</param>
		/// <param name="isUniqueIndex">Определяет, является ли индекс	уникальным.</param>
		/// <param name="capacity">Ёмкость списка/</param>
		public IndexedList(string propName, bool isUniqueIndex, int capacity)
			: this()
		{
			_pn = propName;
			_getFunc = ReflectionHelper.GetPropertyGetMethod<T>(propName);
			_arr = new T[Primes.GetPrime(capacity)];
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
			//if(_si && _si != isUniqueIndex)
			// throw new Exception("Не допускается создание списка для значимого типа с неуникальным индексом!");
			_si = isUniqueIndex;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region IList<T> Members
		/// <summary>
		/// Возвращает или устанавливает первый по списку элемент по его позиции.
		/// </summary>
		/// <param name="pos">Позиция элемента</param>
		/// <returns></returns>
		public T this[int pos]
		{
			get
			{
				if(pos < 0 || pos >= _count)
					throw new ArgumentOutOfRangeException("pos");
				return _arr[pos];
			}
			set
			{
				if(pos < 0 || pos >= _count)
					throw new ArgumentOutOfRangeException("pos");
				if(value == null)
					throw new ArgumentNullException("value");

				Tuple<uint,uint,TIndex> hashVal = CalcItemHashes(value);
				if(_si && Object.Equals(GetItemIndex(value), GetItemIndex(_arr[pos])) == false)
					CheckItemIndex(hashVal);

				T item = _arr[pos];
				
				OnItemChanging(_arr[pos], value, pos);
				if(_isChangeNotify)
				{
					((IObjectChangeNotify)_arr[pos]).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[pos]).ObjectChanged 	-= Item_ObjectChanged;
				}

				Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
				for(int a = 0; a < _index[hash.Item2].Length; a++)
					if(_index[hash.Item2][a].Value2 == pos)
					{
						_index[hash.Item2].RemoveAt(a);
						break;
					}
				if(_index[hash.Item2].Length == 0)
					_index[hash.Item2] = null;

				IndexAdd(hashVal, pos);

				_arr[pos] = value;
				_ver++;

				if(_isChangeNotify)
				{
					((IObjectChangeNotify)_arr[pos]).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)_arr[pos]).ObjectChanged 	+= Item_ObjectChanged;
				}
				OnItemChanged(item,_arr[pos], pos);
			}
		}
		/// <summary>
		///  Возвращает первый по списку элемент по его индексу.
		/// </summary>
		/// <param name="index">Индекс элемента</param>
		/// <returns></returns>
		public T ByIndex(TIndex index)
		{
			uint hash;
			if(Object.Equals(index, null))
				hash = 0;
			else
				hash = (uint)index.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;

			if(_index[hash2] == null)
				return default(T);
			foreach(ValuesPair<uint, UInt> i in _index[hash2])
				if(i.Value1 == hash && Object.Equals(index, GetItemIndex(_arr[i.Value2])))
					return _arr[i.Value2];
			return default(T);
		}
		/// <summary>
		///  Возвращает все элементы списка с указанным индексом.
		/// </summary>
		/// <param name="index">Индекс элемента</param>
		/// <returns></returns>
		public T[] ByIndexAll(TIndex index)
		{
			uint hash;
			if(Object.Equals(index, null))
				hash = 0;
			else
				hash = (uint)index.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;

			if(_index[hash2] == null)
				return new T[0];
			ElasticArray<T> res = new ElasticArray<T>();
			foreach(ValuesPair<uint, UInt> i in _index[hash2])
				if(i.Value1 == hash && Object.Equals(index, GetItemIndex(_arr[i.Value2])))
					res.Add(_arr[i.Value2]);
			return res;
		}
		/// <summary>
		/// Возвращает первую позицию в списке указанного элемента.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_index[hash.Item2] == null)
				return -1;
			foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
				if(i.Value1 == hash.Item1 && Object.Equals(item, _arr[i.Value2]))
					return i.Value2;
			return -1;
		}
		/// <summary>
		/// Возвращает первую позицию в списке элемента с указанным значением индекса.
		/// </summary>
		/// <param name="index">Значение индекса элемента</param>
		/// <returns></returns>
		public int IndexOfIndex(TIndex index)
		{
			uint hash;
			if(Object.Equals(index, null))
				hash = 0;
			else
				hash = (uint)index.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			if(_index[hash2] == null)
				return -1;
			foreach(ValuesPair<uint, UInt> i in _index[hash2])
				if(i.Value1 == hash && Object.Equals(index, GetItemIndex(_arr[i.Value2])))
					return i.Value2;
			return -1;
		}
		/// <summary>
		/// Возвращает все позиции в списке указанного элемента.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int[] IndexOfAll(T item)
		{
			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_index[hash.Item2] == null)
				return new int[0];
			ElasticArray<int> res = new ElasticArray<int>();
			foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
				if(i.Value1 == hash.Item1 && Object.Equals(item, _arr[i.Value2]))
					res.Add(i.Value2);
			return res;
		}
		/// <summary>
		/// Возвращает все позиции элементов с указанным значением индекса.
		/// </summary>
		/// <param name="index">Значение индекса элемента</param>
		/// <returns></returns>
		public int[] IndexOfIndexAll(TIndex index)
		{
			uint hash;
			if(Object.Equals(index, null))
				hash = 0;
			else
				hash = (uint)index.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			if(_index[hash2] == null)
				return new int[0];
			ElasticArray<int> res = new ElasticArray<int>();
			foreach(ValuesPair<uint, UInt> i in _index[hash2])
				if(i.Value1 == hash && Object.Equals(index, GetItemIndex(_arr[i.Value2])))
					res.Add(i.Value2);
			return res;
		}
		/// <summary>
		/// Вставляет элемент в список
		/// </summary>
		/// <param name="pos">Позиция вставки</param>
		/// <param name="item">Вставляемый элемент</param>
		public void Insert(int pos, T item)
		{
			if(pos < 0 || pos > _count)
				throw new ArgumentOutOfRangeException("index");
			if(pos == _count)
			{
				Add(item);
				return;
			}

			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_si)
				CheckItemIndex(hash);

			OnItemAdding(item, pos);
			if(_arr.Length == _count)
			{
				Resize();
				hash = CalcItemHashes(item);
			}

			foreach(var arr in _index)
				if(arr != null)
					foreach(var i in arr)
						if(i.Value2 >= pos)
							i.Value2++;

			for(int a = _count-1; a >= pos; a--)
				_arr[a+1] = _arr[a];

			IndexAdd(hash, pos);

			_arr[pos] = item;
			_count++;
			_ver++;
			if(_isChangeNotify)
			{
				((IObjectChangeNotify)_arr[pos]).ObjectChanging += Item_ObjectChanging;
				((IObjectChangeNotify)_arr[pos]).ObjectChanged 	+= Item_ObjectChanged;
			}
			OnItemAdded(item, pos);
		}
		/// <summary>
		/// Удаляет элемент в указанной позиции
		/// </summary>
		/// <param name="index">Позиция удаляемого элемента</param>
		public void RemoveAt(int index)
		{
			if(index < 0 || index >= _count)
				throw new ArgumentOutOfRangeException("index");
			T item = _arr[index];
			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			OnItemDeleting(item, index);
			for(int a = 0; a < _index[hash.Item2].Length; a++)
				if(_index[hash.Item2][a].Value2 == index)
				{
					_index[hash.Item2].RemoveAt(a);
					break;
				}
			if(_index[hash.Item2].Length == 0)
				_index[hash.Item2] = null;

			foreach(var arr in _index)
				if(arr != null)
					foreach(var i in arr)
						if(i.Value2 > index)
							i.Value2--;

			Array.Copy(_arr, index+1, _arr, index, _count-index-1);
			_arr[_count-1] = default(T);

			if(_isChangeNotify)						
			{
				((IObjectChangeNotify)item).ObjectChanging -= Item_ObjectChanging;
				((IObjectChangeNotify)item).ObjectChanged 	-= Item_ObjectChanged;
			}

			_count--;
			_ver++;
			OnItemDeleted(item, index);
		}
		/// <summary>
		/// Добавляет элемент в список
		/// </summary>
		/// <param name="item">Добавляемый элемент</param>
		public void Add(T item)
		{
			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_si)
				CheckItemIndex(hash);

			OnItemAdding(item, _count);
			if(_arr.Length == _count)
			{
				Resize();
				hash = CalcItemHashes(item);
			}

			IndexAdd(hash, _count);
			if(_isChangeNotify)
			{
				((IObjectChangeNotify)item).ObjectChanging += Item_ObjectChanging;
				((IObjectChangeNotify)item).ObjectChanged 	+= Item_ObjectChanged;
			}
			_arr[_count] = item;

			_count++;
			_ver++;
			OnItemAdded(item, _count-1);
		}
		/// <summary>
		/// Добавляет элемент в список.Если индекс уникальный и ключ уже присутствует, ничего не делает.
		/// </summary>
		/// <param name="item">Добавляемый элемент</param>
		public bool TryAdd(T item)
		{
			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_si && _index[hash.Item2] != null)
			 foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
			 	if(i.Value1 == hash.Item1 && Object.Equals(hash.Item3, GetItemIndex(_arr[i.Value2])))
			 		return false;

			OnItemAdding(item, _count);
			if(_arr.Length == _count)
			{
				Resize();
				hash = CalcItemHashes(item);
			}

			IndexAdd(hash, _count);
			if(_isChangeNotify)
			{
				((IObjectChangeNotify)item).ObjectChanging += Item_ObjectChanging;
				((IObjectChangeNotify)item).ObjectChanged 	+= Item_ObjectChanged;
			}
			_arr[_count] = item;

			_count++;
			_ver++;
			OnItemAdded(item, _count-1);
			return true;
		}

		/// <summary>
		/// Добавляет перечисление элеметов в список.
		/// </summary>
		/// <param name="items">Перечисление элементов</param>
		public void Add(IEnumerable<T> items)
		{
			if(items == null)
				throw new ArgumentNullException("items");
			bool was = IsChangeEventsOff;
			if(was == false)
				EventsOff();
			foreach(T i in items)
				Add(i);
			if(was == false)
				EventsOn();
		}
		/// <summary>
		/// Очищает список
		/// </summary>
		public void Clear()
		{
		 OnObjectResetting();
			if(_isChangeNotify)
				for(int a = 0; a < _count; a++)
				{
					((IObjectChangeNotify)_arr[a]).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a]).ObjectChanged 	-= Item_ObjectChanged;
				}
			Array.Clear(_arr, 0, _arr.Length);
			Array.Clear(_index, 0, _index.Length);
			_count = 0;
			_ver++;
			OnObjectResetted();
		}
		/// <summary>
		/// Определяет, принадлежит ли элемент списку
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}
		/// <summary>
		/// Определяет, содержит ли список элементы с указанным значением индекса.
		/// </summary>
		/// <param name="index">Значение индекса</param>
		/// <returns></returns>
		public bool ContainsIndex(TIndex index)
		{
			return IndexOfIndex(index) != -1;
		}
		/// <summary>
		/// Копирует элементы списка в массив.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(_arr, 0, array, arrayIndex, _count);
		}
		/// <summary>
		/// Возвращает количесто элеметов в списке
		/// </summary>
		public int Count
		{
			get { return _count; }
		}
		/// <summary>
		/// 
		/// </summary>
		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
		/// <summary>
		/// Удаляет элемент из списка
		/// </summary>
		/// <param name="item">Удаляемый элемент</param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_index[hash.Item2] == null)
				return false;
			int index = -1;
			for(int a = 0; a < _index[hash.Item2].Length; a++)
				if(_index[hash.Item2][a].Value1 == hash.Item1 && Object.Equals(item, _arr[_index[hash.Item2][a].Value2]))
				{
					index = _index[hash.Item2][a].Value2;
					OnItemDeleting(item, index);
					_index[hash.Item2].RemoveAt(a);
					break;
				}
			if(index == -1)
				return false;
			if(_index[hash.Item2].Length == 0)
				_index[hash.Item2] = null;

			foreach(var arr in _index)
				if(arr != null)
					foreach(var i in arr)
						if(i.Value2 > index)
							i.Value2--;

			Array.Copy(_arr, index+1, _arr, index, _count-index-1);
			_arr[_count-1] = default(T);

			if(_isChangeNotify)
			{
				((IObjectChangeNotify)item).ObjectChanging -= Item_ObjectChanging;
				((IObjectChangeNotify)item).ObjectChanged 	-= Item_ObjectChanged;
			}

			_count--;
			_ver++;
			OnItemDeleted(item, index);
			return true;
		}
		/// <summary>
		/// Удаляет первый по списку элемент с заданным значением индекса
		/// </summary>
		/// <param name="index">Значение индекса</param>
		/// <returns></returns>
		public bool RemoveByIndex(TIndex index)
		{
			if(Object.Equals(index, null))
				throw new ArgumentNullException("index");
			uint hash;
			if(Object.Equals(index, null))
				hash = 0;
			else
				hash = (uint)index.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			if(_index[hash2] == null)
				return false;
			int pos = -1;
			T item = default(T);
			for(int a = 0; a < _index[hash2].Length; a++)
			{
				item = _arr[_index[hash2][a].Value2];
				if(_index[hash2][a].Value1 == hash && Object.Equals(index, GetItemIndex(item)))
				{
					OnItemDeleting(item, pos);
					pos = _index[hash2][a].Value2;
					_index[hash2].RemoveAt(a);
					break;
				}
			}
			if(pos == -1)
				return false;
			if(_index[hash2].Length == 0)
				_index[hash2] = null;

			foreach(var arr in _index)
				if(arr != null)
					foreach(var i in arr)
						if(i.Value2 > pos)
							i.Value2--;

			Array.Copy(_arr, pos+1, _arr, pos, _count-pos-1);
			_arr[_count-1] = default(T);

			if(_isChangeNotify)
			{
				((IObjectChangeNotify)item).ObjectChanging -= Item_ObjectChanging;
				((IObjectChangeNotify)item).ObjectChanged 	-= Item_ObjectChanged;
			}

			_count--;
			_ver++;
			OnItemDeleted(item, pos);
			return true;
		}
		/// <summary>
		/// Удаляет все вхождения элемента из списка
		/// </summary>
		/// <param name="item">Удаляемый элемент</param>
		/// <returns></returns>
		public void RemoveAll(T item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_index[hash.Item2] == null)
				return;
			int index = -1;
			do
			{
				index = -1;
				for(int a = 0; _index[hash.Item2] != null && a < _index[hash.Item2].Length; a++)
					if(_index[hash.Item2][a].Value1 == hash.Item1 && Object.Equals(item, _arr[_index[hash.Item2][a].Value2]))
					{
						OnItemDeleting(item, index);
						index = _index[hash.Item2][a].Value2;
						_index[hash.Item2].RemoveAt(a);
						break;
					}
				if(index == -1)
					return;
				if(_index[hash.Item2].Length == 0)
					_index[hash.Item2] = null;

				foreach(var arr in _index)
					if(arr != null)
						foreach(var i in arr)
							if(i.Value2 > index)
								i.Value2--;

				Array.Copy(_arr, index+1, _arr, index, _count-index-1);
				_arr[_count-1] = default(T);

				if(_isChangeNotify)
				{
					((IObjectChangeNotify)item).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)item).ObjectChanged 	-= Item_ObjectChanged;
				}

				_count--;
				_ver++;
				OnItemDeleted(item, index);
			} while(index != -1);
		}
		/// <summary>
		/// Удаляет все вхождения элементов с указанным значением индекса из списка
		/// </summary>
		/// <param name="index">Значение индекса</param>
		/// <returns></returns>
		public void RemoveByIndexAll(TIndex index)
		{
			if(Object.Equals(index, null))
				throw new ArgumentNullException("index");
			uint hash;
			if(Object.Equals(index, null))
				hash = 0;
			else
				hash = (uint)index.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			if(_index[hash2] == null)
				return;
			int pos = -1;
			do
			{
				pos = -1;
				T item = default(T);
				for(int a = 0; _index[hash2] != null && a < _index[hash2].Length; a++)
				{
					item = _arr[_index[hash2][a].Value2];
					if(_index[hash2][a].Value1 == hash && Object.Equals(index, GetItemIndex(item)))
					{
						OnItemDeleting(item, pos);
						pos = _index[hash2][a].Value2;
						_index[hash2].RemoveAt(a);
						break;
					}
				}
				if(pos == -1)
					return;
				if(_index[hash2].Length == 0)
					_index[hash2] = null;

				foreach(var arr in _index)
					if(arr != null)
						foreach(var i in arr)
							if(i.Value2 > pos)
								i.Value2--;

				Array.Copy(_arr, pos+1, _arr, pos, _count-pos-1);
				_arr[_count-1] = default(T);

				if(_isChangeNotify)
				{
					((IObjectChangeNotify)item).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)item).ObjectChanged 	-= Item_ObjectChanged;
				}

				_count--;
				_ver++;
				OnItemDeleted(item, pos);
			} while(pos != -1);
		}
		/// <summary>
		/// Перемещает элемент в новую позицию в списке.
		/// </summary>
		/// <param name="item">Перемещаемый элемент.</param>
		/// <param name="newPos">Новая позиция элемента.</param>
		public void Move(T item, int newPos)
		{
			if(newPos < 0 || newPos >= _count)
				throw new ArgumentOutOfRangeException("newPos");
			if(item == null)
				throw new ArgumentNullException("item");

			Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			if(_index[hash.Item2] == null)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", item);
			ValuesPair<uint, UInt> pos = null;
			foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
				if(i.Value1 == hash.Item1 && Object.Equals(item, _arr[i.Value2]))
				{
					pos = i;
					break;
				}
			if(pos == null)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", item);
			if(pos.Value2 == newPos)
				return;

			OnObjectResetting();

			int p = pos.Value2;
			Array.Copy(_arr, p+1, _arr, p, _count-p-1);
			foreach(var arr in _index)
				if(arr != null)
					foreach(var i in arr)
						if(i.Value2 > p)
							i.Value2--;

			for(int a = _count-2; a >= newPos; a--)
				_arr[a+1] = _arr[a];
			_arr[newPos] = item;
			foreach(var arr in _index)
				if(arr != null)
					foreach(var i in arr)
						if(i.Value2 >= newPos)
							i.Value2++;

			pos.Value2 = newPos;

			_ver++;
			OnObjectResetted();
		}
		#endregion
		#region IPList Members
		/// <summary>
		/// Устанавливает наименьшую из возможных емкость списка, исходя из числа элементов.
		/// </summary>
		public void TrimExcess()
		{
			Resize(false);
		}
		/// <summary>
		/// Проверяет элемент на наличие в списке.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public T InList(T item)
		{
			int pos = IndexOf(item);
			if(pos == -1)
				return default(T);
			return this[pos];
		}
		object IPList.InList(object item)
		{
			return InList((T)item);
		}
		/// <summary>
		/// Сортировка списка
		/// </summary>
		/// <param name="comparer"></param>
		public void Sort(Comparison<object> comparer = null)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

		 
			PListComparer comp;
			if(comparer == null)
				comp = new PListComparer((x, y) => string.Compare(x == null ? "" : x.ToString(), y == null ? "" : y.ToString()));
			else
				comp = new PListComparer(comparer);

			OnObjectResetting();

			Array.Sort(_arr, 0, _count, comp);
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
			for(int a = 0; a < _count; a++)
			 IndexAdd(_arr[a],a);

			OnObjectResetted();
		}
		/// <summary>
		/// Версия изменений списка.
		/// </summary>
		public uint Version
		{
			get { return _ver; }
		}
		/// <summary>
		/// Проверяет версию списка. Если версия одинаковая, возвращает сам список, иначе выбрасывает исключение защиты.
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		public virtual IndexedList<T, TIndex> CheckVersion(uint version)
		{
			if(_ver != version)
				throw new PulsarErrorException("Версия серверного объекта отличается от версии клиентского объекта!");
			return this;
		}
		object IVersionedObject.CheckVersion(uint version)
		{
			return CheckVersion(version);
		}
		#endregion
		#region IEnumerable<T> Members
		/// <summary>
		/// GetEnumerator() 
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
		 uint ver = _ver;
			for(int a = 0; a < _count; a++)
				if(ver != _ver)
					throw new Exception("Итерация невозможна, так как коллекция была изменена!");
				else
					yield return _arr[a];
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		#region IList Members
		int IList.Add(object value)
		{
			Add((T)value);
			return _count-1;
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
		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(_arr, 0, array, index, _count);
		}
		int ICollection.Count
		{
			get { return _count; }
		}
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		object ICollection.SyncRoot
		{
			get { return SyncRoot; }
		}
		#endregion
		#region << Private Methods >>
		/// <summary>
		/// Увеличивает размеры списка и индекса
		/// </summary>
		private void Resize(bool x2 = true)
		{
			T[] arr = new T[Primes.GetPrime(x2 ? _arr.Length*2 : _count)];
			Array.Copy(_arr, arr, _count);
			_arr = arr;
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
			for(int a = 0; a < _count; a++)
				if(_arr[a] != null)
					IndexAdd(_arr[a], a);
		}
		/// <summary>
		/// Возвращает кортеж hash - indexPos	 - indexVal
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private Tuple<uint, uint, TIndex> CalcItemHashes(T item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			TIndex val = GetItemIndex(item);
			uint hash;
			if(Object.Equals(val, null))
				hash = 0;
			else
				hash = (uint)val.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			return new Tuple<uint, uint, TIndex>(hash, hash2, val);
		}
		/// <summary>
		/// добавляет ссылку на позицию элемента в индекс
		/// </summary>
		/// <param name="item"></param>
		/// <param name="pos"></param>
		private void IndexAdd(T item, UInt pos)
		{
			Tuple<uint,uint,TIndex>	hash =  CalcItemHashes(item);
			if(_index[hash.Item2] == null)
				_index[hash.Item2] = new ElasticArray<ValuesPair<uint, UInt>>(new ValuesPair<uint, UInt>(hash.Item1, pos));
			else
				_index[hash.Item2].Add(new ValuesPair<uint, UInt>(hash.Item1, pos));
		}
		private void IndexAdd(Tuple<uint, uint, TIndex> hash, UInt pos)
		{
			if(_index[hash.Item2] == null)
				_index[hash.Item2] = new ElasticArray<ValuesPair<uint, UInt>>(new ValuesPair<uint, UInt>(hash.Item1, pos));
			else
				_index[hash.Item2].Add(new ValuesPair<uint, UInt>(hash.Item1, pos));
		}
		private TIndex GetItemIndex(T item)
		{
			if(typeof(T).IsValueType)
				return ((RefFunc<T, TIndex>)_getFunc)(ref item);
			else
				return ((Func<T, TIndex>)_getFunc)(item);
		}
		private void CheckItemIndex(Tuple<uint, uint, TIndex> hash)
		{
			if(_index[hash.Item2] == null)
				return;
			foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
				if(i.Value1 == hash.Item1 && Object.Equals(hash.Item3, GetItemIndex(_arr[i.Value2])))
					throw new PulsarException("Элемент со значеним [{0}] индексируемого свойства уже присутствует в списке!",
						hash.Item3);
		}
		private void IndexRebuild()
		{
			Array.Clear(_index, 0, _index.Length);
			for(int a = 0; a < _count; a++)
				IndexAdd(_arr[a], a);
		}
		//-------------------------------------------------------------------------------------
		void Item_ObjectChanging(object sender, ObjectChangeNotifyEventArgs args)
		{
			if(sender == null)
				throw new ArgumentNullException("sender");
			int pos = -1;
			pos = IndexOf((T)sender);
			if(pos == -1)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", sender);
			OnItemChanging(new CollectionChangeNotifyEventArgs(this,	ChangeNotifyAction.ItemChange,args.SourceArgs ?? args,
			                                                   sender, null, pos));
		}
		void Item_ObjectChanged(object sender, ObjectChangeNotifyEventArgs args)
		{
			if(sender == null)
				throw new ArgumentNullException("sender");
			
			int pos = -1;
			if(args.MemberName != _pn)
				pos = IndexOf((T)sender);
			else
			{
				T item = (T)sender;
				if(args.NewValue == null)
				{
					IndexRebuild();
					pos = IndexOf((T)sender);
				}
				else
				{
					TIndex val = (TIndex)args.NewValue;
					TIndex valOld = (TIndex)args.OldValue;
					uint hash;
					uint hashOld;
					if(Object.Equals(val, null)) hash = 0;
					 else hash = (uint)val.GetHashCode();
					if(Object.Equals(valOld, null)) hashOld = 0;
					 else hashOld = (uint)valOld.GetHashCode();
					if(hash != hashOld)
						IndexRebuild();
					uint hash2 = hash % (uint)_index.Length;
					if(_index[hash2] == null)
						throw new PulsarException("Элемент [{0}] не принадлежит списку!", sender);
					for(int a = 0; a < _index[hash2].Length; a++)
						if(_index[hash2][a].Value1 == hash && Object.Equals(item, _arr[_index[hash2][a].Value2]))
						{
							pos = _index[hash2][a].Value2;
							break;
						}
				}
			}
			if(pos == -1)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", sender);
			OnItemChanged(new CollectionChangeNotifyEventArgs(this,	ChangeNotifyAction.ItemChange,	args.SourceArgs ?? args,
			                                                  sender, null, pos));
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region ISerializable Members
		///
		protected IndexedList(SerializationInfo info, StreamingContext ctx)
		{
			_pn = (string)info.GetValue("_pn", typeof(string));
			_ver = (uint)info.GetValue("_ver", typeof(uint));
			_si = (bool)info.GetValue("_si", typeof(bool));

			_getFunc = ReflectionHelper.GetPropertyGetMethod<T>(_pn);
			_isChangeNotify	= typeof(T).GetInterface("IObjectChangeNotify", false) != null;

			_arr = (T[])info.GetValue("_arr", typeof(T[]));
			_count = _arr.Length;
			Resize(false);
			if(_isChangeNotify)
	 		for(int a = 0; a < _count; a++)
	 		{
					((IObjectChangeNotify)_arr[a]).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a]).ObjectChanged 	+= Item_ObjectChanged;
				}

		}
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_pn",	_pn);
			info.AddValue("_ver", _ver);
			info.AddValue("_si", _si);

			T[] arr = new T[_count];
			Array.Copy(_arr,arr,_count);
			info.AddValue("_arr", arr);
		}
		#endregion

	}
}

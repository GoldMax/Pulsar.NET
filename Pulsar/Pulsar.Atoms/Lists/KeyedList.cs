using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
	#region << public interface IKeyedList : IPList >>
	/// <summary>
	/// Интерфейс списка элементов с явными ключами.
	/// </summary>
	public interface IKeyedList : IPList
	{
		/// <summary>
		/// Возвращает элемент с указанным ключем.
		/// </summary>
		/// <param name="key">Ключ искомого элемента.</param>
		object WithKey(UInt key);
		/// <summary>
		/// Возвращает клуч указанного элемента.
		/// </summary>
		/// <param name="item">Элемент, для которого определяется ключ.</param>
		/// <returns></returns>
		UInt KeyOf(object item);
		/// <summary>
		/// Определяет наличие элемента с указанным ключем.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool ContainsKey(UInt key);
		/// <summary>
		/// Возвращает элеметы списка как перечисление пар ключ-элемент
		/// </summary>
		IEnumerable<IKeyedValue> KeyItemPairs { get; }
	} 
	#endregion << public interface IKeyedList : IPList >>
	//**************************************************************************************
	#region << public class KeyedListItem<T> : KeyedValue<UInt,T> >>
	/// <summary>
	/// Класс элемента списка с явными ключами.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyedListItem<T> : KeyedValue<UInt, T>
	{
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public KeyedListItem(UInt key, T val) : base(key, val) { }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Value == null ? "" : Value.ToString();
		}
	} 
	#endregion << public class KeyedListItem<T> : KeyedValue<UInt,T> >>
	//**************************************************************************************
	/// <summary>
	/// Класс списка элементов с явными ключами.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyedList<T> : CollectionChangeNotify, IList<T>, ICollection<T>, IEnumerable<T>,
																													IPList, IList, ICollection, IEnumerable, IKeyedList, IVersionedObject
	{
		private int _idy = 0;
		[NonSerialized]
		private int _count = 0;
		private uint _ver = 0;

		private KeyedListItem<T>[] _items = null;
		private UInt[] _orders = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Ёмкость списка
		/// </summary>
		public int Capacity
		{
			get { return _items.Length; }
			set
			{
				if(value < _count)
					throw new Exception("Значение емкости не может быть меньше числа элементов!");
				if(value == _items.Length)
					return;
				Array.Resize(ref _items, value);
				if(_orders != null)
					if(value == 0)
						_orders = null;
					else
						Array.Resize(ref _orders, value);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Количество элементов в списке.
		/// </summary>
		public int Count
		{
			get { return _count; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет значение элемента по его индексу.
		/// </summary>
		/// <param name="index">Индекс элемента.</param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				if(index >= _count || index < 0)
					throw new ArgumentOutOfRangeException("index");
				if(_orders != null)
					index = _orders[index];
				return _items[index].Value;
			}
			set
			{
				if(index >= _count || index < 0)
					throw new ArgumentOutOfRangeException("index");

				int pos = _orders == null ? index : (int)_orders[index];
				T item = _items[pos].Value;

				OnItemChanging(_items[pos], value, index);

				if(KeyedValue<UInt,T>.IsChangeNotify)
				{
					_items[pos].ObjectChanging -= Item_ObjectChanging;
					_items[pos].ObjectChanged -= Item_ObjectChanged;
					_items[pos].Dispose();
				}
				_items[pos].Value = value;
				this._ver++;

				if(KeyedValue<UInt, T>.IsChangeNotify)
				{
					_items[pos].ObjectChanging += Item_ObjectChanging;
					_items[pos].ObjectChanged  += Item_ObjectChanged;
				}
				OnItemChanged(item, _items[pos], index);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Версия списка.
		/// </summary>
		public uint Version
		{
			get { return _ver; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает последний присвоенный элементу ключ. 
		/// </summary>
		public UInt Identity
		{
			get { return _idy; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает перечисление ключей.
		/// </summary>
		public IEnumerable<UInt> Keys
		{
			get
			{
				uint ver = _ver;
				for(int i = 0; i < _count; i++)
					if(ver != _ver)
						throw new Exception("Итерация невозможна, так как коллекция была изменена!");
					else
					yield return _items[i].Key;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, используется ли пользовательское упорядочивание элементов списка.
		/// </summary>
		public bool IsOrdered
		{
			get { return _orders != null; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает элеметы списка как перечисление пар ключ-элемент 
		/// </summary>
		public IEnumerable<IKeyedValue> KeyItemPairs
		{
			get
			{
				uint ver = _ver;
				for(int i = 0; i < _count; i++)
					if(ver != _ver)
						throw new Exception("Итерация невозможна, так как коллекция была изменена!");
					else
						yield return _items[i];
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public KeyedList()
		{
			_items = new KeyedListItem<T>[4];
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="collection">Исходные данные списка.</param>
		public KeyedList(IEnumerable<T> collection)
		{
			_items = new KeyedListItem<T>[collection.Count()];
			foreach(T t in collection)
				_items[_idy] = new KeyedListItem<T>(++_idy, t);
			OnDeserialized(new StreamingContext());
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="capacity">Ёмкость списка.</param>
		public KeyedList(int capacity)
		{
			_items = new KeyedListItem<T>[capacity];
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region IPList Members
		/// <summary>
		/// Проверяет версию списка. Если версия одинаковая, возвращает сам список, иначе выбрасывает исключение защиты.
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		public virtual KeyedList<T> CheckVersion(uint version)
		{
			if(_ver != version)
				throw new PulsarErrorException("Версия серверного объекта отличается от версии клиентского объекта!");
			return this;
		}
		object IVersionedObject.CheckVersion(uint version)
		{
			return CheckVersion(version);
		}
		/// <summary>
		/// Устанавливает емкость списка равным числу элементов.
		/// </summary>
		public void TrimExcess()
		{
			if(_count < _items.Length)
				this.Capacity = _count;
		}
		/// <summary>
		/// Проверяет элемент на присутствие в списке и возвращает его.
		/// </summary>
		/// <param name="item">Проверяемый элемент.</param>
		/// <returns>Если элемент в списке, возвращается сам элемент, иначе значение по умолчанию для типа.</returns>
		public T InList(T item)
		{
			if(Contains(item))
				return item;
			else
				return default(T);
		}
		object IPList.InList(object item)
		{
			return InList((T)item);
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		#pragma warning disable
		public UInt Add(T item)
		{
			KeyedListItem<T>	kv = new KeyedListItem<T>(++_idy, item);
			OnItemAdding(kv, _count);

			if(_count == _items.Length)
				this.EnsureCapacity(_count + 1);
			_items[_count++] = kv;
			this._ver++;

			if(KeyedValue<UInt, T>.IsChangeNotify)						
			{
				kv.ObjectChanging += Item_ObjectChanging;
				kv.ObjectChanged  += Item_ObjectChanged;
			}
			OnItemAdded(kv, _count-1);
			if(_orders != null)
			{
				if(_count > _orders.Length)
					Array.Resize(ref _orders, _count);
				_orders[_count-1] = _count-1;
			}
			return _idy;
		}
		/// <summary>
		/// Добавляет элемент с указаным ключем. Ключ должен быть больше текущего значения Identity.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		public void AddWithKey(UInt key, T item)
		{
			if(key <= _idy)
				throw new ArgumentException("Ключ добавляемого элемента должен быть больше значения Identity!", "key");

			KeyedListItem<T>	kv = new KeyedListItem<T>(key, item);
			OnItemAdding(kv, _count);

			if(_count == _items.Length)
				this.EnsureCapacity(_count + 1);
			_items[_count++] = kv;
			_idy = key;
			this._ver++;

			if(KeyedValue<UInt, T>.IsChangeNotify)
			{
				kv.ObjectChanging += Item_ObjectChanging;
				kv.ObjectChanged  += Item_ObjectChanged;
			}
			OnItemAdded(kv, _count-1);
			if(_orders != null)
			{
				if(_count > _orders.Length)
					Array.Resize(ref _orders, _count);
				_orders[_count - 1] = _count - 1;
			}
			return;

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Add
		/// </summary>
		/// <param name="items"></param>
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
		//-------------------------------------------------------------------------------------
		public void Clear()
		{
			if(_count == 0)
				return;
			OnObjectResetting();
			if(KeyedValue<UInt, T>.IsChangeNotify)
				for(int a = 0; a < _count; a++)
				{
					_items[a].ObjectChanging -= Item_ObjectChanging;
					_items[a].ObjectChanged 	-= Item_ObjectChanged;
					_items[a].Dispose();
				}
			_count = 0;
			_items = new KeyedListItem<T>[4];
			_orders = null;
			this._ver++;
			OnObjectResetted();
		}
		//-------------------------------------------------------------------------------------
		public bool Contains(T item)
		{
			EqualityComparer<T> eq = EqualityComparer<T>.Default;
			for(int j = 0; j < _count; j++)
				if(eq.Equals(_items[j].Value, item))
					return true;
			return false;
		}
		/// <summary>
		/// Определяет наличие элемента к указаным ключем.
		/// </summary>
		/// <param name="key">Ключ элемента</param>
		/// <returns></returns>
		public bool ContainsKey(UInt key)
		{
			return IndexOfKey(key) > -1;
		}
		//-------------------------------------------------------------------------------------
		public void CopyTo(T[] array, int arrayIndex)
		{
			for(int a = 0; a < _count; a++, arrayIndex++)
				if(_orders == null)
					array[arrayIndex] = _items[a].Value;
				else
					array[arrayIndex] = _items[_orders[a]].Value;
		}
		//-------------------------------------------------------------------------------------
		public int IndexOf(T item)
		{
			if(_orders == null)
			{
				for(int i = 0; i < _count; i++)
					if(object.Equals(_items[i].Value, item))
						return i;
			}
			else
				for(int i = 0; i < _count; i++)
					if(object.Equals(_items[_orders[i]].Value, item))
						return i;
			return -1;
		}
		public int IndexOfKey(UInt key)
		{
			if(_count == 0)
				return -1;
			int res = -1;
			int pS = 0;
			int pE = _count-1;
			int sign = 0;

			if((sign = Math.Sign(UInt.Compare(key, _items[pS].Key))) < 0)
				return -1;
			else if(sign == 0)
				res = pS;
			else if((sign = Math.Sign(UInt.Compare(key, _items[pE].Key))) > 0)
				return -1;
			else if(sign == 0)
				res = pE;
			else
				while(pE-pS > 1)
				{
					int p = (pE-pS)/2;
					p = pS + (p == 0 ? 1 : p);
					sign = Math.Sign(UInt.Compare(key, _items[p].Key));
					if(sign == 0)
					{
						res = p;
						break;
					}
					if(sign < 0)
						pE = p;
					else
						pS = p;
				}
			if(res == -1)
				return -1;

			if(_orders == null)
				return res;
			else
			{
				for(int a = 0; a < Count; a++)
					if(_orders[a] == res)
						return a;
				return -1;
			}
		}
		//-------------------------------------------------------------------------------------
		public UInt KeyOf(T item)
		{
			foreach(var i in _items)
				if(Object.Equals(i.Value, item))
					return i.Key;
			return 0;
		}
		UInt IKeyedList.KeyOf(object item)
		{
			return KeyOf((T)item);
		}
		public UInt KeyOfIndex(int index)
		{
			if(index < 0 || index >= _count)
				throw new IndexOutOfRangeException("Index за пределами диапазона!");
			return _items[_orders == null ? index : (int)_orders[index]].Key;
		}
		//-------------------------------------------------------------------------------------
		public void Insert(int index, T item)
		{
			if(index > _count)
				throw new ArgumentOutOfRangeException("index");

			KeyedListItem<T>	kv = new KeyedListItem<T>(++_idy, item);
			OnItemAdding(kv, index);

			if(_orders == null)
			{
				_orders = new UInt[_items.Length];
				for(int a = 0; a < _count; a++)
					_orders[a] = a;
			}

			if(_count == _items.Length)
				this.EnsureCapacity(_count + 1);

			_items[_count] = kv;

			if(index < _count)
				Array.Copy(_orders, index, _orders, index + 1, _count - index);
			_orders[index] = _count;

			_count++;
			this._ver++;

			if(KeyedValue<UInt, T>.IsChangeNotify)
			{
				kv.ObjectChanging += Item_ObjectChanging;
				kv.ObjectChanged  += Item_ObjectChanged;
			}
			OnItemAdded(kv, index);
		}
		//-------------------------------------------------------------------------------------
		public bool Remove(T item)
		{
			int index = this.IndexOf(item);
			if(index == -1)
				return false;

			int pos = index;
			if(_orders != null)
				for(int a = 0; a < _count; a++)
					if(_orders[a] == pos)
					{
						pos = a;
						break;
					}

			this.RemoveAt(index);
			return true;
		}
		public bool RemoveByKey(UInt key)
		{
			int pos = IndexOfKey(key);
			if(_orders != null)
				for(int a = 0; a < _count; a++)
					if(_orders[a] == pos)
					{
						pos = a;
						break;
					}
			if(pos == -1)
				return false;
			RemoveAt(pos);
			return true;
		}
		//-------------------------------------------------------------------------------------
		public void RemoveAt(int index)
		{
			if(index >= _count)
				throw new ArgumentOutOfRangeException("index");

			int pos = _orders == null ? index : (int)_orders[index];
			KeyedValue<UInt, T> kv = _items[pos];

			OnItemDeleting(kv, index);

			if(KeyedValue<UInt, T>.IsChangeNotify)
			{
				kv.ObjectChanging -= Item_ObjectChanging;
				kv.ObjectChanged  -= Item_ObjectChanged;
				kv.Dispose();
			}
			_count--;
			if(pos < _count)
				Array.Copy(_items, pos + 1, _items, pos, _count - pos);
			_items[_count] = null;
			if(_count == 0)
				_orders = null;
			if(_orders != null)
			{
				if(index < _count)
					Array.Copy(_orders, index + 1, _orders, index, _count - index);
				_orders[_count] = UInt.Empty;
				for(int a = 0; a < _count; a++)
					if(_orders[a] > pos)
						_orders[a]--;
			}
			this._ver++;
			OnItemDeleted(kv, index);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Перемещает элемент на новую позицию в списке.
		/// </summary>
		/// <param name="item">Перемещаемый элемент.</param>
		/// <param name="newIndex">Новая позиция в списке.</param>
		public void MoveItem(T item, int newIndex)
		{
			if(newIndex < 0 || newIndex >= _count)
				throw new ArgumentException("Значение выходит за границы списка!", "newIndex");
			OnObjectResetting();
			if(_orders == null)
			{
				_orders = new UInt[_items.Length];
				for(int a = 0; a < _count; a++)
					_orders[a] = a;
			}
			int index = IndexOf(item);
			if(index == -1)
				throw new ArgumentException("Элемент не пренадлежит списку!", "item");
			int pos = _orders[index];

			Array.Copy(_orders, index + 1, _orders, index, _count - index-1);
			Array.Copy(_orders, newIndex, _orders, newIndex + 1, _count - newIndex-1);
			_orders[newIndex] = pos;
			_ver++;

			OnObjectResetted();
		}
		//-------------------------------------------------------------------------------------
		public T[] ToArray()
		{
			T[] array = new T[_count];
			if(_orders == null)
				for(int a = 0; a < _count; a++)
					array[a] = _items[a].Value;
			else
				for(int a = 0; a < _count; a++)
					array[a] = _items[_orders[a]].Value;
			return array;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает элемент с указанным ключем.
		/// </summary>
		/// <param name="key">Ключ искомого элемента.</param>
		/// <returns></returns>
		public T WithKey(UInt key)
		{
			int pos = IndexOfKey(key);
			if(pos == -1)
				return default(T);
			return _items[_orders == null ? pos : (int)_orders[pos]].Value;
		}
		object IKeyedList.WithKey(UInt key)
		{
			return WithKey(key);
		}
		//-------------------------------------------------------------------------------------
		private void EnsureCapacity(int min)
		{
			if(_items.Length < min)
			{
				int num = (_items.Length == 0) ? 4 : (_items.Length * 2);
				if(num < min)
				{
					num = min;
				}
				this.Capacity = num;
			}
		}
		//-------------------------------------------------------------------------------------
		void Item_ObjectChanging(object sender, ObjectChangeNotifyEventArgs args)
		{
			if(sender == null)
				throw new ArgumentNullException("sender");
			int pos = -1;
			pos = IndexOfKey(((KeyedListItem<T>)sender).Key);
			if(pos == -1)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", sender);
			OnItemChanging(new CollectionChangeNotifyEventArgs(this,	ChangeNotifyAction.ItemChange,	args.SourceArgs ?? args,
			                            																							sender, null, pos));
		}
		void Item_ObjectChanged(object sender, ObjectChangeNotifyEventArgs args)
		{
			if(sender == null)
				throw new ArgumentNullException("sender");
			int pos = -1;
			pos = IndexOfKey(((KeyedListItem<T>)sender).Key);
			if(pos == -1)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", sender);
			OnItemChanged(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemChange, args.SourceArgs ?? args,
																																																						sender, null, pos));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Сортировка списка
		/// </summary>
		/// <param name="comparer"></param>
		public void Sort(Comparison<object> comparer = null)
		{
			OnObjectResetting();

			this._orders = null;

			KeyedValue<UInt, T>[] arr = new KeyedValue<UInt, T>[_count];
			Array.Copy(_items,arr,_count);

			PListComparer comp;
			if(comparer == null)
				comp = new PListComparer((x, y) => string.Compare(x == null ? "" : x.ToString(), y == null ? "" : y.ToString()));
			else
				comp = new PListComparer((x, y) => comparer(((KeyedValue<UInt, T>)x).Value, ((KeyedValue<UInt, T>)y).Value));
			Array.Sort(arr, comp);

			UInt[] ords = new UInt[_items.Length];
			for(int i = 0; i < _count; i++)
				ords[i] = IndexOf(arr[i].Value);
			this._orders = ords;
			_ver++;

			OnObjectResetted();
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << IList >>
		bool IList.IsFixedSize
		{
			get { return false; }
		}
		bool IList.IsReadOnly
		{
			get { return false; }
		}
		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (T)value; }
		}
		int IList.Add(object item)
		{
			this.Add((T)item);
			return this.Count - 1;
		}
		bool IList.Contains(object item)
		{
			return this.Contains((T)item);
		}
		int IList.IndexOf(object item)
		{
			return this.IndexOf((T)item);
		}
		void IList.Insert(int index, object item)
		{
			this.Insert(index, (T)item);
		}
		void IList.Remove(object item)
		{
			this.Remove((T)item);
		}
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		object ICollection.SyncRoot
		{
			get { return SyncRoot; }
		}
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			CopyTo((T[])array, arrayIndex);
		}
		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}
		#endregion << IList >>
		//-------------------------------------------------------------------------------------
		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			if(_orders == null)
				for(int i = 0; i < _count; i++)
					yield return _items[i].Value;
			else
				for(int i = 0; i < _count; i++)
					yield return _items[_orders[i]].Value;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IDeserializationCallback Members
		[System.Runtime.Serialization.OnSerializing]
		private void OnSerializing(System.Runtime.Serialization.StreamingContext cox)
		{
		 Array.Resize(ref _items, _count);
			if(_orders != null)
			 Array.Resize(ref _orders, _count);
		}
		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		{
		 _count = _items.Length;
			if(KeyedValue<UInt, T>.IsChangeNotify)
				foreach(KeyedValue<UInt,T> t in _items)
				{
					t.ObjectChanging += Item_ObjectChanging;
					t.ObjectChanged  += Item_ObjectChanged;
				}
		}
		#endregion
	}
}

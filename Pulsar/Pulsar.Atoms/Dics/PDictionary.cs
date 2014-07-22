using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
	//**************************************************************************************
	/// <summary>
	/// Класс коллекции ключ/значение с событиями о изменении.
	/// </summary>
	[Serializable]
	public class PDictionary<TKey, TValue> : CollectionChangeNotify,	IVersionedObject,
																																										IDictionary,
																																										ICollection<KeyedValue<TKey, TValue>>, ICollection,
																																										IEnumerable<KeyedValue<TKey, TValue>>, IEnumerable,
																																										ISerializable, IDisposable
	{
		private uint _ver = 0;
		[NonSerialized]
		private int _count = 0;
		
		[NonSerialized]
		KeyedValue<TKey, TValue>[] _arr = null;
		[NonSerialized]
		ElasticArray<ValuesPair<uint, UInt>>[] _index = null;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PDictionary()
		{
			_arr = new KeyedValue<TKey, TValue>[3];
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="capacity">Емкость коллекции.</param>
		public PDictionary(int capacity)
		{
			_arr = new KeyedValue<TKey, TValue>[Primes.GetPrime(capacity)];
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="items">Перечисление элементов из IDictionary&lt;`&gt;.</param>
		public PDictionary(IEnumerable<KeyValuePair<TKey,TValue>> items)
		{
		 int count = items.Count();
			_arr = new KeyedValue<TKey, TValue>[Primes.GetPrime(count)];
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
			foreach(var i in items)
			 Add(i.Key, i.Value);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region IVersionedObject Members
		/// <summary>
		/// Версия изменений.
		/// </summary>
		public uint Version
		{
			get { return _ver; }
		}
		/// <summary>
		/// Проверяет версию. Если версия одинаковая, возвращает сам словарь, иначе выбрасывает исключение защиты.
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		public virtual PDictionary<TKey, TValue> CheckVersion(uint version)
		{
			if(_ver != version)
				throw new PulsarErrorException("Версия серверного объекта отличается от версии клиентского объекта!");
			return this;
		}
		object IVersionedObject.CheckVersion(uint version)
		{
			return CheckVersion(version);
		}
		#endregion	IVersionedObject Members
		//-------------------------------------------------------------------------------------
		#region IDictionary<TKey,TValue> Members
		/// <summary>
		/// Перечисление ключей словаря.
		/// </summary>
		public virtual IEnumerable<TKey> Keys
		{
			get 
			{
				foreach(KeyedValue<TKey,TValue> i in this)
				 yield return i.Key;
			}
		}
		/// <summary>
		/// Перечисление значений словаря.
		/// </summary>
		public virtual IEnumerable<TValue> Values
		{
			get 
			{
				foreach(KeyedValue<TKey,TValue> i in this)
				 yield return i.Value;
			}
		}
		/// <summary>
		/// Добавляет пару ключ - значение
		/// </summary>
		/// <param name="key">Добавляемый ключ</param>
		/// <param name="value">Добавляемое значение</param>
		public virtual void Add(TKey key, TValue value)
		{
			Add(new KeyedValue<TKey, TValue>(key,value));
		}
		/// <summary>
		/// Добавляет пару ключ - значение
		/// </summary>
		/// <param name="item">Добавляемая пара</param>
		public virtual void Add(KeyedValue<TKey, TValue> item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			Tuple<uint,UInt> hash = CalcItemHashes(item.Key);
			CheckItemIndex(item.Key, hash);

			OnItemAdding(item, _count);
			if(_arr.Length == _count)
			{
				Resize();
				hash = CalcItemHashes(item.Key);
			}

			IndexAdd(hash, _count);
			if(KeyedValue<TKey, TValue>.IsChangeNotify)
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
		/// Добавляет перечисление элеметов в словарь.
		/// </summary>
		/// <param name="items">Перечисление элементов</param>
		public virtual void Add(IEnumerable<KeyedValue<TKey, TValue>> items)
		{
			if(items == null)
				throw new ArgumentNullException("items");
			bool was = IsChangeEventsOff;
			if(was == false)
				EventsOff();
			foreach(var i in items)
				Add(i);
			if(was == false)
				EventsOn();
		}
		/// <summary>
		/// Определяет наличие в словаре элемента с указанным ключем.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		public virtual bool ContainsKey(TKey key)
		{
			if(Object.Equals(key, null))
			 throw new ArgumentNullException("key");
			return GetItem(key) != null;
		}
		bool ICollection<KeyedValue<TKey, TValue>>.Contains(KeyedValue<TKey, TValue> item)
		{
			if(Object.Equals(item, null))
				throw new ArgumentNullException("item");
			var i = GetItem(item.Key);
			return Object.Equals(i,item);
		}
		/// <summary>
		/// Удаляет элемент с указанным ключем из словаря.
		/// </summary>
		/// <param name="key">Ключ удаляемого элемента</param>
		public virtual void Remove(TKey key)
		{
			try
			{
				if(key == null)
					throw new ArgumentNullException("key");
				Tuple<uint,UInt> hash = CalcItemHashes(key);
				if(_index[hash.Item2] == null)
					return;

				KeyedValue<TKey, TValue>	item = null;
				int index = -1;
				for(int a = 0; a < _index[hash.Item2].Length; a++)
					if(_index[hash.Item2][a].Value1 == hash.Item1 && Object.Equals(key, _arr[_index[hash.Item2][a].Value2].Key))
					{
						index = _index[hash.Item2][a].Value2;
						item = _arr[index];
						OnItemDeleting(item, index);
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
				_arr[_count-1] = null;

				if(KeyedValue<TKey, TValue>.IsChangeNotify)
				{
					((IObjectChangeNotify)item).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)item).ObjectChanged 	-= Item_ObjectChanged;
					item.Dispose();
				}
				_count--;
				_ver++;
				OnItemDeleted(item, index);
			}
			catch
			{
				
				throw;
			}
			
		}
		/// <summary>
		/// TryGetValue
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual bool TryGetValue(TKey key, out TValue value)
		{
			if(Object.Equals(key, null))
				throw new ArgumentNullException("key");
			var item = GetItem(key);
			value = item == null ? default(TValue) : item.Value;
			return item != null;
		}
		/// <summary>
		/// Получает или устанавливает значение по ключу.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		public virtual TValue this[TKey key]
		{
			get
			{
				if(Object.Equals(key, null))
					throw new ArgumentNullException("key");
				var item = GetItem(key);
				if(item == null)
					throw new Exception(String.Format("В словаре отсутствует элемент с ключем [{0}]!", key));
				return item.Value; 
			}
			set
			{
				if(Object.Equals(key, null))
					throw new ArgumentNullException("key");

				int pos = GetItemPos(key);
				if(pos == -1)
					throw new Exception(String.Format("В словаре отсутствует элемент с ключем [{0}]!", key));
				KeyedValue<TKey, TValue> item = _arr[pos];

				OnItemChanging(_arr[pos], value, pos);
				if(KeyedValue<TKey, TValue>.IsChangeNotify)
				{
					((IObjectChangeNotify)_arr[pos]).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[pos]).ObjectChanged 	-= Item_ObjectChanged;
					_arr[pos].Dispose();
				}
				TValue old = _arr[pos].Value;
				_arr[pos].Value = value;
				_ver++;

				if(KeyedValue<TKey, TValue>.IsChangeNotify)
				{
					((IObjectChangeNotify)_arr[pos]).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)_arr[pos]).ObjectChanged 	+= Item_ObjectChanged;
				}
				OnItemChanged(item, old, pos);
			}
		}
		#endregion
		#region ICollection<KeyedValue<TKey,TValue>> Members
		/// <summary>
		/// Очищает словарь
		/// </summary>
		public virtual void Clear()
		{
			OnObjectResetting();
			if(KeyedValue<TKey, TValue>.IsChangeNotify)
				for(int a = 0; a < _count; a++)
				{
					((IObjectChangeNotify)_arr[a]).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a]).ObjectChanged 	-= Item_ObjectChanged;
					_arr[a].Dispose();
				}
			Array.Clear(_arr, 0, _arr.Length);
			Array.Clear(_index, 0, _index.Length);
			_count = 0;
			_ver++;
			OnObjectResetted();
		}
		void ICollection<KeyedValue<TKey, TValue>>.CopyTo(KeyedValue<TKey, TValue>[] array, int arrayIndex)
		{
			Array.Copy(_arr,0,array,arrayIndex, Count);
		}
		/// <summary>
		/// Возвращает число элементов
		/// </summary>
		public virtual int Count
		{
			get { return _count; }
		}
		bool ICollection<KeyedValue<TKey, TValue>>.IsReadOnly { get { return false; } }
		bool ICollection<KeyedValue<TKey, TValue>>.Remove(KeyedValue<TKey, TValue> item)
		{
			if(((ICollection<KeyedValue<TKey, TValue>>)this).Contains(item))
			{
				Remove(item.Key);
				return true;
			}
			else
			 return false;
		}
		#endregion
		#region IEnumerable<KeyedValue<TKey,TValue>> Members
		/// <summary>
		/// GetEnumerator()
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyedValue<TKey, TValue>> GetEnumerator()
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
		//IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		//{
		// foreach(var i in this)
		//  yield return new KeyValuePair<TKey, TValue>(i.Key,i.Value);
		//}
		#endregion
		#region IDictionary Members
		void IDictionary.Add(object key, object value)
		{
			Add((TKey)key, (TValue)value);
		}
		void IDictionary.Clear()
		{
			Clear();
		}
		bool IDictionary.Contains(object key)
		{
			return ContainsKey((TKey)key);
		}
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new DicEnumerator(this.GetEnumerator()); 
		}
		bool IDictionary.IsFixedSize
		{
			get { return false; }
		}
		bool IDictionary.IsReadOnly
		{
			get { return false; }
		}
		ICollection IDictionary.Keys
		{
			get { return new List<TKey>(Keys); }
		}
		ICollection IDictionary.Values
		{
			get { return new List<TValue>(Values); }
		}
		void IDictionary.Remove(object key)
		{
			Remove((TKey)key);
		}
		object IDictionary.this[object key]
		{
			get { return this[(TKey)key];	}
			set	{ this[(TKey)key] = (TValue)value; }
		}
		#endregion
		#region ICollection Members
		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(_arr, 0, array, index, Count);
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
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Устанавливает наименьшую из возможных емкость, исходя из числа элементов.
		/// </summary>
		public void TrimExcess()
		{
			Resize(false);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет пару ключ - значение. Если ключ уже присутствует, устанавливает ему новое значение.
		/// </summary>
		/// <param name="key">Добавляемый ключ</param>
		/// <param name="value">Добавляемое значение</param>
		public virtual void Push(TKey key, TValue value)
		{
		 Push(new KeyedValue<TKey, TValue>(key, value));
		}
		/// <summary>
		/// Добавляет пару ключ - значение. Если ключ уже присутствует, устанавливает ему новое значение.
		/// </summary>
		/// <param name="item">Добавляемая пара</param>
		public virtual void Push(KeyedValue<TKey, TValue> item)
		{
			if(item == null)
				throw new ArgumentNullException("item");

			Tuple<uint,UInt> hash = CalcItemHashes(item.Key);
			if(_index[hash.Item2]	!= null)
				foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
					if(i.Value1 == hash.Item1 && Object.Equals(item.Key, _arr[i.Value2].Key))
					{
						var ii = _arr[i.Value2];
						OnItemChanging(ii, item.Value, hash.Item2);
						if(KeyedValue<TKey, TValue>.IsChangeNotify)
						{
							ii.ObjectChanging -= Item_ObjectChanging;
							ii.ObjectChanged 	-= Item_ObjectChanged;
							ii.Dispose();
						}
						TValue old = ii.Value;
						_arr[i.Value2].Value = item.Value;
						_ver++;

						if(KeyedValue<TKey, TValue>.IsChangeNotify)
						{
							ii.ObjectChanging += Item_ObjectChanging;
							ii.ObjectChanged 	+= Item_ObjectChanged;
						}
						OnItemChanged(item, old, i.Value2);
						return;
					}

			OnItemAdding(item, _count);
			if(_arr.Length == _count)
			{
				Resize();
				hash = CalcItemHashes(item.Key);
			}

			IndexAdd(hash, _count);
			if(KeyedValue<TKey, TValue>.IsChangeNotify)
			{
				((IObjectChangeNotify)item).ObjectChanging += Item_ObjectChanging;
				((IObjectChangeNotify)item).ObjectChanged 	+= Item_ObjectChanged;
			}
			_arr[_count] = item;

			_count++;
			_ver++;
			OnItemAdded(item, _count-1);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет пару ключ - значение, если ключ отсутствует в словаре.
		/// </summary>
		/// <param name="key">Добавляемый ключ</param>
		/// <param name="value">Добавляемое значение</param>
		public virtual bool TryAdd(TKey key, TValue value)
		{
			return TryAdd(new KeyedValue<TKey, TValue>(key, value));
		}
		/// <summary>
		/// Добавляет пару ключ - значение, если ключ отсутствует в словаре.
		/// </summary>
		/// <param name="item">Добавляемая пара</param>
		public virtual bool TryAdd(KeyedValue<TKey, TValue> item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			Tuple<uint,UInt> hash = CalcItemHashes(item.Key);
			if(GetItemPos(item.Key) != -1)
			 return false;

			OnItemAdding(item, _count);
			if(_arr.Length == _count)
			{
				Resize();
				hash = CalcItemHashes(item.Key);
			}

			IndexAdd(hash, _count);
			if(KeyedValue<TKey, TValue>.IsChangeNotify)
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
		#endregion << Methods >>
		#region << Private Methods >>
		/// <summary>
		/// Увеличивает размеры списка и индекса
		/// </summary>
		private void Resize(bool x2 = true)
		{
			Array.Resize(ref _arr, Primes.GetPrime(x2 ? _arr.Length*2 : _count));
			_index = new ElasticArray<ValuesPair<uint, UInt>>[_arr.Length];
			for(int a = 0; a < _count; a++)
				if(_arr[a] != null)
					IndexAdd(_arr[a].Key, a);
		}
		/// <summary>
		/// Возвращает кортеж hash - indexPos
		/// </summary>
		/// <returns></returns>
		private Tuple<uint, UInt> CalcItemHashes(TKey key)
		{
			if(key == null)
				throw new ArgumentNullException("item");
			uint hash;
			if(Object.Equals(key, null))
				hash = 0;
			else
				hash = (uint)key.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			return new Tuple<uint, UInt>(hash, hash2);
		}
		/// <summary>
		/// добавляет ссылку на позицию элемента в индекс
		/// </summary>
		private void IndexAdd(TKey key, UInt pos)
		{
			Tuple<uint,UInt>	hash =  CalcItemHashes(key);
			if(_index[hash.Item2] == null)
				_index[hash.Item2] = new ElasticArray<ValuesPair<uint, UInt>>(new ValuesPair<uint, UInt>(hash.Item1, pos));
			else
				_index[hash.Item2].Add(new ValuesPair<uint, UInt>(hash.Item1, pos));
		}
		private void IndexAdd(Tuple<uint, UInt> hash, UInt pos)
		{
			if(_index[hash.Item2] == null)
				_index[hash.Item2] = new ElasticArray<ValuesPair<uint, UInt>>(new ValuesPair<uint, UInt>(hash.Item1, pos));
			else
				_index[hash.Item2].Add(new ValuesPair<uint, UInt>(hash.Item1, pos));
		}
		private void CheckItemIndex(TKey key, Tuple<uint, UInt> hash)
		{
			if(_index[hash.Item2] == null)
				return;
			foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
				if(i.Value1 == hash.Item1 && Object.Equals(key, _arr[i.Value2].Key))
					throw new PulsarException("Элемент с ключем [{0}] уже присутствует в словаре!",	key);
		}
		private KeyedValue<TKey, TValue> GetItem(TKey key)
		{
			uint hash = (uint)key.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			if(_index[hash2] == null)
				return null;
			foreach(ValuesPair<uint, UInt> i in _index[hash2])
				if(i.Value1 == hash && Object.Equals(key, _arr[i.Value2].Key))
					return _arr[i.Value2];
			return null;
		}
		private UInt GetItemPos(TKey key)
		{
			uint hash = (uint)key.GetHashCode();
			uint hash2 = hash % (uint)_index.Length;
			if(_index[hash2] == null)
				return -1;
			foreach(ValuesPair<uint, UInt> i in _index[hash2])
				if(i.Value1 == hash && Object.Equals(key, _arr[i.Value2].Key))
					return i.Value2;
			return -1;
		}
		private Tuple<int,int,int,int> MaxIndexCols()
		{
		 int max = 0;
			int count2 = 0;
			int count3 = 0;
			for(int a = 0; a < _index.Length; a++)
			 if(_index[a] != null)
				{
				 if(_index[a].Length > max)
				  max = _index[a].Length;
					if(_index[a].Length == 2)
					 count2 ++;
					else if(_index[a].Length > 2)
						count3++;
				}
			return new Tuple<int,int,int,int>(count2+count3,count2, count3, max);
		}
		//-------------------------------------------------------------------------------------
		void Item_ObjectChanging(object sender, ObjectChangeNotifyEventArgs args)
		{
			OnItemChanging(new CollectionChangeNotifyEventArgs()
			{
				Action = ChangeNotifyAction.ItemChange,
				Item = sender, Sender = this, 
				SourceArgs = args.SourceArgs ?? args
			});
		}
		void Item_ObjectChanged(object sender, ObjectChangeNotifyEventArgs args)
		{
			OnItemChanged(new CollectionChangeNotifyEventArgs()
			{
				Action = ChangeNotifyAction.ItemChange,
				Item = sender, Sender = this,
				SourceArgs = args.SourceArgs ?? args
			});
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region ISerializable Members
		///
		protected PDictionary(SerializationInfo info, StreamingContext ctx)
		{
			_ver = (uint)info.GetValue("_ver", typeof(uint));
			_arr = (KeyedValue<TKey, TValue>[])info.GetValue("_arr", typeof(KeyedValue<TKey, TValue>[]));
			_count = _arr.Length;
			Resize(false);
			if(KeyedValue<TKey,TValue>.IsChangeNotify)
				for(int a = 0; a < _count; a++)
				{
					((IObjectChangeNotify)_arr[a]).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a]).ObjectChanged 	+= Item_ObjectChanged;
				}
		}
		///
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_ver", _ver);

			KeyedValue<TKey, TValue>[] arr = new KeyedValue<TKey, TValue>[_count];
			Array.Copy(_arr,arr,_count);
			info.AddValue("_arr", arr);
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IDisposable Members
		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if(_arr != null)
			{
				Array.Clear(_arr,0, _arr.Length);
				_arr = null;
			}
			if(_index != null)
			{
			 foreach(var i in _index)
				 if(i != null)
				  i.Dispose();
				Array.Clear(_index, 0, _index.Length);
				_index = null;
			}
		}

		#endregion
		//-------------------------------------------------------------------------------------
		class DicEnumerator : IDictionaryEnumerator
		{
		 private IEnumerator ie = null;
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			public DicEnumerator(IEnumerator ie)
			{
				this.ie = ie;
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
			#region IDictionaryEnumerator Members
			public DictionaryEntry Entry
			{
				get { throw new NotImplementedException(); }
			}
			public object Key
			{
				get { return ((IKeyedValue)ie.Current).Key; }
			}
			public object Value
			{
				get { return ((IKeyedValue)ie.Current).Value; }
			}
			public object Current
			{
				get { return ie.Current; }
			}
			public bool MoveNext()
			{
				return ie.MoveNext();
			}
			public void Reset()
			{
				ie.Reset();
			}
			#endregion
		}

	}
}



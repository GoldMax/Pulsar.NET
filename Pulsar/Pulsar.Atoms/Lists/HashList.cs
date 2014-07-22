	using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{/// <summary>
	/// Класс индексированного списка . Null в качестве элементов не допускается.
	/// </summary>
	/// <typeparam name="T">Тип элеметов списка.</typeparam>
	public class HashList<T> : CollectionChangeNotify, IPList, IList<T>, IList, ISerializable
	{
		private uint _ver = 0;

		[NonSerialized]
		private int _count = 0;
		/// <summary>
		/// Массив элементов
		/// </summary>
		[NonSerialized]
		protected T[] _arr = null;

		[NonSerialized]
		ElasticArray<int>[] _index = null;
		[NonSerialized]
		private bool _isChangeNotify = false;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public HashList()
		{
			if(typeof(T).GetInterface("IObjectChangeNotify", false) != null)
				_isChangeNotify = true;
			_arr = new T[Primes.GetPrime(4)];
			_index = new ElasticArray<int>[_arr.Length];
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="capacity">Ёмкость списка.</param>
		public HashList(int capacity)	
		{
			if(typeof(T).GetInterface("IObjectChangeNotify", false) != null)
				_isChangeNotify = true;
			_arr = new T[Primes.GetPrime(capacity)];
			_index = new ElasticArray<int>[_arr.Length];
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

				if(IndexContains(value))
				 throw new PulsarException("Элемент [{0}] уже присутствует в списке!",value);

				T item = _arr[pos];
				
				OnItemChanging(_arr[pos], value, pos);
				if(_isChangeNotify)
				{
					((IObjectChangeNotify)_arr[pos]).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[pos]).ObjectChanged 	-= Item_ObjectChanged;
				}
				IndexRemove(item, pos);

				_arr[pos] = value;
				IndexAdd(value, pos, false);
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
		/// Возвращает первую позицию в списке указанного элемента.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			try
			{
				uint hash = (uint)item.GetHashCode() % (uint)_index.Length;
				ElasticArray<int> ea = _index[hash];
				if (ea != null)
					for (int a = 0; a < ea.Length; a++)
						if (Object.Equals(_arr[ea[a]], item))
							return ea[a];
			}
			catch
			{
				//--- Debbuger Break --- //
				if (System.Diagnostics.Debugger.IsAttached)
					System.Diagnostics.Debugger.Break();
				//--- Debbuger Break --- //

			}
			return -1;
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

			if(IndexContains(item))
				 throw new PulsarException("Элемент [{0}] уже присутствует в списке!",item);

			OnItemAdding(item, pos);
			if(_arr.Length == _count)
				Resize();
			
			foreach(var arr in _index)
				if(arr != null)
					for(int a = 0; a < arr.Length; a++)
						if(arr[a] >= pos)
							arr[a] += 1;

			for(int a = _count-1; a >= pos; a--)
				_arr[a+1] = _arr[a];

			IndexAdd(item, pos, false);

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
		/// Вставляет элементы в список
		/// </summary>
		/// <param name="pos">Позиция вставки</param>
		/// <param name="items">Вставляемые элементы</param>
		public void Insert(int pos, IEnumerable<T> items)
		{
			if(pos < 0 || pos > _count)
				throw new ArgumentOutOfRangeException("index");
			//if(pos == _count)
			//{
			// Add(item);
			// return;
			//}
			int count = 0;
			foreach(T t in items)
			 if(IndexContains(t))
				 throw new PulsarException("Элемент [{0}] уже присутствует в списке!",t);
				else
				 count++;

			OnObjectChanging(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset));
			if(_count + count >_arr.Length)
				Resize(_count + count);
			
			foreach(var arr in _index)
				if(arr != null)
					for(int a = 0; a < arr.Length; a++)
						if(arr[a] >= pos)
							arr[a] += count;

			for(int a = _count-1; a >= pos; a--)
				_arr[a+count] = _arr[a];

			foreach(T i in items)
			{
				_arr[pos] = i;
			 IndexAdd(i, pos, false);
				pos++;
				if(_isChangeNotify)
				{
					((IObjectChangeNotify)i).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)i).ObjectChanged 	+= Item_ObjectChanged;
				}
			}
			
			_count += count;
			_ver++;
			OnObjectChanged(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset));
		}
		/// <summary>
		/// Удаляет элемент в указанной позиции
		/// </summary>
		/// <param name="pos">Позиция удаляемого элемента</param>
		public void RemoveAt(int pos)
		{
			if(pos < 0 || pos >= _count)
				throw new ArgumentOutOfRangeException("index");

			T item = _arr[pos];

			OnItemDeleting(item, pos);
			IndexRemove(item,pos); 

			foreach(var arr in _index)
				if(arr != null)
					for(int a = 0; a < arr.Length; a++)
						if(arr[a] > pos)
							arr[a] -= 1;

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
		}
		/// <summary>
		/// Удаляет элемент в указанной позиции
		/// </summary>
		/// <param name="pos">Позиция удаляемого элемента</param>
		/// <param name="count">Количество удаляемых позиций.</param>
		public void RemoveAt(int pos, int count)
		{
			if(pos < 0 || pos > _count)
				throw new ArgumentOutOfRangeException("index");
			if(pos + count > _count)
			 throw new ArgumentOutOfRangeException("count");

			OnObjectChanging(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset));

			for(int x = 0; x < _index.Length; x++)
			{
				var arr = _index[x];
				if(arr == null)
				 continue;
				for(int a = 0; a < arr.Length; a++)
					if(arr[a] >= pos && arr[a] < pos + count)
					{
						if(_isChangeNotify)
						{
							((IObjectChangeNotify)_arr[arr[a]]).ObjectChanging -= Item_ObjectChanging;
							((IObjectChangeNotify)_arr[arr[a]]).ObjectChanged 	-= Item_ObjectChanged;
						}
						arr.RemoveAt(a--);
						if(arr.Length == 0)
						 _index[x] = null;
					} 
					else if(arr[a] >= pos)
						arr[a] -= count;
			}
						

			Array.Clear(_arr, pos, count);
			Array.Copy(_arr, pos+count, _arr, pos, _count-pos-count);


			_count -= count;
			_ver++;
			OnObjectChanged(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset));

		}
		/// <summary>
		/// Добавляет элемент в список
		/// </summary>
		/// <param name="item">Добавляемый элемент</param>
		public void Add(T item)
		{
			OnItemAdding(item, _count);
			IndexAdd(item, _count, true);
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
			if(IndexContains(item))
			 return false;

			Add(item);
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
			return IndexContains(item);
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

			int pos = IndexOf(item);
			if(pos == -1)
			 return false;
						
			OnItemDeleting(item, pos);

			IndexRemove(item, pos);

			foreach(var arr in _index)
				if(arr != null)
					for(int a = 0; a < arr.Length; a++)
						if(arr[a] >= pos)
							arr[a] -= 1;

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
		/// Перемещает элемент в новую позицию в списке.
		/// </summary>
		/// <param name="item">Перемещаемый элемент.</param>
		/// <param name="newPos">Новая позиция элемента.</param>
		public void Move(T item, int newPos)
		{
		 throw new NotImplementedException("Move");
			//if(newPos < 0 || newPos >= _count)
			// throw new ArgumentOutOfRangeException("newPos");
			//if(item == null)
			// throw new ArgumentNullException("item");

			//Tuple<uint,uint,TIndex> hash = CalcItemHashes(item);
			//if(_index[hash.Item2] == null)
			// throw new PulsarException("Элемент [{0}] не принадлежит списку!", item);
			//ValuesPair<uint, UInt> pos = null;
			//foreach(ValuesPair<uint, UInt> i in _index[hash.Item2])
			// if(i.Value1 == hash.Item1 && Object.Equals(item, _arr[i.Value2]))
			// {
			//  pos = i;
			//  break;
			// }
			//if(pos == null)
			// throw new PulsarException("Элемент [{0}] не принадлежит списку!", item);
			//if(pos.Value2 == newPos)
			// return;

			//OnObjectResetting();

			//int p = pos.Value2;
			//Array.Copy(_arr, p+1, _arr, p, _count-p-1);
			//foreach(var arr in _index)
			// if(arr != null)
			//  foreach(var i in arr)
			//   if(i.Value2 > p)
			//    i.Value2--;

			//for(int a = _count-2; a >= newPos; a--)
			// _arr[a+1] = _arr[a];
			//_arr[newPos] = item;
			//foreach(var arr in _index)
			// if(arr != null)
			//  foreach(var i in arr)
			//   if(i.Value2 >= newPos)
			//    i.Value2++;

			//pos.Value2 = newPos;

			//_ver++;
			//OnObjectResetted();
		}
		#endregion
		#region IPList Members
		/// <summary>
		/// Устанавливает наименьшую из возможных емкость списка, исходя из числа элементов.
		/// </summary>
		public void TrimExcess()
		{
			Resize(_count);
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
			_index = new ElasticArray<int>[_arr.Length];
			for(int a = 0; a < _count; a++)
				IndexAdd(_arr[a],a, false);

			OnObjectResetted();
		}
		/// <summary>
		/// Сортировка списка
		/// </summary>
		/// <param name="comparer"></param>
		public void Sort(Comparison<T> comparer = null)
		{
	
			OnObjectResetting();

			if(comparer == null)
			{
				PListComparer comp = new PListComparer((x, y) => string.Compare(x == null ? "" : x.ToString(), y == null ? "" : y.ToString()));
		 	Array.Sort(_arr, 0, _count, comp);
			}
			else
			{
				Comp c = new Comp(comparer);
				Array.Sort(_arr, 0, _count, c  );
			}
			IndexRebuild();

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
		public virtual HashList<T> CheckVersion(uint version)
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
		private void Resize(int min = 0)
		{
			T[] arr = new T[Primes.GetPrime(min == 0 ? _arr.Length*2 : min)];
			Array.Copy(_arr, arr, _count);
			_arr = arr;
			_index = new ElasticArray<int>[_arr.Length];
			for(int a = 0; a < _count; a++)
				if(_arr[a] != null)
					IndexAdd(_arr[a], a, false);
		}
		private void IndexAdd(T item, int pos, bool checkUniq)
		{
			if(_count == _arr.Length)
			 Resize();

			uint hash = (uint)item.GetHashCode() % (uint)_index.Length;
			if(_index[hash] == null)
				_index[hash] = new ElasticArray<int>(0);
			else if(checkUniq && IndexContains(item, hash))
			 throw new Exception("Элемент [{0}] уже присутствует в списке!");
			
			_index[hash].Add(pos);
		}
		private bool IndexRemove(T item, int pos)
		{
			uint hash = (uint)item.GetHashCode() % (uint)_index.Length;
			ElasticArray<int> ea = _index[hash];

			if(ea != null)
				for(int a = 0; a < ea.Length; a++)
					if(Object.Equals(_arr[ea[a]], item))
					{
						ea.RemoveAt(a);
						if(ea.Length == 0)
							_index[hash] = null;
						return true;
					}
			return false;
		}
		private bool IndexContains(T item, uint hash = 0)
		{
		 if(hash == 0)
			 hash = (uint)item.GetHashCode() % (uint)_index.Length;
			ElasticArray<int> ea = _index[hash];
			 if(ea != null)
					for(int a = 0; a < ea.Length; a++)
						if(Object.Equals(_arr[ea[a]], item))
							return true;
			return false;
		}
		private void IndexRebuild()
		{
			Array.Clear(_index, 0, _index.Length);
			for(int a = 0; a < _count; a++)
				IndexAdd(_arr[a], a, false);
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
			
			int pos = IndexOf((T)sender);
			if(pos == -1)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", sender);
			OnItemChanged(new CollectionChangeNotifyEventArgs(this,	ChangeNotifyAction.ItemChange,	args.SourceArgs ?? args,
																																																					sender, null, pos));
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region ISerializable Members
		///
		protected HashList(SerializationInfo info, StreamingContext ctx)
		{
			_ver = (uint)info.GetValue("_ver", typeof(uint));
			_isChangeNotify	= typeof(T).GetInterface("IObjectChangeNotify", false) != null;

			_arr = (T[])info.GetValue("_arr", typeof(T[]));
			_count = _arr.Length;
			Resize(_count);
			if(_isChangeNotify)
				for(int a = 0; a < _count; a++)
				{
					((IObjectChangeNotify)_arr[a]).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a]).ObjectChanged 	+= Item_ObjectChanged;
				}

		}
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_ver", _ver);

			T[] arr = new T[_count];
			Array.Copy(_arr,arr,_count);
			info.AddValue("_arr", arr);
		}
		#endregion
		//*************************************************************************************
		class Comp : IComparer<T>
		{
			Comparison<T> comp = null;
			public Comp(Comparison<T> c) { comp = c; }
			public int Compare(T x, T y)
			{
				return comp(x, y);
			}
		}

	}
}

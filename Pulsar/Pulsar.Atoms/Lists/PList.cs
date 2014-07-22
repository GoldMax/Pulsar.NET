using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
	#region << public interface IPList : IList, ICollectionChangeNotify, IVersionedObject >>
	/// <summary>
	/// Интерфейс списка Пульсара
	/// </summary>
	public interface IPList : IList, ICollectionChangeNotify, IVersionedObject
	{
		/// <summary>
		/// Устанавливает емкость списка равным числу элементов.
		/// </summary>
		void TrimExcess();
		/// <summary>
		/// Проверяет элемент на присутствие в списке и возвращает его.
		/// </summary>
		/// <param name="item">Проверяемый элемент.</param>
		/// <returns>Если элемент в списке, возвращается сам элемент, иначе значение по умолчанию для типа.</returns>
		object InList(object item);
		/// <summary>
		/// Сортировка списка
		/// </summary>
		/// <param name="comparer">Метод сравнения двух элементов.</param>
		void Sort(Comparison<object> comparer = null);
	} 
	#endregion << public interface IPList : IList, ICollectionChangeNotify, IVersionedObject >>
 //**************************************************************************************
	/// <summary>
	/// Класс списка.
	/// </summary>
	/// <typeparam name="T">Тип элеметов списка.</typeparam>
	public class PList<T> : CollectionChangeNotify, IPList, IList<T>, ICollection<T>, IVersionedObject,
		IEnumerable<T>, IList, ICollection, IEnumerable
	{
	 /// <summary>
	 /// Поле версии
	 /// </summary>
		protected uint _ver = 0;
		[NonSerialized]
		private int _count = 0;
		/// <summary>
		/// Несущий массив
		/// </summary>
		protected T[] _arr = null;

		[NonSerialized]
		private bool _isChangeNotify = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Own Properties >>
		/// <summary>
		/// Ёмкость списка
		/// </summary>
		public virtual int Capacity
		{
			get { return _arr.Length; }
			set
			{
				if(value < 0)
					throw new Exception("Значение ёмкости списка должна быть не меньше 0!");
				if(value < _count)
					throw new Exception("Значение ёмкости не может быть меньше числа элементов!");
				if(value == _arr.Length)
					return;
				T[] array = new T[value];
				if(_count > 0)
					Array.Copy(_arr, 0, array, 0, _count);
				_arr = array;
			}
		}
		#endregion << Own Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PList()
		{
			if(typeof(T).GetInterface("IObjectChangeNotify", false) != null)
				_isChangeNotify = true;
			_arr = new T[4];
		}
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		/// <param name="defaultInit">Определяет, следует ли создавать несущий массив.</param>
		protected PList(bool defaultInit)
		{
			if(typeof(T).GetInterface("IObjectChangeNotify", false) != null)
				_isChangeNotify = true;
			if(defaultInit)
				_arr = new T[4];
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="capacity">Ёмкость списка.</param>
		public PList(int capacity)
		{
			if(capacity < 0)
				throw new Exception("Значение ёмкости списка должна быть не меньше 0!");
			if(typeof(T).GetInterface("IObjectChangeNotify", false) != null)
				_isChangeNotify = true;
			_arr = new T[capacity];
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="items">Перечисление элементов списка</param>
		public PList(IEnumerable<T> items)
		{
			if(typeof(T).GetInterface("IObjectChangeNotify", false) != null)
				_isChangeNotify = true;
			if(items == null)
			 _count = 0;
			else
			 _count = items.Count();

			_arr = new T[_count];
			int a = 0;
			foreach(T t in items)
			{
			 _arr[a++] = t;
				if(_isChangeNotify)
				{
					((IObjectChangeNotify)_arr[a-1]).ObjectChanging	+= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a-1]).ObjectChanged 	+= Item_ObjectChanged;

				}

			}
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << IPList Methods >>
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
		public virtual PList<T> CheckVersion(uint version)
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
		public virtual void TrimExcess()
		{
			if(_count == _arr.Length)
			 return;
			Array.Resize<T>(ref _arr, _count);
		}
		/// <summary>
		/// Проверяет элемент на присутствие в списке и возвращает его.
		/// </summary>
		/// <param name="item">Проверяемый элемент.</param>
		/// <returns>Если элемент в списке, возвращается сам элемент, иначе значение по умолчанию для типа.</returns>
		public virtual T InList(T item)
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
			PListComparer comp;
			if(comparer == null)
			 comp = new PListComparer((x, y) => string.Compare(x == null ? "" : x.ToString(), y == null ? "" : y.ToString()));
			else
			 comp = new PListComparer(comparer); 

			OnObjectResetting();

			Array.Sort(_arr,0, _count, comp);

			OnObjectResetted();
		}
		#endregion << IPList Methods >>
		//-------------------------------------------------------------------------------------
		#region IList<T> Members
		/// <summary>
		/// Возвращает или устанавливает элемент по его позиции.
		/// </summary>
		/// <param name="pos">Позиция элемента</param>
		/// <returns></returns>
		public virtual T this[int pos]
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

				T item = _arr[pos];
				OnItemChanging(item, value, pos);

				if(_isChangeNotify)
				{
					((IObjectChangeNotify)_arr[pos]).ObjectChanging -= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[pos]).ObjectChanged 	-= Item_ObjectChanged;
				}

				_arr[pos] = value;
				_ver++;

				if(_isChangeNotify)
				{
					((IObjectChangeNotify)_arr[pos]).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)_arr[pos]).ObjectChanged 	+= Item_ObjectChanged;
				}
				OnItemChanged(item, _arr[pos], pos);
			}
		}
		/// <summary>
		/// Возвращает первую позицию в списке указанного элемента.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual int IndexOf(T item)
		{
			for(int a = 0; a < _count; a++)
			 if(object.Equals(_arr[a], item))
				 return a;
			return -1;
		}
		/// <summary>
		/// Возвращает все позиции в списке указанного элемента.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual int[] IndexOfAll(T item)
		{
			List<int> res = new List<int>(_count);
			for(int a = 0; a < _count; a++)
			 if(object.Equals(_arr[a], item))
				 res.Add(a);
			return res.ToArray();
		}
		/// <summary>
		/// Вставляет элемент в список.
		/// </summary>
		/// <param name="pos">Позиция вставки</param>
		/// <param name="item">Вставляемый элемент</param>
		public virtual void Insert(int pos, T item)
		{
			if(pos < 0 || pos > _count)
				throw new ArgumentOutOfRangeException("index");
			if(pos == _count)
			{
				Add(item);
				return;
			}
			
			OnItemAdding(item, pos);

			if(_arr.Length == _count)
				IncreaseSize();
			for(int a = _count-1; a >= pos; a--)
				_arr[a+1] = _arr[a];

			_arr[pos] = item;
			_count++;
			_ver++;
			if(_isChangeNotify)
			{
				((IObjectChangeNotify)_arr[pos]).ObjectChanging	+= Item_ObjectChanging;
				((IObjectChangeNotify)_arr[pos]).ObjectChanged 	+= Item_ObjectChanged;

			}
			OnItemAdded(item, pos);
		}
		/// <summary>
		/// Удаляет элемент в указанной позиции
		/// </summary>
		/// <param name="pos">Позиция удаляемого элемента</param>
		public virtual void RemoveAt(int pos)
		{
			if(pos < 0 || pos >= _count)
				throw new ArgumentOutOfRangeException("index");
			
			T item = _arr[pos];
			OnItemDeleting(item, pos);

			if(_isChangeNotify)
			{
				((IObjectChangeNotify)_arr[pos]).ObjectChanging -= Item_ObjectChanging;
				((IObjectChangeNotify)_arr[pos]).ObjectChanged  -= Item_ObjectChanged;
			}

			Array.Copy(_arr, pos+1, _arr, pos, _count-pos-1);
			_arr[_count-1] = default(T);

			_count--;
			_ver++;
			OnItemDeleted(item, pos);
		}
		/// <summary>
		/// Добавляет элемент в список
		/// </summary>
		/// <param name="item">Добавляемый элемент</param>
		public virtual void Add(T item)
		{
			OnItemAdding(item, _count);
			if(_arr.Length == _count)
				IncreaseSize();

			_arr[_count] = item;
			if(_isChangeNotify)
			{
				((IObjectChangeNotify)_arr[_count]).ObjectChanging	+= Item_ObjectChanging;
				((IObjectChangeNotify)_arr[_count]).ObjectChanged 	+= Item_ObjectChanged;
			}

			_count++;
			_ver++;
			OnItemAdded(item, _count-1);
		}
		/// <summary>
		/// Добавляет перечисление элеметов в список.
		/// </summary>
		/// <param name="items">Перечисление элементов</param>
		public virtual void Add(IEnumerable<T> items)
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
		public virtual void Clear()
		{
		 OnObjectResetting();
			if(_isChangeNotify)
				for(int a = 0; a < _count; a++)
				{
					((IObjectChangeNotify)_arr[a]).ObjectChanging	-= Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a]).ObjectChanged 	-= Item_ObjectChanged;
				}
			Array.Clear(_arr, 0, _arr.Length);
			_count = 0;
			_ver++;
		 OnObjectResetted();
		}
		/// <summary>
		/// Определяет, принадлежит ли элемент списку
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}
		/// <summary>
		/// Копирует элементы списка в массив.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(_arr, 0, array, arrayIndex, _count);
		}
		/// <summary>
		/// Возвращает количесто элеметов в списке
		/// </summary>
		public virtual int Count
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
		public virtual bool Remove(T item)
		{
			int pos = IndexOf(item);
			if(pos > -1)
			{
			 RemoveAt(pos); 
			 return true;
			}
			return false;
		}
		/// <summary>
		/// Удаляет все вхождения элемента из списка
		/// </summary>
		/// <param name="item">Удаляемый элемент</param>
		/// <returns></returns>
		public virtual void RemoveAll(T item)
		{
			bool was = IsChangeEventsOff;
			if(was == false)
			 EventsOff();
			int[] poses = IndexOfAll(item);
			Array.Reverse(poses);
			foreach(int i in poses)
				RemoveAt(i);
			if(was == false)
				EventsOn();
		}
		/// <summary>
		/// Перемещает элемент в новую позицию в списке.
		/// </summary>
		/// <param name="item">Перемещаемый элемент.</param>
		/// <param name="newPos">Новая позиция элемента.</param>
		public virtual void Move(T item, int newPos)
		{
			if(newPos < 0 || newPos >= _count)
				throw new ArgumentOutOfRangeException("newPos");

		 int pos = IndexOf(item);
			if(pos == -1)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", item);
			if(pos == newPos)
				return;

			OnObjectResetting();

			if(pos < _count-1)
			 Array.Copy(_arr, pos+1, _arr, pos, _count-pos-1);

			for(int a = _count-2; a >= newPos; a--)
				_arr[a+1] = _arr[a];
			_arr[newPos] = item;

			_ver++;
			OnObjectResetted();
		}
		#endregion
		#region IEnumerable<T> Members
		/// <summary>
		/// GetEnumerator() 
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerator<T> GetEnumerator()
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
			get { return Count; }
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
		#region << Private Methods >>
		/// <summary>
		/// Увеличивает размеры несущего массива
		/// </summary>
		/// <param name="newSize">Новый размер массива (-1 - увеличение на 50%)</param>
		protected virtual void IncreaseSize(int newSize = -1)
		{
		 if(newSize == -1)
			 Array.Resize(ref _arr, _arr.Length < 4 ? 4 : _arr.Length + _arr.Length/2);
			else
			 Array.Resize(ref _arr, newSize);
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
			OnItemChanging(new CollectionChangeNotifyEventArgs() { Action = ChangeNotifyAction.ItemChange,
			                                                       Item = sender, ItemIndex = pos,
																																																										Sender = this,
																																																										SourceArgs = args.SourceArgs ?? args
																																																								}); 
		}
		void Item_ObjectChanged(object sender, ObjectChangeNotifyEventArgs args)
		{
			if(sender == null)
				throw new ArgumentNullException("sender");
			int pos = -1;
			pos = IndexOf((T)sender);
			if(pos == -1)
				throw new PulsarException("Элемент [{0}] не принадлежит списку!", sender);
			OnItemChanged(new CollectionChangeNotifyEventArgs() { Action = ChangeNotifyAction.ItemChange,
			                                                       Item = sender, ItemIndex = pos,
																																																										Sender = this,
																																																										SourceArgs = args.SourceArgs ?? args
																																																								}); 
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region ISerializable Members
		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		{
			_count = _arr.Length;
			_isChangeNotify = typeof(T).GetInterface("IObjectChangeNotify", false) != null;
			if(_isChangeNotify)
				for(int a = 0; a < _count; a++)
				{
					((IObjectChangeNotify)_arr[a]).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)_arr[a]).ObjectChanged += Item_ObjectChanged;
				}
		}
		[System.Runtime.Serialization.OnSerializing]
		private void OnSerializing(System.Runtime.Serialization.StreamingContext cox)
		{
			if(_count < _arr.Length)
				Array.Resize<T>(ref _arr, _count); 
		}
		#endregion
	}
	//**************************************************************************************
	internal class PListComparer : IComparer
	{
		private Comparison<object> _meth = null;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public PListComparer(Comparison<object> meth)
		{
			_meth = meth;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region IComparer Members
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y)
		{
			return _meth(x,y);
		}
		#endregion
	}
}

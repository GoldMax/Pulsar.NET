using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public interface IPulsarCluster >>
	/// <summary>
	/// Интерфейс кластера.
	/// </summary>
	public interface IPulsarCluster
	{
		/// <summary>
		/// Основной элемент кластера.
		/// </summary>
		object Main { get; }
		/// <summary>
		/// Общее количество элементов кластера, включая основной элемент.
		/// </summary>
		int Count { get; }
		/// <summary>
		/// Количество подчиненных элементов кластера.
		/// </summary>
		int SubsCount { get; }
		/// <summary>
		/// Вовзращает перечисление подчиненных элементов
		/// </summary>
		IEnumerable Subordinates { get; }
	} 
	#endregion << public interface IPulsarCluster >>
	//*************************************************************************************
	/// <summary>
	/// Класс сгруппированных сущностей, одна из которых является главной.
	/// </summary>
	[TypeConverter(typeof(PulsarClusterTypeConverter))]
	public class PulsarCluster<T> : CollectionChangeNotify, IList<T>, IEnumerable<T>, IList, IEnumerable,
			IComparable, ICloneable, IPulsarCluster
	{
		private T[] _t = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет, является ли кластер пустым.
		/// </summary>
		public bool IsEmpty 
		{ 
			get { return _t == null; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Основной элемент кластера.
		/// </summary>
		public T Main 
		{ 
			get 
			{
				if(_t == null)
					throw new Exception("Кластер является пустым!");
				return _t[0];
			} 
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Общее количество элементов кластера, включая основной элемент.
		/// </summary>
		public int Count
		{
			get { return _t == null ? 0 : _t.Length; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Количество подчиненных элементов кластера.
		/// </summary>
		public int SubsCount
		{
			get { return _t == null ? 0 : _t.Length - 1; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Вовзращает перечисление подчиненных элементов
		/// </summary>
		public IEnumerable<T> Subordinates
		{
			get
			{
				if(_t != null)
					for(int a = 1; a < _t.Length; a++)
						yield return _t[a];
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает и устанавливает элемент по его индексу.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				if(_t == null)
					throw new Exception("Кластер является пустым!");
				return _t[index];
			}
			set
			{            
				if(_t == null)
					_t = new T[index+1];
				if(index < 0 || index > _t.Length -1)
					throw new IndexOutOfRangeException();
				OnItemChanging(_t[index], value, index);
				T was = _t[index];
				_t[index] = value;
				OnItemChanged(was, value, index);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarCluster() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public PulsarCluster(T item)
		{
			_t = new T[1] { item };
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public PulsarCluster(IEnumerable<T> items)
		{
			_t = items.ToArray();
			if(_t != null && _t.Length == 0)
				_t = null;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Перемещает элемент из одной позиции в другую.
		/// </summary>
		/// <param name="index">Текущая позиция элемента</param>
		/// <param name="newIndex">Новая позиция элемента.</param>
		public void Move(int index, int newIndex)
		{
			if(index < 0 || _t == null || index > _t.Length)
				throw new IndexOutOfRangeException("index");
			if(newIndex < 0 || newIndex > _t.Length)
				throw new IndexOutOfRangeException("newIndex");
			if(index == newIndex)
				return;
			OnObjectResetting();
			T x = _t[index];
			T[] arr = new T[_t.Length];
			for(int a = 0, b = 0; a < _t.Length; a++, b++) 
				if(a == index)
					b--;
				else if(a == newIndex)
				{
					if(index > newIndex)
					{
						arr[b++] = x;
						arr[b] = _t[a];
					}
					else
					{
						arr[b++] = _t[a];
						arr[b] = x;
					}
				}
				else
					arr[b] = _t[a];
			_t = arr;
			OnObjectResetted();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает подчинненый элемент в качестве основного. 
		/// Если элемент отсутствует в кластере, он добавляется.
		/// </summary>
		/// <param name="item">Подчинненый элемент.</param>
		public void SetAsMain(T item)
		{
			int pos = IndexOf(item);
			if(pos == -1)
				Insert(0,item);
			else
				Move(pos,0);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ToString
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _t == null ? "(нет данных)" : _t[0].ToString();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Переустанавливает содержимое кластера из другого кластера.
		/// </summary>
		/// <param name="other">Кластер - источник.</param>
		public void Reset(PulsarCluster<T> other)
		{
			var arg = new ObjectChangeNotifyEventArgs(this,	ChangeNotifyAction.ObjectReset,null,other,this);
			OnObjectResetting(arg);
			if(other._t == null)
				this._t = null;
			else
			{
				_t = new T[other._t.Length];
				Array.Copy(other._t,_t,_t.Length);
			}
			OnObjectResetted(arg);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IList<T> Members
		///
		public int IndexOf(T item)
		{
			if(_t == null)
				return -1;
			for(int a = 0; a < _t.Length; a++)
				if(Object.Equals(item, _t[a]))
					return a;
			return -1;
		}
		//-------------------------------------------------------------------------------------
		///
		public void Insert(int index, T item)
		{
			if(index < 0 || (_t != null && index > _t.Length))
				throw new IndexOutOfRangeException();
			OnItemAdding(item,index);
			if(_t == null)
				_t = new T[index+1];
			else
			{
				T[] arr = new T[_t.Length+1];
				if(index != 0)
					Array.Copy(_t,0,arr,0,index);
				if(index != _t.Length)
					Array.Copy(_t,index,arr,index+1, _t.Length-index);
				_t = arr;
			}
			_t[index] = item;
			OnItemAdded(item, index);
		}
		//-------------------------------------------------------------------------------------
		///
		public void RemoveAt(int index)
		{
			if(index < 0 || _t == null || index > _t.Length)
				throw new IndexOutOfRangeException();
			OnItemDeleting(_t[index],index);
			T was = _t[index];
			T[] arr = new T[_t.Length-1];
			if(index != 0)
				Array.Copy(_t,0,arr,0,index);
			if(index != _t.Length)
				Array.Copy(_t,index + 1,arr,index, arr.Length-index);
			if(arr.Length == 0)
				_t = null;
			else
				_t = arr;
			OnItemDeleted(was,index);
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region ICollection<T> Members
		///
		public void Add(T item)
		{
			if(_t == null)
				Insert(0,item);
			else
				Insert(_t.Length, item);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Очищает все элементы кластера (в том числе и основной).
		/// </summary>
		public void Clear()
		{
			OnObjectResetting();
			_t = null;
			OnObjectResetted();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Очищает подчиненные элементы кластера.
		/// </summary>
		public void ClearSubordinates()
		{
			OnObjectResetting();
			if(_t == null)
				return;
			_t = new T[1] { _t[0] };
			OnObjectResetted();
		}
		//-------------------------------------------------------------------------------------
		///
		public bool Contains(T item)
		{
			if(_t == null)
				return false;
			foreach(T i in _t)
				if(Object.Equals(i, item))
					return true;
			return false; 
		}
		//-------------------------------------------------------------------------------------
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			if(_t != null)
				_t.CopyTo(array, arrayIndex);
		}
		//-------------------------------------------------------------------------------------
		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
		//-------------------------------------------------------------------------------------
		///
		public bool Remove(T item)
		{
			if(_t == null)
				return false;
			for(int a = 0; a < _t.Length; a++)
				if(Object.Equals(item, _t[a]))
				{
					RemoveAt(a);
					return true;
				}
			return false;
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IEnumerable<T> Members
		/// <summary>
		/// GetEnumerator
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			if(_t != null)
				foreach(T i in _t)
					yield return i;
		}
		//-------------------------------------------------------------------------------------
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (_t ?? new T[0]).GetEnumerator();
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IList Members
		int IList.Add(object value)
		{
			Add((T)value);
			return _t.Length;
		}
		void IList.Clear()
		{
			Clear();
		}
		/// <summary>
		/// Contains
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(object value)
		{
			if(value is T == false)
				return false;
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
		void IList.RemoveAt(int index)
		{
			RemoveAt(index);
		}
		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (T)value; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region ICollection Members
		void ICollection.CopyTo(Array array, int index)
		{
			if(_t != null)
				_t.CopyTo(array, index);
		}
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		object ICollection.SyncRoot
		{
			get { return null; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IComparable Members
		int IComparable.CompareTo(object obj)
		{
			if(obj == null)
				return 1;
			if(obj is PulsarCluster<T> == false)
				return -1;
			T x = this.Main;
			T y = ((PulsarCluster<T>)obj).Main;
			if(x == null && y == null)
				return 0;
			if(x == null && y != null)
				return -1;
			if(x != null && y == null)
				return 1;
			if(x.GetType() == y.GetType() && x is IComparable)
				return ((IComparable)x).CompareTo(y);
			return StringComparer.CurrentCulture.Compare(x.ToString(), y.ToString());
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region ICloneable Members
		/// <summary>
		/// Клонирует объект.
		/// </summary>
		/// <returns></returns>
		public PulsarCluster<T> Clone()
		{
			return new PulsarCluster<T>(this);
		}
		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion
		//-------------------------------------------------------------------------------------
		#region IPulsarCluster Members
		object IPulsarCluster.Main
		{
			get { return Main; }
		}
		int IPulsarCluster.Count
		{
			get { return Count; }
		}
		int IPulsarCluster.SubsCount
		{
			get { return SubsCount; }
		}
		IEnumerable IPulsarCluster.Subordinates
		{
			get { return Subordinates; }
		}
		#endregion
	}
	//*************************************************************************************
	public class PulsarClusterTypeConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return destType == typeof(string);
		}
		public override object ConvertTo(ITypeDescriptorContext context, 
																			System.Globalization.CultureInfo culture,object value, Type destType)
		{
			return value == null ? "(null)" : value.ToString();
		}
	}

}

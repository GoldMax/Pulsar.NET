using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс индекса элементов. Непотокобезопасный.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class HashIndexNotify<T> : CollectionChangeNotify, ICollection<T>, IEnumerable<T>, ISerializable, IDisposable
	where T : class
	{
		private static bool _isNotify = typeof(T).GetInterface("IObjectChangeNotify", false) != null;

		[NonSerialized]
		private T[][] _map = new T[3][];
		[NonSerialized]
		private int _count = 0;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get { return _count; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public HashIndexNotify()	{  }
		/// <summary>
		/// Конструктор десериализации
		/// </summary>
		/// <param name="information"></param>
		/// <param name="context"></param>
		protected HashIndexNotify(SerializationInfo info, StreamingContext context)
		{
			T[] arr = (T[])info.GetValue("arr", typeof(T[]));
			if(arr != null)
				foreach(T t in arr)
					Add(t);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Добавляет элемент в индекс.
		/// </summary>
		/// <param name="item">Добавляемый элемент</param>
		public void Add(T item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			OnItemAdding(item);
			_Add(item);
			_count++;
			if(_isNotify)
			{
			 ((IObjectChangeNotify)item).ObjectChanging += Item_ObjectChanging;
			 ((IObjectChangeNotify)item).ObjectChanged 	+= Item_ObjectChanged;
			}
			OnItemAdded(item);
		}
		/// <summary>
		/// Определяет принадлежность элемента индексу.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			uint hash = (uint)item.GetHashCode() % (uint)_map.Length;
			if(_map[hash] == null)
				return false;

			T[] arr = _map[hash];
			for(int a = 0; a < arr.Length; a++)
				if(Object.Equals(arr[a], item))
					return true;
			return false;
		}
		/// <summary>
		/// Очищает индекс.
		/// </summary>
		public void Clear()
		{
			OnObjectResetting();
			_map = new T[3][];
			_count = 0; 
			OnObjectResetted();
		}
		/// <summary>
		/// Dispose()
		/// </summary>
		public void Dispose()
		{
			Clear();
		}
		/// <summary>
		/// Удаляет элемент из индекса
		/// </summary>
		/// <param name="item">Удаляемый элемент</param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			uint hash = (uint)item.GetHashCode() % (uint)_map.Length;
			T[] arr = _map[hash];
			if(arr == null)
				return false;

			int isAlive = 0;
			bool has = false;
			for(int a = 0; a < arr.Length; a++)
			{
				if(Object.Equals(item, arr[a]))
				{
					has = true;
					OnItemDeleting(arr[a]);
					T old = arr[a];
					arr[a] = null;
					_count--;
					OnItemDeleted(old);
				}
				if(arr[a] != null)
					isAlive++;
			}
			if(isAlive == 0)
				_map[hash] = null;
			if(_count == 0)
			{
				_map = new T[3][];
				_count = 0; 
			}
			return has;
		}
		//-------------------------------------------------------------------------------------
		private void _Add(T item)
		{
			uint hash = (uint)item.GetHashCode() % (uint)_map.Length;
			if(_map[hash] == null)
			{
				_map[hash] = new T[1];
				_map[hash][0] = item;
				return;
			}
			//--
			T[] arr = _map[hash];
			int pos = -1;
			for(int a = 0; a < arr.Length; a++)
				if(arr[a] == null)
				{
					pos = a;
					break;
				}

			if(pos > -1)
			{
				arr[pos] = item;
				return;
			}
			if(_count >= _map.Length)
			{
				RebuildMap();
				_Add(item);
				return;
			}
			_map[hash] = new T[arr.Length*2];
			Array.Copy(arr, _map[hash], arr.Length);
			_map[hash][arr.Length] = item;
		}
		private void RebuildMap()
		{
			T[][] map	= _map;
			_map = new T[Primes.GetPrime(_map.Length)][];
			foreach(var arr in map)
				if(arr != null)
					foreach(T s in arr)
						if(s != null)
							_Add(s);
		}
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if(_count == 0)
			 info.AddValue("arr", null);
			else
			 info.AddValue("arr", this.ToArray());
		}
		//-------------------------------------------------------------------------------------
		void Item_ObjectChanging(object sender, ObjectChangeNotifyEventArgs args)
		{
			OnItemChanging(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemChange, args.SourceArgs ?? args,
																																																						sender, null, -1));
		}
		void Item_ObjectChanged(object sender, ObjectChangeNotifyEventArgs args)
		{
			OnItemChanged(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemChange, args.SourceArgs ?? args,
																																																					sender, null, -1));
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region ICollection<T> Members
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			foreach(T t in this)
			 array[arrayIndex++] = t;
		}
		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
		#endregion
		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			foreach(var l in _map)
				if(l != null)
					foreach(var x in l)
						if(x != null)
							yield return x;
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		//-------------------------------------------------------------------------------------
		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		{
			if(_isNotify)
				foreach(T t in this)
				{
					((IObjectChangeNotify)t).ObjectChanging += Item_ObjectChanging;
					((IObjectChangeNotify)t).ObjectChanged += Item_ObjectChanged;
				}

		}
	}
}

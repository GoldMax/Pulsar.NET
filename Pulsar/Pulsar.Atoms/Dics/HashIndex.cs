using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// .
 /// </summary>
 /// <typeparam name="T"></typeparam>
	public class HashIndex<T> : ICollection<T>, IEnumerable<T>, ISerializable, IDisposable 
	where T: class
	{
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
		public HashIndex()
		{

		}
		/// <summary>
		/// Конструктор десериализации
		/// </summary>
		/// <param name="information"></param>
		/// <param name="context"></param>
		protected HashIndex(SerializationInfo info, StreamingContext context)
		{
			T[] arr = (T[])info.GetValue("arr", typeof(T[]));
			if(arr != null)
			 foreach(T t in arr)
				 Add(t); 
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		public void Add(T item)
		{
			if(item == null)
			 throw new ArgumentNullException("item");
			_Add(item);
			_count++;
		}
		public bool Contains(T item)
		{
			if(item == null)
			 throw new ArgumentNullException("item");
			uint hash = (uint)item.GetHashCode() % (uint)_map.Length;
			if(_map[hash] == null)
			 return false;

			T[] arr = _map[hash];
			for(int a = 0; a < arr.Length; a++)
				if(Object.Equals(arr[a],item))
				 return true;
			return false;
		}
		public void Clear()
		{
			_map = new T[3][]; 
			_count = 0;
		}
		public void Dispose()
		{
			Clear();
		}
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
					arr[a] = null;
					_count--;
				}
				if(arr[a] != null)
					isAlive++;
			}
			if(isAlive == 0)
				_map[hash] = null;
			if(_count == 0)
			 Clear();
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
	}
}

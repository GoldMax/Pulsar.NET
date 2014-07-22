using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс очереди, поддерживающей извещения об изменении.
	/// </summary>
	/// <typeparam name="T">Тип элемента очереди.</typeparam>
	[Serializable]
	public class PulsarQueue<T> : CollectionChangeNotify, IEnumerable<T>
	{
		private List<T> list = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает количество элементов в очереди.
		/// </summary>
		public int Count
		{
			get { return list.Count; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает элемент очереди по его индексу
		/// </summary>
		/// <param name="index">Индекс элемента.</param>
		/// <returns></returns>
		public T this[int index]
		{
			get { return list[index]; }
			set 
			{
				OnItemChanging(list[index], value, index); 
				T old = list[index];
				list[index] = value; 
				OnItemChanged(old,list[index],index);
				
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarQueue()
		{
			list = new List<T>();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="capacity">Начальная ёмкость очереди.</param>
		public PulsarQueue(int capacity)
		{
			list = new List<T>(capacity);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="items">Элементы очереди.</param>
		public PulsarQueue(IEnumerable<T> items)
		{
			list = new List<T>(items);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Очищает очередь.
		/// </summary>
		public void Clear()
		{
			OnObjectResetting();
			list.Clear();
			OnObjectResetted();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли очередь указанный элемент.
		/// </summary>
		/// <param name="item">Определяемый элемент.</param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return list.Contains(item);
		}
		/// <summary>
		/// Определяет, содержит ли очередь элемент по критерию.
		/// </summary>
		/// <param name="method">Метод, реализующий критерий равенства.</param>
		/// <returns></returns>
		public bool Contains(Func<T, bool> method)
		{
			foreach(T i in list)
				if(method(i))
					return true;
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет элемент в очередь.
		/// </summary>
		/// <param name="item">Добавляемый элемент.</param>
		public void Enqueue(T item)
		{
			if(item == null)
				throw new Exception("Попытка добавления в очередь элемента со значением null!");
			OnItemAdding(item);
			list.Add(item);
			OnItemAdded(item,list.IndexOf(item));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Извлекает первый элемент из очереди.
		/// </summary>
		/// <returns>Возвращает извлеченный элемент. 
		/// Если очередь пуста, null - для классов, для остальных исключение. </returns>
		public T Dequeue()
		{
			if(list.Count == 0)
				throw new Exception("Попытка извлечь элемент из пустой очереди!");
			T res = list[0];
			OnItemDeleting(res,0);
			list.RemoveAt(0);
			OnItemDeleted(res,0);
			return res;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает первый в очереди элемент, не извлекая его.
		/// </summary>
		/// <returns></returns>
		public T Peek()
		{
			if(list.Count == 0)
				throw new Exception("Попытка извлечь элемент из пустой очереди!");
			return list[0];
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Перемещает элемент в очереди.
		/// </summary>
		/// <param name="item">Перемещаемый элемент.</param>
		/// <param name="pos">Новая позиция элемента в очереди.</param>
		public void Move(T item, int pos)
		{
			if(list.Contains(item) == false)
				throw new ArgumentException("Попытка переместить элемент, не находящийся в очереди!", "item");
			if(pos < 0 || pos > list.Count)
				throw new ArgumentException("Попытка переместить элемент за пределы очереди!", "pos");

			list.Remove(item);
			list.Insert(pos, item);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет элемент из очереди в указанной позиции
		/// </summary>
		/// <param name="pos">Позиция элемента.</param>
		public void RemoveAt(int pos)
		{
			T res = list[pos];
			OnItemDeleting(res, pos);
			list.RemoveAt(pos); 
			OnItemDeleted(res,pos);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет элемент из очереди
		/// </summary>
		/// <param name="item">Элемент очереди.</param>
		public void Remove(T item)
		{
			int pos = list.IndexOf(item);
			if(pos == -1)
				return;
			OnItemDeleting(item, pos);
			list.Remove(item);
			OnItemDeleted(item,pos);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает позицию элемента в очереди.
		/// </summary>
		/// <param name="item">Элемент очереди.</param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			return list.IndexOf(item);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IEnumerable<T> Members
		/// <summary>
		/// GetEnumerator()
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			return list.GetEnumerator();
		}
		//-------------------------------------------------------------------------------------
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}
		#endregion
	}
}

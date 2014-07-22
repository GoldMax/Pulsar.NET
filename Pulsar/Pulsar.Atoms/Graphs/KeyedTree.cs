using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	//*************************************************************************************
	#region << public interface IKeyedTreeItem : ITreeItem >>
	/// <summary>
	/// Интерфейс элемента дерева
	/// </summary>
	public interface IKeyedTreeItem : ITreeItem
	{
		/// <summary>
		/// Возвращает ключ элемента дерева.
		/// </summary>
		UInt Key { get; }
	}
	#endregion << public interface IKeyedTreeItem : ITreeItem >>
	//*************************************************************************************
	#region << public class KeyedTreeItem<T> : TreeItem<T>, IKeyedTreeItem >>
	/// <summary>
	/// Класс элемента дерева с явным ключем.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyedTreeItem<T> : TreeItem<T>, IKeyedTreeItem
	{
		private UInt _key = 0;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает ключ элемента дерева.
		/// </summary>
		public UInt Key
		{
			get { return _key; }
			internal set { _key = value; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public KeyedTreeItem(T obj) : base(obj) { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public KeyedTreeItem(T obj, string altText) : base(obj, altText) { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		internal KeyedTreeItem(UInt key, T obj, string altText)
			: base(obj, altText)
		{
			_key = key;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	} 
	#endregion << 	public class KeyedTreeItem<T> : TreeItem<T>, IKeyedTreeItem >>
	//*************************************************************************************
	/// <summary>
	/// Интерфейс класса дерева с явным ключем у элементов
	/// </summary>
	public interface IKeyedTree	: ITree
	{
		/// <summary>
		/// Возвращает последний выданный ключ элеметов.
		/// </summary>
		UInt Identity { get; }
	 /// <summary>
		/// Возвращает элемент дерева по его ключу.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		ITreeItem GetItemWithKey(UInt key);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли дерево элемент с указаным кючем.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		bool ContainsItemWithKey(UInt key);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет элемент в дерево.
		/// </summary>
		/// <param name="parentKey">Ключ родительского элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <returns></returns>
		UInt AddByKey(ITreeItem item, UInt parentKey);
		/// <summary>
		/// Добавляет элемент в дерево.
		/// </summary>
		/// <param name="parentKey">Ключ родительского элемента.</param>
		/// <param name="obj">Объект добавляемого элемент.</param>
		/// <returns></returns>
		UInt AddByKey(object obj, UInt parentKey);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <param name="parentItem">Родительский элемент добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		void AddWithKey(UInt key, ITreeItem item, ITreeItem parentItem);
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="obj">Объект добавляемого элемента.</param>
		/// <param name="parentObj">Родительский объект добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		ITreeItem AddWithKey(UInt key, object obj, object parentObj);
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <param name="parentObj">Родительский объект добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		void AddWithKey(UInt key, ITreeItem item, object parentObj);
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="obj">Объект добавляемого элемента.</param>
		/// <param name="parentItem">Родительский элемент добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		ITreeItem AddWithKey(UInt key, object obj, ITreeItem parentItem);
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <param name="parentKey">Ключ родительского элемента.</param>
		void AddWithKey(UInt key, ITreeItem item, UInt parentKey);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет элемент по его ключу.
		/// </summary>
		/// <param name="key">Ключ удаляемого элемента.</param>
		/// <returns></returns>
		void RemoveByKey(UInt key);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Перемещает элемент в дереве к новому родительскому элементу.
		/// </summary>
		/// <param name="itemKey">Ключ перемещаемого элемента.</param>
		/// <param name="parentKey">Ключ нового родительского элемента.</param>
		void MoveItem(UInt itemKey, UInt parentKey);

	}
	//*************************************************************************************
	/// <summary>
	/// Класс дерева с явным ключем у элементов
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyedTree<T> : KeyedTree<T, KeyedTreeItem<T>>
	{
	}
	/// <summary>
	/// Класс дерева с явным ключем у элементов
	/// </summary>
	/// <typeparam name="T">Тип объекта элемента графа.</typeparam>
	/// <typeparam name="TItem">Тип элемента дерева.</typeparam>
	public class KeyedTree<T, TItem> : Tree<T, TItem>, IKeyedTree where TItem : KeyedTreeItem<T>
	{
		private int _idy = 0;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает последний выданный ключ элеметов.
		/// </summary>
		public UInt Identity
		{
		 get { return _idy; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public KeyedTree() : base() 
		{ 
			_noObj = new List<TItem>();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Override Methods >>
		/// <summary>
		/// Добавляет элемент в граф
		/// </summary>
		/// <param name="item"></param>
		/// <param name="parent"></param>
		public override void Add(IGraphItem item, IGraphItem parent)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			if(item is TItem == false)
				throw new Exception("Добавляемый элемет имеет неправильный тип!");
			if(item.IsRoot == false || item.HasChildren)
				throw new Exception("Добавляемый элемент принадлежит другому дереву!");

			if(((TItem)item).Key == 0)
			 ((TItem)item).Key = ++_idy;
			base.Add(item, parent);
		}
		#endregion << Override Methods >>
		//-------------------------------------------------------------------------------------
		#region << Keyed Methods >>
		private TItem GetItem(UInt key, IList<TItem> list)
		{
			if(list.Count == 0)
				return null;
			int pS = 0;
			int pE = list.Count-1;
			int sign = 0;

			if((sign = Math.Sign(UInt.Compare(key, list[pS].Key))) < 0)
				return null;
			else if(sign == 0)
				return list[pS];
			else if((sign = Math.Sign(UInt.Compare(key, list[pE].Key))) > 0)
				return null;
			else if(sign == 0)
				return list[pE];
			else
				while(pE-pS > 1)
				{
					int p = (pE-pS)/2;
					p = pS + (p == 0 ? 1 : p);
					sign = Math.Sign(UInt.Compare(key, list[p].Key));
					if(sign == 0)
					 return list[p];
					if(sign < 0)
						pE = p;
					else
						pS = p;
				}
			return null;
		}
		/// <summary>
		/// Возвращает элемент дерева по его ключу.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		public TItem GetItemWithKey(UInt key)
		{
			TItem item = null;
			if((item = GetItem(key, (IList<TItem>)_noObj)) != null)
				return item;
			return GetItem(key, _withObj);
		}
		ITreeItem IKeyedTree.GetItemWithKey(UInt key) { return GetItemWithKey(key); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли дерево элемент с указаным кючем.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		public bool ContainsItemWithKey(UInt key)
		{
			return GetItemWithKey(key) != null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет элемент в дерево.
		/// </summary>
		/// <param name="parentKey">Ключ родительского элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <returns></returns>
		public UInt AddByKey(TItem item, UInt parentKey)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			TItem pi = GetItemWithKey(parentKey);
			if(pi == null && parentKey != 0)
				throw new ArgumentException("Не найден родительский элемент!");
			Add(item, pi);
			return item.Key;
		}
		UInt IKeyedTree.AddByKey(ITreeItem item, UInt parentKey) { return AddByKey((TItem)item, parentKey); }
		/// <summary>
		/// Добавляет элемент в дерево.
		/// </summary>
		/// <param name="parentKey">Ключ родительского элемента.</param>
		/// <param name="obj">Объект добавляемого элемент.</param>
		/// <returns></returns>
		public UInt AddByKey(T obj, UInt parentKey)
		{
			if(object.Equals(obj, null))
				throw new ArgumentNullException("obj");
			if(this.Contains(obj))
			 throw new PulsarException("Элемент с объектом [{0}] уже присутствует в дереве!", obj);
			return AddByKey((TItem)Activator.CreateInstance(typeof(TItem), obj), parentKey);
		}
		UInt IKeyedTree.AddByKey(object obj, UInt parentKey)	{ return AddByKey((T)obj, parentKey); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <param name="parentItem">Родительский элемент добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		public void AddWithKey(UInt key, TItem item, TItem parentItem)
		{
			if(Contains(item))
				throw new ArgumentException("Элемент уже присутствует в дереве!", "item");
			if(parentItem != null && Contains(parentItem) == false)
				throw new Exception("В дереве не найден родительский элемент добавляемого элемента!");
			if(key <= _idy)
				throw new ArgumentException("Ключ добавляемого элемента должен быть больше значения Identity!", "key");
			item.Key = key;
			_idy = key;
			Add(item,parentItem);
		}
		void IKeyedTree.AddWithKey(UInt key, ITreeItem item, ITreeItem parentItem) { AddWithKey(key, (TItem)item, (TItem)parentItem); }
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="obj">Объект добавляемого элемента.</param>
		/// <param name="parentObj">Родительский объект добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		public TItem AddWithKey(UInt key, T obj, T parentObj)
		{
		 if(obj == null)
			 throw new ArgumentNullException("obj");
			TItem item = (TItem)Activator.CreateInstance(typeof(TItem),obj);
			AddWithKey(key, item, parentObj);
			return item;
		}
		ITreeItem IKeyedTree.AddWithKey(UInt key, object obj, object parentObj) { return AddWithKey(key, (T)obj, (T)parentObj); }
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <param name="parentObj">Родительский объект добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		public void AddWithKey(UInt key, TItem item, T parentObj)
		{
			TItem pi = null;
			if(parentObj	!= null)
			{
				pi = this[parentObj];
				if(pi != null)
					throw new Exception("В дереве не найден родительский элемент добавляемого элемента!");
			}
			AddWithKey(key, item, pi);
		}
		void IKeyedTree.AddWithKey(UInt key, ITreeItem item, object parentObj) { AddWithKey(key, (TItem)item, (T)parentObj); }
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="obj">Объект добавляемого элемента.</param>
		/// <param name="parentItem">Родительский элемент добавляемого элемента.
		/// null в случае добавления корневого элемента.</param>
		public TItem AddWithKey(UInt key, T obj, TItem parentItem)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			TItem item = (TItem)Activator.CreateInstance(typeof(TItem), obj);
			AddWithKey(key, item, parentItem);
			return item;
		}
		ITreeItem IKeyedTree.AddWithKey(UInt key, object obj, ITreeItem parentItem)	{ return AddWithKey(key, (T)obj, (TItem)parentItem) ; }
		/// <summary>
		/// Добавяет элемент дерева.
		/// </summary>
		/// <param name="key">Ключ добавляемого элемента.</param>
		/// <param name="item">Добавляемый элемент.</param>
		/// <param name="parentKey">Ключ родительского элемента.</param>
		public void AddWithKey(UInt key, TItem item, UInt parentKey)
		{
			TItem pi = GetItemWithKey(parentKey);
			if(pi == null)
			 throw new Exception("Не найден родительский элемент по указанному ключу!");
			AddWithKey(key,item,pi);
		}
		void IKeyedTree.AddWithKey(UInt key, ITreeItem item, UInt parentKey) { AddWithKey(key, (TItem)item, parentKey); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет элемент по его ключу.
		/// </summary>
		/// <param name="key">Ключ удаляемого элемента.</param>
		/// <returns></returns>
		public void RemoveByKey(UInt key)
		{
			TItem item = GetItemWithKey(key);
			if(item == null)
			 return;
			Remove(item);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Перемещает элемент в дереве к новому родительскому элементу.
		/// </summary>
		/// <param name="itemKey">Ключ перемещаемого элемента.</param>
		/// <param name="parentKey">Ключ нового родительского элемента.</param>
		public void MoveItem(UInt itemKey, UInt parentKey)
		{
			TItem item = GetItemWithKey(itemKey);
			if(item == null)
			 throw new PulsarException("Дерево не содержит элемента с ключем [{0}]!", itemKey);
			TItem pItem;
			if(parentKey == 0)
				pItem = null;
			else if((pItem = GetItemWithKey(parentKey)) == null)
				throw new PulsarException("Дерево не содержит элемента с ключем [{0}]!", parentKey);
			MoveItem(item, pItem);
		}
		#endregion << Keyed Methods >>
		//-------------------------------------------------------------------------------------
						
	}
}

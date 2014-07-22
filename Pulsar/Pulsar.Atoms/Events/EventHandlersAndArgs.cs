using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Обобщенный делегат события.
	/// </summary>
	/// <typeparam name="TObj">Тип объекта, генерирующего событие.</typeparam>
	/// <typeparam name="TArgs">Тип аргумента события.</typeparam>
	/// <param name="sender">Объект, генерирующий событие.</param>
	/// <param name="args">Аргумент события.</param>
	public delegate void EventHandler<TObj, TArgs>(TObj sender, TArgs args);
	//*************************************************************************************
	#region << public class EventArgs<T> : EventArgs >>
	/// <summary>
	/// Класс аргумента события, содержащего объект.
	/// </summary>
	public class EventArgs<T> : EventArgs
	{
		private T obj = default(T);
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Объект данных 
		/// </summary>
		public T Object
		{
			get { return obj; }
			set { obj = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public EventArgs() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public EventArgs(T obj) : base() { this.obj = obj; }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------

	}
	#endregion << public class EventArgs<T> : EventArgs >>
	//*************************************************************************************
	#region << public class EventArgs<T1, T2> : EventArgs >>
	/// <summary>
	/// Класс аргумента события, содержащего два объекта.
	/// </summary>
	public class EventArgs<T1, T2> : EventArgs
	{
		private T1 obj1 = default(T1);
		private T2 obj2 = default(T2);
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Объект данных 1
		/// </summary>
		public T1 Object1
		{
			get { return obj1; }
			set { obj1 = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Объект данных 2
		/// </summary>
		public T2 Object2
		{
			get { return obj2; }
			set { obj2 = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public EventArgs() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public EventArgs(T1 obj1, T2 obj2)
			: base()
		{
			this.obj1 = obj1;
			this.obj2 = obj2;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------

	}
	#endregion << public class EventArgs<T1, T2> : EventArgs >>
	//*************************************************************************************
	#region << public class EventArgs<T1, T2, T3> : EventArgs >>
	/// <summary>
	/// Класс аргумента события, содержащего три объекта.
	/// </summary>
	public class EventArgs<T1, T2, T3> : EventArgs
	{
		private T1 obj1 = default(T1);
		private T2 obj2 = default(T2);
		private T3 obj3 = default(T3);
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Объект данных 1
		/// </summary>
		public T1 Object1
		{
			get { return obj1; }
			set { obj1 = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Объект данных 2
		/// </summary>
		public T2 Object2
		{
			get { return obj2; }
			set { obj2 = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Объект данных 3
		/// </summary>
		public T3 Object3
		{
			get { return obj3; }
			set { obj3 = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public EventArgs() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public EventArgs(T1 obj1, T2 obj2, T3 obj3)
			: base()
		{
			this.obj1 = obj1;
			this.obj2 = obj2;
			this.obj3 = obj3;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------

	}
	#endregion << public class EventArgsEventArgs<T1, T2, T3> : EventArgs >>
	//**************************************************************************************
	#region << public class CancelEventArgs<T> : CancelEventArgs >>
	/// <summary>
	/// Класс аргумента события, содержащего объект и состояние отмены.
	/// </summary>
	public class CancelEventArgs<T> : CancelEventArgs
	{
		private T obj = default(T);
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Объект данных 
		/// </summary>
		public T Object
		{
			get { return obj; }
			set { obj = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public CancelEventArgs() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public CancelEventArgs(T obj) : base() { this.obj = obj; }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class CancelEventArgs : CancelEventArgs >>
	//**************************************************************************************
	#region << public enum ChangeNotifyAction : byte >>
	/// <summary>
	/// Перечисление действий, вызывающих изменение объекта
	/// </summary>
	public enum ChangeNotifyAction : byte
	{
		///
		Unknown = 0,
		///
		ObjectChange,
		///
		ObjectReset,
		///
		ItemAdd,
		///
		ItemChange,
		///
		ItemDelete,
		///
		ItemMove,
		///
		ItemInsert
	} 
	#endregion << public enum ChangeNotifyAction : byte >>
	//**************************************************************************************
	#region << public class ChangeNotifyEventArgs : EventArgs >>
	/// <summary>
	/// Базовый класс аргументов событий изменений объектов
	/// </summary>
	public class ChangeNotifyEventArgs : EventArgs
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Объект-источник события изменения.
		/// </summary>
		public object Sender { get; set; }
		/// <summary>
		/// Действие, вызвавшее изменение объекта
		/// </summary>
		public ChangeNotifyAction Action { get; set; }		
		/// <summary>
		/// Аргументы исходного события
		/// </summary>
		public ChangeNotifyEventArgs SourceArgs { get; set; }
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public ChangeNotifyEventArgs()	{ }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ChangeNotifyEventArgs(object sender, ChangeNotifyAction action, ChangeNotifyEventArgs sourceArgs)
		{
			Sender	= sender;
			Action	= action;
			SourceArgs	= sourceArgs;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class ChangeNotifyEventArgs : EventArgs >>
	//**************************************************************************************
	#region << public class ObjectChangeNotifyEventArgs : ChangeNotifyEventArgs >>
	/// <summary>
	/// Класс аргументов событий изменений объекта
	/// </summary>
	public class ObjectChangeNotifyEventArgs : ChangeNotifyEventArgs
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Имя измененного члена класса объекта.
		/// </summary>
		public string MemberName { get; set; }
		/// <summary>
		/// Новое значение изменяемого члена класса.
		/// </summary>
		public object NewValue { get; set; }
		/// <summary>
		/// Старое значение изменяемого члена класса.
		/// </summary>
		public object OldValue { get; set; }
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public ObjectChangeNotifyEventArgs()	{			}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ObjectChangeNotifyEventArgs(object sender, ChangeNotifyAction action, 
		                              string memberName,	object newValue, object oldValue) 
		 : base (sender, action, null) 
		{
			MemberName	=	memberName;
			NewValue	=	newValue;
			OldValue	=	oldValue;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ObjectChangeNotifyEventArgs(object sender, ChangeNotifyAction action, ChangeNotifyEventArgs sourceArgs,
		                              string memberName,	object newValue, object oldValue) 
		 : base (sender, action, sourceArgs) 
		{
			MemberName	=	memberName;
			NewValue	=	newValue;
			OldValue	=	oldValue;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ObjectChangeNotifyEventArgs(object sender, ChangeNotifyAction action) 
		 : base (sender, action, null) 
		{
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public ObjectChangeNotifyEventArgs(object sender, ChangeNotifyAction action, ChangeNotifyEventArgs sourceArgs)
			: base(sender, action, sourceArgs) 
		{
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class ObjectChangeNotifyEventArgs : ChangeNotifyEventArgs >>
	//**************************************************************************************
	#region << public class CollectionChangeNotifyEventArgs : ChangeNotifyEventArgs >>
	/// <summary>
	/// Класс агрумента событий изменения коллекции.
	/// </summary>
	public class CollectionChangeNotifyEventArgs : ChangeNotifyEventArgs
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Элемент коллекции, над которым производится действие.
		/// </summary>
		public object Item { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Элемент коллекции, заменяющий существующий (для событий ItemChanging и ItemChanged).
		/// </summary>
		public object NewItem { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Индекс элемента в коллекции, над которым произведено действие. 
		/// Указывается если его возможно определить.
		/// </summary>
		public int ItemIndex { get; set; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public CollectionChangeNotifyEventArgs() : base() 
		{ 
			ItemIndex = -1;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public CollectionChangeNotifyEventArgs(object sender, ChangeNotifyAction action, ChangeNotifyEventArgs sourceArgs,
		                                    object item, object newItem, int itemIndex = -1)
			: base(sender, action, sourceArgs) 
		{
			Item = item;
			NewItem = newItem;
			ItemIndex = itemIndex;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public CollectionChangeNotifyEventArgs(object sender, ChangeNotifyAction action, 
		                                    object item, object newItem, int itemIndex = -1)
			: base(sender, action, null) 
		{
			Item = item;
			NewItem = newItem;
			ItemIndex = itemIndex;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public CollectionChangeNotifyEventArgs(object sender, ChangeNotifyAction action, ChangeNotifyEventArgs sourceArgs,
		                                    object item)
			: base(sender, action, sourceArgs) 
		{
			Item = item;
			ItemIndex = -1;
		}

		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class CollectionChangeNotifyEventArgs : ChangeNotifyEventArgs >>

}

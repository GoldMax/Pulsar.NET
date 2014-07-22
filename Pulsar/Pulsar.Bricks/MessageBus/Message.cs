using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public interface IMessage >>
	/// <summary>
	/// Базовый интерфейс сообщения
	/// </summary>
	public interface IMessage
	{
	 /// <summary>
	 /// Тип сообщения
	 /// </summary>
		Type MsgType { get; }
		/// <summary>
		/// Объект, пославший сообщение
		/// </summary>
		object MsgObject { get; }
		/// <summary>
		/// Причина возникновения сообщения
		/// </summary>
		object Reason { get; }
		/// <summary>
		/// Данные сообщения.
		/// </summary>
		object Data { get; set; }
	} 
	#endregion << public interface IMessage >>
	//**************************************************************************************
	#region << public class MessageBase : IMessage >>
	/// <summary>
	/// Базовый класс сообщения.
	/// </summary>
	public class MessageBase	: IMessage
	{
	 private Type _mt;
		private object _mo;
		private object _data;
		private object _reason;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Тип сообщения
		/// </summary>
		public Type MsgType 
		{ 
		 get { return _mt; } 
		}
		/// <summary>
		/// Объект сообщения
		/// </summary>
		public object MsgObject
		{
			get { return _mo; }
			//protected set { _ser = value; }
		}
		/// <summary>
		/// Причина возникновения сообщения
		/// </summary>
		public virtual object Reason
		{
			get { return _reason; }
			set { _reason = value; }
		}
		/// <summary>
		/// Данные сообщения.
		/// </summary>
		public virtual object Data
		{
			get { return _data; }
			set { _data = value; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		protected internal MessageBase()	{ 	}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public MessageBase(Type msgType, object msgObject)
		{
			if(msgType == null)
				throw new ArgumentNullException("msgType");
			if(msgObject == null)
				throw new ArgumentNullException("msgObject");
			_mt = msgType;
			_mo = msgObject;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public MessageBase(Type msgType, object msgObject, object reason)
		{
			if(msgType == null)
				throw new ArgumentNullException("msgType");
			if(msgObject == null)
				throw new ArgumentNullException("msgObject");
			_mt = msgType;
			_mo = msgObject;
			_reason = reason;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public MessageBase(Type msgType, object msgObject, object reason, object data)
		{
			if(msgType == null)
				throw new ArgumentNullException("msgType");
			if(msgObject == null)
				throw new ArgumentNullException("msgObject");
			_mt = msgType;
			_mo = msgObject;
			_reason = reason;
			_data = data;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class MessageBase	: IMessage >>
	//**************************************************************************************
	#region << public class EventMessage : Message >>
	/// <summary>
	/// Класс сообщения о событии.
	/// </summary>
	public class EventMessage : MessageBase
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Причина возникновения сообщения
		/// </summary>
		public new EventMessageReasons Reason
		{
			get { return base.Reason == null ? EventMessageReasons.Unknown : (EventMessageReasons)base.Reason; }
			set { base.Reason = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		protected internal EventMessage() { }																
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public EventMessage(Type msgType, object msgObject) : base(msgType, msgObject) {}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public EventMessage(Type msgType, object msgObject, EventMessageReasons reason) : base(msgType, msgObject, reason) { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public EventMessage(Type msgType, object msgObject, EventMessageReasons reason, object data) : base(msgType, msgObject, reason, data) { }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	} 
	#endregion << public class EventMessage<TObj>	: Message<TObj>	where TObj : class >>
	//**************************************************************************************
	#region << public enum EventMessageReasons : byte >>
	/// <summary>
	/// Перечисление причин сообщений о событии
	/// </summary>
	public enum EventMessageReasons : byte
	{
		/// <summary>
		/// Неизвестная причина
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Событие, возникающее при изменении объекта.
		/// </summary>
		ObjectChanging = 1,
		/// <summary>
		/// Событие, возникающее после изменении объекта.
		/// </summary>
		ObjectChanged = 2,
		/// <summary>
		/// Событие, возникающее при добавлении объекта в контейнер.
		/// </summary>
		ObjectAdding = 3,
		/// <summary>
		/// Событие, возникающее после добавления объекта в контейнер.
		/// </summary>
		ObjectAdded = 4,
		/// <summary>
		/// Событие, возникающее при удалении объекта из контейнера.
		/// </summary>
		ObjectDeleting = 5,
		/// <summary>
		/// Событие, возникающее после удаления объекта из контейнера.
		/// </summary>
		ObjectDeleted = 6,
		/// <summary>
		/// Событие, возникающее при перемещении объекта внутри контейнера или между контейнерами.
		/// </summary>
		ObjectMoving = 7,
		/// <summary>
		/// Событие, возникающее после перемещении объекта внутри контейнера или между контейнерами.
		/// </summary>
		ObjectMoved = 8,
		/// <summary>
		/// Событие, возникающее при существенном изменении объекта.
		/// </summary>
		ObjectResetting = 9,
		/// <summary>
		/// Событие, возникающее после существенного изменения объекта.
		/// </summary>
		ObjectResetted = 10
	} 
	#endregion << public enum EventMessageReasons : byte >>
	//**************************************************************************************
	#region << public class NeedHelpMessage: Message<object> >>
	/// <summary>
	/// Класс сообщения с просьбой о помощи.
	/// </summary>
	public class NeedHelpMessage : MessageBase
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Просьба
		/// </summary>
		public string Request
		{
			get { return (string)base.Reason; }
			set { base.Reason = value; }
		}
		/// <summary>
		/// Тип получателей
		/// </summary>
		public Type RicipientType 
		{ 
			get { return base.MsgObject.GetType(); }
		}
		/// <summary>
		/// Регистрационное имя получателя
		/// </summary>
		public string RegName
		{
			get { return ((NeedHelpRecipient)base.MsgObject).RegName; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		protected internal NeedHelpMessage() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpMessage(NeedHelpRecipient target, string request, object data) : base(target.GetType(),target,request,data) 
		{
			if(string.IsNullOrWhiteSpace(request))
			 throw new ArgumentException("Не указан запрос!");
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpMessage(string regName, string request) 
		 : this(new NeedHelpRecipient(null, regName), request, null) { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpMessage(string regName, string request, object data)
			: this(new NeedHelpRecipient(null, regName), request, data) { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpMessage(Type type, string request)
			: this(new NeedHelpRecipient(type, null), request, null) { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpMessage(Type type, string request, object data) 
		: this(new NeedHelpRecipient(type, null), request, data) { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpMessage(string regName, Type type, string request)
			: this(new NeedHelpRecipient(type, regName), request, null) { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpMessage(string regName, Type type, string request, object data)
			: this(new NeedHelpRecipient(type, regName), request, data) { }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	} 
	#endregion << public class NeedHelpMessage<TObj>	: Message<TObj>	where TObj : class >>
	//*************************************************************************************
	#region << public enum MessageKind : byte >>
	/// <summary>
	/// Перечисление видов сообщений
	/// </summary>
	[Flags]
	public enum MessageKind : byte
	{
		/// <summary>
		/// 
		/// </summary>
		Unknown  = 0,
		/// <summary>
		/// Голосование
		/// </summary>
		Pooling  = 1,
		/// <summary>
		/// Извещение
		/// </summary>
		Notify   = 2,
		/// <summary>
		/// Запрос помощи
		/// </summary>
		NeedHelp = 4
	} 
	#endregion << public enum MessageKind : byte >>
}

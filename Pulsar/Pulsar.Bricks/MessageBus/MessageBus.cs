using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public delegate void MessageHandler(IMessage msg, MessageHandlerCallContext cox) >>
	/// <summary>
	/// Делегат обработчика сообщения
	/// </summary>
	/// <param name="msg">Сообщение</param>
	/// <param name="cox">Контекст вызова обработчика.</param>
	public delegate void MessageHandler(IMessage msg, MessageHandlerCallContext cox); 
	#endregion << public delegate void MessageHandler(IMessage msg, MessageHandlerCallContext cox) >>
	//*************************************************************************************
	/// <summary>
	/// Класс шины сообщений
	/// </summary>
	public abstract class MessageBus
	{
		public static MessageBus CurrentBus = null;
		//-------------------------------------------------------------------------------------
		#region << Register Recipient Methods >>
		/// <summary>
		/// Метод регистрации получателя сообщений.
		/// </summary>
		/// <typeparam name="T">Тип объектов сообщений</typeparam>
		/// <param name="kind">Виды сообщений</param>
		/// <param name="handler">Обработчик сообщений</param>
		/// <param name="defaultLock">Тип блокировки по умолчанию объекта получателя.</param>
		public static void RegisterRecipient<T>(MessageKind kind, MessageHandler handler, LockType defaultLock)	where T : class
		{
			if(CurrentBus != null)
				CurrentBus._RegisterRecipient(typeof(T),kind, handler, defaultLock);
		}
		/// <summary>
		/// Метод регистрации получателя сообщений голосования.
		/// </summary>
		/// <typeparam name="T">Тип объектов сообщений</typeparam>
		/// <param name="handler">Обработчик сообщений</param>
		/// <param name="defaultLock">Тип блокировки по умолчанию объекта получателя.</param>
		public static void RegisterPoolingRecipient<T>(MessageHandler handler, LockType defaultLock)	where T : class
		{
			if(CurrentBus != null)
				CurrentBus._RegisterRecipient(typeof(T),MessageKind.Pooling, handler, defaultLock);
		}
		/// <summary>
		/// Метод регистрации получателя сообщений извещения.
		/// </summary>
		/// <typeparam name="T">Тип объектов сообщений</typeparam>
		/// <param name="handler">Обработчик сообщений</param>
		/// <param name="defaultLock">Тип блокировки по умолчанию объекта получателя.</param>
		public static void RegisterNotifyRecipient<T>(MessageHandler handler, LockType defaultLock)	where T : class
		{
			if(CurrentBus != null)
				CurrentBus._RegisterRecipient(typeof(T),MessageKind.Notify, handler, defaultLock);
		}
		/// <summary>
		/// Метод регистрации получателя сообщений запроса помощи.
		/// </summary>
		/// <typeparam name="T">Тип объектов сообщений</typeparam>
		/// <param name="handler">Обработчик сообщений</param>
		/// <param name="defaultLock">Тип блокировки по умолчанию объекта получателя.</param>
		public static void RegisterNeedHelpRecipient<T>(MessageHandler handler, LockType defaultLock)	where T : class
		{
			if(CurrentBus != null)
				CurrentBus._RegisterRecipient(typeof(T),MessageKind.NeedHelp, handler, defaultLock);
		}
		/// <summary>
		/// Метод регистрации получателя сообщений.
		/// </summary>
		/// <param name="msgType">Тип объектов сообщений</param>
		/// <param name="kind">Вид сообщений</param>
		/// <param name="handler">Обработчик сообщений</param>
		/// <param name="defaultLock">Тип блокировки по умолчанию объекта получателя.</param>
		protected abstract void _RegisterRecipient(Type msgType, MessageKind kind, MessageHandler handler, LockType defaultLock);
		#endregion << Register Recipient Methods >>
		//-------------------------------------------------------------------------------------
		#region << Pooling Methods >>
		/// <summary>
		/// Отсылает голосовательное сообщение подписчикам.	
		/// Тип сообщения определяется по типу объекта сообщения.
		/// </summary>
		/// <param name="obj">Объект сообщения</param>
		/// <param name="reason">Объект причины сообщения</param>
		/// <param name="msgData">Данные сообщения</param>
		public static bool Polling(object obj, object reason, object msgData) //where TObj : class
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			return CurrentBus == null ? true : CurrentBus._Polling(new MessageBase(obj.GetType(), obj, reason, msgData));
		}
		/// <summary>
		/// Отсылает голосовательное сообщение подписчикам.	
		/// </summary>
		/// <typeparam name="T">Тип сообщения. Может отличаться от типа объекта сообщения.</typeparam>
		/// <param name="obj">Объект сообщения</param>
		/// <param name="reason">Объект причины сообщения</param>
		/// <param name="msgData">Данные сообщения</param>
		public static bool Polling<T>(object obj, object reason, object msgData) //where TObj : class
		{
			return CurrentBus == null ? true : CurrentBus._Polling(new MessageBase(typeof(T), obj, reason, msgData));
		}
		/// <summary>
		/// Отсылает голосовательное сообщение подписчикам.
		/// </summary>
		/// <typeparam name="T">Тип сообщения.</typeparam>
		/// <param name="msg">Сообщение</param>
		/// <returns>false - если голосование не состоятельно.</returns>
		public static bool Polling(IMessage msg)
		{
			if(msg == null)
				throw new ArgumentNullException("msg");
			return CurrentBus == null ? true : CurrentBus._Polling(msg);
		}
		/// <summary>
		/// Отсылает голосовательное сообщение подписчикам.
		/// </summary>
		/// <param name="msg">Сообщение</param>
		/// <returns>false - если голосование не состоятельно.</returns>
		protected abstract bool _Polling(IMessage msg);
		#endregion << Pooling Methods >>
		//-------------------------------------------------------------------------------------
		#region << Notify Methods >>
		/// <summary>
		/// Отсылает уведомительное сообщение подписчикам.
		/// Тип сообщения определяется по типу объекта сообщения.
		/// </summary>
		/// <param name="obj">Объект сообщения</param>
		/// <param name="reason">Объект причины сообщения</param>
		/// <param name="msgData">Данные сообщения</param>
		public static void Notify(object obj, object reason, object msgData)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			if(CurrentBus != null)
				CurrentBus._Notify(new MessageBase(obj.GetType(), obj, reason, msgData));
		}
		/// <summary>
		/// Отсылает уведомительное сообщение подписчикам.
		/// </summary>
		/// <typeparam name="T">Тип сообщения. Может отличаться от типа объекта сообщения.</typeparam>
		/// <param name="obj">Объект сообщения</param>
		/// <param name="reason">Объект причины сообщения</param>
		/// <param name="msgData">Данные сообщения</param>
		public static void Notify<T>(object obj, object reason, object msgData)
		{
			if(CurrentBus != null)
				CurrentBus._Notify(new MessageBase(typeof(T), obj, reason, msgData));
		}
		/// <summary>
		/// Отсылает уведомительное сообщение подписчикам.
		/// </summary>
		/// <param name="msg">Сообщение</param>
		public static void Notify(IMessage msg) 
		{
			if(msg == null)
				throw new ArgumentNullException("msg");
			if(CurrentBus != null)
				CurrentBus._Notify(msg);
		}
		/// <summary>
		/// Отсылает уведомительное сообщение подписчикам.
		/// </summary>
		/// <param name="msg">Сообщение</param>
		protected abstract void _Notify(IMessage msg);
		#endregion << Notify Methods >>
		//-------------------------------------------------------------------------------------
		#region << NeedHelp Methods >>
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <typeparam name="T">Тип подписчиков, для которых предназначено сообщение.</typeparam>
		/// <param name="request">Имя задачи, которую нужно выполнить подписчиками.</param>
		/// <param name="msgData">Данные сообщения</param>
		/// <returns>true - если задание выполнено.</returns>
		public static bool NeedHelp<T>(string request, object msgData) where T : class
		{
			return CurrentBus == null ? true : CurrentBus._NeedHelp<T>(request, msgData);
		}
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <param name="regName">Регистрационное имя корневого объекта подписчика.</param>
		/// <param name="request">Имя задачи, которую нужно выполнить подписчику.</param>
		/// <param name="msgData">Данные сообщения</param>
		/// <returns>true - если задание выполнено.</returns>
		public static bool NeedHelp(string regName, string request, object msgData)
		{
			return CurrentBus == null ? true : CurrentBus._NeedHelp(regName, request, msgData);
		}
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <typeparam name="T">Тип подписчиков, для которых предназначено сообщение.</typeparam>
		/// <param name="regName">Регистрационное имя корневого объекта подписчика.</param>
		/// <param name="request">Имя задачи, которую нужно выполнить подписчику.</param>
		/// <param name="msgData">Данные сообщения</param>
		/// <returns>true - если задание выполнено.</returns>
		public static bool NeedHelp<T>(string regName, string request, object msgData)
		{
			return CurrentBus == null ? true : CurrentBus._NeedHelp<T>(regName, request, msgData);
		}
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <param name="msg">Сообщение</param>
		/// <returns>true - если задание выполнено.</returns>
		public static bool NeedHelp(NeedHelpMessage msg)
		{
			return CurrentBus == null ? true : CurrentBus._NeedHelp(msg);
		}
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <typeparam name="T">Тип подписчиков, для которых предназначено сообщение.</typeparam>
		/// <param name="request">Имя задачи, которую нужно выполнить подписчиками.</param>
		/// <param name="msgData">Данные сообщения</param>
		/// <returns>true - если задание выполнено.</returns>
		protected virtual bool _NeedHelp<T>(string request, object msgData) where T : class
		{
			NeedHelpMessage msg = new NeedHelpMessage(typeof(T), request, msgData);
			return _NeedHelp(msg);
		}
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <param name="regName">Регистрационное имя корневого объекта подписчика.</param>
		/// <param name="request">Имя задачи, которую нужно выполнить подписчику.</param>
		/// <param name="msgData">Данные сообщения</param>
		/// <returns>true - если задание выполнено.</returns>
		protected virtual bool _NeedHelp(string regName, string request, object msgData)
		{
			NeedHelpMessage msg = new NeedHelpMessage(regName, request, msgData);
			return _NeedHelp(msg);
		}
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <typeparam name="T">Тип подписчиков, для которых предназначено сообщение.</typeparam>
		/// <param name="regName">Регистрационное имя корневого объекта подписчика.</param>
		/// <param name="request">Имя задачи, которую нужно выполнить подписчику.</param>
		/// <param name="msgData">Данные сообщения</param>
		/// <returns>true - если задание выполнено.</returns>
		protected virtual bool _NeedHelp<T>(string regName, string request, object msgData)
		{
			NeedHelpMessage msg = new NeedHelpMessage(regName, typeof(T), request, msgData);
			return _NeedHelp(msg);
		}
		/// <summary>
		/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
		/// </summary>
		/// <param name="msg">Сообщение</param>
		/// <returns>true - если задание выполнено.</returns>
		protected abstract bool _NeedHelp(NeedHelpMessage msg);
		#endregion << NeedHelp Methods >>
		//-------------------------------------------------------------------------------------

	}
	//*************************************************************************************
	/// <summary>
	/// 
	/// </summary>
	public class NeedHelpRecipient
	{
		public readonly Type Type;
		public readonly string RegName;
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public NeedHelpRecipient(Type type, string regName)
		{
		 if(type == null && String.IsNullOrWhiteSpace(regName))
			 throw new Exception("Для создания объекта NeedHelpRecipient требуется указать как минимум один параметр!");
			Type = type;
			RegName = regName;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="regName"></param>
		public static NeedHelpRecipient New(string regName)
		{
			return new NeedHelpRecipient(null,regName);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="regName"></param>
		public static NeedHelpRecipient New<T>(string regName)
		{
			return new NeedHelpRecipient(typeof(T),regName);
		}
	}
}

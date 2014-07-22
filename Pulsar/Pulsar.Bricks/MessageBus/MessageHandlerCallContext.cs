using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	//*************************************************************************************
	#region << public enum MessageHandlerCallResult : byte >>
	/// <summary>
	/// Перечисление результатов вызова обработчика сообщения.
	/// </summary>
	[Flags]
	public enum MessageHandlerCallResult : byte
	{
		/// <summary>
		/// Нормальное завершение
		/// </summary>
		NormalDone = 0,
		/// <summary>
		/// Необходим повторный вызов с блокировкой на запись
		/// </summary>
		ReCallWithWriteLock = 1,
		/// <summary>
		/// Завершение без модификации объекта. 
		/// При блокировке на запись отменяет вызов сохранения объекта, при других блокировках игнорируется.
		/// </summary>
		DoneWithoutModification = 2,
		/// <summary>
		/// Прерывает проведение опроса. Опрос считается несостоятельным.
		/// </summary>
		BreakPolling	= 4,
		/// <summary>
		/// Определяет, что запрос о помощи обработан и помощь оказана.
		/// </summary>
		Helped = 8,
		/// <summary>
		/// Обозначает, что обработку сообщения надо пропустить для всех получателей данного типа.
		/// </summary>
		SkipThisRecipientType = 16
	} 
	#endregion << public enum MessageHandlerCallResult : byte >>
	//*************************************************************************************
	/// <summary>
	/// Класс контекста вызова обработчика сообщения.
	/// </summary>
	public class MessageHandlerCallContext
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Текущая используемая блокировка.
		/// </summary>
		public LockType CurrentLock { get; private set; }
		/// <summary>
		/// Текущая используемая блокировка.
		/// </summary>
		public MessageHandlerCallResult CallResult { get; set; }
		/// <summary>
		/// Определяет режим голосования
		/// </summary>
		public bool IsPolling { get; private set; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Constructors >>
		private MessageHandlerCallContext() {}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public MessageHandlerCallContext(LockType currentLock, bool isPolling = false)
		{
			CurrentLock = currentLock;
			IsPolling = isPolling;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
						
	}
}

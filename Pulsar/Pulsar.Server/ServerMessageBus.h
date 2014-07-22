#pragma once


using namespace System;

namespace Pulsar
{
	namespace Server
	{
		/// <summary>
		/// Класс серверной шины сообщений
		/// </summary>
		public ref class ServerMessageBus	: MessageBus
		{
			private:
			Dictionary<Type^, WeakEvent<IMessage^, MessageHandlerCallContext^>^>^ _pooling;
			Dictionary<Type^, WeakEvent<IMessage^, MessageHandlerCallContext^>^>^ _notify;
			Dictionary<Type^, WeakEvent<IMessage^, MessageHandlerCallContext^>^>^ _needHelp;
			Action<Object^>^ _notifyAction;

			public:
			ServerMessageBus() 
			{ 
				_pooling =	gcnew Dictionary<Type^, WeakEvent<IMessage^, MessageHandlerCallContext^>^>();
				_notify =	gcnew Dictionary<Type^, WeakEvent<IMessage^, MessageHandlerCallContext^>^>();
				_needHelp =	gcnew Dictionary<Type^, WeakEvent<IMessage^, MessageHandlerCallContext^>^>();
				_notifyAction = gcnew Action<Object^>(this, &ServerMessageBus::_NotifyBody); 
			}

			protected:
			/// <summary>
			/// Метод регистрации получателя сообщений.
			/// </summary>
			/// <param name="msgType">Тип объектов сообщений</param>
			/// <param name="kind">Вид сообщений</param>
			/// <param name="handler">Обработчик сообщений</param>
			/// <param name="defaultLock">Тип блокировки по умолчанию объекта получателя.</param>
			void virtual _RegisterRecipient(Type^ msgType, MessageKind kind, MessageHandler^ handler,	LockType defaultLock) override;
			/// <summary>
			/// Отсылает уведомительное сообщение подписчикам.
			/// </summary>
			/// <typeparam name="T">Тип объекта сообщения.</typeparam>
			/// <param name="msg">Сообщение</param>
			void virtual _Notify(IMessage^ msg) override;
			/// <summary>
			/// Отсылает голосовательное сообщение подписчикам.
			/// </summary>
			/// <typeparam name="T">Тип объекта сообщения.</typeparam>
			/// <param name="msg">Сообщение</param>
			/// <returns>false - если голосование не состоятельно.</returns>
			virtual bool _Polling(IMessage^ msg) override;
			/// <summary>
			/// Отсылает сообщение о необходимости выполнения задачи подписчиками.
			/// </summary>
			/// <param name="msg">Сообщение</param>
			/// <returns>true - если задание выполнено.</returns>
			virtual bool _NeedHelp(NeedHelpMessage^ msg) override;
			private:
			void _NotifyBody(Object^ arg);
		};
	}
}


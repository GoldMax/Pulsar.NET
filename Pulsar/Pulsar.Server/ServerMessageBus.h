#pragma once


using namespace System;

namespace Pulsar
{
	namespace Server
	{
		/// <summary>
		/// ����� ��������� ���� ���������
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
			/// ����� ����������� ���������� ���������.
			/// </summary>
			/// <param name="msgType">��� �������� ���������</param>
			/// <param name="kind">��� ���������</param>
			/// <param name="handler">���������� ���������</param>
			/// <param name="defaultLock">��� ���������� �� ��������� ������� ����������.</param>
			void virtual _RegisterRecipient(Type^ msgType, MessageKind kind, MessageHandler^ handler,	LockType defaultLock) override;
			/// <summary>
			/// �������� �������������� ��������� �����������.
			/// </summary>
			/// <typeparam name="T">��� ������� ���������.</typeparam>
			/// <param name="msg">���������</param>
			void virtual _Notify(IMessage^ msg) override;
			/// <summary>
			/// �������� ��������������� ��������� �����������.
			/// </summary>
			/// <typeparam name="T">��� ������� ���������.</typeparam>
			/// <param name="msg">���������</param>
			/// <returns>false - ���� ����������� �� ������������.</returns>
			virtual bool _Polling(IMessage^ msg) override;
			/// <summary>
			/// �������� ��������� � ������������� ���������� ������ ������������.
			/// </summary>
			/// <param name="msg">���������</param>
			/// <returns>true - ���� ������� ���������.</returns>
			virtual bool _NeedHelp(NeedHelpMessage^ msg) override;
			private:
			void _NotifyBody(Object^ arg);
		};
	}
}


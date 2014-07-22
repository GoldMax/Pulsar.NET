#pragma once
#include "Stdafx.h"

using namespace Pulsar;

namespace Pulsar
{
	namespace Server
	{
		ref class MessageBusRecipient	: Subscriber
		{
			public:
			/// <summary>
			/// Тип блокировки по умолчанию объекта получателя.
			/// </summary>
			property LockType DefaultLock;
			/// <summary>
			/// Oбъект получателя сообщений. 
			/// </summary>
			property IReadWriteLockObject^ Recipient;
		
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			MessageBusRecipient(Delegate^ handler, LockType defLock):  Subscriber(handler)
			{
				DefaultLock = defLock;
			}
		};
	}
}


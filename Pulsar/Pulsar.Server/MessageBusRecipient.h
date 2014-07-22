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
			/// ��� ���������� �� ��������� ������� ����������.
			/// </summary>
			property LockType DefaultLock;
			/// <summary>
			/// O����� ���������� ���������. 
			/// </summary>
			property IReadWriteLockObject^ Recipient;
		
			/// <summary>
			/// ���������������� �����������.
			/// </summary>
			MessageBusRecipient(Delegate^ handler, LockType defLock):  Subscriber(handler)
			{
				DefaultLock = defLock;
			}
		};
	}
}


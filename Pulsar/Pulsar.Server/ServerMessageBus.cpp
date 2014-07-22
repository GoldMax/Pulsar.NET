#include "StdAfx.h"
#include "ServerMessageBus.h"
#include <msclr\auto_handle.h>

using namespace Pulsar;
using namespace Pulsar::Server;
//using namespace System::Threading::Tasks;

void ServerMessageBus::_RegisterRecipient(Type^ msgType, MessageKind kind, MessageHandler^ handler,	LockType defaultLock)
{
	if(handler->Target == nullptr && defaultLock != LockType::NoLock)
		throw gcnew PulsarException("—татические получатели не могут иметь тип блокировки {0}!", defaultLock);
	IReadWriteLockObject^ serv = nullptr;
	if(handler->Target != nullptr)
	{
	 serv = dynamic_cast<IReadWriteLockObject^>(handler->Target);
		if(serv == null)
		{
			for each(Servant^ s in GOL::GetByTypeDerived<Servant^>())
			 if(handler->Target == s->ServedObject)
				{
					serv = s;
					break;
				} 
		}
		if(serv == nullptr && defaultLock != LockType::NoLock && ServerParams::IsServer)
			throw gcnew PulsarException("“ип блокировки не может быть равен {0}!", defaultLock);
	}
	MessageBusRecipient^ rcpt = gcnew MessageBusRecipient(handler, defaultLock);
	rcpt->Recipient = serv;
	
	if(kind.HasFlag(MessageKind::Pooling))
	{
	 lock xxx(_pooling);
	 WeakEvent<IMessage^, MessageHandlerCallContext^>^ eve = nullptr;
	 if(_pooling->TryGetValue(msgType, eve) == false)
	 	_pooling->Add(msgType, eve = gcnew WeakEvent<IMessage^,MessageHandlerCallContext^>());
	 eve->Add(rcpt);
	}
	if(kind.HasFlag(MessageKind::Notify))
	{
		lock xxx(_notify);
		WeakEvent<IMessage^, MessageHandlerCallContext^>^ eve = nullptr;
		if(_notify->TryGetValue(msgType, eve) == false)
			_notify->Add(msgType, eve = gcnew WeakEvent<IMessage^,MessageHandlerCallContext^>());
		eve->Add(rcpt);
	}
	if(kind.HasFlag(MessageKind::NeedHelp))
	{
		lock xxx(_needHelp);
		WeakEvent<IMessage^, MessageHandlerCallContext^>^ eve = nullptr;
		if(_needHelp->TryGetValue(msgType, eve) == false)
			_needHelp->Add(msgType, eve = gcnew WeakEvent<IMessage^,MessageHandlerCallContext^>());
		eve->Add(rcpt);
	}

}

void ServerMessageBus::_Notify(IMessage^ msg)
{
	if(msg == nullptr)
		throw gcnew ArgumentNullException("msg");
	WeakEvent<IMessage^, MessageHandlerCallContext^>^	eve = nullptr;
	{
		lock xxx(_notify);
		if(_notify->TryGetValue(msg->MsgType, eve) == false)
			return;
		if(eve->Subscribers() == nullptr)
			return;
	}
	//IAction2Invoker^ inv = gcnew Action2Invoker<IMessage^,MessageHandlerCallContext^>();
	if(ThreadContext::NoAsyncNotify == false)
	 PulsarThreadPool::Run(_notifyAction, gcnew array<Object^> { msg, eve });
	else
	 _NotifyBody(gcnew array<Object^> { msg, eve });
}

void ServerMessageBus::_NotifyBody(Object^ arg)
{
 array<Object^>^ arr;
	try
	{
	 arr = static_cast<array<Object^>^>(arg);	
		auto msg = static_cast<IMessage^>(arr[0]);
		auto eve = static_cast<WeakEvent<IMessage^, MessageHandlerCallContext^>^>(arr[1]);
		//IAction2Invoker^ inv = static_cast<IAction2Invoker^>(arr[2]);
	 for each(MessageBusRecipient^ x in eve->Subscribers())
	 {
		 String^ xname = x->TargetType->Name;
			if(dynamic_cast<IGlobalObjectMeta^>(x->Recipient) != null)
			{
			 xname = dynamic_cast<IGlobalObjectMeta^>(x->Recipient)->GlobalName;
				if(xname == nullptr)
				 xname = dynamic_cast<IGlobalObjectMeta^>(x->Recipient)->OID->ToString();
			}

			Logger::Log(3, "  => Notify [{3}]: {0}.{1}(IMessage<{2}> : Reason = {4})", true,
				x->TargetType->Name, x->MethodName, msg->MsgObject->GetType(),
				xname, msg->Reason == nullptr ? "null" : msg->Reason);
	  MessageHandlerCallContext^ cox = gcnew MessageHandlerCallContext(x->DefaultLock,false);
	  ReCall:
	  try
	  {
				if(x->Recipient == nullptr)
				{
					if(cox->CurrentLock != LockType::NoLock && ServerParams::IsServer)
						throw gcnew PulsarException("“ип блокировки не может быть равен {0}!",	x->DefaultLock);
				}
				else if(cox->CurrentLock == LockType::ReadLock)
					x->Recipient->BeginRead();
				else if(cox->CurrentLock == LockType::WriteLock)
					x->Recipient->BeginWrite();

				x->Invoke(msg, cox);
				//x->Invoke<IMessage^, MessageHandlerCallContext^>(msg, cox);
	  }
	  catch (Exception^ exc)
	  {
	   Logger::LogError(exc);
	   cox->CallResult = MessageHandlerCallResult::DoneWithoutModification;
	  }
	  finally
	  {
				if(x->Recipient !=	 nullptr	)
					if(cox->CurrentLock == LockType::ReadLock)
						x->Recipient->EndRead();
					else if(cox->CurrentLock == LockType::WriteLock)
							x->Recipient->EndWrite();
					//{
					//	try
					//	{
					//		if(cox->CallResult.HasFlag(MessageHandlerCallResult::DoneWithoutModification) == false)
					//		{
					//			x->Servant->Version++;
					//			x->Servant->Save();
					//		}
					//	}
					//	finally
					//	{
					//	}
					//}
	  }
			if(cox->CallResult.HasFlag(MessageHandlerCallResult::ReCallWithWriteLock) &&
				cox->CurrentLock != LockType::WriteLock)
			{
				delete cox;
				cox = gcnew MessageHandlerCallContext(LockType::WriteLock, false);
				goto ReCall;
			}
			delete cox;
	 }
	}
	catch(Exception^ exc)
	{
	 Logger::LogError(exc);
	}
	finally
	{
		if(arr != nullptr)
		 delete arr;
		// Ќе нужен, так как поток запускаетс€ через PulsarThreadPool
		//if(ThreadContext::NoAsyncNotify == false)
		// ThreadContext::Close();
	}
}

bool ServerMessageBus::_Polling(IMessage^ msg)
{
	if(msg == nullptr)
		throw gcnew ArgumentNullException("msg");
	WeakEvent<IMessage^, MessageHandlerCallContext^>^	eve = nullptr;
	{
		lock xxx(_pooling);
		if(_pooling->TryGetValue(msg->MsgType, eve) == false)
			return true;
		if(eve->Subscribers() == nullptr)
			return true;
	}
	for each(MessageBusRecipient^ x in eve->Subscribers())
	{
		String^ xname = x->TargetType->Name;
		if(dynamic_cast<IGlobalObjectMeta^>(x->Recipient) != null)
		{
			xname = dynamic_cast<IGlobalObjectMeta^>(x->Recipient)->GlobalName;
			if(xname == nullptr)
				xname = dynamic_cast<IGlobalObjectMeta^>(x->Recipient)->OID->ToString();
		}

		Logger::Log(3, "  => Polling [{3}]: {0}.{1}(IMessage<{2}> : Reason = {4})", true,
			x->TargetType->Name, x->MethodName, msg->MsgObject->GetType(),
			xname, msg->Reason == nullptr ? "null" : msg->Reason);
		MessageHandlerCallContext^ cox = gcnew MessageHandlerCallContext(x->DefaultLock,true);
		ReCall:
		try
		{
			if(x->Recipient == nullptr)
			{
				if(cox->CurrentLock != LockType::NoLock && ServerParams::IsServer)
					throw gcnew PulsarException("“ип блокировки не может быть равен {0}!",
					x->DefaultLock);
			}
			else if(cox->CurrentLock == LockType::ReadLock)
				x->Recipient->BeginRead();
			else if(cox->CurrentLock == LockType::WriteLock)
				x->Recipient->BeginWrite();

			x->Invoke(msg, cox);
			//x->Invoke<IMessage^, MessageHandlerCallContext^>(msg, cox);
		}
		catch(...) //(Exception exc)
		{
			delete cox;
			throw;
		}
		finally
		{
			if(x->Recipient !=	 nullptr	)
				if(cox->CurrentLock == LockType::ReadLock)
					x->Recipient->EndRead();
				else if(cox->CurrentLock == LockType::WriteLock)
					x->Recipient->EndWrite();
				//{
				//	try
				//	{
				//		if(cox->CallResult.HasFlag(MessageHandlerCallResult::DoneWithoutModification) == false)
				//		{
				//			x->Servant->Version++;
				//			x->Servant->Save();
				//		}
				//	}
				//	finally
				//	{
				//		x->Servant->EndWrite();
				//	}
				//}
		}
		if(cox->CallResult.HasFlag(MessageHandlerCallResult::ReCallWithWriteLock) &&
			cox->CurrentLock != LockType::WriteLock)
		{
			delete cox;
			cox = gcnew MessageHandlerCallContext(LockType::WriteLock, true);
			goto ReCall;
		}
		bool quit = cox->CallResult.HasFlag(MessageHandlerCallResult::BreakPolling);
		delete cox;
		if(quit)
			return false;
	}
	return true;
}

bool ServerMessageBus::_NeedHelp(NeedHelpMessage^ msg)
{
	if(msg == nullptr)
		throw gcnew ArgumentNullException("msg");
	WeakEvent<IMessage^, MessageHandlerCallContext^>^	eve = nullptr;
	{
		lock xxx(_needHelp);
		if(_needHelp->TryGetValue(NeedHelpRecipient::typeid, eve) == false)
			return false;
		if(eve->Subscribers() == nullptr)
			return false;
	}
	for each(MessageBusRecipient^ x in eve->Subscribers())
	{		 
		IGlobalObjectMeta^	ix = dynamic_cast<IGlobalObjectMeta^>(x->Recipient);
		if(ix != nullptr && msg->RegName != nullptr)
		{
			if(ix->GlobalName != msg->RegName)
				continue;
		}
		else if(x->Recipient != nullptr && x->Recipient->GetType() != msg->RicipientType)
			continue;

		String^ xname = x->TargetType->Name;
		if(ix != null)
		{
			xname = ix->GlobalName;
			if(xname == nullptr)
				xname = ix->OID->ToString();
		}

		MessageHandlerCallContext^ cox = gcnew MessageHandlerCallContext(x->DefaultLock,true);
		ReCall:
		try
		{
			if(x->Recipient == nullptr)
			{
				if(cox->CurrentLock != LockType::NoLock && ServerParams::IsServer)
					throw gcnew PulsarException("“ип блокировки не может быть равен {0}!",	x->DefaultLock);
			}
			else if(cox->CurrentLock == LockType::ReadLock)
				x->Recipient->BeginRead();
			else if(cox->CurrentLock == LockType::WriteLock)
				x->Recipient->BeginWrite();

			x->Invoke(msg, cox);
			//x->Invoke<NeedHelpMessage^, MessageHandlerCallContext^>(msg, cox);
		}
		catch(Exception^ exc)
		{
			Logger::LogError(exc);
			cox->CallResult = MessageHandlerCallResult::DoneWithoutModification;
			delete cox;
			throw;
		}
		finally
		{
			if(x->Recipient !=	 nullptr	)
				if(cox->CurrentLock == LockType::ReadLock)
					x->Recipient->EndRead();
				else if(cox->CurrentLock == LockType::WriteLock)
					x->Recipient->EndWrite();
				//{
				//	try
				//	{
				//		if(cox->CallResult.HasFlag(MessageHandlerCallResult::DoneWithoutModification) == false)
				//		{
				//			x->Servant->Version++;
				//			x->Servant->Save();
				//		}
				//	}
				//	finally
				//	{
				//		x->Servant->EndWrite();
				//	}
				//}
		}
		if(cox->CallResult.HasFlag(MessageHandlerCallResult::ReCallWithWriteLock) &&
			cox->CurrentLock != LockType::WriteLock)
		{
			delete cox;
			cox = gcnew MessageHandlerCallContext(LockType::WriteLock, false);
			goto ReCall;
		}
		bool helped = cox->CallResult.HasFlag(MessageHandlerCallResult::Helped);
		delete cox;
		if(helped)
		{
			Logger::Log(3, "  => NeedHelp [{3}]: {0}.{1}(IMessage<{2}> : Reason = {4})", true,
				x->Target->GetType()->Name, x->MethodName, msg->MsgObject->GetType(),
				xname, msg->Reason == nullptr ? "null" : msg->Reason);
			return true;
		}
	}
	return false;
}


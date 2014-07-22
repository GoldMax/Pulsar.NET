#pragma once
#include "Stdafx.h"

//[assembly:System::Runtime::CompilerServices::InternalsVisibleTo("Pulsar.Server")];

using namespace System::Collections::Generic;
using namespace System::Net::Sockets;
using namespace System::Linq;

namespace Pulsar
{
	/// <summary>
	/// Класс контекста запроса к Пульсару.
	/// </summary>
	public ref class ThreadContext
	{
	internal:
		static Action<PulsarAnswer^, bool>^ _sendToClient;

	private:
		[ThreadStaticAttribute]
		static NetworkStream^ _netStream;
		[ThreadStaticAttribute]
		static PulsarQuery^ _query;
		[ThreadStaticAttribute]
		static HashSet<Type^>^ _noStubTypes = nullptr;
		[ThreadStaticAttribute]
		static HashSet<Type^>^ _asEmptyTypes = nullptr;
		[ThreadStaticAttribute]
		static HashSet<Object^>^ _noStubObject = nullptr;
		[ThreadStaticAttribute]
		static HashSet<Object^>^ _asEmptyObjects = nullptr;
		[ThreadStaticAttribute]
		static HashSet<IReadWriteLockObject^>^ _needUnlock = nullptr;
		[ThreadStaticAttribute]
		static HashSet<GlobalObject^>^ _needSave = nullptr;
		[ThreadStaticAttribute]
		static bool _noAsyncNotify = false;
		[ThreadStaticAttribute]
		static WeakEvent<Action^>^ _onExit = nullptr;
		//-------------------------------------------------------------------------------------
#pragma region Properties
	public:
		/// <summary>
		/// Объект запроса 
		/// </summary>
		static property PulsarQuery^ Query	{ PulsarQuery^ get() { return _query; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Поток сетевого соединения.
		/// </summary>
		static property NetworkStream^ NetStream
		{
			NetworkStream^ get() { return _netStream; }
		internal: void set(NetworkStream^ value) { _netStream = value; }
		}
		//	//-------------------------------------------------------------------------------------
		//	/// <summary>
		//	/// Пользователь, вызвавший запрос.
		//	/// </summary>
		//	public static dynamic User
		//	{
		//		get { return context.User; }
		//		set { context.User = value; }
		//	}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет необходимость отсылки сообщений клиенту.
		/// </summary>
		static property bool Verbose 
		{ 
			bool get() { return _query == nullptr ? false : _query->Params.HasFlag(PulsarQueryParams::Verbose); } }
		/// <summary>
		/// Определяет, что запрос модифицирует объект.
		/// </summary>
		static property bool Modify 
		{ 
			bool get() { return _query == nullptr ? false : _query->Params.HasFlag(PulsarQueryParams::Modify); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет набор типов, которые не должны быть заглушены при сериализации ответа.
		/// </summary>
		static property HashSet<Type^>^ NoStubTypes { HashSet<Type^>^ get() { return _noStubTypes; } }
		/// <summary>
		/// Определяет набор типов, которые не должны быть сериализованы при сериализации ответа.
		/// </summary>
		static property HashSet<Type^>^ AsEmptyTypes { HashSet<Type^>^ get() { return _asEmptyTypes; } }
		/// <summary>
		/// Определяет набор объектов, которые не должны быть заглушены при сериализации ответа.
		/// </summary>
		static property HashSet<Object^>^ NoStubObjects { HashSet<Object^>^ get() { return _noStubObject; } }
		/// <summary>
		/// Определяет набор объектов, которые не должны быть сериализованы при сериализации ответа.
		/// </summary>
		static property HashSet<Object^>^ AsEmptyObjects { HashSet<Object^>^ get() { return _asEmptyObjects; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, что для уведомительных сообщений шины сообщений не должны использоваться отдельные потоки.
		/// </summary>
		static property bool NoAsyncNotify
		{
			bool get() { return _noAsyncNotify; }
			void set(bool value) { _noAsyncNotify = value; }
		}
		/// <summary>
		/// Набор объектов, для которых нужно вызвать снятие блокировки при завершении потока.
		/// </summary>
		static property HashSet<IReadWriteLockObject^>^ NeedUnlock 
		{ 
			HashSet<IReadWriteLockObject^>^ get() 
			{ 
				if(_needUnlock == nullptr)
					_needUnlock	= gcnew HashSet<IReadWriteLockObject^>();	
				return _needUnlock; 
			} 
		}
		/// <summary>
		/// Набор объектов, для которых нужно вызвать сохранение при завершении потока.
		/// </summary>
		static property HashSet<GlobalObject^>^ NeedSave 
		{ 
			HashSet<GlobalObject^>^ get() 
			{ 
				if(_needSave == nullptr)
					_needSave	= gcnew HashSet<GlobalObject^>();	
				return _needSave; 
			} 
		}
		/// <summary>
		/// Событие, вызываемое при завершении потока
		/// </summary>
		static event Action^ ThreadExit
		{
			void add(Action^ handler)
			{
				if(_onExit == nullptr)
					_onExit = gcnew WeakEvent<Action^>();
				_onExit->Add(handler);
			}
			void remove(Action^ handler)
			{
				_onExit->Remove(handler);
				if(_onExit->Count == 0)
				{
					delete _onExit;
					_onExit = nullptr;
				}
			}
			void raise()
			{
				if(_onExit != nullptr)
					_onExit->Raise();
			}
		}
#pragma endregion
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#pragma region << Methods >>
	public:
		/// <summary>
		/// Метод инициализации контекста.
		/// </summary>
		static void Init(PulsarQuery^ query)
		{
			_query = query;
			_noStubTypes = gcnew HashSet<Type^>(_query == nullptr || _query->NoStubTypes == nullptr ? Type::EmptyTypes : _query->NoStubTypes);
			_asEmptyTypes = gcnew HashSet<Type^>(_query == nullptr || _query->NoSerTypes == nullptr ? Type::EmptyTypes : _query->NoSerTypes);
			_noStubObject = gcnew HashSet<Object^>();
			_asEmptyObjects = gcnew HashSet<Object^>();
			_noAsyncNotify = false;
			delete _onExit;

			// не надо, так как до вызова метода десериализация query может открыть объекты	на запись (новые точно)
			//if(_needUnlock != nullptr)
			//{
			// _needUnlock->Clear();
			// delete _needUnlock;
			//}
			//if(_needSave != nullptr)
			//{
			// _needSave->Clear();
			// delete _needSave;
			//}
		}
		/// <summary>
		/// Метод завершения серверного потока. 
		/// Сохраняет и разблокирует объекты, очищает внутренние структуры. 
		/// </summary>
		static void CloseThread()
		{
		 //Logger::Log(0,"!!! Закрытие потока {0} !!!", true, System::Threading::Thread::CurrentThread->ManagedThreadId);
			if(_onExit != null)
				try
			{
				_onExit->Raise();
				delete _onExit;
				_onExit = nullptr;
			} 
			catch (Exception^ e)
			{
				if (dynamic_cast<System::Reflection::TargetInvocationException^>(e) != nullptr)
					e = e->InnerException;
				String^ s = String::Format(" ThreadExit {0} ({1})", 	e->Message, e->StackTrace);
				Exception exc(s, e);
				Logger::LogError(%exc);
			}
			if(GOL::IsInitMode == false &&_needSave != nullptr)
				for each(GlobalObject^ go in _needSave)
				{
					try
					{ 
						IReadWriteLockObject^ ilock = static_cast<IReadWriteLockObject^>(go);
						if(ilock->IsLocked == false)
							ilock->BeginRead();
						GOL::SaveGlobalObject(go);
					}
					catch (Exception^ e)
					{
						if (dynamic_cast<System::Reflection::TargetInvocationException^>(e) != nullptr)
							e = e->InnerException;
						String^ s = String::Format(" Сохранение объекта {2} - {0} ({1})", 	e->Message, e->StackTrace, go);
						Exception exc(s, e);
						Logger::LogError(%exc);
					}
				}
				if(_needUnlock != nullptr)
					for each(IReadWriteLockObject^ i in Enumerable::Reverse(_needUnlock))
					{
						try
						{ 
							i->ClearAllLocks();
						}
						catch (Exception^ e)
						{
							if (dynamic_cast<System::Reflection::TargetInvocationException^>(e) != nullptr)
								e = e->InnerException;
							String^ s = String::Format(" Снятие блокировки объекта {2} - {0} ({1})", 	e->Message, e->StackTrace, i);
							Exception exc(s, e);
							Logger::LogError(%exc);
						}
					}

					_noAsyncNotify = false;
					delete _noStubTypes;
					delete _asEmptyTypes;
					delete _noStubObject;
					delete _asEmptyObjects;
					if(_needUnlock != nullptr)
					{
						_needUnlock->Clear();
						delete _needUnlock;
					}
					if(_needSave != nullptr)
					{
						_needSave->Clear();
						delete _needSave;
					}	

					if (_netStream != nullptr)
					{
						_netStream->Flush();
						delete _netStream;
					}
					if(_query != nullptr)
						delete _query;
		}


		//	/// <summary>
		//	/// Посылает сообщение клиенту, если QueryInfo.Verbose = true
		//	/// </summary>
		//	/// <param name="s">Строка сообщения.</param>
		//	/// <param name="args">Параметры форматирования строки сообщения.</param>
		//	public static void SendMessage(string s, params object[] args)
		//	{
		//		if (QueryContext.Verbose)
		//			SendMessage(new PulsarAnswer()
		//		{
		//			Answer = PulsarAnswerStatus.Message,
		//				Return = String.Format(String.Format(s, args))
		//		});
		//	}
		//	/// <summary>
		//	/// Посылает сообщение клиенту, если QueryInfo.Verbose = true
		//	/// </summary>
		//	/// <param name="answer">Параметры сообщения.</param>
		//	public static void SendMessage(PulsarAnswer answer)
		//	{
		//		if (QueryContext.Verbose)
		//			sendToClient(answer, false);
		//	}

#pragma endregion
		//-------------------------------------------------------------------------------------
	};
}


#include "Stdafx.h"

using namespace System::IO;
using namespace System::Linq;
using namespace System::Net;
using namespace System::Net::Sockets;
using namespace System::Reflection;
//using namespace System::Threading;
using namespace System::Xml;

using namespace Pulsar;
using namespace Pulsar::Server;
using namespace Pulsar::Serialization;

PulsarCore::PulsarCore()
{
	try
	{
		_active = false;
		_autoStart = gcnew Dictionary<String^,Pulsar::OID^>();

		ThreadContext::_sendToClient	= gcnew Action<PulsarAnswer^, bool>(this,&PulsarCore::SendToClient); 
		PulsarThreadPool::OnExitDefaultAction = gcnew Action(&Pulsar::ThreadContext::CloseThread);
		ServerMessageBus::CurrentBus = gcnew ServerMessageBus();

		Init(nullptr);
		
		Logger::Log(1, " Сервер инициализирован со статусом {0}. ", true, Status);
	}
	catch(Exception^ Err)
	{
		Logger::LogError(Err);
		throw;
	}

}

void PulsarCore::Init(String^ objType) 
{
	try
	{
		ServerParams::Default = gcnew ServerParams();
		ServerParams::DefaultRegistryKey = "Software\\EKS\\Pulsar\\";
		#pragma region LoadParams
		Logger::LogLevel = (Byte)static_cast<int>(ServerParams::GetParam("","LogLevel", (int)Logger::LogLevel));
		Logger::LogToConsole = 
			safe_cast<int>(ServerParams::GetParam("","LogToConsole", Logger::LogToConsole ? 1 : 0)) != 0;
		ServerParams::Default->IsSqlStorage	= 	
		 safe_cast<int>(ServerParams::GetParam("","IsSqlStorage", safe_cast<Object^>(0))) > 0;
		ServerParams::Default->Port = 
			safe_cast<int>(ServerParams::GetParam("","Port", ServerParams::Default->Port));
		ServerParams::Default->ReceiveTimeOut = 
			safe_cast<int>(ServerParams::GetParam("","ReceiveTimeout", ServerParams::Default->ReceiveTimeOut));
		ServerParams::Default->SendTimeOut = 
			safe_cast<int>(ServerParams::GetParam("","SendTimeout", ServerParams::Default->SendTimeOut));
		ServerParams::Default->ReceiveBufferSize = 
			safe_cast<int>(ServerParams::GetParam("","ReceiveBufferSize", ServerParams::Default->ReceiveBufferSize));
		ServerParams::Default->SendBufferSize = 
			safe_cast<int>(ServerParams::GetParam("","SendBufferSize", ServerParams::Default->SendBufferSize));
		ServerParams::Default->StorageConnectionString =
			safe_cast<String^>(ServerParams::GetParam("","StorageConnectionString", ServerParams::Default->StorageConnectionString));
		ServerParams::Default->ClientVersion = 
			(UInt32)safe_cast<UInt32>(ServerParams::GetParam("","ClientVersion", ServerParams::Default->ClientVersion));
		ServerParams::Default->ClientUpdatePath =
			safe_cast<String^>(ServerParams::GetParam("","ClientUpdatePath", ServerParams::Default->ClientUpdatePath));

		#pragma endregion

		Logger::Log(0, "Запуск сервера Пульсара ({0})... ",true,DateTime::Now.ToString());
		if(ServerParams::Default->IsSqlStorage)
		{
			System::Data::SqlClient::SqlConnectionStringBuilder sb(ServerParams::Default->StorageConnectionString);
			Logger::Log(3, " Хранилище - SQL ({0}  {1}), Порт - {2}", true, sb.DataSource, sb.InitialCatalog, 
																																																																			ServerParams::Default->Port);
		}
		else
			Logger::Log(3, " Хранилище - Файловая система ({0}), Порт - {1}", true, ServerParams::Default->StorageConnectionString,
																																																																											ServerParams::Default->Port);

		Logger::Log(1, " Серверная сборка мусора - {0}", true,  System::Runtime::GCSettings::IsServerGC ? "Да" : "Нет");
		Logger::Log(1, " Инициализация GOL ... ", true);
		GOL::IsInitMode = true;
		GOL::OnGlobalObjectBeginRead = gcnew ReadWriteLockObjectHandler(&GlobalObjectsManager::BeginRead);
		GOL::OnGlobalObjectEndRead = gcnew ReadWriteLockObjectHandler(&GlobalObjectsManager::EndRead);
		GOL::OnGlobalObjectBeginWrite = gcnew ReadWriteLockObjectHandler(&GlobalObjectsManager::BeginWrite);
		GOL::OnGlobalObjectEndWrite = gcnew ReadWriteLockObjectHandler(&GlobalObjectsManager::EndWrite);
		GOL::OnGlobalObjectClearAllLocks = gcnew ReadWriteLockObjectHandler(&GlobalObjectsManager::ClearAllLocks);

		GOL::LoadGlobalObjectMethod = gcnew Func<GlobalObject^,bool,bool>(&GlobalObjectsManager::LoadGO);
		GOL::LoadGlobalObjectsMethod = gcnew Action<System::Collections::Generic::IEnumerable<GlobalObject^>^>(&GlobalObjectsManager::LoadGOs);
		GOL::SaveGlobalObjectMethod = gcnew Action<GlobalObject^>(&GlobalObjectsManager::SaveGO);
		this->ServedObject = this;
		static_cast<IGlobalObjectMeta^>(this)->GlobalName = "Pulsar";
		GOL::AddHard(this);
		_autoStart->Add("Pulsar",this->OID);

		Logger::Log(1, " Инициализация стартовых объектов ... ", true);
		if(ServerParams::Default->IsSqlStorage)
		{
			//--- Debbuger Break --- //
			if(System::Diagnostics::Debugger::IsAttached)
				System::Diagnostics::Debugger::Break();
			//--- Debbuger Break --- //
		}
		else	if(File::Exists(ServerParams::Default->StorageConnectionString+"\\"+"AutoStart.xml") == false)
			Logger::Log(1, "   Файл AutoStart.xml не найден!", true);
		else
		{
			XmlDocument doc;
			doc.Load(ServerParams::Default->StorageConnectionString+"\\"+"AutoStart.xml");
			XmlNode^ root = doc.DocumentElement;

			for each(Object^ n in root)
			{
				XmlNode^ node = static_cast<XmlNode^>(n);
				if(node->HasChildNodes == false)
					continue;
				if(node->ChildNodes->Count < 3)
					throw gcnew Exception("Ошибка в структуре файла AutoStart.xml!");
				GlobalObject^ go = nullptr;
				try
				{
					String^ id = nullptr;
					String^ gn = nullptr;
					String^ ot = nullptr;
					String^ st = nullptr;
					for each(XmlNode^ cn in node->ChildNodes)
					{
						if(cn->Name == "OID")
							id = cn->InnerText;
						else	if(cn->Name == "GlobalName")
							gn = cn->InnerText;
						else	if(cn->Name == "ObjectClassName")
							ot = cn->InnerText;
						else	if(cn->Name == "ServantClassName")
							st = cn->InnerText;
					}
					if((id == nullptr && gn == nullptr) || ot == nullptr)
						throw gcnew Exception("Ошибка в структуре элемента Object!");
					Logger::Log(2, "  {0} ...\t ", false, gn);
					go = CreateAutoStartObject(id, gn, st, ot);
					if(static_cast<IGlobalObjectMeta^>(go)->GlobalName != nullptr)
						_autoStart->Add(static_cast<IGlobalObjectMeta^>(go)->GlobalName, go->OID);
					else
						_autoStart->Add(go->OID->ToString(), go->OID);
					if(dynamic_cast<Servant^>(go) != nullptr)
						Logger::Log(2, "\t{0}", true, dynamic_cast<Servant^>(go)->Status);
					else
						Logger::Log(2, "\tOK", true);
				}
				catch(Exception^ err)
				{
					if(dynamic_cast<System::Reflection::TargetInvocationException^>(err) != nullptr)
						err = err->InnerException;
					if(go != nullptr && dynamic_cast<Servant^>(go) != nullptr)
						dynamic_cast<Servant^>(go)->Status = ServantStatus::Broken;
					Logger::LogError(err);
					Status = ServantStatus::Error;
				}
			}
		}

		Logger::Log(1, " Инициализация бизнес правил ... ", true);
		if(ServerParams::Default->IsSqlStorage)
		{
			//--- Debbuger Break --- //
			if(System::Diagnostics::Debugger::IsAttached)
				System::Diagnostics::Debugger::Break();
			//--- Debbuger Break --- //
		}
	 else	if(File::Exists(ServerParams::Default->StorageConnectionString+"\\"+"Rules.xml") == false)
			Logger::Log(1, "   Файл Rules.xml не найден!", true);
		else
		{
			XmlDocument doc;
			doc.Load(ServerParams::Default->StorageConnectionString+"\\"+"Rules.xml");
			XmlNode^ root = doc.DocumentElement;

			for each(Object^ n in root)
			{
				try
				{
					XmlNode^ node = static_cast<XmlNode^>(n);
					if(node->Name != "Rule")
						continue;
					String^ ts = node->InnerText;
					if(String::IsNullOrWhiteSpace(ts))
						continue;

					Type^ t = Type::GetType(ts);
					if(t == nullptr)
						throw gcnew PulsarException("Не удалось найти тип [{0}]!", ts);
					t->TypeInitializer->Invoke(nullptr, nullptr);

					if(t != nullptr)
						Logger::Log(2, "\t{0}", true, t->FullName);
				}
				catch(Exception^ err)
				{
					if(dynamic_cast<System::Reflection::TargetInvocationException^>(err) != nullptr)
						err = err->InnerException;
					Logger::LogError(err);
					Status = ServantStatus::Error;
				}
			}
		}

		GOL::IsInitMode = false;

		if(Status == ServantStatus::Ready || Status ==ServantStatus::NotInit)
		{
			Logger::Log	(1, " Подготовка к запуску ...", true);
			for each(Pulsar::OID^ id in _autoStart->Values)
			{
				Servant^ s = dynamic_cast<Servant^>(GOL::GetForRead(id));
				if(s != nullptr)
					s->BeforeServerRun();
			}
			Logger::Log(1, " Инициализация планировщика ... ", true);
			try
			{
				_schedulerServant = (SchedulerServant^)CreateAutoStartObject(nullptr, "Scheduler",
					"Pulsar.Server.SchedulerServant, Pulsar.Server",	"Pulsar.Scheduler, Pulsar.Bricks");
				if(_schedulerServant->Status == ServantStatus::NotInit)
					_schedulerServant->Status = ServantStatus::Ready;
				dynamic_cast<Pulsar::Scheduler^>(_schedulerServant->ServedObject)->UseExternalPulse = true;
				_autoStart->Add("Scheduler",_schedulerServant->OID);
				Logger::Log(2, "\t{0}", true, _schedulerServant->Status);
			}
			catch(Exception^ err)
			{
				if(dynamic_cast<System::Reflection::TargetInvocationException^>(err) != nullptr)
					err = err->InnerException;
				if(_schedulerServant != nullptr)
					_schedulerServant->Status = ServantStatus::Broken;
				Logger::LogError(err);
				Status = ServantStatus::Error;
			}
		}

		if(Status == ServantStatus::NotInit)
			Status = ServantStatus::Ready;
	}
	catch (Exception^ exc)
	{
		Logger::LogError(exc);
		Status = ServantStatus::Broken;
	}
	ThreadContext::NeedSave->Clear();
	ThreadContext::CloseThread();
}

GlobalObject^ PulsarCore::CreateAutoStartObject(String^ oid, String^ regName, String^ servType, 
																																																String^ objType)
{
	if(objType == servType && oid == nullptr)
		throw gcnew Exception("Для старта объекта, являющегося сервантом, необходимо указать OID!");
	Pulsar::OID^ id = oid == nullptr ? gcnew Pulsar::OID() : gcnew Pulsar::OID(oid);
	GlobalObject^ res = nullptr;
	Type^ t = nullptr;
	if(servType != nullptr)
	{
		t = Type::GetType(servType);
		if(t == nullptr)
			throw gcnew PulsarException("Не удалось найти тип [{0}]!", servType);
		if(t != Servant::typeid && t->IsSubclassOf(Servant::typeid) == false)
			throw gcnew PulsarException("Тип [{0}] указан как тип слуги, но он не наследует Pulsar.Server.Servant!",t);
	}
	if(t != nullptr)
	{
		res = GlobalObject::CreateWithOID(t, id, nullptr, nullptr);
		static_cast<IGlobalObjectMeta^>(res)->GlobalName = regName;
		GOL::AddHard(res);
		static_cast<Servant^>(res)->Init(objType);
	}
	else
	{ 
		//--- Debbuger Break --- //
		if(System::Diagnostics::Debugger::IsAttached)
			System::Diagnostics::Debugger::Break();
		//--- Debbuger Break --- //

		if(oid == nullptr)
			throw gcnew Exception("Для старта глобального объекта необходимо указать OID!");
		t = Type::GetType(objType);
		if(t == nullptr)
			throw gcnew PulsarException("Не удалось найти тип [{0}]!", objType);
		res = GlobalObject::CreateUninitialized(t, id);
		try
		{
			GOL::LoadGlobalObject(res, true);
		}
		catch(...)
		{
			Pulsar::Reflection::Dynamic::ReflectionHelper::InvokeCtor(t,res, nullptr, nullptr);
		}
		((IGlobalObjectMeta^)res)->GlobalName = regName;
		GOL::AddHard(res);
	}
	return res;
}

void PulsarCore::Save()
{
	try
	{
		{
			lock xxx(ServerParams::Default);
			ServerParams::SetParam("","LogLevel", Logger::LogLevel);
			ServerParams::SetParam("","LogToConsole", Logger::LogToConsole);
			ServerParams::SetParam("","IsSqlStorage", ServerParams::Default->IsSqlStorage);
			ServerParams::SetParam("","Port", ServerParams::Default->Port);
			ServerParams::SetParam("","ReceiveTimeout", ServerParams::Default->ReceiveTimeOut);
			ServerParams::SetParam("","SendTimeout", ServerParams::Default->SendTimeOut);
			ServerParams::SetParam("","ReceiveBufferSize", ServerParams::Default->ReceiveBufferSize);
			ServerParams::SetParam("","SendBufferSize", ServerParams::Default->SendBufferSize);
			ServerParams::SetParam("","StorageConnectionString", ServerParams::Default->StorageConnectionString);
			ServerParams::SetParam("","ClientVersion", ServerParams::Default->ClientVersion);
			ServerParams::SetParam("","ClientUpdatePath", ServerParams::Default->ClientUpdatePath);
		}
	}
	catch(Exception^ Err)
	{
		Logger::LogError(Err);
		throw;
	}
}

void PulsarCore::Start()
{
	try
	{
		System::Threading::Thread::CurrentThread->Name = "PulsarMainThread";

		if(Status == ServantStatus::Ready && _schedulerServant->Status == ServantStatus::Ready)
		{
			Logger::Log(1, " Запуск планировщика ... ", false);
			dynamic_cast<Pulsar::Scheduler^>(_schedulerServant->ServedObject)->Start();
			Logger::Log(1, "OK");
		}

		Logger::Log(1, " Запуск TCP сервера ... ", false);
		TcpListener listener(IPAddress::Any, ServerParams::Default->Port);
		listener.Start(100);
		Logger::Log(1, "OK");
		Logger::Log(0, "Пульсар запущен (" + DateTime::Now.ToString() + ")");
		Logger::Log(1, "------------------------------------------------------");

		_active = true;
		while(_active)
		{
			try
			{
				if(Console::KeyAvailable)
				{
					ConsoleKeyInfo ki = Console::ReadKey(true);
					if(ki.Key == ConsoleKey::C)
						Console::Clear();
					else if(ki.Key == ConsoleKey::Escape)
						Shutdown();
					else if(ki.Key == ConsoleKey::Tab)
					{
						Logger::Log(1, "***** Test *****",true);
						Test();
						Logger::Log(1, "***** Test *****",true);
					}
				}
				if(listener.Pending())
				{
					PulsarThreadPool::Run(gcnew Action<Object^>(this, &PulsarCore::ClientConnnecting),	listener.AcceptTcpClient());
					continue;
				}
				if(_schedulerServant != nullptr && _schedulerServant->Status == ServantStatus::Ready)
					static_cast<Pulsar::Scheduler^>(_schedulerServant->ServedObject)->Pulse(nullptr);
				System::Threading::Thread::Sleep(100);
			}
			catch(Exception^ exc)
			{
				Logger::LogError(exc);
			}
		}
		listener.Stop();
		Logger::Log(1, "------------------------------------------------------");
		int workT = 0, workIO = 0;
		PulsarThreadPool::GetWorkedThreads(workT, workIO);
		Logger::Log(1, " Ожидание завершения запросов ({0})... ", true, workT + workIO);
		while(workT > 0 || workT > 0)
		{
			System::Threading::Thread::Sleep(100); 
			PulsarThreadPool::GetWorkedThreads(workT, workIO);
		}
		Logger::Log(2, " Сохранение планировщика ... ", false);
		_schedulerServant->Save();
		Logger::Log(2, "OK");
		Logger::Log(0, "Пульсар остановлен ...");
	}
	catch(Exception^ Err)
	{
		Logger::LogError(Err);
	}
}

void PulsarCore::UpdateClientVersion()
{
	{
		lock xxx(ServerParams::Default);
		ServerParams::Default->ClientVersion = 
			(UInt32)safe_cast<Int32>(ServerParams::GetParam("","ClientVersion", ServerParams::Default->ClientVersion));
	}
}

void PulsarCore::ClientConnnecting(Object^ arg)
{
	System::Threading::Thread::CurrentThread->Name = "ClientThread";
	TcpClient^ client = static_cast<TcpClient^>(arg);
	client->ReceiveTimeout = ServerParams::Default->ReceiveTimeOut;
	client->SendTimeout = ServerParams::Default->SendTimeOut;
	client->SendBufferSize = ServerParams::Default->SendBufferSize;
	client->ReceiveBufferSize = ServerParams::Default->ReceiveBufferSize;

	PulsarQuery^ query = nullptr;
	try
	{
		Logger::Log(2, "Client [{0}] connected", true	, client->Client->RemoteEndPoint->ToString());

		ThreadContext::NetStream = client->GetStream();
		ThreadContext::NetStream->ReadTimeout = ServerParams::Default->ReceiveTimeOut;
		ThreadContext::NetStream->WriteTimeout = ServerParams::Default->SendTimeOut;

		// Получение типа запроса
		QueryType queryType = (QueryType)ThreadContext::NetStream->ReadByte();
		if (queryType == QueryType::Update)
			ProcessUpdateQuery(client->Client->RemoteEndPoint->ToString());
		else	if (queryType == QueryType::Object)
		{
			query = (PulsarQuery^)PulsarSerializer::Deserialize(ThreadContext::NetStream);
			ThreadContext::Init(query);
			Logger::Log(2, "Client [{0}] send: {1}",true, client->Client->RemoteEndPoint->ToString(), query);

			PulsarAnswer answer;
			ProcessObjectQuery(query, %answer);

			SendToClient(%answer, true);
		}
	}
	catch (Exception^ e)
	{
		if (dynamic_cast<System::Reflection::TargetInvocationException^>(e) != nullptr)
			e = e->InnerException;
		String^ s = String::Format("[{0}]: {1} ({2})", client->Client->RemoteEndPoint->ToString(),
			e->Message, e->StackTrace);
		Exception exc(s, e);
		Logger::LogError(%exc);			
		PulsarAnswer ans(PulsarAnswerStatus::Error, e->Message);
		try
		{
		 SendToClient(%ans,false);
		}
		catch(...)	{ 	}
	}
	finally
	{
		String^ s = client->Client->RemoteEndPoint->ToString();
		//ThreadContext::Close();
		Logger::Log(2, "Client [{0}] disconnected",true, s);
		if(client != nullptr)
		{
			client->Close();
			delete client;
		}

	}
}

void PulsarCore::ProcessUpdateQuery(String^ address)
{
	try
	{
		Logger::Log(3, "Client [{0}] check update", true, address);
		NetworkStream^ netStream	= ThreadContext::NetStream;
		UInt32 version = StreamExtensions::ReadUInt32(netStream);
		UInt32 clientVersion; 
		{
			lock xxx(ServerParams::Default);
			clientVersion = ServerParams::Default->ClientVersion;
		}

		if(clientVersion > 0 && ServerParams::Default->ClientUpdatePath != nullptr &&
					version < clientVersion)
		{
			// посылаем текущую версию
			Logger::Log(3, "Client [{0}] begin update", true, address);
			StreamExtensions::WriteUInt32(netStream, clientVersion);
			array<Byte>^ bytes;
			int readBytes = 0;
			array<Byte>^ buf = gcnew array<Byte>(ServerParams::Default->SendBufferSize);
			for each(String^ f in Directory::GetFiles(ServerParams::Default->ClientUpdatePath, "*", SearchOption::AllDirectories))
			{
				String^ ff = f->Replace(ServerParams::Default->ClientUpdatePath, "\\");
				ff = Path::GetDirectoryName(ff);
				//директория файла
				bytes = System::Text::UTF8Encoding::UTF8->GetBytes(ff == nullptr ? "" : ff);
				StreamExtensions::WriteBytes(netStream, BitConverter::GetBytes(bytes->Length));
				if(bytes->Length > 0)
					StreamExtensions::WriteBytes(netStream, bytes);
				// имя файла
				ff = Path::GetFileName(f);
				bytes = System::Text::UTF8Encoding::UTF8->GetBytes(ff);
				StreamExtensions::WriteBytes(netStream, BitConverter::GetBytes(bytes->Length));
				StreamExtensions::WriteBytes(netStream, bytes);
				// файл
				FileStream^ file = File::OpenRead(f);
				try
				{
					StreamExtensions::WriteBytes(netStream,BitConverter::GetBytes(file->Length));
					while ((readBytes = file->Read(buf, 0, buf->Length)) != 0)
						netStream->Write(buf, 0, readBytes);
				}
				finally
				{
					file->Close();
					file = nullptr;
				}
			}
			StreamExtensions::WriteBytes(netStream, BitConverter::GetBytes(-1));
		}
		else
			StreamExtensions::WriteUInt32(netStream, 0);


		//Logger::Log(2, String::Format("Client [{0}] send version", true, 333));
		//			Update update = ((Update)objects["Update"].ServedObject);
		//
		//			try
		//			{
		//				update.BeginGetUpdate();
		//
		//				// TODO: со временем отключить мухлёж с угадыванием старого протокола обновления
		//
		//
		//				// uint version = netStream.ReadUInt32(); 
		//				// if (version != update.InternalVersion)
		//				// { 
		//				//		netStream.WriteUInt32(update.InternalVersion);
		//
		//				if ((version != update.InternalVersion && isOld == false) || needUpdate == 1)
		//				{
		//					// посылаем текущую версию
		//					if(isOld == false)
		//						netStream.WriteUInt32(update.InternalVersion);
		//					Logger.LogDebug(2, String.Format("Client [{0}] get update", client.Client.RemoteEndPoint));
		//					byte[] bytes;
		//
		//					int readBytes = 0;
		//					byte[] buf;
		//					buf = new byte[(int)Params["SendBufferSize"]];
		//
		//					foreach (string f in update.FilePaths)
		//					{
		//						// директория файла
		//						bytes = System.Text.UTF8Encoding.UTF8.GetBytes(Path.GetDirectoryName(f));
		//						netStream.WriteBytes(BitConverter.GetBytes(bytes.Length));
		//						netStream.WriteBytes(bytes);
		//						// имя файла
		//						bytes = System.Text.UTF8Encoding.UTF8.GetBytes(Path.GetFileName(f));
		//						netStream.WriteBytes(BitConverter.GetBytes(bytes.Length));
		//						netStream.WriteBytes(bytes);
		//						// файл
		//						using (FileStream file = update.GetFileStream(f))
		//						{
		//							try
		//							{
		//								netStream.WriteBytes(BitConverter.GetBytes((uint)file.Length));
		//								while ((readBytes = file.Read(buf, 0, buf.Length)) != 0)
		//									netStream.Write(buf, 0, readBytes);
		//							}
		//							finally
		//							{
		//								if (file != null)
		//									file.Close();
		//							}
		//						}
		//					}
		//				}
		//				else
		//					netStream.WriteUInt32(0);
		//			}
		//			finally
		//			{
		//				update.EndGetUpdate();
		//			}
	}
	catch (Exception^ e)
	{
		if (dynamic_cast<System::Reflection::TargetInvocationException^>(e) != nullptr)
			e = e->InnerException;
		String^ s = String::Format("[{0}]: {1} ({2})", address,
			e->Message, e->StackTrace);
		Exception exc(s, e);
		Logger::LogError(%exc);
	}
}

void PulsarCore::ProcessObjectQuery(PulsarQuery^ query, PulsarAnswer^ answer)
{
	if (query->RootObject == nullptr)
		throw gcnew PulsarException("В запросе клиента не указан объект!");
	
	// TODO: Заблокировать запросы без пользователя
	//if(QueryInfo.User == null && ps.ServedObject is Users == false && (string)query["GetObject"] != "this")
	// throw new Exception("В запросе не указан пользователь!");

	GlobalObject^ go = dynamic_cast<GlobalObject^>(query->RootObject);
	if(go == nullptr)
	{	
		if(query->Params.HasFlag(PulsarQueryParams::Modify))
			go = GOL::GetForWrite(query->RootObject->ToString());
		else
			go = GOL::GetForRead(query->RootObject->ToString());
	}
	else
	{
		if(query->Params.HasFlag(PulsarQueryParams::Modify))
			static_cast<IReadWriteLockObject^>(go)->BeginWrite();
		else
			static_cast<IReadWriteLockObject^>(go)->BeginRead();
	}

	Servant^ ps = dynamic_cast<Servant^>(go);
	if(ps != nullptr && query->Params.HasFlag(PulsarQueryParams::Modify))
		ps->Version++;

	if (query->Params.HasFlag(PulsarQueryParams::Code))
	{
		if (query->Args["Code"] == nullptr)
			throw gcnew Exception("Для запроса удаленного выполнения кода не указан параметр [Code]!");
		if (dynamic_cast<PulsarCodeTransfer::CodeQuery^>(query->Args["Code"]) == nullptr)
			throw gcnew Exception("Значение параметра [Code] запроса удаленного выполнения кода" +
																									" не является объектом типа PulsarCodeTransfer.CodeQuery!");
		PulsarCodeTransfer::CodeQuery^ q = (PulsarCodeTransfer::CodeQuery^)query->Args["Code"];

		if (ps->ServedObject->GetType()->FullName != q->ObjectType)
			throw gcnew	PulsarException("Pапроса удаленного выполнения кода не предназначен для объекта типа [{0}]!",
																															ps->ServedObject->GetType()->FullName);
		
		answer->Return = PulsarCodeTransfer::ExecCodeQuery(ps->ServedObject, q);
		return;
	}

	BindingFlags bf = BindingFlags::FlattenHierarchy | BindingFlags::Instance |
				BindingFlags::Public | BindingFlags::Static;

	if (query->Params.HasFlag(PulsarQueryParams::NonPublic))
				bf = bf | BindingFlags::NonPublic;

	if (ps == nullptr || query->Params.HasFlag(PulsarQueryParams::Servant))
	{
		if(query->Query == nullptr)
			answer->Return = go;
		else
			answer->Return = Servant::TypesMembersChainingCall(go, query->Query, query->Args, bf,
																																																							query->Params.HasFlag(PulsarQueryParams::Modify));
	}
	else
		answer->Return = ps->Exec(query);

	if (query->Params.HasFlag(PulsarQueryParams::FillVersion))
		answer->ServerObjectVersion = ps->Version;

	if (query->Params.HasFlag(PulsarQueryParams::NoStubEssences))
	{
		for each(auto i in ps->ServedObject->GetType()->GetCustomAttributes(PulsarEssencesHolderAttribute::typeid, true))
		{
			PulsarEssencesHolderAttribute^ t = static_cast<PulsarEssencesHolderAttribute^>(i);
			if(ThreadContext::NoStubTypes->Contains(t->HoldedType) == false)
				ThreadContext::NoStubTypes->Add(t->HoldedType);
		}
	}
}

void PulsarCore::SendToClient(PulsarAnswer^ answer, bool throwError)
{
	System::Diagnostics::Stopwatch^ sw = nullptr;
	if(Logger::LogLevel >= 3)
		sw = System::Diagnostics::Stopwatch::StartNew();

	PulsarSerializationParams pars;
	if(ThreadContext::Query != nullptr)
	{
		if(ThreadContext::Query->Params.HasFlag(PulsarQueryParams::IgnoreAllByDemandSerialization))
			pars.Options = pars.Options | PulsarSerializationOptions::IgnoreAllByDemandSerialization;
		pars.ByDemandTypes = ThreadContext::Query->ByDemand;
	}
	pars.NoStubTypes = ThreadContext::NoStubTypes;
	pars.AsEmptyTypes = ThreadContext::AsEmptyTypes;
	pars.NoStubObjects = ThreadContext::NoStubObjects;
	pars.AsEmptyObjects = ThreadContext::AsEmptyObjects;
	pars.Mode = PulsarSerializationMode::ForClient;
	
	BufferedNetworkStream outStream(ThreadContext::NetStream, ServerParams::Default->SendBufferSize);
	PulsarSerializer::Serialize(%outStream, answer, %pars);
	outStream.Flush();
	if (sw != nullptr)
	{
		sw->Stop();
		Logger::Log(Logger::LogLevel, "AnswerSer: {0}", true, sw->ElapsedMilliseconds);
		delete sw;
	}
}
//-------------------------------------------------------------------------
List<ValuesTrio<String^, Type^, String^>^>^ PulsarCore::GetObjectsList()
{
	List<ValuesTrio<String^, Type^, String^>^>^ res = 
		gcnew List<ValuesTrio<String^, Type^, String^>^>(_autoStart->Count);
	for each(KeyValuePair<String^, Pulsar::OID^> i in _autoStart)
	{
		GlobalObject^ go = GOL::GetForRead(i.Value);
		if(go == nullptr)
			continue;
		IServant^ serv = dynamic_cast<IServant^>(go);
		if(go == this)
			res->Add(gcnew ValuesTrio<String^, Type^, String^>(i.Key, nullptr, Enum::GetName(ServantStatus::typeid,serv->Status)));
		else	if(serv != nullptr)
			res->Add(gcnew ValuesTrio<String^, Type^, String^>(i.Key, serv->ServedObject->GetType(),	Enum::GetName(ServantStatus::typeid,serv->Status)));
		else	
			res->Add(gcnew ValuesTrio<String^, Type^, String^>(i.Key,go->GetType(), Enum::GetName(ServantStatus::typeid,ServantStatus::Ready)));
	}
	return res;
}

System::Collections::Generic::IEnumerable<GlobalObject^>^ 
	PulsarCore::ReadGOs(System::Collections::Generic::IEnumerable<GlobalObject^>^ list, bool noStub)
{
	for each(GlobalObject^ go in list)
	{
		static_cast<IReadWriteLockObject^>(go)->BeginRead();
		if(noStub)
			ThreadContext::NoStubObjects->Add(go);
	}
	return list;
}

//-------------------------------------------------------------------------

void PulsarCore::Test()
{
	//for each(OID^ g in _autoStart->Values)
	// static_cast<Servant^>(GOL::Get(g))->ServedObject->ToString();

	//auto ser = dynamic_cast<IServant^>(GOL::Get("Security"))->ServedObject;
	//Pulsar::Security::PulsarSecurity^ sec = dynamic_cast<Pulsar::Security::PulsarSecurity^>(ser); 
	//for each(Pulsar::Security::SecurityGroup^ g in sec->SecurityGroups->Values)
	// Console::WriteLine("{0}", g->SID);


	//---
	System::Diagnostics::Stopwatch^ watch = nullptr;
	if(Logger::LogLevel >= 3)
		watch = System::Diagnostics::Stopwatch::StartNew();
	//---

		for each(Pulsar::OID^ key in GOL::Keys)
		{
			GlobalObject^ o = GOL::GetForRead(key);
			if(o != nullptr)
				GOL::SaveGlobalObject(o);
		} 
	//---
	if(watch != nullptr)
	{
		watch->Stop();
		Logger::Log(3, "{0}", true, watch->Elapsed);
		delete watch;
	}
	//---
	ThreadContext::CloseThread();
}



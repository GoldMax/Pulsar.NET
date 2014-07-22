#pragma once
#include "Stdafx.h"

using namespace System::IO;

using namespace Pulsar;
using namespace Pulsar::Serialization;

namespace Pulsar
{
	namespace Server
	{
		/// <summary>
		/// Класс серверного загрузчика глобальных объектов.
		/// </summary>
		public ref class GlobalObjectsManager
		{
			private:
			static HashSet<OID^>^ essLoading = gcnew HashSet<OID^>();

			public:
			//-------------------------------------------------------------------------
			// Base Static Methods
			/// <summary>
			/// Загружает объект из хранилища.
			/// </summary>
			static bool Load(String^ fName, Object^% obj, bool throwInNotExists)
			{
				if(fName == nullptr)
					throw gcnew ArgumentNullException("fName");

				if(ServerParams::Default->IsSqlStorage)
				{

				}
				else
				{
					fName = String::Format("{0}\\{1}", ServerParams::Default->StorageConnectionString, fName);
					try
					{
						FileStream fs(fName, FileMode::Open, FileAccess::Read, FileShare::Read);
						if(obj == nullptr)
						 obj = PulsarSerializer::Deserialize(%fs, null,  true);
						else
						 PulsarSerializer::Deserialize(%fs, obj, true);
					}
					catch (FileNotFoundException^ e)
					{
						if(throwInNotExists)
							throw gcnew PulsarException("Не удалось загрузить глобальный объект [{0}] - {1}!", fName, e->Message);
						return false;
					}
					catch (System::IO::DirectoryNotFoundException^ e)
					{
						if(throwInNotExists)
							throw gcnew PulsarException("Не удалось загрузить глобальный объект [{0}] - {1}!", fName, e->Message);
						return false;
					}
				}
				return true;
			} 
			/// <summary>
			/// Сохраняет объект в хранилище.
			/// </summary>
			static void Save(String^ fName, Object^ obj)
			{
				if(fName == nullptr)
					throw gcnew ArgumentNullException("fName");
				if(obj == nullptr)
					throw gcnew ArgumentNullException("obj");

				PulsarSerializationParams		pars;
				pars.Mode = PulsarSerializationMode::OnSave;
				array<Object^>^ arr = gcnew array<Object^> { obj	};
				pars.NoStubObjects = arr;
				
				if(ServerParams::Default->IsSqlStorage)
				{
					//--- Debbuger Break --- //
					if(System::Diagnostics::Debugger::IsAttached)
						System::Diagnostics::Debugger::Break();
					//--- Debbuger Break --- //
				}
				else
				{
					fName = String::Format("{0}\\{1}", ServerParams::Default->StorageConnectionString, fName);
				 String^ dir = Path::GetDirectoryName(fName);
				 if(Directory::Exists(dir) == false)
				 	Directory::CreateDirectory(dir);

					String^ tempfname = fName + "#";
					FileStream fs(tempfname, FileMode::Create);
					PulsarSerializer::Serialize(%fs, obj, %pars);
					fs.Close();
					File::Copy(tempfname, fName,true);
					File::Delete(tempfname);		
				}
				Logger::Log(3,"   Save\t[{0}]",true, obj);
			}



			/// <summary>
			/// Загружает глобальный объект из хранилища.
			/// </summary>
			static bool LoadGO(GlobalObject^ obj, bool throwInNotExists)
			{
				if(obj == nullptr)
				 throw gcnew ArgumentNullException("obj");
				if(obj->OID == nullptr )
					throw gcnew Exception("Пустой OID!");

				IServant^ serv = dynamic_cast<IServant^>(obj);
				IGlobalObjectMeta^ meta = static_cast<IGlobalObjectMeta^>(obj); 
				auto_handle<Type> type = serv == nullptr ? obj->GetType() : serv->ServedObject->GetType();
				auto_handle<array<Object^>> attrs = obj->GetType()->GetCustomAttributes(StoreFolderAttribute::typeid, true);

				if(ServerParams::Default->IsSqlStorage)
				{
				 throw gcnew NotImplementedException("SQL сохранение не реализовано!");
				}
				else
				{
					String^ fname = nullptr;
					if(attrs->Length > 0)
						fname = static_cast<StoreFolderAttribute^>(attrs.get()[0])->Folder;
					if(fname == nullptr)
						fname = type->FullName;

					fname += "\\";
					if(serv != nullptr)
						fname += meta->GlobalName == nullptr ? obj->OID->ToString() : meta->GlobalName;
					else
						fname += obj->OID->ToString();
				 return Load(fname, serv == nullptr ? obj : serv->ServedObject, throwInNotExists); 
				}
			}
			/// <summary>
			/// Загружает глобальные объекты из хранилища.
			/// </summary>
			static void LoadGOs(IEnumerable<GlobalObject^>^ need)
			{
				for each(GlobalObject^ i	in need)
				{
					{
						lock xxx(essLoading);
						if(essLoading->Contains(i->OID) || i->IsInitialized)
							continue;
						essLoading->Add(i->OID);
					}
					LoadGO(i, true);
					{
						lock xxx(essLoading);
						essLoading->Remove(i->OID);
					} 
				}
			}
			/// <summary>
			/// Сохраняет глобальный объект в хранилище.
			/// </summary>
			static void SaveGO(GlobalObject^ go)
			{
				if(go == nullptr)
					throw gcnew ArgumentNullException("go");
				if(go->OID == null)
					throw gcnew Exception("Попытка сохранить глобальный объект с пустым OID!");

				Object^ obj = go;

				IServant^ serv = dynamic_cast<IServant^>(obj);
				IGlobalObjectMeta^ meta = static_cast<IGlobalObjectMeta^>(obj); 
				IReadWriteLockObject^ save = static_cast<IReadWriteLockObject^>(obj);
				bool wasLock = save->IsLocked;

				try
				{
					if(wasLock == false)
						save->BeginRead();
					if(wasLock == false)
						save->BeginRead();

					if(serv != nullptr)
					 obj = serv->ServedObject;

					auto_handle<Type> type = obj->GetType();
					auto_handle<array<Object^>> attrs = obj->GetType()->GetCustomAttributes(StoreFolderAttribute::typeid, true);
					
					if(ServerParams::Default->IsSqlStorage)
					{
						//--- Debbuger Break --- //
						if(System::Diagnostics::Debugger::IsAttached)
							System::Diagnostics::Debugger::Break();
						//--- Debbuger Break --- //
					}
					else
					{
						String^ fname = nullptr;
						if(attrs->Length > 0)
							fname = static_cast<StoreFolderAttribute^>(attrs.get()[0])->Folder;
						if(fname == nullptr && serv == nullptr)
							fname = type->FullName;
						
						fname += "\\";
						if(serv == nullptr)
						 fname += go->OID->ToString();
						else
					 	fname += meta->GlobalName == nullptr ? dynamic_cast<IGlobalObjectMeta^>(serv)->OID->ToString() : meta->GlobalName;

						Save(fname, obj);
					}
				}
				finally
				{
					if(wasLock == false)
						save->EndRead();
				}
			}
			//-------------------------------------------------------------------------
			/// <summary>
			/// Метод установки флага чтения обслуживаемого объекта.
			/// </summary>
			static void BeginRead(IReadWriteLockObject^ obj, Locker^% locker)	
			{ 
				if(GOL::IsInitMode)
					return;
				GlobalObject^ go = dynamic_cast<GlobalObject^>(obj);
				{
					lock xxx(go == nullptr ? obj : go->SyncRoot);
					if(locker == nullptr)
						locker = gcnew Locker();
				}
				Logger::Log(5,"  BeginReadLock [{0}]",true,obj);
				locker->EnterReadLock(); 
				if(ThreadContext::NeedUnlock->Contains(obj) == false)
					ThreadContext::NeedUnlock->Add(obj);
			}
			/// <summary>
			/// Метод снятия флага чтения обслуживаемого объекта.
			/// </summary>
			static void EndRead(IReadWriteLockObject^ obj, Locker^% locker) 
			{
				GlobalObject^ go = dynamic_cast<GlobalObject^>(obj);
				lock xxx(go == nullptr ? obj : go->SyncRoot);
				if(locker == nullptr)
					return;
				if(locker->IsReadLockHeld)
				{ 
					Logger::Log(5,"  EndReadLock [{0}]",true,obj);
					locker->ExitReadLock(); 
				}
				if(locker->HasLock == false)
				{
					delete locker;
					locker = nullptr;
				}
			}
			/// <summary>
			/// Метод установки флага изменения обслуживаемого объекта.
			/// </summary>
			static void BeginWrite(IReadWriteLockObject^ obj, Locker^% locker) 
			{
				if(GOL::IsInitMode)
					return;
				GlobalObject^ go = dynamic_cast<GlobalObject^>(obj);
				{
					lock xxx(go == nullptr ? obj : go->SyncRoot);
					if(locker == nullptr)
						locker = gcnew Locker();
				}
				Logger::Log(5,"  BeginWriteLock [{0}]",true,obj);
				locker->EnterWriteLock(); 
				if(ThreadContext::NeedUnlock->Contains(obj) == false)
					ThreadContext::NeedUnlock->Add(obj);
				if(go != nullptr && ThreadContext::NeedSave->Contains(go) == false)
					ThreadContext::NeedSave->Add(go);
			}
			/// <summary>
			/// Метод снятия флага изменения обслуживаемого объекта.
			/// </summary>
			static void EndWrite(IReadWriteLockObject^ obj, Locker^% locker) 
			{
				GlobalObject^ go = dynamic_cast<GlobalObject^>(obj);
				lock xxx(go == nullptr ? obj : go->SyncRoot);
				if(locker == nullptr)
					return;
				if(locker->IsWriteLockHeld)
				{
					Logger::Log(5,"  EndWriteLock [{0}]",true,obj);
					locker->ExitWriteLock(); 
				}
				if(locker->HasLock == false)
				{
					delete locker;
					locker = nullptr;
				}
			}
			/// <summary>
			/// Метод снятия всех блокировок объекта, установленных вызывающим метод потоком.
			/// </summary>
			static void ClearAllLocks(IReadWriteLockObject^ obj, Locker^% locker)
			{
				GlobalObject^ go = dynamic_cast<GlobalObject^>(obj);
				lock xxx(go == nullptr ? obj : go->SyncRoot);
				if(locker == nullptr)
					return;
				if(locker->IsReadLockHeld || locker->IsWriteLockHeld)
					Logger::Log(5,"  ClearAllLocks [{0}]",true,obj);
				while(locker->IsWriteLockHeld)
					locker->ExitWriteLock();
				while(locker->IsReadLockHeld)
					locker->ExitReadLock();
				if(locker->HasLock == false)
				{
					delete locker;
					locker = nullptr;
				}
			}
		};
	}
}


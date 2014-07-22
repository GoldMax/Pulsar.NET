#pragma once
#include "Stdafx.h"

using namespace msclr;
using namespace System::Reflection;
//using namespace System::Threading;
using namespace System::Data::SqlClient;
using namespace Pulsar;
using namespace Pulsar::Server;

namespace Pulsar
{
	namespace Server
	{
		/// <summary>
		/// Базовый класс слуг корневых объектов сервера Пульсара.
		/// </summary>
		[StoreFolderAttribute("")]
		public ref class Servant : GlobalObject, IServant
		{
			private:
				static SqlConnection^ _con;

				[NonSerialized]
				Object^ _obj;
				[NonSerialized]
				ServantStatus _status;
				[NonSerialized]
				UInt64 _ver;

			//-------------------------------------------------------------------------------------
			#pragma region << Constructors & Destructors >>
			public:
			/// <summary>
			/// Конструктор по умолчанию
			/// </summary>
			Servant()
			{
				_obj = nullptr;
				_status = ServantStatus::NotInit;
				_ver = DateTime::Now.Ticks;
			}
			#pragma endregion
			//-------------------------------------------------------------------------
			//--- Properties
			public:
			/// <summary>
			/// Обслуживаемый сервантом корневой объект.
			/// </summary>
		 [NoLock]
			virtual property Object^ ServedObject
			{
				Object^ get() { return _obj; }
				protected: void set(Object^ value) { _obj = value; }
			}
			/// <summary>
			/// Определяет, поддерживается ли для обслуживаемого объекта получение полной копии.
			/// </summary>
			[NoLock]
			virtual property bool CanGetFullObject 
			{ 
			 bool get() 
				{ 
				 if(_obj == nullptr)
				  return true;
					auto_handle<Type> t = _obj->GetType();
					auto_handle<array<Object^>> att = t->GetCustomAttributes(ServantCanGetFullObjectAttribute::typeid, true);
					if(att->Length == 0)
					 return true;
					return static_cast<ServantCanGetFullObjectAttribute^>(att.get()[0])->Can;
				} 
			}
			/// <summary>
			/// Текущий статус слуги.
			/// </summary>
			virtual property ServantStatus Status
			{
				ServantStatus get() { lock xxx(SyncRoot); return _status; }
				void set(ServantStatus value) { lock xxx(SyncRoot); _status = value; }
			}
			/// <summary>
			/// Версия обслуживаемого объекта. Должда увеличиваться при каждой модификации объекта.
			/// </summary>
			property UInt64 Version
			{
				UInt64 get() 
				{
					lock xxx(SyncRoot);
					if(_ver == 0)
							_ver = (UInt64)DateTime::Now.Ticks;
					return _ver;
				}
				protected public: void set(UInt64 value) { lock xxx(SyncRoot); _ver = value; }
			}

			protected:
			/// <summary>
			/// Объект главного соединения с SQL хранилищем.
			/// </summary>
			static property SqlConnection^ StorageSqlConnection
			{
				SqlConnection^ get() 
				{
					if(ServerParams::Default->IsSqlStorage == false)
						return nullptr;
					if(_con == nullptr)
						_con = gcnew SqlConnection(ServerParams::Default->StorageConnectionString);
					if(_con->State != System::Data::ConnectionState::Open)
						_con->Open();
					return _con;
				}
			}
			//-------------------------------------------------------------------------------------
			public:
			/// <summary>
			/// Метод сохранения объекта.
			/// </summary>
			virtual void Save();
			virtual String^ ToString() override
			{
			 IGlobalObjectMeta^ i = static_cast<IGlobalObjectMeta^>(this);
				return String::Format("{0} - {1}", i->GlobalName == nullptr ? i->OID->ToString() : i->GlobalName, this->GetType()->FullName);
			}

			public protected:
			/// <summary>
			/// Метод инициализации	объекта.
			/// </summary>
			virtual void Init(String^ objType);
			/// <summary>
			/// Метод сохранения объекта.
			/// </summary>
			virtual Object^ Exec(PulsarQuery^ query);
			/// <summary>
			/// Метод, выполняемый после инициализации всех объектов Пульсара, но до запуска сервера Пульсара.
			/// </summary>
			virtual void BeforeServerRun() {}
			private:
			void OnServedObjectChanging(Object^ sender, ObjectChangeNotifyEventArgs^ args)
			{
			 BeginWrite();
			}
			void OnServedObjectChanged(Object^ sender, ObjectChangeNotifyEventArgs^ args)
			{
			 EndWrite();
			}
			//-------------------------------------------------------------------------------------
			//--- Static Methods
			public:
			/// <summary>
			/// Метод последовательного вызова членов классов.
			/// </summary>
			/// <param name="obj">Объект, для которого производится вызов.</param>
			/// <param name="callChain">Цепь вызываемых членов.</param>
			/// <param name="callsArgs">Аргументы вызовов членов</param>
			/// <param name="flag">Флаг поиска членов.</param>
			/// <param name="writeLock">Определяет, будут ли IReadWriteLockObject объекты в цепочке вызова 
			/// заблокированы для записи.</param>
			/// <returns></returns>
			Object^ TypesMembersChainingCall(Object^ obj, String^ callChain, ParamsDic^ callsArgs, BindingFlags flag,
			                                 bool writeLock);
		}; 
	} 
}


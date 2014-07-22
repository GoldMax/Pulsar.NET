#include "stdafx.h"
#include "Servant.h"
#pragma once

using namespace Microsoft::Win32;
using namespace Pulsar;
using namespace Pulsar::Server;

//typedef Guid OID;

namespace Pulsar
{
	namespace Server
	{
		/// <summary>
		/// Основной класс сервера Пульсара.
		/// </summary>
		public ref class PulsarCore : Servant
		{
			private:
				bool _active;
				Dictionary<String^,Pulsar::OID^>^ _autoStart;
			
			 Pulsar::Server::SchedulerServant^ _schedulerServant;
			public:
			/// <summary>
			/// Возвращает параметры сервера
			/// </summary>
			property ServerParams^ Params
			{
				ServerParams^ get() { return ServerParams::Default; }
			}
			//-------------------------------------------------------------------------
			// Constructors & Destructors
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			PulsarCore();
			//-------------------------------------------------------------------------
			// Methods
			public:
			virtual void Init(String^ objType) override;
			virtual void Save() override;

			/// <summary>
			/// Запускает цикл обработки соединений.
			/// </summary>
			void Start();
			/// <summary>
			/// Завершает работу Пульсара.
			/// </summary>
			void Shutdown() { _active = false; }
			///<summary>
			/// Обновляет сведения о номере версии клиентской части из реестра.
			///</summary>
			void UpdateClientVersion();
			//-------------------------------------------------------------------------------------
			// Private Methods
			private:
			/// <summary>
			/// Метод создания и регистрации объектов автозапуска.
			/// </summary>
			///	<param name="oid">OID глобального объекта.</param>
			/// <param name="regName">Имя глобального объекта.</param>
			/// <param name="servType">Имя типа слуги</param>
			/// <param name="objType">Имя типа объекта.</param>
			/// <returns></returns>
			GlobalObject^ CreateAutoStartObject(String^ oid, String^ regName, String^ servType, String^ objType);
			/// <summary>
			/// Метод, вызываемый при принятии соединения пользователя.
			/// </summary>
			void ClientConnnecting(Object^ arg);
			/// <summary>
			/// Отсылает данные клиенту.
			/// </summary>
			/// <param name="answer">Отсылаемые данные.</param>
			/// <param name="throwError">Определяет, надо ли генерировать исключения об ошибках.</param>
			void SendToClient(PulsarAnswer^ answer, bool throwError);
			/// <summary>
			/// Обрабатывает запрос к корневому объекту.
			/// </summary>
			void ProcessObjectQuery(PulsarQuery^ query, PulsarAnswer^ answer);
			/// <summary>
			/// Обрабатывает запрос на обновление.
			/// </summary>
			void ProcessUpdateQuery(String^ address);
			//-------------------------------------------------------------------------
			// Servant && GlobalObject Overrides
			public:
			virtual void BeginRead() override {	}
			virtual void EndRead() override {	}
			virtual void BeginWrite() override { ThreadContext::NeedSave->Add(this); 	}
			virtual void EndWrite() override {	}
			property bool IsWriteLocked	{ virtual bool get() override { return false;	} }
			property bool IsReadLocked { virtual bool get() override { return false;	} }
			property bool IsLocked	{ virtual bool get() override { return false;	} }
			virtual void ClearAllLocks() override {	}

			public:
			// Special Public Methods
			/// <summary>
			/// Взвращает список с информацией о зарегистрированных слугах.
			/// </summary>
			/// <returns></returns>
			List<ValuesTrio<String^, Type^, String^>^>^ GetObjectsList();
			/// <summary>
			/// Блокирует на чтение указанные глобальные объекты. 
			/// Блокируемые объекты добавляются к списку нестабируемых объектов.
			/// </summary>
			/// <param name="list">Перечисление блокируемых глобальных объектов.</param> 
			System::Collections::Generic::IEnumerable<GlobalObject^>^
				ReadFullGOs(System::Collections::Generic::IEnumerable<GlobalObject^>^ list)
				{
					return ReadGOs(list,true);
				}
			/// <summary>
			/// Блокирует на чтение указанные глобальные объекты.
			/// </summary>
			/// <param name="list">Перечисление блокируемых глобальных объектов.</param> 
			/// <param name="noStub">Если true, блокируемые объекты добавляются к списку нестабируемых объектов.</param>
			System::Collections::Generic::IEnumerable<GlobalObject^>^
				ReadGOs(System::Collections::Generic::IEnumerable<GlobalObject^>^ list, bool noStub);
			//-------------------------------------------------------------------------
			private:

			void Test();
		};
	}
}
#pragma once
#include "Stdafx.h"



namespace Pulsar
{
	namespace Server
	{
		/// <summary>
		/// Класс глобальных параметров сервера Пульсара
		/// </summary>
		public ref class ServerParams	sealed : Pulsar::Server::ServerParamsBase
		{
		 private:
				bool _isSqlStorage;
				String^ _conStr;
				String^ _clientUpdatePath;
				UInt32 _clientVer;

		 public:
			ServerParams()
			{
			 IsServer = true;
				_isSqlStorage = true;
			 _conStr = ".\\Data";
				_clientUpdatePath = ".\\Sim";
				_clientVer = 0;
			}
			/// <summary>
			/// Параметры сервера по умолчанию.
			/// </summary>
			static property ServerParams^ Default;
			/// <summary>
			/// Определяет, используется ли SQL сервер для хранения объектов сервера.
			/// </summary>
			property bool IsSqlStorage
			{
				bool get() { return _isSqlStorage; }
				internal: void set(bool value) { _isSqlStorage = value; }
			}
			/// <summary>
			/// Строка доступа к основному хранилищу 
			/// (строка соединения для SQL или каталог для файлового хранилища).
			/// </summary>
			property String^ StorageConnectionString
			{
				String^ get() { return _conStr; }
				internal: void set(String^ value) { _conStr = value; }
			}
			/// <summary>
			/// Путь с папкой клиента.
			/// </summary>
			property String^ ClientUpdatePath
			{
				String^ get() { return _clientUpdatePath; }
				void set(String^ value) { _clientUpdatePath = value; }
			}
			/// <summary>
			/// Версия клиента.
			/// </summary>
			property UInt32 ClientVersion
			{
				UInt32 get() { return _clientVer; }
				void set(UInt32 value) { _clientVer = value; }
			}
		}; 
	}

}

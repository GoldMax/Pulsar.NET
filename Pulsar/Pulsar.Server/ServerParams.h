#pragma once
#include "Stdafx.h"



namespace Pulsar
{
	namespace Server
	{
		/// <summary>
		/// ����� ���������� ���������� ������� ��������
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
			/// ��������� ������� �� ���������.
			/// </summary>
			static property ServerParams^ Default;
			/// <summary>
			/// ����������, ������������ �� SQL ������ ��� �������� �������� �������.
			/// </summary>
			property bool IsSqlStorage
			{
				bool get() { return _isSqlStorage; }
				internal: void set(bool value) { _isSqlStorage = value; }
			}
			/// <summary>
			/// ������ ������� � ��������� ��������� 
			/// (������ ���������� ��� SQL ��� ������� ��� ��������� ���������).
			/// </summary>
			property String^ StorageConnectionString
			{
				String^ get() { return _conStr; }
				internal: void set(String^ value) { _conStr = value; }
			}
			/// <summary>
			/// ���� � ������ �������.
			/// </summary>
			property String^ ClientUpdatePath
			{
				String^ get() { return _clientUpdatePath; }
				void set(String^ value) { _clientUpdatePath = value; }
			}
			/// <summary>
			/// ������ �������.
			/// </summary>
			property UInt32 ClientVersion
			{
				UInt32 get() { return _clientVer; }
				void set(UInt32 value) { _clientVer = value; }
			}
		}; 
	}

}

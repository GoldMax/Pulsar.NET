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
		/// �������� ����� ������� ��������.
		/// </summary>
		public ref class PulsarCore : Servant
		{
			private:
				bool _active;
				Dictionary<String^,Pulsar::OID^>^ _autoStart;
			
			 Pulsar::Server::SchedulerServant^ _schedulerServant;
			public:
			/// <summary>
			/// ���������� ��������� �������
			/// </summary>
			property ServerParams^ Params
			{
				ServerParams^ get() { return ServerParams::Default; }
			}
			//-------------------------------------------------------------------------
			// Constructors & Destructors
			/// <summary>
			/// ����������� �� ���������.
			/// </summary>
			PulsarCore();
			//-------------------------------------------------------------------------
			// Methods
			public:
			virtual void Init(String^ objType) override;
			virtual void Save() override;

			/// <summary>
			/// ��������� ���� ��������� ����������.
			/// </summary>
			void Start();
			/// <summary>
			/// ��������� ������ ��������.
			/// </summary>
			void Shutdown() { _active = false; }
			///<summary>
			/// ��������� �������� � ������ ������ ���������� ����� �� �������.
			///</summary>
			void UpdateClientVersion();
			//-------------------------------------------------------------------------------------
			// Private Methods
			private:
			/// <summary>
			/// ����� �������� � ����������� �������� �����������.
			/// </summary>
			///	<param name="oid">OID ����������� �������.</param>
			/// <param name="regName">��� ����������� �������.</param>
			/// <param name="servType">��� ���� �����</param>
			/// <param name="objType">��� ���� �������.</param>
			/// <returns></returns>
			GlobalObject^ CreateAutoStartObject(String^ oid, String^ regName, String^ servType, String^ objType);
			/// <summary>
			/// �����, ���������� ��� �������� ���������� ������������.
			/// </summary>
			void ClientConnnecting(Object^ arg);
			/// <summary>
			/// �������� ������ �������.
			/// </summary>
			/// <param name="answer">���������� ������.</param>
			/// <param name="throwError">����������, ���� �� ������������ ���������� �� �������.</param>
			void SendToClient(PulsarAnswer^ answer, bool throwError);
			/// <summary>
			/// ������������ ������ � ��������� �������.
			/// </summary>
			void ProcessObjectQuery(PulsarQuery^ query, PulsarAnswer^ answer);
			/// <summary>
			/// ������������ ������ �� ����������.
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
			/// ��������� ������ � ����������� � ������������������ ������.
			/// </summary>
			/// <returns></returns>
			List<ValuesTrio<String^, Type^, String^>^>^ GetObjectsList();
			/// <summary>
			/// ��������� �� ������ ��������� ���������� �������. 
			/// ����������� ������� ����������� � ������ ������������� ��������.
			/// </summary>
			/// <param name="list">������������ ����������� ���������� ��������.</param> 
			System::Collections::Generic::IEnumerable<GlobalObject^>^
				ReadFullGOs(System::Collections::Generic::IEnumerable<GlobalObject^>^ list)
				{
					return ReadGOs(list,true);
				}
			/// <summary>
			/// ��������� �� ������ ��������� ���������� �������.
			/// </summary>
			/// <param name="list">������������ ����������� ���������� ��������.</param> 
			/// <param name="noStub">���� true, ����������� ������� ����������� � ������ ������������� ��������.</param>
			System::Collections::Generic::IEnumerable<GlobalObject^>^
				ReadGOs(System::Collections::Generic::IEnumerable<GlobalObject^>^ list, bool noStub);
			//-------------------------------------------------------------------------
			private:

			void Test();
		};
	}
}
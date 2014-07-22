#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace Pulsar
{
	namespace Security
	{
		/// <summary>
		/// ����� ������� ������� ������������. 
		/// </summary>
		public ref class AccessToken	: IEnumerable<OID^>
		{
			private:
			List<OID^>^ sidsList;

			public:
			/// <summary>
			/// ���������� SID ������������.
			/// </summary>
			property OID^ SID
			{
				OID^ get() { return sidsList->Count == 0 ? nullptr : sidsList[0]; }
				internal: void set(OID^ value)
				{
					if(sidsList->Count == 0)
						sidsList->Add(value);
					else
						sidsList[0] = value;
				}
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// ���������������� �����������.
			/// </summary>
			/// <param name="sid">SID ������������.</param>
			public: 
			AccessToken(OID^ sid)	  
			{		
				this->sidsList = gcnew List<OID^>(1);
				sidsList->Add(sid);
			}
			/// <summary>
			/// ����������
			/// </summary>
			~AccessToken()
			{
				if(sidsList != nullptr)
				{
					sidsList->Clear();
					delete sidsList;
				}
			}
			//-------------------------------------------------------------------------------------
			public:
			// << SID's methods >>
			/// <summary>
			/// ��������� ��������� SID � ������� �������.
			/// </summary>
			/// <param name="sid">����������� SID.</param>
			void Add(OID^ sid)
			{
				if(!sidsList->Contains(sid))
					sidsList->Add(sid);
			}
			/// <summary>
			/// ��������� ������������ SID�� � ������� �������. 
			/// </summary>
			/// <param name="sids">����������� ������������.</param>
			void Add(IEnumerable<OID^>^ sids)
			{
				for each(OID^ s in sids)
					Add(s);
			}
			/// <summary>
			/// ���������, ������������ �� SID � ������� �������.
			/// </summary>
			/// <param name="sid"></param>
			/// <returns></returns>
			bool Contains(OID^ sid)
			{
				return sidsList->Contains(sid);
			}
			/// <summary>
			/// GetEnumerator()
			/// </summary>
			/// <returns></returns>
			virtual IEnumerator<OID^>^ GetEnumerator()	= IEnumerable<OID^>::GetEnumerator
			{
				return sidsList->GetEnumerator();
			}
			virtual System::Collections::IEnumerator^ GetEnumeratorI()	=  System::Collections::IEnumerable::GetEnumerator
			{
				return (System::Collections::IEnumerator^)sidsList->GetEnumerator();
			}
		};
	}
}


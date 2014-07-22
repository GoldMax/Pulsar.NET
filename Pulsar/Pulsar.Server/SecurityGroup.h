#pragma once

using namespace System::ComponentModel;

namespace Pulsar
{
	namespace Security
	{
		/// <summary>
		/// ����� ������ ������������.
		/// </summary>
		public ref class SecurityGroup	: ObjectChangeNotify, ICloneable
		{
			private:
			String^ n;
			String^ d;
			OID^ _sid;
			public:
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//~~~ Properties
			/// <summary>
			/// SID ������.
			/// </summary>
			property OID^ SID { OID^ get() { return _sid; } }
			/// <summary>
			/// ��� ������.
			/// </summary>
			[DisplayName("������������")]
			property String^ Name
			{
				String^ get() { return n; }
				void set(String^ value)
				{
					auto arg = OnObjectChanging("Name", value, n);
					n = value;
					OnObjectChanged(arg);
				}
			}
			/// <summary>
			/// �������� ������.
			/// </summary>
			[DisplayName("��������")]
			property String^ Description
			{
				String^ get() { return d; }
				void set(String^ value)
				{
					auto arg = OnObjectChanging("Description", value, d);
					d = value;
					OnObjectChanged(arg);
				}
			}
			//-------------------------------------------------------------------------------------
			//--- Constructors
			/// <summary>
			/// ����������� �� ���������.
			/// </summary>
			SecurityGroup()
			{
				_sid = gcnew OID();
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// ���������������� �����������.
			/// </summary>
			/// <param name="sid">SID ������.</param>
			/// <param name="name">��� ������.</param>
			/// <param name="desc">�������� ������.</param>
			SecurityGroup(OID^ sid, String^ name, String^ desc)	
			{
				_sid = sid;
				Name = name;
				Description = desc;
			}
			
			private:
			virtual Object^ CloneI() sealed = ICloneable::Clone
			{
				return this->Clone();
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// ������� ����� �������.
			/// </summary>
			/// <returns></returns>
			public:
			SecurityGroup^ Clone()
			{
				SecurityGroup^ g = gcnew SecurityGroup();
				g->_sid = this->SID;
				g->Name = this->Name;
				g->Description = this->Description;
				return g;
			}
		};
	}
}


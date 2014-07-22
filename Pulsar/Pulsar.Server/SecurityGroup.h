#pragma once

using namespace System::ComponentModel;

namespace Pulsar
{
	namespace Security
	{
		/// <summary>
		/// Класс группы безопасности.
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
			/// SID группы.
			/// </summary>
			property OID^ SID { OID^ get() { return _sid; } }
			/// <summary>
			/// Имя группы.
			/// </summary>
			[DisplayName("Наименование")]
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
			/// Описание группы.
			/// </summary>
			[DisplayName("Описание")]
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
			/// Конструктор по умолчанию.
			/// </summary>
			SecurityGroup()
			{
				_sid = gcnew OID();
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			/// <param name="sid">SID группы.</param>
			/// <param name="name">Имя группы.</param>
			/// <param name="desc">Описание группы.</param>
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
			/// Создает копию объекта.
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


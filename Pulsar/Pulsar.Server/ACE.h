#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace Pulsar
{
	namespace Security
	{
		/// <summary>
		/// Класс записи контроля доступа (Access Control Entry).
		/// </summary>
		public ref class ACE: ICloneable
		{
			private:
			OID^ sd;
			OID^ sid;
			bool isDenied;
			Byte rights;
			
			public:
			//-------------------------------------------------------------------------
			//--- Constructors 
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			ACE() 
			{ 
				sd = nullptr;
				sid = nullptr;
				rights = 0;
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			/// <param name="sd">Дескриптор безопасности.</param>
			/// <param name="sid">Идентификатор безопасности.</param>
			/// <param name="isDenied">Определяет, является ли ACE запретительным.</param>
			/// <param name="browse">Определяет право на чтение.</param>
			/// <param name="L1">Определяет право уровня 1.</param>
			/// <param name="L2">Определяет право уровня 2.</param>
			/// <param name="L3">Определяет право уровня 3.</param>
			ACE(OID^ sd, OID^ sid, bool isDenied, bool browse, bool L1, bool L2, bool L3)
			{
				SD = sd;
				SID = sid;
				IsDenied = isDenied;
				Browse = browse;
				Level1 = L1;
				Level2 = L2;
				Level3 = L3;
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//~~~  Properties 
			/// <summary>
			/// Дескриптор безопасности.
			/// </summary>
			property OID^ SD
			{
				OID^ get() { return sd; }
				void set(OID^ value) { sd = value; }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Идентификатор безопасности.
			/// </summary>
			property OID^ SID
			{
				OID^ get() { return sid; }
				void set(OID^ value) { sid = value; }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Определяет, является ли ACE запретительным.
			/// </summary>
			property bool IsDenied
			{
				bool get() { return isDenied; }
				void set(bool value) { isDenied = value; }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Определяет право на чтение/просмотр (Уровень 0).
			/// </summary>
			property bool Browse
			{
				bool get() { return (rights & 1) > 0; }
				void set(bool value) { rights = (Byte)(value ? rights | 1 : rights & 254); }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Определяет право на изменение (Уровень 1).
			/// </summary>
			property bool Level1
			{
				bool get() { return (rights & 2) > 0; }
				void set(bool value) { rights = (Byte)(value ? rights | 2 : rights & 253); }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Определяет право на добавление (Уровень 2).
			/// </summary>
			property bool Level2
			{
				bool get() { return (rights & 4) > 0; }
				void set(bool value) { rights = (Byte)(value ? rights | 4 : rights & 251); }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Определяет право на удаление (Уровень 3).
			/// </summary>
			property bool Level3
			{
				bool get() { return (rights & 8) > 0; }
				void set(bool value) { rights = (Byte)(value ? rights | 8 : rights & 247); }
			}
			//-------------------------------------------------------------------------------------
			//--- Methods
			/// <summary>
			/// ToString()
			/// </summary>
			/// <returns></returns>
			virtual String^ ToString() override
			{
				return String::Format("SID:{{{1}}}; SD:{{{0}}}; ±:{2}; B:{3}; L1:{4}; L2:{5}; L3:{6};", sd, sid,
					isDenied ? 1 : 0,
					(rights & 1) > 0 ? 1 : 0,
					(rights & 2) > 0 ? 1 : 0,
					(rights & 4) > 0 ? 1 : 0,
					(rights & 8) > 0 ? 1 : 0);
			}
			virtual Object^ CloneI()	= ICloneable::Clone
			{
				return this->MemberwiseClone();
			}
			/// <summary>
			/// Создает копию объекта.
			/// </summary>
			/// <returns></returns>
			virtual ACE^ Clone()
			{
				return static_cast<ACE^>(this->MemberwiseClone());
			}
		};
	}
}

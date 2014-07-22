using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar.Clients
{
	#region << public enum SubjectType : byte >>
	/// <summary>
	/// Перечисление типов субъектов
	/// </summary>
	public enum SubjectType : byte
	{
		/// <summary>
		/// Неизвестный
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Физическое лицо
		/// </summary>
		Person = 1,
		/// <summary>
		/// Индивидуальный предприниматель
		/// </summary>
		Employer = 2,
		/// <summary>
		/// Юридическое лицо (компания)
		/// </summary>
		Company = 3
	}
	#endregion << public enum SubjectType : byte >>
	//*************************************************************************************
	#region << public enum BusinessRole : byte >>
	/// <summary>
	/// Перечисление ролей бизнес-субъектов.
	/// </summary>
	[Flags]
	public enum BusinessRole : byte
	{
		/// <summary>
		/// Клиент
		/// </summary>
		Client = 0,
		/// <summary>
		/// Подразделение  
		/// </summary>
		Branch = 1,
		/// <summary>
		/// Поставщик
		/// </summary>
		Supplier = 2,
		/// <summary>
		/// Дилер
		/// </summary>
		Dealer = 4
	}
	#endregion << public enum BusinessRole : byte >>
	//*************************************************************************************
	#region << public enum PersonType : byte >>
	/// <summary>
	/// Перечисление типов персон 
	/// </summary>
	public enum PersonType : byte
	{
		/// <summary>
		/// Внешний пользователь
		/// </summary>
		[EnumItemDisplayName("Внешний пользователь")] 
		Alien = 0,
		/// <summary>
		/// Сотрудник
		/// </summary>
		[EnumItemDisplayName("Сотрудник")] 
		Employee = 1,
		/// <summary>
		/// Уволенный
		/// </summary>
		[EnumItemDisplayName("Уволенный")] 
		Redundant = 2,
		/// <summary>
		/// Системная запись
		/// </summary>
		[EnumItemDisplayName("Системная запись")] 
		System = 255
	}
	#endregion << public enum PersonType : byte >>
}

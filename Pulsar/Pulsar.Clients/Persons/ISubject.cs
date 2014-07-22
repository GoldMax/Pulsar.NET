using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pulsar;

namespace Pulsar.Clients
{
 /// <summary>
 /// Интерфейс субъекта 
 /// </summary>
 public interface ISubject : IOIDObject
 {
  /// <summary>
  /// Полное наименование субъекта
  /// </summary>
  string FullName { get; }
  /// <summary>
  /// Тип субъекта
  /// </summary>
  SubjectType SubjectType { get; }
 }


	/// <summary>
	/// Интерфейс физических субъектов (людей).
	/// </summary>
	public interface IPersonSubject : ISubject
	{
		/// <summary>
		/// Фамилия.
		/// </summary>
		string FName { get; set; }
		/// <summary>
		/// Имя.
		/// </summary>
		string IName { get; set; }
		/// <summary>
		/// Отчество.
		/// </summary>
		string OName { get; set; }
		/// <summary>
		/// Тип персоны.
		/// </summary>
		PersonType PersonType { get; set; }
	}

	/// <summary>
	/// Интерфейс бизнес субъектов (юридических лиц и ИП).
	/// </summary>
	public interface IBusinessSubject : ISubject
	{
		/// <summary>
		/// Роли субъекта
		/// </summary>
		BusinessRole Roles { get; set; }
	} 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pulsar;
using Pulsar.Reflection;

namespace Pulsar.Clients
{
	//**************************************************************************************
	#region << public class Person >>
	/// <summary>
	/// Базовый класс данных о физическом лице.
	/// </summary>
	//[PulsarEssence(typeof(Persons))]
	public class Person : GlobalObject, IPersonSubject
	{
		private string fName = null;
		private string iName = null;
		private string oName = null;
		private PersonType pr = PersonType.Employee;
		private Sex _sex = Sex.Male;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Фамилия.
		/// </summary>
		public string FName
		{
			get { return fName; }
			set
			{
				var arg = OnObjectChanging("FName", value, fName);
				fName = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Имя.
		/// </summary>
		public string IName
		{
			get { return iName; }
			set
			{
				var arg = OnObjectChanging("IName", value, iName);
				iName = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Отчество.
		/// </summary>
		public string OName
		{
			get { return oName; }
			set
			{
				var arg = OnObjectChanging("OName", value, oName);
				oName = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Возвращает полное имя физлица.
		/// </summary>
		public string FullName
		{
			get
			{
				return String.Format("{0} {1} {2}",
					String.IsNullOrEmpty(FName) ? "$" : FName,
					String.IsNullOrEmpty(IName) ? "$" : IName,
					String.IsNullOrEmpty(OName) ? "$" : OName);
			}
		}
		/// <summary>
		/// Возвращает краткое имя физлица.
		/// </summary>
		public string ShortName
		{
			get
			{
				return String.Format("{0} {1}. {2}.",
					String.IsNullOrEmpty(FName) ? "$" : FName,
					String.IsNullOrEmpty(IName) ? "$" : IName[0].ToString(),
					String.IsNullOrEmpty(OName) ? "$" : OName[0].ToString());
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Пол
		/// </summary>
		public Sex Sex
		{
			get { return _sex; }
			set
			{
				var arg = OnObjectChanging("Sex", value, _sex);
				_sex = value;
				OnObjectChanged(arg);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		SubjectType ISubject.SubjectType
		{
			get { return SubjectType.Person; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Статус физлица
		/// </summary>
		public PersonType PersonType
		{
			get { return pr; }
			set
			{
				var arg = OnObjectChanging("PersonStatus", value, pr);
				pr = value;
				OnObjectChanged(arg);
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public Person() { }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		[NoLock]
		public override string ToString()
		{
			return String.Format("{0} {1} {2}",
					String.IsNullOrEmpty(fName) ? "$" : fName,
					String.IsNullOrEmpty(iName) ? "$" : iName,
					String.IsNullOrEmpty(oName) ? "$" : oName);
		}
		#endregion << Methods >>
	}
	#endregion << public class Person >>
	}

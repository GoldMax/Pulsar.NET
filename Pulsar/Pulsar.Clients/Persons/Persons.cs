using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using Pulsar;
//using OID=System.Guid;

namespace Pulsar.Clients
{
	// Pulsar.Clients.Persons
	/// <summary>
	/// Класс справочника физических лиц.
	/// </summary>
	[PulsarEssencesHolderAttribute(typeof(Person))]
	public class Persons : IEnumerable<IPersonSubject>
	{
		private IndexedList<Person,OID> _list = new IndexedList<Person, OID>("OID", true);
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет физлицо по его OID
		/// </summary>
		/// <param name="oid"></param>
		/// <returns></returns>
		public Person this[OID oid]
		{
			get { return _list.ByIndex(oid); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает число физлиц.
		/// </summary>
		public int Count { get { return _list.Count; } }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public Persons()
			: base()
		{
			Person root = GlobalObject.CreateWithOID<Person>(new OID("C0FE2828-BEA5-471F-92C1-6A953D56892B"));
			root.FName = "root";
			root.PersonType = PersonType.System;
			_list.Add(root);
			GOL.Add(root);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Добавляет физлицо в справочник.
		/// </summary>
		/// <param name="person">Оьъект пользователя.</param>
		public void Add(Person person)
		{
			_list.Add(person);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет физлицо из справочника.
		/// </summary>
		/// <param name="person">Пользователь.</param>
		public void Remove(Person person)
		{
			_list.Remove(person);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает коллекцию имен физлиц. 
		/// </summary>
		/// <param name="type">Типы пользователей.</param>
		/// <returns></returns>
		public Dictionary<OID, string> GetNamesList(PersonType type)
		{
			Dictionary<OID,string> res = new Dictionary<OID, string>();
			foreach(IPersonSubject u in _list)
				if(u.PersonType == type && u.PersonType != PersonType.System)
					res.Add(u.OID, u.FullName);
			return res;
		}                   
		/// <summary>
		/// Возвращает коллекцию имен физлиц. 
		/// </summary>
		/// <returns></returns>
		public Dictionary<OID, string> GetNamesList()
		{
			Dictionary<OID,string> res = new Dictionary<OID, string>();
			foreach(IPersonSubject u in _list)
				if(u.PersonType != PersonType.Redundant)
					res.Add(u.OID, u.FullName);
			return res;
		}                   
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IEnumerable<Person> Members
		/// <summary>
		/// GetEnumerator
		/// </summary>
		/// <returns></returns>
		public IEnumerator<IPersonSubject> GetEnumerator()
		{
			return _list.GetEnumerator();
		}
		//-------------------------------------------------------------------------------------
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс категории свойства.
	/// </summary>
	public class PulsarPropertyCategory : GlobalObject
	{
		private string name = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Наименование категории.
		/// </summary>
		public virtual string Name
		{
			get { BeginRead(); return name; }
			set
			{
				BeginWrite();
				if(value == null || value.TrimAll().Length == 0)
					throw new Exception("Значение имени категории не может быть пустым!");
				var arg = OnObjectChanging("Name", value, name);
				name = value.TrimAll();
				OnObjectChanged(arg);
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		private PulsarPropertyCategory() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// <param name="name">Наименование категории.</param>
		/// </summary>
		public PulsarPropertyCategory(string name)
		{
			if(name == null || name.TrimAll().Length == 0)
				throw new Exception("Значение имени категории не может быть пустым!");
			this.name = name;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString
		/// </summary>
		/// <returns></returns>
		[NoLock]
		public override string ToString()
		{
			return name ?? " (нет имени)";
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pulsar;

namespace Sim.Refs
{
	/// <summary>
	/// Класс группы значений свойств
	/// </summary>
	public class PropsValuesGroup : GlobalObject, IPulsarPropertiesValuesContainer, ICloneable
	{
		private string name = null;
		private IPulsarPropertiesValuesCollection _dic = new PulsarPropertiesValuesCollection<PulsarProperty>();

		#region << Properties >>
		/// <summary>
		/// Наименование группы	значений
		/// </summary>
		public string Name
		{
			get { BeginRead(); return name; }
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new Exception("Имя группы значений указано неверно!");
				BeginWrite();
				var arg = OnObjectChanging("Name", value, name);
				name = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Значения свойств группы
		/// </summary>
		public IPulsarPropertiesValuesCollection PropertiesValues
		{
			get { BeginRead(); return _dic; }
		}
		/// <summary>
		/// Свойства группы
		/// </summary>
		public IEnumerable<PulsarProperty> Props
		{
			get { BeginRead(); return _dic.Props; }
		}
		/// <summary>
		/// Рассчетные свойства группы
		/// </summary>
		IEnumerable<PulsarProperty> IPulsarPropertiesValuesContainer.CalculatedProperties
		{
			get { BeginRead(); return _dic.Props; }
		}
		#endregion << Properties >>

		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		protected PropsValuesGroup()
		{
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="name">Имя группы	значений</param>
		public PropsValuesGroup(string name)
			: this()
		{
			if(String.IsNullOrWhiteSpace(name))
				throw new Exception("Имя группы	значений указано неверно!");
			this.name = name;
		}
		#endregion << Constructors >>

		#region << Methods >>
		/// <summary>
		/// Устанавливает значения свойств.
		/// </summary>
		/// <param name="col">Коллекция новых значений свойств.</param>
		public void SetData(IPulsarPropertiesValuesCollection col)
		{
			BeginWrite();
			foreach(var i in col)
			{
				if (i.Value == null)
				{
					if (_dic.Contains(i.Key))
						_dic.Remove(i.Key);
				}
				else
					i.Key.SetValue(this, i.Value);
			}
		}
		/// <summary>
		/// ToString
		/// </summary>
		/// <returns></returns>
		[NoLock]
		public override string ToString()
		{
			return name;
		}
		#endregion << Methods >>

		#region ICloneable Members
		/// <summary>
		/// Клонирует объект. OID не клонируется.
		/// </summary>
		/// <returns></returns>
		public PropsValuesGroup Clone()
		{
			BeginRead();
			PropsValuesGroup s = (PropsValuesGroup)Pulsar.Serialization.PulsarSerializer.CloneObject(this);
			return s;
		}
		/// <summary>
		/// Клонирует объект, OID не клонируется
		/// </summary>
		/// <returns></returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion
	}
}

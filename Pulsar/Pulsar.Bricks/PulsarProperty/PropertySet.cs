using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Pulsar;

namespace Pulsar
{

	#region << public interface IPropertySet >>
	/// <summary>
	/// Интерфейс набора свойств
	/// </summary>
	public interface IPropertySet
	{
		/// <summary>
		/// Имя набора свойств
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Список свойств набора
		/// </summary>
		IEnumerable Props { get; }
	}
	#endregion << public interface IPropertySet >>


	#region << public class PropertySet<T> : IPropertySet<T> >>
	/// <summary>
	/// Класс набора свойств
	/// </summary>
	/// <typeparamref name="T">Тип свойств</typeparamref>
	public class PropertySet<T> : GlobalObject, IPropertySet	where T : PulsarProperty
	{
		private HashIndex<T> _props;
		private string _name = null;

		#region << Properties >>
		/// <summary>
		/// Имя набора свойств
		/// </summary>
		public string Name
		{
			get { BeginRead(); return _name;  }
			set
			{
			 BeginWrite();
				var arg = OnObjectChanging("Name", value, _name);
				_name = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Список свойств набора
		/// </summary>
		public IEnumerable<T> Props
		{
			get { BeginRead(); return _props;  }
		}
		IEnumerable IPropertySet.Props
		{
			get { BeginRead(); return _props;  }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Констуктор по умолчанию
		/// </summary>
		private PropertySet()
		{
			_props = new HashIndex<T>();
		}
		/// <summary>
		/// Инициализирующий конструктор
		/// </summary>
		/// <param name="name"></param>
		public PropertySet(string name) : this()
		{
			_name = name;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Public Methods >>
		/// <summary>
		/// Добавляет свойство в набор.
		/// </summary>
		/// <param name="prop">Добавляемое свойство.</param>
		public void Include(T prop)
		{
			BeginWrite();
			if(_props.Contains(prop) == false)
			 _props.Add(prop);
		}
		/// <summary>
		/// Добавляет свойства в набор.
		/// </summary>
		/// <param name="props">Перечисление добавляемых свойств.</param>
		public void Include(IEnumerable<T> props)
		{
			BeginWrite();
			foreach(T p in props)
			{
				if(_props.Contains(p))
					continue;
				_props.Add(p);
			}
		}
		/// <summary>
		/// Исключает свойство из набора.
		/// </summary>
		/// <param name="props">Исключаемое свойство.</param>
		public void Exclude(T prop)
		{
			BeginWrite();
			_props.Remove(prop);
		}
		/// <summary>
		/// Исключает свойства из набора.
		/// </summary>
		/// <param name="props">Исключаемые свойства.</param>
		public void Exclude(IEnumerable<T> props)
		{
			BeginWrite();
			foreach(T p in props)
				_props.Remove(p);
		}
		/// <summary>
		/// Определяет присутствие свойства в наборе.
		/// </summary>
		/// <param name="prop">Определяемое свойство.</param>
		public bool Contains(T prop)
		{
			BeginRead();
			return _props.Contains(prop);
		}
		/// <summary>
		/// Строковое представление набора
		/// </summary>
		/// <returns></returns>
		[NoLock]
		public override string ToString()
		{
			return _name ?? " (нет значения)";
		}
		#endregion << Public Methods >>

	}

	#endregion << public class PropertySet<T> : IProperty<T> >>
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
	#region << public interface IPulsarPropertiesValuesCollection >>
	/// <summary>
	/// Ковариантный интерфейс коллекции значений свойств.
	/// </summary>
	public interface IPulsarPropertiesValuesCollection : IEnumerable, IEnumerable<IKeyedValue<PulsarProperty,object>>
	{
		/// <summary>
		/// Возвращает значение свойства.
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		object this[PulsarProperty prop] { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает перечисление установленных свойств 
		/// </summary>
		IEnumerable<PulsarProperty> Props { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает количество установленных свойств.
		/// </summary>
		int Count { get; }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет значение свойства из коллекции.
		/// </summary>
		/// <param name="prop">Удаляемое свойство.</param>
		/// <returns></returns>
		bool Remove(PulsarProperty prop);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли коллекция значение свойства.
		/// </summary>
		/// <param name="prop">Определяемое свойство.</param>
		/// <returns></returns>
		bool Contains(PulsarProperty prop);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавление значения свойства в коллекцию.
		/// !!! НЕ ИСПОЛЬЗОВАТЬ для установки значения свойства !!!
		/// Для установки значения свойства использовать PulsarProperty.SetValue
		/// </summary>
		/// <param name="prop">Свойство добавляемого значения.</param>
		/// <param name="value">Добавляемое значение.</param>
		void DirectInject(PulsarProperty prop, object value);
	}
	#endregion << public interface IPulsarPropertiesValuesCollection<in Tin, out Tout> >>
	//*************************************************************************************
	/// <summary>
	/// Класс коллекции значений свойств.
	/// </summary>
	[Serializable]
	public class PulsarPropertiesValuesCollection<T> : CollectionChangeNotify,	IPulsarPropertiesValuesCollection
		where T : PulsarProperty
	{
		private PDictionary<T, object> dic = null;
		private bool noRemoveNull = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает значение свойства.
		/// </summary>
		/// <param name="prop">Свойство.</param>
		/// <returns>Значение свойства.</returns>
		public object this[T prop]
		{
			get
			{
				object res;
				if (dic.TryGetValue(prop, out res) == false)
					return null;
				return res;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает перечисление установленных свойств 
		/// </summary>
		public IEnumerable<T> Props
		{
			get
			{
				foreach (var i in dic.Keys)
					yield return (T)i;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает количество установленных свойств.
		/// </summary>
		public int Count
		{
			get { return dic.Count; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarPropertiesValuesCollection()
		{
			dic = new PDictionary<T, object>();
			OnDeserialized(new StreamingContext());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="noRemoveNull">Определяет возможность добавлять как значение свойств null.</param>
		public PulsarPropertiesValuesCollection(bool noRemoveNull)
			: this()
		{
			this.noRemoveNull = noRemoveNull;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="capacity">Начальная емкость коллекции.</param>
		public PulsarPropertiesValuesCollection(int capacity)
		{
			dic = new PDictionary<T, object>(capacity);
			OnDeserialized(new StreamingContext());
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region IEnumerator<KeyedValue<T, object>> Members
		/// <summary>
		/// GetEnumerator()
		/// </summary>
		/// <returns></returns>
		public IEnumerator<IKeyedValue<T, object>> GetEnumerator()
		{
			foreach (var i in dic)
				yield return (IKeyedValue<T, object>)i;
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return dic.GetEnumerator();
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Удаляет значение свойства из коллекции.
		/// </summary>
		/// <param name="prop">Удаляемое свойство.</param>
		/// <returns></returns>
		public bool Remove(T prop)
		{
			if (dic.ContainsKey(prop) == false)
				return false;
			dic.Remove(prop);
			return true;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли коллекция значение свойства.
		/// </summary>
		/// <param name="prop">Определяемое свойство.</param>
		/// <returns></returns>
		public bool Contains(T prop)
		{
			return dic.ContainsKey(prop);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавление значения свойства в коллекцию.
		/// !!! НЕ ИСПОЛЬЗОВАТЬ для установки значения свойства !!!
		/// Для установки значения свойства использовать PulsarProperty.SetValue
		/// </summary>
		/// <param name="prop">Свойство добавляемого значения.</param>
		/// <param name="value">Добавляемое значение.</param>
		public void DirectInject(T prop, object value)
		{
			if (value == null && noRemoveNull == false)
				Remove(prop);
			else
			{
				if (dic.ContainsKey(prop))
					dic[prop] = value;
				else
					dic.Add(prop, value);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnDeserialized
		/// </summary>
		/// <param name="cox"></param>
		[OnDeserialized]
		protected void OnDeserialized(StreamingContext cox)
		{
			dic.ItemAdding += (s, e) => OnItemAdding(e);
			dic.ItemChanging += (s, e) => OnItemChanging(e);
			dic.ItemDeleting += (s, e) => OnItemDeleting(e);
			dic.ItemAdded += (s, e) => OnItemAdded(e);
			dic.ItemChanged += (s, e) => OnItemChanged(e);
			dic.ItemDeleted += (s, e) => OnItemDeleted(e);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IPulsarPropertiesValuesCollection Members
		object IPulsarPropertiesValuesCollection.this[PulsarProperty prop]
		{
			get
			{
				return this[(T)prop];
			}
		}
		int IPulsarPropertiesValuesCollection.Count
		{
			get { return dic.Count; }
		}
		bool IPulsarPropertiesValuesCollection.Remove(PulsarProperty prop)
		{
			if (dic.ContainsKey((T)prop) == false)
				return false;
			dic.Remove((T)prop);
			return true;
		}
		bool IPulsarPropertiesValuesCollection.Contains(PulsarProperty prop)
		{
			return dic.ContainsKey((T)prop);
		}
		void IPulsarPropertiesValuesCollection.DirectInject(PulsarProperty prop, object value)
		{
			this.DirectInject((T)prop, value);
		}
		IEnumerable<PulsarProperty> IPulsarPropertiesValuesCollection.Props
		{
			get
			{
				foreach (var i in dic.Keys)
					yield return i;
			}
		}
		IEnumerator<IKeyedValue<PulsarProperty, object>> IEnumerable<IKeyedValue<PulsarProperty, object>>.GetEnumerator()
		{
			foreach(var i in dic)
				yield return i;
		}
		#endregion

	}
}

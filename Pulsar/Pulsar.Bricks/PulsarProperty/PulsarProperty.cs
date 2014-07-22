using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Sim.Refs;

namespace Pulsar
{
	/// <summary>
	/// Базовый класс свойства Пульсара
	/// </summary>
	public abstract class PulsarProperty : GlobalObject, ICloneable
	{
		private string name = null;
		private string desc = null;
		private PulsarPropertyType type = PulsarPropertyType.Check; 
		internal object contextVal = null;
		internal IList enums = null;
		private PulsarPropertyOptions ops = PulsarPropertyOptions.None;
		private PulsarPropertyCategory cat = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Наименование свойства.
		/// </summary>
		public virtual string Name
		{
			get { BeginRead(); return name ?? " (нет имени) "; }
			set
			{
			 BeginWrite();
				var arg = OnObjectChanging("Name", value, name);
				name = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Описание свойства.
		/// </summary>
		public virtual string Description
		{
			get { BeginRead(); return desc; }
			set
			{
				BeginWrite();
				var arg = OnObjectChanging("Description", value, desc);
				desc = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Тип свойства.
		/// </summary>
		public virtual PulsarPropertyType Type
		{
			get { BeginRead(); return type; }
			set
			{
				BeginWrite();
				var arg = OnObjectChanging("Type", value, type);
				type = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Минимальное и максимальное значение свойства для числовых типов свойств.
		/// </summary>
		public virtual ValuesPair<decimal?, decimal?> MinMaxValues
		{
			get { BeginRead(); return contextVal as ValuesPair<decimal?, decimal?>; }
			set
			{
				BeginWrite();
				if(value != null)
					if(Type != PulsarPropertyType.Integer && Type != PulsarPropertyType.Decimal)
						throw new Exception("Попытка установить минимальное и максимальное значение для нечислового типа свойства!");
				var arg = OnObjectChanging("MinMaxValues", value, contextVal);
				contextVal = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Тип ресурса для типа ресурсных типов свойств.
		/// </summary>
		public PulsarResourceType ResourceType
		{
			get { BeginRead(); return contextVal is PulsarResourceType ? (PulsarResourceType)contextVal : PulsarResourceType.Unknown; }
			set
			{
				BeginWrite();
				if(Type != PulsarPropertyType.Resource)
					throw new Exception("Попытка установить значение типа ресурса для нересурсного типа свойства!");
				var arg = OnObjectChanging("ResourceType", value, contextVal);
				contextVal = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Список значений для перечисляемых типов свойств.
		/// </summary>
		public virtual IList Enums
		{
			get { BeginRead(); return enums; }
			set
			{
				BeginWrite();
				var arg = OnObjectChanging("Enums", value, enums);
				enums = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Определяет, является ли значением свойства список значений.
		/// </summary>
		public bool IsListValue
		{
			get { BeginRead(); return Options.HasFlag(PulsarPropertyOptions.IsListValue); }
			set 
			{
				BeginWrite();
				var arg = OnObjectChanging("IsListValue", value, ops);
				ops = value ? Options | PulsarPropertyOptions.IsListValue : Options & ~PulsarPropertyOptions.IsListValue;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Опции свойства
		/// </summary>
		public virtual PulsarPropertyOptions Options
		{
			get { BeginRead(); return ops; }
			set
			{
				BeginWrite();
				var arg = OnObjectChanging("Options", value, ops);
				ops = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Категория свойства.
		/// </summary>
		public virtual PulsarPropertyCategory Category
		{
			get { BeginRead(); return cat; }
			set 
			{
				BeginWrite();
				var arg = OnObjectChanging("Category", value, cat);
				cat = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Возвращает значение свойства для типа Check
		/// </summary>
		public string CheckValue
		{
			get { BeginRead(); return contextVal == null || contextVal is string == false ? "Да" : (string)contextVal; }
			set 
			{
				BeginWrite();
				if(value != null && value.TrimAll() == "Да")
				 value = null; 
			 var arg = OnObjectChanging("CheckValue",value, contextVal);
				contextVal = value;
				OnObjectChanged(arg);
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		[NoLock]
		public override string ToString()
		{
			return name ?? " (нет имени) ";
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вовзращает значение свойства для указанного объекта, содержащего коллекцию значений свойств.
		/// </summary>
		/// <param name="obj">Объект, содержащий коллекцию значений свойств.</param>
		/// <param name="selfOnly">Определяет, возвращать ли значения наследуемых всойств</param>
		/// <returns></returns>
		public virtual object GetValue(IPulsarPropertiesValuesContainer obj, bool selfOnly = false)
		{
			BeginRead(); 
			if(obj == null)
				throw new ArgumentNullException("obj");
			if(obj is IReadWriteLockObject && ((IReadWriteLockObject)obj).IsReadLocked == false)
				((IReadWriteLockObject)obj).BeginRead();
			if(obj.PropertiesValues == null || obj.PropertiesValues[this] == null)
				return null;
			return obj.PropertiesValues[this];
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вовзращает строковое значение свойства для указанного объекта, содержащего коллекцию значений свойств.
		/// </summary>
		/// <param name="obj">Объект, содержащий коллекцию значений свойств.</param>
		/// <param name="selfOnly">Определяет, возвращать ли значения наследуемых всойств</param>
		/// <returns></returns>
		public virtual string GetValueAsString(IPulsarPropertiesValuesContainer obj, bool selfOnly = false)
		{
			return ValueToString(GetValue(obj, selfOnly));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает значение свойства для указанного объекта, содержащего коллекцию значений свойств.
		/// </summary>
		/// <param name="obj">Объект, содержащий коллекцию значений свойств.</param>
		/// <param name="value">Устанавливаемое значение.</param>
		/// <returns></returns>
		public virtual void SetValue(IPulsarPropertiesValuesContainer obj, object value)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			if(obj.PropertiesValues == null)
				throw new PulsarException("У объекта [{0}] не определена коллекция значений свойств!");
			BeginRead();
			if(value != null && Type == PulsarPropertyType.Enum && Enums is IKeyedList && IsListValue == false)
			{
				UInt key = 0;
				if(value is UInt)
				{
					key = (UInt)value;
					if(((IKeyedList)Enums).ContainsKey(key) == false)
						key = 0;
				}
				else
					key = ((IKeyedList)Enums).KeyOf(value);
				if(key == 0)
					throw new PulsarException("Не найден ключ элемента [{0}] свойства [{1}]!", value, this);
				value = key;
			}
			if(Type == PulsarPropertyType.Check && value != null)
			 value = (byte)1;
			if(obj is IReadWriteLockObject && ((IReadWriteLockObject)obj).IsWriteLocked == false)
			 ((IReadWriteLockObject)obj).BeginWrite();
			obj.PropertiesValues.DirectInject(this, value);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет строковое отображение значения свойства.
		/// </summary>
		/// <param name="value">Значение свойства.</param>
		/// <returns></returns>
		public virtual string ValueToString(object value)
		{
			if(value == null)
				return null;
			BeginRead();
			if(Type == PulsarPropertyType.Enum && Enums is IKeyedList && IsListValue == false && value is UInt)
				return (((IKeyedList)Enums).WithKey((UInt)value) ?? "(нет значения)").ToString();
			if (IsListValue)
			{
				if (value is ElasticArray<UInt> && Enums is KeyedList<string>)
				{
					ElasticArray<UInt> arr = (ElasticArray<UInt>)value;
					StringBuilder sb = new StringBuilder();
					for (int a = 0; a < (arr.Length > 10 ? 10 : arr.Length); a++)
						sb.AppendFormat("; {0}", ((KeyedList<string>)Enums).WithKey(arr[a]));
					if (sb.Length > 0)
						sb.Remove(0, 2);
					if (arr.Length > 10)
						sb.Append("; ...");
					value = sb.ToString();
				}
				else if (value is UInt && Enums is KeyedList<string>)
					value = ((KeyedList<string>)Enums).WithKey((UInt)value);
				else
					value = value is IList == false || ((IList)value).Count == 0 ? "(нет значения)" : ((IList)value)[0];
			}
			else if (value is Enum)
				return TypeDescriptor.GetConverter(value).ConvertToString(value);
			if(Type == PulsarPropertyType.Check)
			 return value == null ? "" : CheckValue;
			return (value ?? "").ToString();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод нормализации (приведения к допустимому для свойства виду) значения свойства.
		/// </summary>
		/// <param name="value">Нормализуемое значение.</param>
		/// <returns></returns>
		public virtual object NormalizeValue(object value)
		{
			return value;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region ICloneable Members
		object ICloneable.Clone()
		{
			return this.Clone();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает копию объекта.
		/// </summary>
		/// <returns></returns>
		public virtual PulsarProperty Clone()
		{
			BeginRead();
			var res = (PulsarProperty)Pulsar.Serialization.PulsarSerializer.CloneObject(this);
			return res;
		}
		#endregion
	}
}                      

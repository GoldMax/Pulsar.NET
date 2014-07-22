using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Pulsar;
using Pulsar.Reflection;

namespace Pulsar
{
	/// <summary>
	/// Класс фильтра свойств.
	/// </summary>
	public class PropertiesFilter : PList<PropertiesFilterExpression>
	{
		private bool isAnd = true;
		private bool isModified = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет, является ли операции отношения выражений операцией И.
		/// </summary>
		public bool IsAnd
		{
			get { return isAnd; }
			set
			{
				isAnd = value;
				isModified = true;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, был ли фильтр модифицирован.
		/// </summary>
		public bool IsModified
		{
			get { return isModified; }
			set { isModified = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PropertiesFilter()
			: base()
		{
			//base.ObjectChanged += (o, e) => { IsModified = true; };
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Определяет, содержит ли фильтр хотя бы одно выражение по заданному свойству.
		/// </summary>
		/// <param name="property">Определяемое свойство.</param>
		/// <returns></returns>
		public bool ContainsProperty(PulsarProperty property)
		{
			foreach(PropertiesFilterExpression e in this)
				if(e.Property.Equals(property))
					return true;
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет пустое выражение для заданного свойства.
		/// </summary>
		/// <param name="property">Свойство добавляемого выражения.</param>
		/// <returns></returns>
		public PropertiesFilterExpression Add(PulsarProperty property)
		{
			PropertiesFilterExpression e = new PropertiesFilterExpression(property);
			base.Add(e);
			return e;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет выражение в фильтр.
		/// </summary>
		/// <param name="item">Добавляемое выражение.</param>
		public new void Add(PropertiesFilterExpression item)
		{
			base.Add(item);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if(this.Count == 0)
				return "(нет)";
			StringBuilder sb = new StringBuilder();
			if(this.Count == 1)
				return this[0].ToString();
			foreach(PropertiesFilterExpression ex in this)
				if(sb.Length == 0)
					sb.AppendFormat("{0} {1}", isAnd ? "И" : "ИЛИ", ex.ToString());
				else
					sb.AppendFormat("\r\n {0} {1}", isAnd ? "И" : "ИЛИ", ex.ToString());
			return sb.ToString();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Проверяет объект на соответствие значений его свойств фильтру.
		/// </summary>
		/// <param name="obj">Проверяемый объект.</param>
		/// <returns></returns>
		public bool CheckObject(IPulsarPropertiesValuesContainer obj)
		{
			if (this.Count == 0)
				return true;

			bool? res = null;
			foreach (PropertiesFilterExpression ex in this)
			{
				bool passed = false;
				object propValue = ex.Property.GetValue(obj);

				if (ex.Condition == PropertiesFilterConditions.IsSet)
					passed = propValue != null;
				else if (ex.Condition == PropertiesFilterConditions.IsNotSet)
					passed = propValue == null;
				else if (propValue == null)
					passed = false;
				else
					switch (ex.Condition)
					{
						case PropertiesFilterConditions.Equal:
							if (ex.Property.IsListValue && propValue is IList)
							{
								if (ex.Property.Enums is KeyedList<string>)
									passed = ElasticArray<UInt>.Compare((ElasticArray<UInt>)propValue, (ElasticArray<UInt>)ex.Value);
								else
									passed = ((IList)propValue).Count == 0 ? false
										: ex.Value is IList ? CompareIList((IList)propValue, (IList)ex.Value)
											: Object.Equals(((IList)propValue)[0], ex.Value);
							}
							else
								passed = propValue.Equals(ex.Value);
							break;
						case PropertiesFilterConditions.NonEqual:
							if (ex.Property.IsListValue && propValue is IList)
							{
								if (ex.Property.Enums is KeyedList<string>)
									passed = !ElasticArray<UInt>.Compare((ElasticArray<UInt>)propValue, (ElasticArray<UInt>)ex.Value);
								else
									passed = ((IList)propValue).Count == 0 ? false
										: ex.Value is IList ? !CompareIList((IList)propValue, (IList)ex.Value)
											: !Object.Equals(((IList)propValue)[0], ex.Value);
							}
							else
								passed = !propValue.Equals(ex.Value);
							break;
						case PropertiesFilterConditions.More:
							if (propValue is int && ex.Value is int)
								passed = (int)propValue > (int)ex.Value;
							else if (propValue is decimal && ex.Value is decimal)
								passed = (decimal)propValue > (decimal)ex.Value;
							else
								passed = false;
							break;
						case PropertiesFilterConditions.MoreOrEqual:
							if (propValue is int && ex.Value is int)
								passed = (int)propValue >= (int)ex.Value;
							else if (propValue is decimal && ex.Value is decimal)
								passed = (decimal)propValue >= (decimal)ex.Value;
							else
								passed = false;
							break;
						case PropertiesFilterConditions.Less:
							if (propValue is int && ex.Value is int)
								passed = (int)propValue < (int)ex.Value;
							else if (propValue is decimal && ex.Value is decimal)
								passed = (decimal)propValue < (decimal)ex.Value;
							else
								passed = false;
							break;
						case PropertiesFilterConditions.LessOrEqual:
							if (propValue is int && ex.Value is int)
								passed = (int)propValue <= (int)ex.Value;
							else if (propValue is decimal && ex.Value is decimal)
								passed = (decimal)propValue <= (decimal)ex.Value;
							else
								passed = false;
							break;
						case PropertiesFilterConditions.Contains:
							if (ex.Property.IsListValue && propValue is IList)
							{
								if (ex.Property.Enums is KeyedList<string>)
									passed = ((ElasticArray<UInt>)propValue).Contains((ElasticArray<UInt>)ex.Value);
								else
									passed = ((IList)propValue).Count == 0 ? false
										: ex.Value is IList ? ContainsIList((IList)propValue, (IList)ex.Value)
											: ((IList)propValue).Contains(ex.Value);
							}
							else
								passed = ((string)propValue).ToLower().Contains((ex.Value ?? "").ToString().ToLower());
							break;
						case PropertiesFilterConditions.NotContains:
							if (ex.Property.IsListValue && propValue is IList)
							{
								if (ex.Property.Enums is KeyedList<string>)
									passed = !((ElasticArray<UInt>)propValue).Contains((ElasticArray<UInt>)ex.Value);
								else
									passed = !(((IList)propValue).Count == 0 ? false
										: ex.Value is IList ? !ContainsIList((IList)propValue, (IList)ex.Value)
											: !((IList)propValue).Contains(ex.Value));
							}
							else
								passed = !(((string)propValue).ToLower().Contains((ex.Value ?? "").ToString().ToLower()));
							break;
						case PropertiesFilterConditions.StartWith:
							if (ex.Property.IsListValue && propValue is IList)
								passed = ((IList)propValue).Count == 0 ? false :
											((IList)propValue)[0].ToString().ToLower().StartsWith((ex.Value ?? "").ToString().ToLower());
							else
								passed = ((string)propValue).ToLower().StartsWith((ex.Value ?? "").ToString().ToLower());
							break;
						case PropertiesFilterConditions.EndWith:
							if (ex.Property.IsListValue && propValue is IList)
								passed = ((IList)propValue).Count == 0 ? false :
											((IList)propValue)[0].ToString().ToLower().EndsWith((ex.Value ?? "").ToString().ToLower());
							else
								passed = ((string)propValue).ToLower().EndsWith((ex.Value ?? "").ToString().ToLower());
							break;
					}
				if (res == null)
					res = passed;
				else if (this.IsAnd)
					res = (bool)res && passed;
				else
					res = (bool)res || passed;
			}
			return (bool)res;
		}
		//public IEnumerable<T> CheckObjects(IEnumerable<T> 
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Сравнивает два списка
		/// </summary>
		/// <param name="arg1">Первый список</param>
		/// <param name="arg2">Второй список</param>
		/// <returns>Возвращает признак эквивалентности двух списков</returns>
		private bool CompareIList(IList arg1, IList arg2)
		{
			if (arg1 == null && arg2 == null)
				return true;
			if (arg1 == null || arg2 == null)
				return false;
			if (arg1.Count != arg2.Count)
				return false;
			for (int a = 0; a < arg1.Count; a++)
				if (Object.Equals(arg1[a], arg2[a]) == false)
					return false;
			return true;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли массив arg1 все элементы массива arg2.
		/// </summary>
		/// <param name="arg1">Исходный список</param>
		/// <param name="arg2">Список элементы которого необходимо проверить на вхождение</param>
		/// <returns>Возвращает признак вхождения множества второго списка в первый</returns>
		private bool ContainsIList(IList arg1, IList arg2)
		{
			if (arg2 == null || arg2.Count == 0)
				return false;
			for (int b = 0; b < arg2.Count; b++)
			{
				bool has = false;
				for (int a = 0; a < arg1.Count; a++)
					if (Object.Equals(arg1[a], arg2[b]))
					{
						has = true;
						break;
					}
				if (has == false)
					return false;
			}
			return true;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnObjectChanged
		/// </summary>
		/// <param name="args"></param>
		protected override void OnObjectChanged(ObjectChangeNotifyEventArgs args)
		{
			isModified = true;
			base.OnObjectChanged(args);
		}
		#endregion << Methods >>
	}
	//**************************************************************************************
	#region << public class PropertiesFilterExpression >>
	/// <summary>
	/// Класс выражения фильтра свойства.
	/// </summary>
	public class PropertiesFilterExpression : ObjectChangeNotify
	{
		private PulsarProperty prop = null;
		private PropertiesFilterConditions cond = PropertiesFilterConditions.IsSet;
		private object val = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Свойсво выражения.
		/// </summary>
		public PulsarProperty Property
		{
			get { return prop; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Условие выражения.
		/// </summary>
		public PropertiesFilterConditions Condition
		{
			get { return cond; }
			set
			{
				var arg = OnObjectChanging("Condition", value, cond);
				cond = value;
				OnObjectChanged(arg);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Значение свойства.
		/// </summary>
		public object Value
		{
			get { return val; }
			set
			{
				var arg = OnObjectChanging("Value", value, val);
				this.val = value;
				OnObjectChanged(arg);
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		private PropertiesFilterExpression() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="prop">Свойсво выражения.</param>
		public PropertiesFilterExpression(PulsarProperty prop)
			: this()
		{
			this.prop = prop;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="prop">Свойсво выражения.</param>
		/// <param name="condition">Условие выражения.</param>
		/// <param name="value">Значение свойства.</param>
		public PropertiesFilterExpression(PulsarProperty prop, PropertiesFilterConditions condition,
																																					object value)
		{
			this.prop = prop;
			this.cond = condition;
			this.val = value;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if(cond == PropertiesFilterConditions.IsSet || cond == PropertiesFilterConditions.IsNotSet)
				return String.Format("\"{0}\" {1}",
					prop.Name, TypeDescriptor.GetConverter(cond).ConvertToString(cond).ToLower());
			else
				return String.Format("\"{0}\" {1} [{2}]",
					prop.Name, TypeDescriptor.GetConverter(cond).ConvertToString(cond).ToLower(), prop.ValueToString(val));
		}
	}
	#endregion << public class PropertiesFilterExpression >>
	//**************************************************************************************
	#region << public enum PropertiesFilterConditions : byte >>
	/// <summary>
	/// Перечисление условий выражения фильтра свойства.
	/// </summary>
	[TypeConverter(typeof(EnumTypeConverter))]
	public enum PropertiesFilterConditions : byte
	{
		/// <summary>
		/// Определено
		/// </summary>
		[EnumItemDisplayName("Определено")]
		IsSet = 1,
		/// <summary>
		/// Не определено
		/// </summary>
		[EnumItemDisplayName("Не определено")]
		IsNotSet = 2,
		/// <summary>
		/// Равно
		/// </summary>
		[EnumItemDisplayName("Равно")]
		Equal = 3,
		/// <summary>
		/// Не равно
		/// </summary>
		[EnumItemDisplayName("Не равно")]
		NonEqual = 4,
		/// <summary>
		/// Больше
		/// </summary>
		[EnumItemDisplayName("Больше")]
		More = 5,
		/// <summary>
		/// Больше либо равно
		/// </summary>
		[EnumItemDisplayName("Больше либо равно")]
		MoreOrEqual = 6,
		/// <summary>
		/// Меньше
		/// </summary>
		[EnumItemDisplayName("Меньше")]
		Less = 7,
		/// <summary>
		/// Меньше либо равно
		/// </summary>
		[EnumItemDisplayName("Меньше либо равно")]
		LessOrEqual = 8,
		/// <summary>
		/// Содержит
		/// </summary>
		[EnumItemDisplayName("Содержит")]
		Contains = 9,
		/// <summary>
		/// Не содержит
		/// </summary>
		[EnumItemDisplayName("Не содержит")]
		NotContains = 10,
		/// <summary>
		/// Начинается с
		/// </summary>
		[EnumItemDisplayName("Начинается с")]
		StartWith = 11,
		/// <summary>
		/// Заканчивается на
		/// </summary>
		[EnumItemDisplayName("Заканчивается на")]
		EndWith = 12
	}
	#endregion << public enum PropertiesFilterConditions : byte >>
}

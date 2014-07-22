using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pulsar.Reflection
{
	/// <summary>
	/// Класс дескриптора свойства, создаваемый из PropertyInfo. 
	/// </summary>
	public class PropertyInfoDescriptor : PropertyDescriptor
	{
		/// <summary>
		/// PropertyInfo
		/// </summary>
		protected PropertyInfo pi = null;
		private string displayName = null;
		private int dispOrder = Int32.MinValue;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ComponentType
		/// </summary>
		public override Type ComponentType
		{
			get { return pi.DeclaringType; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// IsReadOnly
		/// </summary>
		public override bool IsReadOnly
		{
			get { return this.Attributes.Contains(ReadOnlyAttribute.Yes); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// PropertyType
		/// </summary>
		public override Type PropertyType
		{
			get { return pi.PropertyType; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		///  DisplayName
		/// </summary>
		public override string DisplayName
		{
			get
			{
				if(displayName == null)
				{
					foreach(Attribute a in pi.GetCustomAttributes(true))
						if(a != null && a is DisplayNameAttribute)
						{
							displayName = ((DisplayNameAttribute)a).DisplayName;
							break;
						}
						if(displayName == null)
							displayName = pi.Name;
				}
				return displayName;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Порядковый номер отображения.
		/// </summary>
		public int DisplayOrder
		{
			get 
			{
				if(dispOrder == Int32.MinValue)
					if(pi.IsDefined(typeof(DisplayOrderAttribute), true))
						dispOrder = ((DisplayOrderAttribute)pi.GetCustomAttributes(typeof(DisplayOrderAttribute), true)[0]).Order;
					else 
						dispOrder = Int32.MaxValue;
				return dispOrder; 
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public PropertyInfoDescriptor(PropertyInfo pi)
			: base(pi.Name, pi.GetCustomAttributes(true).Cast<Attribute>().ToArray())
		{
			this.pi = pi;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="componentType"></param>
		/// <param name="name"></param>
		/// <param name="attributes"></param>
		public PropertyInfoDescriptor(Type componentType, string name, Attribute[] attributes)
			: base(name, attributes)
		{
			pi = componentType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);   
			if(pi == null)
				throw new PulsarException("Не удалось определить PropertyInfo для [{0}]!", name);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="propInfo"></param>
		/// <param name="attributes"></param>
		public PropertyInfoDescriptor(PropertyInfo propInfo, Attribute[] attributes) 
			: base(propInfo.Name, attributes)
		{
			pi = propInfo;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// CanResetValue
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override bool CanResetValue(object component)
		{
			if(component == null)
				return false;
			DefaultValueAttribute attribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
			if(attribute == null)
				return false;
			return Object.Equals(attribute.Value, pi.GetValue(component, null));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ResetValue
		/// </summary>
		/// <param name="component"></param>
		public override void ResetValue(object component)
		{
			DefaultValueAttribute attribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
			if(attribute != null)
				this.SetValue(component, attribute.Value);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ShouldSerializeValue
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override bool ShouldSerializeValue(object component)
		{
			if(component == null)
				return false;
			return !CanResetValue(component);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// GetValue
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override object GetValue(object component)
		{
		 if(component == null)
			 return null;
			//if(pi.ReflectedType != component.GetType())
			if(pi.ReflectedType.IsAssignableFrom(component.GetType()) == false)
				return null;
			return pi.GetValue(component, BindingFlags.GetProperty, null, null, null);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// SetValue
		/// </summary>
		/// <param name="component"></param>
		/// <param name="value"></param>
		public override void SetValue(object component, object value)
		{
			if(IsReadOnly == false && pi.CanWrite)
				pi.SetValue(component, value, BindingFlags.SetProperty, null, null, null);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Name;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
}

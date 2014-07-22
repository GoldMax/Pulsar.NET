using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar
{
	//**************************************************************************************
	#region << public class DisplayOrderAttribute : Attribute >>
	/// <summary>
	/// Атрибут порядкового номера свойства при отображении
	/// Работает, если у класса указан атрибут [TypeDescriptionProvider(typeof(Pulsar.DisplayOrderTypesDescriptor))]
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple=false)]
	public class DisplayOrderAttribute : Attribute
	{
		private int order = 0;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Порядковый номер
		/// </summary>
		public int Order
		{
			get { return order; }
			set { order = value; }
		}

		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public DisplayOrderAttribute()
			: base()
		{

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="order">Порядковый номер</param>
		public DisplayOrderAttribute(int order)
			: this()
		{
			this.order = order;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Возвращает список дескрипторов свойств указанного типа, сортированный с учетом DisplayOrderAttribute
		/// </summary>
		/// <param name="type">Тип</param>
		/// <returns></returns>
		public static IEnumerable<PropertyDescriptor> GetSortedProperties(Type type)
		{
			List<Tuple<PropertyDescriptor, int?>> list = new List<Tuple<PropertyDescriptor, int?>>();

			foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(type))
				if(pd.IsBrowsable)
				{
					DisplayOrderAttribute attr = pd.Attributes.OfType<DisplayOrderAttribute>().FirstOrDefault();
					list.Add(new Tuple<PropertyDescriptor, int?>(pd, attr == null ? null : (int?)attr.Order));
				}
			list.Sort(Sorter);
			List<PropertyDescriptor> res = new List<PropertyDescriptor>(list.Count);
			foreach(Tuple<PropertyDescriptor,int?> tu in list)
				res.Add(tu.Item1);
			return res;
		}
		//-------------------------------------------------------------------------------------
		private static int Sorter(Tuple<PropertyDescriptor, int?> a, Tuple<PropertyDescriptor, int?> b)
		{
			if(a.Item2 == null && b.Item2 == null)
				return 0;
			if(a.Item2 != null && b.Item2 != null)
				return Comparer<int?>.Default.Compare(a.Item2, b.Item2);
			if(a.Item2 != null)
				return -1;
			if(b.Item2 != null)
				return 1;
			return 0;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
	#endregion <<  public class DisplayOrderAttribute : Attribute >>
	//**************************************************************************************
	#region << public class DisplayDataAttribute : Attribute >>
	/// <summary>
	/// Класс атрибута для свойств, являющихся данными.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class DisplayDataAttribute : Attribute
	{
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public DisplayDataAttribute()
		{

		}
	}
	#endregion << public class DisplayDataAttribute : Attribute >>
	//**************************************************************************************
	#region << public class DisplayOrderTypesDescriptor : TypeDescriptionProvider, ICustomTypeDescriptor >>
	/// <summary>
	/// Класс дескриптора типа для упорядочивания свойств по их порядковому номеру DisplayOrderAttribute.
	/// </summary>
	public class DisplayOrderTypesDescriptor : TypeDescriptionProvider, ICustomTypeDescriptor
	{
		private Type type = null;

		#region TypeDescriptionProvider
		/// <summary>
		/// GetTypeDescriptor
		/// </summary>
		/// <param name="objectType"></param>
		/// <param name="instance"></param>
		/// <returns></returns>
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			if(instance == null)
				type = objectType;
			else
				type = instance.GetType();
			return this; //base.GetTypeDescriptor(objectType, instance);
		}
		#endregion TypeDescriptionProvider
		//-------------------------------------------------------------------------------------
		#region ICustomTypeDescriptor Members
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AttributeCollection GetAttributes()
		{
			return new AttributeCollection(type.GetCustomAttributes(true).Cast<Attribute>().ToArray());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public TypeConverter GetConverter()
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetEditor(Type editorBaseType)
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents()
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return GetProperties();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties()
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
			PropertyInfo[] properties = type.GetProperties(bindingAttr);

			PropertyInfoDescriptor[] array = new PropertyInfoDescriptor[properties.Length];
			int num = 0;
			for(int i = 0; i < properties.Length; i++)
			{
				if(properties[i].GetIndexParameters().Length <= 0 && properties[i].GetGetMethod() != null)
					array[num++] = new PropertyInfoDescriptor(properties[i]);
			}
			if(num != array.Length)
			{
				PropertyInfoDescriptor[] array2 = new PropertyInfoDescriptor[num];
				Array.Copy(array, 0, array2, 0, num);
				array = array2;
			}
			Array.Sort(array, (x, y) => x.DisplayOrder.CompareTo(y.DisplayOrder));

			return new PropertyDescriptorCollection(array);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
	#endregion << public class DisplayOrderTypesDescriptor : TypeDescriptionProvider, ICustomTypeDescriptor >>
}

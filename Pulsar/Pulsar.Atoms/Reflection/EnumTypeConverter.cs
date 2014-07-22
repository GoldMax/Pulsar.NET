using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

using Pulsar.Reflection.Dynamic;

namespace Pulsar.Reflection
{
	//**************************************************************************************
	#region << EnumTypeConverter : EnumConverter >>
	/// <summary>
	/// TypeConverter для перечислений, преобразовывающий их к строке с учетом атрибута EnumDisplayName.
	/// </summary>
	public class EnumTypeConverter : EnumConverter
	{
		private Type _enumType;
		private Dictionary<Enum, string> _fields = null;
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="type">Тип перечисления.</param>
		public EnumTypeConverter(Type type) : base(type)
		{
			_enumType = type;
			_fields = new Dictionary<Enum,string>();
			foreach(Enum i in Enum.GetValues(_enumType))
			{
				FieldInfo fi = _enumType.GetField(i.ToString());
				DisplayNameAttribute dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(fi, typeof(DisplayNameAttribute));
				if(dna != null)
					_fields.Add(i, dna.DisplayName);
				else
					_fields.Add(i, i.ToString());
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether this converter can convert the object to the specified type, using the specified context.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context. </param>
		/// <param name="destType">A Type that represents the type you want to convert to. </param>
		/// <returns></returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return destType == typeof(string);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information. 
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="culture">A CultureInfo object. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The Object to convert.</param>
		/// <param name="destType">The Type to convert the value parameter to.</param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
			object value, Type destType)
		{
		 if(destType != typeof(string))
			 return base.ConvertTo(context, culture, value, destType);
			Enum val = (Enum)value;
			if(_fields.ContainsKey(val))
			 return _fields[val];
			//Type t = _enumType.Name == "Enum" ? value.GetType() : _enumType;
			if(_enumType.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0)
			{
				StringBuilder sb = new StringBuilder();
				foreach(Enum i in Enum.GetValues(_enumType))
				 if(val.HasFlag(i) && Object.Equals(Enum.ToObject(_enumType,0),i) == false)
					 sb.AppendFormat(", {0}", _fields[i]);
				if(sb.Length == 0)
				 return val.ToString();
				sb.Remove(0,2);
				return sb.ToString();
			}
			else
			 return val.ToString();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context. 
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="srcType">A Type that represents the type you want to convert from.</param>
		/// <returns></returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
		{
			return srcType == typeof(string);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information. 
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="culture">The CultureInfo to use as the current culture.</param>
		/// <param name="value">The Object to convert.</param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
			object value)
		{
			StringBuilder res = new StringBuilder();
			foreach(string s in ((string)value).Split(','))
			{
			 string ss = s.TrimAll();
			 foreach(var i in _fields)
				 if(i.Value == ss)
					{
					 res.AppendFormat(",{0}", i.Key);
					 break;
					}
			}
			if(res.Length > 0)
			 res.Remove(0,1);
			return Enum.Parse(_enumType, res.ToString());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Статический метод, возвращающий значение атрибута EnumItemDisplayNameAttribute элемента перечисления.
		/// </summary>
		/// <param name="value">Элемент перечисления.</param>
		/// <returns></returns>
		public static string GetItemDisplayName(Enum value)
		{
		 if(value == null)
			 throw new ArgumentNullException("value");
			return (new EnumTypeConverter(value.GetType())).ConvertToString(value);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if(_fields != null)
			{
				var col = new PropertyDescriptorCollection(new PropertyDescriptor[0], false);
				foreach(Enum s in Enum.GetValues(_enumType))
				 if(Object.Equals(Enum.ToObject(_enumType,0),s) == false)
					 col.Add(new FlagsEnumValuePropertyDescriptor(s));
				return col;
			}
			return base.GetProperties(context, value, attributes);
		}
		///
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return _enumType.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0;
		}
		///
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return _enumType.GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0;
		}
		//-------------------------------------------------------------------------------------
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			// false - можно вводить вручную
			// true - только выбор из списка
			return _enumType.GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0; 
		}

		/// <summary>
		/// А вот и список
		/// </summary>
		public override StandardValuesCollection GetStandardValues(
				ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(_fields.Keys);
		}
		//**************************************************************************************
		/// <summary>
		/// 
		/// </summary>
		public class FlagsEnumValuePropertyDescriptor : PropertyDescriptor
		{
			private Enum _val = null;
			private string _dispName = null;
			private Action<object,object> _set = null;
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			public FlagsEnumValuePropertyDescriptor(Enum value)
				: base(Enum.GetName(value.GetType(), value), new Attribute[0])
			{
				_val = value;
				_dispName = EnumTypeConverter.GetItemDisplayName(_val);
			 _set =	ReflectionHelper.CreateFieldSetMethod(_val.GetType(),"value__");
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
			#region << Methods >>
			#pragma warning disable 1591
			public override bool CanResetValue(object component)
			{
				return true;
			}

			public override Type ComponentType
			{
				get { return _val.GetType(); }
			}

			public override object GetValue(object component)
			{
				return ((Enum)component).HasFlag(_val);
			}

			public override bool IsReadOnly
			{
				get { return false; }
			}

			public override Type PropertyType
			{
				get { return typeof(bool); }
			}

			public override void ResetValue(object component)
			{
				throw new NotImplementedException();
			}

			public override void SetValue(object component, object value)
			{
			 long c = (long)Convert.ChangeType(component, typeof(long));
			 long v = (long)Convert.ChangeType(_val, typeof(long));
				if((bool)value == true)
					value = Enum.ToObject(_val.GetType(), c | v);
				else
					value = Enum.ToObject(_val.GetType(), c ^ v);
				_set(component, value);
			}

			public override bool ShouldSerializeValue(object component)
			{
				return (bool)GetValue(component);
			}
			public override string DisplayName
			{
				get	{ return _dispName; }
			}
			#pragma warning restore 1591
			#endregion << Methods >>
			//-------------------------------------------------------------------------------------


		}

	}
	#endregion << EnumTypeConverter : EnumConverter >>
	//**************************************************************************************
	#region << class EnumItemDisplayNameAttribute : DisplayNameAttribute >>
	/// <summary>
	/// Атрибут псевдонима элемента перечисления.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Enum | AttributeTargets.Struct)]
	public class EnumItemDisplayNameAttribute : DisplayNameAttribute
	{
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public EnumItemDisplayNameAttribute(string displayName) : base(displayName)
		{
		}
	}
	#endregion << class EnumItemDisplayNameAttribute : DisplayNameAttribute >>
}

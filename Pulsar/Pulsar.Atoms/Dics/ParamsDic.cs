using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar
{
	/// <summary>
	/// ����� ��������� "��������"="��������"
	/// </summary>
	[Serializable]
	[TypeDescriptionProvider(typeof(Pulsar.ParamsDicTypeDescriptor))]
	public class ParamsDic : PDictionary<string, object>, ICloneable, IDisposable
	{
		private char delimiter = '�';
		private bool delNulls = true;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ����������, ����� �� ������� �������� �� ��������� null.
		/// </summary>
		public bool DeleteWithNullValue
		{
			get { return delNulls; }
			set { delNulls = value; }
		}
		/// <summary>
		/// ������ ����������� ��� � ������� ������������� ���������.
		/// </summary>
		public char PairDelimiter
		{
			get { return delimiter; }
			set { delimiter = value; }
		}
		/// <summary>
		/// ���������� �������� ��������� �� ��� �����. ���� �������� ����������, ���������� null ���
		/// ��������� ��������.
		/// </summary>
		/// <param name="param">��� ���������.</param>
		/// <returns></returns>
		public override object this[string param]
		{
			get
			{
				string p = NormalizeParamName(param);
				if(p == null)
					return null;

				if(ContainsKey(param))
					return base[param];
				else
					return null;
			}
			set { Add(param, value); }
		}
		/// <summary>
		/// ���������� ������ ���������� ���������.
		/// </summary>
		public IEnumerable<string> Params
		{
			get { return base.Keys; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public ParamsDic()	: base()	{		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="capacity">��������� ������� ���������.</param>
		public ParamsDic(int capacity)	: base(capacity) { 	}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		public ParamsDic(bool delNulls) : this()
		{
			this.delNulls = delNulls;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="dic">���������������� ���������.</param>
		public ParamsDic(IEnumerable<KeyValuePair<string, object>> dic) : base(dic) { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		public ParamsDic(params KeyValuePair<string, object>[] args) : 	base(args)	{		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		public ParamsDic(params object[][] args) : base(args.Length)
		{
			foreach(var s in args)
				Add((string)s[0],s[1]);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		public ParamsDic(object x)	: base()
		{
			foreach(var pi in x.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
				Add(pi.Name, pi.GetValue(x,null));
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ������� ������ �� ������.
		/// </summary>
		/// <param name="source">��������� ������������� �������.</param>
		/// <param name="args">��������� ��� ����������� � ��������� �������������.</param>
		public static ParamsDic FromString(string source, params object[] args)
		{
			ParamsDic pars = new ParamsDic();
			pars.Parse(String.Format(source,args));
			return pars;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� �������� ������������ � ���������. ���� �������� ��� ������������,
		/// ����������������� ��� ��������.
		/// </summary>
		/// <param name="param">��� ���������.</param>
		/// <param name="value">�������� ���������.</param>
		public override void Add(string param, object value)
		{
			string p = NormalizeParamName(param);
			if(p == null)
				return;
			
			if(ContainsKey(p))
			{
				if((value == null || value.Equals("NULL")) && delNulls)
				 Remove(param);
				else
				base[param] = value;
			} 
			else if(value != null || delNulls == false)
			 base.Add(param, value);
		}
		/// <summary>
		/// ��������� �������� �� ������.
		/// </summary>
		/// <param name="r">����� ������.</param>
		public void Add(SqlDataReader r)
		{
			try
			{
				EventsOff();
				if(r.HasRows == false || r.IsClosed)
					return;
				for(int a = 0; a < r.FieldCount; a++)
					if(r.IsDBNull(a) == false)
						this.Add(r.GetName(a), r[a]);
			}
			finally
			{
				EventsOn();
			}
		}
		/// <summary>
		/// ��������� �������� �� ������ ���������.
		/// </summary>
		/// <param name="pars">���������, �������� ������� �����������.</param>
		public void Add(ParamsDic pars)
		{
			try
			{
				EventsOff();
				foreach(string s in pars.Params)
					this.Add(s, pars[s]);
		}
			finally
			{
				EventsOn();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ����������� ��������� � ���������
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public bool ContainsParam(string param)
		{
			string p = NormalizeParamName(param);
			if(p == null)
				return false;

			return base.ContainsKey(p);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public override bool ContainsKey(string param)
		{
			return ContainsParam(param);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="param"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool TryGetValue(string param, out object value)
		{
			return base.TryGetValue(NormalizeParamName(param), out value);
		}
		/// <summary>
		/// ������� �������� �� ���������, ���� �� ������������.
		/// </summary>
		/// <param name="param">��� ���������</param>
		/// <returns></returns>
		public override void Remove(string param)
		{
			string p = NormalizeParamName(param);
			if(p != null)
			 base.Remove(p);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ��������� ������������� �������.
		/// </summary>
		/// <param name="source">����������� ������.</param>
		/// <returns></returns>
		public bool Parse(string source)
		{
			try
			{
				EventsOff();
				string[] ss = source.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
				if(ss.Length == 0)
					return false;
				foreach(string s in ss)
				{
					if(s.Contains("=") == false)
						Add(s, null);
					else
					{
						int pos = s.IndexOf('=');
						string[] pair = new string[2];
						pair[0] = s.Substring(0, pos);
						pair[1] = s.Substring(pos + 1, s.Length - pos - 1);
						bool bVal;
						int iVal;
						decimal dVal;
						DateTime dtVal;
						OID gVal;
						if(Boolean.TryParse(pair[1], out bVal))
							Add(pair[0], bVal);
						else if(Int32.TryParse(pair[1], out iVal))
							Add(pair[0], iVal);
						else if(OID.TryParse(pair[1], out gVal))
							Add(pair[0], gVal);
						else if(Decimal.TryParse(pair[1].Replace('.', ','), out dVal))
							Add(pair[0], dVal);
						else if(DateTime.TryParse(pair[1], new CultureInfo("ru-RU"), System.Globalization.DateTimeStyles.None, out dtVal))
							Add(pair[0], dtVal);
						else
							Add(pair[0], pair[1]);
					}
				}
				return true;
			}
			finally
			{
				EventsOn();
			}
		}
		private string NormalizeParamName(string param)
		{
			if(param == null)
				return null;
			string p = param.Trim(); // .ToLower()
			if(p.Length == 0)
				return null;
			return p; 
		}
		//-------------------------------------------------------------------------------------
		object ICloneable.Clone()
		{
			return Clone();
		}
		/// <summary>
		/// ���������� ����� �������� �������.
		/// </summary>
		/// <returns></returns>
		public ParamsDic Clone()
		{
			return new ParamsDic(this);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << Static & Override Methods >>
		/// <summary>
		/// ���������� ��������� ������������� �������.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach(var i in this)
				sb.AppendFormat("{0}{1}={2}", delimiter, i.Key, i.Value);
			if(sb.Length > 0)
				sb.Remove(0,1);
			return sb.Length == 0 ? "(�����)" : sb.ToString(); 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ���������, ����������� �� ������.
		/// </summary>
		/// <param name="r">����� ������.</param>
		/// <returns></returns>
		public static ParamsDic LoadFrom(SqlDataReader r)
		{
			ParamsDic p = new ParamsDic();
			p.Add(r);
			return p;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static implicit operator ParamsDic(KeyValuePair<string, object>[] array)
		{
			return new ParamsDic(array);
		}
		#endregion << Static & Override Methods >>
		//-------------------------------------------------------------------------------------
		#region ISerializable Members
		/// <summary>
		/// ����������� ������������.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ParamsDic(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			delimiter = info.GetChar("delimiter");
			//delNulls = 	info.GetBoolean("delNulls");
		}
		/// <summary>
		/// ISerializable.GetObjectData
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context) 
		{
		 base.GetObjectData(info, context);
			info.AddValue("delimiter", delimiter);
			//info.AddValue("delNulls", delNulls);
		}
		#endregion
	}
	//**************************************************************************************
	#region << public class ParamsDicTypeDescriptor : TypeDescriptionProvider, ICustomTypeDescriptor >>
	/// <summary>
	/// ����� ����������� ���� ��������� "��������"="��������".
	/// </summary>
	public class ParamsDicTypeDescriptor : TypeDescriptionProvider, ICustomTypeDescriptor
	{
		private static TypeDescriptionProvider baseDesc = TypeDescriptor.GetProvider(typeof(ParamsDic));
		private ParamsDic dic = null;

		#region TypeDescriptionProvider
		/// <summary>
		/// GetTypeDescriptor
		/// </summary>
		/// <param name="objectType"></param>
		/// <param name="instance"></param>
		/// <returns></returns>
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			if(instance == null || instance is ParamsDic == false)
				return baseDesc.GetTypeDescriptor(objectType, instance);
			dic = (ParamsDic)instance;
			return this; //
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
			return new AttributeCollection((new List<Attribute>(dic.GetType().GetCustomAttributes(true).Cast<Attribute>())).ToArray());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetClassName()
		{
			return typeof(ParamsDic).Name;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetComponentName()
		{
			return typeof(ParamsDic).FullName;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public TypeConverter GetConverter()
		{
			return new TypeConverter(); 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptor GetDefaultEvent()
		{
			return null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetEditor(Type editorBaseType)
		{
			return null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(new EventDescriptor[0]);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public EventDescriptorCollection GetEvents()
		{
			return new EventDescriptorCollection(new EventDescriptor[0]);
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
			List<ParamsDicPropertyDescriptor> list = new List<ParamsDicPropertyDescriptor>(dic.Count);
			foreach(var i in dic)
				list.Add(new ParamsDicPropertyDescriptor(i.Key, (i.Value ?? "").GetType()));
			return new PropertyDescriptorCollection(list.ToArray());
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
	#endregion << public class ParamsDicTypeDescriptor : TypeDescriptionProvider, ICustomTypeDescriptor >>
	//**************************************************************************************
	#region << public class ParamsDicPropertyDescriptor : PropertyDescriptor >>
	/// <summary>
	/// 
	/// </summary>
	public class ParamsDicPropertyDescriptor : PropertyDescriptor
	{
		private Type _type = null;
		private string _name = null;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		public ParamsDicPropertyDescriptor(string name, Type type)
			: base(name.TrimStart('$'), new Attribute[0])
		{
		 _name = name;
			this._type = type;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public override bool CanResetValue(object component)
		{
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override Type ComponentType
		{
			get { return typeof(ParamsDic); }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override object GetValue(object component)
		{
			if(component == null || component is ParamsDic == false)
				throw new ArgumentException("GetValue - �������� �� ParamsDic!", "component");
			return ((ParamsDic)component)[_name];
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override bool IsReadOnly
		{
			get { return _name.StartsWith("$"); }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override string Category
		{
			get { return "���������"; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override Type PropertyType
		{
			get { return _type; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override void ResetValue(object component)
		{
			if(component == null || component is ParamsDic == false)
				throw new ArgumentException("GetValue - �������� �� ParamsDic!", "component");
			((ParamsDic)component)[_name] = null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override void SetValue(object component, object value)
		{
			if(component == null || component is ParamsDic == false)
				throw new ArgumentException("GetValue - �������� �� ParamsDic!", "component");
				if(value is String && ((string)value).TrimAll().Length == 0)
				 value = null;
			((ParamsDic)component)[_name] = value;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			return GetValue(component) != null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public override string DisplayName
		{
			get
			{
			 if(_name == "$Name")
				 return "������������";
				return base.DisplayName;
			}
		}
	} 
	#endregion << public class ParamsDicPropertyDescriptor : PropertyDescriptor >>
}

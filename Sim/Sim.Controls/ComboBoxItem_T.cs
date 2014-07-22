using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Pulsar;

namespace Sim.Controls
{
	#region << public interface IComboBoxItem >>
	/// <summary>
	/// ������� ��������� �������� �������� ComboBox.
	/// </summary>
	public interface IComboBoxItem
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ���� ��������.
		/// </summary>
		object Key { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������� ��������.
		/// </summary>
		string Value { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������������� ������ ��������.
		/// </summary>
		object Tag { get; set; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


	}
	#endregion << public interface IComboBoxItem >>
	//**************************************************************************************
	#region public class ComboBoxItem<T>
	/// <summary>
	/// ����� �������� �������� ComboBox.
	/// </summary>
	public class ComboBoxItem<T> : ObjectChangeNotify, IComparable, IComparable<T>, IComboBoxItem, ICloneable 
	{
		private T key = default(T);
		private string val = null;
		private object tag = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ���� ��������.
		/// </summary>
		public T Key
		{
			get { return key; }
			set
			{ 
				var arg = OnObjectChanging("Key", value, key);
				key = value;
				OnObjectChanged(arg); 
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������� ��������.
		/// </summary>
		public string Value
		{
			get { return val ?? (Object.Equals(key,default(T)) ? "" : key.ToString()); }
			set 
			{
				var arg = OnObjectChanging("Value", value, val);
				val = value;
				OnObjectChanged(arg);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������������� ������ ��������.
		/// </summary>
		public object Tag
		{
			get { return tag; }
			set { tag = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public ComboBoxItem() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="key">����.</param>
		public ComboBoxItem(T key)
		{
			this.key = key;
			//this.Value = TypeDescriptor.GetConverter(key).ConvertToString(key);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="key">����.</param>
		/// <param name="value">��������.</param>
		public ComboBoxItem(T key, string value)
		{
			this.key = key;
			this.Value = value;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Public Methods >>
		/// <summary>
		/// ToString()
		/// </summary>
		public override string ToString()
		{
			if(val != null)
				return val;
			return TypeDescriptor.GetConverter(key).ConvertToString(key);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ������� ������� � ���������.
		/// </summary>
		/// <param name="obj">������, � ������� ������������ �������.</param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			if(obj.GetType() == typeof(String))
				return this.ToString().CompareTo(obj.ToString());
			if(obj.GetType() == typeof(T))
				return Object.Equals(key,obj) ? 0 : -1;
			if(obj.GetType() == typeof(ComboBoxItem<T>))
				return this.ToString().CompareTo(obj.ToString()); ;
			return -1;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Equals()
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(obj is ComboBoxItem<T> == false && CompareTo(obj) == 0)
				return true;
			return base.Equals(obj);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// GetHashCode()
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ������� ������� � ���������.
		/// </summary>
		/// <param name="other">������, � ������� ������������ �������.</param>
		/// <returns></returns>
		public int CompareTo(T other)
		{
			return key.Equals(other) ? 0 : -1;
		}
		#endregion << Public Methods >>
		//-------------------------------------------------------------------------------------
		#region IComboBoxItem Members
		object IComboBoxItem.Key
		{
			get { return (object)key; }
			set { key = (T)Convert.ChangeType(value, typeof(T)); }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region ICloneable Members
		object ICloneable.Clone()
		{
			return this.Clone();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ����� �������� �������.
		/// </summary>
		/// <returns></returns>
		public ComboBoxItem<T> Clone()
		{
			ComboBoxItem<T> c = new ComboBoxItem<T>();
			c.Key = this.Key;
			c.Value = this.Value;
			return c;
		}
		#endregion
	}
	#endregion public class ComboBoxItem<T>
	//**************************************************************************************

}

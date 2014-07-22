using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Pulsar
{
	/// <summary>
	/// ����� �������� ���� �������.
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	[Serializable]
	public class ValuesPair <T1, T2> : ICloneable 
	{
		private T1 val1;
		private T2 val2;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// �������� ������ ��������.
		/// </summary>
		public T1 Value1
		{
			get { return val1; }
			set { val1 = value; }
		}
		/// <summary>
		/// �������� ������ ��������.
		/// </summary>
		public T2 Value2
		{
			get { return val2; }
			set { val2 = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public ValuesPair()
		{
			val1 = default(T1);
			val2 = default(T2);
		}
		//-------------------------------------------------------------------------------------
			/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="tryCreateValues">���� true, ����� ����������� ������� ������ �������������
		/// �� ��������� �������. </param>
		public ValuesPair(bool tryCreateValues) : this()
		{
			if(tryCreateValues == false)
				return;
			if(typeof(T1).IsClass)
			{
				ConstructorInfo ci = typeof(T1).GetConstructor(Type.EmptyTypes);
				if(ci != null)
					val1 = (T1)ci.Invoke(null);
			}
			if(typeof(T2).IsClass)
			{
				ConstructorInfo ci = typeof(T2).GetConstructor(Type.EmptyTypes);
				if(ci != null)
					val2 = (T2)ci.Invoke(null);
			}
		}
		//-------------------------------------------------------------------------------------
	/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="value1">�������� ������ ��������.</param>
		/// <param name="value2">�������� ������ ��������.</param>
		public ValuesPair(T1 value1, T2 value2)
		{
			val1 = value1;
			val2 = value2;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("V1:{0};V2:{1} - {2}", val1 == null ? "null" : val1.ToString(),
																																															val2 == null ? "null" : val2.ToString(),
																																															base.ToString());
		}
		//-------------------------------------------------------------------------------------
		#region ICloneable Members
		/// <summary>
		/// ������� ����� �������
		/// </summary>
		/// <returns></returns>
		public ValuesPair <T1, T2> Clone()
		{
			return (ValuesPair <T1, T2>)MemberwiseClone();
		}
		object ICloneable.Clone()
		{
			return MemberwiseClone();
		}

		#endregion
	}
}

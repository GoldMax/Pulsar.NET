using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	//**************************************************************************************
	#region << public class OID >>
	/// <summary>
	/// Класс идентификатора объекта
	/// </summary>
	[Serializable]
	public sealed class OID : IFormattable, IComparable,
				IComparable<Guid>, IEquatable<Guid>, IEquatable<OID>, IComparable<OID>, IConvertible
	{
		private Guid oid;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public OID() { oid = Guid.NewGuid(); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public OID(string guid) { oid = new Guid(guid); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public OID(byte[] b) { oid = new Guid(b); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public OID(Guid guid) { oid = guid; }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return oid.ToString();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// GetHashCode()
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return oid.GetHashCode();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Equals(object obj)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(obj is OID)
				return (OID)obj == this;
			if(obj is Guid)
				return Equals((Guid)obj);
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static implicit operator OID(Guid guid)
		{
			return new OID(guid);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oid"></param>
		/// <returns></returns>
		public static implicit operator Guid(OID oid)
		{
			return oid.oid;
		}
		//-------------------------------------------------------------------------------------
		public static bool TryParse(string s,	out OID oid)
		{
		 Guid g;
			oid = null;
			bool res = Guid.TryParse(s, out g);
			if(res)
				oid = g;
			return res;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IFormattable Members
		/// <summary>
		/// ToString(string format, IFormatProvider formatProvider)
		/// </summary>
		/// <param name="format"></param>
		/// <param name="formatProvider"></param>
		/// <returns></returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return oid.ToString(format, formatProvider);
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IComparable Members
		int IComparable.CompareTo(object obj)
		{
			if(obj is OID)
				return oid.CompareTo(((OID)obj).oid);
			return oid.CompareTo(obj);
		}
		/// <summary>
		/// CompareTo(Guid guid)
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public int CompareTo(Guid guid)
		{
			return oid.CompareTo(guid);
		}
		/// <summary>
		/// CompareTo(OID oid)
		/// </summary>
		/// <param name="oid"></param>
		/// <returns></returns>
		public int CompareTo(OID oid)
		{
			return oid.CompareTo(oid.oid);
		}
		#endregion IComparable Members
		//-------------------------------------------------------------------------------------
		#region IEquatable<Guid> Members
		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(Guid other)
		{
			return oid.Equals(other);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(OID other)
		{
			return oid.Equals(other.oid);
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IConvertible Members

		TypeCode IConvertible.GetTypeCode()
		{
			throw new NotImplementedException();
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return oid.ToString();
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			if(conversionType == typeof(Guid))
				return oid;
			else
				throw new Pulsar.PulsarException("Нельзя привести значение типа [{0}] к типу [{2}]!",
																																																		conversionType.FullName, typeof(OID).FullName);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		#endregion
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ==
		/// </summary>
		/// <param name="oid1"></param>
		/// <param name="oid2"></param>
		/// <returns></returns>
		public static bool operator ==(OID oid1, OID oid2)
		{
			if(Object.Equals(oid1, null) && Object.Equals(oid2, null))
				return true;
			if(Object.Equals(oid1, null) || Object.Equals(oid2, null))
				return false;
			return oid1.oid == oid2.oid;
		}
		/// <summary>
		/// ==
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="oid"></param>
		/// <returns></returns>
		public static bool operator ==(Guid guid, OID oid)
		{
			if(Object.Equals(guid, null) && Object.Equals(oid, null))
				return true;
			if(Object.Equals(oid, null))
				return false;
			return guid == oid.oid;
		}
		/// <summary>
		/// ==
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="oid"></param>
		/// <returns></returns>
		public static bool operator ==(OID oid, Guid guid)
		{
			if(Object.Equals(guid, null) && Object.Equals(oid, null))
				return true;
			if(Object.Equals(oid, null))
				return false;
			return guid == oid.oid;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// !=
		/// </summary>
		/// <param name="oid1"></param>
		/// <param name="oid2"></param>
		/// <returns></returns>
		public static bool operator !=(OID oid1, OID oid2)
		{
			if(Object.Equals(oid1, null) && Object.Equals(oid2, null))
				return false;
			if(Object.Equals(oid1, null) || Object.Equals(oid2, null))
				return true;
			return oid1.oid != oid2.oid;
		}
		/// <summary>
		/// !=
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="oid"></param>
		/// <returns></returns>
		public static bool operator !=(Guid guid, OID oid)
		{
			if(Object.Equals(guid, null) && Object.Equals(oid, null))
				return false;
			if(Object.Equals(oid, null))
				return true;
			return guid != oid.oid;
		}
		/// <summary>
		/// !=
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="oid"></param>
		/// <returns></returns>
		public static bool operator !=(OID oid, Guid guid)
		{
			if(Object.Equals(guid, null) && Object.Equals(oid, null))
				return false;
			if(Object.Equals(oid, null))
				return true;
			return guid != oid.oid;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class OID >>

}

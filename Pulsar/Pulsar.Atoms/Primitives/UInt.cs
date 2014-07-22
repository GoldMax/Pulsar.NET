using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Структура целого значения с оптимальным количеством занимаемой памяти.
	/// </summary>
	public struct UInt : ISelfSerialization, IComparable<UInt>
	{
		private static UInt empty = new UInt();

		private byte[] _val;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Размер в байтах
		/// </summary>
		public int Size
		{
			get { return _val == null ? 0 : _val.Length; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает пустое число.
		/// </summary>
		public static UInt Empty
		{
			get { return empty; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Значение Byte
		/// </summary>
		public byte ByteValue
		{
			get { return (byte)this; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Значение UShort
		/// </summary>
		public ushort UShortValue
		{
			get { return (ushort)this; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Значение UInt
		/// </summary>
		public uint UIntValue
		{
			get { return (uint)this; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Значение ULong
		/// </summary>
		public ulong ULongValue
		{
			get { return (ulong)this; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор для Byte.
		/// </summary>
		public UInt(byte value)
		{
			_val = null;
			if (value != 0)
				_val = new byte[1] { value };
		}
		/// <summary>
		/// Конструктор для SByte.
		/// </summary>
		public UInt(sbyte value)
		{
			_val = null;
			if (value != 0)
				_val = new byte[1] { (byte)value };
		}
		/// <summary>
		/// Конструктор для Int16.
		/// </summary>
		public UInt(short value)
		{
			_val = null;
			_val = Zip((ulong)value, sizeof(ushort));
		}
		/// <summary>
		/// Конструктор для UInt16.
		/// </summary>
		public UInt(ushort value)
		{
			_val = null;
			_val = Zip(value, sizeof(ushort));

		}
		/// <summary>
		/// Конструктор для Int32.
		/// </summary>
		public UInt(int value)
		{
			_val = null;
			_val = Zip((uint)value, sizeof(uint));
		}
		/// <summary>
		/// Конструктор для UInt32.
		/// </summary>
		public UInt(uint value)
		{
			_val = null;
			_val = Zip(value, sizeof(uint));

		}
		/// <summary>
		/// Конструктор для Int64.
		/// </summary>
		public UInt(long value)
		{
			_val = null;
			_val = Zip((ulong)value, sizeof(ulong));

		}
		/// <summary>
		/// Конструктор для UInt64.
		/// </summary>
		public UInt(ulong value)
		{
			_val = null;
			_val = Zip(value, sizeof(ulong));

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Private Methods >>
		private byte[] Zip(ulong value, int size)
		{
			if (value == 0)
				return null;
			ulong x = ulong.MaxValue;
			x = x >> (8 * (sizeof(ulong) - size));
			while (((x = x >> 8) & value) == value)
				size--;

			byte[] res = new byte[size];
			for (int a = 0; a < size; a++)
			{
				res[a] = (byte)value;
				value = value >> 8;
			}
			return res;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (_val == null)
				return "0";
			if (_val.Length == 1)
				return _val[0].ToString();
			if (_val.Length <= 8)
				return ToUInt64(_val).ToString();
			return " Ошибка формата!";
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is UInt == false || obj == null)
				return false;
			UInt o = (UInt)obj;
			if (_val == null && o._val == null)
				return true;
			if (_val == null || o._val == null)
				return false;
			if (o._val.Length != _val.Length)
				return false;
			for (int a = 0; a < _val.Length; a++)
				if (o._val[a] != _val[a])
					return false;
			return true;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return _val == null ? 0 : ToUInt64(_val).GetHashCode();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод преобразования массива байт в UInt64.
		/// </summary>
		/// <param name="arr">Преобразуемый массив.</param>
		/// <returns></returns>
		public static ulong ToUInt64(byte[] arr)
		{
			ulong res = 0;
			foreach (byte b in arr.Reverse())
				res = (res << 8) + b;
			return res;
		}
		/// <summary>
		/// Метод преобразования массива байт в UInt32.
		/// </summary>
		/// <param name="arr">Преобразуемый массив.</param>
		/// <returns></returns>
		public static uint ToUInt32(byte[] arr)
		{
			uint res = 0;
			foreach (byte b in arr.Reverse())
				res = (res << 8) + b;
			return res;
		}
		/// <summary>
		/// Метод преобразования массива байт в UInt16.
		/// </summary>
		/// <param name="arr">Преобразуемый массив.</param>
		/// <returns></returns>
		public static ushort ToUInt16(byte[] arr)
		{
			return (ushort)ToUInt32(arr);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод сравнения двух чисел.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static int Compare(UInt x, UInt y)
		{
			return x.CompareTo(y);
		}
		#endregion << Private Methods >>
		//-------------------------------------------------------------------------------------
		#region << Operations >>
#pragma warning disable
		public static implicit operator int(UInt val)
		{
			if (val._val == null)
				return 0;
			return (int)ToUInt32(val._val);
		}
		public static implicit operator uint(UInt val)
		{
			if (val._val == null)
				return 0;
			return ToUInt32(val._val);
		}
		public static implicit operator long(UInt val)
		{
			if (val._val == null)
				return 0;
			return (long)ToUInt64(val._val);
		}
		public static implicit operator ulong(UInt val)
		{
			if (val._val == null)
				return 0;
			return ToUInt64(val._val);
		}
		public static implicit operator short(UInt val)
		{
			if (val._val == null)
				return 0;
			return (short)ToUInt16(val._val);
		}
		public static implicit operator ushort(UInt val)
		{
			if (val._val == null)
				return 0;
			return ToUInt16(val._val);
		}
		public static implicit operator sbyte(UInt val)
		{
			if (val._val == null)
				return 0;
			return (sbyte)val._val[0];
		}
		public static implicit operator byte(UInt val)
		{
			if (val._val == null)
				return 0;
			return val._val[0];
		}
		//-------------------------------------------------------------------------------------
		public static implicit operator UInt(int val)
		{
			return new UInt(val);
		}
		public static implicit operator UInt(uint val)
		{
			return new UInt(val);
		}
		public static implicit operator UInt(long val)
		{
			return new UInt(val);
		}
		public static implicit operator UInt(ulong val)
		{
			return new UInt(val);
		}
		public static implicit operator UInt(short val)
		{
			return new UInt(val);
		}
		public static implicit operator UInt(ushort val)
		{
			return new UInt(val);
		}
		public static implicit operator UInt(byte val)
		{
			return new UInt(val);
		}
		public static implicit operator UInt(sbyte val)
		{
			return new UInt(val);
		}
		//-------------------------------------------------------------------------------------
		public static bool operator ==(UInt x, UInt y)
		{
			return Object.Equals(x, y);
		}
		public static bool operator !=(UInt x, UInt y)
		{
			return !Object.Equals(x, y);
		}
		public static bool operator >(UInt x, UInt y)
		{
			return x.CompareTo(y) > 0;
		}
		public static bool operator <(UInt x, UInt y)
		{
			return x.CompareTo(y) < 0;
		}
		public static UInt operator --(UInt x)
		{
			return x -= 1;
		}
		public static UInt operator ++(UInt x)
		{
			return x += 1;
		}
#pragma warning restore
		#endregion << Operations >>
		//-------------------------------------------------------------------------------------
		#region ISelfSerialization Members
		byte[] ISelfSerialization.GetSerializedData()
		{
			return _val;
		}
		//-------------------------------------------------------------------------------------
		void ISelfSerialization.Deserialize(byte[] data)
		{
			_val = data;
		}

		#endregion
		//-------------------------------------------------------------------------------------
		#region IComparable<UInt> Members
		/// <summary>
		/// Сравнивает с другим числом
		/// </summary>
		/// <param name="other">Число для сравнения.</param>
		/// <returns></returns>
		public int CompareTo(UInt other)
		{
			if (_val == null && other._val == null)
				return 0;
			if (_val == null)
				return -1;
			if (other._val == null)
				return 1;
			if (_val.Length != other._val.Length)
				return _val.Length.CompareTo(other._val.Length);
			for (int a = _val.Length - 1; a >= 0; a--)
				if (_val[a] != other._val[a])
					return _val[a].CompareTo(other._val[a]);
			return 0;
		}
		#endregion

	}
}

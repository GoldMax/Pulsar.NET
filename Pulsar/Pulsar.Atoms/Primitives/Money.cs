using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Деньги c 2-мя знаками после запятой
 /// </summary>
 [Serializable]
 [TypeConverter(typeof(DecimalConverter))]
 public struct Money : IFormattable, IComparable, ISelfSerialization,
    IConvertible, IComparable<decimal>, IEquatable<decimal>, IComparable<Money>, IEquatable<Money>
 {
  private decimal val;

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Возвращает нулевую денежку
  /// </summary>
  public static Money Zero
  {
   get { return new Money(0); }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Money(decimal val)
  {
   this.val = Decimal.Round(val, 2);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Money(int val)
  {
   this.val = val;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region Object Members
  /// <summary>
  /// ToString
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return val.ToString("#,0.00");
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// GetHashCode()
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
  {
   return val.GetHashCode();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object obj)
  {
   return val.Equals(obj);
  }
  #endregion Object Members
  //-------------------------------------------------------------------------------------
  #region IFormattable Members
  /// <summary>
  /// ToString
  /// </summary>
  /// <param name="format"></param>
  /// <param name="formatProvider"></param>
  /// <returns></returns>
  public string ToString(string format, IFormatProvider formatProvider)
  {
   return val.ToString(format, formatProvider);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ToString
  /// </summary>
  /// <param name="format"></param>
  /// <returns></returns>
  public string ToString(string format)
  {
   return val.ToString(format);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IComparable Members
  int IComparable.CompareTo(object obj)
  {
   if(obj is decimal)
    return val.CompareTo(obj);
   if(obj is Money)
    return this.CompareTo((Money)obj);
   throw new Exception("IComparable.CompareTo(object obj) - obj должно быть типа decimal или Money!");
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// CompareTo
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(Money other)
  {
   return val.CompareTo(other.val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// CompareTo
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(decimal other)
  {
   return val.CompareTo(other);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IEquatable<decimal> Members
  /// <summary>
  /// 
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public bool Equals(decimal other)
  {
   return val.Equals(other);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public bool Equals(Money other)
  {
   return val.Equals(other.val);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IConvertible Members
  TypeCode IConvertible.GetTypeCode()
  {
   return val.GetTypeCode();
  }
  //-------------------------------------------------------------------------------------
  bool IConvertible.ToBoolean(IFormatProvider provider)
  {
   return ((IConvertible)val).ToBoolean(provider);
  }
  //-------------------------------------------------------------------------------------
  byte IConvertible.ToByte(IFormatProvider provider)
  {
   return ((IConvertible)val).ToByte(provider);
  }
  //-------------------------------------------------------------------------------------
  char IConvertible.ToChar(IFormatProvider provider)
  {
   return ((IConvertible)val).ToChar(provider);
  }
  //-------------------------------------------------------------------------------------
  DateTime IConvertible.ToDateTime(IFormatProvider provider)
  {
   return ((IConvertible)val).ToDateTime(provider);
  }
  //-------------------------------------------------------------------------------------
  decimal IConvertible.ToDecimal(IFormatProvider provider)
  {
   return ((IConvertible)val).ToDecimal(provider);
  }
  //-------------------------------------------------------------------------------------
  double IConvertible.ToDouble(IFormatProvider provider)
  {
   return ((IConvertible)val).ToDouble(provider);
  }
  //-------------------------------------------------------------------------------------
  short IConvertible.ToInt16(IFormatProvider provider)
  {
   return ((IConvertible)val).ToInt16(provider);
  }
  //-------------------------------------------------------------------------------------
  int IConvertible.ToInt32(IFormatProvider provider)
  {
   return ((IConvertible)val).ToInt32(provider);
  }
  //-------------------------------------------------------------------------------------
  long IConvertible.ToInt64(IFormatProvider provider)
  {
   return ((IConvertible)val).ToInt64(provider);
  }
  //-------------------------------------------------------------------------------------
  sbyte IConvertible.ToSByte(IFormatProvider provider)
  {
   return ((IConvertible)val).ToSByte(provider);
  }
  //-------------------------------------------------------------------------------------
  float IConvertible.ToSingle(IFormatProvider provider)
  {
   return ((IConvertible)val).ToSingle(provider);
  }
  //-------------------------------------------------------------------------------------
  string IConvertible.ToString(IFormatProvider provider)
  {
   return ((IConvertible)val).ToString(provider);
  }
  //-------------------------------------------------------------------------------------
  object IConvertible.ToType(Type conversionType, IFormatProvider provider)
  {
   if(conversionType == typeof(Money))
    return this;
   if(conversionType == typeof(MoneyPrecise))
    return new MoneyPrecise(val);
   return ((IConvertible)val).ToType(conversionType, provider);
  }
  //-------------------------------------------------------------------------------------
  ushort IConvertible.ToUInt16(IFormatProvider provider)
  {
   return ((IConvertible)val).ToUInt16(provider);
  }
  //-------------------------------------------------------------------------------------
  uint IConvertible.ToUInt32(IFormatProvider provider)
  {
   return ((IConvertible)val).ToUInt32(provider);
  }
  //-------------------------------------------------------------------------------------
  ulong IConvertible.ToUInt64(IFormatProvider provider)
  {
   return ((IConvertible)val).ToUInt64(provider);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region Operations
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static implicit operator Money(int val)
  {
   return new Money(val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static implicit operator MoneyPrecise(Money val)
  {
   return new MoneyPrecise(val.val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static implicit operator Money(decimal val)
  {
   return new Money(val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static explicit operator decimal(Money val)
  {
   return val.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator < (Money arg1, Money arg2)
  {
   return arg1.val < arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator <= (Money arg1, Money arg2)
  {
   return arg1.val <= arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator > (Money arg1, Money arg2)
  {
   return arg1.val > arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator >= (Money arg1, Money arg2)
  {
   return arg1.val >= arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator == (Money arg1, Money arg2)
  {
   return arg1.val == arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator != (Money arg1, Money arg2)
  {
   return arg1.val != arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// +
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator +(Money arg1, Money arg2)
  {
   return arg1.val + arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// -
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator -(Money arg1, Money arg2)
  {
   return arg1.val - arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// *
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator *(Money arg1, Money arg2)
  {
   return Decimal.Round(arg1.val *arg2.val,2);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// /
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator /(Money arg1, Money arg2)
  {
   return Decimal.Round(arg1.val / arg2.val,2);
  }
  #endregion Operations
  //-------------------------------------------------------------------------------------
  #region Static Methods
  /// <summary>
  /// Parse
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static Money Parse(string str)
  {
   Decimal res;
   if(Decimal.TryParse(str.AsNumeric(),out res))
    return new Money(res);
   else
    throw new ArgumentException("Строка не может быть преобразована к Money!", "str");
  }
  /// <summary>
  /// TryParse
  /// </summary>
  /// <param name="str"></param>
  /// <param name="val"></param>
  /// <returns></returns>
  public static bool TryParse(string str, out Money val)
  {
   Decimal res;
   if(Decimal.TryParse(str.AsNumeric(),out res))
   {
    val = new Money(res);
    return true;
   }
   val = 0;
   return false;
  }
   /// <summary>
  /// Проверяет строку на соответствие ее числу.
  /// </summary>
  /// <param name="s">Проверяемая строка.</param>
  /// <returns></returns>
  public static bool CheckString(string s)
  {
   Decimal res;
   return Decimal.TryParse(s.AsNumeric(),out res);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Преобразует Decimal в массив байт
  /// </summary>
  /// <param name="val">Значение типа Decimal</param>
  /// <returns></returns>
  public static byte[] DecimalToBytes(Decimal val)
  {
   int[] bits = Decimal.GetBits(val);
   byte[] buf = new byte[16];
   buf[0] = (byte)bits[0];
   buf[1] = (byte)(bits[0] >> 8);
   buf[2] = (byte)(bits[0] >> 0x10);
   buf[3] = (byte)(bits[0] >> 0x18);
   buf[4] = (byte)bits[1];
   buf[5] = (byte)(bits[1] >> 8);
   buf[6] = (byte)(bits[1] >> 0x10);
   buf[7] = (byte)(bits[1] >> 0x18);
   buf[8] = (byte)bits[2];
   buf[9] = (byte)(bits[2] >> 8);
   buf[10] = (byte)(bits[2] >> 0x10);
   buf[11] = (byte)(bits[2] >> 0x18);
   buf[12] = (byte)bits[3];
   buf[13] = (byte)(bits[3] >> 8);
   buf[14] = (byte)(bits[3] >> 0x10);
   buf[15] = (byte)(bits[3] >> 0x18);
   return buf;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Преобразует массив байт в Decimal
  /// </summary>
  /// <param name="buf">Массив байт</param>
  /// <returns></returns>
  public static Decimal DecimalFromBytes(byte[] buf)
  {
   int[] bits = new int[4];
   bits[0] = ((buf[0] | (buf[1] << 8)) | (buf[2] << 0x10)) | (buf[3] << 0x18);
   bits[1] = ((buf[4] | (buf[5] << 8)) | (buf[6] << 0x10)) | (buf[7] << 0x18);
   bits[2] = ((buf[8] | (buf[9] << 8)) | (buf[10] << 0x10)) | (buf[11] << 0x18);
   bits[3] = ((buf[12] | (buf[13] << 8)) | (buf[14] << 0x10)) | (buf[15] << 0x18);
   return new Decimal(bits);
  }
  #endregion Static Methods
  //-------------------------------------------------------------------------------------
  #region ISelfSerialization Members
  byte[] ISelfSerialization.GetSerializedData()
  {
   return DecimalToBytes(val);
  }
  //-------------------------------------------------------------------------------------
  void ISelfSerialization.Deserialize(byte[] data)
  {
   val = DecimalFromBytes(data);
  }
  ////-------------------------------------------------------------------------------------
  //int ISelfSerialization.GetSerializedDataSize()
  //{
  // return sizeof(decimal);
  //}

  #endregion
 }
 //**************************************************************************************
 /// <summary>
 /// Деньги c с большим числом знаков после запятой
 /// </summary>
 [Serializable]
 [TypeConverter(typeof(DecimalConverter))]
 public struct MoneyPrecise : IFormattable, IComparable, ISelfSerialization,
    IConvertible, IComparable<decimal>, IEquatable<decimal>, IComparable<MoneyPrecise>, IEquatable<MoneyPrecise>
 {
  private decimal val;

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Возвращает нулевую денежку
  /// </summary>
  public static MoneyPrecise Zero
  {
   get { return new MoneyPrecise(0); }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public MoneyPrecise(decimal val)
  {
   this.val = val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public MoneyPrecise(int val)
  {
   this.val = val;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region Object Members
  /// <summary>
  /// ToString
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return val.ToString("#,0.####");
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// GetHashCode()
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
  {
   return val.GetHashCode();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object obj)
  {
   return val.Equals(obj);
  }
  #endregion Object Members
  //-------------------------------------------------------------------------------------
  #region IFormattable Members
  /// <summary>
  /// ToString
  /// </summary>
  /// <param name="format"></param>
  /// <param name="formatProvider"></param>
  /// <returns></returns>
  public string ToString(string format, IFormatProvider formatProvider)
  {
   return val.ToString(format, formatProvider);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ToString
  /// </summary>
  /// <param name="format"></param>
  /// <returns></returns>
  public string ToString(string format)
  {
   return val.ToString(format);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IComparable Members
  int IComparable.CompareTo(object obj)
  {
   if(obj is decimal)
    return val.CompareTo(obj);
   if(obj is MoneyPrecise)
    return this.CompareTo((MoneyPrecise)obj);
   throw new Exception("IComparable.CompareTo(object obj) - obj должно быть типа decimal или MoneyPrecise!");
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// CompareTo
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(MoneyPrecise other)
  {
   return val.CompareTo(other.val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// CompareTo
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(decimal other)
  {
   return val.CompareTo(other);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IEquatable<decimal> Members
  /// <summary>
  /// 
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public bool Equals(decimal other)
  {
   return val.Equals(other);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public bool Equals(MoneyPrecise other)
  {
   return val.Equals(other.val);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IConvertible Members
  TypeCode IConvertible.GetTypeCode()
  {
   return val.GetTypeCode();
  }
  //-------------------------------------------------------------------------------------
  bool IConvertible.ToBoolean(IFormatProvider provider)
  {
   return ((IConvertible)val).ToBoolean(provider);
  }
  //-------------------------------------------------------------------------------------
  byte IConvertible.ToByte(IFormatProvider provider)
  {
   return ((IConvertible)val).ToByte(provider);
  }
  //-------------------------------------------------------------------------------------
  char IConvertible.ToChar(IFormatProvider provider)
  {
   return ((IConvertible)val).ToChar(provider);
  }
  //-------------------------------------------------------------------------------------
  DateTime IConvertible.ToDateTime(IFormatProvider provider)
  {
   return ((IConvertible)val).ToDateTime(provider);
  }
  //-------------------------------------------------------------------------------------
  decimal IConvertible.ToDecimal(IFormatProvider provider)
  {
   return ((IConvertible)val).ToDecimal(provider);
  }
  //-------------------------------------------------------------------------------------
  double IConvertible.ToDouble(IFormatProvider provider)
  {
   return ((IConvertible)val).ToDouble(provider);
  }
  //-------------------------------------------------------------------------------------
  short IConvertible.ToInt16(IFormatProvider provider)
  {
   return ((IConvertible)val).ToInt16(provider);
  }
  //-------------------------------------------------------------------------------------
  int IConvertible.ToInt32(IFormatProvider provider)
  {
   return ((IConvertible)val).ToInt32(provider);
  }
  //-------------------------------------------------------------------------------------
  long IConvertible.ToInt64(IFormatProvider provider)
  {
   return ((IConvertible)val).ToInt64(provider);
  }
  //-------------------------------------------------------------------------------------
  sbyte IConvertible.ToSByte(IFormatProvider provider)
  {
   return ((IConvertible)val).ToSByte(provider);
  }
  //-------------------------------------------------------------------------------------
  float IConvertible.ToSingle(IFormatProvider provider)
  {
   return ((IConvertible)val).ToSingle(provider);
  }
  //-------------------------------------------------------------------------------------
  string IConvertible.ToString(IFormatProvider provider)
  {
   return ((IConvertible)val).ToString(provider);
  }
  //-------------------------------------------------------------------------------------
  object IConvertible.ToType(Type conversionType, IFormatProvider provider)
  {
   if(conversionType == typeof(MoneyPrecise))
    return this;
   if(conversionType == typeof(Money))
    return new Money(val);
   return ((IConvertible)val).ToType(conversionType, provider);
  }
  //-------------------------------------------------------------------------------------
  ushort IConvertible.ToUInt16(IFormatProvider provider)
  {
   return ((IConvertible)val).ToUInt16(provider);
  }
  //-------------------------------------------------------------------------------------
  uint IConvertible.ToUInt32(IFormatProvider provider)
  {
   return ((IConvertible)val).ToUInt32(provider);
  }
  //-------------------------------------------------------------------------------------
  ulong IConvertible.ToUInt64(IFormatProvider provider)
  {
   return ((IConvertible)val).ToUInt64(provider);
  }
  //-------------------------------------------------------------------------------------
  #endregion
  //-------------------------------------------------------------------------------------
  #region Operations
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static implicit operator decimal(MoneyPrecise val)
  {
   return val.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static implicit operator Money(MoneyPrecise val)
  {
   return new Money(val.val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static implicit operator MoneyPrecise(int val)
  {
   return new MoneyPrecise(val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static implicit operator MoneyPrecise(decimal val)
  {
   return new MoneyPrecise(val);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator <(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val < arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator <=(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val <= arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator >(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val > arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator >=(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val >= arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator ==(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val == arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static bool operator !=(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val != arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// +
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator +(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val + arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// -
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator -(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val - arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// *
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator *(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val * arg2.val;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// /
  /// </summary>
  /// <param name="arg1"></param>
  /// <param name="arg2"></param>
  /// <returns></returns>
  public static decimal operator /(MoneyPrecise arg1, MoneyPrecise arg2)
  {
   return arg1.val / arg2.val;
  }
  #endregion Operations
  //-------------------------------------------------------------------------------------
  #region ISelfSerialization Members
  byte[] ISelfSerialization.GetSerializedData()
  {
   return Money.DecimalToBytes(val);
  }
  //-------------------------------------------------------------------------------------
  void ISelfSerialization.Deserialize(byte[] data)
  {
   val = Money.DecimalFromBytes(data);
  }
  #endregion

 }

}

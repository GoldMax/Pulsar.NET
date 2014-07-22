using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;



namespace Pulsar
{                
 /// <summary>              
 /// Структура количества.
 /// </summary>                       
 [Serializable]    
 [TypeConverter(typeof(AmountTypeConverter))]
 public struct Amount : IComparable, IConvertible, IFormattable, ISelfSerialization
 {
  private uint m;
  private static readonly System.Globalization.NumberFormatInfo NumberFormat = 
   System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет значение степени y в x*10^-y (количество знаков после точки).
  /// </summary>
  private uint Power
  {
   get { return m & 3; }
   set { m = (m & 0xFFFFFFFC) + value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Делитель 1/10^-y , где у - Power
  /// </summary>
  private uint Divisor
  {
   get
   {
    switch(m & 3)
    {
     case 0:
     return 1;
     case 1:
     return 10;
     case 2:
     return 100;
     case 3:
     return 1000;
     default:
     throw new Exception("Ошибка в показателе степени!");
    }
   }
   set
   {
    switch(value)
    {
     case 1:
     m = (m & 0xFFFFFFFC);
     break;
     case 10:
     m = (m & 0xFFFFFFFC) + 1;
     break;
     case 100:
     m = (m & 0xFFFFFFFC) + 2;
     break;
     case 1000:
     m = (m & 0xFFFFFFFC) + 3;
     break;
     default:
     throw new Exception("Ошибка в показателе степени!");
    }
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Максимальное значение для целого числа.
  /// </summary>
  public static Amount MaxValue0
  {
   get { return uint.MaxValue >> 2; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Максимальное значение при одном знаке после запятой.
  /// </summary>
  public static Amount MaxValue1
  {
   get { return ((decimal)(uint.MaxValue >> 2))/10; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Максимальное значение при двух знаках после запятой.
  /// </summary>
  public static Amount MaxValue2
  {
   get { return ((decimal)(uint.MaxValue >> 2))/100; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Максимальное значение при трех знаках после запятой.
  /// </summary>
  public static Amount MaxValue3
  {
   get { return ((decimal)(uint.MaxValue >> 2))/1000; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Возвращает нулевой объект
  /// </summary>
  public static Amount Empty
  {
   get { return new Amount(); }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет, является ли количество нулевым.
  /// </summary>
  public bool IsEmpty
  {
   get { return (m >> 2) == 0; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Amount(int val)
  {
   m = 0;
   if(val < 0)
    throw new Exception("Количество не может быть отрицательным!");

   if(val > 0x3FFFFFFF)
    throw new Exception("Переполнение значения!");
   m += (uint)(val << 2);
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Amount(uint val)
  {
   m = 0;
   if(val > 0x3FFFFFFF)
    throw new Exception("Переполнение значения!");
   m += val << 2;
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Amount(long val)
  {
   m = 0;
   if(val < 0)
    throw new Exception("Количество не может быть отрицательным!");
   if(val > 0x3FFFFFFF)
    throw new Exception("Переполнение значения!");
   m += (uint)val << 2;
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Amount(ulong val)
  {
   m = 0;
   if(val > 0x3FFFFFFF)
    throw new Exception("Переполнение значения!");
   m += (uint)val << 2;
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Amount(decimal val)
  {
   m = 0;
   if(val < 0)
    throw new Exception("Количество не может быть отрицательным!");
   if(val < 0.001m)
    return;

   int x = 1000 + (int)((val - (int)val) * 1000);
   if(x != 1000)
    if(x % 10 > 0)
     Power = 3;
    else if((x / 10) % 10 > 0)
     Power = 2;
    else
     Power = 1;
   if(val > ((decimal)0x3FFFFFFF) / Divisor)
    throw new Exception("Переполнение значения!");
   m += (uint)(val * Divisor) << 2;
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="val">Значение</param>
  /// <param name="quantum">Квант (точность)</param>
  public Amount(decimal val, Amount quantum)
  {
   m = 0;
   if(val < 0)
    throw new Exception("Количество не может быть отрицательным!");
   if(val < 0.001m)
    return;
   if(val > ((decimal)(uint.MaxValue >> 2)) / quantum.Divisor)
    throw new Exception("Переполнение значения!");

   uint res = (uint)(val * quantum.Divisor); // + ((uint)(val - (int)val) * 1000)/quantum.Divisor;
   Divisor = quantum.Divisor;
   // Усушка
   while(Divisor > 1)
   {
    if(res % 10 == 0)
    {
     res /= 10;
     Divisor /= 10;
    }
    else
     break;
   }
   if(res > (uint.MaxValue >> 2))
    throw new Exception("Переполнение значения!");
   m += (uint)(res << 2);
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="val">Значение</param>
  /// <param name="quantum">Квант (точность)</param>
  public Amount(Amount val, Amount quantum) : this((decimal)val,quantum)  {  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="val">Значение</param>
  /// <param name="quantum">Квант (точность)</param>
  public Amount(string val, Amount quantum) : this(Parse(val),quantum)  {  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public Amount(double val)
  {
   m = (new Amount((decimal)val)).m;
  }
  //-------------------------------------------------------------------------------------
  private Amount(uint val, uint div)
  {
   m = 0;
   // Усушка
   while(div > 1)
   {
    if(val % 10 == 0)
    {
     val /= 10;
     div /= 10;
    }
    else
     break;
   }
   if(val > (uint.MaxValue >> 2))
    throw new Exception("Переполнение значения!");
   Divisor = div;
   m += (uint)(val << 2);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// ToString
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return String.Format("{0}{1}", ((m >> 2) / Divisor), //.ToString("#,0"),   
     Divisor == 1 ?  "" : NumberFormat.NumberDecimalSeparator + 
     ((m >> 2)%Divisor).ToString(String.Format("D{0}", Power)));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// IFormattable.ToString
  /// </summary>
  /// <param name="format"></param>
  /// <param name="formatProvider"></param>
  /// <returns></returns>
  public string ToString(string format, IFormatProvider formatProvider = null)
  {
   return String.Format("{0}{1}", ((m >> 2) / Divisor).ToString(format),   
     Divisor == 1 ?  "" : NumberFormat.NumberDecimalSeparator + 
     ((m >> 2)%Divisor).ToString(String.Format("D{0}", Power)));
   
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// GetHashCode
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
  {
   return (int)m;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Equals
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object obj)
  {
   if(obj == null)
    return false;
   if(obj is Amount)
    return m.Equals(((Amount)obj).m);
   return false;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает объект в виде массива байт.
  /// </summary>
  /// <returns></returns>
  public byte[] ToBytes()
  {
   return BitConverter.GetBytes(m);
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  #region << Public Static Methods >>
  #pragma warning disable 1591
  /// <summary>
  /// Парсирует строку в количество.
  /// </summary>
  /// <param name="s">Парсируемая строка.</param>
  /// <returns></returns>
  public static Amount Parse(string s)
  {
   decimal d;
   if(Decimal.TryParse(s.AsNumeric(), out d) == false)
    throw new PulsarException("'{0}' не является вещественным числом!", s);
   return new Amount(d);
  }
  /// <summary>
  /// Парсирует строку в количество с заданной точностью.
  /// </summary>
  /// <param name="s">Парсируемая строка.</param>
  /// <param name="quantum">Квант (точность).</param>
  /// <returns></returns>
  public static Amount Parse(string s, Amount quantum)
  {
   decimal d;
   if(Decimal.TryParse(s.AsNumeric(), out d) == false)
    throw new PulsarException("'{0}' не является вещественным числом!", s);
   return new Amount(d, quantum);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Парсирует строку в количество.
  /// </summary>
  /// <param name="s">Парсируемая строка.</param>
  /// <param name="res">Результат парсинга.</param>
  /// <returns></returns>
  public static bool TryParse(string s, out Amount res)
  {
   res = Amount.Empty;
   if(s == null)
    return false;
   decimal d;
   if(Decimal.TryParse(s.AsNumeric(), out d) == false)
    return false;
   res = new Amount(d);
   return true;
  }
  /// <summary>
  /// Парсирует строку в количество с заданной точностью.
  /// </summary>
  /// <param name="s">Парсируемая строка.</param>
  /// <param name="quantum">Квант (точность).</param>
  /// <returns></returns>
  /// <param name="res">Результат парсинга.</param>
  public static bool TryParse(string s, Amount quantum, out Amount res)
  {
   res = Amount.Empty;
   decimal d;
   if(Decimal.TryParse(s.AsNumeric(), out d) == false)
    return false;
   res = new Amount(d, quantum);
   return true;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Проверяет строку на соответствие ее числу.
  /// </summary>
  /// <param name="s">Проверяемая строка.</param>
  /// <returns></returns>
  public static bool CheckString(string s)
  {
   decimal d;
   return Decimal.TryParse(s.AsNumeric(), out d);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Создает объект из массива байт.
  /// </summary>
  /// <param name="bytes"></param>
  /// <returns></returns>
  public static Amount FromBytes(byte[] bytes)
  {
   return new Amount { m = BitConverter.ToUInt32(bytes,0) };
  }
  //-------------------------------------------------------------------------------------
  #region (Amount)T
  public static implicit operator Amount(byte val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(sbyte val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(short val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(ushort val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(int val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(uint val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(long val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(ulong val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(decimal val)
  {
   return new Amount(val);
  }
  public static implicit operator Amount(double val)
  {
   return new Amount(val);
  } 
  #endregion (Amount)
  //-------------------------------------------------------------------------------------
  #region (T)Amount
  public static explicit operator byte(Amount a)
  {
   return (byte)((a.m >> 2)/a.Divisor);
  }
  public static explicit operator sbyte(Amount a)
  {
   return (sbyte)((a.m >> 2)/a.Divisor);
  }
  public static explicit operator short(Amount a)
  {
   return (short)((a.m >> 2)/a.Divisor);
  }
  public static explicit operator ushort(Amount a)
  {
   return (ushort)((a.m >> 2)/a.Divisor);
  }
  public static explicit operator int(Amount a)
  {
   return (int)((a.m >> 2)/a.Divisor);
  }
  public static explicit operator uint(Amount a)
  {
   return (a.m >> 2)/a.Divisor;
  }
  public static explicit operator long(Amount a)
  {
   return (long)((a.m >> 2)/a.Divisor);
  }
  public static explicit operator ulong(Amount a)
  {
   return (ulong)((a.m >> 2)/a.Divisor);
  }
  public static explicit operator decimal(Amount a)
  {
   return ((decimal)(a.m >> 2))/a.Divisor;
  }
  public static explicit operator double(Amount a)
  {
   return ((double)(a.m >> 2))/a.Divisor;
  } 
  #endregion (T)Amount
  //-------------------------------------------------------------------------------------
  #region ==, != , > ...
  public static bool operator ==(Amount a, Amount b)
  {
   return a.m == b.m;
  }
  public static bool operator !=(Amount a, Amount b)
  {
   return a.m != b.m;
  }
  public static bool operator >(Amount a, Amount b)
  {
   uint x = a.Divisor > b.Divisor ? a.Divisor : b.Divisor;
   return (a.m >> 2) * (x/a.Divisor) > (b.m >> 2) * (x/b.Divisor);
  }
  public static bool operator <(Amount a, Amount b)
  {
   uint x = a.Divisor > b.Divisor ? a.Divisor : b.Divisor;
   return (a.m >> 2) * (x/a.Divisor) < (b.m >> 2) * (x/b.Divisor);
  }
  public static bool operator >=(Amount a, Amount b)
  {
   uint x = a.Divisor > b.Divisor ? a.Divisor : b.Divisor;
   return (a.m >> 2) * (x/a.Divisor) >= (b.m >> 2) * (x/b.Divisor);
  }
  public static bool operator <=(Amount a, Amount b)
  {
   uint x = a.Divisor > b.Divisor ? a.Divisor : b.Divisor;
   return (a.m >> 2) * (x/a.Divisor) <= (b.m >> 2) * (x/b.Divisor);
  } 
  #endregion ==, != , > ...
  //-------------------------------------------------------------------------------------
  #region +
  public static Amount operator +(Amount a, Amount b)
  {
   uint x = a.Divisor > b.Divisor ? a.Divisor : b.Divisor;
   return new Amount((a.m >> 2) * (x/a.Divisor) + (b.m >> 2) * (x/b.Divisor), x);
  }
  public static byte operator +(Amount a, byte b)
  {
   return (byte)((a.m >> 2)/a.Divisor + b);
  }
  public static byte operator +(byte b, Amount a)
  {
   return (byte)((a.m >> 2)/a.Divisor + b);
  }
  public static sbyte operator +(Amount a, sbyte b)
  {
   return (sbyte)((a.m >> 2)/a.Divisor + b);
  }
  public static sbyte operator +(sbyte b, Amount a)
  {
   return (sbyte)((a.m >> 2)/a.Divisor + b);
  }
  public static short operator +(Amount a, short b)
  {
   return (short)((a.m >> 2)/a.Divisor + b);
  }
  public static short operator +(short b, Amount a)
  {
   return (short)((a.m >> 2)/a.Divisor + b);
  }
  public static ushort operator +(Amount a, ushort b)
  {
   return (ushort)((a.m >> 2)/a.Divisor + b);
  }
  public static ushort operator +(ushort b, Amount a)
  {
   return (ushort)((a.m >> 2)/a.Divisor + b);
  }
  public static int operator +(Amount a, int b)
  {
   return (int)((a.m >> 2)/a.Divisor + b);
  }
  public static int operator +(int b, Amount a)
  {
   return (int)((a.m >> 2)/a.Divisor + b);
  }
  public static uint operator +(Amount a, uint b)
  {
   return (uint)((a.m >> 2)/a.Divisor + b);
  }
  public static uint operator +(uint b, Amount a)
  {
   return (uint)((a.m >> 2)/a.Divisor + b);
  }
  public static long operator +(Amount a, long b)
  {
   return (long)((a.m >> 2)/a.Divisor + b);
  }
  public static long operator +(long b, Amount a)
  {
   return (long)((a.m >> 2)/a.Divisor + b);
  }
  public static ulong operator +(Amount a, ulong b)
  {
   return (ulong)((a.m >> 2)/a.Divisor + b);
  }
  public static ulong operator +(ulong b, Amount a)
  {
   return (ulong)((a.m >> 2)/a.Divisor + b);
  }
  public static decimal operator +(Amount a, decimal b)
  {
   return (decimal)a + b;
  }
  public static decimal operator +(decimal b, Amount a)
  {
   return (decimal)a + b;
  }
  public static double operator +(Amount a, double b)
  {
   return (double)a + b;
  }
  public static double operator +(double b, Amount a)
  {
   return (double)a + b;
  } 
  #endregion +
  //-------------------------------------------------------------------------------------
  #region -
  public static Amount operator -(Amount a, Amount b)
  {
   uint x = a.Divisor > b.Divisor ? a.Divisor : b.Divisor;
   if((a.m >> 2) * (x/a.Divisor) < (b.m >> 2) * (x/b.Divisor))
    throw new Exception("В результате вычитания количеств получается отрицательное число!");
   return new Amount((a.m >> 2) * (x/a.Divisor) - (b.m >> 2) * (x/b.Divisor), x);
  }
  public static byte operator -(Amount a, byte b)
  {
   return (byte)((a.m >> 2)/a.Divisor - b);
  }
  public static byte operator -(byte b, Amount a)
  {
   return (byte)(b - (a.m >> 2)/a.Divisor);
  }
  public static sbyte operator -(Amount a, sbyte b)
  {
   return (sbyte)((a.m >> 2)/a.Divisor - b);
  }
  public static sbyte operator -(sbyte b, Amount a)
  {
   return (sbyte)(b - (a.m >> 2)/a.Divisor);
  }
  public static short operator -(Amount a, short b)
  {
   return (short)((a.m >> 2)/a.Divisor - b);
  }
  public static short operator -(short b, Amount a)
  {
   return (short)(b - (a.m >> 2)/a.Divisor);
  }
  public static ushort operator -(Amount a, ushort b)
  {
   return (ushort)((a.m >> 2)/a.Divisor - b);
  }
  public static ushort operator -(ushort b, Amount a)
  {
   return (ushort)(b - (a.m >> 2)/a.Divisor);
  }
  public static int operator -(Amount a, int b)
  {
   return (int)((a.m >> 2)/a.Divisor - b);
  }
  public static int operator -(int b, Amount a)
  {
   return (int)(b - (a.m >> 2)/a.Divisor);
  }
  public static uint operator -(Amount a, uint b)
  {
   return (uint)((a.m >> 2)/a.Divisor - b);
  }
  public static uint operator -(uint b, Amount a)
  {
   return (uint)(b - (a.m >> 2)/a.Divisor);
  }
  public static long operator -(Amount a, long b)
  {
   return (long)((a.m >> 2)/a.Divisor - b);
  }
  public static long operator -(long b, Amount a)
  {
   return (long)(b - (a.m >> 2)/a.Divisor);
  }
  public static ulong operator -(Amount a, ulong b)
  {
   return (ulong)((a.m >> 2)/a.Divisor - b);
  }
  public static ulong operator -(ulong b, Amount a)
  {
   return (ulong)(b - (a.m >> 2)/a.Divisor);
  }
  public static decimal operator -(Amount a, decimal b)
  {
   return (decimal)a - b;
  }
  public static decimal operator -(decimal b, Amount a)
  {
   return b - (decimal)a;
  }
  public static double operator -(Amount a, double b)
  {
   return (double)a - b;
  }
  public static double operator -(double b, Amount a)
  {
   return b - (double)a;
  } 
  #endregion -
  //-------------------------------------------------------------------------------------
  public static Amount operator /(Amount a, Amount b)
  {
   return new Amount((decimal)a / (decimal)b);
  }
  public static Amount operator *(Amount a, Amount b)
  {
   return new Amount((decimal)a * (decimal)b);
  }
  #pragma warning restore 1591
  #endregion << Public Static Methods >>
  //-------------------------------------------------------------------------------------
  #region IComparable Members
  /// <summary>
  /// CompareTo
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public int CompareTo(object obj)
  {
   if(obj == null)
    return 1;
   if(obj is Amount)
    if((Amount)obj == this)
     return 0;
    else
     return this > (Amount)obj ? 1 : -1;
   return this.ToString().CompareTo(obj.ToString());
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IConvertible Members
  TypeCode IConvertible.GetTypeCode()
  {
   return TypeCode.Object;
  }
  bool IConvertible.ToBoolean(IFormatProvider provider)
  {
   return m != 0;
  }
  byte IConvertible.ToByte(IFormatProvider provider)
  {
   return (byte)this;
  }
  char IConvertible.ToChar(IFormatProvider provider)
  {
   throw new NotImplementedException("IConvertible.ToChar");
  }
  DateTime IConvertible.ToDateTime(IFormatProvider provider)
  {
   throw new NotImplementedException("IConvertible.ToDateTime");
  }
  decimal IConvertible.ToDecimal(IFormatProvider provider)
  {
   return (decimal)this;
  }
  double IConvertible.ToDouble(IFormatProvider provider)
  {
   return (double)this;
  }
  short IConvertible.ToInt16(IFormatProvider provider)
  {
   return (short)this;
  }
  int IConvertible.ToInt32(IFormatProvider provider)
  {
   return (int)this;
  }
  long IConvertible.ToInt64(IFormatProvider provider)
  {
   return (long)this;
  }
  sbyte IConvertible.ToSByte(IFormatProvider provider)
  {
   return (sbyte)this;
  }
  float IConvertible.ToSingle(IFormatProvider provider)
  {
   throw new NotImplementedException("IConvertible.ToSingle");
  }
  string IConvertible.ToString(IFormatProvider provider)
  {
   return this.ToString();
  }
  object IConvertible.ToType(Type targetType, IFormatProvider provider)
  {
   if(targetType == null)
    throw new ArgumentNullException("targetType");
   if(this.GetType() == targetType)
    return this;
   if(targetType == typeof(Boolean))
    return ((IConvertible)this).ToBoolean(provider);
   if(targetType == typeof(SByte))
    return ((IConvertible)this).ToSByte(provider);
   if(targetType == typeof(Byte))
    return ((IConvertible)this).ToByte(provider);
   if(targetType == typeof(Int16))
    return ((IConvertible)this).ToInt16(provider);
   if(targetType == typeof(UInt16))
    return ((IConvertible)this).ToUInt16(provider);
   if(targetType == typeof(Int32))
    return ((IConvertible)this).ToInt32(provider);
   if(targetType == typeof(UInt32))
    return ((IConvertible)this).ToUInt32(provider);
   if(targetType == typeof(Int64))
    return ((IConvertible)this).ToInt64(provider);
   if(targetType == typeof(UInt64))
    return ((IConvertible)this).ToUInt64(provider);
   if(targetType == typeof(Double))
    return ((IConvertible)this).ToDouble(provider);
   if(targetType == typeof(Decimal))
    return ((IConvertible)this).ToDecimal(provider);
   if(targetType == typeof(String))
    return ((IConvertible)this).ToString(provider);
   throw new InvalidCastException();
  }
  ushort IConvertible.ToUInt16(IFormatProvider provider)
  {
   return (ushort)this;
  }
  uint IConvertible.ToUInt32(IFormatProvider provider)
  {
   return (uint)this;
  }
  ulong IConvertible.ToUInt64(IFormatProvider provider)
  {
   return (ulong)this;
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region ISelfSerialization Members
  byte[] ISelfSerialization.GetSerializedData()
  {
   return BitConverter.GetBytes(m);
  }
  //-------------------------------------------------------------------------------------
  void ISelfSerialization.Deserialize(byte[] data)
  {
   m = BitConverter.ToUInt32(data,0);
  }
  #endregion

 }
 //*************************************************************************************
 /// <summary>
 /// TypeConverter для Amoun
 /// </summary>
 public class AmountTypeConverter : TypeConverter
 {
  /// <summary>
  /// CanConvertFrom
  /// </summary>
  /// <param name="context"></param>
  /// <param name="sourceType"></param>
  /// <returns></returns>
  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
   if(sourceType == typeof(String))
    return true;
   return base.CanConvertFrom(context, sourceType);
  }
  /// <summary>
  ///  
  /// </summary>
  /// <param name="context"></param>
  /// <param name="culture"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
  {
   if(value is string)
   {
    Amount a;
    if(Amount.TryParse(((string)value).AsNumeric(), out a))
     return a;
   }
   return base.ConvertFrom(context, culture, value);
  }
  /// <summary>
  /// 
  /// </summary>
  /// <param name="context"></param>
  /// <param name="destinationType"></param>
  /// <returns></returns>
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
   if(destinationType == typeof(string))
    return true;
   return base.CanConvertTo(context, destinationType);
  }
  /// <summary>
  /// 
  /// </summary>
  /// <param name="context"></param>
  /// <param name="culture"></param>
  /// <param name="value"></param>
  /// <param name="destinationType"></param>
  /// <returns></returns>
  public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
                                     object value, Type destinationType)
  {
   if(value is Amount && destinationType == typeof(string))
    return ((Amount)value).ToString();
   return base.ConvertTo(context, culture, value, destinationType);
  }
 }
}

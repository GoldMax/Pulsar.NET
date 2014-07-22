using System;
using System.Collections.Generic;
using System.Text;

namespace Pulsar.SQL
{
 /// <summary>
 /// Структура представления SQL типа TimeStamp.
 /// </summary>
 #region public struct SqlTimeStamp : IComparable, IComparable<SqlTimeStamp>
 public struct SqlTimeStamp : IComparable, IComparable<SqlTimeStamp>, ISelfSerialization
 {
  /// <summary>
  /// Возвращает пустой экземпляр.
  /// </summary>
  public static readonly SqlTimeStamp Empty = new SqlTimeStamp();

  private ulong value;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Возвращает или устанавливает значение объекта.
  /// </summary>
  public ulong Value
  {
   get { return value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Возвращает максимально возможное значение объекта.
  /// </summary>
  public ulong MaxValue
  {
   get { return ulong.MaxValue; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Возвращает минимально возможное значение объекта.
  /// </summary>
  public ulong MinValue
  {
   get { return ulong.MinValue; }
  }
  #endregion << Properties >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор для Int32.
  /// </summary>
  public SqlTimeStamp(Int32 val)
  {
   value = (ulong)val;
  }
  /// <summary>
  /// Конструктор для UInt32.
  /// </summary>
  public SqlTimeStamp(UInt32 val)
  {
   value = (ulong)val;
  }
  /// <summary>
  /// Конструктор для Int64.
  /// </summary>
  public SqlTimeStamp(Int64 val)
  {
   value = (ulong)val;
  }
  /// <summary>
  /// Конструктор для UInt64.
  /// </summary>
  public SqlTimeStamp(UInt64 val)
  {
   value = val;
  }
  /// <summary>
  /// Конструктор для SqlTimeStamp.
  /// </summary>
  public SqlTimeStamp(SqlTimeStamp val)
  {
   value = val.value;
  }
  /// <summary>
  /// Конструктор для byte[].
  /// </summary>
  public SqlTimeStamp(byte[] val)
  {
   value = value = SqlTimeStamp.FromByteArray(val).Value;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region IComparable Members
  /// <summary>
  /// Сравнивает текущий объект с указанным.
  /// </summary>
  /// <param name="obj">Объект для сравнения.</param>
  /// <returns></returns>
  public int CompareTo(object obj)
  {
   if (obj.GetType() == typeof(SqlTimeStamp))
    return value.CompareTo(((SqlTimeStamp)obj).value);
   else if (obj.GetType() == typeof(long) || obj.GetType() == typeof(ulong))
    return value.CompareTo((ulong)obj);
   else
    throw new ArgumentException("Не верный тип параметра.", "obj");
  }
  /// <summary>
  /// Сравнивает текущий объект с указанным.
  /// </summary>
  /// <param name="other">Объект для сравнения.</param>
  /// <returns></returns>
  public int CompareTo(SqlTimeStamp other)
  {
   return value.CompareTo(((SqlTimeStamp)other).value);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region Опреаторы
  /// <summary>
  /// Приведение типа Int64 к SqlTimeStamp.
  /// </summary>
  /// <param name="value">Приводимое значение.</param>
  /// <returns></returns>
  public static explicit operator SqlTimeStamp(long value)
  {
   return new SqlTimeStamp(value);
  }
  /// <summary>
  /// Приведение типа UInt64 к SqlTimeStamp.
  /// </summary>
  /// <param name="value">Приводимое значение.</param>
  /// <returns></returns>
  public static explicit operator SqlTimeStamp(ulong value)
  {
   return new SqlTimeStamp(value);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Приведение типа SqlTimeStamp к Int64.
  /// </summary>
  /// <param name="value">Приводимое значение.</param>
  /// <returns></returns>
  public static explicit operator Int64(SqlTimeStamp value)
  {
   return (Int64)value.value;
  }
  /// <summary>
  /// Приведение типа SqlTimeStamp к UInt64.
  /// </summary>
  /// <param name="value">Приводимое значение.</param>
  /// <returns></returns>
  public static explicit operator UInt64(SqlTimeStamp value)
  {
   return (UInt64)value.value;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ==
  /// </summary>
  /// <param name="st1"></param>
  /// <param name="st2"></param>
  /// <returns></returns>
  public static bool operator == (SqlTimeStamp st1, SqlTimeStamp st2)
  {
   return st1.value == st2.value;
  }
  /// <summary>
  /// !=
  /// </summary>
  /// <param name="st1"></param>
  /// <param name="st2"></param>
  /// <returns></returns>
  public static bool operator != (SqlTimeStamp st1, SqlTimeStamp st2)
  {
   return st1.value != st2.value;
  }
  #endregion Опреаторы
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Возвращает строку, предвтавляющую значение объекта.
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return "0x" + value.ToString("X16");
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// GetHashCode()
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
  {
   return value.GetHashCode();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Equals
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object obj)
  {
   if(obj != null && obj is SqlTimeStamp)
    return value == ((SqlTimeStamp)obj).value;
   return false;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает представление объекта SqlTimeStamp в виде массива.
  /// </summary>
  /// <param name="value">Исходный объект.</param>
  /// <returns></returns>
  public static byte[] ToByteArray(SqlTimeStamp value)
  {
   byte[] res = new byte[8];
   for(int i = 7; i >= 0; i--)
   {
    res[i] = (byte)(value.value & 0xff);
    value.value = (value.value >> 8);
   }
   return res;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Создает и возвращает объект SqlTimeStamp из массива.
  /// </summary>
  /// <param name="array">Исходный массив.</param>
  /// <returns></returns>
  public static SqlTimeStamp FromByteArray(byte[] array)
  {
   SqlTimeStamp res = new SqlTimeStamp(0);
   if(array.Length == 0)
    return res;
   if(array.Length != 8)
    throw new ArgumentException("SqlTimeStamp.FromByteArray: Размер массива не равен 8.", "array");
   for(int i = 0; i < 8; i++)
    res.value = (res.value << 8) + (ulong)array[i];
   return res;
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  #region ISelfSerialization Members
  byte[] ISelfSerialization.GetSerializedData()
  {
   return ToByteArray(this);
  }
  //-------------------------------------------------------------------------------------
  void ISelfSerialization.Deserialize(byte[] data)
  {
   value = FromByteArray(data).value;
  }
  #endregion
 }
 #endregion public struct SqlTimeStamp : IComparable, IComparable<SqlTimeStamp>
}

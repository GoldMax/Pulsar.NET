using System;
using System.Collections.Generic;
using System.Text;

namespace Pulsar.SQL
{
 /// <summary>
 /// ��������� ������������� SQL ���� TimeStamp.
 /// </summary>
 #region public struct SqlTimeStamp : IComparable, IComparable<SqlTimeStamp>
 public struct SqlTimeStamp : IComparable, IComparable<SqlTimeStamp>, ISelfSerialization
 {
  /// <summary>
  /// ���������� ������ ���������.
  /// </summary>
  public static readonly SqlTimeStamp Empty = new SqlTimeStamp();

  private ulong value;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// ���������� ��� ������������� �������� �������.
  /// </summary>
  public ulong Value
  {
   get { return value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ���������� ����������� ��������� �������� �������.
  /// </summary>
  public ulong MaxValue
  {
   get { return ulong.MaxValue; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ���������� ���������� ��������� �������� �������.
  /// </summary>
  public ulong MinValue
  {
   get { return ulong.MinValue; }
  }
  #endregion << Properties >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// ����������� ��� Int32.
  /// </summary>
  public SqlTimeStamp(Int32 val)
  {
   value = (ulong)val;
  }
  /// <summary>
  /// ����������� ��� UInt32.
  /// </summary>
  public SqlTimeStamp(UInt32 val)
  {
   value = (ulong)val;
  }
  /// <summary>
  /// ����������� ��� Int64.
  /// </summary>
  public SqlTimeStamp(Int64 val)
  {
   value = (ulong)val;
  }
  /// <summary>
  /// ����������� ��� UInt64.
  /// </summary>
  public SqlTimeStamp(UInt64 val)
  {
   value = val;
  }
  /// <summary>
  /// ����������� ��� SqlTimeStamp.
  /// </summary>
  public SqlTimeStamp(SqlTimeStamp val)
  {
   value = val.value;
  }
  /// <summary>
  /// ����������� ��� byte[].
  /// </summary>
  public SqlTimeStamp(byte[] val)
  {
   value = value = SqlTimeStamp.FromByteArray(val).Value;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region IComparable Members
  /// <summary>
  /// ���������� ������� ������ � ���������.
  /// </summary>
  /// <param name="obj">������ ��� ���������.</param>
  /// <returns></returns>
  public int CompareTo(object obj)
  {
   if (obj.GetType() == typeof(SqlTimeStamp))
    return value.CompareTo(((SqlTimeStamp)obj).value);
   else if (obj.GetType() == typeof(long) || obj.GetType() == typeof(ulong))
    return value.CompareTo((ulong)obj);
   else
    throw new ArgumentException("�� ������ ��� ���������.", "obj");
  }
  /// <summary>
  /// ���������� ������� ������ � ���������.
  /// </summary>
  /// <param name="other">������ ��� ���������.</param>
  /// <returns></returns>
  public int CompareTo(SqlTimeStamp other)
  {
   return value.CompareTo(((SqlTimeStamp)other).value);
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region ���������
  /// <summary>
  /// ���������� ���� Int64 � SqlTimeStamp.
  /// </summary>
  /// <param name="value">���������� ��������.</param>
  /// <returns></returns>
  public static explicit operator SqlTimeStamp(long value)
  {
   return new SqlTimeStamp(value);
  }
  /// <summary>
  /// ���������� ���� UInt64 � SqlTimeStamp.
  /// </summary>
  /// <param name="value">���������� ��������.</param>
  /// <returns></returns>
  public static explicit operator SqlTimeStamp(ulong value)
  {
   return new SqlTimeStamp(value);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ���������� ���� SqlTimeStamp � Int64.
  /// </summary>
  /// <param name="value">���������� ��������.</param>
  /// <returns></returns>
  public static explicit operator Int64(SqlTimeStamp value)
  {
   return (Int64)value.value;
  }
  /// <summary>
  /// ���������� ���� SqlTimeStamp � UInt64.
  /// </summary>
  /// <param name="value">���������� ��������.</param>
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
  #endregion ���������
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// ���������� ������, �������������� �������� �������.
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
  /// ���������� ������������� ������� SqlTimeStamp � ���� �������.
  /// </summary>
  /// <param name="value">�������� ������.</param>
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
  /// ������� � ���������� ������ SqlTimeStamp �� �������.
  /// </summary>
  /// <param name="array">�������� ������.</param>
  /// <returns></returns>
  public static SqlTimeStamp FromByteArray(byte[] array)
  {
   SqlTimeStamp res = new SqlTimeStamp(0);
   if(array.Length == 0)
    return res;
   if(array.Length != 8)
    throw new ArgumentException("SqlTimeStamp.FromByteArray: ������ ������� �� ����� 8.", "array");
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

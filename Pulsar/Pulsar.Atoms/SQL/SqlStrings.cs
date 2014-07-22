using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Pulsar.SQL
{
 /// <summary>
 /// Класс конвертирования значения объектов обычных типов к строковым значениям SQL типов.
 /// </summary>
 public static class SqlStrings
 {
  /// <summary>
  /// Конвертирует значение переменной типа bool в строковое представление значения bit.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlBit(bool value)
  {
   return String.Format("{0}", value ? 1 : 0);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной типа string в строковую константу SQL.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlString(string value)
  {
   if(value == null)
    return "NULL";
   return String.Format("'{0}'", value.Replace("'","''"));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной типа DateTime в строковое представление значения datetime.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlDateTime(DateTime value)
  {
   return ToSqlDateTime(value, false);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной типа DateTime в строковое представление значения datetime.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <param name="noTime">Если true, возвращает строку, не содержащую время.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlDateTime(DateTime value, bool noTime)
  {
   if(noTime)
    return String.Format("'{0}'", value.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo));
   else
    return String.Format("'{0}'", value.ToString("yyyyMMdd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной типа byte[] в строковое представление значения binary.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <param name="length">Количество конвертируемых байт массива.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlByteArray(byte[] value, long length)
  {
   StringBuilder res = new StringBuilder(value.Length*2+4);
   res.Append("0x");
   for(long a = 0; a < length; a++)
    res.Append(value[a].ToString("x02"));
   return res.ToString(); 
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной типа Guid в строковое представление значения uniqueidentifier.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlUniqueIdentifier(Guid value)
  {
   return String.Format("'{0}'", value.ToString());
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной типа Decimal в строковое представление значения Decimal.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlDecimal(Decimal value)
  {
   return String.Format("{0}", value.ToString((new CultureInfo("en-US")).NumberFormat));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной в строковое представление SQL значения.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <param name="length">Количество конвертируемых байт (для массива).</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlValue(object value, long? length = null)
  {
   if (value == null || value.GetType() == typeof(DBNull))
    return "NULL";
   else if (value.GetType() == typeof(DateTime))
    return ToSqlDateTime((DateTime)value);
   else if (value.GetType() == typeof(string))
    return ToSqlString((string)value);
   else if (value.GetType() == typeof(bool))
    return ToSqlBit((bool)value);
   else if (value.GetType() == typeof(byte[]))
    return ToSqlByteArray((byte[])value, length ?? ((byte[])value).Length); 
   else if (value.GetType() == typeof(Guid))
    return ToSqlUniqueIdentifier((Guid)value);
   else if(value.GetType() == typeof(Decimal))
    return ToSqlDecimal((Decimal)value);  
   else if(value.GetType() == typeof(SqlTimeStamp))
    return ((SqlTimeStamp)value).ToString();  
   else
    return Convert.ToString(value,(new CultureInfo("en-US")).NumberFormat); 
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конвертирует значение переменной типа DateTime в строковое представление значения datetime типа 'ГГГГММДД'.
  /// </summary>
  /// <param name="value">Конвертируемое значение.</param>
  /// <returns>Строковое представление значения в SQL формате.</returns>
  public static string ToSqlDate(DateTime value)
  {
   return String.Format("'{0}'", value.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Преобразует струку типа 'ГГГГММДД' к DateTime.
  /// </summary>
  /// <param name="value">Преобразуемая строка.</param>
  /// <returns></returns>
  public static DateTime FromSqlDate(string value)
  {
   return new DateTime(Int32.Parse(value.Substring(0,4)),
                       Int32.Parse(value.Substring(4,2)),
                       Int32.Parse(value.Substring(6,2)));   
  }
 }
}

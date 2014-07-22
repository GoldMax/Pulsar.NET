using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Класс строки, являющейся объектом.
 /// </summary>
 [TypeConverter(typeof(RefStringTypeConverter))]
 public class RefString : ObjectChangeNotify
 {
  private string str = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Значение сроки.
  /// </summary>
  public string Value
  {
   get { return str; }
   set 
   {
    lock(str == null ? (object)this : (object)str)
    {
     OnObjectChanging("Value", value, str);
     var bk = str;
     str = value; 
     OnObjectChanged("Value", str, bk);
    }
   }
  }

  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public RefString() { }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public RefString(string s) : this()
  {
   str = s;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Неявное преобразование к String
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static explicit operator String(RefString str)
  {
   return str.str;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Неявное преобразование к RefString
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static explicit operator RefString(String str)
  {
   return new RefString(str);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ToString()
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return str;
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
 }
 //*************************************************************************************
 internal class RefStringTypeConverter : TypeConverter
 {
  /// <summary>
  /// 
  /// </summary>
  /// <param name="context"></param>
  /// <param name="sourceType"></param>
  /// <returns></returns>
  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
   if(sourceType == typeof(string))
    return true;
   return base.CanConvertFrom(context, sourceType);
  }
  //-------------------------------------------------------------------------------------
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
   if(destinationType == typeof(string))
    return true;
   return base.CanConvertTo(context, destinationType);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="context"></param>
  /// <param name="culture"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  public override object ConvertFrom(ITypeDescriptorContext context, 
                                     System.Globalization.CultureInfo culture, object value)
  {
   if(value != null && value is string)
    return new RefString((string)value);
   return base.ConvertFrom(context, culture, value);
  }
  //-------------------------------------------------------------------------------------
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
   if(value != null && value is RefString && destinationType == typeof(string))
    return ((RefString)value).Value;
   return base.ConvertTo(context, culture, value, destinationType);
  }
 }

}

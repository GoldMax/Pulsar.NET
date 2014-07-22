using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pulsar.Reflection
{
 /// <summary>
 /// Класс конвертера значений типа Bool
 /// </summary>
 public class BooleanTypeConverter : BooleanConverter
 {
  /// <summary>
  /// ConvertTo
  /// </summary>
  /// <param name="context"></param>
  /// <param name="culture"></param>
  /// <param name="value"></param>
  /// <param name="destType"></param>
  /// <returns></returns>
  public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
  {
   return (bool)value ? "Да" : "Нет";
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ConvertFrom
  /// </summary>
  /// <param name="context"></param>
  /// <param name="culture"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  public override object ConvertFrom(ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
   return (string)value == "Да";
  }
 }
}

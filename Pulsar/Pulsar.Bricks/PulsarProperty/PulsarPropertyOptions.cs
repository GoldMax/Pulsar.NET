using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar
{
 //**************************************************************************************
 #region << public enum StockPropertyOptions : byte >>
 /// <summary>
 /// Перечисление параметров свойств.
 /// </summary>
 [Flags]
 [TypeConverter(typeof(EnumTypeConverter))]
 public enum PulsarPropertyOptions : byte
 {
  /// <summary>
  /// Нет параметров (обычное свойство)
  /// </summary>
  [EnumItemDisplayName("Нет параметров")]
  None = 0,
  /// <summary>
  /// Обязательное к установке значения
  /// </summary>
  [EnumItemDisplayName("Обязательное")]
  Imperative = 1,
  /// <summary>
  /// Значением свойства является список.
  /// </summary>
  [EnumItemDisplayName("Множественное значение")]
  IsListValue = 2,
  /// <summary>
  /// Групповое свойство 
  /// </summary>
  [EnumItemDisplayName("Групповое")]
  PropsValuesGroup = 32,
  /// <summary>
  /// Нестандартная установка значения 
  /// </summary>
  [EnumItemDisplayName("Нестандартное")]
  NonStandart = 64,
  /// <summary>
  /// Специальное (предустановленное) свойство.
  /// </summary>
  [EnumItemDisplayName("Специальное")]
  Special = 128
 }
 #endregion << public enum StockPropertyOptions : byte >>
 //**************************************************************************************

}

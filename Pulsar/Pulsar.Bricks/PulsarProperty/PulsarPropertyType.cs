using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
 //**************************************************************************************
 #region << public enum PulsarPropertyType : byte >>
 /// <summary>
 /// Перечисление типов свойств.
 /// </summary>
 [Serializable]
 public enum PulsarPropertyType : byte
 {
  /// <summary>
  /// Неизвестный тип.
  /// </summary>
  Unknown = 0,
  /// <summary>
  /// Булевый тип свойства.
  /// </summary>
  Check = 1,
  /// <summary>
  /// Целый тип свойства.
  /// </summary>
  Integer = 2,
  /// <summary>
  /// Вещественный тип свойства.
  /// </summary>
  Decimal = 3,
  /// <summary>
  /// Строковый тип свойства
  /// </summary>
  String = 4,
  /// <summary>
  /// Перечисляемый тип свойства.
  /// </summary>
  Enum = 5,
  /// <summary>
  /// Ресурс
  /// </summary>
  Resource = 6
 }
 #endregion << public enum PulsarPropertyType : byte >>

}

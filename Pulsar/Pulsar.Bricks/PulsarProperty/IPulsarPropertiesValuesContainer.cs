using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Интерфейс объектов, описываемых свойствами Пульсара.
 /// </summary>
 public interface IPulsarPropertiesValuesContainer 
 {
  /// <summary>
  /// Коллекция собственных значений свойств.
  /// </summary>
  IPulsarPropertiesValuesCollection PropertiesValues { get; }
  ///// <summary>
  ///// Перечисление расчетных значений свойств
  ///// </summary>
  //IEnumerable<KeyValuePair<PulsarProperty, object>> CalculatedPropertiesValues { get; } 
  /// <summary>
  /// Перечисление расчетных свойств
  /// </summary>
  IEnumerable<PulsarProperty> CalculatedProperties { get; } 
 }
}

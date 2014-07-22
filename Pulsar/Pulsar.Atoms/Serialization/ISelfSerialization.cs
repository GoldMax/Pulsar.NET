using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Интерфейс собственной сериализации и десериализации объекта
 /// </summary>
 public interface ISelfSerialization
 {
  /// <summary>
  /// Возвращает данные сериализации.
  /// </summary>
  /// <returns></returns>
  byte[] GetSerializedData();
  /// <summary>
  /// Метод десериализации объекта.
  /// </summary>
  /// <param name="data">Данные сериализации.</param>
  void Deserialize(byte[] data);
 }
}

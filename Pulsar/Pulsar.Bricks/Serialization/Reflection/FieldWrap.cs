using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Reflection.Dynamic
{
 /// <summary>
 /// Класс враппера поля типа
 /// </summary>
 public class FieldWrap
 {
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Имя поля
  /// </summary>
  public readonly string Name = null;
  /// <summary>
  /// Режимы сериализации, при которых поле не сериализуется
  /// </summary>
  public PulsarSerializationMode? NoSerMode { get; set; }
  /// <summary>
  /// Режимы сериализации, только при которых поле сериализуется
  /// </summary>
  public PulsarSerializationMode? ByDemandModes { get; set; }
  /// <summary>
  /// Делегат метода получения значения поля
  /// </summary>
  [NonSerialized]
  public Func<object,object> Get;
  /// <summary>
  /// Делегат метода установки значения поля
  /// </summary>
  [NonSerialized]
  public Action<object, object> Set;
  /// <summary>
  /// Тип поля в описании класса.
  /// </summary>
  public Type Type = null;
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Methods >>
  /// <summary>
  /// ToString
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return Name;
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  private FieldWrap() { }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public FieldWrap(string name)
  {
   Name = name;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------

 } 

}

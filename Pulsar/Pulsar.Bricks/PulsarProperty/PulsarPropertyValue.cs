using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Класс значения свойства. 
 /// Используется при необходимости хранения значения свойства вместе с самим свойством, 
 /// вне коллекции значений свойств.
 /// </summary>
 public class PulsarPropertyValue //: ObjectChangeNotification
 {
  private PulsarProperty prop = null;
  private object val = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Свойство, для которого установлено значение.
  /// </summary>
  public PulsarProperty Property
  {
   get { return prop; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Значение свойства
  /// </summary>
  public object Value
  {
   get 
   {
    //--- Debbuger Break --- //
    if(System.Diagnostics.Debugger.IsAttached)
     System.Diagnostics.Debugger.Break();
    //--- Debbuger Break --- //
     
    return val; 
   }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  private PulsarPropertyValue() { }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public PulsarPropertyValue(PulsarProperty prop, object value) : this()
  {
   //--- Debbuger Break --- //
   if(System.Diagnostics.Debugger.IsAttached)
    System.Diagnostics.Debugger.Break();
   //--- Debbuger Break --- //

   this.prop = prop;
   this.val = value;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// ToString
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return prop.ValueToString(val) ?? "";
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
          
 }
}

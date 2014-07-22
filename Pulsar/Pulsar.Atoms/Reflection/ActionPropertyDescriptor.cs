using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pulsar.Reflection
{
  #region << public class ActionPropertyDescriptor : PropertyDescriptor >>
  /// <summary>
  /// Дескриптор псевдо-свойства
  /// </summary>
  public class ActionPropertyDescriptor : PropertyDescriptor
  {
   private Type propType = null;
   private Func<object, object> get = null;
   private Action<object, object> set = null;
   private Type compType = null;
   //-------------------------------------------------------------------------------------
   #region << Constructors >>
   /// <summary>
   /// Создает псевдо-свойство
   /// </summary>
   /// <param name="name">Имя псевдо-свойства.</param>
   /// <param name="propType">Тип псевдо-свойства.</param>
   /// <param name="componentType">Тип компонента, для которого определено это псевдо-свойство.</param>
   /// <param name="get">Метод получения значения псевдо-свойства.</param>
   /// <param name="set">Метод установки значения псевдо-свойства.</param>
   public ActionPropertyDescriptor(string name, Type propType, Type componentType,
                                    Func<object, object> get, Action<object, object> set)
    : base(name, new Attribute[0])
   {
    this.propType = propType;
    compType = componentType;
    this.get = get;
    this.set = set;
   }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
   ///
   public override object GetValue(object component)
   {
    if(get == null || component == null) //|| component.GetType() == compType
     return null;
    return get(component);
   }
   //-------------------------------------------------------------------------------------
   ///
   public override bool IsReadOnly
   {
    get { return set == null; }
   }
   //-------------------------------------------------------------------------------------
   ///
   public override Type PropertyType
   {
    get { return propType; }
   }
   //-------------------------------------------------------------------------------------
   ///
   public override void SetValue(object component, object value)
   {
    if(set != null && component != null && component.GetType() == compType)
     set(component, value);
   }
   //-------------------------------------------------------------------------------------
   ///
   public override bool CanResetValue(object component)
   {
    return false;
   }
   //-------------------------------------------------------------------------------------
   ///
   public override Type ComponentType
   {
    get { return compType; }
   }
   //-------------------------------------------------------------------------------------
   ///
   public override void ResetValue(object component)
   {
    throw new NotImplementedException();
   }
   //-------------------------------------------------------------------------------------
   ///
   public override bool ShouldSerializeValue(object component)
   {
    throw new NotImplementedException();
   }
  }
  #endregion << public class ActionPropertyDescriptor : PropertyDescriptor >>
 }

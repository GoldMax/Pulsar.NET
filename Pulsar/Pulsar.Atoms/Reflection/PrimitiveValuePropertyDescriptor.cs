using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Pulsar.Reflection
{
  //**************************************************************************************
 /// <summary>
 /// Класс дескриптора свойства для переменной.
 /// </summary>
 public class PrimitiveValuePropertyDescriptor : PropertyDescriptor
 {
  Type t;
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public PrimitiveValuePropertyDescriptor(Type t) : base("Value", null)
  {
   this.t = t;
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public PrimitiveValuePropertyDescriptor(Type t, string name) : base(name, null)
  {
   this.t = t;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <returns></returns>
  public override bool CanResetValue(object component)
  {
   return false;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  public override Type ComponentType
  {
   get { return t; }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <returns></returns>
  public override object GetValue(object component)
  {
   return component;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  public override bool IsReadOnly
  {
   get { return false; }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  public override Type PropertyType
  {
   get { return t; }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  public override void ResetValue(object component)
  {
   throw new Exception("The method ResetValue() is not implemented.");
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <param name="value"></param>
  public override void SetValue(object component, object value)
  {
   component = value;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <returns></returns>
  public override bool ShouldSerializeValue(object component)
  {
   throw new Exception("The method ShouldSerializeValue() is not implemented.");
  }
 }

}

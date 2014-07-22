using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Pulsar
{
 /// <summary>
 /// Класс хранения тройки величин.
 /// </summary>
 /// <typeparam name="T1">Тип первой величины.</typeparam>
 /// <typeparam name="T2">Тип второй величины.</typeparam>
 /// <typeparam name="T3">Тип третьей величины.</typeparam>
 [Serializable]
 public class ValuesTrio<T1, T2, T3>
 {
  private T1 val1;
  private T2 val2;
  private T3 val3;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Значение первой величины.
  /// </summary>
  public T1 Value1
  {
   get { return val1; }
   set { val1 = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Значение второй величины.
  /// </summary>
  public T2 Value2
  {
   get { return val2; }
   set { val2 = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Значение третьей величины.
  /// </summary>
  public T3 Value3
  {
   get { return val3; }
   set { val3 = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public ValuesTrio()
  {
   val1 = default(T1);
   val2 = default(T2);
   val3 = default(T3);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="tryCreateValues">Если true, будет произведена попытка вызова конструкторов
  /// по умолчанию величин. </param>
  public ValuesTrio(bool tryCreateValues) : this()
  {
   if(tryCreateValues == false)
    return;
   if(typeof(T1).IsClass)
   {
    ConstructorInfo ci = typeof(T1).GetConstructor(Type.EmptyTypes);
    if(ci != null)
     val1 = (T1)ci.Invoke(null);
   }
   if(typeof(T2).IsClass)
   {
    ConstructorInfo ci = typeof(T2).GetConstructor(Type.EmptyTypes);
    if(ci != null)
     val2 = (T2)ci.Invoke(null);
   }
   if(typeof(T3).IsClass)
   {
    ConstructorInfo ci = typeof(T3).GetConstructor(Type.EmptyTypes);
    if(ci != null)
     val3 = (T3)ci.Invoke(null);
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="value1">Значение первой величины.</param>
  /// <param name="value2">Значение второй величины.</param>
  /// <param name="value3">Значение третьей величины.</param>
 public ValuesTrio(T1 value1, T2 value2, T3 value3)
  {
   val1 = value1;
   val2 = value2;
   val3 = value3;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ToString()
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return String.Format("V1={{{0}}};V2={{{1}}};V3={{{2}}} - {3}",val1 == null ? "null" : val1.ToString(),
                                                   val2 == null ? "null" : val2.ToString(),
                                                   val3 == null ? "null" : val3.ToString(),
                                                   base.ToString());
  }
 }
}

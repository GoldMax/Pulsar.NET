using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Класс редактирования битов простых типов
 /// </summary>
 public class BitEditor
 {
  /// <summary>
  /// Устанавливает или снимает бит 
  /// </summary>
  /// <param name="val">Начальное значение</param>
  /// <param name="pos">Позиция бита</param>
  /// <param name="value">Значение бита</param>
  public static byte ModifyBit(byte val, int pos, bool value) 
  {
   if(value)
    return (byte)(val | ((byte)1 << pos));
   else 
    return (byte)(val & ~((byte)1 << pos));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Устанавливает бит 
  /// </summary>
  /// <param name="val">Начальное значение</param>
  /// <param name="pos">Позиция бита</param>
  public static byte SetBit(byte val, int pos) 
  {
   return (byte)(val | ((byte)1 << pos));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Снимает бит 
  /// </summary>
  /// <param name="val">Начальное значение</param>
  /// <param name="pos">Позиция бита</param>
  public static byte ClearBit(byte val, int pos) 
  {
   return (byte)(val & ~((byte)1 << pos));
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает значение бита
  /// </summary>
  /// <param name="val">Начальное значение</param>
  /// <param name="pos">Позиция бита</param>
  public static bool GetBit(byte val, int pos) 
  {
    return (val & ((byte)1 << pos)) != 0;
  }

 }
}

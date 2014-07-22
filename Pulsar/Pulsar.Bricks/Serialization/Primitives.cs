using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar.Serialization
{
 #region << public enum PrimitiveTypes : ushort >>
 /// <summary>
 /// Типы, примитивные для сериализации.
 /// </summary>
 public enum PrimitiveTypes : ushort
 {
  #pragma warning disable
  Byte = 1,
  SByte = 2,
  Int16 = 3,
  UInt16 = 4,
  Int32 = 5,
  UInt32 = 6,
  Int64 = 7,
  UInt64 = 8,
  IntPtr = 9,
  UIntPtr = 10,
  Boolean = 11,
  Char = 12,
  Double = 13,
  Single = 14,
  Decimal = 15,
  Guid = 16,
  DateTime = 17,
  // 18,
  // 19,
  // 20,
  OID = 21
  // 22
  // 23
  // 24
  #pragma warning restore
 } 
 #endregion << public enum PrimitiveTypes : ushort >>

}

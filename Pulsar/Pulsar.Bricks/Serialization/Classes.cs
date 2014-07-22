using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



namespace Pulsar.Serialization
{                                  
 //**************************************************************************************
 #region << internal class PulsarEmptyStub >>
 /// <summary>
 /// Класс заглушки для объекта, пропускаемого при сериализации
 /// </summary>
 internal class PulsarEmptyStub {  }
 #endregion << internal class PulsarEmptyStub >>
 //*************************************************************************************
 #region << internal class GOLObjectStub >>
 /// <summary>
 /// Класс заглушки объекта Пульсара для сериализации.
 /// </summary>
 internal class GOLObjectStub {  }
 #endregion << internal class GOLObjectStub >>
 //*************************************************************************************
 #region << internal class GOLObjectRegistrator >>
 /// <summary>
 /// Класс регистратора объекта Пульсара для сериализации.
 /// </summary>
 internal class GOLObjectRegistrator { }
 #endregion << internal class GOLObjectStub >>
 //*************************************************************************************
 #region << internal struct PulsarPrimitiveHolder >>
 /// <summary>
 /// Обертка для сериализации примитивного значения.
 /// </summary>
 internal struct PulsarPrimitiveHolder
 {
  public object Primitive;
  public PulsarPrimitiveHolder(object primitive)
  {
   Primitive = primitive;
  }
 } 
 #endregion << internal struct PulsarPrimitiveHolder >>
 //*************************************************************************************
 #region << public class ArrayIndexEnumerator : IEnumerator >>
 /// <summary>
 /// Перечислитель индексов массива.
 /// </summary>
 public class ArrayIndexEnumerator : IEnumerator
 {
  private int[,] az = null;
  private int[] index = null;
  private int rank = 0;

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Текущее значение индекса.
  /// </summary>
  public int[] Current { get { return index; } }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  private ArrayIndexEnumerator() { }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="arr">Массив, индексы которого перебираются.</param>
  public ArrayIndexEnumerator(Array arr)
  {
   rank = arr.Rank;
   az = new int[rank, 2];
   index = new int[rank];
   for(int a = 0; a < rank; a++)
   {
    az[a, 0] = arr.GetLowerBound(a);
    index[a] = az[a, 0];
    az[a, 1] = arr.GetUpperBound(a);
   }
   index[rank-1]--;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Смещается к следующему индексу.
  /// </summary>
  /// <returns></returns>
  public bool MoveNext()
  {
   //bool shift = true;
   for(int a = rank-1; a >= 0; a--)
   {
    //if(shift)
    index[a]++;
    if(index[a] > az[a, 1])
     index[a] = az[a, 0];
    else
     return true;
   }

   return false;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Сбрасывет к начальному состоянию.
  /// </summary>
  public void Reset()
  {
   for(int a = 0; a < rank; a++)
    index[a] = az[a, 0];
   index[rank-1]--;
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  #region IEnumerator Members
  object IEnumerator.Current
  {
   get { return Current; }
  }
  #endregion
 }
 #endregion << public class ArrayIndexEnumerator : IEnumerator >>
 //*************************************************************************************
 #region << internal class SerObjectInfo >>
 /// <summary>
 /// Описатель объекта
 /// </summary>
 internal class SerObjectInfo
 {
  public ushort typeID;
  public uint objID;
  // byte count
  public List<SerFieldInfo> fields = null;
  public Dictionary<string, SerFieldInfo> index = null;
  //-------------------------------------------------------------------------------------
  public void Save(Stream fs)
  {
   fs.WriteUInt16(typeID);
   fs.WriteUInt32(objID);
   if(fields == null)
   {
    fs.WriteByte(0);
    return;
   }
   if(fields.Count > byte.MaxValue)
    throw new Exception("Fields count > 255 !");
   fs.WriteByte((byte)fields.Count);
   foreach(SerFieldInfo sfi in fields)
    sfi.Save(fs);
  }
  //-------------------------------------------------------------------------------------
  public void Load(Stream fs)
  {
   // typeID - уже должен быть прочитан
   try
   {
    objID = fs.ReadUInt32();
    byte count = (byte)fs.ReadByte();
    if(count > 0)
     index = new Dictionary<string, SerFieldInfo>(count);
    for(; count > 0; count--)
    {
     SerFieldInfo sfi = new SerFieldInfo();
     sfi.Load(fs);
     index.Add(sfi.name, sfi);
    }
   }
   catch
   {
    throw;
   }
  }
 }
 #endregion << internal class SerObjectInfo >>
 //*************************************************************************************
 #region << internal struct SerFieldInfo >>
 internal struct SerFieldInfo
 {
  public ushort typeID;
  //ushort len;
  public string name;
  public byte[] value;
  //-------------------------------------------------------------------------------------
  public void Save(Stream fs)
  {
   fs.WriteUInt16(typeID);

   byte[] buf = UTF8Encoding.UTF8.GetBytes(name ?? "");
   if(buf.Length > ushort.MaxValue)
    throw new PulsarException("Name buffer length > {0} !", ushort.MaxValue);
   fs.WriteUInt16((ushort)buf.Length);
   fs.Write(buf, 0, buf.Length);

   if(value == null)
   {
    fs.WriteByte(0);
    return;
   }
   if(value.Length >= byte.MaxValue)
   {
    fs.WriteByte(255);
    if((uint)value.Length > uint.MaxValue)
     throw new PulsarException("Value buffer length > {0} !", uint.MaxValue);
    fs.WriteUInt32((uint)value.Length);
   }
   else
    fs.WriteByte((byte)value.Length);
   fs.Write(value, 0, value.Length);
  }
  //-------------------------------------------------------------------------------------
  public void Load(Stream fs)
  {
   try
   {
    typeID = fs.ReadUInt16();
    ushort len = fs.ReadUInt16();
    name = UTF8Encoding.UTF8.GetString(fs.ReadBytes(len), 0, len);
    byte buflen = (byte)fs.ReadByte();
    if(buflen == 0)
     value = new byte[0];
    else if(buflen < 255)
     value = fs.ReadBytes(buflen);
    else
     value = fs.ReadBytes(fs.ReadUInt32());
   }
   catch
   {
    throw;
   }
  }
 }
 #endregion << internal struct SerFieldInfo >>
 //*************************************************************************************
 #region << internal struct SerTypeInfo >>
 internal struct SerTypeInfo
 {
  //public ushort typeID;
  //ushort len;
  public string name;
  public ushort value;
  //-------------------------------------------------------------------------------------
  public void Save(Stream fs)
  {
   fs.WriteUInt16(0);

   byte[] buf = UTF8Encoding.UTF8.GetBytes(name);
   if(buf.Length > ushort.MaxValue)
    throw new PulsarException("Type name buffer length > {0} !", ushort.MaxValue);
   fs.WriteUInt16((ushort)buf.Length);
   fs.Write(buf, 0, buf.Length);

   fs.WriteUInt16(value);
  }
  //-------------------------------------------------------------------------------------
  public void Load(Stream fs)
  {
   // typeID - уже должен быть прочитан = 0
   ushort len = fs.ReadUInt16();
   name = UTF8Encoding.UTF8.GetString(fs.ReadBytes(len), 0, len);
   value = fs.ReadUInt16();
  }
  //-------------------------------------------------------------------------------------
  public override string ToString()
  {
   return String.Format("[{0},{1}]", value, name ?? "");
  }
 }
 #endregion << internal struct SerTypeInfo >>
 //*************************************************************************************

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Pulsar.Reflection.Dynamic
{
 //*************************************************************************************
 /// <summary>
 /// Класс враппера сериализации типа. 
 /// </summary>
 public class TypeSerializationWrap
 {
  private static Dictionary<IntPtr, TypeSerializationWrap> dic = new Dictionary<IntPtr, TypeSerializationWrap>(10000);


  private static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public |
                                       BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Тип
  /// </summary>
  public readonly Type Type = null;
  /// <summary>
  /// Массив врапперов полей типа
  /// </summary>
  public readonly FieldWrap[] Fields = null;

  /// <summary>
  /// Делегаты методов OnSerializing
  /// </summary>
  [NonSerialized]
  protected readonly Action<object>[] OnSerializingList = null;
  /// <summary>
  /// Делегаты методов OnSerialized
  /// </summary>
  [NonSerialized]
  protected readonly Action<object>[] OnSerializedList = null;
  /// <summary>
  /// Делегаты методов OnDeserializing
  /// </summary>
  [NonSerialized]
  protected readonly Action<object>[] OnDeserializingList = null;
  /// <summary>
  /// Делегаты методов OnDeserialized
  /// </summary>
  [NonSerialized]
  protected readonly Action<object>[] OnDeserializedList = null;
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  private TypeSerializationWrap() { }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public TypeSerializationWrap(Type t)
  {
   Type = t;
   List<FieldWrap> list = new List<FieldWrap>();
   foreach(FieldInfo fi in t.GetFields(flags))
   {
    if(fi.IsNotSerialized || fi.FieldType.IsSubclassOf(typeof(Delegate)))
     continue;
    FieldWrap fw = new FieldWrap(fi.Name);
    fw.Type = fi.FieldType;
    fw.Get = ReflectionHelper.CreateFieldGetMethod(t, fi);
    fw.Set = ReflectionHelper.CreateFieldSetMethod(t, fi);
    object[] atts = fi.GetCustomAttributes(typeof(PulsarNonSerialized), false);
    if(atts.Length > 0)
     fw.NoSerMode = ((PulsarNonSerialized)atts[0]).Mode;
    atts = fi.GetCustomAttributes(typeof(PulsarByDemandSerialization), false);
    if(atts.Length > 0)
     fw.ByDemandModes = ((PulsarByDemandSerialization)atts[0]).Excepts;
    list.Add(fw);
   }
   list.TrimExcess();
   Fields = list.ToArray();

   Action<object> sering = null, sered = null, desering = null, desered = null;
   foreach(MethodInfo mi in t.GetMethods(flags))
   {
    if(mi.IsDefined(typeof(OnSerializingAttribute), false))
     sering = TypeSerializationWrap.GetOnSerMethodDelegate(t, mi);
    if(mi.IsDefined(typeof(OnSerializedAttribute), false))
     sered = TypeSerializationWrap.GetOnSerMethodDelegate(t, mi);
    if(mi.IsDefined(typeof(OnDeserializingAttribute), false))
     desering = TypeSerializationWrap.GetOnSerMethodDelegate(t, mi);
    if(mi.IsDefined(typeof(OnDeserializedAttribute), false))
     desered = TypeSerializationWrap.GetOnSerMethodDelegate(t, mi);
   }

   Type baseT = t.BaseType;
   if(baseT != null && baseT != typeof(object))
   {
    TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(baseT);
    // -- OnSerializing
    if(tw.OnSerializingList == null)
     OnSerializingList = sering == null ? null : new Action<object>[1] { sering };
    else if(sering == null)
     OnSerializingList = tw.OnSerializingList;
    else
    {
     int len = tw.OnSerializingList.Length;
     OnSerializingList = new Action<object>[len+1];
     Array.Copy(tw.OnSerializingList, OnSerializingList, len);
     OnSerializingList[len] = sering;
    }
    //-- OnSerialized
    if(tw.OnSerializedList == null)
     OnSerializedList = sered == null ? null : new Action<object>[1] { sered };
    else if(sered == null)
     OnSerializedList = tw.OnSerializedList;
    else
    {
     int len = tw.OnSerializedList.Length;
     OnSerializedList = new Action<object>[len+1];
     Array.Copy(tw.OnSerializedList, OnSerializedList, len);
     OnSerializedList[len] = sered;
    }
    // -- OnDeserializing
    if(tw.OnDeserializingList == null)
     OnDeserializingList = desering == null ? null : new Action<object>[1] { desering };
    else if(desering == null)
     OnDeserializingList = tw.OnDeserializingList;
    else
    {
     int len = tw.OnDeserializingList.Length;
     OnDeserializingList = new Action<object>[len+1];
     Array.Copy(tw.OnDeserializingList, OnDeserializingList, len);
     OnDeserializingList[len] = desering;
    }

    if(tw.OnDeserializedList == null)
     OnDeserializedList = desered == null ? null : new Action<object>[1] { desered };
    else if(desered == null)
     OnDeserializedList = tw.OnDeserializedList;
    else
    {
     int len = tw.OnDeserializedList.Length;
     OnDeserializedList = new Action<object>[len+1];
     Array.Copy(tw.OnDeserializedList, OnDeserializedList, len);
     OnDeserializedList[len] = desered;
    }
   }
   else
   {
    OnSerializingList = sering == null ? null : new Action<object>[1] { sering };
    OnSerializedList = sered == null ? null : new Action<object>[1] { sered };
    OnDeserializingList = desering == null ? null : new Action<object>[1] { desering };
    OnDeserializedList = desered == null ? null : new Action<object>[1] { desered };
   }
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Override Methods >>
  /// <summary>
  /// GetHashCode
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
  {
   return (int)Type.TypeHandle.Value;
  }
  /// <summary>
  /// ToString
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
   return Type.ToString();
  }
  /// <summary>
  /// Equals
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object obj)
  {
   return obj is TypeSerializationWrap && 
     (int)((TypeSerializationWrap)obj).Type.TypeHandle.Value == (int)Type.TypeHandle.Value;
  }
  #endregion << Override Methods >>
  //-------------------------------------------------------------------------------------
  #region << Serialization Methods >>
  /// <summary>
  /// Вызывает методы OnSerializing указанного объекта.
  /// </summary>
  /// <param name="target">Объект, методы которого вызываются.</param>
  public void OnSerializing(object target)
  {
   if(OnSerializingList != null)
    for(int a = 0; a < OnSerializingList.Length; a++)
     OnSerializingList[a](target);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Вызывает методы OnSerialized указанного объекта.
  /// </summary>
  /// <param name="target">Объект, методы которого вызываются.</param>
  public void OnSerialized(object target)
  {
   if(OnSerializedList != null)
    for(int a = 0; a < OnSerializedList.Length; a++)
     OnSerializedList[a](target);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Вызывает методы OnDeserializing указанного объекта.
  /// </summary>
  /// <param name="target">Объект, методы которого вызываются.</param>
  public void OnDeserializing(object target)
  {
   if(OnDeserializingList != null)
    for(int a = 0; a < OnDeserializingList.Length; a++)
     OnDeserializingList[a](target);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Вызывает методы OnDeserialized указанного объекта.
  /// </summary>
  /// <param name="target">Объект, методы которого вызываются.</param>
  public void OnDeserialized(object target)
  {
   if(OnDeserializedList != null)
    for(int a = 0; a < OnDeserializedList.Length; a++)
     OnDeserializedList[a](target);
  }
  #endregion << Serialization Methods >>
  //-------------------------------------------------------------------------------------
  #region << Static Methods >>
  /// <summary>
  /// Возвращает объект враппера динамической рефлексии для указанного типа 
  /// </summary>
  /// <param name="t">Тип</param>
  /// <returns></returns>
  public static TypeSerializationWrap GetTypeSerializationWrap(Type t)
  {
   TypeSerializationWrap tw = null;
   lock(dic)
    if(dic.TryGetValue(t.TypeHandle.Value, out tw) == false)
     dic.Add(t.TypeHandle.Value, tw = new TypeSerializationWrap(t));
   return tw;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает делегат вызова метода упралвления сериализацией
  /// </summary>
  /// <param name="type">Тип</param>
  /// <param name="mi">Информация о методе</param>
  /// <returns></returns>
  public static Action<object> GetOnSerMethodDelegate(Type type, MethodInfo mi)
  {
   DynamicMethod meth = 
     new DynamicMethod(mi.Name + "_Call", null, new Type[] { typeof(object) }, type, true);
   ILGenerator il = meth.GetILGenerator();
   LocalBuilder lb = il.DeclareLocal(typeof(StreamingContext));
   il.Emit(OpCodes.Ldarg_0);
   il.Emit(OpCodes.Ldloca_S, lb);
   il.Emit(OpCodes.Initobj, typeof(StreamingContext));
   il.Emit(OpCodes.Ldloc_0);
   il.EmitCall(OpCodes.Call, mi, null);
   il.Emit(OpCodes.Ret);

   return (Action<object>)meth.CreateDelegate(typeof(Action<object>));
  }

  #endregion << Static Methods >>

 }

}

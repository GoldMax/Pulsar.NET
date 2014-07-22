using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pulsar.Reflection
{
 /// <summary>
 /// Класс провайдера для расширения типов псевдо-свойствами.
 /// </summary>
 public class ActionTypeDescriptionProvider : TypeDescriptionProvider, ICustomTypeDescriptor
 {
  private Type type = null;
  private PList<ActionPropertyDescriptor> pseudos = new PList<ActionPropertyDescriptor>(1);
  private ICustomTypeDescriptor baseDesc = null;

  #region TypeDescriptionProvider
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Список псевдо-свойств.
  /// </summary>
  public PList<ActionPropertyDescriptor> PseudoProps
  {
   get { return pseudos; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Constructors >>
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="type">Тип, для которого создается провайдер.</param>
  public ActionTypeDescriptionProvider(Type type)
   : base(TypeDescriptor.GetProvider(type))
  {
   this.type = type;
   baseDesc = TypeDescriptor.GetProvider(type).GetTypeDescriptor(type);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// GetTypeDescriptor
  /// </summary>
  /// <param name="objectType"></param>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object obj)
  {
   if(objectType != type)
    return baseDesc;
   return this; 
  }
  //-------------------------------------------------------------------------------------

  #endregion TypeDescriptionProvider
  //-------------------------------------------------------------------------------------
  #region ICustomTypeDescriptor Members
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public AttributeCollection GetAttributes()
  {
   return new AttributeCollection((new List<Attribute>(type.GetCustomAttributes(true).Cast<Attribute>())).ToArray());
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public string GetClassName()
  {
   return type.Name;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public string GetComponentName()
  {
   return null;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public TypeConverter GetConverter()
  {
   return baseDesc.GetConverter();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public EventDescriptor GetDefaultEvent()
  {
   return baseDesc.GetDefaultEvent();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public PropertyDescriptor GetDefaultProperty()
  {
   return baseDesc.GetDefaultProperty();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public object GetEditor(Type editorBaseType)
  {
   return baseDesc.GetEditor(editorBaseType);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
   return baseDesc.GetEvents(attributes);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public EventDescriptorCollection GetEvents()
  {
   return baseDesc.GetEvents();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
   return GetProperties();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public PropertyDescriptorCollection GetProperties()
  {
   List<PropertyDescriptor> list = new List<PropertyDescriptor>();
   foreach(PropertyDescriptor i in baseDesc.GetProperties())
    list.Add(i);
   foreach(PropertyDescriptor i in pseudos)
    list.Add(i);
   return new PropertyDescriptorCollection(list.ToArray());
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public object GetPropertyOwner(PropertyDescriptor pd)
  {
   throw new NotImplementedException();
  }

  #endregion
 } 

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using Pulsar;

namespace Pulsar
{
 /// <summary>
 /// Класс компонента биндера объекта.
 /// </summary>
 public class ObjectBinder : Component 
 {
  List<WeakReference> _list = new List<WeakReference>(0);

  private Type _type = null;
  private object _obj = null;
  internal Dictionary<string, PropertyDescriptor> _pds = new Dictionary<string,PropertyDescriptor>(0);
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Тип объекта биндинга.
  /// </summary>
  [DefaultValue(null)]
  [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public string BindObjectTypeName
  {
   get { return _type == null ? null : _type.Name; }
   set 
   {
    if(value == null)
    {
     _type = null;
     return;
    }
    Type t = Type.GetType(value);
    BindObjectType = t; 
   }
  }
  /// <summary>
  /// Тип объекта биндинга.
  /// </summary>
  [DefaultValue(null)]
  public Type BindObjectType
  {
   get { return _type; }
   set 
   {
    if(_type == value)
     return;
    _pds.Clear();
    _type = value;
    if(_type != null) 
     foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(_type))
      _pds.Add(pd.Name, pd);
    if(_obj == null || _type.IsAssignableFrom(_obj.GetType()) == false)
     _obj = null;
   }
  }
  /// <summary>
  /// Объект биндинга.
  /// </summary>
  [DefaultValue(null)]
  public object BindObject
  {
   get { return _obj; }
   set 
   {
    if(_obj is IObjectChangeNotify)
     ((IObjectChangeNotify)_obj).ObjectChanged -= new EventHandler<ObjectChangeNotifyEventArgs>(SimObjectBinder_ObjectChanged);
    RaiseNotify(null, null);
    if(_type == null || value == null || _type.IsAssignableFrom(value.GetType()))
     _obj = value;
    else 
     throw new Exception("Объект не соответствует типу биндера!"); 
    if(_obj is IObjectChangeNotify)
     ((IObjectChangeNotify)_obj).ObjectChanged += new EventHandler<ObjectChangeNotifyEventArgs>(SimObjectBinder_ObjectChanged);
    RaiseNotify(_obj == null ? null : "", null);
   }
  }
  /// <summary>
  /// Возвращает дескриптор свойства по его имени.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public PropertyDescriptor this[string name]
  {
   get
   {
    if(string.IsNullOrWhiteSpace(name))
     throw new ArgumentException("name");
    if(_type == null)
     return null;
    PropertyDescriptor pd = null;
    _pds.TryGetValue(name, out pd);
    return pd;
   }
  }
  #endregion << Properties >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public ObjectBinder() : base()
  {

  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Метод регистрации биндинга
  /// </summary>
  /// <param name="binding"></param>
  public void RegBinding(ObjectBinding binding)
  {
   lock(_list)
   {
    bool has = false;
    List<WeakReference> toDel = new List<WeakReference>(0);
    foreach(WeakReference wr in _list)
     if(wr.IsAlive == false)
      toDel.Add(wr);
     else if(wr.Target == binding)
     {
      if(((ObjectBinding)wr.Target).PropertyName == binding.PropertyName)
       break;
      wr.Target = binding;
      has = true;
      break;
     }
    if(toDel.Count > 0)  
     foreach(WeakReference wr in toDel)
      _list.Remove(wr);
    if(has == false)
     _list.Add(new WeakReference(binding));
    if(binding.OnSourceValueChanged != null && _type != null && _obj != null)
    {
     PropertyDescriptor pd = this[binding.PropertyName];
     if(pd == null)
      binding.OnSourceValueChanged(null, true);
     else
      binding.OnSourceValueChanged(pd.GetValue(_obj), false);
    }
   }
  }
  /// <summary>
  /// Метод отмены регистрации биндинга
  /// </summary>
  /// <param name="binding"></param>
  public void UnregBinding(ObjectBinding binding)
  {
   lock(_list)
   {
    List<WeakReference> toDel = new List<WeakReference>(0);
    foreach(WeakReference wr in _list)
     if(wr.IsAlive == false || wr.Target == binding)
      toDel.Add(wr);
    if(toDel.Count > 0)
     foreach(WeakReference wr in toDel)
      _list.Remove(wr);
    if(binding.OnSourceValueChanged != null)
     binding.OnSourceValueChanged(null, false);
   }
  }
  /// <summary>
  /// propName = null && value == null - всем посылаем null как значение
  /// propName = "" && value == null - всем посылаем значение из PropertyDescriptor или увкдомление об обновлении
  /// </summary>
  private void RaiseNotify(string propName, object value)
  {
   lock(_list)
   {
    if(_type == null || _obj == null)
     return;

    List<WeakReference> toDel = new List<WeakReference>(0);
    foreach(WeakReference wr in _list)
     if(wr.IsAlive == false)
      toDel.Add(wr);
     else 
     {
      ObjectBinding binding = (ObjectBinding)wr.Target;
      if(binding.OnSourceValueChanged == null)
       continue;
      if(propName == null)
       binding.OnSourceValueChanged(null, false);
      else if(propName.Length == 0)
      {
       PropertyDescriptor pd = this[binding.PropertyName];
       if(pd == null)
        binding.OnSourceValueChanged(null, true);
       else
        binding.OnSourceValueChanged(pd.GetValue(_obj), false);
      }
      else if(propName == binding.PropertyName)
       binding.OnSourceValueChanged(value, false);
     }
    if(toDel.Count > 0)
     foreach(WeakReference wr in toDel)
      _list.Remove(wr);
   }
  }
  void SimObjectBinder_ObjectChanged(object sender, ObjectChangeNotifyEventArgs e)
  {
   if(e.Action == ChangeNotifyAction.ObjectReset || e.MemberName == null)
    RaiseNotify("",null);
   else 
    RaiseNotify(e.MemberName, e.NewValue);
  }
  /// <summary>
  /// Возвращает значение указнного свойства.
  /// </summary>
  /// <param name="propName">Имя свойства</param>
  /// <returns></returns>
  public object GetValue(string propName)
  {
   if(_obj == null)
    return null;
   PropertyDescriptor pd = this[propName];
   if(pd == null)
    return null;
   return pd.GetValue(_obj);
  }
  /// <summary>
  /// Устанавливает значение указнного свойства.
  /// </summary>
  /// <param name="propName">Имя свойства</param>
  /// <param name="value">Значение</param>
  /// <returns></returns>
  public void SetValue(string propName, object value)
  {
   if(_obj == null)
    return;
   PropertyDescriptor pd = this[propName];
   if(pd == null)
    return;
   pd.SetValue(_obj, value);
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
 }
 //*************************************************************************************
 /// <summary>
 /// Класс биндинга
 /// </summary>
 [TypeConverter(typeof(ExpandableObjectConverter))]
 public class ObjectBinding
 {
  private ObjectBinder _binder = null;
  private string _pName;

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Биндер
  /// </summary>
  [DefaultValue(null)]
  [Description("Биндер")]
  public ObjectBinder ObjectBinder
  {
   get { return _binder; }
   set 
   {
    if(_binder != null && _pName != null)
    _binder.UnregBinding(this);  
    _binder = value;
    if(_binder != null && _pName != null)
    {
     if(OnBindingChanged != null)
      OnBindingChanged();
     _binder.RegBinding(this);
    }
   }
  }
  /// <summary>
  /// Имя свойства источника, к которому выполняется биндинг
  /// </summary>
  [TypeConverter(typeof(ObjectBinderPropertyTypeConverter))]
  [Description("Имя свойства источника, к которому выполняется биндинг")]
  [DefaultValue(null)]
  public string PropertyName
  {
   get { return _pName; }
   set 
   { 
    if(_binder != null && _pName != null)
     _binder.UnregBinding(this);
    _pName = String.IsNullOrWhiteSpace(value) ? null : value;
    if(_binder != null && _pName != null)
    {
     if(OnBindingChanged != null)
      OnBindingChanged();
     _binder.RegBinding(this);
    }
   }
  }
  /// <summary>
  /// Делегат метода, вызываемого при изменении значения свойства.
  /// object - новое значение свойства, bool - необходимость перечитать значение свойства
  /// </summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Action<object, bool> OnSourceValueChanged { get; set; }
  /// <summary>
  /// Делегат метода, вызываемого при изменении биндинга.
  /// </summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public Action OnBindingChanged { get; set; }
  /// <summary>
  /// Возвращает PropDescriptor свойства источника, к которому выполняется биндинг
  /// </summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public PropertyDescriptor PropDescriptor
  {
   get { return _binder == null || _pName == null ? null : _binder[_pName]; }
  }
  [Browsable(false)]
  public bool IsDefined
  {
   get { return _binder != null && _pName != null; }
  }
  #endregion << Properties >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public ObjectBinding() { }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Возвращает значение свойства
  /// </summary>
  /// <returns></returns>
  public object GetValue()
  {
   if(IsDefined == false)
    throw new Exception("Биндинг не инициализирован!");
   return _binder.GetValue(_pName);
  }
  /// <summary>
  /// Устанавливает значение свойства
  /// </summary>
  /// <returns></returns>
  public void SetValue(object value)
  {
   if(IsDefined == false)
    throw new Exception("Биндинг не инициализирован!");
   _binder.SetValue(_pName, value);
  }
  public override string ToString()
  {
   return String.Format("{0}-{1}", _binder == null ? "(none)" : _binder.ToString(), _pName == null ? "(none)" : _pName.ToString());
  }
  #endregion << Methods >>
 }
 //*************************************************************************************
 #region public class ObjectBinderPropertyTypeConverter : StringConverter
 /// <summary>
 /// TypeConverter для списка свойств
 /// </summary>
 public class ObjectBinderPropertyTypeConverter : StringConverter
 {
  public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
  {
   return true;
  }
  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
  {
   return false;
  }
  public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
  {
   object o = context.Instance;
   PropertyInfo pi = o.GetType().GetProperty("ObjectBinder");
   if(pi == null)
    return new StandardValuesCollection(null);
   o = pi.GetValue(o, null);
   if(o == null || o is ObjectBinder == false || ((ObjectBinder)o).BindObjectType == null)
    return new StandardValuesCollection(null);
   List<string> res = new List<string>(((ObjectBinder)o)._pds.Keys);
   res.Sort();
   return new StandardValuesCollection(res);
  }
 } 
 #endregion public classObjectBinderPropertyTypeConverter : StringConverter
}

using System;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;

using Pulsar;

namespace Sim.Controls
{
 //**************************************************************************************
 #region << class SimTreeNodeEx : TreeNode, ISerializable >>
 ///<summary>
 ///Класс узла контрола древовидного представления.
 ///</summary>
 [TypeConverter(typeof(SimTreeNodeExConverter))] 
 public class SimTreeNodeEx : TreeNode, ISerializable
 {
  private CheckState chState = CheckState.Unchecked;
  private SimTreeNodeExCollection col = null;
  private bool enabled = true;
  private Color prevColor;
  private int imageIndex = -1;
  private int expandedImageIndex = -1;
  private ITreeItem treeItem = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет состояние CheckBox'а записи.  
  /// </summary>
  [Category("Addons")]
  [Description("Определяет состояние CheckBox'а записи.")]
  [DefaultValue(typeof(CheckState), "Unchecked")]
  public CheckState CheckState
  {
   get { return chState; }
   set { chState = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет состояние CheckBox'а записи.
  /// </summary>
  [Category("Addons")]
  [Description("Определяет состояние CheckBox'а записи.")]
  [DefaultValue(false)]
  public new bool Checked
  {
   get { return chState == CheckState.Checked ? true : false;}
   set { chState = (value == true ? CheckState.Checked : CheckState.Unchecked); }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Возврящает коллекцию дочерних элементов.
  /// </summary>
  [Category("Behavior")]
  [Description("Определяет коллекцию элементов дерева.")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public new SimTreeNodeExCollection Nodes
  {
   get { return col; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Возвращает родительский элемент.
  /// </summary>
  [Browsable(false)]
  public new SimTreeNodeEx Parent
  {
   get { return base.Parent == null ? null : (SimTreeNodeEx)base.Parent; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет, может ли осуществляться взаимодействие с элементом.
  /// </summary>
  [Category("Addons")]
  [Description("Определяет, может ли осуществляться взаимодействие с элементом.")]
  [DefaultValue(true)]
  public bool Enabled
  {
   get { return enabled; }
   set 
   { 
    enabled = value;
    if(enabled)
     ForeColor = prevColor;
    else
     ForeColor = SystemColors.GrayText;  
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Индекс изображения элемента.
  /// </summary>
  [Category("Addons")]
  [Description("Индекс изображения элемента.")]
  [DefaultValue(-1)]
  public new int ImageIndex
  {
   get 
   {
    if(expandedImageIndex != -1 && base.IsExpanded)
     return expandedImageIndex; 
    else 
     return imageIndex;
   }  
   set 
   { 
    base.ImageIndex = value;
    base.SelectedImageIndex = value;
    imageIndex = value; 
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Индекс изображения раскрытого элемента.
  /// </summary>
  [Category("Addons")]
  [Description("Индекс изображения раскрытого элемента.")]
  [DefaultValue(-1)]
  public int ExpandedImageIndex
  {
   get { return expandedImageIndex; }
   set 
   {
    expandedImageIndex = value; 
    if(expandedImageIndex != -1 && base.IsExpanded)
    {
     base.ImageIndex = value;
     base.SelectedImageIndex = value;
    } 
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Объект данных узла.
  /// </summary>
  public ITreeItem TreeItem
  {
   get { return treeItem;  }
   set { treeItem = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
  //-------------------------------------------------------------------------------------
  #region << Conctructors >>
  /// <summary>
  /// Создает экземпляр SimTreeNodeEx.
  /// </summary>
  private SimTreeNodeEx() : base()
  {
   col = new SimTreeNodeExCollection(this);
   prevColor = base.ForeColor;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Создает узел SimTreeNodeEx.
  /// </summary>
  /// <param name="item">Элемент дерева узла.</param>
  /// <param name="nodeTextPropName">Имя свойства объекта, определяющего имя узла.</param>
  public SimTreeNodeEx(ITreeItem item, string nodeTextPropName) : this()
  {
   treeItem = item;
   RefreshNodeText(nodeTextPropName);
  }
  //-------------------------------------------------------------------------------------
  internal SimTreeNodeEx(string name, string text) : this()
  {
   base.Name = name;
   base.Text = text;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Initializes a new instance of the SimTreeNodeEx class using the specified serialization
  /// information and context. 
  /// </summary>
  /// <param name="si">A SerializationInfo containing the data to deserialize the class.</param>
  /// <param name="context">The StreamingContext containing the source and destination of the serialized stream.</param>
  protected SimTreeNodeEx(SerializationInfo si, StreamingContext context)
   : base(si, context)
  {
   CheckState = (CheckState)si.GetValue("CheckState", typeof(CheckState));
   prevColor = base.ForeColor;
  }
  #endregion << Conctructors >>
  //-------------------------------------------------------------------------------------
  #region ISerializable implements
  /// <summary>
  /// 
  /// </summary>
  /// <param name="si"></param>
  /// <param name="context"></param>
  [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
  protected void GetObjectData(SerializationInfo si, StreamingContext context)
  {
   si.AddValue("CheckState", chState);
  }
  #endregion ISerializable implements
  //-------------------------------------------------------------------------------------   
  #region << Methods >>
  internal void RefreshNodeText(string nodeTextPropName)
  {
   base.Text = SimTreeNodeEx.GetNodeText(treeItem, nodeTextPropName);
  }
  //-------------------------------------------------------------------------------------
  internal static string GetNodeText(ITreeItem item, string nodeTextPropName)
  {
   try
   {
    if(item == null || item.ItemText == null)
     return "NULL";
    if(String.IsNullOrWhiteSpace(nodeTextPropName))
     return item.ItemText;
    Type t = item.Object.GetType();
    PropertyInfo pi = t.GetProperty(nodeTextPropName, BindingFlags.GetProperty | BindingFlags.Instance |
                                                      BindingFlags.NonPublic | BindingFlags.Public);
    if(pi == null)
     throw new PulsarException("Тип [{0}] не содержит свойства {1}!", t.FullName, nodeTextPropName);
    return (pi.GetValue(item.Object, null) ?? "NULL").ToString();
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
    return "ERROR";
   }
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
      
 }
 #endregion << class SimTreeNodeEx : TreeNode, ISerializable >>
 //**************************************************************************************
 #region class SimTreeNodeExConverter : TypeConverter
 internal class SimTreeNodeExConverter : TypeConverter
 {
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimTreeNodeExConverter() : base() { }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="context"></param>
  /// <param name="sourceType"></param>
  /// <returns></returns>
  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
   if(sourceType == typeof(TreeNode) || sourceType == typeof(SimTreeNodeEx) ||
        sourceType == typeof(InstanceDescriptor))
    return true;
   return base.CanConvertFrom(context, sourceType);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="context"></param>
  /// <param name="destinationType"></param>
  /// <returns></returns>
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
   if(destinationType == typeof(string) ||
       destinationType == typeof(TreeNode) ||
       destinationType == typeof(SimTreeNodeEx) ||
       destinationType == typeof(InstanceDescriptor))
    return true;
   return base.CanConvertTo(context, destinationType);
  }
  //-------------------------------------------------------------------------------------
  public override object ConvertFrom(ITypeDescriptorContext context,
                                     System.Globalization.CultureInfo culture, object value)
  {
   //if (value.GetType() == typeof(string))
   // return new GoldTreeNode(value.ToString());
   return base.ConvertFrom(context, culture, value);
  }
  //-------------------------------------------------------------------------------------
  public override object ConvertTo(ITypeDescriptorContext context,
                                   System.Globalization.CultureInfo culture, object value, Type destinationType)
  {
   SimTreeNodeEx node = value as SimTreeNodeEx;
   if(node != null)
   {
    if(destinationType == typeof(string))
     return node.Text;
    if(destinationType == typeof(TreeNode))
     return (TreeNode)node;
    if(destinationType == typeof(SimTreeNodeEx))
     return node;
   }
   return null;
  }
  //-------------------------------------------------------------------------------------
  public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
  {
   return base.GetCreateInstanceSupported(context);
  }
  //-------------------------------------------------------------------------------------
  public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
  {
   return base.CreateInstance(context, propertyValues);
  }
  //-------------------------------------------------------------------------------------
  public override bool GetPropertiesSupported(ITypeDescriptorContext context)
  {
   return base.GetPropertiesSupported(context);
  }
  //-------------------------------------------------------------------------------------
  public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
  {
   return base.GetProperties(context, value, attributes);
  }
  //-------------------------------------------------------------------------------------
  public override bool IsValid(ITypeDescriptorContext context, object value)
  {
   return base.IsValid(context, value);
  }
 }
 #endregion class SimTreeNodeExConverter : TypeConverter
 //**************************************************************************************
}

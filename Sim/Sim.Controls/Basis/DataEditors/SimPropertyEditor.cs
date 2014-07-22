using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Pulsar;
using Pulsar.Reflection;
using Sim.Controls;

namespace Sim.Common
{
 /// <summary>
 /// Класс контрола редактора свойств
 /// Три режима работы:
 /// 1. Не указаны ни дискриптор, ни биндинг
 /// 2. Указан дескриптор и объект
 /// 3. Указан биндинг
 /// </summary>
 public class SimPropertyEditor : Panel
 {
  private Sim.Controls.SimLabel slText;
  private Sim.Controls.SimLabel slValue;

  private string _cap = null;
  private object _val = null;
  private TextBoxFormat _format = TextBoxFormat.NotSet;
  private string _formatExceptions = "";
  private string _outFormat = "";

  private bool _isReadOnly = false;

  private ObjectBinding _binding = new ObjectBinding();

  private PropertyDescriptor _pd = null;
  private Object _obj = null;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << Events >>
  #region ValueChanged
  /// <summary>
  /// Событие, генерируемое при изменении Value из пользовательского интерфейса или при изменении в источнике.  
  /// </summary>
  public event Pulsar.EventHandler<SimPropertyEditor, object> ValueChanged;
  /// <summary>
  /// Вызывает событие ValueChanged
  /// </summary>
  /// <param name="value">Новое значение Value.</param>
  protected void OnValueChanged(object value)
  {
   if(ValueChanged != null)
    ValueChanged(this, value);
  }
  #endregion ValueChanged
  #region EditButtonClick
  /// <summary>
  /// Событие, генерируемое при клике на кнопке редактирования
  /// </summary>
  public event EventHandler<CancelEventArgs<ToolStripDropDown>> EditButtonClick;
  /// <summary>
  /// Вызывает событие EditButtonClick
  /// </summary> 
  /// <returns></returns>
  protected CancelEventArgs<ToolStripDropDown> OnEditButtonClick()
  { 
   CancelEventArgs<ToolStripDropDown> arg = new CancelEventArgs<ToolStripDropDown>();
   if(EditButtonClick != null)
    EditButtonClick(this, arg);
   return arg;
  }
  #endregion EditButtonClick
  #endregion << Events >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Label заголовка
  /// </summary>
  [Category("Own props")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Description("Label заголовка")]
  public SimLabel CaptionLabel
  {
   get { return slText; }
  }
  /// <summary>
  /// Label значения
  /// </summary>
  [Category("Own props")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Description("Label значения")]
  public SimLabel ValueLabel
  {
   get { return slValue; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Заголовок
  /// </summary>
  [Category("Own props")]
  [DefaultValue(null)]
  [Description("Заголовок")]
  public string Caption
  {
   get { return _cap; }
   set 
   { 
    _cap = String.IsNullOrEmpty(value) ? null : value; 
    if(_cap != null)
     slText.Text = _cap;
   }
  }
  /// <summary>
  /// Значение
  /// </summary>
  [Browsable(false)]
  [DefaultValue(null)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public object Value
  {
   get 
   {
    if(_binding.IsDefined)
     return _binding.GetValue();
    if(_pd != null && _obj != null)
     return _pd.GetValue(_obj);
    return _val;
   }
   set 
   {
    try
    {
     if(_binding.IsDefined)
      _binding.SetValue(value);
     else if(_pd != null && _obj != null)
      _pd.SetValue(_obj, value);
     else
      _val = value == null || (value is string && String.IsNullOrEmpty((string)value)) ? null : value;
     OnValueChanged(value);
    }
    catch { }
    slValue.Text = GetValueString(Value);
   }
  }
  /// <summary>
  /// Правило для символов ввода, используемое встроенным редактором.
  /// </summary>
  [Description("Правило для символов ввода, используемое встроенным редактором.")]
  [DefaultValue(typeof(TextBoxFormat), "NotSet")]
  [Category("Own props")]
  public TextBoxFormat InputFormat
  {
   get { return _format; }
   set { _format = value; }
  }
  /// <summary>
  /// Исключения из правила для символов ввода, используемое встроенным редактором.
  /// </summary>
  [DefaultValue("")]
  [Description("Исключения из правила для символов ввода, используемое встроенным редактором.")]
  [Category("Own props")]
  public string InputFormatExceptions
  {
   get { return _formatExceptions; }
   set { _formatExceptions = value; }
  }
  /// <summary>
  /// Формат преобразования к строке.
  /// </summary>
  [Description("Формат преобразования к строке.")]
  [DefaultValue("")]
  [Category("Own props")]
  public string OutputFormat
  {
   get { return _outFormat; }
   set { _outFormat = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Биндинг
  /// </summary>
  [Category("Own props")]
  [Description("Биндинг")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public ObjectBinding Binding
  {
   get { return _binding; }
  }
  /// <summary>
  /// Дискриптор свойства
  /// </summary>
  [Browsable(false)]
  [DefaultValue(null)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public PropertyDescriptor PropDescriptor
  {
   get { return _pd; }
   set 
   { 
    _pd = value;
    if(_pd != null) 
     slText.Text = _pd.DisplayName;
    slValue.Text = GetValueString(Value);
   }
  }
  /// <summary>
  /// Объект данных
  /// </summary>
  [Browsable(false)]
  [DefaultValue(null)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public object DataObject
  {
   get { return _obj; }
   set 
   { 
    if(_obj != null && _obj is ObjectChangeNotify)
     ((ObjectChangeNotify)_obj).ObjectChanged -= slValue_MouseLeave; 
    _obj = value; 
    if(_obj != null && _obj is ObjectChangeNotify)
     ((ObjectChangeNotify)_obj).ObjectChanged += (s, e) => slValue.Text = GetValueString(Value);
    slValue.Text = GetValueString(Value);
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет режим только для чтения
  /// </summary>
  [Description("Определяет режим только для чтения")]
  [Category("Own props")]
  [DefaultValue(false)]
  public bool IsReadOnly
  {
   get { return _isReadOnly; }
   set 
   { 
    if(_isReadOnly == value)
     return;
    _isReadOnly = value;
    if(_isReadOnly)
    {
     slValue.Tag = slValue.Image;
     slValue.Image = null;
    }
    else
     slValue.Image = (Image)slValue.Tag;
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  [DefaultValue(true)]
  public override bool AutoSize
  {
   get { return base.AutoSize; }
   set { base.AutoSize = value; }
  }
  [DefaultValue(typeof(Color), "Transparent")]
  public override Color BackColor
  {
   get { return base.BackColor; }
   set { base.BackColor = value; }
  }
  [DefaultValue(typeof(AutoSizeMode), "GrowAndShrink")]
  public override AutoSizeMode AutoSizeMode
  {
   get { return base.AutoSizeMode; }
   set { base.AutoSizeMode = value; }
  }
  [DefaultValue(typeof(Size), "0,18")]
  public override Size MinimumSize
  {
   get { return base.MinimumSize; }
   set { base.MinimumSize = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimPropertyEditor()	: base()
  {
   InitializeComponent();
   _binding.OnBindingChanged = OnBindingChanged;
   _binding.OnSourceValueChanged = OnSourceValueChanged;
  }
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public SimPropertyEditor(PropertyDescriptor pd, object obj) : this()
  {
   if(pd == null)
    throw new ArgumentNullException("pd");
   PropDescriptor = pd;
   DataObject = obj;
  }
  //-------------------------------------------------------------------------------------
  #region Component Designer generated code
  private System.ComponentModel.IContainer components = null;
  /// <summary> 
  /// Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
   if(disposing && (components != null))
   {
    components.Dispose();
   }
   base.Dispose(disposing);
  }
  /// <summary> 
  /// Required method for Designer support - do not modify 
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
   this.slText = new Sim.Controls.SimLabel();
   this.slValue = new Sim.Controls.SimLabel();
   this.SuspendLayout();
   // 
   // slText
   // 
   this.slText.AutoSize = true;
   this.slText.Dock = System.Windows.Forms.DockStyle.Left;
   this.slText.Location = new System.Drawing.Point(0, 0);
   this.slText.MinimumSize = new System.Drawing.Size(30, 18);
   this.slText.Name = "slText";
   this.slText.Size = new System.Drawing.Size(54, 18);
   this.slText.Text = "Заголовок";
   // 
   // slValue
   // 
   this.slValue.BackColor = System.Drawing.Color.Ivory;
   this.slValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.slValue.Dock = System.Windows.Forms.DockStyle.Fill;
   this.slValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
   this.slValue.Location = new System.Drawing.Point(54, 0);
   this.slValue.MinimumSize = new System.Drawing.Size(30, 18);
   this.slValue.Image = global::Sim.Controls.Properties.Resources.Edit4;
   this.slValue.Name = "slValue";
   this.slValue.Size = new System.Drawing.Size(75, 18);
   this.slValue.Text = null;
   this.slValue.MouseLeave += new System.EventHandler(this.slValue_MouseLeave);
   this.slValue.MouseMove += new System.Windows.Forms.MouseEventHandler(this.slValue_MouseMove);
   this.slValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.slValue_MouseUp);
   // 
   // SimPropertyEditor
   // 
   this.AutoSize = true;
   this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
   this.BackColor = System.Drawing.Color.Transparent;
   this.Controls.Add(this.slValue);
   this.Controls.Add(this.slText);
   this.MinimumSize = new System.Drawing.Size(0, 18);
   this.Name = "SimPropertyEditor";
   this.Size = new System.Drawing.Size(129, 18);
   this.ResumeLayout(false);
   this.PerformLayout();
  }
  #endregion
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Controls Handlers>>
  private void slValue_MouseMove(object sender, MouseEventArgs e)
  {
   if(slValue.Image == null)
    return;
   Rectangle r = slValue.ClientRectangle;
   r.X = r.Width - slValue.Image.Width;
   r.Width = slValue.Image.Width;
   if(r.Contains(e.Location))
    slValue.Cursor = Cursors.Hand;
   else
    slValue.Cursor = Cursors.Default;
  }
  private void slValue_MouseLeave(object sender, EventArgs e)
  {
   slValue.Cursor = Cursors.Default;
  }
  private void slValue_MouseUp(object sender, MouseEventArgs e)
  {
   if(slValue.Cursor != Cursors.Hand || e.Button != MouseButtons.Left)
    return;
   try
   {
    ToolStripDropDown ctrl = null;
    var x = OnEditButtonClick();
    if(x.Cancel)
     return;
    if(x.Object != null)
     ctrl = x.Object;
    else
     ctrl = GetEditor();
    if(ctrl == null)
     return;
    Point p = slValue.PointToScreen(new Point(slValue.Width, slValue.Height));
    p.X -= ctrl.Width;
    ctrl.Show(p);
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  void String_InputDone(SimQuickEdit sender, bool args)
  {
   sender.InputDone -= String_InputDone;
   if(args)
    Value = sender.Value;
  }
  void Number_InputDone(SimQuickEdit sender, bool args)
  {
   sender.InputDone -= Number_InputDone;
   if(args)
   {
    Type t = (Type)sender.Tag;
    if(t == null)
     Value = sender.Value;//Convert.ChangeType(, _pd.PropertyType));
    else
    try
    {
     TypeConverter td = TypeDescriptor.GetConverter(t);
     if(td.CanConvertFrom(typeof(string)))
      Value = td.ConvertFromString(sender.Value.AsNumeric());
    }
    catch(InvalidCastException) { }
    catch(FormatException) { }
    catch(OverflowException) { }
    catch(ArgumentNullException) { }
   }
  }
  void Enum_InputDone(SimSelectList sender, object val)
  {
   sender.ItemSelected -= Enum_InputDone;
   object oldval = Value;
   if(Object.Equals(oldval, val) == false)
    Value = ((IComboBoxItem)val).Key;
  }
  void DateTime_Closed(object sender, DateTime e)
  {
   ((SimDateSelect)sender).UIValueChanged -= DateTime_Closed;
   object oldval = Value;
   if(Object.Equals(oldval, e) == false)
    Value = e;
  }
  #endregion << Controls Handlers>>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  private ToolStripDropDown GetEditor()
  {
   Type t = null;
   if(_binding.IsDefined)
    t = _binding.PropDescriptor.PropertyType;
   if(_pd != null)
    t = _pd.PropertyType;
   else
    t = _val == null ? typeof(string) : _val.GetType();

   if(t == null)
    return null;

   if(InputFormat != TextBoxFormat.NotSet || t == typeof(string) || t == typeof(RefString))
   {
    #region
    SimQuickEdit box = new SimQuickEdit();
    box.Value = (Value ?? "").ToString();
    box.Format = InputFormat;
    box.FormatExceptions = InputFormatExceptions;
    box.InputDone += (String_InputDone);
    if(box.Width < slValue.Width)
     box.Control.Width = slValue.Width - 10;
    return box;
    #endregion
   }
   if(t.IsPrimitive || t == typeof(decimal) || t == typeof(Money) || t == typeof(MoneyPrecise) || t == typeof(Amount) || t == typeof(UInt))
   {
    #region
    SimQuickEdit box = new SimQuickEdit();
    box.Value = (Value ?? "").ToString();
    box.Format = TextBoxFormat.Digits;
    if(t == typeof(int) || t == typeof(long) || t == typeof(sbyte) || t == typeof(short) )
     box.FormatExceptions += "-";
    else if(t == typeof(decimal) || t == typeof(float) || t == typeof(double) ||
            t == typeof(Money) || t == typeof(MoneyPrecise))
     box.FormatExceptions += "-.,";
    box.InputDone += (Number_InputDone);
    if(box.Width < slValue.Width)
     box.Control.Width = slValue.Width - 10;
    box.Tag = t;
    return box;
    #endregion
   }
   else if(t.IsEnum)
   {
    #region
    object val = Value;
    List<ComboBoxItem<Enum>> list = new List<ComboBoxItem<Enum>>();
    SimSelectList box = new SimSelectList(list);
    foreach(Enum i in Enum.GetValues(t))
    {
     var ci = new ComboBoxItem<Enum>(i, EnumTypeConverter.GetItemDisplayName(i));
     list.Add(ci);
     if(i.Equals(val))
      box.SelectedItem = ci;
    }
    box.ItemSelected += Enum_InputDone;
    box.SetAutoWidth(30);
    return box;
    #endregion
   }
   else if(_pd.PropertyType == typeof(DateTime) || _pd.PropertyType == typeof(DateTime?))
   {
    #region
    object val = Value;
    SimDateSelect mc = new SimDateSelect();
    if(val != null)
     mc.Value = (DateTime)val;
    mc.UIValueChanged += new Pulsar.EventHandler<object,DateTime>(DateTime_Closed);
    return mc;
    #endregion
   }
   return null;

  }
  public override Size GetPreferredSize(Size proposedSize)
  {
   Size s1 = slText.PreferredSize;
   Size s2 = slValue.PreferredSize;
   return new Size(s1.Width + s2.Width + Padding.Horizontal, Math.Max(s1.Height, s2.Height) + Padding.Vertical);
  }
  protected override void OnAutoSizeChanged(EventArgs e)
  {
   slValue.AutoSize = AutoSize;
   base.OnAutoSizeChanged(e);
  }
  private void OnBindingChanged()
  {
   _pd = _binding.PropDescriptor;
   if(_pd == null)
    slText.Tag = true;
   else
    slText.Text = _pd.DisplayName;
  }
  private void OnSourceValueChanged(object value, bool needReread)
  {
   if(_pd == null && slText.Tag != null)
   {
    _pd = _binding.PropDescriptor;
    if(_pd != null)
    {
     slText.Text = _pd.DisplayName;
     slText.Tag = null;
    }
   }
   slValue.Text = GetValueString(value);
  }
  private string GetValueString(object value, string nullvalue = null)
  {
   if(value == null)
    return nullvalue;
   if(value is IFormattable && String.IsNullOrWhiteSpace(_outFormat) == false)
    return ((IFormattable)value).ToString(_outFormat, null);
   if(value is Enum)
    return EnumTypeConverter.GetItemDisplayName((Enum)value);
   return value.ToString();
  }
  public override void Refresh()
  {
   slValue.Text = GetValueString(Value);
   base.Refresh();
  }
  #endregion << Methods >>

 }

}

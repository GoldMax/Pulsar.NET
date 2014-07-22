using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace Sim.Controls
{
 /// <summary>
 ///  ласс контрола панели инструментов ToolStrip дл€ выбора даты.
 /// </summary>
 //[DesignerCategory("code")]
 [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
 [ToolboxBitmap(typeof(DateTimePicker))]
 [DefaultProperty("Value")]
 [DefaultEvent("ValueChangeCommitted")]
 public class SimToolStripDateTimePicker : ToolStripControlHost
 {
  private DateTimePicker pick = new DateTimePicker();
  private bool hideValueChangedEvent = false;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << public event ValueChangeCommittedEventHandler ValueChangeCommitted >>
  /// <summary>
  /// ƒелегат событи€ ValueChangeCommitted.
  /// </summary>
  /// <param name="sender">ќбъект, посылающий событие.</param>
  /// <param name="value">Ќовое значение контрола.</param>
  public delegate void ValueChangeCommittedEventHandler(object sender, DateTime value);
  /// <summary>
  /// —обытие, возникающее при выборе даты в выподающем списке пользователем.
  /// </summary>
  public event ValueChangeCommittedEventHandler ValueChangeCommitted;
  /// <summary>
  /// ¬ызывает событие ValueChangeCommitted.
  /// </summary>
  protected void OnValueChangeCommitted()
  {
   if(ValueChangeCommitted != null)
    ValueChangeCommitted(this, pick.Value);
  }
  #endregion << public event ValueChangeCommittedEventHandler ValueChangeCommitted >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// ¬озвращает или задает максимальное значение даты и времени, которые могут быть выбраны в элементе управлени€.
  /// </summary>
  [Category("DateTimePicker")]
  [Description("¬озвращает или задает максимальное значение даты и времени, которые могут быть выбраны в элементе управлени€.")]
  public DateTime MaxDate
  {
   get { return pick.MaxDate; }
   set { pick.MaxDate = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ¬озвращает или задает минимальное значение даты и времени, которые могут быть выбраны в элементе управлени€.
  /// </summary>
  [Category("DateTimePicker")]
  [Description("¬озвращает или задает минимальное значение даты и времени, которые могут быть выбраны в элементе управлени€.")]
  public DateTime MinDate
  {
   get { return pick.MinDate; }
   set { pick.MinDate = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ¬озвращает или задает значение даты/времени, назначаемое элементу управлени€.
  /// </summary>
  [Category("DateTimePicker")]
  [Description("¬озвращает или задает значение даты/времени, назначаемое элементу управлени€.")]
  public DateTime Value
  {
   get { return pick.Value; }
   set 
   {
    hideValueChangedEvent = true; 
    pick.Value = value; 
    hideValueChangedEvent = false;
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ¬озвращает или задает формат даты и времени, отображаемых в элементе управлени€.
  /// </summary>
  [Category("DateTimePicker")]
  [Description("¬озвращает или задает формат даты и времени, отображаемых в элементе управлени€.")]
  public DateTimePickerFormat Format
  {
   get { return pick.Format; }
   set { pick.Format = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ¬озвращает или задает строку пользовательского формата даты и времени.
  /// </summary>
  [Category("DateTimePicker")]
  [Description("¬озвращает или задает строку пользовательского формата даты и времени.")]
  public string CustomFormat
  {
   get { return pick.CustomFormat; }
   set { pick.CustomFormat = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  ///  онструктор по умолчанию.
  /// </summary>
  public SimToolStripDateTimePicker() : base(new Panel())
  {
   Panel p = (Panel)Control;
   
   typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                   null, p, new object[] { true });
   p.BackColor = Color.Transparent;
   p.MinimumSize = new Size(20,20);
   pick.Margin = new Padding(0);
   pick.Anchor = AnchorStyles.Left | AnchorStyles.Right;
   p.Height = pick.Height;
   p.Width = pick.Width;
   p.Controls.Add(pick);
   
   typeof(Control).InvokeMember("SetStyle", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                   null, pick, new object[] { ControlStyles.Selectable, false });
   pick.Location = new Point(0, 0);
   pick.ValueChanged += new EventHandler(pick_ValueChanged);
   pick.DropDown += new EventHandler(pick_DropDown);
   pick.CloseUp += new EventHandler(pick_CloseUp);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Handlers >>
  void pick_DropDown(object sender, EventArgs e)
  {
   hideValueChangedEvent = true;
  }
  //-------------------------------------------------------------------------------------
  void pick_ValueChanged(object sender, EventArgs e)
  {
   if(hideValueChangedEvent == false)
    OnValueChangeCommitted();
  }
  //-------------------------------------------------------------------------------------
  void pick_CloseUp(object sender, EventArgs e)
  {
   hideValueChangedEvent = false;
   OnValueChangeCommitted();
  }
  #endregion << Handlers >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ќсвобождение ресурсов.
  /// </summary>
  /// <param name="disposing"></param>
  protected override void Dispose(bool disposing)
  {
   pick.ValueChanged -= new EventHandler(pick_ValueChanged);
   pick.DropDown -= new EventHandler(pick_DropDown);
   pick.CloseUp -= new EventHandler(pick_CloseUp);
   base.Dispose(disposing);
  }    
 }
}

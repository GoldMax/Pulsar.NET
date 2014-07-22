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
 /// Класс контрола панели инструментов ToolStrip - NumericUpDown.
 /// </summary>
 [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip |
                                    ToolStripItemDesignerAvailability.StatusStrip)]
 [ToolboxBitmap(typeof(NumericUpDown))]
 [DefaultProperty("Value")]
 [DefaultEvent("ValueChanged")]
 public class SimToolStripNumericUpDown : ToolStripControlHost
 {
  private NumericUpDown box = new NumericUpDown();
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << public event EventHandler ValueChanged >>
  /// <summary>
  /// Событие, возникающее при изменении свойства Value
  /// </summary>
  public event EventHandler ValueChanged;
  /// <summary>
  /// Вызывает событие ValueChanged
  /// </summary>
  protected void OnValueChanged()
  {
   if(ValueChanged != null)
    ValueChanged(box, EventArgs.Empty);
  }
  #endregion << public event EventHandler ValueChanged >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Значение NumericUpDown контрола.
  /// </summary>
  [Category("Data")]
  [Description("Значение NumericUpDown контрола.")]
  [DefaultValue(0)]
  public decimal Value
  {
   get { return box.Value; }
   set { box.Value = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Максимальное значение
  /// </summary>
  [Category("Data")]
  [Description("Максимальное значение.")]
  [DefaultValue(100)]
  public decimal Maximun
  {
   get { return box.Maximum; }
   set { box.Maximum = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Минимальное значение
  /// </summary>
  [Category("Data")]
  [Description("Минимальное значение.")]
  [DefaultValue(0)]
  public decimal Minimum
  {
   get { return box.Minimum; }
   set { box.Minimum = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Шаг изменения значения
  /// </summary>
  [Category("Data")]
  [Description("Шаг изменения значения")]
  [DefaultValue(1)]
  public decimal Increment
  {
   get { return box.Increment; }
   set { box.Increment = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Количество знаков после запятой
  /// </summary>
  [Category("Data")]
  [Description("Количество знаков после запятой")]
  [DefaultValue(0)]
  public int DecimalPlaces
  {
   get { return box.DecimalPlaces; }
   set { box.DecimalPlaces = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Разделитель тысяч
  /// </summary>
  [Category("Data")]
  [Description("Разделитель тысяч")]
  [DefaultValue(false)]
  public bool ThousandsSeparator
  {
   get { return box.ThousandsSeparator; }
   set { box.ThousandsSeparator = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Выравнивание текста по горизонтали.
  /// </summary>
  [Category("Appearance")]
  [Description("Выравнивание текста по горизонтали.")]
  [DefaultValue(typeof(HorizontalAlignment), "Left")]
  public new HorizontalAlignment TextAlign
  {
   get { return box.TextAlign; }
   set { box.TextAlign = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Size
  /// </summary>
  public override Size Size
  {
   get { return base.Size; }
   set 
   {
    base.Size = value;
    if(Control != null)
     Control.MinimumSize = new System.Drawing.Size(value.Width, box.Height + 2);
   }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimToolStripNumericUpDown() : base(new SimPanel())
  {
   SimPanel p = (SimPanel)Control;
   p.Dock = DockStyle.Fill;
   p.BorderStyle = BorderStyle.FixedSingle;
   p.BackColor = box.BackColor;
   p.MinimumSize = new Size(40, 20);
   box.BorderStyle = BorderStyle.None;
   box.Dock = DockStyle.Fill;
   p.Controls.Add(box);
   p.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);

   typeof(Control).InvokeMember("SetStyle", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                   null, box, new object[] { ControlStyles.Selectable, false });
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// OnSubscribeControlEvents
  /// </summary>
  /// <param name="control"></param>
  protected override void OnSubscribeControlEvents(Control control)
  {
   base.OnSubscribeControlEvents(control);
   box.ValueChanged += new EventHandler(ToolStripNumericUpDown_ValueChanged);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnUnsubscribeControlEvents
  /// </summary>
  /// <param name="control"></param>
  protected override void OnUnsubscribeControlEvents(Control control)
  {
   base.OnUnsubscribeControlEvents(control);
   box.ValueChanged -= new EventHandler(ToolStripNumericUpDown_ValueChanged);
  }
  //-------------------------------------------------------------------------------------
  void ToolStripNumericUpDown_ValueChanged(object sender, EventArgs e)
  {
   OnValueChanged();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnBoundsChanged
  /// </summary>
  protected override void OnBoundsChanged()
  {
   base.OnBoundsChanged(); 
   if(Control != null)
    Control.Padding = new System.Windows.Forms.Padding(2, (this.Height - 2 - box.Height)/2, 0, 0);

  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnEnabledChanged
  /// </summary>
  /// <param name="e"></param>
  protected override void OnEnabledChanged(EventArgs e)
  {
   ((SimPanel)Control).BackColor = this.Enabled ? box.BackColor : SystemColors.Control;
   base.OnEnabledChanged(e);
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
      
 }
}

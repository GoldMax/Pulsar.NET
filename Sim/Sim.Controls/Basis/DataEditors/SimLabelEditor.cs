using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола Label с редактором
 /// </summary>
 public partial class SimLabelEditor : Sim.Controls.SimLabel
 {
  private TextBoxFormat _format = TextBoxFormat.NotSet;
  private string _formatExceptions = "";
  private bool _isReadOnly = false;
  private Image _img = null;

  #region << Events >>
  #region UIValueChanged
  /// <summary>
  /// Событие, генерируемое при изменении Value из пользовательского интерфейса.  
  /// </summary>
  public event Pulsar.EventHandler<SimLabelEditor, string> UIValueChanged;
  /// <summary>
  /// Вызывает событие UIValueChanged
  /// </summary>
  /// <param name="value">Новое значение Value.</param>
  protected void OnUIValueChanged(string value)
  {
   if(UIValueChanged != null)
    UIValueChanged(this, value);
  } 
  #endregion UIValueChanged
  #region EditButtonClick
  /// <summary>
  /// Событие, генерируемое при клике на кнопке редактирования
  /// </summary>
  public event EventHandler<CancelEventArgs> EditButtonClick;
  /// <summary>
  /// Вызывает событие EditButtonClick
  /// </summary> 
  /// <returns></returns>
  protected bool OnEditButtonClick()
  {
   CancelEventArgs arg = new CancelEventArgs();
   if(EditButtonClick != null)
    EditButtonClick(this, arg);
   return arg.Cancel;
  } 
  #endregion EditButtonClick
  #endregion << Events >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Значение 
  /// </summary>
  [Description("Значение")]
  public string Value
  {
   get { return Text; }
   set { Text = value; }
  }
  /// <summary>
  /// Правило для символов ввода, используемое встроенным редактором.
  /// </summary>
  [Description("Правило для символов ввода, используемое встроенным редактором.")]
  [DefaultValue(typeof(TextBoxFormat), "NotSet")]
  public TextBoxFormat Format
  {
   get { return _format; }
   set { _format = value; }
  }
  /// <summary>
  /// Исключения из правила для символов ввода, используемое встроенным редактором.
  /// </summary>
  [DefaultValue("")]
  [Description("Исключения из правила для символов ввода, используемое встроенным редактором.")]
  public string FormatExceptions
  {
   get { return _formatExceptions; }
   set { _formatExceptions = value; }
  }
  /// <summary>
  /// Определяет, можно ли изменять значение.
  /// </summary>
  [Description("Определяет, можно ли изменять значение.")]
  [DefaultValue(false)]
  public bool IsReadOnly
  {
   get { return _isReadOnly; }
   set 
   { 
    _isReadOnly = value; 
    base.Image = _isReadOnly ? null : _img;
    this.Cursor = Cursors.Default;
   }
  }
  public new Image Image
  {
   get { return base.Image; }
   set 
   { 
    _img = value;
    base.Image = value;
   }
  }

  #endregion << Properties >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  public SimLabelEditor() : base()
  {
   this.Image = global::Sim.Controls.Properties.Resources.DropDownArrow;
   this.ImageAlign = ContentAlignment.MiddleRight;
   this.TextAlign = ContentAlignment.MiddleRight;
   this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.BackColor = SystemColors.Window;
   this.MinimumSize = new Size(50, 17);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  protected override void OnMouseMove(MouseEventArgs e)
  {
   if(Image != null)
   {
    Rectangle r = this.ClientRectangle;
    r = new Rectangle(r.Width - this.Image.Width - this.Padding.Right - 6, 0, this.Image.Width + this.Padding.Right + 3, r.Height);
    if(r.Contains(e.Location))
     this.Cursor = Cursors.Hand;
    else
     this.Cursor = Cursors.Default;
   }
   base.OnMouseMove(e);
  }
  protected override void OnMouseClick(MouseEventArgs e)
  {
   base.OnMouseClick(e);
   if(Image != null && e.Button == System.Windows.Forms.MouseButtons.Left && OnEditButtonClick() == false)
   {
    Rectangle r = this.ClientRectangle;
    r = new Rectangle(r.Width - this.Image.Width - this.Padding.Right - 6, 0, this.Image.Width + this.Padding.Right + 3, r.Height);
    if(r.Contains(e.Location))
    {
     SimQuickEdit box = new SimQuickEdit();
     box.Value = this.Text;
     box.Format = _format;
     box.FormatExceptions = _formatExceptions;
     box.InputDone += new Pulsar.EventHandler<SimQuickEdit, bool>(box_InputDone);
     Point pp = PointToScreen(new Point(this.Width - 1, this.Height - 2));
     pp.X -= box.Width;
     box.Show(pp);
    }
   }
  }
  void box_InputDone(SimQuickEdit sender, bool args)
  {
   sender.Hide();
   if(args)
   {
    Value = sender.Value;
    OnUIValueChanged(Value);
   }
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
 }
}

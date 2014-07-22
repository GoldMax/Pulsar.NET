using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;
using Sim.Controls.WinAPI;

namespace Sim.Controls
{
 /// <summary>
 /// Класс CheckBox
 /// </summary>
 [ToolboxBitmap(typeof(CheckBox))]
 [DefaultEvent("CheckedChanged")]
 public class SimCheckBox : SimPanel
 {
  private Color checkborderColor = SystemColors.ControlDark;
  private Color checkColor = SystemColors.Window;
  private BorderStyle checkBorderStyle = BorderStyle.FixedSingle;
  private bool tabStop = true;
  private bool check = false;
  private bool over = false;
  private bool pressed = false;
  private bool autoCheck = true;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << Events >>
  #region << public event EventHandler CheckedChanged >>
  /// <summary>
  /// Событие, генерируемое при изменении значения Check`а.
  /// </summary>
  [Description("Событие, генерируемое при изменении значения Check`а.")]
  public event EventHandler CheckedChanged;
  /// <summary>
  /// Вызывает событие CheckedChanged
  /// </summary>
  protected void OnCheckedChanged()
  {
   if(CheckedChanged != null)
    CheckedChanged(this, EventArgs.Empty);
  } 
  #endregion << public event EventHandler CheckedChanged >>
  #region << public event EventHandler UICheckedChanged >>
  /// <summary>
  /// Событие, генерируемое при изменении значения Check`а из интерфейса.
  /// </summary>
  [Description("Событие, генерируемое при изменении значения Check`а из интерфейса.")]
  public event EventHandler UICheckedChanged;
  /// <summary>
  /// Вызывает событие UICheckedChanged
  /// </summary>
  protected void OnUICheckedChanged()
  {
   if(UICheckedChanged != null)
    UICheckedChanged(this, EventArgs.Empty);
  }
  #endregion << public event EventHandler UICheckedChanged >>
  #endregion << Events >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// CreateParams
  /// </summary>
  protected override CreateParams CreateParams
  {
   get
   {
    CreateParams createParams = base.CreateParams;
    createParams.ClassName = "BUTTON";
    createParams.Style |= 11;
    createParams.Style |= 5;
    return createParams;
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Текст метки
  /// </summary>
  [Browsable(true)]
  public override string Text
  {
   get { return base.Text; }
   set 
   {
    base.Text = value; 
    if(AutoSize)
     Size = GetPreferredSize(Size);
    Invalidate();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// TabStop
  /// </summary>
  [DefaultValue(true)]
  [Description("TabStop")]
  [Category("Behavior")]
  public new bool TabStop
  {
   get { return tabStop; }
   set 
   { 
    tabStop = value; 
    base.TabStop = value;
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет цвет рамки при CheckBorderStyle = FixedSingle.
  /// </summary>
  [Category("Check")]
  [Description("Определяет цвет рамки при CheckBorderStyle = FixedSingle.")]
  [DefaultValue(typeof(Color), "ControlDark")]
  public Color CheckBorderColor
  {
   get { return checkborderColor; }
   set
   {
    checkborderColor = value;
    Invalidate();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет цвет фона Check`а при CheckBorderStyle = FixedSingle.
  /// </summary>
  [Category("Check")]
  [Description("Определяет цвет фона Check`а при CheckBorderStyle = FixedSingle.")]
  [DefaultValue(typeof(Color), "Window")]
  public Color CheckBackColor
  {
   get { return checkColor; }
   set 
   {
    checkColor = value; 
    Invalidate();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет стиль рамки Check`а.
  /// </summary>
  [Category("Check")]
  [Description("Определяет стиль рамки Check`а.")]
  [DefaultValue(typeof(BorderStyle), "FixedSingle")]
  public BorderStyle CheckBorderStyle
  {
   get { return checkBorderStyle; }
   set 
   { 
    checkBorderStyle = value; 
    Invalidate();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет значение Check`а.
  /// </summary>
  [Category("Check")]
  [Description("Определяет значение Check`а.")]
  [DefaultValue(false)]
  public bool Checked
  {
   get { return check; }
   set 
   { 
    check = value; 
    Invalidate();
    OnCheckedChanged();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// AutoSize
  /// </summary>
  [DefaultValue(true)]
  public override bool AutoSize
  {
   get { return base.AutoSize; }
   set { base.AutoSize = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// AutoSizeMode
  /// </summary>
  [Browsable(false)]
  public override AutoSizeMode AutoSizeMode
  {
   get { return base.AutoSizeMode; }
   set { base.AutoSizeMode = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет возможность смены значение Check`а при клике.
  /// </summary>
  [Category("Check")]
  [Description("Определяет возможность смены значение Check`а при клике.")]
  [DefaultValue(true)]
  public bool AutoCheck
  {
   get { return autoCheck; }
   set { autoCheck = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimCheckBox() : base()
  {
   SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
            ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.StandardClick, true);    
   SetStyle(ControlStyles.ContainerControl, false);
   this.AutoSize = true;
   this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
   base.TabStop = true;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// OnPaint
  /// </summary>
  /// <param name="pevent"></param>
  protected override void OnPaint(PaintEventArgs pevent)
  {
   try
   {
    base.OnPaint(pevent);
    Graphics g = pevent.Graphics;

    bool useVS = VisualStyleInformation.IsEnabledByUser && 
                 VisualStyleInformation.IsSupportedByOS &&
                 Application.RenderWithVisualStyles;


    Rectangle r = this.ClientRectangle;

    #region Check
    if(useVS)
    {
     #region CheckBoxRenderer
     Point p = new Point(2, (r.Height - 13) /2);
     if(Enabled)
     {
      if(check)
      {
       if(pressed)
        CheckBoxRenderer.DrawCheckBox(g, p, CheckBoxState.CheckedPressed);
       else
        CheckBoxRenderer.DrawCheckBox(g, p, over ? CheckBoxState.CheckedHot : CheckBoxState.CheckedNormal);
      }
      else if(pressed)
       CheckBoxRenderer.DrawCheckBox(g, p, CheckBoxState.UncheckedPressed);
      else
       CheckBoxRenderer.DrawCheckBox(g, p, over ? CheckBoxState.UncheckedHot : CheckBoxState.UncheckedNormal);
     }
     else
      if(check)
       CheckBoxRenderer.DrawCheckBox(g, p, CheckBoxState.CheckedDisabled);
      else
       CheckBoxRenderer.DrawCheckBox(g, p, CheckBoxState.UncheckedDisabled); 
     #endregion CheckBoxRenderer
    }
    else if(checkBorderStyle == BorderStyle.Fixed3D)
    {
     #region Fixed3D
     Rectangle rch = new Rectangle(2, (r.Height - 13) /2, 13, 13);
     if(Enabled)
     {
      if(check)
       ControlPaint.DrawCheckBox(g, rch, ButtonState.Checked);
      else
       ControlPaint.DrawCheckBox(g, rch, ButtonState.Normal);
     }
     else
      if(check)
       ControlPaint.DrawCheckBox(g, rch, ButtonState.Checked | ButtonState.Inactive);
      else
       ControlPaint.DrawCheckBox(g, rch, ButtonState.Normal | ButtonState.Inactive);
     #endregion Fixed3D
    }
    else
    {
     #region FixedSingle
     Rectangle rch = new Rectangle(2, (r.Height - 13) /2, 13, 13);
     if(this.Enabled)
      using(SolidBrush b = new SolidBrush(pressed ? SystemColors.ControlLight : this.checkColor))
       g.FillRectangle(b, rch);
     else
      g.FillRectangle(SystemBrushes.Control, rch);
     if(checkBorderStyle == BorderStyle.FixedSingle)
     {
      rch.Width--;
      rch.Height--;
      using(Pen p = new Pen(checkborderColor))
       g.DrawRectangle(p, rch);
     }

     rch.X+=2;
     rch.Y+=2;
     if(check)
      g.DrawImageUnscaled(global::Sim.Controls.Properties.Resources.CheckBox_v, rch.Location);
     //else if(this.CheckState == CheckState.Indeterminate) 
     // g.DrawImageUnscaled(global::Sim.Controls.Properties.Resources.CheckBox_o, rch.Location);
     
     #endregion FixedSingle
    }
    #endregion Check

    #region Text
    StringFormat sf = new StringFormat();
    sf.Alignment = StringAlignment.Near;
    sf.FormatFlags = StringFormatFlags.NoWrap;
    sf.LineAlignment = StringAlignment.Center;
    sf.Trimming = StringTrimming.EllipsisCharacter;
    r.X += 16;
    r.Width -=16;
    if(Enabled)
     using(Brush b = new SolidBrush(this.ForeColor))
      g.DrawString(this.Text, this.Font, b, r, sf);
    else
     g.DrawString(this.Text, this.Font, SystemBrushes.GrayText, r, sf);
    if(Focused && ShowFocusCues)
     ControlPaint.DrawFocusRectangle(g, r); 
    #endregion Text
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnKeyDown
  /// </summary>
  /// <param name="e"></param>
  protected override void OnKeyDown(KeyEventArgs e)
  {
   base.OnKeyDown(e);
   if(e.KeyCode == Keys.Space)
   {
    pressed = true;
    Invalidate();
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnKeyUp
  /// </summary>
  /// <param name="e"></param>
  protected override void OnKeyUp(KeyEventArgs e)
  {
   base.OnKeyUp(e);
   if(e.KeyCode == Keys.Space)
   {
    pressed = false;
    if(autoCheck)
    {
     Checked = !Checked;
     OnUICheckedChanged();
    }
    else 
     Invalidate();
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnMouseDown
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseDown(MouseEventArgs e)
  {
   base.OnMouseDown(e);
   if(e.Button == MouseButtons.Left)
   {
    pressed = true;
    Invalidate();
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnMouseUp
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseUp(MouseEventArgs e)
  {
   base.OnMouseUp(e);
   pressed = false;
   if(autoCheck && this.ClientRectangle.Contains(e.Location))
   {
    Checked = !Checked;
    OnUICheckedChanged();
   }
   else
    Invalidate();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnGotFocus
  /// </summary>
  /// <param name="e"></param>
  protected override void OnGotFocus(EventArgs e)
  {
   base.OnGotFocus(e);
   Invalidate();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnLostFocus
  /// </summary>
  /// <param name="e"></param>
  protected override void OnLostFocus(EventArgs e)
  {
   base.OnLostFocus(e);
   Invalidate();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// GetPreferredSize
  /// </summary>
  /// <param name="proposedSize"></param>
  /// <returns></returns>
  public override Size GetPreferredSize(Size proposedSize)
  {
   Size clientSize = TextRenderer.MeasureText(this.Text, this.Font);
   Size size2 = this.SizeFromClientSize(clientSize);
   size2.Width += 25;
   size2.Height += 4;
   return (size2 + base.Padding.Size);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnMouseEnter
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseEnter(EventArgs e)
  {
   over = true;
   base.OnMouseEnter(e);
   Invalidate();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnMouseLeave
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseLeave(EventArgs e)
  {
   over = false;
   base.OnMouseLeave(e);
   Invalidate();
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
          
 }
}

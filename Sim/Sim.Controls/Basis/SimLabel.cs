using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Sim.Controls.WinAPI;

namespace Sim.Controls
{
 /// <summary>
 /// Класс метки с дополнительными возможностями.
 /// </summary>
 [ToolboxBitmap(typeof(Label))]
 public class SimLabel : Label
 {
  private Color bColor = SystemColors.ControlDark;
  private SimContextMenu contextMenuStripCopy;
  private IContainer components;
  private ToolStripMenuItem toolStripMenuItemCopy;
  private bool transparent = false;
  const int HTTRANSPARENT = -1;
  private bool wordWrap = false;
  private Color backColor2 = SystemColors.ControlDark;
  private Color backColorMid = SystemColors.ControlDark;
  private GradientMode gradientMode = GradientMode.None;
  private float backColorMiddlePosition = 0.5f;
  private bool useVisualStyleBorderColor = true;
  private bool _forceDocksize = false;
  private bool _autoSize = true;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет цвет рамки при BorderStyle = FixedSingle.
  /// </summary>
  [Category("Appearance")]
  [Description("Определяет цвет рамки при BorderStyle = FixedSingle.")]
  [DefaultValue(typeof(Color), "ControlDark")]
  public Color BorderColor
  {
   get { return bColor; }
   set
   {
    bColor = value;
    this.RefreshBorder();
   }
  }
  /// <summary>
  /// Определяет, будут ли использованы цвета схемы.
  /// </summary>
  [Category("Appearance")]
  [Description("Определяет, будут ли использованы цвета схемы.")]
  [DefaultValue(true)]
  public bool UseVisualStyleBorderColor
  {
   get { return useVisualStyleBorderColor; }
   set
   {
    useVisualStyleBorderColor = value;
    RefreshBorder();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет второй цвет градиентной заливки.
  /// </summary>
  [Category("Appearance")]
  [Description("Определяет второй цвет градиентной заливки.")]
  [DefaultValue(typeof(Color), "ControlDark")]
  public Color BackColor2
  {
   get { return backColor2; }
   set
   {
    backColor2 = value;
    Invalidate();
   }
  }
  /// <summary>
  /// Определяет внутренний цвет градиентной заливки для трехцветных заливок.
  /// </summary>
  [Category("Appearance")]
  [Description("Определяет внутренний цвет градиентной заливки для трехцветных заливок")]
  [DefaultValue(typeof(Color), "ControlDark")]
  public Color BackColorMiddle
  {
   get { return backColorMid; }
   set
   {
    backColorMid = value;
    Invalidate();
   }
  }
  /// <summary>
  /// Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок.
  /// </summary>
  [Category("Appearance")]
  [Description("Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок.")]
  [DefaultValue(0.5f)]
  public float BackColorMiddlePosition
  {
   get { return backColorMiddlePosition; }
   set
   {
    if(value < 0)
     backColorMiddlePosition = 0f;
    else if(value > 1f)
     backColorMiddlePosition = 1f;
    else
     backColorMiddlePosition = value;
    this.Refresh();
   }
  }
  /// <summary>
  /// Определяет вид градиентной заливки.
  /// </summary>
  [Category("Appearance")]
  [Description("Определяет вид градиентной заливки.")]
  [DefaultValue(typeof(GradientMode), "None")]
  public GradientMode GradientMode
  {
   get { return gradientMode; }
   set
   {
    gradientMode = value;
    Invalidate();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [DefaultValue(true)]
  public new bool AutoEllipsis
  {
   get { return true; }
   set {  }
  }
  /// <summary>
  /// Определяет перенос строк (многострочность).
  /// </summary>
  [Category("Behavior")]
  [Description("Определяет перенос строк (многострочность).")]
  [DefaultValue(false)]
  public bool WordWrap
  {
   get { return wordWrap; }
   set 
   { 
    wordWrap = value; 
    if(AutoSize)
     this.Size = PreferredSize;
    Refresh();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет прозрачность контрола для событий.
  /// </summary>
  [Category("Behavior")]
  [Description("Определяет прозрачность контрола для событий.")]
  [DefaultValue(false)]
  public bool EventsTransparent
  {
   get { return transparent; }
   set { transparent = value; }
  }
  /// <summary>
  /// CanFocus
  /// </summary>
  public new bool CanFocus
  {
   get { return false; }
  }
  /// <summary>
  /// Определяет форсирование определения размера при AutoSize=true и Dock != None по правилам докинга.
  /// </summary>
  [Category("Layout")]
  [Description("Определяет форсирование определения размера при AutoSize=true и Dock != None по правилам докинга.")]
  [DefaultValue(false)]
  public bool ForceDockSize
  {
   get { return _forceDocksize; }
   set 
   {
    if(_forceDocksize == value)
     return;
    _forceDocksize = value;
    LayoutEngine.Layout(Parent ?? this, new LayoutEventArgs(Parent, "Dock"));
   }
  }
  [DefaultValue(true)]
  public override bool AutoSize
  {
   get { return _autoSize; }
   set
   {
    if(_autoSize == value)
     return;
    _autoSize = value;
    LayoutEngine.Layout(Parent ?? this, new LayoutEventArgs(Parent, "AutoSize"));
   }
  }
  //[DefaultValue(true)]
  //public override bool AutoSize
  //{
  // get { return base.AutoSize; }
  // set
  // {
  //  base.AutoSize = value;
  // }
  //}

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет положение текта в границах контрола.
  /// </summary>
  [DefaultValue(typeof(System.Drawing.ContentAlignment),"MiddleLeft")]
  public new System.Drawing.ContentAlignment TextAlign
  {
   get { return base.TextAlign; }
   set { base.TextAlign = value; }
  }
  /// <summary>
  /// Определяет положение изображения в границах контрола.
  /// </summary>
  [DefaultValue(typeof(System.Drawing.ContentAlignment),"MiddleLeft")]
  public new System.Drawing.ContentAlignment ImageAlign
  {
   get { return base.ImageAlign; }
   set { base.ImageAlign = value; }
  }
  [DefaultValue(null)]
  public new Image Image
  {
   get { return base.Image; }
   set { base.Image = value; 	Invalidate(); }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimLabel() : base ()
  {
   this.SetStyle(ControlStyles.ResizeRedraw, true);
   this.SetStyle(ControlStyles.UserPaint, true);
   this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
   this.SetStyle(ControlStyles.DoubleBuffer, true);

   InitializeComponent();
   this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   this.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
   base.AutoEllipsis = true;
   //base.AutoSize = true;
  }
  private void InitializeComponent()
  {
   this.components = new System.ComponentModel.Container();
   this.contextMenuStripCopy = new SimContextMenu(this.components);
   this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
   this.contextMenuStripCopy.SuspendLayout();
   this.SuspendLayout();
   // 
   // contextMenuStripCopy
   // 
   this.contextMenuStripCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopy});
   this.contextMenuStripCopy.Name = "contextMenuStrip1";
   this.contextMenuStripCopy.Size = new System.Drawing.Size(147, 26);
   this.contextMenuStripCopy.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // toolStripMenuItemCopy
   // 
   this.toolStripMenuItemCopy.Image = global::Sim.Controls.Properties.Resources.Copy;
   this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
   this.toolStripMenuItemCopy.ShortcutKeyDisplayString = "";
   this.toolStripMenuItemCopy.Size = new System.Drawing.Size(146, 22);
   this.toolStripMenuItemCopy.Text = "Копировать";
   // 
   // SimLabel
   // 
   this.ContextMenuStrip = this.contextMenuStripCopy;
   this.contextMenuStripCopy.ResumeLayout(false);
   this.ResumeLayout(false);

  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="text">Текст метки.</param>
  public SimLabel(string text) : this()
  {
   this.Text = text;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Overrides Methods >>
  ///// <summary>
  ///// 
  ///// </summary>
  ///// <param name="e"></param>
  //protected override void OnMouseClick(MouseEventArgs e)
  //{
  // if(e.Button == MouseButtons.Right && this.Text != null && this.Text.Length > 0)
  //  contextMenuStrip1.Show(this, e.Location);
  // base.OnMouseClick(e);
  //}
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="m"></param>
  protected override void WndProc(ref Message m)
  {
   if(transparent && DesignMode == false && m.Msg == WM.NCHITTEST)
    m.Result = (IntPtr)HTTRANSPARENT;
   else if(m.Msg == WM.NCPAINT && this.BorderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
   {
    IntPtr hdc = IntPtr.Zero;
    try
    {
     hdc = APIWrappers.GetWindowDC(this.Handle);
     if(hdc != IntPtr.Zero)
      using(Graphics g = Graphics.FromHdc(hdc))
       OnNonClientPaint(g);
    }
    catch(Exception ex)
    {
     throw ex;
    }
    finally
    {
     if(hdc != IntPtr.Zero)
      APIWrappers.ReleaseDC(this.Handle, hdc);
    } 
    return;
   }
   else
    base.WndProc(ref m);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnPaint(PaintEventArgs e)
  {
   if(gradientMode == GradientMode.None)
    using(SolidBrush b = new SolidBrush(this.BackColor))
      e.Graphics.FillRectangle(b, e.ClipRectangle);
   else
   {
    if((int)gradientMode < 10)
    {
     using(LinearGradientBrush b = new LinearGradientBrush(this.ClientRectangle, this.BackColor, this.BackColor2,
                                                            (LinearGradientMode)(int)gradientMode))
      e.Graphics.FillRectangle(b, e.ClipRectangle);
    }
    else
    {
     LinearGradientBrush b = new LinearGradientBrush(this.ClientRectangle, this.BackColor, this.BackColor2,
                                                           (LinearGradientMode)((int)gradientMode - 10));
     ColorBlend cb = new ColorBlend(3);
     cb.Colors = new Color[] { this.BackColor, this.backColorMid, this.BackColor2 };
     cb.Positions = new float[] { 0.0f, backColorMiddlePosition, 1.0f };
     b.InterpolationColors = cb;
     e.Graphics.FillRectangle(b, e.ClipRectangle);
     b.Dispose();
    }
   }

   Color textColor = this.Enabled ? this.ForeColor : SystemColors.GrayText;
   Rectangle r = base.ClientRectangle;
   r.X += Padding.Left;
   r.Y += Padding.Top;
   r.Width -= Padding.Horizontal;
   r.Height -= Padding.Vertical;

   Size s = TextRenderer.MeasureText(e.Graphics, this.Text, this.Font, r.Size);

   Image image = this.Image;
   if(image != null)
    lock(image)
    {
     if(ImageAlign == ContentAlignment.MiddleCenter)
     {
      Rectangle ir = r;
      ir.Y += ir.Height/2 - (image.Height + s.Height + 3)/2;
      this.DrawImage(e.Graphics, image, ir, ContentAlignment.TopCenter);
      r.Y = ir.Y + image.Height + 3;
      r.Height -= r.Y;
     }
     else
      this.DrawImage(e.Graphics, image, r, base.RtlTranslateAlignment(this.ImageAlign));

     if(ImageAlign == ContentAlignment.BottomLeft ||
        ImageAlign == ContentAlignment.MiddleLeft ||
        ImageAlign == ContentAlignment.TopLeft)
     {   
      r.X += image.Width + 3;
      r.Width -= image.Width + 3;
     } 
     else if(ImageAlign == ContentAlignment.BottomRight ||
        ImageAlign == ContentAlignment.MiddleRight ||
        ImageAlign == ContentAlignment.TopRight)
      r.Width -= image.Width + 3;
     else if(ImageAlign == ContentAlignment.TopCenter)
     {
      r.Y += image.Height + 3;
      r.Height -= image.Height + 3;
     } 
     else if(ImageAlign == ContentAlignment.BottomCenter)
      r.Height -= image.Height + 3;
    }

   //e.Graphics.DrawRectangle(Pens.Red, r);

   TextFormatFlags tf = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
   ContentAlignment t = base.TextAlign;
   if(t==ContentAlignment.MiddleCenter||t==ContentAlignment.MiddleLeft||t==ContentAlignment.MiddleRight)
   {
    tf = tf | TextFormatFlags.VerticalCenter;
    //r.X -= 2;
   } 
   if(t == ContentAlignment.BottomCenter||t == ContentAlignment.BottomLeft||t == ContentAlignment.BottomRight)
    tf = tf | TextFormatFlags.Bottom;
   if(t==ContentAlignment.BottomCenter||t==ContentAlignment.MiddleCenter||t==ContentAlignment.TopCenter)
    tf = tf | TextFormatFlags.HorizontalCenter;
   if(t==ContentAlignment.BottomRight||t==ContentAlignment.MiddleRight||t==ContentAlignment.TopRight)
    tf = tf | TextFormatFlags.Right;
   
   if(wordWrap) 
    tf |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl; 

   TextRenderer.DrawText(e.Graphics, this.Text, this.Font, r, textColor, tf);

   bool showToolTip = (r.Width < s.Width) || (r.Height < s.Height);

   typeof(Label).GetField("showToolTip", BindingFlags.Instance |
                                         BindingFlags.NonPublic|
                                         BindingFlags.SetField).SetValue(this, showToolTip);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="proposedSize"></param>
  /// <returns></returns>
  public override Size GetPreferredSize(Size proposedSize)
  {
   try
   {
    Size s = Size.Empty;
    if(wordWrap)
     s = base.GetPreferredSize(proposedSize);
    else if(Dock != DockStyle.None && Parent != null && _forceDocksize)
    {
     s = TextRenderer.MeasureText(this.Text, this.Font);
     if(Dock == DockStyle.Left || Dock == DockStyle.Right || Dock == DockStyle.Fill)
      s.Height = Parent.Height - (BorderStyle != BorderStyle.None ? 2 : 0);
     else
      s.Height += Padding.Vertical + (BorderStyle != BorderStyle.None ? 2 : 0);

     if(Dock == DockStyle.Top || Dock == DockStyle.Bottom || Dock == DockStyle.Fill)
      s.Width = Parent.Width - (BorderStyle != BorderStyle.None ? 2 : 0);
     else
      s.Width += Padding.Horizontal + (BorderStyle != BorderStyle.None ? 2 : 0);
    }
    else
    {
     s = TextRenderer.MeasureText(this.Text, this.Font, proposedSize);
     s.Width += Padding.Horizontal + (BorderStyle != BorderStyle.None ? 2 : 0);
     s.Height += Padding.Vertical + (BorderStyle != BorderStyle.None ? 2 : 0);
    }
    if(Image != null)
    {
     if(ImageAlign == ContentAlignment.BottomCenter || ImageAlign == ContentAlignment.TopCenter)
     {
      s.Width = s.Width > Image.Width ? s.Width : Image.Width;
      s.Height += Image.Height;
     }
     else if(ImageAlign == ContentAlignment.MiddleCenter)
     {
      s.Width = s.Width > Image.Width ? s.Width : Image.Width;
      s.Height = s.Height > Image.Height ? s.Height : Image.Height;
     }
     else //if(ImageAlign != ContentAlignment.MiddleCenter)
     {
      s.Width += Image.Width + 3;
      s.Height = s.Height > Image.Height ? s.Height : Image.Height;
      if(BorderStyle != BorderStyle.None)
       s.Height += 2;
     }
    }
    if(MinimumSize.Width != 0 && s.Width < MinimumSize.Width)
     s.Width = MinimumSize.Width;
    if(MaximumSize.Width != 0 && s.Width > MaximumSize.Width)
     s.Width = MaximumSize.Width;
    if(MinimumSize.Width != 0 && s.Height < MinimumSize.Height)
     s.Height = MinimumSize.Height;
    if(MaximumSize.Width != 0 && s.Height > MaximumSize.Height)
     s.Height = MaximumSize.Height;
    return s;
   }
   catch
   {
    
    throw;
   }
   finally
   {

   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Осуществляет рисование в неклиенской области.
  /// </summary>
  /// <param name="g"></param>
  protected void OnNonClientPaint(Graphics g)
  {
   bool useVS = System.Windows.Forms.VisualStyles.VisualStyleInformation.IsEnabledByUser && 
                System.Windows.Forms.VisualStyles.VisualStyleInformation.IsSupportedByOS &&
                 Application.RenderWithVisualStyles &&
                 useVisualStyleBorderColor;


   Rectangle r = Rectangle.Round(g.VisibleClipBounds);
   SolidBrush b;
   if(useVS)
    b = new SolidBrush(System.Windows.Forms.VisualStyles.VisualStyleInformation.TextControlBorder);
   else
    b = new SolidBrush(this.BorderColor);
   using(b)  //
   {
    g.FillRectangle(b, 0, 0, r.Width, 1);
    g.FillRectangle(b, r.Width - 1, 0, r.Width, r.Height);
    g.FillRectangle(b, 0, r.Height - 1, r.Width, r.Height);
    g.FillRectangle(b, 0, 0, 1, r.Height);
   }
   g.Flush();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Обновляет рисование рамки.
  /// </summary>
  public void RefreshBorder()
  {
   APIWrappers.SetWindowPos(new HandleRef(this, this.Handle), new HandleRef(), 0, 0, 0, 0,
    0x0001 | 0x0002 | 0x0004 | 0x0010 | 0x0020 | 0x0200);
   Application.DoEvents();
  }
  #endregion << Overrides Methods >>
  //-------------------------------------------------------------------------------------
  #region << ContextMenu Handlers >>
  private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
  {
   if(e.ClickedItem.Equals(toolStripMenuItemCopy) && String.IsNullOrEmpty(this.Text) == false)
   {
    Clipboard.Clear();
    Clipboard.SetText(this.Text);
   } 
  }
  #endregion << ContextMenu Handlers >>
 }
}

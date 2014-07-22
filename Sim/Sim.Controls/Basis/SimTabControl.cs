using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола TabControl
 /// </summary>
 public class SimTabControl : TabControl
 {
  private Color backColor = SystemColors.Control;
  private Color selTabBackColor = SystemColors.Control;
  private Color borderColor = SystemColors.ControlDark;
  private bool useVSborderColor = true;
  private Color selTabTextColor = SystemColors.HotTrack;
  private Color unselTabTextColor = SystemColors.ControlDarkDark;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Цвет фона
  /// </summary>
  [Category("Appearance")]
  [Description("Цвет фона")]
  [DefaultValue(typeof(Color), "Control")]
  [Browsable(true)]
  public override Color BackColor
  {
   get { return backColor; }
   set { backColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Цвет фона выбранной вкладки
  /// </summary>
  [Category("Appearance")]
  [Description("Цвет фона выбранной вкладки")]
  public Color SelectedTabBackColor
  {
   get { return selTabBackColor; }
   set { selTabBackColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Цвет рамки
  /// </summary>
  [Category("Appearance")]
  [Description("Цвет рамки")]
  [DefaultValue(typeof(Color), "ControlDark")]
  public Color BorderColor
  {
   get { return borderColor; }
   set { borderColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет использование цвета рамки визуального стиля
  /// </summary>
  [Category("Appearance")]
  [Description("Определяет использование цвета рамки визуального стиля")]
  [DefaultValue(true)]
  public bool UseVisualStyleBorderColor
  {
   get { return useVSborderColor; }
   set { useVSborderColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Цвет текста выбранной вкладки
  /// </summary>
  [Category("Appearance")]
  [Description("Цвет текста выбранной вкладки")]
  [DefaultValue(typeof(Color), "HotTrack")]
  public Color SelectedTabTextColor
  {
   get { return selTabTextColor; }
   set { selTabTextColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Цвет текста невыбранной вкладки
  /// </summary>
  [Category("Appearance")]
  [Description("Цвет текста невыбранной вкладки")]
  [DefaultValue(typeof(Color), "ControlDarkDark")]
  public Color UnSelectedTabTextColor
  {
   get { return unselTabTextColor; }
   set { unselTabTextColor = value; }
  }




  #region Hide
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  public new TabAlignment Alignment
  {
   get { return base.Alignment; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  public new TabAppearance Appearance
  {
   get { return base.Appearance; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  public new TabDrawMode DrawMode
  {
   get { return base.DrawMode; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  public new bool HotTrack
  {
   get { return false; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  public new bool Multiline
  {
   get { return false; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  public new Size ItemSize
  {
   get { return base.ItemSize; }
   set { base.ItemSize = value; }
  }
  #endregion Hide
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimTabControl() : base()
  {
   this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
   this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
   this.SetStyle(ControlStyles.ResizeRedraw, true);
   this.SetStyle(ControlStyles.UserPaint, true);
   InitializeComponent();

   this.SelectedTabBackColor = ControlPaint.Light(SystemColors.Control, 0.7f);
  }
  //-------------------------------------------------------------------------------------
  private void InitializeComponent()
  {
   this.SuspendLayout();
   // 
   // TabViewer
   // 
   this.ResumeLayout(false);
   base.Multiline = false;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnPaint(PaintEventArgs e)
  {
   try
   {
    //System.Diagnostics.Debug.Write("p");
    Graphics g = e.Graphics;
    g.SmoothingMode = SmoothingMode.AntiAlias;

    Rectangle r = this.ClientRectangle;
    if(this.Enabled)
     using(SolidBrush b = new SolidBrush(backColor))
      g.FillRectangle(b, r);
    else
     g.FillRectangle(SystemBrushes.Control, r);
      
    r.X += 1;
    r.Y += 1;
    r.Width -= 2;
    r.Height -= 2;
    Rectangle d = this.DisplayRectangle;
    d.Y -= 3;
    int arcX = 10, arcY = 10;

    GraphicsPath path = new GraphicsPath();
    path.AddArc(r.Width-arcX, d.Y, arcX, arcY, -90, 90);
    path.AddLine(r.Width, d.Y+arcY/2, r.Width, r.Height-arcY/2);
    path.AddArc(r.Width-arcX, r.Height-arcY, arcX, arcY, 0, 90);
    path.AddLine(r.Width-arcX/2, r.Height, arcX/2, r.Height);
    path.AddArc(0, r.Height-arcX, arcY, arcX, 90, 90);
    path.AddLine(0, r.Height-arcY/2, 0, d.Y+arcY/2);
    if(SelectedIndex != 0)
     path.AddArc(0, d.Y, arcX, arcY, 180, 90);
    else
     path.AddArc(0, d.Y, arcX, arcY, 180, 45);

    if(TabPages.Count == 0)
     path.AddLine(arcX/2, d.Y, r.Width - arcX/2, d.Y);
    else
    {
     // Макушка
     //GraphicsPath pathH = new GraphicsPath();
     Rectangle t = this.GetTabRect(this.SelectedIndex);
     t.Width -= 1;
     int x = t.X;
     path.AddLine(x, d.Y, x, r.Y+arcY/2);
     //pathH.AddLine(x, d.Y+2, x, r.Y+arcY/2);
     path.AddArc(x, 0, arcX, arcY, 180, 90);
     //pathH.AddArc(x, 0, arcX, arcY, 180, 90);
     path.AddLine(x+arcX/2, 0, x+t.Width-arcX, 0);
     //pathH.AddLine(x+arcX/2, 0, x+t.Width-arcX, 0);
     path.AddArc(x+t.Width-arcX, 0, arcX, arcY, -90, 90);
     //pathH.AddArc(x+t.Width-arcX, 0, arcX, arcY, -90, 90);
     path.AddLine(x+t.Width, arcY/2, x+t.Width, d.Y);
     //pathH.AddLine(x+t.Width, arcY/2, x+t.Width, d.Y+2);
     path.AddLine(x+t.Width, d.Y, r.Width-arcX/2, d.Y);

     t.Y -= 1;
     t.Height += 2;
     
     // Градиент на макушке
     //using(LinearGradientBrush b = new LinearGradientBrush(t, SystemColors.ControlLightLight,
     //                                                         SystemColors.Control,
     //                                                         LinearGradientMode.Vertical))
     // g.FillPath(b, pathH);

    }

    if(this.Enabled)
     using(SolidBrush b = new SolidBrush(SelectedTabBackColor))
      g.FillPath(b, path);
    else
     g.FillPath(SystemBrushes.Control, path);

    //------------------------------------------------
    bool useVS = VisualStyleInformation.IsEnabledByUser && 
                 VisualStyleInformation.IsSupportedByOS &&
                 Application.RenderWithVisualStyles &&
                 useVSborderColor;

    Pen p;
    if(this.Enabled == false)
     p = new Pen(SystemColors.ControlDark);
    else if(useVS)
     p = new Pen(VisualStyleInformation.TextControlBorder);
    else
     p = new Pen(borderColor);
    using(p)  
     g.DrawPath(p, path);

    Point mp = this.PointToClient(Control.MousePosition);
    for(int a = 0; a < TabPages.Count; a++)
    {
     Rectangle tt = this.GetTabRect(a);

     //Rectangle t = tt;
     //t.Width -= 28;
     //t.X += 10;
     //g.FillRectangle(Brushes.Red, t);

     StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
     sf.Alignment = StringAlignment.Center;
     sf.LineAlignment = StringAlignment.Center;
     sf.Trimming = StringTrimming.EllipsisCharacter;
     if(this.Enabled == false)
      g.DrawString(TabPages[a].Text, this.Font, SystemBrushes.GrayText, tt, sf);
     else if(this.SelectedIndex == a)
      using(SolidBrush b = new SolidBrush(selTabTextColor))
       g.DrawString(TabPages[a].Text, this.Font, b, tt, sf);
     else
      using(SolidBrush b = new SolidBrush(unselTabTextColor))
       g.DrawString(TabPages[a].Text, this.Font, b, tt, sf);
    }
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseMove(MouseEventArgs e)
  {
   //Graphics g = this.CreateGraphics();
   //try
   //{
   // TabViewerHitTestInfo t = HitTest(e);
   // if((t == null && prev == null) || (t != null && t.Equals(prev)))
   //  return;
   // if(prev != null)
   // {
   //  //System.Diagnostics.Debug.Write("c");
   //  LinearGradientBrush b;
   //  if(prev.IsSelected)
   //  {
   //   Rectangle gr = prev.TabRect;
   //   gr.Y--;
   //   gr.Height += 2;
   //   using(b = new LinearGradientBrush(gr, SystemColors.ControlLightLight,
   //                               SystemColors.Control, LinearGradientMode.Vertical))
   //    g.FillRectangle(b, new Rectangle(prev.BtnRect.X, 1, prev.BtnRect.Width, prev.TabRect.Height));
   //  }
   //  else
   //  {
   //   using(b = new LinearGradientBrush(this.ClientRectangle, SystemColors.GradientInactiveCaption,
   //                               SystemColors.ControlLightLight, LinearGradientMode.Horizontal))
   //    g.FillRectangle(b, prev.BtnRect);
   //  }
   //  if(prev.IsSelected)
   //   g.DrawImage(global::Sim.Properties.Resources.Xbutton, prev.BtnRect);
   //  prev = null;
   // }
   // if(t != null)
   // {
   //  if(t.IsButton)
   //  {
   //   if(e.Button == MouseButtons.Left)
   //    g.DrawImage(global::Sim.Properties.Resources.Xbutton_Pushed, t.BtnRect);
   //   else
   //    g.DrawImage(global::Sim.Properties.Resources.Xbutton_Raised, t.BtnRect);
   //  }
   //  else
   //   g.DrawImage(global::Sim.Properties.Resources.Xbutton, t.BtnRect);
   //  prev = t;
   //  //System.Diagnostics.Debug.Write("d");
   // }
   //}
   //catch(Exception Err)
   //{
   // Sim.Controls.ErrorBox.Show(Err);
   //}
   //finally
   //{
   // g.Dispose();
    base.OnMouseMove(e);
   //}
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseLeave(EventArgs e)
  {
   //Graphics g = this.CreateGraphics();
   //try
   //{
   // if(prev != null)
   // {
   //  //System.Diagnostics.Debug.Write("l");
   //  LinearGradientBrush b;
   //  if(prev.IsSelected)
   //  {
   //   Rectangle gr = prev.TabRect;
   //   gr.Y--;
   //   gr.Height += 2;
   //   using(b = new LinearGradientBrush(gr, SystemColors.ControlLightLight,
   //                               SystemColors.Control, LinearGradientMode.Vertical))
   //    g.FillRectangle(b, new Rectangle(prev.BtnRect.X, 1, prev.BtnRect.Width, prev.TabRect.Height));
   //  }
   //  else
   //   using(b = new LinearGradientBrush(this.ClientRectangle, SystemColors.GradientInactiveCaption,
   //                               SystemColors.ControlLightLight, LinearGradientMode.Horizontal))
   //    g.FillRectangle(b, prev.BtnRect);
   //  if(prev.IsSelected)
   //   g.DrawImage(global::Sim.Properties.Resources.Xbutton, prev.BtnRect);
   //  prev = null;
   // }
   //}
   //catch(Exception Err)
   //{
   // Sim.Controls.ErrorBox.Show(Err);
   //}
   //finally
   //{
   // g.Dispose();
    base.OnMouseLeave(e);
   //}
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseUp(MouseEventArgs e)
  {
   //if(e.Button == System.Windows.Forms.MouseButtons.Left)
   //{
   // TabViewerHitTestInfo t = HitTest(e);
   // if(t != null && t.IsButton)
   // {
   //  prev = null;
   //  TabPages.RemoveAt(t.TabIndex);
   // }
   //}
   base.OnMouseUp(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="m"></param>
  protected override void WndProc(ref Message m)
  {
   //if(m.Msg == WM.LBUTTONDOWN)
   //{
   // MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 1,
   //  (short)(m.LParam), (short)((int)m.LParam >> 16), 0);
   // TabViewerHitTestInfo t = HitTest(e);
   // if(t != null && t.IsButton)
   // {
   //  using(Graphics g = this.CreateGraphics())
   //   g.DrawImage(global::Sim.Properties.Resources.Xbutton_Pushed, t.BtnRect);
   //  return;
   // }
   // else if(prev != null)
   //  using(Graphics g = this.CreateGraphics())
   //  {
   //   LinearGradientBrush b;
   //   if(prev.IsSelected)
   //   {
   //    Rectangle gr = prev.TabRect;
   //    gr.Y--;
   //    gr.Height += 2;
   //    using(b = new LinearGradientBrush(gr, SystemColors.ControlLightLight,
   //                                SystemColors.Control, LinearGradientMode.Vertical))
   //     g.FillRectangle(b, new Rectangle(prev.BtnRect.X, 1, prev.BtnRect.Width, prev.TabRect.Height));
   //   }
   //   else
   //    using(b = new LinearGradientBrush(this.ClientRectangle, SystemColors.GradientInactiveCaption,
   //                                SystemColors.ControlLightLight, LinearGradientMode.Horizontal))
   //     g.FillRectangle(b, prev.BtnRect);
   //   if(prev.IsSelected)
   //    g.DrawImage(global::Sim.Properties.Resources.Xbutton, prev.BtnRect);
   //   prev = null;
   //  }
   //}
   base.WndProc(ref m);
  }
  #endregion << Methods >>
  //*************************************************************************************
 }

}

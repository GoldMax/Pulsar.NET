using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Sim.Controls;
using Sim.Controls.WinAPI;
using Pulsar;

namespace Sim
{
 internal class TabViewer : TabControl
 {
  private TabViewerHitTestInfo prev = null;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  /// <summary>
  /// Событие, возникающее при закрытии вкладки (нажатии кнопки X).
  /// </summary>
  public event EventHandler<TabViewer,CancelEventArgs<TabPage>> NeedPageClose;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  ///// <summary>
  ///// Цвет фона
  ///// </summary>
  //public override Color BackColor
  //{
  // get { return Color.Transparent; }
  //}
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public TabViewer() : base()
  {
   this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
   this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
   this.SetStyle(ControlStyles.ResizeRedraw, true);
   this.SetStyle(ControlStyles.UserPaint, true);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  protected override void OnPaint(PaintEventArgs e)
  {
   //base.OnPaint(e);
   GraphicsPath path = null;
   try
   {
    //System.Diagnostics.Debug.Write("p");
    Graphics g = e.Graphics;
    g.SmoothingMode = SmoothingMode.AntiAlias;
    Rectangle r = this.ClientRectangle;
    r.Y = -1;
    r.X = -1;
    r.Width += 3;
    r.Height += 1;
    using(LinearGradientBrush b = 
     new LinearGradientBrush(r, SystemColors.GradientInactiveCaption, SystemColors.ControlLightLight,
                                  LinearGradientMode.Horizontal))
     g.FillRectangle(b, r);

    r = this.ClientRectangle;
    r.X += 1;
    r.Y += 1;
    r.Width -= 2;
    r.Height -= 2;
    Rectangle d = this.DisplayRectangle;
    d.Y -= 3;
    int arcX = 10, arcY = 10;

    path = new GraphicsPath();
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

    g.FillPath(SystemBrushes.Control, path);


    if(TabPages.Count == 0)
     path.AddLine(arcX/2, d.Y, r.Width - arcX/2, d.Y);
    else 
     using(GraphicsPath pathH = new GraphicsPath())
     {
      Rectangle t = this.GetTabRect(this.SelectedIndex);
      t.Width -= 1;
      int x = t.X;
      path.AddLine(x, d.Y, x, r.Y+arcY/2);
      pathH.AddLine(x, d.Y+2, x, r.Y+arcY/2);
      path.AddArc(x, 0, arcX, arcY, 180, 90);
      pathH.AddArc(x, 0, arcX, arcY, 180, 90);
      path.AddLine(x+arcX/2, 0, x+t.Width-arcX, 0);
      pathH.AddLine(x+arcX/2, 0, x+t.Width-arcX, 0);
      path.AddArc(x+t.Width-arcX, 0, arcX, arcY, -90, 90);
      pathH.AddArc(x+t.Width-arcX, 0, arcX, arcY, -90, 90);
      path.AddLine(x+t.Width, arcY/2, x+t.Width, d.Y);
      pathH.AddLine(x+t.Width, arcY/2, x+t.Width, d.Y+2);
      path.AddLine(x+t.Width, d.Y, r.Width-arcX/2, d.Y);

      t.Y -= 1;
      t.Height += 2;
      using(LinearGradientBrush b = new LinearGradientBrush(t, SystemColors.ControlLightLight,
                                                               SystemColors.Control,
                                                               LinearGradientMode.Vertical))
       g.FillPath(b, pathH);
     }
    bool useVS = VisualStyleInformation.IsEnabledByUser && 
                 VisualStyleInformation.IsSupportedByOS &&
                 Application.RenderWithVisualStyles;

    Pen p;
    if(useVS)
     p = new Pen(VisualStyleInformation.TextControlBorder);
    else
     p = new Pen(SystemColors.ControlDarkDark);
    using(p)  //
     g.DrawPath(p, path);

    Point mp = this.PointToClient(Control.MousePosition);
    for(int a = 0; a < TabPages.Count; a++)
    {
     Rectangle tt = this.GetTabRect(a);
     Rectangle t = tt;
     t.Width -= 28;
     t.X += 10;
     //g.FillRectangle(Brushes.Red, t);
     StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
     sf.Alignment = StringAlignment.Near;
     sf.LineAlignment = StringAlignment.Center;
     sf.Trimming = StringTrimming.EllipsisCharacter;
     string text = TabPages[a] is TabViewerPage ? ((TabViewerPage)TabPages[a]).Text : TabPages[a].Text;
     if(this.SelectedIndex == a)
      g.DrawString(text, this.Font, SystemBrushes.HotTrack, t, sf);
     else
      g.DrawString(text, this.Font, SystemBrushes.ControlDarkDark, t, sf);


     if(this.SelectedIndex == a || tt.Contains(mp))
      g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton, tt.X + tt.Width - 20, tt.Y+3);
    } 
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
   finally
   {
    if(path != null)
     path.Dispose();
   }
  }
  //-------------------------------------------------------------------------------------
  protected override void OnMouseMove(MouseEventArgs e)
  {
   Graphics g = this.CreateGraphics();
   try
   {
    TabViewerHitTestInfo t = HitTest(e);
    if((t == null && prev == null) || (t != null && t.Equals(prev)))
     return;
    if(prev != null)
    {
     //System.Diagnostics.Debug.Write("c");
     LinearGradientBrush b;
     if(prev.IsSelected)
     {
      Rectangle gr = prev.TabRect;
      gr.Y --;
      gr.Height += 2;
      using(b = new LinearGradientBrush(gr, SystemColors.ControlLightLight,
                                  SystemColors.Control, LinearGradientMode.Vertical))
       g.FillRectangle(b, new Rectangle(prev.BtnRect.X, 1, prev.BtnRect.Width, prev.TabRect.Height));
     }
     else
     {
      using (b = new LinearGradientBrush(this.ClientRectangle, SystemColors.GradientInactiveCaption,
                                  SystemColors.ControlLightLight, LinearGradientMode.Horizontal))
       g.FillRectangle(b, prev.BtnRect);
     }
     if(prev.IsSelected)
      g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton, prev.BtnRect);
     prev = null;
    }
    if(t != null)
    {
     if(t.IsButton)
     {
      if(e.Button == MouseButtons.Left)
       g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton_Pushed, t.BtnRect);
      else
       g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton_Raised, t.BtnRect);
     }
     else
      g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton, t.BtnRect);
     prev = t;
     //System.Diagnostics.Debug.Write("d");
    }
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
   finally
   {
    g.Dispose();
    base.OnMouseMove(e);
   }
  }
  //-------------------------------------------------------------------------------------
  protected override void OnMouseLeave(EventArgs e)
  {
   Graphics g = this.CreateGraphics();
   try
   {
    if(prev != null)
    {
     //System.Diagnostics.Debug.Write("l");
     LinearGradientBrush b;
     if(prev.IsSelected)
     {
      Rectangle gr = prev.TabRect;
      gr.Y--;
      gr.Height += 2;
      using(b = new LinearGradientBrush(gr, SystemColors.ControlLightLight,
                                  SystemColors.Control, LinearGradientMode.Vertical))
       g.FillRectangle(b, new Rectangle(prev.BtnRect.X, 1, prev.BtnRect.Width, prev.TabRect.Height));
     }
     else
      using(b = new LinearGradientBrush(this.ClientRectangle, SystemColors.GradientInactiveCaption,
                                  SystemColors.ControlLightLight, LinearGradientMode.Horizontal))
       g.FillRectangle(b, prev.BtnRect);
     if(prev.IsSelected)
      g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton, prev.BtnRect);
     prev = null;
    }
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
   finally
   {
    g.Dispose();
    base.OnMouseLeave(e);
   }
  }
  //-------------------------------------------------------------------------------------
  protected override void OnMouseUp(MouseEventArgs e)
  {
   if(e.Button == System.Windows.Forms.MouseButtons.Left)
   {
    TabViewerHitTestInfo t = HitTest(e);
    if(t != null && t.IsButton)
    {
     CancelEventArgs<TabPage> arg = new CancelEventArgs<TabPage>(TabPages[t.TabIndex]);
     if(NeedPageClose != null)
      NeedPageClose(this, arg);
     if(arg.Cancel)
      return;

     prev = null;
     TabPages.RemoveAt(t.TabIndex);
    } 
   }
   base.OnMouseUp(e);
  }
  //-------------------------------------------------------------------------------------
  protected override void WndProc(ref Message m)
  {
   if(m.Msg == WM.LBUTTONDOWN)
   {
    MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 1,
     (short)(m.LParam), (short)((int)m.LParam >> 16), 0);
    TabViewerHitTestInfo t = HitTest(e);
    if(t != null && t.IsButton)
    {
     using(Graphics g = this.CreateGraphics())
      g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton_Pushed, t.BtnRect);
     return;
    }
    else if(prev != null)
     using(Graphics g = this.CreateGraphics())
     {
      LinearGradientBrush b;
      if(prev.IsSelected)
      {
       Rectangle gr = prev.TabRect;
       gr.Y--;
       gr.Height += 2;
       using(b = new LinearGradientBrush(gr, SystemColors.ControlLightLight,
                                   SystemColors.Control, LinearGradientMode.Vertical))
        g.FillRectangle(b, new Rectangle(prev.BtnRect.X, 1, prev.BtnRect.Width, prev.TabRect.Height));
      }
      else
       using(b = new LinearGradientBrush(this.ClientRectangle, SystemColors.GradientInactiveCaption,
                                   SystemColors.ControlLightLight, LinearGradientMode.Horizontal))
        g.FillRectangle(b, prev.BtnRect);
      if(prev.IsSelected)
       g.DrawImage(global::Sim.Shell.Properties.Resources.Xbutton, prev.BtnRect);
      prev = null;
     }
   }
   base.WndProc(ref m);
  }
  //-------------------------------------------------------------------------------------
  private TabViewerHitTestInfo HitTest(MouseEventArgs e)
  {
   for(int a = 0; a < TabPages.Count; a++)
   {
    TabViewerHitTestInfo t = new TabViewerHitTestInfo();
    Rectangle r = this.GetTabRect(a);
    if(r.Contains(e.Location) == false)
     continue;
    t.TabIndex = a;
    t.TabRect = r;
    t.IsSelected = SelectedIndex == a;
    t.BtnRect = new Rectangle(r.X + r.Width - 20, r.Y+3, 14, 14);
    t.IsButton = t.BtnRect.Contains(e.Location);
    return t;
   } 
   return null;
  }
  #endregion << Methods >>

  private void InitializeComponent()
  {
   this.SuspendLayout();
   // 
   // TabViewer
   // 
   this.Padding = new System.Drawing.Point(1, 3);
   this.ResumeLayout(false);

  }
 }
 //**************************************************************************************
 internal class TabViewerHitTestInfo
 {
  public bool IsButton { get; set; }
  public int TabIndex { get; set; }
  public bool IsSelected { get; set; }
  public Rectangle TabRect { get; set; }
  public Rectangle BtnRect { get; set; }
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public TabViewerHitTestInfo()
  {
   TabIndex = -1;
  }

  public bool Equals(TabViewerHitTestInfo obj)
  {
   if(obj == null)
    return false;
   bool res = true;
   res &= IsButton == obj.IsButton;
   res &= TabIndex == obj.TabIndex;
   res &= IsSelected == obj.IsSelected;
   res &= TabRect == obj.TabRect;
   res &= BtnRect == obj.BtnRect;
   return res;
  }
 }
 //**************************************************************************************
 internal class TabViewerPage : TabPage
 {
  private string text = "XXX";
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Тескт вкладки.
  /// </summary>
  public new string Text
  {
   get { return text; }
   set 
   { 
    text = value; 
    if(Parent != null)
     SetBaseTabText();
   }
  }
  
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public TabViewerPage() : base(" ")
  {
   this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | 
                 ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.UserPaint, true);
   // this.BackColor = Color.Transparent;
  }
  //-------------------------------------------------------------------------------------
  public TabViewerPage(string text) : base(" ")
  {
   this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | 
                 ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.UserPaint, true);
   this.text = text;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  protected override void OnPaintBackground(PaintEventArgs e)
  {
   //base.OnPaintBackground(e);
  }
  //-------------------------------------------------------------------------------------
  protected override void OnPaint(PaintEventArgs e)
  {
   //base.OnPaint(e);
   e.Graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);

   //Rectangle r = new Rectangle(0, 0, 512, 768);
   //using(LinearGradientBrush b = new LinearGradientBrush(r, this.BackColor, this.BackColor,
   //                                                    LinearGradientMode.Vertical))
   //{
   // ColorBlend cb = new ColorBlend(3);
   // cb.Colors = new Color[] { SystemColors.ControlLightLight, SystemColors.Control, SystemColors.Control };
   // cb.Positions = new float[] { 0.0f, 0.00001f, 1.0f };
   // b.InterpolationColors = cb;
   // e.Graphics.FillRectangle(b, this.ClientRectangle);
   //}


   //using(LinearGradientBrush b = 
   //  new LinearGradientBrush(this.Parent.ClientRectangle, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical))
   // e.Graphics.FillRectangle(b, this.ClientRectangle );

  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Устанавливает текст вкладки так, чтобы вкладка была нужного размера.
  /// </summary>
  private void SetBaseTabText()
  {
   try
   {
    TabControl tc = (TabControl)Parent;
    if(tc == null)
     return;

    Graphics g = tc.CreateGraphics();
    string res = " ";

    using(StringFormat sf = new StringFormat(StringFormatFlags.NoWrap))
    {
     sf.Alignment = StringAlignment.Near;
     sf.LineAlignment = StringAlignment.Center;
     sf.Trimming = StringTrimming.EllipsisCharacter;
     SizeF size = g.MeasureString(text, tc.Font, 200, sf);
     size.Width += 30;

     while(tc.GetTabRect(tc.TabPages.IndexOf(this)).Width < size.Width)
      base.Text = (res += " ");
    }
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  protected override void OnParentChanged(EventArgs e)
  {
   if(Parent != null)
    SetBaseTabText();
   base.OnParentChanged(e);
  }
  //-------------------------------------------------------------------------------------
  public override string ToString()
  {
   return String.Format("{{TabViewerPage: {{{0}}}}}", text);
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
      
 }

}

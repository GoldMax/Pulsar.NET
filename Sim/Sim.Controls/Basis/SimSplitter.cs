using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола сплиттера.
 /// </summary>
 public class SimSplitter : System.Windows.Forms.Panel
 {
  Rectangle prevRect;
  Rectangle curRect;
  private Control victim = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Возвращает или устанавливает цвет сплиттера.
  /// </summary>
  [Category("Appearance")]
  [DefaultValue(typeof(Color), "ControlDarkDark")]
  [Description("Возвращает или устанавливает цвет сплиттера.")]
  public override Color BackColor
  {
   get { return base.BackColor; }
   set { base.BackColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region Hidden Properties
  /// <summary>
  /// Не используется.
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color ForeColor
  {
   get { return base.ForeColor; }
   set { base.ForeColor = value; }
  }
  #endregion Hidden Properties
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimSplitter()
  {
   InitializeComponent();
  }
  #region InitializeComponent
  private void InitializeComponent()
  {
   this.SuspendLayout();
   // 
   // GoldSplitter
   // 
   this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
   this.Cursor = System.Windows.Forms.Cursors.HSplit;
   this.Dock = System.Windows.Forms.DockStyle.Top;
   this.Size = new System.Drawing.Size(200, 2);
   this.DockChanged += new System.EventHandler(this.SimSplitter_DockChanged);
   this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SimSplitter_MouseDown);
   this.MouseCaptureChanged += new System.EventHandler(this.SimSplitter_MouseCaptureChanged);
   this.ResumeLayout(false);

  }
  #endregion
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region Common Event Handlers
  private void SimSplitter_DockChanged(object sender, EventArgs e)
  {
   if(this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
    this.Cursor = Cursors.VSplit;
   else if(this.Dock == DockStyle.Top || this.Dock == DockStyle.Bottom)
    this.Cursor = Cursors.HSplit;
   else
    this.Cursor = Cursors.No;
  }
  #endregion Common Event Handlers
  //-------------------------------------------------------------------------------------
  #region Work Event Handlers
  private void SimSplitter_MouseCaptureChanged(object sender, EventArgs e)
  {
   try
   {
    this.MouseMove -= new MouseEventHandler(SimSplitter_MouseMove);
    if(!(prevRect.Height == 0 && prevRect.Width == 0))
     ControlPaint.FillReversibleRectangle(prevRect, Color.Black);
    prevRect = Rectangle.Empty;
    SimSplitter_DockChanged(this, null);
    if(victim == null)
     return;
    if(this.Dock == DockStyle.Left)
     victim.Width += (curRect.X - this.RectangleToScreen(ClientRectangle).X);
    else if(this.Dock == DockStyle.Top)
     victim.Height += (curRect.Y - this.RectangleToScreen(ClientRectangle).Y);
    else if(this.Dock == DockStyle.Right)
     victim.Width += (this.RectangleToScreen(ClientRectangle).X - curRect.X);
    else if(this.Dock == DockStyle.Bottom)
     victim.Height += (this.RectangleToScreen(ClientRectangle).Y - curRect.Y);
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void SimSplitter_MouseDown(object sender, MouseEventArgs e)
  {
   if(DesignMode)
    return;
   if(this.Dock == DockStyle.Fill || this.Dock == DockStyle.None)
   {
    this.Cursor = Cursors.No;
    return;
   }
   //Control prevVictim = victim;
   GetVictim();
   if(victim == null)
   {
    this.Cursor = Cursors.No;
    return;
   }
   //if(prevVictim != null && prevVictim.Equals(victim) == false)
   //{
   // victim = prevVictim;
   //}
   this.MouseMove += new MouseEventHandler(SimSplitter_MouseMove);
   prevRect = RectangleToScreen(this.ClientRectangle);
   ControlPaint.FillReversibleRectangle(prevRect, Color.Black);
  }
  //-------------------------------------------------------------------------------------
  private void SimSplitter_MouseMove(object sender, MouseEventArgs e)
  {
   if(DesignMode)
    return;
   try
   {
    if(!(prevRect.Height == 0 && prevRect.Width == 0))
     ControlPaint.FillReversibleRectangle(prevRect, Color.Black);
    curRect = RectangleToScreen(this.ClientRectangle);

    if(this.Dock == DockStyle.Top || this.Dock == DockStyle.Bottom)
    {
     if((victim.Height + e.Y) < (victim.MinimumSize.Height == 0 ? 1 : victim.MinimumSize.Height))
      curRect.Y += ((victim.MinimumSize.Height == 0 ? 1 : victim.MinimumSize.Height) - victim.Height);
     else if(victim.MaximumSize.Height > 0 && (victim.Height + e.Y) > victim.MaximumSize.Height)
      curRect.Y += (victim.MaximumSize.Height - victim.Height);
     else
      curRect.Y += e.Y;
    }
    else
    {
     if((victim.Width + e.X) < (victim.MinimumSize.Width == 0 ? 1 : victim.MinimumSize.Width))
      curRect.X += ((victim.MinimumSize.Width == 0 ? 1 : victim.MinimumSize.Width) - victim.Width);
     else if(victim.MaximumSize.Width > 0 && (victim.Width + e.X) > victim.MaximumSize.Width)
      curRect.X += (victim.MaximumSize.Width - victim.Width);
     else
      curRect.X += e.X;
    }
    if(this.Parent.RectangleToScreen(this.Parent.ClientRectangle).Contains(curRect))
    {
     ControlPaint.FillReversibleRectangle(curRect, Color.Black);
     prevRect = curRect;
    }
    else
     ControlPaint.FillReversibleRectangle(prevRect, Color.Black);
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
  #endregion Work Event Handlers
  //-------------------------------------------------------------------------------------
  private void GetVictim()
  {
   try
   {
    victim = null;
    Point c;
    if(this.Dock == DockStyle.Top || this.Dock == DockStyle.Bottom)
     c = new Point(Width / 2, 0);
    else
     c = new Point(0, Height / 2);
    c = PointToScreen(c);
    c = this.Parent.PointToClient(c);
    if(this.Dock == DockStyle.Left)
     c.X--;
    else if(this.Dock == DockStyle.Top)
     c.Y--;
    else if(this.Dock == DockStyle.Right)
     c.X += (this.Width + 1);
    else if(this.Dock == DockStyle.Bottom)
     c.Y += (this.Height + 1);
    victim = this.Parent.GetChildAtPoint(c, GetChildAtPointSkip.Invisible);
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
 }
 //**************************************************************************************
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sim.Controls
{
 /// <summary>
 /// Класс всплывающего контрола (котрола, встроенного в меню).
 /// </summary>
 public class SimPopupControl : ToolStripDropDown
 {
  private Control ctrl = null;
  private bool isResizeble = false;
  private Image gripImage = null;
  private Padding padding = new Padding(3);
  private Panel resizePanel = null;
  private bool showBorder = true;
  private Point mp = Point.Empty;
  private Control _parent = null;
  private ToolStripDropDown owner = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Отображаемый Контрол
  /// </summary>
  public Control Control
  {
   get { return ctrl; }
   set
   {
    ctrl = value;
    if(ctrl == null)
     return;

    ctrl.Disposed += (sender, e) =>
    {
     ctrl = null;
     base.Dispose(true);
    };
    ctrl.Margin = Padding.Empty;
    PrepareOpen();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет возможность изменения размера
  /// </summary>
  public bool IsResizeble
  {
   get { return isResizeble; }
   set 
   { 
    isResizeble = value;
    this.Items.Clear();
    PrepareOpen();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Padding
  /// </summary>
  public new Padding Padding
  {
   get { return padding; }
   set { padding = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет, будет ли отображаться рамка.
  /// </summary>
  public bool ShowBorder
  {
   get { return showBorder; }
   set { showBorder = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Анимация открытия
  /// </summary>
  public WinAPI.AnimationFlags ShowingAnimation { get; set; }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Анамация закрытия
  /// </summary>
  public WinAPI.AnimationFlags HidingAnimation { get; set; }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Длительность анимации.
  /// </summary>
  public int AnimationDuration { get; set; }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// CreateParams
  /// </summary>
  protected override CreateParams CreateParams
  {
   get
   {
    CreateParams cp = base.CreateParams;
    cp.ExStyle |= (int)WinAPI.ExWindowStyles.WS_EX_NOACTIVATE;
    return cp;
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Контрол, относительно которого отображается всплывающий контрол.
  /// </summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public new Control Parent
  {
   get { return _parent; }
   set { _parent = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimPopupControl()
   : base()
  {
   InitializeComponent();
   DoubleBuffered = true;
   ResizeRedraw = true;
   ShowingAnimation = WinAPI.AnimationFlags.Mask;
   HidingAnimation = WinAPI.AnimationFlags.Mask;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public SimPopupControl(Control ctrl)
   : this()
  {
   this.Control = ctrl;
  }
  //-------------------------------------------------------------------------------------
  private void InitializeComponent()
  {
   this.SuspendLayout();
   // 
   // SimPopupControl
   // 
   this.Margin = new System.Windows.Forms.Padding(0);
   //this.Size = new System.Drawing.Size(0, 0);
   this.ResumeLayout(false);

  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Public Methods >>
  /// <summary>
  /// Отображает всплывающий контрол.
  /// </summary>
  /// <param name="ctrl">Отображаемый контрол.</param>
  /// <param name="x">X координата экранная</param>
  /// <param name="y">Y координата экранная</param>
  public new void Show(Control ctrl, int x, int y)
  {
   this.Control = ctrl;
   Show(x, y);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Отображает всплывающий контрол.
  /// </summary>
  /// <param name="ctrl">Отображаемый контрол.</param>
  /// <param name="screenPoint">Позиция для отображения</param>
  public new void Show(Control ctrl, Point screenPoint)
  {
   this.Control = ctrl;
   Show(screenPoint.X, screenPoint.Y);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Отображает всплывающий контрол. Вызывает Show(int x, int y).
  /// </summary>
  /// <param name="screenPoint">Точка отображения.</param>
  public virtual new void Show(Point screenPoint)
  {
   this.Show(screenPoint.X, screenPoint.Y);
  }
  //-------------------------------------------------------------------------------------
  private new void Show(Point p, ToolStripDropDownDirection dir)
  {
  
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Отображает всплывающий контрол.
  /// </summary>
  /// <param name="ctrl">Отображаемый контрол.</param>
  /// <param name="screenPoint">Позиция для отображения</param>
  /// <param name="isResizeble">Определяет возможность изменения размера</param>
  /// <param name="showBorder">Определяет, будет ли отображаться рамка.</param>
  public static SimPopupControl Show(Control ctrl, Point screenPoint, bool isResizeble = false, bool showBorder = true)
  {
   SimPopupControl box = new SimPopupControl(ctrl);
   box.IsResizeble = isResizeble;
   box.ShowBorder = showBorder;
   box.Show(screenPoint.X, screenPoint.Y);
   return box;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Отображает всплывающий контрол.
  /// </summary>
  /// <param name="x">X координата экранная</param>
  /// <param name="y">Y координата экранная</param>
  public virtual new void Show(int x, int y)
  {
   //if(this.Items.Count == 0)
   // PrepareOpen();
   base.Show(x, y);
   this.Refresh();
  }
  //-------------------------------------------------------------------------------------
  ///// <summary>
  ///// Возвращает всплывающий конторол как контекстное меню.
  ///// </summary>
  ///// <param name="ctrl"></param>
  ///// <returns></returns>
  //public static ContextMenuStrip AsContextMenu(Control ctrl, bool isResizeble = false, bool showBorder = true)
  //{
  // ContextMenuStrip menu = new ContextMenuStrip();
  // menu.ShowCheckMargin = false;
  // menu.ShowImageMargin = false;

  // if(ctrl == null)
  //  return menu;
  // if(showBorder)
  //  base.Padding = new Padding(1);
  // else
  //  base.Padding = Padding.Empty;

  // ctrl.SizeChanged += new EventHandler(Control_SizeChanged);

  // if(isResizeble)
  // {
  //  resizePanel = new Panel();
  //  resizePanel.BackColor = ctrl.BackColor; //    Color.Red;
  //  resizePanel.Margin = Padding.Empty;
  //  resizePanel.Padding = padding;
  //  resizePanel.Controls.Add(ctrl);
  //  resizePanel.Paint += new PaintEventHandler(resizePanel_Paint);
  //  resizePanel.MouseDown += new MouseEventHandler(resizePanel_MouseDown);
  //  resizePanel.MouseMove += new MouseEventHandler(resizePanel_MouseMove);
  //  resizePanel.MouseLeave += new EventHandler(resizePanel_MouseLeave);
  //  ctrl.Location = new Point(padding.Left, padding.Top);

  //  ToolStripControlHost host = new ToolStripControlHost(resizePanel);
  //  host.Padding = Padding.Empty;
  //  host.Margin = Padding.Empty;
  //  host.AutoSize = false;
  //  this.AutoSize = false;
  //  this.Items.Add(host);
  // }
  // else
  // {
  //  Panel p = new Panel();
  //  p.BackColor = ctrl.BackColor;
  //  p.Width = ctrl.Width + padding.Horizontal;
  //  p.Height = ctrl.Height + padding.Vertical;
  //  p.Controls.Add(ctrl);
  //  ctrl.Location = new Point(padding.Left, padding.Top);

  //  ToolStripControlHost host = new ToolStripControlHost(p);
  //  host.AutoSize = false;
  //  host.Padding = Padding.Empty;
  //  host.Margin = Padding.Empty;
  //  host.Size = p.Size;
  //  this.Items.Add(host);

  //  this.Height = p.Height + base.Padding.Vertical;
  //  this.Width = p.Width + base.Padding.Horizontal;
  // }

  // Control.MinimumSize = new Size(Control.MinimumSize.Width < 30 ? 30 : Control.MinimumSize.Width,
  //                                 Control.MinimumSize.Height < 15 ? 15 : Control.MinimumSize.Height);

  // if(isResizeble)
  //  Control_SizeChanged(ctrl, EventArgs.Empty);


  //}
  #endregion << Public Methods >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// 
  /// </summary>
  void resizePanel_Paint(object sender, PaintEventArgs e)
  {
   
   if(isResizeble == false)
    return;
   if(gripImage == null)
   {
    gripImage = new Bitmap(11, 11);
    using(Graphics g = Graphics.FromImage(gripImage))
      ControlPaint.DrawSizeGrip(g, resizePanel.BackColor, 0, 0, 11, 11);
   }
   //e.Graphics.FillRectangle(Brushes.Gold, resizePanel.Width-11, resizePanel.Height-11, 11,11);
   e.Graphics.DrawImage(gripImage, resizePanel.Width-12, resizePanel.Height-11);
  }
  //-------------------------------------------------------------------------------------
  void Control_SizeChanged(object sender, EventArgs e)
  {
   Control ctrl = (Control)sender;
   if(isResizeble)
   {
    int d = (d = 13 - padding.Right - padding.Bottom) < 0 ? 0 : d;
    GraphicsPath path = new GraphicsPath();
    path.AddLine(0, 0, ctrl.Width, 0);
    path.AddLine(ctrl.Width, ctrl.Height - d, ctrl.Width - d, ctrl.Height);
    path.AddLine(0, ctrl.Height, 0, 0);
    ctrl.Region = new Region(path);

    resizePanel.Size = new Size(ctrl.Width + padding.Horizontal, ctrl.Height + padding.Vertical);
    this.Height = resizePanel.Height + base.Padding.Vertical;
    this.Width = resizePanel.Width + base.Padding.Horizontal;
   }
   else
   {
    ctrl.Parent.Width = ctrl.Width + padding.Horizontal;
    ctrl.Parent.Height = ctrl.Height + padding.Vertical;
    //this.Width = ctrl.Width + base.Padding.Horizontal + padding.Horizontal;
    //this.Height = ctrl.Height + base.Padding.Vertical + padding.Vertical;
   }
  }
  //-------------------------------------------------------------------------------------
  void resizePanel_MouseDown(object sender, MouseEventArgs e)
  {
   if(isResizeble == false)
    return;
   Rectangle r = new Rectangle(resizePanel.Width-11, resizePanel.Height-10, 11, 10);
   Point p = new Point(resizePanel.Width - e.Location.X, resizePanel.Height - e.Location.Y);
   if(r.Contains(e.Location) && p.Y + p.X <= 13 || mp != Point.Empty)
    mp = e.Location;
   else
    mp = Point.Empty;
  }
  //-------------------------------------------------------------------------------------
  void resizePanel_MouseMove(object sender, MouseEventArgs e)
  {
   if(isResizeble == false)
    return;
   Rectangle r = new Rectangle(resizePanel.Width-11, resizePanel.Height-10, 11, 10);
   Point p = new Point(resizePanel.Width - e.Location.X, resizePanel.Height - e.Location.Y);
   if(r.Contains(e.Location) && p.Y + p.X <= 13)
    resizePanel.Cursor = Cursors.SizeNWSE;
   else if(mp == Point.Empty)
    resizePanel.Cursor = Cursors.Default;

   if(Control.MouseButtons == MouseButtons.Left &&  mp != Point.Empty)
   {
    p = new Point(e.Location.X - mp.X, e.Location.Y - mp.Y);
    ctrl.Width += p.X;
    ctrl.Height += p.Y;

    this.Refresh();
    mp = e.Location;
   }
   else if(mp != Point.Empty)
    mp = Point.Empty;
  }
  //-------------------------------------------------------------------------------------
  void resizePanel_MouseLeave(object sender, EventArgs e)
  {
   if(mp == Point.Empty)
    resizePanel.Cursor = Cursors.Default;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void  OnVisibleChanged(EventArgs e)
  {
   if(this.Visible && ShowingAnimation != WinAPI.AnimationFlags.Mask)
    WinAPI.APIWrappers.AnimateWindow(this,AnimationDuration,ShowingAnimation);
   base.OnVisibleChanged(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnOpening(CancelEventArgs e)
  {
   PrepareOpen();
   base.OnOpening(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnOpened
  /// </summary>
  /// <param name="e"></param>
  protected override void OnOpened(EventArgs e)
  {
   base.OnOpened(e);
   Control.Select();
   Control.Focus();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
  {
   if(HidingAnimation != WinAPI.AnimationFlags.Mask)   //   this.Visible == false && 
    WinAPI.APIWrappers.AnimateWindow(this, AnimationDuration, HidingAnimation);
   base.OnClosing(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// По
  /// </summary>
  public void PrepareOpen()
  {
   IntPtr wnd = GetActiveWindow();
   Control c = Control.FromHandle(wnd);
   if(c == null)
    return;
   if(c is ToolStripDropDown)
   {
    owner = (ToolStripDropDown)c;
    this.OwnerItem = owner.Items[0];
   }
   if(this.Parent == null)
   {
    Point p = c.PointToClient(Control.MousePosition);
    for(Control ch; (ch = c.GetChildAtPoint(p)) != null; c = ch)
     p = ch.PointToClient(Control.MousePosition);
    this.Parent = c;
   }
   //---------
   if(this.Items.Count == 0)
   {
    if(ctrl == null)
     return;
    if(showBorder)
     base.Padding = new Padding(1);
    else
     base.Padding = Padding.Empty;

    ctrl.SizeChanged += new EventHandler(Control_SizeChanged);

    if(isResizeble)
    {
     resizePanel = new Panel();
     resizePanel.BackColor = ctrl.BackColor; //    Color.Red;
     resizePanel.Margin = Padding.Empty;
     resizePanel.Padding = padding;
     resizePanel.Controls.Add(ctrl);
     resizePanel.Paint += new PaintEventHandler(resizePanel_Paint);
     resizePanel.MouseDown += new MouseEventHandler(resizePanel_MouseDown);
     resizePanel.MouseMove += new MouseEventHandler(resizePanel_MouseMove);
     resizePanel.MouseLeave += new EventHandler(resizePanel_MouseLeave);
     ctrl.Location = new Point(padding.Left, padding.Top);

     ToolStripControlHost host = new ToolStripControlHost(resizePanel);
     host.Padding = Padding.Empty;
     host.Margin = Padding.Empty;
     host.AutoSize = false;
     this.AutoSize = false;
     this.Items.Add(host);
    }
    else
    {
     Panel p = new Panel();
     p.BackColor = ctrl.BackColor;
     p.Width = ctrl.Width + padding.Horizontal;
     p.Height = ctrl.Height + padding.Vertical;
     p.Controls.Add(ctrl);
     ctrl.Location = new Point(padding.Left, padding.Top);

     ToolStripControlHost host = new ToolStripControlHost(p);
     host.AutoSize = false;
     host.Padding = Padding.Empty;
     host.Margin = Padding.Empty;
     host.Size = p.Size;
     this.Items.Add(host);

     this.Height = p.Height + base.Padding.Vertical;
     this.Width = p.Width + base.Padding.Horizontal;
    }

    Control.MinimumSize = new Size(Control.MinimumSize.Width < 30 ? 30 : Control.MinimumSize.Width,
                                    Control.MinimumSize.Height < 15 ? 15 : Control.MinimumSize.Height);

    if(isResizeble)
     Control_SizeChanged(ctrl, EventArgs.Empty);

   }
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
  private static extern IntPtr GetActiveWindow();
 }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контекстного меню.
 /// </summary>
 public class SimContextMenu : ContextMenuStrip
 {
  private Control sourceControl = null;
  private ToolStripDropDown owner = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Контрол, для которого открывалось меню в последний раз.
  /// </summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public new Control SourceControl 
  {
   get { return sourceControl; }
   set { sourceControl = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  ///// <summary>
  ///// 
  ///// </summary>
  //protected override Padding DefaultPadding
  //{
  // get
  // {
  //  return new Padding(3,0,3,0);
  // }
  //}
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimContextMenu() : base ()  { }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  /// <param name="container"></param>
  public SimContextMenu(IContainer container) : base (container) { }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Overrides Methods >>
  /// <summary>
  /// OnOpening(CancelEventArgs e)
  /// </summary>
  /// <param name="e"></param>
  protected override void OnOpening(CancelEventArgs e)
  {
   sourceControl = base.SourceControl;
   PrepareOpen();
   base.OnOpening(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
  {
   if(owner != null)
    this.OwnerItem = null;
   base.OnClosing(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
  {
   if(owner != null)
   {
    Type t = typeof(ToolStripManager);
    Type[] tt = t.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Static);
    MethodInfo mi = tt[0].GetMethod("SetActiveToolStrip", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(ToolStrip) }, null);
    mi.Invoke(null, new[] { owner });
    owner = null;
   }
   base.OnClosed(e);
  }
  #endregion << Overrides Methods >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  private void PrepareOpen()
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
   if(sourceControl == null)
   {
    Point p = c.PointToClient(Control.MousePosition);
    for(Control ch; (ch = c.GetChildAtPoint(p)) != null; c = ch)
     p = ch.PointToClient(Control.MousePosition);
    sourceControl = c;
   }
  }
  //-------------------------------------------------------------------------------------
  [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
  private static extern IntPtr GetActiveWindow();
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
        
 }
}

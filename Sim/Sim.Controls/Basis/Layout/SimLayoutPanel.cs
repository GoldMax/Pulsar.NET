using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Sim.Controls
{
 /// <summary>
 /// Панель 
 /// </summary>
 [ProvideProperty("FlowBreak", typeof(Control))]
 [ProvideProperty("Fill", typeof(Control))]
 public class SimLayoutPanel : SimPanel, IExtenderProvider
 {
  internal Dictionary<Control, LayoutSettings> _sets = new Dictionary<Control,LayoutSettings>(0);
  internal int _prefHeight = -1;

  public override LayoutEngine LayoutEngine
  {
   get { return SimFlowLayout.Instance; }
  }
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimLayoutPanel() : base()
  {

  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region IExtenderProvider Members
  public bool CanExtend(object extendee)
  {
   if(extendee == null || extendee is Control == false) 
    return false;
   return ((Control)extendee).Parent == this;
  }
  //-------------------------------------------------------------------------------------
  [DefaultValue(false)]
  [DisplayName("FlowBreak")]
  public bool GetFlowBreak(Control control)
  {
   LayoutSettings sets = null;
   if(_sets.TryGetValue(control, out sets) == false)
    return false;
   return sets.FlowBreak;
  }
  [DisplayName("FlowBreak")]
  public void SetFlowBreak(Control control, bool value)
  {
   LayoutSettings sets = null;
   if(_sets.TryGetValue(control, out sets) == false)
    _sets.Add(control, sets = new LayoutSettings());
   sets.FlowBreak = value;
   if(sets.IsEmpty && _sets.ContainsKey(control))
    _sets.Remove(control); 
  } 

  [DefaultValue(false)]
  [DisplayName("Fill")]
  public bool GetFill(Control control)
  {
   LayoutSettings sets = null;
   if(_sets.TryGetValue(control, out sets) == false)
    return false;
   return sets.Fill;
  }
  [DisplayName("Fill")]
  public void SetFill(Control control, bool value)
  {
   LayoutSettings sets = null;
   if(_sets.TryGetValue(control, out sets) == false)
    _sets.Add(control, sets = new LayoutSettings());
   sets.Fill = value;
   if(sets.IsEmpty && _sets.ContainsKey(control))
    _sets.Remove(control); 
  } 
  #endregion
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  public override Size GetPreferredSize(Size proposedSize)
  {
   if(AutoSize)
   {
    //if(_prefHeight == -1)
    LayoutEngine.Layout(this, new LayoutEventArgs(this, "AutoSize"));
    return new Size(proposedSize.Width, _prefHeight);
   }
   return base.GetPreferredSize(proposedSize);
  }
  #endregion << Methods >>
 }
 //*************************************************************************************
 internal class LayoutSettings
 {
  public static LayoutSettings Empty = new LayoutSettings();

  public bool FlowBreak = false;
  public bool Fill = false;

  public bool IsEmpty
  {
   get { return FlowBreak == false && Fill == false; }
  }
 }
 //*************************************************************************************
 internal class SimFlowLayout : LayoutEngine 
 {
  internal static readonly SimFlowLayout Instance = new SimFlowLayout();
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  protected SimFlowLayout() : base()
  {

  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  public override void InitLayout(object child, BoundsSpecified specified)
  {
   base.InitLayout(child, specified);
  }
  public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
  {
   SimLayoutPanel panel = container as SimLayoutPanel;
   if(panel == null)
    return false;

   Rectangle panelDisplayRectangle = panel.DisplayRectangle;
   Point pLoc = panelDisplayRectangle.Location;
   Dictionary<Control, LayoutSettings> sets = ((SimLayoutPanel)panel)._sets;
   int lineHeight = 0, lineY = panelDisplayRectangle.Top;

   int w = 0, h = 0;
   foreach(Control c in panel.Controls.Cast<Control>().Reverse()) 
   {
    if(!c.Visible)
     continue;
    LayoutSettings set;
    sets.TryGetValue(c, out set);
    if(set == null)
     set = LayoutSettings.Empty;

    if(c.AutoSize)
     c.Size = c.GetPreferredSize(new Size(panelDisplayRectangle.Width - c.Margin.Horizontal - pLoc.X, c.Height));
    else if(set.Fill)
     c.Size = new Size(panelDisplayRectangle.Width - pLoc.X - c.Margin.Horizontal, c.Height);

    w = c.Width;
    h = c.Height + c.Margin.Vertical;

    if(w + c.Margin.Horizontal > panelDisplayRectangle.Width - pLoc.X)
    {
     pLoc.X = panelDisplayRectangle.X;
     lineY += lineHeight;
     lineHeight = 0;
     if(set.Fill)
     {
      c.Size = new Size(panelDisplayRectangle.Width - c.Margin.Horizontal, c.Height);
     } 
    }
    if(lineHeight < h)
     lineHeight = h;

    pLoc.X += c.Margin.Left;
    pLoc.Y = lineY + c.Margin.Top;
    c.Location = pLoc;
    pLoc.X += w + c.Margin.Left;
    
    if(set.FlowBreak || set.Fill)
    {
     pLoc.X = panelDisplayRectangle.X;
     lineY += lineHeight;
     lineHeight = 0;
    }
   }
   panel._prefHeight = lineY + lineHeight;
   return false;
  }
 } 
}

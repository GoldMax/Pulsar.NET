using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс перегруженного ToolStrip
 /// </summary>
 public class SimToolStrip : ToolStrip
 {
  SimToolStripColorTable ct = new SimToolStripColorTable();
  ToolStripProfessionalRenderer render = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Цвета SimToolStrip
  /// </summary>
  [Description("Цвета SimToolStrip")]
  public SimToolStripColorTable ColorTable 
  { 
   get { return ct; }
   set 
   { 
    ct = value;
    ct.ColorChanged += new EventHandler(ct_ColorChanged);
    render = new ToolStripProfessionalRenderer(ct);
    base.Invalidate();
   }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimToolStrip() : base() 
  {
   ct.ColorChanged += new EventHandler(ct_ColorChanged);
   render = new ToolStripProfessionalRenderer(ct);
   base.GripStyle = ToolStripGripStyle.Hidden;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public SimToolStrip(params ToolStripItem[] items) : base (items)
  {
   ct.ColorChanged += new EventHandler(ct_ColorChanged);
   render = new ToolStripProfessionalRenderer(ct);
   base.GripStyle = ToolStripGripStyle.Hidden;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  void ct_ColorChanged(object sender, EventArgs e)
  {
   this.Refresh();
   this.Invalidate();
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  #region << Override Methods >>
  /// <summary>
  /// OnPaintBackground
  /// </summary>
  /// <param name="e"></param>
  protected override void OnPaintBackground(PaintEventArgs e)
  {
   ToolStripProfessionalRenderer r = (ToolStripProfessionalRenderer)ToolStripManager.Renderer; 
   ToolStripManager.Renderer = render;
   base.OnPaintBackground(e);
   ToolStripManager.Renderer = r;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnPaint(PaintEventArgs e)
  {
   ToolStripProfessionalRenderer r = (ToolStripProfessionalRenderer)ToolStripManager.Renderer; 
   ToolStripManager.Renderer = render;
   base.OnPaint(e);
   ToolStripManager.Renderer = r;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnRendererChanged
  /// </summary>
  /// <param name="e"></param>
  protected override void OnRendererChanged(EventArgs e)
  {
   //base.OnRendererChanged(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnEnabledChanged(EventArgs e)
  {
   base.OnEnabledChanged(e);
   if(Enabled)
   {
    foreach(ToolStripItem i in this.Items)
     if(i.Selected)
     {
      MethodInfo mi = typeof(ToolStripItem).GetMethod("Unselect",BindingFlags.NonPublic|BindingFlags.Instance);
      if(mi != null)
       mi.Invoke(i, null);
     }
   }
  }
  #endregion << Override Methods >>
  //-------------------------------------------------------------------------------------
      
 }
 //**************************************************************************************
 #region << public class SimToolStripColorTable : ProfessionalColorTable >>
 /// <summary>
 /// Класс цветовой схемы SimToolStrip
 /// </summary>
 [TypeConverter(typeof(ExpandableObjectConverter))]
 public class SimToolStripColorTable : ProfessionalColorTable
 {
  private Color toolStripGradientBegin = Color.Transparent;
  private Color toolStripGradientMiddle = Color.Transparent;
  private Color toolStripGradientEnd = Color.Transparent;
  private Color toolStripBorder = Color.Transparent;
  private Color overflowButtonGradientBegin = Color.Transparent;
  private Color overflowButtonGradientMiddle = Color.Transparent;
  private Color overflowButtonGradientEnd = Color.Transparent;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << public event EventHandler ColorChanged >>
  /// <summary>
  /// Событие смены цвета.
  /// </summary>
  public event EventHandler ColorChanged;
  /// <summary>
  /// 
  /// </summary>
  protected void OnColorChanged()
  {
   if(ColorChanged != null)
    ColorChanged(this, EventArgs.Empty);
  }
  #endregion << public event EventHandler ColorChanged >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color ToolStripGradientBegin
  {
   get { return toolStripGradientBegin; }
   //set { toolStripGradientBegin = value; } 
  }
  /// <summary>
  /// 
  /// </summary>
  [Browsable(true)]
  [NotifyParentProperty(true)]
  [DefaultValue(typeof(Color), "Transparent")]
  public Color ToolStrip_GradientBegin
  {
   get { return toolStripGradientBegin; }
   set 
   { 
    toolStripGradientBegin = value; 
    OnColorChanged();
   } 
  }

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color ToolStripGradientMiddle
  {
   get { return toolStripGradientMiddle; }
  }
  /// <summary>
  /// 
  /// </summary>
  [Browsable(true)]
  [NotifyParentProperty(true)]
  [DefaultValue(typeof(Color), "Transparent")]
  public Color ToolStrip_GradientMiddle
  {
   get { return toolStripGradientMiddle; }
   set { toolStripGradientMiddle = value; OnColorChanged();  }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color ToolStripGradientEnd
  {
   get { return toolStripGradientEnd; }
  }
  /// <summary>
  /// 
  /// </summary>
  [Browsable(true)]
  [NotifyParentProperty(true)]
  [DefaultValue(typeof(Color), "Transparent")]
  public Color ToolStrip_GradientEnd
  {
   get { return toolStripGradientEnd; }
   set { toolStripGradientEnd = value; OnColorChanged(); }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color ToolStripBorder
  {
   get { return toolStripBorder; }
  }
  /// <summary>
  /// 
  /// </summary>
  [Browsable(true)]
  [NotifyParentProperty(true)]
  [DefaultValue(typeof(Color), "Transparent")]
  public Color ToolStrip_Border
  {
   get { return toolStripBorder; }
   set { toolStripBorder = value; OnColorChanged(); }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color OverflowButtonGradientBegin
  {
   get { return overflowButtonGradientBegin; }
  }
  /// <summary>
  /// 
  /// </summary>
  [Browsable(true)]
  [NotifyParentProperty(true)]
  [DefaultValue(typeof(Color), "Transparent")]
  public Color OverflowButton_GradientBegin
  {
   get { return overflowButtonGradientBegin; }
   set { overflowButtonGradientBegin = value; OnColorChanged(); }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color OverflowButtonGradientMiddle
  {
   get { return overflowButtonGradientMiddle; }
  }
  /// <summary>
  /// 
  /// </summary>
  [Browsable(true)]
  [NotifyParentProperty(true)]
  [DefaultValue(typeof(Color), "Transparent")]
  public Color OverflowButton_GradientMiddle
  {
   get { return overflowButtonGradientMiddle; }
   set { overflowButtonGradientMiddle = value; OnColorChanged(); }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override Color OverflowButtonGradientEnd
  {
   get { return overflowButtonGradientEnd; }
  }
  /// <summary>
  /// 
  /// </summary>
  [Browsable(true)]
  [NotifyParentProperty(true)]
  [DefaultValue(typeof(Color), "Transparent")]
  public Color OverflowButton_GradientEnd
  {
   get { return overflowButtonGradientEnd; }
   set { overflowButtonGradientEnd = value; OnColorChanged(); }
  }

  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimToolStripColorTable() : base()
  {
   //toolStripGradientBegin = base.ToolStripGradientBegin;
   //toolStripGradientMiddle = base.ToolStripGradientMiddle;
   //toolStripGradientEnd = base.ToolStripGradientEnd;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
      
 }
 #endregion << public class SimToolStripColorTable : ProfessionalColorTable >>
}

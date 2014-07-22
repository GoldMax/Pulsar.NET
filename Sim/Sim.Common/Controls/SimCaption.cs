using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс заголовка-разграничителя
 /// </summary>
 public partial class SimCaption : UserControl
 {

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Текст заголовка
  /// </summary>
  [Category("Appearance")]
  [Description("Текст заголовка.")]
  [Browsable(true)]
  [DefaultValue("")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
  public new string Text
  {
   get { return finistLabel1.Text; }
   set { finistLabel1.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Основной цвет заголовка
  /// </summary>
  [Category("Appearance")]
  [Description("Основной цвет заголовка.")]
  [DefaultValue(typeof(Color), "ControlLightLight")]
  public Color CaptionBackColor
  {
   get { return finistLabel1.BackColorMiddle; }
   set { finistLabel1.BackColorMiddle = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Изображение заголовка
  /// </summary>
  [Category("Appearance")]
  [Description("Изображение заголовка.")]
  [DefaultValue(null)]
  public Image CaptionImage
  {
   get { return finistLabel1.Image; }
   set { finistLabel1.Image = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Основной цвет текста заголовка
  /// </summary>
  [Category("Appearance")]
  [Description("Основной цвет текста заголовка.")]
  [DefaultValue(typeof(Color), "ControlText")]
  public new Color ForeColor
  {
   get { return finistLabel1.ForeColor; }
   set { finistLabel1.ForeColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// CanFocus
  /// </summary>
  public new bool CanFocus
  {
   get { return false; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Padding
  /// </summary>
  [DefaultValue(typeof(Padding), "3; 3; 3; 3")]
  public new Padding Padding
  {
   get { return base.Padding; }
   set { base.Padding = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [DefaultValue(typeof(DockStyle), "Top")]
  public override DockStyle Dock
  {
   get { return base.Dock; }
   set { base.Dock = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// 
  /// </summary>
  [DefaultValue(true)]
  public override bool AutoSize
  {
   get { return base.AutoSize; }
   set { base.AutoSize = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimCaption()
  {
   this.Dock = DockStyle.Top;
   InitializeComponent();
   this.TabStop = false;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
      
 }
}

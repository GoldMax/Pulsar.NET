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
 /// Контрол двух надписей
 /// </summary>
 public class SimTwoLabel : Panel
 {
  SimLabel L1 = new Sim.Controls.SimLabel();
  SimLabel L2 = new Sim.Controls.SimLabel();
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Первая надпись
  /// </summary>
  [Category("Label1")]
  [Description("Первая надпись")]
  [Browsable(true)]
  public string Label1Text
  {
   get { return L1.Text; }
   set { L1.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Изображение первой надписи.
  /// </summary>
  [Category("Label1")]
  [Description("Изображение первой надписи.")]
  [DefaultValue(null)]
  public Image Label1Image
  {
   get { return L1.Image; }
   set { L1.Image = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Шрифт первой надписи.
  /// </summary>
  [Category("Label1")]
  [Description("Шрифт первой надписи.")]
  public Font Label1Font
  {
   get { return L1.Font; }
   set { L1.Font = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Цвет текста первой надписи.
  /// </summary>
  [Category("Label1")]
  [Description("Цвет текста первой надписи.")]
  [DefaultValue(typeof(Color), "ControlText")]
  public Color Label1ForeColor
  {
   get { return L1.ForeColor; }
   set { L1.ForeColor = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Вторая надпись
  /// </summary>
  [Category("Label2")]
  [Description("Вторая надпись")]
  public string Label2Text
  {
   get { return L2.Text; }
   set { L2.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Выравнивание текста.
  /// </summary>
  [Category("Label2")]
  [Description("Выравнивание текста.")]
  [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
  public ContentAlignment Label2TextAlign
  {
   get { return L2.TextAlign; }
   set { L2.TextAlign = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  ///// <summary>
  ///// MaximumSize
  ///// </summary>
  //public new Size MaximumSize
  //{
  // get 
  // { 
  //  return new Size(finistLabel1.Width + finistLabel2.Width,
  //                  finistLabel1.Height > finistLabel2.Height ? finistLabel1.Height : finistLabel2.Height);
  // }
  //} 
  ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  ///// <summary>
  ///// MinimumSize
  ///// </summary>
  //public new Size MinimumSize
  //{
  // get
  // {
  //  return new Size(finistLabel1.Width + finistLabel2.Width,
  //                  finistLabel1.Height > finistLabel2.Height ? finistLabel1.Height : finistLabel2.Height);
  // }
  //}
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Size
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public new Size Size
  {
   get
   {
    int bH = 0, bV = 0;
    if(BorderStyle == BorderStyle.FixedSingle)
    {
     bH = bV = 2;
     //bV = BorderWidth.Vertical;
     //bH = BorderWidth.Horizontal;
    }
    else if(BorderStyle == BorderStyle.Fixed3D)
    {
     //if(Border3DKind == Border3DKind.RaisedFlat || Border3DKind == Border3DKind.SunkenFlat)
     // bV = bH = 2;
     //else
      bV = bH = 4;
    }
    bV += this.Padding.Vertical;
    bH += this.Padding.Horizontal;
    if(Dock == DockStyle.Fill || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
    {
     if(Parent == null)
      return new Size(L1.PreferredWidth + L2.PreferredWidth + bH,
                      (L1.PreferredHeight > L2.PreferredHeight ? L1.PreferredHeight : L2.PreferredHeight) + bV);
     bH = Parent.ClientSize.Width - Parent.Padding.Horizontal;
     return new Size(bH, (L1.PreferredHeight > L2.PreferredHeight ? L1.PreferredHeight : L2.PreferredHeight) + bV);
    }
    else
    {
     return new Size(L1.PreferredWidth + L2.PreferredWidth + bH,
                     (L1.PreferredHeight > L2.PreferredHeight ? L1.PreferredHeight : L2.PreferredHeight) + bV);
    }
   }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimTwoLabel()
  {
   L1.AutoEllipsis = true;
   L1.AutoSize = true;
   L1.ContextMenuStrip = null;
   L1.Location = new System.Drawing.Point(0, 0);
   L1.Text = "finistLabel1";
   L1.TextChanged += (s, e)=> SetSize();
   L1.Height = 13;
   //L1.BackColor = Color.Red;
   this.Controls.Add(L1);

   L2.AutoEllipsis = true;
   L2.AutoSize = true;
   L2.Location = new System.Drawing.Point(L1.PreferredWidth, 0);
   L2.Text = "finistLabel2";
   L2.TextChanged += (s, e) => SetSize();
   L1.Height = 13;
   //L2.BackColor = Color.Green;
   this.Controls.Add(L2);
   
   this.BackColor = Color.Transparent; // Color.Cyan;
   base.Size = this.Size;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="text1">Текст первой надписи</param>
  /// <param name="text2">Текст второй надписи.</param>
  public SimTwoLabel(string text1, string text2) : this()
  {
   L1.Text = text1;
   L2.Text = text2;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  private void SetSize()
  {
   base.Size = this.Size;
   if(Dock == DockStyle.Fill || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
   {
    L2.AutoSize = false;
    L2.Width = this.ClientSize.Width - L1.PreferredWidth - this.Padding.Horizontal;
    L2.Height = L2.PreferredHeight;
   }
   else if(L2.AutoSize == false)
    L2.AutoSize = true;
   if(L1.PreferredHeight > L2.PreferredHeight)
    L2.Location = new Point(L1.Location.X + L1.PreferredWidth,
                            this.Padding.Top + (L1.PreferredHeight - L2.PreferredHeight)/2 +
                            ((L1.PreferredHeight - L2.PreferredHeight)%2 == 1 ? 2 : 1));
   else
   {
   if(L1.PreferredHeight < L2.PreferredHeight)
    L1.Location = new Point(this.Padding.Left, this.Padding.Top + 
                             (L2.PreferredHeight - L1.PreferredHeight)/2 +
                             ((L2.PreferredHeight - L1.PreferredHeight)%2 == 1 ? 2 : 1));
    L2.Location = new Point(L1.Location.X + L1.PreferredWidth, this.Padding.Top);
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnPaddingChanged
  /// </summary>
  /// <param name="e"></param>
  protected override void OnPaddingChanged(EventArgs e)
  {
   SetSize();
   base.OnPaddingChanged(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnSizeChanged(EventArgs e)
  {
   if(base.Size != this.Size)
    SetSize();
   base.OnSizeChanged(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnFontChanged
  /// </summary>
  /// <param name="e"></param>
  protected override void OnFontChanged(EventArgs e)
  {
   SetSize();
   base.OnFontChanged(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnDockChanged
  /// </summary>
  /// <param name="e"></param>
  protected override void OnDockChanged(EventArgs e)
  {
   SetSize();
   base.OnDockChanged(e);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnParentChanged
  /// </summary>
  /// <param name="e"></param>
  protected override void OnParentChanged(EventArgs e)
  {
   SetSize();
   base.OnParentChanged(e);
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
          
 }
}

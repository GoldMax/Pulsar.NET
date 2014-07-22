using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола загрузки данных
 /// </summary>
 public partial class SimProgress : UserControl
 {
  private System.Timers.Timer timer = null;
  private Image[] imgs = new Image[8];
  private int pos = 0;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimProgress()
  {
   InitializeComponent();
   timer = new System.Timers.Timer(150);
   timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

   imgs[0] = global::Sim.Common.Properties.Resource1._1;
   imgs[1] = global::Sim.Common.Properties.Resource1._2;
   imgs[2] = global::Sim.Common.Properties.Resource1._3;
   imgs[3] = global::Sim.Common.Properties.Resource1._4;
   imgs[4] = global::Sim.Common.Properties.Resource1._5;
   imgs[5] = global::Sim.Common.Properties.Resource1._6;
   imgs[6] = global::Sim.Common.Properties.Resource1._7;
   imgs[7] = global::Sim.Common.Properties.Resource1._8;

  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  ///// <summary>
  ///// 
  ///// </summary>
  ///// <param name="e"></param>
  //protected override void OnEnabledChanged(EventArgs e)
  //{
  // base.OnEnabledChanged(e);
  // if(this.Enabled && this.Visible)
  //  timer.Start();
  // else
  //  timer.Stop();
  //}
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnVisibleChanged(EventArgs e)
  {
   base.OnVisibleChanged(e);
   if(this.Visible)  //   this.Enabled && 
    timer.Start();
   else
    timer.Stop();
  }
  //-------------------------------------------------------------------------------------
  void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
  {
   if(this.Visible == false)// this.Enabled == false || 
   {
    timer.Stop();
    return;
   }
   pos++;
   if(pos == 8)
    pos = 0;
   flCircle.Image = imgs[pos];
   flCircle.Invalidate();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Запускает рисование.
  /// </summary>
  public void Start()
  {
   timer.Start();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Помещает контрол в родителя и запускает рисование.
  /// </summary>
  /// <param name="parent"></param>
  public void Start(Control parent)
  {
   parent.Controls.Add(this);
   this.Location = new Point((parent.Width - this.Width)/2, (parent.Height - this.Height)/2);
   this.BringToFront();
   timer.Start();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Останавливает рисование.
  /// </summary>
  public void Stop()
  {
   timer.Stop();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Отображает контрол в указанном родителе.
  /// </summary>
  /// <param name="parent">Родительский контрол.</param>
  /// <param name="useBack">Определяет, будет ли использоваться эмуляция прозрачности фона.</param>
  public static void Show(Control parent, bool useBack = true)
  {
   SimProgress box = new SimProgress();
   foreach(Control c in parent.Controls)
    if(c is SimProgress)
    {
     ((SimProgress)c).Start();
     return;
    }
   Rectangle r = new Rectangle(Point.Empty, parent.Size);
   try
   {
    if(useBack)
    {
     r.X = -1*(parent.Width - parent.ClientSize.Width)/2;
     r.Y = -1*(parent.Height - parent.ClientSize.Height)/2;
     Bitmap bmp = new Bitmap(r.Width, r.Height);
     parent.DrawToBitmap(bmp, new Rectangle(Point.Empty, parent.Size)); //parent.Location 
     BitmapEffects.GaussianBlur(bmp, 4);
     BitmapEffects.Ligth(bmp, 50);
     if(box.BackgroundImageLayout != ImageLayout.Center)
      box.BackgroundImageLayout = ImageLayout.Center;
     box.BackgroundImage = bmp;  
    }
    else
     box.BackColor = Color.Transparent;
   }
   catch
   {
   }
   box.Bounds = r;
   parent.Controls.Add(box);
   box.BringToFront();
   box.timer.Start();
   parent.SizeChanged += new EventHandler(parent_SizeChanged);
  }
  //-------------------------------------------------------------------------------------
  static void parent_SizeChanged(object sender, EventArgs e)
  {
   Control parent = (Control)sender;
   SimProgress box = new SimProgress();
   foreach(Control c in parent.Controls)
    if(c is SimProgress)
    {
     box = (SimProgress)c;
     break;
    }
   if(box == null)
    return;
   Rectangle r = new Rectangle(Point.Empty, parent.Size);
   if(box.BackgroundImage == null)
    box.Bounds = r;
   else
   {
    box.Hide();
    r.X = -1*(parent.Width - parent.ClientSize.Width)/2;
    r.Y = -1*(parent.Height - parent.ClientSize.Height)/2;
    Bitmap bmp = new Bitmap(r.Width, r.Height);
    parent.DrawToBitmap(bmp, new Rectangle(Point.Empty, parent.Size)); //parent.Location 
    BitmapEffects.GaussianBlur(bmp, 4);
    BitmapEffects.Ligth(bmp, 50);
    if(box.BackgroundImageLayout != ImageLayout.Center)
     box.BackgroundImageLayout = ImageLayout.Center;
    box.BackgroundImage = bmp;
    box.Location = new Point((parent.Width - box.Width)/2, (parent.Height - box.Height)/2);
    box.Bounds = r;
    box.Show();
    box.BringToFront();
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Скрывает контрол в указанном родителе.
  /// </summary>
  /// <param name="parent">Родительский контрол.</param>
  public static void Hide(Control parent)
  {
   bool found = false;
   foreach(Control c in parent.Controls)
    if(found = c is SimProgress)
    {
     ((SimProgress)c).Stop();
     parent.Controls.Remove(c);
     break;
    }
   if(found == false)
    return;
   //foreach(Control c in parent.Controls)
   // c.Visible = true;
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
 }
}

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
 /// Класс контрола выбора даты
 /// </summary>
 public partial class SimDateSelect : SimPopupControl
 {
  private System.Windows.Forms.Panel panelMain;
  private System.Windows.Forms.MonthCalendar monthCalendar1;
  private System.Windows.Forms.Panel panel1;
  private SimLabel simLabel1;
  private SimPopupButton simPopupButton1;
  private DateTime _value = DateTime.Now.Date;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region public event  Pulsar.EventHandler<object,DateTime> UIValueChanged
  [NonSerialized]
  private Pulsar.WeakEvent<DateTime> _UIValueChanged;
  /// <summary>
  /// 
  /// </summary>
  public event Pulsar.EventHandler<object, DateTime> UIValueChanged
  {
   add { _UIValueChanged += value; }
   remove { _UIValueChanged -= value; }
  }
  /// <summary>
  /// Вызывает событие UIValueChanged.
  /// </summary>
  protected virtual void OnUIValueChanged(DateTime arg)
  {
   if(_UIValueChanged != null)
    _UIValueChanged.Raise(this, arg);
  } 
  #endregion public event  Pulsar.EventHandler<object,DateTime> UIValueChanged
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Значение даты
  /// </summary>
  public DateTime Value
  {
   get { return _value; }
   set 
   {
    _value = value; 
    monthCalendar1.SelectionStart = value;
    simLabel1.Text = _value.ToString("dd.MM.yyyy");
   }
  }
  #endregion << Properties >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  public SimDateSelect() : base()
  {
   this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
   this.panel1 = new System.Windows.Forms.Panel();
   this.simLabel1 = new Sim.Controls.SimLabel();
   this.simPopupButton1 = new Sim.Controls.SimPopupButton();
   this.panelMain = new System.Windows.Forms.Panel();
   this.panelMain.SuspendLayout();
   this.panel1.SuspendLayout();
   // 
   // monthCalendar1
   // 
   this.monthCalendar1.Dock = System.Windows.Forms.DockStyle.Top;
   this.monthCalendar1.ShowToday = false;
   this.monthCalendar1.ShowTodayCircle = false;
   this.monthCalendar1.MaxSelectionCount = 1;
   this.monthCalendar1.DateSelected += new DateRangeEventHandler(monthCalendar1_DateSelected);
   // 
   // simLabel1
   // 
   this.simLabel1.AutoSize = false;
   this.simLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.simLabel1.Text = DateTime.Now.Date.ToString("dd.MM.yyyy");
   // 
   // simPopupButton1
   // 
   this.simPopupButton1.Dock = System.Windows.Forms.DockStyle.Right;
   this.simPopupButton1.Image = global::Sim.Controls.Properties.Resources.OK;
   this.simPopupButton1.Size = new System.Drawing.Size(21, 21);
   this.simPopupButton1.Click += new EventHandler(simPopupButton1_Click);
   // 
   // panel1
   // 
   this.panel1.Controls.Add(this.simLabel1);
   this.panel1.Controls.Add(this.simPopupButton1);
   this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
   this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
   this.panel1.Size = new System.Drawing.Size(315, 21);
   // 
   // panelMain
   // 
   this.panelMain.Controls.Add(this.panel1);
   this.panelMain.Controls.Add(this.monthCalendar1);
   this.panelMain.Padding = new System.Windows.Forms.Padding(0);
   this.panelMain.BackColor = System.Drawing.Color.Transparent;
   this.panel1.ResumeLayout();
   this.panelMain.ResumeLayout();
   this.panelMain.Size = new Size(monthCalendar1.Height-5, monthCalendar1.Height + panel1.Height-5);


   base.Control = panelMain;

  }

  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// 
  /// </summary>
  /// <param name="x"></param>
  /// <param name="y"></param>
  public override void Show(int x, int y)
  {
   base.Show(x, y);
   monthCalendar1.Select();
   monthCalendar1.Focus();
  }
  void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
  {
   Value = e.Start;
  // OnUIValueChanged(e.Start);
  }
  void simPopupButton1_Click(object sender, EventArgs e)
  {
   this.Hide();
   OnUIValueChanged(Value);
  }

  #endregion << Methods >>
 }
}

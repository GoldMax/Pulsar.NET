using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Pulsar;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола быстрого ввода значения
 /// </summary>
 public class SimQuickEdit : SimPopupControl
 {
  SimQuickEditCtrl ctrl = new SimQuickEditCtrl();
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << Events >>
  /// <summary>
  /// Событие, возникающее при нажатии кнопок или клавиш Enter или Esc
  /// </summary>
  public event EventHandler<SimQuickEdit, bool> InputDone;

  /// <summary>
  /// Вызывает событие InputDone
  /// </summary>
  /// <param name="isOK">Результат</param>
  protected void OnInputDone(bool isOK)
  {
   this.Hide();
   if(InputDone != null)
    InputDone(this, isOK);
  }
  #endregion << Events >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет отображение в две строки
  /// </summary>
  [DefaultValue(true)]
  public bool TwoLines
  {
   get { return ctrl.TwoLines; }
   set { ctrl.TwoLines = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Текст метки
  /// </summary>
  public string Caption
  {
   get { return ctrl.finistLabel1.Text; }
   set { ctrl.finistLabel1.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Значение ввода
  /// </summary>
  [DefaultValue("")]
  public string Value
  {
   get { return ctrl.ftbValue.Text; }
   set { ctrl.ftbValue.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Формат вводимых данных.
  /// </summary>
  public TextBoxFormat Format
  {
   get { return ctrl.ftbValue.Format; }
   set { ctrl.ftbValue.Format = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Исключения формата ввода
  /// </summary>
  public string FormatExceptions
  {
   get { return ctrl.ftbValue.FormatException; }
   set { ctrl.ftbValue.FormatException = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет отображение кнопки отмены
  /// </summary>
  [DefaultValue(true)]
  public bool ShowCancelButton
  {
   get { return ctrl.btnCancel.Visible; }
   set 
   { 
    ctrl.btnCancel.Visible = value;
    ctrl.panel2.Visible = value; 
   }
  }

  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimQuickEdit ()
  {
   ctrl.btnOK.Click += (s, e) => OnInputDone(true);
   ctrl.btnCancel.Click += (s, e) => OnInputDone(false);
   ctrl.ftbValue.KeyUp += (s, e) => { if (e.KeyCode == Keys.Enter) OnInputDone(true); };
   this.Control = ctrl;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// OnOpening
  /// </summary>
  /// <param name="e"></param>
  protected override void OnOpening(CancelEventArgs e)
  {
   e.Cancel = false;
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
   SetFocusOnControl();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Отображает контрол.
  /// </summary>
  /// <param name="screenPoint">Положение контрола в оконных координатах.</param>
  public override void Show(Point screenPoint)
  {
   base.Show(screenPoint);
   ctrl.ftbValue.SelectAll();
   ctrl.ftbValue.Select();
   ctrl.ftbValue.Focus();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Отображает контрол во всплывающем контроле.
  /// </summary>
  /// <param name="screenPoint">Положение контрола в оконных координатах.</param>
  /// <param name="caption">Текст метки</param>
  /// <param name="value">Значение ввода</param>
  /// <param name="twoLines">Определяет отображение в две строки.</param>
  /// <param name="format">Формат ввода</param>
  /// <param name="formatException">Строка символов исключений формата.</param>
  /// <param name="width">Ширина</param>
  /// <returns></returns>
  public static SimQuickEdit ShowPopup(Point screenPoint, string caption, string value, bool twoLines,
                                             TextBoxFormat format = TextBoxFormat.NotSet, string formatException = "",
                                             int width = 170)
  {
   SimQuickEdit c = new SimQuickEdit();
   c.Caption = caption;
   c.Value = value;
   c.TwoLines = twoLines;
   c.Format = format;
   c.FormatExceptions = formatException;
   c.ctrl.Width = width;

   c.Show(screenPoint);
   return c;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Устанавливает фокус ввода.
  /// </summary>
  public void SetFocusOnControl()
  {
   ctrl.ftbValue.Select();
   ctrl.ftbValue.Focus();
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  #region << private class SimQuickEditCtrl : UserControl >>
  /// <summary>
  /// Класс контрола SimQuickEdit
  /// </summary>
  private class SimQuickEditCtrl : UserControl
  {
   internal System.ComponentModel.IContainer components = null;
   internal Sim.Controls.SimLabel finistLabel1;
   internal System.Windows.Forms.Panel panel1;
   internal System.Windows.Forms.Panel panel3;
   internal Sim.Controls.SimTextBox ftbValue;
   internal Sim.Controls.SimPopupButton btnOK;
   internal System.Windows.Forms.Panel panel2;
   internal Sim.Controls.SimPopupButton btnCancel;

   internal bool twoLines = true;
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Properties >>
   [Category("Behavior")]
   [DefaultValue(true)]
   public bool TwoLines
   {
    get { return twoLines; }
    set
    {
     twoLines = value;
     if(twoLines)
      finistLabel1.Dock = DockStyle.Top;
     else
      finistLabel1.Dock = DockStyle.Left;
     this.Height = 0;
    }
   }
   #endregion << Properties >>
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   //-------------------------------------------------------------------------------------
   #region << Constructors >>
   /// <summary>
   /// Конструктор по умолчанию.
   /// </summary>
   public SimQuickEditCtrl()
   {
    this.BackColor = Color.Transparent;
    InitializeComponent();
   }
   //-------------------------------------------------------------------------------------
   #region Component Designer generated code
   /// <summary> 
   /// Required method for Designer support - do not modify 
   /// the contents of this method with the code editor.
   /// </summary>
   private void InitializeComponent()
   {
    this.panel1 = new System.Windows.Forms.Panel();
    this.ftbValue = new Sim.Controls.SimTextBox();
    this.panel3 = new System.Windows.Forms.Panel();
    this.btnOK = new Sim.Controls.SimPopupButton();
    this.panel2 = new System.Windows.Forms.Panel();
    this.btnCancel = new Sim.Controls.SimPopupButton();
    this.finistLabel1 = new Sim.Controls.SimLabel();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    // 
    // panel1
    // 
    this.panel1.Controls.Add(this.ftbValue);
    this.panel1.Controls.Add(this.panel3);
    this.panel1.Controls.Add(this.btnOK);
    this.panel1.Controls.Add(this.panel2);
    this.panel1.Controls.Add(this.btnCancel);
    this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
    this.panel1.Location = new System.Drawing.Point(2, 15);
    this.panel1.MaximumSize = new System.Drawing.Size(0, 21);
    this.panel1.MinimumSize = new System.Drawing.Size(0, 21);
    this.panel1.Name = "panel1";
    this.panel1.Size = new System.Drawing.Size(166, 21);
    this.panel1.TabIndex = 1;
    // 
    // ftbValue
    // 
    this.ftbValue.AcceptsTab = true;
    this.ftbValue.Dock = System.Windows.Forms.DockStyle.Fill;
    this.ftbValue.Location = new System.Drawing.Point(0, 0);
    this.ftbValue.Name = "ftbValue";
    this.ftbValue.Size = new System.Drawing.Size(118, 21);
    this.ftbValue.TabIndex = 0;
    // 
    // panel3
    // 
    this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
    this.panel3.Location = new System.Drawing.Point(118, 0);
    this.panel3.Name = "panel3";
    this.panel3.Size = new System.Drawing.Size(3, 21);
    this.panel3.TabIndex = 4;
    // 
    // btnOK
    // 
    this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
    this.btnOK.Image = global::Sim.Controls.Properties.Resources.OK;
    this.btnOK.ImagePushed = null;
    this.btnOK.ImageRaised = null;
    this.btnOK.Location = new System.Drawing.Point(121, 0);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new System.Drawing.Size(21, 21);
    this.btnOK.TabIndex = 3;
    this.btnOK.ToolTip = null;
    // 
    // panel2
    // 
    this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
    this.panel2.Location = new System.Drawing.Point(142, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new System.Drawing.Size(3, 21);
    this.panel2.TabIndex = 2;
    // 
    // btnCancel
    // 
    this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
    this.btnCancel.Image = global::Sim.Controls.Properties.Resources.Cancel;
    this.btnCancel.ImagePushed = null;
    this.btnCancel.ImageRaised = null;
    this.btnCancel.Location = new System.Drawing.Point(145, 0);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new System.Drawing.Size(21, 21);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.ToolTip = null;
    // 
    // finistLabel1
    // 
    this.finistLabel1.AutoEllipsis = true;
    this.finistLabel1.AutoSize = true;
    this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Top;
    this.finistLabel1.Location = new System.Drawing.Point(2, 2);
    this.finistLabel1.MaximumSize = new System.Drawing.Size(0, 19);
    this.finistLabel1.MinimumSize = new System.Drawing.Size(0, 19);
    this.finistLabel1.Name = "finistLabel1";
    this.finistLabel1.Size = new System.Drawing.Size(104, 13);
    this.finistLabel1.TabIndex = 0;
    this.finistLabel1.Text = "Введите значение:";
    // 
    // SimQuickEditCtrl
    // 
    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    this.Controls.Add(this.panel1);
    this.Controls.Add(this.finistLabel1);
    this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
    this.Name = "SimQuickEditCtrl";
    this.Padding = new System.Windows.Forms.Padding(2);
    this.Size = new System.Drawing.Size(170, 44);
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();

   }
   #endregion
   //-------------------------------------------------------------------------------------
   /// <summary> 
   /// Clean up any resources being used.
   /// </summary>
   /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
   protected override void Dispose(bool disposing)
   {
    if(disposing && (components != null))
    {
     components.Dispose();
    }
    base.Dispose(disposing);
   }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
   #region << Methods >>
   /// <summary>
   /// 
   /// </summary>
   /// <param name="x"></param>
   /// <param name="y"></param>
   /// <param name="width"></param>
   /// <param name="height"></param>
   /// <param name="specified"></param>
   protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
   {
    base.SetBoundsCore(x, y, width, (twoLines ? 40 : 20) + this.Padding.Vertical, specified);
   }
   #endregion << Methods >>
   //-------------------------------------------------------------------------------------
  }
  #endregion << private class SimQuickEditCtrl : UserControl >>
 }
}

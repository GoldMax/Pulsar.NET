using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Sim.Controls;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола данных detail столбца в SimDataGridViewEx
 /// </summary>
 public partial class DetailColumnViewControl : SimPanel
 {
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << public event EventHandler ButtonHidePressed >>
  /// <summary>
  /// Событие, возникающее при нажатии пользователем на кнопку исключения из детализации.
  /// </summary>
  public event EventHandler ButtonHidePressed;
  /// <summary>
  /// Вызывает событие ButtonHidePressed.
  /// </summary>
  protected void OnButtonHidePressed()
  {
   if(ButtonHidePressed != null)
    ButtonHidePressed(this, EventArgs.Empty);
  }
  #endregion << public event EventHandler ButtonHidePressed >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет текст заголовка.
  /// </summary>
  public string Caption
  {
   get { return finistLabelCaption.Text; }
   set { finistLabelCaption.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет значение.
  /// </summary>
  public override string Text
  {
   get { return finistLabelValue.Text; }
   set { finistLabelValue.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет видимость кнопки.
  /// </summary>
  [DefaultValue(true)]
  public bool ButtonVisible 
  { 
   get { return finistPopupButton1.Visible; }
   set { finistPopupButton1.Visible = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public DetailColumnViewControl()
  {
   InitializeComponent();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  /// <param name="caption">Определяет текст заголовка.</param>
  /// <param name="text">Определяет значение.</param>
  /// <param name="buttonVisible">Определяет видимость кнопки.</param>
  public DetailColumnViewControl(string caption, string text, bool buttonVisible = true) : this()
  {
   Caption = caption;
   Text = text;
   ButtonVisible = buttonVisible;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Controls Handlers >>
  private void finistPopupButton1_Click(object sender, EventArgs e)
  {
   OnButtonHidePressed();
  }
  #endregion << Controls Handlers >>
  //-------------------------------------------------------------------------------------
      
 }
}

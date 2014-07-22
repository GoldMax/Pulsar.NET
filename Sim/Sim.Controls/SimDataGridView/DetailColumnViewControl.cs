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
 /// ����� �������� ������ detail ������� � SimDataGridViewEx
 /// </summary>
 public partial class DetailColumnViewControl : SimPanel
 {
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << public event EventHandler ButtonHidePressed >>
  /// <summary>
  /// �������, ����������� ��� ������� ������������� �� ������ ���������� �� �����������.
  /// </summary>
  public event EventHandler ButtonHidePressed;
  /// <summary>
  /// �������� ������� ButtonHidePressed.
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
  /// ���������� ����� ���������.
  /// </summary>
  public string Caption
  {
   get { return finistLabelCaption.Text; }
   set { finistLabelCaption.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ���������� ��������.
  /// </summary>
  public override string Text
  {
   get { return finistLabelValue.Text; }
   set { finistLabelValue.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ���������� ��������� ������.
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
  /// ����������� �� ���������.
  /// </summary>
  public DetailColumnViewControl()
  {
   InitializeComponent();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ���������������� �����������.
  /// </summary>
  /// <param name="caption">���������� ����� ���������.</param>
  /// <param name="text">���������� ��������.</param>
  /// <param name="buttonVisible">���������� ��������� ������.</param>
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

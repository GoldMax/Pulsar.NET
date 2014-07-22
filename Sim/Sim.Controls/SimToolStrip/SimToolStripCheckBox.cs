using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace Sim.Controls
{
 /// <summary>
 /// ����� �������� ������ ������������ ToolStrip - CheckBox.
 /// </summary>
 //[DesignerCategory("code")]
 [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
 [ToolboxBitmap(typeof(CheckBox))]
 [DefaultProperty("Checked")]
 [DefaultEvent("CheckedChanged")]
 public class SimToolStripCheckBox : ToolStripControlHost
 {
  private SimCheckBox box = null;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << public event EventHandler CheckedChanged >>
  /// <summary>
  /// �������, ����������� ��� ��������� ��������� CheckBox.
  /// </summary>
  public event EventHandler CheckedChanged;
  /// <summary>
  /// �������� ������� CheckedChanged.
  /// </summary>
  protected void OnCheckedChanged()
  {
   if(CheckedChanged != null)
    CheckedChanged(this, EventArgs.Empty);
  }
  #endregion << public event EventHandler CheckedChanged >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// ���������� ��������� CheckBox.
  /// </summary>
  [Category("CheckBox")]
  [Description("���������� ��������� CheckBox.")]
  [DefaultValue(false)]
  public bool Checked
  {
   get { return box.Checked; }
   set { box.Checked = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ���������� ����� CheckBox.
  /// </summary>
  [Category("CheckBox")]
  [Description("���������� ����� CheckBox.")]
  public new string Text
  {
   get { return box.Text; }
   set { box.Text = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// ����������� �� ���������.
  /// </summary>
  public SimToolStripCheckBox() : base(new SimCheckBox())
  {
   box = (SimCheckBox)Control;
   box.BackColor = Color.Transparent;
   this.BackColor = Color.Transparent;
   box.CheckedChanged += new EventHandler(box_CheckedChanged);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Handlers >>
  void box_CheckedChanged(object sender, EventArgs e)
  {
   OnCheckedChanged();
  }
  #endregion << Handlers >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ������������ ��������.
  /// </summary>
  /// <param name="disposing"></param>
  protected override void Dispose(bool disposing)
  {
   box.CheckedChanged -= new EventHandler(box_CheckedChanged);
   base.Dispose(disposing);
  }
 }
}

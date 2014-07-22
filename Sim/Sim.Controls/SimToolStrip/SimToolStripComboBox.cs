using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Sim.Controls
{
 /// <summary>
 /// ToolStripSimComboBox
 /// </summary>
 [ToolboxBitmap(typeof(ComboBox))]
 [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
 [DefaultEvent("UISelectedItemChanged")]
 public class SimToolStripComboBox : ToolStripControlHost
 {
  private SimComboBox box = null;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region Events
  /// <summary>
  /// Событие, возникающее при выборе элемента в выподающем списке пользователем.
  /// </summary>
  [Category("Behavior")]
  [Description(" Событие, возникающее при выборе элемента в выподающем списке пользователем.")]
  public event EventHandler UISelectedItemChanged
  {
   add { box.UISelectedItemChanged += value; }
   remove { box.UISelectedItemChanged -= value; }
  }
  #endregion Events
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// ComboBox.SelectedItem
  /// </summary>
  [Browsable(false)]
  public object SelectedItem
  {
   get { return box.SelectedItem; }
   set { box.SelectedItem = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ComboBox.Items
  /// </summary>
  [Category("Data")]
  [MergableProperty(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Description("ComboBox.Items")]
  public IList Items    
  {
   get { return box.Items; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// ComboBox.Sorted
  /// </summary>
  [Category("Behavior")]
  [DefaultValue(false)]
  [Description("ComboBox.Sorted")]
  public bool Sorted
  {
   get { return box.Sorted; }
   set { box.Sorted = value; }
  }
  #endregion << Properties >>
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimToolStripComboBox() : base(new SimComboBox())
  {
   box = (SimComboBox)Control;
   base.AutoSize = false;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
 }
 /*
 /// <summary>
 /// ToolStripSimComboBox
 /// </summary>
 [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
 [ToolboxBitmap(typeof(ComboBox))]
 [DefaultEvent("SelectionChangeCommitted")]
 public class ToolStripSimComboBox : ToolStripComboBox
 {
  #region Events
  /// <summary>
  /// Событие, возникающее при выборе элемента в выподающем списке пользователем.
  /// </summary>
  [Category("Behavior")]
  [Description(" Событие, возникающее при выборе элемента в выподающем списке пользователем.")]
  public event EventHandler SelectionChangeCommitted
  {
   add { base.ComboBox.SelectionChangeCommitted += value; }
   remove { base.ComboBox.SelectionChangeCommitted -= value; }
  }
  #endregion Events
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public ToolStripSimComboBox() : base()
  {
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
      
 } */
}

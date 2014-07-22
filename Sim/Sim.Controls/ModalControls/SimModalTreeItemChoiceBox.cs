using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Pulsar;

namespace Sim.Controls
{
 /// <summary>
 /// Класс диалогового окна выбора элемента дерева
 /// </summary>
 public partial class SimModalTreeItemChoiceBox : Sim.Controls.SimModalDialogBase
 {
  private Func<ITreeItem, bool> isOkEnabled = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Контрол отображения дерева.
  /// </summary>
  public SimTreeView TreeView
  {
   get { return treeView; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Делегат метода определения правильности выбора.
  /// </summary>
  public Func<ITreeItem, bool> CheckButtonOkEnabledAction
  {
   get { return isOkEnabled; }
   set { isOkEnabled = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Выбранный элемент дерева.
  /// </summary>
  public ITreeItem SelectedItem
  {
   get { return treeView.SelectedNodeItem; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimModalTreeItemChoiceBox()
  {
   InitializeComponent();
   treeView.SelectedNodeChanged += new SimTreeView.SelectedNodeChangedHandler(treeView_SelectedNodeChanged);

   ButtonOkEnabled = false;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  void treeView_SelectedNodeChanged(object sender, ITreeItem item)
  {
   if(isOkEnabled == null)
    ButtonOkEnabled = item != null;
   else
    ButtonOkEnabled = item == null ? false : isOkEnabled(item);
  }
  //-------------------------------------------------------------------------------------
 }
}

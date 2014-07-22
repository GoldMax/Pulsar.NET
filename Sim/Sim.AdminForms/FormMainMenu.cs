using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Pulsar;
using Pulsar.Clients;

using Sim.Controls;

namespace Sim.AdminForms
{
	/// <summary>
	/// Класс формы редактирования главного меню
	/// </summary>
	public partial class FormMainMenu : ClientBaseForm
	{
		private PulsarMainMenu _menu = null;

		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		public FormMainMenu()
		{
			InitializeComponent();

			imageList1.Images.Add(global::Sim.AdminForms.Properties.Resources.Arrow_Gray);
			imageList1.Images.Add(global::Sim.AdminForms.Properties.Resources.Point_2);
			ftvTree.Comparer = delegate(SimTreeNode n1, SimTreeNode n2)
			{
				return PulsarMainMenu.PulsarMainMenuSorter.Default.Compare(n1.TreeItem, n2.TreeItem);
			};
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Form Handlers >>
		///
		protected override void OnLoad(EventArgs e)
		{
			try
			{
				ShowProgressWindow();
				TaskManager.Run("MainMenu", this,	() => PulsarConnection.Default.Get("MainMenu"));
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnClosing(CancelEventArgs e)
		{
			ftvTree.Tree = null;
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void AsyncTaskDoneBody(AsyncTask task)
		{
			#region MainMenu
			if(task.TaskName == "MainMenu")
			{
				_menu = (PulsarMainMenu)task.Result;
				ftvTree.Tree = _menu;
			}
			#endregion MainMenu
			#region Save
			if(task.TaskName == "Save")
			{
				//ITreeItem sel = ftvTree.SelectedNodeItem;
				//ftvTree.Se
			}
			#endregion Save
		}
		#endregion << Form Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void ftvTree_SelectedNodeChanged(object sender, ITreeItem item)
		{
			if(item == null)
			{
				propertyGrid.SelectedObject = null;
				lblEditSortOrder.Value = string.Empty;
				return;
			}
			propertyGrid.SelectedObject = item;
			lblEditSortOrder.Value = item.SortOrder == null ? string.Empty : item.SortOrder.ToString();
		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			TreeItem<FormInfo> item = (TreeItem<FormInfo>)ftvTree.SelectedNodeItem;
			if(item == null)
				return;
			string s = "";
			if(SimModalInputBox.Show(PanelBack,ref s,"Создание элемента меню","Введите наименование элемента меню:") != System.Windows.Forms.DialogResult.OK)
				return;
			_menu.Add(new FormInfo() { Caption = s }, item);
		}
		private void btnDel_Click(object sender, EventArgs e)
		{
			TreeItem<FormInfo> item = (TreeItem<FormInfo>)ftvTree.SelectedNodeItem;
			if(item == null || item.HasChildren)
				return;
			if(SimModalMessageBox.Show(PanelBack,String.Format("Удалить [{0}]?",item.ItemText),"Удаление", MessageBoxIcon.Question, MessageBoxButtons.YesNo) !=
			     System.Windows.Forms.DialogResult.Yes)
				return;
			_menu.Remove(item);
		}
		private void ftvTree_ItemDropped(object sender, SimTreeView.DragDropItemEventArgs e)
		{
			if(SimModalMessageBox.Show(PanelBack, String.Format("Перенести [{0}]?", e.DroppedItem.ItemText), "Перенос", MessageBoxIcon.Question, MessageBoxButtons.YesNo) !=
			    System.Windows.Forms.DialogResult.Yes)
				return;
			_menu.MoveItem((TreeItem<FormInfo>)e.DroppedItem, (TreeItem<FormInfo>)e.DestItem);
		}
		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			TreeItem<FormInfo> item = (TreeItem<FormInfo>)ftvTree.SelectedNodeItem;
			if(item == null || item.IsRoot)
				return;
			_menu.MoveItem(item, null);
		}
		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			ITreeItem sel = ftvTree.SelectedNodeItem;
			_menu.EventsOff();
			_menu.EventsOn();
			ftvTree.SelectedNodeItem = sel;
		}
		private void finButtonSave_Click(object sender, EventArgs e)
		{
			ShowProgressWindow();
			TaskManager.Run("Save", this, 
				()=> PulsarConnection.Default.Modify("MainMenu","ServedObject", new { ServedObject = _menu }, PulsarQueryParams.Servant));
		}
		private void lblEditSortOrder_UIValueChanged(SimLabelEditor sender, string args)
		{
			byte sortOrder;
			ITreeItem item = propertyGrid.SelectedObject as ITreeItem;
			if (item == null)
			{
				sender.Value = string.Empty;
				return;
			}
			if (args == string.Empty)
				item.SortOrder = null;
			else if (byte.TryParse(args, out sortOrder))
				item.SortOrder = sortOrder;
			else
				sender.Value = item.SortOrder == null ? string.Empty : item.SortOrder.ToString();
			propertyGrid.Refresh();
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
	}
}

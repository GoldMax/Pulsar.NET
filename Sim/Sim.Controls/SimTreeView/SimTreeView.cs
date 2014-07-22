using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Data;
using System.Security.Permissions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

using Pulsar;

namespace Sim.Controls
{
	#region public enum TreeViewKind : byte
	/// <summary>
	/// ������������ ����� ������ SimTreeView.
	/// </summary>
	public enum TreeViewKind : byte
	{
		/// <summary>
		/// ������ ���
		/// </summary>
		FirstView = 1,
		/// <summary>
		/// ������ ���
		/// </summary>
		SecondView = 2
	}
	#endregion public enum TreeViewKind : byte
	//**************************************************************************************
	#region public enum CheckBoxesType
	/// <summary>
	/// ��� CheckBox, ������������ � ��������.
	/// </summary>
	public enum CheckBoxesType
	{
		/// <summary>
		/// CheckBox'� �� ������������.
		/// </summary>
		None,
		/// <summary>
		/// ������������ CheckBox'� � ����� �����������.
		/// </summary>
		TwoState,
		/// <summary>
		/// ������������ CheckBox'� � ����� �����������.
		/// </summary>
		ThreeState
	}
	#endregion public enum CheckBoxesType
	//**************************************************************************************
	#region << public enum NeedImageIndexEventTarget >>
	/// <summary>
	/// ������������ ����� ������, ��� ������� ������������ ������� NeedImageIndex.
	/// </summary>
	public enum NeedImageIndexEventTarget
	{
		/// <summary>
		/// ��� ����.
		/// </summary>
		AllNodes,
		/// <summary>
		/// �������� (��� ��������) ����.
		/// </summary>
		EndNodes
	}
	#endregion << public enum NeedImageIndexEventTarget >>
	//**************************************************************************************
	/// <summary>
	/// ����� �������� ������������ �������������.
	/// </summary>
	[DefaultEvent("SelectedNodeChanged")]
	[Designer(typeof(SimTreeViewDesigner))]
	public partial class SimTreeView : UserControl
	{
		BackgroundWorker findWorker = new BackgroundWorker();
		private ITree tree = null;
		private InternalTreeView treeView = null;
		private bool isUpdated = false;
		private int closedImageIndex = 0;
		private int openedImageIndex = 0;
		private int nodeItemsImageIndex = 0;
		private NeedImageIndexEventTarget needImageIndexEventTarget = NeedImageIndexEventTarget.EndNodes;
		private object filter = null;
		private Comparison<SimTreeNode> comparer = null;
		private bool findButtonVisible = true;
		private bool viewButtonsVisible = true;
		private string nodeTextPropName = "";

		private bool allowInternalDragDrop = false;
		private Cursor cursorCan = null;
		private Cursor cursorCanNot = null;

		private Color borderColor = SystemColors.ControlDark;
		private BorderStyle borderStyle = BorderStyle.Fixed3D;
		private Padding borderWidth = new Padding(1);
		private bool useVisualStyleBorderColor = true;

		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		#region NodeCheckStateChanged
		/// <summary>
		/// ������� ������� NodeCheckStateChanged.
		/// </summary>
		/// <param name="sender">��������� SimTreeView.</param>
		/// <param name="node">������, ��������� �������� ����������.</param>
		public delegate void NodeCheckStateChangedDelegate(object sender, SimTreeNode node);
		/// <summary>
		/// �������, ����������� ��� ��������� �������� Checked �������� SimTreeNode.
		/// </summary>
		[Category("Behavior"),
		Description("�������, ����������� ��� ��������� �������� Checked �������� SimTreeNode.")]
		public event NodeCheckStateChangedDelegate NodeCheckStateChanged;
		/// <summary>
		/// �������� ������� NodeCheckStateChanged.
		/// </summary>
		/// <param name="node"></param>
		protected void OnNodeCheckStateChanged(SimTreeNode node)
		{
			if(NodeCheckStateChanged != null)
				NodeCheckStateChanged(this, node);
		}
		#endregion NodeCheckStateChanged
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region SelectedNodeChanging Event
		/// <summary>
		/// ������� ������� SelectedNodeChanging.
		/// </summary>
		/// <param name="sender">������, ���������� �������.</param>
		/// <param name="args">�������� �������.</param>
		public delegate void SelectedNodeChangingHandler(object sender, CancelEventArgs<object> args);
		/// <summary>
		/// �������, ����������� �� ��������� �������� ����.
		/// </summary>
		[Description("�������, ����������� �� ��������� �������� ����."),
			Category("Own events")]
		public event SelectedNodeChangingHandler SelectedNodeChanging;
		/// <summary>
		/// �����, ���������� ������� SelectedNodeChanging.
		/// </summary>
		/// <param name="item">������� ������.</param>
		protected bool OnSelectedNodeChanging(ITreeItem item)
		{
			if(SelectedNodeChanging != null)
			{
				CancelEventArgs<object> args = new CancelEventArgs<object>(item);
				SelectedNodeChanging(this, args);
				return args.Cancel;
			}
			else
				return false; 
		}
		#endregion SelectedNodeChanged Event
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region SelectedNodeChanged Event
		/// <summary>
		/// ������� ������� SelectedNodeChanged.
		/// </summary>
		/// <param name="sender">������, ���������� �������.</param>
		/// <param name="item">������� ������ ���������� ����.</param>
		public delegate void SelectedNodeChangedHandler(object sender, ITreeItem item);
		/// <summary>
		/// �������, ����������� ����� ��������� �������� ����.
		/// </summary>
		[Description("�������, ����������� ����� ��������� �������� ����."),
			Category("Own events")]
		public event SelectedNodeChangedHandler SelectedNodeChanged;
		/// <summary>
		/// �����, ���������� ������� SelectedNodeChanged.
		/// </summary>
		/// <param name="item">������� ������.</param>
		protected void OnSelectedNodeChanged(ITreeItem item)
		{
			if(SelectedNodeChanged != null)
				SelectedNodeChanged(this, item);
		}
				#endregion SelectedNodeChanged Event
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region ViewChanged Event
		/// <summary>
		/// ������� ������� ViewChanged.
		/// </summary>
		/// <param name="sender">������, ���������� �������.</param>
		/// <param name="view">��� ���������� �������������.</param>
		public delegate void ViewChangedHandler(object sender, TreeViewKind view);
		/// <summary>
		/// �������, ����������� ��� ��������� �������� �������������.
		/// </summary>
		[Description("�������, ����������� ��� ��������� �������� �������������.")]
		[Category("Own events")]
		public event ViewChangedHandler ViewChanged;
		/// <summary>
		/// �����, ���������� ������� ViewChanged.
		/// </summary>
		/// <param name="view">��� ���������� �������������.</param>
		protected void OnViewChanged(TreeViewKind view)
		{
			if(ViewChanged != null)
				ViewChanged(this, view);
		}
		#endregion ViewChanged Event
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region NodeDoubleClick Event
		/// <summary>
		/// �������, ����������� ��� ������� ����� �� ����.
		/// </summary>
		[Description("�������, ����������� ��� ������� ����� �� ����."),
			Category("Own events")]
		public event SelectedNodeChangedHandler NodeDoubleClick;
		/// <summary>
		/// �����, ���������� ������� NodeDoubleClick.
		/// </summary>
		/// <param name="item">������� ������.</param>
		protected void OnNodeDoubleClick(ITreeItem item)
		{
			if(NodeDoubleClick != null)
				NodeDoubleClick(this, item);
		}
		#endregion NodeDoubleClick Event
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region NeedImageIndex Event
		/// <summary>
		/// ������� ����������� ������� NeedImageIndex.
		/// </summary>
		/// <param name="sender">������, ���������� �������.</param>
		/// <param name="args">�������� �������.</param>
		public delegate void NeedImageIndexEventHandler (object sender, NeedImageIndexEventArgs args);
		/// <summary>
		/// �������, ����������� ��� ������������� ����������� ������� ����������� ����, 
		/// �� �������� �������� ���������.
		/// </summary>
		[Description("�������, ����������� ��� ������� ����� �� ����."),
			Category("Own events")]
		public event NeedImageIndexEventHandler NeedImageIndex;
		/// <summary>
		/// �����, ���������� ������� NeedImageIndex.
		/// </summary>
		/// <param name="node">����, ��� �������� ������������ ������ �����������.</param>
		/// <returns></returns>
		protected int OnNeedImageIndex(SimTreeNode node)
		{
			if(NeedImageIndex == null)
				return node.TreeItem.IsRoot ? 0 : nodeItemsImageIndex;
			NeedImageIndexEventArgs args = new NeedImageIndexEventArgs(node.TreeItem, 
				node.ImageIndex == -1 ? nodeItemsImageIndex : node.ImageIndex);
			NeedImageIndex(this, args);
			return args.ImageIndex;
		}
		/// <summary>
		/// �����, ���������� ������� NeedImageIndex.
		/// </summary>
		/// <param name="node">����, ��� �������� ������������ ������ �����������.</param>
		/// <param name="imageIndex">�������������� ������ �����������.</param>
		/// <returns></returns>
		protected int OnNeedImageIndex(SimTreeNode node, int imageIndex)
		{
			if(NeedImageIndex == null)
				return node.TreeItem.IsRoot ? 0 : nodeItemsImageIndex;
			NeedImageIndexEventArgs args = new NeedImageIndexEventArgs(node.TreeItem, imageIndex);
			NeedImageIndex(this, args);
			return args.ImageIndex;
		}
		#endregion NeedImageIndex Event
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<CancelEventArgs<string>> BeginKeyFind; >>
		/// <summary>
		/// �������, ����������� �� ������ ������ �� �����
		/// </summary>
		[Description("�������, ����������� �� ������ ������ �� �����."),
			Category("Own events")]
		public event EventHandler<CancelEventArgs<string>> BeginKeyFind;
		/// <summary>
		/// �������� ������� BeginKeyFind
		/// </summary>
		/// <param name="mask">����� ������.</param>
		/// <returns></returns>
		protected virtual bool OnBeginKeyFind(string mask)
		{
			if(BeginKeyFind == null)
				return false;
			CancelEventArgs<string> arg = new CancelEventArgs<string>(mask);
			BeginKeyFind(this, arg);
			return arg.Cancel;
		} 
		#endregion << public event EventHandler<CancelEventArgs<string>> BeginKeyFind; >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<CanDrupItemEventArgs> CanDropItem >>
		/// <summary>
		/// ����� ��������� ������� DragDrop
		/// </summary>
		public class DragDropItemEventArgs : CancelEventArgs
		{
			/// <summary>
			/// ������� ����������
			/// </summary>
			public ITreeItem DestItem { get; set; }
			/// <summary>
			/// ��������������� �������
			/// </summary>
			public ITreeItem DroppedItem { get; set; }
			/// <summary>
			/// ���������������� �����������.
			/// </summary>
			/// <param name="droppedItem">��������������� �������</param>
			/// <param name="destItem">������� ����������</param>
			public DragDropItemEventArgs(ITreeItem droppedItem, ITreeItem destItem)
				: base(false)
			{
				DroppedItem = droppedItem;
				DestItem = destItem;
			}
		}
		[NonSerialized]
		private Pulsar.WeakEvent<DragDropItemEventArgs> _CanDropItem;
		/// <summary>
		/// �������, ����������� ��� ������������� ����������� ����������� ���������� ���������������� ��������
		/// � ��������� �������.
		/// </summary>
		[Description("�������, ����������� ��� ������������� ����������� ����������� ���������� ���������������� �������� � ��������� �������."),
			Category("Own events")]
		public event EventHandler<DragDropItemEventArgs> CanDropItem
		{
			add { _CanDropItem += value; }
			remove { _CanDropItem -= value; }
		}
		/// <summary>
		/// �������� ������� CanDropItem.
		/// </summary>
		protected virtual bool OnCanDropItem(ITreeItem droppedItem, ITreeItem destItem)
		{
			if(_CanDropItem != null)
			{
				DragDropItemEventArgs arg = new DragDropItemEventArgs(droppedItem, destItem);
				_CanDropItem.Raise(this, arg);
				return !arg.Cancel;
			}
			return true;
		} 
		#endregion << public event EventHandler<CanDrupItemEventArgs> CanDropItem >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<DragDropItemEventArgs> ItemDropped >>
		[NonSerialized]
		private Pulsar.WeakEvent<DragDropItemEventArgs> _ItemDropped;
		/// <summary>
		/// �������, ����������� ��� ��������� �������������� ��������
		/// </summary>
		[Description("�������, ����������� ��� ��������� �������������� ��������."),
			Category("Own events")]
		public event EventHandler<DragDropItemEventArgs> ItemDropped
		{
			add { _ItemDropped += value; }
			remove { _ItemDropped -= value; }
		}
		/// <summary>
		/// �������� ������� ItemDropped.
		/// </summary>
		protected virtual void OnItemDropped(ITreeItem droppedItem, ITreeItem destItem)
		{
			if(_ItemDropped != null)
				_ItemDropped.Raise(this, new DragDropItemEventArgs(droppedItem, destItem));
		} 
		#endregion << public event EventHandler<DragDropItemEventArgs> ItemDropped >>
		#endregion << Events >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		#region << NonBrowsable Properties >>
		/// <summary>
		/// ���������� ������ ������.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITree Tree
		{
			get { return tree; }
			set 
			{
				Reset();
				BeginUpdate();
				tree = value;
				if(value != null)
				{
					treeView.Nodes.Comparer = comparer;
					BuildTree();
					tree.ObjectChanged += (tree_TreeChanged);
					tree.ItemChanged += (tree_ItemChanged);
					tree.ItemAdded += (tree_ItemAdded);
					tree.ItemDeleting += (tree_ItemRemoving);
					tree.ItemMoved += new EventHandler<TreeItemMoveEventArgs>(tree_ItemMoved);
				}
				EndUpdate(false);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������ ������� ������.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Filter
		{
			get { return filter; }
			set { filter = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������� ������ ���������� ����.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITreeItem SelectedNodeItem
		{
			get
			{
				if(treeView.SelectedNode == null)
					return null;
				return treeView.SelectedNode.TreeItem;
			}
			set
			{
				SelectNodeWithItem(value);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������� ��� �������������.
		/// </summary>
		[Browsable(false)]
		public TreeViewKind CurrentView
		{
			get
			{
				if(btnFirstView.Checked)
					return TreeViewKind.FirstView;
				else
					return TreeViewKind.SecondView;
			}
			set
			{
				if(value == TreeViewKind.FirstView)
				{
					btnFirstView.Checked = true;
					btnSecondView.Checked = false;
				}
				else
				{
					btnFirstView.Checked = false;
					btnSecondView.Checked = true;
				}
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ����� �� ������� ����� �����.
		/// </summary>
		[Browsable(false)]
		public override bool Focused
		{
			get { return (base.Focused || treeView.Focused); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ��������� �� ������ �������� � ��������� ����������.
		/// </summary>
		[Browsable(false)]
		public bool IsUpdated
		{
			get { return isUpdated; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ �������.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStripItem btnFilter
		{
			get { return this.mainToolStrip.Items["btnFilter"]; }
			set 
			{ 
				if(mainToolStrip.Items.ContainsKey("btnFilter"))
					mainToolStrip.Items.RemoveByKey("btnFilter");
				if(value != null)
				{
					value.Name = "btnFilter";
					mainToolStrip.Items.Add(value);
				}
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����� ��������� ��������� ������ ��� ����������.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Comparison<SimTreeNode> Comparer
		{
			get { return comparer; }
			set 
			{ 
				comparer = value; 
				treeView.Nodes.Comparer = comparer;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����� ������ ����� � ������.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Func<object, string, uint?, bool> KeyFindMethod { get; set; }
		#endregion << NonBrowsable Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Browsable properties >>
		/// <summary>
		/// ��� �������� ������� �������� ������, ������������ � �������� ����� ������.
		/// </summary>
		[Description(" ��� �������� ������� �������� ������, ������������ � �������� ����� ������.")]
		[Category("SimTreeView Properties")]
		[DefaultValue("")]
		public string NodeTextPropName 
		{ 
			get { return nodeTextPropName; }
			set { nodeTextPropName = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ������ ������
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ��������� ������ ������.")]
		[DefaultValue(true)]
		public bool FindButtonVisible
		{
			get { return findButtonVisible; }
			set 
			{
				findButtonVisible = value;
				btnFind.Visible = findButtonVisible; 
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ������ ����� ���� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ��������� ������ ����� ���� ������.")]
		[DefaultValue(true)]
		public bool ViewButtonsVisible
		{
			get { return viewButtonsVisible; }
			set 
			{ 
				viewButtonsVisible = value;
				btnFirstView.Visible = btnSecondView.Visible = toolStripSeparator1.Visible = viewButtonsVisible; 
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ����� �� ���������� �������� ������ ������ ������� ����.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("����������, ����� �� ���������� �������� ������ ������ ������� ����.")]
		[DefaultValue(true)]
		public bool RightClickSelect
		{
			get { return treeView.rightClick; }
			set { treeView.rightClick = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ��������� ������� ��� ����������� CheckBox'�� � ��������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("��������� ������� ��� ����������� CheckBox'�� � ��������.")]
		[DefaultValue(CheckBoxesType.None)]
		public CheckBoxesType CheckBoxes
		{
			get { return treeView.chType; }
			set
			{
				treeView.chType = value;
				treeView.ResetStates();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ����� �� ������ �������������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("����������, ����� �� ������ �������������.")]
		[DefaultValue(false)]
		[Browsable(true)]
		public bool Sorted
		{
			get { return treeView.Nodes.Sorted; }  //  sorted;
			set { treeView.Nodes.Sorted = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ��������� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ��������� ��������� ������.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public SimTreeNodeCollection Nodes
		{
			get { return treeView.Nodes; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ����� �� �������������� ��������� ������� ��� ������ ��������� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("����������, ����� �� �������������� ��������� ������� ��� ������ ��������� ������.")]
		[DefaultValue(true)]
		public bool HideSelection
		{
			get { return treeView.HideSelection; }
			set { treeView.HideSelection = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ����� �������� �������� ���������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����������� ����� �������� �������� ���������.")]
		[DefaultValue(true)]
		public bool ShowRootLines
		{
			get { return treeView.ShowRootLines; }
			set { treeView.ShowRootLines = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ��������� ��������� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����������� ��������� ��������� ������.")]
		[DefaultValue(true)]
		public bool ShowNodeToolTips
		{
			get { return treeView.ShowNodeToolTips; }
			set { treeView.ShowNodeToolTips = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ �������� ��������� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ������ �������� ��������� ������.")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get { return treeView.ImageList; }
			set { treeView.ImageList = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ �������� ��������� ���� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ������ �������� ��������� ���� ������.")]
		[DefaultValue(0)]
		//[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public int ClosedNodeImageIndex
		{
			get { return closedImageIndex; }
			set { closedImageIndex = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ �������� ��������� ���� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ������ �������� ��������� ���� ������.")]
		[DefaultValue(0)]
		public int OpenedNodeImageIndex
		{
			get { return openedImageIndex; }
			set { openedImageIndex = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ �������� ��������� ���� ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ������ �������� ��������� ���� ������.")]
		[DefaultValue(0)]
		public int NodeItemsImageIndex
		{
			get { return nodeItemsImageIndex; }
			set 
			{ 
				treeView.ImageIndex = value;
				treeView.SelectedImageIndex = value;
				nodeItemsImageIndex = value; 
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����, ��� ������� ����� �������������� ������� NeedImageIndex.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����, ��� ������� ����� �������������� ������� NeedImageIndex.")]
		[DefaultValue(typeof(NeedImageIndexEventTarget), "EndNodes")]
		public NeedImageIndexEventTarget NeedImageIndexEventTarget
		{
			get { return needImageIndexEventTarget; }
			set { needImageIndexEventTarget = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ������ ������� ���� �������������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����������� ������ ������� ���� �������������.")]
		[DefaultValue(true)]
		public bool FirstViewButtonEnabled
		{
			get { return btnFirstView.Enabled; }
			set { btnFirstView.Enabled = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����� ����������� ��������� ������ ������� ���� �������������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����� ����������� ��������� ������ ������� ���� �������������.")]
		[DefaultValue("����������� �������������")]
		public string FirstViewButtonToolTipText
		{
			get { return btnFirstView.ToolTipText; }
			set { btnFirstView.ToolTipText = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ������ ������� ���� �������������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����������� ������ ������� ���� �������������.")]
		public Image FirstViewButtonImage
		{
			get { return btnFirstView.Image; }
			set { btnFirstView.Image = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ������ ������� ���� �������������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����������� ������ ������� ���� �������������.")]
		[DefaultValue(false)]
		public bool SecondViewButtonEnabled
		{
			get { return btnSecondView.Enabled; }
			set { btnSecondView.Enabled = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����� ����������� ��������� ������ ������� ���� �������������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����� ����������� ��������� ������ ������� ���� �������������.")]
		[DefaultValue("������������� � ���� ������")]
		public string SecondViewButtonToolTipText
		{
			get { return btnSecondView.ToolTipText; }
			set { btnSecondView.ToolTipText = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ������ ������� ���� �������������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����������� ������ ������� ���� �������������.")]
		public Image SecondViewButtonImage
		{
			get { return btnSecondView.Image; }
			set { btnSecondView.Image = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� MainToolStrip 
		/// </summary>
		[Category("Appearance")]
		[Description("���������� MainToolStrip")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimToolStrip MainToolStrip
		{
			get { return mainToolStrip; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ���� ����� ��� BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("���������� ���� ����� ��� BorderStyle = FixedSingle.")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color BorderColor
		{
			get { return borderColor; }
			set 
			{ 
				borderColor = value; 
				fpMain.BorderColor = borderColor;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��� �����.
		/// </summary>
		[Category("Appearance")]
		[Description("���������� ��� �����.")]
		[DefaultValue(typeof(BorderStyle), "Fixed3D")]
		public new BorderStyle BorderStyle
		{
			get { return borderStyle; }
			set
			{
				borderStyle = value;
				switch(borderStyle)
				{
					case BorderStyle.None : 
						fpMain.BorderStyle = BorderStyle.None;
						treeView.BorderStyle = BorderStyle.None;
						break;
					case BorderStyle.FixedSingle:
						treeView.BorderStyle = BorderStyle.None;
						fpMain.BorderStyle = BorderStyle.FixedSingle;
						break;
					case BorderStyle.Fixed3D:
						fpMain.BorderStyle = BorderStyle.None;
						treeView.BorderStyle = BorderStyle.Fixed3D;
						break;
				}
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ ����� ��� BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("���������� ������ ����� ��� BorderStyle = FixedSingle.")]
		[DefaultValue(typeof(Padding), "1, 1, 1, 1")]
		public Padding BorderWidth
		{
			get { return borderWidth; }
			set
			{
				borderWidth = value;
				fpMain.BorderWidth = borderWidth;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ����� �� ������������ ����� �����.
		/// </summary>
		[Category("Appearance")]
		[Description("����������, ����� �� ������������ ����� �����.")]
		[DefaultValue(true)]
		public bool UseVisualStyleBorderColor
		{
			get { return useVisualStyleBorderColor; }
			set
			{
				useVisualStyleBorderColor = value;
				fpMain.UseVisualStyleBorderColor = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������� ���� ������
		/// </summary>
		[Category("Behavior")]
		[Description("����������� ���� ������")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public override ContextMenuStrip ContextMenuStrip
		{
			get { return this.treeContextMenuStrip; }
			set { this.treeContextMenuStrip = (ContextMenuStrip)value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ����������� �������������� ������ � ������.
		/// </summary>
		[Category("SimTreeView Properties")]
		[Description("���������� ����������� ����������� �������������� ������ � ������.")]
		[DefaultValue(false)]
		public bool AllowInternalDragDrop
		{
			get { return allowInternalDragDrop; }
			set 
			{ 
				allowInternalDragDrop = value; 
				this.AllowDrop = value;
			}
		}
		#endregion << Browsable properties >>
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public SimTreeView()
		{
			InitializeComponent();
			splitContainer1.Panel2Collapsed = true;

			NodeTextPropName = "";

			treeView = new InternalTreeView(this);
			fpMain.Controls.Add(treeView);
			treeView.BorderStyle = BorderStyle.Fixed3D;
			treeView.Dock = DockStyle.Fill;
			treeView.BringToFront();
			treeView.BeforeSelect += new TreeViewCancelEventHandler(tree_BeforeSelect);
			treeView.AfterSelect += new TreeViewEventHandler(tree_AfterSelect);
			treeView.BeforeExpand += new TreeViewCancelEventHandler(tree_BeforeExpand);
			treeView.AfterCollapse += new TreeViewEventHandler(tree_AfterCollapse);
			treeView.KeyDown += new KeyEventHandler(tree_KeyDown);
			treeView.KeyPress += new KeyPressEventHandler(tree_KeyPress);
			treeView.KeyUp += new KeyEventHandler(tree_KeyUp);
			treeView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tree_NodeMouseDoubleClick);
			treeView.MouseLeave += new EventHandler(tree_MouseLeave);
			treeView.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);
			treeView.ContextMenuStrip = treeContextMenuStrip;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << SimTree Handlers >>
		void tree_ItemAdded(object sender, CollectionChangeNotifyEventArgs args)
		{
			try
			{
				ITreeItem item = (ITreeItem)args.Item;
				if(isUpdated)
					return;
				if(item.Parent == null)
				{
					SimTreeNode node = new SimTreeNode(item, NodeTextPropName);
					if(item.HasChildren)
					{
						if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
							node.ImageIndex = OnNeedImageIndex(node, closedImageIndex);
						else
							node.ImageIndex = node.SelectedImageIndex = closedImageIndex;
					}
					else
						node.SelectedImageIndex = node.ImageIndex = OnNeedImageIndex(node);
					treeView.Nodes.Add(node);
				}
				else  //if(item.Parent.Props.ContainsParam("Node"))
				{
					SimTreeNode pNode = treeView.Nodes.Find(item.Parent);
					if(pNode != null)
						if(pNode.Nodes.Count == 1 && pNode.Nodes[0].Name == "-1")
						{
							pNode.Nodes.Clear();
							foreach(ITreeItem key in item.Parent.Children)
							{
								SimTreeNode node = new SimTreeNode(key, NodeTextPropName);
								if(key.HasChildren)
									node.Nodes.Add("-1", "hidden");
								if(treeView.ImageList != null)
									if(key.HasChildren)
									{
										if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
											node.ImageIndex = OnNeedImageIndex(node, closedImageIndex);
										else
											node.ImageIndex = node.SelectedImageIndex = closedImageIndex;
									}
									else
										node.SelectedImageIndex = node.ImageIndex = OnNeedImageIndex(node);
								pNode.Nodes.Add(node);
							}
						}
						else
						{ 
							SimTreeNode node = new SimTreeNode(item, NodeTextPropName);
							if(item.HasChildren)
								node.Nodes.Add("-1", "hidden");
							if(treeView.ImageList != null)
								if(item.IsGroup)  
								{
									if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
										node.ImageIndex = OnNeedImageIndex(node, closedImageIndex);
									else
										node.ImageIndex = node.SelectedImageIndex = closedImageIndex;
								}
								else
									node.SelectedImageIndex = node.ImageIndex = OnNeedImageIndex(node);
							pNode.Nodes.Add(node);
						} 
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
			//throw new Exception("The method or operation is not implemented.");
		}
		//-------------------------------------------------------------------------------------
		void tree_ItemRemoving(object sender, CollectionChangeNotifyEventArgs args)
		{
			try
			{
				SimTreeNode node = treeView.Nodes.Find((ITreeItem)args.Item);
				if(isUpdated == false && node != null)
					node.Remove();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void tree_ItemChanged(object sender, CollectionChangeNotifyEventArgs args)
		{
			SimTreeNode node = treeView.Nodes.Find((ITreeItem)args.Item);
			if(isUpdated == false && node != null)
				node.Text = ((ITreeItem)args.Item).ItemText;
		}
		//-------------------------------------------------------------------------------------
		void tree_ItemMoved(object sender, TreeItemMoveEventArgs e)
		{
			try
			{
				SimTreeNode node = treeView.Nodes.Find((ITreeItem)e.Item);
				SimTreeNode oldNode = node.Parent;
				SimTreeNode newNode = e.NewParentItem == null ? null : treeView.Nodes.Find((ITreeItem)e.NewParentItem);
				if(isUpdated == false && node != null)
				{
					node.Remove();
					if(newNode == null)
						treeView.Nodes.Add(node);
					else if(!(newNode.Nodes.Count == 1 && newNode.Nodes[0].Name == "-1"))
						newNode.Nodes.Add(node);
					//treeView.SelectedNode = node;
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
			
		}
		//-------------------------------------------------------------------------------------
		void tree_TreeChanged(object sender, ObjectChangeNotifyEventArgs e)
		{
		 if(e.Action == ChangeNotifyAction.ObjectReset)
			 BuildTree();
		}
		//-------------------------------------------------------------------------------------
		private void treeContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				ContextMenuStrip menu = (ContextMenuStrip)sender;
				menu.Hide();
				if(tree == null || SelectedNodeItem == null)
					return;
				switch(e.ClickedItem.Name)
				{
					case "toolStripMenuItemCount":
						{
							Point p = menu.Location;
							SimLabel l = new SimLabel();
							l.Image = Properties.Resources.Info;
							l.Text = "���������� �������� ���������: " + tree.GetEndItems(SelectedNodeItem).Count().ToString();
							l.BackColor = Color.Transparent;
							l.Width = l.GetPreferredSize(Size.Empty).Width + 20;
							SimPopupControl.Show(l, p, false, true);
							//MessageBox.Show("���������� �������� ���������: " + 
							//                 tree.GetEndItems(SelectedNodeItem).Count.ToString(),
							//                 "���������� ���������",
							//                 MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						break;
					case "toolStripMenuItemExAll":
						ExpandAll(SelectedNodeItem);
						break;
					case "toolStripMenuItemColAll":
						CollapseAll(SelectedNodeItem);
						if(SelectedNodeItem.Parent == null)
							Expand(SelectedNodeItem);
						break;
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		#endregion << SimTree Handlers >>
		//-------------------------------------------------------------------------------------
		#region Tree Methods
		/// <summary>
		/// ������ ��������� ������.
		/// </summary>
		private void BuildTree()
		{
			try
			{
				List<SimTreeNode> nodes = new List<SimTreeNode>();
				foreach(ITreeItem pid in tree.GetRootItems())
				{
					SimTreeNode node = new SimTreeNode(pid, NodeTextPropName);
					if(pid.HasChildren)
						node.Nodes.Add("-1", "hidden");
					if(treeView.ImageList != null)
						if(pid.HasChildren)
						{
							if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
								node.ImageIndex = OnNeedImageIndex(node, closedImageIndex);
							else
								node.ImageIndex = node.SelectedImageIndex = closedImageIndex;
						}
						else
							node.SelectedImageIndex = node.ImageIndex = OnNeedImageIndex(node);
					nodes.Add(node);
				}
				treeView.BeginUpdate();
				treeView.Nodes.Clear();
				treeView.Nodes.AddRange(nodes);

				if(treeView.Nodes.Count > 0)
					treeView.Nodes[0].Expand();

				btnFind.Enabled = true;
				treeView.ResetStates();
				treeView.EndUpdate();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ���� � �������� ��������� ������.
		/// </summary>
		/// <param name="item">������� ������ ������������� ����.</param>
		public void SelectNodeWithItem(ITreeItem item)
		{
			SelectNodeWithItem(item, false);
		}
		/// <summary>
		/// ���������� ���� � �������� ��������� ������.
		/// </summary>
		/// <param name="item">������� ������ ������������� ����.</param>
		/// <param name="expand">����������, ����� �� ������������ ���� ���������.</param>
		public void SelectNodeWithItem(ITreeItem item, bool expand)
		{
			try
			{
				if(tree == null)
					return; 
				if(item == null)
				{
					treeView.SelectedNode = null;
					OnSelectedNodeChanged(null);
					return;
				} 
				if(!tree.Contains(item))
					throw new ArgumentException(String.Format("������� � {0} �� �������!", item));
				IList pids = tree.GetParentsItemsList(item);
				SimTreeNodeCollection nodes = treeView.Nodes;
				foreach(ITreeItem pos in pids)
				{
					if(!nodes.ContainsItem(pos))
						throw new ArgumentException(String.Format("������� � {0} ����������� � ������� ������!", item));
					nodes[pos].Expand();
					nodes = nodes[pos].Nodes;
				}
				if(!nodes.ContainsItem(item))
					throw new ArgumentException(String.Format("������� � {0} ����������� � ������� ������!", item));
				treeView.SelectedNode = nodes[item];
				if(expand)
					treeView.SelectedNode.Expand();
				nodes[item].EnsureVisible();
				treeView.Select();
				treeView.Focus();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ���� � �������� ��������.
		/// </summary>
		/// <param name="obj">������ ������������� ����.</param>
		public void SelectNodeWithObject(object obj)
		{
			SelectNodeWithObject(obj, false);
		}
		/// <summary>
		/// ���������� ���� � �������� ��������� ������.
		/// </summary>
		/// <param name="obj">������ ������������� ����.</param>
		/// <param name="expand">����������, ����� �� ������������ ���� ���������.</param>
		public void SelectNodeWithObject(object obj, bool expand)
		{
			try
			{
				if(tree.Objects.Contains(obj) == false)
					throw new Exception("������ �� �������� ������� � ��������� ��������!");
				SelectNodeWithItem(tree[obj]);
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ������ � ��������� ����������.
		/// </summary>
		public void BeginUpdate()
		{
			isUpdated = true;
			treeView.BeginUpdate();
		}
		/// <summary>
		/// ������� ������ �� ��������� ����������.
		/// </summary>
		public void EndUpdate()
		{
			EndUpdate(true);
		}
		/// <summary>
		/// ������� ������ �� ��������� ����������.
		/// </summary>
		/// <param name="rebuildTree">����������, ����� �� ������������� ������.</param>
		public void EndUpdate(bool rebuildTree)
		{
			isUpdated = false;
			if(rebuildTree)
				BuildTree();
			treeView.EndUpdate(); 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ������.
		/// </summary>
		public void Sort()
		{
			treeView.Sort();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������������� ��� ���� ������.
		/// </summary>
		public void ExpandAll()
		{
			try
			{
				treeView.BeginUpdate();
				treeView.ExpandAll();
				treeView.EndUpdate();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		/// <summary>
		/// ������������� ��� �������� ���� ���������� ���� ��������.
		/// </summary>
		/// <param name="item">������� ���������������� ����.</param>
		public void ExpandAll(ITreeItem item)
		{
			try
			{
				treeView.BeginUpdate();
				SimTreeNodeCollection nodes = treeView.Nodes;
				foreach(ITreeItem pid in tree.GetParentsItemsList(item))
				{
					nodes[pid].Expand();
					nodes = nodes[pid].Nodes;
				}
				nodes[item].ExpandAll();
				nodes[item].EnsureVisible();
				treeView.EndUpdate();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������������� ���� ���������� ��������.
		/// </summary>
		/// <param name="item">������� ���������������� ����.</param>
		public void Expand(ITreeItem item)
		{
			try
			{
				treeView.BeginUpdate();
				SimTreeNodeCollection nodes = treeView.Nodes;
				foreach(ITreeItem pid in tree.GetParentsItemsList(item))
				{
					nodes[pid].Expand();
					nodes = nodes[pid].Nodes;
				}
				nodes[item].Expand();
				nodes[item].EnsureVisible();
				treeView.EndUpdate();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����������� ��� ���� ������.
		/// </summary>
		public void CollapseAll()
		{
			try
			{
				treeView.CollapseAll();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		} 
		/// <summary>
		/// ����������� ���� ���������� ��������.
		/// </summary>
		/// <param name="item">������� �������������� ����.</param>
		public void CollapseAll(ITreeItem item)
		{
			try
			{
				treeView.BeginUpdate();
				SelectNodeWithItem(item);
				treeView.SelectedNode.Collapse();
				treeView.EndUpdate();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ���� � �������� ��������� ������.
		/// </summary>
		public SimTreeNode FindNode(ITreeItem item)
		{
			if(tree.Contains(item) == false)
				return null;
			return treeView.Nodes.Find(item);
		}
		#endregion Tree Methods
		//-------------------------------------------------------------------------------------
		#region treeView Handlers
		void tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if(e.Node == null)
				e.Cancel = OnSelectedNodeChanging(null); 
			else
				e.Cancel = OnSelectedNodeChanging(((SimTreeNode)e.Node).TreeItem);
		}
		//-------------------------------------------------------------------------------------
		void tree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if(e.Node == null)
				OnSelectedNodeChanged(null); 
			else 
				OnSelectedNodeChanged(((SimTreeNode)e.Node).TreeItem);
		}
		//-------------------------------------------------------------------------------------
		void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			try
			{
				SimTreeNode node = (SimTreeNode)e.Node;

				if(node.TreeItem.HasChildren && node.Nodes[0].Name != "-1")
				{
					treeView.ResetNodeState(node);
					if(treeView.ImageList != null && node.TreeItem.HasChildren)
					{
						if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
							node.ImageIndex = OnNeedImageIndex(node, openedImageIndex);
						else
							node.ImageIndex = node.SelectedImageIndex = openedImageIndex;
					}
					return;
				}
				node.Nodes.Clear();
				ITreeItem item = (ITreeItem)node.TreeItem;
				List<SimTreeNode> nodes = new List<SimTreeNode>(item.Children.Count());
				foreach(ITreeItem key in item.Children)
				{
					SimTreeNode n = new SimTreeNode(key, NodeTextPropName);
					if(key.HasChildren)
						n.Nodes.Add("-1", "hidden");
					if(treeView.ImageList != null)
						if(key.HasChildren)
						{
							if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
								n.ImageIndex = OnNeedImageIndex(n, closedImageIndex);
							else
								n.ImageIndex = n.SelectedImageIndex = closedImageIndex;
						} 
						else
							n.SelectedImageIndex = n.ImageIndex = OnNeedImageIndex(n);
					nodes.Add(n);
				}
				node.Nodes.AddRange(nodes);
				if(treeView.ImageList != null && node.TreeItem.HasChildren)
					if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
						node.ImageIndex = OnNeedImageIndex(node, openedImageIndex);
					else
						node.ImageIndex = node.SelectedImageIndex = openedImageIndex;
				treeView.ResetNodeState(node);
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void tree_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			SimTreeNode node = (SimTreeNode)e.Node;
			if(treeView.ImageList != null && node.TreeItem.HasChildren)  
			{
				if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
					node.ImageIndex = OnNeedImageIndex(node, closedImageIndex);
				else
					node.ImageIndex = node.SelectedImageIndex = closedImageIndex;
			}
		}
		//-------------------------------------------------------------------------------------
		void tree_KeyDown(object sender, KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}
		//-------------------------------------------------------------------------------------
		void tree_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}
		//-------------------------------------------------------------------------------------
		void tree_KeyUp(object sender, KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}
		//-------------------------------------------------------------------------------------
		void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			OnNodeDoubleClick(((SimTreeNode)e.Node).TreeItem);
		}
		//-------------------------------------------------------------------------------------
		void tree_MouseLeave(object sender, EventArgs e)
		{
			base.OnMouseLeave(e);
		}
		#endregion treeView Handlers
		//-------------------------------------------------------------------------------------
		#region mainToolStrip Handlers
		private void btnView_Click(object sender, EventArgs e)
		{
			ToolStripButton btn = (ToolStripButton)sender;
			if(btn.Checked)
				return;
			if(btn.Name == "btnFirstView")
			{
				btnSecondView.Checked = false;
				btnFirstView.Checked = true;
				OnViewChanged(TreeViewKind.FirstView);
			}
			else
			{
				btnFirstView.Checked = false;
				btnSecondView.Checked = true;
				OnViewChanged(TreeViewKind.SecondView);
			}
		}
		//-------------------------------------------------------------------------------------
		private void toolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == '\r')
				contextMenuStripFind.Close(ToolStripDropDownCloseReason.ItemClicked);
			else if(e.KeyChar == '\b')
				e.Handled = false;
			//else if(toolStripTextBoxID.Equals(sender) && !Char.IsDigit(e.KeyChar))
			// e.Handled = true;
		}
		//-------------------------------------------------------------------------------------
		private void toolStripTextBox_TextChanged(object sender, EventArgs e)
		{
			ToolStripTextBox tb = (ToolStripTextBox)sender;
			if(tb.Text.Length == 0)
				return;
			if(tb.Tag.ToString() == "id")
			{
				if(toolStripTextBoxName.Text.Length > 0)
					toolStripTextBoxName.Text = "";
			}
			else
			{
				if(toolStripTextBoxID.Text.Length > 0)
					toolStripTextBoxID.Text = "";
			}
		}
		//-------------------------------------------------------------------------------------
		private void contextMenuStripFind_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			if(e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
				BeginFind();
		}
		//-------------------------------------------------------------------------------------
		private void toolStripDropDownButtonFind_DropDownOpened(object sender, EventArgs e)
		{
			try
			{
				toolStripTextBoxID.SelectAll();
				toolStripTextBoxID.Select();
				toolStripTextBoxID.Focus();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		#endregion mainToolStrip Handlers
		//-------------------------------------------------------------------------------------
		#region ListBox Handlers
		private void listBoxFindResult_DrawItem(object sender, DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			e.DrawBackground();
			if(e.Index == -1)
				return;
			string text = listBoxFindResult.Items[e.Index].ToString();
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Near;
			sf.LineAlignment = StringAlignment.Center;
			sf.Trimming = StringTrimming.EllipsisCharacter;
			g.DrawString(text, listBoxFindResult.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
		}
		//-------------------------------------------------------------------------------------
		private void listBoxFindResult_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				int pos = listBoxFindResult.IndexFromPoint(e.Location);
				if(pos == ListBox.NoMatches)
					return;
				if(toolTip1.GetToolTip(listBoxFindResult) == listBoxFindResult.Items[pos].ToString() && toolTip1.Active)
					return;
				Graphics g = listBoxFindResult.CreateGraphics();

				Size preferredSize = g.MeasureString(listBoxFindResult.Items[pos].ToString(),
																																									listBoxFindResult.Font).ToSize();
				if(preferredSize.Width > listBoxFindResult.ClientRectangle.Width)
					toolTip1.SetToolTip(listBoxFindResult, listBoxFindResult.Items[pos].ToString());
				else
					toolTip1.RemoveAll();
				g.Dispose();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void listBoxFindResult_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int pos = listBoxFindResult.IndexFromPoint(e.Location);
			if(pos == ListBox.NoMatches)
				return;
			ComboBoxItem<ITreeItem> i = (ComboBoxItem<ITreeItem>)listBoxFindResult.Items[pos];
			SelectNodeWithItem(i.Key);
		}
		#endregion ListBox Handlers
		//-------------------------------------------------------------------------------------
		#region Find Methods
		private void BeginFind()
		{
			try
			{
				if(toolStripTextBoxID.Text.Length > 0)
				{
					if(tree == null)
						return;

					string mask = toolStripTextBoxID.Text.TrimAll();

					if(OnBeginKeyFind(mask))
						return;

					if(KeyFindMethod != null)
					{
						uint? u = null;
						uint val;
						if(UInt32.TryParse(mask, out val))
							u = val;
						foreach(ITreeItem i in tree)
							if(i.Object != null && KeyFindMethod(i.Object,mask, u)) 
							{
								SelectNodeWithItem(i);
								return;
							}
					}
					else
					{
						Type t = tree.ObjectType;
						PropertyInfo pi = t.GetProperty("ID", BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.Instance
																																												| BindingFlags.NonPublic | BindingFlags.Public );
						if(pi == null)
							throw new PulsarException("��� �������� [{0}] ����� ������ �� ����� �������� ID!", t.FullName);
						foreach(ITreeItem i in tree)
						{
							if(i.Object == null)
								continue;
							object val = pi.GetValue(i.Object, null);
							if(val != null && val.ToString() == mask)
							{
								SelectNodeWithItem(i);
								return;
							}
						}
					}
					string s = String.Format("������� � {0} �� �������!", toolStripTextBoxID.Text);
					MessageBox.Show(this, s, "�����", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					if(findWorker.IsBusy)
						findWorker.CancelAsync();
					Application.DoEvents();
					pictureBoxFind.Visible = false;
					buttonFindStop.Enabled = true;
					buttonFindStop.Visible = true;
					labelFindQuery.Text = "�����: " + toolStripTextBoxName.Text;
					listBoxFindResult.Items.Clear();
					splitContainer1.Panel2Collapsed = false;
					splitContainer1.SplitterDistance += splitContainer1.Panel2.Height - (panel1.Height + listBoxFindResult.Height) - 3;
					findWorker = new BackgroundWorker();
					findWorker.DoWork += new DoWorkEventHandler(findWorker_DoWork);
					findWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(findWorker_RunWorkerCompleted);
					findWorker.WorkerReportsProgress = true;
					findWorker.WorkerSupportsCancellation = true;
					findWorker.ProgressChanged += new ProgressChangedEventHandler(findWorker_ProgressChanged);
					findWorker.RunWorkerAsync(1);
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void findWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string s = toolStripTextBoxName.Text.ToLower();
			s = Regex.Escape(s);
			s = "^" + s + "$";
			s = s.Replace("\\*", ".*");
			s = s.Replace("\\?", ".{1}");
			Regex rx = new Regex(s, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			//for(int a = 0;a < 10000;a++)
			foreach(ITreeItem item in tree)
			{
				if(rx.IsMatch(SimTreeNode.GetNodeText(item, NodeTextPropName)))
				{
					ProgressChangedEventHandler eh = findWorker_ProgressChanged;
					if(listBoxFindResult.FindForm() == null)
						findWorker.CancelAsync();
					else
						listBoxFindResult.Invoke(eh, new object[] { null, new ProgressChangedEventArgs(0, item) });
						//findWorker.ReportProgress(0, item);
				}
				Thread.Sleep(0);
				if(findWorker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}
			}
		}
		//-------------------------------------------------------------------------------------
		void findWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				ITreeItem i = (ITreeItem)e.UserState;
				listBoxFindResult.Items.Add(new ComboBoxItem<ITreeItem>(i, 
						SimTreeNode.GetNodeText(i, NodeTextPropName)));
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void findWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			buttonFindStop.Visible = false;
			buttonFindStop.Enabled = false;
			pictureBoxFind.Visible = true;
			listBoxFindResult.Sorted = true;
		}
		//-------------------------------------------------------------------------------------
		private void buttonFindClose_Click(object sender, EventArgs e)
		{
			if(findWorker.IsBusy)
				findWorker.CancelAsync();
			splitContainer1.Panel2Collapsed = true;
			buttonFindStop.Visible = false;
			buttonFindStop.Enabled = false;
		}
		//-------------------------------------------------------------------------------------
		private void buttonFindStop_Click(object sender, EventArgs e)
		{
			if(findWorker.IsBusy)
				findWorker.CancelAsync();
			buttonFindStop.Visible = false;
			buttonFindStop.Enabled = false;
			pictureBoxFind.Visible = true;
		}
		#endregion Find Methods
		//-------------------------------------------------------------------------------------
		#region Filters Methods
		/// <summary>
		/// ���������� �������� ���������� ������� � ������������� ��� ��� ����� ����������� ���������.
		/// </summary>
		/// <param name="toolTipText">����� ����������� ���������</param>
		public void ShowFilterImage(string toolTipText)
		{
			toolTipText = "�������� ������:\r\n\r\n" + toolTipText;
		
			pictureBoxFilter.Tag = toolTipText;
			pictureBoxFilter.Visible = true;
			pictureBoxFilter.BringToFront();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// �������� �������� ���������� �������.
		/// </summary>
		public void HideFilterImage()
		{
			pictureBoxFilter.Hide();
		}
		//-------------------------------------------------------------------------------------
		private void pictureBoxFilter_MouseHover(object sender, EventArgs e)
		{
			toolTip1.SetToolTip(pictureBoxFilter, pictureBoxFilter.Tag.ToString());
		}
		#endregion Filters Methods
		//-------------------------------------------------------------------------------------
		#region << Drag&Drop Methods >>
		void treeView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			try
			{
				if(allowInternalDragDrop == false)
					return;
				if((e.Button & MouseButtons.Left) != MouseButtons.Left)
					return;
				StringFormat sf = new StringFormat();
				sf.Trimming = StringTrimming.EllipsisCharacter;

				ITreeItem item = (ITreeItem)((SimTreeNode)e.Item).TreeItem;
				if(item != SelectedNodeItem)
					SelectNodeWithItem(item);

				Bitmap bmp = null;
				Brush back = null;
				Pen border = null;
				try
				{
					back = new SolidBrush(SystemColors.Info);
					border = new Pen(SystemColors.InfoText);

					SizeF ms = this.CreateGraphics().MeasureString(item.ItemText, treeView.Font);
					if(ms.Width > this.Width - 50)
						ms.Width = this.Width - 50;
					Rectangle r = new Rectangle(0,0, (int)ms.Width + 5, (int)ms.Height + 2);
					bmp = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
					Graphics g = Graphics.FromImage(bmp);
					g.FillRectangle(back, r);
					g.DrawRectangle(border, new Rectangle(0,0,r.Width-1, r.Height-1)); //
					using(SolidBrush text = new SolidBrush(SystemColors.InfoText))
						g.DrawString(item.ItemText, treeView.Font, text, new RectangleF(2, 1, r.Width, r.Height), sf);
					//cursorCan = new Cursor(bmp.GetHicon());
					cursorCan = WinAPI.IconInfo.CreateCursorFromBitmap(bmp, 0,0);

					bmp = new Bitmap(r.Width + 7, r.Height + 7, PixelFormat.Format32bppArgb);
					g = Graphics.FromImage(bmp);
					g.FillRectangle(back, r);
					g.DrawRectangle(border, r);
					using(SolidBrush text = new SolidBrush(SystemColors.InfoText))
						g.DrawString(item.ItemText, treeView.Font, text, new RectangleF(2, 1, r.Width, r.Height), sf);
					g.DrawIcon(Properties.Resources.StopIcon, new Rectangle(r.Width - 9, r.Height - 9, 16, 16));
					//cursorCanNot = new Cursor(bmp.GetHicon());
					cursorCanNot = WinAPI.IconInfo.CreateCursorFromBitmap(bmp, 0, 0);

					this.DoDragDrop(item, DragDropEffects.Move);
					treeView.Cursor = Cursors.Default;
					if(treeView.prevDropNode != null)
					{
						treeView.BeginUpdate();
						treeView.prevDropNode.NodeFont = null;
						treeView.prevDropNode.ForeColor = Color.Empty;
						treeView.prevDropNode = null;
						treeView.EndUpdate();
					}
				}
				finally
				{
					if(back != null)
						back.Dispose();
					back = null;
					if(border != null)
						border.Dispose();
					border = null;
					if(cursorCan != null)
						cursorCan.Dispose();
					cursorCan = null;
					if(cursorCanNot != null)
						cursorCanNot.Dispose();
					cursorCanNot = null;
					if(bmp != null)
						bmp.Dispose();
					bmp = null;
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDragEnter(DragEventArgs e)
		{
			if(allowInternalDragDrop)
			{
				ITreeItem item = (ITreeItem)e.Data.GetData(typeof(ITreeItem));
				if(item != null && e.AllowedEffect == DragDropEffects.Move)
					e.Effect = e.AllowedEffect;
				else
					e.Effect = DragDropEffects.None;
			}
			else
				base.OnDragEnter(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			if(allowInternalDragDrop)
			{
				Rectangle r = treeView.RectangleToScreen(treeView.ClientRectangle); 
				if(r.Contains(Control.MousePosition))
				{
					e.UseDefaultCursors = false;
					r = new Rectangle(20,20,r.Width - 40, r.Height - 40);
					Point p = treeView.PointToClient(Control.MousePosition);
					if(p.Y < r.Y)
						WinAPI.APIWrappers.SendMessage(treeView.Handle, WinAPI.WM.VSCROLL, 0, 0);
					if(p.Y > r.Height)
						WinAPI.APIWrappers.SendMessage(treeView.Handle, WinAPI.WM.VSCROLL, 1, 0);
					if(p.X < r.X)
						WinAPI.APIWrappers.SendMessage(treeView.Handle, WinAPI.WM.HSCROLL, 0, 0);
					if(p.X > r.Width)
						WinAPI.APIWrappers.SendMessage(treeView.Handle, WinAPI.WM.HSCROLL, 1, 0);
				}
			}
			else
				base.OnGiveFeedback(e);
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnDragOver(DragEventArgs e)
		{
			try
			{
				if(allowInternalDragDrop && e.AllowedEffect == DragDropEffects.Move)
				{
					Point targetPoint = treeView.PointToClient(new Point(e.X, e.Y));
					SimTreeNode node = (SimTreeNode)treeView.GetNodeAt(targetPoint);
					if(node == null)
					{
						treeView.Cursor = cursorCanNot;
						e.Effect = DragDropEffects.None;
						return;
					}
					if(treeView.prevDropNode != null && treeView.prevDropNode != node)
					{
						treeView.BeginUpdate();
						treeView.prevDropNode.NodeFont = null;
						treeView.prevDropNode.ForeColor = Color.Empty;
						node.NodeFont = new Font(this.Font, FontStyle.Underline);
						node.ForeColor = SystemColors.HotTrack;
						treeView.EndUpdate();
					}
					treeView.prevDropNode = node;
					ITreeItem item = (ITreeItem)e.Data.GetData(tree.ItemType);
					if(!(item == null || item == node.TreeItem || node.TreeItem == item.Parent ||
											tree.GetParentsItemsList(node.TreeItem).Contains(item)))
						if(OnCanDropItem(item, node.TreeItem))
						{
							treeView.Cursor = cursorCan;
							e.Effect = e.AllowedEffect;
							return;
						}
					treeView.Cursor = cursorCanNot;
					e.Effect = DragDropEffects.None;
				}
				else
					base.OnDragOver(e);
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnDragDrop(DragEventArgs e)
		{
			try
			{
				if(allowInternalDragDrop && e.AllowedEffect == DragDropEffects.Move)
				{

					ITreeItem item = (ITreeItem)e.Data.GetData(tree.ItemType);
					if(item == null)
					{
						e.Effect = DragDropEffects.None;
						return;
					}
					OnItemDropped(item, treeView.prevDropNode.TreeItem);
				}
				else
					base.OnDragDrop(e);
					
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		#endregion << Drag&Drop Methods >>
		//-------------------------------------------------------------------------------------
		#region << Reset Methods >>
		/// <summary>
		/// ���������� ������� � ��������� ����������.
		/// </summary>
		public void Reset()
		{
			treeView.BeginUpdate();
			treeView.Nodes.Comparer = null;
			treeView.Nodes.Clear();
			if(tree != null)
			{
				tree.ObjectChanged -= tree_TreeChanged;
				tree.ItemChanged -= tree_ItemChanged;
				tree.ItemAdded -= tree_ItemAdded;
				tree.ItemDeleting -= tree_ItemRemoving;
				tree.ItemMoved -= tree_ItemMoved;
			}
			tree = null;
			//OnSelectedNodeChanged(null);
			btnFind.Enabled = false;
			pictureBoxFilter.Visible = false;
			splitContainer1.Panel2Collapsed = true;
			treeView.EndUpdate();
		}
		#endregion << Reset Methods >>
		//-------------------------------------------------------------------------------------
		#region << Override Methods >>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnter(EventArgs e)
		{
			treeView.Select();
			base.OnEnter(e);
		}
		#endregion << Override Methods >>
		//-------------------------------------------------------------------------------------


	}
	//**************************************************************************************
	#region << public class NeedImageIndexEventArgs : EventArgs >>
	/// <summary>
	/// ����� ��������� ����������� ������� NeedImageIndex.
	/// </summary>
	public class NeedImageIndexEventArgs : EventArgs
	{
		private ITreeItem item = null;
		private int imageIndex = 0;

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ���������� ������ ������, ��� �������� �������� ������������ ������ �����������.
		/// </summary>
		public ITreeItem Item
		{
			get { return item; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� � ������������� ������ �����������.
		/// </summary>
		public int ImageIndex
		{
			get { return imageIndex; }
			set { imageIndex = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="item">������ ������, ��� �������� �������� ������������ ������ �����������.</param>
		/// <param name="imageIndex">������ �����������.</param>
		public NeedImageIndexEventArgs(ITreeItem item, int imageIndex) : base ()
		{
			this.item = item;
			this.imageIndex = imageIndex;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class NeedImageIndexEventArgs : EventArgs >>
	//**************************************************************************************
	#region << internal class InternalTreeView : TreeView >>
	internal class InternalTreeView : TreeView
	{
		private IContainer components;
		internal CheckBoxesType chType = CheckBoxesType.None;
		private ImageList chImageList;
		private SimTreeNodeCollection col  = null;//   = new GoldTreeNodeCollection();
		internal bool rightClick = true;
		internal SimTreeNode prevDropNode = null;
		private List<Icon> iList = new List<Icon>();
		private TreeNode selecting = null;
		private SimTreeView treeView = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ ����������� ���������.
		/// </summary>
		[Category("Behavior"),
			Description("���������� ������ ����������� ���������.")]
		public new ImageList StateImageList
		{
			get 
			{
				if(base.StateImageList == null || base.StateImageList == chImageList)
					return null;
				return base.StateImageList;
			}
			set 
			{
				if(value == null)
					base.StateImageList = chImageList;
				else
					base.StateImageList = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������� ��������� ���������.
		/// </summary>
		//[Browsable(false)]
		[Category("Behavior")]
		[Description("���������� ��������� ��������� ������.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new SimTreeNodeCollection Nodes
		{
			get { return col; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��� ������������� ������� ��������� �������.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new SimTreeNode SelectedNode
		{
			get { return (SimTreeNode)base.SelectedNode; }
			set { base.SelectedNode = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ ��������� ������� �������.
		/// </summary>
		[Browsable(false)]
		public new SimTreeNode TopNode
		{
			get { return (SimTreeNode)base.TopNode; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public InternalTreeView(SimTreeView treeView) : base()
		{
			InitializeComponent();
			col = new SimTreeNodeCollection(base.Nodes);
			base.StateImageList = chImageList;
			this.treeView = treeView;
			base.ShowNodeToolTips = true;
		}
		//-------------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimTreeView));
			this.chImageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// chImageList
			// 
			this.chImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("chImageList.ImageStream")));
			this.chImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.chImageList.Images.SetKeyName(0, "CheckEmpty.ico");
			this.chImageList.Images.SetKeyName(1, "CheckFill.ico");
			this.chImageList.Images.SetKeyName(2, "CheckMiddle.ico");
			this.ResumeLayout(false);

		}
		//-------------------------------------------------------------------------------------
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ����� ����������������� ��������� ��� ���� ���������.
		/// </summary>
		public void ResetStates()
		{
			for (int i = 0; i < Nodes.Count; i++)
			{
				ResetNodeState(Nodes[i]);

				if(chType == CheckBoxesType.None)
					Nodes[i].StateImageIndex = -1;
				else if(chType == CheckBoxesType.ThreeState)
				{
					CheckState ii = Nodes[i].CheckState;
					foreach (SimTreeNode n in Nodes[i].Nodes)
						if (n.CheckState != ii)
						{
							ii = CheckState.Indeterminate;
							break;
						}
					Nodes[i].CheckState = ii;
					Nodes[i].StateImageIndex = (int)ii;
				} 
				else
				{
					Nodes[i].CheckState = (Nodes[i].CheckState == CheckState.Checked ? CheckState.Checked : CheckState.Unchecked);
					Nodes[i].StateImageIndex = (int)Nodes[i].CheckState;
				}
			}
		}
		/// <summary>
		/// ����������������� ��������� ��� ���������� �������� � ���� ��� �������� ���������.
		/// </summary>
		/// <param name="node"></param>
		public void ResetNodeState(SimTreeNode node)
		{
			if(chType == CheckBoxesType.None && node.StateImageIndex == -1)
				return;
		
			for (int a = 0; a < node.Nodes.Count; a++)
				ResetNodeState(node.Nodes[a]);

			if(chType == CheckBoxesType.None)
				node.StateImageIndex = -1;
			else if(chType == CheckBoxesType.ThreeState)
			{
				if(node.TreeItem.HasChildren)
				{
					CheckState ii = node.Nodes[0].CheckState;
					foreach(SimTreeNode n in node.Nodes)
						if(n.CheckState != ii)
						{
							ii = CheckState.Indeterminate;
							break;
						}
					node.CheckState = ii;
				} 
				node.StateImageIndex = (int)node.CheckState;
			}
			else
			{
				node.CheckState = (node.CheckState == CheckState.Checked ? CheckState.Checked : CheckState.Unchecked);
				node.StateImageIndex = (int)node.CheckState;
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << Overrided Methods >>
		/// <summary>
		/// ������ ��������� �������� ��� ������� ������� ����.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			TreeViewHitTestInfo ti = HitTest(e.Location);
			SimTreeNode node = (SimTreeNode)ti.Node;
			if(ti.Location != TreeViewHitTestLocations.StateImage)
			{
				base.OnMouseClick(e);
				return;
			}
			if(node.CheckState != CheckState.Checked )
			{
				if(chType == CheckBoxesType.ThreeState)
				{
					RecursSetNodesChecked(node, CheckState.Checked);
					for (node = node.Parent; node != null; node = node.Parent)
					{
						CheckState ii = CheckState.Checked;
						foreach(SimTreeNode n in node.Nodes)
							if (n.CheckState != CheckState.Checked )
							{
								ii = CheckState.Indeterminate;
								break;
							}
						node.CheckState = ii;
						node.StateImageIndex = (int)ii;
					}
				}
				else
				{
					node.CheckState = CheckState.Checked;
					node.StateImageIndex = 1;
				}   
			}
			else
			{
				if(chType == CheckBoxesType.ThreeState)
				{
					RecursSetNodesChecked(node, CheckState.Unchecked);
					for(node = node.Parent; node != null; node = node.Parent)
					{
						CheckState ii = CheckState.Unchecked;
						foreach(SimTreeNode n in node.Nodes)
							if (n.CheckState != CheckState.Unchecked)
							{
								ii = CheckState.Indeterminate;
								break;
							}
						node.CheckState = ii;
						node.StateImageIndex = (int)ii; 
					}
				}
				else
				{
					node.CheckState = CheckState.Unchecked;
					node.StateImageIndex = 0;
				}   
			}  
			base.OnMouseDown(e);
		}
		private void RecursSetNodesChecked(SimTreeNode node, CheckState check)
		{
			node.CheckState = check;
			node.StateImageIndex = (int)check;
			foreach(SimTreeNode n in node.Nodes)
				RecursSetNodesChecked(n, check );
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������ ��������� �������� ��� ������� ������� �������.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if(e.KeyChar == ' ' && SelectedNode != null && chType != CheckBoxesType.None)
			{
				SimTreeNode node = SelectedNode;
				if (node.CheckState != CheckState.Checked)
				{
					if(chType == CheckBoxesType.ThreeState)
					{
						RecursSetNodesChecked(node, CheckState.Checked);
						for (node = node.Parent; node != null; node = node.Parent)
						{
							CheckState ii = CheckState.Checked;
							foreach(SimTreeNode n in node.Nodes)
								if (n.CheckState != CheckState.Checked)
								{
									ii = CheckState.Indeterminate;
									break;
								}
							node.CheckState = ii;
							node.StateImageIndex = (int)ii;
						}
					}
					else
					{
						node.CheckState = CheckState.Checked;
						node.StateImageIndex = 1;
					}
				}
				else
				{
					if(chType == CheckBoxesType.ThreeState)
					{
						RecursSetNodesChecked(node, CheckState.Unchecked);
						for (node = node.Parent; node != null; node = node.Parent)
						{
							CheckState ii = CheckState.Unchecked;
							foreach(SimTreeNode n in node.Nodes)
								if (n.CheckState != CheckState.Unchecked)
								{
									ii = CheckState.Indeterminate;
									break;
								}
							node.CheckState = ii;
							node.StateImageIndex = (int)ii;
						}
					}
					else
					{
						node.CheckState = CheckState.Unchecked;
						node.StateImageIndex = 0;
					}
				}
			}
			base.OnKeyPress(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if(keyData == Keys.Back)
			{
				KeyPressEventArgs e = new KeyPressEventArgs((char)keyData);
				OnKeyPress(e);
				if(e.Handled)
					return true;
			}
			else if(keyData == (Keys.Shift|Keys.Add))
			{
				if(this.SelectedNode == null)
					return true;
				this.BeginUpdate();
				this.SelectedNode.ExpandAll();
				this.EndUpdate();
				return true;
			}
			else if(keyData == (Keys.Shift|Keys.Subtract))
			{
				if(this.SelectedNode == null)
					return true;
				this.BeginUpdate();
				this.SelectedNode.Collapse(false);
				this.EndUpdate();
				return true;
			}
			else if(keyData == (Keys.Control|Keys.Add))
			{
				this.BeginUpdate();
				this.ExpandAll();
				this.EndUpdate();
				return true;
			} 
			else if(keyData == (Keys.Control|Keys.Subtract))
			{
				this.BeginUpdate();
				this.CollapseAll();
				if(this.Nodes.Count >0)
						this.Nodes[0].Expand();
				this.EndUpdate();
				return true;
			}
			else if(keyData == (Keys.Control|Keys.C))
			{
				string val = null;
				if(this.SelectedNode != null)
					val = this.SelectedNode.Text;

				DataObject obj = new DataObject();
				obj.SetText(val, TextDataFormat.UnicodeText);
				obj.SetText(val, TextDataFormat.Text);
				Clipboard.SetDataObject(obj, true);
				return true;
			} 

			return base.ProcessDialogKey(keyData);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right && this.rightClick)
			{
				SimTreeNode clickedNode = this.GetNodeAt(e.X, e.Y);
				if(clickedNode != null)
					this.SelectedNode = clickedNode;
			} 
			base.OnMouseClick(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ������� � ������� �����.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public new SimTreeNode GetNodeAt(int x, int y) { return (SimTreeNode)(base.GetNodeAt(x, y)); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			selecting = e.Node;
			base.OnBeforeSelect(e);
			if(e.Cancel == true)
				selecting = null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			selecting = null;
			base.OnAfterSelect(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			base.OnBeforeExpand(e);
			if(e.Cancel == false && this.ImageList != null && ((SimTreeNode)e.Node).ExpandedImageIndex != -1)
			{
				//if(needImageIndexEventTarget == NeedImageIndexEventTarget.AllNodes)
				// e.Node.ImageIndex = OnNeedImageIndex(node, );
				//else
				// e.Node.ImageIndex = node.SelectedImageIndex = ;
				e.Node.ImageIndex = ((SimTreeNode)e.Node).ExpandedImageIndex;
				e.Node.SelectedImageIndex = ((SimTreeNode)e.Node).ExpandedImageIndex;
			} 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnAfterCollapse(TreeViewEventArgs e)
		{
			base.OnAfterCollapse(e);
			if(this.ImageList != null && ((SimTreeNode)e.Node).ExpandedImageIndex != -1)
			{
				e.Node.ImageIndex = ((SimTreeNode)e.Node).ImageIndex;
				e.Node.SelectedImageIndex = ((SimTreeNode)e.Node).ImageIndex;
			} 
		}
		//-------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if(base.HideSelection)
			{
				TreeViewHitTestInfo hti = base.HitTest(base.PointToClient(Control.MousePosition));
				if(hti.Location != TreeViewHitTestLocations.Image && hti.Location != TreeViewHitTestLocations.Label)
					OnAfterSelect(new TreeViewEventArgs(base.SelectedNode));
			} 
		}
		//-------------------------------------------------------------------------------------
		protected override void OnEnabledChanged(EventArgs e)
		{
			this.BackColor = this.Enabled ? SystemColors.Window : SystemColors.Control;
			
			base.OnEnabledChanged(e);
		}
		#endregion << Overrided Methods >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << internal class InternalTreeView : TreeView >>
	//**************************************************************************************
	#region << internal class SimTreeViewDesigner : ControlDesigner >>
	internal class SimTreeViewDesigner : ParentControlDesigner
	{
		private SimTreeView tree = null;
		private ToolStrip strip = null;
		private IDesignerHost designerHost = null;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public SimTreeViewDesigner()
			: base()
		{

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Overrides Methods >>
		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			//base.AutoResizeHandles = true;
			this.tree = component as SimTreeView;
			this.strip = this.tree.MainToolStrip;
			base.EnableDesignMode(this.tree.MainToolStrip, "MainToolStrip");
			this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// PostFilterProperties
		/// </summary>
		/// <param name="properties"></param>
		protected override void PostFilterProperties(System.Collections.IDictionary properties)
		{
			properties.Remove("Visible");
			//properties.Remove("BackgroundImage");
			//properties.Remove("BackgroundImageLayout");
			base.PostFilterProperties(properties);
		}
		#endregion << Overrides Methods >>
	}
	#endregion << internal class SimTreeViewDesigner : ControlDesigner >>

}

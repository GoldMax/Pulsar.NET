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
	/// Класс всплывающего контрола выбора элемента дерева.
	/// </summary>
	public partial class PopupTreeChoice : SimPopupControl
	{
		private SimTreeViewEx ftv;
		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripDropDownButton btnOk;
		private System.Windows.Forms.ToolStripDropDownButton btnClear;
		private System.Windows.Forms.ImageList imageList1;


		private ITree _tree = null;
		private ITreeItem _sel = null;
		private List<ITreeItem> _choisedItems = null;
			//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<PopupTreeChoice,ITreeItem> ItemChoiced >>
		[NonSerialized]
		private Pulsar.WeakEvent<PopupTreeChoice,ITreeItem> _ItemChoiced;
		/// <summary>
		/// Событие, вызываемое при подтверждении выбора элемента дерева.
		/// </summary>
		public event EventHandler<PopupTreeChoice, ITreeItem> ItemChoiced
		{
			add { _ItemChoiced += value; }
			remove { _ItemChoiced -= value; }
		}
		/// <summary>
		/// Вызывает событие ItemChoiced.
		/// </summary>
		protected virtual void OnItemChoiced(ITreeItem arg)
		{
			if(_ItemChoiced != null)
				_ItemChoiced.Raise(this, arg);
		}
		#endregion << public event EventHandler<PopupTreeChoice,ITreeItem> ItemChoiced >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет отображаемое дерево.
		/// </summary>
		public ITree Tree
		{
			get { return _tree; }
			set { _tree = value; }
		}
		/// <summary>
		/// Определяет выбранный элемент.
		/// </summary>
		public ITreeItem ChoicedItem
		{
			get { return ftv.SelectedNodeItem; }
			set { _sel = value; }
		}
		/// <summary>
		/// Определяет выбранные элементы
		/// </summary>
		public IEnumerable<ITreeItem> ChoicedItems
		{
			get
			{
				if (ftv.CheckBoxes != CheckBoxesType.None)
				{
					_choisedItems = new List<ITreeItem>();
					foreach (var node in ftv.Nodes)
						ChoicedItemsAll(node);
					return _choisedItems;
				}
				else
					return new[] {ftv.SelectedNodeItem};
			}
		}
		private void ChoicedItemsAll(SimTreeNodeEx node)
		{
			foreach (var n in node.Nodes)
			{
				if (n.CheckState != CheckState.Unchecked && n.TreeItem != null)
					_choisedItems.Add(n.TreeItem);
				ChoicedItemsAll(n);
			}
		}
		/// <summary>
		/// Позволяет выбрать тип отображения checkBox'ов
		/// </summary>
		public CheckBoxesType CheckBoxes
		{
			get { return ftv.CheckBoxes; }
			set { ftv.CheckBoxes = value; }
		}
		/// <summary>
		/// Определяет метод сравнения элементов дерева при сортировке.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Comparison<SimTreeNodeEx> Comparer
		{
			get { return ftv.Comparer; }
			set { ftv.Comparer = value; }
		}
		/// <summary>
		/// Определяет, будет ли дерево сортированным.
		/// </summary>
		[Category("SimPopupTreeChoice Properties")]
		[Description("Определяет, будет ли дерево сортированным.")]
		[DefaultValue(true)]
		[Browsable(true)]
		public bool Sorted
		{
			get { return ftv.Sorted; }
			set { ftv.Sorted = value; }
		}
		/// <summary>
		/// Определяет видимость кнопки очистки значения.
		/// </summary>
		public bool ClearButtonVisible
		{
			get { return btnClear.Visible; }
			set { btnClear.Visible = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PopupTreeChoice():base()
		{
			InitializeComponent();
			imageList1.Images.Add(Properties.Resources.FolderClosed);
			imageList1.Images.Add(Properties.Resources.FolderOpen);
			imageList1.Images.Add(Properties.Resources.Point_2);
			IsResizeble = true;

			this.Control = mainPanel;
		}
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
			 imageList1.Dispose();
			}
			base.Dispose(disposing);
		}
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainPanel = new System.Windows.Forms.Panel();
			this.ftv = new Sim.Controls.SimTreeViewEx();
			this.imageList1 = new System.Windows.Forms.ImageList();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.btnOk = new System.Windows.Forms.ToolStripDropDownButton();
			this.btnClear = new System.Windows.Forms.ToolStripDropDownButton();
			// 
			// ftv
			// 
			this.ftv.BackColor = System.Drawing.Color.Transparent;
			this.ftv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ftv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ftv.HideSelection = false;
			this.ftv.ImageList = this.imageList1;
			this.ftv.Location = new System.Drawing.Point(2, 2);
			// 
			// ftv.MainToolStrip
			// 
			this.ftv.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ftv.MainToolStrip.Name = "MainToolStrip";
			this.ftv.MainToolStrip.Size = new System.Drawing.Size(330, 25);
			this.ftv.MainToolStrip.TabIndex = 0;
			this.ftv.MainToolStrip.Text = "toolStrip1";
			this.ftv.MainToolStrip.Visible = false;
			this.ftv.Name = "ftv";
			this.ftv.NodeItemsImageIndex = 2;
			this.ftv.Size = new System.Drawing.Size(330, 386);
			this.ftv.TabIndex = 1;
			this.ftv.SelectedNodeChanged += new Sim.Controls.SimTreeViewEx.SelectedNodeChangedHandler(this.ftv_SelectedNodeChanged);
			this.ftv.AfterCheck += new SimTreeViewEx.AfterCheckHandler(ftv_AfterCheck);
			this.ftv.NodeDoubleClick += new SimTreeViewEx.SelectedNodeChangedHandler(ftv_NodeDoubleClick);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// statusStrip1
			// 
			this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
												this.btnOk, this.btnClear});
			this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.statusStrip1.Location = new System.Drawing.Point(2, 388);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(330, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// btnOk
			// 
			this.btnOk.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnOk.Enabled = false;
			this.btnOk.Image = global::Sim.Controls.Properties.Resources.OK;
			this.btnOk.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOk.Name = "btnOk";
			this.btnOk.ShowDropDownArrow = false;
			this.btnOk.Size = new System.Drawing.Size(74, 20);
			this.btnOk.Text = "Выбрать";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnClear
			// 
			this.btnClear.Visible = false;
			this.btnClear.Image = global::Sim.Controls.Properties.Resources.Cancel;
			this.btnClear.Name = "btnClear";
			this.btnClear.ShowDropDownArrow = false;
			this.btnClear.Text = "Очистить";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// mainPanel
			// 
			mainPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			mainPanel.BackColor = Color.Transparent;
			mainPanel.Controls.Add(this.ftv);
			mainPanel.Controls.Add(this.statusStrip1);
			mainPanel.BackColor = System.Drawing.Color.Transparent;
			mainPanel.Padding = new System.Windows.Forms.Padding(2);
			mainPanel.Size = new System.Drawing.Size(334, 312);
			mainPanel.MinimumSize = new System.Drawing.Size(200, 200);
			// 
			// PopupTreeChoice
			// 
			this.Name = "PopupTreeChoice";
		}

		#endregion
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void ftv_AfterCheck(object sender, ITreeItem item)
		{
			btnOk.Enabled = item != null;
		}
		private void ftv_SelectedNodeChanged(object sender, ITreeItem item)
		{
			btnOk.Enabled = item != null && item.IsGroup == false;
		}
		void ftv_NodeDoubleClick(object sender, ITreeItem item)
		{
		 if(item != null && item.IsGroup == false)
			 btnOk_Click(btnOk, EventArgs.Empty);
		}
		private void btnOk_Click(object sender, EventArgs e)
		{
			this.Hide();
			OnItemChoiced(ftv.SelectedNodeItem);
		}
		private void btnClear_Click(object sender, EventArgs e)
		{
			this.Hide();
			OnItemChoiced(null);
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		protected override void OnOpening(CancelEventArgs e)
		{
			base.OnOpening(e);

			if(ftv.Tree != _tree)
				ftv.Tree = _tree;
			if (_sel != null)
				ftv.SelectedNodeItem = _sel;
			ftv.Select();
			ftv.Focus();
		}
		///// <summary>
		///// 
		///// </summary>
		///// <param name="screenPoint"></param>
		//public override void Show(Point screenPoint)
		//{
		//  Show(screenPoint.X, screenPoint.Y);
		//}
		///// <summary>
		///// Отображает контрол.
		///// </summary>
		///// <param name="x"></param>
		///// <param name="y"></param>
		//public override void Show(int x, int y)
		//{
		//  //this.Control = mainPanel;

		
		//}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		
	}
}

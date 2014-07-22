using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Pulsar;
using Pulsar.Reflection;

namespace Sim.Controls
{
	/// <summary>
	/// Класс контрола DataGridView + TreeView
	/// </summary>
	[DefaultProperty("MainPropertyName")]
	[DefaultEvent("NeedChildren")]
	public class SimTreeGridView : SimDataGridView
	{
		internal View view = null;
		private ITree tree = null;
		private string mainPropertyName = "";
		private PropertyDescriptor pd = null;
		private int indent = 14;
		private bool drawLines = true;
		private bool drawPlusMinus = true;
		private Comparison<ITreeItem> comparer = null;
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		#region public class SimTreeGridItemEventArgs : EventArgs
		/// <summary>
		/// Класс аргумента события HasChildren
		/// </summary>
		public class SimTreeGridItemEventArgs : EventArgs
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			#region << Properties >>
			/// <summary>
			/// Дерево
			/// </summary>
			public ITree Tree { get; set; }
			/// <summary>
			/// Элемент дерева, для которого определяется наличие дочерних элементов.
			/// </summary>
			public ITreeItem Item { get; set; }
			/// <summary>
			/// Определяет, имеются ли дочерние элементы.
			/// </summary>
			public bool HasChildren { get; set; }
			#endregion << Properties >>
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			public SimTreeGridItemEventArgs()
				: base()
			{

			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			/// <param name="hasChildren">Определяет, имеются ли дочерние элементы.</param>
			/// <param name="item">Элемент дерева, для которого определяется наличие дочерних элементов.</param>
			/// <param name="tree">Дерево</param>
			public SimTreeGridItemEventArgs(ITree tree, ITreeItem item, bool hasChildren)
				: this()
			{
				Item = item;
				HasChildren = hasChildren;
				Tree = tree;
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
		}
		#endregion public class SimTreeGridItemEventArgs : EventArgs
		#region public delegate void SimTreeGridItemHandler
		/// <summary>
		/// Делегат для событий элементов SimTreeGrid
		/// </summary>
		/// <param name="sender">Объект, генерирующий событие.</param>
		/// <param name="args">Аргументы события</param>
		public delegate void SimTreeGridItemHandler(object sender, SimTreeGridItemEventArgs args);
		#endregion public delegate void SimTreeGridItemHandler
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event SimTreeGridItemHandler HasChildren >>
		/// <summary>
		/// Событие, возникающее при необходимости определения наличия дочерних элементов.
		/// </summary>
		[Description("Событие, возникающее при необходимости определения наличия дочерних элементов.")]
		[Category(" Own Events")]
		public new event SimTreeGridItemHandler HasChildren;
		/// <summary>
		/// Вызывает событие HasChildren
		/// </summary>
		/// <param name="item">Элемент дерева, для которого вызывается событие.</param>
		/// <returns></returns>
		protected internal bool OnHasChildren(ITreeItem item)
		{
			if(HasChildren != null)
			{
				SimTreeGridItemEventArgs args = new SimTreeGridItemEventArgs(tree, item, false);
				HasChildren(this, args);
				return args.HasChildren;
			}
			else
				return false;
		}
		#endregion << public event SimTreeGridItemHandler HasChildren >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event SimTreeGridItemHandler NeedChildren >>
		/// <summary>
		/// Событие, возникающее при необходимости дополнить элемент дочерними элементами.
		/// </summary>
		[Description("Событие, возникающее при необходимости дополнить элемент дочерними элементами.")]
		[Category(" Own Events")]
		public event SimTreeGridItemHandler NeedChildren;
		/// <summary>
		/// Вызывает событие HasChildren
		/// </summary>
		/// <param name="item">Элемент дерева, для которого вызывается событие.</param>
		/// <returns></returns>
		protected internal bool OnNeedChildren(ITreeItem item)
		{
			if(NeedChildren != null)
			{
				SimTreeGridItemEventArgs args = new SimTreeGridItemEventArgs(tree, item, true);
				NeedChildren(this, args);
				if(args.HasChildren == false)
					return false;
				//AddChildren(item);
				return args.HasChildren;
			}
			else
				return false;
		}
		#endregion << public event SimTreeGridItemHandler NeedChildren >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<SimTreeGridView, ITreeItem> ItemCollapsing >>
		/// <summary>
		/// Событие, возникающее при закрытии ветви.
		/// </summary>
		public event EventHandler<SimTreeGridView, ITreeItem> ItemCollapsing;
		/// <summary>
		/// Вызывает событие ItemCollapsing
		/// </summary>
		/// <param name="item"></param>
		protected void OnItemCollapsing(ITreeItem item)
		{
			if(ItemCollapsing != null)
				ItemCollapsing(this, item);
		}
		#endregion << public event EventHandler<SimTreeGridView, ITreeItem> ItemCollapsing >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<SimTreeGridView, ITreeItem> ItemCollapsed >>
		/// <summary>
		/// Событие, возникающее после закрытия ветви.
		/// </summary>
		public event EventHandler<SimTreeGridView, ITreeItem> ItemCollapsed;
		/// <summary>
		/// Вызывает событие ItemCollapsed
		/// </summary>
		/// <param name="item"></param>
		protected void OnItemCollapsed(ITreeItem item)
		{
			if(ItemCollapsed != null)
				ItemCollapsed(this, item);
		}
		#endregion << public event EventHandler<SimTreeGridView, ITreeItem> ItemCollapsed >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<SimTreeGridView, ITreeItem> ItemExpanding >>
		/// <summary>
		/// Событие, возникающее при открытии ветви.
		/// </summary>
		public event EventHandler<SimTreeGridView, ITreeItem> ItemExpanding;
		/// <summary>
		/// Вызывает событие ItemExpanding
		/// </summary>
		/// <param name="item"></param>
		protected void OnItemExpanding(ITreeItem item)
		{
			if(ItemExpanding != null)
				ItemExpanding(this, item);
		}
		#endregion << public event EventHandler<SimTreeGridView, ITreeItem> ItemExpanding >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<SimTreeGridView, ITreeItem> ItemExpanded >>
		/// <summary>
		/// Событие, возникающее после открытия ветви.
		/// </summary>
		public event EventHandler<SimTreeGridView, ITreeItem> ItemExpanded;
		/// <summary>
		/// Вызывает событие ItemExpanded
		/// </summary>
		/// <param name="item"></param>
		protected void OnItemExpanded(ITreeItem item)
		{
			if(ItemExpanded != null)
				ItemExpanded(this, item);
		}
		#endregion << public event EventHandler<SimTreeGridView, ITreeItem> ItemExpanded >>
		#endregion << Events >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		#region << Override Properties >>
		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new object DataSource
		{
			get { return null; }
			set { throw new NotImplementedException(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string DataMember
		{
			get { return ""; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 
		/// </summary>
		//[DefaultValue(DataGridViewAutoSizeColumnsMode.Fill)]
		//[Category("Layout")]

		//public new DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
		//{
		//    get { return base.AutoSizeColumnsMode; }
		//    set { base.AutoSizeColumnsMode = value; }
		//}

		#endregion << Override Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Own Properties >>
		/// <summary>
		/// Дерево данных
		/// </summary>
		[Category(" Own Properties")]
		[Description("Дерево данных.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ITree Tree
		{
			get { return tree; }
			set
			{
				if(tree != null)	//	value == null && 
				{
					tree.ItemAdded -= (tree_ItemAdded);
					tree.ItemDeleting -= (tree_ItemRemoved);
					tree.ItemChanged -= (tree_ItemChanged);
					tree.ObjectChanged -= tree_ListChanged;
					//tree.ObjectChanging -= (tree_ObjectChanging);
				}
				if(view != null)
				 view.Clear();
				tree = value;
				if(tree == null)
				{
					base.DataSource = null;
					view = null;
					return;
				}

				if(String.IsNullOrWhiteSpace(MainPropertyName))
					throw new Exception("Не определено значение свойства MainPropertyName!");
				pd = TypeDescriptor.GetProperties(tree.ObjectType)[MainPropertyName];
				if(pd == null)
					throw new PulsarException("Тип [{0}] не содержит свойство {1}, определенное как основное.",
						tree.ObjectType.FullName, MainPropertyName);

				tree.ItemAdded += (tree_ItemAdded);
				tree.ItemDeleting += (tree_ItemRemoved);
				tree.ItemChanged += (tree_ItemChanged);
				tree.ObjectChanged += tree_ListChanged;
				//tree.ObjectChanging += (tree_ObjectChanging);

				SimTreeGridViewMainColumn col = null;
				foreach(DataGridViewColumn c in this.Columns)
					if(c is SimTreeGridViewMainColumn)
					{
						col = (SimTreeGridViewMainColumn)c;
						col.ReadOnly = true;
						break;
					}
					else if(c.DataPropertyName == mainPropertyName)
					{
						col = new SimTreeGridViewMainColumn();
						col.AutoSizeMode = c.AutoSizeMode;
						col.CellTemplate = c.CellTemplate;
						col.ContextMenuStrip = c.ContextMenuStrip;
						col.DefaultCellStyle = c.DefaultCellStyle;
						col.DefaultHeaderCellType = c.DefaultHeaderCellType;
						col.DividerWidth = c.DividerWidth;
						col.FillWeight = c.FillWeight;
						col.Frozen = c.Frozen;
						col.HeaderText = c.HeaderText;
						col.MinimumWidth = c.MinimumWidth;
						col.Name = c.Name;
						col.ReadOnly = true;
						col.Resizable = c.Resizable;
						col.SortMode = c.SortMode;
						col.Tag = c.Tag;
						col.ToolTipText = c.ToolTipText;
						col.ValueType = c.ValueType;
						col.Visible = true;
						col.Width = c.Width;
						col.DataPropertyName = MainPropertyName;
						int index = c.Index;
						base.Columns.Remove(c);
						base.Columns.Insert(index, col);
						break;
					}

				if(col == null)
				{
					col = new SimTreeGridViewMainColumn();
					col.DataPropertyName = MainPropertyName;
					col.HeaderText = pd.DisplayName;
					col.ReadOnly = true;
					col.Visible = true;
					base.Columns.Insert(0, col);
				}

				InitTree();				

			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет имя свойства объекта, которое будет основным в навигации.
		/// </summary>
		[Category(" Own Properties")]
		[Description("Определяет имя свойства объекта, которое будет основным в навигации.")]
		[DefaultValue("")]
		public string MainPropertyName
		{
			get { return mainPropertyName; }
			set { mainPropertyName = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет единичный отступ вложенных элементов дерева.
		/// </summary>
		[Category(" Own Properties")]
		[Description("Определяет единичный отступ вложенных элементов дерева.")]
		[DefaultValue(14)]
		public int Indent
		{
			get { return indent; }
			set { indent = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будут ли рисоваться линии дерева.
		/// </summary>
		[Category(" Own Properties")]
		[Description("Определяет, будут ли рисоваться линии дерева.")]
		[DefaultValue(true)]
		public bool DrawTreeLines
		{
			get { return drawLines; }
			set
			{
				drawLines = value;
				Refresh();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будут ли рисоваться кнопки узлов.
		/// </summary>
		[Category(" Own Properties")]
		[Description("Определяет, будут ли рисоваться кнопки узлов.")]
		[DefaultValue(true)]
		public bool DrawPlusMinus
		{
			get { return drawPlusMinus; }
			set
			{
				drawPlusMinus = value;
				Refresh();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будет ли дерево достраиватся в процессе раскрытия узлов. 
		/// Если true, будут вызываться события HasChildren и NeedChildren для дополнения дерева.
		/// Если false, контрол будет реагировать на событие ItemAdded дерева.
		/// </summary>
		[Category(" Own Properties")]
		[Description("Определяет, будет ли дерево достраиватся в процессе раскрытия узлов.")]
		[DefaultValue(false)]
		public bool IsIncrementalTree { get; set; }
		// <summary>
		/// Определяет метод сравнения элементов дерева при сортировке.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Comparison<ITreeItem> Comparer
		{
			get { return comparer; }
			set
			{
				comparer = value;
				if(view != null)
					view.Comparer = comparer;
			}
		}
		/// <summary>
		/// Определяет метод сравнения элементов дерева по умолчанию при сортировке.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Comparison<ITreeItem> DefaultComparer
		{
			get { return view.DefaultComparer; }
		}

		#endregion << Own Properties >>
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimTreeGridView()
			: base()
		{
			InitializeComponent();
		}
		private void InitializeComponent()
		{
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// SimTreeGridView
			// 
			this.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
			this.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.MultiSelect = false;
			this.RowHeadersVisible = false;
			this.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(191)))), ((int)(((byte)(211)))));
			this.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			this.RowTemplate.Height = 19;
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}
		/// <summary>
		/// Dispose
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if(tree != null)
			{
				tree.ItemAdded -= tree_ItemAdded;
				tree.ItemDeleting -= tree_ItemRemoved;
				tree.ItemChanged -= tree_ItemChanged;
				tree.ObjectChanged -= tree_ListChanged;
			}
			base.Dispose(disposing);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << SimTree Handlers >>
		void tree_ItemChanged(object sender, CollectionChangeNotifyEventArgs args)
		{
			ITreeItem item = (ITreeItem)args.Item;
			if(view.Contains(item))
			{
				if(!this.tree.AllowMultiObject)
				{
					view.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, view.IndexOf(item)));
					Invalidate();
				}
				else
				{
					if(item.Object != null)
					{
						int[] indexAllObject = this.tree.IndexOfAll(item.Object);
						for(int i = 0; i < indexAllObject.Length; i++)
						{
							view.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged,
											view.IndexOf((ITreeItem)tree.GetItemWithIndex(indexAllObject[i]))));
							Invalidate();
						}
					}
				}
			}

		}
		//void tree_ObjectChanging(object sender, ObjectChangeNotifyEventArgs e)
		//{
		// //if(e.Action == ChangeNotifyAction.ObjectReset && view.RaiseEvents)
		// // EventsOff();
		//}
		void tree_ListChanged(object sender, ObjectChangeNotifyEventArgs e)
		{
			if(e.Action == ChangeNotifyAction.ObjectReset)
				InitTree();
		}
		void tree_ItemRemoved(object sender, CollectionChangeNotifyEventArgs args)
		{
			ITreeItem item = (ITreeItem)args.Item;
			if(view.Contains(item))
			{
				HideChildren(item);
				view.Remove(item);
			}
			if(item.Parent != null && (item.Parent.HasChildren == false
																|| (item.Parent.Children.Count() == 1 && item.Parent.Children.Contains(item))))
			{
				if(view.Contains(item.Parent))
				{
					view[item.Parent].hasButton = false;
					view[item.Parent].btnClose = true;
					view[item.Parent].btnRect = new Rectangle();
				}
			}
			Invalidate();
		}
		void tree_ItemAdded(object sender, CollectionChangeNotifyEventArgs args)
		{
			// ?????
			if(IsIncrementalTree == false)
				view.Add((ITreeItem)args.Item);
			Invalidate();
		}
		#endregion << SimTree Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		private void InitTree()
		{
			try
			{
			 bool was = false;
				if(view == null)
				{
				 view = new View(this);
					view.Comparer = comparer;
				}
				else
				{
				 was = view.RaiseEvents;
				 view.RaiseEvents = false;
					view.Clear();
				}

				foreach(ITreeItem i in tree.GetRootItems())
					view.Add(i);
				if(view.IsSorted)
				 ((IBindingListView)view).ApplySort(((IBindingListView)view).SortDescriptions);

				if(base.DataSource == null)
				 base.DataSource = view;
			 else
				{
					view.RaiseEvents = was;
				 if(view._listChanged != null)
				 	view._listChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
				} 
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		private void HideChildren(ITreeItem item)
		{
			bool was = view.RaiseEvents;
			if(was)
				EventsOff(false);
			
			int pos = view.IndexOf(item);
			if(pos == -1)
				throw new Exception(" pos == -1");
			uint level = item.Level;
			int count = 0;

			for(int a = pos+1; a < view.list.Count && view.ItemAt(a).Level > level; a++)
			 count++;

			if(count != 0)
			 view.RemoveAt(pos+1, count);

			if(was)
				EventsOn(true);
		}
		private void AddChildren(ITreeItem item)
		{
			try
			{
				int pos = view.IndexOf(item);
				if(pos == -1)
				 throw new Exception(" pos == -1");
				List<ITreeItem> l = new List<ITreeItem>((IEnumerable<ITreeItem>)item.Children);

				bool was = l.Count > 50 && view.RaiseEvents;
				if(was)
					EventsOff(false);

				if(view.IsSorted)
					l.Sort(view.Sorter);

				Dictionary<ITreeItem,NodeInfo> res = null;
				if(l.Count > 50)
					res = new Dictionary<ITreeItem, NodeInfo>(l.Count);

				for(int i = 0; i < l.Count; i++)
				{
					ITreeItem it = l[i];

					NodeInfo ci = new NodeInfo();
					ci.hasButton = it.HasChildren;
					if(ci.hasButton == false)
						ci.hasButton = OnHasChildren(it);
					ci.IsEndItem = i == l.Count - 1;

					if(res == null)
					{
						view.Insert(++pos, it, ci);
						if(view.RaiseEvents)
							view.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, pos));
					}
					else
						res.Add(it, ci);
				}

				if(res != null)
					view.Insert(pos+1, res);

				if(was)
					EventsOn(true);

			}
			catch
			{
				
				throw;
			}
			
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Раскрывает узел соответствующего элемента.
		/// </summary>
		/// <param name="item">Элемент раскрываемого узла.</param>
		/// <param name="isExpandChilds">Указывает на необходимость раскрывать дочерние узлы</param>
		public void Expand(ITreeItem item, bool isExpandChilds = false)
		{
			if(tree.Contains(item) == false)
				throw new Exception("Дерево не содержит указанный элемент!");
			if(view.Contains(item) == false)
				return;
			NodeInfo ni = view[item];
			if(ni.btnClose == true)
			{
				if(item.HasChildren == false)
					if(this.OnNeedChildren(item) == false)
					{
						ni.hasButton = false;
						return;
					}
				OnItemExpanding(item);
				ni.btnClose = false;
				this.AddChildren(item);
				if(isExpandChilds)
					foreach(ITreeItem iChild in item.Children)
						Expand(iChild, isExpandChilds);
				OnItemExpanded(item);
			}
			else
				if(isExpandChilds)
					foreach(ITreeItem iChild in item.Children)
						Expand(iChild, isExpandChilds);
		}
		/// <summary>
		/// Раскрывает все узлы.
		/// </summary>
		public void ExpandAll()
		{
			foreach(ITreeItem i in tree.GetRootItems())
				Expand(i, true);
		}
		/// <summary>
		/// Определяет раскрыт ли узел дерева.
		/// </summary>
		/// <param name="item">Определяемый элемент.</param>
		/// <returns></returns>
		public bool IsExpanded(ITreeItem item)
		{
			if(tree.Contains(item) == false)
				throw new Exception("Дерево не содержит указанный элемент!");
			if(view.Contains(item) == false)
				return false;
			NodeInfo ni = view[item];
			if(ni.hasButton == false)
				return false;
			return !ni.btnClose;
		}
		/// <summary>
		/// Закрывает узел соответствующего элемента.
		/// </summary>
		/// <param name="item">Элемент закрываемого узла.</param>
		public void Collapse(ITreeItem item)
		{
			if(tree.Contains(item) == false)
				throw new Exception("Дерево не содержит указанный элемент!");
			if(view.Contains(item) == false)
				return;
			NodeInfo ni = view[item];
			if(ni.btnClose == false)
			{
				OnItemCollapsing(item);
				ni.btnClose = true;
				HideChildren(item);
				OnItemCollapsed(item);
			}
		}
		/// <summary>
		/// Закрывает все узлы.
		/// </summary>
		public void CollapseAll()
		{
			foreach(ITreeItem i in tree.GetRootItems())
				Collapse(i);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, имеет ли узел кнопку раскрытия/скрытия.
		/// </summary>
		/// <param name="item">пределяемый элемент.</param>
		/// <returns></returns>
		public bool HasButton(ITreeItem item)
		{
			if(tree.Contains(item) == false)
				throw new Exception("Дерево не содержит указанный элемент!");
			if(view.Contains(item) == false)
				return false;
			NodeInfo ni = view[item];
			return ni.hasButton;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if(keyData == Keys.Space)
			{
				if(this.SelectedRows.Count == 0)
					return base.ProcessDialogKey(keyData);
				ITreeItem i = this.GetItemAt(this.SelectedRows[0].Index);
				if(HasButton(i) == false)
					return base.ProcessDialogKey(keyData);
				if(IsExpanded(i))
					Collapse(i);
				else
					Expand(i);
			}
			return base.ProcessDialogKey(keyData);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает узел дерева по индексу строки.
		/// </summary>
		/// <param name="index">Индекс строки.</param>
		/// <returns>Узел дерева</returns>
		public ITreeItem GetItemAt(int index)
		{
			return view.ItemAt(index);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает индекс строки узла дерева
		/// </summary>
		/// <param name="item">Узел дерева</param>
		/// <returns>Индекс строки</returns>
		public int IndexOf(ITreeItem item)
		{
			return view.IndexOf(item);
		}
		//-------------------------------------------------------------------------------------
		public void EventsOff(bool raiseResetting = true)
		{
			if(raiseResetting && view._listChanged != null)
				view._listChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
			view.RaiseEvents = false;
		}
		public void EventsOn(bool raiseResetting = true)
		{
			view.RaiseEvents = true;
			if(raiseResetting && view._listChanged != null)
				view._listChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------

		//*************************************************************************************
		internal class View : ITypedList, IList, IBindingList, IBindingListView
		{
			internal HashList<ITreeItem> list = new HashList<ITreeItem>();
			Dictionary<ITreeItem, NodeInfo> infos = new Dictionary<ITreeItem, NodeInfo>();
			SimTreeGridView treeView = null;
			Comparison<ITreeItem> comparer = null;
			internal bool RaiseEvents = true;

			private List<ListSortDescription> sort = new List<ListSortDescription>();
			//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			#region << Events >>
			internal ListChangedEventHandler _listChanged;
			/// <summary>
			/// Событие, возникающее при изменении элементов списка.
			/// </summary>
			public event ListChangedEventHandler ListChanged
			{
				add { _listChanged += value; }
				remove { _listChanged -= value; }
			}
			/// <summary>
			/// Вызывает событие ListChanged.
			/// </summary>
			/// <param name="e"></param>
			public void OnListChanged(ListChangedEventArgs e)
			{
				if(_listChanged != null && RaiseEvents)
					_listChanged(this, e);
			}
			#endregion << Events >>
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			#region << Properties >>
			/// <summary>
			/// CellInfo this[ITreeItem item]
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			public NodeInfo this[ITreeItem item]
			{
				get { return infos[item]; }
				set { infos[item] = value; }
			}
			/// <summary>
			/// Определяет метод сравнения элементов дерева при сортировке.
			/// </summary>
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public Comparison<ITreeItem> Comparer
			{
				get { return comparer; }
				set { comparer = value; }
			}
			#endregion << Properties >>
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			private View() : base() { }
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			/// <param name="treeGridView"></param>
			public View(SimTreeGridView treeGridView)
				: this()
			{
				treeView = treeGridView;
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
			#region ITypedList Members
			PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
			{
				Type t = treeView.tree.ObjectType;
				PropertyDescriptorCollection col = TypeDescriptor.GetProperties(t);

				if(t.IsPrimitive || t == typeof(string))
				{
					PropertyDescriptor[] arr = new PropertyDescriptor[col.Count];
					col.CopyTo(arr, 0);
					col = new PropertyDescriptorCollection(arr, false);
					col.Add(new PrimitiveValuePropertyDescriptor(t));
				}
				return col;
			}
			//-------------------------------------------------------------------------------------
			string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
			{
				throw new NotImplementedException();
			}
			#endregion
			//-------------------------------------------------------------------------------------
			#region IList Members
			/// <summary>
			/// Add(ITreeItem item)
			/// </summary>
			public void Add(ITreeItem item, NodeInfo ci = null)
			{
				try
				{
					bool endItem = true;
					int index = -1;
					List<ITreeItem> items = null;

					if(list.Count == 0)
						index = 0;
					else
					{
						if(item.Parent == null)
							items = new List<ITreeItem>((IEnumerable<ITreeItem>)treeView.tree.GetRootItems());
						else
						{
							if(infos.ContainsKey(item.Parent) == false)
								return;
							if(infos[item.Parent].hasButton == false)
								infos[item.Parent].hasButton = true;
							// Добавление элемента в Закрытую ветвь блокируется
							if(infos[item.Parent].btnClose)
								return;
							// Это правильно, так как при этом не неужна сортировака ***
							items = new List<ITreeItem>();
							foreach(var i in list)
								if(item.Parent.Children.Contains(i))
									items.Add(i);
						}
						for(int a = 0; a < items.Count; a++)
							if(infos.ContainsKey(items[a]) == false)
							{
								items.RemoveAt(a);
								a--;
							}
						if(IsSorted)
						{
							//items.Sort(Sorter);   // ***
							foreach(ITreeItem i in items)
								if(Sorter(i, item) > 0)
								{
									index = list.IndexOf(i);
									endItem = false;
									break;
								}
						}
					}
					if(index == -1)
						if(items == null)
							return;
						else
							index = FindPos(item, items);

					list.Insert(index, item);
					//items.Insert(index, item);

					ci = new NodeInfo();
					ci.hasButton = item.HasChildren;
					if(ci.hasButton == false)
						ci.hasButton = treeView.OnHasChildren(item);
					ci.IsEndItem = endItem; // index >= (items == null ? 0 : items.Count -1);
					infos.Add(item, ci);

					if(item.Parent != null && infos[item.Parent].hasButton == false)
					{
						infos[item.Parent].hasButton = true;
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, list.IndexOf(item.Parent)));
					}

					if(ci.IsEndItem && items != null)
						foreach(ITreeItem i in items)
						{
							bool end = infos[i].IsEndItem;
							infos[i].IsEndItem = false;
							if(end)
								OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, list.IndexOf(i)));
						}
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
				}
				catch(Exception Err)
				{
					ErrorBox.Show(Err);
				}
			}
			int IList.Add(object value)
			{
				Add((ITreeItem)value);
				return IndexOf((ITreeItem)value);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// IndexOf(ITreeItem item)
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			public int IndexOf(ITreeItem item)
			{
				return list.IndexOf(item);
			}
			int IList.IndexOf(object value)
			{
				return IndexOf((ITreeItem)value);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Insert
			/// </summary>
			internal void Insert(int index, ITreeItem item, NodeInfo ni)
			{
				list.Insert(index, item);
				infos.Add(item, ni);
			}
			void IList.Insert(int index, object value)
			{
				throw new NotImplementedException("Insert");
			}
			internal void Insert(int pos, 	Dictionary<ITreeItem,NodeInfo> res)
			{
				list.Insert(pos, res.Keys);
				foreach(var kv in res)
				 infos.Add(kv.Key, kv.Value);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Contains(ITreeItem item)
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			public bool Contains(ITreeItem item)
			{
				return infos.ContainsKey(item);
			}
			bool IList.Contains(object value)
			{
				return Contains((ITreeItem)value);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Remove(ITreeItem item)
			/// </summary>
			/// <param name="item"></param>
			public void Remove(ITreeItem item)
			{
				int pos = list.IndexOf(item);
				list.Remove(item);
				infos.Remove(item);
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, pos));
				if(item.Parent != null)
				{
					List<ITreeItem> items =
																					new List<ITreeItem>((IEnumerable<ITreeItem>)item.Parent.Children);
					for(int a = 0; a < items.Count; a++)
						if(infos.ContainsKey(items[a]) == false)
						{
							items.RemoveAt(a);
							a--;
						}
					if(IsSorted)
						items.Sort(Sorter);
					foreach(ITreeItem i in items)
					{
						bool end = infos[i].IsEndItem;
						infos[i].IsEndItem = items.IndexOf(i) == items.Count - 1;
						if(end != infos[i].IsEndItem)
							OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, list.IndexOf(i)));
					}
				}
			}
			void IList.Remove(object value)
			{
				Remove((ITreeItem)value);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// RemoveAt(int index)
			/// </summary>
			/// <param name="index"></param>
			public void RemoveAt(int index)
			{
				infos.Remove(list[index]);
				list.RemoveAt(index);
			}
			public void RemoveAt(int pos, int count)
			{
			 for(int a = 0; a < count; a++)
			 	infos.Remove(list[pos+a]);
				list.RemoveAt(pos,count);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Clear()
			/// </summary>
			public void Clear()
			{
				list.Clear();
				infos.Clear();
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
			}
			//-------------------------------------------------------------------------------------
			bool IList.IsReadOnly
			{
				get { return true; }
			}
			//-------------------------------------------------------------------------------------
			bool IList.IsFixedSize
			{
				get { return false; }
			}
			//-------------------------------------------------------------------------------------
			object IList.this[int index]
			{
				get { return list[index].Object; }
				set { throw new NotImplementedException(); }
			}
			#endregion
			//-------------------------------------------------------------------------------------
			#region ICollection Members
			void ICollection.CopyTo(Array array, int index)
			{
				list.CopyTo((ITreeItem[])array, index);
			}
			//-------------------------------------------------------------------------------------
			public int Count
			{
				get { return RaiseEvents ? list.Count : 0; }
			}
			//-------------------------------------------------------------------------------------
			bool ICollection.IsSynchronized
			{
				get { throw new NotImplementedException(); }
			}
			//-------------------------------------------------------------------------------------
			object ICollection.SyncRoot
			{
				get { throw new NotImplementedException(); }
			}
			#endregion
			//-------------------------------------------------------------------------------------
			#region IEnumerable Members
			IEnumerator IEnumerable.GetEnumerator()
			{
				return list.GetEnumerator();
			}
			#endregion
			//-------------------------------------------------------------------------------------
			#region IBindingList Members
			void IBindingList.AddIndex(PropertyDescriptor property)
			{
				throw new NotImplementedException();
			}
			//-------------------------------------------------------------------------------------
			object IBindingList.AddNew()
			{
				throw new NotImplementedException();
			}
			//-------------------------------------------------------------------------------------
			bool IBindingList.AllowEdit
			{
				get { return false; }
			}
			//-------------------------------------------------------------------------------------
			bool IBindingList.AllowNew
			{
				get { return false; }
			}
			//-------------------------------------------------------------------------------------
			bool IBindingList.AllowRemove
			{
				get { return false; }
			}
			//-------------------------------------------------------------------------------------
			void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
			{
				ListSortDescription lsd = new ListSortDescription(property, direction);
				int pos = sort.FindIndex(delegate(ListSortDescription l)
				{
					return l.PropertyDescriptor.Name == lsd.PropertyDescriptor.Name;
				});
				if(pos == -1)
				{
					sort.Clear();
					sort.Add(lsd);
				}
				else
					sort[pos] = lsd;

				Sort();
			}
			//-------------------------------------------------------------------------------------
			public int Find(PropertyDescriptor property, object key)
			{
				for(int a = 0; a < list.Count; a++)
					if(list[a].Object != null)
						if(property.GetValue(list[a]).Equals(key))
							return a;
				return -1;
			}
			//-------------------------------------------------------------------------------------
			public bool IsSorted
			{
				get { return sort.Count > 0; }
			}
			//-------------------------------------------------------------------------------------
			void IBindingList.RemoveIndex(PropertyDescriptor property)
			{
				throw new NotImplementedException("RemoveIndex");
			}
			//-------------------------------------------------------------------------------------
			void IBindingList.RemoveSort()
			{
				sort.Clear(); ;
			}
			//-------------------------------------------------------------------------------------
			ListSortDirection IBindingList.SortDirection
			{
				get { return sort.Count == 0 ? ListSortDirection.Ascending : sort[0].SortDirection; }
			}
			//-------------------------------------------------------------------------------------
			PropertyDescriptor IBindingList.SortProperty
			{
				get { return sort.Count == 0 ? null : sort[0].PropertyDescriptor; }
			}
			//-------------------------------------------------------------------------------------
			bool IBindingList.SupportsChangeNotification
			{
				get { return true; }
			}
			//-------------------------------------------------------------------------------------
			bool IBindingList.SupportsSearching
			{
				get { return true; }
			}
			//-------------------------------------------------------------------------------------
			bool IBindingList.SupportsSorting
			{
				get { return true; }
			}
			#endregion
			//-------------------------------------------------------------------------------------
			#region IBindingListView Members
			void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
			{
				sort.Clear();
				foreach(ListSortDescription lsd in sorts)
				{
					if(lsd == null)
						continue;
					sort.Add(lsd);
				}
				Sort();
			}
			ListSortDescriptionCollection IBindingListView.SortDescriptions
			{
				get { return new ListSortDescriptionCollection(sort.ToArray()); }
			}
			string IBindingListView.Filter
			{
				get { return null; }
				set { throw new NotImplementedException(); }
			}
			void IBindingListView.RemoveFilter()
			{
				throw new NotImplementedException();
			}
			bool IBindingListView.SupportsAdvancedSorting
			{
				get { return true; }
			}
			bool IBindingListView.SupportsFiltering
			{
				get { return false; }
			}
			#endregion
			//-------------------------------------------------------------------------------------
			#region << Methods >>
			private void ResetLines()
			{
				Dictionary<ITreeItem, byte> opened = new Dictionary<ITreeItem, byte>();
				foreach(KeyValuePair<ITreeItem, NodeInfo> ni in infos)
					if(ni.Value.hasButton && ni.Value.btnClose == false)
						opened.Add(ni.Key, 0);
				infos.Clear();

				//foreach(ITreeItem i in tree)
				//{
				// NodeInfo ci = new NodeInfo();
				// ci.hasButton = ((ITreeItem)i.Object).HasChildren;
				// if(ci.hasButton == false)
				//  ci.hasButton = treeView.OnHasChildren((ITreeItem)i.Object);
				// if(i.Parent != null)
				//  ci.IsEndItem = i.Parent.Children.Last() == i;

				// if(opened.ContainsKey((ITreeItem)i.Object))
				//  ci.btnClose = false;
				// infos.Add((ITreeItem)i.Object, ci);
				//}
				//uint prev = 0;
				//for(int a = list.Count-1; a>= 0 ; a--)


				for(int a = 0; a < list.Count; a++)
				{
				 ITreeItem i = list[a];
					NodeInfo ci = new NodeInfo();
					ci.hasButton = i.HasChildren;
					if(ci.hasButton == false)
						ci.hasButton = treeView.OnHasChildren(i);
					if(i.Parent != null)
						ci.IsEndItem = a +1 >= list.Count || list[a+1].Level < i.Level; // i.Parent.Children.IndexOf(i) == i.Parent.Children.Count()-1;
					//ci.IsEndItem = prev < i.Level;
					//prev = i.Level;

					if(opened.ContainsKey(i))
						ci.btnClose = false;
					infos.Add(i, ci);
				}
		}
			//-------------------------------------------------------------------------------------
			private int FindPos(ITreeItem item, List<ITreeItem> items)
			{
				if(item.Parent == null)
					return list.Count;
				if(items.Count == 0)
					return list.IndexOf(item.Parent) + 1;
				return list.IndexOf(items[items.Count - 1]) + 1;
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public ITreeItem ItemAt(int index)
			{
				if(index < 0 || index > list.Count)
					throw new ArgumentException(String.Format("Индекс {{{0}}} вне допустимого диапазона!", index), "index");
				return list[index];
			}
			//-------------------------------------------------------------------------------------
			internal void Sort()
			{
				//list.Sort(Sorter);
				//Tree<object> tree = new Tree<object>();
				//bool has = true;
				//int a = 0;
				//while(has)
				//{
				// has = false;
				// foreach(ITreeItem i in list)
				//  if(i.Level == a)
				//  {
				//   tree.Add((object)i, (object)i.Parent);
				//   has = true;
				//  }
				// a++;
				//}
				//list.Clear();
				//foreach(TreeItem<object> i in tree.GetRootItems())
				//{
				// list.Add((ITreeItem)i.Object);
				// foreach(TreeItem<object> ch in tree.GetAllChildren(i))
				//  list.Add((ITreeItem)ch.Object);
				//}
				list.Sort(Sorter);
				//ResetLines(tree);
				ResetLines();
			}
			internal int Sorter(ITreeItem i1, ITreeItem i2)
			{
				if(comparer != null)
					return comparer(i1, i2);
				else
				{
					int res =  DefaultComparer(i1, i2);
					return res;
				}
			}

			internal int DefaultComparer(ITreeItem i1, ITreeItem i2)
			{
				//try
				//{
				uint lmax = Math.Max(i1.Level, i2.Level);
				ITreeItem[] p1 = new ITreeItem[lmax+1];
				ITreeItem[] p2 = new ITreeItem[lmax+1];

				for(int a = (int)i1.Level; a >= 0; a--)
				{
					p1[a] = i1;
					i1 = i1.Parent;
				}
				for(int a = (int)i2.Level; a >= 0; a--)
				{
					p2[a] = i2;
					i2 = i2.Parent;
				}
				

				for(int a = 0; a < lmax+1; a++)
				{
					i1 = p1[a];
					i2 = p2[a];

					if(Object.Equals(i1, i2))
						continue;
					if(Object.Equals(i1, null))
						return -1;
					if(Object.Equals(i2, null))
						return 1;

					object o1, o2;
					foreach(ListSortDescription lsd in sort)
					{
						o1 = i1.Object == null ? i1.ItemText : lsd.PropertyDescriptor.GetValue(i1.Object);
						o2 = i2.Object == null ? i2.ItemText : lsd.PropertyDescriptor.GetValue(i2.Object);

						int res = 0;
						if(o1 == null && o2 != null)
							res = 1;
						else if(o2 == null && o1 != null)
							res = -1;
						else if(Object.Equals(o1, o2))
							res = 0; //i1.Level.CompareTo(i2.Level);
						else if(o1.GetType() == o2.GetType() && o1 is IComparable)
							res = ((IComparable)o1).CompareTo(o2);
						else
							res = String.Compare(o1.ToString(), o2.ToString());
						//if(res == 0)
						// res = i1.GetHashCode().CompareTo(i2.GetHashCode());
						if(res != 0)
							return res * (lsd.SortDirection == ListSortDirection.Ascending ? 1 : -1);
					}
					return String.Compare(i1.ItemText, i2.ItemText);
				}
				//}
				//catch
				//{

				// throw;
				//}
				return 0;
			}

			//internal int DefaultComparer(ITreeItem i1, ITreeItem i2)
			//{
			// //try
			// //{
			// uint lmax = Math.Max(i1.Level, i2.Level);
			// ITreeItem[] p1 = new ITreeItem[lmax+1];
			// ITreeItem[] p2 = new ITreeItem[lmax+1];

			// for(int a = (int)i1.Level; a >= 0; a--)
			// {
			//  p1[a] = i1;
			//  i1 = i1.Parent;
			// }
			// for(int a = (int)i2.Level; a >= 0; a--)
			// {
			//  p2[a] = i2;
			//  i2 = i2.Parent;
			// }
			// uint x = 0;
			// for(; x < lmax+1 && p1[x] == p2[x]; x++) ;
			// i1 = p1[x < lmax+1 ? x : lmax];
			// i2 = p2[x < lmax+1 ? x : lmax];

			// if(Object.Equals(i1, i2))
			//  return 0;
			// if(Object.Equals(i1, null))
			//  return -1;
			// if(Object.Equals(i2, null))
			//  return 1;

			// object o1, o2;
			// foreach(ListSortDescription lsd in sort)
			// {
			//  o1 = i1.Object == null ? i1.ItemText : lsd.PropertyDescriptor.GetValue(i1.Object);
			//  o2 = i2.Object == null ? i2.ItemText : lsd.PropertyDescriptor.GetValue(i2.Object);

			//  int res = 0;
			//  if(o1 == null && o2 != null)
			//   res = 1;
			//  else if(o2 == null && o1 != null)
			//   res = -1;
			//  else if(Object.Equals(o1, o2))
			//   res = 0; //i1.Level.CompareTo(i2.Level);
			//  else if(o1.GetType() == o2.GetType() && o1 is IComparable)
			//   res = ((IComparable)o1).CompareTo(o2);
			//  else
			//   res = String.Compare(o1.ToString(), o2.ToString());
			//  //if(res == 0)
			//  // res = i1.GetHashCode().CompareTo(i2.GetHashCode());
			//  if(res != 0)
			//   return res * (lsd.SortDirection == ListSortDirection.Ascending ? 1 : -1);
			// }
			// //}
			// //catch
			// //{

			// // throw;
			// //}
			// return 0;
			//}

			#endregion << Methods >>
			//-------------------------------------------------------------------------------------



		}
		//*************************************************************************************
		internal class NodeInfo
		{
			internal bool hasButton = false;
			internal Rectangle btnRect = Rectangle.Empty;
			internal bool btnClose = true;
			internal bool IsEndItem = false;
		}
	}
	//**************************************************************************************
}
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
	/// Контрол горизонтального отображения древовидной структуры данных.
	/// </summary>
	[DefaultEvent("CurrentItemChanged")]
	public partial class SimTreeBar : UserControl
	{
		private ITree tree = null;
		private bool showRootButton = true;
		private IComparer<ITreeItem> comparer = null;

		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		/// <summary>
		/// Событие, возникающее при изменении текущего элемента основного представления.
		/// </summary>
		[Description("Событие, возникающее при изменении текущего элемента основного представления.")]
		public event EventHandler<object, ITreeItem> CurrentItemChanged;
		/// <summary>
		/// Метод, вызывающий событие CurrentItemChanged.
		/// </summary>
		/// <param name="item"></param>
		protected void OnCurrentItemChanged(ITreeItem item)
		{
			if(CurrentItemChanged != null)
				CurrentItemChanged(this, item);
		}
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		/// <summary>
		/// Класс аргумента события NeedItemMenu
		/// </summary>
		public class NeedItemMenuEventArgs : EventArgs
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			#region << Properties >>
			/// <summary>
			/// Элемент дерева.
			/// </summary>
			public ITreeItem Item { get; set; }
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Меню
			/// </summary>
			public ContextMenuStrip Menu { get; set; }
			#endregion << Properties >>
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			public NeedItemMenuEventArgs() : base()
			{

			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			public NeedItemMenuEventArgs(ITreeItem item) : base()
			{
				Item = item;
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
						
		}
		/// <summary>
		/// Событие, возникающее при нажатии на кнопку элемента.
		/// </summary>
		public event EventHandler<NeedItemMenuEventArgs> NeedItemMenu;
		/// <summary>
		/// Вызывает событие NeedItemMenu
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual ContextMenuStrip OnNeedItemMenu(ITreeItem item)
		{
			if(NeedItemMenu == null)
				return null;
			NeedItemMenuEventArgs args = new NeedItemMenuEventArgs(item);
			NeedItemMenu(this,args);
			return args.Menu;
		}
		#endregion << Events >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает и устанавливает текущий элемент дерева.
		/// Событие CurrentItemChanged не генерируется.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITreeItem CurrentItem
		{
			get
			{
				if(toolStrip1.Items.Count == 0)
					return null;
				return (ITreeItem)toolStrip1.Items[toolStrip1.Items.Count-1].Tag;
			}
			set
			{
				ResetQueue();
				if(tree.Contains(value) == false)
					throw new PulsarException("Элемент [{0}] не принадлежит дереву!", value);
				foreach(ITreeItem i in tree.GetParentsItemsList(value))
					AddButton(i);
				AddButton(value);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает и устанавливает текущий элемент дерева по значению его объекта.
		/// Событие CurrentItemChanged не генерируется.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object CurrentObject
		{
			get
			{
				if(toolStrip1.Items.Count == 0)
					return null;
				return ((ITreeItem)toolStrip1.Items[toolStrip1.Items.Count-1].Tag).Object;
			}
			set
			{
				ResetQueue();
				ITreeItem item = tree[value];
				if(item == null)
					throw new PulsarException("Элемент для объекта [{0}] не принадлежит дереву!", value);
				foreach(ITreeItem i in tree.GetParentsItemsList(item))
					AddButton(i);
				AddButton(item);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает или устанавливет ссылку на объект SimLinkedListViewManager основного представления.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITree Tree
		{
			get { return tree; }
			set
			{
				tree = value;
				if(value != null)
					ResetQueue();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет отображение корневой кнопки
		/// </summary>
		[DefaultValue(true)]
		[Description("Определяет отображение корневой кнопки")]
		public bool ShowRootButton
		{
			get { return showRootButton; }
			set 
			{ 
				showRootButton = value; 
				if(showRootButton)
				{
					if(rootQueueButton.Owner == null)
					toolStrip1.Items.Insert(0, rootQueueButton);
				}
				else if(rootQueueButton.Owner != null)
						toolStrip1.Items.Remove(rootQueueButton);
			}
		}
		public override Color BackColor
		{
			get	{ return base.BackColor; }
			set 
			{ 
				base.BackColor = value;
				finistPanel1.BackColor = BackColor; //ControlPaint.Light(SystemColors.Control, 0.7f);
				toolStrip1.BackColor = BackColor;
			}
		}
		/// <summary>
		/// Определяет метод сравнения элементов дерева при сортировке.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IComparer<ITreeItem> Comparer
		{
			get { return comparer; }
			set
			{
				comparer = value;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public SimTreeBar()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Dock = DockStyle.Top;
			finistPanel1.BackColor = BackColor; //ControlPaint.Light(SystemColors.Control, 0.7f);
			toolStrip1.BackColor = BackColor;

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Events Handlers >>
		private void toolStripSplitButton_Paint(object sender, PaintEventArgs e)
		{
			ToolStripSplitButton btn = (ToolStripSplitButton)sender;
			Graphics g = e.Graphics;

			bool btnOver = false;
			Rectangle r = Rectangle.Empty;

			if(btn.DropDown.Visible)
			{
				r = e.ClipRectangle;
				r.Width --;
				using(SolidBrush b = new SolidBrush(ProfessionalColors.ButtonSelectedHighlight))
					g.FillRectangle(b, r); // SystemBrushes.Info
				using(Pen pen = new Pen(ProfessionalColors.ButtonSelectedBorder))
					g.DrawRectangle(pen, r);
			} 
			else
			{
				using(SolidBrush b = 
					new SolidBrush(Enabled ? BackColor : SystemColors.Control))
					g.FillRectangle(b, e.ClipRectangle);

				Point p = toolStrip1.PointToClient(Control.MousePosition);
				if(btn.Bounds.Contains(p))
				{
					p.X -= btn.Bounds.X;
					p.Y -= btn.Bounds.Y;
					if(btn.ButtonBounds.Contains(p))
					{
						r = btn.ButtonBounds;
						btnOver = true;
					}
					else //if(btn.DropDownButtonBounds.Contains(p))
					{
						r =  e.ClipRectangle;
						r.Width --;
						//r = btn.DropDownButtonBounds;
					
						//if(r.X == 0)
						// r.Width--;
						//else
						//{
						// r.X -= 2;
						// r.Width ++;
						//}
					}
					if(r.IsEmpty == false)
					{
						r.Height -=2;
						using (SolidBrush b = new SolidBrush(ProfessionalColors.ButtonSelectedHighlight))
							g.FillRectangle(b , r); // SystemBrushes.Info
						using(Pen pen = new Pen(ProfessionalColors.ButtonSelectedBorder))
							g.DrawRectangle(pen, r);
					}
				}
			}
			r = ((ToolStripSplitButton)btn).ButtonBounds;

			Font fnt = new Font(toolStrip1.Font, FontStyle.Regular);//, FontStyle.Bold);
			SizeF s = g.MeasureString(btn.Text, toolStrip1.Font);
			PointF pf = new PointF();
			pf.X = (r.Width - s.Width) / 2;
			pf.Y = (r.Height - s.Height) / 2;
			using (var b = new SolidBrush(Enabled ? toolStrip1.ForeColor : SystemColors.GrayText))
				g.DrawString(btn.Text, fnt, b, pf);

			if(btn.DropDownButtonWidth > 0)
			{
				r = ((ToolStripSplitButton)btn).DropDownButtonBounds;
				pf.X = r.X + (r.Width - Properties.Resources.ArrowRight.Width) / 2;
				pf.Y = r.Y + (r.Height - Properties.Resources.ArrowRight.Height) / 2;

				if(btnOver == false && (btn.Selected || btn.Pressed))
					g.DrawImage(Properties.Resources.ArrowDown, (int)pf.X - (pf.X == 0 ? 1 : 2), (int)pf.Y);
				else
					g.DrawImage(Properties.Resources.ArrowRight, (int)pf.X, (int)pf.Y);
			}
		}
		//-------------------------------------------------------------------------------------
		private void toolStripSplitButton_ButtonClick(object sender, EventArgs e)
		{
			ToolStripItem btn = (ToolStripItem)sender;
			ContextMenuStrip menu = OnNeedItemMenu((ITreeItem)btn.Tag);
			if(menu == null)
				return;
			Rectangle r = toolStrip1.RectangleToScreen(btn.Bounds);
			//menu.Owner = btn;
			menu.Show(r.X, r.Y + r.Height);
		}
		//-------------------------------------------------------------------------------------
		void btn_DropDownOpening(object sender, EventArgs e)
		{
			try
			{
				ToolStripSplitButton btn = (ToolStripSplitButton)sender;
								
				if(btn.DropDown == null || btn.DropDown is ToolStripDropDownMenu)
					if(btn.Tag == null)
					{
						ContextMenuStrip menu = new ContextMenuStrip();
						menu.ShowImageMargin = false;
						menu.ShowCheckMargin = false;
						menu.ItemClicked += new ToolStripItemClickedEventHandler(toolStripSplitButton_DropDownItemClicked);

						IEnumerable<IGraphItem> roots = comparer == null
							? tree.GetRootItems()
							: tree.GetRootItems().OrderBy(x => x, (IComparer<IGraphItem>)comparer);

						foreach(ITreeItem i in roots)
						{
							ToolStripMenuItem mi = new ToolStripMenuItem();
							mi.Text = i.ItemText;
							mi.Tag = i;
							if(i.HasChildren)
								mi.DropDown = CreateTreeMenu(i);
							menu.Items.Add(mi);
						}
						btn.DropDown =  menu;
					}
					else
						btn.DropDown = CreateTreeMenu((ITreeItem)btn.Tag);
				if(btn.DropDown != null)
					btn.DropDown.OwnerItem = btn;
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void toolStripSplitButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			List<ITreeItem> sel = new List<ITreeItem>();
			ToolStripItem mi = (ToolStripItem)e.ClickedItem;
			ContextMenuStrip menu = null;
			do
			{
				sel.Add((ITreeItem)mi.Tag);
				if(mi.Owner is ContextMenuStrip)
					menu = (ContextMenuStrip)mi.Owner;
				mi = mi.OwnerItem;
			} while (mi != null && mi is ToolStripSplitButton == false);
			int pos = toolStrip1.Items.IndexOf(mi);
			while(toolStrip1.Items.Count-1 > pos)
				toolStrip1.Items.RemoveAt(pos+1);
			sel.Reverse();
			foreach(ITreeItem i in sel)
				AddButton(i);
			if(menu == null)
				((ContextMenuStrip)sender).Hide();
			else
				menu.Hide();
			OnCurrentItemChanged(sel.Count == 0 ? (ITreeItem)mi.Tag : sel[sel.Count-1]);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			finistPanel1.BackColor = Enabled ? ControlPaint.Light(SystemColors.Control, 0.7f) : SystemColors.Control;
			base.OnEnabledChanged(e);
		}
		#endregion << Events Handlers >>
		//-------------------------------------------------------------------------------------
		#region << QueueButtons Methods >>
		/// <summary>
		/// Очищает очередь кнопок и создает корневую кнопку.
		/// </summary>
		public void ResetQueue()
		{
			toolStrip1.Items.Clear();
			if(showRootButton)
				toolStrip1.Items.Add(rootQueueButton);
		}
		/// <summary>
		/// Добавляет кнопку в конец очереди.
		/// </summary>
		/// <param name="item">Элемент дерева.</param>
		private void AddButton(ITreeItem item)
		{
			ToolStripSplitButton btn = new ToolStripSplitButton();
			btn.DisplayStyle = ToolStripItemDisplayStyle.Text;
			btn.AutoToolTip = false;
			btn.Tag = item;
			btn.Text = item.ItemText;
			btn.Paint += new PaintEventHandler(toolStripSplitButton_Paint);
			btn.ButtonClick += new EventHandler(toolStripSplitButton_ButtonClick);
			btn.MouseMove += (s,e) => ((ToolStripItem)s).Invalidate();

			if(item.HasChildren)
			{
				btn.DropDownButtonWidth = 13;
				btn.DropDownOpening += new EventHandler(btn_DropDownOpening);
			}
			else
				btn.DropDownButtonWidth = 0;
			toolStrip1.Items.Add(btn);
		}
		//-------------------------------------------------------------------------------------
		private ContextMenuStrip CreateTreeMenu(ITreeItem item)
		{
			if(item.HasChildren == false)
				return null;
			ContextMenuStrip menu = new ContextMenuStrip();
			menu.ShowImageMargin = false;
			menu.ShowCheckMargin = false;
			menu.ItemClicked += new ToolStripItemClickedEventHandler(toolStripSplitButton_DropDownItemClicked);

			IEnumerable<ITreeItem> children = comparer == null
				? item.Children
				: item.Children.OrderBy(x => x, comparer);

			foreach(ITreeItem i in children)
			{
				ToolStripMenuItem mi = new ToolStripMenuItem();
				mi.Text = i.ItemText;
				mi.Tag = i;
				if(i.HasChildren)
					mi.DropDown = CreateTreeMenu(i);
				menu.Items.Add(mi);
			}
			return menu;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает текущий элемент дерева.
		/// Событие CurrentItemChanged генерируется.
		/// </summary>
		/// <param name="item"></param>
		public void SetCurrentItem(ITreeItem item)
		{
			if(item == null)
			 throw new ArgumentNullException("item");
			if(tree.IndexOf(item) == -1)
				throw new PulsarException("Элемент для объекта [{0}] не принадлежит дереву!", item);
			ResetQueue();
			foreach(ITreeItem i in tree.GetParentsItemsList(item))
				AddButton(i);
			AddButton(item);
			OnCurrentItemChanged(item);
		}
		#endregion << QueueButtons Methods >>
	}
}

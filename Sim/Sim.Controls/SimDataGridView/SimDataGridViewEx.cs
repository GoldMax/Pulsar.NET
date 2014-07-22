using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Sim.Controls
{
	/// <summary>
	/// Класс контрола SimDataGridView с боковой панелью.
	/// </summary>
	[Designer(typeof(SimDataGridViewExDesigner))]
	public partial class SimDataGridViewEx : UserControl, IDisposable
	{
		private object selRow = null;
		private bool isInit = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает SimDataGridView 
		/// </summary>
		[Category("Appearance")]
		[Description("Возвращает SimDataGridView")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimDataGridView Grid
		{
			get { return fdgv; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет источник данных таблицы.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue((string)null)]
		[Category("Data")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[Description("Определяет источник данных таблицы.")]
		public object DataSource
		{
			get { return fdgv.DataSource; }
			set 
			{
				isInit = true; 
				fdgv.DataSource = value; 
				isInit = false;
				RebuildDetail();
				fdgv_SelectionChanged(this, EventArgs.Empty);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет отображение панели детализации.
		/// </summary>
		[DefaultValue(true)]
		[Category("Behavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("Определяет отображение панели детализации.")]
		public bool ShowDetailPanel
		{
			get { return !splitContainer1.Panel2Collapsed; }
			set { splitContainer1.Panel2Collapsed = !value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridViewEx() : base()
		{
			InitializeComponent();
			fdgv.ColumnStateChanged += new DataGridViewColumnStateChangedEventHandler(fdgv_ColumnStateChanged);
			fdgv.CurrentCellChanged += new EventHandler(fdgv_SelectionChanged);
			fdgv.ColumnNameChanged += new DataGridViewColumnEventHandler(fdgv_ColumnNameChanged);
			fdgv.ColumnHeaderCellChanged += new DataGridViewColumnEventHandler(fdgv_ColumnHeaderCellChanged);
			fdgv.PreviewMouseDown += new SimDataGridView.SimPreviewMouseDown(fdgv_PreviewMouseDown);
			fdgv.ColumnRemoved += new DataGridViewColumnEventHandler(fdgv_ColumnRemoved);
			fdgv.ColumnAdded += new DataGridViewColumnEventHandler(fdgv_ColumnAdded);
		}
		//-------------------------------------------------------------------------------------
		void IDisposable.Dispose()
		{
			fdgv.ColumnStateChanged -= new DataGridViewColumnStateChangedEventHandler(fdgv_ColumnStateChanged);
			fdgv.CurrentCellChanged -= new EventHandler(fdgv_SelectionChanged);
			fdgv.ColumnNameChanged -= new DataGridViewColumnEventHandler(fdgv_ColumnNameChanged);
			fdgv.ColumnHeaderCellChanged -= new DataGridViewColumnEventHandler(fdgv_ColumnHeaderCellChanged);
			fdgv.PreviewMouseDown -= new SimDataGridView.SimPreviewMouseDown(fdgv_PreviewMouseDown);
			fdgv.ColumnRemoved -= new DataGridViewColumnEventHandler(fdgv_ColumnRemoved);
			fdgv.ColumnAdded += new DataGridViewColumnEventHandler(fdgv_ColumnAdded);

			base.Dispose();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers >>
		void fdgv_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
		{
			try
			{
				if(isInit)
					return;
				if(e.StateChanged == DataGridViewElementStates.Visible)
					if(e.Column.Visible == false)
					{
						DetailColumnViewControl c = new DetailColumnViewControl();
						c.Name = e.Column.Name;
						c.Caption = e.Column.HeaderText;
						c.ButtonHidePressed += new EventHandler(detailColumnViewControl_ButtonHidePressed);
						panelDetail.Controls.Add(c);
						c.Dock = DockStyle.Top;
						c.BringToFront();
						if(fdgv.CurrentRow != null)
							if(e.Column.ValueType == typeof(bool))
								c.Text = ((bool)fdgv.CurrentRow.Cells[e.Column.Index].Value ? "Да" : "Нет");
							else if(typeof(Image).IsAssignableFrom(e.Column.ValueType))
								c.Text = fdgv.CurrentRow.Cells[e.Column.Index].Value.ToString();
							else
								c.Text = fdgv.CurrentRow.Cells[e.Column.Index].FormattedValue.ToString();
					}
					else if(panelDetail.Controls.ContainsKey(e.Column.Name))
						panelDetail.Controls.RemoveByKey(e.Column.Name);
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void detailColumnViewControl_ButtonHidePressed(object sender, EventArgs e)
		{
			try
			{
				fdgv.Columns[((Control)sender).Name].Visible = true;
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void fdgv_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				if(isInit)
					return;
				if(fdgv.CurrentRow == null)
				{
					foreach(Control c in panelDetail.Controls)
						((DetailColumnViewControl)c).Text = "";
					selRow = null; 
					return; 
				}
				if(fdgv.CurrentRow.GetData().Equals(selRow) == false)
				{
					foreach(DataGridViewColumn col in fdgv.Columns)
						if(col.Visible == false && panelDetail.Controls.IndexOfKey(col.Name) != -1)
							if(col.ValueType == typeof(bool))
								panelDetail.Controls[col.Name].Text = 
									((bool)fdgv.CurrentRow.Cells[col.Index].Value ? "Да" : "Нет");
							else if(typeof(Image).IsAssignableFrom(col.ValueType))
								panelDetail.Controls[col.Name].Text = fdgv.CurrentRow.Cells[col.Index].Value.ToString(); 
							else
								panelDetail.Controls[col.Name].Text = fdgv.CurrentRow.Cells[col.Index].FormattedValue.ToString();
					selRow = fdgv.CurrentRow.GetData(); 
				} 
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void fdgv_ColumnNameChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if(isInit)
				return;
			RebuildDetail();
		}
		//-------------------------------------------------------------------------------------
		void fdgv_ColumnHeaderCellChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if(isInit)
				return;
			((DetailColumnViewControl)panelDetail.Controls[e.Column.Index]).Caption = e.Column.HeaderText;
		}
		//-------------------------------------------------------------------------------------
		void fdgv_PreviewMouseDown(object sender, SimMouseEventArgs args)
		{
			try
			{
				if(args.Button != MouseButtons.Right)
					return;
				DataGridView.HitTestInfo hti = fdgv.HitTest(args.X, args.Y);
				if(hti.Type == DataGridViewHitTestType.ColumnHeader)
				{
					menuItemFrozen.Checked = fdgv.Columns[hti.ColumnIndex].Frozen;
					finistContextMenu1.Tag = fdgv.Columns[hti.ColumnIndex];
					finistContextMenu1.Show(fdgv, args.Location);     
					args.Handled = true;
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			} 
		}
		//-------------------------------------------------------------------------------------
		void fdgv_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
		{
			//try
			//{
			// if(isInit)
			//  return;
			// int pos = panelDetail.Controls.IndexOfKey(e.Column.Name);
			// if(pos != -1)
			//  panelDetail.Controls.RemoveAt(pos);

			//}
			//catch(Exception Err)
			//{
			// ErrorBox.Show(Err);
			//}
		}
		//-------------------------------------------------------------------------------------
		void fdgv_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
		{
			//try
			//{
			// if(isInit)
			//  return;
			// if(e.Column.Visible == false)
			// {
			//  DetailColumnViewControl c = new DetailColumnViewControl();
			//  c.Name = e.Column.Name;
			//  c.Caption = e.Column.HeaderText;
			//  c.ButtonHidePressed += new EventHandler(detailColumnViewControl_ButtonHidePressed);
			//  panelDetail.Controls.Add(c);
			//  c.Dock = DockStyle.Top;
			//  c.BringToFront();
			//  if(fdgv.CurrentRow != null)
			//   if(e.Column.ValueType == typeof(bool))
			//    c.Text = ((bool)fdgv.CurrentRow.Cells[e.Column.Index].Value ? "Да" : "Нет");
			//   else if(typeof(Image).IsAssignableFrom(e.Column.ValueType))
			//    c.Text = fdgv.CurrentRow.Cells[e.Column.Index].Value.ToString();
			//   else
			//    c.Text = fdgv.CurrentRow.Cells[e.Column.Index].FormattedValue.ToString();
			// }
			// else if(panelDetail.Controls.ContainsKey(e.Column.Name))
			//  panelDetail.Controls.RemoveByKey(e.Column.Name);
			//}
			//catch(Exception Err)
			//{
			// ErrorBox.Show(Err);
			//}
		}
		//-------------------------------------------------------------------------------------
		private void finistContextMenu1_Opening(object sender, CancelEventArgs e)
		{
			if(splitContainer1.Panel2Collapsed)
			{
				toolStripSeparator1.Visible = true;
				menuItemShowDetail.Visible = true;
			}
			else
			{
				toolStripSeparator1.Visible = false;
				menuItemShowDetail.Visible = false;
			}
		}
		//-------------------------------------------------------------------------------------
		private void HeaderMenuItems_Click(object sender, EventArgs e)
		{
			try
			{
				ToolStripMenuItem item = (ToolStripMenuItem)sender;
				switch(item.Name)
				{
					case "menuItemToDetail" :
						((DataGridViewColumn)item.Owner.Tag).Visible = false;
						if(this.ShowDetailPanel == false)
							ShowDetailPanel = true;
						break;
					case "menuItemShowDetail":
						ShowDetailPanel = true;
						break;
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void finistPopupButton1_Click(object sender, EventArgs e)
		{
			ShowDetailPanel = false;
		}
		//-------------------------------------------------------------------------------------
		void menuItemFrozen_Click(object sender, System.EventArgs e)
		{
			DataGridViewColumn col = (DataGridViewColumn)finistContextMenu1.Tag;
			col.Frozen = menuItemFrozen.Checked;
		}
		#endregion << Controls Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Public Methods >>
		/// <summary>
		/// Добавляет столбец в описание. 
		/// </summary>
		/// <param name="column">Столбец, добавляемый в описание.</param>
		public void SetColumnAsDetail(DataGridViewColumn column)
		{
			//fdgv.Columns[column].Visible = false;
			column.Visible = false;
			//RebuildDetail();
		}
		/// <summary>
		/// Добавляет столбец в описание.
		/// </summary>
		/// <param name="columnName">Имя столбца, добавляемого в описание.</param>
		public void SetColumnAsDetail(string columnName)
		{
			SetColumnAsDetail(fdgv.Columns[columnName]);
		}
		/// <summary>
		/// Добавляет столбец в описание.
		/// </summary>
		/// <param name="index">Индекс столбца, добавляемого в описание.</param>
		public void SetColumnAsDetail(int index)
		{
			SetColumnAsDetail(fdgv.Columns[index]);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обновляет сетку(Grid) и панель детализации.
		/// </summary>
		public override void Refresh()
		{
			try
			{
				fdgv.Refresh();
				RebuildDetail();
				foreach(DataGridViewColumn col in fdgv.Columns)
					if(col.Visible == false)
						if(fdgv.CurrentRow == null)
							panelDetail.Controls[col.Name].Text = "";
						else
						{
							if(col.ValueType == typeof(bool))
								panelDetail.Controls[col.Name].Text =
									((bool)fdgv.CurrentRow.Cells[col.Index].Value ? "Да" : "Нет");
							else if(typeof(Image).IsAssignableFrom(col.ValueType))
								panelDetail.Controls[col.Name].Text = fdgv.CurrentRow.Cells[col.Index].Value.ToString();
							else
								panelDetail.Controls[col.Name].Text = fdgv.CurrentRow.Cells[col.Index].FormattedValue.ToString();
						}  
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Перестраивает панель детализации.
		/// </summary>
		private void RebuildDetail()
		{
			try
			{
				foreach(DataGridViewColumn col in fdgv.Columns)
					if(col.Visible == false)
					{
						if(panelDetail.Controls.IndexOfKey(col.Name) != -1)
							continue;
						DetailColumnViewControl c = new DetailColumnViewControl();
						c.Name = col.Name;
						c.Caption = col.HeaderText;
						panelDetail.Controls.Add(c);
						c.Dock = DockStyle.Top;
						c.BringToFront();
					}
					else
					{
						if(panelDetail.Controls.IndexOfKey(col.Name) != -1)
							panelDetail.Controls.RemoveByKey(col.Name);
					}
				List<Control> toDel = new List<Control>();
				foreach(Control c in panelDetail.Controls)
					if(fdgv.Columns.Contains(c.Name) == false)
					toDel.Add(c);
				foreach(Control c in toDel)
					panelDetail.Controls.Remove(c); 
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		#endregion << Public Methods >>
		//-------------------------------------------------------------------------------------
	}
	//**************************************************************************************
	#region << internal class SimDataGridViewExDesigner : ControlDesigner >>
	internal class SimDataGridViewExDesigner : ParentControlDesigner
	{
		private SimDataGridViewEx ex = null;
		private SimDataGridView grid = null;
		private IDesignerHost designerHost = null;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridViewExDesigner() : base()
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
			this.ex = component as SimDataGridViewEx;
			this.grid = this.ex.Grid;
			base.EnableDesignMode(this.ex.Grid, "Grid");
			this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// PostFilterProperties
		/// </summary>
		/// <param name="properties"></param>
		protected override void PostFilterProperties(System.Collections.IDictionary properties)
		{
			properties.Remove("BorderStyle");
			properties.Remove("BackgroundImage");
			properties.Remove("BackgroundImageLayout");
			base.PostFilterProperties(properties);
		}
		#endregion << Overrides Methods >>    
	}
	#endregion << internal class SimDataGridViewExDesigner : ControlDesigner >>

}

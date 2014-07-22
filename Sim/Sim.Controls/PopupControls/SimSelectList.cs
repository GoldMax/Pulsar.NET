using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Pulsar;

namespace Sim.Controls
{
	/// <summary>
	/// Класс всплывающего окна выбора элемента.
	/// </summary>
	public class SimSelectList : SimPopupControl
	{
		private int rowHeight = 16;
		private SimDataGridView fdgv = new SimDataGridView();
		private object sel = null;
		private int maxItems = 25;
		private Timer searchTimer;
		private string _searchString;
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler<SimSelectList, object> ItemSelected; >>
		/// <summary>
		/// Событие, возникающее при выборе элемента в списке.
		/// </summary>
		public event EventHandler<SimSelectList, object> ItemSelected;
		/// <summary>
		/// Вызывает событие ItemSelected.
		/// </summary>
		protected virtual void OnItemSelected()
		{
			if(ItemSelected != null)
				ItemSelected(this, sel);
		} 
		#endregion << public event EventHandler<SimSelectList, object> ItemSelected; >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Binder отображаемого списока.
		/// </summary>
		public ListBinder Binder
		{
			get { return (ListBinder)fdgv.DataSource; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает выбранный объект
		/// </summary>
		public object SelectedItem
		{
			get { return sel; }
			set { sel = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Width
		/// </summary>
		public new int Width
		{
			get { return base.Width; }
			set { fdgv.Parent.Width = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Макисмальное число отображаемых элементов.
		/// </summary>
		public int MaxItems
		{
			get { return maxItems; }
			set { maxItems = value; }
		}
		
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		protected SimSelectList()
			: base()
		{
			using (Graphics g = CreateGraphics())
				rowHeight = (int)g.MeasureString("уЙ", Font).Height + 1;

			searchTimer = new Timer();
			searchTimer.Interval = 950;
			searchTimer.Tick += new EventHandler(searchTimer_Tick);

			fdgv.ColumnHeadersVisible = false;
			fdgv.RowHeadersVisible = false;
			fdgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
			fdgv.RowTemplate.Height = rowHeight;
			fdgv.MultiSelect = false;
			fdgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			fdgv.BorderStyle = BorderStyle.None;
			fdgv.Dock = DockStyle.Fill;
			fdgv.EnterMovesDown = false;
			fdgv.CellClick += new DataGridViewCellEventHandler(fdgv_CellClick);
			fdgv.KeyUp += new KeyEventHandler(fdgv_KeyUp);
			fdgv.KeyPress += new KeyPressEventHandler(fdgv_KeyPress);
			fdgv.CellFormatting += new DataGridViewCellFormattingEventHandler(fdgv_CellFormatting);

			DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
			col.FillWeight = 100;
			col.Resizable = DataGridViewTriState.False;
			fdgv.Columns.Add(col);
			fdgv.AllowAutoGenerateColumns = false;
			SimPanel p = new SimPanel();
			p.Padding = new Padding(1);
			p.BackColor = SystemColors.Window;
			p.Controls.Add(fdgv);

			base.Padding = new Padding(0);
			base.Control = p;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="list">Отображаемый список.</param>
		public SimSelectList(IList list) : this()
		{
			ListBinder b = new ListBinder(list);
			fdgv.DataSource = b;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void searchTimer_Tick(object sender, EventArgs e)
		{
			searchTimer.Stop();
			_searchString = "";
		}
		private void fdgv_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Escape)
			{
				if (e.KeyChar == (char)Keys.Back)
				{
					_searchString = "";
					searchTimer.Stop();
					fdgv.CurrentCell = fdgv.Rows.Count > 0 ? fdgv[0, 0] : null;
					return;
				}
				if ((e.KeyChar >= (char)Keys.D0 && e.KeyChar <= (char)Keys.D9)
					|| e.KeyChar.ToString().Length == 1)
				{
					string code;
					if (e.KeyChar >= (char)Keys.D0 && e.KeyChar <= (char)Keys.D9)
						code = e.KeyChar.ToString().Replace("D", "");
					else
						code = e.KeyChar.ToString();
					SearchSelectList(code);
				}
			}
		}
		private void SearchSelectList(string keyString)
		{
			_searchString += keyString;
			bool isRepSequence = _searchString.Trim(keyString.ToCharArray()).Length == 0 && _searchString.Length > 0;
			int currentIndex = 0, countCheck = 0, countRow;
			countRow = fdgv.Rows.Count;
			DataGridViewCell currentCell;
			if (fdgv.CurrentRow == null || fdgv.CurrentRow.Index < 0)
				currentIndex = 0;
			else
				currentIndex = fdgv.CurrentRow.Index;

			if (_searchString.Length == 1 || isRepSequence)
			{
				currentCell = fdgv[0, currentIndex];
				if (currentCell.FormattedValue != null && currentCell.FormattedValue.ToString().StartsWith(keyString,
						StringComparison.CurrentCultureIgnoreCase))
				{
					if (currentIndex < (countRow - 1))
						currentIndex++;
					else
						currentIndex = 0;
				}
			}

			while (countCheck < countRow)
			{
				currentCell = fdgv[0, currentIndex];
				if (isRepSequence)
				{
					if (currentCell.FormattedValue != null && currentCell.FormattedValue.ToString().StartsWith(keyString,
						StringComparison.CurrentCultureIgnoreCase))
					{
						fdgv.CurrentCell = currentCell;
						break;
					}
				}
				else
					if (currentCell.FormattedValue != null && currentCell.FormattedValue.ToString().StartsWith(_searchString,
						StringComparison.CurrentCultureIgnoreCase))
					{
						fdgv.CurrentCell = currentCell;
						break;
					}

				countCheck++;
				if (currentIndex < (countRow - 1))
					currentIndex++;
				else
					currentIndex = 0;
			}

			searchTimer.Stop();
			searchTimer.Enabled = true;
		}
		private void fdgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
				return;
			sel = fdgv.Rows[e.RowIndex].GetData();
			this.Hide();
			OnItemSelected();
		}
		//-------------------------------------------------------------------------------------
		private void fdgv_KeyUp(object sender, KeyEventArgs e)
		{
			if (fdgv.SelectedRows.Count == 0)
				return;
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.Handled = true;
				sel = fdgv.SelectedRows[0].GetData();
				this.Hide();
				OnItemSelected();
			}
			else
				if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
				{
					_searchString = "";
					searchTimer.Stop();
				}
		}
		//-------------------------------------------------------------------------------------
		private void fdgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex > -1)
				{
					object obj = fdgv.Rows[e.RowIndex].GetData();
					if (obj == null)
						e.Value = "";
					else
					{
						TypeConverter tc = TypeDescriptor.GetConverter(obj);
						if (tc == null || tc.CanConvertTo(typeof(string)) == false)
							e.Value = obj.ToString();
						else
							e.Value = tc.ConvertToString(obj);
					}
					e.FormattingApplied = true;
				}
			}
			catch
			{
				//--- Debbuger Break --- //
				if (System.Diagnostics.Debugger.IsAttached)
					System.Diagnostics.Debugger.Break();
				//--- Debbuger Break --- //
				throw;
			}
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public override void Show(int x, int y)
		{
		 Binder.Refresh();
			fdgv.Parent.Height = (Binder.Count > maxItems ? maxItems : Binder.Count) * rowHeight + 2;
			if (sel != null)
				for (int a = 0; a < Binder.Count; a++)
					if(Object.Equals(fdgv.Rows[a].GetData(), sel))
						fdgv.CurrentCell = fdgv[0, a];
			base.Show(x, y);
			fdgv.Select();
			fdgv.Focus();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает ширину в соответствии с длиной элементов.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public void SetAutoWidth(int min = 10, int max = 600)
		{
			int width = min;
			using (Graphics g = CreateGraphics())
				for (int a = 0; a < (maxItems > Binder.Count ? Binder.Count : maxItems); a++)
				{
					string value;
					TypeConverter tc = TypeDescriptor.GetConverter(Binder[a]);
					if (tc == null || tc.CanConvertTo(typeof(string)) == false)
						value = Binder[a].ToString();
					else
						value = tc.ConvertToString(Binder[a]);
					int w = (int)g.MeasureString(value, Font).Width;
					if (width < w)
						width = w;
				}
			if (width > max)
				width = max;
			else if(width+5 >= min) //(width != min)
				width += 5;
			Width = width;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
}


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
	/// Класс всплывающего окна выбора нескольких элементов.
	/// </summary>
	public class SimCheckList : SimPopupControl
	{
		//private static Bitmap empty =  new Bitmap(9,9);
		private static int rowHeight = 16;

		private SimDataGridView fdgv = new SimDataGridView();
		private PList<ValuesPair<bool, object>> list = null;
		private int maxItems = 25;
		private Timer searchTimer;
		private string _searchString;
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
		/// Возвращает выбранные объекты
		/// </summary>
		public IEnumerable CheckedItems
		{
			get
			{
				foreach (var i in list)
					if (i.Value1)
						yield return i.Value2;
			}
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
		protected SimCheckList()
			: base()
		{
			Graphics g = CreateGraphics();
			rowHeight = (int)g.MeasureString("уЙ", Font).Height + 1;

			searchTimer = new Timer();
			searchTimer.Interval = 950;
			searchTimer.Tick += new EventHandler(searchTimer_Tick);

			fdgv.ColumnHeadersVisible = false;
			fdgv.RowHeadersVisible = false;
			fdgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
			fdgv.RowTemplate.Height = rowHeight;
			fdgv.ShowCellErrors = false;
			fdgv.ShowRowErrors = false;
			fdgv.MultiSelect = false;
			fdgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			fdgv.BorderStyle = BorderStyle.None;
			fdgv.Dock = DockStyle.Fill;
			fdgv.EnterMovesDown = false;
			fdgv.CellClick += new DataGridViewCellEventHandler(fdgv_CellClick);
			fdgv.CellDoubleClick += new DataGridViewCellEventHandler(fdgv_CellDoubleClick);
			fdgv.KeyUp += new KeyEventHandler(fdgv_KeyUp);
			fdgv.KeyPress += new KeyPressEventHandler(fdgv_KeyPress);
			fdgv.CellFormatting += new DataGridViewCellFormattingEventHandler(fdgv_CellFormatting);

			DataGridViewImageColumn ci = new DataGridViewImageColumn();
			ci.Width = 20;
			ci.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			ci.Resizable = DataGridViewTriState.False;
			ci.DataPropertyName = "Value1";
			fdgv.Columns.Add(ci);

			DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
			col.FillWeight = 100;
			col.DataPropertyName = "Value2";
			col.Resizable = DataGridViewTriState.False;
			fdgv.Columns.Add(col);
			fdgv.AllowAutoGenerateColumns = false;
			Panel p = new Panel() { Padding = new Padding(1), BackColor = SystemColors.Window };
			p.Controls.Add(fdgv);
			base.Padding = new Padding(0);
			base.Control = p;
		}

		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="items">Отображаемые элементы.</param>
		/// <param name="checkedItems">Выбраные элементы</param>
		public SimCheckList(IEnumerable items, IEnumerable checkedItems)
			: this()
		{
			if (items == null)
				throw new ArgumentNullException("items");
			list = new PList<ValuesPair<bool, object>>();
			foreach (var x in items)
			{
				ValuesPair<bool, object> i = new ValuesPair<bool, object>(false, x);
				i.Value1 = checkedItems != null && checkedItems.Contains(x);
				list.Add(i);
			}
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
			if (e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Escape && e.KeyChar != (char)Keys.Space)
			{
				if (e.KeyChar == (char)Keys.Back)
				{
					_searchString = "";
					searchTimer.Stop();
					fdgv.CurrentCell = fdgv.Rows.Count > 0 ? fdgv[1, 0] : null;
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
				currentCell = fdgv[1, currentIndex];
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
				currentCell = fdgv[1, currentIndex];
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
		//-------------------------------------------------------------------------------------
		private void fdgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != 0 || e.RowIndex == -1)
				return;
			ValuesPair<bool, object> x = (ValuesPair<bool, object>)fdgv.Rows[e.RowIndex].GetData();
			x.Value1 = !x.Value1;
			fdgv.InvalidateRow(e.RowIndex);
		}
		private void fdgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != 1 || e.RowIndex == -1)
				return;
			ValuesPair<bool, object> x = (ValuesPair<bool, object>)fdgv.Rows[e.RowIndex].GetData();
			x.Value1 = !x.Value1;
			fdgv.InvalidateRow(e.RowIndex);
		}
		private void fdgv_KeyUp(object sender, KeyEventArgs e)
		{
			if (fdgv.SelectedRows.Count == 0)
				return;
			if (e.KeyCode == Keys.Space)
			{
				e.Handled = true;
				ValuesPair<bool, object> x = (ValuesPair<bool, object>)fdgv.SelectedRows[0].GetData();
				x.Value1 = !x.Value1;
				fdgv.InvalidateRow(fdgv.SelectedRows[0].Index);
			}
			else
				if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
				{
					_searchString = "";
					searchTimer.Stop();
				}
		}
		private void fdgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex == -1 || e.ColumnIndex == -1)
				return;
			ValuesPair<bool, object> x = (ValuesPair<bool, object>)fdgv.Rows[e.RowIndex].GetData();
			if (e.ColumnIndex == 0)
				e.Value = x.Value1 ? Properties.Resources.CheckBox_Ov : Properties.Resources.CheckBox_Oo;
			else
			{
				if (x.Value2 == null)
					e.Value = "";
				else
				{
					TypeConverter tc = TypeDescriptor.GetConverter(x.Value2);
					if (tc == null || tc.CanConvertTo(typeof(string)) == false)
						e.Value = x.Value2.ToString();
					else
						e.Value = tc.ConvertToString(x.Value2);
				}
				e.FormattingApplied = true;
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
			fdgv.Parent.Height = (Binder.Count > maxItems ? maxItems : Binder.Count) * rowHeight + 2;
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
				for (int a = 0; a < (maxItems > list.Count ? list.Count : maxItems); a++)
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
			else if (width != min)
				width += 25;
			Width = width;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
	/// <summary>
	/// Класс столбца SimDataGridView с текстом и картинкой.
	/// </summary>
	public class SimDataGridViewLabelColumn : DataGridViewColumn
	{
		private Image img = global::Sim.Controls.Properties.Resources.ArrowDown;
		private bool isLeftImageAlign = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет изображение ячейки по умолчанию
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет изображение ячейки по умолчанию.")]
		public Image Image
		{
			get { return img; }
			set { img = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет,будет ли изображение рисоваться слева.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет,будет ли изображение рисоваться слева.")]
		[DefaultValue(false)]
		public bool IsLeftImageAlign
		{
			get { return isLeftImageAlign; }
			set { isLeftImageAlign = value; }
		}
		[DefaultValue(true)]
		public override bool ReadOnly
		{
			get
			{
				return base.ReadOnly;
			}
			set
			{
				base.ReadOnly = value;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridViewLabelColumn() : base(new SimDataGridViewLabelCell())
		{
			this.ReadOnly = true;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Clone()
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			object o = base.Clone();
			((SimDataGridViewLabelColumn)o).img = img;
			((SimDataGridViewLabelColumn)o).isLeftImageAlign = isLeftImageAlign;
			return o;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ToString
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return base.ToString().Replace("DataGridViewColumn", "SimDataGridViewLabelColumn");
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
										
	}
	//*************************************************************************************
	/// <summary>
	/// Класс ячейки столбца SimDataGridViewLabel
	/// </summary>
	public class SimDataGridViewLabelCell : DataGridViewTextBoxCell
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// FormattedValueType
		/// </summary>
		public override Type FormattedValueType
		{
			get { return typeof(String); }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridViewLabelCell() : base()
		{

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Paint
		/// </summary>
		protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
				DataGridViewElementStates cellState, object value, object formattedValue, string errorText, 
				DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, 
				DataGridViewPaintParts paintParts)
		{
			if(OwningColumn is SimDataGridViewLabelColumn == false || DataGridView is SimDataGridView == false)
			{ 
				base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
				return;
			}
			
			Rectangle r = cellBounds;

			Image img = ((SimDataGridView)DataGridView).OnNeedCellImage(rowIndex,OwningColumn.Index,
																			((SimDataGridViewLabelColumn)OwningColumn).Image);
			int imgWidth = 0;
			if(img != null)
				imgWidth = img.Width < 5 ? 7 : img.Width + 2;

			if(((SimDataGridViewLabelColumn)OwningColumn).IsLeftImageAlign)
			{
				r.Width -= imgWidth+2;
				r.X += imgWidth+2;
			}
			else
				r.Width -= imgWidth;
			SolidBrush b;

			// Фон 
			if(cellState.HasFlag(DataGridViewElementStates.Selected))
				b = new SolidBrush(cellStyle.SelectionBackColor);
			else	if(OwningColumn.DataGridView.Enabled == false)
				b = new SolidBrush(SystemColors.Control);
			else
				b = new SolidBrush(cellStyle.BackColor);
			using(b)
				g.FillRectangle(b, cellBounds);

			// Рамки
			base.PaintBorder(g,clipBounds,cellBounds,cellStyle, advancedBorderStyle );

			// Текст
			//StringFormat sf = new StringFormat() { Trimming = StringTrimming.EllipsisCharacter,
			//                                       FormatFlags = StringFormatFlags.NoWrap
			//                                     };

			//#region StringFormat
			//switch(cellStyle.Alignment)
			//{
			// case DataGridViewContentAlignment.BottomCenter:
			// tf |= TextFormatFlags.
			//  break;
			// case DataGridViewContentAlignment.BottomLeft:
			//  sf.Alignment = StringAlignment.Near;
			//  sf.LineAlignment = StringAlignment.Far;
			//  break;
			// case DataGridViewContentAlignment.BottomRight:
			//  sf.Alignment = StringAlignment.Far;
			//  sf.LineAlignment = StringAlignment.Far;
			//  break;
			// case DataGridViewContentAlignment.MiddleCenter:
			//  sf.Alignment = StringAlignment.Center;
			//  sf.LineAlignment = StringAlignment.Center;
			//  break;
			// case DataGridViewContentAlignment.MiddleLeft:
			//  sf.Alignment = StringAlignment.Near;
			//  sf.LineAlignment = StringAlignment.Center;
			//  break;
			// case DataGridViewContentAlignment.MiddleRight:
			//  sf.Alignment = StringAlignment.Far;
			//  sf.LineAlignment = StringAlignment.Center;
			//  break;
			// case DataGridViewContentAlignment.TopCenter:
			//  sf.Alignment = StringAlignment.Center;
			//  sf.LineAlignment = StringAlignment.Near;
			//  break;
			// case DataGridViewContentAlignment.TopLeft:
			//  sf.Alignment = StringAlignment.Near;
			//  sf.LineAlignment = StringAlignment.Near;
			//  break;
			// case DataGridViewContentAlignment.TopRight:
			//  sf.Alignment = StringAlignment.Far;
			//  sf.LineAlignment = StringAlignment.Near;
			//  break;
			// default: goto case DataGridViewContentAlignment.MiddleLeft;
			//} 
			//#endregion StringFormat

			TextFormatFlags tf = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
			DataGridViewContentAlignment t = cellStyle.Alignment;
			if(t==DataGridViewContentAlignment.MiddleCenter||t==DataGridViewContentAlignment.MiddleLeft||t==DataGridViewContentAlignment.MiddleRight)
				tf = tf | TextFormatFlags.VerticalCenter;
			if(t == DataGridViewContentAlignment.BottomCenter||t == DataGridViewContentAlignment.BottomLeft||t == DataGridViewContentAlignment.BottomRight)
				tf = tf | TextFormatFlags.Bottom;
			if(t==DataGridViewContentAlignment.BottomCenter||t==DataGridViewContentAlignment.MiddleCenter||t==DataGridViewContentAlignment.TopCenter)
				tf = tf | TextFormatFlags.HorizontalCenter;
			if(t==DataGridViewContentAlignment.BottomRight||t==DataGridViewContentAlignment.MiddleRight||t==DataGridViewContentAlignment.TopRight)
				tf = tf | TextFormatFlags.Right;


			Color fc;
			if(cellState.HasFlag(DataGridViewElementStates.Selected))
				fc = cellStyle.SelectionForeColor;
			else if(OwningColumn.DataGridView.Enabled == false)
				fc = SystemColors.GrayText;
			else
				fc = cellStyle.ForeColor;
			using (b)
				//g.DrawString((string)formattedValue, cellStyle.Font, b, r, sf);
				TextRenderer.DrawText(g, (string)formattedValue, cellStyle.Font, r, fc, tf);

			if(img == null)
				return;
			if(((SimDataGridViewLabelColumn)OwningColumn).IsLeftImageAlign)
			{
				r.X -= imgWidth;
				r.Width = imgWidth;
				r.Y += (r.Height - img.Height)/2;
				r.Height = img.Height;
			}
			else
			{ 
				r.X += r.Width;//     + imgWidth/2
				r.Width = imgWidth;
				r.Y += (r.Height - img.Height)/2;
				r.Height = img.Height;
			}
			g.DrawImageUnscaled(DataGridView.Enabled ? img : BitmapEffects.GetGrayImage(img),r);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rowIndex"></param>
		protected override void OnMouseEnter(int rowIndex)
		{
			try
			{
				base.OnMouseEnter(rowIndex);
				if(OwningColumn is SimDataGridViewLabelColumn == false || DataGridView is SimDataGridView == false)
					return;
				if(rowIndex == -1 || this.RowIndex == -1)
					return;
				Image img = ((SimDataGridView)DataGridView).OnNeedCellImage(rowIndex, OwningColumn.Index,
																	((SimDataGridViewLabelColumn)OwningColumn).Image);
				int imgWidth = 0;
				if(img != null)
					imgWidth = img.Width < 5 ? 7 : img.Width + 2;

				int shift = 0;
				if(img != null)
					shift = imgWidth;
				Size r = this.GetSize(rowIndex);
				Size s = TextRenderer.MeasureText((string)FormattedValue ?? "", (this.HasStyle ? this.Style: this.InheritedStyle).Font);
				if(r.Width - shift < s.Width)
					this.ToolTipText = (string)FormattedValue;
				else
					this.ToolTipText = null;
			}
			catch
			{
				throw;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseMove
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(OwningColumn is SimDataGridViewLabelColumn == false || DataGridView is SimDataGridView == false)
				return;
			if(e.RowIndex == -1)
				return;
			Image img = ((SimDataGridView)DataGridView).OnNeedCellImage(e.RowIndex, e.ColumnIndex,
																((SimDataGridViewLabelColumn)OwningColumn).Image);
			if(img == null)
				return;
			int imgWidth = img.Width < 5 ? 7 : img.Width + 2;

			Rectangle r ;
			if(((SimDataGridViewLabelColumn)OwningColumn).IsLeftImageAlign)
				r = new Rectangle(0, 0, imgWidth, OwningRow.Height);
			else
				r = new Rectangle(OwningColumn.Width-(imgWidth), 0, (imgWidth), OwningRow.Height);
			if(e.RowIndex > -1 && r.Contains(e.Location))
				this.DataGridView.Cursor = Cursors.Hand;
			else if(this.DataGridView.Cursor != Cursors.Default)
				this.DataGridView.Cursor = Cursors.Default;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseLeave
		/// </summary>
		/// <param name="rowIndex"></param>
		protected override void OnMouseLeave(int rowIndex)
		{
			base.OnMouseLeave(rowIndex);
			if(this.DataGridView.Cursor != Cursors.Default)
				this.DataGridView.Cursor = Cursors.Default;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseClick
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseClick(e);
			if(e.Clicks > 1)
				return;
			if(DataGridView is SimDataGridView == false || e.RowIndex == -1 || e.Button != MouseButtons.Left)
				return;
			Image img = ((SimDataGridView)DataGridView).OnNeedCellImage(e.RowIndex, e.ColumnIndex,
																((SimDataGridViewLabelColumn)OwningColumn).Image);
			if(img == null)
				return;
			int imgWidth = img.Width < 5 ? 7 : img.Width + 2;

			Rectangle r;
			if(((SimDataGridViewLabelColumn)OwningColumn).IsLeftImageAlign)
				r = new Rectangle(0, 0, imgWidth, OwningRow.Height);
			else
				r = new Rectangle(OwningColumn.Width-imgWidth, 0, imgWidth, OwningRow.Height);
			if(e.RowIndex > -1 && r.Contains(e.Location))
			{
				r =  ((SimDataGridView)DataGridView).GetCellDisplayRectangle(e.ColumnIndex,e.RowIndex,false);
				e = new DataGridViewCellMouseEventArgs(e.ColumnIndex,e.RowIndex, r.X + e.X, r.Y + e.Y, (MouseEventArgs)e);
				((SimDataGridView)DataGridView).OnCellImageClick(e); 
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
										
	}
}

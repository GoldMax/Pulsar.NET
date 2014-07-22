using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Pulsar;
using Sim.Controls;

namespace Sim.Controls
{
	/// <summary>
	/// Класс столбца, не имеющего привязки данных.
	/// </summary>
	public class SimDataGridViewUnboundColumn : DataGridViewColumn
	{
		private Func<DataGridViewColumn, object, object> _getVal = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Делегат метода получения значения столбца.
		/// </summary>
		public Func<DataGridViewColumn,object,object> GetValueFunc
		{
			get { return _getVal; }
			set 
			{ 
				if(value == null)
					_getVal = GetValue;
				else
					_getVal = value; 
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridViewUnboundColumn()
			: base(new SimDataGridViewUnboundColumnCell()) 
		{
			_getVal = GetValue;
			SortMode = DataGridViewColumnSortMode.Automatic; 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public SimDataGridViewUnboundColumn(Func<DataGridViewColumn,object, object> getValueMethod) : this()
		{
			if(getValueMethod== null)
				throw new ArgumentNullException("getValueMethod");
			_getVal = getValueMethod;
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
			SimDataGridViewUnboundColumn col = (SimDataGridViewUnboundColumn)base.Clone();
			col._getVal = this._getVal;
			return col;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return base.ToString().Replace("DataGridViewColumn", "FDataGridViewUnboundColumn");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="col"></param>
		/// <param name="rowData"></param>
		/// <returns></returns>
		public virtual object GetValue(DataGridViewColumn col, object rowData)
		{
			return ((SimDataGridView)this.DataGridView).OnUnboundColumnCellValueNeed(col, rowData); 
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------

	}
		//*************************************************************************************
	/// <summary>
	/// 
	/// </summary>
	public class SimDataGridViewUnboundColumnCell : DataGridViewTextBoxCell
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridViewUnboundColumnCell() : base()
		{

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <returns></returns>
		protected override object GetValue(int rowIndex)
		{
			if(rowIndex < 0)
				return base.GetValue(rowIndex);
			if(this.OwningColumn is SimDataGridViewUnboundColumn == false)
				return base.GetValue(rowIndex);
			object o = this.DataGridView.Rows[rowIndex].GetData();
			return ((SimDataGridViewUnboundColumn)this.OwningColumn).GetValueFunc(this.OwningColumn, o);
		}

	}

}

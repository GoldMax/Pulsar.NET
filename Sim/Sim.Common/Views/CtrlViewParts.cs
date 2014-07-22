//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//using Pulsar;
//using Pulsar.WMS;
//using Sim.Refs;
//using Sim.Controls;

//namespace Sim.Common
//{
// /// <summary>
// /// Класс грида для просмотра партий
// /// </summary>
// public class CtrlViewParts : SimDataGridView
// {
//  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//  #region << Properties >>
//  /// <summary>
//  /// DataSource
//  /// </summary>
//  public new object DataSource
//  {
//   get { return base.DataSource; }
//   set
//   {
//    base.DataSource = value;
//    this.Height = this.GetPreferredSize(this.Size).Height;
//    this.CurrentCell = null;
//   }
//  }
//  #endregion << Properties >>
//  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//  //-------------------------------------------------------------------------------------
//  #region << Constructors >>
//  /// <summary>
//  /// Конструктор по умолчанию.
//  /// </summary>
//  public CtrlViewParts() : base() 
//  {
//   this.SuspendLayout();
//   this.AllowAutoGenerateColumns = false;
//   this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
//   this.BorderStyle = System.Windows.Forms.BorderStyle.None;
//   this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
//   this.ColumnHeadersHeight = 17;
//   this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
//   this.ForeColor = SystemColors.WindowText;
//   this.HideSelection = true;
//   this.RowHeadersVisible = false;
//   this.RowTemplate.Height = 18;
//   this.RightClickChangeCurrent = true;
//   this.VirtualMode = true;

//   SimDataGridViewLabelColumn col1 = new SimDataGridViewLabelColumn();
//   col1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//   col1.DataPropertyName = "Amount";
//   col1.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
//   col1.HeaderText = "Кол-во, ед.";
//   col1.Image = Properties.Resources.DropDownArrow;
//   col1.IsLeftImageAlign = true;
//   col1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
//   col1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
//   col1.Width = 70;
//   this.Columns.Add(col1);

//   DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
//   col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//   col.DataPropertyName = "InPrice";
//   col.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
//   col.HeaderText = "Цена вх., руб.";
//   col.Width = 55;
//   this.Columns.Add(col);

//   col = new DataGridViewTextBoxColumn();
//   col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//   col.DataPropertyName = "GTD";
//   col.HeaderText = "ГТД";
//   col.Width = 80;
//   this.Columns.Add(col);

//   col = new DataGridViewTextBoxColumn();
//   col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//   col.DataPropertyName = "Country";
//   col.HeaderText = "Страна";
//   col.Width = 65;
//   this.Columns.Add(col);

//   col = new DataGridViewTextBoxColumn();
//   col.DataPropertyName = "Comment";
//   col.HeaderText = "Коментарий";
//   this.Columns.Add(col);

//   col = new DataGridViewTextBoxColumn();
//   col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//   col.DataPropertyName = "OutDate";
//   col.HeaderText = "Срок годн.";
//   col.Width = 65;
//   this.Columns.Add(col);

//   this.ResumeLayout();

//  }
//  #endregion << Constructors >>
//  //-------------------------------------------------------------------------------------
//  #region << Methods >>
//  /// <summary>
//  /// OnRowsAdded
//  /// </summary>
//  /// <param name="e"></param>
//  protected override void OnRowsAdded(System.Windows.Forms.DataGridViewRowsAddedEventArgs e)
//  { 
//   this.Size = this.PreferredSize;
//   base.OnRowsAdded(e);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// OnRowsRemoved
//  /// </summary>
//  /// <param name="e"></param>
//  protected override void OnRowsRemoved(System.Windows.Forms.DataGridViewRowsRemovedEventArgs e)
//  {
//   if(IsDisposed)
//    return;
//   this.Size = this.PreferredSize;
//   base.OnRowsRemoved(e);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// 
//  /// </summary>
//  /// <param name="e"></param>
//  protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
//  {
//   if(e.Value is DateTime)
//   {
//    e.Value = ((DateTime)e.Value).ToShortDateString();
//    e.FormattingApplied = true;
//   }
//   else 
//    base.OnCellFormatting(e);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// 
//  /// </summary>
//  /// <param name="rowIndex"></param>
//  /// <param name="columnIndex"></param>
//  /// <param name="img"></param>
//  /// <returns></returns>
//  protected override Image OnNeedCellImage(int rowIndex, int columnIndex, Image img)
//  {
//   if(rowIndex > -1 && this.Columns[columnIndex].DataPropertyName == "Amount")
//    if(((StockPart)(dynamic)this.Rows[rowIndex].GetData()).Amount.HasPackInfo == false)
//    {
//     img = null;
//     return null;
//    }
//   return base.OnNeedCellImage(rowIndex, columnIndex, img);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// 
//  /// </summary>
//  /// <param name="e"></param>
//  protected override void OnCellImageClick(DataGridViewCellMouseEventArgs e)
//  {
//   //--- Debbuger Break --- //
//   if(System.Diagnostics.Debugger.IsAttached)
//    System.Diagnostics.Debugger.Break();
//   //--- Debbuger Break --- //

//   //if(e.RowIndex > -1 && this.Columns[e.ColumnIndex].DataPropertyName == "Amount")
//   //{
//   // StockPart pi = (StockPart)(dynamic)this.Rows[e.RowIndex].GetData();
//   // if(pi.Amount.HasPackInfo == false)
//   //  return;
//   // StockPropertyPackMap mapProp = GOL.EssenceManager.GetFirstEssence<StockPropertyPackMap>();
//   // if(mapProp == null)
//   //  return;
//   // PackMap map = (PackMap)mapProp.GetValue(pi.Stock);
//   // if(map == null)
//   //  return;
//   // Panel p = new Panel();
//   // p.Width = 0;
//   // p.Height = 0;
//   // p.BackColor = Color.Transparent;
//   // for(int a = 0; a < map.Count; a++)
//   // {
//   //  if(pi.Amount[a] == 0)
//   //   continue;
//   //  SimTwoLabel l = new SimTwoLabel();
//   //  l.Label1Text = map[a].PackName.ToString();
//   //  l.Label2TextAlign = ContentAlignment.MiddleRight;
//   //  l.Label2Text = pi.Amount[a].ToString();
//   //  l.Dock = DockStyle.Top;
//   //  p.Height += l.Height;
//   //  p.Width = p.Width > l.PreferredSize.Width ? p.Width : l.PreferredSize.Width;
//   //  p.Controls.Add(l);
//   // }
//   // Rectangle r = this.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
//   // r.Y += r.Height;
//   // r = this.RectangleToScreen(r);
//   // SimPopupControl.Show(p, r.Location, false);

//   //}
//   //else
//   // base.OnCellImageClick(e);
//  }
//  #endregion << Methods >>
//  //-------------------------------------------------------------------------------------
          
// }
//}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;


//using Sim.Controls;
//using Pulsar;
//using Pulsar.Docs;
//using Sim.Refs;
//using Pulsar.WMS;

//namespace Sim.Common
//{
// /// <summary>
// /// Класс контрола просмотра списка строк (базовый вариант).
// /// </summary>
// public class CtrlViewDocLinesList : SimControlsGrid
// {
//  private Type lineType = null;
//  private ListBinder listBinder = null;

//  private PropertiesViewControl stockInfo = new PropertiesViewControl();
//  private PropertiesViewControl placeInfo = new PropertiesViewControl();
//  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//  #region << Properties >>
//  /// <summary>
//  /// Список строк
//  /// </summary>
//  public ListBinder ListBinder
//  {
//   get { return listBinder; }
//   set 
//   {
//    listBinder = value; 
//    Fill(); 
//   }
//  }
//  #endregion << Properties >>
//  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//  //-------------------------------------------------------------------------------------
//  #region << Constructors >>
//  /// <summary>
//  /// Конструктор по умолчанию.
//  /// </summary>
//  public CtrlViewDocLinesList(): base()
//  {
//   InitializeComponent();

//   stockInfo.ShowCategories = true;
//   stockInfo.ShowAllProps = true;

//   placeInfo.ShowAllProps = true;
//   placeInfo.ShowCategories = false;
//   this.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);

//   var col1 = new SimDataGridViewLabelColumn();
//   col1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//   col1.IsLeftImageAlign = true;
//   col1.Image = Properties.Resources.DropDownArrow;
//   base.TypedColumns.Add(typeof(Pulsar.PackAmount), col1);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// Инициализирующий конструктор.
//  /// </summary>
//  public CtrlViewDocLinesList(ListBinder listBinder) : this()
//  {
//   this.listBinder = listBinder;
//   Fill();
//  }
//  //-------------------------------------------------------------------------------------
//  private void InitializeComponent()
//  {
//   ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
//   this.SuspendLayout();
//   // 
//   // CtrlViewOperLines
//   // 
//   this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
//   this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
//   this.RowHeadersVisible = false;
//   this.RowTemplate.Height = 18;
//   this.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
//   ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
//   this.ResumeLayout(false);

//  }
//  #endregion << Constructors >>
//  //-------------------------------------------------------------------------------------
//  #region << Methods >>
//  private void Fill()
//  {
//   try
//   {
//    if(listBinder == null)
//     return;

//    object obj = listBinder.Count > 0 ? listBinder.List[0] : null;
//    if(obj == null)
//     lineType = listBinder.List.GetType().GetGenericArguments()[0];
//    else
//     lineType = obj.GetType();

//    this.DataSource = listBinder;
//    foreach(DataGridViewColumn c in this.Columns)
//     if(c.ValueType == typeof(Sim.Refs.Stock))
//     {
//      c.FillWeight = 100;
//      c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
//     }
//     else
//      c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
//    this.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

//    if(lineType.IsSubclassOf(typeof(DocLinePart)))
//    {
//     #region
//     foreach(DataGridViewColumn c in this.Columns)
//      if(c.DataPropertyName == "Part")
//      {
//       this.Columns.Remove(c);
//       break;
//      }

//     this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
//     this.RightClickChangeCurrent = true;

//     DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
//     col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//     col.DataPropertyName = "ID";
//     col.HeaderText = "Код";
//     col.Width = 60;
//     this.Columns.Add(col);

//     col = new DataGridViewTextBoxColumn();
//     col.DataPropertyName = "Stock";
//     col.HeaderText = "Позиция номенклатуры";
//     col.Width = 260;
//     this.Columns.Add(col);

//     SimDataGridViewLabelColumn col1 = new SimDataGridViewLabelColumn();
//     col1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//     col1.DataPropertyName = "Amount";
//     col1.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
//     col1.HeaderText = "Кол-во, ед.";
//     col1.Image = Properties.Resources.DropDownArrow;
//     col1.IsLeftImageAlign = true;
//     col1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
//     col1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
//     col1.Width = 70;
//     this.Columns.Add(col1);

//     col = new DataGridViewTextBoxColumn();
//     col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//     col.DataPropertyName = "InPrice";
//     col.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
//     col.HeaderText = "Цена вх., руб.";
//     col.Width = 55;
//     this.Columns.Add(col);

//     col = new DataGridViewTextBoxColumn();
//     col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//     col.DataPropertyName = "GTD";
//     col.HeaderText = "ГТД";
//     col.Width = 80;
//     this.Columns.Add(col);

//     col = new DataGridViewTextBoxColumn();
//     col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//     col.DataPropertyName = "Country";
//     col.HeaderText = "Страна";
//     col.Width = 65;
//     this.Columns.Add(col);

//     col = new DataGridViewTextBoxColumn();
//     col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//     col.DataPropertyName = "Comment";
//     col.HeaderText = "Коментарий";
//     col.Width = 65;
//     this.Columns.Add(col);

//     col = new DataGridViewTextBoxColumn();
//     col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
//     col.DataPropertyName = "OutDate";
//     col.HeaderText = "Срок годн.";
//     col.Width = 65;
//     this.Columns.Add(col);

//     #endregion
//    }
//    else
//    {
     
//    }
//   }
//   catch(Exception Err)
//   {
//    Sim.Controls.ModalErrorBox.Show(Err, this.FindModalContainer());
//   }
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// 
//  /// </summary>
//  /// <param name="column"></param>
//  /// <param name="boundedObject"></param>
//  /// <returns></returns>
//  protected override object OnUnboundColumnCellValueNeed(DataGridViewColumn column, object boundedObject)
//  {
//   if(lineType.IsSubclassOf(typeof(DocLinePart)))
//   {
//    StockPart p = ((DocLinePart)(dynamic)boundedObject).Part;
//    switch(column.DataPropertyName)
//    {
//     case "ID" : return p.Stock.ID;
//     case "Stock" : return p.Stock;
//     case "Amount" : return p.Amount;
//     case "InPrice" : return p.InPrice;
//     case "GTD" : return p.GTD;
//     case "Country" : return p.Country;
//     case "Comment" : return p.Comment;
//     case "OutDate" : return p.OutDate;
//     default : return base.OnUnboundColumnCellValueNeed(column, boundedObject);
//    }
//   }
//   else
//    return base.OnUnboundColumnCellValueNeed(column, boundedObject);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// 
//  /// </summary>
//  /// <param name="e"></param>
//  protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
//  {
//   try
//   {
//    if(e.Value is DateTime)
//    {
//     DateTime val = (DateTime)e.Value;
//     if(val.TimeOfDay.Ticks == 0)
//      e.Value = val.ToShortDateString();
//     else
//      e.Value = val.ToString();
//     e.FormattingApplied = true;
//    }
//    else if(e.Value is Enum)
//    {
//     e.Value = Pulsar.Reflection.EnumTypeConverter.GetItemDisplayName(e.Value);
//     e.FormattingApplied = true;
//    }
//    else if(e.Value is IPulsarCluster)
//    {
//     e.Value = e.Value.ToString();
//     e.FormattingApplied = true;
//    }
//    else
//     base.OnCellFormatting(e);
//   }
//   catch(Exception Err)
//   {
//    Sim.Controls.ErrorBox.Show(Err);
//   }
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
//   if(rowIndex == -1)
//    return base.OnNeedCellImage(rowIndex, columnIndex, img);
//   PackAmount val = base[columnIndex, rowIndex].Value as PackAmount;
//   if(val != null && val.HasPackInfo == false)
//   {
//    img = null;
//    return null;
//   }
//   return base.OnNeedCellImage(rowIndex, columnIndex, img);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// 
//  /// </summary>
//  /// <param name="e"></param>
//  protected override void OnCellImageClick(DataGridViewCellMouseEventArgs e)
//  {
//   if(e.RowIndex == -1)
//    return;
//   PackAmount val = base[e.ColumnIndex, e.RowIndex].Value as PackAmount;
//   if(val != null && val.HasPackInfo)
//   {
//    Stock s = null;
//    if(lineType == typeof(DocLinePart) || lineType.IsSubclassOf(typeof(DocLinePart)))
//     s = ((DocLinePart)(dynamic)this.Rows[e.RowIndex].GetData()).Part.Stock;
//    if(lineType == typeof(DocLineStock) || lineType.IsSubclassOf(typeof(DocLineStock)))
//     s = ((DocLineStock)(dynamic)this.Rows[e.RowIndex].GetData()).Stock;
//    if(lineType == typeof(DocLineStockAmount) || lineType.IsSubclassOf(typeof(DocLineStockAmount)))
//     s = ((DocLineStockAmount)(dynamic)this.Rows[e.RowIndex].GetData()).Stock;
//    if(s == null)
//     return;
//    StockPropertyPackMap mapProp = GOL.EssenceManager.GetFirstEssence<StockPropertyPackMap>();
//    if(mapProp == null)
//     return;
//    PackMap map = (PackMap)mapProp.GetValue(s);
//    if(map == null)
//     return;
//    Panel p = new Panel();
//    p.Width = 0;
//    p.Height = 0;
//    p.BackColor = Color.Transparent;
//    for(int a = 0; a < map.Count; a++)
//    {
//     if(val[a] == 0)
//      continue;
//     SimTwoLabel l = new SimTwoLabel();
//     l.Label1Text = map[a].PackName.ToString();
//     l.Label2TextAlign = ContentAlignment.MiddleRight;
//     l.Label2Text = val[a].ToString();
//     l.Dock = DockStyle.Top;
//     p.Height += l.Height;
//     p.Width = p.Width > l.PreferredSize.Width ? p.Width : l.PreferredSize.Width;
//     p.Controls.Add(l);
//    }
//    Rectangle r = this.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
//    r.Y += RowTemplate.Height;
//    r = this.RectangleToScreen(r);
//    SimPopupControl.Show(p, r.Location, false);

//   }
//   else
//    base.OnCellImageClick(e);
//  }
//  //-------------------------------------------------------------------------------------
//  /// <summary>
//  /// 
//  /// </summary>
//  /// <param name="e"></param>
//  protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
//  {
//   if(e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex > -1)
//   {
//    object val = this[e.ColumnIndex, e.RowIndex].Value;
//    if(val is Stock)
//    {
//     Stock s  = (Stock)val;
//     ToolStripMenuItem i = new ToolStripMenuItem("Свойства позиции", Properties.Resources.Property);
//     i.Name = "propInfo";
//     i.DropDown = new SimPopupControl(stockInfo);
//     this.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
//     this.ContextMenuStrip.Items.Insert(0, i);
//     stockInfo.Object = s;
//    }
//    else if(val is StorePlace)
//    {
//     StorePlace s  = (StorePlace)val;
//     ToolStripMenuItem i = new ToolStripMenuItem("Параметры места хранения", Properties.Resources.Property);
//     i.Name = "propInfo";
//     i.DropDown = new SimPopupControl(placeInfo);
//     this.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
//     this.ContextMenuStrip.Items.Insert(0, i);
//     placeInfo.Object = s;
//    }
//   }
//   base.OnCellMouseClick(e);
//  }
//  //-------------------------------------------------------------------------------------
//  void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
//  {
//   ContextMenuStrip menu = (ContextMenuStrip)sender;
//   if(menu.Items.Count > 0 && menu.Items[0].Name == "propInfo")
//   {
//    menu.Items.RemoveAt(0);
//    menu.Items.RemoveAt(0);
//   } 
//  }
//  #endregion << Methods >>
//  //-------------------------------------------------------------------------------------

// }
//}

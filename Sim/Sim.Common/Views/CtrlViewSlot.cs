//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//
//using Sim.Collections.ExTypes;
//using Sim.Controls;
//using Sim.Pulsar;
//using Sim.Pulsar.References;
//using Sim.Pulsar.RestStore;

//namespace Sim.Common
//{
// /// <summary>
// /// Класс контрола отображения товаров слота.
// /// </summary>
// public partial class CtrlViewSlot : UserControl
// {
//  private StockInfoToolTip stockInfo = new StockInfoToolTip();
//  private Slot slot = null;
//  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//  #region << Properties >>
//  /// <summary>
//  /// Слот, отображаемый контролом.
//  /// </summary>
//  [Browsable(false)]
//  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//  public Slot Slot
//  {
//   get { return slot; }
//   set 
//   { 
//    slot = value; 
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
//  public CtrlViewSlot()
//  {
//   InitializeComponent();
//  }
//  #endregion << Constructors >>
//  //-------------------------------------------------------------------------------------
//  #region << Controls Handlers>>
//  private void grid_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
//  {
//   try
//   {
//    if(e.RowIndex < 0 || e.ColumnIndex < 0)
//     return;
//    if(grid.Columns[e.ColumnIndex].DataPropertyName != "Name")
//     return;
//    SlotStockInfo si = (SlotStockInfo)grid.Rows[e.RowIndex].GetData();
//    if(si.Kind != null)
//     return;
//    stockInfo.Stock = si.Stock;
//    e.ContextMenuStrip = stockInfo;
//   }
//   catch(Exception Err)
//   {
//    Sim.Controls.ModalErrorBox.Show(Err, this.FindModalContainer());
//   }
//  }
//  #endregion << Controls Handlers>>
//  //-------------------------------------------------------------------------------------
//  #region << Methods >>
//  private void Fill()
//  {
//   try
//   {
//    SimTree<SlotStockInfo> tree = new SimTree<SlotStockInfo>();
//    foreach(Stock s in slot)
//    {
//     SlotStockInfo root = new SlotStockInfo();
//     root.Stock = s;
//     root.Info = new SlotStockAmountInfo();
//     tree.Add(root, (SlotStockInfo)null);
//     foreach(RestSource ak in slot[s].Keys)
//     {
//      SlotStockAmountInfo si = slot[s][ak];
//      root.Info.Need += si.Need;
//      root.Info.Set += si.Set;
//      root.Info.Paid += si.Paid;
//      root.Info.Rest += si.Rest;
//      tree.Add(new SlotStockInfo(s, ak, si), root);
//     }
//    }
//    Dictionary<string, int> colSizes = null;
//    if(grid.SimTree != null)
//    {
//     colSizes = new Dictionary<string, int>();
//     foreach(DataGridViewColumn col in grid.Columns)
//      colSizes.Add(col.Name, col.Width);
//    }
//    grid.SimTree = tree;
//    foreach(DataGridViewColumn col in grid.Columns)
//    {
//     if(colSizes != null && colSizes.ContainsKey(col.Name))
//      col.Width = colSizes[col.Name];
//     else if(col.ValueType == typeof(Decimal))
//     {
//      col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
//      col.Width = 70;
//     }
//     if(col.ValueType == typeof(Decimal) || col.ValueType == typeof(Decimal?))
//     {
//      col.DefaultCellStyle.Format = "#,0.###";
//      col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//     }
//    }

//   }
//   catch(Exception Err)
//   {
//    Sim.Controls.ModalErrorBox.Show(Err, this.FindModalContainer());
//   }
//  }
//  #endregion << Methods >>
//  //-------------------------------------------------------------------------------------
//  //*************************************************************************************
//  #region << private class SlotStockInfo >>
//  private class SlotStockInfo
//  {
//   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//   #region << Properties >>
//   [DisplayName("Наименование")]
//   public string Name
//   {
//    get { return Kind == null ? Stock.ToString() : EnumTypeConverter.GetItemDisplayName(Kind); }
//   }
//   internal Stock Stock { get; set; }
//   internal RestSource? Kind { get; set; }
//   internal SlotStockAmountInfo Info { get; set; }
//   [DisplayName("Необходимо")]
//   public decimal Need { get { return Info.Need; } }
//   [DisplayName("Обработано")]
//   public decimal Set { get { return Info.Set; } }
//   [DisplayName("Оплачено")]
//   public decimal Paid { get { return Info.Paid; } }
//   [DisplayName("Подготовлено")]
//   public decimal Rest { get { return Info.Rest; } }
//   #endregion << Properties >>
//   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//   //-------------------------------------------------------------------------------------
//   #region << Constructors >>
//   /// <summary>
//   /// Конструктор по умолчанию.
//   /// </summary>
//   public SlotStockInfo() { }
//   //-------------------------------------------------------------------------------------
//   /// <summary>
//   /// Инициализирующий конструктор.
//   /// </summary>
//   public SlotStockInfo(Stock stock, RestSource? kind, SlotStockAmountInfo info)
//   {
//    Stock = stock;
//    Kind = kind;
//    Info = info;
//   }
//   #endregion << Constructors >>
//   //-------------------------------------------------------------------------------------
//  } 
//  #endregion << private class SlotStockInfo >>
// }
//}

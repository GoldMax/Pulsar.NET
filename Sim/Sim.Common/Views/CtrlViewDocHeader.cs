using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


using Sim.Controls;
using Pulsar;

namespace Sim.Common
{
 /// <summary>
 /// Класс контрола отображения свойств объекта.
 /// </summary>
 public class CtrlViewDocHeader : SimPanel
 {
  private object obj = null;
  private bool isTable = false;
  private HashSet<Type> typeFilter = null;
  private bool showID = false;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Объект, для которого отображаются свойства.
  /// </summary>
  [Browsable(false)]
  public object Object
  {
   get { return obj; }
   set
   {
    obj = value;
    Fill();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет стиль отображения.
  /// </summary>
  [Description("Определяет стиль отображения.")]
  [DefaultValue(false)]
  public bool IsTableView
  {
   get { return isTable; }
   set
   {
    isTable = value;
    Fill();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Набор типов, свойства которых будут отображаться.
  /// </summary>
  [Browsable(false)]
  public HashSet<Type> TypeFilter
  {
   get { return typeFilter; }
   set { typeFilter = value; Fill(); }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет необходимость отображения OID
  /// </summary>
  [DefaultValue(false)]
  public bool ShowOID
  {
   get { return showID; }
   set { showID = value; Fill(); }

  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public CtrlViewDocHeader()
   : base()
  {
   //this.BorderStyle = BorderStyle.FixedSingle;
   this.BackColor = ControlPaint.Light(SystemColors.Control, 0.7f);
   //this.AutoScroll = true;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public CtrlViewDocHeader(object obj)
   : this()
  {
   this.obj = obj;
   Fill();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public CtrlViewDocHeader(object obj, HashSet<Type> filter)
   : this()
  {
   this.obj = obj;
   this.typeFilter = filter;
   Fill();
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  private void Fill()
  {
   try
   {
    this.Controls.Clear();
    if(obj == null)
     return;
    this.SuspendLayout();

    //List<Tuple<PropertyDescriptor,int?>> list = new List<Tuple<PropertyDescriptor, int?>>();
    //foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(obj))
    // if(((showID && pd.Name == "OID") || pd.IsBrowsable) && 
    //    (typeFilter == null || typeFilter.Contains(pd.ComponentType)))
    // {
    //  DisplayOrderAttribute attr = pd.Attributes.OfType<DisplayOrderAttribute>().FirstOrDefault();
    //  list.Add(new Tuple<PropertyDescriptor, int?>(pd, attr == null ? null : (int?)attr.Order));
    // }
    //list.Sort(Sorter);

    if(IsTableView)
    {
     SimDataGridView grid = new SimDataGridView();
     List<ValuesPair<string, string>> viewList = new List<ValuesPair<string, string>>();

     foreach(PropertyDescriptor i in TypeDescriptor.GetProperties(obj))
     {
      if(!((showID && i.Name == "OID") || i.IsBrowsable))
       continue;
      if(typeFilter != null && typeFilter.Contains(i.ComponentType) == false)
       continue;
      ValuesPair<string, string> item = new ValuesPair<string, string>();
      item.Value1 = i.DisplayName;

      object val = i.GetValue(obj) ?? "(пусто)";
      if(val is DateTime)
      {
       DateTime d = (DateTime)val;
       if(d.TimeOfDay.Ticks == 0)
        item.Value2 = d.ToShortDateString();
       else
        item.Value2 = val.ToString();
      }
      else if(val is IList)
      {
       StringBuilder sb = new StringBuilder();
       for(int a = 0; a < ((IList)val).Count; a++)
       {
        sb.AppendFormat("{0}", i.Converter is ReferenceConverter ? (((IList)val)[a] ?? "").ToString() :
                                                           i.Converter.ConvertToString(((IList)val)[a]));
        if(!(((IList)val).Count < 2 || a == ((IList)val).Count -1))
         sb.AppendLine("");
       }
       if(sb.Length == 0)
        item.Value2 = "(пусто)";
       else
        item.Value2 = sb.ToString();
      }
      else if(i.Converter is ReferenceConverter)
       item.Value2 = (val ?? "").ToString();
      else
       item.Value2 = i.Converter.ConvertToString(val);
      viewList.Add(item);
     }
     grid.RowHeadersVisible = false;
     grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
     grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
     grid.RowTemplate.Height = 16;
     grid.ColumnHeadersVisible = false;
     grid.DefaultCellStyle.SelectionForeColor = grid.DefaultCellStyle.ForeColor;
     grid.Dock = DockStyle.Fill;
     grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
     grid.BackgroundColor = this.BackColor;
     grid.DefaultCellStyle.BackColor = this.BackColor;
     grid.DefaultCellStyle.SelectionBackColor = this.BackColor;
     grid.CellPainting += new DataGridViewCellPaintingEventHandler(grid_CellPainting);

     grid.DataSource = new ListBinder(viewList);
     grid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
     this.Controls.Add(grid);
    }
    else
    {
     SimLayoutPanel p = new SimLayoutPanel() { AutoSizeMode = AutoSizeMode.GrowAndShrink, AutoSize = true };
     foreach(PropertyDescriptor i in TypeDescriptor.GetProperties(obj))
     {
      if(!((showID && i.Name == "OID") || i.IsBrowsable))
       continue;
      if(typeFilter != null && typeFilter.Contains(i.ComponentType) == false)
       continue;
      SimTwoLabel l = new SimTwoLabel();
      l.Label1Text = i.DisplayName + ":";
      l.Label1Image = global::Sim.Common.Properties.Resources.Point_x6;
      l.Label1Font = new Font(this.Font, FontStyle.Bold);
      l.Label1ForeColor = SystemColors.ControlDark;
      object val = i.GetValue(obj) ?? "(пусто)";
      if(val is DateTime)
       l.Label2Text = ((DateTime)val).TimeOfDay.Ticks == 0 ? ((DateTime)val).ToShortDateString() : val.ToString();
      else
       l.Label2Text = TypeDescriptor.GetConverter(val).ConvertToString(val);
      l.Margin = new Padding(0, 0, 6, 4);
      p.Controls.Add(l);
     }
     //p.BackColor = Color.Red;
     p.Dock = DockStyle.Top;
     this.Controls.Add(p);
    }

   }
   catch(Exception Err)
   {
    Sim.Controls.ModalErrorBox.Show(Err, this.FindModalContainer());
   }
   finally
   {
    this.PerformLayout();
    this.ResumeLayout();
   }
  }
  //-------------------------------------------------------------------------------------
  void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
  {
   if(e.ColumnIndex > -1 && e.RowIndex > -1 && e.Value is string && 
      ((string)e.Value).Contains("\r\n"))
   {
    SimDataGridView grid = (SimDataGridView)sender;
    string[] ss = ((string)e.Value).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    if(ss.Length > 1)
    {
     int h = grid.RowTemplate.Height;
     h = h * ss.Length; // (int)e.Graphics.MeasureString(ss[0], e.CellStyle.Font).Height
     grid.Rows[e.RowIndex].Height = h;
     e.CellStyle.WrapMode = DataGridViewTriState.True;
     grid[0, e.RowIndex].Style.Alignment = DataGridViewContentAlignment.TopLeft;
    }
   }
  }
  //-------------------------------------------------------------------------------------
  private int Sorter(Tuple<PropertyDescriptor, int?> a, Tuple<PropertyDescriptor, int?> b)
  {
   if(a.Item2 == null && b.Item2 == null)
    return 0;
   if(a.Item2 != null && b.Item2 != null)
    return Comparer<int?>.Default.Compare(a.Item2, b.Item2);
   if(a.Item2 != null)
    return -1;
   if(b.Item2 != null)
    return 1;
   return 0;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// GetPreferredSize
  /// </summary>
  /// <param name="proposedSize"></param>
  /// <returns></returns>
  public override Size GetPreferredSize(Size proposedSize)
  {
   if(this.Controls.Count == 0)
    return new Size(proposedSize.Width, MinimumSize.Height);
   int height = 0;
   foreach(Control c in this.Controls)
   {
    Size s = new Size(proposedSize.Width - this.Padding.Horizontal - c.Margin.Horizontal, 1);
    height += c.GetPreferredSize(s).Height; // + c.Margin.Vertical;
   }
   return new Size(proposedSize.Width, height + this.Padding.Vertical);
   //if(isTable)
   //{
   // return base.GetPreferredSize(proposedSize);
   //}
   //else
   //{
   //}
  }
  //-------------------------------------------------------------------------------------
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnBackColorChanged(EventArgs e)
  {
   base.OnBackColorChanged(e);
   if(isTable)
   {
    Control c = this;
    do
    {
     if(c.BackColor != Color.Transparent)
     {
      ((SimDataGridView)this.Controls[0]).BackgroundColor = c.BackColor;
      ((SimDataGridView)this.Controls[0]).DefaultCellStyle.BackColor = c.BackColor;
      ((SimDataGridView)this.Controls[0]).DefaultCellStyle.SelectionBackColor = c.BackColor;
      break;
     }
     c = c.Parent;
    }
    while(c != null);

   }
  }
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnVisibleChanged(EventArgs e)
  {
   base.OnBackColorChanged(e);
   if(this.Visible && isTable && this.Controls.Count > 0)
   {
    Control c = this;
    do
    {
     if(c.BackColor != Color.Transparent)
     {
      ((SimDataGridView)this.Controls[0]).BackgroundColor = c.BackColor;
      ((SimDataGridView)this.Controls[0]).DefaultCellStyle.BackColor = c.BackColor;
      ((SimDataGridView)this.Controls[0]).DefaultCellStyle.SelectionBackColor = c.BackColor;
      break;
     }
     c = c.Parent;
    }
    while(c != null);

   }
  }
 }
}

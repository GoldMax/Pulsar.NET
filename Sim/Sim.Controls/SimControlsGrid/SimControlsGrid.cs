using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

//using Pulsar;
using Sim.Controls.WinAPI;

namespace Sim.Controls
{
 /// <summary>
 /// 
 /// Respect to Jorge Paulino
 /// </summary>
 [DefaultEvent("NeedControl")]
 public class SimControlsGrid : SimDataGridView
 {
  private delegate void SetCellPaintingEventArgsGrid(DataGridViewCellPaintingEventArgs args, DataGridView grid);
  private static SetCellPaintingEventArgsGrid setGrid = null;

  //private HashSet<Control> _opened = new HashSet<Control>();
  private Dictionary<object, ControlInfo> index = new Dictionary<object, ControlInfo>();
  private int cIndent = 17;
  private bool cFill = true;
  private int cMaxHeight = 300;
  private bool followControlFocus = true;
  private bool removeControlOnRowCollapce = false;

  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << Events >>
  #region << public event EventHandler<NeedControlEventArgs> NeedControl >>
  /// <summary>
  /// Класс аргумента события NeedControl
  /// </summary>
  public class NeedControlEventArgs : EventArgs
  {
   private Control ctrl = null;
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Properties >>
   /// <summary>
   /// Контрол
   /// </summary>
   public Control Control
   {
    get { return ctrl; }
    set { ctrl = value; }
   }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Индекс строки
   /// </summary>
   public object RowIndex { get; set; }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Объект строки
   /// </summary>
   public object RowData { get; set; }
   #endregion << Properties >>
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   //-------------------------------------------------------------------------------------
   #region << Constructors >>
   /// <summary>
   /// Конструктор по умолчанию.
   /// </summary>
   public NeedControlEventArgs() : base() { }
   //-------------------------------------------------------------------------------------
   /// <summary>
   /// Инициализирующий конструктор.
   /// </summary>
   public NeedControlEventArgs(Control control, int rowIndex, object rowData)
    : base()
   {
    ctrl = control;
    RowIndex = rowIndex;
    RowData = rowData;
   }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
  }
  /// <summary>
  /// Событие, возникаюшее при необходимости получить контрол.
  /// </summary>
  public event EventHandler<NeedControlEventArgs> NeedControl;
  /// <summary>
  /// Вызывает событие NeedControl
  /// </summary>
  /// <returns></returns>
  protected Control OnNeedControl(int rowIndex, object rowData)
  {
   if(NeedControl != null)
   {
    NeedControlEventArgs args = new NeedControlEventArgs() { RowIndex = rowIndex, RowData = rowData };
    NeedControl(this, args);
    return args.Control;
   }
   return null;
  }
  #endregion << public event EventHandler<NeedControlEventArgs> NeedControl >>
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region public event EventHandler<HasControlEventArgs> HasControl
  /// <summary>
  /// Класс аргумента события HasControl
  /// </summary>
  public class HasControlEventArgs : GridRowEventArgs
  {
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Properties >>
   /// <summary>
   /// Определяет, имеется ли контрол.
   /// </summary>
   public bool HasControl { get; set; }
   #endregion << Properties >>
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   //-------------------------------------------------------------------------------------
   #region << Constructors >>
   /// <summary>
   /// Конструктор по умолчанию.
   /// </summary>
   public HasControlEventArgs() : base() { }
   //-------------------------------------------------------------------------------------
   /// <summary>
   /// Инициализирующий конструктор.
   /// </summary>
   public HasControlEventArgs(int rowIndex, object rowData) : base(rowIndex, rowData) { }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
  }
  /// <summary>
  /// Событие, вызываемое при необходимости определить, если контрол для строки.
  /// </summary>
  public event EventHandler<HasControlEventArgs> HasControl;
  /// <summary>
  /// Вызывает событие HasControl
  /// </summary>
  /// <returns></returns>
  protected internal virtual bool OnHasControl(int rowIndex, object rowData)
  {
   if(HasControl != null)
   {
    HasControlEventArgs args = new HasControlEventArgs(rowIndex, rowData);
    HasControl(this, args);
    return args.HasControl;
   }
   return false;
  }
  #endregion public event EventHandler<HasControlEventArgs> HasControl
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region public event EventHandler<GridRowEventArgs> RowExpanded
  /// <summary>
  /// Класс аргумента событий SimControlsGrid
  /// </summary>
  public class GridRowEventArgs : EventArgs
  {
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Properties >>
   /// <summary>
   /// Индекс строки
   /// </summary>
   public int RowIndex { get; set; }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Объект строки
   /// </summary>
   public object RowData { get; set; }
   #endregion << Properties >>
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   //-------------------------------------------------------------------------------------
   #region << Constructors >>
   /// <summary>
   /// Конструктор по умолчанию.
   /// </summary>
   public GridRowEventArgs() : base() { }
   //-------------------------------------------------------------------------------------
   /// <summary>
   /// Инициализирующий конструктор.
   /// </summary>
   public GridRowEventArgs(int rowIndex, object rowData)
    : base()
   {
    RowIndex = rowIndex;
    RowData = rowData;
   }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
  }
  /// <summary>
  /// Событие, вызываемое при раскрытии строки.
  /// </summary>
  public event EventHandler<GridRowEventArgs> RowExpanded;
  /// <summary>
  /// Вызывает событие RowExpanded
  /// </summary>
  /// <returns></returns>
  protected internal virtual void OnRowExpanded(int rowIndex, object rowData)
  {
   if(RowExpanded != null)
   {
    GridRowEventArgs args = new GridRowEventArgs(rowIndex, rowData);
    RowExpanded(this, args);
   }
  }
  #endregion public event EventHandler<GridRowEventArgs> RowExpanded
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region public event EventHandler<GridRowEventArgs> RowCollapsed
  /// <summary>
  /// Событие, вызываемое при закрытии строки.
  /// </summary>
  public event EventHandler<GridRowEventArgs> RowCollapsed;
  /// <summary>
  /// Вызывает событие RowExpanded
  /// </summary>
  /// <returns></returns>
  protected internal virtual void OnRowCollapsed(int rowIndex, object rowData)
  {
   if(RowCollapsed != null)
   {
    GridRowEventArgs args = new GridRowEventArgs(rowIndex, rowData);
    RowCollapsed(this, args);
   }
  }
  #endregion public event EventHandler<GridRowEventArgs> RowCollapsed
  #endregion << Events >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Отступ контрола
  /// </summary>
  [DefaultValue(17)]
  public int ControlIndent
  {
   get { return cIndent; }
   set { cIndent = value; Refresh(); }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет растягивание контрола по ширине столбцов
  /// </summary>
  [DefaultValue(true)]
  public bool FillControl
  {
   get { return cFill; }
   set { cFill = value; Refresh(); }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет источник данных.
  /// </summary>
  [Category("Data")]
  [Description("Определяет источник данных.")]
  [DefaultValue(null)]
  public new object DataSource
  {
   get { return base.DataSource; }
   set
   {
    if(value != null && value is IBindingList)
     ((IBindingList)value).ListChanged -= new ListChangedEventHandler(DataSource_ListChanged);

    Reset();
    base.DataSource = value;
    if(value is IBindingList)
     ((IBindingList)value).ListChanged += new ListChangedEventHandler(DataSource_ListChanged);
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Закрыто.
  /// </summary>
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public new string DataMember
  {
   get { return null; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Максимальная высота контрола при открытии строки.
   /// </summary>
  [DefaultValue(300)]
  [Description("Максимальная высота контрола при открытии строки.")]
  public int MaxControlHeight
  {
   get { return cMaxHeight; }
   set { cMaxHeight = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет следование за фокусом контрола.
  /// </summary>
  [Description("Определяет следование за фокусом контрола.")]
  [DefaultValue(true)]
  public bool FollowControlFocus
  {
   get { return followControlFocus; }
   set { followControlFocus = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет режим удаления контрола при закрытии строки.
  /// </summary>
  [Description("Определяет режим удаления контрола при закрытии строки.")]
  [DefaultValue(false)]
  public bool RemoveControlOnRowCollapce
  {
   get { return removeControlOnRowCollapce; }
   set { removeControlOnRowCollapce = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Возвращает перечисление контролов, кешированных гридом.
  /// </summary>
  public IEnumerable<Control> CachedControls
  {
   get
   {
    foreach(var i in index.Values)
     if(i.Control != null)
      yield return i.Control;
   }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimControlsGrid() : base()
  {
   InitializeComponent();
   this.DefaultCellStyle.SelectionBackColor = ProfessionalColors.ButtonSelectedHighlight;
   this.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
   this.RowHeadersDefaultCellStyle.SelectionBackColor = ProfessionalColors.ButtonSelectedHighlight;
   this.RowHeadersDefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
  }
  //-------------------------------------------------------------------------------------
  static SimControlsGrid()
  {
   DynamicMethod meth = new DynamicMethod("xxx", null,
    new Type[] { typeof(DataGridViewCellPaintingEventArgs), typeof(DataGridView) },
    typeof(DataGridViewCellPaintingEventArgs)
   );

   FieldInfo fid = typeof(DataGridViewCellPaintingEventArgs).GetField("dataGridView",
       BindingFlags.NonPublic |BindingFlags.Instance
   );

   ILGenerator ilg = meth.GetILGenerator();

   // Load the instance of Example again, load the new value 
   // of id, and store the new field value. 
   ilg.Emit(OpCodes.Ldarg_0);
   ilg.Emit(OpCodes.Ldarg_1);
   ilg.Emit(OpCodes.Stfld, fid);

   // The original value of the id field is now the only 
   // thing on the stack, so return from the call.
   ilg.Emit(OpCodes.Ret);


   setGrid = (SetCellPaintingEventArgsGrid)meth.CreateDelegate(typeof(SetCellPaintingEventArgsGrid));
  }
  //-------------------------------------------------------------------------------------
  private void InitializeComponent()
  {
   ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
   this.SuspendLayout();
   // 
   // SimControlsGrid
   // 
   this.MultiSelect = false;
   this.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
   this.RowHeadersWidth = 18;
   this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
   this.RowTemplate.Height = 18;
   this.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
   this.StandardTab = true;
   ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
   this.ResumeLayout(false);

  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="disposing"></param>
  protected override void Dispose(bool disposing)
  {
   object ds = this.DataSource;
   this.Columns.Clear();
   this.DataSource = null;
   if(ds != null && ds is IDisposable)
    ((IDisposable)ds).Dispose();
   base.Dispose(disposing);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// OnPaint
  /// </summary>
  /// <param name="e"></param>
  protected override void OnPaint(PaintEventArgs e)
  {
   try
   {
    this.SuspendLayout();
    base.OnPaint(e);

    HashSet<Control> has = new HashSet<Control>(this.Controls.Cast<Control>());

    foreach(DataGridViewRow row in this.Rows)
    {
     ControlInfo ci = GetControl(row.Index);
     if(ci == null)
      continue;
     if(row.Displayed == false || ci.Opened == false)
     {
      if(has.Contains(ci.Control))
       this.Controls.Remove(ci.Control);
      continue;
     }
     Rectangle r = this.GetRowDisplayRectangle(row.Index, false);
     r.X += cIndent;
     r.Width = cFill ? r.Width-cIndent : ci.Control.Width;
     r.Y += this.RowTemplate.Height-1;
     r.Height -= this.RowTemplate.Height;

     ci.Control.SetBounds(r.X, r.Y, r.Width, r.Height, BoundsSpecified.All);
     if(has.Contains(ci.Control) == false)
      this.Controls.Add(ci.Control);
     ci.Control.Refresh();
    }
   }
   finally
   {
    this.ResumeLayout();
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// OnCellPainting
  /// </summary>
  /// <param name="e"></param>
  protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
  {
   if(e.RowIndex == -1)
    return;
   if(e.ColumnIndex == -1)
   {
    e.Handled = true;
    if(this.Enabled)
    {
     if(e.State.HasFlag(DataGridViewElementStates.Selected))
      using(SolidBrush b = new SolidBrush(e.CellStyle.SelectionBackColor))
       e.Graphics.FillRectangle( b, e.CellBounds);
     else
      using(SolidBrush b = new SolidBrush(e.CellStyle.BackColor))
       e.Graphics.FillRectangle( b, e.CellBounds);
      
     e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
    }
    else
    {
     e.Graphics.FillRectangle(SystemBrushes.Control, e.CellBounds);
     e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
    }
    
    object data = Rows[e.RowIndex].GetData();
    if(IsHasControl(e.RowIndex, data) == false)
     return;
    ControlInfo ci = GetControl(e.RowIndex, data);
    Image img = Properties.Resources.TreeBtn_Plus;
    if(ci != null && ci.Opened)
     img = Properties.Resources.TreeBtn_Minus;

    Rectangle r = e.CellBounds;
    r.X += Math.Abs(r.Width-img.Width)/2;
    r.Width = img.Width;
    r.Y += Math.Abs(this.RowTemplate.Height - img.Height)/2;
    r.Height = img.Height;
    e.Graphics.DrawImageUnscaled(img, r);
   }
   else 
   {
    ControlInfo ci = GetControl(e.RowIndex);
    if(ci == null || ci.Opened == false)
     return;

    Rectangle r = e.CellBounds;
    using(SolidBrush b = new SolidBrush(e.CellStyle.BackColor))
     e.Graphics.FillRectangle(b, r);

    e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);

    r.Height = this.RowTemplate.Height - 1;
    DataGridViewCellPaintingEventArgs xxx = null;
    xxx = new DataGridViewCellPaintingEventArgs(this, e.Graphics, e.ClipBounds, r, e.RowIndex, e.ColumnIndex,
          e.State, e.Value, e.FormattedValue, e.ErrorText, e.CellStyle,
          new DataGridViewAdvancedBorderStyle()
          {
           Bottom = DataGridViewAdvancedCellBorderStyle.None, Left = e.AdvancedBorderStyle.Left,
           Right = e.AdvancedBorderStyle.Right, Top = e.AdvancedBorderStyle.Top
          }, e.PaintParts ^ DataGridViewPaintParts.Background);

    setGrid(xxx, this);

    xxx.Paint(e.ClipBounds, DataGridViewPaintParts.All);
    e.Handled = true;
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
  {
   base.OnCellMouseDown(e);
   int rowIndex = e.RowIndex;
   Rectangle r = this.RectangleToScreen(this.GetCellDisplayRectangle(e.ColumnIndex, rowIndex, true));
   //r.Y += 3;
   //r.Height -= 6;
   Image img = Properties.Resources.TreeBtn_Plus;
   r.X += Math.Abs(r.Width-img.Width)/2;
   r.Width = img.Width;
   r.Y += Math.Abs(this.RowTemplate.Height - img.Height)/2;
   r.Height = img.Height;
   if(e.ColumnIndex != -1 || rowIndex == -1 || e.Button != System.Windows.Forms.MouseButtons.Left ||
       r.Contains(Control.MousePosition) == false)
    return;
   ControlInfo ci = GetControl(e.RowIndex);
   object data = Rows[e.RowIndex].GetData();
   if(ci == null)
   {
    if(IsHasControl(e.RowIndex, data))
    {
     Control c = OnNeedControl(rowIndex, data);
     if(c == null)
     {
      base.OnCellMouseDown(e);
      return;
     }
     ci = new ControlInfo() { Owner = this, Control = c };
     index.Add(data, ci);
    }
    else
    {
     base.OnCellMouseDown(e);
     return;
    }
   }
   if(ci.Opened)
   {
    ci.Opened = false;
    Rows[rowIndex].MinimumHeight = 2;
    Rows[rowIndex].Height = this.RowTemplate.Height;
    Rows[rowIndex].Resizable = DataGridViewTriState.False;
    OnRowCollapsed(e.RowIndex, data);
    if(removeControlOnRowCollapce)
    {
     this.Controls.Remove(ci.Control);
     ci.OnDelete();
     index.Remove(data);
    }
   }
   else
   {
    int h = ci.Height == 0 ? (ci.Control.Height > cMaxHeight ? cMaxHeight : ci.Control.Height) : ci.Height;
    ci.Opened = true;
    Rows[rowIndex].Height = this.RowTemplate.Height + h;
    Rows[rowIndex].Resizable = this.AllowUserToResizeRows ? DataGridViewTriState.True : DataGridViewTriState.False;
    Rows[rowIndex].MinimumHeight = this.RowTemplate.Height * 2;
    OnRowExpanded(e.RowIndex, data);
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnRowDividerDoubleClick(DataGridViewRowDividerDoubleClickEventArgs e)
  {
   base.OnRowDividerDoubleClick(e);
   ControlInfo ci = GetControl(e.RowIndex);
   if(ci == null || ci.Control == null || ci.Opened == false)
    return;
   this.Rows[e.RowIndex].Height = 
    RowTemplate.Height + 
    (ci.Control.PreferredSize.Height > cMaxHeight ? cMaxHeight : ci.Control.PreferredSize.Height); 
   ci.Height = 0;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnRowHeightInfoPushed(DataGridViewRowHeightInfoPushedEventArgs e)
  {
   ControlInfo ci = GetControl(e.RowIndex);
   if(ci == null || ci.Control == null || ci.Opened == false)
    return;
   ci.Height = e.Height - this.RowTemplate.Height;
   if(ci.Height > cMaxHeight)
   {
    ci.Height = cMaxHeight - this.RowTemplate.Height;
    Rows[e.RowIndex].Height = cMaxHeight;
    e.Handled = true;
   }
   base.OnRowHeightInfoPushed(e);
  }
  //-------------------------------------------------------------------------------------
  void DataSource_ListChanged(object sender, ListChangedEventArgs e)
  {
   try
   {
    if(e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemAdded)
    {
     this.Refresh();
     return;
    }
    HashSet<object> has = new HashSet<object>();
    foreach(DataGridViewRow r in Rows)
    {
     object val = r.GetData();
     has.Add(val);
     if(e.ListChangedType == ListChangedType.ItemDeleted)
      continue;

     ControlInfo ci = GetControl(r.Index, val);
     int h = this.RowTemplate.Height;
     if(ci != null && ci.Opened)
      //h += (ci.Height == 0 ? (ci.Control.Height > cMaxHeight ? cMaxHeight : ci.Control.Height) : ci.Height) + 1;
      h += (ci.Control.Height > cMaxHeight ? cMaxHeight : ci.Control.Height);// + 1;
     if(r.Height != h)
      if(ci != null && ci.Opened)
      {
       r.Height = h;
       r.MinimumHeight = this.RowTemplate.Height * 2;
       r.Resizable = DataGridViewTriState.True;
      }
      else
      {
       r.MinimumHeight = 2;
       r.Height = h;
       r.Resizable = DataGridViewTriState.False;
      }
    }
    this.SuspendLayout();
    foreach(object o in index.Keys.ToArray())
     if(has.Contains(o) == false)
     {
      if(this.Controls.Contains(index[o].Control))
       this.Controls.Remove(index[o].Control);
      index[o].OnDelete();
      index.Remove(o);
     }
    this.ResumeLayout();
    //this.Refresh();

   }
   catch
   {
    
    throw;
   }
  }
  //-------------------------------------------------------------------------------------
  private void ControlSizeChanged(ControlInfo ci)
  {
   if(ci.Opened == false || ci.Height != 0)
    return;
   foreach(DataGridViewRow r in Rows)
    if(GetControl(r.Index) == ci)
    {
     int h = ci.Control.Height > cMaxHeight ? cMaxHeight : ci.Control.Height;
     r.Height = this.RowTemplate.Height + h;
     break;
    }
  }
  //-------------------------------------------------------------------------------------
  private void ControlGotFocus(ControlInfo ci)
  {
   if(followControlFocus == false)
    return;
   foreach(DataGridViewRow r in Rows)
    if(GetControl(r.Index) == ci)
    {
     this.CurrentCell = this[0, r.Index];
     break;
    }
  }
  //-------------------------------------------------------------------------------------
  private void Reset()
  {
   foreach(var i in index)
    i.Value.OnDelete();
   index.Clear();
   this.SuspendLayout();
   List<Control> toDel = new List<Control>();
   foreach(Control c in this.Controls)
   {
    if(c is HScrollBar || c is VScrollBar)
     continue;
    toDel.Add(c);
   }
   foreach(var c in toDel)
    this.Controls.Remove(c);
   this.ResumeLayout();
  }
  //-------------------------------------------------------------------------------------
  private bool IsHasControl(int rowIndex, object data = null)
  {
   if(data == null)
    data = Rows[rowIndex].GetData();
   if(data == null)
    return false;
   return OnHasControl(rowIndex, data);
  }
  private ControlInfo GetControl(int rowIndex, object data = null)
  {
   if(data == null)
    data = Rows[rowIndex].GetData();
   if(data == null)
    return null;
   if(OnHasControl(rowIndex, data) == false)
   {
    if(index.ContainsKey(data))
    {
     index.Remove(data);
     Refresh();
    }
    return null;
   }
   if(index.ContainsKey(data))
    return index[data];
   //{
   // Control c = OnNeedControl(rowIndex, val);
   // index.Add(val, new ControlInfo() { Owner = this, Control = c });
   //}
   return null;
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
  #region << Public Methods >>
  /// <summary>
  /// Возвращает контрол строки.
  /// </summary>
  /// <param name="rowData">Объект данных строки.</param>
  /// <returns></returns>
  public Control GetRowControl(object rowData)
  {
   if(index.ContainsKey(rowData) == false)
    return null;
   return index[rowData].Control;
  }
  ////-------------------------------------------------------------------------------------
  ///// <summary>
  ///// Закрывает строку.
  ///// </summary>
  ///// <param name="rowData">Объект данных строки.</param>
  //public void CloseRow(object rowData)
  //{
   
  //}
  #endregion << Public Methods >>
  //-------------------------------------------------------------------------------------
  //*************************************************************************************
  private class ControlInfo
  {
   internal SimControlsGrid Owner = null;
   private Control c = null;
   public bool Opened = false;
   public int Height = 0;

   public Control Control
   {
    get { return c; }
    set
    {
     c = value;
     if(c != null)
     {
      c.SizeChanged += new EventHandler(c_SizeChanged);
      c.Enter += new EventHandler(c_Enter);
     }
    }
   }

   void c_Enter(object sender, EventArgs e)
   {
    if(Owner != null)
     Owner.ControlGotFocus(this);
   }

   void c_SizeChanged(object sender, EventArgs e)
   {
    if(Owner != null)
     Owner.ControlSizeChanged(this);
   }

   public void OnDelete()
   {
    if(c != null)
    {
     c.SizeChanged -= new EventHandler(c_SizeChanged);
     c.Enter -= new EventHandler(c_Enter);
     c.Dispose();
     c = null;
    }
   }   
  }
 }
}

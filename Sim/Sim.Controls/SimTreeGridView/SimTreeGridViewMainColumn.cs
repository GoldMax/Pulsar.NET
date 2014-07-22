using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Pulsar;

namespace Sim.Controls
{
 /// <summary>
 /// Класс навигационного столбца SimTreeGridView
 /// </summary>
 public class SimTreeGridViewMainColumn : DataGridViewTextBoxColumn
 {
  private bool indentWithoutButton = false;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет наличие сдвига текста узлов без кнопки.
  /// </summary>
  [Description("Определяет наличие сдвига текста узлов без кнопки.")]
  [DefaultValue(false)]
  public bool IndentWithoutButton
  {
   get { return indentWithoutButton; }
   set { indentWithoutButton = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimTreeGridViewMainColumn() : base()
  {
   this.CellTemplate = new SimTreeGridViewMainCell(); 
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Override Methods >>
  /// <summary>
  /// Clone()
  /// </summary>
  /// <returns></returns>
  public override object Clone()
  {
   SimTreeGridViewMainColumn col = (SimTreeGridViewMainColumn)base.Clone();
   col.indentWithoutButton = this.indentWithoutButton;
   return col;
  }
  #endregion << Override Methods >>
  //-------------------------------------------------------------------------------------
          
 }
 //**************************************************************************************
 /// <summary>
 /// Класс ячейки навигационного столбца SimTreeGridView
 /// </summary>
 public class SimTreeGridViewMainCell : DataGridViewTextBoxCell
 {
  private StringFormat stringFormat = new StringFormat();
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimTreeGridViewMainCell() : base()
  {
   stringFormat.LineAlignment = StringAlignment.Center;
   stringFormat.Trimming = StringTrimming.EllipsisCharacter;
   stringFormat.FormatFlags = StringFormatFlags.NoWrap;

  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Override Methods >>
  /// <summary>
  /// Clone()
  /// </summary>
  /// <returns></returns>
  public override object Clone()
  {
   return base.Clone();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="graphics"></param>
  /// <param name="clipBounds"></param>
  /// <param name="cellBounds"></param>
  /// <param name="rowIndex"></param>
  /// <param name="cellState"></param>
  /// <param name="value"></param>
  /// <param name="formattedValue"></param>
  /// <param name="errorText"></param>
  /// <param name="cellStyle"></param>
  /// <param name="advancedBorderStyle"></param>
  /// <param name="paintParts"></param>
  protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
   int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue,
   string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
   DataGridViewPaintParts paintParts)
  {
   paintParts &= ~DataGridViewPaintParts.ContentForeground;
   base.Paint(graphics, clipBounds, cellBounds,
              rowIndex, cellState, value, formattedValue, errorText,
              cellStyle, advancedBorderStyle, paintParts);

   SimTreeGridView treeView = (SimTreeGridView)DataGridView;
   ITreeItem item = treeView.GetItemAt(rowIndex);
   if(item.Object == null)
    return;
   
   Rectangle r = cellBounds;

   DrawTreeElements(graphics, treeView, item, ref r);

   string s = (formattedValue ?? "").ToString();
   if(graphics.MeasureString(s, cellStyle.Font).Width > r.Width)
    this.ToolTipText = s;
   else 
    this.ToolTipText = String.Empty;

   Brush b;
   if(treeView.Enabled == false)
   {
    if((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
     b = new SolidBrush(SystemColors.Control);
    else
     b = new SolidBrush(SystemColors.GrayText);
   }
   else if((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
    b = new SolidBrush(cellStyle.SelectionForeColor);
   else
    b = new SolidBrush(cellStyle.ForeColor);

   #region StringFormat
   StringFormat sf = stringFormat;
   switch(cellStyle.Alignment)
   {
    case DataGridViewContentAlignment.BottomCenter:
     sf.Alignment = StringAlignment.Center;
     sf.LineAlignment = StringAlignment.Far;
     break;
    case DataGridViewContentAlignment.BottomLeft:
     sf.Alignment = StringAlignment.Near;
     sf.LineAlignment = StringAlignment.Far;
     break;
    case DataGridViewContentAlignment.BottomRight:
     sf.Alignment = StringAlignment.Far;
     sf.LineAlignment = StringAlignment.Far;
     break;
    case DataGridViewContentAlignment.MiddleCenter:
     sf.Alignment = StringAlignment.Center;
     sf.LineAlignment = StringAlignment.Center;
     break;
    case DataGridViewContentAlignment.MiddleLeft:
     sf.Alignment = StringAlignment.Near;
     sf.LineAlignment = StringAlignment.Center;
     break;
    case DataGridViewContentAlignment.MiddleRight:
     sf.Alignment = StringAlignment.Far;
     sf.LineAlignment = StringAlignment.Center;
     break;
    case DataGridViewContentAlignment.TopCenter:
     sf.Alignment = StringAlignment.Center;
     sf.LineAlignment = StringAlignment.Near;
     break;
    case DataGridViewContentAlignment.TopLeft:
     sf.Alignment = StringAlignment.Near;
     sf.LineAlignment = StringAlignment.Near;
     break;
    case DataGridViewContentAlignment.TopRight:
     sf.Alignment = StringAlignment.Far;
     sf.LineAlignment = StringAlignment.Near;
     break;
    default: goto case DataGridViewContentAlignment.MiddleLeft;
   }
   #endregion StringFormat

   using(b)
    graphics.DrawString(s, cellStyle.Font, b, r, sf);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
  {
   base.OnMouseClick(e);
   SimTreeGridView treeView = (SimTreeGridView)DataGridView;
   ITreeItem item = treeView.GetItemAt(e.RowIndex); // this.OwningRow.Index);
   SimTreeGridView.NodeInfo ci = treeView.view[item];
   if(ci.hasButton == false || ci.btnRect.Contains(e.Location) == false)
    return;
   if(ci.btnClose)
    treeView.Expand(item);
   else
    treeView.Collapse(item);
   if(item.HasChildren == false)
    DataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Рисует элементы дерева
  /// </summary>
  /// <param name="g"></param>
  /// <param name="treeView"></param>
  /// <param name="item"></param>
  /// <param name="r">Ссылка на прямоугольник ячейки. После рисования, он будет изменен!!!!</param>
  ///// <param name="indentWithoutButton">Определяет, будет ли отступ вместо кнопки, если кнопки нет.</param>
  public void DrawTreeElements(Graphics g, SimTreeGridView treeView, ITreeItem item, ref Rectangle r/*,
                                bool indentWithoutButton = true*/)
  {
   Rectangle cellBounds = r;
   r.X +=2;
   r.Width -= 2;
   r.X += (int)(treeView.Indent * item.Level);
   r.Width -= (int)(treeView.Indent * item.Level);
   SimTreeGridView.NodeInfo ci = treeView.view[item];
   if(treeView.DrawPlusMinus && ci.hasButton && ci.btnRect == Rectangle.Empty)
    ci.btnRect = new Rectangle(r.X - cellBounds.X, r.Y + (r.Height - 9)/2 - cellBounds.Y, 9, 9);

   if(treeView.DrawTreeLines)
   {
    if(item.IsRoot == false)
    {
     int shift = cellBounds.X + 6;
     foreach(ITreeItem i in treeView.Tree.GetParentsItemsList(item))
     {
      if(i.IsRoot)
       continue;
      if(i.Level < item.Level)
       if(treeView.view[i].IsEndItem == false)
        g.DrawLine(SystemPens.ControlDark, shift, r.Y, shift, r.Y + r.Height-1);
      shift = shift + treeView.Indent; // cellBounds.X + 6 + treeView.Indent * i.Level;
     }
     shift = r.X - treeView.Indent + 4;
     if(ci.IsEndItem)
      g.DrawLine(SystemPens.ControlDark, shift, r.Y, shift, r.Y+r.Height/2-1);
     else
      g.DrawLine(SystemPens.ControlDark, shift, r.Y, shift, r.Y + r.Height-1);
     g.DrawLine(SystemPens.ControlDark, shift, r.Y+r.Height/2-1, shift + 9, r.Y+r.Height/2-1);
    }
    if(ci.hasButton && ci.btnClose == false)
    {
     g.DrawLine(SystemPens.ControlDark, r.X + 4, r.Y + r.Height/2, r.X + 4, r.Y + r.Height-1);
    }
   }
   if(treeView.DrawPlusMinus && ci.hasButton)
   {
    if(ci.btnClose)
     g.DrawImage(global::Sim.Controls.Properties.Resources.TreeBtn_Plus, r.X, r.Y + (r.Height - 9)/2);
    else
     g.DrawImage(global::Sim.Controls.Properties.Resources.TreeBtn_Minus, r.X, r.Y + (r.Height - 9)/2);
   }
   if((treeView.DrawPlusMinus && ci.hasButton) || 
       (((SimTreeGridViewMainColumn)base.OwningColumn).IndentWithoutButton && item.Level == 0))
   {
    r.X += 11;
    r.Width -= 11;
   }
  }
  #endregion << Override Methods >>
  //-------------------------------------------------------------------------------------
 }
}

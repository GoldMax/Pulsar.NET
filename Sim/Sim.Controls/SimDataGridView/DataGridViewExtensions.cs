using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс методов расширений для DataGridView
 /// </summary>
 public static class DataGridViewExtensions
 {
  /// <summary>
  /// Возвращает объект, представленный строкой.
  /// </summary>
  /// <returns></returns>
  public static object GetData(this DataGridViewRow row)
  {
   try
   {
    if(row.DataGridView is SimDataGridView && row.Index > -1)
     if(row.Index >= ((IList)((SimDataGridView)row.DataGridView).DataSource).Count)
      return null;
     else
      return ((IList)((SimDataGridView)row.DataGridView).DataSource)[row.Index];
    return row.DataBoundItem;
   }
   catch
   {
    throw;
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает коллекцию столбцов как массив.
  /// </summary>
  /// <returns></returns>
  public static DataGridViewColumn[] ToArray(this DataGridViewColumnCollection cols)
  {
   DataGridViewColumn[] res = new DataGridViewColumn[cols.Count];
   for(int a = 0; a < cols.Count; a++)
    res[a] = cols[a];
   return res;
  }
 }
}

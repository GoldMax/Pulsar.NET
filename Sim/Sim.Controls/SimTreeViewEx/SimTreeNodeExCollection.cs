using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;

using Pulsar;

namespace Sim.Controls
{
 #region class SimTreeNodeExCollection
 //**************************************************************************************
 /// <summary>
 /// Класс коллекции SimTreeNodeEx элементов.
 /// </summary>
 public class SimTreeNodeExCollection :IList<SimTreeNodeEx>, ICollection<SimTreeNodeEx>, IList,
                                      IEnumerable<SimTreeNodeEx>, IEnumerable, ICollection
 {
  SimTreeNodeEx _node = null;
  TreeNodeCollection _col = null;
  private bool sorted = false;
  private Comparison<SimTreeNodeEx> comparer = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет сортировку коллекции.
  /// </summary>
  public bool Sorted
  {
   get { return sorted; }
   set 
   {
    sorted = value;
    foreach(TreeNode node in _col)
     ((SimTreeNodeEx)node).Nodes.Sorted = true;
    if(value == true)
     Sort();
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет метод сравнения двух элементов коллекции при сортировке.
  /// </summary>
  public Comparison<SimTreeNodeEx> Comparer
  {
   get { return comparer; }
   set 
   { 
    comparer = value;
    foreach(TreeNode node in _col)
     ((SimTreeNodeEx)node).Nodes.Comparer = value; 
    if(value != null && sorted)
     Sort(); 
   }
  }
 
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Инициализирующий конструтор.
  /// </summary>
  /// <param name="node"></param>
  internal SimTreeNodeExCollection(SimTreeNodeEx node)
  {
   this._node = node;
   _col = ((TreeNode)_node).Nodes;
  }
  /// <summary>
  /// Инициализирующий конструтор.
  /// </summary>
  /// <param name="nodes"></param>
  internal SimTreeNodeExCollection(TreeNodeCollection nodes)
  {
   _col = nodes;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region ICollection<SimTreeNodeEx> Members
  /// <summary>
  /// Добавляет элемент в коллекцию.
  /// </summary>
  /// <param name="node">Добавляемый элемент.</param>
  public void Add(SimTreeNodeEx node) 
  {
   node.Nodes.Comparer = comparer;
   if(Sorted)
   {
    node.Nodes.Sorted = true;
   
    int a = 0;
    for(; a < _col.Count; a++)
     if(Compare(node, (SimTreeNodeEx)_col[a]) < 0)
     {
      _col.Insert(a, node);
      break;
     }
    if(a == _col.Count)
     _col.Add(node); 
    if(_col.Count < 2 && node.Parent != null) 
    {
     bool isSelect = false;
     if(node.TreeView != null && node.TreeView.SelectedNode != null &&
         node.TreeView.SelectedNode.Equals(node.Parent))
      isSelect = true; 
     CheckSortPosition(node.Parent);
     if(isSelect)
      node.TreeView.SelectedNode = node.Parent;
    } 
   }
   else
    _col.Add(node);
  }
  /// <summary>
  /// Создает элемент и добавляет его в коллекцию.
  /// </summary>
  /// <param name="name">Имя элемента.</param>
  /// <param name="text">Текст метки.</param>
  public SimTreeNodeEx Add(string name, string text)
  {
   SimTreeNodeEx node = new SimTreeNodeEx(name, text);
   node.Nodes.Comparer = comparer;
   if(Sorted)
    node.Nodes.Sorted = true;
   _col.Add(node);
   return node;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Удаляет все элементы из коллекции.
  /// </summary>
  public void Clear() { _col.Clear(); }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Определяет, является ли указанный элемент членом коллекции.
  /// </summary>
  /// <param name="node">Определяемый элемент.</param>
  /// <returns></returns>
  public bool Contains(SimTreeNodeEx node) { return _col.Contains(node); }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Копирует содержимое коллекции в существующий массив в указанную позицию.
  /// </summary>
  /// <param name="array">Массив для копирования.</param>
  /// <param name="arrayIndex">Индекс вставки.</param>
  public void CopyTo(SimTreeNodeEx[] array, int arrayIndex) { _col.CopyTo(array, arrayIndex); }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает число элементов в коллекции.
  /// </summary>
  public int Count 
  {
   get { return _col.Count; }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Определяет, является ли коллекция только для чтения.
  /// </summary>
  public bool IsReadOnly
  {
   get { return _col.IsReadOnly; }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Удаляет указанный элемент из коллекции.
  /// </summary>
  /// <param name="node">Удаляемый элемент.</param>
  /// <returns></returns>
  public bool Remove(SimTreeNodeEx node) { _col.Remove(node); return true; }
  #endregion
  //-------------------------------------------------------------------------------------
  #region IList<SimTreeNodeEx> Members
  /// <summary>
  /// Возвращает индекс элемента в коллекции.
  /// </summary>
  /// <param name="node">Элемент, для которого определяется индекс.</param>
  /// <returns></returns>
  public int IndexOf(SimTreeNodeEx node) { return _col.IndexOf(node); }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Вставляет элемент в указанную позицию.
  /// </summary>
  /// <param name="index">Позиция вставки.</param>
  /// <param name="node">Вставляемый элемент.</param>
  public void Insert(int index, SimTreeNodeEx node) 
  {
   node.Nodes.Comparer = comparer;
   if(Sorted)
    node.Nodes.Sorted = true;
 
   _col.Insert(index, node); 
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Удаляет элемент в указанной позиции.
  /// </summary>
  /// <param name="index">Позиция удаляемого элемента.</param>
  public void RemoveAt(int index) { _col.RemoveAt(index); }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает значение элемента в указанной позиции.
  /// </summary>
  /// <param name="index">Позиция элемента.</param>
  /// <returns></returns>
  public SimTreeNodeEx this[int index]
  {
   get { return (SimTreeNodeEx)_col[index]; }
   set { _col[index] = value; }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает узел по его элементу.
  /// </summary>
  /// <param name="item">Элемент дерева.</param>
  /// <returns></returns>
  public SimTreeNodeEx this[ITreeItem item]
  {
   get 
   { 
    foreach(SimTreeNodeEx node in _col)
     if(node.TreeItem == item)
      return node;
    return null; 
   }
  } 
  #endregion
  //-------------------------------------------------------------------------------------
  #region IEnumerable<SimTreeNodeEx> and IEnumerable Members
  /// <summary>
  /// Возвращает  IEnumerator/<GoldTreeNode/>.
  /// </summary>
  /// <returns></returns>
  public IEnumerator<SimTreeNodeEx> GetEnumerator()
  {
   for (int i = 0; i < _col.Count; i++ )
   {
    yield return (SimTreeNodeEx)_col[i];
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает  IEnumerator.
  /// </summary>
  /// <returns></returns>
  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
  {
   return GetEnumerator();
  }
 
  #endregion
  //-------------------------------------------------------------------------------------
  #region ICollection Members
  int ICollection.Count
  {
   get { return _col.Count; }
  }
  //-------------------------------------------------------------------------------------
  bool ICollection.IsSynchronized
  {
   get { return false ; }
  }
  //-------------------------------------------------------------------------------------
  object ICollection.SyncRoot
  {
   get { return this; }
  }
  //-------------------------------------------------------------------------------------
  void ICollection.CopyTo(Array array, int index)
  {
   throw new Exception("ICollection.CopyTo is not implemented.");
  }
  #endregion ICollection Members
  //-------------------------------------------------------------------------------------
  #region IList Members
  int IList.Add(object value)
  {
   this.Add((SimTreeNodeEx)value);
   return this.IndexOf((SimTreeNodeEx)value);
  }
  //-------------------------------------------------------------------------------------
  void IList.Clear()
  {
   this.Clear();
  }
  //-------------------------------------------------------------------------------------
  bool IList.Contains(object value)
  {
   return this.Contains((SimTreeNodeEx)value);
  }
  //-------------------------------------------------------------------------------------
  int IList.IndexOf(object value)
  {
   return this.IndexOf((SimTreeNodeEx)value);
  }
  //-------------------------------------------------------------------------------------
  void IList.Insert(int index, object value)
  {
   this.Insert(index, (SimTreeNodeEx)value);
  }
  //-------------------------------------------------------------------------------------
  bool IList.IsFixedSize
  {
   get { return false; }
  }
  //-------------------------------------------------------------------------------------
  bool IList.IsReadOnly
  {
   get { return this.IsReadOnly; }
  }
  //-------------------------------------------------------------------------------------
  void IList.Remove(object value)
  {
   this.Remove((SimTreeNodeEx)value); 
  }
  //-------------------------------------------------------------------------------------
  void IList.RemoveAt(int index)
  {
   this.RemoveAt(index);
  }
  //-------------------------------------------------------------------------------------
  object IList.this[int index]
  {
   get { return this[index]; }
   set { this[index] = (SimTreeNodeEx)value;}
  }
  #endregion
  //-------------------------------------------------------------------------------------
  #region Others methods
  /// <summary>
  /// Доюавляет в коллекцию массив ранее созданных элементов.
  /// </summary>
  /// <param name="nodes">Массив ранее созданных элементов.</param>
  public void AddRange(SimTreeNodeEx[] nodes)
  { 
   if(Sorted == false && comparer == null)
    _col.AddRange(nodes); 
   else    
   {
    List<SimTreeNodeEx> list = new List<SimTreeNodeEx>(nodes);
    AddRange(list);
   }
  }
  /// <summary>
  /// Доюавляет в коллекцию массив ранее созданных элементов.
  /// </summary>
  /// <param name="nodes">Список ранее созданных элементов.</param>
  public void AddRange(List<SimTreeNodeEx> nodes)
  {
   if(Sorted == false && comparer == null)
    _col.AddRange(nodes.ToArray()); 
   else
   {
    nodes.Sort(Compare);
   
    foreach(SimTreeNodeEx node in nodes)
    {
     node.Nodes.Comparer = comparer;
     if(Sorted)
      node.Nodes.Sorted = true;
    }  
    _col.AddRange(nodes.ToArray());
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Определяет, является ли узел c указанным элементом членом коллекции.
  /// </summary>
  /// <param name="item">Элемент дерева.</param>
  /// <returns></returns>
  public bool ContainsItem(ITreeItem item) 
  { 
   foreach(SimTreeNodeEx node in _col)
    if(Object.Equals(node.TreeItem, item))
     return true;
   return false;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Ищет узел с указанным элементом.
  /// </summary>
  /// <param name="item">Искомый элемент.</param>
  public SimTreeNodeEx Find(ITreeItem item)
  {
   if(this._node != null && Object.Equals(this._node.TreeItem, item))
    return this._node;
   foreach(SimTreeNodeEx node in _col)
   {
    SimTreeNodeEx n = node.Nodes.Find(item);
    if(n != null)
     return n;
   }
   return null;
  }
  //////-------------------------------------------------------------------------------------
  /////// <summary>
  /////// Возвращает индекс элемента с указанным именем.
  /////// </summary>
  /////// <param name="name">Имя искомого элемента.</param>
  /////// <returns></returns>
  ////public int IndexOfName(string name) { return ((TreeNodeCollection)_col).IndexOfKey(name); }
  //////-------------------------------------------------------------------------------------
  /////// <summary>
  /////// Удаляет элемент с указанным именем.
  /////// </summary>
  /////// <param name="name">Имя удаляемого элемента.</param>
  ////public void RemoveByName(string name) { ((TreeNodeCollection)_col).RemoveByKey(name); }
  #endregion Others methods
  //-------------------------------------------------------------------------------------
  #region Sort Methods
  /// <summary>
  /// Сортирует элементы коллекции.
  /// </summary>
  public void Sort()
  {
   if(_col.Count == 0)
    return;
   SimTreeNodeEx sel = null;
   List<SimTreeNodeEx> list = new List<SimTreeNodeEx>();

   foreach(SimTreeNodeEx node in _col)
   {
    list.Add(node);
    if(node.IsSelected)
     sel = node;
   }
   list.Sort(Compare);
   _col.Clear();
   _col.AddRange(list.ToArray());
   //if(sel != null)
   // sel.S
  }
  //-------------------------------------------------------------------------------------
  private int Compare(SimTreeNodeEx n1, SimTreeNodeEx n2)
  {
   if(comparer != null)
    return comparer(n1, n2);
   else
    return DefaultComparer(n1, n2); 
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Проверяет позицию записи.
  /// </summary>
  /// <param name="node"></param>
  public void CheckSortPosition(SimTreeNodeEx node)
  {
   TreeNodeCollection col = null;
   if(node.Parent != null)
    col = node.Parent.Nodes._col;
   else if(node.TreeView != null)
    col = node.TreeView.Nodes;
   else 
    return;

   for (int a = 0; a < col.Count; a++)
   {
    int res = Compare(node, (SimTreeNodeEx)col[a]);
    if(res < 0)
    {
     col.Remove(node);
     col.Insert(a, node);
     break;
    }
    if(res == 0)
     break;
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Сравнивает два элемента дерева.
  /// </summary>
  /// <param name="x"></param>
  /// <param name="y"></param>
  /// <returns></returns>
  public static int DefaultComparer(SimTreeNodeEx x, SimTreeNodeEx y)
  {
   try
   {
    if(x.TreeItem.IsGroup && y.TreeItem.IsGroup == false)
     return -1;
    if(x.TreeItem.IsGroup == false && y.TreeItem.IsGroup)
     return 1;
    if(x.TreeItem.IsGroup && y.TreeItem.IsGroup)
     return String.Compare(x.Text, y.Text);
    if(x.Nodes.Count > 0 && y.Nodes.Count == 0)
     return -1;
    if(x.Nodes.Count == 0 && y.Nodes.Count > 0)
     return 1;
    return String.Compare(x.Text, y.Text);
   }
   catch
   {
    throw;
   }
  }
  #endregion Sort Methods
 }
 #endregion class SimTreeNodeExCollection 
 //**************************************************************************************
 #region << public class SimTreeNodeExSorter : IComparer>>
 /////<summary>
 /////Класс сортировщика объектов SimTreeNodeEx.
 /////</summary>
 //public class SimTreeNodeExSorter : IComparer, IComparer<SimTreeNodeEx>, IComparer<TreeNode>
 //{
 // #region IComparer Members
 // /// <summary>
 // /// Сравнивает два элемента дерева.
 // /// </summary>
 // /// <param name="x"></param>
 // /// <param name="y"></param>
 // /// <returns></returns>
 // public int Compare(object x, object y)
 // {
 //  return SimTreeNodeExSorter.Compare((SimTreeNodeEx)x, (SimTreeNodeEx)y);
 // }
 // #endregion IComparer Members

 // #region IComparer<SimTreeNodeEx> Members
 // /// <summary>
 // /// Сравнивает два элемента дерева.
 // /// </summary>
 // /// <param name="x"></param>
 // /// <param name="y"></param>
 // /// <returns></returns>
 // int IComparer<SimTreeNodeEx>.Compare(SimTreeNodeEx x, SimTreeNodeEx y)
 // {
 //  return SimTreeNodeExSorter.Compare(x, y);
 // }
 // #endregion
  
 // #region IComparer<TreeNode> Members
 // /// <summary>
 // /// Сравнивает два элемента дерева.
 // /// </summary>
 // /// <param name="x"></param>
 // /// <param name="y"></param>
 // /// <returns></returns>
 // int IComparer<TreeNode>.Compare(TreeNode x, TreeNode y)
 // {
 //  return SimTreeNodeExSorter.Compare((SimTreeNodeEx)x, (SimTreeNodeEx)y);
 // }
 // #endregion  IComparer<TreeNode> Members
  
  
 //}
 #endregion << public class GoldTreeNodeSorter >>

}

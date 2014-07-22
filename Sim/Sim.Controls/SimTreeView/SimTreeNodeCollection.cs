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
	#region class SimTreeNodeCollection
	//**************************************************************************************
	/// <summary>
	/// ����� ��������� SimTreeNode ���������.
	/// </summary>																									 
	public class SimTreeNodeCollection : IList<SimTreeNode>, ICollection<SimTreeNode>, IList,
																																									IEnumerable<SimTreeNode>, IEnumerable, ICollection
	{
		SimTreeNode _node = null;
		TreeNodeCollection _col = null;
		private bool sorted = false;
		private Comparison<SimTreeNode> comparer = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ���������� ���������� ���������.
		/// </summary>
		public bool Sorted
		{
			get { return sorted; }
			set
			{
				sorted = value;
				foreach (TreeNode node in _col)
					((SimTreeNode)node).Nodes.Sorted = true;
				if (value == true)
					Sort();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����� ��������� ���� ��������� ��������� ��� ����������.
		/// </summary>
		public Comparison<SimTreeNode> Comparer
		{
			get { return comparer; }
			set
			{
				comparer = value;
				foreach (TreeNode node in _col)
					((SimTreeNode)node).Nodes.Comparer = value;
				if (value != null && sorted)
					Sort();
			}
		}

		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ���������������� ����������.
		/// </summary>
		/// <param name="node"></param>
		internal SimTreeNodeCollection(SimTreeNode node)
		{
			this._node = node;
			_col = ((TreeNode)_node).Nodes;
		}
		/// <summary>
		/// ���������������� ����������.
		/// </summary>
		/// <param name="nodes"></param>
		internal SimTreeNodeCollection(TreeNodeCollection nodes)
		{
			_col = nodes;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region ICollection<SimTreeNode> Members
		/// <summary>
		/// ��������� ������� � ���������.
		/// </summary>
		/// <param name="node">����������� �������.</param>
		public void Add(SimTreeNode node)
		{
			node.Nodes.Comparer = comparer;
			if (Sorted)
			{
				node.Nodes.Sorted = true;

				int a = 0;
				for (; a < _col.Count; a++)
					if (Compare(node, (SimTreeNode)_col[a]) < 0)
					{
						_col.Insert(a, node);
						break;
					}
				if (a == _col.Count)
					_col.Add(node);
				if (_col.Count < 2 && node.Parent != null)
				{
					bool isSelect = false;
					if (node.TreeView != null && node.TreeView.SelectedNode != null &&
									node.TreeView.SelectedNode.Equals(node.Parent))
						isSelect = true;
					CheckSortPosition(node.Parent);
					if (isSelect)
						node.TreeView.SelectedNode = node.Parent;
				}
			}
			else
				_col.Add(node);
		}
		/// <summary>
		/// ������� ������� � ��������� ��� � ���������.
		/// </summary>
		/// <param name="name">��� ��������.</param>
		/// <param name="text">����� �����.</param>
		public SimTreeNode Add(string name, string text)
		{
			SimTreeNode node = new SimTreeNode(name, text);
			node.Nodes.Comparer = comparer;
			if (Sorted)
				node.Nodes.Sorted = true;
			_col.Add(node);
			return node;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ��� �������� �� ���������.
		/// </summary>
		public void Clear() { _col.Clear(); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����������, �������� �� ��������� ������� ������ ���������.
		/// </summary>
		/// <param name="node">������������ �������.</param>
		/// <returns></returns>
		public bool Contains(SimTreeNode node) { return _col.Contains(node); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// �������� ���������� ��������� � ������������ ������ � ��������� �������.
		/// </summary>
		/// <param name="array">������ ��� �����������.</param>
		/// <param name="arrayIndex">������ �������.</param>
		public void CopyTo(SimTreeNode[] array, int arrayIndex) { _col.CopyTo(array, arrayIndex); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ����� ��������� � ���������.
		/// </summary>
		public int Count
		{
			get { return _col.Count; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����������, �������� �� ��������� ������ ��� ������.
		/// </summary>
		public bool IsReadOnly
		{
			get { return _col.IsReadOnly; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ��������� ������� �� ���������.
		/// </summary>
		/// <param name="node">��������� �������.</param>
		/// <returns></returns>
		public bool Remove(SimTreeNode node) { _col.Remove(node); return true; }
		#endregion
		//-------------------------------------------------------------------------------------
		#region IList<SimTreeNode> Members
		/// <summary>
		/// ���������� ������ �������� � ���������.
		/// </summary>
		/// <param name="node">�������, ��� �������� ������������ ������.</param>
		/// <returns></returns>
		public int IndexOf(SimTreeNode node) { return _col.IndexOf(node); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ������� � ��������� �������.
		/// </summary>
		/// <param name="index">������� �������.</param>
		/// <param name="node">����������� �������.</param>
		public void Insert(int index, SimTreeNode node)
		{
			node.Nodes.Comparer = comparer;
			if (Sorted)
				node.Nodes.Sorted = true;

			_col.Insert(index, node);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ������� � ��������� �������.
		/// </summary>
		/// <param name="index">������� ���������� ��������.</param>
		public void RemoveAt(int index) { _col.RemoveAt(index); }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� �������� �������� � ��������� �������.
		/// </summary>
		/// <param name="index">������� ��������.</param>
		/// <returns></returns>
		public SimTreeNode this[int index]
		{
			get { return (SimTreeNode)_col[index]; }
			set { _col[index] = value; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ���� �� ��� ��������.
		/// </summary>
		/// <param name="item">������� ������.</param>
		/// <returns></returns>
		public SimTreeNode this[ITreeItem item]
		{
			get
			{
				foreach (SimTreeNode node in _col)
					if (node.TreeItem == item)
						return node;
				return null;
			}
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IEnumerable<SimTreeNode> and IEnumerable Members
		/// <summary>
		/// ����������  IEnumerator/<GoldTreeNode/>.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<SimTreeNode> GetEnumerator()
		{
			for (int i = 0; i < _col.Count; i++)
			{
				yield return (SimTreeNode)_col[i];
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����������  IEnumerator.
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
			get { return false; }
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
			this.Add((SimTreeNode)value);
			return this.IndexOf((SimTreeNode)value);
		}
		//-------------------------------------------------------------------------------------
		void IList.Clear()
		{
			this.Clear();
		}
		//-------------------------------------------------------------------------------------
		bool IList.Contains(object value)
		{
			return this.Contains((SimTreeNode)value);
		}
		//-------------------------------------------------------------------------------------
		int IList.IndexOf(object value)
		{
			return this.IndexOf((SimTreeNode)value);
		}
		//-------------------------------------------------------------------------------------
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (SimTreeNode)value);
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
			this.Remove((SimTreeNode)value);
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
			set { this[index] = (SimTreeNode)value; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region Others methods
		/// <summary>
		/// ��������� � ��������� ������ ����� ��������� ���������.
		/// </summary>
		/// <param name="nodes">������ ����� ��������� ���������.</param>
		public void AddRange(SimTreeNode[] nodes)
		{
			if (Sorted == false && comparer == null)
				_col.AddRange(nodes);
			else
			{
				List<SimTreeNode> list = new List<SimTreeNode>(nodes);
				AddRange(list);
			}
		}
		/// <summary>
		/// ��������� � ��������� ������ ����� ��������� ���������.
		/// </summary>
		/// <param name="nodes">������ ����� ��������� ���������.</param>
		public void AddRange(List<SimTreeNode> nodes)
		{
			if (Sorted == false && comparer == null)
				_col.AddRange(nodes.ToArray());
			else
			{
				nodes.Sort(Compare);

				foreach (SimTreeNode node in nodes)
				{
					node.Nodes.Comparer = comparer;
					if (Sorted)
						node.Nodes.Sorted = true;
				}
				_col.AddRange(nodes.ToArray());
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����������, �������� �� ���� c ��������� ��������� ������ ���������.
		/// </summary>
		/// <param name="item">������� ������.</param>
		/// <returns></returns>
		public bool ContainsItem(ITreeItem item)
		{
			foreach (SimTreeNode node in _col)
				if (Object.Equals(node.TreeItem, item))
					return true;
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���� ���� � ��������� ���������.
		/// </summary>
		/// <param name="item">������� �������.</param>
		public SimTreeNode Find(ITreeItem item)
		{
			if (this._node != null && Object.Equals(this._node.TreeItem, item))
				return this._node;
			foreach (SimTreeNode node in _col)
			{
				SimTreeNode n = node.Nodes.Find(item);
				if (n != null)
					return n;
			}
			return null;
		}
		//////-------------------------------------------------------------------------------------
		/////// <summary>
		/////// ���������� ������ �������� � ��������� ������.
		/////// </summary>
		/////// <param name="name">��� �������� ��������.</param>
		/////// <returns></returns>
		////public int IndexOfName(string name) { return ((TreeNodeCollection)_col).IndexOfKey(name); }
		//////-------------------------------------------------------------------------------------
		/////// <summary>
		/////// ������� ������� � ��������� ������.
		/////// </summary>
		/////// <param name="name">��� ���������� ��������.</param>
		////public void RemoveByName(string name) { ((TreeNodeCollection)_col).RemoveByKey(name); }
		#endregion Others methods
		//-------------------------------------------------------------------------------------
		#region Sort Methods
		/// <summary>
		/// ��������� �������� ���������.
		/// </summary>
		public void Sort()
		{
			if (_col.Count == 0)
				return;
			SimTreeNode sel = null;
			List<SimTreeNode> list = new List<SimTreeNode>();

			foreach (SimTreeNode node in _col)
			{
				list.Add(node);
				if (node.IsSelected)
					sel = node;
			}
			list.Sort(Compare);
			_col.Clear();
			_col.AddRange(list.ToArray());
			//if(sel != null)
			// sel.S
		}
		//-------------------------------------------------------------------------------------
		private int Compare(SimTreeNode n1, SimTreeNode n2)
		{
			if (comparer != null)
				return comparer(n1, n2);
			else
				return SimTreeNodeSorter.Default.Compare(n1, n2);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ������� ������.
		/// </summary>
		/// <param name="node"></param>
		public void CheckSortPosition(SimTreeNode node)
		{
			TreeNodeCollection col = null;
			if (node.Parent != null)
				col = node.Parent.Nodes._col;
			else if (node.TreeView != null)
				col = node.TreeView.Nodes;
			else
				return;

			for (int a = 0; a < col.Count; a++)
			{
				int res = Compare(node, (SimTreeNode)col[a]);
				if (res < 0)
				{
					col.Remove(node);
					col.Insert(a, node);
					break;
				}
				if (res == 0)
					break;
			}
		}
		#endregion Sort Methods
	}
	#endregion class SimTreeNodeCollection
	//**************************************************************************************
	#region << public class SimTreeNodeSorter : IComparer>>
	///<summary>
	///����� ������������ �������� SimTreeNode.
	///</summary>
	public class SimTreeNodeSorter : IComparer<SimTreeNode>, IComparer<ITreeItem>
	{
		public static SimTreeNodeSorter Default = new SimTreeNodeSorter();

		#region IComparer<SimTreeNode> Members
		/// <summary>
		/// ���������� ��� �������� ������.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(SimTreeNode x, SimTreeNode y)
		{
			return Compare(x.TreeItem, y.TreeItem);
		}
		#endregion

		#region IComparer<TreeNode> Members
		/// <summary>
		/// ���������� ��� �������� ������.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(ITreeItem x, ITreeItem y)
		{
			if (x.IsGroup && y.IsGroup == false)
				return -1;
			if (x.IsGroup == false && y.IsGroup)
				return 1;
			if (x.HasChildren && y.HasChildren == false)
				return -1;
			if (x.HasChildren == false && y.HasChildren)
				return 1;

			return String.Compare(x.ItemText, y.ItemText);

			//if(x.IsGroup && y.IsGroup == false)
			// return -1;
			//if(x.IsGroup == false && y.IsGroup)
			// return 1;
			//if(x.IsGroup && y.IsGroup)
			//{
			// if(x.SortOrder == y.SortOrder)
			//  return String.Compare(x.ItemText, y.ItemText);
			// else
			//  return x.SortOrder.CompareTo(y.SortOrder);
			//}
			//if(x.HasChildren && y.HasChildren == false)
			// return -1;
			//if(x.HasChildren == false && y.HasChildren)
			// return 1;
			//if(x.SortOrder == y.SortOrder)
			// return String.Compare(x.ItemText, y.ItemText);
			//else
			// return x.SortOrder.CompareTo(y.SortOrder);
		}
		#endregion  IComparer<TreeNode> Members
	}
	#endregion << public class GoldTreeNodeSorter >>
}

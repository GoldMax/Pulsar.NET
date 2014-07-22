using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar        
{
	#region << public enum PulsarFilterConditions >>
	/// <summary>
	/// ������������ ������� �������.
	/// </summary>
	[Serializable]
	public enum PulsarFilterConditions
	{
		/// <summary>
		/// � ������
		/// </summary>
		In,
		/// <summary>
		/// �� � ������
		/// </summary>
		NotIn,
		/// <summary>
		/// ���������� ����� ��������� �����.
		/// </summary>
		CustomFiltering
	}
	#endregion << public enum PulsarFilterConditions >>
	//**************************************************************************************
	#region << public interface IPulsarBinder :  IList >>
	/// <summary>
	/// ������� ��������� ������� �������� ������.
	/// </summary>
	public interface IPulsarBinder : IList, ITypedList, IBindingList, IBindingListView, IDisposable
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ������� ���������.
		/// </summary>
		ICollection Collection { get; }
		/// <summary>
		/// ����������, �������� �� ������ ���������������.
		/// </summary>
		bool IsFiltered { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, �������������� �� �������� �� ����������.
		/// </summary>
		bool IsBindOff { get; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Methods >>
		/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="propName">������������ ��������, �� �������� ��������������� ����������.</param>
		/// <param name="condition">������� ����������.</param>
		/// <param name="variant">������� �������� ��������.</param>
		void ApplyFilter(string propName, PulsarFilterConditions condition, object variant);
		/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="propName">������������ ��������, �� �������� ��������������� ����������.</param>
		/// <param name="condition">������� ����������.</param>
		/// <param name="variants">������ ��������� �������� ��������.</param>
		void ApplyFilter(string propName, PulsarFilterConditions condition, IEnumerable variants);
				/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="filterMethod">����� ���������� ������. ����� ���: bool Method&lt;T&gt; (T obj)</param>
		void ApplyFilter(Func<object, bool> filterMethod);
			/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="condition">������� ����������.</param>
		/// <param name="variants">������ ��������� �������� ������.</param>
		void ApplyFilter(PulsarFilterConditions condition, IEnumerable variants);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ������� ������ ��������� ���� ��������� ������.
		/// </summary>
		Comparison<object> Comparer { get; set; }
		/// <summary>
		/// ��������� �������� ������ �� �������
		/// </summary>
		void Sort();
		/// <summary>
		/// ��������� ���������� ��� ���������� ����� ��������.
		/// </summary>
		/// <param name="property">��� ������������ ��������.</param>
		/// <param name="direction">���������� ����������.</param>
		void Sort(string property, ListSortDirection direction);
		/// <summary>
		/// ��������� ������, ��������� ��������� ����� ��������� ��������� ������.
		/// ����� ���������� �� ������������, � ������ �� ��������� � ������������� ���������.
		/// </summary>
		/// <param name="comparison">������� ������ ��������� ���������.</param>
		void Sort(Comparison<object> comparison);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ������.
		/// </summary>
		void Refresh(bool needListEvent = true);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �������� �� ����������.
		/// </summary>
		void BindOff(bool needListEvent = true);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������������� �������� �� ����������.
		/// </summary>
		void BindOn(bool needListEvent = true);
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	} 
	#endregion << public interface IPulsarListBinder :  IList >>
	//**************************************************************************************
	/// <summary>
	/// ����� �������� ������� � ���������. 
	/// ��� ���������� � ���������� �� ������������ ������� ������.
	/// </summary>
	public sealed class ListBinder : IList, IPulsarBinder, ITypedList, IBindingList, 
																																					IBindingListView, IDisposable
	{
		private IList _list = null;
		private List<ListSortDescription> sort = new List<ListSortDescription>();
		private ValuesTrio<PropertyDescriptor, PulsarFilterConditions, IEnumerable> filter = null;
		private PList<ActionPropertyDescriptor> pseudos = new PList<ActionPropertyDescriptor>(0);
		
		[NonSerialized]
		private Comparison<object> comparer = null;
		[NonSerialized]
		private Func<object,bool> filterMethod = null;
		[NonSerialized]
		private bool isBindOff = false;
		[NonSerialized]
		private Type itemType = null;

		List<int> index = null;
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		[NonSerialized]
		private ListChangedEventHandler listChanged = null;
		/// <summary>
		/// �������, ����������� ��� ��������� ��������� ������.
		/// </summary>
		public event ListChangedEventHandler ListChanged
		{
			add { listChanged += value; }
			remove { listChanged -= value; }
		}
		/// <summary>
		/// �������� ������� ListChanged.
		/// </summary>
		/// <param name="e"></param>
		public void OnListChanged(ListChangedEventArgs e)
		{
			if(listChanged != null && isBindOff == false)
				listChanged(this, e);
		}
		#endregion << Events >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Own Properties >>
		/// <summary>
		/// ������� ������.
		/// </summary>
		public IList List
		{
			get { return _list; }
			set 
			{
				if(value == null)
				 throw new ArgumentNullException("this");
			 _list = value;
				RefreshInternal();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		ICollection IPulsarBinder.Collection
		{
			get { return List; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, �������������� �� �������� �� ����������.
		/// </summary>
		public bool IsBindOff { get { return isBindOff; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������ ������-�������.
		/// </summary>
		public PList<ActionPropertyDescriptor> PseudoProps
		{
			get { return pseudos; }
		}
		#endregion << Own Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ */
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		private ListBinder() 
		{ 
			CacheSort = true; 
			pseudos.ItemAdded += (s,e) => 
				OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorAdded, (PropertyDescriptor)e.Item));
			pseudos.ItemChanged += (s,e) => 
				OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, (PropertyDescriptor)e.Item));
			pseudos.ItemDeleted += (s,e) => 
				OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorDeleted, (PropertyDescriptor)e.Item));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="list">������� ������.</param>
		public ListBinder(IList list) : this()
		{ 
			if(list == null)
				throw new ArgumentNullException("list");
			_list = list;
			if(_list is ICollectionChangeNotify)
			{
				((ICollectionChangeNotify)_list).ItemAdded += Binder_ItemAdded;
				((ICollectionChangeNotify)_list).ItemChanged += Binder_ItemChanged;
				((ICollectionChangeNotify)_list).ItemDeleted += Binder_ItemDeleted;
				//((ICollectionChangeNotify)_list).ObjectChanging += Binder_ObjectChanging;
				((ICollectionChangeNotify)_list).ObjectChanged += Binder_ObjectChanged;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="list">������� ������.</param>
		/// <param name="itemType">��� �������� ������. ������������ ��� ����������� ������������ �������.</param>
		public ListBinder(IList list, Type itemType)
		{
			if(list == null)
				throw new ArgumentNullException("list");
			_list = list;
			this.itemType = itemType;
			if(_list is ICollectionChangeNotify)
			{
				((ICollectionChangeNotify)_list).ItemAdded += Binder_ItemAdded;
				((ICollectionChangeNotify)_list).ItemChanged += Binder_ItemChanged;
				((ICollectionChangeNotify)_list).ItemDeleted += Binder_ItemDeleted;
				//((ICollectionChangeNotify)_list).ObjectChanging += Binder_ObjectChanging;
				((ICollectionChangeNotify)_list).ObjectChanged += Binder_ObjectChanged;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Dispose()
		/// </summary>
		public void Dispose()
		{
			if(_list is ICollectionChangeNotify)
			{
				((ICollectionChangeNotify)_list).ItemAdded -= Binder_ItemAdded;
				((ICollectionChangeNotify)_list).ItemChanged -= Binder_ItemChanged;
				((ICollectionChangeNotify)_list).ItemDeleted -= Binder_ItemDeleted;
				//((ICollectionChangeNotify)_list).ObjectChanging -= Binder_ObjectChanging;
				((ICollectionChangeNotify)_list).ObjectChanged -= Binder_ObjectChanged;
			}
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Own Methods >>
		private void CreateIndex()
		{
			index = new List<int>(_list.Count+1);
			for(int a = 0; a < _list.Count; a++)
				index.Add(a);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ������.
		/// </summary>
		public void Refresh(bool needListEvent = true)
		{
			RefreshInternal();
			if(needListEvent)
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		private void RefreshInternal()
		{
			if(IsFiltered == false)
				index = null;
			else if(filter.Value2 == PulsarFilterConditions.CustomFiltering && filterMethod == null)
			{
				filter = null;
				filterMethod = null;
				index = null;
			}
			else
			{
				if(index == null)
					index = new List<int>();
				else
					index.Clear();
				for(int a = 0; a < _list.Count; a++)
					if(PassFilterCheck(_list[a]))
						index.Add(a);
			}
			if(IsSorted)
				SortInternal();
		}
		//-------------------------------------------------------------------------------------
		void Binder_ObjectChanging(object sender, ChangeNotifyEventArgs args)
		{
			//if(isBindOff == false)
			// BindOff();
		}
		void Binder_ObjectChanged(object sender, ChangeNotifyEventArgs args)
		{
			if(isBindOff == false && args.Action == ChangeNotifyAction.ObjectReset)
			{
				RefreshInternal();
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
			}
		}
		//-------------------------------------------------------------------------------------
		void Binder_ItemAdded(object sender, CollectionChangeNotifyEventArgs args)
		{
			if(isBindOff)
				return;
			if(args.ItemIndex == -1)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
				return;
			} 
			if(index == null)
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, args.ItemIndex));
			else
			{  
				int pos = -1;
				if(IsFiltered && PassFilterCheck(_list[args.ItemIndex]) == false)
					pos = -1;
				else
				{
					for(int a = 0; a < index.Count; a++)
						if(index[a] >= args.ItemIndex)
							index[a]++;
					if(IsSorted == false)
					{
						index.Add(args.ItemIndex);
						pos = index.Count-1;
					}
					else
					{
						pos = FindSortPosition(args.ItemIndex);
						index.Insert(pos, args.ItemIndex);
					}
				}
				if(pos == -1 && index.Contains(args.ItemIndex))
					RefreshInternal();
				if(pos != -1)
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, pos)); 
			}
		}
		//-------------------------------------------------------------------------------------
		void Binder_ItemDeleted(object sender, CollectionChangeNotifyEventArgs args)
		{
			if(isBindOff)
				return;
			if(args.ItemIndex == -1)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
				return;
			} 
			if(index == null)
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, args.ItemIndex));
			else
			{
				int pos = index.IndexOf(args.ItemIndex);
				if(pos == -1)
					return;
				index.RemoveAt(pos);
				for(int i = 0; i < index.Count; i++)
					if(index[i] > args.ItemIndex)
						index[i]--;
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, pos));   
			} 
		}
		//-------------------------------------------------------------------------------------
		void Binder_ItemChanged(object sender, CollectionChangeNotifyEventArgs args)
		{
			if(isBindOff)
				return;
			if(args.ItemIndex == -1)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
				return;
			} 
			if(index == null)
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, args.ItemIndex));
			else
			{
				int pos = index.IndexOf(args.ItemIndex);
				if(IsFiltered)
				{
					bool pass = PassFilterCheck(_list[args.ItemIndex]);
					if(pass == false && pos > -1)
					{
						this.index.RemoveAt(pos);
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, pos));
						return;
					}
					if(pass && pos == -1)
					{
						this.index.Add(args.ItemIndex);
						pos = index.IndexOf(args.ItemIndex);
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, pos));
					}
				}
				if(IsSorted && pos > -1 )
				{
					int p = pos;
					index.RemoveAt(pos);
					pos = FindSortPosition(args.ItemIndex);
					index.Insert(pos, args.ItemIndex);
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, pos, p));
				}
				else
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
			}
		}
		#endregion << Own Methods >>
		//-------------------------------------------------------------------------------------
		#region << IPulsarBinder Methods >>
		/// <summary>
		/// ���������������� �������� �� ����������
		/// </summary>
		public void BindOff(bool needListEvent = true)
		{
			if(isBindOff)
				return;

			isBindOff = true;
			if(needListEvent && listChanged != null)
				listChanged(this, new ListChangedEventArgs(ListChangedType.Reset,0)); 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������������� �������� �� ����������.
		/// </summary>
		public void BindOn(bool needListEvent = true)
		{
			if(isBindOff == false)
				return;
			RefreshInternal();
			isBindOff = false;
			if(needListEvent)
				//OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, 0));
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		#endregion << IPulsarBinder Methods >>
		//-------------------------------------------------------------------------------------
		#region << Sort Methods >>
		/// <summary>
		/// ����������, �������� �� ������ �������������.
		/// </summary>
		public bool IsSorted
		{
			get { return sort.Count > 0 || comparer != null; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������� ������ ��������� ���� ��������� ������.
		/// </summary>
		public Comparison<object> Comparer
		{
			get { return comparer; }
			set { comparer = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ ����������. ���� true, ����������� ������ ���������� ����� ����������� (������ ��������),
		/// ���� false, �������� ������� ��������, ����� Reflection.
		/// </summary>
		public bool CacheSort { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ���������� ������.
		/// </summary>
		bool IBindingList.SupportsSorting
		{
			get { return true; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		bool IBindingListView.SupportsAdvancedSorting
		{
			get { return true; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ����������� ����������.
		/// </summary>
		ListSortDirection IBindingList.SortDirection
		{
			get { return sort.Count == 0 ? ListSortDirection.Ascending : sort[0].SortDirection; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ���������� ��������, �� �������� ��������� ����������.
		/// </summary>
		PropertyDescriptor IBindingList.SortProperty
		{
			get { return sort.Count == 0 ? null : sort[0].PropertyDescriptor; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������� ����������.
		/// </summary>
		public ListSortDescriptionCollection SortDescriptions
		{
			get { return new ListSortDescriptionCollection(sort.ToArray()); }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ���������� ��� ���������� ����� ��������.
		/// </summary>
		/// <param name="property">��� ������������ ��������.</param>
		/// <param name="direction">���������� ����������.</param>
		public void Sort(string property, ListSortDirection direction)
		{
			Type t = itemType;
			if(t == null)
			{
				t = List.GetType();
				if(t.IsGenericType == false)
					throw new Exception("����� Sort �� ���������� �������� �� �������������� ��� �� generic ���������!");
				t = t.GetGenericArguments()[0];
			}

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(t); 
			if(props.Count == 0)
				throw new Exception("����� �������� ������ �� ����� ��������� �������!");
			PropertyDescriptor d = props.Find(property, false);
			if(d == null)
				throw new ArgumentException(String.Format("� ���� \"{1}\" ����������� ��������� �������� \"{0}\"!", property,
																																															t.FullName), "property");
			((IBindingList)this).ApplySort(d, direction);
		}
		//-------------------------------------------------------------------------------------
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			comparer = null;
			ListSortDescription lsd = new ListSortDescription(property, direction);
			int pos = sort.FindIndex(delegate(ListSortDescription l)
																													{
																														return l.PropertyDescriptor.Name == lsd.PropertyDescriptor.Name;
																													});
			if(pos == -1)
			{
				sort.Clear();
				sort.Add(lsd);
			}
			else
				sort[pos] = lsd;
			
			Sort();
		}
		void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
		{
			sort.Clear();
			foreach(ListSortDescription lsd in sorts)
			{
				if(lsd == null)
					continue;
				sort.Add(lsd);
			}
			Sort();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� �������� ������ �� �������
		/// </summary>
		public void Sort()
		{
			SortInternal();
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� �������� ������ �� �������
		/// </summary>
		private void SortInternal()
		{
			if(IsSorted == false)
				return;
			if(index == null)
				CreateIndex();
			if(comparer == null)
				SortIndex(new Comparison<int>(DefaultComparier));
			else
				index.Sort(delegate(int x, int y) { return comparer(_list[x], _list[y]); });
		}
		//-------------------------------------------------------------------------------------
		private int FindSortPosition(int listIndex)
		{
			int pos = -1;
			for(int i = 0; i < index.Count; i++)
				if((comparer == null ? DefaultComparier(index[i], listIndex) : comparer(_list[index[i]], _list[listIndex])) > 0)
				{
					pos = i;
					break;
				}
			if(pos == -1)
				pos = index.Count;
			return pos;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ������, ��������� ��������� ����� ��������� ��������� ������.
		/// ����� ���������� �� ������������, � ������ �� ��������� � ������������� ���������.
		/// </summary>
		/// <param name="comparison">������� ������ ��������� ���������.</param>
		public void Sort(Comparison<object> comparison)
		{
			if(CacheSort)
				throw new Exception("������ ����������� ������ ��������� ������� ��������� ���������, " + 
																									"��� ��� ������� ����� ������������ ����������.");
			this.comparer = comparison;
			SortInternal();
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		private void SortIndex(Comparison<int> comparison)
		{
			if(CacheSort)
			{
				if(comparison != DefaultComparier)
					throw new Exception("������ ����������� ������ ��������� ������� ��������� ���������, " + 
																									"��� ��� ������� ����� ������������ ����������.");

				List<object[]> l = new List<object[]>(index.Count);
				foreach(int key in index)
				{
					object[] val = new object[sort.Count+1];
					val[val.Length-1] = key;
					for(int a = 0; a < sort.Count; a++)
						val[a] = sort[a].PropertyDescriptor.GetValue(_list[key]);
					l.Add(val);
				}
				l.Sort(RealValsComparer);
				for(int a = 0; a < l.Count; a++)
					index[a] = (int)l[a][sort.Count];
			}
			else
				index.Sort(comparison);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����� ��������� ��������� ��� reflection ����������
		/// </summary>
		private int DefaultComparier(int index1, int index2)
		{
			foreach(ListSortDescription lsd in sort)
			{
				//Trace.TraceTime.BeginTrace();
				object x = lsd.PropertyDescriptor.GetValue(_list[index1]);
				object y = lsd.PropertyDescriptor.GetValue(_list[index2]);
				//Trace.TraceTime.Reset();

				int sign = lsd.SortDirection == ListSortDirection.Ascending ? 1 : -1;
				int res = 0;

				if(x == null && y == null)
					return 0;
				else if(x == null)
					return -1 * sign;
				else if(y == null)
					return sign;
				else if(x is IComparable)
					res = ((IComparable)x).CompareTo(y);
				else
					res = StringComparer.CurrentCulture.Compare(x.ToString(),
																																																	y.ToString());
				if(res != 0)
					return sign * res;
			}
			return 0;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����� ��������� ��������� ��� ������������ ����������
		/// </summary>
		private int RealValsComparer(object X, object Y)
		{
			for(int a = 0; a < sort.Count; a++)
			{
				object x = ((object[])X)[a];
				object y = ((object[])Y)[a];
			
				ListSortDescription lsd =  sort[a];
				int sign = lsd.SortDirection == ListSortDirection.Ascending ? 1 : -1;
				int res = 0;

				if(x == null && y == null)
					return 0;
				else if(x == null)
					return -1 * sign;
				else if(y == null)
					return sign;
				else if(x is IComparable)
					res = ((IComparable)x).CompareTo(y);
				else
					res = StringComparer.CurrentCulture.Compare(x.ToString(),
																																																	y.ToString());
				if(res != 0)
					return sign * res;
			}
			return 0;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ���������� ������.
		/// </summary>
		public void RemoveSort()
		{
			sort.Clear();
			comparer = null;
			RefreshInternal();
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		#endregion << Sort Methods >>
		//-------------------------------------------------------------------------------------
		#region << Filter Methods >>
		/// <summary>
		/// ����������, �������� �� ������ ���������������.
		/// </summary>
		public bool IsFiltered
		{
			get { return filter != null; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="propName">������������ ��������, �� �������� ��������������� ����������.</param>
		/// <param name="condition">������� ����������.</param>
		/// <param name="variant">������� �������� ��������.</param>
		public void ApplyFilter(string propName, PulsarFilterConditions condition, object variant)
		{
			if(variant is string == false &&  variant is IEnumerable)
				ApplyFilter(propName, condition, (IEnumerable)variant);
			else
			{ 
				object[] vars = new object[1] { variant };
				ApplyFilter(propName, condition, vars);
			} 
		}
		/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="propName">������������ ��������, �� �������� ��������������� ����������.</param>
		/// <param name="condition">������� ����������.</param>
		/// <param name="variants">������ ��������� �������� ��������.</param>
		public void ApplyFilter(string propName, PulsarFilterConditions condition, IEnumerable variants)
		{
			if(variants is string)
				variants = new object[1] { variants };

			filterMethod = null;

				
			Type t = itemType;
			if(t == null)
			{
				t = List.GetType();
				if(t.IsGenericType == false)
					throw new Exception("����� Sort �� ���������� �������� �� �������������� ��� �� generic ���������!");
				t = t.GetGenericArguments()[0];
			}

			PropertyDescriptor d = null;
			if(t.IsValueType || t == typeof(String))
				d = new PrimitiveValuePropertyDescriptor(t);
			else
			{
				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(t);
				if(props.Count == 0)
					throw new Exception("����� �������� ������ �� ����� ��������� �������!");
				d = props.Find(propName, false);
			}
			if(d == null)
				throw new ArgumentException(String.Format("� ���� \"{1}\" ����������� ��������� �������� \"{0}\"!", propName,
																																															t.FullName), "propName");
			filter = new ValuesTrio<PropertyDescriptor, PulsarFilterConditions, IEnumerable>(d, condition,
				variants ?? new List<object>());
			Refresh();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="condition">������� ����������.</param>
		/// <param name="variants">������ ��������� �������� ������.</param>
		public void ApplyFilter(PulsarFilterConditions condition, IEnumerable variants)
		{
			filter = new ValuesTrio<PropertyDescriptor, PulsarFilterConditions, IEnumerable>(null, condition,
				variants ?? new List<object>());
			Refresh();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������������� ���������� ������.
		/// </summary>
		/// <param name="filterMethod">����� ���������� ������. ����� ���: bool Method(object obj)</param>
		public void ApplyFilter(Func<object,bool> filterMethod)
		{
			if(filterMethod == null)
				RemoveFilter();
			else
			{
				this.filterMethod = filterMethod;
				filter = new ValuesTrio<PropertyDescriptor, PulsarFilterConditions, IEnumerable>(null,
						PulsarFilterConditions.CustomFiltering, null);
				Refresh();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ������ �� ������.
		/// </summary>
		public void RemoveFilter()
		{
			filter = null;
			filterMethod = null;
			Refresh();
		}
		//-------------------------------------------------------------------------------------
		private bool PassFilterCheck(object item)
		{
			if(filter.Value2 == PulsarFilterConditions.CustomFiltering)
				return filterMethod(item);
			object obj = item;
			if(filter.Value1 != null)
				obj = filter.Value1.GetValue(item);
			foreach(object var in filter.Value3)
				if(Object.Equals(var,obj))
					return (filter.Value2 == PulsarFilterConditions.In);
			return !(filter.Value2 == PulsarFilterConditions.In);
		}
		#endregion << Filter Methods >>
		//-------------------------------------------------------------------------------------
		#region IBindingList Members
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		//-------------------------------------------------------------------------------------
		object IBindingList.AddNew()
		{
			Type t = List.GetType();
			if(t.IsGenericType == false)
				return null;
			t = t.GetGenericArguments()[0];
			ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
			if(ci == null)
				return null;
			object item = ci.Invoke(null);
			_list.Add(item);
			return item;
		}
		//-------------------------------------------------------------------------------------
		bool IBindingList.AllowEdit
		{
			get { return true; }
		}
		//-------------------------------------------------------------------------------------
		bool IBindingList.AllowNew
		{
			get { return true; }
		}
		//-------------------------------------------------------------------------------------
		bool IBindingList.AllowRemove
		{
			get { return true; }
		}
		//-------------------------------------------------------------------------------------
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			return this.IndexOf(property, key);   
		}
		//-------------------------------------------------------------------------------------
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		//-------------------------------------------------------------------------------------
		bool IBindingList.SupportsChangeNotification
		{
			get { return true; }
		}
		//-------------------------------------------------------------------------------------
		bool IBindingList.SupportsSearching
		{
			get { return true; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IList Members
		/// <summary>
		/// ���������� � ������������� ������� �� ���������� �������.
		/// </summary>
		/// <param name="index">������ ��������.</param>
		/// <returns></returns>
		public object this[int index]
		{
			get { return this.index == null ? _list[index] : _list[this.index[index]]; }
			set 
			{
				if(this.index == null)
					_list[index] = value;
				else
					_list[this.index[index]] = value;
				if(_list is ICollectionChangeNotify == false)
					Binder_ItemChanged(_list, new CollectionChangeNotifyEventArgs(this,ChangeNotifyAction.ItemChange,null,_list[index], null,index));
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// IndexOf
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(object item)
		{
			return this.index == null ? _list.IndexOf(item) : this.index.IndexOf(_list.IndexOf(item));
		}
		/// <summary>
		/// ���������� ������ �������� � ������, � �������� ������� �������� ������������� ��������.
		/// </summary>
		/// <param name="propertyName">��� ��������.</param>
		/// <param name="value">��������.</param>
		/// <returns></returns>
		public int IndexOf(string propertyName, object value)
		{
			Type t = itemType;
			if(t == null)
			{
				t = List.GetType();
				if(t.IsGenericType == false)
					throw new Exception("����� IndexOf �� ����� �������� �� �������������� ��� �� generic ���������!");
				t = t.GetGenericArguments()[0];
			}

			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(t);
			if(props.Count == 0)
				throw new Exception("����� �������� ��������� �� ����� ��������� �������!");
			PropertyDescriptor d = props.Find(propertyName, false);
			if(d == null)
				throw new ArgumentException(String.Format("� ���� \"{1}\" ����������� ��������� �������� \"{0}\"!",
					propertyName, t.FullName), "propertyName");
			return IndexOf(d,value);
		}
		private int IndexOf(PropertyDescriptor property, object value)
		{
			if(index == null)
			{
				for(int a = 0; a < _list.Count; a++)
					if(property.GetValue(_list[a]).Equals(value))
						return a;
			}
			else
			{
				for(int a = 0; a < index.Count; a++)
					if(property.GetValue(_list[index[a]]).Equals(value))
						return a;
			}
			return -1;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ��������� ������ � ��������� ������� ������ (�� ������� !!!).
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, object value)
		{
			int pos = index;
			_list.Insert(index, value);
			if(_list is ICollectionChangeNotify == false)
				Binder_ItemAdded(_list, new CollectionChangeNotifyEventArgs(this,ChangeNotifyAction.ItemAdd,null,_list[index],null, pos));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ������ �� ��������� ������� ������ (�� ������� !!!).
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
			if(_list is ICollectionChangeNotify == false)
				Binder_ItemDeleted(_list, new CollectionChangeNotifyEventArgs(this,ChangeNotifyAction.ItemDelete,null, _list[index],null,index));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// IList.IsReadOnly
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// IList.IsFixedSize
		/// </summary>
		public bool IsFixedSize
		{
			get { return false; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ��������� ������� � ��������� �������� ���������� � �������.
		/// </summary>
		/// <param name="item">����������� �������.</param>
		/// <returns>���������� ������� �������� � ������.</returns>
		public int Add(object item)
		{
			_list.Add(item);
			int pos = _list.Count-1;
			if(_list is ICollectionChangeNotify == false)
				Binder_ItemAdded(_list, new CollectionChangeNotifyEventArgs(this,ChangeNotifyAction.ItemChange,null,_list[pos],null, pos));
			return pos;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ������� ������.
		/// </summary>
		public void Clear()
		{
			_list.Clear();
			index = null;
			if(_list is ICollectionChangeNotify == false)
				Binder_ObjectChanged(_list, null);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Contains
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(object item)
		{
			return index == null ? _list.Contains(item) : index.Contains(_list.IndexOf(item));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Remove
		/// </summary>
		/// <param name="value"></param>
		public void Remove(object value)
		{
			int pos = _list.IndexOf(value);
			if(pos != -1)
			{
				_list.Remove(value);
				if(_list is ICollectionChangeNotify == false)
					Binder_ItemDeleted(_list, new CollectionChangeNotifyEventArgs(this,ChangeNotifyAction.ItemChange,null, value,null,pos));
			}
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region ICollection Members
		/// <summary>
		/// ICollection.CopyTo
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(Array array, int arrayIndex)
		{
			if(index == null)
				_list.CopyTo(array, arrayIndex);
			else
				for(int a = 0; a < index.Count; a++)
					array.SetValue(_list[index[a]], a + arrayIndex);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ICollection.Count
		/// </summary>
		public int Count
		{
			get 
			{
				if(isBindOff)
					return 0; 
				return index == null ? _list.Count : index.Count; 
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		object ICollection.SyncRoot
		{
			get { return null; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IEnumerable Members
		/// <summary>
		/// GetEnumerator()
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			if(index == null)
				for(int a = 0; a < _list.Count; a++)
					yield return _list[a];
			else
				//return new IE
				for(int a = 0; a < index.Count; a++)
					yield return _list[index[a]];
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region << Own Methods >>
		/// <summary>
		/// AddRange
		/// </summary>
		/// <param name="collection"></param>
		public void AddRange(IEnumerable collection)
		{
			foreach(object i in collection)
				_list.Add(i);
			if(_list is ICollectionChangeNotify == false)
				Binder_ObjectChanged(_list, null);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� ������� �� ����� ������� � �������.
		/// </summary>
		/// <param name="item">������������ �������.</param>
		/// <param name="newIndex">����� ������� � ������.</param>
		public void MoveItem(object item, int newIndex)
		{
			if(IsSorted)
				return;
			
			if(index == null)
				CreateIndex();

			int listIndex = _list.IndexOf(item);
			int pos = index.IndexOf(listIndex);
			if(pos == -1)
				throw new ArgumentException("������� �� ����������� ������!", "item");
			if(newIndex < 0 || newIndex > index.Count)
				throw new ArgumentException("�������� ������� �� ������� ������!", "newIndex");
			index.RemoveAt(pos);
			index.Insert(newIndex, listIndex);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, newIndex, pos));
		}
		#endregion << Own Methods >>
		//-------------------------------------------------------------------------------------
		#region ITypedList Members
		/// <summary>
		/// 
		/// </summary>
		/// <param name="listAccessors"></param>
		/// <returns></returns>
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			if(List == null)
				return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
			Type t = itemType;
			if(t == null)
			{
				t = List.GetType();
				if(t.IsGenericType)
					t = t.GetGenericArguments()[0];
				else if(t.BaseType != null && t.BaseType.IsGenericType)
					t = t.BaseType.GetGenericArguments()[0];
				else if(t.GetInterface("IList`1", true) != null)
					t = t.GetInterface("IList`1", true).GetGenericArguments()[0];
				else
					t = typeof(object);
			}

			//if(t.IsInterface && List.Count > 0)
			// t = List[0].GetType();
			
			List<PropertyDescriptor> col = new List<PropertyDescriptor>();
			col.AddRange(pseudos);

			foreach(var i in TypeDescriptor.GetProperties(t))
				col.Add((PropertyDescriptor)i);

			if(t.IsPrimitive || t == typeof(string) || t.IsEnum || t == typeof(object))
				col.Add(new PrimitiveValuePropertyDescriptor(t));


			return new PropertyDescriptorCollection(col.ToArray());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="listAccessors"></param>
		/// <returns></returns>
		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		#endregion ITypedList Members
		//-------------------------------------------------------------------------------------
		#region IBindingListView Members
		string IBindingListView.Filter
		{
			get { return null; }
			set { throw new NotImplementedException(); }
		}
		//-------------------------------------------------------------------------------------
		bool IBindingListView.SupportsFiltering
		{
			get { return false; }
		}
		#endregion
		//**************************************************************************************
	}

}

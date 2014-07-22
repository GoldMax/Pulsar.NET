using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar
{
	/// <summary>
	/// Класс коллекции ключ/значение с возможностью биндинга.
	/// </summary>
	public sealed class DictionaryBinder : IDictionary, IPulsarBinder, ITypedList, IBindingList,
																																																					IBindingListView, IDisposable, IList
	{
		private IDictionary _dic = null;
		private List<object> _index = null;

		[NonSerialized]
		private Comparison<object> _sortMethod = null;
		[NonSerialized]
		private Func<object,bool> _filterMethod = null;

		private List<ListSortDescription> _sorts = null;
	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		private event ListChangedEventHandler listChanged;
		/// <summary>
		/// Событие, возникающее при изменении элементов списка.
		/// </summary>
		public event ListChangedEventHandler ListChanged
		{
			add { listChanged += value; }
			remove { listChanged -= value; }
		}
		/// <summary>
		/// Вызывает событие ListChanged.
		/// </summary>
		/// <param name="e"></param>
		public void OnListChanged(ListChangedEventArgs e)
		{
			if(listChanged != null && IsBindOff == false)
				listChanged(this, e);
		}
		#endregion << Events >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает несущую коллекцию.
		/// </summary>
		public IDictionary Dic
		{
			get { return _dic; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		ICollection IPulsarBinder.Collection
		{
			get { return Dic; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будут ли ключи добавлены как свойства (столбец).
		/// </summary>
		public bool KeyAsProperties { get; set; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		private DictionaryBinder()
		{
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="dic">Несущая коллекция.</param>
		public DictionaryBinder(IDictionary dic) : this()
		{
			this._dic = dic;
			CreateIndex();
			if(dic is ICollectionChangeNotify)
			{
				((ICollectionChangeNotify)dic).ItemAdded	+= Binder_ItemAdded;
				((ICollectionChangeNotify)dic).ItemChanged += Binder_ItemChanged;
				((ICollectionChangeNotify)dic).ItemDeleted += Binder_ItemDeleted;
			}
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="dic">Несущая коллекция.</param>
		/// <param name="keyAsProp">Определяет, будут ли ключи добавлены как свойства (столбец).</param>
		public DictionaryBinder(IDictionary dic, bool keyAsProp) : this(dic)
		{
			KeyAsProperties = keyAsProp;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Dispose()
		/// </summary>
		public void Dispose()
		{
			_index = null;
			if(_dic is ICollectionChangeNotify)
			{
				((ICollectionChangeNotify)_dic).ItemAdded	-= Binder_ItemAdded;
				((ICollectionChangeNotify)_dic).ItemChanged -= Binder_ItemChanged;
				((ICollectionChangeNotify)_dic).ItemDeleted -= Binder_ItemDeleted;
			}
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Own Methods >>
		private void CreateIndex()
		{
			_index = new List<object>(_dic.Count);
			foreach(object e in _dic)
				_index.Add(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обновляет индекс.
		/// </summary>
		public void Refresh(bool needListChanged = true)
		{
			if(_index == null)
				_index = new List<object>();
			else
				_index.Clear();
			foreach(object kv in _dic)
				if(_filterMethod == null || _filterMethod(kv))	//		KeyAsProperties ? key : dic[key]
					_index.Add(kv);
			if(IsSorted)
				ApplySort(_sortMethod);
			if(needListChanged)
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		//-------------------------------------------------------------------------------------
		private int FindSortPosition(object kv)
		{
			int pos = -1;
			for(int i = 0; i < _index.Count; i++)
				if(_sortMethod(_index[i], kv) > 0)
				{
					pos = i;
					break;
				}
			if(pos == -1)
				pos = _index.Count;
			return pos;
		}
		//-------------------------------------------------------------------------------------
		void Binder_ItemDeleted(object sender, CollectionChangeNotifyEventArgs args)
		{
			if(IsBindOff)
				return;
			int pos = _index.IndexOf(args.Item);
			if(pos == -1)
				return;
			_index.RemoveAt(pos);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, pos));
		}
		void Binder_ItemChanged(object sender, CollectionChangeNotifyEventArgs args)
		{
			if(IsBindOff)
				return;
			if(args.Action == ChangeNotifyAction.ObjectReset)
			{
			 Refresh();
				return;
			}

			int pos = _index.IndexOf(args.Item);
			if( pos > -1 && _filterMethod != null && _filterMethod(args.Item) == false)
			{
				this._index.RemoveAt(pos);
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, pos));
				return;
			}
			if(IsSorted && pos > -1)
			{
				int p = pos;
				_index.RemoveAt(pos);
				pos = FindSortPosition(args.Item);
				_index.Insert(pos, args.Item);
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, pos, p));
			}
			else if(pos != -1)
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
		}
		void Binder_ItemAdded(object sender, CollectionChangeNotifyEventArgs args)
		{
			if(IsBindOff)
				return;
			int pos = -1;
			if(_filterMethod == null || _filterMethod(args.Item))
			{
				if(_index.Contains(args.Item))
					_index.Remove(args.Item);
				if(IsSorted == false)
				{
					_index.Add(args.Item ?? args);
					pos = _index.Count-1;
				}
				else
				{
					pos = FindSortPosition(args.Item);
					_index.Insert(pos, args.Item);
				}
			}
			if(pos == -1 && _index.Contains(args.Item))
				Refresh();
			if(pos == -1)
				return;
			else
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, pos));
		}
		#endregion << Own Methods >>
		//-------------------------------------------------------------------------------------
		#region << IPulsarBinder Methods >>
		/// <summary>
		/// Определяет, приостановлено ли слежение за источником.
		/// </summary>
		public bool IsBindOff { get; private set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Приостанавливает слежение за источником
		/// </summary>
		public void BindOff(bool needListEvent = true)
		{
			if(IsBindOff)
				return;
			IsBindOff = true;
			if(needListEvent && listChanged != null)
				listChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвобнавляет слежение за источником.
		/// </summary>
		public void BindOn(bool needListEvent = true)
		{
			if(IsBindOff == false)
				return;
			IsBindOff = false;
			Refresh(false);
			if(needListEvent)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, 0));
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset,0)); 
			}
		}
		#endregion << IPulsarBinder Methods >>
		//-------------------------------------------------------------------------------------
		#region Filter Members
		/// <summary>
		/// Определяет, применен ли фильтр.
		/// </summary>
		public bool IsFiltered
		{
			get { return _filterMethod != null; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает фильтрацию.
		/// </summary>
		/// <param name="filterMethod">Метод фильтрации данных. Имеет вид: bool Method(object obj)</param>
		public void ApplyFilter(Func<object, bool> filterMethod)
		{
			if(filterMethod == null)
				RemoveFilter();
			else
			{
				this._filterMethod = filterMethod;
				Refresh();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Снимает фильтр со списка.
		/// </summary>
		public void RemoveFilter()
		{
			_filterMethod = null;
			Refresh();
		}
		#endregion Filter Members
		//-------------------------------------------------------------------------------------
		#region Sort Members
		/// <summary>
		/// Определяет, применена ли сортировка.
		/// </summary>
		public bool IsSorted
		{
			get { return _sortMethod != null; }
		}
		/// <summary>
		/// Устанавливает сортировку, используя указанный метод сравнения элементов списка.
		/// </summary>
		/// <param name="comparison">Делегат метода сравнения элементов.</param>
		public void ApplySort(Comparison<object> comparison)
		{
		 this._sortMethod = comparison;
			if(_index == null)
				CreateIndex();
			_index.Sort(_sortMethod);
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		/// <summary>
		/// Снимает сортировку.
		/// </summary>
		public void RemoveSort()
		{
			_sortMethod = null;
			_sorts = null;
			Refresh();
		}
		private int DefaultSorter(object X, object Y)
		{
			foreach(ListSortDescription lsd in _sorts)
			{
				//Trace.TraceTime.BeginTrace();
				object x = lsd.PropertyDescriptor.GetValue(X);
				object y = lsd.PropertyDescriptor.GetValue(Y);
				//Trace.TraceTime.Reset();

				int sign = lsd.SortDirection == ListSortDirection.Ascending ? 1 : -1;
				int res = 0;

				if(x == null && y == null)
					continue;
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
		#endregion
		//-------------------------------------------------------------------------------------
		#region IDictionary Members
		/// <summary>
		/// Добавляет элемент в коллекцию согласно сортировке и фильтру.
		/// </summary>
		public void Add(object key, object value)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //


			_dic.Add(key, value);
			if(_dic is ICollectionChangeNotify)
			 return;
			object kv = null;
			foreach(dynamic i in	_dic)
				if(Object.Equals(i.Key, key))
				{
					kv = i;
					break;
				}
			if(kv == null)
			 throw new Exception("Элемент не был добавлен в словарь!");
			int pos = -1;
			if(_filterMethod == null || _filterMethod(kv))
				if(IsSorted == false)
				{
					_index.Add(kv);
					pos = _index.Count -1;
				}
				else
				{
					pos = FindSortPosition(kv);
					_index.Insert(pos, kv);
				}
			if(pos != -1)
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, pos));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Очищает список.
		/// </summary>
		public void Clear()
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			_dic.Clear();
			if(_dic is ICollectionChangeNotify)
				return;
			_index.Clear();
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// IDictionary.Contains
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(object key)
		{
			return _index.Contains(key);
		}
		//-------------------------------------------------------------------------------------
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return (IDictionaryEnumerator)this.GetEnumerator();
		}
		//-------------------------------------------------------------------------------------
		bool IDictionary.IsFixedSize
		{
			get { return _dic.IsFixedSize; }
		}
		//-------------------------------------------------------------------------------------
		bool IDictionary.IsReadOnly
		{
			get { return _dic.IsReadOnly; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает ключи согласно фильтру и сортировке
		/// </summary>
		public ICollection Keys
		{
			get 
			{  
			 List<object> res = new List<object>(_index.Count);
			 foreach(dynamic i in _index)
				 res.Add(i.Key);
				return res;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает значения согласно фильтру и сортировке
		/// </summary>
		public ICollection Values
		{
			get 
			{
				List<object> res = new List<object>(_index.Count);
				foreach(dynamic i in _index)
					res.Add(i.Value);
				return res;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Remove
		/// </summary>
		/// <param name="key"></param>
		public void Remove(object key)
		{
			int pos = -1;
			for(int a = 0; a < _index.Count; a++)
			{
			 dynamic i = _index[a];
				if(Object.Equals(i.Key, key))
				{
					pos = a;
					break;
				}
			}
			_dic.Remove(key);

			if(_dic is ICollectionChangeNotify)
				return;

			if(pos > -1)
			{
				_index.RemoveAt(pos);
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, pos));
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает и устанавливает элемент по указанному ключу.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		public object this[object key]
		{
			get { return _dic[key]; }
			set
			{
				//--- Debbuger Break --- //
				if(System.Diagnostics.Debugger.IsAttached)
					System.Diagnostics.Debugger.Break();
				//--- Debbuger Break --- //


				_dic[key] = value;
				if(_dic is ICollectionChangeNotify)
					return;

				int pos = -1;
				object kv = null;
				for(int a = 0; a < _index.Count; a++)
				{
					dynamic i = _index[a];
					if(Object.Equals(i.Key, key))
					{
						pos = a;
						kv = i;
						break;
					}
				}

				if(_filterMethod != null)
					if(_filterMethod(kv))
					{
						if(pos == -1)
						{
							if(IsSorted)
								pos = FindSortPosition(kv); 
							else
								pos = _index.Count;
							_index.Insert(pos,kv);
							pos = _index.Count -1;
						 OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, pos));
							return;
						} 
					}
					else
						if(pos != -1)
						{
							_index.Remove(kv);
							OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, pos));
							return;
						}  

				if(IsSorted)
				{
					int p = pos;
					pos = FindSortPosition(key);
					if(p!= pos)
					{
						_index.RemoveAt(p);
						_index.Insert(pos, key);
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, pos, p));
						return;
					}
				}
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
			}
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region ICollection Members
		void ICollection.CopyTo(Array array, int index)
		{
			for(int a = 0; a < this._index.Count; a++)
				array.SetValue(_index[a], a + index);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает количество элементов согласно фильтру.
		/// </summary>
		public int Count
		{
			get { return IsBindOff || _index == null ? 0 : _index.Count; }
		}
		//-------------------------------------------------------------------------------------
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		//-------------------------------------------------------------------------------------
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
			return _index.GetEnumerator();
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IBindingList Members
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		object IBindingList.AddNew()
		{
			//Type t = Dic.GetType();
			//if(t.IsGenericType == false)
			// return null;
			//t = t.GetGenericArguments()[1];
			//ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
			//if(ci == null)
			// return null;
			//object item = ci.Invoke(null);
			//dic.Add(item);
			//return item;
			return null;
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
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			ListSortDescription lsd = new ListSortDescription(property, direction);
			if(_sorts == null)
			 _sorts = new List<ListSortDescription>();
			int pos = _sorts.FindIndex(delegate(ListSortDescription l)
			{
				return l.PropertyDescriptor.Name == lsd.PropertyDescriptor.Name;
			});
			if(pos == -1)
			{
				_sorts.Clear();
				_sorts.Add(lsd);
			}
			else
				_sorts[pos] = lsd;
			ApplySort(DefaultSorter);
		}
		//-------------------------------------------------------------------------------------
		int IBindingList.Find(PropertyDescriptor property, object value)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return -1;
		}
		//-------------------------------------------------------------------------------------
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		ListSortDirection IBindingList.SortDirection
		{
			get { return _sorts == null || _sorts.Count == 0 ? ListSortDirection.Ascending : _sorts[0].SortDirection; }
		}
		//-------------------------------------------------------------------------------------
		PropertyDescriptor IBindingList.SortProperty
		{
			get { return _sorts == null || _sorts.Count == 0 ? null : _sorts[0].PropertyDescriptor; }
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
		//-------------------------------------------------------------------------------------
		bool IBindingList.SupportsSorting
		{
			get { return true; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IBindingListView Members
		void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
		{
			if(_sorts == null)
			 _sorts = new List<ListSortDescription>();
			else
			 _sorts.Clear();
			foreach(ListSortDescription lsd in sorts)
			{
				if(lsd == null)
					continue;
				_sorts.Add(lsd);
			}
			ApplySort(DefaultSorter);
		}
		//-------------------------------------------------------------------------------------
		string IBindingListView.Filter
		{
			get { return null; }
			set { throw new NotImplementedException(); }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Условия сортировки.
		/// </summary>
		public ListSortDescriptionCollection SortDescriptions
		{
			get { return _sorts == null ? null : new ListSortDescriptionCollection(_sorts.ToArray()); }
		}
		//-------------------------------------------------------------------------------------
		bool IBindingListView.SupportsAdvancedSorting
		{
			get { return true; }
		}
		//-------------------------------------------------------------------------------------
		bool IBindingListView.SupportsFiltering
		{
			get { return false; }
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region IList Members
		int IList.Add(object value)
		{
			throw new NotImplementedException("Метод IList.Add не поддерживается!");
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает позицию элемета в индексе.
		/// </summary>
		/// <param name="key">Элемент.</param>
		/// <returns></returns>
		public int IndexOf(object key)
		{
			return _index.IndexOf(key);
		}
		/// <summary>
		/// Удаляет элемент по его индексу. 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			_dic.Remove(((dynamic)this._index[index]).Key);
			this._index.RemoveAt(index);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}
		object IList.this[int index]
		{
			get { return _index[index]; }
			set { throw new NotImplementedException(); }
		}
		bool IList.IsFixedSize
		{
			get { return _dic.IsFixedSize; }
		}
		bool IList.IsReadOnly
		{
			get { return _dic.IsReadOnly; }
		}
		void IList.Insert(int index, object value)
		{
			throw new NotImplementedException();
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region ITypedList Members
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection col = new PropertyDescriptorCollection(new PropertyDescriptor[0], false);
		 Type t = _dic.GetType();
			if(t.IsGenericType == false)
			{
			 if(KeyAsProperties)
					col.Add(new DicEntryPropertyDescriptor(typeof(object), "Key", true));
				col.Add(new DicEntryPropertyDescriptor(typeof(object), "Value", false));
				return col;
			}
			Type[] types = t.GetGenericArguments();
			if(KeyAsProperties)
			{
				if(types[0].IsPrimitive || types[0] == typeof(string) || types[0] == typeof(System.Object))
					col.Add(new DicEntryPropertyDescriptor(types[0],"Key",true));
				else
					foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(types[0]))
						if(pd.IsBrowsable)
							col.Add(new DicEntryPropertyDescriptor(pd, true));  // pd.ComponentType
			}
			if(types[1].IsPrimitive || types[1] == typeof(string) || types[1] == typeof(System.Object))
				col.Add(new DicEntryPropertyDescriptor(types[1], "Value", false));
			else
				foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(types[1]))
					if(pd.IsBrowsable)
						col.Add(new DicEntryPropertyDescriptor(pd, false));  // pd.ComponentType
			return col;

		}
		//-------------------------------------------------------------------------------------
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			throw new NotImplementedException();
		}
		#endregion
		//**************************************************************************************
		#region << class DicEntryPropertyDescriptor : PrimitiveValuePropertyDescriptor >>
		class DicEntryPropertyDescriptor : PropertyDescriptor
		{
			bool isKey = false;
			Type propType = null;
			PropertyDescriptor pd = null;
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			public DicEntryPropertyDescriptor(Type propType, string name, bool isKey)
				: base(name, new Attribute[0])
			{
				this.isKey = isKey;
				this.propType = propType;
			}
			public DicEntryPropertyDescriptor(PropertyDescriptor pd, bool isKey)
				: base(pd.Name, new Attribute[0])
			{
				this.isKey = isKey;
				this.pd = pd;
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
			public override string DisplayName
			{
				get
				{
					return pd == null ? base.DisplayName :  pd.DisplayName;
				}
			}

			public override object GetValue(object component)
			{
			 object k,v;
			 if(component is IKeyedValue)
				{
					k = ((IKeyedValue)component).Key;
					v = ((IKeyedValue)component).Value;
				}
				else if(component is DictionaryEntry)
				{
					k = ((DictionaryEntry)component).Key;
					v = ((DictionaryEntry)component).Value;
				}
				else
				 throw new Exception("Тип элемента списка неопознан!");
				if(isKey)
				 return pd == null ? k : pd.GetValue(k);
				else
					return pd == null ? v : pd.GetValue(v);
			}

			public override bool CanResetValue(object component)
			{
				throw new NotImplementedException();
			}

			public override Type ComponentType
			{
				get { throw new NotImplementedException(); }
			}

			public override bool IsReadOnly
			{
				get { return pd.IsReadOnly; }
			}

			public override Type PropertyType
			{
				get { return pd == null ? propType : pd.PropertyType; }
			}

			public override void ResetValue(object component)
			{
				throw new NotImplementedException();
			}

			public override void SetValue(object component, object value)
			{
				if(pd == null)
					throw new NotImplementedException("Для DicEntryPropertyDescriptor не указан дескриптор свойсва!");
				pd.SetValue(((IKeyedValue)component).Value, value);
			}

			public override bool ShouldSerializeValue(object component)
			{
				throw new NotImplementedException();
			}

		} 
		#endregion << class DicEntryPropertyDescriptor : PrimitiveValuePropertyDescriptor >>

		#region IPulsarBinder Members
		Comparison<object> IPulsarBinder.Comparer
		{
			get { return _sortMethod; }
			set	{ _sorts = null; ApplySort(value); }
		}
		void IPulsarBinder.ApplyFilter(string propName, PulsarFilterConditions condition, object variant)
		{
			throw new NotImplementedException();
		}
		void IPulsarBinder.ApplyFilter(string propName, PulsarFilterConditions condition, IEnumerable variants)
		{
			throw new NotImplementedException();
		}
		void IPulsarBinder.ApplyFilter(PulsarFilterConditions condition, IEnumerable variants)
		{
			throw new NotImplementedException();
		}
		void IPulsarBinder.Sort()
		{
			_sorts = null;
			ApplySort(DefaultSorter);
		}
		void IPulsarBinder.Sort(string property, ListSortDirection direction)
		{
			throw new NotImplementedException();
		}
		void IPulsarBinder.Sort(Comparison<object> comparison)
		{
			_sorts = null;
			ApplySort(comparison);
		}
		#endregion
	}
}

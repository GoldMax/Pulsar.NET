using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
    [Flags()]
    internal enum GraphItemLinkType : byte
    {
        Singles = 0,
        MultiParent = 1,
        MultiChild = 2
    }
    //*************************************************************************************
    /// <summary>
    /// Базовый интерфейс графов
    /// </summary>
    public interface IGraph : ICollectionChangeNotify, IList, IVersionedObject
    {
        /// <summary>
        /// Возвращает тип объекта элементов графа.
        /// </summary>
        Type ObjectType { get; }
        /// <summary>
        /// Возвращает тип элементов графа.
        /// </summary>
        Type ItemType { get; }
        /// <summary>
        /// Определяет возможность создания множественных родительских связей элементов графа.
        /// </summary>
        bool IsMuliParent { get; }
        /// <summary>
        /// Определяет возможность создания множественных дочерних связей элементов графа.
        /// </summary>
        bool IsMuliChild { get; }
        /// <summary>
        /// Определяет возможность добавления одних и тех же объектов несколько раз
        /// </summary>
        bool AllowMultiObject { get; }
        /// <summary>
        /// Параметры дерева.
        /// </summary>
        ParamsDic Params { get; set; }
        /// <summary>
        /// Определяет положение в списке указанного элемента.
        /// </summary>
        /// <param name="item">Определяемый элемент.</param>
        /// <returns></returns>
        int IndexOf(IGraphItem item);
        /// <summary>
        /// Определяет положения в списке всех элементов с указанным объектом.
        /// </summary>
        /// <param name="obj">Объект определяемых элементов.</param>
        /// <returns></returns>
        int[] IndexOfAll(object obj);
        /// <summary>
        /// Добавляет элемент в граф
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        void Add(IGraphItem item, IGraphItem parent);
        /// <summary>
        /// Добавляет элемент в граф
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parentObj"></param>
        void Add(IGraphItem item, object parentObj);
        /// <summary>
        /// Создает элемент и добавляет в граф
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
								IGraphItem Add(object obj, IGraphItem parent);
        /// <summary>
        /// Создает элемент и добавляет в граф
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parentObj"></param>
								IGraphItem Add(object obj, object parentObj);
        /// <summary>
        /// Удаляет все элементы с заданным объектом.
        /// </summary>
        /// <param name="obj">Объект удаляемых элементов.</param>
        /// <returns></returns>
        void RemoveAll(object obj);
        /// <summary>
        /// Удаляет указанный элемент графа.
        /// </summary>
        /// <param name="item">Удаляемый элемент.</param>
        /// <returns></returns>
        void Remove(IGraphItem item);
        /// <summary>
        /// Перечисление элементов графа.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGraphItem> Items { get; }
        /// <summary>
        /// Перечисление объектов элементов графа.
        /// </summary>
        /// <returns></returns>
        IEnumerable Objects { get; }
        /// <summary>
        /// Проверяет, принадлежит ли элемент с указанным объектом графу.
        /// Если принадлежит, возвращает этот же объект, иначе возвращает null.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        object InList(object obj);
        /// <summary>
        /// Добавляет родительскую связь элемента графа.
        /// </summary>
        /// <param name="item">Изменяемый элемент.</param>
        /// <param name="parentItem">Родительский элемент.</param>
        void AddParentLink(IGraphItem item, IGraphItem parentItem);
        /// <summary>
        /// Удаляет родительскую связь элемента графа.
        /// </summary>
        /// <param name="item">Изменяемый элемент.</param>
        /// <param name="parentItem">Родительский элемент.</param>
        void RemoveParentLink(IGraphItem item, IGraphItem parentItem);
        /// <summary>
        /// Возвращает конечные элементы (элементы без дочерних связей) графа.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGraphItem> GetEndItems();
        /// <summary>
        /// Возвращает конечные элементы (элементы без дочерних связей) графа, 
        /// являющиеся рекурсивно дочерними относительно указанного элемента.
        /// </summary>
        /// <param name="item">Элемент, относительно которого ищутся конечные элементы.</param>
        /// <returns></returns>
        IEnumerable<IGraphItem> GetEndItems(IGraphItem item);
        /// <summary>
        /// Возвращает перечисление корневых элементов.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGraphItem> GetRootItems();
        /// <summary>
        /// Возвращает элемент по его индексу.
        /// </summary>
        /// <param name="pos">Позиция элемента в графе.</param>
        /// <returns></returns>
        IGraphItem GetItemWithIndex(int pos);
    }
    //*************************************************************************************
    /// <summary>
    /// Базовый класс графов
    /// </summary>
    /// <typeparam name="T">Тип объектов элементов графа.</typeparam>
    /// <typeparam name="TItem">Тип элемента дерева.</typeparam>
    public abstract class Graph<T, TItem> : CollectionChangeNotify, IGraph, IList<T>, IList
     where TItem : GraphItem
    {
        private static TItem[] empty = new TItem[0];

        private uint _ver = 0;
        private ParamsDic _pars = null;
        private GraphItemLinkType _type = GraphItemLinkType.Singles;
        private bool _allowMultiObject = false;
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        #region << Events >>
        [NonSerialized]
        private Pulsar.WeakEvent<IGraphItem> _ParentChanged;
        /// <summary>
        /// Событие, возникающее при изменении родительских ссылок элемента.
        /// </summary>
        public event EventHandler<Graph<T, TItem>, IGraphItem> ParentChanged
        {
            add { _ParentChanged += value; }
            remove { _ParentChanged -= value; }
        }
        /// <summary>
        /// Вызывает событие ParentChanged.
        /// </summary>
        protected virtual void OnParentChanged(IGraphItem arg)
        {
            if (_ParentChanged != null && IsChangeEventsOff == false)
                _ParentChanged.Raise(this, arg);
            //OnCollectionChanged(new CollectionChangeNotificationArgs(arg));
        }
        #endregion << Events >>
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #region << Properties >>
        /// <summary>
        /// Версия изменений
        /// </summary>
        public uint Version
        {
            get { return _ver; }
        }
        /// <summary>
        /// Параметры дерева.
        /// </summary>
        public ParamsDic Params
        {
            get { return _pars; }
            set
            {
                OnObjectChanging("Params", value, _pars);
                ParamsDic old = _pars;
                _pars = value;
                if (_pars != null)
                {
                    _pars.ObjectChanging += Params_ObjectChanging;
                    _pars.ObjectChanged += Params_ObjectChanged;
                }
                OnObjectChanged("Params", value, old);
            }
        }
        /// <summary>
        /// Определяет возможность создания множественных родительских связей элементов графа.
        /// </summary>
        public bool IsMuliParent
        {
            get { return _type.HasFlag(GraphItemLinkType.MultiParent); }
        }
        /// <summary>
        /// Определяет возможность создания множественных дочерних связей элементов графа.
        /// </summary>
        public bool IsMuliChild
        {
            get { return _type.HasFlag(GraphItemLinkType.MultiChild); }
        }
        /// <summary>
        /// Определяет возможность добавления одних и тех же объектов несколько раз 
        /// </summary>
        public bool AllowMultiObject
        {
            get { return _allowMultiObject; }
        }
        /// <summary>
        /// Перечисление элементов графа.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<TItem> Items { get; }
        IEnumerable<IGraphItem> IGraph.Items { get { return (IEnumerable<IGraphItem>)Items; } }
        /// <summary>
        /// Перечисление объектов элементов графа.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<T> Objects { get; }
        IEnumerable IGraph.Objects { get { return Objects; } }
        /// <summary>
        /// Возвращает тип объекта элементов графа.
        /// </summary>
        public Type ObjectType { get { return typeof(T); } }
        /// <summary>
        /// Возвращает тип элементов графа.
        /// </summary>
        public Type ItemType { get { return typeof(TItem); } }
        #endregion << Properties >>
        //-------------------------------------------------------------------------------------
        #region << Constructors >>
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        protected Graph() { }
        //-------------------------------------------------------------------------------------
        /// <summary>
        /// Инициализирующий конструктор.
        /// </summary>
        /// <param name="isMultiParent">Определяет возможность создания множественных родительских связей</param>
        /// <param name="isMultiChild">Определяет возможность создания множественных дочерних связей</param>
        /// <param name="allowMultiObject">Определяет возможность добавления одних и тех же объектов несколько раз</param>
        public Graph(bool isMultiParent, bool isMultiChild, bool allowMultiObject = false)
        {
            if (isMultiParent)
                _type |= GraphItemLinkType.MultiParent;
            if (isMultiChild)
                _type |= GraphItemLinkType.MultiChild;
            _allowMultiObject = allowMultiObject;
        }
        #endregion << Constructors >>
        //-------------------------------------------------------------------------------------
        #region IList's Members
        /// <summary>
        /// Возвращает объект элемента, находящегося в указанной позиции.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public abstract T this[int pos] { get; set; }
        object IList.this[int pos]
        {
            get { return this[pos]; }
            set { this[pos] = (T)value; }
        }
        /// <summary>
        /// Определяет положение в списке первого элемента с указанным объектом.
        /// </summary>
        /// <param name="obj">Объект определяемого элемента.</param>
        /// <returns></returns>
        public abstract int IndexOf(T obj);
        int IList.IndexOf(object obj) { return IndexOf((T)obj); }
        /// <summary>
        /// Определяет положение в списке указанного элемента.
        /// </summary>
        /// <param name="item">Определяемый элемент.</param>
        /// <returns></returns>
        public abstract int IndexOf(IGraphItem item);
        int IGraph.IndexOf(IGraphItem item) { return IndexOf(item); }
        /// <summary>
        /// Определяет положения в списке всех элементов с указанным объектом.
        /// </summary>
        /// <param name="obj">Объект определяемых элементов.</param>
        /// <returns></returns>
        public abstract int[] IndexOfAll(T obj);
        int[] IGraph.IndexOfAll(object obj) { return IndexOfAll((T)obj); }
        /// <summary>
        /// Определяет, содержится ли хоть один элемент с указанным объектом в графе.
        /// </summary>
        /// <param name="obj">Объект определяемого элемента.</param>
        /// <returns></returns>
        public abstract bool Contains(T obj);
        bool IList.Contains(object obj)
        {
            if (obj is T)
                return Contains((T)obj);
            return Contains((IGraphItem)obj);
        }
        /// <summary>
        /// Определяет, содержится ли указанный элемент в графе.
        /// </summary>
        /// <param name="item">Определяемый элемент.</param>
        /// <returns></returns>
        public abstract bool Contains(IGraphItem item);
        /// <summary>
        /// Копирует список объектов элементов в массив.
        /// </summary>
        /// <param name="array">Массив назначения.</param>
        /// <param name="arrayIndex">Стратовый индекс в массиве назначения.</param>
        public abstract void CopyTo(T[] array, int arrayIndex);
        void ICollection.CopyTo(Array array, int arrayIndex) { CopyTo((T[])array, arrayIndex); }
        /// <summary>
        /// Возвращает количество элементов с объектами
        /// </summary>
        public abstract int Count { get; }
        ///
        bool ICollection<T>.IsReadOnly { get { return false; } }
        ///
        void ICollection<T>.Add(T obj) { throw new NotImplementedException("Метод Add не поддерживается!"); }
        int IList.Add(object value) { throw new NotImplementedException("Метод Add не поддерживается!"); }
        /// <summary>
        /// Добавляет элемент в граф
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        public abstract void Add(IGraphItem item, IGraphItem parent);
        /// <summary>
        /// Добавляет элемент в граф
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parentObj"></param>
        public abstract void Add(IGraphItem item, T parentObj);
        void IGraph.Add(IGraphItem item, object parentObj) { Add(item, (T)parentObj); }
        /// <summary>
        /// Создает элемент и добавляет в граф
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
								public abstract IGraphItem Add(T obj, IGraphItem parent);
								IGraphItem IGraph.Add(object obj, IGraphItem parent) { return Add((T)obj, parent); }
        /// <summary>
        /// Создает элемент и добавляет в граф
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parentObj"></param>
								public abstract IGraphItem Add(T obj, T parentObj);
        IGraphItem IGraph.Add(object obj, object parentObj) { return Add((T)obj, (T)parentObj); }
        ///
        void IList<T>.Insert(int index, T item) { throw new NotImplementedException("Метод Insert не поддерживается!"); }
        void IList.Insert(int index, object value) { throw new NotImplementedException("Метод Insert не поддерживается!"); }
        /// <summary>
        /// Удаляет элемент из списка в указанной позиции
        /// </summary>
        /// <param name="pos">Позиция удаляемого элемента</param>
        public abstract void RemoveAt(int pos);
        /// <summary>
        /// Удаляет первый в списке элемент с заданным объектом.
        /// </summary>
        /// <param name="obj">Объект удаляемого элемента.</param>
        /// <returns></returns>
        public abstract bool Remove(T obj);
        void IList.Remove(object obj) { Remove((T)obj); }
        /// <summary>
        /// Удаляет заданный элемент
        /// </summary>
        /// <param name="item">Удаляемый элемент.</param>
        /// <returns></returns>
        public abstract void Remove(IGraphItem item);
        /// <summary>
        /// Удаляет все элементы с заданным объектом.
        /// </summary>
        /// <param name="obj">Объект удаляемых элементов.</param>
        /// <returns></returns>
        public abstract void RemoveAll(T obj);
        void IGraph.RemoveAll(object obj) { RemoveAll((T)obj); }
        /// <summary>
        /// Удаляет все элементы графа
        /// </summary>
        public abstract void Clear();
        ///
        bool IList.IsFixedSize { get { return false; } }
        bool IList.IsReadOnly { get { return false; } }
        bool ICollection.IsSynchronized { get { return false; } }
        object ICollection.SyncRoot { get { return SyncRoot; } }
        #endregion IList's Members
        //-------------------------------------------------------------------------------------
        #region << Graph Methods >>
        /// <summary>
        /// Добавляет родительскую связь элемента графа.
        /// </summary>
        /// <param name="item">Изменяемый элемент.</param>
        /// <param name="parentItem">Родительский элемент.</param>
        protected virtual void AddParentLink(IGraphItem item, IGraphItem parentItem)
        {
            TItem it = (TItem)item;
            TItem pi = (TItem)parentItem;

            if (this.Contains(it) == false)
                throw new Exception("Изменяемый элемент не содержится в графе!");
            if (pi != null && this.Contains(pi) == false)
                throw new Exception("Родительский элемент не содержится в графе!");
            if (IsMuliParent == false && it.HasParents)
                throw new Exception("Множественный родительские ссылки не разрешены!");
            if (IsMuliChild == false && pi.HasChildren)
                throw new Exception("Множественный дочерние ссылки не разрешены!");
            it.LinkParent(pi);
            if (pi != null)
                pi.LinkChild(it);
            OnParentChanged(it);
        }
        void IGraph.AddParentLink(IGraphItem item, IGraphItem parentItem) { AddParentLink(item, parentItem); }
        /// <summary>
        /// Удаляет родительскую связь элемента графа.
        /// </summary>
        /// <param name="item">Изменяемый элемент.</param>
        /// <param name="parentItem">Родительский элемент.</param>
        protected virtual void RemoveParentLink(IGraphItem item, IGraphItem parentItem)
        {
            if (this.Contains(item) == false)
                throw new Exception("Изменяемый элемент не содержится в графе!");
            if (this.Contains(parentItem) == false)
                throw new Exception("Родительский элемент не содержится в графе!");
            if (item.Parents.Contains(parentItem) == false)
                throw new Exception("Отсутствует родительская ссылка для указанного элемента!");
            ((TItem)item).UnlinkParent((TItem)parentItem);
            ((TItem)parentItem).UnlinkChild(((TItem)item));
            OnParentChanged(item);
        }
        void IGraph.RemoveParentLink(IGraphItem item, IGraphItem parentItem) { RemoveParentLink(item, parentItem); }
        //-------------------------------------------------------------------------------------
        /// <summary>
        /// Возвращает конечные элементы (элементы без дочерних связей) графа.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TItem> GetEndItems()
        {
            foreach (TItem i in Items)
                if (i.HasChildren == false)
                    yield return i;
        }
        IEnumerable<IGraphItem> IGraph.GetEndItems() { return (IEnumerable<IGraphItem>)GetEndItems(); }
        /// <summary>
        /// Возвращает конечные элементы (элементы без дочерних связей) графа, 
        /// являющиеся рекурсивно дочерними относительно указанного элемента.
        /// </summary>
        /// <param name="item">Элемент, относительно которого ищутся конечные элементы.</param>
        /// <returns></returns>
        public virtual IEnumerable<TItem> GetEndItems(IGraphItem item)
        {
            Stack<TItem> stack = new Stack<TItem>();
            foreach (TItem i in item.Children)
                if (Object.Equals(i, item))
                    stack.Push(i);
            HashSet<TItem> was = new HashSet<TItem>();
            was.Add((TItem)item);
            while (stack.Count > 0)
            {
                TItem it = stack.Pop();
                if (was.Contains(it))
                    continue;
                was.Add(it);
                if (it.HasChildren == false)
                    yield return it;
                foreach (TItem i in it.Children)
                    if (was.Contains(i) == false)
                        stack.Push(i);
            }
        }
        IEnumerable<IGraphItem> IGraph.GetEndItems(IGraphItem item) { return GetEndItems(item); }
        /// <summary>
        /// Возвращает перечисление корневых элементов.
        /// </summary>
        /// <returns></returns>		
        public virtual IEnumerable<TItem> GetRootItems()
        {
            foreach (TItem i in Items)
                if (i.IsRoot)
                    yield return i;
        }
        IEnumerable<IGraphItem> IGraph.GetRootItems() { return GetRootItems(); }
        /// <summary>
        /// Проверяет, принадлежит ли элемент с указанным объектом графу.
        /// Если принадлежит, возвращает этот же объект, иначе возвращает null.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual T InList(T obj)
        {
            int pos = IndexOf(obj);
            return pos == -1 ? default(T) : this[pos];
        }
        object IGraph.InList(object obj) { return InList((T)obj); }
        /// <summary>
        /// Проверяет версию графа. Если версия одинаковая, возвращает сам граф, иначе выбрасывает исключение защиты.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public virtual Graph<T, TItem> CheckVersion(uint version)
        {
            if (this._ver != version)
                throw new PulsarErrorException("Версия серверного объекта отличается от версии клиентского объекта!");
            return this;
        }
        object IVersionedObject.CheckVersion(uint version) { return CheckVersion(version); }
        /// <summary>
        /// Возвращает элемент по его индексу.
        /// </summary>
        /// <param name="pos">Позиция элемента в графе.</param>
        /// <returns></returns>
        public abstract IGraphItem GetItemWithIndex(int pos);
        IGraphItem IGraph.GetItemWithIndex(int pos) { return GetItemWithIndex(pos); }
        #endregion << Graph Methods >>
        //-------------------------------------------------------------------------------------
        #region IEnumerable<T> Members
        /// <summary>
        /// Перечислитель объектов элементов графа.
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() { return Objects.GetEnumerator(); }
        /// <summary>
        /// Перечислитель элементов графа.
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion
        //-------------------------------------------------------------------------------------
        #region << Methods >>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnObjectChanged(ObjectChangeNotifyEventArgs args)
        {
            _ver++;
            base.OnObjectChanged(args);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnObjectResetted()
        {
            _ver++;
            base.OnObjectResetted();
        }
        void Params_ObjectChanging(object sender, ChangeNotifyEventArgs e)
        {
            OnObjectChanging(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange, e.SourceArgs ?? e,
                                                                                                                                                                                                                "Params", sender, null));
        }
        void Params_ObjectChanged(object sender, ChangeNotifyEventArgs e)
        {
            OnObjectChanged(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange, e.SourceArgs ?? e,
                                                                                                                                                                                                                    "Params", sender, null));
        }
        /// <summary>
        /// Метод, вызываемый до изменения объекта элемента графа.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GraphItem_ObjectChanging(object sender, ObjectChangeNotifyEventArgs e)
        {
            OnItemChanging(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemChange, e.SourceArgs ?? e, sender));
        }
        /// <summary>
        /// Метод, вызываемый после изменения объекта элемента графа.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GraphItem_ObjectChanged(object sender, ObjectChangeNotifyEventArgs e)
        {
            OnItemChanged(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemChange, e.SourceArgs ?? e, sender));
        }
        //-------------------------------------------------------------------------------------
        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
        {
            if (_pars != null)
            {
                _pars.ObjectChanging += Params_ObjectChanging;
                _pars.ObjectChanged += Params_ObjectChanged;
            }
            foreach (IGraphItem i in Items)
            {
                i.ObjectChanging += GraphItem_ObjectChanging;
                i.ObjectChanged += GraphItem_ObjectChanged;
            }
        }

        #endregion << Methods >>
    }
}
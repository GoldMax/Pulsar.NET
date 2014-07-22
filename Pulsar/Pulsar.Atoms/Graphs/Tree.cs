using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
    #region << public class TreeItemMoveEventArgs : EventArgs >>
    /// <summary>
    /// Класс аргумента событий перемещения 
    /// </summary>
    public class TreeItemMoveEventArgs : EventArgs
    {
        /// <summary>
        /// Перемещаемый элемент
        /// </summary>
        public ITreeItem Item { get; set; }
        /// <summary>
        /// Старый родительский элемент
        /// </summary>
        public ITreeItem OldParentItem { get; set; }
        /// <summary>
        /// Новый родительский элемент
        /// </summary>
        public ITreeItem NewParentItem { get; set; }
    }
    #endregion << public class TreeItemMoveEventArgs : EventArgs >>
    //*************************************************************************************
    #region << public interface ITree : IGraph >>
    /// <summary>
    /// Интерфейс специального графа - дерева
    /// </summary>
    public interface ITree : IGraph
    {
        /// <summary>
        /// Возвращает элемент с указанным объектом.
        /// </summary>
        /// <param name="itemObject">Объект элемента.</param>
        /// <returns></returns>
        ITreeItem this[object itemObject] { get; }
        /// <summary>
        /// Событие, возникающее до перемещения элемента в дереве (смене родителя).
        /// </summary>
        event EventHandler<TreeItemMoveEventArgs> ItemMoving;
        /// <summary>
        /// Событие, возникающее после перемещения элемента в дереве (смене родителя).
        /// </summary>
        event EventHandler<TreeItemMoveEventArgs> ItemMoved;
        /// <summary>
        /// Перемещает элемент в дереве к новому родительскому элементу.
        /// </summary>
        /// <param name="item">Перемещаемый элемент.</param>
        /// <param name="parent">Новый родительский элемент.</param>
        void MoveItem(ITreeItem item, ITreeItem parent);
        /// <summary>
        /// Возвращает список всех родительских элементов.
        /// </summary>
        /// <param name="item">Элемент, для которого строится список.</param>
        /// <param name="reverseOrder">Определяет порядок расположения элементов в списке (false - с корневого).</param>
        /// <returns></returns>
        IList GetParentsItemsList(ITreeItem item, bool reverseOrder = false);
        /// <summary>
        /// Возвращает список всех дочерних элементов для указанного элемента.
        /// </summary>
        /// <param name="item">Элемент, дочерние элементы которого добавляются в список.</param>
        /// <returns></returns>
        IList GetAllChildren(ITreeItem item);
        /// <summary>
        /// Вовзвращает элементы дерева, упорядоченные как при рекурсивном обходе.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITreeItem> GetRightOrderedItems();
        /// <summary>																																							
        /// Определяет корневой элемент для указанного элемента.
        /// </summary>
        /// <param name="item">Элемент, для которого определяется корневой элемент.</param>
        /// <returns></returns>
        ITreeItem GetRootItem(ITreeItem item);
    }
    #endregion << public interface ITree	: IGraph >>
    //*************************************************************************************
    /// <summary>
    /// Класс специального графа - дерева. Элементом дерева является TreeItem'T
    /// </summary>
    /// <typeparam name="T">Тип объекта элемента графа.</typeparam>
    public class Tree<T> : Tree<T, TreeItem<T>>, ITree, IList<T>, IList
    {
        //-------------------------------------------------------------------------------------
        #region << Constructors >>
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Tree() : base() { }
        /// <summary>
        /// Инициализирующий конструктор
        /// </summary>
        /// <param name="allowMultiObj">Определяет возможность добавления тех же 
        /// объектов несколько раз</param>
        public Tree(bool allowMultiObj) : base(allowMultiObj) { }
        #endregion << Constructors >>
    }
    //*************************************************************************************
    /// <summary>
    /// Класс специального графа - дерева
    /// </summary>
    /// <typeparam name="T">Тип объекта элемента графа.</typeparam>
    /// <typeparam name="TItem">Тип элемента дерева.</typeparam>
    public class Tree<T, TItem> : Graph<T, TItem>, ITree, IList<T>, IList
        where TItem : TreeItem<T>
    {
        /// <summary>
        /// Элементы без объекта (группы)
        /// </summary>
        protected ICollection<TItem> _noObj = new HashIndex<TItem>();
        /// <summary>
        /// Элементы с объектом
        /// </summary>
        protected IndexedList<TItem, T> _withObj;
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        #region << Events >>
        #region << public event EventHandler<TreeItemMoveEventArgs> ItemMoving >>
        [NonSerialized]
        private Pulsar.WeakEvent<TreeItemMoveEventArgs> _ItemMoving;
        /// <summary>
        /// Событие, возникающее до перемещения элемента в дереве (смене родителя).
        /// </summary>
        public event EventHandler<TreeItemMoveEventArgs> ItemMoving
        {
            add { _ItemMoving += value; }
            remove { _ItemMoving -= value; }
        }
        /// <summary>
        /// Вызывает событие ItemMoving.
        /// </summary>
        protected virtual void OnItemMoving(TreeItemMoveEventArgs arg)
        {
            if (_ItemMoving != null && IsChangeEventsOff == false)
                _ItemMoving.Raise(this, arg);
        }
        #endregion << public event EventHandler<TreeItemMoveEventArgs> ItemMoving >>
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        #region << public event EventHandler<TreeItemMoveEventArgs> ItemMoved >>
        [NonSerialized]
        private Pulsar.WeakEvent<TreeItemMoveEventArgs> _ItemMoved;
        /// <summary>
        /// Событие, возникающее после перемещения элемента в дереве (смене родителя).
        /// </summary>
        public event EventHandler<TreeItemMoveEventArgs> ItemMoved
        {
            add { _ItemMoved += value; }
            remove { _ItemMoved -= value; }
        }
        /// <summary>
        /// Вызывает событие ItemMoved.
        /// </summary>
        protected virtual void OnItemMoved(TreeItemMoveEventArgs arg)
        {
            if (_ItemMoved != null && IsChangeEventsOff == false)
                _ItemMoved.Raise(this, arg);
        }
        #endregion << public event EventHandler<TreeItemMoveEventArgs> ItemMoved >>
        #endregion << Events >>
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #region << Properties >>
        /// <summary>
        /// Перечисление элементов графа.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TItem> Items
        {
            get
            {
                foreach (TItem i in _noObj)
                    yield return i;
                foreach (TItem i in _withObj)
                    yield return i;
            }
        }
        /// <summary>
        /// Перечисление объектов элементов графа.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<T> Objects
        {
            get
            {
                foreach (TItem i in _withObj)
                    yield return i.Object;
            }
        }
        #endregion << Properties >>
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //-------------------------------------------------------------------------------------
        #region << Constructors >>
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Tree()
            : base(false, true)
        {
            _withObj = new IndexedList<TItem, T>("Object");
        }

        /// <summary>
        /// Инициализирующий конструктор
        /// </summary>
        /// <param name="allowMultiObj">Определяет возможность добавления тех же 
        /// объектов несколько раз</param>
        public Tree(bool allowMultiObj)
            : base(false, true, allowMultiObj)
        {
            _withObj = new IndexedList<TItem, T>("Object", !this.AllowMultiObject);
        }

        #endregion << Constructors >>
        //-------------------------------------------------------------------------------------
        #region << IList<T> Methods >>
        /// <summary>
        /// Возвращает объект элемента, находящегося в указанной позиции.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public override T this[int pos]
        {
            get { return _withObj[pos].Object; }
            set { throw new NotImplementedException("Метод set_Item не поддерживается!"); }
        }
        /// <summary>
        /// Возвращает элемент с указанным объектом.
        /// </summary>
        /// <param name="obj">Объект элемента.</param>
        /// <returns></returns>
        public virtual TItem this[T obj]
        {
            get { return _withObj.ByIndex(obj); }
        }
        ITreeItem ITree.this[object obj] { get { return this[(T)obj]; } }
        /// <summary>
        /// Определяет положение в списке первого элемента с указанным объектом.
        /// </summary>
        /// <param name="obj">Объект определяемого элемента.</param>
        /// <returns></returns>
        public override int IndexOf(T obj)
        {
            return _withObj.IndexOfIndex(obj);
        }
        /// <summary>
        /// Определяет положение в списке указанного элемента. 
        /// Элемент должен иметь установленным значение объекта.
        /// </summary>
        /// <param name="item">Определяемый элемент.</param>
        /// <returns></returns>
        public override int IndexOf(IGraphItem item)
        {
            return _withObj.IndexOf((TItem)item);
        }
        /// <summary>
        /// Определяет положения в списке всех элементов с указанным объектом.
        /// </summary>
        /// <param name="obj">Объект определяемых элементов.</param>
        /// <returns></returns>
        public override int[] IndexOfAll(T obj)
        {
            return _withObj.IndexOfIndexAll(obj);
        }
        /// <summary>
        /// Определяет, содержится ли хоть один элемент с указанным объектом в графе.
        /// </summary>
        /// <param name="obj">Объект определяемого элемента.</param>
        /// <returns></returns>
        public override bool Contains(T obj)
        {
            return _withObj.ContainsIndex(obj);
        }
        /// <summary>
        /// Определяет, содержится ли указанный элемент в графе.
        /// </summary>
        /// <param name="item">Определяемый элемент.</param>
        /// <returns></returns>
        public override bool Contains(IGraphItem item)
        {
            if (_withObj.Contains((TItem)item))
                return true;
            if (_noObj.Contains((TItem)item))
                return true;
            return false;
        }
        /// <summary>
        /// Копирует список объектов элементов в массив.
        /// </summary>
        /// <param name="array">Массив назначения.</param>
        /// <param name="arrayIndex">Стратовый индекс в массиве назначения.</param>
        public override void CopyTo(T[] array, int arrayIndex)
        {
            for (int a = 0; a < _withObj.Count; a++)
                array[a + arrayIndex] = _withObj[a].Object;
        }
        /// <summary>
        /// Возвращает количество элементов с объектами
        /// </summary>
        int ICollection.Count
        {
            get { return _withObj.Count; }
        }
        /// <summary>
        /// Возвращает общее количество элементов
        /// </summary>
        public override int Count
        {
            get { return _withObj.Count + _noObj.Count; }
        }
        /// <summary>
        /// Добавляет элемент в граф
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        public override void Add(IGraphItem item, IGraphItem parent)
        {
            if (item is TItem == false)
                throw new Exception("Добавляемый элемет имеет неправильный тип!");
            TItem it = (TItem)item;
            TItem pi = (TItem)parent;

            if (it == null)
                throw new ArgumentNullException("item");
            if (it.IsRoot == false || it.HasChildren)
                throw new Exception("Добавляемый элемент принадлежит другому дереву!");
            if (it.IsGroup && _noObj.Contains(it))
                throw new Exception("Добавляемый элемент уже принадлежит дереву!");
            if (it.IsGroup == false && _withObj.Contains(it))
                throw new Exception("Добавляемый элемент уже принадлежит дереву!");

            if (pi != null)
            {
                if (pi.IsGroup && _noObj.Contains(pi) == false)
                    throw new Exception("Родительский элемент не принадлежит дереву!");
                if (pi.IsGroup == false && _withObj.Contains(pi) == false)
                    throw new Exception("Родительский элемент не принадлежит дереву!");
                if (IsMuliChild == false && pi.HasChildren)
                    throw new Exception("Родительский элемент уже имеет дочерние связи!");
            }

            OnItemAdding(it, it.IsGroup ? -1 : _withObj.Count);
            if (it.IsGroup)
                _noObj.Add(it);
            else
                _withObj.Add(it);
            if (pi != null)
            {
                pi.LinkChild(it);
                it.LinkParent(pi);
            }
            it.ObjectChanging += GraphItem_ObjectChanging;
            it.ObjectChanged += GraphItem_ObjectChanged;

            OnItemAdded(it);

        }
        /// <summary>
        /// Добавляет элемент в граф
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parentObj"></param>
        public override void Add(IGraphItem item, T parentObj)
        {
            if (parentObj == null)
                Add(item, null);
            else
            {
                IGraphItem pi = this[parentObj];
                if (pi == null)
                    throw new Exception("Родительский элемент не принадлежит дереву!");
                Add(item, pi);
            }
        }
        /// <summary>
        /// Создает элемент и добавляет в граф
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
								public override IGraphItem Add(T obj, IGraphItem parent)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
												TItem item = (TItem)Activator.CreateInstance(typeof(TItem), obj);
            Add(item, parent);
												return item;
        }
        /// <summary>
        /// Создает элемент и добавляет в граф
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parentObj"></param>
								public override IGraphItem Add(T obj, T parentObj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
												TItem item;
            if (parentObj == null)
                Add(item = (TItem)Activator.CreateInstance(typeof(TItem), obj), null);
            else
            {
                IGraphItem pi = this[parentObj];
                if (pi == null)
                    throw new Exception("Родительский элемент не принадлежит дереву!");
                Add(item = (TItem)Activator.CreateInstance(typeof(TItem), obj), pi);
            }
												return item;
        }
        /// <summary>
        /// Удаляет элемент из списка в указанной позиции
        /// </summary>
        /// <param name="pos">Позиция удаляемого элемента</param>
        public override void RemoveAt(int pos)
        {
            TItem item = _withObj[pos];
            Remove(item);
        }
        /// <summary>
        /// Удаляет первый в списке элемент с заданным объектом.
        /// </summary>
        /// <param name="obj">Объект удаляемого элемента.</param>
        /// <returns></returns>
        public override bool Remove(T obj)
        {
            if (Object.Equals(obj, null))
                throw new ArgumentNullException("obj");
            else if (_withObj.ContainsIndex(obj) == false)
                return false;

            int pos = _withObj.IndexOfIndex(obj);
            TItem item = _withObj[pos];

            OnItemDeleting(item, pos);

            DeleteChildItems(item);
            item.UnlinkItem();
            if (item.IsGroup)
                _noObj.Remove(item);
            else
                _withObj.Remove(item);

            item.ObjectChanging -= GraphItem_ObjectChanging;
            item.ObjectChanged -= GraphItem_ObjectChanged;

            OnItemDeleted(item, pos);
            item.Dispose();
            return true;
        }
        /// <summary>
        /// Удаляет заданный элемент
        /// </summary>
        /// <param name="item">Удаляемый элемент.</param>
        /// <returns></returns>
        public override void Remove(IGraphItem item)
        {
            TItem it = (TItem)item;

            if (Object.Equals(it, null))
                throw new ArgumentNullException("item");

            int pos = -1;
            if (it.IsGroup && _noObj.Contains(it) == false)
                return;
            if (it.IsGroup == false && (pos = _withObj.IndexOf((TItem)it)) == -1)
                return;

            OnItemDeleting(it, pos);

            DeleteChildItems(it);
            it.UnlinkItem();
            if (it.IsGroup)
                _noObj.Remove(it);
            else
                _withObj.Remove(it);

            it.ObjectChanging -= GraphItem_ObjectChanging;
            it.ObjectChanged -= GraphItem_ObjectChanged;

            OnItemDeleted(it, pos);
            it.Dispose();
        }
        /// <summary>
        /// Удаляет все элементы с заданным объектом.
        /// </summary>
        /// <param name="obj">Объект удаляемых элементов.</param>
        /// <returns></returns>
        public override void RemoveAll(T obj)
        {
            //--- Debbuger Break --- //
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
            //--- Debbuger Break --- //

            if (Object.Equals(obj, null))
                throw new ArgumentNullException("obj");

            while (Remove(obj)) ;
        }
        /// <summary>
        /// Удаляет все элементы графа
        /// </summary>
        public override void Clear()
        {
            OnObjectResetting();
            foreach (TItem i in _withObj)
            {
                i.ObjectChanging -= GraphItem_ObjectChanging;
                i.ObjectChanged -= GraphItem_ObjectChanged;
                i.Dispose();
            }
            foreach (TItem i in _noObj)
            {
                i.ObjectChanging -= GraphItem_ObjectChanging;
                i.ObjectChanged -= GraphItem_ObjectChanged;
                i.Dispose();
            }
												_withObj.Clear();
												_noObj.Clear();
            OnObjectResetted();
        }
        #endregion << IList<T> Methods >>
        //-------------------------------------------------------------------------------------
        #region << Override Methods >>
        /// <summary>
        /// Возвращает конечные элементы (элементы без дочерних связей), 
        /// являющиеся рекурсивно дочерними относительно указанного элемента.
        /// </summary>
        /// <param name="item">Элемент, относительно которого ищутся конечные элементы.</param>
        /// <returns></returns>
        public override IEnumerable<TItem> GetEndItems(IGraphItem item)
        {
            Stack<TItem> stack = new Stack<TItem>();
            foreach (TItem i in item.Children)
                stack.Push(i);
            while (stack.Count > 0)
            {
                TItem it = stack.Pop();
                if (it.HasChildren == false)
                    yield return it;
                foreach (TItem i in it.Children)
                    stack.Push(i);
            }
        }
        //-------------------------------------------------------------------------------------
        public override void EventsOff(bool raiseResetting = true)
        {
            base.EventsOff(raiseResetting);
            foreach (var i in GetRootItems())
                i.EventsOff(false);
        }
        public override void EventsOn(bool raiseResetted = true)
        {
            foreach (var i in GetRootItems())
                i.EventsOn(false);
            base.EventsOn(raiseResetted);
        }
        /// <summary>
        /// Возвращает элемент по его индексу.
        /// </summary>
        /// <param name="pos">Позиция элемента в дереве.</param>
        /// <returns></returns>
        public override IGraphItem GetItemWithIndex(int pos)
        {
            return _withObj[pos];
        }
        #endregion << Override Methods >>
        //-------------------------------------------------------------------------------------
        #region << Self Methods >>
        /// <summary>
        /// Удаляет дочерние элементы из списка.
        /// </summary>
        protected virtual void DeleteChildItems(TItem item)
        {
            foreach (TItem i in item.Children)
            {
                OnItemDeleting(i);
                DeleteChildItems(i);
                item.UnlinkChild(i);
                i.ObjectChanging -= GraphItem_ObjectChanging;
                i.ObjectChanged -= GraphItem_ObjectChanged;
                if (_withObj.Contains(i))
                    _withObj.Remove(i);
                else if (_noObj.Contains(i))
                    _noObj.Remove(i);
                OnItemDeleted(i);
                i.Dispose();
            }
        }
        /// <summary>
        /// Перемещает элемент в дереве к новому родительскому элементу.
        /// </summary>
        /// <param name="item">Перемещаемый элемент.</param>
        /// <param name="parent">Новый родительский элемент.</param>
        public virtual void MoveItem(TItem item, TItem parent)
        {
            ITreeItem old = item.Parent;
            OnItemMoving(new TreeItemMoveEventArgs() { Item = item, OldParentItem = old, NewParentItem = parent });
            if (item.IsRoot == false)
            {
                this.EventsOff(false);
                RemoveParentLink(item, item.Parent);
                this.EventsOn(false);
            }
            AddParentLink(item, parent);
            OnItemMoved(new TreeItemMoveEventArgs() { Item = item, OldParentItem = old, NewParentItem = parent });
        }
        void ITree.MoveItem(ITreeItem item, ITreeItem parent) { MoveItem((TItem)item, (TItem)parent); }
        /// <summary>
        /// Возвращает список всех родительских элементов.
        /// </summary>
        /// <param name="item">Элемент, для которого строится список.</param>
        /// <param name="reverseOrder">Определяет порядок расположения элементов в списке (false - с корневого).</param>
        /// <returns></returns>
        public virtual IList<TItem> GetParentsItemsList(ITreeItem item, bool reverseOrder = false)
        {
            if (item == null)
                return null;
            List<TItem> res = new List<TItem>();

            while (item.IsRoot == false)
            {
                if (reverseOrder)
                    res.Add((TItem)item.Parent);
                else
                    res.Insert(0, (TItem)item.Parent);
                item = (TItem)item.Parent;
            }
            return res;
        }
        IList ITree.GetParentsItemsList(ITreeItem item, bool reverseOrder)
        {
            return (IList)GetParentsItemsList(item, reverseOrder);
        }
        //-------------------------------------------------------------------------------------
        /// <summary>
        /// Возвращает список всех дочерних элементов для указанного элемента.
        /// </summary>
        /// <param name="item">Элемент, дочерние элементы которого добавляются в список.</param>
        /// <returns></returns>
        public IList<ITreeItem> GetAllChildren(ITreeItem item)
        {
            List<ITreeItem> list = new List<ITreeItem>();
            Stack<ITreeItem> stack = new Stack<ITreeItem>();
            stack.Push(item);
            while (stack.Count > 0)
            {
                ITreeItem i = stack.Pop();
                list.Add(i);
                foreach (ITreeItem ii in i.Children)
                    stack.Push(ii);
            }
            list.Remove(item);
            return list;
        }
        IList ITree.GetAllChildren(ITreeItem item)
        {
            return (IList)GetAllChildren(item);
        }
        //-------------------------------------------------------------------------------------
        /// <summary>
        /// Вовзвращает элементы дерева, упорядоченные как при рекурсивном обходе.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TItem> GetRightOrderedItems()
        {
            Queue<TItem> q = new Queue<TItem>(GetRootItems());
            foreach (var i in q)
                yield return i;
            while (q.Count > 0)
                foreach (var i in q.Dequeue().Children)
                {
                    yield return (TItem)i;
                    if (i.HasChildren)
                        q.Enqueue((TItem)i);
                }
        }
        IEnumerable<ITreeItem> ITree.GetRightOrderedItems() { return GetRightOrderedItems(); }
        //-------------------------------------------------------------------------------------
        /// <summary>
        /// Определяет корневой элемент для указанного элемента.
        /// </summary>
        /// <param name="item">Элемент, для которого определяется корневой элемент.</param>
        /// <returns></returns>
        TItem GetRootItem(TItem item)
        {
            while (item.IsRoot == false)
                item = (TItem)item.Parent;
            return item;
        }
        ITreeItem ITree.GetRootItem(ITreeItem item) { return (TItem)GetRootItem((TItem)item); }
        #endregion << Self Methods >>
    }
}

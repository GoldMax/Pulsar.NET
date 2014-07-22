using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Базовый интерфейс элемента (вершины) графа.
 /// </summary>
	public interface IGraphItem	: IObjectChangeNotify
	{
		/// <summary>
		/// Объект элемента
		/// </summary>
		object Object { get; }
		/// <summary>
		/// Альтернативный текст элемента
		/// </summary>
		string ItemText { get; set; }
		/// <summary>
		/// Параметры элемента дерева.
		/// </summary>
		ParamsDic Params { get; set; }
		/// <summary>
		/// Определяет, является ли элемент группой (объект не определен)
		/// </summary>
		bool IsGroup { get; }
		/// <summary>
		/// Определяет, является ли элемент корневым (нет родителей)
		/// </summary>
		bool IsRoot { get; }
		/// <summary>
		/// Определяет, имеет ли элемент дочерние связи
		/// </summary>
		bool HasChildren { get; }
		/// <summary>
		/// Определяет, имеет ли элемент родительские связи
		/// </summary>
		bool HasParents { get; }
		/// <summary>
		/// Родительский элемент. Если родителей несколько, возвращает первый родительский элемент
		/// </summary>
		IGraphItem Parent { get; }
		/// <summary>
		/// Родительские элементы.
		/// </summary>
		IEnumerable<IGraphItem> Parents { get; }
		/// <summary>
		/// Дочерний элемент. Если дочерних элементов несколько, возвращает первый дочерний элемент.
		/// </summary>
		IGraphItem Child { get; }
		/// <summary>
		/// Дочерние элементы.
		/// </summary>
		IEnumerable<IGraphItem> Children { get; }

	}

 /// <summary>
 /// Базовый класс элемента (вершины) графа.
 /// </summary>
	public abstract class GraphItem : ObjectChangeNotify, IGraphItem, IDisposable
	{		
	 private static GraphItem[] _empty = new GraphItem[0];

		private object _ob;
		private GraphItem _p = null;
		private ElasticArray<GraphItem> _ps = null;
		[NonSerialized]
		private GraphItem _ch = null;
		[NonSerialized]
		private ElasticArray<GraphItem> _chs = null;


		private ParamsDic _pars = null;
		private string _txt = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Объект элемента
		/// </summary>
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
		public object Object { get { return _ob; } }
		/// <summary>
		/// Альтернативный текст элемента
		/// </summary>
		public virtual string ItemText
		{
			get { return _txt ?? (object.Equals(_ob, null) ? "(нет значения)" : _ob.ToString()); }
			set
			{
				OnObjectChanging("ItemText", value, _txt);
				string old = _txt;
				_txt = value;
				OnObjectChanged("ItemText", _txt, old);
			}
		}
		/// <summary>
		/// Параметры элемента дерева.
		/// </summary>
		public virtual ParamsDic Params
		{
			get { return _pars; }
			set
			{
				OnObjectChanging("Params", value, _pars);
				ParamsDic old = _pars;
				_pars = value;
				if(_pars != null)
				{
					_pars.ObjectChanging += Params_ObjectChanging;
					_pars.ObjectChanged += Params_ObjectChanged;
				}
				OnObjectChanged("Params", _pars, old);
			}
		}
		/// <summary>
		/// Определяет, является ли элемент группой (объект не определен)
		/// </summary>
		public bool IsGroup
		{
			get { return object.Equals(_ob,null); }
		}
		/// <summary>
		/// Определяет, является ли элемент корневым (нет родителей)
		/// </summary>
		public bool IsRoot 
		{ 
		 get { return _p == null && _ps == null; }
		}
		/// <summary>
		/// Определяет, имеет ли элемент дочерние связи
		/// </summary>
		public bool HasChildren 
		{ 
		 get { return !(_ch == null && _chs == null); }
		}
		/// <summary>
		/// Определяет, имеет ли элемент родительские связи
		/// </summary>
		public bool HasParents 
		{ 
		 get { return !(_p == null && _ps == null); }
		}
		/// <summary>
		/// Родительский элемент. Если родителей несколько, возвращает первый родительский элемент
		/// </summary>
		public GraphItem Parent
		{
			get { return _p ?? (_ps == null ? null : _ps[0]); }
		}
		IGraphItem IGraphItem.Parent { get  { return Parent; } }
		/// <summary>
		/// Родительские элементы.
		/// </summary>
		public IEnumerable<GraphItem> Parents
		{
			get { return _ps ?? (_p == null ? _empty : new[] { _p }); }
		}
		IEnumerable<IGraphItem> IGraphItem.Parents { get { return Parents; } }
		/// <summary>
		/// Дочерний элемент. Если дочерних элементов несколько, возвращает первый дочерний элемент.
		/// </summary>
		public GraphItem Child
		{
			get { return _ch ?? (_chs == null ? null : _chs[0]); }
		}
		IGraphItem IGraphItem.Child { get { return Child; } }
		/// <summary>
		/// Дочерние элементы.
		/// </summary>
		public IEnumerable<GraphItem> Children
		{
			get { return _chs ?? (_ch == null ? _empty : new[] { _ch }); }
		}
		IEnumerable<IGraphItem> IGraphItem.Children { get { return Children; } }
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		protected GraphItem(object obj)
		{
			_ob = obj;
			OnDeserialized(new System.Runtime.Serialization.StreamingContext());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		protected GraphItem(object obj, string altText) : this(obj)
		{
			_txt = altText;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region IDisposable Members
		/// <summary>
		/// Подготавливает объект к уничтожению.
		/// </summary>
		public void Dispose()
		{
			UnlinkItem();
			if(_ob != null && _ob is IObjectChangeNotify)
			{
				((IObjectChangeNotify)_ob).ObjectChanging -= Object_ObjectChanging;
				((IObjectChangeNotify)_ob).ObjectChanged -= Object_ObjectChanged;
			}
			if(_pars != null)
			{
				_pars.ObjectChanging -= Params_ObjectChanging;
				_pars.ObjectChanged -= Params_ObjectChanged;
			}
			_ob = null;
		 _pars = null;
		 _txt = null;
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region << Protected & Override Methods >>
		/// <summary>
		/// Удаляет все дочерние и родительские связи элемента.
		/// </summary>
		protected internal void UnlinkItem()
		{
			if(IsRoot == false)
			{
			 foreach(GraphItem i in Parents.ToArray())
				 i.UnlinkChild(this);
				_p = null;
				_ps = null;
			}
			if(HasChildren)
			{
			 foreach(GraphItem i in Children.ToArray())
				 i.UnlinkParent(this);
				_ch = null;
				_chs = null;
			}
		}
		/// <summary>
		/// Добавляет связь с родителским элементом
		/// </summary>
		/// <param name="parent">Родительский элемент.</param>
		protected internal void LinkParent(GraphItem parent)
		{
		 if(_ps != null)
			 _ps.Add(parent);
			else if(_p == null)
			 _p = parent;
			else
			{
				_ps = new ElasticArray<GraphItem>() { _p, parent };
			 _p = null;
			}
		}
		/// <summary>
		/// Добавляет связь с дочерним элементом
		/// </summary>
		/// <param name="child">Дочерний элемент.</param>
		protected internal void LinkChild(GraphItem child)
		{
		 if(_chs != null)
				_chs.Add(child);
			else if(_ch == null)
				_ch = child;
			else
			{
				_chs = new ElasticArray<GraphItem>() { _ch, child };
				_ch = null;
			}
		}
		/// <summary>
		/// Добавляет связь с дочерним элементом в начало списка связей
		/// </summary>
		/// <param name="child">Дочерний элемент.</param>
		protected internal void LinkChildFirst(GraphItem child)
		{
			if(_chs != null)
				_chs.Insert(0, child);
			else if(_ch == null)
				_ch = child;
			else
			{
				_chs = new ElasticArray<GraphItem>() {child,  _ch };
				_ch = null;
			}
		}
		/// <summary>
		/// Удаляет связь с родителским элементом
		/// </summary>
		/// <param name="parent">Родительский элемент.</param>
		protected internal void UnlinkParent(GraphItem parent)
		{
			if(Object.Equals(_p, parent))
			 _p = null;
			else if(_ps != null)
			{
				_ps.Remove(parent);
				if(_ps.Length == 0)
				 _ps = null;
				else if(_ps.Length == 1)
				{
					_p = _ps[0];
					_ps = null;
				}
			}
		}
		/// <summary>
		/// Удаляет связь с дочерним элементом
		/// </summary>
		/// <param name="child">Дочерний элемент.</param>
		protected internal void UnlinkChild(GraphItem child)
		{
			if(Object.Equals(_ch, child))
				_ch = null;
			else if(_chs != null)
			{
				_chs.Remove(child);
				if(_chs.Length == 0)
					_chs = null;
				else if(_chs.Length == 1)
				{
					_ch = _chs[0];
					_chs = null;
				}
			}
		}
		/// <summary>
		/// ToString
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ItemText;
		}
		#endregion << Protected & Override Methods >>
		//-------------------------------------------------------------------------------------
		#region << Private Methods >>
		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		{
			if(_ob != null && _ob is IObjectChangeNotify)
			{
				((IObjectChangeNotify)_ob).ObjectChanging += Object_ObjectChanging;
				((IObjectChangeNotify)_ob).ObjectChanged += Object_ObjectChanged;
			}
			if(_pars != null)
			{
				_pars.ObjectChanging += Params_ObjectChanging;
				_pars.ObjectChanged += Params_ObjectChanged;
			}
			foreach(GraphItem p in Parents)
			 p.LinkChildFirst(this);
		}
		//-------------------------------------------------------------------------------------
		void Object_ObjectChanging(object sender, ChangeNotifyEventArgs e)
		{
			OnObjectChanging(new ObjectChangeNotifyEventArgs(this,	ChangeNotifyAction.ObjectChange,	e.SourceArgs ?? e,
				 																																															"Object",		sender,	null));
		}
		void Object_ObjectChanged(object sender, ChangeNotifyEventArgs e)
		{
			OnObjectChanged(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange, e.SourceArgs ?? e,
																																																			"Object", sender, null));
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
		#endregion << Private Methods >>
		//-------------------------------------------------------------------------------------


	}
}

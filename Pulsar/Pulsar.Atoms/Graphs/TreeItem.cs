using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public interface ITreeItem : IGraphItem >>
	/// <summary>
	/// Интерфейс элемента дерева
	/// </summary>
	public interface ITreeItem : IGraphItem
	{
		/// <summary>
		/// Возвращает уровень вложенности элемента в дереве.
		/// </summary>
		uint Level { get; }
		/// <summary>
		/// Родительский элемент.
		/// </summary>
		new ITreeItem Parent { get; }
		/// <summary>
		/// Список дочерних элементов.
		/// </summary>
		new IEnumerable<ITreeItem> Children { get; }
		/// <summary>
		/// Возвращает родительский элемент с указанным уровнем
		/// </summary>
		/// <param name="level">Уровенб родительского элемента</param>
		/// <returns></returns>
		ITreeItem GetParentWithLevel(int level);
		/// <summary>
		/// Порядок, используемый при сортировке
		/// </summary>
		object SortOrder { get; set; }
	} 
	#endregion << public interface ITreeItem : IGraphItem >>
	//*************************************************************************************
	#region << public class TreeItem<T> : GraphItem<T> >>
	/// <summary>
	/// Класс элемента дерева.
	/// </summary>
	/// <typeparam name="T">Тип объекта элемента дерева.</typeparam>
	public class TreeItem<T> : GraphItem, ITreeItem, IObjectChangeNotify, IDisposable
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Объект элемента
		/// </summary>
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
		public new T Object
		{
			get { return (T)base.Object; }
		}
		/// <summary>
		/// Возвращает уровень вложенности элемента в дереве.
		/// </summary>
		public uint Level
		{
			get { return Parent == null ? 0 : Parent.Level + 1; ; }
		}
		/// <summary>
		/// Родительский элемент. Если родителей несколько, возвращает первый родительский элемент
		/// </summary>
		public new TreeItem<T> Parent
		{
			get { return (TreeItem<T>)base.Parent; }
		}
		ITreeItem ITreeItem.Parent	{ get { return Parent; } }
		/// <summary>
		/// Перечисление дочерних элементов.
		/// </summary>
		public new IEnumerable<TreeItem<T>> Children
		{
			get { return base.Children.Cast<TreeItem<T>>(); }
		}
		IEnumerable<ITreeItem> ITreeItem.Children { get { return Children; } }
		/// <summary>
		/// Порядок, используемый при сортировке
		/// </summary>
		public object SortOrder 
		{
			get { return (Params == null) ? null : Params["SortOrder"]; }
			set 
			{ 
			 if(Params == null)
				 Params = new ParamsDic(1);
			 Params["SortOrder"] = value;
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public TreeItem(T obj)	: base(obj)	{ }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public TreeItem(T obj, string altText)	: base(obj, altText)	{ 	}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << GraphItem<T> overrides >>
		public override bool IsChangeEventsOff
		{
			get
			{
			 if(Parent == null)
				 return base._isEventOff;
				return base._isEventOff ? true : Parent.IsChangeEventsOff;
			}
		}
		#endregion << GraphItem<T> overrides >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Возвращает родительский элемент с указанным уровнем
		/// </summary>
		/// <param name="level">Уровенб родительского элемента</param>
		/// <returns></returns>
		public TreeItem<T> GetParentWithLevel(int level)
		{
			if(IsRoot)
				return null;
			TreeItem<T> p = Parent;
			while(p != null)
			{
				if(p.Level == level)
					return p;
				p = p.Parent;
			}
			return null;
		}
		ITreeItem ITreeItem.GetParentWithLevel(int level)
		{
	 	return GetParentWithLevel(level);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
						
	} 
	#endregion << public class TreeItem<T> : GraphItem<T> >>
}

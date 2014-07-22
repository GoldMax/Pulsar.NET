using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pulsar;
using Pulsar.Reflection;

namespace Sim.Refs
{
 /// <summary>
 /// Класс дерева ресурсов.
 /// </summary>
	public class PulsarResourceTree	: KeyedTree<PulsarResource>
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает тип ресурсов дерева.
		/// </summary>
		public PulsarResourceType ResourcesType
		{
			get { return (PulsarResourceType)this.Params["Type"]; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		internal PulsarResourceTree() : base()	{ }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public PulsarResourceTree(PulsarResourceType type) : base()
		{
			var root = new KeyedTreeItem<PulsarResource>(null);
			root.ItemText = EnumTypeConverter.GetItemDisplayName(((PulsarResourceType)type));
			this.Params = new ParamsDic(1);
			this.Params["Type"] = type;
			this.Add(root, (IGraphItem)null);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
}

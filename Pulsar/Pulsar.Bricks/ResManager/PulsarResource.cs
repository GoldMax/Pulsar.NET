using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;

using Pulsar;
using Pulsar.Reflection;
using Pulsar.Serialization;
using Pulsar.Server;

namespace Sim.Refs
{
	/// <summary>
	/// Класс внешнего ресурса
	/// </summary>
	public class PulsarResource : GlobalObject
	{
		private PulsarResourceType t = PulsarResourceType.Unknown;
		private PulsarResourceContentType c = PulsarResourceContentType.Unknown;
		private string d = null;
		[PulsarNonSerialized(PulsarSerializationMode.OnSave)]
		private ResourceContent r = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Тип ресурса.
		/// </summary>
		public PulsarResourceType Type
		{
			get { BeginRead(); return t; }
			internal set { BeginWrite(); t = value; }
		}
		/// <summary>
		/// Тип контента ресурса.
		/// </summary>
		public PulsarResourceContentType ContentType
		{
			get { BeginRead(); return c; }
			set
			{
				BeginWrite();
				var arg = OnObjectChanging("ContentType", value, c);
				c = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Описание ресурса.
		/// </summary>
		public string Description
		{
			get { BeginRead(); return d; }
			set
			{
				BeginWrite();
				var arg = OnObjectChanging("Description", value, d);
				d = value;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Объект значения ресурса
		/// </summary>
		public object Value
		{
			get { BeginRead(); return r == null ? null : r.Value; }
			set
			{
				BeginWrite();
				var arg = OnObjectChanging("Value", value, r);
				if(r == null)
					r = new ResourceContent();
				if(r != null && r.Value is IObjectChangeNotify)
					((IObjectChangeNotify)r.Value).ObjectChanged -= ResourceValue_ObjectChanged;
				r.Value = value;
				if(r != null && r.Value is IObjectChangeNotify)
					((IObjectChangeNotify)r.Value).ObjectChanged += ResourceValue_ObjectChanged;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Объект контента 
		/// </summary>
		public ResourceContent Content
		{
			get { BeginRead(); return r; }
			set 
			{
				BeginWrite();
				var arg = OnObjectChanging("Content", value, r);
				if(r != null && r.Value is IObjectChangeNotify)
					((IObjectChangeNotify)r.Value).ObjectChanged -= ResourceValue_ObjectChanged;
				r = value;
				if(r != null && r.Value is IObjectChangeNotify)
					((IObjectChangeNotify)r.Value).ObjectChanged += ResourceValue_ObjectChanged;
				OnObjectChanged(arg);
			}
		}
		/// <summary>
		/// Определяет, инициализирован ли ресурс.
		/// </summary>
		public bool IsInit
		{
			get { BeginRead(); return r != null; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		private PulsarResource() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="type">Тип ресурса.</param>
		public PulsarResource(PulsarResourceType type)
		{
			t = type;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString
		/// </summary>
		/// <returns></returns>
		[NoLock]
		public override string ToString()
		{
			return String.Format("{0}", d ?? EnumTypeConverter.GetItemDisplayName(t));
		}
		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		{
			if(r != null && r.Value is IObjectChangeNotify)
				((IObjectChangeNotify)r.Value).ObjectChanged += ResourceValue_ObjectChanged;
		}
		private void ResourceValue_ObjectChanged(object sender, ObjectChangeNotifyEventArgs e)
		{
			OnObjectChanged("Value", r == null ? null : r.Value, null);
		}
		#endregion << Methods >>
		//*************************************************************************************
		/// <summary>
		/// Класс контента ресурса
		/// </summary>
		public class ResourceContent 
		{
			private object val = null;
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			#region << Properties >>
			/// <summary>
			/// Объект - значение
			/// </summary>
			public object Value
			{
				get { return val; }
				set { val = value; }
			}
			#endregion << Properties >>
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar				
{
	//**************************************************************************************
	#region << public interface ICollectionChangeNotify >>
	/// <summary>
	/// Интерфейс, определяющий события изменения коллекции.
	/// </summary>
	public interface ICollectionChangeNotify	: IObjectChangeNotify
	{
		/// <summary>
		/// Событие, возникающее при добавлении элемента в коллекцию.
		/// </summary>
		event EventHandler<CollectionChangeNotifyEventArgs> ItemAdding;
		/// <summary>
		/// Событие, возникающее после добавления элемента в коллекцию.
		/// </summary>
		event EventHandler<CollectionChangeNotifyEventArgs> ItemAdded;
		/// <summary>
		/// Событие, возникающее при изменении элемента коллекции.
		/// </summary>
		event EventHandler<CollectionChangeNotifyEventArgs> ItemChanging;
		/// <summary>
		/// Событие, возникающее после изменения элемента коллекции.
		/// </summary>
		event EventHandler<CollectionChangeNotifyEventArgs> ItemChanged;
		/// <summary>
		/// Событие, возникающее при удалении элемента из коллекции.
		/// </summary>
		event EventHandler<CollectionChangeNotifyEventArgs> ItemDeleting;
		/// <summary>
		/// Событие, возникающее после удаления элемента из коллекции.
		/// </summary>
		event EventHandler<CollectionChangeNotifyEventArgs> ItemDeleted;
		///// <summary>
		///// Событие, возникающее при тотальном изменении коллекции.
		///// </summary>
		//event EventHandler<ChangeNotifyEventArgs> CollectionResetting;
		///// <summary>
		///// Событие, возникающее после тотального изменения коллекции.
		///// </summary>
		//event EventHandler<ChangeNotifyEventArgs> CollectionResetted;
	}
	#endregion << public interface ICollectionChangeNotify >>
	//**************************************************************************************
	#region << public class CollectionChangeNotify : ObjectChangeNotify, ICollectionChangeNotify >>
	/// <summary>
	/// Класс реализации интерфейса ICollectionChangeNotify
	/// </summary>
	[Serializable]
	public abstract class CollectionChangeNotify : ObjectChangeNotify, ICollectionChangeNotify
	{
		#region ItemAdding
		[NonSerialized]
		private WeakEvent<CollectionChangeNotifyEventArgs> itemAdding;
		/// <summary>
		/// Событие, возникающее при добавлении элемента в коллекцию.
		/// </summary>
		public event EventHandler<CollectionChangeNotifyEventArgs> ItemAdding
		{
			add { lock(SyncRoot) itemAdding += value; }
			remove { lock(SyncRoot) itemAdding -= value; }
		}
		/// <summary>
		/// Вызывает событие ItemAdding.
		/// </summary>
		protected virtual void OnItemAdding(object item, int itemIndex = -1)
		{
			if(IsChangeEventsOff == false)
				OnItemAdding(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemAdd, item, item, itemIndex));
		}
		/// <summary>
		/// Вызывает событие ItemAdding.
		/// </summary>
		protected virtual void OnItemAdding(CollectionChangeNotifyEventArgs args)
		{
			if(IsChangeEventsOff)
			 return;
			if(args == null)
				throw new ArgumentNullException("args");
			if(args.Sender == null)
				args.Sender = this;
			lock(SyncRoot)
			{
				if(itemAdding != null)
					itemAdding.Raise(this, args);
		 	OnObjectChanging(new ObjectChangeNotifyEventArgs(this, args.Action, args.SourceArgs ?? args));
			}
		}
		#endregion ItemAdding
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region ItemAdded
		[NonSerialized]
		private WeakEvent<CollectionChangeNotifyEventArgs> itemAdded;
		/// <summary>
		/// Событие, возникающее после добавления элемента в коллекцию.
		/// </summary>
		public event EventHandler<CollectionChangeNotifyEventArgs> ItemAdded
		{
			add { lock(SyncRoot) itemAdded += value; }
			remove { lock(SyncRoot) itemAdded -= value; }
		}
		/// <summary>
		/// Вызывает событие ItemAdded.
		/// </summary>
		protected virtual void OnItemAdded(object item, int itemIndex = -1)
		{
			if(IsChangeEventsOff == false)
				OnItemAdded(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemAdd, item, item, itemIndex));
		}
		/// <summary>
		/// Вызывает событие ItemAdded.
		/// </summary>
		protected virtual void OnItemAdded(CollectionChangeNotifyEventArgs args)
		{
			if(IsChangeEventsOff)
				return;
			if(args == null)
				throw new ArgumentNullException("args");
			if(args.Sender == null)
				args.Sender = this;
			lock(SyncRoot)
			{
				if(itemAdded != null)
					itemAdded.Raise(this, args);
				OnObjectChanged(new ObjectChangeNotifyEventArgs(this, args.Action, args.SourceArgs ?? args));
			}
		}
		#endregion ItemAdded
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region ItemChanging
		[NonSerialized]
		private WeakEvent<CollectionChangeNotifyEventArgs> itemChanging;
		/// <summary>
		/// Событие, возникающее при изменении элемента коллекции.
		/// </summary>
		public event EventHandler<CollectionChangeNotifyEventArgs> ItemChanging
		{
			add { lock(SyncRoot) itemChanging += value; }
			remove { lock(SyncRoot) itemChanging -= value; }
		}
		/// <summary>
		/// Вызывает событие ItemChanging.
		/// </summary>
		protected virtual void OnItemChanging(object item, object newItem, int itemIndex = -1)
		{
			if(IsChangeEventsOff == false)
			 OnItemChanging(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemChange, item, null, itemIndex));
		}
		/// <summary>
		/// Вызывает событие ItemChanging.
		/// </summary>
		protected virtual void OnItemChanging(CollectionChangeNotifyEventArgs args)
		{
			if(IsChangeEventsOff)
			 return;
			if(args == null)
				throw new ArgumentNullException("args");
			if(args.Sender == null)
				args.Sender = this;
	  lock(SyncRoot)
			{
				if(itemChanging != null)
					itemChanging.Raise(this, args);
				OnObjectChanging(new ObjectChangeNotifyEventArgs(this, args.Action, args.SourceArgs ?? args));
			}
		}
		#endregion ItemChanging
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region ItemChanged
		[NonSerialized]
		private WeakEvent<CollectionChangeNotifyEventArgs> itemChanged;
		/// <summary>
		/// Событие, возникающее после изменения элемента коллекции.
		/// </summary>
		public event EventHandler<CollectionChangeNotifyEventArgs> ItemChanged
		{
			add { lock(SyncRoot) itemChanged += value; }
			remove { lock(SyncRoot) itemChanged -= value; }
		}
		/// <summary>
		/// Вызывает событие ItemChanged.
		/// </summary>
		protected virtual void OnItemChanged(object item, object newItem, int itemIndex = -1)
		{
			if(IsChangeEventsOff == false)
				OnItemChanged(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemChange, item, null, itemIndex));
		}
		/// <summary>
		/// Вызывает событие ItemChanged.
		/// </summary>
		protected virtual void OnItemChanged(CollectionChangeNotifyEventArgs args)
		{
			if(IsChangeEventsOff)
			 return;
			if(args == null)
				throw new ArgumentNullException("args");
			if(args.Sender == null)
				args.Sender = this;
			lock(SyncRoot)
			{
				if(itemChanged != null)
					itemChanged.Raise(this, args);
				OnObjectChanged(new ObjectChangeNotifyEventArgs(this, args.Action, args.SourceArgs ?? args));
			}
		}
		#endregion ItemChanged
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region ItemDeleting
		[NonSerialized]
		private WeakEvent<CollectionChangeNotifyEventArgs> itemDeleting;
		/// <summary>
		/// Событие, возникающее при удалении элемента из коллекции.
		/// </summary>
		public event EventHandler<CollectionChangeNotifyEventArgs> ItemDeleting
		{
			add { lock(SyncRoot) itemDeleting += value; }
			remove { lock(SyncRoot) itemDeleting -= value; }
		}
		/// <summary>
		/// Вызывает событие ItemDeleting.
		/// </summary>
		protected virtual void OnItemDeleting(object item, int itemIndex = -1)
		{
			if(IsChangeEventsOff == false)
				OnItemDeleting(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemDelete, item, null, itemIndex));
		}
		/// <summary>
		/// Вызывает событие ItemDeleting.
		/// </summary>
		protected virtual void OnItemDeleting(CollectionChangeNotifyEventArgs args)
		{
			if(IsChangeEventsOff)
			 return;
			if(args == null)
				throw new ArgumentNullException("args");
			if(args.Sender == null)
				args.Sender = this;
			if(IsChangeEventsOff == false)
			{
				if(itemDeleting != null)
					itemDeleting.Raise(this, args);
				OnObjectChanging(new ObjectChangeNotifyEventArgs(this, args.Action, args.SourceArgs ?? args));
			}
		}
		#endregion ItemDeleting
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region ItemDeleted
		[NonSerialized]
		private WeakEvent<CollectionChangeNotifyEventArgs> itemDeleted;
		/// <summary>
		/// Событие, возникающее после удаления элемента из коллекции.
		/// </summary>
		public event EventHandler<CollectionChangeNotifyEventArgs> ItemDeleted
		{
			add { lock(SyncRoot) itemDeleted += value; }
			remove { lock(SyncRoot) itemDeleted -= value; }
		}
		/// <summary>
		/// Вызывает событие ItemDeleted.
		/// </summary>
		protected virtual void OnItemDeleted(object item, int itemIndex = -1)
		{
			if(IsChangeEventsOff == false)
				OnItemDeleted(new CollectionChangeNotifyEventArgs(this, ChangeNotifyAction.ItemDelete, item, null, itemIndex));
		}
		/// <summary>
		/// Вызывает событие ItemDeleted.
		/// </summary>
		protected virtual void OnItemDeleted(CollectionChangeNotifyEventArgs args)
		{
			if(IsChangeEventsOff)
			 return;
			if(args == null)
				throw new ArgumentNullException("args");
			if(args.Sender == null)
				args.Sender = this;
			if(IsChangeEventsOff == false)
			{
				if(itemDeleted != null)
					itemDeleted.Raise(this, args);
				OnObjectChanged(new ObjectChangeNotifyEventArgs(this, args.Action, args.SourceArgs ?? args));
			}
		}
		#endregion ItemDeleted
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	} 
	#endregion << public class CollectionChangeNotify : ObjectChangeNotify, ICollectionChangeNotify >>
	//***************************************************************************************

}

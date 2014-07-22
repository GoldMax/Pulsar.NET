using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public interface IObjectChangeNotify >>
	/// <summary>
	/// Интерфейс объектов, посылающих уведомления об изменении объекта.
	/// </summary>
	public interface IObjectChangeNotify
	{
		/// <summary>
		/// Определяет, отключена ли генерация сообщений об изменении объекта.
		/// </summary>
		bool IsChangeEventsOff { get; }
		/// <summary>
		/// Отключает генерацию сообщений об изменении объекта.
		/// </summary>
		/// <param name="raiseResetting">Определяет необходимость генерации события тотального изменения.</param>
		void EventsOff(bool raiseResetting = true);
		/// <summary>
		/// Включает генерацию сообщений об изменении объекта.
		/// </summary>
		/// <param name="raiseResetted">Определяет необходимость генерации события тотального изменения.</param>
		void EventsOn(bool raiseResetted = true);
		/// <summary>
		/// Объект синхронизации.
		/// </summary>
		object SyncRoot { get; }
		/// <summary>
		/// Событие, возникающее при изменении объекта.
		/// </summary>
		event EventHandler<ObjectChangeNotifyEventArgs> ObjectChanging;
		/// <summary>
		/// Событие, возникающее после изменении объекта.
		/// </summary>
		event EventHandler<ObjectChangeNotifyEventArgs> ObjectChanged;
		///// <summary>
		///// Событие, возникающее до тотального изменении объекта.
		///// </summary>
		//event EventHandler<ChangeNotifyEventArgs> ObjectResetting;
		///// <summary>
		///// Событие, возникающее после тотального изменения объекта.
		///// </summary>
		//event EventHandler<ChangeNotifyEventArgs> ObjectResetted;
	}
	#endregion << public interface IObjectChangeNotify >>
	//**************************************************************************************
	#region << public class ObjectChangeNotify : IObjectChangeNotify >>
	/// <summary>
	/// Класс реализации интерфейса IObjectChangeNotify
	/// </summary>
	public abstract class ObjectChangeNotify : IObjectChangeNotify
	{
		[NonSerialized]
		private object _xxx = new object();
		[NonSerialized]
		protected bool _isEventOff = false;
		/// <summary>
		/// Объект синхронизации.
		/// </summary>
		[Browsable(false)]
		public object SyncRoot
		{
			get 
			{
				if(_xxx == null)
					return _xxx = _xxx ?? new object(); 
				return _xxx;
			}
		}
		//-------------------------------------------------------------------------------------
		#region ObjectChanging
		///
		[NonSerialized]
		protected WeakEvent<ObjectChangeNotifyEventArgs> _objectChanging = null;
		/// <summary>
		/// Событие, возникающее до изменения объекта.
		/// </summary>
		public event EventHandler<ObjectChangeNotifyEventArgs> ObjectChanging
		{
			add { lock(SyncRoot) _objectChanging += value; }
			remove { lock(SyncRoot) _objectChanging -= value; }
		}
		/// <summary>
		/// Вызывает событие ObjectChanging.
		/// </summary>
		/// <param name="memberName">Имя изменяемого члена класса объекта.</param>
		/// <param name="oldValue">Старое значение члена класса.</param>
		/// <param name="newValue">Новое значение изменяемого члена класса.</param>
		protected virtual ObjectChangeNotifyEventArgs OnObjectChanging(string memberName, object newValue,
																																																																	object oldValue)
		{
			var args = new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange,
																																																				memberName, newValue, oldValue);
			OnObjectChanging(args);
			return args;
		}
		/// <summary>
		/// Вызывает событие ObjectChanging.
		/// </summary>
		/// <param name="args">Аргументы события</param>
		protected virtual void OnObjectChanging(ObjectChangeNotifyEventArgs args)
		{
			if(args == null)
				throw new ArgumentNullException("args");
			if(args.Sender == null)
				args.Sender = this;
			lock(SyncRoot)
				if(_objectChanging != null && IsChangeEventsOff == false)
					_objectChanging.Raise(this, args);
		} 
		#endregion ObjectChanging
		//-------------------------------------------------------------------------------------
		#region ObjectChanged
		///
		[NonSerialized]
		protected WeakEvent<ObjectChangeNotifyEventArgs> _objectChanged = null;
		/// <summary>
		/// Событие, возникающее после изменения объекта.
		/// </summary>
		public event EventHandler<ObjectChangeNotifyEventArgs> ObjectChanged
		{
			add { lock(SyncRoot) _objectChanged += value; }
			remove { lock(SyncRoot) _objectChanged -= value; }
		}
		/// <summary>
		/// Вызывает событие ObjectChanged.
		/// </summary>
		/// <param name="memberName">Имя измененного члена класса объекта.</param>
		/// <param name="oldValue">Старое значение члена класса.</param>
		/// <param name="newValue">Новое значение изменяемого члена класса.</param>
		protected virtual void OnObjectChanged(string memberName, object newValue, object oldValue)
		{
			var args = new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange,
																																																	memberName, newValue, oldValue);
			OnObjectChanged(args);
		}
		/// <summary>
		/// Вызывает событие OnObjectChanged.
		/// </summary>
		/// <param name="args">Аргументы события</param>
		protected virtual void OnObjectChanged(ObjectChangeNotifyEventArgs args)
		{
			if(args == null)
				return;
			if(args.Sender == null)
				args.Sender = this;
			lock(SyncRoot)
				if(_objectChanged != null && IsChangeEventsOff == false)
					_objectChanged.Raise(this, args);
		} 
		#endregion ObjectChanged
		//-------------------------------------------------------------------------------------
		#region ObjectResetting
		/// <summary>
		/// Вызывает событие ObjectResetting.
		/// </summary>
		protected virtual void OnObjectResetting()
		{
			OnObjectChanging(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset));
		}
		/// <summary>
		/// Вызывает событие ObjectResetting.
		/// </summary>
		/// <param name="args">Аргументы события</param>
		protected virtual void OnObjectResetting(ObjectChangeNotifyEventArgs args)
		{
			OnObjectChanging(args);
		} 
		#endregion ObjectResetting
		//-------------------------------------------------------------------------------------
		#region ObjectResetted
		/// <summary>
		/// Вызывает событие ObjectResetted.
		/// </summary>
		protected virtual void OnObjectResetted()
		{
			OnObjectChanged(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset, null));
		}
		/// <summary>
		/// Вызывает событие ObjectResetted.
		/// </summary>
		/// <param name="args">Аргументы события</param>
		protected virtual void OnObjectResetted(ObjectChangeNotifyEventArgs args)
		{
			OnObjectChanged(args);
		}
		#endregion ObjectResetted
		//-------------------------------------------------------------------------------------
		#region IObjectChangeNotify Members
		/// <summary>
		/// Определяет, отключена ли генерация сообщений об изменении объекта.
		/// </summary>
		[Browsable(false)]
		public virtual bool IsChangeEventsOff
		{
			get { lock(SyncRoot) return _isEventOff; }
			private set { lock(SyncRoot)	_isEventOff = value; }
		}
		/// <summary>
		/// Отключает генерацию сообщений об изменении объекта.
		/// </summary>
		/// <param name="raiseResetting">Определяет необходимость генерации события тотального изменения.</param>
		public virtual void EventsOff(bool raiseResetting = true)
		{
			lock(SyncRoot)
			{
				if(raiseResetting)
					OnObjectResetting();
				_isEventOff = true;
			}
		}
		/// <summary>
		/// Включает генерацию сообщений об изменении объекта.
		/// </summary>
		/// <param name="raiseResetted">Определяет необходимость генерации события тотального изменения.</param>
		public virtual void EventsOn(bool raiseResetted = true)
		{
			lock(SyncRoot)
			{
				_isEventOff = false;
				if(raiseResetted)
					OnObjectResetted();
			}
		}
		#endregion
	} 
	#endregion << public class ObjectChangeNotify : IObjectChangeNotify >>

}

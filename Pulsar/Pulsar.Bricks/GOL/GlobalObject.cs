using System;
using System.ComponentModel;
using System.Reflection;
//using System.Threading;

using Pulsar.Reflection.Dynamic;
using Pulsar.Server;

namespace Pulsar
{
	#region << public abstract class GOLObject >>
	/// <summary>
	/// Базовый класс глобальных объектов.
	/// </summary>
	public abstract class GlobalObject : IObjectChangeNotify,	IReadWriteLockObject, IGlobalObjectMeta
	{
	 [PulsarNonSerialized(PulsarSerializationMode.All ^ PulsarSerializationMode.Backup)]
		private OID _oid;
		private string _gn = null;
		[NonSerialized]
		private Locker _locker = null;
		// 1 : IsInitialized
		// 2 : IsEventsOff
		[NonSerialized]
		private byte _flags = 0;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		///  Идентификатор объекта.
		/// </summary>
		[Browsable(false)]
		[DisplayOrder(999999)]
		public OID OID
		{
			get { return _oid; }
			internal set { _oid = value; }
		}
		/// <summary>
		/// Глобальное имя объекта
		/// </summary>
		string IGlobalObjectMeta.GlobalName
		{
			get 
			{
			 //BeginRead(); 
			 return _gn; 
			}
			set
			{	
			 if(value == _gn)
				 return;
				//BeginWrite();
			 string old = _gn;
				var arg = OnObjectChanging("GlobalName",value,_gn);
				_gn = value;
				OnObjectChanged(arg); 
				GOL.ChangeGlobalName(this, old, _gn);
			}
		}
		/// <summary>
		/// Объект синхронизации.
		/// </summary>
		[Browsable(false)]
		public object SyncRoot
		{
			get { return _oid; }
		}
		/// <summary>
		/// Определяет, иниицализирован ли объект.
		/// Метод set только устанавливает, не снимает , флаг.
		/// </summary>
		[Browsable(false)]
		public bool IsInitialized
		{
			get { lock(_oid) return (_flags & 1) == 1; }
			internal set { lock(_oid) _flags = (byte)(_flags | 1); }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		protected GlobalObject()
		{
			if(_oid == null)
				_oid = new OID();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Equals(object obj)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(obj == null || Object.Equals(_oid, null) || this.GetType() != obj.GetType())
				return false;
			return this._oid == ((GlobalObject)obj).OID;
		}
		/// <summary>
		/// ==
		/// </summary>
		/// <param name="pObj"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool operator ==(GlobalObject pObj, object obj)
		{
			if(Object.Equals(pObj, null) && Object.Equals(obj, null))
				return true;
			if(Object.Equals(pObj, null) || Object.Equals(obj, null))
				return false;
			return pObj.Equals(obj);
		}
		/// <summary>
		/// ==
		/// </summary>
		/// <param name="pObj"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool operator !=(GlobalObject pObj, object obj)
		{
			if(Object.Equals(pObj, null) && Object.Equals(obj, null))
				return false;
			if(Object.Equals(pObj, null) || Object.Equals(obj, null))
				return true;
			return !pObj.Equals(obj);
		}
		/// <summary>
		///  GetHashCode()
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			if(Object.Equals(_oid,null))
				return 0;
			return _oid.GetHashCode();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обновляет объект
		/// </summary>
		/// <param name="box"></param>
		public void Update(TransBox box)
		{
			if(box == null)
			 throw new ArgumentNullException("box");
		 try
			{
			 BeginWrite();
				OnObjectResetting();
			 box.Unpack(this);
				OnObjectResetted();
			}
			finally
			{
				EndWrite();
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << GlobalObject Methods >>
		/// <summary>
		/// Метод установки флага чтения обслуживаемого объекта.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void BeginRead()	
		{ 
			if(GOL.OnGlobalObjectBeginRead != null)
			 GOL.OnGlobalObjectBeginRead(this, ref _locker);
		}
		/// <summary>
		/// Метод снятия флага чтения обслуживаемого объекта.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void EndRead() 
		{
			if(GOL.OnGlobalObjectEndRead != null)
				GOL.OnGlobalObjectEndRead(this, ref _locker);
		}
		/// <summary>
		/// Метод установки флага изменения обслуживаемого объекта.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void BeginWrite() 
		{
			if(GOL.OnGlobalObjectBeginWrite != null)
				GOL.OnGlobalObjectBeginWrite(this, ref _locker);
		}
		/// <summary>
		/// Метод снятия флага изменения обслуживаемого объекта.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void EndWrite() 
		{
			if(GOL.OnGlobalObjectEndWrite != null)
				GOL.OnGlobalObjectEndWrite(this, ref _locker);
		}
		/// <summary>
		/// Определяет, находится ли объект в блокировке записи.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool IsWriteLocked	
		{ 
			get 
			{
				lock(_oid)
					return _locker == null ? false : _locker.IsWriteLockHeld; 
			} 
		}
		/// <summary>
		/// Определяет, находится ли объект в блокировке чтения.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool IsReadLocked
		{
			get
			{
				lock(_oid)
					return _locker == null ? false : _locker.IsReadLockHeld;
			}
		}
		/// <summary>
		/// Определяет, находится ли объект в блокировке.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool IsLocked
		{
			get
			{
				lock(_oid)
					return _locker == null ? false : (_locker.IsWriteLockHeld || _locker.IsReadLockHeld); ;
			}
		}
		/// <summary>
		/// Метод снятия всех блокировок объекта.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ClearAllLocks()
		{
			if(GOL.OnGlobalObjectClearAllLocks != null)
				GOL.OnGlobalObjectClearAllLocks(this, ref _locker);
		}
		#endregion << GlobalObject Methods >>
		//-------------------------------------------------------------------------------------
		#region << IObjectChangeNotify Members >>
		#region ObjectChanging
		///
		[NonSerialized]
		protected WeakEvent<ObjectChangeNotifyEventArgs> _objectChanging = null;
		/// <summary>
		/// Событие, возникающее до изменения объекта.
		/// </summary>
		public event EventHandler<ObjectChangeNotifyEventArgs> ObjectChanging
		{
			add { lock(_oid) _objectChanging += value; }
			remove { lock(_oid) _objectChanging -= value; }
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
			lock(_oid)
			{
				if(IsChangeEventsOff == false)
			  MessageBus.Polling(this, EventMessageReasons.ObjectChanging, args);
				if(_objectChanging != null && IsChangeEventsOff == false)
					_objectChanging.Raise(this, args);
			}
		}
		#endregion ObjectChanging
		//-------------------------------------------------------------------------------------
		#region ObjectChanged
		///
		[NonSerialized]
		protected WeakEvent<ObjectChangeNotifyEventArgs> objectChanged = null;
		/// <summary>
		/// Событие, возникающее после изменения объекта.
		/// </summary>
		public event EventHandler<ObjectChangeNotifyEventArgs> ObjectChanged
		{
			add { lock(_oid) objectChanged += value; }
			remove { lock(_oid) objectChanged -= value; }
		}
		/// <summary>
		/// Вызывает событие ObjectChanged.
		/// </summary>
		/// <param name="memberName">Имя измененного члена класса объекта.</param>
		/// <param name="oldValue">Старое значение члена класса.</param>
		/// <param name="newValue">Новое значение изменяемого члена класса.</param>
		protected virtual void OnObjectChanged(string memberName, object newValue, object oldValue)
		{
			OnObjectChanged(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange,
																																																			memberName, newValue, oldValue));
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
			lock(_oid)
			{
				if(objectChanged != null && IsChangeEventsOff == false)
					objectChanged.Raise(this, args);
				if(IsChangeEventsOff == false)
					MessageBus.Notify(this, EventMessageReasons.ObjectChanged, args);
			}
		}
		#endregion ObjectChanged
		//-------------------------------------------------------------------------------------
		#region ObjectResetting
		/// <summary>
		/// Вызывает событие ObjectResetting.
		/// </summary>
		protected virtual void OnObjectResetting()
		{
			OnObjectResetting(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset));
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
			OnObjectResetted(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset));
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
		public bool IsChangeEventsOff
		{
			get { lock(_oid) return (_flags & 2) == 2; }
			private set { lock(_oid)	_flags = (byte)(value ? _flags | 2 :  _flags & ~2); }
		}

		/// <summary>
		/// Отключает генерацию сообщений об изменении объекта.
		/// </summary>
		/// <param name="raiseResetting">Определяет необходимость генерации события тотального изменения.</param>
		public virtual void EventsOff(bool raiseResetting = true)
		{
			lock(_oid)
			{
				if(raiseResetting)
					OnObjectResetting();
				IsChangeEventsOff = true;
			}
		}
		/// <summary>
		/// Включает генерацию сообщений об изменении объекта.
		/// </summary>
		/// <param name="raiseResetted">Определяет необходимость генерации события тотального изменения.</param>
		public void EventsOn(bool raiseResetted = true)
		{
			lock(_oid)
			{
				IsChangeEventsOff = false;
				if(raiseResetted)
					OnObjectResetted();
			}
		}
		#endregion
		#endregion << IObjectChangeNotify Members >>
		//-------------------------------------------------------------------------------------
		#region << Static Methods >>
		/// <summary>
		/// Создает объект с указанным OID.
		/// </summary>
		/// <typeparam name="T">Тип создаваемого объекта.</typeparam>
		/// <param name="oid">OID создаваемого объекта.</param>
		/// <param name="ctorParamsTypes">Типы параметров конструктора</param>
		/// <param name="ctorArgs">Аргументы конструктора. </param>
		/// <returns></returns>
		public static T CreateWithOID<T>(OID oid, Type[] ctorParamsTypes = null, object[] ctorArgs = null)
		{
			if(oid == null)
				throw new ArgumentNullException("oid");
			Type t = typeof(T);
			if(t.IsSubclassOf(typeof(GlobalObject)) == false)
				throw new PulsarException("Невозможно создать объект типа [{0}] методом CreateWithOID<T> !", t.FullName);
			object obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t);
			((GlobalObject)obj)._oid = oid;
			ReflectionHelper.InvokeCtor(obj, ctorParamsTypes, ctorArgs);
			((GlobalObject)obj)._flags = 1;
			return (T)obj;
		}
		/// <summary>
		/// Создает объект с указанным OID.
		/// </summary>
		/// <typeparam name="T">Тип создаваемого объекта.</typeparam>
		/// <param name="oid">OID создаваемого объекта.</param>
		/// <param name="ctorArgs">Аргументы конструктора. </param>
		/// <returns></returns>
		public static T CreateWithOID<T>(OID oid, params object[] ctorArgs)
		{
			if(oid == null)
				throw new ArgumentNullException("oid");
			Type t = typeof(T);
			if(t.IsSubclassOf(typeof(GlobalObject)) == false)
				throw new PulsarException("Невозможно создать объект типа [{0}] методом CreateWithOID<T> !", t.FullName);
			object obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t);
			((GlobalObject)obj)._oid = oid;
			Type[] argsTypes = new Type[ctorArgs == null ? 0 : ctorArgs.Length];
			if(ctorArgs != null)
			 for(int i = 0; i < ctorArgs.Length; i++)
				 argsTypes[i] = ctorArgs[i].GetType();
			ReflectionHelper.InvokeCtor(obj, argsTypes, ctorArgs);
			((GlobalObject)obj)._flags = 1;
			return (T)obj;
		}
		/// <summary>
		/// Создает объект с указанным OID.
		/// </summary>
		/// <param name="type">Тип создаваемого объекта.</param>
		/// <param name="oid">OID создаваемого объекта.</param>
		/// <param name="ctorParamsTypes">Типы параметров конструктора</param>
		/// <param name="ctorArgs">Аргументы конструктора. </param>
		/// <returns></returns>
		public static GlobalObject CreateWithOID(Type type, OID oid, Type[] ctorParamsTypes = null, object[] ctorArgs = null)
		{
			if(oid == null)
				throw new ArgumentNullException("oid");
			if(type == null)
				throw new ArgumentNullException("type");
			//if(oid == OID.Empty)
			// throw new ArgumentException("OID объекта не может быть пустым!","oid");
			if(type.IsSubclassOf(typeof(GlobalObject)) == false)
				throw new PulsarException("Невозможно создать объект типа [{0}] методом CreateWithOID<T> !", type.FullName);
			GlobalObject obj = (GlobalObject)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
			obj.OID = oid;
			ReflectionHelper.InvokeCtor(obj, ctorParamsTypes, ctorArgs);
			obj._flags = 1;
			return obj;
		}
		/// <summary>
		/// Создает объект с указанным OID.
		/// </summary>
		/// <param name="type">Тип создаваемого объекта.</param>
		/// <param name="oid">OID создаваемого объекта.</param>
		/// <param name="ctorParamsTypes">Типы параметров конструктора</param>
		/// <param name="ctorArgs">Аргументы конструктора. </param>
		/// <returns></returns>
		public static GlobalObject CreateUninitialized(Type type, OID oid)
		{
			if(oid == null)
				throw new ArgumentNullException("oid");
			if(type == null)
				throw new ArgumentNullException("type");
			//if(oid == OID.Empty)
			// throw new ArgumentException("OID объекта не может быть пустым!","oid");
			if(type.IsSubclassOf(typeof(GlobalObject)) == false)
				throw new PulsarException("Невозможно создать объект типа [{0}] методом CreateWithOID<T> !", type.FullName);
			GlobalObject obj = (GlobalObject)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
			obj.OID = oid;
			return obj;
		}
		#endregion << Static Methods >>
	}
	#endregion << public abstract class GOLObject >>
}

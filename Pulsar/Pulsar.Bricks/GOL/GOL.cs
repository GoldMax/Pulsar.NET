using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Pulsar.Serialization;
using Pulsar.Server;

namespace Pulsar
{
	public enum GOLStorageMode : byte
	{
		Weak,
		Hard
	}

	#region << public static class GlobalObjectsList >>
	/// <summary>
	/// Класс списка глобальных объектов Пульсара
	/// !!! ПОЛИТИКА !!! В GOL объекты обновляются
	/// </summary>
	public static class GOL 
	{
	 private static ReaderWriterLockSlim _xxx = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private static GOLStorageMode _mode = ServerParamsBase.IsServer ? GOLStorageMode.Weak : GOLStorageMode.Hard;

		private static HashSet<Type> _forceWeakRef = new HashSet<Type>();
		private static HashSet<Type> _forceHardRef = new HashSet<Type>();

		private static Dictionary<OID, Object> _dic = new Dictionary<OID, Object>(10000);
		private static Dictionary<string,OID>	_named = new Dictionary<string,OID>(100);

		////private static Dictionary<OID, List<LateDeserItem>> _late = 
		//// new Dictionary<OID, List<LateDeserItem>>(5000);

		private static Dictionary<Type,List<Func<OID, GlobalObject>>> _locators = 
			new Dictionary<Type,List<Func<OID, GlobalObject>>>();
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет режим загрузки глобальных объектов на сервере.
		/// </summary>
		public static bool IsInitMode { get; set; }
		/// <summary>
		/// Определяет режим по умолчанию хранения ссылок на глобальные объекты.
		/// </summary>
		public static GOLStorageMode	DefaultMode
		{
			get { return _mode; }
			set { _mode = value; }
		}
		/// <summary>
		/// Набор типов глобальных объектов, для которых всегда создаются мягкие ссылки
		/// </summary>
		public static HashSet<Type> ForceWeakRef
		{
			get { return _forceWeakRef; }
		}
		/// <summary>
		/// Набор типов глобальных объектов, для которых всегда создаются жесткие ссылки
		/// </summary>
		public static HashSet<Type> ForceHardRef
		{
			get { return _forceHardRef; }
		}				
		/// <summary>
		/// Количество объектов в списке
		/// </summary>
		public static int Count
		{
			get 
			{ 
				try
				{
					_xxx.EnterReadLock();
					return _dic.Count; 
				}
				finally { _xxx.ExitReadLock(); }
			}
		}
////  /// <summary>
////  /// Количестов объектов, ожидающих десериализации.
////  /// </summary>
////  public static int UninitializedCount
////  {
////   get 
////   { 
////    try
////    {
////     _xxx.EnterReadLock();
////     return _late.Count; 
////    }
////    finally { _xxx.ExitReadLock(); }
////   }
////}
////  /// <summary>
////  /// Возвращает перечисление OIDов неинициализированных объектов.
////  /// </summary>
////  public static IEnumerable<OID> UninitializedObjects
////  {
////   get 
////   { 
////    try
////    {
////     _xxx.EnterReadLock();
////     return _late.Keys.ToArray();
////    }
////    finally { _xxx.ExitReadLock(); }
////   }
////  }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Делегат метода загрузки глобальных объектов.
		/// </summary>
		public static Func<GlobalObject,bool, bool> LoadGlobalObjectMethod;
		/// <summary>
		/// Делегат метода загрузки глобальных объектов.
		/// </summary>
		public static Action<IEnumerable<GlobalObject>> LoadGlobalObjectsMethod;
		/// <summary>
		/// Делегат метода сохранения глобальных объектов.
		/// </summary>
		public static Action<GlobalObject> SaveGlobalObjectMethod;
		//-------------------------------------------------------------------------------------
		public static ReadWriteLockObjectHandler OnGlobalObjectBeginRead;
		public static ReadWriteLockObjectHandler OnGlobalObjectEndRead;
		public static ReadWriteLockObjectHandler OnGlobalObjectBeginWrite;
		public static ReadWriteLockObjectHandler OnGlobalObjectEndWrite;
		public static ReadWriteLockObjectHandler OnGlobalObjectClearAllLocks;
		[NonSerialized]
		private static Pulsar.WeakEvent<GlobalObject, EventArgs> _ObjectAdded;
		/// <summary>
		/// 
		/// </summary>
		public static event EventHandler<GlobalObject, EventArgs> ObjectAdded
		{
			add { _ObjectAdded += value; }
			remove { _ObjectAdded -= value; }
		}
		/// <summary>
		/// Вызывает событие ObjectAdded.
		/// </summary>
		private static void OnObjectAdded(GlobalObject obj, EventArgs arg)
		{
			if (_ObjectAdded != null)
				_ObjectAdded.Raise(obj, arg);
		}

		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Public Methods >>
		public static void RegistryLocator(Type type, Func<OID,GlobalObject> method)
		{
			lock(_locators)
			{
				if(_locators.ContainsKey(type) == false)
					_locators.Add(type, new List<Func<OID,GlobalObject>>());
				_locators[type].Add(method);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод загрузки глобальных объектов.
		/// </summary>
		public static bool LoadGlobalObject(GlobalObject go, bool throwInNotExists)
		{
			if(LoadGlobalObjectMethod != null)
				return LoadGlobalObjectMethod(go, throwInNotExists);
			return false;
		}
		/// <summary>
		/// Метод загрузки глобальных объектов.
		/// </summary>
		public static void LoadGlobalObjects(IEnumerable<GlobalObject> objs)
		{
			if(LoadGlobalObjectsMethod != null)
				LoadGlobalObjectsMethod(objs);
		}
		/// <summary>
		/// Метод сохранения глобальных объектов.
		/// </summary>
		public static void SaveGlobalObject(GlobalObject go)
		{
				if(SaveGlobalObjectMethod != null)
					SaveGlobalObjectMethod(go);
		}
		//--	---------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет ссылку на глобальный объект.
		/// </summary>
		/// <param name="obj">Глобальный объект.</param>
		public static void Add(GlobalObject obj)
		{
			if(obj == null || obj.OID == null)
				return;
			OID oid = obj.OID;
			try
			{
				_xxx.EnterWriteLock();
				////List<LateDeserItem> list = null;
				////if(_late.TryGetValue(oid, out list))
				////{
				//// _late.Remove(oid);
				//// if(list != null)
				////  foreach(LateDeserItem i in list)
				////   PulsarSerializer.OnFullDeser(i.cox, i.id, true);
				////}
				Object o;
				if(_dic.TryGetValue(oid, out o))
					if(o is WeakReference && ((WeakReference)o).IsAlive == false)
					{
						_dic.Remove(oid);
						o = null;
					}

				if(o == null)
				{
					Type t = obj.GetType();
					if(_forceWeakRef.Contains(t))
						_dic.Add(oid, new WeakReference(obj, false));
					else if(_forceHardRef.Contains(t))
						_dic.Add(oid, obj);
					else if(_mode == GOLStorageMode.Weak)
						_dic.Add(oid, new WeakReference(obj, false));
					else
						_dic.Add(oid, obj);
					OnObjectAdded(obj, EventArgs.Empty);
				}

				if(((IGlobalObjectMeta)obj).GlobalName != null)
					if(_named.ContainsKey(((IGlobalObjectMeta)obj).GlobalName))
						_named[((IGlobalObjectMeta)obj).GlobalName] = oid;
					else
						_named.Add(((IGlobalObjectMeta)obj).GlobalName, oid);
			}
			finally { _xxx.ExitWriteLock(); }
		}
		/// <summary>
		/// Добавляет мягкую ссылку на глобальный объект.
		/// </summary>
		/// <param name="obj">Глобальный объект.</param>
		public static void AddWeak(GlobalObject obj)
		{
			if(obj == null || obj.OID == null)
				return;
			OID oid = obj.OID;
			try
			{
				_xxx.EnterWriteLock();
				////_late.Remove(oid);
				Object o;
				if(_dic.TryGetValue(oid, out o))
					if(o is WeakReference == false || ((WeakReference)o).IsAlive == false)
					{
						_dic.Remove(oid);
						o = null;
					}

				if (o == null)
				{
					_dic.Add(oid, new WeakReference(obj, false));
					OnObjectAdded(obj, EventArgs.Empty);
				}

				if(((IGlobalObjectMeta)obj).GlobalName != null)
					if(_named.ContainsKey(((IGlobalObjectMeta)obj).GlobalName))
						_named[((IGlobalObjectMeta)obj).GlobalName] = oid;
					else
						_named.Add(((IGlobalObjectMeta)obj).GlobalName, oid);
			}
			finally { _xxx.ExitWriteLock(); }
		}
		/// <summary>
		/// Добавляет жесткую ссылку на глобальный объект.
		/// </summary>
		/// <param name="obj">Глобальный объект.</param>
		public static void AddHard(GlobalObject obj)
		{
			if(obj == null || obj.OID == null)
				return;
			OID oid = obj.OID;
			try
			{
				_xxx.EnterWriteLock();
				////_late.Remove(oid);
				Object o;
				if(_dic.TryGetValue(oid, out o))
					if(o is WeakReference)
					{
						_dic.Remove(oid);
						o = null;
					}

				if (o == null)
				{
					_dic.Add(oid, obj);
					OnObjectAdded(obj, EventArgs.Empty);
				}

				if(((IGlobalObjectMeta)obj).GlobalName != null)
					if(_named.ContainsKey(((IGlobalObjectMeta)obj).GlobalName))
						_named[((IGlobalObjectMeta)obj).GlobalName] = oid;
					else
						_named.Add(((IGlobalObjectMeta)obj).GlobalName, oid);
			}
			finally { _xxx.ExitWriteLock(); }
		}
		/////// <summary>
		/////// Добавляет объект в GOL и в список ожидающих инициализации
		/////// </summary>
		////internal static void AddUninitialized(GlobalObject obj, uint id, PulsarSerializer.DeserContext cox)
		////{
		//// if(obj == null || obj.OID == null)
		////  return;
		//// OID oid = obj.OID;
		//// try
		//// {
		////  _xxx.EnterWriteLock();
		////  Add(obj);
		////  List<LateDeserItem> list = null;
		////  if(_late.TryGetValue(oid, out list) == false)
		////  {
		////   if(cox != null)
		////    list = new List<LateDeserItem>();
		////   _late.Add(oid, list);
		////  }
		////  if(list != null)
		////   list.Add(new LateDeserItem(id, cox));
		//// }
		//// finally { _xxx.ExitWriteLock(); }
		////}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли GOL указанный объект.
		/// </summary>
		/// <param name="obj">Глобальный объект.</param>
		/// <returns></returns>
		public static bool Contains(GlobalObject obj)
		{
			if(obj == null || obj.OID == null)
				return false;
			try
			{
				_xxx.EnterReadLock();
				object o = null;
				if(_dic.TryGetValue(obj.OID, out o))
					if(o is WeakReference && ((WeakReference)o).IsAlive == false)
						return false;
					else
						return true;
				return false;
			}
			finally	{ _xxx.ExitReadLock(); }
		}
		/// <summary>
		/// Определяет, содержит ли GOL указанный объект.
		/// </summary>
		/// <param name="oid">OID глобального объекта.</param>
		/// <returns></returns>
		public static bool Contains(OID oid)
		{
			if(oid == null)
				return false;
			try
			{
				_xxx.EnterReadLock();
				object o = null;
				if(_dic.TryGetValue(oid, out o))
					if(o is WeakReference && ((WeakReference)o).IsAlive == false)
						return false;
					else
						return true;
				return false;
			}
			finally	{ _xxx.ExitReadLock(); }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает объект из GOL по OID объекта.
		/// </summary>
		/// <param name="oid"></param>
		/// <returns></returns>
		internal static GlobalObject Get(OID oid)
		{
			if(oid == null)
				return null;
			Object o  = null;
			if(_dic.TryGetValue(oid, out o))
				if(o is WeakReference)
					return ((WeakReference)o).IsAlive ? (GlobalObject)((WeakReference)o).Target : null;
				else
					return (GlobalObject)o;
			return null;
		}
		/// <summary>
		/// Возвращает объект из GOL по имени объекта.
		/// </summary>
		///<param name="globalName">Глобальное имя объекта</param>
		/// <returns></returns>
		internal static GlobalObject Get(string globalName)
		{
			if(globalName == null)
				throw new ArgumentNullException("globalName");
			OID oid;
			if(_named.TryGetValue(globalName, out oid) == false)
				//throw new PulsarException("Не удалось извлечь глобальный объект по имени [{0}]!", globalName);
				return null;
			return Get(oid);
		}
		/// <summary>
		/// Извлекает глобальный объект из GOL и устанавливает блокировку на чтение.
		/// </summary>
		/// <param name="oid">OID объекта</param>
		/// <returns></returns>
		public static GlobalObject GetForRead(OID oid)
		{
			GlobalObject go = Get(oid);
			if(go != null)
				((IReadWriteLockObject)go).BeginRead();
			return go;
		}
		/// <summary>
		/// Извлекает глобальный объект из GOL и устанавливает блокировку на чтение.
		/// </summary>
		/// <param name="globalName">Глобальное имя объекта</param>
		/// <returns></returns>
		public static GlobalObject GetForRead(string globalName)
		{
			GlobalObject go = Get(globalName);
			if(go != null)
				((IReadWriteLockObject)go).BeginRead();
			return go;
		}
		/// <summary>
		/// Извлекает глобальный объект из GOL и устанавливает блокировку на запись.
		/// </summary>
		/// <param name="oid">OID объекта</param>
		/// <returns></returns>
		public static GlobalObject GetForWrite(OID oid)
		{
			GlobalObject go = Get(oid);
			if(go != null)
				((IReadWriteLockObject)go).BeginWrite();
			return go;
		}
		/// <summary>
		/// Извлекает глобальный объект из GOL и устанавливает блокировку на запись.
		/// </summary>
		/// <param name="globalName">Глобальное имя объекта</param>
		/// <returns></returns>
		public static GlobalObject GetForWrite(string globalName)
		{
			GlobalObject go = Get(globalName);
			if(go != null)
				((IReadWriteLockObject)go).BeginWrite();
			return go;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Ищет объект в GOL, если не найден пытается загрузить.
		/// </summary>
		/// <param name="type">Тип объекта.</param>
		/// <param name="oid">OID объекта.</param>
		/// <param name="writeLock">true - объект лочится на запись, иначе на чтение</param>
		/// <returns></returns>
		public static GlobalObject Locate(Type type, OID oid, bool throwIfNotFound = false, bool writeLock = false)
		{
			GlobalObject go = writeLock ? GetForWrite(oid) : GetForRead(oid);
			if(go != null)
				return go;
			go = GlobalObject.CreateUninitialized(type, oid);
			if(GOL.LoadGlobalObject(go,false))
				Add(go);
			else	
			{	 
				go = null;
				if(_locators.ContainsKey(type))
				 foreach(var a in _locators[type])
					{
				  go = a(oid);
						if(go != null)
						{
							Add(go);
						 break;
						}
					}
			}
			if(go == null)
			 if(throwIfNotFound)
			  throw new PulsarException("Не удалось найти глобальный объект [{0}]!", oid);
				else
				 return null;
			if(writeLock)
				go.BeginWrite();
			else
				go.BeginRead();
			return go;
		}
		//-------------------------------------------------------------------------------------
		/////// <summary>
		/////// Определяет, содержит ли GOL указанный объект в списке ожидающих инициализации.
		/////// </summary>
		/////// <param name="oid">OID глобального объекта.</param>
		/////// <returns></returns>
		////public static bool IsUninitialized(OID oid)
		////{
		//// if(oid == null)
		////  return false;
		//// try
		//// {
		////  _xxx.EnterReadLock();
		////  return _late.ContainsKey(oid);
		//// }
		//// finally	{ _xxx.ExitReadLock(); }
		////}
		/////// <summary>
		/////// Определяет, содержит ли GOL указанный объект в списке ожидающих инициализации.
		/////// </summary>
		/////// <param name="obj">Глобальный объект.</param>
		/////// <returns></returns>
		////public static bool IsUninitialized(GlobalObject obj)
		////{
		//// if(obj == null || obj.OID == null)
		////  return false;
		//// try
		//// {
		////  _xxx.EnterReadLock();
		////  return _late.ContainsKey(obj.OID);
		//// }
		//// finally	{ _xxx.ExitReadLock(); }
		////}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет объект из GOL.
		/// </summary>
		/// <param name="obj">Глобальный объект.</param>
		public static void Remove(GlobalObject obj)
		{
			if(obj == null || obj.OID == null)
				return;
			try
			{
				_xxx.EnterWriteLock();
				_dic.Remove(obj.OID);
				////_late.Remove(obj.OID);
				if(((IGlobalObjectMeta)obj).GlobalName != null)
					_named.Remove(((IGlobalObjectMeta)obj).GlobalName);
			}
			finally { _xxx.ExitWriteLock(); }
		}
		/// <summary>
		/// Очищает GOL
		/// </summary>
		public static void Clear()
		{
			try
			{
				_xxx.EnterWriteLock();
				_dic.Clear();
				_named.Clear();
				////_late.Clear();
			}
			finally { _xxx.ExitWriteLock(); }
		}
		//-------------------------------------------------------------------------------------
		internal static void RegistryGlobalName(GlobalObject obj)
		{
			if(((IGlobalObjectMeta)obj).GlobalName != null)
				if(_named.ContainsKey(((IGlobalObjectMeta)obj).GlobalName))
					_named[((IGlobalObjectMeta)obj).GlobalName] = obj.OID;
				else
					_named.Add(((IGlobalObjectMeta)obj).GlobalName, obj.OID);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="oldName"></param>
		/// <param name="newName"></param>
		internal static void ChangeGlobalName(GlobalObject obj, string oldName, string newName)
		{
			if(obj == null || obj.OID == null)
				return;
			try
			{
				_xxx.EnterWriteLock();
				if(oldName != null)
					_named.Remove(oldName);
				if(newName != null)
					_named.Add(newName, obj.OID);
			}
			finally { _xxx.ExitWriteLock(); }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает перечисление объектов Пульсара указанного типа из хранилища сущностей.
		/// Объекты блокируются на чтение
		/// </summary>
		/// <param name="type">Тип сущностей.</param>
		/// <returns></returns>
		public static IEnumerable<GlobalObject> GetByType(Type type)
		{
			try
			{
				_xxx.EnterReadLock();
				foreach(var i in _dic.Values)
				{
					Type t;
					if(i is WeakReference == false)
						t = i.GetType();
					else	if(((WeakReference)i).IsAlive)
						t = ((WeakReference)i).Target.GetType();
					else continue;
					if(t == type)
					{
						GlobalObject go = (GlobalObject)(i is WeakReference ? ((WeakReference)i).Target : i);
						go.BeginRead();
						yield return go; 
					}
				}
			}
			finally	{ _xxx.ExitReadLock(); }
		}
		/// <summary>
		/// Возвращает перечисление объектов Пульсара указанного типа из хранилища сущностей.
		/// Объекты блокируются на чтение
		/// </summary>
		/// <typeparam name="T">Тип сущностей.</typeparam>
		/// <returns></returns>
		public static IEnumerable<T> GetByType<T>()	where T: GlobalObject
		{
			try
			{
				_xxx.EnterReadLock();
				foreach(var i in _dic.Values)
				{
					Type t;
					if(i is WeakReference == false)
						t = i.GetType();
					else if(((WeakReference)i).IsAlive)
						t = ((WeakReference)i).Target.GetType();
					else continue;
					if(t == typeof(T))
					{
						GlobalObject go = (GlobalObject)(i is WeakReference ? ((WeakReference)i).Target : i);
						go.BeginRead();
						yield return (T)go;
					}
				}
			}
			finally
			{
				_xxx.ExitReadLock();
			}
		}
		/// <summary>
		/// Возвращает перечисление объектов Пульсара указанного типа и его наследующих из хранилища сущностей.
		/// </summary>
		/// <param name="type">Тип сущностей.</param>
		/// <returns></returns>
		public static IEnumerable<GlobalObject> GetByTypeDerived(Type type)
		{
			try
			{
				_xxx.EnterReadLock();
				foreach(var i in _dic.Values)
				{
					Type t;
					if(i is WeakReference == false)
						t = i.GetType();
					else if(((WeakReference)i).IsAlive)
						t = ((WeakReference)i).Target.GetType();
					else continue;
					if(t == type || t.IsSubclassOf(type))
					{
						GlobalObject go = (GlobalObject)(i is WeakReference ? ((WeakReference)i).Target : i);
						go.BeginRead();
						yield return go;
					}
				}
			}
			finally	{ _xxx.ExitReadLock(); }
		}
		/// <summary>
		/// Возвращает перечисление объектов Пульсара указанного типа и его наследующих из хранилища сущностей.
		/// </summary>
		/// <typeparam name="T">Тип сущностей.</typeparam>
		/// <returns></returns>
		public static IEnumerable<T> GetByTypeDerived<T>() where T : GlobalObject
		{
			try
			{
				_xxx.EnterReadLock();
				foreach(var i in _dic.Values)
				{
					Type t;
					if(i is WeakReference == false)
						t = i.GetType();
					else if(((WeakReference)i).IsAlive)
						t = ((WeakReference)i).Target.GetType();
					else continue;
					if(t == typeof(T) || t.IsSubclassOf(typeof(T)))
					{
						GlobalObject go = (GlobalObject)(i is WeakReference ? ((WeakReference)i).Target : i);
						go.BeginRead();
						yield return (T)go;
					}
				}
			}
			finally
			{
				_xxx.ExitReadLock();
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		//public static void Clear()
		//{
		// _refs.Clear();
		// essMan.Clear();
		//}
		public static IEnumerable<OID> Keys
		{
			get { return _dic.Keys; }
		}
		//*************************************************************************************
		private class LateDeserItem
		{
			public uint id;
			public PulsarSerializer.DeserContext cox;
			public LateDeserItem(uint id, PulsarSerializer.DeserContext cox)
			{
				this.id = id;
				this.cox = cox;
			}
		}
	}

	#endregion << public class GlobalObjectsList >>
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Expressions;

using Pulsar.Reflection.Dynamic;


namespace Pulsar
{
	#region << public class WeakEvent<TSender, TArgs> >>
	/// <summary>
	/// Класс "слабого" события
	/// </summary>
	/// <typeparam name="TSender"></typeparam>
	/// <typeparam name="TArgs"></typeparam>
	public class WeakEvent<TSender, TArgs>
	{
		[NonSerialized]
		private XXX _xxx = new XXX();
		[NonSerialized]
		private Subscriber[] _arr = null;
		[NonSerialized]
		private Subscriber[][] _map = null;
		[NonSerialized]
		private int _count = 0;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает количество подписчиков
		/// </summary>
		public int Count
		{
			get { return _count; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public WeakEvent() { }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Добавляет обработчик события.
		/// </summary>
		/// <param name="handler">Обработчик события.</param>
		public void Add(Delegate handler)
		{
			if(handler == null)
				throw new ArgumentNullException("Параметр handler не должен быть равен null!");
			lock(_xxx)
			{
				Subscriber ns = new Subscriber(handler);
				if(_xxx.RaiseCount > 0)
				{
				 if(_xxx.lateAdd == null)
					 _xxx.lateAdd = new List<Subscriber>();
					_xxx.lateAdd.Add(ns);
					return;
				}

				if(_arr == null && _map == null)
				{
					#region
					_arr = new Subscriber[1];
					_arr[0] = ns;
					_count = 1;
					#endregion
					return;
				}
				//--
				Subscriber[]	arr = null;
				object target = ns.Target;

				if(_arr != null)
					arr = _arr;
				else
				{
					uint hash = (uint)ns.Target.GetHashCode() % (uint)_map.Length;
					if(_map[hash] != null)
						arr = _map[hash];
				}
				//--
				int pos = -1;
				if(arr != null)
					for(int a = 0; a < arr.Length; a++)
					{
						if(arr[a] == null)
							pos = a;
						else if(arr[a].IsAlive == false)
						{
							arr[a].Dispose();
							arr[a] = null;
							_count--;
							pos = a;
						}
						else if(Object.Equals(arr[a].Target, target))
							return;
					}
				if(pos > -1)
				{
					arr[pos] = ns;
					_count++;
					return;
				}
				if(_arr != null)
					if(_arr.Length < 8)
					{
						_arr = new Subscriber[_arr.Length*2];
						Array.Copy(arr,_arr,arr.Length);
						_arr[arr.Length] = ns;
						_count++;
						return;
					}
					else
					{
						_map = new Subscriber[7][];
						_map[0] = _arr;
						_arr = null;
					}
				if(_count >= _map.Length)
					RebuildMap();
				if(AddMap(ns))
					_count++;
			}
		}
		/// <summary>
		/// Добавляет подписчика.
		/// </summary>
		/// <param name="suber">Добавляемый подписчик.</param>
		public void Add(Subscriber suber)
		{
			if(suber == null)
				throw new ArgumentNullException("suber");
			lock(_xxx)
			{
				if(_xxx.RaiseCount > 0)
				{
					_xxx.lateAdd.Add(suber);
					return;
				}


				if(_arr == null && _map == null)
				{
					#region
					_arr = new Subscriber[1];
					_arr[0] = suber;
					_count = 1;
					#endregion
					return;
				}
				//--
				Subscriber[]	arr = null;
				object target = suber.Target;

				if(_arr != null)
					arr = _arr;
				else
				{
					uint hash = (uint)target.GetHashCode() % (uint)_map.Length;
					if(_map[hash] != null)
						arr = _map[hash];
				}
				//--
				int pos = -1;
				if(arr != null)
					for(int a = 0; a < arr.Length; a++)
					{
						if(arr[a] == null)
							pos = a;
						else if(arr[a].IsAlive == false)
						{
							arr[a].Dispose();
							arr[a] = null;
							_count--;
							pos = a;
						}
						else if(Object.Equals(arr[a].Target, target))
							return;
					}
				if(pos > -1)
				{
					arr[pos] = suber;
					_count++;
					return;
				}
				if(_arr != null)
					if(_arr.Length < 8)
					{
						_arr = new Subscriber[_arr.Length*2];
						Array.Copy(arr, _arr, arr.Length);
						_arr[arr.Length] = suber;
						_count++;
						return;
					}
					else
					{
						_map = new Subscriber[7][];
						_map[0] = _arr;
						_arr = null;
					}
				if(_count >= _map.Length)
					RebuildMap();
				if(AddMap(suber))
					_count++;
			}
		}

		private void RebuildMap()
		{
			Subscriber[][] map	= _map;
			_map = new Subscriber[Primes.GetPrime(_map.Length)][];
			foreach(var line in map)
				if(line != null)
					foreach(Subscriber s in line)
						if(s != null)
							AddMap(s);
		}
		private bool AddMap(Subscriber s)
		{
			try
			{
				if(s.IsAlive == false)
					return false;
				uint hash = (uint)s.Target.GetHashCode() % (uint)_map.Length;
				if(_map[hash] == null)
				{
					_map[hash] = new Subscriber[2];
					_map[hash][0] = s;
					return true;
				}
				Subscriber[] line = _map[hash];
				for(int a = 0; a< line.Length; a++)
					if(line[a] == null)
					{
						line[a] = s;
						return true;
					}
				int size = line.Length < 10 ? line.Length : (int)(line.Length * 0.5);
				_map[hash] = new Subscriber[line.Length + size];
				Array.Copy(line, _map[hash], line.Length);
				_map[hash][line.Length] = s;
				return true;
			}
			catch
			{
				throw;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет обработчик события.
		/// </summary>
		/// <param name="handler">Обработчик события.</param>
		public void Remove(Delegate handler)
		{
			if(handler == null)
				return;
			lock(_xxx)
			{
				if(_arr == null && _map == null)
					return;
				Subscriber[]	arr = null; 
				object target = handler.Target ?? handler.Method.DeclaringType;
				int lives = 0;
				uint hash = 0;

				if(_arr != null)
					arr = _arr;
				else
				{
					hash = (uint)target.GetHashCode() % (uint)_map.Length;
					if((arr =_map[hash]) == null)
						return;
				}

				for(int a = 0; a < arr.Length; a++)
					if(arr[a] != null)
						if(arr[a].IsAlive == false || Object.Equals(arr[a].Target, target))
						{
							arr[a].Dispose();
							arr[a] = null;
							_count--;
						}
						else
							lives++;
				if(lives == 0)
					if(_arr == null)
					{
						_map[hash] = null;
						if(_count == 0)
							_map = null;
					}
					else
						_arr = null;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет все обработчики событий.
		/// </summary>
		public void Clear()
		{
			lock(_xxx)
			{
				if(_arr != null)
					foreach(Subscriber s in _arr)
						if(s != null)
							s.Dispose();
				if(_map != null)
					foreach(var line in _map)
						if(line != null)
							foreach(Subscriber s in line)
								if(s != null)
									s.Dispose();
				_arr = null;
				_map = null;
				_count = 0;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает сообщение.
		/// </summary>
		/// <param name="args">Аргументы вызова обработчиков.</param>
		public void Raise(params object[] args)
		{
			lock(_xxx)
			{
				_xxx.RaiseCount++;
				try
			 {
					if(_arr != null)
					{
						for(int a = 0; _arr != null && a < _arr.Length; a++)
							if(_arr[a] != null)
								if(_arr[a].IsAlive == false)
								{
									_arr[a].Dispose();
									_arr[a] = null;
									_count--;
								}
								else
									_arr[a].Invoke(args);
						if(_count <= 0)
						 _arr = null;
					}
					if(_map != null)
					{
						for(int i = 0; _map != null && i < _map.Length; i++)
							if(_map[i] != null)
							{
							 int lives = 0;
								for(int a = 0;  _map != null && _map[i] != null && a < _map[i].Length; a++)
									if(_map[i][a] != null)
										if(_map[i][a].IsAlive == false)
										{
											_map[i][a].Dispose();
											_map[i][a] = null;
											_count--;
										}
										else
										{
											_map[i][a].Invoke(args);
											lives++;
										}
								if(lives == 0)
									_map[i] = null;
							}
						if(_count == 0)
						 _map = null;
					}
				}
				catch(Exception err)
				{
					if(err is TargetInvocationException && err.InnerException != null)
						throw err.InnerException;
					else
						throw;
				}
				_xxx.RaiseCount--;
			 if(_xxx.RaiseCount == 0 && _xxx.lateAdd != null)
				{
				 foreach(Subscriber s in _xxx.lateAdd)
					 Add(s);
					_xxx.lateAdd.Clear();
					_xxx.lateAdd = null;
				}
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Subscriber> Subscribers()
		{
			lock(_xxx)
			{
				if(_arr != null)
				{
					for(int a = 0; a < _arr.Length; a++)
						if(_arr[a] != null &&_arr[a].IsAlive)
							yield return _arr[a];
				}
				if(_map != null)
				{
					for(int i = 0; i < _map.Length; i++)
						if(_map[i] != null)
							for(int a = 0; a < _map[i].Length; a++)
								if(_map[i][a] != null && _map[i][a].IsAlive)
									 yield return _map[i][a];
				}
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region Operators += and -=
		/// <summary>
		/// +=
		/// </summary>
		/// <param name="eventt"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static WeakEvent<TSender, TArgs> operator +(WeakEvent<TSender, TArgs> eventt, Delegate handler)
		{
			if(eventt == null)
				eventt = new WeakEvent<TSender, TArgs>();

			eventt.Add(handler);
			return eventt;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// -=
		/// </summary>
		/// <param name="eventt"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static WeakEvent<TSender, TArgs> operator -(WeakEvent<TSender, TArgs> eventt, Delegate handler)
		{
			if(eventt == null)
				return null;

			eventt.Remove(handler);
			return eventt;
		}
		#endregion
		//*************************************************************************************
		private class XXX
		{
			public int RaiseCount = 0;
			public List<Subscriber> lateAdd = null;
		}
	}
	#endregion << public class WeakEvent<TSender, TArgs> >>
	//**************************************************************************************
	#region << public class WeakEvent<TArgs> : WeakEvent<object, TArgs> >>
	/// <summary>
	/// Класс "слабого" события
	/// </summary>
	/// <typeparam name="TArgs">Тип аргумента события</typeparam>
	public class WeakEvent<TArgs> : WeakEvent<object, TArgs>
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public WeakEvent()	: base() { }
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region Operators += and -=
		/// <summary>
		/// +=
		/// </summary>
		/// <param name="eventt"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static WeakEvent<TArgs> operator +(WeakEvent<TArgs> eventt, Delegate handler)
		{
			if(eventt == null)
				eventt = new WeakEvent<TArgs>();

			eventt.Add(handler);
			return eventt;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// -=
		/// </summary>
		/// <param name="eventt"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static WeakEvent<TArgs> operator -(WeakEvent<TArgs> eventt, Delegate handler)
		{
			if(eventt == null)
				return null;

			eventt.Remove(handler);
			return eventt;
		}
		#endregion
	}
	#endregion << public class WeakEvent<TArgs> : WeakEvent<object, TArgs> >>
	//**************************************************************************************
	#region << public class Subscriber >>
	/// <summary>
	/// Класс "слабого" подписчика 
	/// </summary>
	public class Subscriber // : IDisposable
	{
		private Type _objType = null;
		private Delegate _del = null;
		private WeakReference _refObj;

		private static Action<object,object> _setter = ReflectionHelper.CreateFieldSetMethod(typeof(Delegate), "_target");
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет доступность подписчика
		/// </summary>
		public bool IsAlive
		{
			get { return _refObj.IsAlive; }
		}
		/// <summary>
		/// Объект подписчика на событие.
		/// </summary>
		public object Target
		{
			get { return _refObj.Target; }
		}
		/// <summary>
		/// Тип объекта подписчика на событие.
		/// </summary>
		public Type TargetType
		{
			get { return _objType; }
		}
		/// <summary>
		/// Имя метода обработчика события
		/// </summary>
		public string MethodName
		{
			get { return _del.Method.Name; }
		}
		///// <summary>
		///// Делегат метода обработчика события
		///// </summary>
		//public Delegate Method
		//{
		// get
		// {
		//  ////if(_del == null && _refObject.IsAlive)
		//  ////{
		//  //// _del = ReflectionHelper.GetDelegate(_mi, _refObject.Target);
		//  //// _mi = null;
		//  ////}
		//  return _del;
		// }
		//}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public Subscriber(Delegate handler)
		{
			if(handler == null)
				throw new ArgumentNullException("Параметр handler не должен быть равен null!");
			object t = handler.Target;
			if(t == null)
			{
				MethodInfo mi = handler.Method;
				if(mi.IsStatic == false)
					throw new Exception("Открытые делегаты не поддерживаются!");
				_refObj = new WeakReference(mi.DeclaringType);
				_objType = mi.DeclaringType;
			}
			else
			{
				_refObj = new WeakReference(t);
				_objType = t.GetType();
			}
			_setter(handler, null);
			_del = handler;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		///// <summary>
		///// Вызывает обработчик события.
		///// </summary>
		///// <param name="sender">Объет, вызывающий событие.</param>
		///// <param name="args">Аргументы события.</param>
		//public void Invoke<TSender, TArgs>(TSender sender, TArgs args)
		//{
		// if(_del == null)
		// {
		//  //////_del = ReflectionHelper.CreateCallLambdaAction2<TSender,TArgs>(_mi);
		//  ////_del = ReflectionHelper.GetDelegate(_mi, _refObject.Target);
		//  ////_mi = null;
		// }
		// if(IsAlive)
		//  //((Action<object,TSender,TArgs>)_del)(_refObject.Target, sender, args);
		//  ((Action<TSender, TArgs>)_del)(sender, args);
		//}
		/// <summary>
		/// Вызывает обработчик события.
		/// </summary>
		/// <param name="sender">Объет, вызывающий событие.</param>
		/// <param name="args">Аргументы события.</param>
		public void Invoke(params object[] args)
		{
			try
			{
				if(_refObj.IsAlive == false)
					return;
				try
				{
					if(_refObj.Target is Type)
						_setter(_del, _del);
					else
						_setter(_del, _refObj.Target);

					_del.DynamicInvoke(args);
				}
				finally
				{
					if(_del != null)
						_setter(_del, null);
				}
			}
			catch(Exception err)
			{
				if(err is TargetInvocationException && err.InnerException != null)
				 throw err.InnerException;

				throw;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Subscriber:" + (_refObj.Target ?? "").ToString();
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IDisposable Members
		/// <summary>
		/// Dispose()
		/// </summary>
		public void Dispose()
		{
			_objType = null;
			_del = null;
			_refObj = null;
		}
		#endregion
	}
	#endregion << public class Subscriber >>

	
}

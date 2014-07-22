using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public interface IKeyedValue : IObjectChangeNotify >>
	/// <summary>
	/// Интерфейс классов пары ключ-значение
	/// </summary>
	public interface IKeyedValue : IObjectChangeNotify
	{
		/// <summary>
		/// Ключ
		/// </summary>
		object Key { get; }
		/// <summary>
		/// Объект
		/// </summary>
		object Value { get; }
	}
	#endregion << public interface IKeyedValue : IObjectChangeNotify >>
	//*************************************************************************************
	#region << public interface IKeyedValue : IObjectChangeNotify >>
	/// <summary>
	/// Generic интерфейс классов пары ключ-значение
	/// </summary>
	public interface IKeyedValue<out TKey, out TValue> : IObjectChangeNotify
	{
		/// <summary>
		/// Ключ
		/// </summary>
		TKey Key { get; }
		/// <summary>
		/// Объект
		/// </summary>
		TValue Value { get; }
	}
	#endregion << public interface IKeyedValue : IObjectChangeNotify >>
	//*************************************************************************************

	/// <summary>
	/// Класс пары ключ-значение
	/// </summary>
	/// <typeparam name="TKey">Тип ключа.</typeparam>
	/// <typeparam name="TValue">Тип значения</typeparam>
	public class KeyedValue<TKey, TValue> : ObjectChangeNotify, IKeyedValue, IKeyedValue<TKey, TValue>, IDisposable
	{
		[NonSerialized]
		private static bool? _isChangeNotify;

		private readonly TKey _k;
		private TValue _v;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Ключ
		/// </summary>
		public TKey Key
		{
			get { return _k; }
		}
		object IKeyedValue.Key
		{
			get { return _k; }
		}
		/// <summary>
		/// Значение
		/// </summary>
		public TValue Value
		{
			get { return _v; }
			protected internal set
			{
				//var arg = new ObjectChangeNotifyEventArgs()
				//{
				// Action = ChangeNotifyAction.ObjectChange, MemberName = "Value",
				// NewValue = value, Sender = this, OldValue = 
				//};
				//OnObjectChanging(arg);
				Dispose();
				_v = value;
				if (IsChangeNotify && _v != null)
				{
					((IObjectChangeNotify)_v).ObjectChanging += (KeyedValue_ObjectChanging);
					((IObjectChangeNotify)_v).ObjectChanged += (KeyedValue_ObjectChanged);
				}

			}
		}
		object IKeyedValue.Value
		{
			get { return _v; }
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		public static bool IsChangeNotify
		{
			get
			{
				if (_isChangeNotify == null)
					_isChangeNotify = typeof(TValue).GetInterface("IObjectChangeNotify", false) != null;
				return _isChangeNotify.Value;
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public KeyedValue(TKey key, TValue val)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			_k = key;
			_v = val;
			if (IsChangeNotify && _v != null)
			{
				((IObjectChangeNotify)_v).ObjectChanging += (KeyedValue_ObjectChanging);
				((IObjectChangeNotify)_v).ObjectChanged += (KeyedValue_ObjectChanged);
			}
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("[{0};{1}]", _k, _v);
		}
		/// <summary>
		/// Отписывается от событий	объекта
		/// </summary>
		public void Dispose()
		{
			if (IsChangeNotify && _v != null)
			{
				((IObjectChangeNotify)_v).ObjectChanging -= (KeyedValue_ObjectChanging);
				((IObjectChangeNotify)_v).ObjectChanged -= (KeyedValue_ObjectChanged);
			}

		}
		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		{
			if (IsChangeNotify && _v != null)
			{
				((IObjectChangeNotify)_v).ObjectChanging += (KeyedValue_ObjectChanging);
				((IObjectChangeNotify)_v).ObjectChanged += (KeyedValue_ObjectChanged);
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IObjectChangeNotify Members
		void KeyedValue_ObjectChanging(object sender, ObjectChangeNotifyEventArgs e)
		{
			switch (e.Action)
			{
				case ChangeNotifyAction.ObjectReset:
					OnObjectResetting(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset, e));
					break;
				default:
					OnObjectChanging(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange, e, "Value", _v, null));
					break;
			}
		}
		void KeyedValue_ObjectChanged(object sender, ObjectChangeNotifyEventArgs e)
		{
			switch (e.Action)
			{
				case ChangeNotifyAction.ObjectReset:
					OnObjectResetted(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectReset, e));
					break;
				default:
					OnObjectChanged(new ObjectChangeNotifyEventArgs(this, ChangeNotifyAction.ObjectChange, e, "Value", _v, null));
					break;
			}
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс задачи планировщика с открытым рабочим методом.
	/// </summary>
	public class SchedulerTaskAction : SchedulerTask
	{
		[NonSerialized]
		private Action<object> _action = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Рабочий метод
		/// </summary>
		public Action<object> Action
		{
			get { lock(SyncRoot) return _action; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("Action", value, _action);
					_action = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Возвращает true, если задачу необходимо запустить.
		/// </summary>
		public override bool TimeToRun
		{
			get
			{
				lock(SyncRoot)
				{
					if(_action == null)
						return false;
					return base.TimeToRun;
				}
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SchedulerTaskAction() : base() { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public SchedulerTaskAction(string name, string group, Action<object> action)
			: base(name, group)
		{
			_action = action;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public SchedulerTaskAction(string name, string group, object data, Action<object> action)
			: base(name, group, data)
		{
			_action = action;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Рабочий метод.
		/// </summary>
		public override void Work()
		{
			if(_action != null)
				_action(Data);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public enum SchedulerTaskPriority : byte >>
	/// <summary>
	/// Перечисление приоритетов задач планировщика.
	/// </summary>
	public enum SchedulerTaskPriority : byte
	{
		/// <summary>
		/// Ниже обычного
		/// </summary>
		Low = 0,
		/// <summary>
		/// Обычный приоритет
		/// </summary>
		Normal = 1,
		/// <summary>
		/// Выше обычного
		/// </summary>
		High	= 2,
		/// <summary>
		/// Немедленный запуск
		/// </summary>
		Critical = 16
	}
	#endregion << public enum SchedulerTaskPriority : byte >>
	//*************************************************************************************
	#region << public enum SchedulerTaskState : byte >>
	/// <summary>
	/// Перечисление состояний задачи планировщика.
	/// </summary>
	public enum SchedulerTaskState : byte
	{
		/// <summary>
		/// Задача не была запланирована (добавлена в планировщик)
		/// </summary>
		NotScheduled	= 0,
		/// <summary>
		/// Ожидание первого запуска или запуска по интервалу после успешного завершения.
		/// </summary>
		Waiting = 1,
		/// <summary>
		/// Ожидание запуска по интервалу после завершения с ошибкой.
		/// </summary>
		WasErrorWating = 2,
		/// <summary>
		/// Ожидание повторного запуска из-за ошибки
		/// </summary>
		WaitingErrorReRun = 4,
		/// <summary>
		/// Задача выполнятся
		/// </summary>
		Running = 8,
		/// <summary>
		/// Задача завершена успешно.
		/// </summary>
		SuccessDone = 16,
		/// <summary>
		/// Задача завершена с ошибкой.
		/// </summary>
		ErrorDone = 32
	}
	#endregion << public enum SchedulerTaskState : byte >>
	//*************************************************************************************
	/// <summary>
	/// Класс задачи планировщика. Потокобезопасный (надеюсь:). Не реализует IReadWriteLockObject
	/// </summary>
	public abstract class SchedulerTask : GlobalObject
	{
		private string _name;
		private string _group = "(нет категории)";
		private SchedulerTaskPriority _prior = SchedulerTaskPriority.Normal;
		private object _data = null;
		private DateTime _nextRun = DateTime.Now;
		private DateTime _lastDone = DateTime.MinValue;
		private uint _interval = 0;
		private uint _reRunErrorInterval = 120;
		private bool _delDone = true;
		private SchedulerTaskState _state= SchedulerTaskState.NotScheduled;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Имя задачи
		/// </summary>
		public virtual string Name
		{
			get { lock(SyncRoot) return _name; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("Name", value, _name);
					_name = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Группа задачи
		/// </summary>
		public virtual string Group
		{
			get { lock(SyncRoot) return _group; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("Group", value, _group);
					_group = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Приоритет задачи
		/// </summary>
		public virtual SchedulerTaskPriority Priority
		{
			get { lock(SyncRoot) return _prior; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("Priority", value, _prior);
					_prior = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Дата следующего запуска	(DateTime.Now по умолчанию)
		/// </summary>
		public virtual DateTime NextRun
		{
			get { lock(SyncRoot) return _nextRun; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("NextRun", value, _nextRun);
					_nextRun = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Дата последнего успешного выполнения
		/// </summary>
		public DateTime LastDone
		{
			get { lock(SyncRoot) return _lastDone; }
			internal set { lock(SyncRoot) _lastDone = value; }
		}
		/// <summary>
		/// Интервал повторного запуска, секунд (0 по умолчанию).
		/// </summary>
		public virtual uint RunInterval
		{
			get { lock(SyncRoot) return _interval; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("RunInterval", value, _interval);
					_interval = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Определяет, будет ли перезапущена задача в случае ошибки (ReRunErrorInterval > 0).
		/// </summary>
		public virtual bool ReRunIfError
		{
			get { lock(SyncRoot) return _reRunErrorInterval > 0; }
		}
		/// <summary>
		/// Определяет интервал, через который будет запущена задача в случае ошибки выполнения, секунд (120 по умолчанию).
		/// </summary>
		public uint ReRunErrorInterval
		{
			get { lock(SyncRoot) return _reRunErrorInterval; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("ReRunErrorInterval", value, _reRunErrorInterval);
					_reRunErrorInterval = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Определяет, будет ли удалена задача после выполнения (true по умолчанию).
		/// </summary>
		public virtual bool DeleteAfterDone
		{
			get { lock(SyncRoot) return _delDone; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("DeleteAfterDone", value, _delDone);
					_delDone = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Статус задачи
		/// </summary>
		public SchedulerTaskState State
		{
			get { lock(SyncRoot) return _state; }
			internal set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("State", value, _state);
					_state = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Данные задачи
		/// </summary>
		public virtual object Data
		{
			get { lock(SyncRoot) return _data; }
			set
			{
				lock(SyncRoot)
				{
					var arg = OnObjectChanging("Data", value, _data);
					_data = value;
					OnObjectChanged(arg);
				}
			}
		}
		/// <summary>
		/// Возвращает true, если задачу необходимо запустить.
		/// </summary>
		public virtual bool TimeToRun
		{
			get
			{
				lock(SyncRoot)
				{
					if(_state != SchedulerTaskState.Waiting && _state != SchedulerTaskState.WasErrorWating &&
								_state != SchedulerTaskState.WaitingErrorReRun)
						return false;
					if(_prior == SchedulerTaskPriority.Critical)
						return true;
					if(_state == SchedulerTaskState.WaitingErrorReRun && ReRunIfError)
						return _lastDone.AddSeconds(_reRunErrorInterval) <= DateTime.Now;
					return NextRun < DateTime.Now;
				}
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SchedulerTask() { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public SchedulerTask(string name, string group)
			: this()
		{
			_name = name;
			_group = group;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public SchedulerTask(string name, string group, object data)
			: this(name, group)
		{
			_data = data;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Рабочий метод.
		/// </summary>
		public abstract void Work();
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _name ?? "(нет имени)";
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------

	}
}

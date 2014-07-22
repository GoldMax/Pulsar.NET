using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

using Pulsar.Server;
//using OID = System.Guid;


namespace Pulsar
{
	/// <summary>
	/// Класс планировщика задач. Потокобезопасный (надеюсь:).
	/// </summary>
	public class Scheduler : ISerializable
	{
		[NonSerialized]
		private Timer _timer = null;
		[PulsarNonSerialized(PulsarSerializationMode.All ^ PulsarSerializationMode.OnSave)]
		private DateTime _lastCheck = DateTime.MinValue;
		[PulsarNonSerialized(PulsarSerializationMode.All ^ PulsarSerializationMode.OnSave)]
		private bool _isStarted = false;
		[PulsarNonSerialized(PulsarSerializationMode.All ^ PulsarSerializationMode.OnSave)]
		private byte _runned = 0;
		[NonSerialized]
		private bool _iAmWorking = false;
		[NonSerialized]
		private Action _needSave;

		private IndexedList<SchedulerTask, OID> _tasks = new IndexedList<SchedulerTask, OID>("OID");
		private byte _maxRunned = (byte)(Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount/2);
		private byte _checkRunTimeInterval = 5;
		private bool _useExternalPulse = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет максимальное число одновременно запущенных задач.(по умолчанию Environment.ProcessorCount) 
		/// </summary>
		public byte MaxRunnedTasks
		{
			get { lock(_tasks.SyncRoot) return _maxRunned; }
			set { lock(_tasks.SyncRoot) _maxRunned = value; }
		}
		/// <summary>
		/// Интервал проверки необходимости запуска задач, сек. (по умолчанию 5)
		/// </summary>
		public byte CheckRunTimeInterval
		{
			get { lock(_tasks.SyncRoot) return _checkRunTimeInterval; }
			set
			{
				if(value < 1)
					throw new Exception("Интервал проверки необходимости запуска задач должен быть больше нуля!");
				lock(_tasks.SyncRoot)
					_checkRunTimeInterval = value;
			}
		}
		/// <summary>
		/// Определяет, запущен ли планировщик
		/// </summary>
		public bool IsStarted
		{
			get { lock(_tasks.SyncRoot) return _isStarted; }
		}
		/// <summary>
		/// Определяет, используется ли внешний источник периодических сигналов или внутренний таймер
		/// </summary>
		public bool UseExternalPulse
		{
			get { lock(_tasks.SyncRoot) return _useExternalPulse; }
			set
			{
				lock(_tasks.SyncRoot)
				{
					_useExternalPulse = value;
					if(value)
					{
						if(_timer != null)
						{
							_timer.Change(Timeout.Infinite, Timeout.Infinite);
							_timer = null;
						}
					}
					else
					{
						if(_timer != null)
							_timer.Change(Timeout.Infinite, Timeout.Infinite);
						_timer = new Timer(Pulse, null, Timeout.Infinite, Timeout.Infinite);
					}
				}
			}
		}
		/// <summary>
		/// Возвращает количество запущенных задач.
		/// </summary>
		public byte RunnedTaskCount
		{
			get { lock(_tasks.SyncRoot) return _runned; }
		}
		/// <summary>
		/// Возвращает перечисление задач планировщика.
		/// </summary>
		public IEnumerable<SchedulerTask> Tasks
		{
			get { lock(_tasks.SyncRoot) return _tasks; }
		}
		/// <summary>
		/// Возвращает задачу по ее OID.
		/// </summary>
		/// <param name="taskOID">OID задачи.</param>
		/// <returns></returns>
		public SchedulerTask this[OID taskOID]
		{
			get { lock(_tasks.SyncRoot) return _tasks.ByIndex(taskOID); }
		}
		/// <summary>
		/// Определяет делегат, вызываемый при необходимости сохранения планировщика.
		/// </summary>
		public Action NeedSave
		{
			get { return _needSave; }
			set { _needSave = value; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public Scheduler()
		{
			_timer = new Timer(Pulse, null, Timeout.Infinite, Timeout.Infinite);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Hidden Methods >>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_maxRunned", _maxRunned);
			info.AddValue("_checkRunTimeInterval", _checkRunTimeInterval);
			info.AddValue("_useExternalPulse", _useExternalPulse);
			info.AddValue("_tasks", _tasks.ToArray());
		}
		///
		private void SetObjectData(SerializationInfo info, StreamingContext context)
		{
			_maxRunned = info.GetByte("_maxRunned");
			_checkRunTimeInterval = info.GetByte("_checkRunTimeInterval");
			_useExternalPulse = info.GetBoolean("_useExternalPulse");
			SchedulerTask[] arr = (SchedulerTask[])info.GetValue("_tasks", typeof(SchedulerTask[]));
			if(_tasks == null)
				_tasks = new IndexedList<SchedulerTask, OID>("OID");
			_tasks.Add(arr);
			if(_useExternalPulse == false)
				_timer = new Timer(Pulse, null, Timeout.Infinite, Timeout.Infinite);
		}
		/// <summary>
		/// Запускает задачу на выполнение. НЕ ПОТОКОБЕЗОПАСНЫЙ!
		/// </summary>
		/// <param name="task"></param>
		protected void RunTaskAction(SchedulerTask task)
		{
			lock(task.SyncRoot)
				task.State = SchedulerTaskState.Running;
			Pulsar.PulsarThreadPool.Run(TaskWork, task);
			_runned++;
		}
		private void TaskWork(object x)
		{
			SchedulerTask t = (SchedulerTask)x;
			try
			{
				t.Work();
				lock(t.SyncRoot)
				{
					if(t.RunInterval == 0)
						t.State = SchedulerTaskState.SuccessDone;
					else
					{
						t.State = SchedulerTaskState.Waiting;
						//t.NextRun = t.NextRun.AddSeconds(t.RunInterval);
						while(t.NextRun < DateTime.Now)
							t.NextRun = t.NextRun.AddSeconds(t.RunInterval);
					}
				}
			}
			catch(Exception exc)
			{
				lock(t.SyncRoot)
					if(t.ReRunIfError)
						t.State = SchedulerTaskState.WaitingErrorReRun;
					else if(t.RunInterval == 0)
						t.State = SchedulerTaskState.ErrorDone;
					else
						t.State = SchedulerTaskState.WasErrorWating;

				Pulsar.Logger.LogError(exc);
			}
			finally
			{
				lock(t.SyncRoot)
					t.LastDone = DateTime.Now;
				lock(_tasks.SyncRoot)
				{
					if(t.DeleteAfterDone && t.State == SchedulerTaskState.SuccessDone)
						_tasks.Remove(t);
					_runned--;
				}
				GOL.SaveGlobalObject(t);
				Logger.Log("Выход из потока задачи. Должно начаться сохранение!!!");
			}

		}
		#endregion << Hidden Methods >>
		//-------------------------------------------------------------------------------------
		#region << Public Methods >>
		/// <summary>
		/// Запускает планировщик
		/// </summary>
		public void Start()
		{
			lock(_tasks.SyncRoot)
			{
				if(_isStarted)
					return;
				_isStarted = true;
				if(_useExternalPulse == false)
					_timer.Change(0, _checkRunTimeInterval * 500);
			}
		}
		/// <summary>
		/// Останавливает планировщик
		/// </summary>
		public void Stop()
		{
			lock(_tasks.SyncRoot)
			{
				if(_isStarted == false)
					return;
				_isStarted = false;
				if(_useExternalPulse == false)
					_timer.Change(Timeout.Infinite, Timeout.Infinite);
			}
		}
		/// <summary>
		/// Метод, вызываемый источником периодических сигналов. 
		/// </summary>
		public void Pulse(object state)
		{
			//System.Diagnostics.Debug.WriteLine("Tic in {1} at  {0}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
			lock(_tasks.SyncRoot)
			{
				if(_isStarted == false || _iAmWorking || _tasks.Count == 0)
					return;
				if(_lastCheck.AddSeconds(_checkRunTimeInterval) > DateTime.Now)
					return;
				if(_runned >= _maxRunned)
					return;
				_iAmWorking = true;
			}
			//Console.WriteLine(">{0}", DateTime.Now);
			try
			{
				List<SchedulerTask> toRun;
				lock(_tasks.SyncRoot)
					toRun = new List<SchedulerTask>(_tasks);
				for(int a = 0; a < toRun.Count; a++)
					lock(toRun[a].SyncRoot)
						if(toRun[a].TimeToRun == false)
						{
							toRun.RemoveAt(a);
							a--;
						}
				if(toRun.Count == 0)
					return;
				toRun.Sort((x, y) =>
				{
					int r = -1 * ((byte)x.Priority).CompareTo((byte)y.Priority);
					if(r != 0)
						return r;
					return -1 * x.NextRun.CompareTo(y.NextRun);
				});
				lock(_tasks.SyncRoot)
					foreach(SchedulerTask t in toRun)
						if(_runned < _maxRunned)
							RunTaskAction(t);
						else
							break;
			}
			finally
			{
				lock(_tasks.SyncRoot)
				{
					_lastCheck = DateTime.Now;
					_iAmWorking = false;
				}
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет задачу в планировщик
		/// </summary>
		/// <param name="task">Добавляемая задача.</param>
		public void AddTask(SchedulerTask task)
		{
			lock(_tasks.SyncRoot)
			{
				_tasks.Add(task);
				lock(task)
				{
					task.State = SchedulerTaskState.Waiting;
					if(task.Priority == SchedulerTaskPriority.Critical)
						RunTaskAction(task);
				}
			}
		}
		/// <summary>
		/// Удаляет задачу
		/// </summary>
		/// <param name="taskOID">OID задачи.</param>
		public void RemoveTask(OID taskOID)
		{
			lock(_tasks.SyncRoot)
				_tasks.RemoveByIndex(taskOID);
		}
		/// <summary>
		/// Удаляет задачу
		/// </summary>
		/// <param name="task">Удаляемая задача.</param>
		public void RemoveTask(SchedulerTask task)
		{
			lock(_tasks.SyncRoot)
				_tasks.Remove(task);
		}
		/// <summary>
		/// Удаляет все задачи
		/// </summary>
		public void Clear()
		{
			lock(_tasks.SyncRoot)
				_tasks.Clear();
		}
		/// <summary>
		/// Принудительно мнгновенно запускает задачу, находящуюся в планировщике
		/// </summary>
		/// <param name="task">Запускаемая задача.</param>
		public void RunTask(SchedulerTask task)
		{
			lock(_tasks.SyncRoot)
			{
				if(_tasks.Contains(task) == false)
					throw new PulsarException("Планировщик не содержит задачу [{0}]!", task);
				lock(task)
				{
					task.State = SchedulerTaskState.Waiting;
					RunTaskAction(task);
				}
			}
		}
		#endregion << Public Methods >>
	}
}

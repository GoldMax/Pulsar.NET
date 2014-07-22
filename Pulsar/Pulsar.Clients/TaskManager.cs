using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pulsar
{
	//*************************************************************************************
	#region << public interface IAsyncTaskDoneHandler >>
	/// <summary>
	/// Интерфейс объектов, поддерживающих прием уведомлений о выполнении асинхронных задач.
	/// </summary>
	public interface IAsyncTaskDoneHandler
	{
		/// <summary>
		/// Метод, принимающий уведомления о выполнении задач.
		/// </summary>
		void AsyncTaskDone(AsyncTask task);
		/// <summary>
		/// Определяет необходимость вызова Invoke
		/// </summary>
		bool InvokeRequired { get; }
		/// <summary>
		/// Вызывает принимающий метод в UI потоке.
		/// </summary>
		/// <param name="task"></param>
		void Invoke(AsyncTask task);
	}
	#endregion << public interface IAsyncTaskDoneHandler >>
	//*************************************************************************************
	/// <summary>
	/// Класс менеджера асинхронных задач.
	/// </summary>
	public static class TaskManager
	{
		private static HashSet<AsyncTask> tasks = new HashSet<AsyncTask>();
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Создает асинхронную задачу и выполняет ее.
		/// </summary>
		/// <param name="taskName">Имя задачи.</param>
		/// <param name="dst">Объект, обрабатывающий уведомления о завершении задачи.</param>
		/// <param name="workMethod">Рабочий метод задачи.</param>
		/// <param name="tag">Дополнительная информацияю</param>
		/// <returns>Объект асинхронной задачи.</returns>
		public static AsyncTask Run(string taskName, IAsyncTaskDoneHandler dst, Func<object> workMethod, object tag = null)
		{
			AsyncTask task = new AsyncTask(taskName, dst) { WorkMethod = workMethod, Tag = tag };
			Run(task);
			return task;
		}
		/// <summary>
		/// Выполняет асинхронную задачу.
		/// </summary>
		/// <param name="task">Объект выполняемой задачи.</param>
		public static void Run(AsyncTask task)
		{
			if (task == null)
				throw new ArgumentNullException("task");
			lock (tasks)
				tasks.Add(task);

			lock (task)
			{
				task.thread = new Thread(() =>
				{
					try
					{
						if (task.WorkMethod != null)
							task.Result = task.WorkMethod();
					}
					catch (ThreadAbortException)
					{
						task.IsAborted = true;
					}
					catch (Exception exc)
					{
						task.Error = exc;
					}
					finally
					{
						lock (tasks)
							tasks.Remove(task);
						if (task.TaskDoneHandler != null)
							try
							{
								if (task.TaskDoneHandler.InvokeRequired)
									task.TaskDoneHandler.Invoke(task);
								else
									task.TaskDoneHandler.AsyncTaskDone(task);
							}
							catch { }
						if (task.are != null)
							task.are.Set();
					}
				});
				task.thread.IsBackground = true;
				task.thread.Name = "AsyncTask-" + task.TaskName;
				task.thread.Start();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Выполняет указанные задачи последовательно. В случае ошибки или прерывания задачи,
		/// последующие задачи не выполняются.
		/// </summary>
		/// <param name="queue">Выполняемые задачи.</param>
		public static void RunQueue(params AsyncTask[] queue)
		{
			ThreadPool.QueueUserWorkItem((state) =>
			{
				int count = 0;
				for (int a = 0; a < queue.Length; a++)
					if (queue[a] != null)
						count++;
				if (count == 0)
					return;
				for (int a = 0; a < queue.Length; a++)
				{
					AsyncTask at = queue[a];
					if (at == null)
						continue;
					if (count > 1)
						at.IsQueued = true;
					count--;
					Run(at);
					at.thread.Join();
					if (at.IsAborted || at.Error != null)
						break;
				}
			});
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, выполняется ли хотя бы одна задача для указанного объекта.
		/// </summary>
		/// <param name="dest">Объект, обрабатывающий уведомления о завершении задачи.</param>
		/// <returns></returns>
		public static bool ContainsTask(IAsyncTaskDoneHandler dest)
		{
			lock (tasks)
				foreach (AsyncTask t in tasks)
					if (t.TaskDoneHandler.Equals(dest))
						return true;
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Прерывает все задачи для указанного объекта уведомления.
		/// </summary>
		/// <param name="dest">Объект, обрабатывающий уведомления о завершении задачи.</param>
		public static void AbortTasks(IAsyncTaskDoneHandler dest)
		{
			List<AsyncTask> toDel = new List<AsyncTask>();
			lock (tasks)
			{
				foreach (AsyncTask at in tasks)
					if (at.TaskDoneHandler != null && at.TaskDoneHandler.Equals(dest))
						toDel.Add(at);
				foreach (AsyncTask at in toDel)
					at.thread.Abort();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Запускает метод после завершения всех указанных задач.
		/// </summary>
		/// <param name="postRun">Метод, запускаемый после завершения задач.</param>
		/// <param name="tasks">Запускаемые задачи.</param>
		public static void RunAfterAll(Action postRun, params AsyncTask[] tasks)
		{
			SynchronizationContext cox = SynchronizationContext.Current;
			ThreadPool.QueueUserWorkItem((state) =>
				{
					List<AutoResetEvent> ares = new List<AutoResetEvent>(tasks.Length);
					for (int a = 0; a < tasks.Length; a++)
					{
						if (tasks[a] == null)
							continue;
						tasks[a].are = new AutoResetEvent(false);
						ares.Add(tasks[a].are);
						Run(tasks[a]);
					}
					if (ares.Count > 0)
						WaitHandle.WaitAll(ares.ToArray());
					foreach (AsyncTask t in tasks)
						if (t != null)
						{
							t.are.Close();
							t.are.Dispose();
						}
					if (postRun != null)
						cox.Send((obj) => { postRun(); }, null);
				}
			);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Запускает метод после завершения указанной очереди задач.
		/// </summary>
		/// <param name="postRun">Метод, запускаемый после завершения очереди задач.</param>
		/// <param name="queue">Очередь задач.</param>
		public static void RunAfterQueue(Action postRun, params AsyncTask[] queue)
		{
			RunAfterQueue(postRun, true, queue);
		}
		/// <summary>
		/// Запускает метод после завершения указанной очереди задач.
		/// </summary>
		/// <param name="postRun">Метод, запускаемый после завершения очереди задач.</param>
		/// <param name="interruptError">Прерывать очередь задач при ошибке</param>
		/// <param name="queue">Очередь задач.</param>
		public static void RunAfterQueue(Action postRun, bool interruptError = true,
			params AsyncTask[] queue)
		{
			SynchronizationContext cox = SynchronizationContext.Current;
			ThreadPool.QueueUserWorkItem((state) =>
			{
				int count = 0;
				for (int a = 0; a < queue.Length; a++)
					if (queue[a] != null)
						count++;
				if (count != 0)
					for (int a = 0; a < queue.Length; a++)
					{
						AsyncTask at = queue[a];
						if (count > 1)
							at.IsQueued = true;
						count--;
						Run(at);
						at.thread.Join();
						if (interruptError && (at.IsAborted || at.Error != null))
							break;
					}
				if (postRun != null)
					cox.Send((obj) => { postRun(); }, null);
			});
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
	//**************************************************************************************
	#region << public class AsyncTask >>
	/// <summary>
	/// Класс асинхронно выполняющейся задачи.
	/// </summary>
	public class AsyncTask
	{
		private string name;
		private IAsyncTaskDoneHandler dst;
		private bool isAborted;
		private Exception exc;
		private object result;
		private Func<object> workMethod;
		private object tag = null;
		private bool isQueued = false;

		internal Thread thread = null;
		internal AutoResetEvent are = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает имя задачи.
		/// </summary>
		public string TaskName
		{
			get { return name; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет объект, обрабатывающий уведомления о завершении задачи. 
		/// </summary>
		public IAsyncTaskDoneHandler TaskDoneHandler
		{
			get { return dst; }
			set { dst = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, была ли задача прервана.
		/// </summary>
		public bool IsAborted
		{
			get { return isAborted; }
			internal set { isAborted = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет ошибку выполнения задачи.
		/// </summary>
		public Exception Error
		{
			get { return exc; }
			internal set { exc = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает результат выполнения задачи.
		/// </summary>
		public object Result
		{
			get { return result; }
			internal set { result = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Рабочий метод задачи.
		/// </summary>
		public Func<object> WorkMethod
		{
			get { return workMethod; }
			set { workMethod = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Дополнительная информация
		/// </summary>
		public object Tag
		{
			get { return tag; }
			set { tag = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, присутствует ли задача в очереди задач.
		/// </summary>
		public bool IsQueued
		{
			get { return isQueued; }
			set { isQueued = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public AsyncTask(string taskName, IAsyncTaskDoneHandler dst)
		{
			this.dst = dst;
			this.name = taskName;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public AsyncTask(string taskName, IAsyncTaskDoneHandler dst, Func<object> workMethod)
		{
			this.dst = dst;
			this.name = taskName;
			this.workMethod = workMethod;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public AsyncTask(string taskName, IAsyncTaskDoneHandler dst, Func<object> workMethod, object tag)
		{
			this.dst = dst;
			this.name = taskName;
			this.workMethod = workMethod;
			this.tag = tag;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Static Methods >>
		/// <summary>
		/// Возвращает асинхронную задачу через запуск делегата.
		/// </summary>
		/// <param name="func">Запускаемый делегат</param>
		/// <returns></returns>
		public static AsyncTask FromDelegate(Func<AsyncTask> func)
		{
			return func();
		}
		#endregion << Static Methods >>
		//-------------------------------------------------------------------------------------

	}
	#endregion << public class AsyncTask >>
}

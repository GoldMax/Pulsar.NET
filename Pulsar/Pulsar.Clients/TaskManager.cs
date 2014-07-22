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
	/// ��������� ��������, �������������� ����� ����������� � ���������� ����������� �����.
	/// </summary>
	public interface IAsyncTaskDoneHandler
	{
		/// <summary>
		/// �����, ����������� ����������� � ���������� �����.
		/// </summary>
		void AsyncTaskDone(AsyncTask task);
		/// <summary>
		/// ���������� ������������� ������ Invoke
		/// </summary>
		bool InvokeRequired { get; }
		/// <summary>
		/// �������� ����������� ����� � UI ������.
		/// </summary>
		/// <param name="task"></param>
		void Invoke(AsyncTask task);
	}
	#endregion << public interface IAsyncTaskDoneHandler >>
	//*************************************************************************************
	/// <summary>
	/// ����� ��������� ����������� �����.
	/// </summary>
	public static class TaskManager
	{
		private static HashSet<AsyncTask> tasks = new HashSet<AsyncTask>();
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ������� ����������� ������ � ��������� ��.
		/// </summary>
		/// <param name="taskName">��� ������.</param>
		/// <param name="dst">������, �������������� ����������� � ���������� ������.</param>
		/// <param name="workMethod">������� ����� ������.</param>
		/// <param name="tag">�������������� �����������</param>
		/// <returns>������ ����������� ������.</returns>
		public static AsyncTask Run(string taskName, IAsyncTaskDoneHandler dst, Func<object> workMethod, object tag = null)
		{
			AsyncTask task = new AsyncTask(taskName, dst) { WorkMethod = workMethod, Tag = tag };
			Run(task);
			return task;
		}
		/// <summary>
		/// ��������� ����������� ������.
		/// </summary>
		/// <param name="task">������ ����������� ������.</param>
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
		/// ��������� ��������� ������ ���������������. � ������ ������ ��� ���������� ������,
		/// ����������� ������ �� �����������.
		/// </summary>
		/// <param name="queue">����������� ������.</param>
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
		/// ����������, ����������� �� ���� �� ���� ������ ��� ���������� �������.
		/// </summary>
		/// <param name="dest">������, �������������� ����������� � ���������� ������.</param>
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
		/// ��������� ��� ������ ��� ���������� ������� �����������.
		/// </summary>
		/// <param name="dest">������, �������������� ����������� � ���������� ������.</param>
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
		/// ��������� ����� ����� ���������� ���� ��������� �����.
		/// </summary>
		/// <param name="postRun">�����, ����������� ����� ���������� �����.</param>
		/// <param name="tasks">����������� ������.</param>
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
		/// ��������� ����� ����� ���������� ��������� ������� �����.
		/// </summary>
		/// <param name="postRun">�����, ����������� ����� ���������� ������� �����.</param>
		/// <param name="queue">������� �����.</param>
		public static void RunAfterQueue(Action postRun, params AsyncTask[] queue)
		{
			RunAfterQueue(postRun, true, queue);
		}
		/// <summary>
		/// ��������� ����� ����� ���������� ��������� ������� �����.
		/// </summary>
		/// <param name="postRun">�����, ����������� ����� ���������� ������� �����.</param>
		/// <param name="interruptError">��������� ������� ����� ��� ������</param>
		/// <param name="queue">������� �����.</param>
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
	/// ����� ���������� ������������� ������.
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
		/// ���������� ��� ������.
		/// </summary>
		public string TaskName
		{
			get { return name; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������, �������������� ����������� � ���������� ������. 
		/// </summary>
		public IAsyncTaskDoneHandler TaskDoneHandler
		{
			get { return dst; }
			set { dst = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ���� �� ������ ��������.
		/// </summary>
		public bool IsAborted
		{
			get { return isAborted; }
			internal set { isAborted = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ ���������� ������.
		/// </summary>
		public Exception Error
		{
			get { return exc; }
			internal set { exc = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� ���������� ������.
		/// </summary>
		public object Result
		{
			get { return result; }
			internal set { result = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������� ����� ������.
		/// </summary>
		public Func<object> WorkMethod
		{
			get { return workMethod; }
			set { workMethod = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������������� ����������
		/// </summary>
		public object Tag
		{
			get { return tag; }
			set { tag = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������, ������������ �� ������ � ������� �����.
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
		/// ���������������� �����������.
		/// </summary>
		public AsyncTask(string taskName, IAsyncTaskDoneHandler dst)
		{
			this.dst = dst;
			this.name = taskName;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		public AsyncTask(string taskName, IAsyncTaskDoneHandler dst, Func<object> workMethod)
		{
			this.dst = dst;
			this.name = taskName;
			this.workMethod = workMethod;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
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
		/// ���������� ����������� ������ ����� ������ ��������.
		/// </summary>
		/// <param name="func">����������� �������</param>
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

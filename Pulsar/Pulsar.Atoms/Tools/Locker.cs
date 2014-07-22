//#define LockTrace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pulsar
{
	/// <summary>
	/// Класс объекта блокировки
	/// </summary>
	public class Locker	: ILocker
	{
		private static int _processorCount = Environment.ProcessorCount;
		private int _lockThis = 0;

		private byte _waitRead = 0;
		private byte _waitWrite = 0;
		private byte _runRead = 0;
		private byte _runWrite = 0;
		private byte _ups = 0;

		// TODO : Переделать
		private List<LockInfo> locks = new List<LockInfo>();
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Private Properties >>
		private byte WaitRead
		{
			get { return _waitRead; }
			set { _waitRead = value <= 0	? (byte)0 : value; }
		}
		private byte WaitWrite
		{
			get { return _waitWrite; }
			set { _waitWrite = value <= 0	? (byte)0 : value; }
		}
		private byte RunRead
		{
			get { return _runRead; }
			set { _runRead = value <= 0	? (byte)0 : value; }
		}
		private byte RunWrite
		{
			get { return _runWrite; }
			set { _runWrite = value <= 0	? (byte)0 : value; }
		}
		private byte Ups
		{
			get { return _ups; }
			set { _ups = value <= 0	? (byte)0 : value; }
		}
		#endregion << Private Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет, находится ли текущий поток в блокировке на чтение.
		/// </summary>
		public bool IsReadLockHeld
		{
			get
			{
				LockThis();
				LockInfo li = GetInfo();
				bool res = li.ReadCount > 0;
				UnLockThis();
				return res;
			}
		}
		/// <summary>
		/// Определяет, находится ли текущий поток в блокировке на запись.
		/// </summary>
		public bool IsWriteLockHeld
		{
			get
			{
				LockThis();
				LockInfo li = GetInfo();
				bool res = li.WriteCount > 0;
				UnLockThis();
				return res;
			}
		}
		/// <summary>
		/// Определяет, имеет ли объект хоть одну блокировку.
		/// </summary>
		public bool HasLock
		{
			get
			{
				LockThis();
				bool res = _waitRead > 0 || _waitWrite > 0 || _runRead > 0 || _runWrite > 0;
				UnLockThis();
				return res;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Public Methods >>
		public void EnterReadLock()
		{
			//Console.WriteLine("\t{0}:{2}->{1}", Thread.CurrentThread.ManagedThreadId, "EnterReadLock", Thread.CurrentThread.Name);
			LockThis();
			LockInfo li = GetInfo();
			if(li.WriteCount > 0)
			{
				#if LockTrace
				Console.WriteLine("\t   {0}:{1} Begin read ignore...", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
				#endif
				return;
			}
			li.ReadCount++;
			if(li.ReadCount == 1)
			{
				WaitRead++;
				//if(li.WriteCount == 0)
				Wait();
			}
			#if LockTrace
			Console.WriteLine("\t{0}:{2}-{1}", Thread.CurrentThread.ManagedThreadId, "EnterReadLock", Thread.CurrentThread.Name);
			#endif
			UnLockThis();
		}
		public void ExitReadLock()
		{
			//Console.WriteLine("\t{0}:{2}->{1}", Thread.CurrentThread.ManagedThreadId, "ExitReadLock", Thread.CurrentThread.Name);
			LockThis();
			LockInfo li = GetInfo();
			if(li.WriteCount > 0)
			{
				#if LockTrace
				Console.WriteLine("\t   {0}:{1} End read ignore...", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
				#endif
				return;
			}

			li.ReadCount--;
			if(li.Mode == 0)
			{
				RunRead--;
				locks.Remove(li);
			}

			#if LockTrace
			Console.WriteLine("\t{0}:{2}-{1}", Thread.CurrentThread.ManagedThreadId, "ExitReadLock", Thread.CurrentThread.Name);
			#endif
			UnLockThis();
		}
		public void EnterWriteLock()
		{
			//Console.WriteLine("\t{0}:{2}->{1}", Thread.CurrentThread.ManagedThreadId, "EnterWriteLock", Thread.CurrentThread.Name);
			LockThis();
			LockInfo li = GetInfo();
			li.WriteCount++;
			if(li.WriteCount == 1)
			{
				WaitWrite++;
				if(li.ReadCount > 0)
					Ups++;
				Wait();
			}
			#if LockTrace
			Console.WriteLine("\t{0}:{2}-{1}", Thread.CurrentThread.ManagedThreadId, "EnterWriteLock", Thread.CurrentThread.Name);
			#endif
			UnLockThis();
		}
		public void ExitWriteLock()
		{
			//Console.WriteLine("\t{0}:{2}->{1}", Thread.CurrentThread.ManagedThreadId, "ExitWriteLock", Thread.CurrentThread.Name);
			LockThis();
			LockInfo li = GetInfo();

			int m = li.Mode;
			li.WriteCount--;
			if(li.WriteCount == 0)
			{
				RunWrite--;
				if(m == 3)
					Ups--;
			}
			if(li.Mode == 0)
				locks.Remove(li);

			#if LockTrace
			Console.WriteLine("\t{0}:{2}-{1}", Thread.CurrentThread.ManagedThreadId, "ExitWriteLock", Thread.CurrentThread.Name);
			#endif
			UnLockThis();
		}
		#endregion << Public Methods >>
		//-------------------------------------------------------------------------------------
		#region << Private Methods >>
		private void Wait()
		{
		 int waitTime = 0;
			while(true)
			{
				LockThis();

				bool goWork = false;

				LockInfo li = GetInfo(false);
				switch(li.Mode)
				{
					case 1:
						if(WaitWrite == 0 && RunWrite == 0)
							goWork = true;
						break;
					case 2:
						if(RunRead == 0 && RunWrite == 0)
							goWork = true;
						break;
					case 3:
						if(RunRead - Ups == 0 && RunWrite == 0)
							goWork = true;
						break;
				}
				if(goWork == false)
				{
					#if LockTrace
					// TODO : Убрать
					if(li.wait == false)
						Console.WriteLine("\t   {0}:{1} waiting...", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
					li.wait = true;
					#endif

					UnLockThis();
					Thread.Sleep(30);
					waitTime += 30;
					if(waitTime > 60000)
					 throw new Exception("!!!! LOCKER TIMEOUT !!!");
					continue;
				}

				#if LockTrace
				// TODO : Убрать
				li.wait = false;
				#endif
				if(li.Mode == 1)
				{
					WaitRead--;
					RunRead++;
				}
				else if(li.Mode == 2 || li.Mode == 3)
				{
					WaitWrite--;
					RunWrite++;
				}
				else
					throw new Exception("Oops!");

				return;
			}
		}
		private LockInfo GetInfo(bool create = true)
		{
			int tId = Thread.CurrentThread.ManagedThreadId;
			LockInfo li = null;
			lock(locks)
			{
				foreach(var i in locks)
					if(i.threadId == tId)
					{
						li = i;
						break;
					}
				if(li == null && create)
					locks.Add(li = new LockInfo(tId));
			}
			return li;
		}
		private void LockThis()
		{
			int id = Thread.CurrentThread.ManagedThreadId;
			if(Interlocked.CompareExchange(ref _lockThis, id, 0) != 0)
			{
				int num = 0;
				do
				{
					if(Interlocked.CompareExchange(ref _lockThis, id, 0) == id)
						break;
					if(num < 10 && _processorCount > 1)
						Thread.SpinWait(20 * (num + 1));
					else if(num < 15)
						Thread.Sleep(0);
					else
						Thread.Sleep(1);
					num++;
				} while(true);
			}
		}
		private void UnLockThis()
		{
			Interlocked.Exchange(ref _lockThis, 0);
		}
		#endregion << Private Methods >>
	}

	//*************************************************************************************
	#region << internal class LockInfo >>
	internal class LockInfo
	{
		internal int pos = -1;
		internal readonly int threadId = -1;
		private int _readCount = 0;
		private int _writeCount = 0;

		#if LockTrace
		// TODO : Убрать, нужна только для отдадки
		public bool wait = false;
		#endif

		public int ReadCount
		{
			get { return _readCount; }
			set { _readCount = value <= 0	? 0 : value; }
		}
		public int WriteCount
		{
			get { return _writeCount; }
			set { _writeCount = value <= 0	? 0 : value; }
		}


		public int Mode
		{
			get
			{
				if(_readCount > 0 && _writeCount == 0)
					return 1;
				if(_readCount > 0 && _writeCount > 0)
					return 3;
				if(_readCount == 0 && _writeCount > 0)
					return 2;
				return 0;
			}
		}

		public LockInfo(int thID)
		{
			this.threadId = thID;
		}
		public LockInfo(int thID, int pos)
		{
			this.threadId = thID;
			this.pos = pos;
		}

	} 
	#endregion << internal class LockInfo >>

}

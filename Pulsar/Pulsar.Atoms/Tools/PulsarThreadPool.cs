using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pulsar
{
	/// <summary>
	/// Класс пула потоков Пульсара
	/// </summary>
	public static class PulsarThreadPool
	{
	 /// <summary>
	 /// Метод по умолчанию, выполняемый в отдельном потоке до вызова рабочего метода.
	 /// </summary>
		public static Action OnStartDefaultAction { get; set; }
	 /// <summary>
		///  Метод по умолчанию, выполняемый в отдельном потоке после вызова рабочего метода.
	 /// </summary>
		public static Action OnExitDefaultAction { get; set; }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Запускает выполнение рабочего метода в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод</param>
		/// <returns></returns>
		public static Task Run(Action work)
		{
			return Run(work, null, false, null, false);
		}
		/// <summary>
		/// Запускает выполнение рабочего метода в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод</param>
		/// <param name="noDefaults">Определяет блокирование вызовов методов по умолчанию.</param>
		/// <returns></returns>
		public static Task Run(Action work, bool noDefaults)
		{
			return Run(work, null, noDefaults, null, noDefaults);
		}
		/// <summary>
		/// Запускает выполнение рабочего метода в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод</param>
		/// <param name="noDefaultOnStart">Определяет блокирование вызова метода OnStartDefaultAction.</param>
		/// <param name="onStart">Метод, вызываемый в отдельном потоке до вызова рабочего метода.</param>
		/// <returns></returns>
		public static Task Run(Action work, bool noDefaultOnStart, Action onStart)
		{
			return Run(work, onStart, noDefaultOnStart, null, false);
		}
		/// <summary>
		///Запускает выполнение рабочего метода в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод</param>
		/// <param name="onExit">Метод, вызываемый в отдельном потоке после вызова рабочего метода.</param>
		/// <param name="noDefaultOnExit">Определяет блокирование вызова метода OnExitDefaultAction.</param>
		/// <returns></returns>
		public static Task Run(Action work, Action onExit, bool noDefaultOnExit)
		{
			return Run(work, null, false, onExit, noDefaultOnExit);
		}
		/// <summary>
		/// Запускает выполнение рабочего метода в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод</param>
		/// <param name="onStart">Метод, вызываемый в отдельном потоке до вызова рабочего метода.</param>
		/// <param name="noDefaultOnStart">Определяет блокирование вызова метода OnStartDefaultAction.</param>
		/// <param name="onExit">Метод, вызываемый в отдельном потоке после вызова рабочего метода.</param>
		/// <param name="noDefaultOnExit">Определяет блокирование вызова метода OnExitDefaultAction.</param>
		/// <returns></returns>
		public static Task Run(Action work, Action onStart, bool noDefaultOnStart, Action onExit, bool noDefaultOnExit)
		{
			if(work == null)
				throw new ArgumentNullException("work");
			var par = new Tuple<Action, bool, Action, Action, bool>(onStart,noDefaultOnStart,  work, onExit, noDefaultOnExit);
			return Task.Factory.StartNew(DefaultWork, par);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Запускает выполнение рабочего метода с параметром в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод с параметром.</param>
		/// <param name="data">Аргумент рабочего метода.</param>
		/// <returns></returns>
		public static Task Run(Action<object> work, object data)
		{
			return Run(work, data, null, false, null, false);
		}
		/// <summary>
		/// Запускает выполнение рабочего метода с параметром в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод с параметром.</param>
		/// <param name="data">Аргумент рабочего метода.</param>
		/// <param name="noDefaults">Определяет блокирование вызовов методов по умолчанию.</param>
		/// <returns></returns>
		public static Task Run(Action<object> work, object data, bool noDefaults)
		{
			return Run(work, data, null, noDefaults, null, noDefaults);
		}
		/// <summary>
		/// Запускает выполнение рабочего метода с параметром в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод с параметром.</param>
		/// <param name="data">Аргумент рабочего метода.</param>
		/// <param name="noDefaultOnStart">Определяет блокирование вызова метода OnStartDefaultAction.</param>
		/// <param name="onStart">Метод, вызываемый в отдельном потоке до вызова рабочего метода.</param>
		/// <returns></returns>
		public static Task Run(Action<object> work, object data, bool noDefaultOnStart, Action onStart)
		{
			return Run(work, data, onStart, noDefaultOnStart, null, false);
		}
		/// <summary>
		/// Запускает выполнение рабочего метода с параметром в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод с параметром.</param>
		/// <param name="data">Аргумент рабочего метода.</param>
		/// <param name="onExit">Метод, вызываемый в отдельном потоке после вызова рабочего метода.</param>
		/// <param name="noDefaultOnExit">Определяет блокирование вызова метода OnExitDefaultAction.</param>
		/// <returns></returns>
		public static Task Run(Action<object> work, object data, Action onExit, bool noDefaultOnExit)
		{
			return Run(work, data, null, false, onExit, noDefaultOnExit);
		}
		/// <summary>
		/// Запускает выполнение рабочего метода с параметром в отдельном потоке.
		/// </summary>
		/// <param name="work">Рабочий метод с параметром.</param>
		/// <param name="data">Аргумент рабочего метода.</param>
		/// <param name="onStart">Метод, вызываемый в отдельном потоке до вызова рабочего метода.</param>
		/// <param name="noDefaultOnStart">Определяет блокирование вызова метода OnStartDefaultAction.</param>
		/// <param name="onExit">Метод, вызываемый в отдельном потоке после вызова рабочего метода.</param>
		/// <param name="noDefaultOnExit">Определяет блокирование вызова метода OnExitDefaultAction.</param>
		/// <returns></returns>
		public static Task Run(Action<object> work, object data, Action onStart, bool noDefaultOnStart, Action onExit, bool noDefaultOnExit)
		{
			if(work == null)
				throw new ArgumentNullException("work");
			var par = new Tuple<Action, bool, Action<object>, object, Action, bool>(onStart, noDefaultOnStart, work, data, onExit, noDefaultOnExit);
			return Task.Factory.StartNew(DefaultWorkArg, par);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Ожидает завершения всех рабочих потоков
		/// </summary>
		/// <param name="tasks">Массив рабочих потоков.</param>
		public static void WaitAll(params Task[] tasks)
		{
			Task.WaitAll(tasks);
		}
		/// <summary>
		/// Ожидает завершения любого рабочего потока ихз указанных.
		/// </summary>
		/// <param name="tasks">Массив рабочих потоков.</param>
		public static void WaitAny(params Task[] tasks)
		{
			Task.WaitAny(tasks);
		}
		/// <summary>
		/// Возвращает максимальное число потоков пула.
		/// </summary>
		/// <param name="workerThreads">Число рабочих потоков.</param>
		/// <param name="completionPortThreads">Число потоков асинхронного ввода/вывода.</param>
		public static void GetMaxThreads(out int workerThreads, out int completionPortThreads)
		{
			ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
		}
		/// <summary>
		/// Возвращает число доступных потоков пула.
		/// </summary>
		/// <param name="workerThreads">Число рабочих потоков.</param>
		/// <param name="completionPortThreads">Число потоков асинхронного ввода/вывода.</param>
		public static void GetAvailableThreads(out int workerThreads, out int completionPortThreads)
		{
			ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
		}
		/// <summary>
		/// Возвращает число выполняемых потоков пула.
		/// </summary>
		/// <param name="workerThreads">Число рабочих потоков.</param>
		/// <param name="completionPortThreads">Число потоков асинхронного ввода/вывода.</param>
		public static void GetWorkedThreads(out int workerThreads, out int completionPortThreads)
		{
			int maxT = 0, maxIO = 0, avalT = 0, avalIO = 0;
		 ThreadPool.GetMaxThreads(out maxT, out maxIO);
		 ThreadPool.GetAvailableThreads(out avalT, out avalIO);
			workerThreads = maxT - avalT;
			completionPortThreads	= maxIO	- avalIO;
		}
		//-------------------------------------------------------------------------------------
		private static void DefaultWork(object data)
		{
			var par = (Tuple<Action, bool, Action, Action, bool>)data;
			if(par.Item2 == false && OnStartDefaultAction != null)
				OnStartDefaultAction();
			if(par.Item1 != null)
				par.Item1();
			try
			{
				par.Item3();
			}
			finally
			{
				if(par.Item4 != null)
					par.Item4();
				if(par.Item5 == false && OnExitDefaultAction != null)
					OnExitDefaultAction();
			}
		}
		private static void DefaultWorkArg(object data)
		{
			var par = (Tuple<Action, bool, Action<object>, object, Action, bool>)data;
			if(par.Item2 == false && OnStartDefaultAction != null)
				OnStartDefaultAction();
			if(par.Item1 != null)
				par.Item1();
			try
			{
				par.Item3(par.Item4);
			}
			finally
			{
				if(par.Item5 != null)
					par.Item5();
				if(par.Item6 == false && OnExitDefaultAction != null)
					OnExitDefaultAction();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс логирования.
	/// </summary>
	public class Logger
	{
		private static Byte _logLevel;

		#region Properties
		/// <summary>
		/// Текущий уровень логируемых сообщений.
		/// </summary>
		public static Byte LogLevel
		{
			get { return _logLevel; }
			set { _logLevel = value; }
		}
		/// <summary>
		/// Определяет, будет ли производиться вывод на Console или Debug
		/// </summary>
		public static bool LogToConsole { get; set; } 
		#endregion	Properties

		static Logger() { _logLevel = 3; }

		/// <summary>
		/// Записывает сообщение в журнал логирования.
		/// </summary>
		public static void Log(String message)
		{
			Log(3, message, true, null);
		}
		/// <summary>
		/// Записывает сообщение в журнал логирования.
		/// </summary>
		public static void Log(Byte level, String message)
		{
			Log(level, message, true, null);
		}

		/// <summary>
		/// Записывает сообщение в журнал логирования.
		/// </summary>
		public static void Log(Byte level, String message, bool breakLine)
		{
			Log(level, message, breakLine, null);
		}

		/// <summary>
		/// Записывает сообщение в журнал логирования.
		/// </summary>
		public static void Log(Byte level, String message, bool breakLine, params Object[] args)
		{
			if(level > _logLevel)
				return;
			if(args != null && args.Length > 0)
				message = String.Format(message,args);
			if(breakLine)
				message += System.Environment.NewLine;
			if(LogToConsole)
				Console.Write(message);
			else
				System.Diagnostics.Debug.Write(message, "Pulsar");
		}

		/// <summary>
		/// Записывает ошибку в журнал логирования.
		/// </summary>
		public static void LogError(Exception exc)
		{
			String s = "ERROR: " + exc.Message + exc.StackTrace;
			Log(0, s, true, null);
		}
	}
}


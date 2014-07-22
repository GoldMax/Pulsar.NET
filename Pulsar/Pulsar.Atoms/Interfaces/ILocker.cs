using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
 /// <summary>
 /// Интерфейс блокировщика объектов
 /// </summary>
	public interface ILocker
	{
	 /// <summary>
		/// Определяет, находится ли текущий поток в блокировке на чтение.
		/// </summary>
		bool IsReadLockHeld	{ get;	}
		/// <summary>
		/// Определяет, находится ли текущий поток в блокировке на запись.
		/// </summary>
		bool IsWriteLockHeld		{ get;	}
		/// <summary>
		/// Определяет, имеет ли объект хоть одну блокировку.
		/// </summary>
		bool HasLock		{ get;	}

		void EnterReadLock();
		void ExitReadLock();
		void EnterWriteLock();
		void ExitWriteLock();

	}
}

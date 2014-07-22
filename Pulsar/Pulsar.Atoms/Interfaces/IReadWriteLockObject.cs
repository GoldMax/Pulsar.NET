using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading;

namespace Pulsar
{
 /// <summary>
 /// Делегат обработчика вызова блокировки и разблокироваки объекта.
 /// </summary>
 /// <param name="obj">Блокируемый объект</param>
 /// <param name="locker">Объект блокировки.</param>
	public delegate void ReadWriteLockObjectHandler(IReadWriteLockObject obj, ref Locker locker);
	//**************************************************************************************
	#region << public enum LockType : byte >>
	/// <summary>
	/// Типы блокировки
	/// </summary>
	public enum LockType : byte
	{
		/// <summary>
		/// Нет блокировки
		/// </summary>
		NoLock = 0,
		/// <summary>
		/// Блокировка на чтение
		/// </summary>
		ReadLock = 1,
		/// <summary>
		/// Блокировка на запись
		/// </summary>
		WriteLock = 2
	}
	#endregion << public enum LockType : byte >>
	//**************************************************************************************
	#region << public interface IReadWriteLockObject >>
	/// <summary>
	/// Итерфейс объектов, поддерживающих блокировку на чтение и модификацию.
	/// </summary>
	public interface IReadWriteLockObject
	{
		/// <summary>
		/// Метод установки флага чтения объекта.
		/// </summary>
		void BeginRead();
		/// <summary>
		/// Метод снятия флага чтения объекта.
		/// </summary>
		void EndRead();
		/// <summary>
		/// Метод установки флага изменения объекта.
		/// </summary>
		void BeginWrite();
		/// <summary>
		/// Метод снятия флага изменения обслуживаемого объекта.
		/// </summary>
		void EndWrite();
		/// <summary>
		/// Определяет, находится ли объект в блокировке записи.
		/// </summary>
		bool IsWriteLocked { get; }
		/// <summary>
		/// Определяет, находится ли объект в блокировке чтения.
		/// </summary>
		bool IsReadLocked { get; }
		/// <summary>
		/// Определяет, находится ли объект в какой-нибудь блокировке.
		/// </summary>
		bool IsLocked { get; }
		/// <summary>
		/// Метод снятия всех блокировок объекта.
		/// </summary>
		void ClearAllLocks();
	} 
	#endregion << public interface IReadWriteLockObject >>
	//**************************************************************************************
	/// <summary>
	/// Атрибут типа блокировки объекта, устанавливаемой методом назначения.
	/// Для сборок и типов определяет возможность использования блокировки
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Assembly | AttributeTargets.Class |
		               AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Struct,
																	Inherited = true, AllowMultiple = false)]
	public class ReadWriteLockAttribute : Attribute
	{
		public LockType Type { get; set; }
		public ReadWriteLockAttribute() { }
		public ReadWriteLockAttribute(LockType type)
		{
			Type = type;
		}
	}
	/// <summary>
	/// Атрибут блокировки объекта на чтение, устанавливаемой методом назначения.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Assembly | AttributeTargets.Class |
		               AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Struct,
																	Inherited = true, AllowMultiple = false)]
	public class ReadLockAttribute : ReadWriteLockAttribute
	{
		public ReadLockAttribute() : base(LockType.ReadLock) { }
	}
	/// <summary>
	/// Атрибут блокировки объекта на запись, устанавливаемой методом назначения.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Assembly | AttributeTargets.Class |
		               AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Struct,
																	Inherited = true, AllowMultiple = false)]
	public class WriteLockAttribute : ReadWriteLockAttribute
	{
		public WriteLockAttribute() : base(LockType.WriteLock) { }
	}
	/// <summary>
	/// Атрибут отсутствия блокировки объекта.
	/// Для сборок и типов определяет возможность использования блокировки
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Assembly | AttributeTargets.Class |
		               AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Struct,
																	Inherited = true, AllowMultiple = false)]
	public class NoLockAttribute : ReadWriteLockAttribute
	{
		public NoLockAttribute() : base(LockType.NoLock) { }
	}

}

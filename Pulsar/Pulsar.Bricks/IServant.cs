using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Server
{
	/// <summary>
	/// Перечисление статусов сервантов
	/// </summary>
	public enum ServantStatus // : byte
	{
		/// <summary>
		/// Состояние неизвестно.
		/// </summary>
		Unknown,
		/// <summary>
		/// Слуга не инициализирован.
		/// </summary>
		NotInit,
		/// <summary>
		/// Слуга готов.
		/// </summary>
		Ready,
		/// <summary>
		/// Слуга занят.
		/// </summary>
		Busy,
		/// <summary>
		/// Ошибка слуги.
		/// </summary>
		Error,
		/// <summary>
		/// Фатальная ошибка слуги.
		/// </summary>
		Broken
	}

	//-------------------------------------------------------------------------------------
	/// <summary>
	/// Интерфейс слуг
	/// </summary>
	public interface IServant
	{
		/// <summary>
		/// Определяет, поддерживается ли для обслуживаемого объекта получение полной копии.
		/// </summary>
		bool CanGetFullObject { get; }
		/// <summary>
		/// Обслуживаемый объект
		/// </summary>
		Object ServedObject { get; }
		/// <summary>
		/// Метод сохранения объекта
		/// </summary>
		void Save();
		/// <summary>
		/// Статус слуги
		/// </summary>
		ServantStatus Status { get; }
	}
}

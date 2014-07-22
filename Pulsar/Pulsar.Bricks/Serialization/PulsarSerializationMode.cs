using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public enum PulsarSerializationMode >>
	/// <summary>
	/// Перечисление режимов сериализации
	/// </summary>
	[Flags]
	public enum PulsarSerializationMode : byte
	{
		/// <summary>
		/// Сериализация по умолчанию
		/// </summary>
		Default = 0,
		/// <summary>
		/// Сериализация для клиента
		/// </summary>
		ForClient = 1,
		/// <summary>
		/// Сериализация для сервера
		/// </summary>
		ForServer = ForClient,
		/// <summary>
		/// Сериализация для записи
		/// </summary>
		OnSave = 2,
		/// <summary>
		/// Сериализация для бэкапа.
		/// </summary>
		Backup = 4,
		/// <summary>
		/// Все режимы
		/// </summary>
		All = Default	| ForClient | OnSave	| Backup
	}
	#endregion << public enum PulsarSerializationMode >>
	//*************************************************************************************
	/// <summary>
	/// Перечисление опций сериализации
	/// </summary>
	[Flags]
	public enum PulsarSerializationOptions : byte
	{
		/// <summary>
		/// Нет опций
		/// </summary>
		None = 0,
		/// <summary>
		/// Игнорирование всех атрибутов PulsarByDemandSerialization
		/// </summary>
		IgnoreAllByDemandSerialization = 1,
		/// <summary>
		/// Заставляет сериализовать GOL объекты как обычные объекты.
		/// </summary>
		DeepSerialization = 2
	}
}

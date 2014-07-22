using System;

namespace Pulsar
{
	/// <summary>
	/// Перечисление типов клиентов Пульсара
	/// </summary>
	public enum ClientType : byte
	{
		/// <summary>
		/// Тип клиента не известен
		/// </summary>
		None = 0,
		/// <summary>
		/// Windows приложение
		/// </summary>
		WinForms = 1,
		/// <summary>
		/// Web приложение
		/// </summary>
		Web = 2,
		/// <summary>
		/// Консольное приложение
		/// </summary>
		Console = 3,
		/// <summary>
		/// Windows служба
		/// </summary>
		WinService = 4,
		/// <summary>
		/// Web служба
		/// </summary>
		WebService = 5,
		/// <summary>
		/// Другой клиент
		/// </summary>
		Other = 20
	}
}

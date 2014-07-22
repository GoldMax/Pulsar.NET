using System;

namespace Pulsar
{
	/// <summary>
	/// Тип запроса
	/// </summary>
	public enum QueryType: byte
	{
		/// <summary>
		/// Тип запроса не известен
		/// </summary>
		None = 0,
		/// <summary>
		/// Запрос на сверку версий и обновление (инсталляцию) программы
		/// </summary>
		Update = 1,
		/// <summary>
		/// Стандартный тип запроса, предназначен для работы с объектам
		/// </summary>
		Object = 2
	}
}

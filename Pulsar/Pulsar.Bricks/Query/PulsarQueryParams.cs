using System;

namespace Pulsar
{
	/// <summary>
	/// Перечисление параметров запроса.
	/// </summary>
	[Flags]
	public enum PulsarQueryParams : byte
	{
		/// <summary>
		/// Нет параметров.
		/// </summary>
		None = 0,
		/// <summary>
		/// Указание принудительной модификации объекта Пульсара.
		/// </summary>
		Modify = 1,
		/// <summary>
		/// Разрешение доступа к непубличным элементам объекта Пульсара.
		/// </summary>
		NonPublic = 2,
		/// <summary>
		/// Выполнение запроса слугой.
		/// </summary>
		Servant = 4,
		/// <summary>
		/// Выполнение кода на объекте
		/// </summary>
		Code = 8,
		/// <summary>
		/// Определяет возможность получения клиентом информации о выполнении запроса.
		/// </summary>
		Verbose = 16,
		/// <summary>
		/// Указывает на необходимость проверки наличия атрибутов сущностей у результирующего объекта,
		/// и добавлении типов, указанных в этих атрибутах, в список не заглушиваемых.
		/// </summary>
		NoStubEssences = 32,
		/// <summary>
		/// Игнорирование всех атрибутов PulsarByDemandSerialization
		/// </summary>
		IgnoreAllByDemandSerialization = 64,
		/// <summary>
		/// Указывает на заполнение версии в ответе номером версии объекта Пульсара.
		/// </summary>
		FillVersion = 128

	}
}

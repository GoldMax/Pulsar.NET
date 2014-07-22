using System;

namespace Pulsar
{
	/// <summary>
	/// Структура контекста запроса
	/// </summary>
	public struct Context
	{
		/// <summary>
		/// Пользователь
		/// </summary>
		public dynamic User;

		/// <summary>
		/// Версия клиента
		/// </summary>
		public uint ClientVersion;

		/// <summary>
		/// Тип клиента
		/// </summary>
		public ClientType ClientType;

		/// <summary>
		/// Имя клиента
		/// </summary>
		public string ClientName;

		//#region << Constructors >>

		///// <summary>
		///// Инициализирующий конструктор.
		///// </summary>
		//public Context(dynamic user, AssemblyVersion version, ClientType clientType,
		//    )
		//{
		//    User = null;
		//    ClientType = Pulsar.ClientType.None;
		//}

		//#endregion << Constructors >>
	}
}

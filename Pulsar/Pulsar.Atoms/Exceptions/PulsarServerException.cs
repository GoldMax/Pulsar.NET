using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс исключения, сигнализирующего об ошибке сервера.
	/// </summary>		
	[Serializable]
	public class PulsarServerException : PulsarException
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarServerException() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		public PulsarServerException(string message)
			: base(message)
		{

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="args">Аргументы текста сообщения.</param>
		public PulsarServerException(string message, params object[] args)
			: base(String.Format(message ?? "", args))
		{
		}
		#endregion << Constructors >>
	}
}

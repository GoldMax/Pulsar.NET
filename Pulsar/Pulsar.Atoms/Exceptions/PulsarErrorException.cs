using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс исключения, сигнализирующего об ошибке.
	/// </summary>		
	[Serializable]
	public class PulsarErrorException : PulsarException
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarErrorException() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		public PulsarErrorException(string message)
			: base(message)
		{

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="args">Аргументы текста сообщения.</param>
		public PulsarErrorException(string message, params object[] args)
			: base(String.Format(message ?? "", args))
		{
		}
		#endregion << Constructors >>
	}
}

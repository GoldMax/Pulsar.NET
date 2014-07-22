using System;
using System.Collections.Generic;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс исключения, принимающий параметры для составления сообщения.
	/// </summary>
	[Serializable]
	public class PulsarException : Exception
	{
		private string msg = null;

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Текст сообщения.
		/// </summary>
		public override string Message
		{
			get { return String.IsNullOrEmpty(msg) ? base.Message : msg; }
		}
	
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarException() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		public PulsarException(string message) : base (message)
		{

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="args">Аргументы текста сообщения.</param>
		public PulsarException(string message, params object[] args)
			: base() //: base(String.Format(message, args))
		{
			if(message == null)
				return;
			//message = message.Replace("{{", "\u0001");
			//message = message.Replace("}}", "\u0002");
			//for(int a = 0; a < args.Length; a++)
			// message = message.Replace("{" + a.ToString() + "}", args[a].ToString());
			//message = message.Replace("\u0001", "{{");
			//message = message.Replace("\u0002", "}}");
			msg = String.Format(message,args);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// GetObjectData
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("msg",msg);
			base.GetObjectData(info, context);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
}

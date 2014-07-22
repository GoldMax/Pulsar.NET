using System;
using System.Collections.Generic;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// ����� ����������, ����������� ��������� ��� ����������� ���������.
	/// </summary>
	[Serializable]
	public class PulsarException : Exception
	{
		private string msg = null;

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ����� ���������.
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
		/// ����������� �� ���������.
		/// </summary>
		public PulsarException() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������������� �����������.
		/// </summary>
		/// <param name="message">����� ���������.</param>
		public PulsarException(string message) : base (message)
		{

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		/// <param name="message">����� ���������.</param>
		/// <param name="args">��������� ������ ���������.</param>
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

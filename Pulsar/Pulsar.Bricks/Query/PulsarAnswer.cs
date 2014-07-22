using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс объекта ответа сервера Пульсара на запрос.
	/// </summary>
	public class PulsarAnswer : IDisposable
	{
		private PulsarAnswerStatus _a = PulsarAnswerStatus.OK;
		private object _r = null;
		private ulong? _v;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Статус ответа.
		/// </summary>
		public PulsarAnswerStatus Answer
		{
			get { return _a; }
			set { _a = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Объект с сообщением или данными.
		/// </summary>
		public object Return
		{
			get { return _r; }
			set { _r = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Версия вызываемого объекта Пульсара на сервере.
		/// </summary>
		public ulong? ServerObjectVersion
		{
			get { return _v; }
			set { _v = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarAnswer()
		{

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// <param name="status">Статус ответа.</param>
		/// <param name="ret"> Объект с сообщением или данными.</param>
		/// </summary>
		public PulsarAnswer(PulsarAnswerStatus status, object ret)
			: this()
		{
			Answer = status;
			Return = ret;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("Answer={0}¦Return={1}", Answer, Return ?? "null");
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IDisposable Members
		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			_r = null;
			_v = null;
		}

		#endregion
	}
	//*************************************************************************************
	#region << public enum PulsarAnswerStatus : byte >>
	/// <summary>
	/// Перечисление статусов ответа Пульсара.
	/// </summary>
	public enum PulsarAnswerStatus : byte
	{
		/// <summary>
		/// ОКеюшки
		/// </summary>
		OK = 0,
		/// <summary>
		/// Ошибка
		/// </summary>
		Error = 1,
		/// <summary>
		/// Сообщение
		/// </summary>
		TestMessage = 2,
		/// <summary>
		/// Сообщение об ошибке
		/// </summary>
		ErrorMessage = 3
	}
	#endregion << public enum PulsarAnswerStatus : byte >>
}

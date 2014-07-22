using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Serialization
{
	/// <summary>
	/// Класс параметров сериализации
	/// </summary>
	public class PulsarSerializationParams	
	{
		private PulsarSerializationMode mode = PulsarSerializationMode.Default;
		private IEnumerable<Type> noStubTypes = null;
		private IEnumerable<Type> asEmptyTypes = null;
		private IEnumerable<object> noStubObjects = null;
		private IEnumerable<object> asEmptyObjects = null;
		private IEnumerable<Type> byDemandTypes = null;
		private PulsarSerializationOptions opts = PulsarSerializationOptions.None;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Режим сериализации.
		/// </summary>
		public PulsarSerializationMode Mode
		{
			get { return mode; }
			set { mode = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Опции сериализации
		/// </summary>
		public PulsarSerializationOptions Options
		{
			get { return opts; }
			set { opts = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Типы объектов GOL, которые не должны быть заглушены.
		/// </summary>
		public IEnumerable<Type> NoStubTypes
		{
			get { return noStubTypes; }
			set { noStubTypes = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Типы объектов, которые должны быть сериализованы как пустые.
		/// </summary>
		public IEnumerable<Type> AsEmptyTypes
		{
			get { return asEmptyTypes; }
			set { asEmptyTypes = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Перечисление объектов GOL, которые не должны быть заглушены.
		/// </summary>
		public IEnumerable<object> NoStubObjects
		{
			get { return noStubObjects; }
			set { noStubObjects = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Перечисление объектов, которые должны быть сериализованы как пустые.
		/// </summary>
		public IEnumerable<object> AsEmptyObjects
		{
			get { return asEmptyObjects; }
			set { asEmptyObjects = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Перечисление типов полей, помеченных как сериализуемые по требованию, которые должны быть сериализованы.
		/// </summary>
		public IEnumerable<Type> ByDemandTypes
		{
			get { return byDemandTypes; }
			set { byDemandTypes = value; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarSerializationParams() { 	}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public PulsarSerializationParams(PulsarSerializationMode mode)
		{
			this.mode = mode;
		}
		#endregion << Constructors >>
	}

}

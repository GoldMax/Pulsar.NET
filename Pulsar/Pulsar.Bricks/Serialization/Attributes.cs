using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public abstract class PulsarSerializationAttribute : Attribute >>
	/// <summary>
	/// Базовый класс атрибутов сериализации Пульсара.
	/// </summary>
	public abstract class PulsarSerializationAttribute : Attribute
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarSerializationAttribute()
		{

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	} 
	#endregion << public abstract class PulsarSerializationAttribute : Attribute >>
	//**************************************************************************************
	/// <summary>
	/// Указывает на возможность пересылать объект наддого класса полностью в ответ на запрос.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
	public class ServantCanGetFullObjectAttribute : PulsarSerializationAttribute
	{
	 /// <summary>
	 /// 
	 /// </summary>
		public bool Can { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public ServantCanGetFullObjectAttribute(bool can)
		{
			Can = can;
		}
	}
	//*************************************************************************************
	/// <summary>
	/// Атрибут папки хранилища, в которой будет сохранятся объект.
	/// Значение null отменяет применение атрибута.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
	public class StoreFolderAttribute : PulsarSerializationAttribute
	{
	 /// <summary>
	 /// 
	 /// </summary>
		public String Folder { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public StoreFolderAttribute(string folder)
		{
			Folder = folder;
		}
	}
	//**************************************************************************************
	/// <summary>
	/// Класс атрибута контейнера сущностей. 
	/// Используется только для поддержания работы опции сериализации NoStubEssences.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
	public class PulsarEssencesHolderAttribute : PulsarSerializationAttribute
	{
		private Type types = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Типы сущности.
		/// </summary>
		public Type HoldedType
		{
			get { return types; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="holdedType">Тип сущности.</param>
		public PulsarEssencesHolderAttribute(Type holdedType)
			: base()
		{
			types = holdedType;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	//**************************************************************************************
	/// <summary>
	/// Класс атрибута, применяемого для управления сериализацией полей при различных режимах сериализации.
	/// Если режимы атрибута не указаны, то поле не сериализуется при любом режиме сериализации.
	/// Если указаны, то поле не сериализуется только в этих режимах.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class PulsarNonSerialized : PulsarSerializationAttribute
	{
		private PulsarSerializationMode mode = PulsarSerializationMode.Default;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Режим сериализации, при котором поле назначенияне будет сериализованно.
		/// </summary>
		public PulsarSerializationMode Mode
		{
			get { return mode;}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarNonSerialized() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="mode">Режим сериализации, при котором поле назначенияне будет сериализованно.</param>
		public PulsarNonSerialized(PulsarSerializationMode mode) : base()
		{
			this.mode = mode;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	//**************************************************************************************
	/// <summary>
	/// Класс атрибута, применяемого для управления сериализацией полей по требованию.
	/// Если режимы атрибута не указаны, то поле не сериализуется при любом режиме сериализации.
	/// Если указаны, то поле сериализуется только в этих режимах.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class PulsarByDemandSerialization : PulsarSerializationAttribute
	{
		private PulsarSerializationMode mode = PulsarSerializationMode.Default;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Режимы сериализации, при которых поле всегда сериализуется.
		/// </summary>
		public PulsarSerializationMode Excepts
		{
			get { return mode;}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarByDemandSerialization() : base() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="excepts">Режимы сериализации, при которых поле всегда сериализуется.</param>
		public PulsarByDemandSerialization(PulsarSerializationMode excepts) : base()
		{
			this.mode = excepts;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Clients
{
	/// <summary>
	/// Класс информации о форме и элементе меню.
	/// </summary>
	[Serializable]
	public class FormInfo
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		///// <summary>
		///// ID элемента меню (формы).
		///// </summary>
		//public int ID { get; set; }
		////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Название элемента меню.
		/// </summary>
		public string Caption { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Имя ресурса картинки элемента меню.
		/// </summary>
		public string Image { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Полное имя класса формы.
		/// </summary>
		public string FormClassName { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// SD формы.
		/// </summary>
		public OID SD { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет результирующий доступ к форме пользователя на чтение.
		/// </summary>
		public bool HasBrowseAccess { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет результирующий доступ уровня 1 к форме.
		/// </summary>
		public bool HasLevel1Access { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет результирующий доступ уровня 2 к форме.
		/// </summary>
		public bool HasLevel2Access { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет результирующий доступ уровня 3 к форме.
		/// </summary>
		public bool HasLevel3Access { get; set; }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public FormInfo()
		{
			SD = new OID();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Caption;
		}
		//[System.Runtime.Serialization.OnDeserialized]
		//private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		//{
		// if(SD.GetType() == typeof(Guid))
		//  SD = new OID((Guid)SD);
		//}
	}
}

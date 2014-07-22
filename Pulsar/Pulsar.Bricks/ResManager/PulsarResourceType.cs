using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Pulsar.Reflection;
using System.ComponentModel;

namespace Sim.Refs
{
	/// <summary>
	/// Перечисление типов ресурсов.
	/// </summary>
	// Числа не менять !!!
	[TypeConverter(typeof(EnumTypeConverter))]
	public enum PulsarResourceType : byte
	{
		/// <summary>
		/// Тип не известен
		/// </summary>
		[EnumItemDisplayName("(тип не известен)")]
		Unknown = 0,
		/// <summary>
		/// Коллекция изображений
		/// </summary>
		[EnumItemDisplayName("Коллекция изображений")]
		ImageList = 1,
		/// <summary>
		/// Web документ
		/// </summary>
		[EnumItemDisplayName("Web документ")]
		WebDoc = 2,
		/// <summary>
		/// Шаблон отчета
		/// </summary>
		[EnumItemDisplayName("Шаблон отчета")]
		ReportTemplate = 3
	}
}

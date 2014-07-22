using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Pulsar.Reflection;

namespace Pulsar
{
 [TypeConverter(typeof(EnumTypeConverter))]
	public enum Sex : byte
	{
	 /// <summary>
	 /// Самец
	 /// </summary>
		[EnumItemDisplayName("Мужской")]
	 Male = 0,
	 /// <summary>
	 /// Баба
	 /// </summary>
		[EnumItemDisplayName("Женский")]
		Female = 1
	}
}

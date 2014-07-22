using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar
{
	#region << public interface IOIDObject >>
	/// <summary>
	/// Интерфейс идентифицируемого по OID объекта.
	/// </summary>
	public interface IOIDObject
	{
		/// <summary>
		/// Идентификатор объекта.
		/// </summary>
		OID OID { get; }
	}
	#endregion << public interface IOIDObject >>
	//-------------------------------------------------------------------------------------
	#region << public interface IGlobalObjectMeta >>
	/// <summary>
	/// Интерфейс метаданных глобальных объектов.
	/// </summary>
	public interface IGlobalObjectMeta : IOIDObject
	{
		/// <summary>
		/// Глобальное имя объекта
		/// </summary>
		string GlobalName { get; set; }
	}
	#endregion << public interface IGlobalObjectMeta >>
}

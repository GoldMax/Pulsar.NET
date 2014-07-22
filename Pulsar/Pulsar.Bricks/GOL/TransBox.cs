using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Pulsar.Serialization;

namespace Pulsar
{
 /// <summary>
 /// Класс объекта транспортной упаковки объектов.
 /// </summary>
	public class TransBox
	{
		private byte[] _data = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		protected TransBox() { }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public TransBox(GlobalObject obj, PulsarSerializationParams pars)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			if(pars == null)
				throw new ArgumentNullException("obj");
			if(pars.NoStubObjects == null)
			 pars.NoStubObjects = new object[1] { obj };
			else	if(pars.NoStubObjects.Contains(obj) == false)
			{
			 pars.NoStubObjects = new List<object>(pars.NoStubObjects);
				((List<object>)pars.NoStubObjects).Add(obj);
			}
			using(MemoryStream ms = PulsarSerializer.Serialize(obj, pars))
			{
				ms.Position = 0;
				_data = ms.ToArray();
			}
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Распаковывает данные и десериализует их в объект.
		/// </summary>
		/// <param name="obj">Объект-назначение десериализации.</param>
		public void Unpack(GlobalObject obj)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			using(MemoryStream ms = new MemoryStream(_data))
			 PulsarSerializer.Deserialize(ms,obj, true);
		}
		///// <summary>
		///// Создает транспортный контейнер для объекта
		///// </summary>
		///// <param name="obj">Упаковываемый объект</param>
		///// <returns></returns>
		//public static TransBox<T> Pack(T obj)
		//{
		// return new TransBox<T>(obj);
		//}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------

	}
}

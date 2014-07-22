using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Serialization
{
	/// <summary>
	/// Данные для сериализации типа
	/// </summary>
	public class TypeData	//: System::Collections::IEnumerable			//			 IEnumerable<FieldData^>,
	{
		private long _handle = 0;
		private FieldData[] _fields = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Указатель на тип
		/// </summary>
		public long Handle 
		{
			get { return _handle; }
		} 
		public FieldData this[int index]
		{
			get { return _fields[index]; }
		} 
		public int FieldsCount { get { return _fields.Length; } }
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		private	TypeData() { }
		public TypeData(Type t)	
		{
			_handle = t.TypeHandle.Value.ToInt64();
			//_fields = new FieldData[0];
		}	
		#endregion << Constructors >>

	};
}

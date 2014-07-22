using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Serialization
{
	public class NativeSerializer
	{
	 private static TypeDataLibrary _types = new TypeDataLibrary();
				
		//-------------------------------------------------------------------------------------
	 #region << Constructors >>
	 static NativeSerializer()
		{
			_types.GetOrAdd(typeof(Boolean));
			_types.GetOrAdd(typeof(Byte));
			_types.GetOrAdd(typeof(SByte));
			_types.GetOrAdd(typeof(Int16));
			_types.GetOrAdd(typeof(UInt16));
			_types.GetOrAdd(typeof(Int32));
			_types.GetOrAdd(typeof(UInt32));
			_types.GetOrAdd(typeof(Int64));
			_types.GetOrAdd(typeof(UInt64));
			_types.GetOrAdd(typeof(Single));
			_types.GetOrAdd(typeof(Double));
			_types.GetOrAdd(typeof(Decimal));
			_types.GetOrAdd(typeof(IntPtr));
			_types.GetOrAdd(typeof(UIntPtr));
			_types.GetOrAdd(typeof(Char));
			_types.GetOrAdd(typeof(String));
			_types.GetOrAdd(typeof(Guid));
			_types.GetOrAdd(typeof(Pulsar.OID));
		}

		public	NativeSerializer() {}
	 #endregion << Constructors >>
		//-------------------------------------------------------------------------
		#region << Public Methods >>
		public void Serialize(Object obj)
		{
		
		} 
		#endregion << Public Methods >>
	}
}

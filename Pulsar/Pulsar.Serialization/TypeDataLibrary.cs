using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Serialization
{
	public class TypeDataLibrary : IDisposable
	{
		/// <summary>
		/// Максимальное количество типов
		/// </summary>
		public static readonly ushort MaxCount = 32749;
		
		private	TypeData[] _arr;
		private ushort[][] _index;
		private ushort _count;
		
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		public	TypeDataLibrary()
		{
			_count = 1;
			_arr = new TypeData[MaxCount];
			_index = new ushort[MaxCount][];
		}
		public void  Dispose()
		{
			_arr = null;
			_index = null;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		public ushort GetOrAdd(Type type)
		{
			lock(_arr)
			{
				long h = (long)type.TypeHandle.Value.ToInt64();

				ushort[] buf = _index[(ulong)h % MaxCount]; 
				if(buf != null)
				{
					for(ushort i = 1; i < buf.Length; i++)
						if(_arr[buf[i]].Handle == h)
							return buf[i];
				}
				_arr[_count] = new TypeData(type);
				IndexAdd(_count, h);
				_count++;			
				return (ushort)(_count-1);
			}
		}
		void IndexAdd(ushort pos, long handle )
		{
			int h = (int)((ulong)handle % MaxCount);
			if(_index[h] == null)
			{
				_index[h] = new ushort[1];
				_index[h][0]	= pos;		
			}
			else
			{
				ushort[] arr = _index[h];
				int count = arr.Length+1;
				_index[h] = new ushort[count];
				for(int a = 0; a < (count-1); a++)
					_index[h][a]	= arr[a];
				_index[h][count-1] = pos;
			}
		}
		#endregion << Methods >>		
	
	}
}

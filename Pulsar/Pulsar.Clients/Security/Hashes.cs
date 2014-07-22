using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Security
{
	/// <summary>
	/// Класс методов получения хэша.
	/// </summary>
	public static class Hash
	{
		/// <summary>
		/// Возвращает CRC32 контрольную сумму строки.
		/// </summary>
		/// <param name="str">Строка, для которой определяется контрольная сумма.</param>
		/// <returns></returns>
		public static int GetCRC32(string str)
		{
			ulong[] crc_table = new ulong[256];
			ulong crc = 0;

			for(int i = 0; i < 256; i++)
			{
				crc = (ulong)i;
				for(int j = 0; j < 8; j++)
					crc = (crc & 1) > 0 ? (crc >> 1) ^ 0xEDB88320UL : crc >> 1;
				crc_table[i] = crc;
			}
			crc = 0xFFFFFFFFUL;

			for(int i = 0; i < str.Length; i++)
				crc = crc_table[(crc ^ str[i]) & 0xFF] ^ (crc >> 8);

			return (int)(crc ^ 0xFFFFFFFFUL);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает хеш строки. Алгоритм хэширования можно повторить на T-SQL.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int GetSQLHash(string str)
		{
			int res = 0;
			foreach(char ch in str)
			{
				res = (res ^ ((int)ch *2)) * 2;
			}
			return (int)res;
		}
	}
}
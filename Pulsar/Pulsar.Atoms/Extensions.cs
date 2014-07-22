using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pulsar
{
	//*************************************************************************************
	/// <summary>
	/// Класс методов расширений для System.String
	/// </summary>
	public static class PulsarStringExtensions
	{
		/// <summary>
		/// Убирает начальные, конечные и сдвоенные (строенные и т.д ) пробелы из строки.
		/// </summary>
		/// <param name="str">Исходная строка.</param>
		/// <returns></returns>
		public static string TrimAll(this string str)
		{
			str = str.Trim();
			str = str.Replace("  ", " ");
			str = str.Replace("  ", " ");
			return str;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, является ли строка частью набора.
		/// </summary>
		/// <param name="str">Определяемая строка.</param>
		/// <param name="variants">Набор вариантов.</param>
		/// <returns></returns>
		public static bool In(this string str, IEnumerable<string> variants)
		{
			foreach(string s in variants)
				if(s == str)
					return true;
			return false;
		}
		/// <summary>
		/// Определяет, является ли строка частью набора.
		/// </summary>
		/// <param name="str">Определяемая строка.</param>
		/// <param name="variants">Набор вариантов.</param>
		/// <returns></returns>
		public static bool In(this string str, params string[] variants)
		{
			foreach(string s in variants)
				if(s == str)
					return true;
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает строку как заданное количество повторений указанной подстроки.
		/// </summary>
		/// <param name="str">Подстрока.</param>
		/// <param name="count">Количество повторений.</param>
		/// <returns></returns>
		public static string Repeat(string str, int count)
		{
			StringBuilder sb = new StringBuilder(str.Length*count);
			for(int a = 0; a < count; a++)
				sb.Append(str);
			return sb.ToString();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обращает строку.
		/// </summary>
		/// <param name="s">Обращаемая строка.</param>
		/// <returns></returns>
		public static string Reverse(this string s)
		{
			if(s == null)
				return null;
			char[] res = new char[s.Length];
			for(int a = 0; a < s.Length; a++)
				res[a] = s[s.Length-1-a];
			return new string(res);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Подготавливает строку, содержащую число, к преобразованию в число.
		/// </summary>
		/// <param name="s">Строка.</param>
		/// <returns></returns>
		public static string AsNumeric(this string s)
		{
			s = s.Replace(" ","");
			if(s.Length == 0)
				return null;
			return s.Replace('.',',').Replace(",",System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
		}
	}
	//*************************************************************************************
	/// <summary>
	/// Класс методов расширений для System.DateTime
	/// </summary>
	public static class PulsarDateTimeExtensions
	{
		/// <summary>
		/// Возвращает номер недели в году.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static int WeekOfYear(this DateTime date)
		{
			int shift = (int)(new DateTime(date.Year, 1, 1)).DayOfWeek;
			shift = shift == 0 ? 6 : shift -1;
			return (date.DayOfYear + shift)/7 + ((date.DayOfYear + shift)%7 == 0 ? 0 : 1);
		}
	}
	//*************************************************************************************
	/// <summary>
	/// Класс методов расширений для IEnumerable&lt;T&gt; и IEnumerable
	/// </summary>
	public static class PulsarIEnumerableExtensions
	{
		/// <summary>
		/// Проверяет элемент на присутствие в списке.
		/// </summary>
		/// <param name="ien"></param>
		/// <param name="item">Проверяемый элемент.</param>
		/// <returns>Если элемент в списке, возвращается сам элемент, иначе значение по умолчанию для типа.</returns>
		public static T InList<T>(this IEnumerable<T> ien, T item)
		{
			foreach(T t in ien)
				if(Object.Equals(t, item))
					return t;
			return default(T);
		}
		/// <summary>
		/// Проверяет элемент на присутствие в списке.
		/// </summary>
		/// <param name="ien"></param>
		/// <param name="comparer">Метод стравнения</param>
		/// <returns>Если элемент в списке, возвращается сам элемент, иначе значение по умолчанию для типа.</returns>
		public static T InList<T>(this IEnumerable<T> ien, Func<T,bool> comparer)
		{
			foreach(T t in ien)
				if(comparer(t))
					return t;
			return default(T);
		}
		/// <summary>
		/// Проверяет элемент на присутствие в перечислении.
		/// </summary>
		public static bool Contains(this IEnumerable ien, object item)
		{
			if(ien == null)
				return false;
			foreach(var i in ien)
				if(Object.Equals(i, item))
					return true;
			return false;
		}
		/// <summary>
		/// Создает массив с элементами перечисления.
		/// </summary>
		public static object[] ToObjectsArray(this IEnumerable ien)
		{
			if(ien == null)
				return new object[0];
			int count = 0;
			foreach(var i in ien)
				count ++;
			object[] res = new object[count];
			count = 0;
			foreach(var i in ien)
				res[count++] = i;
			return res;
		}
		/// <summary>
		/// Возвращает количество элементов в перечислении
		/// </summary>
		public static int Count(this IEnumerable ien)
		{
			if(ien == null)
				return 0;
			ICollection collection = ien as ICollection;
			if(collection != null)
				return collection.Count;
			int num = 0;
			foreach(var i in ien)
				num++;
			return num;
			
		}
		/// <summary>
		/// Проверяет два перечисления на идентичность их элементов
		/// </summary>
		public static bool SequenceEqual(this IEnumerable first, IEnumerable second)
		{
			if(first == null)
				throw new ArgumentNullException("first");
			if(second == null)
				throw new ArgumentNullException("first");
			IEnumerator enumerator = first.GetEnumerator();
			IEnumerator enumerator2 = second.GetEnumerator();
			bool result = true;
			while(enumerator.MoveNext())
			{
				if(!enumerator2.MoveNext() || !Object.Equals(enumerator.Current, enumerator2.Current))
				{
					result = false;
					break;
				}
			}
			if(result && enumerator2.MoveNext())
				result = false;
			return result;
		}
		/// <summary>
		/// Выполняет указанный метод для каждого элемента последовательности.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="ien"></param>
		/// <param name="method"></param>
		/// <returns>Последовальность результатов</returns>
		public static IEnumerable<U> ForEach<T,U>(this IEnumerable<T> ien, Func<T,U> method)
		{
			foreach(T t in ien)
			 yield return method(t);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ien"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static int IndexOf(this IEnumerable ien, object item)
		{
			if(ien is IList)
			 return ((IList)ien).IndexOf(item);
			int a = 0;
			foreach(var i in ien)
			 if(Object.Equals(i, item))
				 return a;
				else
				 a++;
				return -1;
		}
	}
	//*************************************************************************************
	/// <summary>
	/// Класс методов расширений для byte[]
	/// </summary>
	public static class PulsarByteArrayExtensions
	{
		/// <summary>
		/// Возвращает массив, составленный из элементов текущего массива.
		/// </summary>
		/// <param name="arr">Текущий массив.</param>
		/// <param name="pos">Позиция в текущем массиве, с которой начинается копирование.</param>
		/// <param name="lenght">Количество копируемых байт.</param>
		/// <returns></returns>
		public static byte[] GetSubArray(this byte[] arr, int pos, int lenght)
		{
			byte[] res = new byte[lenght];
			if(lenght == 0)
				return res;
			// ТОЛЬКО ДЛЯ МАССИВА БАЙТ
			Buffer.BlockCopy(arr,pos,res,0,lenght);
			return res;
		}
		/// <summary>
		/// Возвращает массив, составленный из элементов текущего массива, от указанной позиции до конца.
		/// </summary>
		/// <param name="arr">Текущий массив.</param>
		/// <param name="pos">Позиция в текущем массиве, с которой начинается копирование.</param>
		/// <returns></returns>
		public static byte[] GetSubArray(this byte[] arr, int pos)
		{
			byte[] res = new byte[arr.Length - pos];
			if(arr.Length-1 == pos)
				return res;
			// ТОЛЬКО ДЛЯ МАССИВА БАЙТ
			Buffer.BlockCopy(arr,pos,res,0,res.Length);
			return res;
		}
	}
	//*************************************************************************************
	/// <summary>
	/// Класс методов расширений для System.IO.Stream
	/// </summary>
	public static class StreamExtensions
	{
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Читает и возвращает заданное количество байт. В случае ошибки выбрасывает исключение.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="count">Количество байт для чтения.</param>
		/// <returns></returns>
		public static byte[] ReadBytes(this Stream s, uint count)
		{
			if(count <= 0)
				return new byte[0];
			byte[] res = new byte[count];
			int shift = 0;
			do
			{
				int step = s.Read(res, shift, (int)count - shift);
				if(step == 0)
					throw new Exception("Не удалость прочитать из потока заданое количество байт!");
				shift += step;
				//if (shift != count)
				// System.Threading.Thread.Sleep(50);
			} while(count != shift);
			return res;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Читает и возвращает значение UInt16 из потока.
		/// </summary>
		/// <returns></returns>
		public static ushort ReadUInt16(this Stream s)
		{
			//byte[] buf = new byte[2];
			//s.Read(buf,0,2);
			byte[] buf = ReadBytes(s, 2);
			return (ushort)(buf[0] +(buf[1] << 8));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Читает и возвращает значение UInt32 из потока.
		/// </summary>
		/// <returns></returns>
		public static uint ReadUInt32(this Stream s)
		{
			//byte[] buf = new byte[4];
			//s.Read(buf, 0, 4);
			byte[] buf = ReadBytes(s, 4);
			return (uint)(buf[0] + (buf[1] << 8) + (buf[2] << 16) + (buf[3] << 24));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Читает и возвращает значение Int32 из потока.
		/// </summary>
		/// <returns></returns>
		public static int ReadInt32(this Stream s)
		{
			return BitConverter.ToInt32(ReadBytes(s, sizeof(Int32)), 0);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Записывет буфер в поток.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="buf">Записываемый буфер.</param>
		public static void WriteBytes(this Stream s, byte[] buf)
		{
			s.Write(buf, 0, buf.Length);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Записывает значение UInt16 в поток.
		/// </summary>
		/// <returns></returns>
		public static void WriteUInt16(this Stream s, ushort value)
		{
			s.Write(new byte[] { (byte)value, (byte)(value >> 8) }, 0, 2);
			//WriteByte((byte)value);
			//WriteByte((byte)(value >> 8));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Записывает значение UInt32 в поток.
		/// </summary>
		/// <returns></returns>
		public static void WriteUInt32(this Stream s, uint value)
		{
			s.Write(new byte[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) }, 0, 4);
		}

	}

}

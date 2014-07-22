using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace Pulsar
{
	/// <summary>
	/// Класс буферизированного сетевого потока, с буферизированной записью
	/// </summary>
	public class BufferedNetworkStream : Stream
	{
		#region << Members >>

		private byte[] _buff = null;
		private int _pos = 0;
		private NetworkStream _netStream = null;

		#endregion << Members >>

		#region << Properties >>

		/// <summary>
		/// Возвращает значение, показывающее, поддерживает ли текущий поток возможность чтения
		/// </summary>
		public override bool CanRead
		{
			get { return _netStream.CanRead; }
		}

		/// <summary>
		/// Возвращает значение, которое показывает, поддерживается ли в текущем потоке возможность поиска
		/// </summary>
		public override bool CanSeek
		{
			get { return _netStream.CanSeek; }
		}

		/// <summary>
		/// Возвращает значение, показывающее, поддерживает ли текущий поток возможность записи
		/// </summary>
		public override bool CanWrite
		{
			get { return _netStream.CanWrite; }
		}

		/// <summary>
		/// Возращает длину потока в байтах
		/// </summary>
		public override long Length
		{
			get { return _netStream.Length; }
		}

		/// <summary>
		/// Получает или задает позицию в потоке
		/// </summary>
		public override long Position
		{
			get
			{
				return _netStream.Position;
			}
			set
			{
				_netStream.Position = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool CanTimeout
		{
			get
			{
				return _netStream.CanTimeout;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool DataAvailable
		{
			get { return _netStream.DataAvailable; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override int ReadTimeout
		{
			get
			{
				return _netStream.ReadTimeout;
			}
			set
			{
				_netStream.ReadTimeout = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override int WriteTimeout
		{
			get
			{
				return _netStream.WriteTimeout;
			}
			set
			{
				_netStream.WriteTimeout = value;
			}
		}

		#endregion << Properties >>

		#region << Constructors >>

		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="stream">Сетевой поток, который необходимо буферизировать</param>
		/// <param name="size">Размер буфера</param>
		public BufferedNetworkStream(NetworkStream stream, int size)
		{
			if(stream == null)
				throw new ArgumentNullException("stream");
			if(size <= 0)
				throw new ArgumentOutOfRangeException("size", "Размер буфера должен быть больше 0!");

			_netStream = stream;
			_buff = new byte[size];
		}

		#endregion << Constructors >>

		#region << Methods >>

		/// <summary>
		/// Выполняет запись одного байта в буферизированный поток, при заполнении буфера 
		/// сбрасывает его содержимое в сетевой поток
		/// </summary>
		/// <param name="value"></param>
		public override void WriteByte(byte value)
		{
			if (!_netStream.CanWrite)
				throw new InvalidOperationException("Текущий поток не позволяет вести запись!");

			if (_pos < _buff.Length)
				_buff[_pos++] = value;
			if (_pos == _buff.Length)
			{
				_netStream.Write(_buff, 0, _buff.Length);
				_pos = 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="size"></param>
		public override void Write(byte[] buffer, int offset, int size)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");
			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentOutOfRangeException("offset");
			if (size < 0 || size > (buffer.Length - offset))
				throw new ArgumentOutOfRangeException("size");
			if (!_netStream.CanWrite)
				throw new InvalidOperationException("Текущий поток не позволяет вести запись!");

			int copyBytes = 0, sentBytes = 0;
			while (sentBytes < size)
			{
				copyBytes = (size - sentBytes) > (_buff.Length - _pos)
					? (_buff.Length - _pos)
					: (size - sentBytes);
				Array.Copy(buffer, offset + sentBytes, _buff, _pos, copyBytes);
				sentBytes += copyBytes;
				_pos += copyBytes;
				if (_pos == _buff.Length)
				{
					_netStream.Write(_buff, 0, _buff.Length);
					_pos = 0;
				}
			}
		}

		/// <summary>
		/// Очищает все буферы данного потока и вызывает запись данных буферов в сетевой поток
		/// </summary>
		public override void Flush()
		{
			if (_pos > 0)
			{
				_netStream.Write(_buff, 0, _pos);
				_pos = 0;
			}
		}

		/// <summary>
		/// Считывает последовательность байт из потока
		/// </summary>
		/// <param name="buffer">Массив байтов - буфер в который необходимо произвести чтение</param>
		/// <param name="offset">Смещение в буфере, позиция с которой начинать запись данных в буфер</param>
		/// <param name="count">Количество байт, которое необходимо прочитать</param>
		/// <returns>Общее количество байт считанных в буфер</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			return _netStream.Read(buffer, offset, count);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			return _netStream.Seek(offset, origin);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public override void SetLength(long value)
		{
			_netStream.SetLength(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && _netStream != null)
				{
					Flush();
				}
			}
			finally
			{
				_buff = null;
				_netStream = null;
				base.Dispose(disposing);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int ReadByte()
		{
			return _netStream.ReadByte();
		}

		#endregion << Methods >>
	}
}

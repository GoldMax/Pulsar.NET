using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pulsar.Serialization
{
 #pragma warning disable
 public class DebugStream : Stream
 {
  Stream s = null;
  FileStream fs = null;
  string fsName = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  public string DebugFileName
  {
   get { return fsName; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public DebugStream(Stream stream, string debugDir)
  {
   if(stream == null)
    throw new ArgumentNullException("stream");
   s = stream;
   fsName = (debugDir ?? Path.GetTempPath()) + "\\" + Path.GetRandomFileName();
   fs = new FileStream(fsName, FileMode.CreateNew, FileAccess.Write);
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  public override bool CanRead
  {
   get { return s.CanRead; }
  }

  public override bool CanSeek
  {
   get { return s.CanSeek; }
  }

  public override bool CanWrite
  {
   get { return s.CanWrite; }
  }

  public override void Flush()
  {
   fs.Flush();
   s.Flush();
  }

  public override long Length
  {
   get { return s.Length; }
  }

  public override long Position
  {
   get { return s.Position; }
   set { s.Position = value; }
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
   int len = s.Read(buffer, offset, count);
   fs.Write(buffer, offset, len);
   return len;
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
   return s.Seek(offset, origin);
  }

  public override void SetLength(long value)
  {
   s.SetLength(value);
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
   s.Write(buffer, offset, count);
   fs.Write(buffer, offset, count);
  }

  public override void Close()
  {
   Flush();
   fs.Close();
  }

  public override int ReadByte()
  {
   int res = s.ReadByte();
   fs.WriteByte((byte)res);
   return res;
  }

  public override void WriteByte(byte value)
  {
   s.WriteByte(value);
   fs.WriteByte(value);
  }

 }
 #pragma warning restore
}

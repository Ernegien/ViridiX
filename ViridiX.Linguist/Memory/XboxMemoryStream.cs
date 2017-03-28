using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;
using ViridiX.Linguist.Network;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Memory
{
    /// <summary>
    /// Provides streaming access to Xbox memory.
    /// </summary>
    public class XboxMemoryStream : Stream
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BinaryReader _reader;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BinaryWriter _writer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int BufferSize = 128 * 1024;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const long XbeHeaderAddress = 0x10000;

        #endregion

        #region Properties

        /// <summary>
        /// Prevents crashing due to invalid memory address access. Does not protect against certain physical addresses mapped to hardware.
        /// </summary>
        public bool ProtectedMode { get; set; }

        /// <summary>
        /// TODO: description
        /// </summary>
        public sealed override long Position { get; set; }

        /// <summary>
        /// TODO: description
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// TODO: description
        /// </summary>
        public override bool CanSeek => true;

        /// <summary>
        /// TODO: description
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// TODO: description
        /// </summary>
        public override bool CanTimeout => true;

        /// <summary>
        /// TODO: description
        /// </summary>
        public override int ReadTimeout
        {
            get
            {
                return _xbox?.CommandSession.ReceiveTimeout ?? 0;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        public override int WriteTimeout
        {
            get
            {
                return _xbox?.CommandSession.SendTimeout ?? 0;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="xbox"></param>
        /// <param name="logger"></param>
        public XboxMemoryStream(Xbox xbox, ILogger logger = null)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = logger;
            _reader = new BinaryReader(this);
            _writer = new BinaryWriter(this);

            Position = XbeHeaderAddress;    // start at a valid memory address
            ProtectedMode = _xbox.CommandSession.Options.HasFlag(XboxConnectionOptions.ProtectedMode);
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: return Position = (uint)offset; // zero-based address
                case SeekOrigin.Current: return Position += (uint)offset;
                default: throw new InvalidOperationException("Invalid SeekOrigin.");
            }
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = 0;
            while (bytesRead < count)
            {
                int bytesToRead = Math.Min(BufferSize, count - bytesRead);
                ReadBytes(Position, buffer, offset + bytesRead, bytesToRead);

                bytesRead += bytesToRead;
                Position += bytesToRead;
            }

            return bytesRead;   // bytes returned should always be equal to count (unless a crash), using the accumulator for consistency anyways
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            int bytesWritten = 0;
            while (bytesWritten < count)
            {
                int bytesToWrite = Math.Min(BufferSize, count - bytesWritten);
                WriteBytes(Position, buffer, offset + bytesWritten, bytesToWrite);

                bytesWritten += bytesToWrite;
                Position += bytesToWrite;
            }
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferOffset"></param>
        /// <param name="count"></param>
        private void ReadBytes(long address, byte[] buffer, int bufferOffset, int count)
        {
            if (ProtectedMode && !_xbox.Memory.IsValidAddressRange(address, address + count))
            {
                throw new Exception("Invalid address detected during memory read.");
            }

            // TODO: protected mode getmem using patched xbdm.dll
            _xbox.CommandSession.SendCommandStrict("getmem2 addr={0} length={1}", address.ToHexString(), count);
            _xbox.CommandSession.Stream.Read(buffer, bufferOffset, count);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferOffset"></param>
        /// <param name="count"></param>
        private void WriteBytes(long address, byte[] buffer, int bufferOffset, int count)
        {
            if (ProtectedMode && !_xbox.Memory.IsValidAddressRange(address, address + count))
            {
                throw new Exception("Invalid address detected during memory write.");
            }

            // TODO: faster binary setmem with protected mode support using patched xbdm.dll
            const int maxBytesPerLine = 240;
            int totalWritten = 0;
            while (totalWritten < count)
            {
                int bytesToWrite = Math.Min(maxBytesPerLine, count - totalWritten);
                string hexData = buffer.ToHexString(bufferOffset + totalWritten, bytesToWrite);
                _xbox.CommandSession.SendCommandStrict("setmem addr={0} data={1}", (address + totalWritten).ToHexString(), hexData);
                totalWritten += bytesToWrite;
            }
        }

        #endregion

        #region Unsupported

        /// <summary>
        /// TODO: description. possibly remove exception and just do nothing
        /// </summary>
        public override void Flush() { throw new NotSupportedException(); }

        /// <summary>
        /// TODO: description. possibly return total memory size
        /// </summary>
        public override long Length { get { throw new NotSupportedException(); } }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value) { throw new NotSupportedException(); }

        #endregion

        #region Explicit Reads

        /// <summary>
        /// Reads a generic value type from Xbox memory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="peek"></param>
        /// <param name="size">Used only for arrays like strings and byte[]</param>
        /// <returns></returns>
        public T Read<T>(bool peek = false, int size = 0)
        {
            _logger?.Trace("Reading {0} from address {1}", typeof(T).Name, Position.ToHexString());
            return _reader.Read<T>(peek, size);
        }

        /// <summary>
        /// Reads a generic value type from Xbox memory at the specified address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="size">Used only for arrays like strings and byte[]</param>
        /// <returns></returns>
        public T Read<T>(long address, int size = 0)
        {
            Position = address;
            return Read<T>(false, size);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public byte ReadByte(long address)
        {
            Position = address;
            return Read<byte>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public byte ReadByte(bool peek = false)
        {
            return Read<byte>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public short ReadSByte(long address)
        {
            Position = address;
            return Read<sbyte>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public short ReadSByte(bool peek = false)
        {
            return Read<sbyte>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public char ReadChar(long address)
        {
            Position = address;
            return Read<char>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public char ReadChar(bool peek = false)
        {
            return Read<char>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public short ReadInt16(long address)
        {
            Position = address;
            return Read<short>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public short ReadInt16(bool peek = false)
        {
            return Read<short>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ReadUInt16(long address)
        {
            Position = address;
            return Read<ushort>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public int ReadUInt16(bool peek = false)
        {
            return Read<ushort>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ReadInt32(long address)
        {
            Position = address;
            return Read<int>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public int ReadInt32(bool peek = false)
        {
            return Read<int>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public long ReadUInt32(long address)
        {
            Position = address;
            return Read<uint>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public long ReadUInt32(bool peek = false)
        {
            return Read<uint>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public long ReadInt64(long address)
        {
            Position = address;
            return Read<long>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public long ReadInt64(bool peek = false)
        {
            return Read<long>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public BigInteger ReadUInt64(long address)
        {
            Position = address;
            return Read<ulong>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public BigInteger ReadUInt64(bool peek = false)
        {
            return Read<ulong>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public float ReadSingle(long address)
        {
            Position = address;
            return Read<float>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public float ReadSingle(bool peek = false)
        {
            return Read<float>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public double ReadDouble(long address)
        {
            Position = address;
            return Read<double>();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="peek"></param>
        /// <returns></returns>
        public double ReadDouble(bool peek = false)
        {
            return Read<double>(peek);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReadString(long address, int length)
        {
            Position = address;
            return Read<string>(false, length);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="length"></param>
        /// <param name="peek"></param>
        /// <returns></returns>
        public string ReadString(int length, bool peek = false)
        {
            return Read<string>(peek, length);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadBytes(long address, int length)
        {
            Position = address;
            return Read<byte[]>(false, length);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="length"></param>
        /// <param name="peek"></param>
        /// <returns></returns>
        public byte[] ReadBytes(int length, bool peek = false)
        {
            return Read<byte[]>(peek, length);
        }

        #endregion

        #region Explicit Writes

        /// <summary>
        /// Writes a generic value type to the Xbox memory stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Write<T>(T value)
        {
            _logger?.Trace("Writing {0} to address {1}", typeof(T).Name, Position.ToHexString());
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a generic value type to Xbox memory at the specified address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void Write<T>(long address, T value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteSByte(long address, byte value)
        {
            Position = address;
            Write((sbyte)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteSByte(byte value)
        {
            Write((sbyte)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteByte(long address, byte value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public override void WriteByte(byte value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteChar(long address, char value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteChar(char value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteInt16(long address, short value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt16(short value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt16(int value)
        {
            Write((ushort)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteUInt16(long address, int value)
        {
            Position = address;
            Write((ushort)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt32(int value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteInt32(long address, int value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt32(long value)
        {
            Write((uint)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteUInt32(long address, long value)
        {
            Position = address;
            Write((uint)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt64(long value)
        {
            Write<long>(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteInt64(long address, long value)
        {
            Position = address;
            Write<long>(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt64(BigInteger value)
        {
            Write((ulong)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteUInt64(long address, BigInteger value)
        {
            Position = address;
            Write((ulong)value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteSingle(float value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteSingle(long address, float value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteDouble(double value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteDouble(long address, double value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteString(long address, string value)
        {
            Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteBytes(byte[] value)
        {
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteBytes(long address, byte[] value)
        {
            Position = address;
            Write(value);
        }

        #endregion

        #region Implicit Writes

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="values"></param>
        public void Write(params object[] values)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                foreach (object value in values)
                {
                    switch (Convert.GetTypeCode(value))
                    {
                        case TypeCode.Boolean: bw.Write(Convert.ToBoolean(value)); break;
                        case TypeCode.Byte: bw.Write(Convert.ToByte(value)); break;
                        case TypeCode.SByte: bw.Write(Convert.ToSByte(value)); break;
                        case TypeCode.Char: bw.Write(Convert.ToByte(value)); break;
                        case TypeCode.Int16: bw.Write(Convert.ToInt16(value)); break;
                        case TypeCode.UInt16: bw.Write(Convert.ToUInt16(value)); break;
                        case TypeCode.Int32: bw.Write(Convert.ToInt32(value)); break;
                        case TypeCode.UInt32: bw.Write(Convert.ToUInt32(value)); break;
                        case TypeCode.Int64: bw.Write(Convert.ToInt64(value)); break;
                        case TypeCode.UInt64: bw.Write(Convert.ToUInt64(value)); break;
                        case TypeCode.Single: bw.Write(Convert.ToSingle(value)); break;
                        case TypeCode.Double: bw.Write(Convert.ToDouble(value)); break;
                        case TypeCode.String: bw.Write(Encoding.ASCII.GetBytes((string)value)); break;
                        default:

                            if (value is byte[])
                            {
                                bw.Write(value);
                            }

                            throw new NotSupportedException("Unsupported datatype.");
                    }
                }
                Write(ms.ToArray());
            }
        }

        /// <summary>
        /// Combines a series of objects and performs a single write operation to the specified memory address.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        public void Write(long address, params object[] values)
        {
            Position = address;
            Write(values);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Memory
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxMemory : IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BinaryReader _reader;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BinaryWriter _writer;

        /// <summary>
        /// TODO: description
        /// </summary>
        public const int PageSize = 0x1000;

        /// <summary>
        /// TODO: description
        /// </summary>
        public XboxMemoryStream Stream { get; }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="xbox"></param>
        /// <param name="logger"></param>
        public XboxMemory(Xbox xbox, ILogger logger = null)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = logger;
            Stream = new XboxMemoryStream(xbox, logger);
            _reader = new BinaryReader(Stream);
            _writer = new BinaryWriter(Stream);

        }

        /// <summary>
        /// TODO: description
        /// </summary>
        public List<XboxMemoryRegion> Regions
        {
            get
            {
                _xbox.CommandSession.SendCommandStrict("walkmem");
                List<XboxMemoryRegion> regions = new List<XboxMemoryRegion>();
                List<string> regionInfo = _xbox.CommandSession.ReceiveLines();

                foreach (var region in regionInfo)
                {
                    var info = region.ParseXboxResponseLine();
                    regions.Add(new XboxMemoryRegion((long)info["base"], (int)(long)info["size"], (XboxMemoryFlags)(long)info["protect"]));
                }

                return regions;
            }
        }

        /// <summary>
        /// Gets memory statistics.
        /// </summary>
        public XboxMemoryStatistics Statistics
        {
            get
            {
                _xbox.CommandSession.SendCommandStrict("mmglobal");
                var statText = _xbox.CommandSession.ReceiveText().ParseXboxResponseLine();

                // consolidate memory access into a single read operation for performance reasons
                byte[] pageInfo = ReadBytes((long)statText["AllocatedPagesByUsage"], 0x2C);

                XboxMemoryStatistics stats = new XboxMemoryStatistics
                {
                    AvailablePages = (long) statText["MmAvailablePages"],
                    TotalPages = (long) statText["MmNumberOfPhysicalPages"],
                    StackPages = BitConverter.ToUInt32(pageInfo, 4),
                    VirtualPageTablePages = BitConverter.ToUInt32(pageInfo, 8),
                    SystemPageTablePages = BitConverter.ToUInt32(pageInfo, 12),
                    PoolPages = BitConverter.ToUInt32(pageInfo, 16),
                    VirtualMappedPages = BitConverter.ToUInt32(pageInfo, 20),
                    ImagePages = BitConverter.ToUInt32(pageInfo, 28),
                    FileCachePages = BitConverter.ToUInt32(pageInfo, 32),
                    ContiguousPages = BitConverter.ToUInt32(pageInfo, 36),
                    DebuggerPages = BitConverter.ToUInt32(pageInfo, 40)
                };

                return stats;
            }
        }

        /// <summary>
        /// Calculates the checksum of a block of memory on the xbox.
        /// </summary>
        /// <param name="address">Memory address on the Xbox console of the first byte of memory in the block. This address must be aligned on an 8-byte boundary, and it cannot point to code.</param>
        /// <param name="length">Number of bytes on which to perform the checksum. This value must be a multiple of 8.</param>
        /// <returns></returns>
        public long GetChecksum(long address, int length)
        {
            // TODO: check if address range has any code pages if safe mode is enabled
            // TODO: reimplement with xbox-side script

            if ((address % 8) != 0) throw new ArgumentException("Address must be aligned on an 8-byte boundary.", nameof(address));
            if ((length % 8) != 0) throw new ArgumentException("Length must be a multiple of 8.", nameof(length));

            _xbox.CommandSession.SendCommandStrict("getsum addr={0} length={1} blocksize={1}", address.ToHexString(), length);
            return _xbox.CommandSession.Reader.ReadUInt32();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool IsValidAddress(long address)
        {
            _xbox.CommandSession.SendCommandStrict("getmem addr={0} length=1", address.ToHexString());
            return !_xbox.CommandSession.ReceiveText().Contains("??");
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        public bool IsValidAddressRange(long startAddress, long endAddress)
        {
            // TODO: offer secondary optimized method that executes a remote script on the xbox instead
            long address = startAddress & 0xFFFFF000;
            while (address < endAddress)
            {
                if (!IsValidAddress(address))
                {
                    return false;
                }
                address += PageSize;
            }

            return true;
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        public void Dispose()
        {
            Stream?.Dispose();
        }


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
            _logger?.Trace("Reading {0} from address {1}", typeof(T).Name, Stream.Position.ToHexString());
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
            Stream.Position = address;
            return Read<T>(false, size);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public byte ReadByte(long address)
        {
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            _logger?.Trace("Writing {0} to address {1}", typeof(T).Name, Stream.Position.ToHexString());
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
            Stream.Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteSByte(long address, byte value)
        {
            Stream.Position = address;
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
            Stream.Position = address;
            Write(value);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
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
            Stream.Position = address;
            Write(values);
        }

        #endregion
    }
}

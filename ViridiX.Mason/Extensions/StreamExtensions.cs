using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ViridiX.Mason.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads a value from a stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="peek"></param>
        /// <param name="size">Used only for arrays like strings and byte[]</param>
        /// <returns></returns>
        public static T Read<T>(this BinaryReader reader, bool peek = false, int size = 0)
        {
            long originalPosition = reader.BaseStream.Position;

            try
            {
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Boolean:
                        return (T)(object)reader.ReadBoolean();
                    case TypeCode.Char:
                        return (T)(object)reader.ReadChar();
                    case TypeCode.SByte:
                        return (T)(object)reader.ReadSByte();
                    case TypeCode.Byte:
                        return (T)(object)reader.ReadByte();
                    case TypeCode.Int16:
                        return (T)(object)reader.ReadInt16();
                    case TypeCode.UInt16:
                        return (T)(object)reader.ReadUInt16();
                    case TypeCode.Int32:
                        return (T)(object)reader.ReadInt32();
                    case TypeCode.UInt32:
                        return (T)(object)reader.ReadUInt32();
                    case TypeCode.Int64:
                        return (T)(object)reader.ReadInt64();
                    case TypeCode.UInt64:
                        return (T)(object)reader.ReadUInt64();
                    case TypeCode.Single:
                        return (T)(object)reader.ReadSingle();
                    case TypeCode.Double:
                        return (T)(object)reader.ReadDouble();
                    case TypeCode.String:
                        return (T)(object)Encoding.ASCII.GetString(reader.ReadBytes(size));
                    default:

                        if (typeof(T).Name == "Byte[]")
                        {
                            return (T)(object)reader.ReadBytes(size);
                        }

                        throw new InvalidCastException();
                }
            }
            finally
            {
                if (peek)
                {
                    reader.BaseStream.Position = originalPosition;
                }
            }
        }

        /// <summary>
        /// Writes a value to a stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void Write<T>(this BinaryWriter writer, T value)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    writer.Write((bool)(object)value);
                    break;
                case TypeCode.Char:
                    writer.Write((char)(object)value);
                    break;
                case TypeCode.SByte:
                    writer.Write((sbyte)(object)value);
                    break;
                case TypeCode.Byte:
                    writer.Write((byte)(object)value);
                    break;
                case TypeCode.Int16:
                    writer.Write((short)(object)value);
                    break;
                case TypeCode.UInt16:
                    writer.Write((ushort)(object)value);
                    break;
                case TypeCode.Int32:
                    writer.Write((int)(object)value);
                    break;
                case TypeCode.UInt32:
                    writer.Write((uint)(object)value);
                    break;
                case TypeCode.Int64:
                    writer.Write((long)(object)value);
                    break;
                case TypeCode.UInt64:
                    writer.Write((ulong)(object)value);
                    break;
                case TypeCode.Single:
                    writer.Write((float)(object)value);
                    break;
                case TypeCode.Double:
                    writer.Write((double)(object)value);
                    break;
                case TypeCode.String:
                    writer.Write((string)(object)value);
                    break;
                default:

                    if (value is byte[])
                    {
                        writer.Write(value as byte[]);
                        break;
                    }

                    throw new InvalidCastException();
            }
        }

        /// <summary>
        /// Copies stream data of specified count to the destination stream.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="count"></param>
        public static void CopyTo(this Stream source, Stream destination, long count)
        {
            long bytesCopied = 0;
            byte[] buffer = new byte[0x1000];

            do
            {
                int bytesToRead = Math.Min((int) (count - bytesCopied), buffer.Length);
                int bytesRead = source.Read(buffer, 0, bytesToRead);
                destination.Write(buffer, 0, bytesRead);
                bytesCopied += bytesRead;
            } while (bytesCopied < count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViridiX.Mason.Serialization
{
    public class DataDeserializer
    {
        public static T DeserializeBlock<T>(BinaryReader reader)
        {
            return (T)DeserializeBlock(reader, typeof(T));
        }

        public static object DeserializeBlock(BinaryReader reader, Type type)
        {
            // Create a new instance of the type to populate with data.
            object value = Activator.CreateInstance(type);

            // Get the field list for the specified type.
            FieldInfo[] fields = SerializerCache.GetOrCacheTypeInfo(type);

            // Loop and deserialize all fields.
            for (int i = 0; i < fields.Length; i++)
            {
                // Check if this is a primitive or complex type.
                if (fields[i].FieldType.IsArray == true)
                {
                    // Get the field value and check it is not null (arrays must be pre-initialized to get the length).
                    dynamic fieldValue = fields[i].GetValue(value);
                    if (fieldValue == null)
                        throw new InvalidDataException("Arrays must be pre-initialized with compile time constant length");

                    // Loop and deserialize the array.
                    for (int x = 0; x < fieldValue.Length; x++)
                    {
                        // Ahh yes, C# in all it's glory...
                        dynamic temp = DeserializePrimitiveValue(reader, fields[i].FieldType);
                        fieldValue[x] = temp;
                    }
                }
                else if (fields[i].FieldType.IsEnum == true)
                {
                    // Deserialize enum value.
                    fields[i].SetValue(value, DeserializePrimitiveValue(reader, Enum.GetUnderlyingType(fields[i].FieldType)));
                }
                else if (fields[i].FieldType.IsPrimitive == true)
                {
                    // Deserialize primitive value.
                    fields[i].SetValue(value, DeserializePrimitiveValue(reader, fields[i].FieldType));
                }
                else
                {
                    // Deserialize complex type.
                    fields[i].SetValue(value, DeserializeBlock(reader, fields[i].FieldType));
                }
            }

            // Return the deserialized field.
            return value;
        }

        public static T DeserializePrimitiveValue<T>(BinaryReader reader)
        {
            return (T)DeserializePrimitiveValue(reader, typeof(T));
        }

        public static object DeserializePrimitiveValue(BinaryReader reader, Type type)
        {
            // Check the type code of the field and handle accordingly.
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte: return reader.ReadByte();
                case TypeCode.Char: return reader.ReadChar();
                case TypeCode.Decimal: return reader.ReadDecimal();
                case TypeCode.Double: return reader.ReadDouble();
                case TypeCode.Int16: return reader.ReadInt16();
                case TypeCode.Int32: return reader.ReadInt32();
                case TypeCode.Int64: return reader.ReadInt64();
                case TypeCode.SByte: return reader.ReadSByte();
                case TypeCode.Single: return reader.ReadSingle();
                case TypeCode.UInt16: return reader.ReadUInt16();
                case TypeCode.UInt32: return reader.ReadUInt32();
                case TypeCode.UInt64: return reader.ReadUInt64();
                default:
                    {
                        // Field type not suppported.
                        throw new NotSupportedException($"Field type '{Type.GetTypeCode(type)}' not supported");
                    }
            }
        }
    }
}

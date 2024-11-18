using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViridiX.Mason.Utilities
{
    public static class BitUtilities
    {
        public static void ToBytes(short value, byte[] buffer, int index)
        {
            ToBytes(value, 2, buffer, index);
        }

        public static void ToBytes(ushort value, byte[] buffer, int index)
        {
            ToBytes(value, 2, buffer, index);
        }

        public static void ToBytes(int value, byte[] buffer, int index)
        {
            ToBytes(value, 4, buffer, index);
        }

        public static void ToBytes(uint value, byte[] buffer, int index)
        {
            ToBytes(value, 4, buffer, index);
        }

        public static void ToBytes(float value, byte[] buffer, int index)
        {
            ToBytes(value, 4, buffer, index);
        }

        public static void ToBytes(long value, byte[] buffer, int index)
        {
            ToBytes(value, 8, buffer, index);
        }

        public static void ToBytes(ulong value, byte[] buffer, int index)
        {
            ToBytes(value, 8, buffer, index);
        }

        private static void ToBytes(dynamic value, int count, byte[] buffer, int index)
        {
            // Loop for the number of bytes in the value.
            for (int i = 0; i < count; i++)
            {
                // Convert to bytes.
                buffer[index + i] = (byte)((value >> (i * 8)) & 0xFF);
            }
        }

        public static uint ParseUintHexString(string value)
        {
            // Remove the hex specifier if it exists as C# still can't handle strings with hex specifiers even when using
            // the 'AllowHexSpecifier' flag, 2022, what a time to be alive...
            if (value.StartsWith("0x") == true)
                return uint.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
            else
                return uint.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }
    }
}

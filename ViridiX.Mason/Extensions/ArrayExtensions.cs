using System;
using System.Text;

namespace ViridiX.Mason.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Converts an array of bytes to a hexidecimal string representation.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] data, int? offset = null, int? count = null)
        {
            int dataOffset = offset.GetValueOrDefault(0);
            int dataCount = count.GetValueOrDefault(data.Length - dataOffset);

            if (dataOffset < 0 || dataOffset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (dataCount <= 0 || dataOffset + dataCount > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            StringBuilder hexString = new StringBuilder();
            for (int i = 0; i < dataCount; i++)
            {
                hexString.Append(Convert.ToString(data[dataOffset + i], 16).ToUpperInvariant().PadLeft(2, '0'));
            }

            return hexString.ToString();
        }
    }
}

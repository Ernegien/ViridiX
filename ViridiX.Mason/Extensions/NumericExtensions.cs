using System;

namespace ViridiX.Mason.Extensions
{
    public static class NumericExtensions
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="value"></param>
        /// <param name="padWidth"></param>
        /// <returns></returns>
        public static string ToHexString(this long value, int padWidth = 0)
        {
            return "0x" + Convert.ToString(value, 16).PadLeft(padWidth, '0');
        }
    }
}

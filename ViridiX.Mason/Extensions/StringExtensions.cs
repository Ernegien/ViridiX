using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ViridiX.Mason.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extracts name/value pairs from an Xbox response line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ParseXboxResponseLine(this string line)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            var items = Regex.Matches(line, @"(\S+)\s*=\s*(\S+)");

            foreach (Match item in items)
            {
                string name = item.Groups[1].Value;
                string value = item.Groups[2].Value;

                long longValue;
                if (value.StartsWith("\""))
                {
                    // string
                    values[name] = value.Trim('"');
                }
                else if (value.StartsWith("0x"))
                {
                    // hexidecimal integer
                    values[name] = Convert.ToInt64(value, 16);
                }
                else if (long.TryParse(value, out longValue))
                {
                    // decimal integer
                    values[name] = longValue;
                }
                else
                {
                    throw new InvalidCastException("Unknown data type");
                }
            }

            return values;
        }
    }
}

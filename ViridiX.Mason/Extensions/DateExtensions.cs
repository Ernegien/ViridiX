using System;

namespace ViridiX.Mason.Extensions
{
    public static class DateExtensions
    {
        /// <summary>
        /// Converts a Win32 TimeDateStamp composed of epoch seconds into a DateTime object.
        /// </summary>
        /// <param name="epochSeconds"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeFromEpochSeconds(this long epochSeconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epochSeconds);
        }
    }
}

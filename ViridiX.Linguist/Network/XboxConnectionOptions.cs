using System;

namespace ViridiX.Linguist.Network
{
    /// <summary>
    /// Xbox connection options.
    /// </summary>
    [Flags]
    public enum XboxConnectionOptions
    {
        /// <summary>
        /// No connection options specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Substitutes most sleep calls with nothing at the expense of higher CPU usage.
        /// </summary>
        PerformanceMode = 1 << 0,

        /// <summary>
        /// Provides additional safety where possible.
        /// </summary>
        ProtectedMode = 1 << 1,

        /// <summary>
        /// Indicates whether or not this connection is reserved for notifications.
        /// </summary>
        NotificationSession = 1 << 3
    }
}

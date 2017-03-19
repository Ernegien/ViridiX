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
        PerformanceMode,

        /// <summary>
        /// Provides additional safety by severely limiting performance waiting the full jitter threshold for an idle link and flushing before sending each command.
        /// </summary>
        SafeMode,

        /// <summary>
        /// Indicates whether or not this connection is reserved for notifications.
        /// </summary>
        NotificationSession
    }
}

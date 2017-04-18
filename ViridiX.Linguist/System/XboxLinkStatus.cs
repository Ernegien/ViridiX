using System;

namespace ViridiX.Linguist.System
{
    /// <summary>
    /// Ethernet link status flags.
    /// </summary>
    [Flags]
    public enum XboxLinkStatus
    {
        /// <summary>
        /// Ethernet cable is connected and active.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Ethernet link speed is set to 100 Mbps.
        /// </summary>
        FastEthernet = 2,

        /// <summary>
        /// Ethernet link speed is set to 10 Mbps.
        /// </summary>
        Ethernet = 4,

        /// <summary>
        /// Ethernet link is in full duplex mode.
        /// </summary>
        FullDuplex = 8,

        /// <summary>
        /// Ethernet link is in half duplex mode.
        /// </summary>
        HalfDuplex = 16
    }
}

using System;

namespace ViridiX.Linguist.System
{
    /// <summary>
    /// Xbox hardware information.
    /// </summary>
    [Flags]
    public enum XboxHardwareFlags
    {
        /// <summary>
        /// Contains an internal USB hub.
        /// </summary>
        InternalUsbHub = 1 << 0,

        /// <summary>
        /// Contains a devkit kernel.
        /// </summary>
        DevkitKernel = 1 << 1,

        /// <summary>
        /// Contains an XCalibur TV encoder.
        /// </summary>
        XCaliburEncoder = 1 << 2,

        /// <summary>
        /// Chihiro
        /// </summary>
        Arcade = 1 << 3,

        /// <summary>
        /// Unknown flag 4
        /// </summary>
        Unknown4 = 1 << 4,

        /// <summary>
        /// Contains a Focus TV encoder.
        /// </summary>
        FocusEncoder = 1 << 5,

        /// <summary>
        /// Unknown flag 6
        /// </summary>
        Unknown6 = 1 << 6,

        /// <summary>
        /// Unknown flag 7
        /// </summary>
        Unknown7 = 1 << 7,

        /// <summary>
        /// Unknown flag 8
        /// </summary>
        Unknown8 = 1 << 8,

        /// <summary>
        /// Unknown flag 9
        /// </summary>
        Unknown9 = 1 << 9,

        /// <summary>
        /// Possibly unused
        /// </summary>
        Unknown10 = 1 << 10
    }
}

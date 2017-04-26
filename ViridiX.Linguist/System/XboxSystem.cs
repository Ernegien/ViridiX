using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ViridiX.Linguist.Network;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.System
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxSystem
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        /// <summary>
        /// Gets or sets the Xbox name.
        /// </summary>
        public string Name
        {
            get
            {
                // Xbox BUG: returns "OK" when un-named
                return _xbox.CommandSession.SendCommandStrict("dbgname").Message;
            }
            set
            {
                _xbox.CommandSession.SendCommandStrict("dbgname name=\"{0}\"", value);
            }
        }

        /// <summary>
        /// Gets or sets the xbox system time.
        /// </summary>
        public DateTime Time
        {
            get
            {
                XboxCommandResponse response = _xbox.CommandSession.SendCommandStrict("systime");
                var timeParts = response.Message.ParseXboxResponseLine();
                long ticks = ((long)timeParts["high"] << 32) | (long)timeParts["low"];
                return DateTime.FromFileTime(ticks);
            }
            set
            {
                _logger?.Info("Setting the Xbox time to {0}", value.ToString(CultureInfo.InvariantCulture));

                long fileTime = value.ToFileTimeUtc();
                long hi = fileTime >> 32;
                long lo = fileTime & uint.MaxValue;
                _xbox.CommandSession.SendCommandStrict("setsystime clockhi={0} clocklo={1} tz=1", hi, lo);
            }
        }

        /// <summary>
        /// Xbox system hardware information.
        /// </summary>
        public XboxHardwareInformation HardwareInfo
        {
            get
            {
                XboxHardwareInformation info = new XboxHardwareInformation();
                var data = _xbox.Memory.ReadBytes(_xbox.Kernel.Exports.HardwareInfo, 6);
                info.Flags = (XboxHardwareFlags)BitConverter.ToUInt32(data, 0);
                info.GpuRevision = data[4];
                info.McpRevision = data[5];
                return info;
            }
        }

        /// <summary>
        /// Retrieves the current network link status.
        /// </summary>
        public XboxLinkStatus LinkStatus => (XboxLinkStatus)(long)_xbox.Process.Call(_xbox.Kernel.Exports.PhyGetLinkState, 0);

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="xbox"></param>
        public XboxSystem(Xbox xbox)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = xbox.Logger;

            _logger?.Info("XboxSystem subsystem initialized");
        }

        /// <summary>
        /// Sets the Xbox LED state.
        /// </summary>
        /// <param name="state1">First LED state.</param>
        /// <param name="state2">Second LED state.</param>
        /// <param name="state3">Third LED state.</param>
        /// <param name="state4">Fourth LED state.</param>
        public void SetLedState(LedState state1, LedState state2, LedState state3, LedState state4)
        {
            byte state = 0;
            state |= (byte)state1;
            state |= (byte)((byte)state2 >> 1);
            state |= (byte)((byte)state3 >> 2);
            state |= (byte)((byte)state4 >> 3);
            _xbox.Process.Call(_xbox.Kernel.Exports.HalWriteSMBusValue, SmcDevices.SmBus, SmBusCommand.LedStates, 0, state);
            _xbox.Process.Call(_xbox.Kernel.Exports.HalWriteSMBusValue, SmcDevices.SmBus, SmBusCommand.LedOverride, 0, LedSubCommand.Custom);
            Thread.Sleep(10);
        }

        /// <summary>
        /// Restores the Xbox LED to its default state.
        /// </summary>
        public void RestoreDefaultLedState()
        {
            _xbox.Process.Call(_xbox.Kernel.Exports.HalWriteSMBusValue, SmcDevices.SmBus, SmBusCommand.LedOverride, 0, LedSubCommand.Default);
            Thread.Sleep(10);
        }
    }
}

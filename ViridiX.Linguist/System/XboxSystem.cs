using System;
using System.Diagnostics;
using System.Globalization;
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
        /// TODO: description
        /// </summary>
        /// <param name="xbox"></param>
        /// <param name="logger"></param>
        public XboxSystem(Xbox xbox, ILogger logger)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = logger;

            _logger?.Info("XboxSystem subsystem initialized");
        }
    }
}

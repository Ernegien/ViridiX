using System;
using System.Diagnostics;
using ViridiX.Mason.Collections;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Process
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxDebugMonitor
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Version _version;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XboxModule _module;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] _data;

        /// <summary>
        /// The Xbox debug monitor module name.
        /// </summary>
        public const string ModuleName = "xbdm.dll";

        /// <summary>
        /// Gets xbdm data from memory. This is cached for future processing.
        /// </summary>
        public byte[] Data => _data ?? (_data = _xbox.Memory.ReadBytes(Module.BaseAddress, Module.Size));

        /// <summary>
        /// Returns the debug monitor version.
        /// </summary>
        public Version Version
        {
            // TODO: figure out a more elegant way to do this
            get
            {
                if (_version != null) return _version;

                int rsrcOffset = 0;
                foreach (var section in Module.Sections)
                {
                    if (!section.Name.Equals(".rsrc")) continue;
                    rsrcOffset = (int)(section.Base - Module.BaseAddress);
                    break;
                }

                if (rsrcOffset != 0)
                {
                    // half-assed VS_VERSIONINFO parsing that works fine for our intended purposes
                    int major = BitConverter.ToUInt16(Data, rsrcOffset + 0x9A);
                    int minor = BitConverter.ToUInt16(Data, rsrcOffset + 0x98);
                    int build = BitConverter.ToUInt16(Data, rsrcOffset + 0x9E);
                    int revision = BitConverter.ToUInt16(Data, rsrcOffset + 0x9C);
                    _version = new Version(major, minor, build, revision);
                    _logger?.Info("Debug monitor version {0} detected", _version);
                }
                else _version = new Version("0.0.0.0");

                return _version;
            }
        }

        /// <summary>
        /// The Xbox debug monitor module.
        /// </summary>
        public XboxModule Module => _module ?? (_module = _xbox.Process.GetModule(ModuleName));

        public XboxDebugMonitor(Xbox xbox)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = xbox.Logger;

            _logger?.Info("XboxDebugMonitor subsystem initialized");
        }

        /// <summary>
        /// The HrFunctionCall function address.
        /// </summary>
        public long HrFunctionCall => Module.BaseAddress + new DefaultDictionary<int, long>(Version.Build)
        {
            // TODO: more versions
            [3146] = 0x4BBB,
            [5455] = 0xCC30,
            [5558] = 0xD870,
            [5933] = 0xD82D,
            [7887] = 0xCC30
        };
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Memory
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxMemory : IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        /// <summary>
        /// TODO: description
        /// </summary>
        public const int PageSize = 0x1000;

        /// <summary>
        /// TODO: description
        /// </summary>
        public XboxMemoryStream Stream { get; }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="xbox"></param>
        /// <param name="logger"></param>
        public XboxMemory(Xbox xbox, ILogger logger = null)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = logger;
            Stream = new XboxMemoryStream(xbox, logger);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        public List<XboxMemoryRegion> Regions
        {
            get
            {
                _xbox.CommandSession.SendCommandStrict("walkmem");
                List<XboxMemoryRegion> regions = new List<XboxMemoryRegion>();
                List<string> regionInfo = _xbox.CommandSession.ReceiveLines();

                foreach (var region in regionInfo)
                {
                    var info = region.ParseXboxResponseLine();
                    regions.Add(new XboxMemoryRegion((long)info["base"], (int)(long)info["size"], (XboxMemoryFlags)(long)info["protect"]));
                }

                return regions;
            }
        }

        /// <summary>
        /// Gets memory statistics.
        /// </summary>
        public XboxMemoryStatistics Statistics
        {
            get
            {
                _xbox.CommandSession.SendCommandStrict("mmglobal");
                var statText = _xbox.CommandSession.ReceiveText().ParseXboxResponseLine();

                // consolidate memory access into a single read operation for performance reasons
                byte[] pageInfo = Stream.ReadBytes((long)statText["AllocatedPagesByUsage"], 0x2C);

                XboxMemoryStatistics stats = new XboxMemoryStatistics
                {
                    AvailablePages = (long) statText["MmAvailablePages"],
                    TotalPages = (long) statText["MmNumberOfPhysicalPages"],
                    StackPages = BitConverter.ToUInt32(pageInfo, 4),
                    VirtualPageTablePages = BitConverter.ToUInt32(pageInfo, 8),
                    SystemPageTablePages = BitConverter.ToUInt32(pageInfo, 12),
                    PoolPages = BitConverter.ToUInt32(pageInfo, 16),
                    VirtualMappedPages = BitConverter.ToUInt32(pageInfo, 20),
                    ImagePages = BitConverter.ToUInt32(pageInfo, 28),
                    FileCachePages = BitConverter.ToUInt32(pageInfo, 32),
                    ContiguousPages = BitConverter.ToUInt32(pageInfo, 36),
                    DebuggerPages = BitConverter.ToUInt32(pageInfo, 40)
                };

                return stats;
            }
        }

        /// <summary>
        /// Calculates the checksum of a block of memory on the xbox.
        /// </summary>
        /// <param name="address">Memory address on the Xbox console of the first byte of memory in the block. This address must be aligned on an 8-byte boundary, and it cannot point to code.</param>
        /// <param name="length">Number of bytes on which to perform the checksum. This value must be a multiple of 8.</param>
        /// <returns></returns>
        public long GetChecksum(long address, int length)
        {
            // TODO: check if address range has any code pages if safe mode is enabled
            // TODO: reimplement with xbox-side script

            if ((address % 8) != 0) throw new ArgumentException("Address must be aligned on an 8-byte boundary.", nameof(address));
            if ((length % 8) != 0) throw new ArgumentException("Length must be a multiple of 8.", nameof(length));

            _xbox.CommandSession.SendCommandStrict("getsum addr={0} length={1} blocksize={1}", address.ToHexString(), length);
            return _xbox.CommandSession.Reader.ReadUInt32();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool IsValidAddress(long address)
        {
            _xbox.CommandSession.SendCommandStrict("getmem addr={0} length=1", address.ToHexString());
            return !_xbox.CommandSession.ReceiveText().Contains("??");
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        public bool IsValidAddressRange(long startAddress, long endAddress)
        {
            // TODO: offer secondary optimized method that executes a remote script on the xbox instead
            long address = startAddress & 0xFFFFF000;
            while (address <= endAddress)
            {
                if (!IsValidAddress(address))
                {
                    return false;
                }
                address += PageSize;
            }

            return true;
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        public void Dispose()
        {
            Stream?.Dispose();
        }
    }
}

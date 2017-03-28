using System.Diagnostics;

namespace ViridiX.Linguist.Memory
{
    /// <summary>
    /// Xbox memory region.
    /// </summary>
    [DebuggerDisplay("{(uint)Address} - {(uint)(Address + Size)} : {Protect}")]
    public class XboxMemoryRegion
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        public long Address;

        /// <summary>
        /// TODO: description
        /// </summary>
        public int Size;

        /// <summary>
        /// TODO: description
        /// </summary>
        public XboxMemoryFlags Protect;

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        /// <param name="protect"></param>
        public XboxMemoryRegion(long address, int size, XboxMemoryFlags protect)
        {
            Address = address;
            Size = size;
            Protect = protect;
        }
    }
}

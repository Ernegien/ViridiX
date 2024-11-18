using System;
using ViridiX.Mason.Utilities;

namespace ViridiX.Linguist.Process
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxThread
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        public int Id;

        /// <summary>
        /// TODO: description
        /// </summary>
        public int Suspend;

        /// <summary>
        /// TODO: description
        /// </summary>
        public int Priority;

        /// <summary>
        /// TODO: description
        /// </summary>
        public long TlsBase;

        /// <summary>
        /// TODO: description
        /// </summary>
        public long Start;

        /// <summary>
        /// TODO: description
        /// </summary>
        public long Base;

        /// <summary>
        /// TODO: description
        /// </summary>
        public long Limit;

        /// <summary>
        /// TODO: description
        /// </summary>
        public DateTime CreationTime;
    }

    public enum ContextFlags : uint
    {
        Control = 1,
        Integer = 2,
        FloatingPoint = 8,

        Full = (Control | Integer | FloatingPoint),
    }

    public class ThreadContext
    {
        public ContextFlags Flags;
        public uint Edi;
        public uint Esi;
        public uint Ebx;
        public uint Edx;
        public uint Ecx;
        public uint Eax;
        public uint Ebp;
        public uint Eip;
        public uint Esp;
        public uint EFlags;
        public uint SegCs;
        public uint SegSs;
    }
}

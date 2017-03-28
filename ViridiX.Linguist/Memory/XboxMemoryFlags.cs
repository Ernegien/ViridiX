using System;

namespace ViridiX.Linguist.Memory
{
    /// <summary>
    /// Xbox memory flags.
    /// </summary>
    [Flags]
    public enum XboxMemoryFlags
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        None = 0,

        /// <summary>
        /// TODO: description
        /// </summary>
        PageNoAccess = 1 << 0,  // 0x1

        /// <summary>
        /// TODO: description
        /// </summary>
        PageReadOnly = 1 << 1, // 0x2

        /// <summary>
        /// TODO: description
        /// </summary>
        PageReadWrite = 1 << 2, // 0x4

        /// <summary>
        /// TODO: description
        /// </summary>
        PageWriteCopy = 1 << 3, // 0x8

        /// <summary>
        /// TODO: description
        /// </summary>
        PageExecute = 1 << 4, // 0x10

        /// <summary>
        /// TODO: description
        /// </summary>
        PageExecuteRead = 1 << 5, // 0x20

        /// <summary>
        /// TODO: description
        /// </summary>
        PageExecuteReadWrite = 1 << 6, // 0x40

        /// <summary>
        /// TODO: description
        /// </summary>
        PageExecuteWriteCopy = 1 << 7, // 0x80

        /// <summary>
        /// TODO: description
        /// </summary>
        PageGuard = 1 << 8, // 0x100

        /// <summary>
        /// TODO: description
        /// </summary>
        PageNoCache = 1 << 9, // 0x200

        /// <summary>
        /// TODO: description
        /// </summary>
        PageWriteCombine = 1 << 10, // 0x400

        /// <summary>
        /// TODO: description
        /// </summary>
        MemCommit = 1 << 12, // 0x1000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemReserve = 1 << 13, // 0x2000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemDecommit = 1 << 14, // 0x4000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemRelease = 1 << 15, // 0x8000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemFree = 1 << 16, // 0x10000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemPrivate = 1 << 17, // 0x20000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemReset = 1 << 19, // 0x80000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemTopDown = 1 << 20, // 0x100000

        /// <summary>
        /// TODO: description
        /// </summary>
        MemNoZero = 1 << 23 // 0x800000
    }
}

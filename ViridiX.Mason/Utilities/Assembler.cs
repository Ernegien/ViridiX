using System.Linq;
using Binarysharp.Assemblers.Fasm;

namespace ViridiX.Mason.Utilities
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public static class Assembler
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public static byte[] Assemble(string[] mnemonics, long baseAddress = 0)
        {
            var oplist = mnemonics.ToList();
            oplist.Insert(0, "use32");
            oplist.Insert(1, $"org {baseAddress}");
            return FasmNet.Assemble(oplist.ToArray(), 0x10000, 10);
        }
    }
}

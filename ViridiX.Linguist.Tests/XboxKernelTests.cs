using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.Kernel;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxKernelTests
    {
        private Xbox _xbox;

        [TestInitialize]
        public void Initialize()
        {
            _xbox = new Xbox(AssemblyGlobals.Logger);
            _xbox.Connect(AssemblyGlobals.TestXbox.Ip);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _xbox?.Dispose();
            AssemblyGlobals.Logger.Level = LogLevel.Trace;
        }

        [TestMethod]
        public void GeneralInfoTest()
        {
            // check that the size isn't outrageously big or small
            int size = _xbox.Kernel.Size;
            Assert.IsTrue((size & 0xFF) == 0);
            Assert.IsTrue(size > 0x10000);
            Assert.IsTrue(size < 0x200000);

            // check that the date makes sense
            Assert.IsTrue(_xbox.Kernel.Date < DateTime.Now);
            Assert.IsTrue(_xbox.Kernel.Date > new DateTime(1999, 1, 1, 0, 0, 0));

            var version = _xbox.Kernel.Version;
        }

        [TestMethod]
        public void ExportTableTest()
        {
            Assert.IsTrue(_xbox.Kernel.ExportTable.Length > 200);
            Assert.IsTrue(_xbox.Kernel.ExportTable.Length < 385);

            foreach (var export in _xbox.Kernel.ExportTable)
            {
                if (export != 0)
                {
                    Assert.IsTrue(export >= XboxKernel.Address && export < XboxKernel.Address + _xbox.Kernel.Size);
                }
            }
        }

        [TestMethod]
        public void MemDumpTest()
        {
            // temporarly decrease the verbosity to prevent useless entries from getting logged
            AssemblyGlobals.Logger.Level = LogLevel.Info;

            string temp = Path.GetTempFileName();
            _xbox.Kernel.MemoryDump(temp);
            File.Delete(temp);
        }
    }
}

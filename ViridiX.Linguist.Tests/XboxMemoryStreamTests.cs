using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.FileSystem;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxMemoryStreamTests
    {
        private Xbox _xbox;

        [TestInitialize]
        public void Initialize()
        {
            _xbox = new Xbox(AssemblyGlobals.Logger);
            _xbox.Connect(AssemblyGlobals.TestXbox.Ip);
            _xbox.CommandSession.SendBufferSize = 1024;
            _xbox.CommandSession.ReceiveBufferSize = 1024;
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                // attempt to remove all temporary files
                var tempFiles = _xbox.FileSystem.GetDirectoryList(XboxFileSystem.TempDirectory);
                foreach (var file in tempFiles)
                {
                    _xbox.FileSystem.DeleteFile(file.FullName);
                }
                _xbox.FileSystem.DeleteDirectory(XboxFileSystem.TempDirectory);
            }
            catch (Exception e)
            {
                
            }

            _xbox?.Dispose();
            AssemblyGlobals.Logger.Level = LogLevel.Trace;
        }

        [TestMethod]
        public void ReadWriteTest()
        {
            Assert.AreEqual(_xbox.Memory.ReadString(0x10000, 4), "XBEH");

            long address = 0;
            const int allocationSize = 0x10000;
            byte[] data = new byte[allocationSize].FillRandom();

            try
            {
                address = _xbox.Process.Call(_xbox.Kernel.Exports.MmDbgAllocateMemory, allocationSize, 4);
                _xbox.Memory.Write(address, data);
                byte[] data2 = _xbox.Memory.ReadBytes(address, data.Length);
                Assert.IsTrue(data.IsEqual(data2));
            }
            finally
            {
                if (address > 0)
                {
                    _xbox.Process.Call(_xbox.Kernel.Exports.MmDbgFreeMemory, address, 0);
                }
            }
        }
    }
}

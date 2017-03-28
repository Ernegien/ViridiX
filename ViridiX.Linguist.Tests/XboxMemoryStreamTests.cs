using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        }

        [TestCleanup]
        public void Cleanup()
        {
            _xbox?.Dispose();
            AssemblyGlobals.Logger.Level = LogLevel.Trace;
        }

        [TestMethod]
        public void ReadTest()
        {
            Assert.AreEqual(_xbox.Memory.Stream.ReadString(0x10000, 4), "XBEH");

            // TODO: more
        }

    }
}

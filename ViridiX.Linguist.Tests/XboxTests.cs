using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxTests
    {
        private Xbox _xbox;

        [TestInitialize]
        public void Initialize()
        {
            _xbox = new Xbox(AssemblyGlobals.Logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _xbox?.Dispose();
            AssemblyGlobals.Logger.Level = LogLevel.Trace;
        }

        [TestMethod]
        public void DiscoveryTest()
        {
            var discovered = Xbox.Discover(10, AssemblyGlobals.Logger);
            if (discovered.Count == 0)
            {
                Assert.Inconclusive();
            }
        }

        [TestMethod]
        public void CtorTest()
        {
            for (int i = 0; i < 5; i++)
            {
                _xbox.Dispose();
                _xbox = new Xbox(AssemblyGlobals.Logger);
            }
        }

        [TestMethod]
        public void ConnectionTest()
        {
            _xbox.Connect(AssemblyGlobals.TestXbox.Ip);
            _xbox.Disconnect();

            // subsequent connections should reconnect
            for (int i = 0; i < 5; i++)
            {
                _xbox.Connect(AssemblyGlobals.TestXbox.Ip);
            }

            for (int i = 0; i < 5; i++)
            {
                _xbox.Reconnect();
            }
        }
    }
}

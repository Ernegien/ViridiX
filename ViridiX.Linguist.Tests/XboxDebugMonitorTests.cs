using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.Network;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxDebugMonitorTests
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
        public void InfoTest()
        {
            var data = _xbox.DebugMonitor.Data;
            var version = _xbox.DebugMonitor.Version;
        }
    }
}

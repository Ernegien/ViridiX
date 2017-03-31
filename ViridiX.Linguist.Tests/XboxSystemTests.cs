using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxSystemTests
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
        public void NameTest()
        {
            var originalName = _xbox.System.Name;
            var testName = DateTime.Now.Ticks.ToString();
            _xbox.System.Name = testName;
            Assert.AreEqual(_xbox.System.Name, testName);
            _xbox.System.Name = originalName;
            Assert.AreEqual(_xbox.System.Name, originalName);
        }

        [TestMethod]
        public void TimeTest()
        {
            var today = _xbox.System.Time;
            var yesterday = today.Subtract(TimeSpan.FromDays(1));
            _xbox.System.Time = yesterday;
            Assert.IsTrue(_xbox.System.Time - yesterday < TimeSpan.FromSeconds(3));
            _xbox.System.Time = DateTime.Now;   // syncs xbox time with computer time instead of preserving original, assumes this isn't an issue
            Assert.IsTrue(_xbox.System.Time - DateTime.Now < TimeSpan.FromSeconds(3));
        }

        [TestMethod]
        public void HardwareInfoTest()
        {
            var hardware = _xbox.System.HardwareInfo;
        }
    }
}

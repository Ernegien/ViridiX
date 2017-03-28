using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxMemoryTests
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
        public void ValidAddressTest()
        {
            Assert.IsTrue(_xbox.Memory.IsValidAddress(0x10000));
            Assert.IsFalse(_xbox.Memory.IsValidAddress(0xFFFFF000));
            Assert.IsFalse(_xbox.Memory.IsValidAddress(long.MaxValue));

            // TODO: more
        }

        [TestMethod]
        public void StatsTest()
        {
            var mmglobal = _xbox.Memory.Statistics;
            Assert.IsTrue(mmglobal.TotalPages == 0x4000 || mmglobal.TotalPages == 0x8000);
            Assert.IsTrue(mmglobal.AvailablePages < mmglobal.TotalPages);
        }

        [TestMethod]
        public void RegionTest()
        {
            var regions = _xbox.Memory.Regions;
            foreach (var region in regions)
            {
                // make sure everything is 4kb aligned
                Assert.IsTrue((region.Address & 0xFFF) == 0 && (region.Size & 0xFFF) == 0);
            }
        }
    }
}

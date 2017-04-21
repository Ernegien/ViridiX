using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.Process;
using ViridiX.Linguist.System;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxProcessTests
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
        public void ModuleTest()
        {
            var modules = _xbox.Process.Modules;
            Assert.IsNotNull(_xbox.Process.GetModule("xboxkrnl.exe"));
            Assert.IsNotNull(_xbox.Process.GetModule("xbdm.dll"));
            Assert.IsNull(_xbox.Process.GetModule("thisbetternotexist.dll"));
            // TODO: more
        }

        [TestMethod]
        public void ThreadTest()
        {
            var threads = _xbox.Process.Threads;
            // TODO: more
        }

        [TestMethod]
        public void CallTest()
        {
            var status = (XboxLinkStatus)(long)_xbox.Process.Call(_xbox.Kernel.Exports.PhyGetLinkState, 0);
            Assert.IsTrue(status.HasFlag(XboxLinkStatus.Active));
            Assert.IsTrue(status.HasFlag(XboxLinkStatus.FastEthernet));
            Assert.IsTrue(status.HasFlag(XboxLinkStatus.FullDuplex));

            try
            {
                _xbox.Process.Call(0, new XboxCallContext());
            }
            catch (NotImplementedException) { }
        }
    }
}

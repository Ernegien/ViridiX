using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.Network;
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

        [TestMethod]
        public void RceTest()
        {
            // BE 00 00 DB 82	; mov		esi, 82DB0000h (error)
            // BE 00 00 DB 02	; mov		esi, 02DB0000h (success)
            // 8B C6			; mov		eax, esi
            // C2 10 00 		; retn		10h

            var hookAddress = _xbox.DebugMonitor.HrFunctionCall;
            _xbox.Memory.WriteBytes(hookAddress, new byte[] { 0xBE, 0x00, 0x00, 0xDB, 0x02, 0x8B, 0xC6, 0xC2, 0x10, 0x00 });
            _xbox.CommandSession.SendCommandStrict("funccall thread=0");
            _xbox.Memory.WriteBytes(hookAddress, new byte[] { 0xBE, 0x00, 0x00, 0xDB, 0x82, 0x8B, 0xC6, 0xC2, 0x10, 0x00 });
            Assert.IsTrue(_xbox.CommandSession.SendCommand("funccall thread=0").Type == XboxCommandResponseType.UndefinedError);            
        }
    }
}

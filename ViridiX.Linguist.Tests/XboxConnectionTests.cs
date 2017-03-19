using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.Network;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxConnectionTests
    {
        private XboxConnection _connection;

        [TestInitialize]
        public void Initialize()
        {
            _connection = new XboxConnection(AssemblyGlobals.Logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection?.Dispose();
            AssemblyGlobals.Logger.Level = LogLevel.Trace;
        }

        [TestMethod]
        public void OpenTest()
        {
            _connection.Open(AssemblyGlobals.TestXbox.Ip);
        }

        [TestMethod]
        public void CommandPerformanceTest()
        {
            // temporarly decrease the verbosity to prevent useless entries from getting logged
            AssemblyGlobals.Logger.Level = LogLevel.Info;

            _connection.Open(AssemblyGlobals.TestXbox.Ip);

            int count = 0;
            Stopwatch timer = Stopwatch.StartNew();
            while (timer.Elapsed.TotalSeconds < 3)
            {
                _connection.SendCommand(string.Empty);
                count++;
            }
            int commandsPerSecond = (int) (count / (float) timer.Elapsed.TotalSeconds);

            AssemblyGlobals.Logger.Info("Measured {Count} commands per second.", commandsPerSecond);

            if (commandsPerSecond < 3000)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Ensures connections are being closed properly.
        /// </summary>
        [TestMethod]
        public void PortExhaustionTest()
        {
            // temporarly decrease the verbosity to prevent useless entries from getting logged
            AssemblyGlobals.Logger.Level = LogLevel.Warn;

            XboxConnectionOptions options = XboxConnectionOptions.PerformanceMode;

            for (int i = 0; i < 1000; i++)
            {
                using (_connection = new XboxConnection(AssemblyGlobals.Logger))
                {
                    // register notification session every other connection
                    _connection.Open(AssemblyGlobals.TestXbox.Ip, options ^= XboxConnectionOptions.NotificationSession);

                    // get active xbox connections
                    int connectionCount =
                        IPGlobalProperties.GetIPGlobalProperties()
                            .GetActiveTcpConnections()
                            .Count(connection => Equals(connection.RemoteEndPoint.Address, AssemblyGlobals.TestXbox.Ip));

                    // fail if we detect they aren't being closed properly
                    if (connectionCount > 10)
                    {
                        Assert.Fail();
                    }
                }
            }
        }
    }
}
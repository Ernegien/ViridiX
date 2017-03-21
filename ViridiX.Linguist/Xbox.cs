using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViridiX.Linguist.Network;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist
{
    /// <summary>
    /// Interfaces with a networked Xbox running debug software.
    /// </summary>
    public class Xbox : IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isDisposed;

        /// <summary>
        /// Session reserved for sending commands and receiving data.
        /// </summary>
        public XboxConnection CommandSession { get; private set; }

        /// <summary>
        /// Session reserved for receiving notifications.
        /// </summary>
        public XboxConnection NotificationSession { get; private set; }

        /// <summary>
        /// The maximum number of notifications to keep a history of.
        /// </summary>
        public int MaxNotificationHistoryCount { get; set; } = 100;

        /// <summary>
        /// Maintains a thread-safe history of Xbox notifications in the order they were received.
        /// </summary>
        public ConcurrentQueue<string> NotificationHistory { get; private set; } = new ConcurrentQueue<string>();

        /// <summary>
        /// Fires when an Xbox notification message has been received.
        /// </summary>
        public event EventHandler<NotificationEventArgs> NotificationReceived;

        /// <summary>
        /// Constructs the Xbox class.
        /// </summary>
        /// <param name="logger"></param>
        public Xbox(ILogger logger = null)
        {
            _logger = logger;

            // TODO: likely need a better way to handle this
            Task.Factory.StartNew(NotificationProcessingThread);
        }

        /// <summary>
        /// Destructs the Xbox class.
        /// </summary>
        ~Xbox()
        {
            Dispose();
        }

        /// <summary>
        /// Initializes access to various Xbox sub-systems.
        /// </summary>
        private void Initialize()
        {
            // TODO: memory, filesystem, audio/video etc.
        }

        /// <summary>
        /// Connects to an Xbox with the specified ip address. If already connected, it will reconnect.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="options"></param>
        public void Connect(IPAddress ip, XboxConnectionOptions options = XboxConnectionOptions.PerformanceMode)
        {
            Disconnect();
            CommandSession = new XboxConnection(_logger);
            CommandSession.Open(ip, options);
            NotificationSession = new XboxConnection(_logger);
            NotificationSession.Open(ip, XboxConnectionOptions.NotificationSession);
            Initialize();
        }

        /// <summary>
        /// Disconnects from the Xbox.
        /// </summary>
        public void Disconnect()
        {
            CommandSession?.Dispose();
            NotificationSession?.Dispose();
        }

        /// <summary>
        /// Reconnects to the Xbox.
        /// </summary>
        public void Reconnect()
        {
            if (CommandSession?.Ip == null)
                throw new Exception("No previous connection detected.");

            Disconnect();
            Connect(CommandSession.Ip, CommandSession.Options);
        }

        /// <summary>
        /// Disposes the Xbox.
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;

            Disconnect();

            NotificationHistory = null;
            NotificationReceived = null;

            // TODO: dispose sub-systems
        }

        /// <summary>
        /// Receives incoming notifications and informs any subscribers.
        /// </summary>
        private void NotificationProcessingThread()
        {
            while (!_isDisposed)
            {
                Thread.Sleep(1);

                // only bother if it's been fully converted
                if (NotificationSession == null || !NotificationSession.Options.HasFlag(XboxConnectionOptions.NotificationSession))
                    continue;
               
                // look for a new notification
                string notification = NotificationSession.TryReceiveLine();
                if (notification == null) continue;

                // save for later
                _logger?.Debug($"Notification received: {notification}");
                NotificationHistory.Enqueue(notification);

                // start dequeuing old entries if greater than max history count
                if (NotificationHistory.Count > MaxNotificationHistoryCount)
                {
                    string garbage;
                    NotificationHistory.TryDequeue(out garbage);
                }

                // inform any subscribers
                NotificationReceived?.Invoke(this, new NotificationEventArgs(notification));
            }
        }

        /// <summary>
        /// Attempts to discover debug Xboxes listening on the network.
        /// </summary>
        /// <param name="timeout">The time in milliseconds to wait for responses.</param>
        /// <param name="logger">The optional logger to use.</param>
        /// <returns></returns>
        public static List<XboxConnectionInformation> Discover(int timeout = 10, ILogger logger = null)
        {
            logger?.Info("Performing Xbox network discovery on port {Port}.", XboxConnection.Port);

            List<XboxConnectionInformation> connections = new List<XboxConnectionInformation>();

            // iterate through each network interface
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // only worry about active IPv4 interfaces
                if (nic.OperationalStatus != OperationalStatus.Up || !nic.Supports(NetworkInterfaceComponent.IPv4))
                    continue;

                // iterate through each ip address assigned to the interface
                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {
                    // don't bother broadcasting from IPv6 or loopback addresses
                    if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6 || IPAddress.IsLoopback(ip.Address))
                        continue;

                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    {
                        try
                        {
                            const short wildcardDiscoveryType = 3;

                            logger?.Trace("Broadcasting wildcard discovery packet from {IP} on interface {Name}",
                                $"{ip.Address}/{ip.PrefixLength}", nic.Name);

                            // broadcast wildcard discovery packet
                            socket.EnableBroadcast = true;
                            socket.Bind(new IPEndPoint(ip.Address, 0));
                            socket.SendTo(BitConverter.GetBytes(wildcardDiscoveryType), new IPEndPoint(IPAddress.Broadcast, XboxConnection.Port));

                            // listen for any responses
                            Stopwatch timer = Stopwatch.StartNew();
                            while (timer.ElapsedMilliseconds < timeout)
                            {
                                if (socket.Available > 0)
                                {
                                    // receive the response
                                    byte[] datagramBuffer = new byte[10240];
                                    EndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
                                    int bytesReceived = socket.ReceiveFrom(datagramBuffer, datagramBuffer.Length, SocketFlags.None, ref endpoint);

                                    // perform some simple sanity checks to be more certain it was an xbox device that has responded and not some freak chance of nature
                                    if (bytesReceived >= 2)
                                    {
                                        int nameLength = datagramBuffer[1];
                                        if (datagramBuffer[0] == 2 && nameLength + 2 == bytesReceived)
                                        {
                                            IPAddress xboxIp = ((IPEndPoint)endpoint).Address;
                                            string xboxName = Encoding.ASCII.GetString(datagramBuffer, 2, nameLength);
                                            var foundXbox = new XboxConnectionInformation(xboxIp, xboxName);

                                            // skip duplicates in the case that multiple ip addresses sharing the same subnet are assigned to an interface
                                            if (!connections.Contains(foundXbox))
                                            {
                                                logger?.Info("Discovered an xbox named {Name} at {Address}", foundXbox.Name, foundXbox.Ip);
                                                connections.Add(foundXbox);
                                            }
                                        }
                                    }

                                    // reset the timer and keep listening for any additional responses
                                    timer = Stopwatch.StartNew();
                                }

                                Thread.Sleep(1);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger?.Warn(ex, "An error has occurred during Xbox network discovery.");
                        }
                    }
                }
            }

            return connections;
        }
    }
}
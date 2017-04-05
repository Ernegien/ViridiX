using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ViridiX.Mason.Logging;
using ViridiX.Mason.Network;

namespace ViridiX.Linguist.Network
{
    /// <summary>
    /// Provides connectivity to an Xbox over a network.
    /// </summary>
    public sealed class XboxConnection : TcpClient
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isDisposed;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int MaxJitterThreshold = 1000;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _jitterThreshold;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int MinJitterThreshold = 1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string NewLine = "\r\n";

        #endregion

        #region Properties

        /// <summary>
        /// The Xbox ip address.
        /// </summary>
        public IPAddress Ip { get; private set; }

        /// <summary>
        /// The Xbox port.
        /// </summary>
        public const int Port = 731;

        /// <summary>
        /// The options to use when opening a connection.
        /// </summary>
        public XboxConnectionOptions Options { get; private set; }

        /// <summary>
        /// The amount of jitter in milliseconds to account for while receiving data.
        /// If this value is too small, the link could be identified as being idle while still receiving data.
        /// If the value is too large, efficiency suffers while waiting for an idle link.
        /// </summary>
        public int JitterThreshold
        {
            get { return _jitterThreshold; }
            set { _jitterThreshold = Math.Max(Math.Min(value, MaxJitterThreshold), MinJitterThreshold); }
        }

        /// <summary>
        /// Blocking NetworkStream wrapper.
        /// </summary>
        public BlockingNetworkStream Stream { get; private set; }

        /// <summary>
        /// The underlying TCPClient binary reader.
        /// </summary>
        public BinaryReader Reader { get; private set; }

        /// <summary>
        /// The underlying TCPClient binary writer.
        /// </summary>
        public BinaryWriter Writer { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes an Xbox connection.
        /// </summary>
        public XboxConnection(ILogger logger = null)
        {
            _logger = logger;
            NoDelay = true;
            JitterThreshold = 3;
            ReceiveTimeout = 100;
            SendTimeout = 100;
            SendBufferSize = 1024 * 1024 * 10;
            ReceiveBufferSize = 1024 * 1024 * 10;
        }

        #endregion

        #region Destruction

        /// <summary>
        /// TODO: description
        /// </summary>
        ~XboxConnection()
        {
            Dispose(false);
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_isDisposed) return;

                _logger?.Info("Disconnecting");

                if (!Options.HasFlag(XboxConnectionOptions.NotificationSession))
                {
                    // avoid port exhaustion by attempting to gracefully inform the xbox we're leaving
                    SendCommandText("bye");
                }

                if (disposing)
                {
                    Reader?.Dispose();
                    Writer?.Dispose();
                    Stream?.Dispose();
                }

                // dispose any unmanaged resources
            }
            finally
            {
                _isDisposed = true;
                base.Dispose(disposing);
            }
        }

        #endregion

        #region Connection

        // TODO: use Ping to determine RTT to better adjust the default timeout values
        /// <summary>
        /// Attempts to connect to an Xbox with the specified IP address.
        /// </summary>
        /// <param name="ip">The Xbox IP address.</param>
        /// <param name="options">The options to use.</param>
        public void Open(IPAddress ip, XboxConnectionOptions options = XboxConnectionOptions.PerformanceMode)
        {
            if (_isDisposed)
            {
                throw new Exception("An XboxConnection cannot be reused after disposal.");
            }

            if (Ip != null)
            {
                throw new Exception("An XboxConnection can only be opened once.");
            }

            _logger?.Info("Establishing {Type} session with {IP}", options.HasFlag(XboxConnectionOptions.NotificationSession) ? "notification" : "command", ip);

            if (!ConnectAsync(ip, Port).Wait(ReceiveTimeout))
            {
                throw new Exception("Failed to connect within the specified timeout period.");
            }

            Stream = new BlockingNetworkStream(GetStream());
            Reader = new BinaryReader(Stream);
            Writer = new BinaryWriter(Stream);

            var response = ReceiveStatusResponse();
            if (!response.Success)
            {
                throw new Exception(response.Full);
            }

            Ip = ip;

            // convert to a notification session if desired
            if (options.HasFlag(XboxConnectionOptions.NotificationSession))
            {
                SendCommandText("notify");

                // wait a bit extra to give the Xbox enough time
                if (ReceiveStatusResponse(ReceiveTimeout + 50).Type != XboxCommandResponseType.NowNotifySession)
                {
                    throw new Exception("Failed to open notification session.");
                }
            }

            // update the options only after all prerequisite initialization has been performed (notification session)
            Options = options;
        }

        #endregion

        #region Receive Data

        /// <summary>
        /// Peeks into the stream to determine if a new line exists and returns the text size (including return carriage and new line characters). Returns -1 if no full line exists.
        /// </summary>
        /// <returns></returns>
        public int GetAvailableLineSize()
        {
            // no new lines exist
            if (Available < NewLine.Length) return -1;

            // peek into the receive buffer for a new line
            byte[] textBuffer = new byte[1024]; // large enough to safely contain a single line of text
            int peekSize = Math.Min(Available, textBuffer.Length);
            Client.Receive(textBuffer, peekSize, SocketFlags.Peek);
            int newLineIndex = Encoding.ASCII.GetString(textBuffer, 0, peekSize).IndexOf(NewLine, StringComparison.Ordinal);

            // return the line size if found, otherwise -1
            return newLineIndex > -1 ? newLineIndex + NewLine.Length : -1;
        }

        /// <summary>
        /// Waits for a single line of text to be available before returning it.
        /// </summary>
        /// <returns></returns>
        public string ReceiveLine()
        {
            Stopwatch timer = Stopwatch.StartNew();
            while (timer.ElapsedMilliseconds < ReceiveTimeout)
            {
                string line = TryReceiveLine();
                if (line != null)
                {
                    return line;
                }

                Sleep();
            }

            throw new TimeoutException();
        }

        /// <summary>
        /// Receives a single line of text if immediately available, otherwise returns null.
        /// </summary>
        /// <returns></returns>
        public string TryReceiveLine()
        {
            try
            {
                int lineSize = GetAvailableLineSize();

                // return null if no full line available
                if (lineSize <= -1) return null;

                // otherwise return the line
                byte[] textBuffer = new byte[lineSize];
                Client.Receive(textBuffer, 0, lineSize, SocketFlags.None);
                string line = Encoding.ASCII.GetString(textBuffer, 0, lineSize - NewLine.Length);
                _logger?.Trace("Line Received: {0}", line);
                return line;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Receives multiple lines of text returned from an XboxConnection.
        /// </summary>
        /// <returns></returns>
        public List<string> ReceiveLines()
        {
            List<string> lines = new List<string>();

            string line;
            while ((line = ReceiveLine()) != ".")
            {
                lines.Add(line);
            }

            return lines;
        }

        /// <summary>
        /// Receives multiple lines of text returned from an XboxConnection as a single string.
        /// </summary>
        /// <returns></returns>
        public string ReceiveText()
        {
            StringBuilder text = new StringBuilder();

            foreach (string line in ReceiveLines())
            {
                text.AppendLine(line);
            }

            return text.ToString();
        }

        /// <summary>
        /// Flushes the receive buffer immediately without waiting for the link to become idle.
        /// </summary>
        public void FlushAvailable()
        {
            Reader.ReadBytes(Available);
        }

        /// <summary>
        /// Waits for the specified amount of data to become available in the receive buffer before flushing it.
        /// </summary>
        /// <param name="size"></param>
        public void Flush(int size)
        {
            Reader.ReadBytes(size);
        }

        /// <summary>
        /// Flushes the receive buffer while waiting for the link to become idle.
        /// </summary>
        public void FlushAll()
        {
            while (!IsReceiveIdle())
            {
                FlushAvailable();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Sends a command to the xbox without waiting for a response.
        /// </summary>
        /// <param name="command">Command to be sent</param>
        /// <param name="args">Arguments</param>
        public void SendCommandText(string command, params object[] args)
        {
            string commandText = string.Format(command, args);
            _logger?.Trace("Sending command: {Command}", commandText);
            Writer?.Write(Encoding.ASCII.GetBytes(commandText + NewLine));
        }

        /// <summary>
        /// Sends a command to the xbox and waits until it receives a status response.
        /// </summary>
        /// <param name="command">Command to be sent</param>
        /// <param name="args">Arguments</param>
        /// <returns>Status response</returns>
        public XboxCommandResponse SendCommand(string command, params object[] args)
        {
            if (Options.HasFlag(XboxConnectionOptions.ProtectedMode))
            {
                FlushAll();
            }
            else
            {
                FlushAvailable();
            }

            SendCommandText(command, args);
            return ReceiveStatusResponse();
        }

        /// <summary>
        /// Sends a command to the xbox and waits until it receives a status response. An error response is rethrown as an exception.
        /// </summary>
        /// <param name="command">The command to be sent.</param>
        /// <param name="args">The formatted command arguments.</param>
        /// <returns>The status response.</returns>
        public XboxCommandResponse SendCommandStrict(string command, params object[] args)
        {
            XboxCommandResponse response = SendCommand(command, args);
            if (response.Success) return response;
            throw new Exception(response.Full);
        }

        /// <summary>
        /// Receives a command for a status response to be received from the xbox.
        /// </summary>
        /// <param name="timeout">The optional receive timeout in milliseconds, overriding TcpClient.ReceiveTimeout.</param>
        /// <returns></returns>
        public XboxCommandResponse ReceiveStatusResponse(int? timeout = null)
        {
            int origTimeout = ReceiveTimeout;
            if (timeout != null)
            {
                ReceiveTimeout = timeout.Value;
            }

            try
            {
                string response = ReceiveLine();
                return new XboxCommandResponse(response, (XboxCommandResponseType)Convert.ToInt32(response.Remove(3)), response.Remove(0, 5));
            }
            finally
            {
                ReceiveTimeout = origTimeout;
            }
        }

        #endregion

        #region Waits

        /// <summary>
        /// Performs Sleep(milliseconds) unless HighPerformanceSleep is specified, in which case it does nothing.
        /// </summary>
        public void Sleep(int milliseconds = 1)
        {
            if (!Options.HasFlag(XboxConnectionOptions.PerformanceMode))
            {
                Thread.Sleep(milliseconds);
            }
        }

        /// <summary>
        /// Waits for the specified amount of data to become available in the receive buffer.
        /// </summary>
        /// <param name="available"></param>
        public void WaitForAvailable(int available = 1)
        {
            int previousAvailable = Available;
            Stopwatch timeoutClock = Stopwatch.StartNew();
            
            while (Available < available)
            {
                Sleep();

                // reset the timeout clock if data is actively being received
                if (Available > previousAvailable)
                {
                    previousAvailable = Available;
                    timeoutClock = Stopwatch.StartNew();
                }

                if (timeoutClock.ElapsedMilliseconds > ReceiveTimeout)
                {
                    throw new TimeoutException();
                }
            }
        }

        /// <summary>
        /// Waits for data to stop being received.
        /// </summary>
        public void WaitForIdle()
        {
            while (!IsReceiveIdle())
            {
                // waits for an indefinite amount of time
            }
        }

        /// <summary>
        /// Detects if the connection receive stream is idle. Returns false if data is actively being received or true if no data was received throughout the jitter threshold window.
        /// </summary>
        /// <returns></returns>
        public bool IsReceiveIdle()
        {
            int originalAvailable = Available;

            Stopwatch timer = Stopwatch.StartNew();
            while (timer.ElapsedMilliseconds < JitterThreshold)
            {
                Sleep();
                if (Available > originalAvailable)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
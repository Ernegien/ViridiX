using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using ViridiX.Mason.Logging;

namespace ViridiX.Mason.Network
{
    /// <summary>
    /// Wraps a network stream with timeout-capable blocking read access.
    /// </summary>
    public class BlockingNetworkStream : Stream
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly NetworkStream _stream;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        #endregion

        #region Properties

        /// <summary>
        /// TODO: description
        /// </summary>
        public override bool CanRead => _stream.CanRead;

        /// <summary>
        /// TODO: description
        /// </summary>
        public override bool CanSeek => _stream.CanSeek;

        /// <summary>
        /// TODO: description
        /// </summary>
        public override bool CanWrite => _stream.CanWrite;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the BlockingNetworkStream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="logger"></param>
        public BlockingNetworkStream(NetworkStream stream, ILogger logger = null)
        {
            _stream = stream;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            Stopwatch timer = Stopwatch.StartNew();

            int bytesReceived = 0;
            while (bytesReceived < count)
            {
                // read any data available in the receive buffer
                int bytesRead = _stream.Read(buffer, offset + bytesReceived, count - bytesReceived);
                bytesReceived += bytesRead;

                _logger?.Trace("Read {Count} bytes from buffer {Buffer} at offset {Offset}", bytesRead, buffer.GetHashCode(), offset);

                // reset the timeout if still actively receiving data
                if (bytesRead > 0)
                {
                    timer = Stopwatch.StartNew();
                }

                // check for timeouts
                if (timer.ElapsedMilliseconds > _stream.ReadTimeout)
                {
                    throw new TimeoutException();
                }
            }

            return bytesReceived;
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _logger?.Trace("Writing {Count} bytes from buffer {Buffer} at offset {Offset}", count, buffer.GetHashCode(), offset);
            _stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Flushes the underlying NetworkStream.
        /// </summary>
        public override void Flush()
        {
            _logger?.Trace("Flushing the stream");
            _stream.Flush();
        }

        #endregion

        #region Unsupported

        /// <summary>
        /// Not supported.
        /// </summary>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

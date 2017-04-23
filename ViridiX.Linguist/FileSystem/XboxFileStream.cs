using System;
using System.Diagnostics;
using System.IO;

namespace ViridiX.Linguist.FileSystem
{
    /// <summary>
    /// Provides streaming access to Xbox file data over a network.
    /// </summary>
    public class XboxFileStream : Stream
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        #endregion

        #region Properties

        /// <summary>
        /// The file name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the stream supports seeking.
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// Gets a value indicating whether the stream supports seeking. Always returns true.
        /// </summary>
        public override bool CanSeek => true;

        /// <summary>
        /// Gets a value indicating whether the stream supports writing.
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// Gets the file size.
        /// </summary>
        public override long Length => _xbox.FileSystem.GetFileSize(Name);

        /// <summary>
        /// Gets the current stream position.
        /// </summary>
        public override long Position { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Establishes a stream to the specified file name on an Xbox using the desired <see cref="FileMode"/>.
        /// </summary>
        /// <param name="xbox"></param>
        /// <param name="fileName"></param>
        /// <param name="mode"></param>
        public XboxFileStream(Xbox xbox, string fileName, XboxFileMode mode)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (xbox.DebugMonitor.Version.Build < 4531)
                throw new NotSupportedException("Requires xbdm.dll version 4531 or higher");

            _xbox = xbox;
            Name = fileName;
            xbox.FileSystem.CreateFile(fileName, mode);

            _xbox.Logger?.Trace("Creating stream access to {0}", fileName);
        }

        /// <summary>
        /// Establishes a stream to the specified file name on an Xbox using the default <see cref="XboxFileMode.Create"/> option.
        /// </summary>
        /// <param name="xbox"></param>
        /// <param name="fileName"></param>
        public XboxFileStream(Xbox xbox, string fileName) : this(xbox, fileName, XboxFileMode.Create)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Flushes the file stream buffer. This has no affect since it's still a reserved operation within the NetworkStream class.
        /// </summary>
        public override void Flush()
        {
            _xbox.CommandSession.Stream.Flush();
        }

        /// <summary>
        /// Seeks to the specified stream offset starting from the specified origin.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: return Position = (uint)offset;
                case SeekOrigin.Current: return Position += (uint)offset;
                case SeekOrigin.End: return Position = (uint)Length - (uint)offset;
                default: throw new Exception("Invalid SeekOrigin.");
            }
        }

        /// <summary>
        /// Sets the file length to the specified value.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            _xbox.FileSystem.SetFileSize(Name, (int)value);
        }

        /// <summary>
        /// Reads file data from the stream into the specified buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _xbox.FileSystem.ReadFilePartial(Name, Position, buffer, offset, count); ;
        }

        /// <summary>
        /// Writes data to the stream from the specified buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _xbox.FileSystem.WriteFilePartial(Name, Position, buffer, offset, count);
        }

        #endregion
    }
}

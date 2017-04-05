using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ViridiX.Linguist.Network;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.FileSystem
{
    /// <summary>
    /// The Xbox file subsystem.
    /// </summary>
    public class XboxFileSystem
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        /// <summary>
        /// Account for the occasional hard drive slowness by increasing this value.
        /// </summary>
        public int ReceiveTimeoutDelay { get; set; } = 1000;

        /// <summary>
        /// Initializes the Xbox file subsystem.
        /// </summary>
        /// <param name="xbox"></param>
        public XboxFileSystem(Xbox xbox)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = xbox.Logger;

            _logger?.Info("XboxFileSystem subsystem initialized");
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="fileName">The Xbox file path.</param>
        /// <param name="offset">The Xbox file offset to start reading data.</param>
        /// <param name="buffer">The buffer to receive data.</param>
        /// <param name="bufferOffset">The buffer offset to start receiving data at.</param>
        /// <param name="count">The amount of data to receive.</param>
        /// <returns>The data available.</returns>
        public int ReadFilePartial(string fileName, long offset, byte[] buffer, int bufferOffset, int count)
        {
            _logger?.Trace("Partial file read for {0}", fileName);

            if (_xbox.DebugMonitor.Version.Build < 4531)
            {
                throw new NotSupportedException("Requires xbdm.dll version 4531 or higher");
            }

            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                _xbox.CommandSession.SendCommandStrict("getfile name=\"{0}\" offset={1} size={2}", fileName, offset, count);
                int bytesToRead = _xbox.CommandSession.Reader.ReadInt32();
                _xbox.CommandSession.Reader.Read(buffer, bufferOffset, bytesToRead);

                return bytesToRead;
            }
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] ReadFilePartial(string fileName, long offset, int count)
        {
            byte[] data = new byte[count];

            if (ReadFilePartial(fileName, offset, data, 0, count) != count)
                throw new IOException("The amount of data returned is less than that requested.");

            return data;
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileOffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferOffset"></param>
        /// <param name="count"></param>
        public void WriteFilePartial(string fileName, long fileOffset, byte[] buffer, int bufferOffset, int count)
        {
            _logger?.Trace("Partial file write for {0}", fileName);

            if (_xbox.DebugMonitor.Version.Build < 4531)
            {
                throw new NotSupportedException("Requires xbdm.dll version 4531 or higher");
            }

            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                _xbox.CommandSession.SendCommandStrict("writefile name=\"{0}\" offset={1} length={2}", fileName, fileOffset, count);
                _xbox.CommandSession.Writer.Write(buffer, bufferOffset, count);
                if (!_xbox.CommandSession.ReceiveStatusResponse().Success)
                    throw new IOException();
            }
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileOffset"></param>
        /// <param name="data"></param>
        public void WriteFilePartial(string fileName, long fileOffset, byte[] data)
        {
            WriteFilePartial(fileName, fileOffset, data, 0, data.Length);
        }

        /// <summary>
        /// Returns a list of mapped drive letters.
        /// </summary>
        public List<string> DriveLetters
        {
            get
            {
                using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
                {
                    List<string> drives = _xbox.CommandSession.SendCommandStrict("drivelist").Message.Select(p => p + @":\").ToList();
                    drives.Sort();
                    return drives;
                }
            }
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<XboxFileInformation> GetDirectoryList(string path)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                List<XboxFileInformation> list = new List<XboxFileInformation>();

                _xbox.CommandSession.SendCommandStrict("dirlist name=\"{0}\"", path);
                foreach (string file in _xbox.CommandSession.ReceiveLines())
                {
                    Dictionary<string, object> fileInfo = file.ParseXboxResponseLine();

                    XboxFileInformation info = new XboxFileInformation();
                    info.FullName = Path.Combine(path, (string)fileInfo["name"]);
                    info.Size = ((long)fileInfo["sizehi"] << 32) | (long)fileInfo["sizelo"];
                    info.CreationTime = DateTime.FromFileTimeUtc(((long)fileInfo["createhi"] << 32) | (long)fileInfo["createlo"]);
                    info.ChangeTime = DateTime.FromFileTimeUtc(((long)fileInfo["changehi"] << 32) | (long)fileInfo["changelo"]);
                    info.Attributes |= file.Contains("directory") ? FileAttributes.Directory : FileAttributes.Normal;
                    info.Attributes |= file.Contains("readonly") ? FileAttributes.ReadOnly : 0;
                    info.Attributes |= file.Contains("hidden") ? FileAttributes.Hidden : 0;
                    list.Add(info);
                }

                return list;
            }
        }

        /// <summary>
        /// Creates a directory on the Xbox. Does nothing if the directory already exists.
        /// </summary>
        /// <param name="path"></param>
        public void CreateDirectory(string path)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                XboxCommandResponse response = _xbox.CommandSession.SendCommand("mkdir name=\"{0}\"", path);
                if (!response.Success && response.Type != XboxCommandResponseType.FileAlreadyExists)
                {
                    throw new IOException(response.Message);
                }
            }
        }

        /// <summary>
        /// Deletes a directory on the Xbox. Does nothing if the directory doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        public void DeleteDirectory(string path)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                XboxCommandResponse response = _xbox.CommandSession.SendCommand("delete name=\"{0}\" dir", path);
                if (!response.Success && response.Type != XboxCommandResponseType.FileNotFound)
                {
                    throw new IOException(response.Message);
                }
            }
        }

        /// <summary>
        /// Determines if the given file or directory exists.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool FileExists(string fileName)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                XboxCommandResponse response = _xbox.CommandSession.SendCommand("getfileattributes name=\"{0}\"", fileName);
                if (response.Type == XboxCommandResponseType.MultiResponse)
                {
                    _xbox.CommandSession.ReceiveLines();
                }
                else if (response.Type != XboxCommandResponseType.FileNotFound)
                {
                    throw new IOException(response.Message);
                }
                return response.Success;
            }
        }

        /// <summary>
        /// Dont use this, higher-level methods are available. Use GetDriveFreeSpace or GetDriveSize instead.
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="freeBytes"></param>
        /// <param name="driveSize"></param>
        /// <param name="totalFreeBytes"></param>
        private void GetDriveSizeInformation(string drive, out long freeBytes, out long driveSize, out long totalFreeBytes)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                _xbox.CommandSession.SendCommandStrict("drivefreespace name=\"{0}\"", drive + ":\\");

                string msg = _xbox.CommandSession.ReceiveText();
                var driveInfo = msg.ParseXboxResponseLine();

                freeBytes = ((long) driveInfo["freetocallerhi"] << 32) | (long) driveInfo["freetocallerlo"];
                driveSize = ((long) driveInfo["totalbyteshi"] << 32) | (long) driveInfo["totalbyteslo"];
                totalFreeBytes = ((long) driveInfo["totalfreebyteshi"] << 32) | (long) driveInfo["totalfreebyteslo"];
            }
        }

        /// <summary>
        /// Retrieves Xbox drive free space.
        /// </summary>
        /// <param name="driveLabel"></param>
        /// <returns></returns>
        public long GetDriveFreeSpace(string driveLabel)
        {
            long freeBytes, driveSize, totalFreeBytes;
            GetDriveSizeInformation(driveLabel, out freeBytes, out driveSize, out totalFreeBytes);
            return freeBytes;
        }

        /// <summary>
        /// Retrieves Xbox drive size.
        /// </summary>
        /// <param name="driveLabel"></param>
        /// <returns></returns>
        public long GetDriveSize(string driveLabel)
        {
            long freeBytes, driveSize, totalFreeBytes;
            GetDriveSizeInformation(driveLabel, out freeBytes, out driveSize, out totalFreeBytes);
            return driveSize;
        }
        /// <summary>
        /// Renames or moves a file on the Xbox.
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        public void RenameFile(string oldFileName, string newFileName)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                _xbox.CommandSession.SendCommandStrict("rename name=\"{0}\" newname=\"{1}\"", oldFileName, newFileName);
            }
        }

        /// <summary>
        /// Creates a file on the xbox.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="createDisposition">Creation options.</param>
        public void CreateFile(string fileName, XboxFileMode createDisposition)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                switch (createDisposition)
                {
                    case XboxFileMode.Open:
                        if (!FileExists(fileName)) throw new FileNotFoundException("File does not exist.");
                        break;
                    case XboxFileMode.Create:
                        _xbox.CommandSession.SendCommandStrict("fileeof name=\"" + fileName + "\" size=0 cancreate");
                        break;
                    case XboxFileMode.CreateNew:
                        _xbox.CommandSession.SendCommandStrict("fileeof name=\"" + fileName + "\" size=0 mustcreate");
                        break;
                    default:
                        throw new NotSupportedException("Unsupported FileMode.");
                }
            }
        }

        /// <summary>
        /// Deletes a file on the Xbox. Does nothing if the file doesn't exist.
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteFile(string fileName)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                XboxCommandResponse response = _xbox.CommandSession.SendCommand("delete name=\"{0}\"", fileName);
                if (!response.Success && response.Type != XboxCommandResponseType.FileNotFound)
                {
                    throw new IOException(response.Message);
                }
            }
        }

        /// <summary>
        /// Sets the size of a specified file on the Xbox. This method will not zero out any extra bytes that may have been created.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="size"></param>
        public void SetFileSize(string fileName, int size)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                _xbox.CommandSession.SendCommandStrict("fileeof name=\"{0}\" size={1}", fileName, size);
            }
        }

        /// <summary>
        /// Retrieves file information.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public XboxFileInformation GetFileInformation(string fileName)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                XboxCommandResponse response = _xbox.CommandSession.SendCommandStrict("getfileattributes name=\"{0}\"", fileName);
                if (response.Type != XboxCommandResponseType.MultiResponse)
                {
                    throw new IOException("Invalid file name or file does not exist.");
                }

                string infoText = _xbox.CommandSession.ReceiveLine();
                Dictionary<string, object> infoValues = infoText.ParseXboxResponseLine();

                XboxFileInformation info = new XboxFileInformation();
                info.FullName = fileName;
                info.Size = ((long)infoValues["sizehi"] << 32) | (long)infoValues["sizelo"];
                info.CreationTime = DateTime.FromFileTimeUtc(((long)infoValues["createhi"] << 32) | (long)infoValues["createlo"]);
                info.ChangeTime = DateTime.FromFileTimeUtc(((long)infoValues["changehi"] << 32) | (long)infoValues["changelo"]);
                info.Attributes |= infoText.Contains("directory") ? FileAttributes.Directory : FileAttributes.Normal;
                info.Attributes |= infoText.Contains("readonly") ? FileAttributes.ReadOnly : 0;
                info.Attributes |= infoText.Contains("hidden") ? FileAttributes.Hidden : 0;

                return info;
            }
        }

        /// <summary>
        /// Modifies file creation information. If you wish to specify a new file size, use SetFileSize instead.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="info"></param>
        public void SetFileInformation(string fileName, XboxFileInformation info)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                _xbox.CommandSession.SendCommandStrict(
                    "setfileattributes name=\"{0}\" createhi={1} createlo={2} changehi={3} changelo={4} readonly={5} hidden={6}",
                    fileName, (uint) (info.CreationTime.ToFileTime() >> 32),
                    (uint) (info.CreationTime.ToFileTime() & uint.MaxValue),
                    (uint) (info.ChangeTime.ToFileTime() >> 32),
                    (uint) (info.ChangeTime.ToFileTime() & uint.MaxValue),
                    info.Attributes.HasFlag(FileAttributes.ReadOnly),
                    info.Attributes.HasFlag(FileAttributes.Hidden));
            }
        }

        /// <summary>
        /// Retrieves the file attributes.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileAttributes GetFileAttributes(string fileName)
        {
            return GetFileInformation(fileName).Attributes;
        }

        /// <summary>
        /// Sets the file attributes.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="attributes"></param>
        public void SetFileAttributes(string fileName, FileAttributes attributes)
        {
            using (_xbox.CommandSession.ExtendReceiveTimeout(ReceiveTimeoutDelay))
            {
                _xbox.CommandSession.SendCommand("setfileattributes name=\"{0}\" readonly={1} hidden={2}",
                    fileName, attributes.HasFlag(FileAttributes.ReadOnly), attributes.HasFlag(FileAttributes.Hidden));
            }
        }

        /// <summary>
        /// Retrieves the file size.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public long GetFileSize(string fileName)
        {
            return GetFileInformation(fileName).Size;
        }
    }
}

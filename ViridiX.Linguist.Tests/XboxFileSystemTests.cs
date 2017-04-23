using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.FileSystem;
using ViridiX.Mason.Extensions;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxFileSystemTests
    {
        private Xbox _xbox;

        [TestInitialize]
        public void Initialize()
        {
            _xbox = new Xbox(AssemblyGlobals.Logger);
            _xbox.Connect(AssemblyGlobals.TestXbox.Ip);
            _xbox.CommandSession.SendBufferSize = 1024;
            _xbox.CommandSession.ReceiveBufferSize = 1024;
            _xbox.FileSystem.CreateDirectory(XboxFileSystem.TempDirectory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _xbox.FileSystem.DeleteDirectory(XboxFileSystem.TempDirectory, true);
            _xbox?.Dispose();
            AssemblyGlobals.Logger.Level = LogLevel.Trace;
        }

        [TestMethod]
        public void GetDrives()
        {
            var drives = _xbox.FileSystem.DriveLetters;
            Assert.IsNotNull(drives);
            Assert.IsTrue(drives.Contains(@"E:\"));
            Assert.IsTrue(drives.Contains(@"S:\"));
            Assert.IsTrue(drives.Contains(@"T:\"));
            Assert.IsTrue(drives.Contains(@"U:\"));
        }

        [TestMethod]
        public void GetDirectories()
        {
            var directories = _xbox.FileSystem.GetDirectoryList(@"E:\");
            Assert.IsNotNull(directories);
            Assert.IsTrue(directories.Any(dir => dir.Name.Equals("xbdm.ini", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void FileExistsTest()
        {
            // file test
            Assert.IsTrue(_xbox.FileSystem.FileExists(@"E:\xbdm.ini"));

            // directory test
            Assert.IsTrue(_xbox.FileSystem.FileExists(@"E:\dxt"));

            // misc. things that shouldn't exist
            Assert.IsFalse(_xbox.FileSystem.FileExists(null));
            Assert.IsFalse(_xbox.FileSystem.FileExists(string.Empty));
            Assert.IsFalse(_xbox.FileSystem.FileExists(@"ABC:\test."));
            Assert.IsFalse(_xbox.FileSystem.FileExists($@"Y:\{DateTime.Now.Ticks}"));
        }

        [TestMethod]
        public void CreateDeleteDirectoryTest()
        {
            // test double creation
            _xbox.FileSystem.CreateDirectory(XboxFileSystem.TempDirectory);
             
            // test existence and deletion
            Assert.IsTrue(_xbox.FileSystem.FileExists(XboxFileSystem.TempDirectory));
            _xbox.FileSystem.DeleteDirectory(XboxFileSystem.TempDirectory);
            Assert.IsFalse(_xbox.FileSystem.FileExists(XboxFileSystem.TempDirectory));

            // test double deletion
            _xbox.FileSystem.DeleteDirectory(XboxFileSystem.TempDirectory);
        }

        [TestMethod]
        public void CreateDeleteFileTest()
        {
            string testFile = Path.Combine(XboxFileSystem.TempDirectory, $"{DateTime.Now.Ticks}.tmp");

            // test successful file creation
            _xbox.FileSystem.CreateFile(testFile, XboxFileMode.CreateNew);
            Assert.IsTrue(_xbox.FileSystem.FileExists(testFile));

            // test Create overwrite success
            _xbox.FileSystem.CreateFile(testFile, XboxFileMode.Create);
            Assert.IsTrue(_xbox.FileSystem.FileExists(testFile));

            // test Create overwrite success
            _xbox.FileSystem.CreateFile(testFile, XboxFileMode.Open);
            Assert.IsTrue(_xbox.FileSystem.FileExists(testFile));

            // test successful file deletion
            _xbox.FileSystem.DeleteFile(testFile);
            Assert.IsFalse(_xbox.FileSystem.FileExists(testFile));
            _xbox.FileSystem.DeleteFile(testFile);  // test double deletion

            //test unsuccessful file creation
            try
            {
                _xbox.FileSystem.CreateFile(null, XboxFileMode.CreateNew);
                Assert.Fail();
            }
            catch { /* pass */ }
            try
            {
                _xbox.FileSystem.CreateFile(string.Empty, XboxFileMode.CreateNew);
                Assert.Fail();
            }
            catch { /* pass */ }
            try
            {
                _xbox.FileSystem.CreateFile(@"", XboxFileMode.CreateNew);
                Assert.Fail();
            }
            catch { /* pass */ }
            try
            {
                // test CreateNew failure (file already exists)
                _xbox.FileSystem.CreateFile(testFile, XboxFileMode.CreateNew);
                Assert.Fail();
            }
            catch { /* pass */ }
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            string testFile = Path.Combine(XboxFileSystem.TempDirectory, $"{DateTime.Now.Ticks}.tmp");
            _xbox.FileSystem.CreateFile(testFile, XboxFileMode.CreateNew);

            try
            {
                _xbox.FileSystem.DeleteDirectory(XboxFileSystem.TempDirectory);
                Assert.Fail();  // directory not empty
            }
            catch { /* pass */ }

            string testDirectory = Path.Combine(XboxFileSystem.TempDirectory, DateTime.Now.Ticks.ToString());
            _xbox.FileSystem.CreateDirectory(testDirectory);
            testFile = Path.Combine(testDirectory, $"{DateTime.Now.Ticks}.tmp");
            _xbox.FileSystem.CreateFile(testFile, XboxFileMode.CreateNew);
            _xbox.FileSystem.DeleteDirectory(XboxFileSystem.TempDirectory, true);
        }

        [TestMethod]
        public void GetFileInformationTest()
        {
            XboxFileInformation info = _xbox.FileSystem.GetFileInformation(@"E:\xbdm.ini");
            Assert.IsTrue(info.Attributes == FileAttributes.Normal);
            Assert.IsTrue(info.Size > 0 && info.Size < 0x1000);

            info = _xbox.FileSystem.GetFileInformation(XboxFileSystem.TempDirectory);
            Assert.IsTrue(info.Attributes == FileAttributes.Directory);
            Assert.IsTrue(info.Size == 0);
        }

        [TestMethod]
        public void FileStreamTest()
        {
            byte[] data = new byte[0x10000].FillRandom();

            string tempFile = XboxFileSystem.GetTempFileName();
            using (XboxFileStream fs = new XboxFileStream(_xbox, tempFile))
            {
                fs.Write(data, 0, data.Length);
                fs.Position = 0;
                byte[] data2 = new byte[data.Length];
                fs.Read(data2, 0, data2.Length);
                Assert.IsTrue(data.IsEqual(data2));
            }
            _xbox.FileSystem.DeleteFile(tempFile);
        }

        [TestMethod]
        public void SendReceiveFileTest()
        {
            string tempFile = XboxFileSystem.GetTempFileName();
            byte[] data = new byte[0x100000].FillRandom();
            _xbox.FileSystem.WriteFile(tempFile, data);
            byte[] data2 = _xbox.FileSystem.ReadFile(tempFile);
            Assert.IsTrue(data.IsEqual(data2));
            _xbox.FileSystem.DeleteFile(tempFile);
        }
    }
}

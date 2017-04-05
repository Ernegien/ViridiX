using System;
using System.Collections;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Linguist.FileSystem;
using ViridiX.Mason.Logging;

namespace ViridiX.Linguist.Tests
{
    [TestClass]
    public class XboxFileSystemTests
    {
        private Xbox _xbox;

        private const string TestDirectory = @"E:\tests";

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
        public void CreateDeleteDirectory()
        {
            // make sure the test directory exists
            _xbox.FileSystem.CreateDirectory(TestDirectory);
            _xbox.FileSystem.CreateDirectory(TestDirectory);    // test double creation

            string testDirectory = $@"E:\tests\{DateTime.Now.Ticks}";
            _xbox.FileSystem.CreateDirectory(testDirectory);
            Assert.IsTrue(_xbox.FileSystem.FileExists(testDirectory));
            _xbox.FileSystem.DeleteDirectory(testDirectory);
            Assert.IsFalse(_xbox.FileSystem.FileExists(testDirectory));
            _xbox.FileSystem.DeleteDirectory(testDirectory);    // test double deletion
        }

        [TestMethod]
        public void CreateDeleteFile()
        {
            // make sure the test directory exists
            _xbox.FileSystem.CreateDirectory(TestDirectory);

            string testFile = $@"E:\tests\{DateTime.Now.Ticks}.tmp";

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


            // TODO: more
        }

        [TestMethod]
        public void GetFileInformationTest()
        {
            // make sure the test directory exists
            _xbox.FileSystem.CreateDirectory(TestDirectory);

            XboxFileInformation info = _xbox.FileSystem.GetFileInformation(@"E:\xbdm.ini");
            Assert.IsTrue(info.Attributes == FileAttributes.Normal);
            Assert.IsTrue(info.Size > 0 && info.Size < 0x1000);

            info = _xbox.FileSystem.GetFileInformation(TestDirectory);
            Assert.IsTrue(info.Attributes == FileAttributes.Directory);
            Assert.IsTrue(info.Size == 0);
        }

        [TestMethod]
        public void FileStreamTest()
        {
            // make sure the test directory exists
            _xbox.FileSystem.CreateDirectory(TestDirectory);
            
            byte[] data = new byte[31];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(i % byte.MaxValue);
            }

            using (XboxFileStream fs = new XboxFileStream(_xbox, Path.Combine(TestDirectory, $"{DateTime.Now.Ticks}.tmp")))
            {
                fs.Write(data, 0, data.Length);

                fs.Position = 0;

                byte[] data2 = new byte[data.Length];
                fs.Read(data2, 0, data2.Length);

                if (!StructuralComparisons.StructuralEqualityComparer.Equals(data, data2))
                {
                    throw new Exception("File stream inconsistency detected");
                }
            }
        }

        // TODO: more

    }
}

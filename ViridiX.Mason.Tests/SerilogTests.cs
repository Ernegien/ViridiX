using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Mason.Logging;

namespace ViridiX.Mason.Tests
{
    [TestClass]
    public class SerilogTests
    {
        private ILogger _logger;
        readonly string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "{Date}.log");

        [TestInitialize]
        public void LoggerInitialize()
        {
            string logDirectory = Path.GetDirectoryName(_logPath);
            if (Directory.Exists(logDirectory))
            {
                Directory.Delete(logDirectory, true);
            }
            _logger = new SeriLogger(path: _logPath);
        }

        [TestMethod]
        public void LogLevelTest()
        {
            const int totalLevels = 6;

            // log all level combinations
            for (int logLevel = 0; logLevel < totalLevels; logLevel++)
            {
                _logger.Level = (LogLevel) logLevel;
                for (int eventLevel = 0; eventLevel < totalLevels; eventLevel++)
                {
                    string logMessage = $"LogLevel: {logLevel}, EventLevel {eventLevel}";
                    _logger.Log((LogLevel)eventLevel, logMessage);
                }
            }

            // wait for the log output to be flushed
            System.Threading.Thread.Sleep(15000);

            string logText = GetLogText();

            // check for the right output
            for (int logLevel = 0; logLevel < totalLevels; logLevel++)
            {
                _logger.Level = (LogLevel)logLevel;
                for (int eventLevel = 0; eventLevel < totalLevels; eventLevel++)
                {
                    string logMessage = $"LogLevel: {logLevel}, EventLevel {eventLevel}";
                    bool wasLogged = logText.Contains(logMessage);

                    if (eventLevel >= logLevel)
                    {
                        Assert.IsTrue(wasLogged);
                    }
                    else
                    {
                        Assert.IsFalse(wasLogged);
                    }
                }
            }
        }

        private string GetLogText()
        {
            StringBuilder text = new StringBuilder();

            // ReSharper disable once AssignNullToNotNullAttribute
            FileInfo[] logFiles = new DirectoryInfo(Path.GetDirectoryName(_logPath)).GetFiles();

            foreach (FileInfo file in logFiles)
            {
                using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                using (TextReader tr = new StreamReader(fs))
                {
                    text.Append(tr.ReadToEnd());
                }
            }

            return text.ToString();
        }

        [TestCleanup]
        public void LoggerCleanup()
        {
            _logger?.Dispose();
        }
    }
}

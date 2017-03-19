using System;
using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ViridiX.Mason.Logging
{
    /// <summary>
    /// Serilog structured event logger.
    /// </summary>
    public sealed class SeriLogger : LoggerBase
    {
        /// <summary>
        /// Allows the switching of log event levels without creating a new logger.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly LoggingLevelSwitch _levelSwitch = new LoggingLevelSwitch();

        /// <summary>
        /// Determines whether or not log messages are written.
        /// </summary>
        public override bool IsEnabled { get; set; }

        /// <summary>
        /// The log file path.
        /// </summary>
        public override string Path { get; set; }

        /// <summary>
        /// The minimum level used to filter message output.
        /// </summary>
        public override LogLevel Level
        {
            get { return (LogLevel) _levelSwitch.MinimumLevel; }
            set
            {
                if (value == (LogLevel) _levelSwitch.MinimumLevel) return;

                Info(null, "Logger level changed to {LogLevel}.", Level);
                _levelSwitch.MinimumLevel = (LogEventLevel)value;
            }
        }

        /// <summary>
        /// Creates a Serilog file.
        /// </summary>
        /// <param name="level">The minimum log level.</param>
        /// <param name="path">The log file path.</param>
        public SeriLogger(LogLevel level = LogLevel.Info, string path = null)
        {
            IsEnabled = true;
            Level = level;
            Path = path ?? System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "{Date}.log");

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(_levelSwitch)
                .WriteTo.RollingFile(Path, buffered: true,
                flushToDiskInterval: TimeSpan.FromSeconds(10))
                .CreateLogger();

            Info(null, "Logger initialized with {LogLevel} level.", Level);
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public override void Log(LogLevel level, Exception exception, string message, params object[] parameters)
        {
            try
            {
                if (IsEnabled)
                {
                    Serilog.Log.Write((LogEventLevel)level, exception, message, parameters);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public override void Flush()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Closes the log file.
        /// </summary>
        public override void Dispose()
        {
            Serilog.Log.CloseAndFlush();
        }
    }
}
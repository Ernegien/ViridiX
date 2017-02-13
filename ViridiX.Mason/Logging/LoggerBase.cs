using System;

namespace ViridiX.Mason.Logging
{
    /// <summary>
    /// Abstract base logger with some default functionality.
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        /// <summary>
        /// Determines whether or not log messages are written.
        /// </summary>
        public abstract bool IsEnabled { get; set; }

        /// <summary>
        /// The minimum level used to filter message output.
        /// </summary>
        public abstract LogLevel Level { get; set; }

        /// <summary>
        /// The log directory or file path depending on the specific implementation.
        /// </summary>
        public abstract string Path { get; set; }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Trace(Exception exception, string message, params object[] parameters)
        {
            Log(LogLevel.Trace, exception, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Trace(string message, params object[] parameters)
        {
            Trace(null, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Debug(Exception exception, string message, params object[] parameters)
        {
            Log(LogLevel.Debug, exception, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Debug(string message, params object[] parameters)
        {
            Debug(null, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Info"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Info(Exception exception, string message, params object[] parameters)
        {
            Log(LogLevel.Info, exception, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Info(string message, params object[] parameters)
        {
            Info(null, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Warn(Exception exception, string message, params object[] parameters)
        {
            Log(LogLevel.Warn, exception, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Warn(string message, params object[] parameters)
        {
            Warn(null, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Error"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Error(Exception exception, string message, params object[] parameters)
        {
            Log(LogLevel.Error, exception, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Error(string message, params object[] parameters)
        {
            Error(null, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Fatal(Exception exception, string message, params object[] parameters)
        {
            Log(LogLevel.Fatal, exception, message, parameters);
        }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Fatal(string message, params object[] parameters)
        {
            Fatal(null, message, parameters);
        }

        /// <summary>
        /// Logs a message at the specified log level.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public abstract void Log(LogLevel level, Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the specified log level.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        public void Log(LogLevel level, string message, params object[] parameters)
        {
            Log(level, null, message, parameters);
        }

        /// <summary>
        /// Flushes any cached messages.
        /// </summary>
        public abstract void Flush();

        /// <summary>
        /// Logger cleanup.
        /// </summary>
        public abstract void Dispose();
    }
}

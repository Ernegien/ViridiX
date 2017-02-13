using System;

namespace ViridiX.Mason.Logging
{
    /// <summary>
    /// A generic interface for logging.
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Determines whether or not log messages are written.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// The minimum level used to filter message output.
        /// </summary>
        LogLevel Level { get; set; }

        /// <summary>
        /// The log directory or file path depending on the specific implementation.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Trace(Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Trace"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Trace(string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Debug(Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Debug(string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Info"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Info(Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Info(string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Warn(Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Warn(string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Error"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Error(Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Error(string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Fatal(Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the <see cref="F:LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Fatal(string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the specified log level.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The related exception.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Log(LogLevel level, Exception exception, string message, params object[] parameters);

        /// <summary>
        /// Logs a message at the specified log level.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="parameters">Additional context.</param>
        void Log(LogLevel level, string message, params object[] parameters);

        /// <summary>
        /// Flushes any cached messages.
        /// </summary>
        void Flush();
    }
}

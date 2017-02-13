namespace ViridiX.Mason.Logging
{
    /// <summary>
    /// The minimum level a logger uses to filter event message output.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Verbose information reserved for targeted troubleshooting efforts.
        /// </summary>
        Trace,

        /// <summary>
        /// Useful information to developers.
        /// </summary>
        Debug,

        /// <summary>
        /// Normal operational information.
        /// </summary>
        Info,

        /// <summary>
        /// Potential issue. 
        /// </summary>
        Warn,

        /// <summary>
        /// Recoverable issue.
        /// </summary>
        Error,

        /// <summary>
        /// Unrecoverable issue.
        /// </summary>
        Fatal
    }
}

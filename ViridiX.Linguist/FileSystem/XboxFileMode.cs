namespace ViridiX.Linguist.FileSystem
{
    /// <summary>
    /// Specifies how a file should be opened.
    /// </summary>
    public enum XboxFileMode
    {
        /// <summary>
        /// Opens an existing file. Throws an exception if the file does not exist.
        /// </summary>
        Open,

        /// <summary>
        /// Creates a file, overwriting any existing file.
        /// </summary>
        Create,

        /// <summary>
        /// Creates a new file. Throws an exception if the file already exists.
        /// </summary>
        CreateNew
    }
}

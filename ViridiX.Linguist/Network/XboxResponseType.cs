namespace ViridiX.Linguist.Network
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public enum XboxResponseType
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        SingleResponse = 200,

        /// <summary>
        /// TODO: description
        /// </summary>
        Connected = 201,

        /// <summary>
        /// TODO: description
        /// </summary>
        MultiResponse = 202,  //terminates with period

        /// <summary>
        /// TODO: description
        /// </summary>
        BinaryResponse = 203,

        /// <summary>
        /// TODO: description
        /// </summary>
        ReadyForBinary = 204,

        /// <summary>
        /// TODO: description
        /// </summary>
        NowNotifySession = 205,  // notificaiton channel/ dedicated connection (XBDM_DEDICATED)

        /// <summary>
        /// TODO: description
        /// </summary>
        UndefinedError = 400,

        /// <summary>
        /// TODO: description
        /// </summary>
        MaxConnectionsExceeded = 401,

        /// <summary>
        /// TODO: description
        /// </summary>
        FileNotFound = 402,

        /// <summary>
        /// TODO: description
        /// </summary>
        NoSuchModule = 403,

        /// <summary>
        /// TODO: description
        /// </summary>
        MemoryNotMapped = 404,  //setzerobytes or setmem failed

        /// <summary>
        /// TODO: description
        /// </summary>
        NoSuchThread = 405,

        /// <summary>
        /// TODO: description
        /// </summary>
        ClockNotSet = 406,  //linetoolong or clocknotset

        /// <summary>
        /// TODO: description
        /// </summary>
        UnknownCommand = 407,

        /// <summary>
        /// TODO: description
        /// </summary>
        NotStopped = 408,

        /// <summary>
        /// TODO: description
        /// </summary>
        FileMustBeCopied = 409,

        /// <summary>
        /// TODO: description
        /// </summary>
        FileAlreadyExists = 410,

        /// <summary>
        /// TODO: description
        /// </summary>
        DirectoryNotEmpty = 411,

        /// <summary>
        /// TODO: description
        /// </summary>
        BadFileName = 412,

        /// <summary>
        /// TODO: description
        /// </summary>
        FileCannotBeCreated = 413,

        /// <summary>
        /// TODO: description
        /// </summary>
        AccessDenied = 414,

        /// <summary>
        /// TODO: description
        /// </summary>
        NoRoomOnDevice = 415,

        /// <summary>
        /// TODO: description
        /// </summary>
        NotDebuggable = 416,

        /// <summary>
        /// TODO: description
        /// </summary>
        TypeInvalid = 417,

        /// <summary>
        /// TODO: description
        /// </summary>
        DataNotAvailable = 418,

        /// <summary>
        /// TODO: description
        /// </summary>
        BoxIsNotLocked = 420,

        /// <summary>
        /// TODO: description
        /// </summary>
        KeyExchangeRequired = 421,

        /// <summary>
        /// TODO: description
        /// </summary>
        DedicatedConnectionRequired = 422,

        /// <summary>
        /// TODO: description
        /// </summary>
        InvalidArgument = 423,

        /// <summary>
        /// TODO: description
        /// </summary>
        ProfileNotStarted = 424,

        /// <summary>
        /// TODO: description
        /// </summary>
        ProfileAlreadyStarted = 425,

        /// <summary>
        /// TODO: description
        /// </summary>
        D3DDebugCommandNotImplemented = 480,

        /// <summary>
        /// TODO: description
        /// </summary>
        D3DInvalidSurface = 481,

        /// <summary>
        /// TODO: description
        /// </summary>
        VxTaskPending = 496,

        /// <summary>
        /// TODO: description
        /// </summary>
        VxTooManySessions = 497,
    }
}

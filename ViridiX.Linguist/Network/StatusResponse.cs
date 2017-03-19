namespace ViridiX.Linguist.Network
{
    /// <summary>
    /// Xbox command status response.
    /// </summary>
    public class StatusResponse
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        public string Full { get; }

        /// <summary>
        /// TODO: description
        /// </summary>
        public XboxResponseType Type { get; }

        /// <summary>
        /// TODO: description
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// TODO: description
        /// </summary>
        public bool Success => ((int)Type & 200) == 200;

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="full"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public StatusResponse(string full, XboxResponseType type, string message)
        {
            Full = full;
            Type = type;
            Message = message;
        }
    }
}

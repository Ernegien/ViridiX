using System;

namespace ViridiX.Linguist.Network
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxNotificationEventArgs : EventArgs
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="message"></param>
        public XboxNotificationEventArgs(string message)
        {
            Message = message;
        }
    }
}

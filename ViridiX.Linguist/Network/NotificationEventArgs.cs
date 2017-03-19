using System;

namespace ViridiX.Linguist.Network
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="message"></param>
        public NotificationEventArgs(string message)
        {
            Message = message;
        }
    }
}

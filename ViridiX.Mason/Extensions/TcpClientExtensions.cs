using System;
using System.Net.Sockets;
using ViridiX.Mason.Utilities;

namespace ViridiX.Mason.Extensions
{
    public static class TcpClientExtensions
    {
        /// <summary>
        /// Temporarily extends the receive timeout value.
        /// Wrap with a using statement to control scope automatically, or dispose manually.
        /// </summary>
        /// <param name="client">The TcpClient.</param>
        /// <param name="extension">The number of milliseconds to extend the timeout for.</param>
        /// <returns>An object that will restore the original timeout value once disposed.</returns>
        public static IDisposable ExtendReceiveTimeout(this TcpClient client, int extension)
        {
            return new TemporaryPropertyAssignment<int>(client, nameof(client.ReceiveTimeout), client.ReceiveTimeout + extension);
        }

        /// <summary>
        /// Temporarily extends the send timeout value.
        /// Wrap with a using statement to control scope automatically, or dispose manually.
        /// </summary>
        /// <param name="client">The TcpClient.</param>
        /// <param name="extension">The number of milliseconds to extend the timeout for.</param>
        /// <returns>An object that will restore the original timeout value once disposed.</returns>
        public static IDisposable ExtendSendTimeout(this TcpClient client, int extension)
        {
            return new TemporaryPropertyAssignment<int>(client, nameof(client.SendTimeout), client.SendTimeout + extension);
        }
    }
}

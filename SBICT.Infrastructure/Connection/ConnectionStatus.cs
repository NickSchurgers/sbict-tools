// <copyright file="ConnectionStatus.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    /// <summary>
    /// Status of connections.
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// Client is connecting.
        /// </summary>
        Connecting,

        /// <summary>
        /// Client is reconnecting.
        /// </summary>
        Reconnecting,

        /// <summary>
        /// Client is connected.
        /// </summary>
        Connected,

        /// <summary>
        /// Client is disconnected.
        /// </summary>
        Disconnected,
    }
}
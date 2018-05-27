// <copyright file="IConnection.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;

    /// <summary>
    /// Instance of a connection with a hub.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// Raised when the status changes.
        /// </summary>
        event EventHandler<ConnectionEventArgs> ConnectionStatusChanged;

        /// <summary>
        /// Raised when the status of another user changes.
        /// </summary>
        event EventHandler<ConnectionEventArgs> UserStatusChanged;

        /// <summary>
        /// Gets status of the connection.
        /// </summary>
        ConnectionStatus Status { get; }

        /// <summary>
        /// Gets the hub the connection is used to connect with.
        /// </summary>
        HubConnection Hub { get; }

        /// <summary>
        /// Gets name of the hub connected with.
        /// </summary>
        string HubName { get; }

        /// <summary>
        /// Activate the connection.
        /// </summary>
        /// <returns>Task.</returns>
        Task StartAsync();

        /// <summary>
        /// Stop the connection with the hub.
        /// </summary>
        /// <returns>Task.</returns>
        Task StopAsync();
    }
}
// <copyright file="ConnectionEventArgs.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    using System;
    using SBICT.Data;

    /// <summary>
    /// Arguments for events related to a connection.
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets status of the connection.
        /// </summary>
        public ConnectionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the hub the event is triggered by.
        /// </summary>
        public string HubName { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the event.
        /// </summary>
        public IUser User { get; set; }
    }
}
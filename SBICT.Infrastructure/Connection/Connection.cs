// <copyright file="Connection.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.AspNetCore.SignalR.Client;
    using SBICT.Data;

    /// <inheritdoc />
    public class Connection : IConnection
    {

        private ConnectionStatus status = ConnectionStatus.Disconnected;
        private bool isStarted;

        /// <inheritdoc />
        public event EventHandler<ConnectionEventArgs> ConnectionStatusChanged;

        /// <inheritdoc />
        public event EventHandler<ConnectionEventArgs> UserStatusChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="connection">Instance of Hubconnection.</param>
        /// <param name="hubName">Name of the hub.</param>
        public Connection(HubConnection connection, string hubName)
        {
            this.Hub = connection;
            this.HubName = hubName;
        }

        /// <inheritdoc />
        public ConnectionStatus Status
        {
            get => this.status;
            set
            {
                if (value == this.status)
                {
                    return;
                }

                this.status = value;
                this.OnConnectionStatusChanged(new ConnectionEventArgs { Status = this.status });
            }
        }

        /// <inheritdoc />
        public HubConnection Hub { get; }

        /// <inheritdoc />
        public string HubName { get; }

        /// <inheritdoc/>
        public async Task StartAsync()
        {
            if (!this.isStarted)
            {
                try
                {
                    this.Status = ConnectionStatus.Connecting;
                    this.Hub.On<User, ConnectionScope>(
                        "Connected",
                        (connUser, scope) => this.OnUserStatusChanged(new ConnectionEventArgs
                        {
                            User = connUser,
                            HubName = this.HubName,
                            Status = ConnectionStatus.Connected,
                        }));

                    this.Hub.On<User, ConnectionScope>(
                        "Disconnected",
                        (connUser, scope) => this.OnUserStatusChanged(new ConnectionEventArgs
                        {
                            User = connUser,
                            HubName = this.HubName,
                            Status = ConnectionStatus.Disconnected,
                        }));
                    await this.Hub.StartAsync();
                    this.Status = ConnectionStatus.Connected;
                    this.isStarted = true;
                }
                catch (Exception e)
                {
                    await this.StopAsync();
                    throw new HubException(e.Message);
                }
            }
        }

        /// <inheritdoc />
        public async Task StopAsync()
        {
            if (this.isStarted)
            {
                await this.Hub.StopAsync();
                this.Status = ConnectionStatus.Disconnected;
                this.isStarted = false;
            }
        }

        /// <summary>
        /// Raised when the status of this connection changes
        /// </summary>
        /// <param name="e">ConnectionEventArgs</param>
        protected virtual void OnConnectionStatusChanged(ConnectionEventArgs e)
        {
            this.ConnectionStatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raised when the status of a user changes.
        /// </summary>
        /// <param name="e">ConnectionEventArgs</param>
        protected virtual void OnUserStatusChanged(ConnectionEventArgs e)
        {
            this.UserStatusChanged?.Invoke(this, e);
        }
    }
}
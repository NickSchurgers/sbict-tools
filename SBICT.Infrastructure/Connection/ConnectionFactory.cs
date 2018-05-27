// <copyright file="ConnectionFactory.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.AspNetCore.SignalR.Client;

    /// <inheritdoc />
    public class ConnectionFactory : IConnectionFactory
    {
        /// <inheritdoc />
        public IConnection Create(string url, string hubName)
        {
            return new Connection(
                new HubConnectionBuilder().WithUrl(url, u =>
                    {
                        u.UseDefaultCredentials = true;
                        u.Transports = HttpTransportType.WebSockets;
                    })
                    .Build(),
                hubName);
        }
    }
}
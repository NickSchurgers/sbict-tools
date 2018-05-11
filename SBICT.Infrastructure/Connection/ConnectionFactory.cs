﻿using System.Net;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace SBICT.Infrastructure.Connection
{
    public static class ConnectionFactory
    {
        /// <summary>
        /// Creates a new Connection class with the required settings
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IConnection Create(string url)
        {
            return new Connection(new HubConnectionBuilder().WithUrl(url, u =>
            {
                u.UseDefaultCredentials = true;
                u.Transports = HttpTransportType.WebSockets;
            }).Build());
        }
    }
}
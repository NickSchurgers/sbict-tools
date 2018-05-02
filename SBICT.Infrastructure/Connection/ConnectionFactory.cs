using System.Net;
using Microsoft.AspNetCore.SignalR.Client;

namespace SBICT.Infrastructure.Connection
{
    public static class ConnectionFactory
    {
        public static Connection Create(string url)
        {
            return new Connection(new HubConnectionBuilder().WithUrl(url)
                .WithConsoleLogger()
                .WithTransport(Microsoft.AspNetCore.Http.Connections.TransportType.WebSockets)
                .WithCredentials(CredentialCache.DefaultCredentials).Build());
        }
    }
}
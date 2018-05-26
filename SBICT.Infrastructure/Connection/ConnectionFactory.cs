using System.Net;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;

namespace SBICT.Infrastructure.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IEventAggregator eventAggregator;

        public ConnectionFactory(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Creates a new Connection class with the required settings
        /// </summary>
        /// <param name="url"></param>
        /// <param name="hubName"></param>
        /// <returns></returns>
        public IConnection Create(string url, string hubName)
        {
            return new Connection(new HubConnectionBuilder().WithUrl(url, u =>
            {
                u.UseDefaultCredentials = true;
                u.Transports = HttpTransportType.WebSockets;
            }).Build(), hubName, this.eventAggregator);
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;

namespace SBICT.Infrastructure.Connection
{
    public interface IConnection
    {
        ConnectionStatus Status { get; set; }
        HubConnection Hub { get; set; }
        string HubName { get; }
        Task StartAsync();
        Task StopAsync();

        event EventHandler<ConnectionEventArgs> ConnectionStatusChanged;
        event EventHandler<ConnectionEventArgs> UserStatusChanged;
    }
}
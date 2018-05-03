using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Hubs
{
    [Authorize]
    public abstract class HubBase: Hub
    {
        protected static readonly ConnectionStore<string> ConnectedUsers =
            new ConnectionStore<string>();

        public override async Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
            ConnectedUsers.Add(name, Context.ConnectionId);

            await Clients.Others.SendAsync("Connected", name, ConnectionScope.System);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var name = Context.User.Identity.Name;
            ConnectedUsers.Remove(name, Context.ConnectionId);

            await Clients.Others.SendAsync("Disconnected", name, ConnectionScope.System);
            await base.OnDisconnectedAsync(ex);
        }

    }
}
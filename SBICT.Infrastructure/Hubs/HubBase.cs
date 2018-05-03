using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Hubs
{
    [Authorize]
    public abstract class HubBase : Hub
    {
        #region Fields

        protected static readonly ConnectionStore<string> ConnectedUsers =
            new ConnectionStore<string>();

        #endregion

        #region Methods

        /// <summary>
        /// Triggered when a client connects to this hub
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
            ConnectedUsers.Add(name, Context.ConnectionId);

            await Clients.Others.SendAsync("Connected", name, ConnectionScope.System);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Triggered when a client disconnects from this hub
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var name = Context.User.Identity.Name;
            ConnectedUsers.Remove(name, Context.ConnectionId);

            await Clients.Others.SendAsync("Disconnected", name, ConnectionScope.System);
            await base.OnDisconnectedAsync(ex);
        }

        #endregion
    }
}
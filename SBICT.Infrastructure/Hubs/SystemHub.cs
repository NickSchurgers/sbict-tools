using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Hubs
{
    [Authorize]
    public class SystemHub: Hub
    {
        private static readonly ConnectionStore<string> ConnectedUsers =
            new ConnectionStore<string>();

        public override async Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
            ConnectedUsers.Add(name, Context.ConnectionId);

            await Clients.All.SendAsync("Connected", name, ConnectionScope.System);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var name = Context.User.Identity.Name;
            ConnectedUsers.Remove(name, Context.ConnectionId);

            await Clients.All.SendAsync("Disconnected", name, ConnectionScope.System);
            await base.OnDisconnectedAsync(ex);
        }

//        public async Task GetUserList()
//        {
//            var users = ConnectedUsers.GetConnections();
//            await Clients.Caller.SendAsync("UserList", users);
//        }
//        
//        public async Task SendMessage(string recipient, string message, ConnectionScope scope)
//        {
//            var sender = Context.User.Identity.Name;
//            var target = Clients.All;
//            switch (scope)
//            {
//                case ConnectionScope.Group:
//                    target = Clients.Group(recipient);
//                    break;
//                case ConnectionScope.User:
//                    target = Clients.User(recipient);
//                    break;
//                case ConnectionScope.Broadcast:
//                case ConnectionScope.System:
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
//            }
//
//            await target.SendAsync("SendMessage", sender, message, scope == ConnectionScope.Broadcast);
//        }
    }
}
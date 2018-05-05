using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : HubBase
    {
        public IEnumerable<string> GetUserList()
        {
#if DEBUG
            return ConnectedUsers.GetConnections();
#else
            return ConnectedUsers.GetConnections().Where(u => u != Context.User.Identity.Name); 
#endif
        }

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using SBICT.Data;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Hubs
{
    [Authorize]
    public abstract class HubBase : Hub
    {
//        protected static readonly ConnectionStore<Guid> UserConnectionStore = new ConnectionStore<Guid>();
//        protected static readonly HashSet<User> UserList = new HashSet<User>(new UserComparer());

        protected static readonly IStore<Guid, string> UserConnectionStore =
            new InMemoryStore<Guid, string>();

        protected static readonly IStore<IUser, byte>
            UserStore = new InMemoryStore<IUser, byte>(new UserComparer());


        /// <summary>
        /// Triggered when a client connects to this hub
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var query = Context.Features.Get<IHttpContextFeature>()?.HttpContext.Request.Query;
            query?.TryGetValue("guid", out var id);

            var guid = Guid.Parse(id);

            if (UserConnectionStore.Count(guid) == 0)
            {
                query?.TryGetValue("displayName", out var name);
                var user = new User(guid, Context.User.Identity.Name)
                {
                    DisplayName = name
                };

                UserStore.Add(user, new byte());
                await Clients.AllExcept(Context.ConnectionId).SendAsync("Connected", user, ConnectionScope.System);
            }

            UserConnectionStore.Add(guid, Context.ConnectionId);
//            UserList.Add(user);
//            UserConnectionStore.Add(user.Id, Context.ConnectionId);
//
//            if (UserConnectionStore.GetConnections(user.Id).Count() == 1)
//            {
//                await Clients.AllExcept(UserConnectionStore.GetConnections(user.Id).ToList())
//                    .SendAsync("Connected", user, ConnectionScope.System);
//            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Triggered when a client disconnects from this hub
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var query = Context.Features.Get<IHttpContextFeature>()?.HttpContext.Request.Query;
            query?.TryGetValue("guid", out var id);
            var guid = Guid.Parse(id);

            UserConnectionStore.Remove(guid, Context.ConnectionId);
            if (UserConnectionStore.Count(guid) == 0)
            {
                var user = UserStore.GetKey(u => u.Id == guid);
                UserStore.Remove(user);
                await Clients.All.SendAsync("Disconnected", user, ConnectionScope.System);
            }
//            var user = UserList.Single(u => u.Id == Guid.Parse(id));
//            UserConnectionStore.Remove(user.Id, Context.ConnectionId);
//
//            if (!UserConnectionStore.GetConnections(user.Id).Any())
//            {
//                UserList.Remove(user);
//                await Clients.All.SendAsync("Disconnected", user, ConnectionScope.System);
//            }

            await base.OnDisconnectedAsync(ex);
        }
    }
}
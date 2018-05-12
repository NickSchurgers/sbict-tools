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
        #region Fields

        protected static readonly ConnectionStore<Guid> UserConnectionStore = new ConnectionStore<Guid>();
        protected static readonly HashSet<User> UserList = new HashSet<User>(new UserComparer());

        #endregion

        #region Methods

        /// <summary>
        /// Triggered when a client connects to this hub
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var query = Context.Features.Get<IHttpContextFeature>()?.HttpContext.Request.Query;
            query?.TryGetValue("displayName", out var name);
            query?.TryGetValue("guid", out var id);
            var user = new User(Guid.Parse(id), Context.User.Identity.Name)
            {
                DisplayName = name
            };
            UserList.Add(user);
            UserConnectionStore.Add(user.Id, Context.ConnectionId);

#if DEBUG
            await Clients.Others.SendAsync("Connected", user, ConnectionScope.System);
#else
            if (UserConnectionStore.GetConnections(user.Id).Count() <= 1)
            {
                await Clients.AllExcept(UserConnectionStore.GetConnections(user.Id).ToList())
                    .SendAsync("Connected", user, ConnectionScope.System);
            }
#endif


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
            var user = UserList.First(u => u.Id == Guid.Parse(id));
            UserConnectionStore.Remove(user.Id, Context.ConnectionId);

#if DEBUG
            await Clients.Others.SendAsync("Disconnected", user, ConnectionScope.System);
#else
            if (!UserConnectionStore.GetConnections(user.Id).Any())
            {
                UserList.Remove(user);
                await Clients.All.SendAsync("Disconnected", user, ConnectionScope.System);
            }
#endif
            await base.OnDisconnectedAsync(ex);
        }

        #endregion
    }
}
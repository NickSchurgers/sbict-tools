// <copyright file="HubBase.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Connections.Features;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using SBICT.Data;
    using SBICT.Infrastructure.Connection;

    /// <inheritdoc />
    [Authorize]
    public abstract class HubBase : Hub
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HubBase"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of ILogggerfactory.</param>
        protected HubBase(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("Hub");
        }

        /// <inheritdoc />
        public override async Task OnConnectedAsync()
        {
            var query = this.Context.Features.Get<IHttpContextFeature>()?.HttpContext.Request.Query;
            query?.TryGetValue("guid", out var id);
            query?.TryGetValue("displayName", out var name);

            var guid = Guid.Parse(id);
            var user = new User(guid, this.Context.User.Identity.Name)
            {
                DisplayName = name,
            };

            this.GetUserConnectionStore().Add(user, this.Context.ConnectionId);
            if (this.GetUserConnectionStore().Count(user) < 2)
            {
                await this.Clients.AllExcept(this.Context.ConnectionId)
                    .SendAsync("Connected", user, ConnectionScope.System);
            }

            await base.OnConnectedAsync();
        }

        /// <inheritdoc />
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var query = this.Context.Features.Get<IHttpContextFeature>()?.HttpContext.Request.Query;
            query?.TryGetValue("guid", out var id);

            var guid = Guid.Parse(id);
            var user = this.GetUserConnectionStore().GetKey(u => u.Id == guid);
            this.GetUserConnectionStore().Remove(user, this.Context.ConnectionId);
            if (this.GetUserConnectionStore().Count(user) == 0)
            {
                await this.Clients.All.SendAsync("Disconnected", user, ConnectionScope.System);
            }

            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// Get store with connected users and their clients.
        /// </summary>
        /// <returns>Instance of store.</returns>
        protected abstract IStore<IUser, string> GetUserConnectionStore();
    }
}
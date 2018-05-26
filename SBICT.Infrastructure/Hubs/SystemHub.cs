// <copyright file="SystemHub.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SBICT.Data;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Hubs
{
    using Microsoft.AspNetCore.Authorization;

    /// <inheritdoc />
    /// <summary>
    /// Hub used for system connections.
    /// </summary>
    [Authorize]
    public class SystemHub : Hub
    {
        private ILogger _logger;

        public SystemHub(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger("SystemHub");
        }

        private static readonly IStore<IUser, string> _userConnectionStore =
            new InMemoryStore<IUser, string>(new UserComparer());

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

            _userConnectionStore.Add(user, this.Context.ConnectionId);
            if (_userConnectionStore.Count(user) < 2)
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
            var user = _userConnectionStore.GetKey(u => u.Id == guid);
            _userConnectionStore.Remove(user, this.Context.ConnectionId);
            if (_userConnectionStore.Count(user) == 0)
            {
                await this.Clients.All.SendAsync("Disconnected", user, ConnectionScope.System);
            }

            await base.OnDisconnectedAsync(ex);
        }
    }
}
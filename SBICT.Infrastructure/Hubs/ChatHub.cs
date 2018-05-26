// <copyright file="ChatHub.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

// ReSharper disable ClassNeverInstantiated.Global

using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.Extensions.Logging;

namespace SBICT.Infrastructure.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using SBICT.Data;
    using SBICT.Infrastructure.Connection;

    /// <summary>
    /// Hub used for chat connections.
    /// </summary>
    [Authorize]
    public class ChatHub: Hub
    {
        private static readonly IStore<IUser, string> _userConnectionStore =
            new InMemoryStore<IUser, string>(new UserComparer());
        
        private ILogger _logger;
        
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

        /// <summary>
        /// Store for groups.
        /// </summary>
        private static readonly ConnectionStore<Guid> GroupStore = new ConnectionStore<Guid>();

        private static readonly HashSet<Group> GroupList = new HashSet<Group>();


        /// <summary>
        /// Return list of currently connected users.
        /// </summary>
        /// <param name="userId">UserId of user to get the list of users for.</param>
        /// <returns>List of connected users.</returns>
        public IEnumerable<User> GetUserList(Guid userId)
        {
            return _userConnectionStore.GetKeys(u => u.Id != userId).Cast<User>();
        }


        /// <summary>
        /// Return a list of groups the user is currently connected to on other clients.
        /// </summary>
        /// <param name="userId">UserId of the user to get the groups for.</param>
        /// <returns>List of groups the users is in.</returns>
        // ReSharper disable once UnusedMember.Global
        public IEnumerable<Group> GetGroupsForUser(Guid userId)
        {
            var connectedGroups = GroupStore.GetByConnection(userId.ToString());
            return GroupList.Where(g => connectedGroups.Contains(g.Id));
        }

        /// <summary>
        /// Join a group or create if not exists.
        /// </summary>
        /// <param name="group">Group to create or join.</param>
        /// <param name="userId">UserId of the user to add to the group.</param>
        /// <returns>GroupCreated or GroupJoined event.</returns>
        // ReSharper disable once UnusedMember.Global
        public async Task GroupJoin(Group group, Guid userId)
        {
            var user = _userConnectionStore.GetKey(u => u.Id == userId);
            var userConnections = _userConnectionStore.GetValues(user);
            foreach (var conId in userConnections)
            {
                await this.Groups.AddToGroupAsync(conId, group.Name);
            }

            GroupStore.Add(group.Id, userId.ToString());

            if (GroupStore.GetConnections(group.Id).Count() < 2)
            {
                GroupList.Add(group);
                await this.Clients.Group(group.Name).SendAsync("GroupCreated", group);
            }
            else
            {
                await this.Clients.Group(group.Name).SendAsync("GroupJoined", group, user);
            }
        }

        /// <summary>
        /// Invite a client to join a group.
        /// </summary>
        /// <param name="group">Group to invite the user to.</param>
        /// <param name="userId">UserId of the user to invite to the group.</param>
        /// <returns>GroupInvited event.</returns>
        // ReSharper disable once UnusedMember.Global
        public async Task GroupInvite(Group group, Guid userId)
        {
            var user = _userConnectionStore.GetKey(u => u.Id == userId);
            var userConnections = _userConnectionStore.GetValues(user);
            var storedGroup = GroupList.Single(g => g.Id == group.Id);
            await this.Clients.Clients(userConnections.ToList()).SendAsync("GroupInvited", storedGroup);
        }

        /// <summary>
        /// Leave a group.
        /// </summary>
        /// <param name="group">Group to leave.</param>
        /// <param name="userId">UserId of the user to leave the group.</param>
        /// <returns>GroupLeft event.</returns>
        // ReSharper disable once UnusedMember.Global
        public async Task GroupLeave(Group group, Guid userId)
        {
            var user = _userConnectionStore.GetKey(u => u.Id == userId);
            var userConnections = _userConnectionStore.GetValues(user);

            foreach (var conId in userConnections)
            {
                await this.Groups.RemoveFromGroupAsync(conId, group.Name);
            }

            GroupStore.Remove(group.Id, user.Id.ToString());

            await this.Clients.Group(group.Name).SendAsync("GroupLeft", group, user);

            if (!GroupStore.GetConnections(group.Id).Any())
            {
                GroupList.RemoveWhere(g => g.Id == group.Id);
            }
        }

        /// <summary>
        /// Send message to a user/group.
        /// </summary>
        /// <param name="recipient">Target of the message.</param>
        /// <param name="sender">Sender of the message.</param>
        /// <param name="message">Message being sent.</param>
        /// <param name="scope">Scope of the message.</param>
        /// <returns>MessageReceived event.</returns>
        // ReSharper disable once UnusedMember.Global
        public async Task SendMessage(Guid recipient, Guid sender, Message message, ConnectionScope scope)
        {
            IClientProxy target;
            var user = _userConnectionStore.GetKey(u => u.Id == sender);
            switch (scope)
            {
                case ConnectionScope.Group:
                    target = this.Clients.GroupExcept(recipient.ToString(),
                        _userConnectionStore.GetValues(user).ToList());
                    break;
                case ConnectionScope.User:
                    var rec = _userConnectionStore.GetKey(r => r.Id == recipient);
                    var values = _userConnectionStore.GetValues(rec);
                    target = this.Clients.Clients(values.ToList());
                    break;
                default:
                    target = this.Clients.All;
                    break;
            }


            await target.SendAsync("MessageReceived", recipient, user, message, scope);
        }

        public ChatHub(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger("ChatHub");
        }
    }
}
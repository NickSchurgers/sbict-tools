// <copyright file="ChatHub.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

// ReSharper disable ClassNeverInstantiated.Global

namespace SBICT.Infrastructure.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using SBICT.Data;
    using SBICT.Infrastructure.Connection;

    /// <summary>
    /// Hub used for chat connections.
    /// </summary>
    [Authorize]
    public class ChatHub : HubBase
    {
        private static readonly IStore<IUser, string> UserConnectionStore =
            new InMemoryStore<IUser, string>(new UserComparer());

        private static readonly IStore<IGroup, Guid> GroupConnectionStore = new InMemoryStore<IGroup, Guid>();

        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatHub"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of ILoggerFactory.</param>
        public ChatHub(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("ChatHub");
        }

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Return list of currently connected users.
        /// </summary>
        /// <param name="userId">UserId of user to get the list of users for.</param>
        /// <returns>List of connected users.</returns>
        public IEnumerable<User> GetUserList(Guid userId)
        {
            return UserConnectionStore.GetKeys(u => u.Key.Id != userId).Cast<User>();
        }

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Return a list of groups the user is currently connected to on other clients.
        /// </summary>
        /// <param name="userId">UserId of the user to get the groups for.</param>
        /// <returns>List of groups the users is in.</returns>
        public IEnumerable<Group> GetGroupsForUser(Guid userId)
        {
            return GroupConnectionStore.GetKeys(x => x.Value.Contains(userId)).Cast<Group>();
        }

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Join a group or create if not exists.
        /// </summary>
        /// <param name="group">Group to create or join.</param>
        /// <param name="userId">UserId of the user to add to the group.</param>
        /// <returns>GroupCreated or GroupJoined event.</returns>
        public async Task GroupJoin(Group group, Guid userId)
        {
            var pair = UserConnectionStore.GetKeyValuePair(u => u.Key.Id == userId);
            foreach (var conId in pair.Value)
            {
                await this.Groups.AddToGroupAsync(conId, group.Name);
            }

            GroupConnectionStore.Add(group, pair.Key.Id);

            if (GroupConnectionStore.Count(group) == 1)
            {
                await this.Clients.Group(group.Name).SendAsync("GroupCreated", group);
            }
            else
            {
                await this.Clients.Group(group.Name).SendAsync("GroupJoined", group, pair.Key);
            }
        }

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Invite a client to join a group.
        /// </summary>
        /// <param name="group">Group to invite the user to.</param>
        /// <param name="userId">UserId of the user to invite to the group.</param>
        /// <returns>GroupInvited event.</returns>
        public async Task GroupInvite(Group group, Guid userId)
        {
            var pair = UserConnectionStore.GetKeyValuePair(u => u.Key.Id == userId);
            var storedGroup = GroupConnectionStore.GetKey(g => g.Id == group.Id);
            await this.Clients.Clients(pair.Value.ToList()).SendAsync("GroupInvited", storedGroup);
        }

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Leave a group.
        /// </summary>
        /// <param name="group">Group to leave.</param>
        /// <param name="userId">UserId of the user to leave the group.</param>
        /// <returns>GroupLeft event.</returns>
        public async Task GroupLeave(Group group, Guid userId)
        {
            var pair = UserConnectionStore.GetKeyValuePair(u => u.Key.Id == userId);
            var storedGroup = GroupConnectionStore.GetKey(g => g.Id == group.Id);
            foreach (var conId in pair.Value)
            {
                await this.Groups.RemoveFromGroupAsync(conId, group.Name);
            }

            GroupConnectionStore.Remove(storedGroup, pair.Key.Id);

            await this.Clients.Group(storedGroup.Name).SendAsync("GroupLeft", group, pair.Key);
        }

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Send message to a user/group.
        /// </summary>
        /// <param name="recipient">Target of the message.</param>
        /// <param name="sender">Sender of the message.</param>
        /// <param name="message">Message being sent.</param>
        /// <param name="scope">Scope of the message.</param>
        /// <returns>MessageReceived event.</returns>
        public async Task SendMessage(Guid recipient, Guid sender, Message message, ConnectionScope scope)
        {
            IClientProxy target;
            var user = UserConnectionStore.GetKey(u => u.Id == sender);
            switch (scope)
            {
                case ConnectionScope.Group:
                    var group = GroupConnectionStore.GetKey(g => g.Id == recipient);
                    target = this.Clients.GroupExcept(
                        group.Name,
                        UserConnectionStore.GetValues(user).ToList());
                    break;
                case ConnectionScope.User:
                    var rec = UserConnectionStore.GetKey(r => r.Id == recipient);
                    var values = UserConnectionStore.GetValues(rec);
                    target = this.Clients.Clients(values.ToList());
                    break;
                default:
                    target = this.Clients.All;
                    break;
            }

            await target.SendAsync("MessageReceived", recipient, user, message, scope);
        }

        /// <inheritdoc />
        protected override IStore<IUser, string> GetUserConnectionStore()
        {
            return UserConnectionStore;
        }
    }
}
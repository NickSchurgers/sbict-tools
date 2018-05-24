using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SBICT.Data;
using SBICT.Infrastructure.Chat;
using SBICT.Infrastructure.Connection;
//http://www.tugberkugurlu.com/archive/mapping-asp-net-signalr-connections-to-real-application-users
namespace SBICT.Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : HubBase
    {
        /// <summary>
        /// Store for groups
        /// </summary>
        private static readonly ConnectionStore<Guid> GroupStore = new ConnectionStore<Guid>();

        private static readonly HashSet<Group> GroupList = new HashSet<Group>();


        /// <summary>
        /// Return list of currently connected users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUserList(Guid userId)
        {
            return UserList.Where(u => u.Id != userId);
        }


        /// <summary>
        /// Return a list of groups the user is currently connected to on other clients.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public IEnumerable<Group> GetGroupsForUser(Guid userId)
        {
            var connectedGroups = GroupStore.GetByConnection(userId.ToString());
            return GroupList.Where(g => connectedGroups.Contains(g.Id));
        }

        /// <summary>
        /// Join a group or create if not exists.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public async Task GroupJoin(Group group, Guid userId)
        {
            var userConnections = UserConnectionStore.GetConnections(userId).ToList();
            var user = UserList.Single(u => u.Id == userId);
            foreach (var conId in userConnections)
            {
                await Groups.AddToGroupAsync(conId, group.Name);
            }

            GroupStore.Add(group.Id, userId.ToString());

            if (GroupStore.GetConnections(group.Id).Count() <= 1)
            {
                GroupList.Add(group);
                await Clients.Group(group.Name).SendAsync("GroupCreated", group);
            }
            else
            {
                await Clients.Group(group.Name).SendAsync("GroupJoined", group, user);
            }
        }

        /// <summary>
        /// Invite a client to join a group.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public async Task GroupInvite(Group group, Guid userId)
        {
            var userConnections = UserConnectionStore.GetConnections(userId).ToList();
            var storedGroup = GroupList.Single(g => g.Id == group.Id);
            await Clients.Clients(userConnections).SendAsync("GroupInvited", storedGroup);
        }

        /// <summary>
        /// Leave a group.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public async Task GroupLeave(Group group, Guid userId)
        {
            var userConnections = UserConnectionStore.GetConnections(userId).ToList();
            var user = UserList.Single(u => u.Id == userId);
            foreach (var conId in userConnections)
            {
                await Groups.RemoveFromGroupAsync(conId, group.Name);
            }

            GroupStore.Remove(group.Id, user.Id.ToString());

            await Clients.Group(group.Name).SendAsync("GroupLeft", group, user);

            if (!GroupStore.GetConnections(group.Id).Any())
            {
                GroupList.RemoveWhere(g => g.Id == group.Id);
            }
        }

        /// <summary>
        /// Send message to a user/group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="scope"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        // ReSharper disable once UnusedMember.Global
        public async Task SendMessage(Guid recipient, Guid sender, Message message, ConnectionScope scope)
        {
            IClientProxy target;
            switch (scope)
            {
                case ConnectionScope.Group:
                    target = Clients.GroupExcept(recipient.ToString(),
                        UserConnectionStore.GetConnections(sender).ToList());
                    break;
                case ConnectionScope.User:
                    target = Clients.Clients(UserConnectionStore.GetConnections(recipient).ToList());
                    break;
                default:
                    target = Clients.All;
                    break;
            }


            await target.SendAsync("MessageReceived", recipient, UserList.Single(u => u.Id == sender), message, scope);
        }
    }
}
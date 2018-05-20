﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SBICT.Data;
using SBICT.Infrastructure.Chat;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : HubBase
    {
        #region Fields

        /// <summary>
        /// Store for groups
        /// </summary>
        private static readonly ConnectionStore<Guid> GroupStore = new ConnectionStore<Guid>();

        #endregion

        #region Properties

        /// <summary>
        /// Return list of currently connected users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUserList(Guid userId)
        {
            return UserList.Where(u => u.Id != userId);
        }

        #endregion

        #region Group Methods

        /// <summary>
        /// Join a group or create if not exists
        /// </summary>
        /// <param name="group"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task GroupJoin(Group group, Guid userId)
        {
            var userConnections = UserConnectionStore.GetConnections(userId).ToList();

            foreach (var conId in userConnections)
            {
                await Groups.AddToGroupAsync(conId, group.Name);
            }

            GroupStore.Add(group.Id, userId.ToString());

            if (GroupStore.GetConnections(group.Id).Count(c => c.Contains(userId.ToString())) <= 1)
            {
                await Clients.Group(group.Name).SendAsync("GroupCreated", group);
            }
            else
            {
                await Clients.Group(group.Name).SendAsync("GroupJoined", UserList.First(u => u.Id == userId));
            }
        }
//
//        /// <summary>
//        /// Invite a client to join a group
//        /// </summary>
//        /// <param name="groupName"></param>
//        /// <param name="userName"></param>
//        /// <returns></returns>
//        public async Task GroupInvite(string groupName, string userName)
//        {
//            await Clients.Clients(UserStore.GetConnections(userName).ToList())
//                .SendAsync("GroupInvited", groupName);
//        }
//
//        /// <summary>
//        /// Leave a group
//        /// </summary>
//        /// <param name="groupName"></param>
//        /// <returns></returns>
//        public async Task GroupLeave(string groupName)
//        {
//            var userConnections = UserStore.GetConnections(Context.User.Identity.Name);
//
//            foreach (var conId in userConnections)
//            {
//                await Groups.RemoveFromGroupAsync(conId, groupName);
//            }
//
//            await Clients.Group(groupName).SendAsync("GroupLeft", Context.User.Identity.Name);
//        }

        #endregion

        /// <summary>
        /// Send message to a user/group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="scope"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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


            await target.SendAsync("MessageReceived", recipient, UserList.First(u => u.Id == sender), message, scope);
        }
    }
}
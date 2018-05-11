using System;
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
        #region Fields

        /// <summary>
        /// Store for groups
        /// </summary>
        protected static readonly ConnectionStore<string> GroupUserStore =
            new ConnectionStore<string>();

        #endregion

        #region Properties

        /// <summary>
        /// Return list of currently connected users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetUserList()
        {
#if DEBUG
            return UserStore.GetConnections();
#else
            return ConnectedUsers.GetConnections().Where(u => u != Context.User.Identity.Name); 
#endif
        }

        #endregion

        #region Group Methods

        /// <summary>
        /// Join a group or create if not exists
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task GroupJoin(string groupName)
        {
            var joiner = Context.User.Identity.Name;
            var userConnections = UserStore.GetConnections(joiner).ToList();

            foreach (var conId in userConnections)
            {
                await Groups.AddToGroupAsync(conId, groupName);
            }

            GroupUserStore.Add(groupName, joiner);

            if (GroupUserStore.GetConnections(groupName).Count(c => c.Contains(joiner)) <= 1)
            {
                await Clients.Group(groupName).SendAsync("GroupCreated", groupName);
            }
            else
            {
                await Clients.GroupExcept(groupName, userConnections).SendAsync("GroupJoined", joiner);
            }
        }

        /// <summary>
        /// Invite a client to join a group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task GroupInvite(string groupName, string userName)
        {
            await Clients.Clients(UserStore.GetConnections(userName).ToList())
                .SendAsync("GroupInvited", groupName);
        }

        /// <summary>
        /// Leave a group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task GroupLeave(string groupName)
        {
            var userConnections = UserStore.GetConnections(Context.User.Identity.Name);

            foreach (var conId in userConnections)
            {
                await Groups.RemoveFromGroupAsync(conId, groupName);
            }

            await Clients.Group(groupName).SendAsync("GroupLeft", Context.User.Identity.Name);
        }

        #endregion

        /// <summary>
        /// Send message to a user/group
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        /// <param name="scope"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task SendMessage(string recipient, string message, ConnectionScope scope)
        {
            var sender = Context.User.Identity.Name;
            IClientProxy target;
            switch (scope)
            {
                case ConnectionScope.Group:
                    target = Clients.GroupExcept(recipient,
                        UserStore.GetConnections(Context.User.Identity.Name).ToList());
                    break;
                case ConnectionScope.User:
                    target = Clients.Clients(UserStore.GetConnections(recipient).ToList());
                    break;
                default:
                    target = Clients.All;
                    break;
            }


            await target.SendAsync("MessageReceived", sender, message, scope, recipient);
        }
    }
}
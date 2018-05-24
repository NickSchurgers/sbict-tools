using System;
using System.Collections.ObjectModel;
using SBICT.Data;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Chat
{
    public interface IChatManager
    {
        IConnection Connection { get; set; }
        IChatWindow ActiveChat { get; set; }
        ObservableCollection<IChatChannel> Channels { get; set; }
        ObservableCollection<IUser> ConnectedUsers { get; set; }

        event EventHandler<ChatMessageEventArgs> ChatMessageReceived;
        event EventHandler<BroadcastEventArgs> BroadcastReceived;
        event EventHandler<ChatGroupEventArgs> GroupInviteReceived;

        /// <summary>
        /// Create list of channels and populate them.
        /// </summary>
        void InitChannels();
        
        /// <summary>
        /// Add channel to list of channels.
        /// </summary>
        /// <param name="channel">Channel to add.</param>
        void AddChatChannel(IChatChannel channel);
        
        /// <summary>
        /// Add chatgroup to the groups channel.
        /// </summary>
        /// <param name="chatGroup">Group to add.</param>
        void AddChatGroup(IChatGroup chatGroup);
        
        /// <summary>
        /// Add chat to the users channel.
        /// </summary>
        /// <param name="chat">Chat to add.</param>
        void AddChat(IChat chat);
        
        /// <summary>
        /// Activate a chat(group) by navigating to the chat window.
        /// </summary>
        /// <param name="window">Window to activate.</param>
        void ActivateWindow(IChatWindow window);
        
        /// <summary>
        /// Join group on server.
        /// </summary>
        /// <param name="group">Group to join.</param>
        void JoinChatGroup(IChatGroup group);

        /// <summary>
        /// Send invite request for the specified group to the specified user.
        /// </summary>
        /// <param name="group">Group to invite to.</param>
        /// <param name="userId">User to invite.</param>
        void InviteChatGroup(IChatGroup group, Guid userId);
        
        /// <summary>
        /// Remove chat from user channel.
        /// </summary>
        /// <param name="chat">Chat to remove.</param>
        void RemoveChat(IChat chat);
               
        /// <summary>
        /// Remove chat group from groups channel.
        /// </summary>
        /// <param name="chatGroup">Group to remove.</param>
        void RemoveChatGroup(IChatGroup chatGroup);
        
        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="recipient">Target ot the message.</param>
        /// <param name="message">Content of the message.</param>
        /// <param name="scope">Scope of the message.</param>
        void SendMessage(Guid recipient, string message, ConnectionScope scope);
        
    }
}
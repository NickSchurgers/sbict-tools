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

        void InitChannels();
        void AddChatChannel(IChatChannel channel);
        void AddChatGroup(IChatGroup chatGroup);
        void AddChat(IChat chat);
        void ActivateWindow(IChatWindow chat);
        void JoinChatGroup(IChatGroup chatGroup);
        void RemoveChat(IChat chat);
        void RemoveChatGroup(IChatGroup chatGroup);
        void SendMessage(Guid recipient, string message, ConnectionScope scope);
        void InviteChatGroup(IChatGroup group, Guid userId);
    }
}
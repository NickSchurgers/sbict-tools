using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public interface IChatManager
    {
        IConnection Connection { get; set; }
        IChatWindow ActiveChat { get; set; }
        ObservableCollection<ChatChannel> Channels { get; set; }

        void InitChannels();
        void AddChatChannel(ChatChannel channel);
        void AddChatGroup(ChatGroup group);
        void AddChat(Chat chat);
        void ActivateChat(Chat chat);
        void ActivateChatGroup(ChatGroup chatGroup);
        void JoinChatGroup(ChatGroup group);
        void RemoveChat(Chat chat);
        void RemoveChatGroup(ChatGroup chatGroup);
        void SendMessage(Guid recipient, string message, ConnectionScope scope);
    }
}
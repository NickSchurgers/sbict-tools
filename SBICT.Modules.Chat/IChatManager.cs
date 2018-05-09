using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public interface IChatManager
    {
        IConnection Connection { get; set; }
        Chat ActiveChat { get; set; }
        ChatGroup ActiveGroup { get; set; }
        ObservableCollection<ChatChannel> Channels { get; set; }

        Task<ObservableCollection<ChatChannel>> RefreshChannels();
        void AddChatChannel(ChatChannel channel);
        void AddChatGroup(ChatGroup group, ChatChannel channel = null);
        void AddChat(Chat chat);
        void ActivateChat(Chat chat);
        void ActivateChatGroup(ChatGroup group);
        void RemoveChat(Chat chat);
        void RemoveChatGroup(ChatGroup chatGroup);
        void SendMessage(string recipient, string message, ConnectionScope scope);
    }
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public interface IChatManager
    {
        IConnection Connection { get; set; }
        Chat ActiveChat { get; set; }
        ChatChannel UserChannel { get; set; }
        ChatChannel GroupChannel { get; set; }
        ObservableCollection<ChatChannel> Channels { get; set; }
        
        Task<ObservableCollection<ChatChannel>> InitHub();
        void DeinitHub();
        Task<ObservableCollection<ChatChannel>> RefreshChannels();
        void SendMessage(string recipient, string message, ConnectionScope scope);
        
    }
}
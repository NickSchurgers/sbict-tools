using System.Collections;
using System.Collections.ObjectModel;

namespace SBICT.Infrastructure.Chat
{
    public interface IChatChannel
    {
        string Name { get; }
        bool IsExpanded { get; set; }
        ObservableCollection<IChat> Chats { get; } 
        ObservableCollection<IChatGroup> ChatGroups { get; } 
        IList Items { get; }
    }
}
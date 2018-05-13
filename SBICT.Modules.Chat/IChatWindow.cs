using System;
using System.Collections.ObjectModel;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat
{
    public interface IChatWindow
    {
        string Title { get; set; }
        Guid Id { get; }
        bool IsActive { get; set; }
        ObservableCollection<ChatMessage> ChatMessages { get; set; }
    }
}
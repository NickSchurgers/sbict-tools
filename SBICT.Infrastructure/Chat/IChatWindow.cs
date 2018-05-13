using System;
using System.Collections.ObjectModel;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure.Chat
{
    public interface IChatWindow
    {
        string Title { get; set; }
        bool IsActive { get; set; }
        ConnectionScope Scope { get; }
        ObservableCollection<IChatMessage> Messages { get; }
        Guid GetRecipient();
    }
}
using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using SBICT.Infrastructure.Chat;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public abstract class ChatBase : BindableBase, IChatWindow
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public ConnectionScope Scope { get; }
        public ObservableCollection<IChatMessage> Messages { get; }


        protected ChatBase(ConnectionScope scope)
        {
            Scope = scope;
            Messages = new ObservableCollection<IChatMessage>();
        }

        public abstract Guid GetRecipient();
    }
}
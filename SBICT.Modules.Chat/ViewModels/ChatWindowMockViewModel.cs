using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowMockViewModel : BindableBase
    {
        public string Header { get; } = "Chat";
        public Chat Chat { get; set; }

        public ChatWindowMockViewModel()
        {
            Chat = new Chat
            {
                Name = "Henk",
                ChatMessages = new ObservableCollection<ChatMessage>
                {
                    new ChatMessage
                    {
                        Message = "Test",
                        Received = DateTime.Now,
                        Sender = "Henk"
                    }
                }
            };
        }
    }
}
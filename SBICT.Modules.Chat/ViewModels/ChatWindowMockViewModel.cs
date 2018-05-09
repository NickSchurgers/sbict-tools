using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SBICT.Infrastructure;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowMockViewModel : BindableBase
    {
        #region Fields

        private string _recipient;
        private string _message;
        private ObservableCollection<ChatMessage> _chatMessages;

        #endregion

        #region Properties

        public string Header { get; } = "Chat";
        public DelegateCommand SendMessage { get; set; }

        public string Recipient
        {
            get => _recipient;
            set => SetProperty(ref _recipient, value);
        }

        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        #endregion

        public ChatWindowMockViewModel()
        {
            Recipient = "Henk";

            ChatMessages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage
                {
                    Message = "Test",
                    Received = DateTime.Now,
                    Sender = "Henk"
                }
            };
        }
    }
}
using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using SBICT.Infrastructure;

namespace SBICT.Modules.Chat
{
    public class ChatGroup : BindableBase
    {
        private string _name;
        private ObservableCollection<ChatMessage> _chatMessages;

        public Guid Id { get; }

        /// <summary>
        /// Name of this chat
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        public bool IsOpen { get; set; }

        public ObservableCollection<string> Participants { get; set; }

        public ChatGroup()
        {
            Participants = new ObservableCollection<string>();
            ChatMessages = new ObservableCollection<ChatMessage>();
            Id = Guid.NewGuid();
        }
    }
}
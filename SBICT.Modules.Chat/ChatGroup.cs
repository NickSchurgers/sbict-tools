using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using SBICT.Infrastructure;
using SBICT.Data;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat
{
    public class ChatGroup : BindableBase, IChatWindow, IGroup
    {
        private string _title;
        private ObservableCollection<ChatMessage> _chatMessages;


        public Guid Id { get; }

        /// <summary>
        /// Name of this chat group
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        public bool IsActive { get; set; }

        public ObservableCollection<User> Participants { get; set; }

        public ChatGroup()
        {
            Participants = new ObservableCollection<User>();
            ChatMessages = new ObservableCollection<ChatMessage>();
            Id = Guid.NewGuid();
        }
    }
}
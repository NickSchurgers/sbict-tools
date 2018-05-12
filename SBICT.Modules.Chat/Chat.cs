using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Events;
using Prism.Mvvm;
using SBICT.Data;
using SBICT.Infrastructure;

namespace SBICT.Modules.Chat
{
    public class Chat : BindableBase, IChatWindow
    {
        private ObservableCollection<ChatMessage> _chatMessages;
        private string _title;

        #region Properties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public Guid Id { get; }

        public User User { get; set; }

        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        public bool IsActive { get; set; }

        #endregion

        public Chat()
        {
            ChatMessages = new ObservableCollection<ChatMessage>();
            Id = Guid.NewGuid();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Events;
using Prism.Mvvm;
using SBICT.Data;
using SBICT.Infrastructure;

namespace SBICT.Modules.Chat
{
    public class Chat : BindableBase
    {
        private string _name;
        private ObservableCollection<ChatMessage> _chatMessages;

        #region Properties

        public Guid Id { get; }

        /// <summary>
        /// Name of this chat
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public User User { get; set; }

        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        public bool IsOpen { get; set; }

        #endregion

        public Chat()
        {
            ChatMessages = new ObservableCollection<ChatMessage>();
            Id = Guid.NewGuid();
        }
    }
}
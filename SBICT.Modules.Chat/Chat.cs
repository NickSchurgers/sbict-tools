using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class Chat : BindableBase
    {
        private string _name;
        private ObservableCollection<ChatMessage> _chatMessages = new ObservableCollection<ChatMessage>();

        #region Properties

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

        #endregion
    }
}
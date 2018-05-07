using System;
using System.Collections.Generic;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class Chat : BindableBase
    {
        private string _name;
        private List<ChatMessage> _chatMessages;

        #region Properties

        /// <summary>
        /// Name of this chat
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public List<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        #endregion
    }
}
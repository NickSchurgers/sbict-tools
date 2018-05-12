using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Regions;
using SBICT.Infrastructure.Connection;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using SBICT.Infrastructure;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowViewModel : BindableBase, INavigationAware
    {
        #region Fields

        private readonly IChatManager _chatManager;
        private string _title;
        private string _message;
        private ObservableCollection<ChatMessage> _chatMessages;
        private ObservableCollection<string> _participants;

        #endregion

        #region Properties

        public string Header { get; } = "Chat";
        public DelegateCommand SendMessage { get; set; }

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

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ObservableCollection<string> Participants
        {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        public Guid Recipient { get; set; }

        #endregion

        #region Methods

        public ChatWindowViewModel(IChatManager chatManager)
        {
            _chatManager = chatManager;
            SendMessage = new DelegateCommand(OnMessageSent);
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        #endregion

        #region Event Handlers 

        private void OnMessageSent()
        {
            //As a chatgroup is cast to a chat, we use participants to determine what the scope is
            var scope = Participants != null ? ConnectionScope.Group : ConnectionScope.User;
            _chatManager.SendMessage(Recipient, Message, scope);
            ChatMessages.Add(new ChatMessage {Message = Message, Received = DateTime.Now});
            Message = string.Empty;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Chat"))
            {
                navigationContext.Parameters.TryGetValue("Chat", out Chat chat);
                ChatMessages = chat.ChatMessages;
                Title = chat.User.DisplayName;
                Recipient = chat.User.Id;
            }
            else if (navigationContext.Parameters.ContainsKey("ChatGroup"))
            {
                navigationContext.Parameters.TryGetValue("ChatGroup", out ChatGroup chatGroup);
                Participants = chatGroup.Participants;
                ChatMessages = chatGroup.ChatMessages;
                Title = chatGroup.Name;
                Recipient = chatGroup.Id;
            }
        }


        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //Reset participants in case we switch from a group to a single user chat
            Participants = null;
        }

        #endregion
    }
}
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
        private string _name;
        private string _message;
        private ObservableCollection<ChatMessage> _chatMessages;
        private ObservableCollection<string> _participants;

        #endregion

        #region Properties

        public string Header { get; } = "Chat";
        public DelegateCommand SendMessage { get; set; }

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
            _chatManager.SendMessage(Name, Message, ConnectionScope.User);
            ChatMessages.Add(new ChatMessage {Message = Message});
            Message = string.Empty;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            navigationContext.Parameters.TryGetValue("Chat", out Chat chat);

            if (chat == null)
            {
                navigationContext.Parameters.TryGetValue("ChatGroup", out ChatGroup chatGroup);
                Participants = chatGroup.Participants;
                chat = chatGroup;
            }

            ChatMessages = chat.ChatMessages;
            Name = chat.Name;
        }


        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
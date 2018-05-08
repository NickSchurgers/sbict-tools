using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        private Chat _chat;
        private string _message;

        #endregion

        #region Properties

        public string Header { get; } = "Chat";
        public DelegateCommand SendMessage { get; set; }

        public Chat Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        #endregion

        #region Methods

        public ChatWindowViewModel(IChatManager chatManager, IEventAggregator aggregator)
        {
            _chatManager = chatManager;

            aggregator.GetEvent<ChatMessageReceivedEvent>()
                .Subscribe(OnMessageReceived, ThreadOption.UIThread, false, message => message.Sender == Chat.Name);

            SendMessage = new DelegateCommand(OnMessageSent);
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        #endregion

        #region Event Handlers 

        private void OnMessageReceived(ChatMessage chatMessage)
        {
            Chat.ChatMessages.Add(chatMessage);
        }

        private void OnMessageSent()
        {
            Chat.ChatMessages.Add(new ChatMessage {Message = Message});
            _chatManager.SendMessage(Chat.Name, Message, ConnectionScope.User);
            Message = string.Empty;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Chat = navigationContext.Parameters["Chat"] as Chat;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
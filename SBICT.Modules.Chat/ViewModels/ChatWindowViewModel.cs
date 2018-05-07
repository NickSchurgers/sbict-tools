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

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowViewModel : BindableBase, INavigationAware
    {
        private readonly IChatManager _chatManager;
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand SendMessage { get; set; }

        private Chat _chat;

        public string Header { get; } = "Chat";

        public Chat Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public string Message { get; set; }

        public ChatWindowViewModel(IChatManager chatManager)
        {
            _chatManager = chatManager;
            SendMessage = new DelegateCommand(OnMessageSent);
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
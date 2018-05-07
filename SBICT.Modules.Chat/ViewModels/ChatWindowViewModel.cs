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
        private readonly IChatManager _chatManager;
        private readonly IEventAggregator _aggregator;
        public DelegateCommand SendMessage { get; set; }

        private Chat _chat;

        public string Header { get; } = "Chat";

        public Chat Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public string Message { get; set; }

        public ChatWindowViewModel(IChatManager chatManager, IEventAggregator aggregator)
        {
            _chatManager = chatManager;
            _aggregator = aggregator;
            _aggregator.GetEvent<ChatMessageReceivedEvent>()
                .Subscribe(OnMessageReceived, ThreadOption.UIThread, false, message => message.Sender == Chat.Name);
            SendMessage = new DelegateCommand(OnMessageSent);
        }

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
            if (Chat != null)
            {
                Chat.IsOpen = true;
            }
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
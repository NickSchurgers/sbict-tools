using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Prism.Regions;
using SBICT.Infrastructure.Connection;
using Microsoft.AspNetCore.SignalR.Client;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand SendMessage { get; set; }

        private Chat _chat;
        private readonly IConnection _chatConnection;

        public string Header { get; } = "Chat";

        public Chat Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }


        public ChatWindowViewModel(IConnectionManager<IConnection> connectionManager)
        {
            SendMessage = new DelegateCommand(OnMessageSent);
            _chatConnection = connectionManager.Get("Chat");
        }

        private async void OnMessageSent()
        {
            Chat.ChatMessages.Add(new ChatMessage {Message = "Test"});
            await _chatConnection.Hub.InvokeAsync("SendMessage", Chat.Name, "Message", ConnectionScope.User);
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
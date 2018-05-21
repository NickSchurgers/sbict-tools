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
using SBICT.Data;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowViewModel : BindableBase, INavigationAware
    {
        #region Fields

        private readonly IChatManager _chatManager;
        private readonly ISettingsManager _settingsManager;
        private string _message;
        private IChatWindow _chatWindow;
        private ObservableCollection<IUser> _participants;

        #endregion

        #region Properties

        public string Header { get; } = "Chat";
        public DelegateCommand SendMessage { get; set; }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ObservableCollection<IUser> Participants
        {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        public IChatWindow ChatWindow
        {
            get => _chatWindow;
            set => SetProperty(ref _chatWindow, value);
        }

        #endregion

        #region Methods

        public ChatWindowViewModel(IChatManager chatManager, ISettingsManager settingsManager)
        {
            _chatManager = chatManager;
            _settingsManager = settingsManager;
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
            if (ChatWindow == null) return;
            
            //As a chatgroup is cast to a chat, we use participants to determine what the scope is
            _chatManager.SendMessage(ChatWindow.GetRecipient(), Message, ChatWindow.Scope);
            ChatWindow.Messages.Add(new ChatMessage(Message, DateTime.Now) {Sender = _settingsManager.User});
            Message = string.Empty;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            navigationContext.Parameters.TryGetValue("Chat", out IChatWindow chat);
            ChatWindow = chat;
            if (chat is IChatGroup group)
            {
                Participants = group.Participants;
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
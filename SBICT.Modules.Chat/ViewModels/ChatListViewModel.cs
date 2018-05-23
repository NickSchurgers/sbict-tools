using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Chat;
using SBICT.Infrastructure.Connection;
using SBICT.Infrastructure.Extensions;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatListViewModel : BindableBase
    {
        #region Commands

        public DelegateCommand<object> ChatListSelectedItemChanged { get; set; }
        public DelegateCommand ChatListAddGroup { get; set; }

        #endregion

        #region Fields

        private readonly IChatManager _chatManager;
        private ObservableCollection<IChatChannel> _channels = new ObservableCollection<IChatChannel>();

        #endregion

        #region Properties

        /// <summary>
        /// Collection of chatgroups used as root node in the treeview
        /// </summary>
        public ObservableCollection<IChatChannel> Channels
        {
            get => _channels;
            set => SetProperty(ref _channels, value);
        }

        public InteractionRequest<GroupInviteCreateNotification> GroupCreateRequest { get; private set; }
        public InteractionRequest<IConfirmation> ConfirmInviteRequest { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public ChatListViewModel(IChatManager chatManager)
        {
            _chatManager = chatManager;
            _chatManager.InitChannels();
            _chatManager.GroupInviteReceived += OnGroupInviteReceived;

            ChatListSelectedItemChanged = new DelegateCommand<object>(OnSelectedItemChanged);
            ChatListAddGroup = new DelegateCommand(OnChatListAddGroup);

            Channels = _chatManager.Channels;
            GroupCreateRequest = new InteractionRequest<GroupInviteCreateNotification>();
            ConfirmInviteRequest = new InteractionRequest<IConfirmation>();
        }

        #endregion

        #region Event Handlers

        private void OnSelectedItemChanged(object obj)
        {
            switch (obj)
            {
                case Chat chat:
                    _chatManager.ActivateWindow(chat);
                    break;
                case ChatGroup group:
                    _chatManager.ActivateWindow(group);
                    break;
            }
        }

        private void OnChatListAddGroup()
        {
            var notification = new GroupInviteCreateNotification(_chatManager.ConnectedUsers) {Title = "Items"};
            GroupCreateRequest.Raise(notification, result =>
            {
                if (result == null || !result.Confirmed || result.GroupName == null) return;
                var group = new ChatGroup(result.GroupName);
                _chatManager.JoinChatGroup(group);
                foreach (var user in result.SelectedItems)
                {
                    _chatManager.InviteChatGroup(group, user.Id);
                }
            });
        }

        private void OnGroupInviteReceived(object sender, ChatGroupEventArgs e)
        {
            var confirm = new Confirmation
            {
                Title = "Group Invitation",
                Content = $"You have been invited to {e.ChatGroup.Name}"
            };

            ConfirmInviteRequest.Raise(confirm, result =>
            {
                if (result != null && result.Confirmed)
                {
                    _chatManager.JoinChatGroup(e.ChatGroup);
                }
            });
        }

        #endregion
    }
}
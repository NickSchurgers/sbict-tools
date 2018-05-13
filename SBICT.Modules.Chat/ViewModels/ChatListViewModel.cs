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
        public DelegateCommand<object> ChatListAddGroup { get; set; }

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

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public ChatListViewModel(IChatManager chatManager)
        {
            _chatManager = chatManager;
            _chatManager.InitChannels();

            ChatListSelectedItemChanged = new DelegateCommand<object>(OnSelectedItemChanged);
            ChatListAddGroup = new DelegateCommand<object>(OnChatListAddGroup);

            Channels = _chatManager.Channels;
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

        private void OnChatListAddGroup(object obj)
        {
            _chatManager.JoinChatGroup(new ChatGroup("Henkies"));
        }

        #endregion
    }
}
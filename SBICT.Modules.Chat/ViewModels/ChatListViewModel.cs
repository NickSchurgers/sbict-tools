using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using Prism.Regions;
using SBICT.Infrastructure;
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
        private ObservableCollection<ChatChannel> _channels = new ObservableCollection<ChatChannel>();

        #endregion

        #region Properties

        /// <summary>
        /// Collection of chatgroups used as root node in the treeview
        /// </summary>
        public ObservableCollection<ChatChannel> Channels
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

            ChatListSelectedItemChanged = new DelegateCommand<object>(OnSelectedItemChanged);
            ChatListAddGroup = new DelegateCommand<object>(OnChatListAddGroup);
            _chatManager.Connection.UserStatusChanged += OnUserStatusChanged;
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Closing += OnMainWindowClosing;

            InitChannels();
        }

        private void OnChatListAddGroup(object obj)
        {
            throw new NotImplementedException();
        }

        private async void InitChannels()
        {
            Channels = await _chatManager.InitHub();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Triggered when a user (dis)connects from the chat hub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void OnUserStatusChanged(object sender, ConnectionEventArgs e)
        {
            switch (e.Status)
            {
                case ConnectionStatus.Connected:
#if DEBUG
                    _chatManager.AddChat(new Chat {Name = "Henk"});
#else
                     _chatManager.AddChat(new Chat{Name = e.User});
#endif
                    Channels = _chatManager.Channels;
                    break;
                case ConnectionStatus.Disconnected:
#if DEBUG
                    _chatManager.RemoveChat(new Chat {Name = "Henk"});
#else
                   _chatManager.RemoveChat(new Chat {Name = e.User});
#endif
                    Channels = _chatManager.Channels;
                    break;
                case ConnectionStatus.Connecting:
                case ConnectionStatus.Reconnecting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void OnSelectedItemChanged(object obj)
        {
            if (obj.GetType() == typeof(Chat))
            {
                _chatManager.ActivateChat((Chat) obj);
            }
        }

        /// <summary>
        /// Triggered on closing of the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            _chatManager.DeinitHub();
        }

        #endregion
    }
}
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
        private readonly IRegionManager _regionManager;
        private readonly IChatManager _chatManager;

        #region Commands

        public DelegateCommand<object> ChatListSelectedItemChanged { get; set; }

        #endregion

        #region Fields

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
        public ChatListViewModel(IRegionManager regionManager, IChatManager chatManager)
        {
            _regionManager = regionManager;
            _chatManager = chatManager;

            ChatListSelectedItemChanged = new DelegateCommand<object>(OnSelectedItemChanged);
            _chatManager.Connection.UserStatusChanged += ChatConnectionOnUserStatusChanged;
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Closing += MainWindowOnClosing;

            InitChannels();
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
        private async void ChatConnectionOnUserStatusChanged(object sender, ConnectionEventArgs e)
        {
            var message = $"{e.User} has ";
            switch (e.Status)
            {
                case ConnectionStatus.Connected:
                    SystemLogger.LogEvent($"{message} joined");
#if DEBUG
                    _chatManager.UserChannel.Chats.Add(new Chat {Name = "Henk"});
#else
                     _chatManager.UserChannel.Chats.Add(new Chat{Name = e.User});
#endif
                    Channels = await _chatManager.RefreshChannels();
                    break;
                case ConnectionStatus.Disconnected:
                    SystemLogger.LogEvent($"{message} left");
#if DEBUG
                    _chatManager.UserChannel.Chats.RemoveAll(c => c.Name == "Henk");
#else
                  / _chatManager.UserChannel.Chats.RemoveAll(c => c.Name == e.User);
#endif
                    Channels = await _chatManager.RefreshChannels();
                    break;
                case ConnectionStatus.Connecting:
                case ConnectionStatus.Reconnecting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Triggered on closing of the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowOnClosing(object sender, CancelEventArgs e)
        {
            _chatManager.DeinitHub();
        }

        private void OnSelectedItemChanged(object obj)
        {
            if (obj.GetType() == typeof(Chat))
            {
                var param = new NavigationParameters {{"Chat", (Chat) obj}};
                _regionManager.RequestNavigate(RegionNames.MainRegion, new Uri("ChatWindow", UriKind.Relative), param);
            }
        }

        #endregion
    }
}
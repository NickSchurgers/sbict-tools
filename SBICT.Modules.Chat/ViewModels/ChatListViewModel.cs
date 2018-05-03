using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatListViewModel : BindableBase
    {
        #region Fields

        private ObservableCollection<ChatGroup> _chatGroups = new ObservableCollection<ChatGroup>();
        private Connection _chatConnection;

        #endregion

        #region Properties

        /// <summary>
        /// Collection of chatgroups used as root node in the treeview
        /// </summary>
        public ObservableCollection<ChatGroup> ChatGroups
        {
            get => _chatGroups;
            set => SetProperty(ref _chatGroups, value);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public ChatListViewModel()
        {
            InitializeChatHub();
            CreateChatList();
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Closing += MainWindowOnClosing;
        }

        /// <summary>
        /// Create list of root nodes and populate the users node with a list off active users
        /// </summary>
        private async void CreateChatList()
        {
            var users = await _chatConnection.Hub.InvokeAsync<IEnumerable<string>>("GetUserList");
            ChatGroups.Add(new ChatGroup {Name = "Users", Chats = CreateUserList(users)});
            ChatGroups.Add(new ChatGroup {Name = "Groups"});
            ChatGroups.Add(new ChatGroup {Name = "Projects"});
        }

        /// <summary>
        /// Initiate the connection with the chat hub
        /// </summary>
        private async void InitializeChatHub()
        {
            _chatConnection = ConnectionFactory.Create("http://localhost:5000/hubs/chat");
            _chatConnection.UserStatusChanged += ChatConnectionOnUserStatusChanged;
            await _chatConnection.StartAsync();
        }

        /// <summary>
        /// Dispose of the chat hub connection
        /// </summary>
        private async void DeInitializeChatHub()
        {
            await _chatConnection.StopAsync();
        }

        /// <summary>
        /// Parse the list of strings returned by the chat hub getuserlist action, to a list of chats
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private ObservableCollection<Chat> CreateUserList(IEnumerable<string> users)
        {
            return new ObservableCollection<Chat>(users.Select(u => new Chat {Name = u}));
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Triggered when a user (dis)connects from the chat hub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ChatConnectionOnUserStatusChanged(object sender, ConnectionEventArgs e)
        {
            var message = $"{e.User} has ";
            switch (e.Status)
            {
                case ConnectionStatus.Connected:
                    SystemLogger.LogEvent($"{message} joined");
                    break;
                case ConnectionStatus.Disconnected:
                    SystemLogger.LogEvent($"{message} left");
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
            DeInitializeChatHub();
        }

        #endregion
    }
}
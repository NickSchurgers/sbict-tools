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
    public class ChatWindowViewModel : BindableBase
    {
        private ObservableCollection<ChatGroup> _chatGroups = new ObservableCollection<ChatGroup>();
        private Connection _chatConnection;

        public ObservableCollection<ChatGroup> ChatGroups
        {
            get => _chatGroups;
            set => SetProperty(ref _chatGroups, value);
        }

        public ChatWindowViewModel()
        {
            InitializeChatHub();
            CreateChatList();
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Closing += MainWindowOnClosing;
        }

        private async void CreateChatList()
        {
            var users = await _chatConnection.Hub.InvokeAsync<IEnumerable<string>>("GetUserList");
            _chatGroups.Add(new ChatGroup {Name = "Users", Chats = CreateUserList(users)});
            _chatGroups.Add(new ChatGroup {Name = "Groups"});
            _chatGroups.Add(new ChatGroup {Name = "Projects"});
        }


        private async void InitializeChatHub()
        {
            _chatConnection = ConnectionFactory.Create("http://localhost:5000/hubs/chat");
            _chatConnection.UserStatusChanged += ChatConnectionOnUserStatusChanged;
            await _chatConnection.StartAsync();
        }


        private async void DeInitializeChatHub()
        {
            await _chatConnection.StopAsync();
        }


        /// <summary>
        /// Triggered when a user (dis)connects from the suystem hub
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


        private ObservableCollection<Chat> CreateUserList(IEnumerable<string> users)
        {
            return new ObservableCollection<Chat>(users.Select(u => new Chat {Name = u}));
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs e)
        {
            DeInitializeChatHub();
        }
    }
}
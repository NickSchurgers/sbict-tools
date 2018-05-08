using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using Prism.Mvvm;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public class ChatManager : BindableBase, IChatManager
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionManager<IConnection> _connectionManager;
        private ChatChannel _userChannel = new ChatChannel {Name = "Users"};
        private ChatChannel _groupChannel = new ChatChannel {Name = "Groups"};

        #endregion

        #region Properties

        public IConnection Connection { get; set; }
        public Chat ActiveChat { get; set; }
        public ObservableCollection<ChatChannel> Channels { get; set; } = new ObservableCollection<ChatChannel>();

        public ChatChannel UserChannel
        {
            get => _userChannel;
            set => SetProperty(ref _userChannel, value);
        }

        public ChatChannel GroupChannel
        {
            get => _groupChannel;
            set => SetProperty(ref _groupChannel, value);
        }

        #endregion

        public ChatManager(IEventAggregator eventAggregator, IConnectionManager<IConnection> connectionManager)
        {
            _eventAggregator = eventAggregator;
            _connectionManager = connectionManager;
            Connection = ConnectionFactory.Create("http://localhost:5000/hubs/chat");
        }

        /// <summary>
        /// Initiate the connection with the chat hub
        /// </summary>
        public async Task<ObservableCollection<ChatChannel>> InitHub()
        {
            Connection.Hub.On<string, string, ConnectionScope>("MessageReceived", OnMessageReceived);
            _connectionManager.Set("Chat", Connection);
            await Connection.StartAsync();
            return await RefreshChannels();
        }

        /// <summary>
        /// Dispose of the chat hub connection
        /// </summary>
        public async void DeinitHub()
        {
            _connectionManager.Unset("Chat");
            await Connection.StopAsync();
        }

        /// <summary>
        /// Create list of root nodes and populate the users node with a list off active users
        /// </summary>
        public async Task<ObservableCollection<ChatChannel>> RefreshChannels()
        {
            if (UserChannel.Chats.Count == 0)
            {
                var users = await Connection.Hub.InvokeAsync<IEnumerable<string>>("GetUserList");
                var chats = new ObservableCollection<Chat>(users.Select(u =>
                    new Chat {Name = u}));
                UserChannel.Chats = chats;
                UserChannel.IsExpanded = true;
            }

            return Channels = new ObservableCollection<ChatChannel>
            {
                UserChannel,
                GroupChannel
            };
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        /// <param name="scope"></param>
        public async void SendMessage(string recipient, string message, ConnectionScope scope)
        {
            await Connection.Hub.InvokeAsync("SendMessage", recipient, message, scope);
        }

        #region Event Handlers

        private void OnMessageReceived(string sender, string message, ConnectionScope scope)
        {
            var newMessage = new ChatMessage
            {
                Sender = sender,
                Scope = scope,
                Message = message,
                Received = DateTime.Now
            };

            switch (scope)
            {
                case ConnectionScope.System:
                    break;
                case ConnectionScope.User:
                    UserMessageReceived(newMessage);
                    break;
                case ConnectionScope.Group:
                    GroupMessageReceived(newMessage);
                    break;
                case ConnectionScope.Broadcast:
                    BroadcastReceived(newMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }

            _eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(newMessage);
        }


        private void BroadcastReceived(ChatMessage newMessage)
        {
            throw new NotImplementedException();
        }

        private void GroupMessageReceived(ChatMessage newMessage)
        {
            throw new NotImplementedException();
        }

        private void UserMessageReceived(ChatMessage newMessage)
        {
            //Active chat is handled by the window to avoid UI refreshing issues
            if (newMessage.Sender != ActiveChat.Name)
            {
                UserChannel.Chats.Single(c => c.Name == newMessage.Sender).ChatMessages.Add(newMessage);
            }
        }

        #endregion
    }
}
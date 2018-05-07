using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        #endregion

        #region Properties

        public IConnection Connection { get; set; }

        public ChatChannel UserChannel
        {
            get => _userChannel;
            set => SetProperty(ref _userChannel, value);
        }

        public ChatChannel GroupChannel { get; set; } = new ChatChannel {Name = "Groups"};
        public ObservableCollection<ChatChannel> Channels { get; set; } = new ObservableCollection<ChatChannel>();

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
                UserChannel.Chats = new ObservableCollection<Chat>(users.Select(u => new Chat {Name = u}));
                UserChannel.IsExpanded = true;
            }

            return Channels = new ObservableCollection<ChatChannel>
            {
                UserChannel,
                GroupChannel
            };
        }

        public async void SendMessage(string recipient, string message, ConnectionScope scope)
        {
            await Connection.Hub.InvokeAsync("SendMessage", recipient, message, scope);
        }

        private void OnMessageReceived(string sender, string message, ConnectionScope scope)
        {
            if (scope == ConnectionScope.User)
            {
                _eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(new ChatMessage
                {
                    Sender = sender,
                    Scope = scope,
                    Message = message,
                    Received = DateTime.Now
                });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Connection;
using SBICT.Infrastructure.Extensions;
using SBICT.Infrastructure.Logger;

namespace SBICT.Modules.Chat
{
    public class ChatManager : BindableBase, IChatManager
    {
        #region Fields

        private readonly SynchronizationContext _uiContext = SynchronizationContext.Current;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionManager<IConnection> _connectionManager;
        private readonly IRegionManager _regionManager;

        #endregion

        #region Properties

        public IConnection Connection { get; set; }
        public Chat ActiveChat { get; set; }
        public ObservableCollection<ChatChannel> Channels { get; set; } = new ObservableCollection<ChatChannel>();

        #endregion

        public ChatManager(IEventAggregator eventAggregator, IConnectionManager<IConnection> connectionManager,
            IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _connectionManager = connectionManager;
            _regionManager = regionManager;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Closing += OnMainWindowClosing;

            InitHub();
        }

        /// <summary>
        /// Initiate the connection with the chat hub
        /// </summary>
        private async void InitHub()
        {
            Connection = ConnectionFactory.Create("http://localhost:5000/hubs/chat");
            Connection.UserStatusChanged += OnUserStatusChanged;
            Connection.Hub.On<string, string, ConnectionScope, string>("MessageReceived", OnMessageReceived);
            Connection.Hub.On<string>("GroupCreated", OnGroupCreated);

            _connectionManager.Set("Chat", Connection);
            await Connection.StartAsync();
        }


        /// <summary>
        /// Dispose of the chat hub connection
        /// </summary>
        private async void DeinitHub()
        {
            _connectionManager.Unset("Chat");
            await Connection.StopAsync();
        }

        /// <summary>
        /// Create list of root nodes and populate the users node with a list off active users
        /// </summary>
        public async void InitChannels()
        {
            AddChatChannel(new ChatChannel {Name = "Users", IsExpanded = true});

            var users = await Connection.Hub.InvokeAsync<IEnumerable<string>>("GetUserList");
            users.ToList().ForEach(u => AddChat(new Chat {Name = u}));

            AddChatChannel(new ChatChannel {Name = "Groups", IsExpanded = true});
        }

        public void AddChatChannel(ChatChannel channel)
        {
            Channels.Add(channel);
            SystemLogger.LogEvent($"Channel \"{channel.Name}\" was added", LogLevel.Debug);
        }

        public void AddChatGroup(ChatGroup group)
        {
            GetGroupChannel().ChatGroups.Add(group);
            SystemLogger.LogEvent($"{group.Name} was created");
        }

        public void AddChat(Chat chat)
        {
            GetUserChannel().Chats.Add(chat);
            SystemLogger.LogEvent($"{chat.Name} has joined");
        }

        private void Activate(Chat chat, NavigationParameters parameters)
        {
            if (ActiveChat != null)
            {
                ActiveChat.IsOpen = false;
            }

            chat.IsOpen = true;
            ActiveChat = chat;
            _regionManager.RequestNavigate(RegionNames.MainRegion, new Uri("ChatWindow", UriKind.Relative), parameters);
        }

        public void ActivateChat(Chat chat)
        {
            Activate(chat, new NavigationParameters {{"Chat", chat}});
        }

        public void ActivateChatGroup(ChatGroup chatGroup)
        {
            Activate(chatGroup, new NavigationParameters {{"ChatGroup", chatGroup}});
        }

        public void RemoveChat(Chat chat)
        {
            GetUserChannel().Chats.RemoveAll(c => c.Name == chat.Name);
            SystemLogger.LogEvent($"{chat.Name} has left");
        }

        public void RemoveChatGroup(ChatGroup chatGroup)
        {
            throw new NotImplementedException();
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

        public async void JoinChatGroup(ChatGroup group)
        {
            await Connection.Hub.InvokeAsync("GroupJoin", group.Name);
        }

        private ChatChannel GetUserChannel()
        {
            return Channels.Single(c => c.Name == "Users");
        }

        private ChatChannel GetGroupChannel()
        {
            return Channels.Single(c => c.Name == "Groups");
        }


        #region Event Handlers

        private void OnMessageReceived(string sender, string message, ConnectionScope scope, string recipient)
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
                    OnUserMessageReceived(newMessage);
                    break;
                case ConnectionScope.Group:
                    OnGroupMessageReceived(newMessage, recipient);
                    break;
                case ConnectionScope.Broadcast:
                    OnBroadcastReceived(newMessage);
                    break;
            }

            _eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(newMessage);
        }


        private void OnBroadcastReceived(ChatMessage newMessage)
        {
            throw new NotImplementedException();
        }

        private void OnGroupMessageReceived(ChatMessage newMessage, string groupName)
        {
            _uiContext.Send(
                x => GetGroupChannel().ChatGroups.Single(c => c.Name == groupName).ChatMessages
                    .Add(newMessage), null);
        }

        private void OnUserMessageReceived(ChatMessage newMessage)
        {
            _uiContext.Send(
                x => GetUserChannel().Chats.Single(c => c.Name == newMessage.Sender).ChatMessages
                    .Add(newMessage), null);
        }

        private void OnGroupCreated(string groupName)
        {
            _uiContext.Send(
                x => AddChatGroup(new ChatGroup
                {
                    Name = groupName,
                    Participants = new ObservableCollection<string> {"Me"}
                }), null);
        }


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
                    _uiContext.Send(x => AddChat(new Chat {Name = "Henk"}), null);
#else
                     _uiContext.Send(x => AddChat(new Chat {Name = e.User}), null);
#endif
                    break;
                case ConnectionStatus.Disconnected:
#if DEBUG
                    _uiContext.Send(x => RemoveChat(new Chat {Name = "Henk"}), null);
#else
                    _uiContext.Send(x => RemoveChat(new Chat {Name = e.User}), null);
#endif
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
        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            DeinitHub();
        }

        #endregion
    }
}
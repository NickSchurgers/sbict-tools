using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

namespace SBICT.Modules.Chat
{
    public class ChatManager : BindableBase, IChatManager
    {
        #region Fields

        private readonly SynchronizationContext _uiContext = SynchronizationContext.Current;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionManager<IConnection> _connectionManager;
        private readonly IRegionManager _regionManager;

        private readonly ChatChannel _userChannel = new ChatChannel {Name = "Users"};
        private readonly ChatChannel _groupChannel = new ChatChannel {Name = "Groups"};

        #endregion

        #region Properties

        public IConnection Connection { get; set; }
        public Chat ActiveChat { get; set; }
        public ChatGroup ActiveGroup { get; set; }
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
            Connection.Hub.On<string, string, ConnectionScope>("MessageReceived", OnMessageReceived);

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
               var users = await Connection.Hub.InvokeAsync<IEnumerable<string>>("GetUserList");
                _userChannel.Chats = new ObservableCollection<Chat>(users.Select(u => new Chat {Name = u}));
                _userChannel.IsExpanded = true;
            
            Channels.Add(_userChannel);
            Channels.Add(_groupChannel);
            
        }

        public void AddChatChannel(ChatChannel channel)
        {
            throw new NotImplementedException();
        }

        public void AddChatGroup(ChatGroup group, ChatChannel channel = null)
        {
            throw new NotImplementedException();
        }

        public void AddChat(Chat chat)
        {
            Channels.Single(c => c.Name == "Users").Chats.Add(chat);
            SystemLogger.LogEvent($"{chat.Name} has joined");
        }

        public void ActivateChat(Chat chat)
        {
            var param = new NavigationParameters {{"Chat", chat}};

            if (ActiveChat != null)
            {
                ActiveChat.IsOpen = false;
            }

            chat.IsOpen = true;
            ActiveChat = chat;
            _regionManager.RequestNavigate(RegionNames.MainRegion, new Uri("ChatWindow", UriKind.Relative), param);
        }

        public void ActivateChatGroup(ChatGroup chatGroup)
        {
            throw new NotImplementedException();
        }

        public void RemoveChat(Chat chat)
        {
            _userChannel.Chats.RemoveAll(c => c.Name == chat.Name);
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
                    OnUserMessageReceived(newMessage);
                    break;
                case ConnectionScope.Group:
                    OnGroupMessageReceived(newMessage);
                    break;
                case ConnectionScope.Broadcast:
                    OnBroadcastReceived(newMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }

            _eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(newMessage);
        }


        private void OnBroadcastReceived(ChatMessage newMessage)
        {
            throw new NotImplementedException();
        }

        private void OnGroupMessageReceived(ChatMessage newMessage)
        {
            throw new NotImplementedException();
        }

        private void OnUserMessageReceived(ChatMessage newMessage)
        {
            _uiContext.Send(
                x => _userChannel.Chats.Single(c => c.Name == newMessage.Sender).ChatMessages.Add(newMessage), null);
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
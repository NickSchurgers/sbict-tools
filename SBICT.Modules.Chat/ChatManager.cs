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
using SBICT.Data;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Chat;
using SBICT.Infrastructure.Connection;
using SBICT.Infrastructure.Extensions;
using SBICT.Infrastructure.Logger;
using Group = SBICT.Data.Group;

namespace SBICT.Modules.Chat
{
    public class ChatManager : BindableBase, IChatManager
    {
        #region Fields

        private readonly SynchronizationContext _uiContext = SynchronizationContext.Current;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionManager<IConnection> _connectionManager;
        private readonly IRegionManager _regionManager;
        private readonly ISettingsManager _settingsManager;
        private readonly User _user;

        #endregion

        #region Properties

        public IConnection Connection { get; set; }
        public IChatWindow ActiveChat { get; set; }
        public ObservableCollection<ChatChannel> Channels { get; set; } = new ObservableCollection<ChatChannel>();

        #endregion

        public ChatManager(IEventAggregator eventAggregator, IConnectionManager<IConnection> connectionManager,
            IRegionManager regionManager, ISettingsManager settingsManager)
        {
            _eventAggregator = eventAggregator;
            _connectionManager = connectionManager;
            _regionManager = regionManager;
            _settingsManager = settingsManager;
            _user = _settingsManager.User;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Closing += OnMainWindowClosing;

            InitHub();
        }

        /// <summary>
        /// Initiate the connection with the chat hub
        /// </summary>
        private async void InitHub()
        {
            Connection =
                ConnectionFactory.Create(
                    $"http://localhost:13338/hubs/chat?displayName={_user.DisplayName}&guid={_user.Id.ToString()}");
            Connection.UserStatusChanged += OnUserStatusChanged;
            Connection.Hub.On<ChatMessage, ConnectionScope>("MessageReceived", OnMessageReceived);
            Connection.Hub.On<ChatGroup>("GroupCreated", OnGroupCreated);

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
        /// Create list of root nodes and populate the users node with a list of active users
        /// </summary>
        public async void InitChannels()
        {
            AddChatChannel(new ChatChannel {Name = "Users", IsExpanded = true});

            var users = await Connection.Hub.InvokeAsync<IEnumerable<User>>("GetUserList", _user.Id);
            users.ToList().ForEach(u => AddChat(new Chat {User = u, Title = u.DisplayName}));

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
            SystemLogger.LogEvent($"{group.Title} was created");
        }

        public void AddChat(Chat chat)
        {
            GetUserChannel().Chats.Add(chat);
            SystemLogger.LogEvent($"{chat.User.DisplayName} has joined");
        }

        private void Activate(IChatWindow chat, NavigationParameters parameters)
        {
            if (ActiveChat != null)
            {
                ActiveChat.IsActive = false;
            }

            chat.IsActive = true;
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
            GetUserChannel().Chats.RemoveAll(c => c.User.Id == chat.User.Id);
            SystemLogger.LogEvent($"{chat.User.DisplayName} has left");
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
        public async void SendMessage(Guid recipient, string message, ConnectionScope scope)
        {
            var chatMessage = new ChatMessage
            {
                Message = message,
                Received = DateTime.Now,
                Recipient = recipient,
                Sender = _user
            };
            await Connection.Hub.InvokeAsync("SendMessage", chatMessage, scope);
        }

        public async void JoinChatGroup(ChatGroup group)
        {
            await Connection.Hub.InvokeAsync("GroupJoin", group, _user.Id);
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

        private void OnMessageReceived(ChatMessage message, ConnectionScope scope)
        {
            switch (scope)
            {
                case ConnectionScope.System:
                    break;
                case ConnectionScope.User:
                    OnUserMessageReceived(message);
                    break;
                case ConnectionScope.Group:
                    OnGroupMessageReceived(message);
                    break;
                case ConnectionScope.Broadcast:
                    OnBroadcastReceived(message);
                    break;
            }

            _eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(message);
        }


        private void OnBroadcastReceived(ChatMessage newMessage)
        {
            throw new NotImplementedException();
        }

        private void OnGroupMessageReceived(ChatMessage newMessage)
        {
            _uiContext.Send(
                x => GetGroupChannel().ChatGroups.Single(c => c.Id == newMessage.Recipient).ChatMessages
                    .Add(newMessage), null);
        }

        private void OnUserMessageReceived(ChatMessage newMessage)
        {
            _uiContext.Send(
                x => GetUserChannel().Chats.Single(c => c.User.Id == newMessage.Sender.Id).ChatMessages
                    .Add(newMessage), null);
        }

        private void OnGroupCreated(ChatGroup group)
        {
            _uiContext.Send(x => AddChatGroup(group), null);
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
                    var user = new User(Guid.NewGuid(), "Henk");
                    _uiContext.Send(x => AddChat(new Chat {User = user, Title = "Henk"}), null);
#else
                     _uiContext.Send(x => AddChat(new Chat {User = e.User, Title = e.User.DisplayName}), null);
#endif
                    break;
                case ConnectionStatus.Disconnected:
#if DEBUG
                    _uiContext.Send(x => GetUserChannel().Chats.RemoveAll(c => c.User.DisplayName == "Henk"), null);
                    SystemLogger.LogEvent("Henk has left");
#else
                    _uiContext.Send(x => RemoveChat(new Chat {User = e.User}), null);
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
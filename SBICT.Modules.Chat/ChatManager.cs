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
using SBICT.Modules.Chat.Extensions;

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
        public ObservableCollection<IChatChannel> Channels { get; set; } = new ObservableCollection<IChatChannel>();
        public ObservableCollection<IUser> ConnectedUsers { get; set; } = new ObservableCollection<IUser>();

        #endregion

        #region Events

        public event EventHandler ChatMessageReceived;
        public event EventHandler BroadcastReceived;
        public event EventHandler GroupInviteReceived;

        protected virtual void OnChatMessageReceived()
        {
            ChatMessageReceived?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnBroadcastReceived()
        {
            BroadcastReceived?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnGroupInviteReceived(IChatGroup group)
        {
            GroupInviteReceived?.Invoke(this, new ChatGroupEventArgs(group));
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="connectionManager"></param>
        /// <param name="regionManager"></param>
        /// <param name="settingsManager"></param>
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
            var (address, port) = _settingsManager.Server;
            Connection =
                ConnectionFactory.Create(
                    $"{address}:{port}/hubs/chat?displayName={_user.DisplayName}&guid={_user.Id.ToString()}");
            Connection.UserStatusChanged += OnUserStatusChanged;

            Connection.Hub.On<Guid, User, Message, ConnectionScope>("MessageReceived", OnMessageReceived);

            //Set up handling of group created notification
            Connection.Hub.On<Group>("GroupCreated", grp => _uiContext.Send(x =>
            {
                AddChatGroup((ChatGroup) grp);
                SystemLogger.LogEvent($"{grp.Name} was created");
            }, null));

            //Set up handling of a group invite
            Connection.Hub.On<Group>("GroupInvited", grp => _uiContext.Send(x =>
            {
                var group = (ChatGroup) grp;
                OnGroupInviteReceived(group);
                _eventAggregator?.GetEvent<GroupInviteReceivedEvent>().Publish(group);
                SystemLogger.LogEvent($"Invite received for group {group.Name}");
            }, null));

            Connection.Hub.On<User>("GroupJoined", OnGroupJoined);
            Connection.Hub.On<User>("GroupLeft", OnGroupLeft);

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
            ConnectedUsers = new ObservableCollection<IUser>(users.Cast<IUser>());
            foreach (var user in ConnectedUsers)
            {
                AddChat(new Chat(user));
            }

            AddChatChannel(new ChatChannel {Name = "Groups", IsExpanded = true});
        }

        /// <summary>
        /// Add channel to list of channels
        /// </summary>
        /// <param name="channel"></param>
        public void AddChatChannel(IChatChannel channel)
        {
            Channels.Add(channel);
            SystemLogger.LogEvent($"Channel \"{channel.Name}\" was added", LogLevel.Debug);
        }

        /// <summary>
        /// Add chatgroup to the groups channel
        /// </summary>
        /// <param name="group"></param>
        public void AddChatGroup(IChatGroup group)
        {
            Channels.ByName("Groups").ChatGroups.Add(group);
        }

        /// <summary>
        /// Add chat to the users channel
        /// </summary>
        /// <param name="chat"></param>
        public void AddChat(IChat chat)
        {
            Channels.ByName("Users").Chats.Add(chat);
            SystemLogger.LogEvent($"{chat.Recipient.DisplayName} has joined");
        }

        /// <summary>
        /// Activate a chat(group) by navigating to the chat window
        /// </summary>
        /// <param name="window"></param>
        public void ActivateWindow(IChatWindow window)
        {
            if (ActiveChat != null)
            {
                ActiveChat.IsActive = false;
            }

            window.IsActive = true;
            ActiveChat = window;
            _regionManager.RequestNavigate(RegionNames.MainRegion, new Uri("ChatWindow", UriKind.Relative),
                new NavigationParameters {{"Chat", window}});
        }
        
        /// <summary>
        /// Remove chat from user channel
        /// </summary>
        /// <param name="chat"></param>
        public void RemoveChat(IChat chat)
        {
            Channels.ByName("Users").Chats.RemoveAll(c => c.Recipient.Id == chat.Recipient.Id);
            SystemLogger.LogEvent($"{chat.Recipient.DisplayName} has left");
        }

        /// <summary>
        /// Remove chat group from groups channel
        /// </summary>
        /// <param name="chatGroup"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void RemoveChatGroup(IChatGroup chatGroup)
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
            var chatMessage = new ChatMessage(message, DateTime.Now);
            await Connection.Hub.InvokeAsync("SendMessage", recipient, _user.Id, chatMessage, scope);
        }

        /// <summary>
        /// Join group on server
        /// </summary>
        /// <param name="group"></param>
        public async void JoinChatGroup(IChatGroup group)
        {
            await Connection.Hub.InvokeAsync("GroupJoin", group, _user.Id);
        }

        #region Handlers

        private void OnMessageReceived(Guid recipient, User sender, Message message, ConnectionScope scope)
        {
            var chatMessage = new ChatMessage(message.Content, message.Received)
            {
                Recipient = recipient,
                Sender = sender
            };

            switch (scope)
            {
                case ConnectionScope.User:
                    var chat = (Chat) Channels.ByName("Users").Chats.ById(chatMessage.Sender.Id);
                    _uiContext.Send(x => chat.Messages.Add(chatMessage), null);
                    break;
                case ConnectionScope.Group:
                    var group = (ChatGroup) Channels.ByName("Groups").ChatGroups.ById(chatMessage.Recipient);
                    _uiContext.Send(x => group.Messages.Add(chatMessage), null);
                    break;
                case ConnectionScope.Broadcast:
                case ConnectionScope.System:
                    break;
            }

            _eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(chatMessage);
        }

        private void OnGroupLeft(User obj)
        {
            throw new NotImplementedException();
        }

        private void OnGroupJoined(User obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Triggered when a user (dis)connects from the chat hub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void OnUserStatusChanged(object sender, ConnectionEventArgs e)
        {
            IUser user;
            switch (e.Status)
            {
                case ConnectionStatus.Connected:
                    user = e.User;
                    ConnectedUsers.Add(user);
                    _uiContext.Send(x => AddChat(new Chat(user)), null);
                    break;
                case ConnectionStatus.Disconnected:
                    user = ConnectedUsers.Single(u => u.Id == e.User.Id);
                    var chat = Channels.ByName("Users").Chats.ById(user.Id);
                    ConnectedUsers.Remove(user);
                    _uiContext.Send(x => RemoveChat(chat), null);
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
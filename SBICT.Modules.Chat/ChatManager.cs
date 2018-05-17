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
            var (address, port) = _settingsManager.Server;
            Connection =
                ConnectionFactory.Create(
                    $"{address}:{port}/hubs/chat?displayName={_user.DisplayName}&guid={_user.Id.ToString()}");
            Connection.UserStatusChanged += OnUserStatusChanged;
            Connection.Hub.On<Guid, User, Message, ConnectionScope>("MessageReceived", OnMessageReceived);
            Connection.Hub.On<Group>("GroupCreated", OnGroupCreated);

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
            users.ToList().ForEach(u => AddChat(new Chat(u)));

            AddChatChannel(new ChatChannel {Name = "Groups", IsExpanded = true});
        }

        public void AddChatChannel(IChatChannel channel)
        {
            Channels.Add(channel);
            SystemLogger.LogEvent($"Channel \"{channel.Name}\" was added", LogLevel.Debug);
        }

        public void AddChatGroup(IChatGroup group)
        {
            Channels.ByName("Groups").ChatGroups.Add(group);
            SystemLogger.LogEvent($"{group.Name} was created");
        }

        public void AddChat(IChat chat)
        {
            Channels.ByName("Users").Chats.Add(chat);
            SystemLogger.LogEvent($"{chat.Recipient.DisplayName} has joined");
        }

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

        public void RemoveChat(IChat chat)
        {
            Channels.ByName("Users").Chats.RemoveAll(c => c.Recipient.Id == chat.Recipient.Id);
            SystemLogger.LogEvent($"{chat.Recipient.DisplayName} has left");
        }

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

        public async void JoinChatGroup(IChatGroup group)
        {
            await Connection.Hub.InvokeAsync("GroupJoin", group, _user.Id);
        }

        #region Event Handlers

        private void OnMessageReceived(Guid recipient, User sender, Message message, ConnectionScope scope)
        {
            var chatMessage = new ChatMessage(message.Content, message.Received)
            {
                Recipient = recipient,
                Sender = sender
            };

            switch (scope)
            {
                case ConnectionScope.System:
                    break;
                case ConnectionScope.User:
                    OnUserMessageReceived(chatMessage);
                    break;
                case ConnectionScope.Group:
                    OnGroupMessageReceived(chatMessage);
                    break;
                case ConnectionScope.Broadcast:
                    OnBroadcastReceived(chatMessage);
                    break;
            }

            _eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(chatMessage);
        }


        private void OnBroadcastReceived(ChatMessage newMessage)
        {
            throw new NotImplementedException();
        }

        private void OnGroupMessageReceived(ChatMessage newMessage)
        {
            var group = (ChatGroup) Channels.ByName("Groups").ChatGroups.ById(newMessage.Recipient);
            _uiContext.Send(x => group.Messages.Add(newMessage), null);
        }

        private void OnUserMessageReceived(ChatMessage newMessage)
        {
            var chat = (Chat) Channels.ByName("Users").Chats.ById(newMessage.Sender.Id);
            _uiContext.Send(x => chat.Messages.Add(newMessage), null);
        }

        private void OnGroupCreated(Group group)
        {
            var chatGroup = ChatGroup.FromIGroup(group);
            _uiContext.Send(x => AddChatGroup(chatGroup), null);
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
                    _uiContext.Send(x => AddChat(new Chat(user)), null);
#else
                     _uiContext.Send(x => AddChat(new Chat(e.User)), null);
#endif
                    break;
                case ConnectionStatus.Disconnected:
#if DEBUG
                    _uiContext.Send(
                        x => Channels.ByName("Users").Chats.RemoveAll(c => c.Recipient.DisplayName == "Henk"),
                        null);
                    SystemLogger.LogEvent("Henk has left");
#else
                    _uiContext.Send(x => RemoveChat(new Chat(e.User)), null);
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
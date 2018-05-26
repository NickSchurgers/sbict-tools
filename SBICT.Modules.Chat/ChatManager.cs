// <copyright file="ChatManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Data;
using System.Threading.Tasks;

namespace SBICT.Modules.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;
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
    using SBICT.Modules.Chat.Extensions;

    /// <inheritdoc cref="IChatManager" />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ChatManager : BindableBase, IChatManager
    {
        private readonly SynchronizationContext uiContext = SynchronizationContext.Current;
        private readonly IEventAggregator eventAggregator;
        private readonly IConnectionManager<IConnection> connectionManager;
        private readonly Infrastructure.Connection.IConnectionFactory connectionFactory;
        private readonly IRegionManager regionManager;
        private readonly ISettingsManager settingsManager;
        private readonly IUser user;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatManager"/> class.
        /// </summary>
        /// <param name="eventAggregator">Instance of Prism EventAggregator.</param>
        /// <param name="connectionManager">Instance of ConnectionManager.</param>
        /// <param name="connectionFactory">Instance of ConnectionFactory.</param>
        /// <param name="regionManager">Instance of the Prism RegionManager.</param>
        /// <param name="settingsManager">Instance of SettingsManager.</param>
        public ChatManager(
            IEventAggregator eventAggregator,
            IConnectionManager<IConnection> connectionManager,
            Infrastructure.Connection.IConnectionFactory connectionFactory,
            IRegionManager regionManager,
            ISettingsManager settingsManager)
        {
            this.eventAggregator = eventAggregator;
            this.connectionManager = connectionManager;
            this.connectionFactory = connectionFactory;

            this.regionManager = regionManager;
            this.settingsManager = settingsManager;
            this.user = this.settingsManager.User;

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Closing += this.OnMainWindowClosing;
            }

            this.InitHub();
        }

        /// <inheritdoc/>
        public event EventHandler<ChatGroupEventArgs> GroupInviteReceived;

        /// <inheritdoc/>
        public event EventHandler<ChatMessageEventArgs> ChatMessageReceived;

        /// <inheritdoc/>
        public event EventHandler<BroadcastEventArgs> BroadcastReceived;

        /// <inheritdoc/>
        public IConnection Connection { get; set; }

        /// <inheritdoc/>
        public IChatWindow ActiveChat { get; set; }

        /// <inheritdoc/>
        public ObservableCollection<IChatChannel> Channels { get; set; } = new ObservableCollection<IChatChannel>();

        /// <inheritdoc/>
        public ObservableCollection<IUser> ConnectedUsers { get; set; } = new ObservableCollection<IUser>();

        /// <inheritdoc/>
        public void ActivateWindow(IChatWindow window)
        {
            if (this.ActiveChat != null)
            {
                this.ActiveChat.IsActive = false;
            }

            window.IsActive = true;
            this.ActiveChat = window;
            this.regionManager.RequestNavigate(
                RegionNames.MainRegion,
                new Uri("ChatWindow", UriKind.Relative),
                new NavigationParameters { { "Chat", window } });
        }

        /// <inheritdoc />
        public async void SendMessage(Guid recipient, string message, ConnectionScope scope)
        {
            var chatMessage = new ChatMessage(message, DateTime.Now);
            await this.Connection.Hub.InvokeAsync("SendMessage", recipient, this.user.Id, chatMessage, scope);
        }

        /// <inheritdoc />
        public async Task InitChannels()
        {
            this.AddChatChannel(new ChatChannel { Name = "Users", IsExpanded = true });

            var users = await this.Connection.Hub.InvokeAsync<IEnumerable<User>>("GetUserList", this.user.Id);
            this.ConnectedUsers = new ObservableCollection<IUser>(users.Cast<IUser>());
            foreach (var connectedUser in this.ConnectedUsers)
            {
                this.AddChat(new Chat(connectedUser));
            }

            this.AddChatChannel(new ChatChannel { Name = "Groups", IsExpanded = true });

            // var groups = await this.Connection.Hub.InvokeAsync<IEnumerable<Group>>("GetGroupsForUser", this.user.Id);
            // TODO: Add Groups when user connects with a new client
        }

        /// <inheritdoc />
        public void AddChatChannel(IChatChannel channel)
        {
            this.Channels.Add(channel);
            SystemLogger.LogEvent($"Channel \"{channel.Name}\" was added", LogLevel.Debug);
        }

        /// <inheritdoc/>
        public void AddChatGroup(IChatGroup chatGroup)
        {
            this.Channels.ByName("Groups").ChatGroups.Add(chatGroup);
            SystemLogger.LogEvent($"Group \"{chatGroup.Name}\" was added", LogLevel.Debug);
        }

        /// <inheritdoc/>
        public void AddChat(IChat chat)
        {
            this.Channels.ByName("Users").Chats.Add(chat);
            SystemLogger.LogEvent($"{chat.Recipient.DisplayName} has joined");
        }

        /// <inheritdoc/>
        public void RemoveChat(IChat chat)
        {
            this.Channels.ByName("Users").Chats.RemoveAll(c => c.Recipient.Id == chat.Recipient.Id);
            SystemLogger.LogEvent($"{chat.Recipient.DisplayName} has left");
        }

        /// <inheritdoc/>
        public void RemoveChatGroup(IChatGroup chatGroup)
        {
            this.Channels.ByName("Groups").ChatGroups.Add(chatGroup);
            SystemLogger.LogEvent($"Group \"{chatGroup.Name}\" was added", LogLevel.Debug);
        }

        /// <inheritdoc/>
        public async void JoinChatGroup(IChatGroup group)
        {
            await this.Connection.Hub.InvokeAsync("GroupJoin", group, this.user.Id);
        }

        /// <inheritdoc/>
        public async void InviteChatGroup(IChatGroup group, Guid userId)
        {
            await this.Connection.Hub.InvokeAsync("GroupInvite", group, userId);
        }

        /// <summary>
        /// Raised when a chat message has been received.
        /// </summary>
        /// <param name="e">ChatMessageEventArgs.</param>
        protected virtual void OnChatMessageReceived(ChatMessageEventArgs e)
        {
            this.ChatMessageReceived?.Invoke(this, e);
            this.eventAggregator.GetEvent<ChatMessageReceivedEvent>().Publish(e.ChatMessage);
        }

        /// <summary>
        /// Raised when a broadcast has been receveived.
        /// </summary>
        /// <param name="e">BroadcastEventArgs.</param>
        protected virtual void OnBroadcastReceived(BroadcastEventArgs e)
        {
            this.BroadcastReceived?.Invoke(this, e);
            this.eventAggregator.GetEvent<BroadcastReceivedEvent>().Publish(e.ChatMessage);
        }

        /// <summary>
        /// Raised when a group invite has been receveived.
        /// </summary>
        /// <param name="e">ChatGroupEventArgs.</param>
        protected virtual void OnGroupInviteReceived(ChatGroupEventArgs e)
        {
            this.GroupInviteReceived?.Invoke(this, e);
            this.eventAggregator?.GetEvent<GroupInviteReceivedEvent>().Publish(e.ChatGroup);
        }

        /// <summary>
        /// Initiate the connection with the chat hub.
        /// </summary>
        private async void InitHub()
        {
            var (address, port) = this.settingsManager.Server;
            var url = $"{address}:{port}/hubs/chat?displayName={this.user.DisplayName}&guid={this.user.Id.ToString()}";
            this.Connection = this.connectionFactory.Create(url, HubNames.ChatHub);
            this.Connection.UserStatusChanged += this.OnUserStateChanged;
            this.Connection.Hub.On<Guid, User, Message, ConnectionScope>("MessageReceived", this.OnMessageReceived);

            // Set up handling of group creation
            this.Connection.Hub.On<Group>("GroupCreated", this.OnGroupCreated);

            // Set up handling of a group invite.
            this.Connection.Hub.On<Group>("GroupInvited", this.OnGroupInvited);

            // Set up handling of group join.
            this.Connection.Hub.On<Group, User>("GroupJoined", this.OnGroupJoined);

            // Set up handling of group leave.
            this.Connection.Hub.On<Group, User>("GroupLeft", this.OnGroupLeft);

            this.connectionManager.Set("Chat", this.Connection);
            await this.Connection.StartAsync();
            await this.InitChannels();
        }

        /// <summary>
        /// Handle received messages.
        /// </summary>
        /// <param name="recipient">Target of message.</param>
        /// <param name="sender">Sender of message.</param>
        /// <param name="message">Content of message.</param>
        /// <param name="scope">Scope of message.</param>
        private void OnMessageReceived(Guid recipient, User sender, Message message, ConnectionScope scope)
        {
            var chatMessage = new ChatMessage(message.Content, message.Received)
            {
                Recipient = recipient,
                Sender = sender,
            };

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (scope == ConnectionScope.User)
            {
                var chat = (Chat) this.Channels.ByName("Users").Chats.ById(chatMessage.Sender.Id);
                this.uiContext.Send(x => chat.Messages.Add(chatMessage), null);
                this.OnChatMessageReceived(new ChatMessageEventArgs(chatMessage));
            }
            else if (scope == ConnectionScope.Group)
            {
                var group = (ChatGroup) this.Channels.ByName("Groups").ChatGroups.ById(chatMessage.Recipient);
                this.uiContext.Send(x => group.Messages.Add(chatMessage), null);
                this.OnChatMessageReceived(new ChatMessageEventArgs(chatMessage));
            }
            else if (scope == ConnectionScope.Broadcast)
            {
                this.OnBroadcastReceived(new BroadcastEventArgs(chatMessage));
            }
            else if (scope == ConnectionScope.System)
            {
                SystemLogger.LogEvent(chatMessage.Content, LogLevel.Broadcast);
            }
        }

        /// <summary>
        /// Triggered when a new group is created on teh server.
        /// </summary>
        /// <param name="group">Created group.</param>
        private void OnGroupCreated(Group group)
        {
            SystemLogger.LogEvent($"{group.Name} was created.");
            this.uiContext.Send(x => { this.AddChatGroup((ChatGroup) group); }, null);
        }

        /// <summary>
        /// Triggered when receiving a group invitation.
        /// </summary>
        /// <param name="group">Group invited to.</param>
        private void OnGroupInvited(Group group)
        {
            var chatGroup = (ChatGroup) group;
            SystemLogger.LogEvent($"Invite received for group {chatGroup.Name}.");
            this.uiContext.Send(x => this.OnGroupInviteReceived(new ChatGroupEventArgs(chatGroup)), null);
        }

        /// <summary>
        /// Triggered when a user leaves a group.
        /// </summary>
        /// <param name="group">Group being left.</param>
        /// <param name="leaver">User leaving the group.</param>
        private void OnGroupLeft(Group group, User leaver)
        {
            var chatGroup = (ChatGroup) group;
            if (leaver.Id == this.user.Id)
            {
                SystemLogger.LogEvent($"Group {chatGroup.Name} joined.");
                this.uiContext.Send(x => this.RemoveChatGroup(chatGroup), null);
            }
            else
            {
                SystemLogger.LogEvent($"{leaver.DisplayName} has left {chatGroup.Name}.");
            }
        }

        /// <summary>
        /// Triggerd when a user joins a group.
        /// </summary>
        /// <param name="group">Group being joined.</param>
        /// <param name="joiner">User joining the group.</param>
        private void OnGroupJoined(Group group, User joiner)
        {
            var chatGroup = (ChatGroup) group;
            if (joiner.Id == this.user.Id)
            {
                SystemLogger.LogEvent($"Group {chatGroup.Name} joined.");
                this.uiContext.Send(x => this.AddChatGroup(chatGroup), null);
            }
            else
            {
                SystemLogger.LogEvent($"{joiner.DisplayName} has joined {chatGroup.Name}.");
            }
        }

        private void OnUserStateChanged(object sender, ConnectionEventArgs connectionEventArgs)
        {
            if (connectionEventArgs.HubName != HubNames.ChatHub)
            {
                return;
            }

            switch (connectionEventArgs.Status)
            {
                case ConnectionStatus.Connected:
                    this.ConnectedUsers.Add(connectionEventArgs.User);
                    this.uiContext.Send(x => this.AddChat(new Chat(connectionEventArgs.User)), null);
                    break;
                case ConnectionStatus.Disconnected:
                    var subject = this.ConnectedUsers.Single(u => u.Id == connectionEventArgs.User.Id);
                    var chat = this.Channels.ByName("Users").Chats.ById(subject.Id);
                    this.ConnectedUsers.Remove(subject);
                    this.uiContext.Send(x => this.RemoveChat(chat), null);
                    break;
                case ConnectionStatus.Connecting:
                case ConnectionStatus.Reconnecting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.eventAggregator.GetEvent<UserStateChangedEvent>().Publish(new ConnectionEventArgs
            {
                HubName = HubNames.ChatHub,
                Status = connectionEventArgs.Status,
                User = connectionEventArgs.User,
            });
        }


        /// <summary>
        /// Triggered on closing of the main window.
        /// </summary>
        /// <param name="sender">Event Sender.</param>
        /// <param name="e">Event arguments.</param>
        private async void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            this.connectionManager.Unset("Chat");
            await this.Connection.StopAsync();
        }
    }
}
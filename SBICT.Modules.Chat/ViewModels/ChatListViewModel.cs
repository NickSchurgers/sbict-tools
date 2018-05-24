namespace SBICT.Modules.Chat.ViewModels
{
    using System.Collections.ObjectModel;
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using SBICT.Infrastructure.Chat;

    /// <inheritdoc />
    /// <summary>
    /// Viewmodel for ChatList.xaml.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ChatListViewModel : BindableBase
    {
        private readonly IChatManager chatManager;
        private ObservableCollection<IChatChannel> channels = new ObservableCollection<IChatChannel>();


        /// <summary>
        /// Initializes a new instance of the <see cref="ChatListViewModel"/> class.
        /// </summary>
        /// <param name="chatManager">Instance of chat manager.</param>
        public ChatListViewModel(IChatManager chatManager)
        {
            this.chatManager = chatManager;
            this.chatManager.InitChannels();
            this.chatManager.GroupInviteReceived += this.OnGroupInviteReceived;

            this.ChatListSelectedItemChanged = new DelegateCommand<object>(this.OnSelectedItemChanged);
            this.ChatListAddGroup = new DelegateCommand(this.OnChatListAddGroup);

            this.Channels = this.chatManager.Channels;
            this.GroupCreateRequest = new InteractionRequest<GroupInviteCreateNotification>();
            this.ConfirmInviteRequest = new InteractionRequest<IConfirmation>();
            this.BroadcastRequest = new InteractionRequest<INotification>();
        }

        /// <summary>
        /// Gets or Sets Command triggered when selection changes.
        /// </summary>
        public DelegateCommand<object> ChatListSelectedItemChanged { get; set; }

        /// <summary>
        /// Gets or Sets Command triggered when the add group button is pressed.
        /// </summary>
        public DelegateCommand ChatListAddGroup { get; set; }

        /// <summary>
        /// Gets or Sets collection of channels.
        /// </summary>
        public ObservableCollection<IChatChannel> Channels
        {
            get => this.channels;
            set => this.SetProperty(ref this.channels, value);
        }

        /// <summary>
        /// Gets the group create group popup request.
        /// </summary>
        public InteractionRequest<GroupInviteCreateNotification> GroupCreateRequest { get; private set; }

        /// <summary>
        /// Gets the group invitation popup request.
        /// </summary>
        public InteractionRequest<IConfirmation> ConfirmInviteRequest { get; private set; }

        /// <summary>
        /// Gets the broadcast popup request.
        /// </summary>
        public InteractionRequest<INotification> BroadcastRequest { get; private set; }

        /// <summary>
        /// Raised when a Chat or ChatGroup gets selected.
        /// </summary>
        /// <param name="obj">Chat or ChatGroup.</param>
        private void OnSelectedItemChanged(object obj)
        {
            switch (obj)
            {
                case Chat chat:
                    this.chatManager.ActivateWindow(chat);
                    break;
                case ChatGroup group:
                    this.chatManager.ActivateWindow(group);
                    break;
            }
        }

        /// <summary>
        /// Raised when the Add Group button gets clicked.
        /// </summary>
        private void OnChatListAddGroup()
        {
            var notification = new GroupInviteCreateNotification(this.chatManager.ConnectedUsers) {Title = "Items"};
            this.GroupCreateRequest.Raise(notification, result =>
            {
                if (result == null || !result.Confirmed || result.GroupName == null)
                {
                    return;
                }

                var group = new ChatGroup(result.GroupName);
                this.chatManager.JoinChatGroup(group);
                foreach (var user in result.SelectedItems)
                {
                    this.chatManager.InviteChatGroup(group, user.Id);
                }
            });
        }

        /// <summary>
        /// Raised when a group invite is received.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">ChatGroupEventArgs.</param>
        private void OnGroupInviteReceived(object sender, ChatGroupEventArgs e)
        {
            var confirm = new Confirmation
            {
                Title = "Group Invitation",
                Content = $"You have been invited to {e.ChatGroup.Name}",
            };

            this.ConfirmInviteRequest.Raise(confirm, result =>
            {
                if (result != null && result.Confirmed)
                {
                    this.chatManager.JoinChatGroup(e.ChatGroup);
                }
            });
        }
    }
}
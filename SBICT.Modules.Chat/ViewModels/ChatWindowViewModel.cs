namespace SBICT.Modules.Chat.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using SBICT.Data;
    using SBICT.Infrastructure;
    using SBICT.Infrastructure.Chat;

    /// <inheritdoc cref="BindableBase" />
    /// <inheritdoc cref="INavigationAware" />
    /// <summary>
    /// ViewModel for ChatWindow.xaml.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ChatWindowViewModel : BindableBase, INavigationAware
    {
        private readonly IChatManager chatManager;
        private readonly ISettingsManager settingsManager;
        private string message;
        private IChatWindow chatWindow;
        private ObservableCollection<IUser> participants;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatWindowViewModel"/> class.
        /// </summary>
        /// <param name="chatManager">Instance of IChatManager.</param>
        /// <param name="settingsManager">Instance of ISettingsManager.</param>
        public ChatWindowViewModel(IChatManager chatManager, ISettingsManager settingsManager)
        {
            this.chatManager = chatManager;
            this.settingsManager = settingsManager;
            this.SendMessage = new DelegateCommand(this.OnMessageSent);
        }

        /// <summary>
        /// Gets Header for the tab the view is hosted in.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Header { get; } = "Chat";

        /// <summary>
        /// Gets or sets the command used to trigger the sending of a new message.
        /// </summary>
        public DelegateCommand SendMessage { get; set; }

        /// <summary>
        /// Gets or sets the message to send.
        /// </summary>
        public string Message
        {
            get => this.message;
            set => this.SetProperty(ref this.message, value);
        }

        /// <summary>
        /// Gets or sets the particpants in case of an IChatGroup.
        /// </summary>
        public ObservableCollection<IUser> Participants
        {
            get => this.participants;
            set => this.SetProperty(ref this.participants, value);
        }

        /// <summary>
        /// Gets or sets the IChatWindow to interact with.
        /// </summary>
        public IChatWindow ChatWindow
        {
            get => this.chatWindow;
            set => this.SetProperty(ref this.chatWindow, value);
        }

        /// <inheritdoc/>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc/>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            navigationContext.Parameters.TryGetValue("Chat", out IChatWindow chat);
            this.ChatWindow = chat;
            if (chat is IChatGroup group)
            {
                this.Participants = group.Participants;
            }
        }

        /// <inheritdoc/>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //Reset participants in case we switch from a group to a single user chat
            this.Participants = null;
        }

        /// <summary>
        /// Raised when the Send button is clicked.
        /// </summary>
        private void OnMessageSent()
        {
            if (this.ChatWindow == null)
            {
                return;
            }

            //As a chatgroup is cast to a chat, we use participants to determine what the scope is
            this.chatManager.SendMessage(this.ChatWindow.GetRecipient(), this.Message, this.ChatWindow.Scope);
            this.ChatWindow.Messages.Add(
                new ChatMessage(this.Message, DateTime.Now) {Sender = this.settingsManager.User});
            this.Message = string.Empty;
        }
    }
}
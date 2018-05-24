namespace SBICT.Modules.Chat
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using Prism.Mvvm;
    using SBICT.Infrastructure.Chat;

    /// <inheritdoc cref="IChatChannel" />
    public class ChatChannel : BindableBase, IChatChannel
    {
        private ObservableCollection<IChat> chats;
        private ObservableCollection<IChatGroup> chatGroups;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatChannel"/> class.
        /// </summary>
        public ChatChannel()
        {
            this.Chats = new ObservableCollection<IChat>();
            this.ChatGroups = new ObservableCollection<IChatGroup>();
        }

        /// <inheritdoc/>
        public ObservableCollection<IChat> Chats
        {
            get => this.chats;
            set => this.SetProperty(ref this.chats, value);
        }

        /// <inheritdoc/>
        public ObservableCollection<IChatGroup> ChatGroups
        {
            get => this.chatGroups;
            set => this.SetProperty(ref this.chatGroups, value);
        }

        /// <inheritdoc/>
        public bool IsExpanded { get; set; }

        /// <inheritdoc/>
        public IList Items => new CompositeCollection
        {
            new CollectionContainer {Collection = Chats},
            new CollectionContainer {Collection = ChatGroups},
        };

        /// <inheritdoc/>
        public string Name { get; set; }
    }
}
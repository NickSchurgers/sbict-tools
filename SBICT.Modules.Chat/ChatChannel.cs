using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class ChatChannel : BindableBase
    {
        #region Fields

        private ObservableCollection<Chat> _chats;
        private ObservableCollection<ChatGroup> _chatGroups;

        #endregion

        #region Properties

        /// <summary>
        /// Collection of chats 
        /// </summary>
        public ObservableCollection<Chat> Chats
        {
            get => _chats;
            set => SetProperty(ref _chats, value);
        }

        /// <summary>
        /// Collection of the chatgroups
        /// </summary>
        public ObservableCollection<ChatGroup> ChatGroups
        {
            get => _chatGroups;
            set => SetProperty(ref _chatGroups, value);
        }

        public bool IsExpanded { get; set; }

        /// <summary>
        /// Property used for enumerating items in the treeview
        /// </summary>
        public IList Items => new CompositeCollection
        {
            new CollectionContainer {Collection = Chats},
            new CollectionContainer {Collection = ChatGroups}
        };

        /// <summary>
        /// Name of this group
        /// </summary>
        public string Name { get; set; }

        #endregion

        public ChatChannel()
        {
            Chats = new ObservableCollection<Chat>();
            ChatGroups = new ObservableCollection<ChatGroup>();
        }
    }
}
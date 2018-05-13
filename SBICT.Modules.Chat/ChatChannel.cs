using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using Prism.Mvvm;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat
{
    public class ChatChannel : BindableBase, IChatChannel
    {
        #region Fields

        private ObservableCollection<IChat> _chats;
        private ObservableCollection<IChatGroup> _chatGroups;

        #endregion

        #region Properties

        /// <summary>
        /// Collection of chats 
        /// </summary>
        public ObservableCollection<IChat> Chats
        {
            get => _chats;
            set => SetProperty(ref _chats, value);
        }

        /// <summary>
        /// Collection of the chatgroups
        /// </summary>
        public ObservableCollection<IChatGroup> ChatGroups
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
            Chats = new ObservableCollection<IChat>();
            ChatGroups = new ObservableCollection<IChatGroup>();
        }
    }
}
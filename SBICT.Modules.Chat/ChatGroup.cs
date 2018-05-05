using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class ChatGroup : BindableBase
    {
        #region Fields

        private List<Chat> _chats = new List<Chat>();
        private ObservableCollection<ChatGroup> _chatGroups = new ObservableCollection<ChatGroup>();

        #endregion

        #region Properties

        /// <summary>
        /// Collection of chats 
        /// </summary>
        public List<Chat> Chats
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
        public IEnumerable Items => ChatGroups?.Cast<object>().Concat(Chats);

        /// <summary>
        /// Name of this group
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}
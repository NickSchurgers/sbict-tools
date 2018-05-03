using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class ChatGroup : BindableBase
    {
        #region Fields

        private ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
        private ObservableCollection<ChatGroup> _chatGroups = new ObservableCollection<ChatGroup>();

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
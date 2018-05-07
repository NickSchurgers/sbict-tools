using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class ChatChannel : BindableBase
    {
        #region Fields

        private ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
        private ObservableCollection<ChatChannel> _chatChannels = new ObservableCollection<ChatChannel>();

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
        public ObservableCollection<ChatChannel> ChatChannels
        {
            get => _chatChannels;
            set => SetProperty(ref _chatChannels, value);
        }

        public bool IsExpanded { get; set; }

        /// <summary>
        /// Property used for enumerating items in the treeview
        /// </summary>
        public IEnumerable Items => ChatChannels?.Cast<object>().Concat(Chats);

        /// <summary>
        /// Name of this group
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}
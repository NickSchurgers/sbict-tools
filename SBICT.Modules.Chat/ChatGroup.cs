using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class ChatGroup: BindableBase
    {
        private ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
        private ObservableCollection<ChatGroup> _chatGroups = new ObservableCollection<ChatGroup>();

        public ObservableCollection<Chat> Chats
        {
            get => _chats;
            set => SetProperty(ref _chats, value);
        }

        public ObservableCollection<ChatGroup> ChatGroups
        {
            get => _chatGroups;
            set => SetProperty(ref _chatGroups, value);
        }

        public IEnumerable Items => ChatGroups?.Cast<object>().Concat(Chats);

        public string Name { get; set; }
    }
}
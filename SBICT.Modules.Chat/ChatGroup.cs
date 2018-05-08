using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class ChatGroup : Chat
    {
        public ObservableCollection<string> Participants { get; set; }
    }
}
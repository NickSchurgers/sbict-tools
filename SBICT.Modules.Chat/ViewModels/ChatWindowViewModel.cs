using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowViewModel : BindableBase
    {
        public string Header { get; } = "Chat";
        public ChatWindowViewModel()
        {

        }
    }
}

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Prism.Regions;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatWindowViewModel : BindableBase, INavigationAware
    {
        private Chat _chat;
        public string Header { get; } = "Chat";

        public Chat Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public ChatWindowViewModel()
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Chat = navigationContext.Parameters["Chat"] as Chat;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
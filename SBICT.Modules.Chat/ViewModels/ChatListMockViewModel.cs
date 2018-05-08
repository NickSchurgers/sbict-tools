using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat.ViewModels
{
    public class ChatListMockViewModel : BindableBase
    {

        #region Commands

        public DelegateCommand ChatListSelectedItemChanged { get; set; }
        public DelegateCommand SendMessage { get; set; }

        #endregion

        #region Fields

        private ObservableCollection<ChatChannel> _channels = new ObservableCollection<ChatChannel>();

        #endregion

        #region Properties
        public ChatChannel UserChannel { get; set; } = new ChatChannel { Name = "Users" };
        public ChatChannel GroupChannel { get; set; } = new ChatChannel { Name = "Groups"};
        
        /// <summary>
        /// Collection of chatgroups used as root node in the treeview
        /// </summary>
        public ObservableCollection<ChatChannel> Channels
        {
            get => _channels;
            set => SetProperty(ref _channels, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public ChatListMockViewModel()
        {
            RefreshChatList();
        }

        /// <summary>
        /// Create list of root nodes and populate the users node with a list off active users
        /// </summary>
        private void RefreshChatList()
        {
            UserChannel.Chats.Add(new Chat {Name = "Henk"});
            UserChannel.Chats.Add(new Chat {Name = "Piet"});
            UserChannel.IsExpanded = true;
            GroupChannel.ChatGroups.Add(new ChatGroup {Name = "Alpha"});
            GroupChannel.ChatGroups.Add(new ChatGroup {Name = "Beta"});
            GroupChannel.ChatGroups.Add(new ChatGroup {Name = "Gamma"});

            Channels = new ObservableCollection<ChatChannel>
            {
                UserChannel,
                GroupChannel
            };
        }

        #endregion
    }
}
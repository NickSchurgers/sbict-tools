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

        private ObservableCollection<ChatChannel> _chatGroups = new ObservableCollection<ChatChannel>();
        private readonly ChatChannel _userChannel = new ChatChannel {Name = "Users"};
        private readonly ChatChannel _groupChannel = new ChatChannel {Name = "Groups"};
//        private readonly ChatGroup _projectChannel = new ChatGroup {Name = "Projects"};

        #endregion

        #region Properties

        /// <summary>
        /// Collection of chatgroups used as root node in the treeview
        /// </summary>
        public ObservableCollection<ChatChannel> ChatGroups
        {
            get => _chatGroups;
            set => SetProperty(ref _chatGroups, value);
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
            _userChannel.Chats.Add(new Chat {Name = "Henk"});
            _userChannel.Chats.Add(new Chat {Name = "Piet"});
            _userChannel.IsExpanded = true;
            _groupChannel.Chats.Add(new Chat {Name = "Alpha"});
            _groupChannel.Chats.Add(new Chat {Name = "Beta"});
            _groupChannel.Chats.Add(new Chat {Name = "Gamma"});
//            _projectChannel.Chats.Add(new Chat {Name = "Alphabet"});
//            _projectChannel.Chats.Add(new Chat {Name = "Windows2.0"});
//            _projectChannel.Chats.Add(new Chat {Name = "TestProject"});

            ChatGroups = new ObservableCollection<ChatChannel>
            {
                _userChannel,
                _groupChannel,
//                _projectChannel
            };
        }

        #endregion
    }
}
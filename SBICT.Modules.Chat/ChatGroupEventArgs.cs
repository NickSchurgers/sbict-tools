using System;
using SBICT.Data;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat
{
    public class ChatGroupEventArgs : EventArgs
    {
        public IChatGroup ChatGroup { get; }

        public ChatGroupEventArgs(IChatGroup chatGroup)
        {
            ChatGroup = chatGroup;
        }
    }
}
using System;

namespace SBICT.Infrastructure.Chat
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
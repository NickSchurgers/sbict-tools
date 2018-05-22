using System;

namespace SBICT.Infrastructure.Chat
{
    public class BroadcastEventArgs : ChatMessageEventArgs
    {
        public BroadcastEventArgs(IChatMessage chatMessage) : base(chatMessage)
        {
        }
    }
}
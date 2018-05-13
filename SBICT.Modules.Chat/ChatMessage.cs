using System;
using SBICT.Data;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat
{
    public struct ChatMessage : IChatMessage
    {
        public ChatMessage(string messageContent, DateTime messageReceived)
        {
            Received = messageReceived;
            Content = messageContent;
            Sender = null;
            Recipient = Guid.Empty;
        }

        public string Content { get; }
        public IUser Sender { get; set; }
        public Guid Recipient { get; set; }
        public DateTime Received { get; }
    }
}
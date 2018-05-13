using System;
using SBICT.Data;

namespace SBICT.Infrastructure.Chat
{
    public struct ChatMessage
    {
        public string Message { get; set; }
        public User Sender { get; set; }
        public Guid Recipient { get; set; }
        public DateTime Received { get; set; }
    }
}
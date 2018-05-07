using System;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public struct ChatMessage
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Received { get; set; }
        public ConnectionScope Scope { get; set; }
    }
}
using System;
using SBICT.Data;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure
{
    public struct ChatMessage
    {
        public string Message { get; set; }
        public User Sender { get; set; }
        public Guid Recipient { get; set; }
        public DateTime Received { get; set; }
    }
}
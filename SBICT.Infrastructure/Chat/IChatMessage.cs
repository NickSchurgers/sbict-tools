using System;
using SBICT.Data;

namespace SBICT.Infrastructure.Chat
{
    public interface IChatMessage : IMessage
    {
        IUser Sender { get; set; }
        Guid Recipient { get; set; }
    }
}
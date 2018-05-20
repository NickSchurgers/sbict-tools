﻿using System;
using SBICT.Data;
using SBICT.Infrastructure.Chat;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public class Chat : ChatBase, IChat
    {
        public IUser Recipient { get; }

        public Chat(IUser recipient) : base(ConnectionScope.User)
        {
            Recipient = recipient;
            Title = recipient.DisplayName;
        }

        public override Guid GetRecipient()
        {
            return Recipient.Id;
        }
    }
}
// <copyright file="IChatMessage.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System;
    using SBICT.Data;

    public interface IChatMessage : IMessage
    {
        IUser Sender { get; set; }

        Guid Recipient { get; set; }
    }
}
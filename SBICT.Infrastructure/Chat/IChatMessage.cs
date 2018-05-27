// <copyright file="IChatMessage.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System;
    using SBICT.Data;

    /// <summary>
    /// Single chat message.
    /// </summary>
    public interface IChatMessage : IMessage
    {
        /// <summary>
        /// Gets sender of the message.
        /// </summary>
        IUser Sender { get; }

        /// <summary>
        /// Gets receiver of the message.
        /// </summary>
        Guid Recipient { get; }
    }
}
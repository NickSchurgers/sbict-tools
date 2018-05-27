// <copyright file="IChat.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using SBICT.Data;

    /// <summary>
    /// Instance of a single chat.
    /// </summary>
    public interface IChat
    {
        /// <summary>
        /// Gets the user associated as the recipient of this chat.
        /// </summary>
        IUser Recipient { get; }
    }
}
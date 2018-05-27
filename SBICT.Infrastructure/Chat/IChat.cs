// <copyright file="IChat.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using SBICT.Data;

    public interface IChat
    {
        IUser Recipient { get; }
    }
}
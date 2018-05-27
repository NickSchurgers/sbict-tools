// <copyright file="GroupInviteReceivedEvent.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using Prism.Events;

    /// <summary>
    /// Event raised when a group invite is received.
    /// </summary>
    public class GroupInviteReceivedEvent: PubSubEvent<IChatGroup>
    {
    }
}
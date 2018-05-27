// <copyright file="GroupInviteReceivedEvent.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using Prism.Events;

    public class GroupInviteReceivedEvent: PubSubEvent<IChatGroup>
    {
    }
}
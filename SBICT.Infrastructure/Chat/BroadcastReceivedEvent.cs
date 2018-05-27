// <copyright file="BroadcastReceivedEvent.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using Prism.Events;

    /// <summary>
    /// Event raised when a broadcast is received.
    /// </summary>
    public class BroadcastReceivedEvent : PubSubEvent<IChatMessage>
    {
    }
}
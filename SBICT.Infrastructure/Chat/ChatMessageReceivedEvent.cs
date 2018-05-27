// <copyright file="ChatMessageReceivedEvent.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using Prism.Events;

    /// <summary>
    /// Event raised when a chatmassege is received.
    /// </summary>
    public class ChatMessageReceivedEvent : PubSubEvent<IChatMessage>
    {
    }
}
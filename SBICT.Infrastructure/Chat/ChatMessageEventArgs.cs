// <copyright file="ChatMessageEventArgs.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    /// <summary>
    /// Event arguments for ChatMessage related events.
    /// </summary>
    public class ChatMessageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageEventArgs"/> class.
        /// </summary>
        /// <param name="chatMessage">Instance of the ChatMessage.</param>
        public ChatMessageEventArgs(IChatMessage chatMessage)
        {
            this.ChatMessage = chatMessage;
        }

        /// <summary>
        /// Gets the ChatMessage associated with this event.
        /// </summary>
        public IChatMessage ChatMessage { get; }
    }
}
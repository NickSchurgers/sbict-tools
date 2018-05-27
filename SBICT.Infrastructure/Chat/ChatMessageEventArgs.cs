// <copyright file="ChatMessageEventArgs.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    public class ChatMessageEventArgs
    {
        public IChatMessage ChatMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageEventArgs"/> class.
        /// </summary>
        /// <param name="chatMessage"></param>
        public ChatMessageEventArgs(IChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}
// <copyright file="ChatGroupEventArgs.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System;

    /// <summary>
    /// Event arguments for ChatGroup related events.
    /// </summary>
    public class ChatGroupEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGroupEventArgs"/> class.
        /// </summary>
        /// <param name="chatGroup">Group associated with the event.</param>
        public ChatGroupEventArgs(IChatGroup chatGroup)
        {
            this.ChatGroup = chatGroup;
        }

        /// <summary>
        /// Gets ChatGroup associated with the event.
        /// </summary>
        public IChatGroup ChatGroup { get; }
    }
}
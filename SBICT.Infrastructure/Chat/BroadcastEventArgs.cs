// <copyright file="BroadcastEventArgs.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    /// <summary>
    /// Event args for broadcast events.
    /// </summary>
    public class BroadcastEventArgs : ChatMessageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BroadcastEventArgs"/> class.
        /// </summary>
        /// <param name="chatMessage">Message broadcasted.</param>
        public BroadcastEventArgs(IChatMessage chatMessage)
            : base(chatMessage)
        {
        }
    }
}
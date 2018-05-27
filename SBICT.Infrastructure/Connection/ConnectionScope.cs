// <copyright file="ConnectionScope.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    /// <summary>
    /// Scope of a connection event.
    /// </summary>
    public enum ConnectionScope
    {
        /// <summary>
        /// Event is meant for system only.
        /// </summary>
        System,

        /// <summary>
        /// Event is meant for a user.
        /// </summary>
        User,

        /// <summary>
        /// Event is meant for a group.
        /// </summary>
        Group,

        /// <summary>
        /// Event is meant as a broadcast.
        /// </summary>
        Broadcast,
    }
}
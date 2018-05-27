// <copyright file="LogLevel.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Logger
{
    /// <summary>
    /// Enum used to indicite the level of a logmessage.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debug message.
        /// </summary>
        Debug,

        /// <summary>
        /// Info message.
        /// </summary>
        Info,

        /// <summary>
        /// Warning message.
        /// </summary>
        Warning,

        /// <summary>
        /// Error message.
        /// </summary>
        Error,

        /// <summary>
        /// Broadcast message.
        /// </summary>
        Broadcast,
    }
}
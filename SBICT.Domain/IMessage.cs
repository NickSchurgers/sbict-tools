// <copyright file="IMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Data
{
    using System;

    /// <summary>
    /// Basic Message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        string Content { get; }

        /// <summary>
        /// Gets the timestamp of when the message was received/sent.
        /// </summary>
        DateTime Received { get; }
    }
}
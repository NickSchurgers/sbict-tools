// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Data
{
    using System;

    /// <inheritdoc cref="IMessage" />
    public struct Message : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> struct.
        /// </summary>
        /// <param name="content">Content of the message.</param>
        public Message(string content)
        {
            this.Content = content;
            this.Received = DateTime.Now;
        }

        /// <inheritdoc />
        public string Content { get; }

        /// <inheritdoc />
        public DateTime Received { get; }
    }
}
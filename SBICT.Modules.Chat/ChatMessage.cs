namespace SBICT.Modules.Chat
{
    using System;
    using SBICT.Data;
    using SBICT.Infrastructure.Chat;

    /// <inheritdoc cref="IChatMessage" />
    public struct ChatMessage : IChatMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> struct.
        /// </summary>
        /// <param name="messageContent">Content of the message.</param>
        /// <param name="messageReceived">Date when the message is sent/received.</param>
        public ChatMessage(string messageContent, DateTime messageReceived)
        {
            this.Received = messageReceived;
            this.Content = messageContent;
            this.Sender = null;
            this.Recipient = Guid.Empty;
        }

        /// <inheritdoc/>
        public string Content { get; }

        /// <inheritdoc/>
        public IUser Sender { get; set; }

        /// <inheritdoc/>
        public Guid Recipient { get; set; }

        /// <inheritdoc/>
        public DateTime Received { get; }
    }
}
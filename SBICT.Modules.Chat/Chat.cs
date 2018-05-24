namespace SBICT.Modules.Chat
{
    using System;
    using SBICT.Data;
    using SBICT.Infrastructure.Chat;
    using SBICT.Infrastructure.Connection;

    /// <inheritdoc cref="IChat" />
    public class Chat : ChatBase, IChat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Chat"/> class.
        /// </summary>
        /// <param name="recipient">User chatting with.</param>
        public Chat(IUser recipient)
            : base(ConnectionScope.User)
        {
            this.Recipient = recipient;
            this.Title = recipient.DisplayName;
        }

        /// <inheritdoc/>
        public IUser Recipient { get; }

        /// <inheritdoc/>
        public override Guid GetRecipient()
        {
            return this.Recipient.Id;
        }
    }
}
// <copyright file="ChatGroup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Modules.Chat
{
    using System;
    using System.Collections.ObjectModel;
    using SBICT.Data;
    using SBICT.Infrastructure.Chat;
    using SBICT.Infrastructure.Connection;

    /// <inheritdoc cref="IChatGroup" />
    public class ChatGroup : ChatBase, IChatGroup
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SBICT.Modules.Chat.ChatGroup" /> class.
        /// </summary>
        /// <param name="groupName">Name of this group.</param>
        public ChatGroup(string groupName)
            : base(ConnectionScope.Group)
        {
            this.Participants = new ObservableCollection<IUser>();
            this.Id = Guid.NewGuid();
            this.Name = groupName;
            this.Title = groupName;
        }

        /// <inheritdoc/>
        public Guid Id { get; private set; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public ObservableCollection<IUser> Participants { get; }

        /// <summary>
        /// Cast from Group to an instance of this class.
        /// </summary>
        /// <param name="group">Group to cast.</param>
        /// <returns>ChatGroup.</returns>
        public static explicit operator ChatGroup(Group group)
        {
            return new ChatGroup(group.Name) {Id = group.Id};
        }

        /// <summary>
        /// Cast from an instance of this class to Group.
        /// </summary>
        /// <param name="chatGroup">Chatgroup to cast.</param>
        /// <returns>Group.</returns>
        public static explicit operator Group(ChatGroup chatGroup)
        {
            return new Group(chatGroup.Id, chatGroup.Name);
        }

        /// <inheritdoc/>
        public override Guid GetRecipient()
        {
            return this.Id;
        }
    }
}
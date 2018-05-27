// <copyright file="IChatChannel.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System.Collections;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A collection of chats and/or chatgroups.
    /// </summary>
    public interface IChatChannel
    {
        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the channel is expanded, in for example a treeview, or not.
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Gets the chats associated with this channel.
        /// </summary>
        ObservableCollection<IChat> Chats { get; }

        /// <summary>
        /// Gets the chatgroups associated with this channel.
        /// </summary>
        ObservableCollection<IChatGroup> ChatGroups { get; }

        /// <summary>
        /// Gets a combined list of chats and chatgroups associated with this channel.
        /// </summary>
        IList Items { get; }
    }
}
// <copyright file="IChatChannel.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System.Collections;
    using System.Collections.ObjectModel;

    public interface IChatChannel
    {
        string Name { get; }

        bool IsExpanded { get; set; }

        ObservableCollection<IChat> Chats { get; }

        ObservableCollection<IChatGroup> ChatGroups { get; }

        IList Items { get; }
    }
}
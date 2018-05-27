// <copyright file="IChatGroup.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System.Collections.ObjectModel;
    using SBICT.Data;

    /// <summary>
    /// A chatgroup.
    /// </summary>
    public interface IChatGroup : IGroup
    {
        /// <summary>
        /// Gets all users participating in this group.
        /// </summary>
        ObservableCollection<IUser> Participants { get; }
    }
}
// <copyright file="GroupInviteCreateNotification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Modules.Chat
{
    using System.Collections.Generic;
    using Prism.Interactivity.InteractionRequest;
    using SBICT.Data;

    /// <inheritdoc />
    public class GroupInviteCreateNotification : Confirmation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupInviteCreateNotification"/> class.
        /// </summary>
        /// <param name="items">List of Users to invite.</param>
        /// <param name="groupName">Name of the group to create or invite to.</param>
        public GroupInviteCreateNotification(IEnumerable<IUser> items, string groupName = null)
        {
            this.Items = new List<IUser>();
            this.SelectedItems = new List<IUser>();
            this.GroupName = groupName ?? "New Group";
            this.IsNew = groupName == null;

            foreach (var item in items)
            {
                this.Items.Add(item);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the group is new or existing. 
        /// </summary>
        public bool IsNew { get; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the items selected.
        /// </summary>
        public IList<IUser> SelectedItems { get; set; }

        /// <summary>
        /// Gets the Items to pick from.
        /// </summary>
        // ReSharper disable once CollectionNeverQueried.Global
        public IList<IUser> Items { get; }
    }
}
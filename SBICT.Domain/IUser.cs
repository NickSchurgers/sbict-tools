// <copyright file="IUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Data
{
    using System;

    /// <summary>
    /// Basic User.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Gets id of the user.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Gets or sets the displayname of the user..
        /// </summary>
        string DisplayName { get; set; }
    }
}
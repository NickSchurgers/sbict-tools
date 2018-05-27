// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Data
{
    using System;

    /// <inheritdoc cref="IUser" />
    public struct User : IUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> struct.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <param name="userName">Username of the user.</param>
        public User(Guid id, string userName)
        {
            this.Id = id;
            this.UserName = userName;
            this.DisplayName = userName;
        }

        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public string UserName { get; }

        /// <inheritdoc />
        public string DisplayName { get; set; }
    }
}
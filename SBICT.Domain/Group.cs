// <copyright file="Group.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Data
{
    using System;

    /// <inheritdoc cref="IGroup" />
    public struct Group : IGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> struct.
        /// </summary>
        /// <param name="id">Group Id</param>
        /// <param name="title">Group Title</param>
        public Group(Guid id, string title)
        {
            this.Id = id;
            this.Name = title;
        }

        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }
    }
}
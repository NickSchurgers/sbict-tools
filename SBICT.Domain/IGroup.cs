// <copyright file="IGroup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Data
{
    using System;

    /// <summary>
    /// Basic Group
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Gets the id of the group.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        string Name { get; }
    }
}
// <copyright file="IConnectionFactory.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    /// <summary>
    /// Connection factory.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Creates a new Connection class with the required settings.
        /// </summary>
        /// <param name="url">Connection string the hub to connect with.</param>
        /// <param name="hubName">Name of the hub connection to create.</param>
        /// <returns>Returns new instance of Connection.</returns>
        IConnection Create(string url, string hubName);
    }
}
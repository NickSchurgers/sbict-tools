// <copyright file="ISettingsManager.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure
{
    using SBICT.Data;

    /// <summary>
    /// Used to manage application wide settings.
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// Gets a value indicating whether the application is run for the first time.
        /// </summary>
        bool IsFirstRun { get; }

        /// <summary>
        /// Gets the user using the application.
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Gets the server url and port as tuple.
        /// </summary>
        (string, int) Server { get; }
    }
}